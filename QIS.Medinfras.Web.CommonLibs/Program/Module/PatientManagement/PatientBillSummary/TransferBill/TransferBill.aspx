<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="TransferBill.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransferBill" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessGenerateBill" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Transfer")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnIsHasPatientBill" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        }

        $('#<%= btnProcessGenerateBill.ClientID %>').live('click', function () {
            if ($('#<%=hdnIsUsedBillTransferBlock.ClientID %>').val() == "0") {
                var registrationID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
                var filterExpression = "RegistrationID = " + registrationID;
                var outstanding = 0;
                var message = "Masih ada outstanding order di registrasi asal, lanjutkan proses ?"

                if ($('#<%=hdnRowCountData.ClientID %>').val() != "0") {
                    Methods.getObject('GetvRegistrationOutstandingInfoList', filterExpression, function (result) {
                        if (result != null) {
                            outstanding = result.ServiceOrder + result.PrescriptionOrder + result.PrescriptionReturnOrder + result.LaboratoriumOrder + result.RadiologiOrder + result.OtherOrder;
                        }
                    });

                    if (outstanding == 0) {
                        onCustomButtonClick('transferbill');
                    }
                    else {
                        showToastConfirmation(message, function (resultMessage) {
                            if (resultMessage) {
                                onCustomButtonClick('transferbill');
                            }
                        });
                    }
                }
                else {
                    showToast('Warning', 'Tidak ada tagihan yang dapat di transfer');
                }
            } else {
                var registrationID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
                var filterExpression = "RegistrationID = " + registrationID;
                var outstanding = 0;
                var message = "Masih ada outstanding order di registrasi asal, lanjutkan proses ?"

                if ($('#<%=hdnRowCountData.ClientID %>').val() != "0") {
                    Methods.getObject('GetvRegistrationOutstandingInfoList', filterExpression, function (result) {
                        if (result != null) {
                            outstanding = result.ServiceOrder + result.PrescriptionOrder + result.PrescriptionReturnOrder + result.LaboratoriumOrder + result.RadiologiOrder + result.OtherOrder;
                        }
                    });

                    if (outstanding == 0) {
                        onCustomButtonClick('transferbill');
                    }
                    else {
                        showToast('Masih ada order yang belum diproses di registrasi asal');
                    }
                }
                else {
                    showToast('Warning', 'Tidak ada tagihan yang dapat di transfer');
                }
            }
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }

        $('.imgInfo.imgLink').die('click');
        $('.imgInfo.imgLink').live('click', function () {
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Registration/OutstandingPaymentInformation.ascx');
            var mrn = $('#<%=hdnMRN.ClientID %>').val();
            var id = mrn;
            openUserControlPopup(url, id, 'Payment Outstanding Information', 780, 500);
        });
    </script>
    <div>
        <input type="hidden" id="hdnLstSelectedValue" runat="server" value="" />
        <input type="hidden" id="hdnRowCountData" runat="server" value="0" />
        <input type="hidden" id="hdnIsTransferUsedHighestClass" runat="server" value="0" />
        <input type="hidden" id="hdnIsUsedBillTransferBlock" runat="server" value="0" />
        <input type="hidden" id="hdnIsUsedDifferentCustomerBlock" runat="server" value="0" />
        <input type="hidden" id="hdnIsDifferentCustomer" runat="server" value="0" />
        <input type="hidden" id="hdnIsUsedHasDownPaymentBlock" runat="server" value="0" />
        <input type="hidden" id="hdnIsHasDownPayment" runat="server" value="0" />
        <div>
            <table align="right">
                <tr id="trWarning" align="right" runat="server">
                    <td rowspan="2" align="right">
                        <img class="imgLink imgInfo" height="60" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>'
                            alt='' />
                    </td>
                    <td style="vertical-align: center">
                        <label class="lblWarning" id="lblWarningOutstandingAR" runat="server">
                            <%=GetLabel("Masih ada piutang pribadi yang masih outstanding.")%></label>
                        <br />
                        <label class="lblWarning" id="lblWarningDifferentCustomer" runat="server">
                            <%=GetLabel("Penjamin bayar di registrasi asal berbeda dengan penjamin bayar registrasi ini.")%></label><br />
                        <label class="lblWarning" id="lblWarningHasDownPayment" runat="server">
                            <%=GetLabel("Registrasi asal memiliki uang muka.")%></label>
                        <label class="lblWarning" id="lblWarningHasPatientBill" runat="server">
                            <%=GetLabel("Sudah ada Tagihan Pasien yang terbentuk di registrasi asal.")%></label>
                    </td>
                </tr>
            </table>
            <table class="tblContentArea">
                <tr>
                    <td>
                        <div>
                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(s); onLoad();}" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                            margin-right: auto; position: relative; font-size: 0.95em;">
                                            <div class="containerUlTabPage">
                                                <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                                                    <li class="selected" contentid="containerService">
                                                        <%=GetLabel("Pelayanan") %></li>
                                                    <li contentid="containerDrugMS">
                                                        <%=GetLabel("Obat & Alkes") %></li>
                                                    <li contentid="containerLogistics">
                                                        <%=GetLabel("Barang Umum") %></li>
                                                </ul>
                                            </div>
                                            <div id="containerService" class="containerBillSummaryDetailAll">
                                                <uc1:ServiceCtl ID="ctlService" runat="server" />
                                            </div>
                                            <div id="containerDrugMS" style="display: none" class="containerBillSummaryDetailAll">
                                                <uc1:DrugMSCtl ID="ctlDrugMS" runat="server" />
                                            </div>
                                            <div id="containerLogistics" style="display: none" class="containerBillSummaryDetailAll">
                                                <uc1:DrugMSCtl ID="ctlLogistic" runat="server" />
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
    </div>
</asp:Content>
