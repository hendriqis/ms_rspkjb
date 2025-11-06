<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="AntenatalRecordList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.AntenatalRecordList" %>
    
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientallergylist">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnPregnancyNo.ClientID %>').val($(this).find('.pregnancyNo').html());
                $('#<%=hdnLMPDate.ClientID %>').val($(this).find('.lmpDate').html());

                if ($('#<%=hdnID.ClientID %>').val() != "") {
                    cbpViewDt1.PerformCallback('refresh');
                    //                    cbpViewDt2.PerformCallback('refresh');
                    //                    cbpViewDt3.PerformCallback('refresh');
                    //                    cbpViewDt4.PerformCallback('refresh');
                    //                    cbpViewDt5.PerformCallback('refresh');
                    //                    cbpViewDt6.PerformCallback('refresh');
                    //                    cbpViewDt7.PerformCallback('refresh');
                    //                    cbpViewDt8.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $('#<%=grdViewDt1.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt1.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnFetusID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnFetusNo.ClientID %>').val($(this).find('.fetusNo').html());
            if ($('#<%=hdnFetusID.ClientID %>').val() != "") {
                cbpViewDt1_1.PerformCallback('refresh');
            }
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
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
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt1EndCallback(s) {
            $('#containerImgLoadingViewDt1').hide();

            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt1_1EndCallback(s) {
            $('#containerImgLoadingViewDt1_1').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt1_1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1_1"), pageCount1, function (page) {
                    cbpViewDt1_1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt1_1.ClientID %> tr:eq(1)').click();
        }

        $('.imgAddFetus.imgLink').die('click');
        $('.imgAddFetus.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalEntryCtl1.ascx");
                var param = "0" + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Data Janin", 700, 350, "");
            }
        });

        $('.imgEditFetus.imgLink').die('click');
        $('.imgEditFetus.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalEntryCtl1.ascx");
                var param = recordID + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Data Janin", 700, 350, "");
            }
        });

        $('.imgDeleteFetus.imgLink').die('click');
        $('.imgDeleteFetus.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi pengukuran fetal/janin untuk pasien ini ?";
            displayConfirmationMessageBox("Fetal Measurement", message, function (result) {
                if (result) {
                    var param = recordID;
                    cbpDeleteFetus.PerformCallback(param);
                }
            });
        });

        $('.imgAddMeasurement.imgLink').die('click');
        $('.imgAddMeasurement.imgLink').live('click', function (evt) {
            addMeasurement();
        });

        $('#lblAddMeasurement').die('click');
        $('#lblAddMeasurement').live('click', function (evt) {
            addMeasurement();
        });

        function addMeasurement() {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalMeasurementEntryCtl.ascx");
                var param = "0" + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Fetal Measurement", 700, 500, "X487^002");
            }
        }

        $('.imgEditMeasurement.imgLink').die('click');
        $('.imgEditMeasurement.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalMeasurementEntryCtl.ascx");
                var param = recordID + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Fetal Measurement", 700, 500, "X487^002");
            }
        });

        $('.imgDeleteMeasurement.imgLink').die('click');
        $('.imgDeleteMeasurement.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDelete(paramedicID)) {
                var message = "Hapus informasi pengukuran fetal/janin untuk pasien ini ?";
                displayConfirmationMessageBox("Fetal Measurement", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpDeleteMeasurement.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgCopyMeasurement.imgLink').die('click');
        $('.imgCopyMeasurement').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var message = "Lakukan copy pengukuran fetal/janin ?";
            displayConfirmationMessageBox('COPY :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalMeasurementEntryCtl.ascx");
                    var param = "0" + "|" + $('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val() + "|" + recordID;
                    openUserControlPopup(url, param, "Fetal Measurement", 700, 500, "X487^002");
                }
            });
        });


        function onCbpDeleteFetusEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Informasi Janin', param[1]);
            }
        }

        function onCbpDeleteMeasurementEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1_1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Fetal Measurement', param[1]);
            }
        }
        //#endregion

        function onBeforeEditDelete(paramedicID) {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, pengkajian hanya bisa diubah/dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        $('.imgViewHistoryAntenatalRecord.imgLink').die('click');
        $('.imgViewHistoryAntenatalRecord.imgLink').live('click', function () {
            var param = $('#<%=hdnID.ClientID %>').val();
            openUserControlPopup("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/AntenatalRecordFormHistoryCtl.ascx", param, "Antenatal Record History", 680, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPregnancyNo" runat="server" />
    <input type="hidden" value="" id="hdnLMPDate" runat="server" />
    <input type="hidden" value="" id="hdnFetusID" runat="server" />
    <input type="hidden" value="" id="hdnFetusNo" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnSubMenuType" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PregnancyNo" HeaderText="Kehamilan Ke-" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="pregnancyNo" />
                                            <asp:BoundField DataField="cfLMP" HeaderText="HPHT" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="cfEDB" HeaderText="Estimasi Tanggal Persalinan " HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="cfGPA" HeaderText="G-P-A" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px"/>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgViewHistoryAntenatalRecord imgLink" title='<%=GetLabel("History")%>'
                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/list.png")%>' alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
<%--                                            <asp:TemplateField HeaderText="Catatan Riwayat Menstruasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <asp:BoundField DataField="PregnancyNo" HeaderText="Kehamilan Ke-" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="pregnancyNo" />
                                            <asp:BoundField DataField="cfLMP" HeaderText="HPHT" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="cfEDB" HeaderText="Estimasi Tanggal Persalinan " HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="cfGPAL" HeaderText="G-P-A-L" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                            <%--                                            <asp:TemplateField HeaderText="Catatan Riwayat Menstruasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <textarea style="padding-left: 10px; border: 0; width: 99%; height: 100px; background-color: transparent"
                                                            readonly><%#: DataBinder.Eval(Container.DataItem, "MenstrualHistory") %> </textarea>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <%--                                            <asp:TemplateField HeaderText="Catatan Riwayat Kehamilan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <textarea style="padding-left: 10px; border: 0; width: 99%; height: 100px; background-color: transparent"
                                                            readonly><%#: DataBinder.Eval(Container.DataItem, "MedicalHistory") %> </textarea>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:BoundField DataField="cfIsActiveRecord" HeaderText="Status Record" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfLMP2" HeaderText="Menstruasi Terakhir" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hiddenColumn"
                                                ItemStyle-CssClass="hiddenColumn lmpDate" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada catatan antenatal record untuk pasien ini")%>
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
                </div>
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="fetalMeasurement">
                                        <%=GetLabel("Informasi Janin")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="fetalMeasurement">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDt1_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddFetus imgLink" title='<%=GetLabel("+ Janin")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="1" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditFetus imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" antenatalrecordid="<%#:Eval("AntenatalRecordID") %>"
                                                                                            fetusno="<%#:Eval("FetusNo") %>" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteFetus imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" antenatalrecordid="<%#:Eval("AntenatalRecordID") %>"
                                                                                            fetusno="<%#:Eval("FetusNo") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Janin" DataField="FetusNo" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="fetusNo" />
                                                                    <asp:BoundField HeaderText="Jenis Kelamin" DataField="Sex" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="sex" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div class="blink">
                                                                            <%=GetLabel("Belum ada informasi janin") %></div>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt1">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                <div class="wrapperPaging">
                                                </div>
                                                <div class="containerPaging">
                                                    <div id="pagingDt1">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <div id="preVitalSign">
                                                <dxcp:ASPxCallbackPanel ID="cbpViewDt1_1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1_1"
                                                    ShowLoadingPanel="false" OnCallback="cbpViewDt1_1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1_1').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDt1_1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdViewDt1_1" runat="server" CssClass="grdSelected grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-Width="80px">
                                                                            <HeaderTemplate>
                                                                                <img class="imgAddMeasurement imgLink" title='<%=GetLabel("+ Pengukuran")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                    alt="" recordid="<%#:Eval("ID") %>" fetusid="<%#:Eval("FetusID") %>" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                    <img class="imgEditMeasurement imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" recordid="<%#:Eval("ID") %>" fetusid="<%#:Eval("FetusID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img class="imgDeleteMeasurement imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" recordid="<%#:Eval("ID") %>" fetusid="<%#:Eval("FetusID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img class="imgCopyMeasurement imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                        alt="" recordid="<%#:Eval("ID") %>" fetusid="<%#:Eval("FetusID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="cfMeasurementDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="GestationalWeek" HeaderText="Usia Kehamilan" HeaderStyle-Width="60px"
                                                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="AC" HeaderText="AC (mm)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="BPD" HeaderText="BPD (mm)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="HL" HeaderText="HL (mm)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="HC" HeaderText="HC (mm)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="FL" HeaderText="FL (mm)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <%--<asp:BoundField DataField="EFW" HeaderText="EFW" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>--%>
                                                                        <asp:BoundField DataField="OFD" HeaderText="OFD (mm)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="CRL" HeaderText="CRL" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <%--<asp:BoundField DataField="FHR" HeaderText="FHR"  HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                        <asp:BoundField DataField="GS" HeaderText="GS" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />--%>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <div>
                                                                            <div>
                                                                                <%=GetLabel("Belum ada informasi pengukuran janin ini.") %></div>
                                                                            <br />
                                                                            <span class="lblLink" id="lblAddMeasurement">
                                                                                <%= GetLabel("+ Pengukuran")%></span>
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt1_1">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingDt1_1">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteFetus" runat="server" Width="100%" ClientInstanceName="cbpDeleteFetus"
            ShowLoadingPanel="false" OnCallback="cbpDeleteFetus_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteFetusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteMeasurement" runat="server" Width="100%" ClientInstanceName="cbpDeleteMeasurement"
            ShowLoadingPanel="false" OnCallback="cbpDeleteMeasurement_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteMeasurementEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
