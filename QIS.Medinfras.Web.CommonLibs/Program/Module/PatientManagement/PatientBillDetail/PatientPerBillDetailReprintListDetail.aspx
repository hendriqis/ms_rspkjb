<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="PatientPerBillDetailReprintListDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPerBillDetailReprintListDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnBack.ClientID %>').live('click', function () {
            showLoadingPanel();
            var reqID = $('#<%=hdnRequestID.ClientID %>').val();
            var url = "~/Libs/Program/Module/PatientManagement/PatientBillDetail/PatientPerBillDetailReprintList.aspx?id=" + reqID;
            document.location = ResolveUrl(url);
        });

        $(function () {
            $('.lnkTransactionNo').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillDiscount/PatientBillSummaryDiscountDtCtl.ascx");
                openUserControlPopup(url, id, 'Informasi Tagihan Pasien Item', 1100, 500);
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var billingID = $('#<%=hdnID.ClientID %>').val();

            filterExpression.text = 'RegistrationID = ' + registrationID + '|' + 'PatientBillingID = ' + billingID;

            return true;
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
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
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("No. Tagihan Pasien")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal Proses")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("JUMLAH TAGIHAN")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("INSTANSI")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("PASIEN")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("TOTAL")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
                                                            <%=GetLabel("Tidak ada informasi tagihan pasien pada saat ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("No. Tagihan Pasien")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal Proses")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("JUMLAH")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("INSTANSI")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("PASIEN")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("TOTAL")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientBillingID")%>" />
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("PatientBillingNo")%></a>
                                                            <div>
                                                                <%#: Eval("BillingDateInString")%>
                                                                <%#: Eval("BillingTime")%>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div>
                                                                <%#: Eval("PayerBillAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div>
                                                                <%#: Eval("PatientBillAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalBillAmount", "{0:N}")%></div>
                                                        </div>
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