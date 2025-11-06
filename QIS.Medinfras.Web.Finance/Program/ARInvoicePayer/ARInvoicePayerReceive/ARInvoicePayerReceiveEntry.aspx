<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/CustomerPage/MPBaseCustomerPageTrx.master"
    AutoEventWireup="true" CodeBehind="ARInvoicePayerReceiveEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerReceiveEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/ARInvoicePayer/ARInvoicePayerToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
    <script type="text/javascript">

        function onLoad() {
            setCustomToolbarVisibility();
            setRightPanelButtonEnabled();

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            $('#btnPaymentCardInformationAdd').click(function (evt) {
                if (IsValid(evt, 'fsCardInformation', 'vgCardInformation'))
                    closePcCardInformation('save');
            });

            //#region ARReceivingNo
            $('#lblARReceivingNo.lblLink').click(function () {
                var filterExpression = 'BusinessPartnerID = ' + $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                openSearchDialog('arreceivinghd', filterExpression, function (value) {
                    onTxtPaymentNoChanged(value);
                });
            });
            $('#<%=txtARReceivingNo.ClientID %>').change(function () {
                var val = $(this).val();
                var filterExpression = 'BusinessPartnerID = ' + $('#<%=hdnBusinessPartnerID.ClientID %>').val() + " AND ARReceivingNo = '" + val + "'";
                Methods.getObject('GetvARReceivingHdList', filterExpression, function (result) {
                    if (result != null)
                        onTxtPaymentNoChanged(val);
                    else
                        $('#<%=txtARReceivingNo.ClientID %>').val('');
                });
            });

            function onTxtPaymentNoChanged(value) {
                onLoadObject(value);
            }

            //#endregion

            if (getIsAdd()) {
                if ($('#<%=hdnIsAllowBackdateARI.ClientID %>').val() == '1') {
                    setDatePicker('<%=txtReceivingDate.ClientID %>');
                }

                $('.chkIsSelected input').change(function () {
                    $('.chkSelectAll input').prop('checked', false);
                    setddeInvoiceNoText();
                });

                $('.chkSelectAll input').change(function () {
                    var isChecked = $(this).is(":checked");
                    $('.chkIsSelected').each(function () {
                        $(this).find('input').prop('checked', isChecked);
                    });
                    setddeInvoiceNoText();
                });

                if (cboPaymentMethod != null && cboEDCMachine != null && cboBank != null)
                    init();

                $('#divContainerGrdDetailAdd').show();
                $('#divContainerGrdDetailEdit').hide();
                $('#divContainerGrdDetailAR').hide();
                $('#divDdeInvoiceNo').show();
                $('#divTxtInvoiceNo').hide();
                $('#btnPaymentCardInformationAdd').show();
                $('#btnPaymentCardInformationCancel').show();
                $('#btnPaymentCardInformationClose').hide();

                setddeInvoiceNoText();
            }
            else {
                $('#divContainerGrdDetailAdd').hide();
                $('#divContainerGrdDetailEdit').show();
                $('#divContainerGrdDetailAR').hide();

                $('#divDdeInvoiceNo').hide();
                $('#divTxtInvoiceNo').show();

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
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            if (transactionStatus == Constant.TransactionStatus.OPEN) {
                if (isVoid == 1) {
                    $('#<%=btnVoidByReason.ClientID %>').show();
                } else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                }
            } else {
                $('#<%=btnVoidByReason.ClientID %>').hide();
            }
        }

        $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
            showDeleteConfirmation(function (data) {
                var param = 'voidbyreason;' + data.GCDeleteReason + ';' + data.Reason;
                onCustomButtonClick(param);
            });
        });

        function onAfterSaveAddRecordEntryPopup() {
            cbpMPEntryContent.PerformCallback('load');
        }

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function setddeInvoiceNoText() {
            var transactionAmount = 0;
            var lineAmount = 0;
            var invoiceNo = '';
            var lstInvoiceID = '';
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                if (invoiceNo != '') {
                    invoiceNo += ', ';
                    lstInvoiceID += ',';
                }
                lstInvoiceID += $tr.find('.hdnKeyField').val();
                invoiceNo += $.trim($tr.find('.lnkARInvoiceNo').html());
                transactionAmount += parseFloat($tr.find('.hdnClaimedAmount').val());
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
            });
            ddeInvoiceNo.SetText(invoiceNo);
            $('#<%=hdnListInvoiceID.ClientID %>').val(lstInvoiceID);
            $('#<%=txtRemainingTotal.ClientID %>').val(lineAmount).trigger('changeValue');
            $('#<%=hdnTotalTransactionAmount.ClientID %>').val(lineAmount);

        }

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

                    var isCreditOrDebit = (newValue == 'X254^003' || newValue == 'X254^004');
                    grdPayment.setComboBoxProperties($row, 'cboEDCMachine', { "isEnabled": isCreditOrDebit, "value": "" });
                    grdPayment.setButtonEditProperties($row, 'bteCardInformation', { "isButtonEnabled": isCreditOrDebit, "value": "" });

                    var isBankTransfer = (newValue == 'X254^002');
                    grdPayment.setTextBoxProperties($row, 'txtReferenceNo', { "isEnabled": isBankTransfer, "value": "" });

                    grdPayment.setComboBoxProperties($row, 'cboBank', { "isEnabled": isBankTransfer, "value": "" });
                    //var isBankTransferOrCreditOrDebit = (isCreditOrDebit || isBankTransfer);
                    //grdPayment.setComboBoxProperties($row, 'cboBank', { "isEnabled": isBankTransferOrCreditOrDebit, "value": "" });

                    if (isCreditOrDebit) {
                        var amount = parseInt($('#<%=hdnCashbackAmount.ClientID %>').val()) * -1;
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

            $('#tdTotalPayment').html(totalPayment.formatMoney(2, '.', ','));
            $('#tdTotalCardFee').html(totalCardFee.formatMoney(2, '.', ','));
            $('#tdTotalLineTotal').html(totalLineTotal.formatMoney(2, '.', ','));

            $('#<%=hdnTotalPaymentAmount.ClientID %>').val(totalPayment);
            $('#<%=hdnTotalFeeAmount.ClientID %>').val(totalCardFee);

            calculateCashbackAmount();
        }

        function calculateCashbackAmount() {
            var totalLineTotal = grdPayment.getColumnTotal('txtLineTotal');
            var totalBilling = $('#<%=txtRemainingTotal.ClientID %>').attr('hiddenVal');
            var cashback = totalLineTotal - totalBilling;
            if (cashback < 0) cashback = 0
            $('#<%=hdnCashbackAmount.ClientID %>').val(cashback);
            $('#<%=txtCashbackAmount.ClientID %>').val(cashback).trigger('changeValue');
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
            var errMessage = '';

            if (isAllowSave) {
                var isValid = grdPayment.validate();
                if (isValid) {
                    $('#<%=hdnInlineEditingData.ClientID %>').val(grdPayment.getTableData());
                    return true;
                }
                return false;
            }
            else {
                showToast('Warning', errMessage);
                return false;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var arReceivingID = $('#<%=hdnARReceivingID.ClientID %>').val();
            if (arReceivingID == '' || arReceivingID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "ARReceivingID = " + arReceivingID + ";"
                                        + "ARInvoiceID IN (SELECT ARInvoiceID FROM ARInvoiceReceiving WHERE ARReceivingID = " + arReceivingID + " AND IsDeleted = 0)";
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnARReceivingID.ClientID %>').val();
            if (param != "" && param != "0") {
                return param;
            }
            else {
                showToast('Failed', 'Maaf, pilih Nomor Pembayaran terlebih dahulu.');
            }
        }

        function setRightPanelButtonEnabled() {            
            var arReceivingID = $('#<%=hdnARReceivingID.ClientID %>').val();
            if (arReceivingID != '' && arReceivingID != null) {
                var filterExpression = "ARReceivingID = " + arReceivingID;
                Methods.getObject('GetARReceivingHdList', filterExpression, function (result) {
                    if (result != null) {
                        var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
                        if (transactionStatus == 'X121^001') {
                            $('#btnAlocationPayer').removeAttr('enabled');
                        }
                        else {
                            $('#btnAlocationPayer').attr('enabled', 'false');
                        }

                        $('#btninfoReceivingInvoicePerReceive').removeAttr('enabled');
                    }
                    else {
                        $('#btnAlocationPayer').attr('enabled', 'false');
                        $('#btninfoReceivingInvoicePerReceive').attr('enabled', 'false');
                    }
                });
            } else {
                $('#btnAlocationPayer').attr('enabled', 'false');
                $('#btninfoReceivingInvoicePerReceive').attr('enabled', 'false');
            }
        }

        $(function () {
            $('.lnkARInvoiceNo').live('click', function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();

                var url = ResolveUrl("~/Program/ARInvoicePayer/ARInvoicePayerReceive/ARInvoiceDetailInformationReceivingCtl.ascx");
                openUserControlPopup(url, id, 'Detail Invoice', 1100, 550);
            });
        });
    </script>
    <div style="height: 435px; overflow-y: auto;">
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
        <input type="hidden" value="" id="hdnListInvoiceID" runat="server" />
        <input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
        <input type="hidden" value="" id="hdnARReceivingID" runat="server" />
        <input type="hidden" value="" id="hdnIsAllowBackdateARI" runat="server" />
        <input type="hidden" value="" id="hdnTotalPaymentAmount" runat="server" />
        <input type="hidden" value="" id="hdnTotalTransactionAmount" runat="server" />
        <input type="hidden" value="" id="hdnCashbackAmount" runat="server" />
        <input type="hidden" value="" id="hdnCreditCardFeeFilterExpression" runat="server" />
        <input type="hidden" value="" id="hdnTotalFeeAmount" runat="server" />
        <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
        <input type="hidden" id="hdnIsAllowVoidByReason" runat="server" value="" />
        <input type="hidden" id="hdnSelectedMember" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <h4 style="text-align: center;">
                        <%=GetLabel("Informasi Pembayar") %></h4>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <div style="position: relative;">
                                    <label class="lblLink lblKey" id="lblARReceivingNo">
                                        <%=GetLabel("No Pembayaran")%></label></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtARReceivingNo" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Bayar")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceivingDate" Width="150px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
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
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Voucher")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoucherNo" Width="150px" runat="server" ReadOnly="true" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tgl. Voucher")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoucherDate" Width="150px" runat="server" ReadOnly="true" Style="text-align: center" />
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
                                <div id="divDdeInvoiceNo">
                                    <dxe:ASPxDropDownEdit ClientInstanceName="ddeInvoiceNo" ID="ddeInvoiceNo" Width="100%"
                                        runat="server" EnableAnimation="False">
                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                        <DropDownWindowTemplate>
                                            <asp:ListView ID="lvwInvoice" runat="server">
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
                                                                        <%= GetLabel("No Tagihan")%></div>
                                                                    <div>
                                                                        <%= GetLabel("Tgl Tagihan")%></div>
                                                                    <div>
                                                                        <%= GetLabel("No Ref")%></div>
                                                                </div>
                                                            </th>
                                                            <th colspan="4">
                                                                <%=GetLabel("Jumlah")%>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total Klaim")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total Bayar")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total Oustanding")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="6">
                                                                <%=GetLabel("Data Tidak Tersedia") %>
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
                                                                        <%= GetLabel("No Tagihan")%></div>
                                                                    <div>
                                                                        <%= GetLabel("Tgl Tagihan")%></div>
                                                                    <div>
                                                                        <%= GetLabel("No Ref")%></div>
                                                                </div>
                                                            </th>
                                                            <th colspan="4">
                                                                <%=GetLabel("Jumlah")%>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total Klaim")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total Bayar")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total Oustanding")%>
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
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("ARInvoiceID")%>" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; float: left;">
                                                                <a class="lnkARInvoiceNo">
                                                                    <%#: Eval("ARInvoiceNo")%></a>
                                                                <div>
                                                                    <%#: Eval("ARInvoiceDateInString")%></div>
                                                                <div>
                                                                    <label class="lblNormal" style="font-style: italic; font-size: x-small">
                                                                        <%#: Eval("ARReferenceNo")%></label></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <input type="hidden" class="hdnClaimedAmount" value='<%#: Eval("TotalClaimedAmount")%>' />
                                                                <div>
                                                                    <%#: Eval("TotalClaimedAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <input type="hidden" class="hdnTotalPaymentAmount" value='<%#: Eval("TotalPaymentAmount")%>' />
                                                                <div>
                                                                    <%#: Eval("TotalPaymentAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <input type="hidden" class="hdnLineAmount" value='<%#: Eval("RemainingAmount")%>' />
                                                                <div>
                                                                    <%#: Eval("RemainingAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </DropDownWindowTemplate>
                                    </dxe:ASPxDropDownEdit>
                                </div>
                                <div id="divTxtInvoiceNo" style="display: none">
                                    <asp:TextBox ID="txtInvoiceNo" Width="100%" runat="server" />
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
                                <asp:TextBox ID="txtRemainingTotal" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Pembayaran")%></label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentAmount" CssClass="txtCurrency" Width="100%" runat="server" />
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
                                    <div style="text-align: right; padding: 3px" id="tdTotalPayment">
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
                        <dxcp:ASPxCallbackPanel ID="cbpARReceivingDt" runat="server" Width="100%" ClientInstanceName="cbpARReceivingDt"
                            ShowLoadingPanel="false" OnCallback="cbpARReceivingDt_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <table class="grdNormal" id="tblARReceivingDtEdit" style="width: 100%; font-size: 0.9em"
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
                                        <asp:ListView ID="lvwARReceivingDt" runat="server">
                                            <LayoutTemplate>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#:Eval("ARPaymentMethod") %>
                                                    </td>
                                                    <td>
                                                        <%#:Eval("EDCMachineName")%>
                                                    </td>
                                                    <td>
                                                        <a class="lnkCardNumber">
                                                            <%#:Eval("CardNumber")%></a>
                                                        <input type="hidden" class="hdnGCCardType" value="<%#:Eval("GCCardType")%>" />
                                                        <input type="hidden" class="hdnGCCardProvider" value="<%#:Eval("GCCardProvider")%>" />
                                                        <input type="hidden" class="hdnCardNumber" value="<%#:Eval("CardNumber")%>" />
                                                        <input type="hidden" class="hdnCardHolderName" value="<%#:Eval("CardHolderName")%>" />
                                                        <input type="hidden" class="hdnCardValidThru" value="<%#:Eval("CardValidThru")%>" />
                                                    </td>
                                                    <td>
                                                        <%#:Eval("BankName")%>
                                                    </td>
                                                    <td>
                                                        <%#:Eval("ReferenceNo")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PaymentAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("CardFeeAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("LineTotal", "{0:N}")%>
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
                                                <div style="text-align: right; padding: 3px" id="tdTotalPaymentEdit" runat="server">
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
                    <div id="divContainerGrdDetailAR" style="display: none">
                        <table class="grdNormal" id="Table1" style="width: 100%; font-size: 0.9em" cellpadding="0"
                            cellspacing="0">
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
                            <tr>
                                <td id="tdARPaymentMethod" runat="server">
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right" id="tdPaymentDtAR">
                                    0
                                </td>
                                <td align="right">
                                    0
                                </td>
                                <td align="right" id="tdLineAmountAR">
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
        </dxe:ASPxComboBox>
        <div id="containerCbo" style="display: none">
        </div>
        <!-- Popup Entry Notes -->
        <dxpc:ASPxPopupControl ID="pcCardInformation" runat="server" ClientInstanceName="pcCardInformation"
            CloseAction="CloseButton" Height="180px" HeaderText="Informasi Kartu" Width="400px"
            Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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
                                                        <tr>
                                                            <td>
                                                                <%=GetLabel("Tipe Kartu")%>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboCardType" ClientInstanceName="cboCardType" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%=GetLabel("Bank Penerbit")%>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboCardProvider" ClientInstanceName="cboCardProvider" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%=GetLabel("No Kartu")%>
                                                            </td>
                                                            <td>
                                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber1" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                                                runat="server" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber2" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                                                runat="server" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber3" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                                                runat="server" />
                                                                        </td>
                                                                        <td style="width: 3px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCardNumber4" Width="100%" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%=GetLabel("Pemegang Kartu")%>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHolderName" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%=GetLabel("Masa Berlaku")%>
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
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationAdd" value='<%= GetLabel("Ok")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationCancel" value='<%= GetLabel("Batal")%>'
                                                        onclick="closePcCardInformation('cancel');" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPaymentCardInformationClose" value='<%= GetLabel("Tutup")%>'
                                                        onclick="pcCardInformation.Hide();" />
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
                                <tr id="trApprovedBy" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Approved Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trApprovedDate" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Approved Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidBy" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Void Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidDate" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Void Pada")%>
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
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
