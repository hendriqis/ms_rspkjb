<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/CustomerPage/MPBaseCustomerPageTrx2.master"
    AutoEventWireup="true" CodeBehind="ARInvoicePayerEditEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerEditEntry" %>

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
            if ($('#<%=hdnTransactionStatus.ClientID %>').val() == "X121^001") {
                $('#btnProcessDiscount').show();

                setDatePicker('<%=txtDocumentDate.ClientID %>');
                setDatePicker('<%=txtARDocumentReceiveDate.ClientID %>');
            } else {
                $('#btnProcessDiscount').hide();
            }

            setRightPanelButtonEnabled();
            setCustomToolbarVisibility();

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            $('#<%=txtLetterNo.ClientID %>').keyup(function () {
                var letterNo = $(this).val();
                if (letterNo.length > 10) {
                    letterNo = letterNo.substring(0, letterNo.length - 1);
                    $(this).val(letterNo);
                    showToast('Warning', 'Maksimal 10 Karakter untuk No Urut Cetak');
                }
            });

            $('#<%=btnVoidByReason.ClientID %>').click(function () {
                showDeleteConfirmation(function (data) {
                    var param = 'justvoid;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
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

            $('#btnProcessDiscount').click(function () {
                var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
                if (arInvoiceID != 0 && arInvoiceID != "" && arInvoiceID != null) {
                    cbpProcessDiscount.PerformCallback();
                } else {
                    showToast('Process Failed', 'Error Message : ' + 'Pilih no. invoice terlebih dahulu.');
                }
            });

        }

        //#region DiscountPercent
        $('#<%:chkIsDiscountPercent.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtEntryDiscountAmount.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtEntryDiscountPercentage.ClientID%>').removeAttr('readonly');
            } else {
                $('#<%=txtEntryDiscountAmount.ClientID%>').removeAttr('readonly');
                $('#<%=txtEntryDiscountPercentage.ClientID%>').attr('readonly', 'readonly');
            }
        });
        //#endregion

        //#region AR Invoice
        function onGetARInvoiceFilterExpression() {
            var filterExpression = "<%:onGetARInvoiceFilterExpression() %>";
            return filterExpression;
        }

        $('#lblARInvoiceNo.lblLink').live('click', function () {
            openSearchDialog('arinvoicehd', onGetARInvoiceFilterExpression(), function (value) {
                $('#<%=txtARInvoiceNo.ClientID %>').val(value);
                onTxtProcessedDateChanged(value);
            });
        });

        $('#<%=txtARInvoiceNo.ClientID %>').live('change', function () {
            onTxtProcessedDateChanged($(this).val());
        });

        function onTxtProcessedDateChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        $('.imgDelete').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Apakah Anda Yakin?', function (result) {
                if (result) {
                    var entity = rowToObject($tr);
                    $('#<%=hdnARInvoiceDtID.ClientID %>').val(entity.ID);
                    $('#<%=hdnRegistrationID.ClientID %>').val(entity.RegistrationID);
                    $('#<%=hdnPaymentID.ClientID %>').val(entity.PaymentID);
                    $('#<%=hdnPaymentDetailID.ClientID %>').val(entity.PaymentDetailID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('.grdARInvoiceHD .txtVarianceAmount').live('change', function () {
            $(this).blur();
            $tr = $(this).closest('tr');
            var transactionAmount = parseFloat($tr.find('.hdnTransactionAmount').val());
            var varianceAmount = parseFloat($(this).attr('hiddenVal'));
            var discountAmount = parseFloat($tr.find('.txtDiscountAmount').attr('hiddenVal'));
            $tr.find('.txtClaimedAmount').val(transactionAmount + varianceAmount - discountAmount).trigger('changeValue');
            $tr.find('.btnSave').removeAttr('enabled');
        });

        $('.grdARInvoiceHD .txtDiscountAmount').live('change', function () {
            $(this).blur();
            $tr = $(this).closest('tr');
            var transactionAmount = parseFloat($tr.find('.hdnTransactionAmount').val());
            var varianceAmount = parseFloat($tr.find('.txtVarianceAmount').attr('hiddenVal'));
            var discountAmount = parseFloat($(this).attr('hiddenVal'));
            $tr.find('.txtClaimedAmount').val(transactionAmount + varianceAmount - discountAmount).trigger('changeValue');
            $tr.find('.btnSave').removeAttr('enabled');
        });

        $btnSave = null;
        $('.btnSave').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                $tr = $(this).closest('tr');
                var entity = rowToObject($tr);
                $('#<%=hdnARInvoiceDtID.ClientID %>').val(entity.ID);
                $('#<%=hdnRegistrationID.ClientID %>').val(entity.RegistrationID);
                $('#<%=hdnPaymentID.ClientID %>').val(entity.PaymentID);
                $('#<%=hdnPaymentDetailID.ClientID %>').val(entity.PaymentDetailID);
                var claimedAmount = $tr.find('.txtClaimedAmount').val();
                var varianceAmount = $tr.find('.txtVarianceAmount').val();
                var discountAmount = $tr.find('.txtDiscountAmount').val();
                var param = 'save|' + claimedAmount + '|' + varianceAmount + '|' + discountAmount;
                $btnSave = $(this);
                cbpProcess.PerformCallback(param);
            }
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                $('#<%=txtTotalTransaction.ClientID %>').val(parseFloat(param[2])).trigger('changeValue');
                $('#<%=txtTotalClaimed.ClientID %>').val(parseFloat(param[3])).trigger('changeValue');
                $('#<%=txtTotalVariance.ClientID %>').val(parseFloat(param[4])).trigger('changeValue');
                $('#<%=txtTotalDiscount.ClientID %>').val(parseFloat(param[5])).trigger('changeValue');
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });

                $('.grdARInvoiceHD .txtCurrency').each(function () {
                    $(this).trigger('changeValue');
                });
            }
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

        $('.lblChangeSEP').die('click');
        $('.lblChangeSEP').live('click', function () {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus == "X121^001") {
                $tr = $(this).closest('tr');
                var entity = rowToObject($tr);
                var isBPJS = entity.IsBPJS;
                if (isBPJS == "True") {
                    var id = entity.RegistrationID;
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/GenerateSEPManualCtl.ascx");
                    openUserControlPopup(url, id, 'Ubah SEP Manual', 700, 300);
                } else {
                    displayErrorMessageBox('Khusus Pasien BPJS', "Maaf, penjamin bayar piutang ini bukan BPJS. Silahkan coba lagi.");
                }
            } else {
                displayErrorMessageBox('GAGAL', "Maaf, sudah tidak dapat mengubah data ini lagi karena status sudah tidak OPEN lagi.");
            }
        });

        $('.lblChangeSJP').die('click');
        $('.lblChangeSJP').live('click', function () {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus == "X121^001") {
                $tr = $(this).closest('tr');
                var entity = rowToObject($tr);
                var isInhealth = entity.IsInhealth;
                if (isInhealth == "True") {
                    var id = entity.RegistrationID;
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/GenerateSJPManualCtl.ascx");
                    openUserControlPopup(url, id, 'Ubah SJP Manual', 700, 300);
                } else {
                    displayErrorMessageBox('Khusus Pasien Inhealth', "Maaf, penjamin bayar piutang ini bukan Inhealth. Silahkan coba lagi.");
                }
            } else {
                displayErrorMessageBox('GAGAL', "Maaf, sudah tidak dapat mengubah data ini lagi karena status sudah tidak OPEN lagi.");
            }
        });

        $('.lblChangeReferral').die('click');
        $('.lblChangeReferral').live('click', function () {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus == "X121^001") {
                $tr = $(this).closest('tr');
                var entity = rowToObject($tr);
                var id = entity.RegistrationID;
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ChangeReferralManualCtl.ascx");
                openUserControlPopup(url, id, 'Ubah Rujukan Manual', 700, 300);
            } else {
                displayErrorMessageBox('GAGAL', "Maaf, sudah tidak dapat mengubah data ini lagi karena status sudah tidak OPEN lagi.");
            }
        });

        $('.lblChangeSequenceNo').die('click');
        $('.lblChangeSequenceNo').live('click', function () {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus == "X121^001") {
                $tr = $(this).closest('tr');
                var entity = rowToObject($tr);
                var id = entity.PaymentDetailID;
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/Payment/ChangeSequenceNoCtl.ascx");
                openUserControlPopup(url, id, 'Ubah No Urut Pembayaran', 700, 300);
            } else {
                displayErrorMessageBox('GAGAL', "Maaf, sudah tidak dapat mengubah data ini lagi karena status sudah tidak OPEN lagi.");
            }
        });

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $tr = $btnSave.closest('tr');
                    $btnSave.attr('enabled', 'false');
                    onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
                }

            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
                }
            }
            hideLoadingPanel();
        }

        function onCbpProcessDiscountEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[1]);
            else {
                showToast('Process Success', 'Proses update seluruh diskon di bawah berhasil dilakukan.');
                onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
            }

            hideLoadingPanel();
        }

        function onAfterPopupControlClosing() {
            onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
        }

        function onAfterSaveAddRecordEntryPopup() {
            cbpView.PerformCallback('refresh');
            onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "closenew") {
                showToast('Re-Insert Success', 'Proses Pembuatan Tagihan Piutang Pasien Instansi Baru Berhasil Dibuat Dengan Nomor <b>' + retval + '</b>', function () {
                    cbpView.PerformCallback('refresh');
                });
                onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
            } else if (type.substring(0, 8) == "justvoid") {
                cbpView.PerformCallback('refresh');
                onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
            } else if (type.substring(0, 9) == "closevoid") {
                cbpView.PerformCallback('refresh');
                onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
            }
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isvoidbyreason = $('#<%=hdnIsVoidByReason.ClientID %>').val();
            var grouperAmountFinal = parseFloat($('#<%=hdnTotalGrouperAmountFinal.ClientID %>').val());
            var isUsedClaimFinal = parseFloat($('#<%=hdnIsUsedClaimFinal.ClientID %>').val());
            var isBPJS = parseFloat($('#<%=hdnIsBPJS.ClientID %>').val());

            if (transactionStatus == 'X121^001') {
                $('#lblAddData').removeAttr('style');

                if (isvoidbyreason == "1") {
                    $('#<%=btnVoidByReason.ClientID %>').show();
                }
                else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                }
            }
            else {
                $('#lblAddData').attr('style', 'display:none');

                $('#<%=btnVoidByReason.ClientID %>').hide();
            }

            if (transactionStatus == 'X121^003') {
                $('#<%=btnCloseNew.ClientID %>').show();

                if (isvoidbyreason == "1") {
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

            if (isBPJS == "1") {
                if (grouperAmountFinal > 0) {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                    $('#<%=btnCloseNew.ClientID %>').hide();
                    $('#<%=btnCloseVoid.ClientID %>').hide();
                }
            }
        }

        function setRightPanelButtonEnabled() {
            var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isBPJS = $('#<%=hdnIsBPJS.ClientID %>').val();

            if (arInvoiceID != '' && arInvoiceID != null) {
                var filterExpression = "ARInvoiceID = " + arInvoiceID;
                Methods.getObject('GetARInvoiceReceivingList', filterExpression, function (result) {
                    if (result != null) {
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
                $('#btnUpdateARDocRecieveDate').removeAttr('enabled');
                $('#btnUpdateARDocRecieveByName').removeAttr('enabled');
                $('#btninfoReceivingInvoice').removeAttr('enabled');
                $('#btninfoARReceipt').removeAttr('enabled');
                $('#downloadExcelTelkomPerARInvoice').removeAttr('enabled');
                $('#downloadReportTagihan').removeAttr('enabled');
                if (isBPJS == "1") {
                    $('#btnProportionalARInvoice').attr('enabled', 'false');
                } else {
                    if (transactionStatus != "X121^001" && transactionStatus != "X121^002") {
                        $('#btnProportionalARInvoice').removeAttr('enabled');
                    } else {
                        $('#btnProportionalARInvoice').attr('enabled', 'false');
                    }
                }

            } else {
                $('#btnARNote').attr('enabled', 'false');
                $('#btnAlocationRegistration').attr('enabled', 'false');
                $('#btnUpdateDocDate').attr('enabled', 'false');
                $('#btnUpdateARDocRecieveDate').attr('enabled', 'false');
                $('#btnUpdateARDocRecieveByName').attr('enabled', 'false');
                $('#btninfoReceivingInvoice').attr('enabled', 'false');
                $('#btninfoARReceipt').attr('enabled', 'false');
                $('#downloadExcelTelkomPerARInvoice').attr('enabled', 'false');
                $('#btnProportionalARInvoice').attr('enabled', 'false');
                $('#downloadReportTagihan').attr('enabled', 'false');
            }
        }

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
                var url = ResolveUrl('~/Program/ARInvoicePayer/ARInvoicePayerEdit/ARInvoicePayerAddRevisionCtl.ascx');
                openUserControlPopup(url, arInvoiceID, 'Tambah Data', 1200, 500);
            }
        });

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'informasiInstansi') {
                var payer = $('#<%:hdnBusinessPartnerID.ClientID %>').val();
                var contract = $('#<%:hdnContractID.ClientID %>').val();
                if (contract != null && contract != "" && contract != "0") {
                    return payer + '|' + contract;
                } else {
                    showToast('Failed', 'Maaf, instansi ini tidak memiliki kontrak.');
                }
            }
            else if (code == 'proportionalARInvoice') {
                var arInvoiceID = $('#<%:hdnARInvoiceID.ClientID %>').val();
                var arInvoiceNo = $('#<%:txtARInvoiceNo.ClientID %>').val();
                return arInvoiceID + "|" + arInvoiceNo;
            }
            else if (code == 'downloadReportTagihan') {
                var arInvoiceID = $('#<%:hdnARInvoiceID.ClientID %>').val();
                var arInvoiceNo = $('#<%:txtARInvoiceNo.ClientID %>').val();
                var menuID = $('#<%:hdnMenuID.ClientID %>').val();  ///  "FN040112"; 
                return arInvoiceID + "|" + arInvoiceNo + "|" + menuID;
            }
            else {
                var param = $('#<%:hdnARInvoiceID.ClientID %>').val();
                if (param != "" && param != 0) {
                    if (code == 'ARNote') {
                        return "5103|" + param;
                    }
                    else {
                        return param;
                    }
                }
                else {
                    showToast('Failed', 'Maaf, pilih No. Invoice terlebih dahulu.');
                    return false;
                }
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
            var gcTransactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();

            if (arInvoiceID == '' || arInvoiceID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            } else {
                if (code == 'FN-00078' || code == 'FN-00218') {
                    filterExpression.text = arInvoiceID;
                    return true;
                }
                else if (code == 'FN-00061' || code == 'FN-00062' || code == 'FN-00206') {
                    filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                    return true;
                }
                else if (code == 'FN-00102' || code == 'FN-00103' || code == 'FN-00061' || code == 'FN-00062'
                            || code == 'FN-00174' || code == 'FN-00175' || code == 'FN-00182' || code == 'FN-00183'
                            || code == 'FN-00189' || code == 'FN-00190' || code == 'FN-00191' || code == 'FN-00192'
                            || code == 'FN-00193' || code == 'FN-00194' || code == 'FN-00195' || code == 'FN-00196'
                            || code == 'FN-00239' || code == 'FN-00180' || code == 'FN-00181' || code == 'FN-00184'
                            || code == 'FN-00261' || code == 'FN-00262' || code == 'FN-00269') {
                    if (printStatus == 'true') {
                        filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                        return true;
                    }
                    else {
                        errMessage.text = 'Transaksi harus diapprove terlebih dahulu sebelum print bisa dilakukan.';
                        return false;
                    }
                } else {
                    filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                    return true;
                }
            }
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnPrintStatus" runat="server" value="" />
        <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
        <input type="hidden" id="hdnContractID" value="" runat="server" />
        <input type="hidden" id="hdnBusinessPartnerVirtualAccountID" runat="server" value="" />
        <input type="hidden" id="hdnIsUsedClaimFinal" runat="server" value="0" />
        <input type="hidden" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" value="" />
        <input type="hidden" id="hdnIsBPJS" runat="server" value="0" />
        <input type="hidden" id="hdnPaymentID" runat="server" value="" />
        <input type="hidden" id="hdnPaymentDetailID" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnARInvoiceDtID" runat="server" />
        <input type="hidden" id="hdnARInvoiceID" value="" runat="server" />
        <input type="hidden" id="hdnGCTransactionStatus" value="" runat="server" />
        <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
        <input type="hidden" id="hdnTotalGrouperAmountClaim" runat="server" value="" />
        <input type="hidden" id="hdnTotalGrouperAmountFinal" runat="server" value="" />
        <input type="hidden" id="hdnIsEditable" runat="server" value="1" />
        <input type="hidden" id="hdnIsVoidByReason" runat="server" value="0" />
        <input type="hidden" id="hdnPageCount" value="" runat="server" />
        <input type="hidden" id="hdnSetvarLeadTime" runat="server" value="0" />
        <input type="hidden" id="hdnSetvarHitungJatuhTempoDari" runat="server" value="0" />
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col style="width: 35%" />
                <col style="width: 35%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td valign="top" align="left">
                    <table>
                        <colgroup>
                            <col style="width: 33%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblARInvoiceNo">
                                    <%=GetLabel("No. Invoice")%></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtARInvoiceNo" Width="150px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Tgl Invoice")%></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" CssClass="datepicker" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Tgl Jatuh Tempo")%></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtDueDate" CssClass="datepicker" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tgl Kirim Invoice") %></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:TextBox runat="server" ID="txtDocumentDate" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tgl Terima Dokumen") %></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:TextBox runat="server" ID="txtARDocumentReceiveDate" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Terima Dokumen Oleh") %></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtARDocumentReceiveByName" Width="165%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Bank") %></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="2">
                                            <dxe:ASPxComboBox ID="cboBank" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblBankVA">
                                    <%=GetLabel("Bank & VA") %></label>
                            </td>
                            <td align="left">
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBankName" Width="100%" ReadOnly="true" />
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
                            <col style="width: 150px" />
                            <col style="width: 300px" />
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
                                <asp:TextBox ID="txtRecipientName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cetak Atas Nama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrintAsName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Urut Cetak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLetterNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="font-style: oblique">
                                <label>
                                    <%=GetLabel("Total Transaksi") %></label>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtTotalTransaction" ReadOnly="true" Width="70%" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="font-style: oblique">
                                <label>
                                    <%=GetLabel("Total Klaim") %></label>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtTotalClaimed" ReadOnly="true" Width="70%" CssClass="txtCurrency"
                                    runat="server" ForeColor="Blue" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="font-style: oblique">
                                <label>
                                    <%=GetLabel("Total Penyesuaian") %></label>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtTotalVariance" ReadOnly="true" Width="70%" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="font-style: oblique">
                                <label>
                                    <%=GetLabel("Total Diskon") %></label>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtTotalDiscount" ReadOnly="true" Width="70%" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="font-style: oblique">
                                <label>
                                    <%=GetLabel("Total Penerimaan") %></label>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtTotalPayment" ReadOnly="true" Width="70%" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="right">
                    <table>
                        <colgroup>
                            <col style="width: 160px" />
                            <col style="width: 10px" />
                            <col style="width: 170px" />
                        </colgroup>
                        <tr>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td style="font-size: large; color: Red; font-style: italic; font-weight: bold" class="blink-alert">
                                            <%=GetLabel("Perhatian")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small; color: Red;">
                                            <%=GetLabel("Proses Diskon akan menimpa semua diskon yang di detail.")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jmlh Diskon Persen")%></label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkIsDiscountPercent" Checked="false" runat="server" />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEntryDiscountPercentage" ReadOnly="true" CssClass="txtCurrency"
                                    runat="server" Style="width: 50px" />
                                <label class="lblNormal">
                                    <%=GetLabel("%")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal" id="lblDiskon">
                                    <%=GetLabel("Jmlh Diskon Nominal")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEntryDiscountAmount" CssClass="txtCurrency" runat="server" hiddenVal="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnProcessDiscount" value="Proses Diskon" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="bottom">
                            <td class="tdLabel" style="font-style: oblique">
                                <label>
                                    <%=GetLabel("Total Registrasi") %></label>
                            </td>
                            <td align="right" colspan="2">
                                <asp:TextBox ID="txtRegistrationCount" ReadOnly="true" Width="100%" CssClass="txtCurrency"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%;
                                        margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                        <table class="grdARInvoiceHD grdSelected" cellspacing="0" width="100%" rules="all">
                                            <tr>
                                                <th>
                                                </th>
                                                <th class="keyField">
                                                </th>
                                                <th align="left" style="width: 150px">
                                                    <%=GetLabel("Informasi Registrasi") %>
                                                </th>
                                                <th align="left" style="width: 150px">
                                                    <%=GetLabel("Informasi Peserta") %>
                                                </th>
                                                <th align="left" style="width: 150px">
                                                    <%=GetLabel("Tgl/No Piutang") %>
                                                </th>
                                                <th align="left" style="width: 100px">
                                                    <%=GetLabel("No Referensi") %>
                                                </th>
                                                <th align="right" style="width: 130px">
                                                    <%=GetLabel("Jmlh Grouper Klaim") %>
                                                </th>
                                                <th align="right" style="width: 130px">
                                                    <%=GetLabel("Jmlh Grouper Final") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Jmlh Transaksi") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Jmlh Klaim") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Jmlh Penyesuaian") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Jmlh Diskon") %>
                                                </th>
                                                <th align="center">
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
                                                            <input type="hidden" bindingfield="PaymentID" value='<%#: Eval("PaymentID")%>' />
                                                            <input type="hidden" bindingfield="PaymentDetailID" value='<%#: Eval("PaymentDetailID")%>' />
                                                            <input type="hidden" bindingfield="RegistrationID" value='<%#: Eval("RegistrationID")%>' />
                                                            <input type="hidden" bindingfield="RegistrationNo" value='<%#: Eval("RegistrationNo")%>' />
                                                            <input type="hidden" bindingfield="MedicalNo" value='<%#: Eval("MedicalNo")%>' />
                                                            <input type="hidden" bindingfield="PatientName" value='<%#: Eval("PatientName")%>' />
                                                            <input type="hidden" bindingfield="IsBPJS" value='<%#: Eval("IsBPJS")%>' />
                                                            <input type="hidden" bindingfield="IsInhealth" value='<%#: Eval("IsInhealth")%>' />
                                                            <input type="hidden" bindingfield="GrouperAmountClaim" value='<%#: Eval("GrouperAmountClaim")%>' />
                                                            <input type="hidden" bindingfield="GrouperAmountFinal" value='<%#: Eval("GrouperAmountFinal")%>' />
                                                            <input type="hidden" bindingfield="TransactionAmount" value='<%#: Eval("TransactionAmount")%>'
                                                                class="hdnTransactionAmount" />
                                                            <input type="hidden" bindingfield="VarianceAmount" value='<%#: Eval("VarianceAmount")%>' />
                                                            <input type="hidden" bindingfield="ClaimedAmount" value='<%#: Eval("ClaimedAmount")%>' />
                                                            <input type="hidden" bindingfield="DiscountAmount" value='<%#: Eval("DiscountAmount")%>' />
                                                            <%#: Eval("ARInvoiceID")%>|<%#:Eval("RegistrationID") %>
                                                        </td>
                                                        <td align="left">
                                                            <div>
                                                                <%#: Eval("RegistrationDateInString")%></div>
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("RegistrationNo") %></b></div>
                                                            <div>
                                                                <%#: Eval("DepartmentID") %>
                                                            </div>
                                                            <div>
                                                                <b>(<%#:Eval("MedicalNo") %>)</b></div>
                                                            <div>
                                                                <%#:Eval("PatientName") %></div>
                                                        </td>
                                                        <td align="left">
                                                            <div>
                                                                <label style="font-size: smaller">
                                                                    <%=GetLabel("No.SEP : ") %></label>
                                                                <label class="lblLink lblChangeSEP">
                                                                    <u>
                                                                        <%#: Eval("NoSEP") == "" ? "(+ SEP)" : Eval("NoSEP")%></u></label>
                                                            </div>
                                                            <div>
                                                                <label style="font-size: smaller">
                                                                    <%=GetLabel("No.SJP : ") %></label>
                                                                <label class="lblLink lblChangeSJP">
                                                                    <u>
                                                                        <%#: Eval("NoSJP") == "" ? "(+ SJP)" : Eval("NoSJP")%></u></label></div>
                                                            <div>
                                                                <label style="font-size: smaller">
                                                                    <%=GetLabel("No.Rujukan : ") %></label>
                                                                <label class="lblLink lblChangeReferral">
                                                                    <u>
                                                                        <%#: Eval("ReferralNo") == "" ? "(+ Rujukan)" : Eval("ReferralNo")%></u></label></div>
                                                            <div>
                                                                <hr style="margin: 0 0 0 0;" />
                                                            </div>
                                                            <div>
                                                                <label style="font-size: x-small">
                                                                    <%=GetLabel("NIK : ") %><%#: Eval("EmployeeCode")%></label>
                                                            </div>
                                                            <div>
                                                                <label style="font-size: x-small">
                                                                    <%=GetLabel("Nama : ") %><%#: Eval("EmployeeName")%></label>
                                                            </div>
                                                        </td>
                                                        <td align="left">
                                                            <div>
                                                                <%#: Eval("PaymentDateInString")%></div>
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("PaymentNo")%></b></div>
                                                            <div>
                                                                <label class="lblLink lblChangeSequenceNo">
                                                                    <%=GetLabel("No. Urut : ") %><%#: Eval("SequenceNo")%></label></div>
                                                            <div>
                                                                <i>
                                                                    <%=GetLabel("Jenis Klaim = ")%></i><%#: Eval("BPJSClaimType") %></div>
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("ReferenceNo") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("GrouperAmountClaim","{0:N}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("GrouperAmountFinal","{0:N}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("TransactionAmount","{0:N}") %>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtClaimedAmount" runat="server" ReadOnly="true" Width="95%" CssClass="txtCurrency txtClaimedAmount"
                                                                Text='<%# Eval("ClaimedAmount","{0:N2}") %>' />
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtVarianceAmount" runat="server" Width="95%" CssClass="txtCurrency txtVarianceAmount"
                                                                Text='<%# Eval("VarianceAmount","{0:N2}") %>' ReadOnly='<%# Eval("GCTransactionStatus").ToString() == "X121^001" ? false : true%>' />
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtDiscountAmount" runat="server" Width="95%" CssClass="txtCurrency txtDiscountAmount"
                                                                Text='<%# Eval("DiscountAmount","{0:N2}") %>' ReadOnly='<%# Eval("GCTransactionStatus").ToString() == "X121^001" ? false : true%>' />
                                                        </td>
                                                        <td align="center">
                                                            <input type="button" <%# IsEditable() == "0" ? "style='display:none'" : ""%> id="btnSave"
                                                                class="btnSave" enabled="false" value="Simpan" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div style="width: 100%; text-align: center">
                            <div style="width: 100%; text-align: center">
                                <span class="lblLink" id="lblAddData" style="margin-right: 300px; margin-left: 300px">
                                    <%= GetLabel("Tambah Data")%></span>
                            </div>
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
                                                    <tr>
                                                        <td align="left">
                                                            <%=GetLabel("Jumlah Revisi") %>
                                                        </td>
                                                        <td align="center">
                                                            :
                                                        </td>
                                                        <td>
                                                            <div runat="server" id="divRevisionCount">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <%=GetLabel("Terakhir Revisi Oleh") %>
                                                        </td>
                                                        <td align="center">
                                                            :
                                                        </td>
                                                        <td>
                                                            <div runat="server" id="divLastRevisionBy">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <%=GetLabel("Terakhir Revisi Pada") %>
                                                        </td>
                                                        <td align="center">
                                                            :
                                                        </td>
                                                        <td>
                                                            <div runat="server" id="divLastRevisionDate">
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
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
        OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessDiscount" ClientInstanceName="cbpProcessDiscount"
        OnCallback="cbpProcessDiscount_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessDiscountEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
