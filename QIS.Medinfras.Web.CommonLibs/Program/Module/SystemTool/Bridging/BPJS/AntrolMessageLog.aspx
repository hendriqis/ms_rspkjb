<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="AntrolMessageLog.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AntrolMessageLog" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            setDatePicker('<%=txtToDate.ClientID %>');

            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    onRefreshGrid();
                }
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGrid();
        }, interval);

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            ////            var detail = $(this).find('.keyField').html();
            ////            var mrn = $(this).find('.hiddenColumn').html();
            ////            $('#<%=hdnID.ClientID %>').val(detail);
            ////            $('#<%=hdnMRN.ClientID %>').val(mrn);
            ////            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            ////            $(this).addClass('selected');
            ////            $('#divErrorDetail').html('');
            ////            $('#divErrorDetail').append(convert(detail));
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        var convert = function (convert) {
            return $('<span />', { html: convert }).text();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
            intervalID = window.setInterval(function () {
                onRefreshGridView();
            }, interval);
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
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
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $btnResend = null;
        $('.btnResend').live('click', function () {
//            showLoadingPanel();
            $tr = $(this).closest('tr');
            var taskID = $tr.find('.TaskID').val();
            var regID = $tr.find('.keyField').html();

            cbpSendToJKN.PerformCallback(regID + "|" + taskID);
        });

        function onCbpSendToJKNEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'failed') {
                ShowSnackbarError(param[1]);
            }
            else {
                onRefreshGrid();
            }
        }

    </script>
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="position: relative;">
        <table tyle="width:60%;">
            <colgroup>
                <col style="width: 120px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Registrasi")%></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtFromDate" Width="120px" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtToDate" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Quick Filter")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="378px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="No. Registrasi" FieldName="RegistrationNo" />
                            <qis:QISIntellisenseHint Text="Nama" FieldName="PatientName" />
                            <qis:QISIntellisenseHint Text="No. RM" FieldName="MedicalNo" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
        </table>
        <div style="padding: 10px 0 10px 3px; font-size: 0.95em">
            <%=GetLabel("Halaman ini akan auto refresh")%>
            <%=GetLabel("setiap ")%>
            <%=GetRefreshGridInterval() %>
            <%=GetLabel("menit ")%>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="MedicalNo" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                <asp:BoundField DataField="cfRegistrationDateInString" HeaderText="Tanggal Registrasi" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderText="Kode Booking">
                                    <ItemTemplate>
                                        <label class="lblRegistrationNo">
                                            <%#:Eval("cfKodeBooking")%></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"/>
                                <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" HeaderText="Detail Status">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <div>
                                                        <asp:Repeater ID="rptTaskID" runat="server">
                                                            <ItemTemplate>
                                                                <div style="padding-left: 10px;">
                                                                    - TaskID <%#: DataBinder.Eval(Container.DataItem, "TaskID") %>
                                                                    (<%#: DataBinder.Eval(Container.DataItem, "TaskLogStatus") %>)
                                                                </div>
                                                                <div style="padding-left: 10px;font-style:italic;font-weight:bold">
                                                                    Log Date : <%#: DataBinder.Eval(Container.DataItem, "cfLogDateTimeInString") %>
                                                                </div>
                                                                <div style="padding-left: 10px;font-style:italic">
                                                                    <%#: DataBinder.Eval(Container.DataItem, "ResponseRemarks") %>
                                                                </div>
                                                                <br />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <br style="clear: both" />
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" HeaderText="Re-Send">
                                    <ItemTemplate>
                                        <input type="button" id="btnResend" class="btnResend" value="Re-Send" runat="server" />
                                        <input type="hidden" class="TaskID" value='<%#: Eval("TaskID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada record")%>
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
    <dxcp:ASPxCallbackPanel ID="cbpSendToJKN" runat="server" Width="100%" ClientInstanceName="cbpSendToJKN"
        ShowLoadingPanel="false" OnCallback="cbpSendToJKN_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendToJKNEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
