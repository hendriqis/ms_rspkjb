<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDepositInfoDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.PatientDepositInfoDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_patientdepositdtctl">
    $(function () {
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();
    }
    //#endregion

</script>
<input type="hidden" id="hdnParamPeriode" runat="server" />
<input type="hidden" id="hdnParamPeriodeFrom" runat="server" />
<input type="hidden" id="hdnParamPeriodeTo" runat="server" />
<input type="hidden" id="hdnParamMRN" runat="server" />
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width: 70%">
                <colgroup>
                    <col style="width: 150px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("No. RM")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMedicalNoDt" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Nama Pasien")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPatientNameDt" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdPopupView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="BalanceID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PaymentNo" HeaderText="No. Pembayaran" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfPaymentDate" HeaderText="Tanggal Pembayaran" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="PaymentMethod" HeaderText="Cara Pembayaran" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="cfBalanceBEGIN" HeaderText="Balance BEGIN" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" />
                                        <asp:BoundField DataField="cfBalanceIN" HeaderText="Balance IN" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" />
                                        <asp:BoundField DataField="cfBalanceOUT" HeaderText="Balance OUT" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" />
                                        <asp:BoundField DataField="cfBalanceEND" HeaderText="Balance END" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" />
                                        <asp:BoundField DataField="cfCreatedDate" HeaderText="Dibuat Pada" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160px" />
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
