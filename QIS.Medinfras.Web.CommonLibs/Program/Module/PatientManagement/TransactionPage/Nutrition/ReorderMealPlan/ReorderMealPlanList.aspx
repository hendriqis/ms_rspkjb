<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ReorderMealPlanList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ReorderMealPlanList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnReorderProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnReorderProcess.ClientID %>').click(function () {
                showLoadingPanel();
                clickReorderProcess();
            });

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtScheduleDate1.ClientID %>');

            setDatePicker('<%=txtScheduleDateCopy.ClientID %>');
            $('#<%=txtScheduleDateCopy.ClientID %>').datepicker('option', 'minDate', '-1');
            $('#<%=txtScheduleDateCopy.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtScheduleDateCopy.ClientID %>').datepicker('option', 'maxDate', '1');

            cbpViewDt.PerformCallback('refresh');
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'success') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
                showToast("SUCCESS", "Proses Reorder berhasil!");
                $('#<%=hdnSelectedMember.ClientID %>').val('');
            }
            else if (param[0] == 'failed') {
                showToast("FAILED!", "Harap pilih pasien untuk copy order menu makan.");
            }
            else if (param[0] == 'copyfailed') {
                showToast("FAILED!", param[1]);
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Check Box
        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }
        //#endregion

        //#region Button Print
        function clickReorderProcess() {
            getCheckedMember();
            var visitID = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
            cbpView.PerformCallback('reorder|' + visitID);
            hideLoadingPanel();
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        });

        $('#btnGetOrder').live('click', function () {
            $('#<%=hdnSelectedMember.ClientID %>').val('')
            cbpViewDt.PerformCallback('refresh');
        });

        $('#<%=txtScheduleDate1.ClientID %>').change(function () {
            cbpView.PerformCallback('refresh');
        });

    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnGCMealDay" runat="server" />
    <input type="hidden" value="" id="hdnReorderByMealTimePeriod" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative">
        <tr>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col style="width: 70%" />
                </colgroup>
                <tr>
                    <td>
                        <div style="border: 1px solid #000000; width: 400px;">
                            <table class="tblContentArea" style="width: 100%">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 70%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <%=GetLabel("Unit") %>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" runat="server" ClientInstanceName="cboServiceUnit">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="button" id="btnRefresh" value="Cari" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        <div style="border: 1px solid #000000; width: 400px;">
                            <table class="tblContentArea" style="width: 100%">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 70%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <%=GetLabel("Tanggal Makan") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtScheduleDate1" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <%=GetLabel("Unit") %>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnitFilter" Width="200px" runat="server" ClientInstanceName="cboServiceUnitFilter">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <%=GetLabel("Jadwal Makan") %>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboMealTime" Width="200px" runat="server" ClientInstanceName="cboMealTime">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="button" id="btnGetOrder" value="Cari" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 400px;">
                            <table class="tblContentArea" style="width: 800px">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 50%" />
                                </colgroup>
                                <td>
                                    <label class="lblNormal" style="font-style:oblique">
                                        <%=GetLabel("Copy Menu Makan") %></label>
                                    <table class="tblContentArea" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Tanggal Makan") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScheduleDateCopy" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Jadwal Makan") %>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboMealTimeCopy" Width="200px" runat="server" ClientInstanceName="cboMealTimeCopy">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <label class="lblNormal" style="font-style:oblique">
                                        <%=GetLabel("Generate Menu Makan") %></label>
                                    <table class="tblContentArea" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Tanggal Makan") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScheduleDateGenerate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Jadwal Makan") %>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboMealTimeGenerate" Width="200px" runat="server" ClientInstanceName="cboMealTimeGenerate">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </tr>
        <tr>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col style="width: 75%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BedCode" HeaderStyle-HorizontalAlign="Left" HeaderText="No. Bed"
                                                    HeaderStyle-Width="50px" />
                                                <asp:BoundField DataField="MedicalNo" HeaderStyle-HorizontalAlign="Left" HeaderText="No. RM"
                                                    HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="PatientName" HeaderStyle-HorizontalAlign="Left" HeaderText="Nama Pasien"
                                                    HeaderStyle-Width="150px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada transaksi permintaan barang")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div style="position: relative;">
                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList">
                                            <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="NutritionOrderHdID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Jadwal Makan")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("MealTime")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Unit")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("No. Bed")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("BedCode")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("No. RM")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("MedicalNo")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Nama Pasien")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("PatientName")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Diit")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("LastOrder")%>,<%#: Eval("DietType")%>,<%#: Eval("NumberOfCalories")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Catatan")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("Remarks")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada order menu makan pasien")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingDt">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </tr>
    </div>
</asp:Content>
