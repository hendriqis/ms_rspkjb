<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="InfoPatientPayment.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientPayment" %>

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
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.lnkPaymentNo').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/Information/InfoPatientPaymentCtl.ascx");
                openUserControlPopup(url, id, 'Informasi Pembayaran Pasien', 1000, 200);
            });

            $('.grdBilling tr:gt(0):not(.trEmpty)').click(function () {
                if ($('.grdBilling tr').index($(this)) > 1) {
                    $('.grdBilling tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.hdnKeyField').val());
                }
            });

            $('.grdBilling tr:eq(2)').click();
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
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
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 150px">
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
                                                            <%=GetLabel("Status Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Dibuat Oleh")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="7">
                                                            <%=GetLabel("Tidak ada informasi tagihan pasien pada saat ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 150px">
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
                                                            <%=GetLabel("Status Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Dibuat Oleh")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trItem">
                                                    <td align="center">
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
                                                            <%#: Eval("TransactionStatusWatermark") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("CreatedByUser") %></div>
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
