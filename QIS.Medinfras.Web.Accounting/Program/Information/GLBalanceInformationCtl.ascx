<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GLBalanceInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.GLBalanceInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_glbalanceinfoctl">
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
    <input type="hidden" id="hdnYear" runat="server" />
    <input type="hidden" id="hdnMonth" runat="server" />
    <input type="hidden" id="hdnStatus" runat="server" />
    <table width="100%">
        <colgroup>
            <col width="120px" />
            <col width="120px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdlabel">
                <label>
                    <%=GetLabel("Akun") %></label>
            </td>
            <td>
                <asp:TextBox runat="server" ReadOnly="true" ID="txtGLAccountNo" Width="120px" />
            </td>
            <td>
                <asp:TextBox runat="server" ReadOnly="true" ID="txtGLAccountName" Width="500px" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em; max-height: 450px; overflow-y: scroll">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="JournalNo" ItemStyle-HorizontalAlign="Left" HeaderText="Nomor Voucher"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="JournalDateInString" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="Remarks" ItemStyle-HorizontalAlign="Left" HeaderText="Catatan"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DEBITAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}"
                                            HeaderStyle-Width="100px" HeaderText="DEBIT" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="CREDITAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}"
                                            HeaderStyle-Width="100px" HeaderText="CREDIT" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="BalanceEND" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}"
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
            </td>
        </tr>
    </table>
</div>
