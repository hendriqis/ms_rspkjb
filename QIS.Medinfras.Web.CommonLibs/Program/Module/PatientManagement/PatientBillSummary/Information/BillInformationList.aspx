<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="BillInformationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.BillInformationList" %>

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
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <%--<li id="btnProcessTransactionVerification" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Cetak")%></div>
    </li>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
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

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'sendBARNotificationMessage') {
                if ($('#<%:hdnID.ClientID %>').val() != '' || $('#<%:hdnID.ClientID %>').val() != '0') {
                    var messageType = "02";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = $('#<%:hdnID.ClientID %>').val();
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var billingID = $('#<%=hdnID.ClientID %>').val();

            if (billingID == '' || billingID == 0) {
                errMessage.text = "Tidak ada Tagihan pasien yang dapat di cetak!";
                return false;
            }
            else {
                if (code == 'PM-00377' || code == 'PM-00378' || code == 'PM-00379' || code == 'PM-00380' || code == 'PM-00381' || code == 'PM-00382') {
                    filterExpression.text = 'RegistrationID = ' + registrationID + '|PatientBillingID = ' + billingID;
                    return true;
                }
                else if (code == 'PM-00490' || code == 'PM-00278' || code == 'PM-00279' || code == 'PM-00492' || code == 'PM-00493' || code == 'PM-00494' || code == 'PM-00495' || code == 'PM-00496' || code == 'PM-00294' || code == 'PM-00295' || code == 'PM-00287') {
                    filterExpression.text = 'RegistrationID = ' + registrationID;
                    return true;
                }
                 else {
                    filterExpression.text = 'RegistrationID = ' + registrationID + ' AND PatientBillingID = ' + billingID;
                    return true;
                }
            }
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
