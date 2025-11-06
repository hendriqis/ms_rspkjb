<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PatientBillSummaryPayment.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPayment" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnCardDetailInformationMandatory" runat="server" />
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPatientName" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnIsCustomerTypeBPJS" runat="server" />
    <input type="hidden" value="" id="hdnIsSendToLISAfterSavePayment" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingEdc" runat="server" />
    <input type="hidden" value="" id="hdnBridgingEdcCode" runat="server" />
    <input type="hidden" value="" id="hdnEDCMachineID" runat="server" />
    <input type="hidden" value="0" id="hdnIsBridgingToPaymentGateway" runat="server" />
    <input type="hidden" value="0" id="hdnIsBridgingToMaspion" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
    <script type="text/javascript">
        //#region Right Panel
        $(function () {
            setRightPanelButtonEnabled();
            ////setConfigEDC();
        });

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoInvoice') {
                var param = $('#<%:hdnPaymentHdID.ClientID %>').val();
                return param;
            }
            else if (code == 'sendBARNotificationMessage') {
                if ($('#<%:hdnPaymentHdID.ClientID %>').val() != '' || $('#<%:hdnPaymentHdID.ClientID %>').val() != '0') {
                    var messageType = "03";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = $('#<%:hdnPaymentHdID.ClientID %>').val();
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else if (code == 'sendEdcStatus') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val();
                return param;
            }
            else if (code == 'sendPaymentGateway') {
                var paymentCount = grdPayment.getTableData().split("|");
                if (cboPaymentType.GetValue() == "X034^002") {
                    if (paymentCount.length == 2) {
                        var checkSecondPayment = paymentCount[1].split(";");
                        var checkPayment = checkSecondPayment[0];
                        if (checkPayment == "0") {
                            if (paymentCount.length == 2) {
                                var RegistrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                                var billingID = $('#<%=hdnListBillingID.ClientID %>').val();
                                var billingNo = ddeBillingNo.GetText();
                                var billingTotal = $('#<%=txtBillingTotalPatient.ClientID %>').val();
                                var payment = paymentCount[0].split(";");
                                var paymentMethod = payment[2]; //payment method
                                var bankID = payment[5]; //bank ID
                                var Chasier = cboCashierGroup.GetValue();
                                var Shift = cboShift.GetValue();
                                var id = billingID + "|" + billingNo + "|" + billingTotal + "|" + paymentMethod + "|" + bankID + "|" + RegistrationID + "|" + Chasier + "|" + Shift;

                                return id;
                            }
                            else {
                                displayMessageBox("WARNING", "Harap pilih satu metode pembayaran");

                            }
                        }
                    }

                }
                else {
                    displayMessageBox("WARNING", "Hanya bisa untuk tipe pembayaran : PELUNASAN");
                    return false;

                }

            }
            else {
                return $('#<%:hdnPaymentHdID.ClientID %>').val();
            }
        }
        function isVirtualPayment() {
            var paymentCount = grdPayment.getTableData().split("|");
            var iStrue = false;
            if (paymentCount.length > 0) {
                for (var i = 0; i < paymentCount.length; i++) {
                    if (paymentCount[i] != "") {
                        var check = paymentCount[i].split(";");
                        if (check[2] == "X035^013") {
                            iStrue = true;
                            return iStrue;
                        }
                    }

                }
            }
            return iStrue;
        }
        function onAfterVirtualPayment(value) {
            onLoadObject(value);
        }
        function setRightPanelButtonPaymentgateway() {
            var paymentCount = grdPayment.getTableData().split("|");
            if (cboPaymentType.GetValue() == "X034^002") {
                console.log(paymentCount);
                if (isVirtualPayment()) {
                    //                    $('#btnSendPaymentGateway').removeAttr('enabled');
                } else {
                    $('#btnSendPaymentGateway').attr('enabled', 'false');
                }
                if (paymentCount.length == 2) {
                    var checkSecondPayment = paymentCount[1].split(";");
                    var checkPayment = checkSecondPayment[0];
                    if (checkPayment == "0") {
                        if (paymentCount.length == 2) {
                            //                            $('#btnSendPaymentGateway').removeAttr('enabled');
                        } else {
                            $('#btnSendPaymentGateway').attr('enabled', 'false');
                        }
                    } else {
                        $('#btnSendPaymentGateway').attr('enabled', 'false');
                    }
                } else {
                    $('#btnSendPaymentGateway').attr('enabled', 'false');
                }
            } else {
                $('#btnSendPaymentGateway').attr('enabled', 'false');
            }
        }
        function setRightPanelButtonEnabled() {
            var isEnabled = false;
            if (!getIsAdd()) {
                var paymentType = cboPaymentType.GetValue();
                var paymentAllocation = parseInt($('#<%=hdnPaymentAllocation.ClientID %>').val());
                if (paymentType == Constant.PaymentType.DOWN_PAYMENT && paymentAllocation > 0)
                    isEnabled = true;
            }

            if (!isEnabled) {
                $('#btnBillAllocation').attr('enabled', 'false');
            }
            else {
                $('#btnBillAllocation').removeAttr('enabled');
            }

            if ($('#<%:hdnPaymentHdID.ClientID %>').val() == '' || $('#<%:hdnPaymentHdID.ClientID %>').val() == '0') {
                $('#btninfoInvoice').attr('enabled', 'false');
                $('#btnSetBPJSClaimType').attr('enabled', 'false');
                $('#btnSendBARNotification').attr('enabled', 'false');
            }
            else {
                var filter = "PaymentID = " + $('#<%:hdnPaymentHdID.ClientID %>').val();
                $('#btnSendBARNotification').removeAttr('enabled');
                Methods.getObject('GetPatientPaymentHdList', filter, function (result) {
                    if ((result.GCPaymentType == Constant.PaymentType.AR_PATIENT || result.GCPaymentType == Constant.PaymentType.AR_PAYER) && result.GCTransactionStatus != Constant.TransactionStatus.VOID) {
                        $('#btninfoInvoice').removeAttr('enabled');

                        if (result.GCPaymentType == Constant.PaymentType.AR_PAYER) {
                            Methods.getObject('GetvPatientPaymentDtList', filter, function (dt) {
                                if (dt.GCCustomerType == Constant.CustomerType.BPJS) {
                                    $('#btnSetBPJSClaimType').removeAttr('enabled');
                                }
                                else {
                                    $('#btnSetBPJSClaimType').attr('enabled', 'false');
                                }
                            });
                        }
                    }
                    else {
                        $('#btninfoInvoice').attr('enabled', 'false');
                        $('#btnSetBPJSClaimType').attr('enabled', 'false');
                    }
                });
            }


            setRightPanelButtonPaymentgateway();
        }

        $(function () {
            var gender = $('#<%=hdnPatientGender.ClientID %>').val();
            Methods.checkImageError('imgPatientProfilePicture', 'patient', gender);

            $('.lnkTransactionNo').live('click', function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillDiscount/PatientBillSummaryDiscountDtCtl.ascx");
                openUserControlPopup(url, id, 'Penerimaan Pembayaran Pasien Item', 1100, 470);
            });
        });

        function onAfterSaveAddRecord(param) {
            var paymentType = cboPaymentType.GetValue();
            if (param == 'ar') {
                if (paymentType == Constant.PaymentType.CUSTOM || Constant.PaymentType.SETTLEMENT) {
                    if ($('#<%=hdnListBillingID.ClientID %>').val() != '') {
                        openPaymentCreateAccountReceivable();
                    }
                }
            }

            getStatusPerRegOutstanding(); //general ctl
        }

        function onAfterSaveAddRecordEntryPopup() {
            cbpMPEntryContent.PerformCallback('load');
            setRightPanelButtonEnabled();
            getStatusPerRegOutstanding(); //general ctl
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var paymentID = $('#<%=hdnPaymentHdID.ClientID %>').val();
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();

            if (code == 'PM-00124' || code == 'PM-00324' || code == 'PM-002155' || code == 'PM-00639' || code == 'PM-00187' ||
                code == 'PM-002175' || code == 'PM-002176' || code == 'PM-002177' || code == 'PM-00195' || code == 'PM-002167'
                 || code == 'PM-00292' || code == 'PM-90047' || code == 'PM-90048' || code == 'PM-90049' || code == 'PM-90050') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            } else {
                if (paymentID == '' || paymentID == 0) {
                    errMessage.text = 'Please Save Transaction First!';
                    return false;
                }
                else {
                    var paymentType = cboPaymentType.GetValue();
                    if (code == 'PM-00302' || code == 'PM-00309') {
                        if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.CUSTOM || paymentType == Constant.PaymentType.DEPOSIT_OUT) {
                            filterExpression.text = 'PaymentID = ' + paymentID;
                            return true;
                        }
                        else {
                            errMessage.text = 'Pilih pembayaran dengan tipe Pelunasan atau Custom Pelunasan';
                            return false;
                        }
                    }
                    if (code == 'PM-00308' || code == 'PM-00322' || code == 'PM-00328' || code == 'PM-00347' || code == 'PM-90031' || code == 'PM-90032'
                        || code == 'PM-00682') {
                        var filter = "PaymentID = " + paymentID + "AND IsDeleted = 0 AND PaymentAmount < 0";
                        var downpayment = false;
                        Methods.getObject('GetvPatientPaymentDtList', filter, function (result) {
                            if (result != null) {
                                downpayment = true;
                            }
                            else downpayment = false;
                        });
                        if (downpayment == false) {
                            errMessage.text = 'Pembayaran ini tidak memiliki Uang Muka kembali';
                            return false;
                        }
                    }
                    if (code == 'PM-00344') {
                        var filter = "PaymentID = " + paymentID + " AND (TotalPaymentAmount - CashBackAmount) < 0";
                        Methods.getObject('GetPatientPaymentHdList', filter, function (result) {
                            if (result == null) {
                                errMessage.text = 'Cetakan ini hanya untuk pembayaran yang bernilai minus';
                                return false;
                            } else {
                                return true;
                            }
                        });
                    }
                    if (code == 'PM-00304' || code == 'PM-00310') {
                        if (paymentType != Constant.PaymentType.AR_PATIENT) {
                            errMessage.text = 'Pilih pembayaran dengan tipe Piutang Pribadi';
                            return false;
                        }
                    }
                    if (code == 'PM-00306' || code == 'PM-00311') {
                        if (paymentType != Constant.PaymentType.AR_PAYER) {
                            errMessage.text = 'Pilih pembayaran dengan tipe Piutang Instansi';
                            return false;
                        }
                    }
                    if (code == 'PM-00329') {
                        filterExpression.text = "PaymentID = " + paymentID + "AND IsDeleted = 0";
                        return true;
                    }
                    if (code == 'PM-00307' || code == 'PM-00327' || code == 'PM-90030') {
                        if (paymentType != Constant.PaymentType.DOWN_PAYMENT) {
                            errMessage.text = 'Pilih pembayaran dengan tipe Uang Muka';
                            return false;
                        }
                    }
                    if (code == 'PM-00619') {
                        if (paymentType != Constant.PaymentType.DEPOSIT_IN) {
                            errMessage.text = 'Pilih pembayaran dengan tipe Deposit IN';
                            return false;
                        }
                    }
                    if (code == 'PM-00218') {
                        filterExpression.text = paymentID;
                        return true;
                    }
                    if (code == 'PM-00219') {
                        filterExpression.text = paymentID;
                        return true;
                    }

                    if (code == 'PM-00312' || code == 'PM-00313' || code == 'PM-00314' || code == 'PM-00315' ||
                        code == 'PM-00316' || code == 'PM-00317' || code == 'PM-00318' || code == 'PM-00319' ||
                        code == 'PM-00320' || code == 'PM-00321' || code == 'PM-00325' || code == 'PM-00330' ||
                        code == 'PM-00331' || code == 'PM-00350' || code == 'PM-00351' || code == 'PM-00352' ||
                        code == 'PM-00353' || code == 'PM-00354' || code == 'PM-00355' ||
                        code == 'PM-00365' || code == 'PM-00366' || code == 'PM-00367' || code == 'PM-00368' ||
                        code == 'PM-00383' || code == 'PM-00384'
                        ) {
                        filterExpression.text = 'RegistrationID = ' + registrationID;
                        filterExpression.text += '|PaymentID = ' + paymentID;
                        return true;
                    }

                    if (filterExpression.text == '') {
                        filterExpression.text = 'PaymentID = ' + paymentID;
                    } else {
                        filterExpression.text += ' AND PaymentID = ' + paymentID;
                    }
                    return true;
                }
            }
        }

        //#endregion

        //#region Inline Editing AR Payer
        var grdPaymentARPayer = new InlineEditing();
        var numFinishLoad = 0;
        function init2() {
            var listParam = new Array();
            var cboBusinessPartnerID = '<%=cboBusinessPartner.ClientID%>';

            listParam[0] = { "type": "cbo", "className": "cboBusinessPartner", "cboID": cboBusinessPartnerID, "isEnabled": true, "isRequired": true };
            listParam[1] = { "type": "txt", "className": "txtPayment", "isRequired": true, "isEnabled": true, "dataType": "money" };
            listParam[2] = { "type": "txt", "className": "txtLineTotal", "isEnabled": false, "dataType": "money" };

            grdPaymentARPayer.setOnCboValueChangedHandler(function ($row, cboClass, oldValue, newValue) {
                var amount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val()) * -1;
                if (amount < 0) {
                    amount = 0;
                }
                if (amount > 0) {
                    grdPaymentARPayer.setTextBoxProperties($row, 'txtPayment', { "value": amount });
                }
                var payment = parseFloat(grdPaymentARPayer.getTextBoxValue($row, 'txtPayment'));
                grdPaymentARPayer.setTextBoxProperties($row, 'txtLineTotal', { "value": payment });
                calculatePayerPaymentDtTotal();
            });

            grdPaymentARPayer.setOnTxtValueChangedHandler(function ($row, txtClass, oldValue, newValue) {
                if (txtClass == 'txtPayment') {
                    var payment = parseFloat(grdPaymentARPayer.getTextBoxValue($row, 'txtPayment'));
                    grdPaymentARPayer.setTextBoxProperties($row, 'txtLineTotal', { "value": payment });
                    calculatePayerPaymentDtTotal();
                }
            });

            grdPaymentARPayer.setOnRowDeletedHandler(function (objDeleted) {
                calculatePayerPaymentDtTotal();
            });

            grdPaymentARPayer.init('tblPaymentDtPayer', listParam);
            grdPaymentARPayer.addRow(true);
        }
        //#endregion

        //#region Inline Editing Pelunasan
        var grdPayment = new InlineEditing();
        function init() {
            var listParam = new Array();
            var cboPaymentMethodID = '<%=cboPaymentMethod.ClientID%>';
            var cboEDCMachineID = '<%=cboEDCMachine.ClientID%>';
            var cboBankID = '<%=cboBank.ClientID%>';

            listParam[0] = { "type": "cbo", "className": "cboPaymentMethod", "cboID": cboPaymentMethodID, "isUnique": false, "isEnabled": true, "isRequired": true };
            listParam[1] = { "type": "cbo", "className": "cboEDCMachine", "cboID": cboEDCMachineID, "isRequired": true, "isUnique": false, "isEnabled": false };
            listParam[2] = { "type": "bte", "className": "bteCardInformation", "isEnabled": false, "isRequired": true, "isButtonEnabled": false };
            listParam[3] = { "type": "cbo", "className": "cboBank", "cboID": cboBankID, "isUnique": false, "isRequired": true, "isEnabled": false };
            listParam[4] = { "type": "txt", "className": "txtReferenceNo", "isRequired": true, "isEnabled": false };
            listParam[5] = { "type": "txt", "className": "txtPayment", "isRequired": true, "isEnabled": false, "dataType": "money" };
            listParam[6] = { "type": "txt", "className": "txtFee", "isEnabled": false, "dataType": "money" };
            listParam[7] = { "type": "txt", "className": "txtLineTotal", "isEnabled": false, "dataType": "money" };
            listParam[8] = { "type": "hdn", "className": "hdnCardType" };
            listParam[9] = { "type": "hdn", "className": "hdnCardNo1" };
            listParam[10] = { "type": "hdn", "className": "hdnCardNo4" };
            listParam[11] = { "type": "hdn", "className": "hdnHolderName" };
            listParam[12] = { "type": "hdn", "className": "hdnExpiredDateMonth" };
            listParam[13] = { "type": "hdn", "className": "hdnExpiredDateYear" };
            listParam[14] = { "type": "hdn", "className": "hdnCardFee", "value": "0" };
            listParam[15] = { "type": "hdn", "className": "hdnCardProvider" };
            listParam[16] = { "type": "hdn", "className": "hdnBatchNo" };
            listParam[17] = { "type": "hdn", "className": "hdnTraceNo" };
            listParam[18] = { "type": "hdn", "className": "hdnReferenceNo" };
            listParam[19] = { "type": "hdn", "className": "hdnApprovalCode" };
            listParam[20] = { "type": "hdn", "className": "hdnTerminalID" };
            listParam[21] = { "type": "hdn", "className": "hdnPaymentAmountInCard" };

            grdPayment.setOnBteButtonClickHandler(function ($row, bteClass) {
                $currEditedRow = $row;
                if (bteClass == 'bteCardInformation') {
                    var cardtype = grdPayment.getCellHiddenValue($row, 'hdnCardType');
                    var cardNo1 = grdPayment.getCellHiddenValue($row, 'hdnCardNo1');
                    var cardNo4 = grdPayment.getCellHiddenValue($row, 'hdnCardNo4');
                    var holderName = grdPayment.getCellHiddenValue($row, 'hdnHolderName');
                    var expiredDateMonth = grdPayment.getCellHiddenValue($row, 'hdnExpiredDateMonth');
                    var expiredDateYear = grdPayment.getCellHiddenValue($row, 'hdnExpiredDateYear');
                    var cardProvider = grdPayment.getCellHiddenValue($row, 'hdnCardProvider');
                    var batchNo = grdPayment.getCellHiddenValue($row, 'hdnBatchNo');
                    var traceNo = grdPayment.getCellHiddenValue($row, 'hdnTraceNo');
                    var referenceNo = grdPayment.getCellHiddenValue($row, 'hdnReferenceNo');
                    var approvalCode = grdPayment.getCellHiddenValue($row, 'hdnApprovalCode');
                    var terminalID = grdPayment.getCellHiddenValue($row, 'hdnTerminalID');
                    var paymentAmount = grdPayment.getCellHiddenValue($row, 'hdnPaymentAmountInCard');

                    var payment = parseFloat(grdPayment.getTextBoxValue($row, 'txtPayment'));

                    cboCardType.SetValue(cardtype);
                    $('#<%=txtCardNumber1.ClientID %>').val(cardNo1);
                    $('#<%=txtCardNumber4.ClientID %>').val(cardNo4);

                    if ($('#<%=txtPaymentNo.ClientID %>').val() == "") {
                        var d = new Date();
                        if (holderName == '') {
                            holderName = '-';
                        }
                        if (expiredDateMonth == '') {
                            expiredDateMonth = d.getMonth() + 1;
                        }
                        if (expiredDateYear == '') {
                            expiredDateYear = d.getFullYear();
                        }
                    }

                    $('#<%=txtHolderName.ClientID %>').val(holderName);
                    $('#<%=txtBatchNo.ClientID %>').val(batchNo);
                    $('#<%=txtTraceNo.ClientID %>').val(traceNo);
                    $('#<%=txtReferenceNo.ClientID %>').val(referenceNo);
                    $('#<%=txtApprovalCode.ClientID %>').val(approvalCode);
                    $('#<%=txtTerminalID.ClientID %>').val(terminalID);
                    $('#<%=txtPaymentAmount.ClientID %>').val(payment);
                    cboCardDateMonth.SetValue(expiredDateMonth);
                    cboCardDateYear.SetValue(expiredDateYear);
                    cboCardProvider.SetValue(cardProvider);



                    $('#btnGetResponseEDC').attr('style', 'display:none');

                    cboCardType.SetFocus();
                    pcCardInformation.Show();
                }
            });

            grdPayment.setOnCboValueChangedHandler(function ($row, cboClass, oldValue, newValue) {
                if (cboClass == 'cboPaymentMethod') {

                    grdPayment.setTextBoxProperties($row, 'txtPayment', { "isEnabled": true });
                    grdPayment.setCellHiddenValue($row, 'hdnCardFee ', '0');
                    grdPayment.setTextBoxProperties($row, 'txtFee', { "value": 0 });
                    var isCreditOrDebit = (newValue == 'X035^002' || newValue == 'X035^003');
                    grdPayment.setComboBoxProperties($row, 'cboEDCMachine', { "isEnabled": isCreditOrDebit, "value": "" });
                    grdPayment.setButtonEditProperties($row, 'bteCardInformation', { "isButtonEnabled": isCreditOrDebit, "value": "" });
                    var isBankTransferOrVoucher = (newValue == 'X035^002' || newValue == 'X035^003' || newValue == 'X035^004' || newValue == 'X035^008' || newValue == 'X035^013');
                    grdPayment.setTextBoxProperties($row, 'txtReferenceNo', { "isEnabled": isBankTransferOrVoucher, "value": "" });
                    grdPayment.setComboBoxProperties($row, 'cboBank', { "isEnabled": (newValue == 'X035^004' || newValue == 'X035^013'), "value": "" });
                    //var isBankTransferOrCreditOrDebit = (isCreditOrDebit || isBankTransfer);
                    //grdPayment.setComboBoxProperties($row, 'cboBank', { "isEnabled": isBankTransferOrCreditOrDebit, "value": "" });
                    if (isCreditOrDebit) {
                        var amount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val()) * -1;
                        if (amount < 0)
                            amount = 0;
                        grdPayment.setTextBoxProperties($row, 'txtPayment', { "value": amount });
                    }
                    else {
                        if (grdPayment.getRowEnabled($row)) {
                            grdPayment.setTextBoxProperties($row, 'txtPayment', { "value": 0 });
                            grdPayment.setTextBoxProperties($row, 'txtLineTotal', { "value": 0 });
                        }
                    }
                    calculateCardFeeAndLineTotal($row);
                    //                    cbpChangeBank.PerformCallback(newValue);
                    cboBank.ClearItems();
                    var filter = "IsDeleted = 0 ";
                    if (newValue == 'X035^013') {
                        filter += "AND IsVirtualPayment = 1"
                    }
                    else {
                        filter += "AND IsVirtualPayment = 0"
                    }
                    filter += "ORDER BY BankName ASC";
                    Methods.getListObject("GetBankList", filter, function (bankRes) {
                        for (var i = 0; i < bankRes.length; i++) {
                            cboBank.AddItem(bankRes[i].BankName, bankRes[i].BankID);
                        }
                    });

                    /////khusus untuk edc sementara baru type debit
                    setConfigEdcForm(newValue);
                }
                else if (cboClass == 'cboEDCMachine') {
                    getCardFee($row);
                }
                else if (cboClass == 'cboBank') {
                    $('#btnSendPaymentGateway').removeAttr('enabled');
                }
            });

            grdPayment.setOnTxtValueChangedHandler(function ($row, txtClass, oldValue, newValue) {
                if (txtClass == 'txtPayment') {
                    calculateCardFeeAndLineTotal($row);
                }
            });

            grdPayment.setOnRowDeletedHandler(function (objDeleted) {
                calculatePaymentDtTotal();
            });

            grdPayment.init('tblPaymentDt', listParam);
            grdPayment.addRow(true);
        }

        function calculateCardFeeAndLineTotal($row) {
            var payment = parseFloat(grdPayment.getTextBoxValue($row, 'txtPayment'));
            var cardFeeInPercentage = parseFloat(grdPayment.getCellHiddenValue($row, 'hdnCardFee'));
            var cardFee = payment * cardFeeInPercentage / 100;

            var lineTotal = payment + cardFee;
            grdPayment.setTextBoxProperties($row, 'txtFee', { "value": cardFee });
            grdPayment.setTextBoxProperties($row, 'txtLineTotal', { "value": lineTotal });

            $('#<%=txtPaymentAmount.ClientID %>').val(payment);

            calculatePaymentDtTotal();

            setRightPanelButtonPaymentgateway();
        }

        function getCardFee($row) {
            var cardProvider = grdPayment.getCellHiddenValue($row, 'hdnCardProvider');
            var cardtype = grdPayment.getCellHiddenValue($row, 'hdnCardType');
            var edcMachine = grdPayment.getComboBoxValue($row, 'cboEDCMachine');
            if (edcMachine != '' && cardtype != '' && cardProvider != '') {
                var filterExpressionEDC = "EDCMachineID = '" + edcMachine + "' AND IsDeleted = 0 AND IsChargeFeeToPatient = 1";
                Methods.getObject('GetEDCMachineList', filterExpressionEDC, function (result) {
                    if (result != null) {
                        var filterExpression = $('#<%=hdnCreditCardFeeFilterExpression.ClientID %>').val().replace('[GCCardType]', cardtype).replace('[GCCardProvider]', cardProvider).replace('[EDCMachineID]', edcMachine);
                        filterExpression += " AND IsDeleted = 0";
                        Methods.getObject('GetCreditCardList', filterExpression, function (resultCC) {
                            if (resultCC != null) {
                                var paymentDate = $('#<%:txtPaymentDate.ClientID %>').val();
                                var paymentDateInDatePicker = Methods.getDatePickerDate(paymentDate);
                                var paymentDateFormatString = Methods.dateToString(paymentDateInDatePicker);
                                var filterExpressionEDCCardFee = "CreditCardID = '" + resultCC.CreditCardID + "' AND IsDeleted = 0 AND '" + paymentDateFormatString + "' >= EffectiveDate ORDER BY ID DESC";
                                Methods.getObject('GetEDCCardFeeList', filterExpressionEDCCardFee, function (resultEDCCardFee) {
                                    if (resultEDCCardFee != null) {
                                        if (resultEDCCardFee.SurchargeFee <= 0) {
                                            grdPayment.setCellHiddenValue($row, 'hdnCardFee ', resultCC.CreditCardFee);
                                        }
                                        else {
                                            grdPayment.setCellHiddenValue($row, 'hdnCardFee ', resultEDCCardFee.SurchargeFee);
                                        }
                                    }
                                    else {
                                        grdPayment.setCellHiddenValue($row, 'hdnCardFee ', resultCC.CreditCardFee);
                                    }
                                });
                            }
                            else {
                                grdPayment.setCellHiddenValue($row, 'hdnCardFee ', 0);
                            }
                        });
                    }
                    else {
                        grdPayment.setCellHiddenValue($row, 'hdnCardFee ', 0);
                    }
                    calculateCardFeeAndLineTotal($row);
                });
            }
            else {
                grdPayment.setCellHiddenValue($row, 'hdnCardFee ', '0');
                calculateCardFeeAndLineTotal($row);
            }
        }

        function calculatePaymentDtTotal() {
            var totalPayment = grdPayment.getColumnTotal('txtPayment');
            var totalCardFee = grdPayment.getColumnTotal('txtFee');
            var totalLineTotal = grdPayment.getColumnTotal('txtLineTotal');

            $('#tdTotalPatient').html(totalPayment.formatMoney(2, '.', ','));
            $('#tdTotalCardFee').html(totalCardFee.formatMoney(2, '.', ','));
            $('#tdTotalLineTotal').html(totalLineTotal.formatMoney(2, '.', ','));

            $('#<%=hdnTotalPaymentAmount.ClientID %>').val(totalPayment);
            $('#<%=hdnTotalFeeAmount.ClientID %>').val(totalCardFee);

            calculateCashbackAmount();

        }

        function calculatePayerPaymentDtTotal() {
            var totalPayment = grdPaymentARPayer.getColumnTotal('txtPayment');
            var totalLineTotal = grdPaymentARPayer.getColumnTotal('txtLineTotal');
            $('#tdTotalPayer').html(totalPayment.formatMoney(2, '.', ','));
            $('#tdTotalLineAmountPayer').html(totalLineTotal.formatMoney(2, '.', ','));
            $('#<%=hdnTotalPayerPaymentAmount.ClientID %>').val(totalPayment);
            calculateCashbackAmountPayer();
        }

        function calculateCashbackAmountPayer() {
            var totalBilling = parseFloat($('#<%=txtBillingTotalPayer.ClientID %>').attr('hiddenVal'));
            var totalPayment = parseFloat($('#<%=hdnTotalPayerPaymentAmount.ClientID %>').val());
            var cashBackAmount = totalPayment - totalBilling;
            $('#<%=hdnCashbackAmount.ClientID %>').val(cashBackAmount.toFixed(2));
            $('#<%=txtCashbackAmount.ClientID %>').val(cashBackAmount.toFixed(2)).trigger('changeValue');
        }

        function calculateCashbackAmount() {
            var totalBilling = parseFloat($('#<%=txtBillingTotalPatient.ClientID %>').attr('hiddenVal'));
            var totalPayment = parseFloat($('#<%=hdnTotalPaymentAmount.ClientID %>').val());
            var cashBackAmount = totalPayment - totalBilling;
            $('#<%=hdnCashbackAmount.ClientID %>').val(cashBackAmount.toFixed(2));
            $('#<%=txtCashbackAmount.ClientID %>').val(cashBackAmount.toFixed(2)).trigger('changeValue');
        }

        function closePcCardInformation(action) {
            if (action == 'save') {
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardType', cboCardType.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardNo1', $('#<%=txtCardNumber1.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardNo4', $('#<%=txtCardNumber4.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnHolderName', $('#<%=txtHolderName.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnExpiredDateMonth', cboCardDateMonth.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnExpiredDateYear', cboCardDateYear.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardProvider', cboCardProvider.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnBatchNo', $('#<%=txtBatchNo.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnTraceNo', $('#<%=txtTraceNo.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnReferenceNo', $('#<%=txtReferenceNo.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnApprovalCode', $('#<%=txtApprovalCode.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnTerminalID', $('#<%=txtTerminalID.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnPaymentAmountInCard', $('#<%=txtPaymentAmount.ClientID %>').val());

                grdPayment.setTextBoxProperties($currEditedRow, 'txtReferenceNo', { "value": $('#<%=txtReferenceNo.ClientID %>').val() });

                //Payment Amount from Popup
                grdPayment.setTextBoxProperties($currEditedRow, 'txtPayment', { "value": $('#<%=txtPaymentAmount.ClientID %>').val() });

                var cardInformation = $('#<%=txtCardNumber1.ClientID %>').val() + '-XXXX-XXXX-' + $('#<%=txtCardNumber4.ClientID %>').val();
                grdPayment.setButtonEditProperties($currEditedRow, 'bteCardInformation', { value: cardInformation });
                grdPayment.setButtonEditFocus($currEditedRow, 'bteCardInformation');
                pcCardInformation.Hide();
                getCardFee($currEditedRow);
            }
            else if (action == 'send') {
                if (cboCardType.GetValue() == null || cboCardProvider.GetValue() == null || $('#<%=txtHolderName.ClientID %>').val() == '' || cboCardDateMonth.GetValue() == null || cboCardDateYear.GetValue() == null || cboLocationEDC.GetValue() == null) {
                    alert("Silahkan lengkapi tipe kartu, bank penerbit, pemegang kartu, dan masa berlaku kartu");
                    return false;
                }
                else {
                    ////// var MessageType = Constant.Sender.MD201; // BCA
                    var cashBackAmount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val());
                    if (cashBackAmount < 0) {
                        if (confirm("Nilai pembayaran EDC masih kurang dari tagihan pasien. Lanjutkan proses? (Jika OK maka akan terbentuk Pelunasan & Piutang Pasien jika pembayaran berhasil dilakukan dengan edc)")) {
                            SendToEdc();
                            return true;
                        } else {
                            return false;
                        }
                    }
                    else {
                        SendToEdc();
                    }

                }
            }
            else if (action == 'void') {
                var TransactionType = "06"; //kode void
                var TerminalID = "T01";
                var HealthcareID = $('#<%=hdnHealthcareID.ClientID %>').val();
                var RegistrationNo = $('#<%=hdnRegistrationNo.ClientID %>').val();
                var MRN = $('#<%=hdnMRN.ClientID %>').val();
                var MedicalNo = $('#<%=hdnMedicalNo.ClientID %>').val();
                var PatientName = $('#<%=hdnPatientName.ClientID %>').val();
                var DateOfBirth = $('#<%=hdnDateOfBirth.ClientID %>').val();
                var TransactionDate = Methods.getDatePickerDate($('#<%=txtPaymentDate.ClientID %>').val());
                var d = TransactionDate.getDate();
                var m = TransactionDate.getMonth() + 1;
                var y = TransactionDate.getFullYear();
                var TransactionDateFormat = '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
                var TransactionAmount = $('#<%=hdnTotalPaymentAmount.ClientID %>').val();

                var Header = MessageType + ";" + TransactionType + ";" + TerminalID + "|";
                var VisitInfo = HealthcareID + ";" + RegistrationNo + "|";
                var PatienInfo = MRN + ";" + MedicalNo + ";" + PatientName + ";" + DateOfBirth + "|";

                var PaymentHdID = $('#<%=hdnPaymentHdID.ClientID %>').val();
                var PaymentDetailID = $('#<%=hdnPaymentDetailID.ClientID %>').val();
                var EDCMachineTransactionID = $('#<%=hdnEDCMachineTransactionID.ClientID %>').val();
                var HolderName = $('#<%=txtHolderName.ClientID %>').val();
                var txtCardNumber1 = $('#<%=txtCardNumber1.ClientID %>').val();
                var txtCardNumber4 = $('#<%=txtCardNumber4.ClientID %>').val();
                var txtPaymentAmount = $('#<%=txtPaymentAmount.ClientID %>').val();
                var txtTerminalID = $('#<%=txtTerminalID.ClientID %>').val();
                var txtBatchNo = $('#<%=txtBatchNo.ClientID %>').val();
                var txtTraceNo = $('#<%=txtTraceNo.ClientID %>').val();
                var txtReferenceNo = $('#<%=txtReferenceNo.ClientID %>').val();
                var txtApprovalCode = $('#<%=txtApprovalCode.ClientID %>').val();
                var CardNumber = txtCardNumber1 + "-XXXX-XXXX-" + txtCardNumber4;

                var filter = "EDCMachineID ='" + $('#<%=hdnEDCMachineID.ClientID %>').val() + "'";
                Methods.getObject('GetvEDCMachineList', filter, function (result) {

                    if (result != null) {
                        $('#<%=hdnBridgingEdcCode.ClientID %>').val(result.BridgingCode);
                        //// $('#<%=hdnEDCMachineID.ClientID %>').val(result.EDCMachineID);
                        //set edc form
                        setEdcForm(result.IsUsingECR);
                    }
                });

                var MessageType = $('#<%=hdnBridgingEdcCode.ClientID %>').val(); //// Constant.Sender.MD202; //MTI
                //PaymentID;PaymentDetailID;CardNumber;ReferenceNo;BatchNo;TraceNo;ApprovalCode;TerminalID;

                var RegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var toEdcIp = cboLocationEDC.GetValue();

                var Header = MessageType + ";" + TransactionType + ";" + TerminalID + "|";
                var VisitInfo = HealthcareID + ";" + RegistrationNo + "|";
                var PatienInfo = MRN + ";" + MedicalNo + ";" + PatientName + ";" + DateOfBirth + "|";
                var RegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var toEdcIp = cboLocationEDC.GetValue();
                var PaymentCardInfo = "|" + PaymentHdID + ";" + PaymentDetailID + ";" + EDCMachineTransactionID + ";" + CardNumber + ";" + txtReferenceNo + ";" + txtBatchNo + ";" + txtTraceNo + ";" + txtApprovalCode + ";" + txtTerminalID + "|";

                //Hide button 
                $('#btnPaymentCardInformationAdd').hide();
                $('#btnPaymentCardInformationSend').hide();
                $('#btnGetResponseEDC').hide();
                EDCService.SendVoidService(Header, VisitInfo, PatienInfo, PaymentCardInfo, TransactionDateFormat, TransactionAmount, RegistrationID, toEdcIp, function (result) {
                    if (result != null) {
                        var response = jQuery.parseJSON(result);

                        if (response.Status != 1) {
                            showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + response.Remarks + "</span>");
                        } else {
                            //                                $('#<%=hdnEdcRequestID.ClientID %>').val(response.RequestID);
                            //                                $('#btnGetResponseEDC').removeAttr('style');
                        }

                    }

                });

            }
            else {

                availableClose();
                var AvailablePopupEdc = $('#<%=hdnAvailablePopupEdc.ClientID %>').val();

                if (AvailablePopupEdc == "1") {
                    grdPayment.setButtonEditFocus($currEditedRow, 'bteCardInformation');
                    pcCardInformation.Hide();
                } else {

                    showToast("Process Failed", "Error Message : <br/><span style='color:red'>silahkan di selesaikan dahulu untuk proses ke edc</span>");
                }


            }

        }
        function SendToEdc() {

            var MessageType = $('#<%=hdnBridgingEdcCode.ClientID %>').val(); //// Constant.Sender.MD202; //MTI
            var TransactionType = "01";
            var TerminalID = "T01";
            var HealthcareID = $('#<%=hdnHealthcareID.ClientID %>').val();

            var RegistrationNo = $('#<%=hdnRegistrationNo.ClientID %>').val();
            var MRN = $('#<%=hdnMRN.ClientID %>').val();
            var MedicalNo = $('#<%=hdnMedicalNo.ClientID %>').val();
            var PatientName = $('#<%=hdnPatientName.ClientID %>').val();
            var DateOfBirth = $('#<%=hdnDateOfBirth.ClientID %>').val();
            var TransactionDate = Methods.getDatePickerDate($('#<%=txtPaymentDate.ClientID %>').val());
            var d = TransactionDate.getDate();
            var m = TransactionDate.getMonth() + 1;
            var y = TransactionDate.getFullYear();
            var TransactionDateFormat = '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
            var TransactionAmount = $('#<%=txtPaymentAmount.ClientID %>').val();
            /////////$('#<%=hdnTotalPaymentAmount.ClientID %>').val();

            var Header = MessageType + ";" + TransactionType + ";" + TerminalID + "|";
            var VisitInfo = HealthcareID + ";" + RegistrationNo + "|";
            var PatienInfo = MRN + ";" + MedicalNo + ";" + PatientName + ";" + DateOfBirth + "|";
            var RegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var toEdcIp = cboLocationEDC.GetValue();

            var PaymentHdID = "";
            var PaymentDetailID = "";
            var EDCMachineTransactionID = "";
            var CardNumber = "";
            var txtReferenceNo = "";
            var txtBatchNo = "";
            var txtTraceNo = "";
            var txtApprovalCode = "";
            var txtTerminalID = "";
            var pemegangKartu = $('#<%=txtHolderName.ClientID %>').val();

            var PaymentCardInfo = "|" + PaymentHdID + ";" + PaymentDetailID + ";" + EDCMachineTransactionID + ";" + CardNumber + ";" + txtReferenceNo + ";" + txtBatchNo + ";" + txtTraceNo + ";" + txtApprovalCode + ";" + txtTerminalID + ";" + pemegangKartu + "|";

            EDCService.SendService(Header, VisitInfo, PatienInfo, PaymentCardInfo, TransactionDateFormat, TransactionAmount, RegistrationID, toEdcIp, function (result) {
                if (result != null) {
                    var response = jQuery.parseJSON(result);

                    if (response.Status != 1) {
                        showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + response.Remarks + "</span>");
                    } else {
                        $('#<%=hdnEdcRequestID.ClientID %>').val(response.RequestID);
                        $('#btnGetResponseEDC').removeAttr('style');
                        $('#btnPaymentCardInformationSend').hide();

                    }

                }

            });
        }
        function availableClose() {
            var result = true;
            $('#<%=hdnAvailablePopupEdc.ClientID %>').val("1");
            var MethodePembayaran = $('#<%=hdnLastSelectMethodePembayaran.ClientID %>').val();
            var IsBridgingEdc = $('#<%=hdnIsBridgingEdc.ClientID %>').val();
            var id = $('#<%=hdnEDCMachineID.ClientID %>').val();
            if (IsBridgingEdc == "1") {
                if (MethodePembayaran == "X035^003" || MethodePembayaran == "X035^002") { //sementara debit
                    var filter = "EDCMachineID = " + id + " AND IsDeleted=0";
                    Methods.getObject('GetvEDCMachineList', filter, function (result) {
                        if (result != null) {
                            var IsEcr = result.IsUsingECR;
                            if (IsEcr == true) {
                                var filterTransaction = "RegistrationID= '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' and IsFinish=0 ";
                                Methods.getObject('GetEDCMachineTransactionList', filterTransaction, function (result) {
                                    if (result != null) {
                                        $('#<%=hdnAvailablePopupEdc.ClientID %>').val("0");
                                    }
                                });
                            }

                        }
                    });
                }
            }

        }

        function getResponseEdc() {
            var RequestID = $('#<%=hdnEdcRequestID.ClientID %>').val();
            //Get Data
            var filterExpression = "ID = '" + RequestID + "'";

            Methods.getObject('GetEDCMachineTransactionList', filterExpression, function (result) {
                if (result != null) {
                    if (result.ResponseText != null) {
                        var edc = jQuery.parseJSON(result.ResponseText);
                        if (edc.ResponseCode == "0") {
                            var CardNumber = edc.PAN.trim();
                            var ExpiryDate = edc.ExpiryDate;
                            var BatchNumber = edc.BatchNumber;
                            var ReffNumber = edc.ReffNumber;
                            var InvoiceNo = edc.InvoiceNumber;
                            var TraceNumber = edc.TraceNumber;
                            var ApprovalCode = edc.ApprovalCode;
                            var TerminalID = edc.TerminalID;
                            var TotalAmount = edc.TotalAmount;

                            var digit1 = CardNumber.substring(0, 3);
                            var lenghtCardNumber = CardNumber.length;
                            var subCardNumber = lenghtCardNumber - 4;
                            var digit4 = CardNumber.substring(subCardNumber, lenghtCardNumber);

                            var digitDepan = CardNumber.substring(0, 4);
                            var digitBelakang = CardNumber.slice(-4);

                            var namaPemegangKartu = edc.CardHolderName;

                            if (namaPemegangKartu != '' || namaPemegangKartu != null) {
                                $('#<%=txtHolderName.ClientID %>').val(namaPemegangKartu);
                            }

                            $('#<%=txtTraceNo.ClientID %>').val(TraceNumber);
                            $('#<%=txtBatchNo.ClientID %>').val(BatchNumber);
                            $('#<%=txtCardNumber1.ClientID %>').val(digitDepan);
                            $('#<%=txtCardNumber4.ClientID %>').val(digitBelakang);
                            $('#<%=txtReferenceNo.ClientID %>').val(ReffNumber);
                            $('#<%=txtApprovalCode.ClientID %>').val(ApprovalCode)
                            $('#<%=txtTerminalID.ClientID %>').val(TerminalID)
                            $('#<%=txtPaymentAmount.ClientID %>').val(TotalAmount)

                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardType', cboCardType.GetValue());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardNo1', $('#<%=txtCardNumber1.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardNo4', $('#<%=txtCardNumber4.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnHolderName', $('#<%=txtHolderName.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnExpiredDateMonth', cboCardDateMonth.GetValue());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnExpiredDateYear', cboCardDateYear.GetValue());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardProvider', cboCardProvider.GetValue());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnBatchNo', $('#<%=txtBatchNo.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnTraceNo', $('#<%=txtTraceNo.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnReferenceNo', $('#<%=txtReferenceNo.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnApprovalCode', $('#<%=txtApprovalCode.ClientID %>').val());
                            ////// grdPayment.setTextBoxProperties($currEditedRow, 'txtReferenceNo', { "value": $('#<%=txtReferenceNo.ClientID %>').val() });
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnTerminalID', $('#<%=txtTerminalID.ClientID %>').val());
                            grdPayment.setCellHiddenValue($currEditedRow, 'hdnPaymentAmountInCard', $('#<%=txtPaymentAmount.ClientID %>').val());
                            grdPayment.setTextBoxProperties($currEditedRow, 'txtReferenceNo', { "value": $('#<%=txtReferenceNo.ClientID %>').val() });

                            //Payment Amount from Popup
                            grdPayment.setTextBoxProperties($currEditedRow, 'txtPayment', { "value": $('#<%=txtPaymentAmount.ClientID %>').val() });

                            var cardInformation = $('#<%=txtCardNumber1.ClientID %>').val() + '-XXXX-XXXX-' + $('#<%=txtCardNumber4.ClientID %>').val();
                            grdPayment.setButtonEditProperties($currEditedRow, 'bteCardInformation', { value: cardInformation });
                            grdPayment.setButtonEditFocus($currEditedRow, 'bteCardInformation');
                            getCardFee($currEditedRow);
                            $('#<%=hdnInlineEditingData.ClientID %>').val(grdPayment.getTableData());
                            onCustomButtonClick('save');
                            pcCardInformation.Hide();
                        } else {

                            showToast("Process Failed", "Error Message : <br/><span style='color:red'>Data EDC Invalid.</span>");
                        }

                    } else {

                        showToast("Process Failed", "Error Message : <br/><span style='color:red'>Belum ada data response.</span>");
                    }

                }
                else {
                    showToast("Process Failed", "Error Message : <br/><span style='color:red'>Data EDC belum masuk.</span>");
                }
            });

        }
        //#endregion

        function onAfterCustomClickSuccess(type, paramUrl) {
            var result = paramUrl.split('|');
            if (result[0] == "edc") {
                if (result[1] != "") {
                    $('#<%=txtPaymentNo.ClientID %>').val(result[1]);
                    onLoadObject(result[1]);
                    pcCardInformation.Hide();
                    displayMessageBox('Success', "Pembayaran berhasil dilakukan");
                } else {
                    showToast("Process Failed", "Error Message : <br/><span style='color:red'>Pembayaran gagal diproses.</span>");
                }

            }
            pcCardInformation.Hide();
        }

        function onBeforeSaveRecord() {
            var isSettlementAllowWithAR = $('#<%=hdnIsSettlementAllowWithARPatient.ClientID %>').val();
            $('#<%=hdnIsProcessARPatient.ClientID %>').val("0");
            var isNeedConfirmation = false;
            var isAllowSave = true;
            var errMessage = '';
            var paymentType = cboPaymentType.GetValue();
            if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.DEPOSIT_OUT) {
                if ($('#<%=hdnCashbackAmount.ClientID %>').val() == "") {
                    $('#<%=hdnCashbackAmount.ClientID %>').val("0");
                }
                if (paymentType == Constant.PaymentType.DEPOSIT_OUT) {
                    $('#<%=hdnCashbackAmount.ClientID %>').val("0");
                }
                var cashBackAmount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val());
                if (cashBackAmount < 0) {
                    if (isSettlementAllowWithAR == "1") {
                        $('#<%=hdnIsProcessARPatient.ClientID %>').val("1");
                        isNeedConfirmation = true;
                    } else {
                        errMessage = '<%=GetErrorMsgCashBackAmount() %>';
                        isAllowSave = false;
                    }
                }
                $('#<%=hdnIsProcessARPatientWithoutSettlement.ClientID %>').val("0");
            }
            else if (paymentType == Constant.PaymentType.CUSTOM) {
                var cashBackAmount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val());
                if (cashBackAmount > 0) {
                    errMessage = '<%=GetErrorMsgCashBackAmountCustom() %>';
                    isAllowSave = false;
                }
            }
            else if (paymentType == Constant.PaymentType.AR_PATIENT) {
                var totalARPatient = $('#<%=hdnBillingTotalPatient.ClientID %>').val();
                if (totalARPatient <= 0) {
                    errMessage = 'Tidak ada tagihan yang bisa di proses ke piutang pasien';
                    isAllowSave = false;
                }
                if (isSettlementAllowWithAR == "1") {
                    $('#<%=hdnIsProcessARPatientWithoutSettlement.ClientID %>').val("1");
                }
            }
            else if (paymentType == Constant.PaymentType.AR_PAYER) {
                var cashBackAmount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val());
                if (cashBackAmount > 0) {
                    errMessage = 'Jumlah Piutang yang di input melebihi tagihan';
                    isAllowSave = false;
                }
            }

            if (isAllowSave) {
                if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.CUSTOM) {
                    var isValid = grdPayment.validate();
                    if (isValid) {
                        $('#<%=hdnInlineEditingData.ClientID %>').val(grdPayment.getTableData());
                        if (isNeedConfirmation) {
                            if (confirm("Nilai pelunasan masih kurang dari tagihan pasien. Lanjutkan proses? (Jika OK maka akan terbentuk Pelunasan & Piutang Pasien)")) {
                                return true;
                            } else {
                                return false;
                            }
                        } else {
                            return true;
                        }
                    } else {
                        return false;
                    }
                }
                else if (paymentType == Constant.PaymentType.AR_PAYER) {
                    var isValid = grdPaymentARPayer.validate();
                    if (isValid) {
                        $('#<%=hdnInlineEditingPayerData.ClientID %>').val(grdPaymentARPayer.getTableData());
                        return true;
                    } else {
                        return false;
                    }
                }
                else if (paymentType == Constant.PaymentType.DOWN_PAYMENT || paymentType == Constant.PaymentType.DEPOSIT_IN || paymentType == Constant.PaymentType.DEPOSIT_OUT) {
                    grdPayment.validate();
                    $('#<%=hdnInlineEditingData.ClientID %>').val(grdPayment.getTableData());

                    return true;
                }
                else {
                    return true;
                }
            }
            else {
                showToast('Warning', errMessage);
                return false;
            }
        }

        function onLoad() {
            setCustomToolbarVisibility();

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            $('#btnPaymentCardInformationAdd').click(function (evt) {
                if (IsValid(evt, 'fsCardInformation', 'vgCardInformation'))
                    closePcCardInformation('save');
            });

            $('#btnPaymentCardInformationSend').click(function (evt) {
                if (IsValid(evt, 'fsCardInformation', 'vgCardInformation')) {
                    ////////                    var cashBackAmount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val());
                    ////////                    if (cashBackAmount < 0) {
                    ////////                        if (confirm("Nilai pembayaran EDC masih kurang dari tagihan pasien. Lanjutkan proses? (Jika OK maka akan terbentuk Pelunasan & Piutang Pasien)")) {
                    ////////                            return true;
                    ////////                        } else {
                    ////////                            return false;
                    ////////                        }
                    ////////                    }
                    ////////                    else {
                    ////////                        alert('Send to EDC');
                    ////////                    }
                    /////////////////closePcCardInformation('save');
                }

            });

            //#region Payment No
            $('#lblPaymentNo.lblLink').click(function () {
                var filterExpression = 'RegistrationID = ' + $('#<%=hdnRegistrationID.ClientID %>').val();
                openSearchDialog('patientpaymenthd', filterExpression, function (value) {
                    $('#<%=txtPaymentNo.ClientID %>').val(value);
                    onTxtPaymentNoChanged(value);
                });
            });

            $('#<%=txtPaymentNo.ClientID %>').change(function () {
                onTxtPaymentNoChanged($(this).val());
            });

            function onTxtPaymentNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            if (getIsAdd()) {
                var isAllowBackDatePayment = $('#<%=hdnIsAllowBackDatePayment.ClientID %>').val();
                if (isAllowBackDatePayment != '0') {
                    setDatePicker('<%=txtPaymentDate.ClientID %>');
                }

                $('.chkIsSelected input').change(function () {
                    $('.chkSelectAll input').prop('checked', false);
                    setDdeBillingNoText();
                    setRightPanelButtonPaymentgateway();
                });

                $('.chkSelectAll input').change(function () {
                    var isChecked = $(this).is(":checked");
                    $('.chkIsSelected').each(function () {
                        $(this).find('input').prop('checked', isChecked);
                    });
                    setDdeBillingNoText();
                    setRightPanelButtonPaymentgateway();
                });

                if (cboPaymentMethod != null && cboEDCMachine != null && cboBank != null)
                    init();

                if (cboBusinessPartner != null)
                    init2();

                $('#divContainerGrdDetailAdd').show();
                $('#divContainerGrdDetailEdit').hide();
                $('#divContainerGrdDetailPayerEdit').hide();
                //sini
                //$('#divContainerGrdDetailARPayer').hide();
                $('#divContainerGrdDetailAR').hide();
                $('#divDdeBillingNo').show();
                $('#divTxtBillingNo').hide();
                $('#btnPaymentCardInformationAdd').show();
                $('#btnPaymentCardInformationCancel').show();
                $('#btnPaymentCardInformationClose').hide();

                showLoadingPanel();

                setTimeout(function () {
                    onCboPaymentTypeValueChanged();
                    hideLoadingPanel();
                }, 1500);
            }
            else {
                $('#divContainerGrdDetailAdd').hide();
                $('#divContainerGrdDetailPayerEdit').hide();
                $('#divContainerGrdDetailEdit').show();
                $('#divContainerGrdDetailARPayer').hide();
                var paymentType = cboPaymentType.GetValue();
                if (paymentType == Constant.PaymentType.AR_PAYER) {
                    $('#divContainerGrdDetailPayerEdit').show();
                    $('#divContainerGrdDetailEdit').hide();
                }
                $('#divContainerGrdDetailAR').hide();
                $('#divDdeBillingNo').hide();
                $('#divTxtBillingNo').show();
                $('#btnPaymentCardInformationAdd').hide();
                $('#btnPaymentCardInformationCancel').hide();
                $('#btnPaymentCardInformationClose').show();

                $('.lnkCardNumber').click(function () {
                    $td = $(this).parent();
                    cboCardType.SetValue($td.find('.hdnGCCardType').val());
                    $('#<%=txtCardNumber1.ClientID %>').val($td.find('.hdnCardNumber1').val());
                    $('#<%=txtCardNumber4.ClientID %>').val($td.find('.hdnCardNumber4').val());
                    $('#<%=txtHolderName.ClientID %>').val($td.find('.hdnCardHolderName').val());
                    $('#<%=txtBatchNo.ClientID %>').val($td.find('.hdnBatchNoEdit').val());
                    $('#<%=txtTraceNo.ClientID %>').val($td.find('.hdnTraceNoEdit').val());
                    $('#<%=txtReferenceNo.ClientID %>').val($td.find('.hdnReferenceNoEdit').val());
                    $('#<%=txtApprovalCode.ClientID %>').val($td.find('.hdnApprovalCodeEdit').val());
                    $('#<%=txtPaymentAmount.ClientID %>').val($td.find('.hdnPaymentAmountEdit').val());
                    $('#<%=hdnPaymentDetailID.ClientID %>').val($td.find('.hdnPaymentDtID').val());
                    $('#<%=hdnEDCMachineTransactionID.ClientID %>').val($td.find('.hdnEDCMachineTransactionID').val());
                    $('#<%=hdnEDCMachineTransactionIsFinish.ClientID %>').val($td.find('.hdnEDCMachineTransactionIsFinish').val());
                    $('#<%=hdnEDCMachineID.ClientID %>').val($td.find('.hdnEDCMachineID').val());


                    $('#<%=hdnEdcIsVoid.ClientID %>').val($td.find('.hdnEdcIsVoid').val());

                    /////alert($('#<%=hdnEDCMachineTransactionIsFinish.ClientID %>').val());
                    var cardValidThru = $td.find('.hdnCardValidThru').val().split('/');
                    var expiredDateMonth = parseInt(cardValidThru[0]);
                    var expiredDateYear = 2000 + parseInt(cardValidThru[1]);
                    if (cardValidThru != '') {
                        cboCardDateMonth.SetValue(expiredDateMonth);
                        cboCardDateYear.SetValue(expiredDateYear);
                    }
                    else {
                        cboCardDateMonth.SetValue('');
                        cboCardDateYear.SetValue('');
                    }
                    cboCardProvider.SetValue($td.find('.hdnGCCardProvider').val());

                    var paymentID = $('#<%=hdnPaymentHdID.ClientID %>').val();
                    if (paymentID != '') {
                        $('#btnPaymentCardInformationSend').attr('style', 'display:none');
                        $('#btnGetResponseEDC').attr('style', 'display:none');
                        //                        cboCardDateMonth.SetValue("");
                        //                        cboCardDateYear.SetValue("");
                        $('#<%=txtHolderName.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtCardNumber1.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtCardNumber4.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtPaymentAmount.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtTerminalID.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtBatchNo.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtTraceNo.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtReferenceNo.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtApprovalCode.ClientID %>').attr('readonly', 'readonly');

                        //is bridging 
                        var IsBridgingEdc = $('#<%=hdnIsBridgingEdc.ClientID %>').val();
                        if (IsBridgingEdc == "1") {
                            var EDCMachineTransactionID = $('#<%=hdnEDCMachineTransactionID.ClientID %>').val();
                            var IsProcessFinish = $('#<%=hdnEDCMachineTransactionIsFinish.ClientID %>').val();
                            var IsProcessVoid = $('#<%=hdnEdcIsVoid.ClientID %>').val();

                            if (EDCMachineTransactionID != 0 || EDCMachineTransactionID > 0) {
                                if (IsProcessFinish == "True" && IsProcessVoid == "False") {
                                    $('#btnVoidCardInformationSend').show();
                                    $('#trLocationEDC').show();
                                } else {
                                    $('#btnVoidCardInformationSend').hide();
                                    $('#trLocationEDC').hide();
                                }
                            } else {
                                $('#btnVoidCardInformationSend').hide();
                                $('#trLocationEDC').hide();
                            }
                        } else {
                            $('#btnVoidCardInformationSend').hide();
                            $('#trLocationEDC').hide();
                        }


                    }
                    else {
                        $('#btnPaymentCardInformationSend').removeAttr('style');
                        $('#btnGetResponseEDC').removeAttr('style');
                    }

                    pcCardInformation.Show();
                });
            }
            setRightPanelButtonEnabled();
        }

        function validatePaymentTypeDate() {
            var isAllowBackDatePaymentPersonalAR = $('#<%=hdnIsAllowBackDatePaymentPersonalAR.ClientID %>').val();

            var paymentType = cboPaymentType.GetValue();
            if (paymentType != Constant.PaymentType.DOWN_PAYMENT && paymentType != Constant.PaymentType.SETTLEMENT && paymentType != Constant.PaymentType.CUSTOM) {
                setDatePicker('<%=txtPaymentDate.ClientID %>');
                $('#<%=txtPaymentDate.ClientID %>').removeAttr('readonly');
                $('#<%:txtPaymentDate.ClientID %>').datepicker('enable');
                $('#<%=txtPaymentTime.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtPaymentDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtPaymentDate.ClientID %>').datepicker('disable');
                $('#<%=txtPaymentTime.ClientID %>').attr('readonly', 'readonly');

                var currentDate = new Date();
                var date = currentDate.getDate();
                var month = currentDate.getMonth() + 1;
                var year = currentDate.getFullYear();

                var hour = currentDate.getHours();
                var minutes = currentDate.getMinutes();

                if (date < 10) {
                    date = '0' + date;
                }

                if (month < 10) {
                    month = '0' + month;
                }

                if (hour < 10) {
                    hour = '0' + hour;
                }

                if (minutes < 10) {
                    minutes = '0' + minutes;
                }

                var date = date + '-' + month + '-' + year;
                var time = hour + ':' + minutes;

                $('#<%:txtPaymentDate.ClientID %>').val(date);
                $('#<%=txtPaymentTime.ClientID %>').val(time);
            }
        }

        function validatePaymentPersonalARTypeDate() {
            var paymentType = cboPaymentType.GetValue();
            if (paymentType == Constant.PaymentType.AR_PATIENT) {
                $('#<%=txtPaymentDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtPaymentDate.ClientID %>').datepicker('disable');
                $('#<%=txtPaymentTime.ClientID %>').attr('readonly', 'readonly');

                var currentDate = new Date();
                var date = currentDate.getDate();
                var month = currentDate.getMonth() + 1;
                var year = currentDate.getFullYear();

                var hour = currentDate.getHours();
                var minutes = currentDate.getMinutes();

                if (date < 10) {
                    date = '0' + date;
                }

                if (month < 10) {
                    month = '0' + month;
                }

                if (hour < 10) {
                    hour = '0' + hour;
                }

                if (minutes < 10) {
                    minutes = '0' + minutes;
                }

                var date = date + '-' + month + '-' + year;
                var time = hour + ':' + minutes;

                $('#<%:txtPaymentDate.ClientID %>').val(date);
                $('#<%=txtPaymentTime.ClientID %>').val(time);
            } else {
                if (paymentType != Constant.PaymentType.DOWN_PAYMENT && paymentType != Constant.PaymentType.SETTLEMENT && paymentType != Constant.PaymentType.CUSTOM) {
                }
            }
        }

        function onCboPaymentTypeValueChanged() {
            grdPayment.clearTable();
            grdPayment.addRow();
            ddeBillingNo.SetEnabled(true);

            var isAllowBackDatePayment = $('#<%=hdnIsAllowBackDatePayment.ClientID %>').val();
            var isAllowBackDatePaymentPersonalAR = $('#<%=hdnIsAllowBackDatePaymentPersonalAR.ClientID %>').val();

            var paymentType = cboPaymentType.GetValue();
            if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.CUSTOM || paymentType == Constant.PaymentType.DEPOSIT_OUT) {
                $('#tblCashback').show();
                $('.chkIsSelected input').each(function () {
                    $(this).prop('checked', true);
                });

                if (paymentType == Constant.PaymentType.SETTLEMENT && $('#<%=hdnIsAllowRoundingAmount.ClientID %>').val() == "1" && $('#<%=hdnUbahNilaiPembutalan.ClientID %>').val() == "1") {
                    $('#<%=txtPatientRoundingAmount.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=txtPatientRoundingAmount.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPatientRoundingAmount.ClientID %>').val("0");
                    $('#<%=txtPatientRoundingAmount.ClientID %>').change();
                }

                setDdeBillingNoText();
                $('#<%=trOutstandingDP.ClientID %>').removeAttr('style');
            }
            else {
                if (paymentType == Constant.PaymentType.DOWN_PAYMENT || paymentType == Constant.PaymentType.DEPOSIT_IN) {
                    $('.chkIsSelected input:checked').each(function () {
                        $(this).prop('checked', false);
                    });
                    ddeBillingNo.SetEnabled(false);
                    setDdeBillingNoText();
                }

                $('#tblCashback').hide();
                $('#<%=trOutstandingDP.ClientID %>').attr('style', 'display:none');

                $('#<%=txtPatientRoundingAmount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPatientRoundingAmount.ClientID %>').val("0");
                $('#<%=txtPatientRoundingAmount.ClientID %>').change();
            }

            if (paymentType == Constant.PaymentType.PAYMENT_RETURN || paymentType == Constant.PaymentType.AR_PATIENT || paymentType == Constant.PaymentType.AR_PAYER) {
                if (paymentType == Constant.PaymentType.AR_PAYER) {
                    $('#divContainerGrdDetailARPayer').show();
                    $('#divContainerGrdDetailAR').hide();
                    $('#divContainerGrdDetailEdit').hide();
                    $('#divContainerGrdDetailAdd').hide();

                    $('#<%=txtPatientRoundingAmount.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPatientRoundingAmount.ClientID %>').val("0");
                    $('#<%=txtPatientRoundingAmount.ClientID %>').change();

                    var defaultBPID = $('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val();
                    cboBPOptions = { "value": defaultBPID };
                    $row = $('#tblPaymentDtPayer .trTransaction').first();
                    grdPaymentARPayer.isChanged = true;
                    grdPaymentARPayer.setRowChanged($row, true);
                    grdPaymentARPayer.setComboBoxProperties($row, 'cboBusinessPartner', cboBPOptions);
                    $('#<%=hdnTotalPayerPaymentAmount.ClientID %>').val('0')
                    calculateCashbackAmountPayer();
                    var amount = $('#<%=hdnCashbackAmount.ClientID %>').val() * -1;
                    grdPayment.setTextBoxProperties($row, 'txtPayment', { "value": amount });
                    grdPaymentARPayer.setTextBoxProperties($row, 'txtLineTotal', { "value": amount });
                    calculatePayerPaymentDtTotal();
                    $('#tblCashback').show();
                }
                else if (paymentType == Constant.PaymentType.AR_PATIENT) {
                    $('#divContainerGrdDetailAR').show();
                    $('#divContainerGrdDetailARPayer').hide();
                    $('#divContainerGrdDetailEdit').hide();
                    $('#divContainerGrdDetailAdd').hide();
                    setARTotal();

                    var totalOutstandingDP = parseFloat($('#<%=hdnOutstandingDP.ClientID %>').val());
                    if (totalOutstandingDP != 0) {
                        $('#trUangMukaKeluar').show();
                        $('#<%=trOutstandingDP.ClientID %>').removeAttr('style');
                    } else {
                        $('#trUangMukaKeluar').hide();
                        $('#<%=trOutstandingDP.ClientID %>').attr('style', 'display:none');
                    }

                    $('#<%=txtPatientRoundingAmount.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPatientRoundingAmount.ClientID %>').val("0");
                    $('#<%=txtPatientRoundingAmount.ClientID %>').change();
                }
                else {
                    $('#divContainerGrdDetailAR').show();
                    $('#divContainerGrdDetailARPayer').hide();
                    $('#divContainerGrdDetailEdit').hide();
                    $('#divContainerGrdDetailAdd').hide();
                    setARTotal();

                    $('#<%=txtPatientRoundingAmount.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPatientRoundingAmount.ClientID %>').val("0");
                    $('#<%=txtPatientRoundingAmount.ClientID %>').change();
                }
            }
            else {
                if (getIsAdd()) {
                    $('#divContainerGrdDetailAdd').show();
                    $('#divContainerGrdDetailARPayer').hide();
                    $('#divContainerGrdDetailEdit').hide();
                    $('#divContainerGrdDetailAR').hide();
                }
                else {
                    $('#divContainerGrdDetailAdd').hide();
                    $('#divContainerGrdDetailARPayer').hide();
                    $('#divContainerGrdDetailEdit').show();
                    $('#divContainerGrdDetailAR').hide();
                }

                //$('#<%=txtPatientRoundingAmount.ClientID %>').attr('readonly', 'readonly');
                //$('#<%=txtPatientRoundingAmount.ClientID %>').val("0");
            }

            if (isAllowBackDatePayment == '0') {
                validatePaymentTypeDate();
            }

            if (isAllowBackDatePaymentPersonalAR == "0") {
                validatePaymentPersonalARTypeDate();
            }
        }

        function setARTotal() {
            var total = 0;
            var paymentType = cboPaymentType.GetValue();
            if (paymentType == Constant.PaymentType.PAYMENT_RETURN) {
                $('#tdARPaymentMethod').html($('#<%=hdnPaymentReturnText.ClientID %>').val());
                total = parseFloat($('#<%=txtBillingTotalPatient.ClientID %>').attr('hiddenVal')) * -1;
                total = total.formatMoney(2, '.', ',');

                $('#tdPaymentDtAR').html(total);
                $('#tdLineAmountAR').html(total);
            }
            else if (paymentType == Constant.PaymentType.AR_PATIENT) {
                $('#tdUangMukaKeluar').html($('#<%=hdnCaptionUangMukaKeluar.ClientID %>').val());
                $('#tdARPaymentMethod').html($('#<%=hdnARText.ClientID %>').val());

                total = $('#<%=txtBillingTotalPatient.ClientID %>').val();

                var totalOutstandingDP = parseFloat($('#<%=hdnOutstandingDP.ClientID %>').val());
                var billingPatient = parseFloat($('#<%=hdnBillingTotalPatient.ClientID %>').val());

                var arPatientWithoutDP = billingPatient - totalOutstandingDP;
                arPatientWithoutDP = arPatientWithoutDP.formatMoney(2, '.', ',');

                $('#<%=hdnARPatientWithoutDP.ClientID %>').val(arPatientWithoutDP);

                $('#tdNilaiUangMukaKeluar').html($('#<%=txtOutstandingDP.ClientID %>').val());
                $('#tdNilaiAkhirUangMukaKeluar').html($('#<%=txtOutstandingDP.ClientID %>').val());
                $('#tdPaymentDtAR').html(arPatientWithoutDP);
                $('#tdLineAmountAR').html(arPatientWithoutDP);
            }
            else {
                $('#tdARPaymentMethod').html($('#<%=hdnARText.ClientID %>').val());
                if (paymentType == Constant.PaymentType.AR_PATIENT)
                    total = $('#<%=txtBillingTotalPatient.ClientID %>').val();
                else if (paymentType == Constant.PaymentType.AR_PAYER)
                    total = $('#<%=txtBillingTotalPayer.ClientID %>').val();


                var totalOutstandingDP = parseFloat($('#<%=hdnOutstandingDP.ClientID %>').val());
                var billingPatient = parseFloat($('#<%=hdnBillingTotalPatient.ClientID %>').val());

                var arPatientWithoutDP = billingPatient - totalOutstandingDP;
                arPatientWithoutDP = arPatientWithoutDP.formatMoney(2, '.', ',');

                $('#<%=hdnARPatientWithoutDP.ClientID %>').val(arPatientWithoutDP);

                $('#tdPaymentDtAR').html(total);
                $('#tdLineAmountAR').html(total);
            }
            $('#tdTotalAR').html(total);
            $('#tdLineTotalAR').html(total);
        }

        $('#<%:txtPatientRoundingAmount.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');

            var billingTotalPatient = 0
            var patientAmount = parseFloat($('#<%=txtBillingOriginalPatient.ClientID %>').attr('hiddenVal'));
            var patientRoundingAmount = parseFloat($('#<%=txtPatientRoundingAmount.ClientID %>').attr('hiddenVal'));

            billingTotalPatient = patientAmount + patientRoundingAmount;
            $('#<%=txtBillingTotalPatient.ClientID %>').val(billingTotalPatient).trigger('changeValue');
            $('#<%=hdnBillingTotalPatient.ClientID %>').val(billingTotalPatient);
            calculateCashbackAmount();
        });

        $('#<%:txtPayerRoundingAmount.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');

            var billingTotalPayer = 0
            var payerAmount = parseFloat($('#<%=txtBillingTotalPayer.ClientID %>').attr('hiddenVal'));
            var payerRoundingAmount = parseFloat($('#<%=txtPayerRoundingAmount.ClientID %>').attr('hiddenVal'));

            billingTotalPayer = payerAmount + payerRoundingAmount;
            $('#<%=txtBillingTotalPayer.ClientID %>').val(billingTotalPayer).trigger('changeValue');
            calculateCashbackAmountPayer();
        });

        function setDdeBillingNoText() {
            var payerAmount = 0;
            var patientAmount = 0;
            var payerRoundingSumAmount = 0;
            var patientRoundingSumAmount = 0;
            var lineAmount = 0;
            var billingNo = '';
            var lstBillingID = '';

            var regID = $('#<%=hdnRegistrationID.ClientID %>').val();

            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                if (billingNo != '') {
                    billingNo += ', ';
                    lstBillingID += ',';
                }
                lstBillingID += $tr.find('.hdnKeyField').val();
                billingNo += $.trim($tr.find('.lnkTransactionNo').html());
                payerAmount += parseFloat($tr.find('.hdnPayerAmount').val());
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
            });
            ddeBillingNo.SetText(billingNo);
            $('#<%=hdnListBillingID.ClientID %>').val(lstBillingID);

            var paymentType = cboPaymentType.GetValue();
            var isRounding = $('#<%=hdnIsAllowRoundingAmount.ClientID %>').val();
            var pembulatanKeAtas = $('#<%=hdnPembulatanKeAtas.ClientID %>').val(); // 1 = atas

            if (isRounding == "1") {
                if (paymentType == Constant.PaymentType.SETTLEMENT) {
                    $('#<%=txtBillingOriginalPatient.ClientID %>').val(patientAmount).trigger('changeValue');
                    $('#<%=txtBillingOriginalPayer.ClientID %>').val(payerAmount).trigger('changeValue');

                    var setRounding = parseFloat($('#<%=hdnNilaiPembulatan.ClientID %>').val());
                    var diffRouding = parseFloat(patientAmount % setRounding);

                    if (pembulatanKeAtas == "1") {
                        var patientRoundingAmount = setRounding - diffRouding;
                    } else {
                        var patientRoundingAmount = diffRouding * -1;
                    }

                    if ((patientRoundingAmount % setRounding) == 0) {
                        patientRoundingAmount = 0;
                    }

                    var payerRoundingAmount = 0;

                    $('#<%=txtPatientRoundingAmount.ClientID %>').val(patientRoundingAmount).trigger('changeValue');
                    $('#<%=txtPayerRoundingAmount.ClientID %>').val(payerRoundingAmount).trigger('changeValue');

                    $('#<%=txtBillingTotal.ClientID %>').val(lineAmount).trigger('changeValue');
                    $('#<%=txtBillingTotalPatient.ClientID %>').val(patientAmount + patientRoundingAmount).trigger('changeValue');
                    $('#<%=txtBillingTotalPayer.ClientID %>').val(payerAmount + payerRoundingAmount).trigger('changeValue');

                    $('#<%=hdnBillingTotalPatient.ClientID %>').val(patientAmount + patientRoundingAmount);
                    $('#<%=hdnBillingTotalPayer.ClientID %>').val(payerAmount + payerRoundingAmount);
                } else {
                    $('#<%=txtBillingOriginalPatient.ClientID %>').val(patientAmount).trigger('changeValue');
                    $('#<%=txtBillingOriginalPayer.ClientID %>').val(payerAmount).trigger('changeValue');

                    var patientRoundingAmount = 0;
                    var payerRoundingAmount = 0;

                    $('#<%=txtPatientRoundingAmount.ClientID %>').val(patientRoundingAmount).trigger('changeValue');
                    $('#<%=txtPayerRoundingAmount.ClientID %>').val(payerRoundingAmount).trigger('changeValue');

                    $('#<%=txtBillingTotal.ClientID %>').val(lineAmount).trigger('changeValue');
                    $('#<%=txtBillingTotalPatient.ClientID %>').val(patientAmount).trigger('changeValue');
                    $('#<%=txtBillingTotalPayer.ClientID %>').val(payerAmount).trigger('changeValue');

                    $('#<%=hdnBillingTotalPatient.ClientID %>').val(patientAmount);
                    $('#<%=hdnBillingTotalPayer.ClientID %>').val(payerAmount);
                }
            } else {
                $('#<%=txtBillingOriginalPatient.ClientID %>').val(patientAmount).trigger('changeValue');
                $('#<%=txtBillingOriginalPayer.ClientID %>').val(payerAmount).trigger('changeValue');

                var patientRoundingAmount = 0;
                var payerRoundingAmount = 0;

                $('#<%=txtPatientRoundingAmount.ClientID %>').val(patientRoundingAmount).trigger('changeValue');
                $('#<%=txtPayerRoundingAmount.ClientID %>').val(payerRoundingAmount).trigger('changeValue');

                $('#<%=txtBillingTotal.ClientID %>').val(lineAmount).trigger('changeValue');
                $('#<%=txtBillingTotalPatient.ClientID %>').val(patientAmount).trigger('changeValue');
                $('#<%=txtBillingTotalPayer.ClientID %>').val(payerAmount).trigger('changeValue');

                $('#<%=hdnBillingTotalPatient.ClientID %>').val(patientAmount);
                $('#<%=hdnBillingTotalPayer.ClientID %>').val(payerAmount);
            }

            if (paymentType == Constant.PaymentType.AR_PATIENT || paymentType == Constant.PaymentType.AR_PAYER) {
                setARTotal();
            }
            else if (paymentType == Constant.PaymentType.SETTLEMENT || paymentType == Constant.PaymentType.CUSTOM) {
                calculatePaymentDtTotal();
                grdPayment.clearTable();
                grdPayment.addRow();
                var totalBill = parseFloat($('#<%=hdnBillingTotalPatient.ClientID %>').val());
                var totalOutstandingDP = parseFloat($('#<%=hdnOutstandingDP.ClientID %>').val());
                if (totalOutstandingDP > 0) {
                    grdPayment.addRow();
                    $row = grdPayment.getRow(0);

                    var tempCboDPOut = $('#<%=hdnCboDPOut.ClientID %>').val().split('|');
                    cboPaymentMethod.AddItem(tempCboDPOut[1], tempCboDPOut[0]);
                    grdPayment.setComboBoxProperties($row, 'cboPaymentMethod', { "value": 'X035^006' });

                    var filterPM = "IsDeleted = 0 AND IsActive = 1 AND ParentID = 'X035' AND StandardCodeID NOT IN ('X035^005','X035^006','X035^007')";
                    var paymentMethodLength = 0;
                    Methods.getListObject("GetStandardCodeList", filterPM, function (dataPM) {
                        paymentMethodLength = dataPM.length;
                    });
                    cboPaymentMethod.RemoveItem(paymentMethodLength);

                    var total = 0;
                    total = totalOutstandingDP;
                    grdPayment.setTextBoxProperties($row, 'txtPayment', { "value": total });
                    grdPayment.setTextBoxProperties($row, 'txtLineTotal', { "value": total });
                    grdPayment.setCellHiddenValue($row, 'hdnCardFee ', '0');
                    grdPayment.setTextBoxProperties($row, 'txtFee', { "value": 0 });
                    grdPayment.setRowEnabled($row, false);
                    grdPayment.setRowChanged($row, true);
                    if (paymentType == Constant.PaymentType.SETTLEMENT && totalOutstandingDP > totalBill) {
                        grdPayment.addRow();
                        $row = grdPayment.getRow(1);
                        grdPayment.setComboBoxProperties($row, 'cboPaymentMethod', { "value": 'X035^001' });
                        var total = totalBill - totalOutstandingDP;
                        grdPayment.setTextBoxProperties($row, 'txtPayment', { "value": total });
                        grdPayment.setTextBoxProperties($row, 'txtLineTotal', { "value": total });
                        grdPayment.setCellHiddenValue($row, 'hdnCardFee ', '0');
                        grdPayment.setTextBoxProperties($row, 'txtFee', { "value": 0 });
                        grdPayment.setRowEnabled($row, false);
                        grdPayment.setRowChanged($row, true);
                        grdPayment.setComboBoxProperties($row, 'cboPaymentMethod', { "isEnabled": true });
                    }
                }
                calculatePaymentDtTotal();
            }
        }

        function openPaymentCreateAccountReceivable() {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val() + '|' + $('#<%=hdnDepartmentID.ClientID %>').val() + '|' + $('#<%=hdnListBillingID.ClientID %>').val() + '|' + cboCashierGroup.GetValue() + '|' + cboShift.GetValue() + '|' + $('#<%=txtPaymentDate.ClientID %>').val() + '|' + $('#<%=txtPaymentTime.ClientID %>').val() + '|' + $('#<%=hdnTanggalPiutangPribadi.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/Payment/PaymentAccountReceivableCtl.ascx");
            openUserControlPopup(url, id, 'Sisa Tagihan', 700, 150);
        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnTransactionStatus.ClientID %>').val() != Constant.TransactionStatus.VOID) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/Payment/PaymentVoidCtl.ascx');
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var paymentID = $('#<%=hdnPaymentHdID.ClientID %>').val();
                var outstandingDP = $('#<%=hdnOutstandingDP.ClientID %>').val();
                var id = registrationID + '|' + paymentID + '|' + outstandingDP;
                openUserControlPopup(url, id, 'Void Payment', 400, 230);
            }
        });

        function onCbpEDCProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            var retval = s.cpRetval.split('|');
            if (param[1] == '0') {
                showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcCardInformation.Hide();
            }
        }

        function onCboEDCMachineValueChanged(s) {
            /////hdnIsBridgingEdc
            configEDC();
            /*var id = cboEDCMachine.GetValue();
            if (id == 2) {
            $('#btnPaymentCardInformationSend').removeAttr('style');
            $('#btnGetResponseEDC').removeAttr('style');
            }
            else {
            $('#btnPaymentCardInformationSend').attr('style', 'display:none');
            $('#btnGetResponseEDC').attr('style', 'display:none');
            }*/
        }
        function setEdcForm(isUsingEcr) {
            var SelectMethodePembayaran = $('#<%=hdnLastSelectMethodePembayaran.ClientID %>').val();
            if ((SelectMethodePembayaran == "X035^003" || SelectMethodePembayaran == "X035^002") && isUsingEcr == true) { //debit
                //button
                $('#btnPaymentCardInformationAdd').attr('style', 'display:none');
                $('#trLocationEDC').show();
                $('#btnPaymentCardInformationSend').removeAttr('style');
                $('#btnGetResponseEDC').removeAttr('style');


                //form edc mandatory

                $('#<%=lblTipeKartu.ClientID %>').addClass("lblMandatory");
                $('#<%=cboCardType.ClientID %>').addClass("required");
                $('#<%=lblBankPenerbit.ClientID %>').addClass("lblMandatory");
                $('#<%=cboCardProvider.ClientID %>').addClass("required");
                $('#<%=lblMasaBerlaku.ClientID %>').addClass("lblMandatory");
                $('#<%=cboCardDateYear.ClientID %>').addClass("required");
                $('#<%=cboCardDateMonth.ClientID %>').addClass("required");

                $('#<%=lblNoKartu.ClientID %>').removeClass("lblMandatory");
                $('#<%=txtCardNumber1.ClientID %>').removeClass("required");
                $('#<%=txtCardNumber4.ClientID %>').removeClass("required");
                $('#<%=lblNoBatch.ClientID %>').removeClass("lblMandatory");
                $('#<%=txtBatchNo.ClientID %>').removeClass("required");
                $('#<%=lblReferenceNo.ClientID %>').removeClass("lblMandatory");
                $('#<%=txtReferenceNo.ClientID %>').removeClass("required");
                $('#<%=lblApprovalCode.ClientID %>').removeClass("lblMandatory");
                $('#<%=txtApprovalCode.ClientID %>').removeClass("required");
                $('#lblLocationEdc').addClass("lblMandatory");
                $('#<%=cboLocationEDC.ClientID %>').addClass("required");
                //hide button close
                $('#ctl00_ctl00_ctl00_plhMPBase_plhMPPatientPage_cbpMPEntryContent_plhEntry_pcCardInformation_HCB-1').hide();
            }
            else {

                //button 
                $('#btnPaymentCardInformationAdd').removeAttr('style');
                $('#btnPaymentCardInformationSend').attr('style', 'display:none');
                $('#btnGetResponseEDC').attr('style', 'display:none');
                $('#trLocationEDC').hide();

                //form edc mandatory
                $('#<%=lblNoKartu.ClientID %>').addClass("lblMandatory");
                $('#<%=txtCardNumber1.ClientID %>').addClass("required");
                $('#<%=txtCardNumber4.ClientID %>').addClass("required");
                $('#<%=lblNoBatch.ClientID %>').addClass("lblMandatory");
                $('#<%=txtBatchNo.ClientID %>').addClass("required");
                $('#<%=lblReferenceNo.ClientID %>').addClass("lblMandatory");
                $('#<%=txtReferenceNo.ClientID %>').addClass("required");
                $('#<%=lblApprovalCode.ClientID %>').addClass("lblMandatory");
                $('#<%=txtApprovalCode.ClientID %>').addClass("required");
                $('#lblLocationEdc').removeClass("lblMandatory");
                $('#<%=cboLocationEDC.ClientID %>').removeClass("required");
                //hide button close
                $('#ctl00_ctl00_ctl00_plhMPBase_plhMPPatientPage_cbpMPEntryContent_plhEntry_pcCardInformation_HCB-1').show();
            }
        }
        function setConfigEdcForm(MethodePembayaran) {
            $('#<%=hdnLastSelectMethodePembayaran.ClientID %>').val(MethodePembayaran);
        }
        function setConfigEDC() {
            var IsBridgingEdc = $('#<%=hdnIsBridgingEdc.ClientID %>').val();
            if (IsBridgingEdc == "1") {
                $('#btnPaymentCardInformationSend').show();
                $('#trLocationEDC').show();
                $('#btnPaymentCardInformationAdd').hide();
            } else {
                $('#btnPaymentCardInformationSend').hide();
                $('#trLocationEDC').hide();
                $('#btnPaymentCardInformationAdd').show();
            }
        }
        function configEDC() {
            var IsBridgingEdc = $('#<%=hdnIsBridgingEdc.ClientID %>').val();
            var MethodePembayaran = $('#<%=hdnLastSelectMethodePembayaran.ClientID %>').val();
            var id = cboEDCMachine.GetValue();

            if (IsBridgingEdc == "1") {
                var filter = "EDCMachineID = " + id + " AND IsDeleted=0";
                Methods.getObject('GetvEDCMachineList', filter, function (result) {

                    if (result != null) {
                        $('#<%=hdnBridgingEdcCode.ClientID %>').val(result.BridgingCode);
                        $('#<%=hdnEDCMachineID.ClientID %>').val(result.EDCMachineID);

                        //set edc form
                        setEdcForm(result.IsUsingECR);
                    }
                });
            } else {
                $('#<%=hdnBridgingEdcCode.ClientID %>').val("");
                $('#<%=hdnEDCMachineID.ClientID %>').val("");
            }
        }
        function SnackbarNotify(message) {

            var x = document.getElementById("snackbar");
            x.innerHTML = message;
            x.className = "show";
            $('#snackbar').show();
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 5000);
        }
    </script>
    <style type="text/css">
        #tblPaymentDtEdit td
        {
            padding: 3px;
        }
        
        #snackbar
        {
            visibility: hidden;
            min-width: 250px;
            background-color: #333;
            color: #fff;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            right: 30px;
            font-size: 17px;
        }
        
        #snackbar.show
        {
            visibility: visible;
            -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
            animation: fadein 0.5s, fadeout 0.5s 2.5s;
        }
    </style>
    <input type="hidden" value="" id="hdnCaptionDownPayment" runat="server" />
    <input type="hidden" value="" id="hdnIsGrouperAmountClaimDefaultZero" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowRoundingAmount" runat="server" />
    <input type="hidden" value="" id="hdnNilaiPembulatan" runat="server" />
    <input type="hidden" value="" id="hdnUbahNilaiPembutalan" runat="server" />
    <input type="hidden" value="" id="hdnPembulatanKeAtas" runat="server" />
    <input type="hidden" value="" id="hdnTanggalPiutangPribadi" runat="server" />
    <input type="hidden" value="" id="hdnCardFeeDitanggungPasien" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnEdcRequestID" runat="server" />
    <input type="hidden" value="" id="hdnListBillingID" runat="server" />
    <input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
    <input type="hidden" value="" id="hdnInlineEditingPayerData" runat="server" />
    <input type="hidden" value="" id="hdnCreditCardFeeFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnTotalPaymentAmount" runat="server" />
    <input type="hidden" value="0" id="hdnTotalPayerPaymentAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnPaymentHdID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultBusinessPartnerID" runat="server" />
    <input type="hidden" value="0" id="hdnCashbackAmount" runat="server" />
    <input type="hidden" value="" id="hdnBillingTotalPatient" runat="server" />
    <input type="hidden" value="" id="hdnBillingTotalPayer" runat="server" />
    <input type="hidden" value="" id="hdnPaymentAllocation" runat="server" />
    <input type="hidden" value="" id="hdnListShift" runat="server" />
    <input type="hidden" value="" id="hdnDefaultShift" runat="server" />
    <input type="hidden" value="" id="hdnCboDPOut" runat="server" />
    <input type="hidden" value="" id="hdnARText" runat="server" />
    <input type="hidden" value="" id="hdnCaptionUangMukaKeluar" runat="server" />
    <input type="hidden" value="" id="hdnARPatientWithoutDP" runat="server" />
    <input type="hidden" value="" id="hdnPaymentReturnText" runat="server" />
    <input type="hidden" value="" id="hdnPatientGender" runat="server" />
    <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowBackDatePayment" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowBackDatePaymentPersonalAR" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="0" id="hdnIsProcessARPatient" runat="server" />
    <input type="hidden" value="0" id="hdnIsProcessARPatientWithoutSettlement" runat="server" />
    <input type="hidden" value="0" id="hdnIsSettlementAllowWithARPatient" runat="server" />
    <input type="hidden" value="0" id="hdnDepositBalanceEnd" runat="server" />
    <input type="hidden" value="" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" />
    <input type="hidden" value="" id="hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim"
        runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="6000" id="hdnPort" runat="server" />
    <%--EDC--%>
    <input type="hidden" value="" id="hdnHealthcareID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnDateOfBirth" runat="server" />
    <input type="hidden" value="" id="hdnTransactionDate" runat="server" />
    <input type="hidden" value="" id="hdnPaymentNo" runat="server" />
    <input type="hidden" value="" id="hdnTransactionAmount" runat="server" />
    <input type="hidden" value="" id="hdnLastSelectMethodePembayaran" runat="server" />
    <input type="hidden" value="" id="hdnAvailablePopupEdc" runat="server" />
    <div style="height: 450px; overflow-y: auto;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table id="tblInfoOutstandingTransfer" runat="server" style="display: none">
                        <tr>
                            <td rowspan="2" style="vertical-align: top">
                                <img height="60" class="blink-alert" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>'
                                    alt='' />
                            </td>
                            <td style="vertical-align: middle">
                                <table>
                                    <tr id="trCharges" runat="server" style="display: none">
                                        <td>
                                            <label class="lblWarning" id="lblInfoOutstandingBill">
                                                <%=GetLabel("Masih Ada Tagihan Outstanding") %></label>
                                        </td>
                                    </tr>
                                    <tr id="trOrder" runat="server" style="display: none">
                                        <td>
                                            <label class="lblWarning" id="lblInfoOutstandingOrder">
                                                <%=GetLabel("Masih Ada Order Outstanding") %></label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <h4 style="text-align: center">
                        <%=GetLabel("Pembayaran") %></h4>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 35%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <div style="position: relative;">
                                    <label class="lblLink lblKey" id="lblPaymentNo">
                                        <%=GetLabel("No Pembayaran")%></label></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentNo" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal / Jam")%></label>
                            </td>
                            <td colspan="2">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtPaymentDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPaymentTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Pembayaran Asal")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourcePaymentNo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Pembayaran")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboPaymentType" ClientInstanceName="cboPaymentType" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPaymentTypeValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trOutstandingDP" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=HttpUtility.HtmlEncode(GetDownPaymentCaption())%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnOutstandingDP" value="0" runat="server" />
                                <asp:TextBox ID="txtOutstandingDP" ReadOnly="true" CssClass="number" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lokasi Kasir")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboCashierGroup" ClientInstanceName="cboCashierGroup" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Shift")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboShift" ClientInstanceName="cboShift" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <h4 style="text-align: center">
                        <%=GetLabel("Informasi Tagihan") %></h4>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 30%" />
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Tagihan")%></label>
                            </td>
                            <td colspan="2">
                                <div id="divDdeBillingNo">
                                    <dxe:ASPxDropDownEdit ClientInstanceName="ddeBillingNo" ID="ddeBillingNo" Width="100%"
                                        runat="server" EnableAnimation="False">
                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                        <DropDownWindowTemplate>
                                            <asp:ListView ID="lvwBilling" runat="server">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" style="font-size: 0.8em" runat="server" class="grdNormal notAllowSelect"
                                                        cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 40px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" align="left">
                                                                <div style="padding: 3px; float: left;">
                                                                    <div>
                                                                        <%= GetLabel("No Bill")%></div>
                                                                    <div>
                                                                        <%= GetLabel("Tanggal  / Jam")%></div>
                                                                </div>
                                                            </th>
                                                            <th colspan="3">
                                                                <%=GetLabel("Amount")%>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="5">
                                                                <%=GetLabel("No Data To Display") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" style="font-size: 0.8em" runat="server" class="grdNormal notAllowSelect"
                                                        cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 40px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" align="left">
                                                                <div style="padding: 3px; float: left;">
                                                                    <div>
                                                                        <%= GetLabel("No Bill")%></div>
                                                                    <div>
                                                                        <%= GetLabel("Tanggal / Jam")%></div>
                                                                </div>
                                                            </th>
                                                            <th colspan="3">
                                                                <%=GetLabel("Amount")%>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkIsSelected" Checked="true" CssClass="chkIsSelected" runat="server" />
                                                                <input type="hidden" class="hdnKeyField" value="<%# Eval("PatientBillingID")%>" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; float: left;">
                                                                <a class="lnkTransactionNo">
                                                                    <%# Eval("PatientBillingNo")%></a>
                                                                <div>
                                                                    <%# Eval("BillingDateInString")%>
                                                                    <%# Eval("BillingTime")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <input type="hidden" class="hdnPayerAmount" value='<%# Eval("PayerRemainingAmount")%>' />
                                                                <div>
                                                                    <%# Eval("PayerRemainingAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <input type="hidden" class="hdnPatientAmount" value='<%# Eval("PatientRemainingAmount")%>' />
                                                                <div>
                                                                    <%# Eval("PatientRemainingAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <input type="hidden" class="hdnLineAmount" value='<%# Eval("RemainingAmount")%>' />
                                                                <div>
                                                                    <%# Eval("RemainingAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </DropDownWindowTemplate>
                                    </dxe:ASPxDropDownEdit>
                                </div>
                                <div id="divTxtBillingNo" style="display: none">
                                    <asp:TextBox ID="txtBillingNo" Width="100%" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Tagihan")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingTotal" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 30%" />
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td>
                                <h4 style="text-align: center">
                                    <%=GetLabel("Tagihan Harus Dibayar") %></h4>
                            </td>
                            <td>
                                <h4 style="text-align: center">
                                    <%=GetLabel("Pasien") %></h4>
                            </td>
                            <td>
                                <h4 style="text-align: center">
                                    <%=GetLabel("Instansi") %></h4>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tagihan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingOriginalPatient" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingOriginalPayer" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pembulatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientRoundingAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayerRoundingAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tagihan Akhir")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingTotalPatient" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingTotalPayer" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Pembayaran")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTotalPatient" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTotalPayer" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4 style="text-align: left">
                        <%=GetLabel("Detil Pembayaran")%></h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divContainerGrdDetailAdd">
                        <table class="grdNormal" id="tblPaymentDt" style="width: 100%; font-size: 0.9em"
                            cellpadding="0" cellspacing="0">
                            <tr>
                                <th rowspan="2" align="center" style="width: 30px">
                                    <div style="padding: 3px;">
                                        #</div>
                                </th>
                                <th rowspan="2" align="left">
                                    <div style="padding: 3px; float: left;">
                                        <div>
                                            <%= GetLabel("Metode Pembayaran")%></div>
                                    </div>
                                </th>
                                <th colspan="2">
                                    <%=GetLabel("Kartu Kredit")%>
                                </th>
                                <th colspan="2">
                                    <%=GetLabel("Informasi Bank")%>
                                </th>
                                <th colspan="3">
                                    <%=GetLabel("Jumlah")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 120px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("EDC")%>
                                    </div>
                                </th>
                                <th style="width: 180px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("Informasi Kartu")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("Bank")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("No Referensi")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Pembayaran")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Fee")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Line Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr class="trFooter">
                                <td colspan="6">
                                    <div style="text-align: right; padding: 3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalPatient">
                                        0</div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalCardFee">
                                        0</div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalLineTotal">
                                        0</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divContainerGrdDetailEdit" style="display: none">
                        <dxcp:ASPxCallbackPanel ID="cbpPaymentDt" runat="server" Width="100%" ClientInstanceName="cbpPaymentDt"
                            ShowLoadingPanel="false" OnCallback="cbpPaymentDt_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <table class="grdNormal" id="tblPaymentDtEdit" style="width: 100%; font-size: 0.9em"
                                        cellpadding="0" cellspacing="0">
                                        <tr>
                                            <th rowspan="2" align="left">
                                                <div style="padding: 3px; float: left;">
                                                    <div>
                                                        <%= GetLabel("Metode Pembayaran")%></div>
                                                </div>
                                            </th>
                                            <th colspan="2">
                                                <%=GetLabel("Kartu Kredit")%>
                                            </th>
                                            <th colspan="2">
                                                <%=GetLabel("Informasi Bank")%>
                                            </th>
                                            <th colspan="3">
                                                <%=GetLabel("Jumlah")%>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="width: 120px">
                                                <div style="padding-left: 3px">
                                                    <%=GetLabel("EDC")%>
                                                </div>
                                            </th>
                                            <th style="width: 180px">
                                                <div style="padding-left: 3px">
                                                    <%=GetLabel("Informasi Kartu")%>
                                                </div>
                                            </th>
                                            <th style="width: 150px">
                                                <div style="padding-left: 3px">
                                                    <%=GetLabel("Bank")%>
                                                </div>
                                            </th>
                                            <th style="width: 150px">
                                                <div style="padding-left: 3px">
                                                    <%=GetLabel("No Referensi")%>
                                                </div>
                                            </th>
                                            <th style="width: 150px">
                                                <div style="text-align: right; padding-right: 3px">
                                                    <%=GetLabel("Pembayaran")%>
                                                </div>
                                            </th>
                                            <th style="width: 150px">
                                                <div style="text-align: right; padding-right: 3px">
                                                    <%=GetLabel("Fee")%>
                                                </div>
                                            </th>
                                            <th style="width: 150px">
                                                <div style="text-align: right; padding-right: 3px">
                                                    <%=GetLabel("Line Total")%>
                                                </div>
                                            </th>
                                        </tr>
                                        <asp:ListView ID="lvwPaymentDt" runat="server">
                                            <LayoutTemplate>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#Eval("PaymentMethod") %>
                                                    </td>
                                                    <td>
                                                        <%#Eval("EDCMachineName")%>
                                                    </td>
                                                    <td>
                                                        <a class="lnkCardNumber">
                                                            <%#Eval("CardNumber")%></a>
                                                        <input type="hidden" class="hdnGCCardType" value="<%#Eval("GCCardType")%>" />
                                                        <input type="hidden" class="hdnGCCardProvider" value="<%#Eval("GCCardProvider")%>" />
                                                        <input type="hidden" class="hdnCardNumber1" value="<%#Eval("CardNumber1")%>" />
                                                        <input type="hidden" class="hdnCardNumber4" value="<%#Eval("CardNumber4")%>" />
                                                        <input type="hidden" class="hdnCardHolderName" value="<%#Eval("CardHolderName")%>" />
                                                        <input type="hidden" class="hdnCardValidThru" value="<%#Eval("CardValidThru")%>" />
                                                        <input type="hidden" class="hdnBatchNoEdit" value="<%#Eval("BatchNo")%>" />
                                                        <input type="hidden" class="hdnTraceNoEdit" value="<%#Eval("TraceNo")%>" />
                                                        <input type="hidden" class="hdnReferenceNoEdit" value="<%#Eval("ReferenceNo")%>" />
                                                        <input type="hidden" class="hdnApprovalCodeEdit" value="<%#Eval("ApprovalCode")%>" />
                                                        <input type="hidden" class="hdnPaymentAmountEdit" value="<%#Eval("PaymentAmount")%>" />
                                                        <input type="hidden" class="hdnPaymentDtID" value="<%#Eval("PaymentDetailID")%>" />
                                                        <input type="hidden" class="hdnEDCMachineTransactionID" value="<%#Eval("EDCMachineTransactionID")%>" />
                                                        <input type="hidden" class="hdnEDCMachineTransactionIsFinish" value="<%#Eval("IsFinish")%>" />
                                                        <input type="hidden" class="hdnEDCMachineID" value="<%#Eval("EDCMachineID")%>" />
                                                        <input type="hidden" class="hdnEdcIsVoid" value="<%#Eval("IsVoid")%>" />
                                                    </td>
                                                    <td>
                                                        <%#Eval("BankName")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("ReferenceNo")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#Eval("PaymentAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#Eval("CardFeeAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#Eval("LineTotal", "{0:N}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <tr class="trFooter">
                                            <td colspan="5">
                                                <div style="text-align: right; padding: 3px">
                                                    <%=GetLabel("Total")%>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="text-align: right; padding: 3px" id="tdTotalPatientEdit" runat="server">
                                                    0</div>
                                            </td>
                                            <td>
                                                <div style="text-align: right; padding: 3px" id="tdTotalCardFeeEdit" runat="server">
                                                    0</div>
                                            </td>
                                            <td>
                                                <div style="text-align: right; padding: 3px" id="tdLineTotalEdit" runat="server">
                                                    0</div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div id="divContainerGrdDetailPayerEdit" style="display: none">
                        <dxcp:ASPxCallbackPanel ID="cbpPaymentPayerEditDt" runat="server" Width="100%" ClientInstanceName="cbpPaymentDt"
                            ShowLoadingPanel="false" OnCallback="cbpPaymentPayerDt_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                    <table class="grdNormal" id="Table2" style="width: 100%; font-size: 0.9em" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <th rowspan="2" align="left">
                                                <div style="padding: 3px; float: left;">
                                                    <div>
                                                        <%= GetLabel("Metode Pembayaran")%></div>
                                                </div>
                                            </th>
                                            <th rowspan="2" align="left">
                                                <div style="padding: 3px; float: left;">
                                                    <div>
                                                        <%= GetLabel("Instansi")%></div>
                                                </div>
                                            </th>
                                            <th colspan="2">
                                                <%=GetLabel("Jumlah")%>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="width: 225px">
                                                <div style="text-align: right; padding-right: 3px">
                                                    <%=GetLabel("Pembayaran")%>
                                                </div>
                                            </th>
                                            <th style="width: 225px">
                                                <div style="text-align: right; padding-right: 3px">
                                                    <%=GetLabel("Line Total")%>
                                                </div>
                                            </th>
                                        </tr>
                                        <asp:ListView ID="lvwPaymentPayerDt" runat="server">
                                            <LayoutTemplate>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#Eval("PaymentMethod") %>
                                                    </td>
                                                    <td>
                                                        <%#Eval("BusinessPartnerName") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#Eval("PaymentAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#Eval("LineTotal", "{0:N}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <tr class="trFooter">
                                            <td colspan="2">
                                                <div style="text-align: right; padding: 3px">
                                                    <%=GetLabel("Total")%>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="text-align: right; padding: 3px" id="tdTotalPayerEdit" runat="server">
                                                    0</div>
                                            </td>
                                            <td>
                                                <div style="text-align: right; padding: 3px" id="tdLineTotalPayerEdit" runat="server">
                                                    0</div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div id="divContainerGrdDetailARPayer">
                        <table class="grdNormal" id="tblPaymentDtPayer" style="width: 100%; font-size: 0.9em"
                            cellpadding="0" cellspacing="0">
                            <tr>
                                <th rowspan="2" align="center" style="width: 30px">
                                    <div style="padding: 3px;">
                                        #</div>
                                </th>
                                <th rowspan="2" align="left">
                                    <div style="padding: 3px; float: left;">
                                        <div>
                                            <%= GetLabel("Instansi")%></div>
                                    </div>
                                </th>
                                <th colspan="2">
                                    <%=GetLabel("Jumlah")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 225px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Pembayaran")%>
                                    </div>
                                </th>
                                <th style="width: 225px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Line Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr class="trFooter">
                                <td colspan="2">
                                    <div style="text-align: right; padding: 3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalPayer">
                                        0</div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalLineAmountPayer">
                                        0</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divContainerGrdDetailAR" style="display: none">
                        <table class="grdNormal" id="Table1" style="width: 100%; font-size: 0.9em" cellpadding="0"
                            cellspacing="0">
                            <tr>
                                <th rowspan="2" align="left" style="width: 200px">
                                    <div style="padding: 3px; float: left;">
                                        <div>
                                            <%= GetLabel("Metode Pembayaran")%></div>
                                    </div>
                                </th>
                                <th colspan="2">
                                    <%=GetLabel("Kartu Kredit")%>
                                </th>
                                <th colspan="2">
                                    <%=GetLabel("Informasi Bank")%>
                                </th>
                                <th colspan="3">
                                    <%=GetLabel("Jumlah")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 120px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("EDC")%>
                                    </div>
                                </th>
                                <th style="width: 180px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("Informasi Kartu")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("Bank")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="padding-left: 3px">
                                        <%=GetLabel("No Referensi")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Pembayaran")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Fee")%>
                                    </div>
                                </th>
                                <th style="width: 150px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Line Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr id="trUangMukaKeluar">
                                <td style="padding: 3px" id="tdUangMukaKeluar">
                                </td>
                                <td style="padding: 3px">
                                    &nbsp;
                                </td>
                                <td style="padding: 3px">
                                </td>
                                <td style="padding: 3px">
                                    &nbsp;
                                </td>
                                <td style="padding: 3px">
                                    &nbsp;
                                </td>
                                <td style="padding: 3px" align="right" id="tdNilaiUangMukaKeluar">
                                    0
                                </td>
                                <td style="padding: 3px" align="right">
                                    0
                                </td>
                                <td style="padding: 3px" align="right" id="tdNilaiAkhirUangMukaKeluar">
                                    0
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 3px" id="tdARPaymentMethod">
                                </td>
                                <td style="padding: 3px">
                                    &nbsp;
                                </td>
                                <td style="padding: 3px">
                                </td>
                                <td style="padding: 3px">
                                    &nbsp;
                                </td>
                                <td style="padding: 3px">
                                    &nbsp;
                                </td>
                                <td style="padding: 3px" align="right" id="tdPaymentDtAR">
                                    0
                                </td>
                                <td style="padding: 3px" align="right">
                                    0
                                </td>
                                <td style="padding: 3px" align="right" id="tdLineAmountAR">
                                    0
                                </td>
                            </tr>
                            <tr class="trFooter">
                                <td colspan="5">
                                    <div style="text-align: right; padding: 3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalAR">
                                        0</div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdTotalCardFeeAR">
                                        0</div>
                                </td>
                                <td>
                                    <div style="text-align: right; padding: 3px" id="tdLineTotalAR">
                                        0</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table style="width: 100%" id="tblCashback">
                        <tr>
                            <td align="right" style="padding-right: 5px">
                                <%=GetLabel("Uang Kembalian") %>
                            </td>
                            <td style="width: 150px">
                                <asp:TextBox ID="txtCashbackAmount" runat="server" CssClass="txtCurrency min" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" runat="server"
            Width="100%" EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains">
            <ClientSideEvents LostFocus="function(s,e){ grdPayment.hideAspxComboBox(s); }" KeyDown="grdPayment.onCboKeyDown" />
        </dxe:ASPxComboBox>
        <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" runat="server" Width="100%"
            EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains">
            <ClientSideEvents LostFocus="function(s,e){ grdPayment.hideAspxComboBox(s); }" KeyDown="grdPayment.onCboKeyDown" />
        </dxe:ASPxComboBox>
        <dxe:ASPxComboBox ID="cboEDCMachine" ClientInstanceName="cboEDCMachine" runat="server"
            Width="100%" EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains">
            <ClientSideEvents LostFocus="function(s,e){ grdPayment.hideAspxComboBox(s); }" KeyDown="grdPayment.onCboKeyDown" />
            <ClientSideEvents ValueChanged="function(s){ onCboEDCMachineValueChanged(s); }" />
        </dxe:ASPxComboBox>
        <dxe:ASPxComboBox ID="cboBusinessPartner" ClientInstanceName="cboBusinessPartner"
            runat="server" Width="100%" EnableSynchronization="False" ClientVisible="false"
            IncrementalFilteringMode="Contains">
            <ClientSideEvents LostFocus="function(s,e){ grdPaymentARPayer.hideAspxComboBox(s); }"
                KeyDown="grdPaymentARPayer.onCboKeyDown" />
        </dxe:ASPxComboBox>
        <div id="containerCbo" style="display: none">
        </div>
        <!-- Popup Entry Notes -->
        <dxpc:ASPxPopupControl ID="pcCardInformation" runat="server" ClientInstanceName="pcCardInformation"
            CloseAction="CloseButton" Height="180px" HeaderText="Informasi Detail Pembayaran dengan Kartu"
            Width="400px" Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dxpc:PopupControlContentControl runat="server" ID="pccc1">
                    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <fieldset id="fsCardInformation" style="margin: 0">
                                    <div style="text-align: left; width: 100%;">
                                        <table>
                                            <colgroup>
                                                <col style="width: 500px" />
                                            </colgroup>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <colgroup>
                                                            <col style="width: 150px" />
                                                            <col style="width: 200px" />
                                                        </colgroup>
                                                        <tr id="trLocationEDC" style="display: none;">
                                                            <td>
                                                                <label id="lblLocationEdc">
                                                                    <%=GetLabel("Lokasi EDC")%></label>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboLocationEDC" ClientInstanceName="cboLocationEDC" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td1" class="tdTipeKartu" runat="server">
                                                                <label id="lblTipeKartu" class="lblMandatory" runat="server">
                                                                    <%=GetLabel("Tipe Kartu")%></label>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboCardType" ClientInstanceName="cboCardType" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td2" class="tdBankPenerbit" runat="server">
                                                                <label id="lblBankPenerbit" class="lblMandatory" runat="server">
                                                                    <%=GetLabel("Bank Penerbit")%></label>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboCardProvider" ClientInstanceName="cboCardProvider" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td3" class="tdNoKartu" runat="server">
                                                                <label id="lblNoKartu" runat="server">
                                                                    <%=GetLabel("No. Kartu")%></label>
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber1" Text="XXXX" Width="100%" runat="server" Style="text-align: center" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber2" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                                                runat="server" Style="text-align: center" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber3" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                                                runat="server" Style="text-align: center" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber4" Width="100%" runat="server" Style="text-align: center" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td4" class="tdPemegangKartu" runat="server">
                                                                <label id="lblPemegangKartu" runat="server">
                                                                    <%=GetLabel("Pemegang Kartu")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHolderName" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td5" class="tdMasaBerlaku" runat="server">
                                                                <label id="lblMasaBerlaku" runat="server">
                                                                    <%=GetLabel("Masa Berlaku")%></label>
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="cboCardDateMonth" ClientInstanceName="cboCardDateMonth" Width="100px"
                                                                                runat="server" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="cboCardDateYear" ClientInstanceName="cboCardDateYear" Width="80px"
                                                                                runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td6" class="tdJumlah" runat="server">
                                                                <label id="lblJumlah" runat="server">
                                                                    <%=GetLabel("Jumlah")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPaymentAmount" Width="100%" runat="server" placeholder="isi tanpa titik ataupun koma" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td7" class="tdTerminal" runat="server">
                                                                <label id="lblTerminal" runat="server">
                                                                    <%=GetLabel("Terminal")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTerminalID" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td8" class="tdNoBatch" runat="server">
                                                                <label id="lblNoBatch" runat="server">
                                                                    <%=GetLabel("No. Batch")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBatchNo" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td9" class="tdNoTrace" runat="server">
                                                                <label id="lblNoTrace" runat="server">
                                                                    <%=GetLabel("No. Trace")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTraceNo" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td10" class="tdReferenceNo" runat="server">
                                                                <label id="lblReferenceNo" runat="server">
                                                                    <%=GetLabel("Reference No.")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td11" class="tdApprovalCode" runat="server">
                                                                <label id="lblApprovalCode" runat="server">
                                                                    <%=GetLabel("Appr Code")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="hdnPaymentDetailID" runat="server" />
                                                                <asp:HiddenField ID="hdnEDCMachineTransactionID" runat="server" />
                                                                <asp:HiddenField ID="hdnEDCMachineTransactionIsFinish" runat="server" />
                                                                <asp:HiddenField ID="hdnEdcIsVoid" runat="server" />
                                                                <asp:TextBox ID="txtApprovalCode" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationAdd" value='<%= GetLabel("OK")%>'
                                                        style="width: 95px" class="w3-button" />
                                                </td>
                                                <td>
                                                    <input type="button" style="display: none;" id="btnPaymentCardInformationSend" value='<%= GetLabel("Kirim ke EDC")%>'
                                                        style="width: 95px" onclick="closePcCardInformation('send');" class="w3-button" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnGetResponseEDC" value='<%= GetLabel("Ambil dari EDC")%>'
                                                        style="width: 95px" onclick="getResponseEdc();" class="w3-button" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationCancel" value='<%= GetLabel("Batal")%>'
                                                        style="width: 95px" onclick="closePcCardInformation('cancel');" class="w3-button" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationClose" value='<%= GetLabel("Tutup")%>'
                                                        style="width: 95px" onclick="pcCardInformation.Hide();" class="w3-button" />
                                                </td>
                                                <td>
                                                    <input type="button" style="display: none;" id="btnVoidCardInformationSend" value='<%= GetLabel("Void ke EDC")%>'
                                                        style="width: 95px" onclick="closePcCardInformation('void');" class="w3-button" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:ASPxPopupControl>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpEDCProcess" runat="server" Width="100%" ClientInstanceName="cbpEDCProcess"
        ShowLoadingPanel="false" OnCallback="cbpEDCProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpEDCProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <div style="width: 600px;">
                        <div class="pageTitle" style="text-align: center">
                            <%=GetLabel("Informasi")%></div>
                        <div style="background-color: #EAEAEA;">
                            <table width="600px" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col width="30px" />
                                </colgroup>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dibuat Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divCreatedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dibuat Pada") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divCreatedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidBy" style="display: none" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Divoid Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidDate" style="display: none" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Divoid Pada") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divLastUpdatedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Terakhir Diubah Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divLastUpdatedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidReason" style="display: none" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Alasan Batal")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidReason">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
