<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDepositBalanceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientDepositBalanceCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatient" ReadOnly="true" Width="700px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Saldo Akhir")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtBalanceEND" ReadOnly="true" Width="200px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
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
                                        <asp:BoundField DataField="PaymentNo" HeaderText="No. Pembayaran"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PaymentDateInString" HeaderText="Tgl. Pembayaran" HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Keterangan"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfBalanceBEGINInString" HeaderText="Saldo Awal" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="cfBalanceINInString" HeaderText="Deposit IN" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="cfBalanceOUTInString" HeaderText="Deposit OUT" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="cfBalanceENDInString" HeaderText="Saldo Akhir" HeaderStyle-Width="120px"
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
