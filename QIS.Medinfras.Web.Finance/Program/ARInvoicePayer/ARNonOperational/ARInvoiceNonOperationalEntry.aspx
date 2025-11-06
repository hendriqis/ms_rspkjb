<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/CustomerPage/MPBaseCustomerPageTrx.master"
    AutoEventWireup="true" CodeBehind="ARInvoiceNonOperationalEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoiceNonOperationalEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/ARInvoicePayer/ARInvoicePayerToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnApprove" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnCloseNew" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Void&New")%></div>
    </li>
    <li id="btnCloseVoid" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setRightPanelButtonEnabled();
            setCustomToolbarVisibility();

            if ($('#<%=hdnTransactionStatus.ClientID %>').val() == "X121^001" || $('#<%=hdnTransactionStatus.ClientID %>').val() == "") {
                setDatePicker('<%=txtInvoiceDate.ClientID %>');
                setDatePicker('<%=txtDueDate.ClientID %>');
                setDatePicker('<%=txtDocumentDate.ClientID %>');
                setDatePicker('<%=txtAmortizationFirstDate.ClientID %>');

                var oIsAllowBackdate = $('#<%=hdnIsAllowBackDate.ClientID %>').val();
                var oIsAllowFuturedate = $('#<%=hdnIsAllowFutureDate.ClientID %>').val();

                if (oIsAllowBackdate != "1") {
                    $('#<%=txtInvoiceDate.ClientID %>').datepicker('option', 'minDate', '0');
                }

                if (oIsAllowFuturedate != "1") {
                    $('#<%=txtInvoiceDate.ClientID %>').datepicker('option', 'maxDate', '0');
                }

            }

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            cbpView.PerformCallback('refresh');
        }

        $('#<%=txtInvoiceDate.ClientID %>').live('change', function () {
            var invoiceDate = $('#<%=txtInvoiceDate.ClientID %>').val();
            var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
            var isAllowBackdate = $('#<%=hdnIsAllowBackDate.ClientID %>').val();
            var isAllowFuturedate = $('#<%=hdnIsAllowFutureDate.ClientID %>').val();

            var from = invoiceDate.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (isAllowBackdate != "1") {
                if (f < t) {
                    $('#<%=txtInvoiceDate.ClientID %>').val(dateToday);
                }
            }

            if (isAllowFuturedate != "1") {
                if (f > t) {
                    $('#<%=txtInvoiceDate.ClientID %>').val(dateToday);
                }
            }
        });

        //#region AR Invoice
        function onGetARInvoiceFilterExpression() {
            var filterExpression = "<%:onGetARInvoiceFilterExpression() %>";
            return filterExpression;
        }

        $('#lblARInvoiceNo.lblLink').live('click', function () {
            openSearchDialog('arinvoicehd', onGetARInvoiceFilterExpression(), function (value) {
                $('#<%=txtARInvoiceNo.ClientID %>').val(value);
                onTxtARInvoiceChanged(value);
            });
        });

        $('#<%=txtARInvoiceNo.ClientID %>').live('change', function () {
            onTxtARInvoiceChanged($(this).val());
        });

        function onTxtARInvoiceChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        //#region Bank VA
        $('#lblBankVA.lblLink').live('click', function () {
            var filter = "IsDeleted = 0 AND BusinessPartnerID = " + $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            openSearchDialog('businesspartnervirtualaccount', filter, function (value) {
                var filterID = filter + " AND ID = " + value;
                Methods.getObject('GetvBusinessPartnerVirtualAccountList', filterID, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val(result.ID);
                        $('#<%=txtBankName.ClientID %>').val(result.BankName);
                        $('#<%=txtAccountNo.ClientID %>').val(result.AccountNo);
                    }
                    else {
                        $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val("");
                        $('#<%=txtBankName.ClientID %>').val("");
                        $('#<%=txtAccountNo.ClientID %>').val("");
                    }
                });
            });
        });
        //#endregion

        //#region Transaction Non Operational Type
        function getTransactionNonOperationalFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND TransactionCode = '5109'";
            return filterExpression;
        }

        $('#lblTransactionNonOperational.lblLink').live('click', function () {
            openSearchDialog('transactionnonoperational', getTransactionNonOperationalFilterExpression(), function (value) {
                $('#<%=txtTransactionNonOperationalTypeCode.ClientID %>').val(value);
                onTxtTransactionNonOperationalChanged(value);
            });
        });

        $('#<%=txtTransactionNonOperationalTypeCode.ClientID %>').live('change', function () {
            onTxtTransactionNonOperationalChanged($(this).val());
        });

        function onTxtTransactionNonOperationalChanged(value) {
            var filterExpression = getTransactionNonOperationalFilterExpression() + " AND TransactionNonOperationalTypeCode ='" + value + "'";
            Methods.getObject('GetvTransactionNonOperationalTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnTransactionNonOperationalTypeID.ClientID %>').val(result.TransactionNonOperationalTypeID);
                    $('#<%=txtTransactionNonOperationalTypeCode.ClientID %>').val(result.TransactionNonOperationalTypeCode);
                    $('#<%=txtTransactionNonOperationalTypeName.ClientID %>').val(result.TransactionNonOperationalTypeName);
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val(result.RevenueCostCenterCode);
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);
                }
                else {
                    $('#<%=hdnTransactionNonOperationalTypeID.ClientID %>').val('');
                    $('#<%=txtTransactionNonOperationalTypeCode.ClientID %>').val('');
                    $('#<%=txtTransactionNonOperationalTypeName.ClientID %>').val('');
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region TransactionAmount
        $('#<%:txtTransactionAmount.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');
            calculateTotal();
        });
        //#endregion

        //#region DiscountPercent
        $('#<%:chkIsDiscountPercent.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDiscountPercentage.ClientID%>').removeAttr('readonly');
                $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtDiscountPercentage.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtDiscountAmount.ClientID%>').removeAttr('readonly');
            }
        });

        $('#<%:txtDiscountPercentage.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');
            calculateTotal();
        });

        $('#<%:txtDiscountAmount.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');
            calculateTotal();
        });

        //#endregion

        //#region PPN
        $('#<%:chkIsVATAmount.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');

            var isEditableVAT = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
            if ($(this).is(':checked')) {
                if (isEditableVAT == '1') {
                    $('#<%:txtVATPercentage.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#<%:txtVATPercentage.ClientID %>').attr('readonly', 'readonly');
                }
            }
            else {
                $('#<%:txtVATPercentage.ClientID %>').attr('readonly', 'readonly');
            }

            calculateTotal();
        });

        $('#<%=txtVATPercentage.ClientID %>').die('change');
        $('#<%=txtVATPercentage.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');
            calculateTotal();
        });
        //#endregion

        //#region PPH
        $('#<%=txtPPHPI.ClientID %>').die('change');
        $('#<%=txtPPHPI.ClientID %>').live('change', function () {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        });

        function oncboPPHTypeValueChanged(evt) {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        }

        function oncboPPHOptionsValueChanged(evt) {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        }

        $('#<%=chkPPHPercent.ClientID %>').live('change', function () {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $('#<%=txtPPH.ClientID %>').change();
                $('#<%=txtPPH.ClientID%>').removeAttr('readonly');
                $('#<%=txtPPHPI.ClientID%>').attr('readonly', 'readonly');

                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $('#<%=txtPPHPI.ClientID %>').change();
                $('#<%=txtPPH.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtPPHPI.ClientID%>').removeAttr('readonly');

                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        });

        $('#<%=txtPPH.ClientID %>').live('change', function () {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        });
        //#endregion

        //#region Revenue Cost Center
        function getRevenueCostCenterFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblRevenueCostCenter.lblLink').live('click', function () {
            openSearchDialog('revenuecostcenter', getRevenueCostCenterFilterExpression(), function (value) {
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val(value);
                onTxtRevenueCostCenterChanged(value);
            });
        });

        $('#<%=txtRevenueCostCenterCode.ClientID %>').live('change', function () {
            onTxtRevenueCostCenterChanged($(this).val());
        });

        function onTxtRevenueCostCenterChanged(value) {
            var filterExpression = getRevenueCostCenterFilterExpression() + " AND RevenueCostCenterCode ='" + value + "'";
            Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val(result.RevenueCostCenterCode);
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);
                }
                else {
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Amortisasi
        $('#<%:chkIsAmortization.ClientID %>').live('change', function () {
            $(this).trigger('changeValue');
            if ($(this).is(':checked')) {
                $('#<%=trPeriodAmortization.ClientID %>').show();
                $('#<%=trAmortizationFirstDate.ClientID %>').show();
            } else {
                $('#<%=trPeriodAmortization.ClientID %>').hide();
                $('#<%=trAmortizationFirstDate.ClientID %>').hide();
            }
        });

        $('.lblAmortization.lblLink').live('click', function () {
            showLoadingPanel();
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            var url = ResolveUrl('~/Program/ARInvoicePayer/ARNonOperational/AmortizationEntryCtl.ascx');
            var arInvoiceDtID = entity.ID;

            openUserControlPopup(url, arInvoiceDtID, 'Amortization Entry', 500, 500);
        });
        //#endregion

        //#region calculateTotal
        function calculateTotal() {
            if ($('#<%=chkIsDiscountPercent.ClientID %>').is(':checked')) {
                var transactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));
                var discountPercentage = parseFloat($('#<%=txtDiscountPercentage.ClientID %>').attr('hiddenVal'));
                var disc = (transactionAmount * discountPercentage) / 100;
                var totalAmount = transactionAmount - disc;

                $('#<%=txtDiscountPercentage.ClientID %>').val(discountPercentage).trigger('changeValue');
                $('#<%=txtDiscountAmount.ClientID %>').val(disc).trigger('changeValue');
                $('#<%=txtClaimAmount.ClientID %>').val(totalAmount).trigger('changeValue');
                $('#<%=hdnClaimAmount.ClientID %>').val(totalAmount);
            } else {
                var discAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal'));
                var transactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));

                var discountPercentage = (discAmount / transactionAmount) * 100;
                var claimAmount = transactionAmount - discAmount;

                $('#<%=txtDiscountPercentage.ClientID %>').val(discountPercentage).trigger('changeValue');
                $('#<%=txtClaimAmount.ClientID %>').val(claimAmount).trigger('changeValue');
                $('#<%=hdnClaimAmount.ClientID %>').val(claimAmount);
            }

            if ($('#<%=chkIsVATAmount.ClientID %>').is(':checked')) {
                var claim = parseFloat($('#<%=txtClaimAmount.ClientID %>').attr('hiddenVal'));
                var PPNPercent = parseFloat($('#<%=txtVATPercentage.ClientID %>').val());
                var PPN = (claim * PPNPercent) / 100;
                var FinalAmount = claim + PPN;

                $('#<%=txtVATAmount.ClientID %>').val(PPN).trigger('changeValue');
                $('#<%=hdnVATAmount.ClientID %>').val(PPN);
                $('#<%=txtClaimAmount.ClientID %>').val(FinalAmount).trigger('changeValue');
                $('#<%=hdnClaimAmount.ClientID %>').val(FinalAmount);
            } else {
                $('#<%=txtVATAmount.ClientID %>').val("0.00").trigger('changeValue');
                $('#<%=hdnVATAmount.ClientID %>').val("0").trigger('changeValue');
            }

            var PPH = parseFloat($('#<%=txtPPHPI.ClientID %>').attr('hiddenVal'));
            var nilaiClaim = parseFloat($('#<%=txtClaimAmount.ClientID %>').attr('hiddenVal'));

            var nilaiClaimFinal = nilaiClaim + PPH;
            $('#<%=txtClaimAmount.ClientID %>').val(nilaiClaimFinal).trigger('changeValue');
            $('#<%=hdnClaimAmount.ClientID %>').val(nilaiClaimFinal);
        }
        //#endregion

        //#region calculatePPH
        function calculatePPH(kode) {
            var transactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));
            if (kode == "fromPctg") {
                var pctg1 = parseFloat($('#<%=txtPPH.ClientID %>').attr('hiddenVal'));
                var totalPPH1 = transactionAmount * (pctg1 / 100);
                if (cboPPHOptions.GetText() == 'Minus') {
                    totalPPH1 *= -1;
                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }
                $('#<%=txtPPHPI.ClientID %>').val(totalPPH1).trigger('changeValue');
                $('#<%=txtPPH.ClientID %>').val(pctg1).trigger('changeValue');

                var pctg = parseFloat($('#<%=txtPPH.ClientID %>').attr('hiddenVal'));
                var totalPPH = transactionAmount * (pctg / 100);

                if (cboPPHOptions.GetText() == 'Minus') {
                    totalPPH *= -1;
                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }

                $('#<%=txtPPHPI.ClientID %>').val(totalPPH).trigger('changeValue');
                $('#<%=txtPPH.ClientID %>').val(pctg).trigger('changeValue');
            } else if (kode == "fromTxt") {
                var pph = parseFloat($('#<%=txtPPHPI.ClientID %>').attr('hiddenVal'));
                var pctg = pph / (transactionAmount / 100);

                if (cboPPHOptions.GetText() == 'Minus') {
                    if (pph > 0) {
                        pph *= -1;
                    }

                    if (pctg > 0) {
                        pctg *= -1;
                    }
                }
                else {
                    if (pph < 0) {
                        pph *= -1;
                    }

                    if (pctg < 0) {
                        pctg *= -1;
                    }
                }

                $('#<%=txtPPHPI.ClientID %>').val(pph).trigger('changeValue');
                $('#<%=txtPPH.ClientID %>').val(pctg).trigger('changeValue');
            }
            var PPH = parseFloat($('#<%=txtPPHPI.ClientID %>').attr('hiddenVal'));
        }
        //#endregion

        //#region After Save
        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            if (transactionStatus != Constant.TransactionStatus.OPEN) {
                $('#<%=btnVoidByReason.ClientID %>').hide();
                $('#<%=btnApprove.ClientID %>').hide();
            }
            else if (transactionStatus == Constant.TransactionStatus.OPEN) {
                if (isVoid == 1) {
                    $('#<%=btnVoidByReason.ClientID %>').show();
                    $('#<%=btnApprove.ClientID %>').show();
                } else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                    $('#<%=btnApprove.ClientID %>').hide();
                }
            }

            if (transactionStatus == 'X121^003') {
                $('#<%=btnCloseNew.ClientID %>').show();

                if (isVoid == "1") {
                    $('#<%=btnCloseVoid.ClientID %>').show();
                }
                else {
                    $('#<%=btnCloseVoid.ClientID %>').hide();
                }
            }
            else {
                $('#<%=btnCloseNew.ClientID %>').hide();
                $('#<%=btnCloseVoid.ClientID %>').hide();
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=hdnARInvoiceID.ClientID %>').val() == '0' || $('#<%=hdnARInvoiceID.ClientID %>').val() == '') {
                $('#<%=hdnARInvoiceID.ClientID %>').val(param);
                var filterExpression = 'ARInvoiceID = ' + param;
                Methods.getObject('GetARInvoiceHdList', filterExpression, function (result) {
                    $('#<%=txtARInvoiceNo.ClientID %>').val(result.ARInvoiceNo);
                    onLoadObject(result.ARInvoiceNo);
                    onAfterCustomSaveSuccess();
                    calculateTotal();
                });
            }
            else {
                calculateTotal();
            }
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordDtSuccess(ARInvoiceID) {
            if ($('#<%=hdnARInvoiceID.ClientID %>').val() == '0' || $('#<%=hdnARInvoiceID.ClientID %>').val() == '') {
                $('#<%=hdnARInvoiceID.ClientID %>').val(ARInvoiceID);
                var filterExpression = 'ARInvoiceID = ' + ARInvoiceID;
                Methods.getObject('GetARInvoiceHdList', filterExpression, function (result) {
                    $('#<%=txtARInvoiceNo.ClientID %>').val(result.ARInvoiceNo);
                    onLoadObject(result.ARInvoiceNo);
                    onAfterCustomSaveSuccess();
                });
            }
        }

        function cbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            $('#containerEntry').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    isAfterAdd = true;
                    var ARInvoiceID = param[2];
                    onAfterSaveRecordDtSuccess(ARInvoiceID);
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    cbpView.PerformCallback('refresh');
                }
            }

            if ($('#<%=txtARInvoiceNo.ClientID %>').val() != '') {
                onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
            }
        }
        //#endregion

        //#region Add Data
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                if ($('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() != "") {
                    $('#containerEntry').show();
                } else {
                    displayMessageBox('Informasi', "Maaf, pilih Bank & VA terlebih dahulu");
                }
            }
        });

        $('#lblAddData').live('click', function (evt) {
            $('#<%=hdnEntryID.ClientID %>').val("");
            $('#<%=txtTransactionNonOperationalTypeCode.ClientID %>').val("");
            $('#<%=txtTransactionNonOperationalTypeName.ClientID %>').val("");
            $('#<%=txtTransactionAmount.ClientID %>').val(0);

            if ($('#<%=chkIsDiscountPercent.ClientID %>').is(':checked')) {
                $('#<%=txtDiscountPercentage.ClientID%>').removeAttr('readonly');
                $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtDiscountPercentage.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtDiscountAmount.ClientID%>').removeAttr('readonly');
            }

            $('#<%=txtDiscountPercentage.ClientID %>').val(0);
            $('#<%=txtDiscountAmount.ClientID %>').val(0);
            $('#<%=txtVATAmount.ClientID %>').val(0);
            $('#<%=hdnVATAmount.ClientID %>').val("0");

            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $('#<%=txtPPH.ClientID%>').removeAttr('readonly');
                $('#<%=txtPPHPI.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtPPH.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtPPHPI.ClientID%>').removeAttr('readonly');
            }

            $('#<%=txtPPH.ClientID%>').val(0);
            $('#<%=txtPPHPI.ClientID%>').val(0);
            $('#<%=txtClaimAmount.ClientID %>').val(0);
            $('#<%=hdnClaimAmount.ClientID %>').val("0");
            $('#<%=txtRevenueCostCenterCode.ClientID %>').val("");
            $('#<%=txtRevenueCostCenterName.ClientID %>').val("");

            $('#<%=chkIsVATAmount.ClientID %>').prop('checked', false);

            if ($('#<%=chkIsAmortization.ClientID %>').is(':checked')) {
                $('#<%=trPeriodAmortization.ClientID %>').show();
                $('#<%=trAmortizationFirstDate.ClientID %>').show();
            } else {
                $('#<%=trPeriodAmortization.ClientID %>').hide();
                $('#<%=trAmortizationFirstDate.ClientID %>').hide();
            }

            $('#<%=txtAmortizationPeriodInMonth.ClientID %>').val("1");
            $('#<%=txtAmortizationFirstDate.ClientID %>').val($('#<%=txtInvoiceDate.ClientID %>').val());
        });
        //#endregion

        //#region Edit and Delete
        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });
        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnTransactionNonOperationalTypeID.ClientID %>').val(entity.TransactionNonOperationalTypeID);
            $('#<%=txtTransactionNonOperationalTypeCode.ClientID %>').val(entity.TransactionNonOperationalTypeCode);
            $('#<%=txtTransactionNonOperationalTypeName.ClientID %>').val(entity.TransactionNonOperationalTypeName);
            $('#<%=txtTransactionAmount.ClientID %>').val(entity.TransactionAmount).trigger('changeValue');
            $('#<%=txtDiscountPercentage.ClientID %>').val(entity.DiscountPercentage);
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
            $('#<%=txtDiscountAmount.ClientID%>').attr('readonly', 'readonly');

            $('#<%=txtVATAmount.ClientID %>').val(entity.VATAmount).trigger('changeValue');
            $('#<%=hdnVATAmount.ClientID %>').val(entity.VATAmount);
            $('#<%=txtRemarksDt.ClientID %>').val(entity.Remarks).trigger('changeValue');
            var vatPercent = 0;
            if (entity.VATAmount != 0) {
                vatPercent = parseFloat(entity.VATAmount / (entity.TransactionAmount - entity.DiscountPercentage) * 100);
                $('#<%=chkIsVATAmount.ClientID %>').prop('checked', true);
            } else {
                $('#<%=chkIsVATAmount.ClientID %>').prop('checked', false);
            }
            $('#<%=txtVATPercentage.ClientID %>').val(vatPercent).trigger('changeValue');

            var isEditableVAT = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
            if ($('#<%=chkIsVATAmount.ClientID %>').is(':checked')) {
                if (isEditableVAT == '1') {
                    $('#<%:txtVATPercentage.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#<%:txtVATPercentage.ClientID %>').attr('readonly', 'readonly');
                }
            }
            else {
                $('#<%:txtVATPercentage.ClientID %>').attr('readonly', 'readonly');
            }

            cboPPHType.SetValue(entity.GCPPHType);
            $('#<%=txtPPH.ClientID %>').val(entity.PPHPercentage);
            $('#<%=txtPPHPI.ClientID %>').val(entity.PPHAmount).trigger('changeValue');
            if (entity.PPHMode == "False") {
                cboPPHOptions.SetValue("Minus");
            } else {
                cboPPHOptions.SetValue("Plus");
            }

            $('#<%=txtClaimAmount.ClientID %>').val(entity.ClaimedAmount).trigger('changeValue');
            $('#<%=hdnClaimAmount.ClientID %>').val(entity.ClaimedAmount);
            $('#<%=hdnRevenueCostCenterID.ClientID %>').val(entity.RevenueCostCenterID);
            $('#<%=txtRevenueCostCenterCode.ClientID %>').val(entity.RevenueCostCenterCode);
            $('#<%=txtRevenueCostCenterName.ClientID %>').val(entity.RevenueCostCenterName);

            if (entity.IsAmortization == "True") {
                $('#<%=chkIsAmortization.ClientID %>').prop('checked', true);
            } else {
                $('#<%=chkIsAmortization.ClientID %>').prop('checked', false);
            }

            if ($('#<%=chkIsAmortization.ClientID %>').is(':checked')) {
                $('#<%=trPeriodAmortization.ClientID %>').show();
                $('#<%=trAmortizationFirstDate.ClientID %>').show();
            } else {
                $('#<%=trPeriodAmortization.ClientID %>').hide();
                $('#<%=trAmortizationFirstDate.ClientID %>').hide();
            }

            $('#<%=txtAmortizationPeriodInMonth.ClientID %>').val(entity.AmortizationPeriodInMonth);
            $('#<%=txtAmortizationFirstDate.ClientID %>').val(entity.cfAmortizationFirstDate);

            $('#containerEntry').show();
        });
        //#endregion

        //#region Button
        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                if ($('#<%=hdnTransactionNonOperationalTypeID.ClientID %>').val() != null && $('#<%=hdnTransactionNonOperationalTypeID.ClientID %>').val() != "" && $('#<%=hdnTransactionNonOperationalTypeID.ClientID %>').val() != "0"
                    && $('#<%=hdnRevenueCostCenterID.ClientID %>').val() != null && $('#<%=hdnRevenueCostCenterID.ClientID %>').val() != "" && $('#<%=hdnRevenueCostCenterID.ClientID %>').val() != "0"
                    ) {
                    cbpProcess.PerformCallback('save');
                } else {
                    var messageBody = "Harap pilih terlebih dahulu jenis transaksi non operasional dan revenue cost center nya.";
                    displayMessageBox('INFORMATION', messageBody);
                }
            }
        });

        $('#btnClose').live('click', function (evt) {
            $('#<%=hdnEntryID.ClientID %>').val(0);
            $('#<%=txtTransactionAmount.ClientID %>').val(0).trigger('changeValue');
            $('#<%=chkIsAmortization.ClientID %>').prop('checked', false);

            $('#containerEntry').hide();
        });

        $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
            showDeleteConfirmation(function (data) {
                var param = 'justvoid;' + data.GCDeleteReason + ';' + data.Reason;
                onCustomButtonClick(param);
            });
        });

        $('#<%=btnApprove.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                onCustomButtonClick('approve');
            }
        });

        $('#<%=btnCloseNew.ClientID %>').click(function () {
            onCustomButtonClick('closenew');
        });

        $('#<%=btnCloseVoid.ClientID %>').click(function () {
            showDeleteConfirmation(function (data) {
                var param = 'closevoid;' + data.GCDeleteReason + ';' + data.Reason;
                onCustomButtonClick(param);
            });
        });
        //#endregion

        function setRightPanelButtonEnabled() {
            var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
            if (arInvoiceID != '' && arInvoiceID != null && arInvoiceID != 0) {
                var filterExpression = "ARInvoiceID = " + arInvoiceID;
                Methods.getObject('GetARInvoiceReceivingList', filterExpression, function (result) {
                    if (result != null) {
                        var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
                        if (transactionStatus == "X121^004" || transactionStatus == "X121^005") {
                            $('#btnAlocationRegistration').removeAttr('enabled');
                        }
                        else {
                            $('#btnAlocationRegistration').attr('enabled', 'false');
                        }
                    }
                    else {
                        $('#btnAlocationRegistration').attr('enabled', 'false');
                    }
                });

                $('#btnARNote').removeAttr('enabled');
                $('#btnUpdateDocDate').removeAttr('enabled');
                $('#btninfoReceivingInvoice').removeAttr('enabled');
                $('#btninfoARReceipt').removeAttr('enabled');
                $('#downloadExcelTelkomPerARInvoice').removeAttr('enabled');
            } else {
                $('#btnARNote').attr('enabled', 'false');
                $('#btnAlocationRegistration').attr('enabled', 'false');
                $('#btnUpdateDocDate').attr('enabled', 'false');
                $('#btninfoReceivingInvoice').attr('enabled', 'false');
                $('#btninfoARReceipt').attr('enabled', 'false');
                $('#downloadExcelTelkomPerARInvoice').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnARInvoiceID.ClientID %>').val();
            if (param != "" && param != 0) {
                if (code == "ARNote") {
                    return "5109|" + param;
                } else {
                    return param;
                }
            } else {
                showToast('Failed', 'Maaf, pilih No. Invoice terlebih dahulu.');
                return false;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
            var GCtransaction = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();

            if (arInvoiceID == '' || arInvoiceID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            if (code == 'FN-00078') {
                filterExpression.text = arInvoiceID;
                return true;
            }

            if (code == 'FN-00102' || code == 'FN-00103' || code == 'FN-00062' || code == 'FN-00061'
                    || code == 'FN-00115' || code == 'FN-00116' || code == 'FN-00117' || code == 'FN-00118' || code == 'FN-00119'
                    || code == 'FN-00120' || code == 'FN-00123' || code == 'FN-00148' || code == 'FN-00212' || code == 'FN-00213'
                    || code == 'FN-00220' || code == 'FN-00221' || code == 'FN-00222' || code == 'FN-00262') {
                if (printStatus == 'true') {
                    filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                    return true;
                }
                else {
                    errMessage.text = 'Transaksi harus diapprove terlebih dahulu sebelum print.';
                    return false;
                }

            } else {
                filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                return true;
            }


        }

    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" value="0" id="hdnDateToday" runat="server" />
        <input type="hidden" value="0" id="hdnIsAllowBackDate" runat="server" />
        <input type="hidden" value="0" id="hdnIsAllowFutureDate" runat="server" />
        <input type="hidden" id="hdnARInvoiceID" runat="server" value="0" />
        <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
        <input type="hidden" id="hdnBusinessPartnerVirtualAccountID" runat="server" value="" />
        <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
        <input type="hidden" id="hdnIsEditable" runat="server" value="1" />
        <input type="hidden" id="hdnIsVoidByReason" runat="server" value="0" />
        <input type="hidden" id="hdnPageCount" value="" runat="server" />
        <input type="hidden" id="hdnClaimAmount" runat="server" value="0" />
        <input type="hidden" id="hdnVATAmount" runat="server" value="0" />
        <input type="hidden" id="hdnIsAllowVoidByReason" runat="server" value="" />
        <input type="hidden" id="hdnPrintStatus" runat="server" value="" />
        <input type="hidden" id="hdnIsAllowPrintInvoiceAfterApprove" runat="server" value="0" />
        <input type="hidden" id="hdnIsPpnAllowChanged" runat="server" value="0" />
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col style="width: 40%" />
                <col style="width: 30%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblARInvoiceNo">
                                    <%=GetLabel("Nomor Invoice")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtARInvoiceNo" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Invoice") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtInvoiceDate" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Jatuh Tempo") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtDueDate" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Kirim Invoice") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtDocumentDate" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Bank") %></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBank" runat="server" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblBankVA">
                                    <%=GetLabel("Bank & VA") %></label>
                            </td>
                            <td style="width: 100%">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBankName" Width="100%" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAccountNo" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="left">
                    <table>
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 250px" />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <%=GetLabel("No. Referensi") %>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama UP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRecipientName" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cetak Atas Nama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrintAsName" Width="300px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="right">
                    <table>
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 250px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Total Klaim") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalClaimed" ReadOnly="true" Width="150px" CssClass="txtCurrency"
                                    runat="server" ForeColor="Blue" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Total Diskon") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalDiscount" ReadOnly="true" Width="150px" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Total Penerimaan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalPayment" ReadOnly="true" Width="150px" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Transaksi Non Operasional")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 65%" />
                                    <col style="width: 5%" />
                                    <col style="width: 30%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 180px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblTransactionNonOperational">
                                                        <%=GetLabel("Transaksi Non Operasional")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnTransactionNonOperationalTypeID" runat="server" value="0" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 170px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTransactionNonOperationalTypeCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTransactionNonOperationalTypeName" ReadOnly="true" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Nilai Transaksi")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 170px" />
                                                            <col style="width: 200px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox class="txtTransactionAmount txtCurrency" ID="txtTransactionAmount" Width="100%"
                                                                    runat="server" hiddenVal="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diskon")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 20px" />
                                                            <col style="width: 90px" />
                                                            <col style="width: 150px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="width: 5px">
                                                                <asp:CheckBox ID="chkIsDiscountPercent" Checked="true" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox class="txtDiscountPercentage txtCurrency" ID="txtDiscountPercentage"
                                                                    Width="70%" runat="server" hiddenVal="0" />
                                                                %
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" Width="90%" runat="server"
                                                                    hiddenVal="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("PPN")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 20px" />
                                                            <col style="width: 90px" />
                                                            <col style="width: 150px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="width: 5px">
                                                                <asp:CheckBox ID="chkIsVATAmount" Checked="true" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox class="txtVATPercentage txtCurrency" ID="txtVATPercentage" Width="70%"
                                                                    runat="server" hiddenVal="0" ReadOnly="true" />
                                                                %
                                                            </td>
                                                            <td>
                                                                <asp:TextBox class="txtVATAmount txtCurrency" ID="txtVATAmount" Width="90%" runat="server"
                                                                    hiddenVal="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="text-align: left">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("PPH")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 170px" />
                                                            <col style="width: 200px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <dxe:ASPxComboBox ID="cboPPHType" ClientInstanceName="cboPPHType" runat="server"
                                                                    Width="100%">
                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHTypeValueChanged(e); }" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 20px" />
                                                            <col style="width: 120px" />
                                                            <col style="width: 150px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxComboBox ID="cboPPHOptions" ClientInstanceName="cboPPHOptions" runat="server"
                                                                    Width="90%">
                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHOptionsValueChanged(e); }"
                                                                        SelectedIndexChanged="calculateTotal" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkPPHPercent" Checked="true" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox class="txtPPH txtCurrency" ID="txtPPH" Width="80%" runat="server" hiddenVal="0" />
                                                                %
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPPHPI" CssClass="txtCurrency" Width="80%" runat="server" hiddenVal="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Nilai Claim")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 170px" />
                                                            <col style="width: 200px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtClaimAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblRevenueCostCenter">
                                                        <%=GetLabel("Revenue Cost Center")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnRevenueCostCenterID" runat="server" value="0" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 170px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRevenueCostCenterCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRevenueCostCenterName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemarksDt" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 180px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Amortisasi") %>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 50%" />
                                                            <col style="width: 3px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="width: 5px">
                                                                <asp:CheckBox ID="chkIsAmortization" Checked="false" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trPeriodAmortization" style="display: none" runat="server">
                                                <td>
                                                    <%=GetLabel("Periode Amortisasi") %>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 50%" />
                                                            <col style="width: 3px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtAmortizationPeriodInMonth" runat="server" Width="120px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trAmortizationFirstDate" style="display: none" runat="server">
                                                <td>
                                                    <%=GetLabel("Tanggal Mulai Amortisasi") %>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 50%" />
                                                            <col style="width: 3px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" Width="120px" ID="txtAmortizationFirstDate" CssClass="datepicker" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSave" value='<%= GetLabel("Simpan")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnClose" value='<%= GetLabel("Tutup")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ cbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <input type="hidden" value="0" id="hdnTotalAmount" runat="server" />
                                    <input type="hidden" value="0" id="hdnTotalAmountBeforeDP" runat="server" />
                                    <table class="grdARInvoice grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th>
                                            </th>
                                            <th class="keyField">
                                            </th>
                                            <th align="left" style="width: 170px">
                                                <%=GetLabel("Transaksi Non Operasional") %>
                                            </th>
                                            <th align="left" style="width: 170px">
                                                <%=GetLabel("Cost Revenue Sharing") %>
                                            </th>
                                            <th align="right" style="width: 150px">
                                                <%=GetLabel("Nilai Transaksi") %>
                                            </th>
                                            <th align="right" style="width: 150px">
                                                <%=GetLabel("Diskon Transaksi") %>
                                            </th>
                                            <th align="right" style="width: 150px">
                                                <%=GetLabel("PPN") %>
                                            </th>
                                            <th align="right" style="width: 150px">
                                                <%=GetLabel("Nilai PPH") %>
                                            </th>
                                            <th align="right" style="width: 150px">
                                                <%=GetLabel("Nilai Klaim") %>
                                            </th>
                                            <th align="center" style="width: 150px">
                                                <%=GetLabel("Amortisasi") %>
                                            </th>
                                             <th align="center" style="width: 350px">
                                                <%=GetLabel("Keterangan") %>
                                            </th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>'
                                                                        src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td style="width: 3px;">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                                        src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="keyField">
                                                        <input type="hidden" bindingfield="ID" value='<%#: Eval("ID")%>' />
                                                        <input type="hidden" bindingfield="ARInvoiceID" value='<%#: Eval("ARInvoiceID")%>' />
                                                        <input type="hidden" bindingfield="TransactionNonOperationalTypeID" value='<%#: Eval("TransactionNonOperationalTypeID")%>' />
                                                        <input type="hidden" bindingfield="TransactionNonOperationalTypeCode" value='<%#: Eval("TransactionNonOperationalTypeCode")%>' />
                                                        <input type="hidden" bindingfield="TransactionNonOperationalTypeName" value='<%#: Eval("TransactionNonOperationalTypeName")%>' />
                                                        <input type="hidden" bindingfield="TransactionAmount" value='<%#: Eval("TransactionAmount")%>' />
                                                        <input type="hidden" bindingfield="IsDiscountInPercentage" value='<%#: Eval("IsDiscountInPercentage")%>' />
                                                        <input type="hidden" bindingfield="DiscountPercentage" value='<%#: Eval("DiscountPercentage")%>' />
                                                        <input type="hidden" bindingfield="DiscountAmount" value='<%#: Eval("DiscountAmount")%>' />
                                                        <input type="hidden" bindingfield="VATAmount" value='<%#: Eval("VATAmount")%>' />
                                                        <input type="hidden" bindingfield="PPHPercentage" value='<%#: Eval("PPHPercentage")%>' />
                                                        <input type="hidden" bindingfield="PPHAmount" value='<%#: Eval("PPHAmount")%>' />
                                                        <input type="hidden" bindingfield="GCPPHType" value='<%#: Eval("GCPPHType")%>' />
                                                        <input type="hidden" bindingfield="PPHType" value='<%#: Eval("PPHType")%>' />
                                                        <input type="hidden" bindingfield="PPHMode" value='<%#: Eval("PPHMode")%>' />
                                                        <input type="hidden" bindingfield="ClaimedAmount" value='<%#: Eval("ClaimedAmount")%>' />
                                                        <input type="hidden" bindingfield="RevenueCostCenterID" value='<%#: Eval("RevenueCostCenterID")%>' />
                                                        <input type="hidden" bindingfield="RevenueCostCenterCode" value='<%#: Eval("RevenueCostCenterCode")%>' />
                                                        <input type="hidden" bindingfield="RevenueCostCenterName" value='<%#: Eval("RevenueCostCenterName")%>' />
                                                        <input type="hidden" bindingfield="IsAmortization" value='<%#: Eval("IsAmortization")%>' />
                                                        <input type="hidden" bindingfield="AmortizationPeriodInMonth" value='<%#: Eval("AmortizationPeriodInMonth")%>' />
                                                        <input type="hidden" bindingfield="AmortizationFirstDate" value='<%#: Eval("AmortizationFirstDate")%>' />
                                                        <input type="hidden" bindingfield="cfAmortizationFirstDate" value='<%#: Eval("cfAmortizationFirstDate")%>' />
                                                        <%#: Eval("ARInvoiceID")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("TransactionNonOperationalTypeCode")%>
                                                        <%#:Eval("TransactionNonOperationalTypeName")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("RevenueCostCenterCode")%>
                                                        <%#:Eval("RevenueCostCenterName")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("TransactionAmount","{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("DiscountAmount","{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("VATAmount","{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PPHAmount","{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("ClaimedAmount","{0:N}")%>
                                                    </td>
                                                    <td align="left">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="center" style="width: 100%">
                                                                    <label class="lblAmortization lblLink" style="text-align: center">
                                                                        <%#: Eval("cfIsAmortizationInString")%></label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <label style="font-size: x-small">
                                                            <%=GetLabel("Periode : ") %><%#:Eval("AmortizationPeriodInMonth")%>
                                                            <br />
                                                            <%=GetLabel("Tgl Mulai : ") %><%#:Eval("cfAmortizationFirstDateInString")%>
                                                        </label>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("Remarks")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 200px;">
                            <%= GetLabel("Tambah Data")%>
                        </span>
                    </div>
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
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
        OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
