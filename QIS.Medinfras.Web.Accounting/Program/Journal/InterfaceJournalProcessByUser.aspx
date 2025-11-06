<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
CodeBehind="InterfaceJournalProcessByUser.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.InterfaceJournalProcessByUser" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtFromJournalDate.ClientID %>');
            $('#<%=txtFromJournalDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            
            var minDate = parseInt('<%=minDate %>');
            if (minDate > -1) {
                $('#<%=txtFromJournalDate.ClientID %>').datepicker('option', 'minDate', '-' + minDate);    
            }

            $('#<%=btnProcess.ClientID %>').click(function () {
                if ($('#<%=hdnID.ClientID %>').val() == '')
                    showToast('Warning', 'Silakan Pilih Jurnal Terlebih Dahulu');
                else
                    onCustomButtonClick('process');
            });
        });

        function onCboGCJournalGroupValueChanged(s) {
            var value = s.GetValue();
            cbpView.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Proses Berhasil', retval);
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

        $('.lnkViewSetting a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Journal/InterfaceJournalDtViewCtl.ascx");
            openUserControlPopup(url, id, 'Detil', 1000, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnLastPostingDate" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label>
                    <%=GetLabel("Grup Jurnal") %></label>
            </td>
            <td style="padding-top: 3px">
                <dxe:ASPxComboBox ID="cboGCJournalGroup" ClientInstanceName="cboGCJournalGroup" Width="300px"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboGCJournalGroupValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Periode Transaksi")%></label></td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:145px"/>
                        <col style="width:10px"/>
                        <col style="width:145px"/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtFromJournalDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <%--<td><%=GetLabel("-") %></td>
                        <td><asp:TextBox ID="txtToJournalDate" Width="120px" CssClass="datepicker" runat="server" /></td>--%>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionCode" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="TransactionName" HeaderText="Transaksi" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="LastProcessInfo" HeaderText="Informasi Proses Terakhir" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:HyperLinkField HeaderText="Detail" Text="Setting" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkViewSetting" HeaderStyle-Width="120px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
