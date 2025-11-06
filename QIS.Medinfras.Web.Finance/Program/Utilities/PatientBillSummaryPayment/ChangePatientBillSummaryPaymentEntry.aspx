<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangePatientBillSummaryPaymentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ChangePatientBillSummaryPaymentEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangePaymentBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnChangePaymentBack.ClientID %>').live('click', function () {
            showLoadingPanel();
            document.location = ResolveUrl('~/Program/Utilities/PatientBillSummaryPayment/ChangePatientBillSummaryPayment.aspx');
        });

        function onRefreshControl() {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('.lblBankInformation.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var paymentID = $(this).closest('tr').find('.keyField').html();
            var id = paymentID;
            var url = ResolveUrl("~/Program/Utilities/PatientBillSummaryPayment/ChangePatientBillSummaryPaymentDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'Bank Information', 700, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="PaymentNo" ItemStyle-HorizontalAlign="Left" HeaderText="Payment No"
                                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="PaymentDateInString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Payment Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                <asp:BoundField DataField="PaymentTime" ItemStyle-HorizontalAlign="Center" HeaderText="Payment Time"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                <asp:BoundField DataField="PaymentType" ItemStyle-HorizontalAlign="Center" HeaderText="Payment Type"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                                <asp:TemplateField HeaderText="Payment Amount" ItemStyle-HorizontalAlign="Right"
                                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <label>
                                                            <%#:Eval("TotalPaymentAmount", "{0:N}")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank Information" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <label class="lblBankInformation lblLink">
                                                            <%=GetLabel("Informasi") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
