<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutstandingARInvoicePatient.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.OutstandingARInvoicePatient" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnMRNCtl" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 60%" />
            <col style="width: 40%" />
        </colgroup>
        <tr>
             <td align="left">
                <table>
                    <colgroup>
                        <col style="width: 60px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                            <%=GetLabel("Patient")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatient" ReadOnly="true" Width="400px" runat="server" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <table>
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Klaim")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalClaimedAmount" ReadOnly="true" Width="140px" align runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Penerimaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPaymentAmount" ReadOnly="true" Width="140px" align runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Sisa Piutang")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalSaldoAmount" ReadOnly="true" Width="140px" align runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ARInvoiceNo" HeaderText="No. Invoice" HeaderStyle-Width="60px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ARInvoiceDateInString" HeaderText="Tanggal Invoice" HeaderStyle-Width="40px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalClaimedAmountInString" HeaderText="Nilai Klaim" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TotalPaymentAmountInString" HeaderText="Nilai Penerimaan" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TotalSaldoAmountInString" HeaderText="Sisa Piutang" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
