<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="TreasuryInformationDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryInformationDetail" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        //#region GL Account Treasury
        function onGetGLAccountTreasuryFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsUsedAsTreasury = 1";
            return filterExpression;
        }

        $('#lblGLAccountTreasury.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountTreasuryFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode.ClientID %>').val(value);
                onTxtGLAccountCodeChanged(value);
            });
        });

        $('#<%=txtGLAccountCode.ClientID %>').change(function () {
            onTxtGLAccountCodeChanged($(this).val());
        });

        function onTxtGLAccountCodeChanged(value) {
            var filterExpression = onGetGLAccountTreasuryFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);
                }
                else {
                    $('#<%=hdnGLAccountID.ClientID %>').val('');
                    $('#<%=txtGLAccountCode.ClientID %>').val('');
                    $('#<%=txtGLAccountName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <table width="100%">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 100px" />
                <col style="width: 150px" />
                <col style="width: 350px" />
                <col style="width: 40%" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Periode")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboYear" ClientInstanceName="cboYear" Width="100%" runat="server" />
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboMonth" ClientInstanceName="cboMonth" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Filter Status")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboStatus" Width="100%" ClientInstanceName="cboStatus" runat="server" />
                    <input type="hidden" id="hdnDataSource" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <input type="hidden" id="hdnGLAccountTreasuryID" runat="server" />
                    <input type="hidden" id="hdnGLAccountID" runat="server" />
                    <input type="hidden" id="hdnFilterExpression" runat="server" />
                    <label class="lblLink lblMandatory" id="lblGLAccountTreasury">
                        <%=GetLabel("Akun Treasury")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtGLAccountCode" Width="100%" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtGLAccountName" Width="100%" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal" id="Label1">
                        <%=GetLabel("Total Saldo")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox Width="100%" runat="server" ReadOnly="true" ID="txtTotalBalanceEnd"
                        CssClass="number" />
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <div style="height: 450px; overflow-y: auto;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <input type="hidden" id="hdnSaldo" runat="server" />
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="JournalDateInString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Tgl. Voucher" HeaderStyle-Width="100px" />
                                                <asp:BoundField DataField="ReferenceNo" ItemStyle-HorizontalAlign="Left" HeaderText="Referensi" />
                                                <asp:BoundField DataField="JournalNo" ItemStyle-HorizontalAlign="Left" HeaderText="No. Voucher" />
                                                <asp:BoundField DataField="RemarksHd" ItemStyle-HorizontalAlign="Left" HeaderText="Catatan"
                                                    HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="GLAccountNo" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="No. Kontrol" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CfUser" ItemStyle-HorizontalAlign="Left" HeaderText="User" />
                                                <asp:BoundField DataField="DebitAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="DEBIT" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="CreditAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="CREDIT" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="BalanceEND" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="SALDO" HeaderStyle-HorizontalAlign="Right" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
