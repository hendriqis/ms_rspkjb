<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ProcessTransferDownPayment.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessTransferDownPayment" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedDepartmentID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.lnkPaymentNo').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/Information/InfoPatientPaymentCtl.ascx");
                openUserControlPopup(url, id, 'Informasi Pembayaran Pasien', 1000, 200);
            });
        });

        $('.btnTransfer').live('click', function () {
            var paymentID = $(this).closest('tr').find('.hdnKeyField').val();
            cbpView.PerformCallback("transfer|" + paymentID);
        });

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'transfer') {
                if (param[1] == 'fail') {
                    showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
                } else {
                    showToast("INFORMATION", "Process transfer down payment success.");
                }
                cbpView.PerformCallback("refresh");
            }
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdPayment"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 150px" align="left">
                                                            <%= GetLabel("No. Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%= GetLabel("Waktu Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Cara Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px" align="right">
                                                            <%=GetLabel("Total Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Dibuat Oleh")%>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <%=GetLabel("Transfer")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="10">
                                                            <%=GetLabel("Tidak ada uang muka yang dapat diproses transfer.") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdPayment"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 150px" align="left">
                                                                <%= GetLabel("No. Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%= GetLabel("Waktu Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Cara Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px" align="right">
                                                            <%=GetLabel("Total Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Dibuat Oleh")%>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <%=GetLabel("Transfer")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trItem">
                                                    <td align="left">
                                                        <div>
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("PaymentID")%>" />
                                                            <a class="lnkPaymentNo">
                                                                <%#: Eval("PaymentNo") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("PaymentDateTimeInString") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("PaymentType") %></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("TotalPaymentAmount", "{0:N2}")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("CreatedByUser") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <input type="button" id="btnTransfer" class="btnTransfer" value="Transfer" runat="server" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
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
