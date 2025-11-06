<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="RequestRealizationCashReceiptEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RequestRealizationCashReceiptEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCustomToolbarVisibility();
            setLabelLinkVisibility();

            if (getIsAdd()) {
                if ($('#<%=hdnMenuType.ClientID %>').val() != "currentDate") {
                    setDatePicker('<%=txtJournalDate.ClientID %>');
                    $('#<%=txtJournalDate.ClientID %>').datepicker('option', 'maxDate', '0');
                    var minDate = parseInt('<%=minDate %>');
                    if (minDate > -1)
                        $('#<%=txtJournalDate.ClientID %>').datepicker('option', 'minDate', '-' + minDate);
                }
            }

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                    var hdnIsReferenceNoMandatory = $('#<%=hdnIsReferenceNoMandatory.ClientID %>').val();
                    var txtReferenceNo = $('#<%=txtReferenceNo.ClientID %>').val();

                    if (hdnIsReferenceNoMandatory == "1" && txtReferenceNo == "") {
                        displayErrorMessageBox('MEDINFRAS', "Maaf, isi terlebih dahulu nomor referensinya.");
                    } else {
                        cbpProcess.PerformCallback('save');
                    }
                }
            });
        }

        function setLabelLinkVisibility() {
            var treasuryType = $('#<%=hdnTreasuryType.ClientID %>').val();
            var treasuryGroup = $('#<%=hdnTreasuryGroup.ClientID %>').val();
            if (treasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && treasuryGroup == Constant.TreasuryGroup.KAS_BON) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    if ($('#<%=hdnID.ClientID %>').val() != '0' && $('#<%=hdnID.ClientID %>').val() != '') {
                        $('#lblAddData').show();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    } else {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    }
                } else {
                    $('#lblAddData').hide();
                    $('#lblCopyRequestCashReceipt').hide();
                    $('#lblCopyPurchaseRequest').hide();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyRequestDirectPurchase').hide();
                    $('#lblCopyRealizationDirectPurchase').hide();

                    $('#<%=trDocument.ClientID %>').removeAttr('style');
                }
            } else if (treasuryType == Constant.TreasuryType.REALISASI_KAS_BON && treasuryGroup == Constant.TreasuryGroup.KAS_BON) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    if ($('#<%=hdnID.ClientID %>').val() != '0' && $('#<%=hdnID.ClientID %>').val() != '') {
                        $('#lblAddData').show();
                        $('#lblCopyRequestCashReceipt').show();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    } else {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    }
                } else {
                    $('#lblAddData').hide();
                    $('#lblCopyRequestCashReceipt').hide();
                    $('#lblCopyPurchaseRequest').hide();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyRequestDirectPurchase').hide();
                    $('#lblCopyRealizationDirectPurchase').hide();

                    $('#<%=trDocument.ClientID %>').removeAttr('style');
                }
            } else if (treasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && treasuryGroup == Constant.TreasuryGroup.SURAT_PERINTAH_KERJA) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    if ($('#<%=hdnID.ClientID %>').val() != '0' && $('#<%=hdnID.ClientID %>').val() != '') {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').show();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    } else {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    }
                } else {
                    $('#lblAddData').hide();
                    $('#lblCopyRequestCashReceipt').hide();
                    $('#lblCopyPurchaseRequest').hide();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyRequestDirectPurchase').hide();
                    $('#lblCopyRealizationDirectPurchase').hide();

                    $('#<%=trDocument.ClientID %>').removeAttr('style');
                }
            } else if (treasuryType == Constant.TreasuryType.REALISASI_KAS_BON && treasuryGroup == Constant.TreasuryGroup.SURAT_PERINTAH_KERJA) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    if ($('#<%=hdnID.ClientID %>').val() != '0' && $('#<%=hdnID.ClientID %>').val() != '') {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').show();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    } else {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    }
                } else {
                    $('#lblAddData').hide();
                    $('#lblCopyRequestCashReceipt').hide();
                    $('#lblCopyPurchaseRequest').hide();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyRequestDirectPurchase').hide();
                    $('#lblCopyRealizationDirectPurchase').hide();

                    $('#<%=trDocument.ClientID %>').removeAttr('style');
                }
            } else if (treasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON && treasuryGroup == Constant.TreasuryGroup.PERMINTAAN_PEMBELIAN_TUNAI) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    if ($('#<%=hdnID.ClientID %>').val() != '0' && $('#<%=hdnID.ClientID %>').val() != '') {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').show();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    } else {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    }
                } else {
                    $('#lblAddData').hide();
                    $('#lblCopyRequestCashReceipt').hide();
                    $('#lblCopyPurchaseRequest').hide();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyRequestDirectPurchase').hide();
                    $('#lblCopyRealizationDirectPurchase').hide();

                    $('#<%=trDocument.ClientID %>').removeAttr('style');
                }
            } else if (treasuryType == Constant.TreasuryType.REALISASI_KAS_BON && treasuryGroup == Constant.TreasuryGroup.REALISASI_PEMBELIAN_TUNAI) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                    if ($('#<%=hdnID.ClientID %>').val() != '0' && $('#<%=hdnID.ClientID %>').val() != '') {
                        $('#lblAddData').show();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').show();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    } else {
                        $('#lblAddData').hide();
                        $('#lblCopyRequestCashReceipt').hide();
                        $('#lblCopyPurchaseRequest').hide();
                        $('#lblCopyPurchaseReceive').hide();
                        $('#lblCopyRequestDirectPurchase').hide();
                        $('#lblCopyRealizationDirectPurchase').hide();

                        $('#<%=trDocument.ClientID %>').removeAttr('style');
                    }
                } else {
                    $('#lblAddData').hide();
                    $('#lblCopyRequestCashReceipt').hide();
                    $('#lblCopyPurchaseRequest').hide();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyRequestDirectPurchase').hide();
                    $('#lblCopyRealizationDirectPurchase').hide();

                    $('#<%=trDocument.ClientID %>').removeAttr('style');
                }
            } else {
                $('#lblAddData').hide();
                $('#lblCopyRequestCashReceipt').hide();
                $('#lblCopyPurchaseRequest').hide();
                $('#lblCopyPurchaseReceive').hide();
                $('#lblCopyRequestDirectPurchase').hide();
                $('#lblCopyRealizationDirectPurchase').hide();

                $('#<%=trDocument.ClientID %>').removeAttr('style');
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

        //#region Add, Edit, Delete
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnEntryID.ClientID %>').val('');

                $('#<%=hdnGLAccountID.ClientID %>').val('');
                $('#<%=txtGLAccountCode.ClientID %>').val('');
                $('#<%=txtGLAccountName.ClientID %>').val('');
                $('#<%=hdnSubLedgerID.ClientID %>').val('');
                $('#<%=hdnSearchDialogTypeName.ClientID %>').val('');
                $('#<%=hdnIDFieldName.ClientID %>').val('');
                $('#<%=hdnCodeFieldName.ClientID %>').val('');
                $('#<%=hdnDisplayFieldName.ClientID %>').val('');
                $('#<%=hdnMethodName.ClientID %>').val('');
                $('#<%=hdnGCBusinessPartnerType.ClientID %>').val('');
                $('#<%=hdnFilterExpression.ClientID %>').val('');

                $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px; display:none");

                $('#<%=txtReferenceNo.ClientID %>').val("");
                $('#<%=txtSaldoReference.ClientID %>').val("0").trigger('changeValue');

                onSubLedgerIDChanged();
                $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                $('#<%=txtSubLedgerDtName.ClientID %>').val('');
                cboHealthcare.SetValue($('#<%=hdnHealthcare.ClientID %>').val());

                $('#<%=hdnRevenueCostCenterID.ClientID %>').val($('#<%=hdnDefaultRevenueCostCenterID.ClientID %>').val());
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val($('#<%=hdnDefaultRevenueCostCenterCode.ClientID %>').val());
                $('#<%=txtRevenueCostCenterName.ClientID %>').val($('#<%=hdnDefaultRevenueCostCenterName.ClientID %>').val());

                $('#<%=hdnDepartmentID.ClientID %>').val($('#<%=hdnDefaultDepartmentID.ClientID %>').val());
                $('#<%=txtDepartmentID.ClientID %>').val($('#<%=hdnDefaultDepartmentID.ClientID %>').val());
                $('#<%=txtDepartmentName.ClientID %>').val($('#<%=hdnDefaultDepartmentName.ClientID %>').val());

                $('#<%=hdnServiceUnitID.ClientID %>').val($('#<%=hdnDefaultServiceUnitID.ClientID %>').val());
                $('#<%=txtServiceUnitCode.ClientID %>').val($('#<%=hdnDefaultServiceUnitCode.ClientID %>').val());
                $('#<%=txtServiceUnitName.ClientID %>').val($('#<%=hdnDefaultServiceUnitName.ClientID %>').val());

                $('#<%=hdnCustomerGroupID.ClientID %>').val($('#<%=hdnDefaultCustomerGroupID.ClientID %>').val());
                $('#<%=txtCustomerGroupCode.ClientID %>').val($('#<%=hdnDefaultCustomerGroupCode.ClientID %>').val());
                $('#<%=txtCustomerGroupName.ClientID %>').val($('#<%=hdnDefaultCustomerGroupName.ClientID %>').val());

                $('#<%=hdnBusinessPartnerID.ClientID %>').val($('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val());
                $('#<%=txtBusinessPartnerCode.ClientID %>').val($('#<%=hdnDefaultBusinessPartnerCode.ClientID %>').val());
                $('#<%=txtBusinessPartnerName.ClientID %>').val($('#<%=hdnDefaultBusinessPartnerName.ClientID %>').val());

                $('#<%=hdnMRN.ClientID %>').val('');
                $('#<%=txtMedicalNo.ClientID %>').val('');
                $('#<%=txtPatientName.ClientID %>').val('');
                $('#<%=txtRemarksDt.ClientID %>').val('');

                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');

                $('#<%=txtVoucherAmount.ClientID %>').val('0').trigger('changeValue');

                if ($('#<%=hdnTreasuryType.ClientID %>').val() == Constant.TreasuryType.PENERIMAAN) {
                    cboPosition.SetValue("K");
                } else {
                    cboPosition.SetValue("D");
                }

                $('#<%=txtDisplayOrder.ClientID %>').val($('#<%=hdnDisplayCount.ClientID %>').val());

                $('#btnGLAccount').attr('enabled', 'false');
                $('#btnSubLedger').attr('enabled', 'false');

                $('#<%=trPatient.ClientID %>').hide();
                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            if (entity.GCTreasuryGroup == Constant.TreasuryGroup.SUPPLIER_PAYMENT) {
            } else {
                $('#<%=hdnEntryID.ClientID %>').val(entity.TransactionDtID);

                $('#<%=hdnGLAccountID.ClientID %>').val(entity.GLAccount);
                $('#<%=txtGLAccountCode.ClientID %>').val(entity.GLAccountNo);
                $('#<%=txtGLAccountName.ClientID %>').val(entity.GLAccountName);

                $('#<%=hdnSubLedgerID.ClientID %>').val(entity.SubLedgerID);
                $('#<%=hdnSearchDialogTypeName.ClientID %>').val(entity.SearchDialogTypeName);
                $('#<%=hdnIDFieldName.ClientID %>').val(entity.IDFieldName);
                $('#<%=hdnCodeFieldName.ClientID %>').val(entity.CodeFieldName);
                $('#<%=hdnDisplayFieldName.ClientID %>').val(entity.DisplayFieldName);
                $('#<%=hdnMethodName.ClientID %>').val(entity.MethodName);
                $('#<%=hdnGCBusinessPartnerType.ClientID %>').val(entity.GCBusinessPartnerType);
                $('#<%=hdnFilterExpression.ClientID %>').val(entity.FilterExpression);
                $('#btnGLAccount').removeAttr('enabled');
                onSubLedgerIDChanged();
                $('#<%=hdnSubLedgerDtID.ClientID %>').val(entity.SubLedger);
                $('#<%=txtSubLedgerDtCode.ClientID %>').val(entity.SubLedgerCode);
                $('#<%=txtSubLedgerDtName.ClientID %>').val(entity.SubLedgerName);
                $('#<%=hdnHealthcare.ClientID %>').val(entity.HealthcareID);
                cboHealthcare.SetValue(entity.HealthcareID);

                $('#<%=hdnRevenueCostCenterID.ClientID %>').val(entity.RevenueCostCenterID);
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val(entity.RevenueCostCenterCode);
                $('#<%=txtRevenueCostCenterName.ClientID %>').val(entity.RevenueCostCenterName);
                ontxtRevenueCostCenterCodeChanged(entity.RevenueCostCenterCode);
                $('#<%=hdnDepartmentID.ClientID %>').val(entity.DepartmentID);
                $('#<%=txtDepartmentID.ClientID %>').val(entity.DepartmentID);
                $('#<%=txtDepartmentName.ClientID %>').val(entity.DepartmentID);
                $('#<%=hdnServiceUnitID.ClientID %>').val(entity.ServiceUnitID);
                $('#<%=txtServiceUnitCode.ClientID %>').val(entity.ServiceUnitCode);
                $('#<%=txtServiceUnitName.ClientID %>').val(entity.ServiceUnitName);

                $('#<%=hdnCustomerGroupID.ClientID %>').val(entity.CustomerGroupID);
                $('#<%=txtCustomerGroupCode.ClientID %>').val(entity.CustomerGroupCode);
                $('#<%=txtCustomerGroupName.ClientID %>').val(entity.CustomerGroupName);

                $('#<%=hdnBusinessPartnerID.ClientID %>').val(entity.BusinessPartnerID);
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(entity.BusinessPartnerCode);
                $('#<%=txtBusinessPartnerName.ClientID %>').val(entity.BusinessPartnerName);

                if (entity.BusinessPartnerID == "1") {
                    $('#<%=trPatient.ClientID %>').show();
                    $('#<%=hdnMRN.ClientID %>').val(entity.MRN);
                    $('#<%=txtMedicalNo.ClientID %>').val(entity.MedicalNo);
                    $('#<%=txtPatientName.ClientID %>').val(entity.PatientName);
                } else {
                    $('#<%=trPatient.ClientID %>').hide();
                    $('#<%=hdnMRN.ClientID %>').val('');
                    $('#<%=txtMedicalNo.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                }

                $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);

                if (entity.DebitAmount != 0) {
                    cboPosition.SetValue("D");
                    $('#<%=txtVoucherAmount.ClientID %>').val(entity.DebitAmount).trigger('changeValue');
                } else {
                    cboPosition.SetValue("K");
                    $('#<%=txtVoucherAmount.ClientID %>').val(entity.CreditAmount).trigger('changeValue');
                }

                $('#<%=txtDisplayOrder.ClientID %>').val(entity.DisplayOrder);
                $('#<%=txtRemarksDt.ClientID %>').val(entity.Remarks);

                if (entity.ReferenceNo != "" && entity.ReferenceNo != null) {
                    $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px;");

                    $('#<%=txtReferenceNo.ClientID %>').val(entity.ReferenceNo);
                    $('#<%=txtSaldoReference.ClientID %>').val(entity.BalanceEND).trigger('changeValue');
                } else {
                    if (entity.IsUsingDocumentControl == "True") {
                        $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px;");

                        $('#<%=txtReferenceNo.ClientID %>').val(entity.ReferenceNo);
                        $('#<%=txtSaldoReference.ClientID %>').val(entity.BalanceEND).trigger('changeValue');
                    } else {
                        $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px; display:none");

                        $('#<%=txtReferenceNo.ClientID %>').val("");
                        $('#<%=txtSaldoReference.ClientID %>').val("0").trigger('changeValue');
                    }
                }
                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.TransactionDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });
        //#endregion

        //#region Akun
        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
            return filterExpression;
        }

        $('#lblGLAccount.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode.ClientID %>').val(value);
                ontxtGLAccountCodeChanged(value);
            });
        });

        $('#<%=txtGLAccountCode.ClientID %>').live('change', function () {
            ontxtGLAccountCodeChanged($(this).val());
        });

        function ontxtGLAccountCodeChanged(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);

                    $('#<%=hdnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                    $('#<%=hdnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                    $('#<%=hdnFilterExpression.ClientID %>').val(result.FilterExpression);
                    $('#<%=hdnIDFieldName.ClientID %>').val(result.IDFieldName);
                    $('#<%=hdnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                    $('#<%=hdnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                    $('#<%=hdnMethodName.ClientID %>').val(result.MethodName);
                    $('#<%=hdnGCBusinessPartnerType.ClientID %>').val(result.GCBusinessPartnerType);

                    if (result.IsUsingDocumentControl == "1") {
                        $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px;");

                        $('#<%=hdnIsReferenceNoMandatory.ClientID %>').val("1");
                        $('#<%=txtReferenceNo.ClientID %>').val("");
                        $('#<%=txtSaldoReference.ClientID %>').val("0").trigger('changeValue');
                    } else {
                        $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px; display:none");

                        $('#<%=hdnIsReferenceNoMandatory.ClientID %>').val("0");
                        $('#<%=txtReferenceNo.ClientID %>').val("");
                        $('#<%=txtSaldoReference.ClientID %>').val("0").trigger('changeValue');
                    }

                    $('#btnGLAccount').removeAttr('enabled');
                }
                else {
                    $('#<%=hdnGLAccountID.ClientID %>').val('');
                    $('#<%=txtGLAccountCode.ClientID %>').val('');
                    $('#<%=txtGLAccountName.ClientID %>').val('');

                    $('#<%=hdnSubLedgerID.ClientID %>').val('');
                    $('#<%=hdnSearchDialogTypeName.ClientID %>').val('');
                    $('#<%=hdnFilterExpression.ClientID %>').val('');
                    $('#<%=hdnIDFieldName.ClientID %>').val('');
                    $('#<%=hdnCodeFieldName.ClientID %>').val('');
                    $('#<%=hdnDisplayFieldName.ClientID %>').val('');
                    $('#<%=hdnMethodName.ClientID %>').val('');
                    $('#<%=hdnGCBusinessPartnerType.ClientID %>').val('');

                    $('#<%=trDocument.ClientID %>').attr("style", "padding-top: 2px; padding-left: 0px; display:none");

                    $('#<%=hdnIsReferenceNoMandatory.ClientID %>').val("0");
                    $('#<%=txtReferenceNo.ClientID %>').val("");
                    $('#<%=txtSaldoReference.ClientID %>').val("0").trigger('changeValue');

                    $('#btnGLAccount').attr('enabled', 'false');
                }
                onSubLedgerIDChanged();
                $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                $('#<%=txtSubLedgerDtName.ClientID %>').val('');
            });
        }

        function onSubLedgerIDChanged() {
            if ($('#<%=hdnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID.ClientID %>').val() == '') {
                $('#<%=lblSubLedgerDt.ClientID %>').attr('class', 'lblDisabled');
                $('#btnSubLedger').attr('enabled', 'false');
                $('#<%=txtSubLedgerDtCode.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=lblSubLedgerDt.ClientID %>').attr('class', 'lblLink lblMandatory');
                $('#<%=txtSubLedgerDtCode.ClientID %>').removeAttr('readonly');
                $('#btnSubLedger').removeAttr('enabled');
            }
        }
        //#endregion

        //#region Sub Akun
        function onGetSubLedgerDtFilterExpression() {
            var filterExpression = $('#<%=hdnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID.ClientID %>').val());
            return filterExpression;
        }

        $('#<%=lblSubLedgerDt.ClientID %>.lblLink').live('click', function () {
            if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
                openSearchDialog($('#<%=hdnSearchDialogTypeName.ClientID %>').val(), onGetSubLedgerDtFilterExpression(), function (value) {
                    $('#<%=txtSubLedgerDtCode.ClientID %>').val(value);
                    ontxtSubLedgerDtCodeChanged(value);
                });
            }
        });

        $('#<%=txtSubLedgerDtCode.ClientID %>').live('change', function () {
            ontxtSubLedgerDtCodeChanged($(this).val());
        });

        function ontxtSubLedgerDtCodeChanged(value) {
            if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
                var filterExpression = onGetSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                Methods.getObject($('#<%=hdnMethodName.ClientID %>').val(), filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSubLedgerDtID.ClientID %>').val(result[$('#<%=hdnIDFieldName.ClientID %>').val()]);
                        $('#<%=txtSubLedgerDtName.ClientID %>').val(result[$('#<%=hdnDisplayFieldName.ClientID %>').val()]);
                    }
                    else {
                        $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtName.ClientID %>').val('');
                    }
                });
            }
        }
        //#endregion

        //#region Department
        function onGetDepartmentFilterExpression() {
            var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsActive = 1";
            return filterExpression;
        }

        $('#<%=lblDepartment.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('departmentakunt', onGetDepartmentFilterExpression(), function (value) {
                $('#<%=txtDepartmentID.ClientID %>').val(value);
                ontxtDepartmentIDChanged(value);
            });
        });

        $('#<%=txtDepartmentID.ClientID %>').live('change', function () {
            var param = $('#<%=txtDepartmentID.ClientID %>').val();
            ontxtDepartmentIDChanged(param);
        });

        function ontxtDepartmentIDChanged(value) {
            var filterExpression = onGetDepartmentFilterExpression() + " AND DepartmentID = '" + value + "'";
            Methods.getObject('GetDepartmentList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnDepartmentID.ClientID %>').val(result.DepartmentID);
                    $('#<%=txtDepartmentName.ClientID %>').val(result.DepartmentName);
                }
                else {
                    $('#<%=hdnDepartmentID.ClientID %>').val('');
                    $('#<%=txtDepartmentID.ClientID %>').val('');
                    $('#<%=txtDepartmentName.ClientID %>').val('');
                }

                checkRevenueCostCenter();
            });
        }
        //#endregion

        //#region Service Unit
        function onGetServiceUnitFilterExpression() {
            var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('serviceunitakunt', onGetServiceUnitFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                ontxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            var param = $('#<%=txtServiceUnitCode.ClientID %>').val();
            ontxtServiceUnitCodeChanged(param);
        });

        function ontxtServiceUnitCodeChanged(value) {
            var filterExpression = onGetServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetServiceUnitMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }

                checkRevenueCostCenter();
            });
        }
        //#endregion

        function checkRevenueCostCenter() {
            var filter = "IsDeleted = 0";
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var serviceUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
            var healthcareServiceUnitID = "0";

            if (departmentID != "" && serviceUnitID != "" && serviceUnitID != "0") {
                var filterHSU = "IsDeleted = 0";
                if (departmentID != "") {
                    filterHSU += " AND DepartmentID = '" + departmentID + "'";
                }
                if (serviceUnitID != "" && serviceUnitID != "0") {
                    filterHSU += " AND ServiceUnitID = " + serviceUnitID;
                }

                Methods.getObject('GetvHealthcareServiceUnitList', filterHSU, function (result) {
                    if (result != null) {
                        healthcareServiceUnitID = result.HealthcareServiceUnitID;
                    }
                });
            }

            filter += " AND HealthcareServiceUnitID = " + healthcareServiceUnitID;

            Methods.getObject('GetvRevenueCostCenterDtList', filter, function (resultRCC) {
                if (resultRCC != null) {
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val(resultRCC.RevenueCostCenterID);
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val(resultRCC.RevenueCostCenterCode);
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val(resultRCC.RevenueCostCenterName);
                }
                else {
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val('');

                }
            });
        }

        //#region Revenue Cost Center
        function onRevenueCostCenterFilterExpression() {
            var filter = "IsDeleted = 0";

            return filter;
        }

        $('#lblRevenueCostCenter.lblLink').live('click', function () {
            openSearchDialog('vrevenuecostcenter', onRevenueCostCenterFilterExpression(), function (value) {
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val(value);
                ontxtRevenueCostCenterCodeChanged(value);
            });
        });

        $('#<%=txtRevenueCostCenterCode.ClientID %>').live('change', function () {
            var param = $('#<%=txtRevenueCostCenterCode.ClientID %>').val();
            ontxtRevenueCostCenterCodeChanged(param);
        });

        function ontxtRevenueCostCenterCodeChanged(value) {
            var filterExpression = "RevenueCostCenterCode = '" + $('#<%=txtRevenueCostCenterCode.ClientID %>').val() + "'";
            Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);

                    var filterRCCDT = "RevenueCostCenterID = " + result.RevenueCostCenterID + " AND IsDeleted = 0";
                    Methods.getObject('GetvRevenueCostCenterDtList', filterRCCDT, function (resultDt) {
                        if (resultDt != null) {
                            $('#<%=hdnRevenueCostCenterID.ClientID %>').val(resultDt.RevenueCostCenterID);
                            $('#<%=txtRevenueCostCenterName.ClientID %>').val(resultDt.RevenueCostCenterName);

                            var filterDept = "IsActive = 1 AND DepartmentID = '" + resultDt.DepartmentID + "'";
                            Methods.getObject('GetDepartmentList', filterDept, function (resultDept) {
                                if (resultDept != null) {
                                    $('#<%=hdnDepartmentID.ClientID %>').val(resultDept.DepartmentID);
                                    $('#<%=txtDepartmentID.ClientID %>').val(resultDept.DepartmentID);
                                    $('#<%=txtDepartmentName.ClientID %>').val(resultDept.DepartmentName);
                                }
                            });

                            var filterSrvUnit = "IsDeleted = 0 AND ServiceUnitCode = '" + resultDt.ServiceUnitCode + "'";
                            Methods.getObject('GetServiceUnitMasterList', filterSrvUnit, function (resultSrvUnit) {
                                if (resultSrvUnit != null) {
                                    $('#<%=hdnServiceUnitID.ClientID %>').val(resultSrvUnit.ServiceUnitID);
                                    $('#<%=txtServiceUnitCode.ClientID %>').val(resultSrvUnit.ServiceUnitCode);
                                    $('#<%=txtServiceUnitName.ClientID %>').val(resultSrvUnit.ServiceUnitName);
                                }
                            });
                        }
                    });
                }
                else {
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val('');

                }
            });
        }
        //#endregion

        //#region Customer Group
        $('#lblCustomerGroup.lblLink').live('click', function () {
            openSearchDialog('customergroup', 'IsDeleted = 0', function (value) {
                $('#<%=txtCustomerGroupCode.ClientID %>').val(value);
                ontxtCustomerGroupCodeChanged(value);
            });
        });

        $('#<%=txtCustomerGroupCode.ClientID %>').live('change', function () {
            var param = $('#<%=txtCustomerGroupCode.ClientID %>').val();
            ontxtCustomerGroupCodeChanged(param);
        });

        function ontxtCustomerGroupCodeChanged(value) {
            var filterExpression = "CustomerGroupCode = '" + $('#<%=txtCustomerGroupCode.ClientID %>').val() + "'";
            Methods.getObject('GetCustomerGroupList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnCustomerGroupID.ClientID %>').val(result.CustomerGroupID);
                    $('#<%=txtCustomerGroupName.ClientID %>').val(result.CustomerGroupName);
                }
                else {
                    $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                    $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                    $('#<%=txtCustomerGroupName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Business Partner
        function onGetBusinessPartnerFilterExpression() {
            var filterExpression = "IsDeleted = 0";

            if ($('#<%=hdnCustomerGroupID.ClientID %>').val() != "" && $('#<%=hdnCustomerGroupID.ClientID %>').val() != "0") {
                filterExpression += " AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE CustomerGroupID = " + $('#<%=hdnCustomerGroupID.ClientID %>').val() + ")";
            }

            if ($('#<%=hdnGCBusinessPartnerType.ClientID %>').val() != "" && $('#<%=hdnGCBusinessPartnerType.ClientID %>').val() != "") {
                filterExpression += " AND GCBusinessPartnerType = '" + $('#<%=hdnGCBusinessPartnerType.ClientID %>').val() + "'";
            }

            return filterExpression;
        }

        $('#lblBusinessPartner.lblLink').live('click', function () {
            openSearchDialog('businesspartnersakun', onGetBusinessPartnerFilterExpression(), function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                ontxtBusinessPartnerCodeChanged(value);
            });
        });

        $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
            var param = $('#<%=txtBusinessPartnerCode.ClientID %>').val();
            ontxtBusinessPartnerCodeChanged(param);
        });

        function ontxtBusinessPartnerCodeChanged(value) {
            var filterExpression = onGetBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + $('#<%=txtBusinessPartnerCode.ClientID %>').val() + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);

                    if (result.BusinessPartnerID == "1") {
                        $('#<%=trPatient.ClientID %>').show();
                        $('#<%=hdnMRN.ClientID %>').val('');
                        $('#<%=txtMedicalNo.ClientID %>').val('');
                        $('#<%=txtPatientName.ClientID %>').val('');
                    } else {
                        $('#<%=trPatient.ClientID %>').hide();
                        $('#<%=hdnMRN.ClientID %>').val('');
                        $('#<%=txtMedicalNo.ClientID %>').val('');
                        $('#<%=txtPatientName.ClientID %>').val('');

                        if ($('#<%=hdnCustomerGroupID.ClientID %>').val() == "" || $('#<%=hdnCustomerGroupID.ClientID %>').val() == "0") {
                            var filterCust = "CustomerGroupID = (SELECT CustomerGroupID FROM Customer WHERE BusinessPartnerID = " + result.BusinessPartnerID + ")";
                            Methods.getObject('GetCustomerGroupList', filterCust, function (resultCust) {
                                if (resultCust != null) {
                                    $('#<%=hdnCustomerGroupID.ClientID %>').val(resultCust.CustomerGroupID);
                                    $('#<%=txtCustomerGroupCode.ClientID %>').val(resultCust.CustomerGroupCode);
                                    $('#<%=txtCustomerGroupName.ClientID %>').val(resultCust.CustomerGroupName);
                                }
                                else {
                                    $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                                    $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                                    $('#<%=txtCustomerGroupName.ClientID %>').val('');
                                }
                            });
                        }
                    }
                }
                else {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');

                    $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                    $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                    $('#<%=txtCustomerGroupName.ClientID %>').val('');

                    $('#<%=trPatient.ClientID %>').hide();
                    $('#<%=hdnMRN.ClientID %>').val('');
                    $('#<%=txtMedicalNo.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Paramedic Master
        function onGetParamedicMasterFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblParamedicMaster.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetParamedicMasterFilterExpression(), function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                ontxtParamedicMasterCodeChanged(value);
            });
        });

        $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
            var param = $('#<%=txtParamedicCode.ClientID %>').val();
            ontxtParamedicMasterCodeChanged(param);
        });

        function ontxtParamedicMasterCodeChanged(value) {
            var filterExpression = onGetParamedicMasterFilterExpression() + " AND ParamedicCode = '" + $('#<%=txtParamedicCode.ClientID %>').val() + "'";
            Methods.getObject('GetvParamedicMastersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnParamedicID.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Reference
        function onGetDocumentFilterExpression() {
            var coa = $('#<%=hdnGLAccountID.ClientID %>').val();
            if (coa == null || coa == "") {
                coa = 0;
            }

            var filterExpression = "BalanceEND != 0 AND ReferenceNo IS NOT NULL AND ReferenceNo != '' AND GLAccount = " + coa;
            return filterExpression;
        }

        $('#lblDocument').live('click', function () {
            if ($('#<%=hdnGLAccountID.ClientID %>').val() != null && $('#<%=hdnGLAccountID.ClientID %>').val() != '' && $('#<%=hdnGLAccountID.ClientID %>').val() != '0') {
                openSearchDialog('glbalancedtdocument', onGetDocumentFilterExpression(), function (value) {
                    $('#<%=txtReferenceNo.ClientID %>').val(value);
                    onTxtReferenceNoChanged(value);
                });
            } else {
                displayErrorMessageBox('ERROR', "Pilih COA terlebih dahulu.");
            }
        });

        $('#<%=txtReferenceNo.ClientID %>').live('change', function () {
            onTxtReferenceNoChanged($(this).val());
        });

        function onTxtReferenceNoChanged(value) {
            var filterExpression = onGetDocumentFilterExpression() + " AND ReferenceNo = '" + value + "'";
            Methods.getObject('GetvGLBalanceDtDocumentPerReferenceNoList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
                    $('#<%=txtSaldoReference.ClientID %>').val(result.BalanceEND).trigger('changeValue');
                    $('#<%=txtVoucherAmount.ClientID %>').val(result.BalanceEND).trigger('changeValue');
                }
            });
        }

        //#endregion

        function onLoadCurrentRecord() {
            onLoadObject($('#<%=txtJournalNo.ClientID %>').val());
        }

        function onAfterSaveRecordDtSuccess(GLTransactionID) {
            if ($('#<%=hdnID.ClientID %>').val() == '' || $('#<%=hdnID.ClientID %>').val() == '0') {
                $('#<%=hdnID.ClientID %>').val(GLTransactionID);
                var filterExpression = 'GLTransactionID = ' + GLTransactionID;
                Methods.getObject('GetGLTransactionHdList', filterExpression, function (result) {
                    $('#<%=tdTransactionNoAdd.ClientID %>').attr('style', 'display:none');
                    $('#<%=tdTransactionNoEdit.ClientID %>').removeAttr('style');
                    $('#<%=txtJournalNo.ClientID %>').val(result.JournalNo);
                    cboTransactionCode.SetEnabled(false);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            } else {
                cbpView.PerformCallback('refresh');
            }
        }

        var isAfterAdd = false;
        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    isAfterAdd = true;
                    var GLTransactionID = s.cpGLTransactionID;
                    onAfterSaveRecordDtSuccess(GLTransactionID);
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    isAfterAdd = false;
                    cbpView.PerformCallback('refresh');
                }
            }
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=txtTotalDebit.ClientID %>').val(param[1]).trigger('changeValue');
                $('#<%=txtTotalKredit.ClientID %>').val(param[2]).trigger('changeValue');
                $('#<%=txtTotalSelisih.ClientID %>').val(param[3]).trigger('changeValue');

                if (isAfterAdd)
                    $('#lblAddData').click();
            }
        }
        //#endregion

        //#region Journal No
        $('#lblJournalNo.lblLink').live('click', function () {
            var filterExpression = "GCTreasuryGroup IS NOT NULL AND TransactionCode BETWEEN '7281' AND '7300'";

            openSearchDialog('cashreceipthd', filterExpression, function (value) {
                $('#<%=txtJournalNo.ClientID %>').val(value);
                onTxtJournalNoChanged(value);
            });
        });

        $('#<%=txtJournalNo.ClientID %>').live('change', function () {
            onTxtJournalNoChanged($(this).val());
        });

        function onTxtJournalNoChanged(value) {
            onLoadObject(value);

        }
        //#endregion

        function onCboTransactionCodeValueChanged(s) {
            var value = s.GetValue();
            var filterExpression = "TransactionCode = '" + value + "'";
            Methods.getObject('GetTransactionTypeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtJournalPrefix.ClientID %>').val(result.TransactionInitial);
                else
                    $('#<%=txtJournalPrefix.ClientID %>').val('');
            });
        }

        function onCboTreasuryTypeChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnTreasuryType.ClientID %>').val(value);
            setLabelLinkVisibility();
        }

        function onCboTreasuryGroupChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnTreasuryGroup.ClientID %>').val(value);
            setLabelLinkVisibility();
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            cbpView.PerformCallback('refresh');
        }

        $('#btnGLAccount').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var accountID = $('#<%=hdnGLAccountID.ClientID %>').val();
                var url = ResolveUrl('~/Program/Information/FNGLBalanceInformationCtl.ascx');
                var id = accountID;
                var date = $('#<%=txtJournalDate.ClientID %>').val().split('-');
                var period = date[2] + '|' + date[1];
                var status = "1";
                var param = id + '|' + period + '|' + status;
                openUserControlPopup(url, param, 'Informasi Mutasi Detail COA', 1000, 600);
            }
        });

        $('#btnSubLedger').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var subLedgerDtID = $('#<%=hdnSubLedgerDtID.ClientID %>').val();
                var glAccountID = $('#<%=hdnGLAccountID.ClientID %>').val();
                var url = ResolveUrl('~/Program/Information/GLSubLedgerInformationCtl.ascx');
                var code = $('#<%=txtSubLedgerDtCode.ClientID %>').val();
                var name = $('#<%=txtSubLedgerDtName.ClientID %>').val();
                var date = $('#<%=txtJournalDate.ClientID %>').val().split('-');
                var period = date[2] + '|' + date[1];
                var status = "1";
                var param = glAccountID + '|' + subLedgerDtID + '|' + period + '|' + code + '|' + name + '|' + status;
                openUserControlPopup(url, param, 'Detail Sub Akun', 1000, 600);
            }
        });

        $('#btnReference').live('click', function () {
            if ($('#<%=txtReferenceNo.ClientID %>').val() != '') {
                var url = ResolveUrl('~/Program/Treasury/TreasuryTransaction/JournalDocumentCtl.ascx');
                var referenceNo = $('#<%=txtReferenceNo.ClientID %>').val();
                var glAccount = $('#<%=hdnGLAccountID.ClientID %>').val();
                var subLedgerDt = "0";
                if ($('#<%=hdnSubLedgerDtID.ClientID %>').val() != "" && $('#<%=hdnSubLedgerDtID.ClientID %>').val() != "0") {
                    subLedgerDt = $('#<%=hdnSubLedgerDtID.ClientID %>').val();
                }
                var param = glAccount + '|' + subLedgerDt + '|' + referenceNo;
                openUserControlPopup(url, param, 'Detail No. Referensi', 1000, 600);
            }
        });

        function onCboHealthcareValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnHealthcare.ClientID %>').val(value);
        }

        //#region Copy Request Cash Receipt
        $('#lblCopyRequestCashReceipt').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                if (glTransactionID != "0" && glTransactionID != "") {
                    var coaKasBon = $('#<%=hdnCOAKasBon.ClientID %>').val();
                    if (coaKasBon != "0" && coaKasBon != "") {
                        var treasuryType = $('#<%=hdnTreasuryType.ClientID %>').val();
                        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        var srvUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                        var bpID = $('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val();
                        var param = glTransactionID + "|" + treasuryType + "|" + deptID + "|" + srvUnitID + "|" + bpID + "|" + coaKasBon;
                        var url = ResolveUrl('~/Program/Treasury/CashReceipt/CopyRequestCashReceipt.ascx');
                        openUserControlPopup(url, param, 'Copy Permintaan Kas Bon', 1200, 600);
                    } else {
                        displayErrorMessageBox('WARNING', "COA Kas Bon (kdcoa_kas_bon) belum di-setting di L1 - General Sistem Perkiraan !");
                        hideLoadingPanel();
                    }
                } else {
                    displayErrorMessageBox('WARNING', "Harap simpan dahulu agar mendapat nomor vouchernya !");
                    hideLoadingPanel();
                }
            }
        });
        //#endregion

        //#region Copy Purchase Request
        $('#lblCopyPurchaseRequest').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                if (glTransactionID != "0" && glTransactionID != "") {
                    var coaPermintaanSPK = $('#<%=hdnCOAPermintaanSPK.ClientID %>').val();
                    if (coaPermintaanSPK != "0" && coaPermintaanSPK != "") {
                        var treasuryType = $('#<%=hdnTreasuryType.ClientID %>').val();
                        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        var srvUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                        var bpID = $('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val();
                        var param = glTransactionID + "|" + treasuryType + "|" + deptID + "|" + srvUnitID + "|" + bpID + "|" + coaPermintaanSPK;
                        var url = ResolveUrl('~/Program/Treasury/CashReceipt/CopyPurchaseRequestCtl.ascx');
                        openUserControlPopup(url, param, 'Copy Permintaan Pembelian', 1200, 600);
                    } else {
                        displayErrorMessageBox('WARNING', "COA Permintaan SPK (kdcoa_permintaan_spk) belum di-setting di L1 - General Sistem Perkiraan !");
                        hideLoadingPanel();
                    }
                } else {
                    displayErrorMessageBox('WARNING', "Harap simpan dahulu agar mendapat nomor vouchernya !");
                    hideLoadingPanel();
                }
            }
        });
        //#endregion

        //#region Copy Purchase Receive
        $('#lblCopyPurchaseReceive').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                if (glTransactionID != "0" && glTransactionID != "") {
                    var coaRealisasiSPK = $('#<%=hdnCOARealisasiSPK.ClientID %>').val();
                    if (coaRealisasiSPK != "0" && coaRealisasiSPK != "") {
                        var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                        var treasuryType = $('#<%=hdnTreasuryType.ClientID %>').val();
                        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        var srvUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                        var bpID = $('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val();
                        var param = glTransactionID + "|" + treasuryType + "|" + deptID + "|" + srvUnitID + "|" + bpID + "|" + coaRealisasiSPK;
                        var url = ResolveUrl('~/Program/Treasury/CashReceipt/CopyPurchaseReceiveCtl.ascx');
                        openUserControlPopup(url, param, 'Copy Penerimaan Pembelian', 1200, 600);
                    } else {
                        displayErrorMessageBox('WARNING', "COA Realisasi SPK  (kdcoa_realisasi_spk) belum di-setting di L1 - General Sistem Perkiraan !");
                        hideLoadingPanel();
                    }
                } else {
                    displayErrorMessageBox('WARNING', "Harap simpan dahulu agar mendapat nomor vouchernya !");
                    hideLoadingPanel();
                }
            }
        });
        //#endregion

        //#region Copy Request Direct Purchase
        $('#lblCopyRequestDirectPurchase').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                if (glTransactionID != "0" && glTransactionID != "") {
                    var coaDirectPurchase = $('#<%=hdnCOADirectPurchase.ClientID %>').val();
                    if (coaDirectPurchase != "0" && coaDirectPurchase != "") {
                        var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                        var treasuryType = $('#<%=hdnTreasuryType.ClientID %>').val();
                        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        var srvUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                        var bpID = $('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val();
                        var param = glTransactionID + "|" + treasuryType + "|" + deptID + "|" + srvUnitID + "|" + bpID + "|" + coaDirectPurchase;
                        var url = ResolveUrl('~/Program/Treasury/CashReceipt/CopyRequestDirectPurchaseCtl.ascx');
                        openUserControlPopup(url, param, 'Copy Permintaan Pembelian Tunai', 1200, 600);
                    } else {
                        displayErrorMessageBox('WARNING', "COA Pembelian Tunai (kdcoa_pembelian_cash) belum di-setting di L1 - General Sistem Perkiraan !");
                        hideLoadingPanel();
                    }
                } else {
                    displayErrorMessageBox('WARNING', "Harap simpan dahulu agar mendapat nomor vouchernya !");
                    hideLoadingPanel();
                }
            }
        });
        //#endregion

        //#region Copy Realization Direct Purchase
        $('#lblCopyRealizationDirectPurchase').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                if (glTransactionID != "0" && glTransactionID != "") {
                    var coaDirectPurchase = $('#<%=hdnCOADirectPurchase.ClientID %>').val();
                    if (coaDirectPurchase != "0" && coaDirectPurchase != "") {
                        var glTransactionID = $('#<%=hdnID.ClientID %>').val();
                        var treasuryType = $('#<%=hdnTreasuryType.ClientID %>').val();
                        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        var srvUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                        var bpID = $('#<%=hdnDefaultBusinessPartnerID.ClientID %>').val();
                        var param = glTransactionID + "|" + treasuryType + "|" + deptID + "|" + srvUnitID + "|" + bpID + "|" + coaDirectPurchase;
                        var url = ResolveUrl('~/Program/Treasury/CashReceipt/CopyRealizationDirectPurchaseCtl.ascx');
                        openUserControlPopup(url, param, 'Copy Realisasi Pembelian Tunai', 1200, 600);
                    } else {
                        displayErrorMessageBox('WARNING', "COA Pembelian Tunai (kdcoa_pembelian_cash) belum di-setting di L1 - General Sistem Perkiraan !");
                        hideLoadingPanel();
                    }
                } else {
                    displayErrorMessageBox('WARNING', "Harap simpan dahulu agar mendapat nomor vouchernya !");
                    hideLoadingPanel();
                }
            }
        });
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnID.ClientID %>').val();

            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Pilih Voucher Terlebih Dahulu!';
                return false;
            }
            else {
                if (code == "FN-00053" || code == "FN-00055" || code == "FN-00209" || code == "FN-00211") {
                    filterExpression.text = 'GLTransactionID = ' + transactionID;
                    return true;
                } else {
                    var status = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
                    if (status == "<%=GetGCTransactionStatusOpen() %>") {
                        errMessage.text = 'Silahkan Approve Voucher Terlebih Dahulu!';
                        return false;
                    } else {
                        if (code == "FN-00020" || code == "FN-00051" || code == "FN-00049" || code == "FN-00050" || code == "FN-00054" || 
                            code == "FN-00186" || code == "FN-00187" || code == "FN-00210") {
                            filterExpression.text = 'GLTransactionID = ' + transactionID;
                            return true;
                        } else {
                            errMessage.text = 'ReportCode tidak ditemukan.';
                            return false;
                        }
                    }
                }
            }
        }
    </script>
    <input type="hidden" id="hdnMenuType" runat="server" value="" />
    <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnLastPostingDate" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" id="hdnRecordFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowVoidByReason" runat="server" value="" />
    <input type="hidden" id="hdnTreasuryType" runat="server" value="" />
    <input type="hidden" id="hdnTreasuryGroup" runat="server" value="" />
    <input type="hidden" id="hdnCOADirectPurchase" runat="server" value="" />
    <input type="hidden" id="hdnCOAPermintaanSPK" runat="server" value="" />
    <input type="hidden" id="hdnCOARealisasiSPK" runat="server" value="" />
    <input type="hidden" id="hdnCOAKasBon" runat="server" value="" />
    <input type="hidden" id="hdnDefaultDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultDepartmentName" runat="server" value="" />
    <input type="hidden" id="hdnDefaultServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultServiceUnitCode" runat="server" value="" />
    <input type="hidden" id="hdnDefaultServiceUnitName" runat="server" value="" />
    <input type="hidden" id="hdnDefaultRevenueCostCenterID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultRevenueCostCenterCode" runat="server" value="" />
    <input type="hidden" id="hdnDefaultRevenueCostCenterName" runat="server" value="" />
    <input type="hidden" id="hdnDefaultCustomerGroupID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultCustomerGroupCode" runat="server" value="" />
    <input type="hidden" id="hdnDefaultCustomerGroupName" runat="server" value="" />
    <input type="hidden" id="hdnDefaultBusinessPartnerID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultBusinessPartnerCode" runat="server" value="" />
    <input type="hidden" id="hdnDefaultBusinessPartnerName" runat="server" value="" />
    <input type="hidden" id="hdnGCBusinessPartnerType" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 120px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Sumber Data") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTransactionCode" ClientInstanceName="cboTransactionCode"
                                Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboTransactionCodeValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" id="lblJournalNo">
                                <%=GetLabel("Nomor Voucher") %></label>
                        </td>
                        <td id="tdTransactionNoAdd" runat="server">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtJournalPrefix" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJournalNo1" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="display: none;" id="tdTransactionNoEdit" runat="server">
                            <asp:TextBox runat="server" ID="txtJournalNo" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Voucher") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtJournalDate" CssClass="datepicker" Width="130px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Transaksi") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTreasuryType" ClientInstanceName="cboTreasuryType" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboTreasuryTypeChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Sumber Transaksi") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTreasuryGroup" ClientInstanceName="cboTreasuryGroup" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboTreasuryGroupChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="width: 150px; vertical-align: top; padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Keterangan Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr style="padding-top: 2px; padding-left: 0px">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="padding-top: 2px; padding-left: 0px">
                        <td>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <div class="lblComponent" style="text-align: right; padding-right: 5px">
                                            <%=GetLabel("Total Debet") %></div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalDebit" runat="server" CssClass="txtCurrency" Width="200px"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="padding-top: 2px; padding-left: 0px">
                        <td>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <div class="lblComponent" style="text-align: right; padding-right: 5px">
                                            <%=GetLabel("Total Kredit") %></div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalKredit" runat="server" CssClass="txtCurrency" Width="200px"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="padding-top: 2px; padding-left: 0px">
                        <td>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <div class="lblComponent" style="text-align: right; padding-right: 5px">
                                            <%=GetLabel("Total Selisih") %></div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalSelisih" runat="server" CssClass="txtCurrency" Width="200px"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="containerEntry" style="margin-top: 4px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Edit atau Tambah Data")%></div>
                    <fieldset id="fsTrxPopup" style="margin: 0">
                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                        <table style="width: 100%" class="tblEntryDetail">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td style="vertical-align: top">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 140px" />
                                            <col />
                                        </colgroup>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblMandatory" id="lblHealthcare">
                                                    <%=GetLabel("Healthcare")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboHealthcare" ClientInstanceName="cboHealthcare" Width="100%"
                                                    runat="server">
                                                    <ClientSideEvents ValueChanged="function(s,e){ onCboHealthcareValueChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                                <input type="hidden" id="hdnHealthcare" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblGLAccount">
                                                    <%=GetLabel("COA")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnGLAccountID" runat="server" />
                                                <input type="hidden" id="hdnSubLedgerID" runat="server" />
                                                <input type="hidden" id="hdnSearchDialogTypeName" runat="server" />
                                                <input type="hidden" id="hdnIDFieldName" runat="server" />
                                                <input type="hidden" id="hdnCodeFieldName" runat="server" />
                                                <input type="hidden" id="hdnDisplayFieldName" runat="server" />
                                                <input type="hidden" id="hdnIsReferenceNoMandatory" runat="server" value="0" />
                                                <input type="hidden" id="hdnMethodName" runat="server" />
                                                <input type="hidden" id="hdnFilterExpression" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtGLAccountCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtGLAccountName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnGLAccount" value="..." />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px; display: none;">
                                            <td class="tdLabel">
                                                <label class="ldlDisabled" runat="server" id="lblSubLedgerDt">
                                                    <%=GetLabel("Sub Akun")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnSubLedgerDtID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtSubLedgerDtCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtSubLedgerDtName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnSubLedger" value="..." />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblDepartment" runat="server">
                                                    <%=GetLabel("Department")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnDepartmentID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDepartmentID" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDepartmentName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblServiceUnit" runat="server">
                                                    <%=GetLabel("Service Unit")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnServiceUnitID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtServiceUnitCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtServiceUnitName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblRevenueCostCenter">
                                                    <%=GetLabel("Revenue Cost Center")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnRevenueCostCenterID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtRevenueCostCenterCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtRevenueCostCenterName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblCustomerGroup">
                                                    <%=GetLabel("Customer Group")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnCustomerGroupID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCustomerGroupCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCustomerGroupName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblBusinessPartner">
                                                    <%=GetLabel("Business Partner")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnBusinessPartnerID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trPatient" class="trPatient" runat="server" style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblMRN">
                                                    <%=GetLabel("Patient")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnMRN" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtMedicalNo" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtPatientName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblLink lblNormal" id="lblParamedicMaster" runat="server">
                                                    <%=GetLabel("Paramedic Master")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnParamedicID" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtParamedicCode" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtParamedicName" ReadOnly="true" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblMandatory" id="lblVoucherAmount">
                                                    <%=GetLabel("Jumlah Voucher")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="Hidden1" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="cboPosition" ClientInstanceName="cboPosition" Width="100%"
                                                                runat="server">
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtVoucherAmount" CssClass="txtCurrency" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px; display: none" id="trDocument" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblMandatory lblLink" id="lblDocument">
                                                    <%=GetLabel("No. Referensi") %></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 250px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 200px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 50px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtReferenceNo" runat="server" Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSaldoReference" runat="server" ReadOnly="true" CssClass="txtCurrency"
                                                                Width="100%" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnReference" value="..." />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                                    </td>
                                                                    <td>
                                                                        <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; padding-left: 10px">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 140px" />
                                            <col />
                                        </colgroup>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Keterangan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemarksDt" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                            </td>
                                        </tr>
                                        <tr style="padding-top: 2px; padding-left: 0px">
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Urutan Tampilan") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDisplayOrder" Width="120px" CssClass="txtNumeric" />
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
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative;">
                                <input type="hidden" value="0" id="hdnDisplayCount" runat="server" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>'
                                                                src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td style="width: 1px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                                src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("TransactionDtID") %>" bindingfield="TransactionDtID" />
                                                <input type="hidden" value="<%#:Eval("GLAccount") %>" bindingfield="GLAccount" />
                                                <input type="hidden" value="<%#:Eval("GLAccountNo") %>" bindingfield="GLAccountNo" />
                                                <input type="hidden" value="<%#:Eval("GLAccountName") %>" bindingfield="GLAccountName" />
                                                <input type="hidden" value="<%#:Eval("CashFlowTypeID") %>" bindingfield="CashFlowTypeID" />
                                                <input type="hidden" value="<%#:Eval("CashFlowTypeCode") %>" bindingfield="CashFlowTypeCode" />
                                                <input type="hidden" value="<%#:Eval("CashFlowTypeName") %>" bindingfield="CashFlowTypeName" />
                                                <input type="hidden" value="<%#:Eval("SubLedgerID") %>" bindingfield="SubLedgerID" />
                                                <input type="hidden" value="<%#:Eval("SearchDialogTypeName") %>" bindingfield="SearchDialogTypeName" />
                                                <input type="hidden" value="<%#:Eval("IDFieldName") %>" bindingfield="IDFieldName" />
                                                <input type="hidden" value="<%#:Eval("CodeFieldName") %>" bindingfield="CodeFieldName" />
                                                <input type="hidden" value="<%#:Eval("DisplayFieldName") %>" bindingfield="DisplayFieldName" />
                                                <input type="hidden" value="<%#:Eval("MethodName") %>" bindingfield="MethodName" />
                                                <input type="hidden" value="<%#:Eval("GCBusinessPartnerType") %>" bindingfield="GCBusinessPartnerType" />
                                                <input type="hidden" value="<%#:Eval("FilterExpression") %>" bindingfield="FilterExpression" />
                                                <input type="hidden" value="<%#:Eval("SubLedger") %>" bindingfield="SubLedger" />
                                                <input type="hidden" value="<%#:Eval("SubLedgerCode") %>" bindingfield="SubLedgerCode" />
                                                <input type="hidden" value="<%#:Eval("SubLedgerName") %>" bindingfield="SubLedgerName" />
                                                <input type="hidden" value="<%#:Eval("HealthcareID") %>" bindingfield="HealthcareID" />
                                                <input type="hidden" value="<%#:Eval("DepartmentID") %>" bindingfield="DepartmentID" />
                                                <input type="hidden" value="<%#:Eval("DepartmentName") %>" bindingfield="DepartmentName" />
                                                <input type="hidden" value="<%#:Eval("ServiceUnitID") %>" bindingfield="ServiceUnitID" />
                                                <input type="hidden" value="<%#:Eval("ServiceUnitCode") %>" bindingfield="ServiceUnitCode" />
                                                <input type="hidden" value="<%#:Eval("ServiceUnitName") %>" bindingfield="ServiceUnitName" />
                                                <input type="hidden" value="<%#:Eval("RevenueCostCenterID") %>" bindingfield="RevenueCostCenterID" />
                                                <input type="hidden" value="<%#:Eval("RevenueCostCenterCode") %>" bindingfield="RevenueCostCenterCode" />
                                                <input type="hidden" value="<%#:Eval("RevenueCostCenterName") %>" bindingfield="RevenueCostCenterName" />
                                                <input type="hidden" value="<%#:Eval("CustomerGroupID") %>" bindingfield="CustomerGroupID" />
                                                <input type="hidden" value="<%#:Eval("CustomerGroupCode") %>" bindingfield="CustomerGroupCode" />
                                                <input type="hidden" value="<%#:Eval("CustomerGroupName") %>" bindingfield="CustomerGroupName" />
                                                <input type="hidden" value="<%#:Eval("BusinessPartnerID") %>" bindingfield="BusinessPartnerID" />
                                                <input type="hidden" value="<%#:Eval("BusinessPartnerCode") %>" bindingfield="BusinessPartnerCode" />
                                                <input type="hidden" value="<%#:Eval("BusinessPartnerName") %>" bindingfield="BusinessPartnerName" />
                                                <input type="hidden" value="<%#:Eval("MRN") %>" bindingfield="MRN" />
                                                <input type="hidden" value="<%#:Eval("MedicalNo") %>" bindingfield="MedicalNo" />
                                                <input type="hidden" value="<%#:Eval("PatientName") %>" bindingfield="PatientName" />
                                                <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                <input type="hidden" value="<%#:Eval("Position") %>" bindingfield="Position" />
                                                <input type="hidden" value="<%#:Eval("DebitAmount") %>" bindingfield="DebitAmount" />
                                                <input type="hidden" value="<%#:Eval("CreditAmount") %>" bindingfield="CreditAmount" />
                                                <input type="hidden" value="<%#:Eval("ReferenceNo") %>" bindingfield="ReferenceNo" />
                                                <input type="hidden" value="<%#:Eval("BalanceEND") %>" bindingfield="BalanceEND" />
                                                <input type="hidden" value="<%#:Eval("DisplayOrder") %>" bindingfield="DisplayOrder" />
                                                <input type="hidden" value="<%#:Eval("IsUsedAsTreasury") %>" bindingfield="IsUsedAsTreasury" />
                                                <input type="hidden" value="<%#:Eval("GCTreasuryGroup") %>" bindingfield="GCTreasuryGroup" />
                                                <input type="hidden" value="<%#:Eval("IsUsingDocumentControl") %>" bindingfield="IsUsingDocumentControl" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("COA")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("GLAccountNo")%>
                                                    <%#:Eval("GLAccountName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                            <HeaderTemplate>
                                                <%=GetLabel("Sub Akun")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("SubLedgerCode")%>
                                                    <%#:Eval("SubLedgerName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfSegmentNo" HeaderText="Segment" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                        <asp:BoundField DataField="cfCustomerGroupBusinessPartner" HeaderText="Customer Group / Business Partner"
                                            HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="120px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Debet") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("Position").ToString() == "D" ? Eval("DebitAmount", "{0:N}") : "0"%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="120px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Kredit") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("Position").ToString() == "K" ? Eval("CreditAmount", "{0:N}") : "0"%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ReferenceNo" HeaderText="No. Referensi" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="130px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 200px; margin-left: 200px">
                            <%= GetLabel("Tambah Data")%></span><span class="lblLink" id="lblCopyPurchaseRequest"
                                style="margin-right: 200px; margin-left: 200px">
                                <%= GetLabel("Salin Permintaan Pembelian")%></span> <span class="lblLink" id="lblCopyPurchaseReceive"
                                    style="margin-right: 200px; margin-left: 200px">
                                    <%= GetLabel("Salin Penerimaan Pembelian")%></span> <span class="lblLink" id="lblCopyRequestDirectPurchase"
                                        style="margin-right: 200px; margin-left: 200px">
                                        <%= GetLabel("Salin Permintaan Pembelian Tunai")%></span> <span class="lblLink" id="lblCopyRealizationDirectPurchase"
                                            style="margin-right: 200px; margin-left: 200px">
                                            <%= GetLabel("Salin Realisasi Pembelian Tunai")%></span><span class="lblLink" id="lblCopyRequestCashReceipt"
                                                style="margin-right: 200px; margin-left: 200px">
                                                <%= GetLabel("Salin Permintaan Kas Bon")%></span>
                    </div>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 450px;">
                                    <div class="pageTitle" style="text-align: center">
                                        <%=GetLabel("Informasi Jurnal")%></div>
                                    <div style="background-color: #EAEAEA;">
                                        <table width="400px" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="10px" />
                                                <col />
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
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
