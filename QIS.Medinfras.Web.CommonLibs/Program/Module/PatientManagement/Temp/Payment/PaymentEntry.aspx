<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="PaymentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PaymentEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPaymentBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />   
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />   
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPatientName" runat="server" />
    <input type="hidden" value="" id="hdnCoverageLimit" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
    <script type="text/javascript">
        //#region Right Panel
        $(function () {
            setRightPanelButtonEnabled();
        });
        
        function setRightPanelButtonEnabled() {
            var isEnabled = false;
            if (!getIsAdd()) {
                var paymentType = cboPaymentType.GetValue();
                var paymentAllocation = parseInt($('#<%=hdnPaymentAllocation.ClientID %>').val());
                if (paymentType == Constant.PaymentType.DOWN_PAYMENT && paymentAllocation > 0)
                    isEnabled = true;
            }
            if (!isEnabled)
                $('#btnBillAllocation').attr('enabled', 'false');
            else
                $('#btnBillAllocation').removeAttr('enabled');
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%=hdnPaymentHdID.ClientID %>').val();
        }

        $(function () {
            Methods.checkImageError('imgPatientProfilePicture', 'patient', 'hdnPatientGender');

            $('#<%=btnPaymentBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/PatientList/RegistrationList.aspx?id=py');
            });

            $('.lnkTransactionNo').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillDiscount/PatientBillDiscountDtCtl.ascx");
                openUserControlPopup(url, id, 'Penerimaan Pembayaran Pasien Item', 1100, 500);
            });
        });


        function onAfterSaveAddRecordEntryPopup() {
            cbpMPEntryContent.PerformCallback('load');
        }
        //#endregion

        //#region Inline Editing
        var grdPayment = new InlineEditing();
        var numFinishLoad = 0;
        function init() {            
            var listParam = new Array();
            var cboPaymentMethodID = '<%=cboPaymentMethod.ClientID%>';
            var cboEDCMachineID = '<%=cboEDCMachine.ClientID%>';
            var cboBankID = '<%=cboBank.ClientID%>';

            listParam[0] = { "type": "cbo", "className": "cboPaymentMethod", "cboID": cboPaymentMethodID, "isUnique": false, "isEnabled": true };
            listParam[1] = { "type": "cbo", "className": "cboEDCMachine", "cboID": cboEDCMachineID, "isRequired": true, "isUnique": false, "isEnabled": false };
            listParam[2] = { "type": "bte", "className": "bteCardInformation", "isEnabled": false, "isRequired": true, "isButtonEnabled": false };
            listParam[3] = { "type": "cbo", "className": "cboBank", "cboID": cboBankID, "isUnique": false, "isRequired": true, "isEnabled": false };
            listParam[4] = { "type": "txt", "className": "txtReferenceNo", "isRequired": true, "isEnabled": false };
            listParam[5] = { "type": "txt", "className": "txtPayment", "isRequired": true, "isEnabled": true, "dataType": "money" };
            listParam[6] = { "type": "txt", "className": "txtFee", "isEnabled": false, "dataType": "money" };
            listParam[7] = { "type": "txt", "className": "txtLineTotal", "isEnabled": false, "dataType": "money" };
            listParam[8] = { "type": "hdn", "className": "hdnCardType" };
            listParam[9] = { "type": "hdn", "className": "hdnCardNo" };
            listParam[10] = { "type": "hdn", "className": "hdnHolderName" };
            listParam[11] = { "type": "hdn", "className": "hdnExpiredDateMonth" };
            listParam[12] = { "type": "hdn", "className": "hdnExpiredDateYear" };
            listParam[13] = { "type": "hdn", "className": "hdnCardFee", "value": "0" };
            listParam[14] = { "type": "hdn", "className": "hdnCardProvider" };

            grdPayment.setOnBteButtonClickHandler(function ($row, bteClass) {
                $currEditedRow = $row;
                if (bteClass == 'bteCardInformation') {
                    var cardtype = grdPayment.getCellHiddenValue($row, 'hdnCardType');
                    var cardNo = grdPayment.getCellHiddenValue($row, 'hdnCardNo');
                    var holderName = grdPayment.getCellHiddenValue($row, 'hdnHolderName');
                    var expiredDateMonth = grdPayment.getCellHiddenValue($row, 'hdnExpiredDateMonth');
                    var expiredDateYear = grdPayment.getCellHiddenValue($row, 'hdnExpiredDateYear');
                    var cardProvider = grdPayment.getCellHiddenValue($row, 'hdnCardProvider');

                    cboCardType.SetValue(cardtype);
                    $('#<%=txtCardNumber4.ClientID %>').val(cardNo);
                    $('#<%=txtHolderName.ClientID %>').val(holderName);
                    cboCardDateMonth.SetValue(expiredDateMonth);
                    cboCardDateYear.SetValue(expiredDateYear);
                    cboCardProvider.SetValue(cardProvider);

                    cboCardType.SetFocus();

                    pcCardInformation.Show();
                }
            });

            grdPayment.setOnCboValueChangedHandler(function ($row, cboClass, oldValue, newValue) {
                if (cboClass == 'cboPaymentMethod') {
                    grdPayment.setCellHiddenValue($row, 'hdnCardFee ', '0');
                    grdPayment.setTextBoxProperties($row, 'txtFee', { "value": 0 });
                    grdPayment.setTextBoxProperties($row, 'txtPayment', { "value": 0 });
                    grdPayment.setTextBoxProperties($row, 'txtLineTotal', { "value": 0 });

                    var isCreditOrDebit = (newValue == 'X035^002' || newValue == 'X035^003');
                    grdPayment.setComboBoxProperties($row, 'cboEDCMachine', { "isEnabled": isCreditOrDebit, "value": "" });
                    grdPayment.setButtonEditProperties($row, 'bteCardInformation', { "isButtonEnabled": isCreditOrDebit, "value": "" });

                    var isBankTransfer = (newValue == 'X035^004');
                    grdPayment.setTextBoxProperties($row, 'txtReferenceNo', { "isEnabled": isBankTransfer, "value": "" });

                    var isBankTransferOrCreditOrDebit = (isCreditOrDebit || isBankTransfer);
                    grdPayment.setComboBoxProperties($row, 'cboBank', { "isEnabled": isBankTransferOrCreditOrDebit, "value": "" });
                }
                else if (cboClass == 'cboEDCMachine') {
                    getCardFee($row);
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
            calculatePaymentDtTotal();
        }

        function calculateCardFeeAndLineTotal($row) {
            var payment = parseFloat(grdPayment.getTextBoxValue($row, 'txtPayment'));
            var cardFeeInPercentage = parseFloat(grdPayment.getCellHiddenValue($row, 'hdnCardFee'));
            var cardFee = payment * cardFeeInPercentage / 100;
            var lineTotal = payment + cardFee;
            grdPayment.setTextBoxProperties($row, 'txtFee', { "value": cardFee });
            grdPayment.setTextBoxProperties($row, 'txtLineTotal', { "value": lineTotal });
            calculatePaymentDtTotal();
        }

        function getCardFee($row) {
            var cardProvider = grdPayment.getCellHiddenValue($row, 'hdnCardProvider');
            var cardtype = grdPayment.getCellHiddenValue($row, 'hdnCardType');
            var edcMachine = grdPayment.getComboBoxValue($row, 'cboEDCMachine');
            if (edcMachine != '' && cardtype != '' && cardProvider != '') {
                var filterExpression = $('#<%=hdnCreditCardFeeFilterExpression.ClientID %>').val().replace('[GCCardType]', cardtype).replace('[GCCardProvider]', cardProvider).replace('[EDCMachineID]', edcMachine);
                Methods.getObjectValue('GetCreditCardList', filterExpression, 'CreditCardFee', function (result) {
                    if (result == '')
                        result = '0';
                    grdPayment.setCellHiddenValue($row, 'hdnCardFee ', result);
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

        function calculateCashbackAmount() {
            var totalBilling = parseFloat($('#<%=txtBillingTotalPatient.ClientID %>').attr('hiddenVal'));
            var totalPayment = parseFloat($('#<%=hdnTotalPaymentAmount.ClientID %>').val());
            var cashBackAmount = totalPayment - totalBilling;
            $('#<%=hdnCashbackAmount.ClientID %>').val(cashBackAmount);
            $('#<%=txtCashbackAmount.ClientID %>').val(cashBackAmount).trigger('changeValue');
        }

        function closePcCardInformation(action) {
            if (action == 'save') {
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardType', cboCardType.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardNo', $('#<%=txtCardNumber4.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnHolderName', $('#<%=txtHolderName.ClientID %>').val());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnExpiredDateMonth', cboCardDateMonth.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnExpiredDateYear', cboCardDateYear.GetValue());
                grdPayment.setCellHiddenValue($currEditedRow, 'hdnCardProvider', cboCardProvider.GetValue());

                var cardInformation = 'XXXX-XXXX-XXXX-' + $('#<%=txtCardNumber4.ClientID %>').val();
                grdPayment.setButtonEditProperties($currEditedRow, 'bteCardInformation', { value: cardInformation });
                    
                grdPayment.setButtonEditFocus($currEditedRow, 'bteCardInformation');
                pcCardInformation.Hide();

                getCardFee($currEditedRow);
            }
            else {
                grdPayment.setButtonEditFocus($currEditedRow, 'bteCardInformation');
                pcCardInformation.Hide();
            }
                
        }
        //#endregion

        function onBeforeSaveRecord() {
            var isAllowSave = true;
            if (cboPaymentType.GetValue() == Constant.PaymentType.SETTLEMENT) {
                var cashBackAmount = parseFloat($('#<%=hdnCashbackAmount.ClientID %>').val());
                if (cashBackAmount < 0)
                    isAllowSave = false;
            }
            if (isAllowSave) {
                var isValid = grdPayment.validate();
                if (isValid) {
                    $('#<%=hdnInlineEditingData.ClientID %>').val(grdPayment.getTableData());
                    return true;
                }
                return false;
            }
            else {
                showToast('Warning', '<%=GetErrorMsgCashBackAmount() %>');
                return false;
            }
        }

        function onLoad() {
            $('#btnPaymentCardInformationAdd').click(function (evt) {
                if (IsValid(evt, 'fsCardInformation', 'vgCardInformation'))
                    closePcCardInformation('save');
            });

            //#region Transaction No
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
                setDatePicker('<%=txtPaymentDate.ClientID %>');
                setDdeBillingNoText();

                $('.chkIsSelected input').change(function () {
                    $('.chkSelectAll input').prop('checked', false);
                    setDdeBillingNoText();
                });

                $('.chkSelectAll input').change(function () {
                    var isChecked = $(this).is(":checked");
                    $('.chkIsSelected').each(function () {
                        $(this).find('input').prop('checked', isChecked);
                    });
                    setDdeBillingNoText();
                });

                if (cboPaymentMethod != null && cboEDCMachine != null && cboBank != null)
                    init();

                $('#divContainerGrdDetailAdd').show();
                $('#divContainerGrdDetailEdit').hide();
                $('#divContainerGrdDetailAR').hide();
                $('#divDdeBillingNo').show();
                $('#divTxtBillingNo').hide();
                $('#btnPaymentCardInformationAdd').show();
                $('#btnPaymentCardInformationCancel').show();
                $('#btnPaymentCardInformationClose').hide();
            }
            else {
                $('#divContainerGrdDetailAdd').hide();
                $('#divContainerGrdDetailEdit').show();
                $('#divContainerGrdDetailAR').hide();

                $('#divDdeBillingNo').hide();
                $('#divTxtBillingNo').show();

                $('#btnPaymentCardInformationAdd').hide();
                $('#btnPaymentCardInformationCancel').hide();
                $('#btnPaymentCardInformationClose').show();

                $('.lnkCardNumber').click(function () {
                    $td = $(this).parent();
                    cboCardType.SetValue($td.find('.hdnGCCardType').val());
                    $('#<%=txtCardNumber4.ClientID %>').val($td.find('.hdnCardNumber4').val());
                    $('#<%=txtHolderName.ClientID %>').val($td.find('.hdnCardHolderName').val());
                    var cardValidThru = $td.find('.hdnCardValidThru').val().split('/');
                    var expiredDateMonth = parseInt(cardValidThru[0]);
                    var expiredDateYear = 2000 + parseInt(cardValidThru[1]);
                    cboCardDateMonth.SetValue(expiredDateMonth);
                    cboCardDateYear.SetValue(expiredDateYear);
                    cboCardProvider.SetValue($td.find('.hdnGCCardProvider').val());

                    pcCardInformation.Show();
                });
            }
            onCboPaymentTypeValueChanged();
        }

        function onCboPaymentTypeValueChanged() {
            var paymentType = cboPaymentType.GetValue();
            if (paymentType == Constant.PaymentType.SETTLEMENT) 
                $('#tblCashback').show();
            else 
                $('#tblCashback').hide();
            if (paymentType == Constant.PaymentType.AR_PATIENT || paymentType == Constant.PaymentType.AR_PAYER) {
                $('#divContainerGrdDetailAR').show();
                $('#divContainerGrdDetailEdit').hide();
                $('#divContainerGrdDetailAdd').hide();

                setARTotal()
            }
            else {
                if (getIsAdd()) {
                    $('#divContainerGrdDetailAdd').show();
                    $('#divContainerGrdDetailEdit').hide();
                    $('#divContainerGrdDetailAR').hide();
                }
                else {
                    $('#divContainerGrdDetailAdd').hide();
                    $('#divContainerGrdDetailEdit').show();
                    $('#divContainerGrdDetailAR').hide();
                }
            }
        }

        function setARTotal() {
            if (cboPaymentType.GetValue() == Constant.PaymentType.AR_PATIENT)
                var total = $('#<%=txtBillingTotalPatient.ClientID %>').val();
            else
                var total = $('#<%=txtBillingTotalPayer.ClientID %>').val();

            $('#tdPaymentDtAR').html(total);
            $('#tdLineAmountAR').html(total);
            $('#tdTotalAR').html(total);
            $('#tdLineTotalAR').html(total); 
        }

        function setDdeBillingNoText() {
            var payerAmount = 0;
            var patientAmount = 0;
            var lineAmount = 0;
            var billingNo = '';
            var lstBillingID = '';
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                if (billingNo != '') {
                    billingNo += ', ';
                    lstBillingID += ',';
                }
                lstBillingID += $tr.find('.hdnKeyField').val();
                billingNo += $tr.find('.lnkTransactionNo').html();
                payerAmount += parseFloat($tr.find('.hdnPayerAmount').val());
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
            });
            ddeBillingNo.SetText(billingNo);
            $('#<%=hdnListBillingID.ClientID %>').val(lstBillingID);
            $('#<%=txtBillingTotal.ClientID %>').val(lineAmount).trigger('changeValue');

            var coverageLimit = parseInt($('#<%=hdnCoverageLimit.ClientID %>').val());
            if (payerAmount > coverageLimit) {
                patientAmount = patientAmount + payerAmount - coverageLimit;
                payerAmount = coverageLimit;
            }

            $('#<%=txtBillingTotalPatient.ClientID %>').val(patientAmount).trigger('changeValue');
            $('#<%=txtBillingTotalPayer.ClientID %>').val(payerAmount).trigger('changeValue');

            $('#<%=hdnBillingTotalPatient.ClientID %>').val(patientAmount);
            $('#<%=hdnBillingTotalPayer.ClientID %>').val(payerAmount);

            var paymentType = cboPaymentType.GetValue();
            if (paymentType == Constant.PaymentType.AR_PATIENT || paymentType == Constant.PaymentType.AR_PAYER)
                setARTotal();
            calculateCashbackAmount();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnPaymentHdID.ClientID %>').val();
            if (transactionID == '') {
                errMessage.text = 'Please Save Transaction First!';
                return false;
            }
            else {
                filterExpression.text = transactionID;
                return true;
            }
        }
    </script> 
    <style type="text/css">
        #tblPaymentDtEdit td           { padding: 3px; }
    </style>
    <input type="hidden" value="" id="hdnListBillingID" runat="server" />
    <input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
    <input type="hidden" value="" id="hdnCreditCardFeeFilterExpression" runat="server" />  
    <input type="hidden" value="" id="hdnTotalPaymentAmount" runat="server" />  
    <input type="hidden" value="" id="hdnTotalFeeAmount" runat="server" />  
    <input type="hidden" value="" id="hdnPaymentHdID" runat="server" />  
    <input type="hidden" value="" id="hdnCashbackAmount" runat="server" />  
    <input type="hidden" value="" id="hdnBillingTotalPatient" runat="server" />  
    <input type="hidden" value="" id="hdnBillingTotalPayer" runat="server" />  
    <input type="hidden" value="" id="hdnPaymentAllocation" runat="server" />  
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Penerimaan Pembayaran Pasien")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <h4 style="text-align:center"><%=GetLabel("Informasi Pembayaran") %></h4>
                    <table style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col style="width:35%"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblPaymentNo"><%=GetLabel("No Pembayaran")%></label></td>
                            <td><asp:TextBox ID="txtPaymentNo" Width="100%" runat="server" /></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal / Jam")%></label></td>
                            <td colspan="2">
                                 <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtPaymentDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                        <td style="width:5px">&nbsp;</td>
                                        <td><asp:TextBox ID="txtPaymentTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Keterangan")%></label></td>
                            <td colspan="2"><asp:TextBox ID="txtRemarks" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tipe Pembayaran")%></label></td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboPaymentType" ClientInstanceName="cboPaymentType" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPaymentTypeValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding:5px;vertical-align:top">
                    <h4 style="text-align:center"><%=GetLabel("Informasi Tagihan") %></h4>
                    <table style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col style="width:30%"/>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Tagihan")%></label></td>
                            <td colspan="2">
                                <div id="divDdeBillingNo">
                                    <dxe:ASPxDropDownEdit ClientInstanceName="ddeBillingNo" ID="ddeBillingNo"
                                        Width="100%" runat="server" EnableAnimation="False">
                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                        <DropDownWindowTemplate>
                                            <asp:ListView ID="lvwBilling" runat="server">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" style="font-size:0.8em" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                        <tr>  
                                                            <th style="width:40px" rowspan="2">
                                                                <div style="padding:3px">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" align="left">
                                                                <div style="padding:3px;float:left;">
                                                                    <div><%= GetLabel("No Bill")%></div>
                                                                    <div><%= GetLabel("Tanggal / Jam")%></div>                                                    
                                                                </div>
                                                            </th>
                                                            <th colspan="3"><%=GetLabel("Amount")%></th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width:70px">
                                                                <div style="text-align:right;padding-right:3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width:70px">
                                                                <div style="text-align:right;padding-right:3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width:70px">
                                                                <div style="text-align:right;padding-right:3px">
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
                                                    <table id="tblView" style="font-size:0.8em" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                        <tr>  
                                                            <th style="width:40px" rowspan="2">
                                                                <div style="padding:3px">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" align="left">
                                                                <div style="padding:3px;float:left;">
                                                                    <div><%= GetLabel("No Bill")%></div>
                                                                    <div><%= GetLabel("Tanggal / Jam")%></div>                                                    
                                                                </div>
                                                            </th>
                                                            <th colspan="3"><%=GetLabel("Amount")%></th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width:70px">
                                                                <div style="text-align:right;padding-right:3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width:70px">
                                                                <div style="text-align:right;padding-right:3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width:70px">
                                                                <div style="text-align:right;padding-right:3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder" ></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center">
                                                            <div style="padding:3px">
                                                                <asp:CheckBox ID="chkIsSelected" Checked="true" CssClass="chkIsSelected" runat="server" />
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientBillingID")%>" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;float:left;">
                                                                <a class="lnkTransactionNo"><%#: Eval("PatientBillingNo")%></a>
                                                                <div><%#: Eval("BillingDateInString")%> <%#: Eval("BillingTime")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;text-align:right;">
                                                                <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayer")%>' />
                                                                <div><%#: Eval("PayerRemainingAmount", "{0:N}")%></div>                                                   
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;text-align:right;">
                                                                <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("PatientRemainingAmount")%>' />
                                                                <div><%#: Eval("PatientRemainingAmount", "{0:N}")%></div>                                                   
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;text-align:right;">
                                                                <input type="hidden" class="hdnLineAmount" value='<%#: Eval("RemainingAmount")%>' />
                                                                <div><%#: Eval("RemainingAmount", "{0:N}")%></div>                                                   
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </DropDownWindowTemplate>
                                    </dxe:ASPxDropDownEdit>
                                </div>
                                <div id="divTxtBillingNo" style="display:none" >
                                    <asp:TextBox ID="txtBillingNo" Width="100%" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total Tagihan")%></label></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ID="txtBillingTotal" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                        <tr id="trCoverageLimit" runat="server">
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Batas Tanggungan")%></label></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                    <table style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col style="width:30%"/>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td><h4 style="text-align:center"><%=GetLabel("Tagihan Harus Dibayar") %></h4></td>
                            <td><h4 style="text-align:center"><%=GetLabel("Pasien") %></h4></td>
                            <td><h4 style="text-align:center"><%=GetLabel("Instansi") %></h4></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tagihan")%></label></td>
                            <td><asp:TextBox ID="txtBillingTotalPatient" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtBillingTotalPayer" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="3"><hr /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total Pembayaran")%></label></td>
                            <td><asp:TextBox ID="txtPaymentTotalPatient" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                            <td><asp:TextBox ID="txtPaymentTotalPayer" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4 style="text-align:left"><%=GetLabel("Detil Pembayaran")%></h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divContainerGrdDetailAdd">
                        <table class="grdNormal" id="tblPaymentDt" style="width:100%;font-size:0.9em" cellpadding="0" cellspacing="0">
                            <tr>  
                                <th rowspan="2" align="center" style="width:30px">
                                    <div style="padding:3px;">#</div>
                                </th>
                                <th rowspan="2" align="left">
                                    <div style="padding:3px;float:left;">
                                        <div><%= GetLabel("Metode Pembayaran")%></div>
                                    </div>
                                </th>
                                <th colspan="2"><%=GetLabel("Kartu Kredit")%></th>
                                <th colspan="2"><%=GetLabel("Informasi Bank")%></th>
                                <th colspan="3"><%=GetLabel("Jumlah")%></th>
                            </tr>
                            <tr>
                                <th style="width:120px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("EDC")%>
                                    </div>
                                </th>
                                <th style="width:180px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("Informasi Kartu")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("Bank")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("No Referensi")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Pembayaran")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Fee")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Line Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr class="trFooter">  
                                <td colspan="6">
                                    <div style="text-align:right;padding:3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align:right;padding:3px" id="tdTotalPatient">0</div>
                                </td>
                                <td>
                                    <div style="text-align:right;padding:3px" id="tdTotalCardFee">0</div>
                                </td>
                                <td>
                                    <div style="text-align:right;padding:3px" id="tdTotalLineTotal">0</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divContainerGrdDetailEdit" style="display:none">
                        <dxcp:ASPxCallbackPanel ID="cbpPaymentDt" runat="server" Width="100%" ClientInstanceName="cbpPaymentDt"
                            ShowLoadingPanel="false" OnCallback="cbpPaymentDt_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <table class="grdNormal" id="tblPaymentDtEdit" style="width:100%;font-size:0.9em" cellpadding="0" cellspacing="0">
                                        <tr>  
                                            <th rowspan="2" align="left">
                                                <div style="padding:3px;float:left;">
                                                    <div><%= GetLabel("Metode Pembayaran")%></div>
                                                </div>
                                            </th>
                                            <th colspan="2"><%=GetLabel("Kartu Kredit")%></th>
                                            <th colspan="2"><%=GetLabel("Informasi Bank")%></th>
                                            <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                        </tr>
                                        <tr>
                                            <th style="width:120px">
                                                <div style="padding-left:3px">
                                                    <%=GetLabel("EDC")%>
                                                </div>
                                            </th>
                                            <th style="width:180px">
                                                <div style="padding-left:3px">
                                                    <%=GetLabel("Informasi Kartu")%>
                                                </div>
                                            </th>
                                            <th style="width:150px">
                                                <div style="padding-left:3px">
                                                    <%=GetLabel("Bank")%>
                                                </div>
                                            </th>
                                            <th style="width:150px">
                                                <div style="padding-left:3px">
                                                    <%=GetLabel("No Referensi")%>
                                                </div>
                                            </th>
                                            <th style="width:150px">
                                                <div style="text-align:right;padding-right:3px">
                                                    <%=GetLabel("Pembayaran")%>
                                                </div>
                                            </th>
                                            <th style="width:150px">
                                                <div style="text-align:right;padding-right:3px">
                                                    <%=GetLabel("Fee")%>
                                                </div>
                                            </th>
                                            <th style="width:150px">
                                                <div style="text-align:right;padding-right:3px">
                                                    <%=GetLabel("Line Total")%>
                                                </div>
                                            </th>
                                        </tr>
                                    <asp:ListView ID="lvwPaymentDt" runat="server">
                                        <LayoutTemplate>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#:Eval("PaymentMethod") %></td>
                                                <td><%#:Eval("EDCMachineName")%></td>
                                                <td>
                                                    <a class="lnkCardNumber"><%#:Eval("CardNumber")%></a>
                                                    <input type="hidden" class="hdnGCCardType" value="<%#:Eval("GCCardType")%>" />
                                                    <input type="hidden" class="hdnGCCardProvider" value="<%#:Eval("GCCardProvider")%>" />
                                                    <input type="hidden" class="hdnCardNumber4" value="<%#:Eval("CardNumber4")%>" />
                                                    <input type="hidden" class="hdnCardHolderName" value="<%#:Eval("CardHolderName")%>" />
                                                    <input type="hidden" class="hdnCardValidThru" value="<%#:Eval("CardValidThru")%>" />
                                                </td>
                                                <td><%#:Eval("BankName")%></td>
                                                <td><%#:Eval("ReferenceNo")%></td>
                                                <td align="right"><%#:Eval("PaymentAmount", "{0:N}")%></td>
                                                <td align="right"><%#:Eval("CardFeeAmount", "{0:N}")%></td>
                                                <td align="right"><%#:Eval("LineTotal", "{0:N}")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                        <tr class="trFooter">  
                                            <td colspan="5">
                                                <div style="text-align:right;padding:3px">
                                                    <%=GetLabel("Total")%>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="text-align:right;padding:3px" id="tdTotalPatientEdit" runat="server">0</div>
                                            </td>
                                            <td>
                                                <div style="text-align:right;padding:3px" id="tdTotalCardFeeEdit" runat="server">0</div>
                                            </td>
                                            <td>
                                                <div style="text-align:right;padding:3px" id="tdLineTotalEdit" runat="server">0</div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div id="divContainerGrdDetailAR" style="display:none">
                        <table class="grdNormal" id="Table1" style="width:100%;font-size:0.9em" cellpadding="0" cellspacing="0">
                            <tr>  
                                <th rowspan="2" align="left">
                                    <div style="padding:3px;float:left;">
                                        <div><%= GetLabel("Metode Pembayaran")%></div>
                                    </div>
                                </th>
                                <th colspan="2"><%=GetLabel("Kartu Kredit")%></th>
                                <th colspan="2"><%=GetLabel("Informasi Bank")%></th>
                                <th colspan="3"><%=GetLabel("Jumlah")%></th>
                            </tr>
                            <tr>
                                <th style="width:120px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("EDC")%>
                                    </div>
                                </th>
                                <th style="width:180px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("Informasi Kartu")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("Bank")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="padding-left:3px">
                                        <%=GetLabel("No Referensi")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Pembayaran")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Fee")%>
                                    </div>
                                </th>
                                <th style="width:150px">
                                    <div style="text-align:right;padding-right:3px">
                                        <%=GetLabel("Line Total")%>
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <td id="tdARPaymentMethod" runat="server"></td>
                                <td>&nbsp;</td>
                                <td></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td align="right" id="tdPaymentDtAR">0</td>
                                <td align="right">0</td>
                                <td align="right" id="tdLineAmountAR">0</td>
                            </tr>
                            <tr class="trFooter">  
                                <td colspan="5">
                                    <div style="text-align:right;padding:3px">
                                        <%=GetLabel("Total")%>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align:right;padding:3px" id="tdTotalAR">0</div>
                                </td>
                                <td>
                                    <div style="text-align:right;padding:3px" id="tdTotalCardFeeAR">0</div>
                                </td>
                                <td>
                                    <div style="text-align:right;padding:3px" id="tdLineTotalAR">0</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table style="width:100%" id="tblCashback">
                        <tr>
                            <td align="right" style="padding-right:5px"><%=GetLabel("Uang Kembalian") %></td>
                            <td style="width:150px"><asp:TextBox ID="txtCashbackAmount" runat="server" CssClass="txtCurrency min" Width="150px" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>  
        <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" runat="server" Width="100%" 
            EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains" >
            <ClientSideEvents LostFocus="function(s,e){ grdPayment.hideAspxComboBox(s); }" 
                KeyDown="grdPayment.onCboKeyDown" />
        </dxe:ASPxComboBox>
        <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" runat="server" Width="100%" 
            EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains" >
            <ClientSideEvents LostFocus="function(s,e){ grdPayment.hideAspxComboBox(s); }" 
                KeyDown="grdPayment.onCboKeyDown" />
        </dxe:ASPxComboBox>
        <dxe:ASPxComboBox ID="cboEDCMachine" ClientInstanceName="cboEDCMachine" runat="server" Width="100%" 
            EnableSynchronization="False" ClientVisible="false" IncrementalFilteringMode="Contains" >
            <ClientSideEvents LostFocus="function(s,e){ grdPayment.hideAspxComboBox(s); }" 
                KeyDown="grdPayment.onCboKeyDown" />
        </dxe:ASPxComboBox>
        <div id="containerCbo" style="display:none"></div>

        <!-- Popup Entry Notes -->
        <dxpc:ASPxPopupControl ID="pcCardInformation" runat="server" ClientInstanceName="pcCardInformation" CloseAction="CloseButton"
            Height="180px" HeaderText="Card Information" Width="400px" Modal="True" PopupAction="None"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dxpc:PopupControlContentControl runat="server" ID="pccc1">
                    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <fieldset id="fsCardInformation" style="margin:0"> 
                                    <div style="text-align: left; width: 100%;">
                                        <table>
                                            <colgroup>
                                                <col style="width: 500px"/>
                                            </colgroup>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <colgroup>
                                                            <col style="width:150px"/>
                                                            <col style="width:200px"/>
                                                        </colgroup>
                                                        <tr>
                                                            <td><%=GetLabel("Card Type")%></td>
                                                            <td><dxe:ASPxComboBox ID="cboCardType" ClientInstanceName="cboCardType" Width="100%" runat="server" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td><%=GetLabel("Card Provider")%></td>
                                                            <td><dxe:ASPxComboBox ID="cboCardProvider" ClientInstanceName="cboCardProvider" Width="100%" runat="server" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td><%=GetLabel("Card Number")%></td>
                                                            <td>
                                                                <table style="width:100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td><asp:TextBox ID="txtCardNumber1" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%" runat="server" /></td>
                                                                        <td style="width:3px">&nbsp;</td>
                                                                        <td><asp:TextBox ID="txtCardNumber2" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%" runat="server" /></td>
                                                                        <td style="width:3px">&nbsp;</td>
                                                                        <td><asp:TextBox ID="txtCardNumber3" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%" runat="server" /></td>
                                                                        <td style="width:3px">&nbsp;</td>
                                                                        <td><asp:TextBox ID="txtCardNumber4" Width="100%" runat="server" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><%=GetLabel("Holder Name")%></td>
                                                            <td><asp:TextBox ID="txtHolderName" Width="100%" runat="server" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td><%=GetLabel("Card Date")%></td>
                                                            <td>
                                                                <table style="width:100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td><dxe:ASPxComboBox ID="cboCardDateMonth" ClientInstanceName="cboCardDateMonth" Width="100px" runat="server" /></td>
                                                                        <td style="width:3px">&nbsp;</td>
                                                                        <td><dxe:ASPxComboBox ID="cboCardDateYear" ClientInstanceName="cboCardDateYear" Width="80px" runat="server" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>                                                     
                                                    </table>  
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationAdd" value='<%= GetLabel("Add")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationCancel" value='<%= GetLabel("Cancel")%>' onclick="closePcCardInformation('cancel');" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationClose" value='<%= GetLabel("Close")%>' onclick="pcCardInformation.Hide();" />
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
</asp:Content>