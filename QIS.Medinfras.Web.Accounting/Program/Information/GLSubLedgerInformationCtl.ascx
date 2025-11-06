<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GLSubLedgerInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.GLSubLedgerInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_subledgerinfoctl">
    $(function () {
        $('#btnProcess').click(function () {
            cbpViewPopup.PerformCallback('refresh');
        });
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div style="padding: 10px;">
    <input type="hidden" id="hdnGLAccountID" runat="server" />
    <input type="hidden" id="hdnSubledger" runat="server" />
    <input type="hidden" id="hdnYear" runat="server" />
    <input type="hidden" id="hdnMonth" runat="server" />
    <input type="hidden" id="hdnStatus" runat="server" />
    <table width="100%">
        <colgroup>
            <col width="120px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdlabel">
                <label>
                    <%=GetLabel("Sub Ledger No") %></label>
            </td>
            <td>
                <asp:TextBox runat="server" ReadOnly="true" ID="txtGLAccountNo" Width="150px" />
            </td>
        </tr>
        <tr>
            <td class="tdlabel">
                <label>
                    <%=GetLabel("Sub Ledger Name")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ReadOnly="true" ID="txtGLAccountName" Width="600px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="height: 400px; overflow-y: auto; overflow-x: hidden">
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="JournalNo" ItemStyle-HorizontalAlign="Left" HeaderText="Journal No"
                                                HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="JournalDateInString" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Tanggal" />
                                            <asp:BoundField DataField="Remarks" ItemStyle-HorizontalAlign="Left" HeaderText="Catatan" />
                                            <asp:BoundField DataField="BalanceBEGIN" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" HeaderText="BEGIN" DataFormatString="{0:N}" />
                                            <asp:BoundField DataField="DEBITAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" HeaderText="DEBIT" DataFormatString="{0:N}" />
                                            <asp:BoundField DataField="CREDITAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" HeaderText="CREDIT" DataFormatString="{0:N}" />
                                            <asp:BoundField DataField="BalanceEND" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" HeaderText="SALDO" DataFormatString="{0:N}" />
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
        <tr>
            <td colspan="2" align="right">
                <table>
                    <colgroup>
                        <col style="width: 120px;" />
                        <col style="width: 150px;" />
                    </colgroup>
                    <tr>
                        <td>
                            <%=GetLabel("Total Debit") %>
                        </td>
                        <td>
                            <asp:TextBox Width="100%" runat="server" ReadOnly="true" CssClass="number" ID="txtTotalBalanceDEBIT" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Total Kredit") %>
                        </td>
                        <td>
                            <asp:TextBox Width="100%" runat="server" ReadOnly="true" CssClass="number" ID="txtTotalBalanceCREDIT" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
