<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.Master"
    AutoEventWireup="true" CodeBehind="MedicationCharges.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationCharges" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/Prescription/UDD/UDDToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnPrintDrugLabel" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Drug Label")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnHSUImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHSULaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="1" id="hdnIsRightPanelPrintMustProposedCharges" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnHealthcareID" runat="server" />
    <input type="hidden" id="hdnVisitDepartmentID" runat="server" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsReviewPrescriptionMandatoryForProposedTransaction" runat="server" value="0" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setRightPanelButtonEnabled();
            setCustomToolbarVisibility();

            //#region Transaction No
            function onGetFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblPrescriptionOrderNo').click(function () {
                openSearchDialog('patientchargeshd', onGetFilterExpression(), function (value) {
                    $('#<%=txtPrescriptionOrderNo.ClientID %>').val(value);
                    onTxtPrescriptionOrderNoChanged(value);
                });
            });

            function onTxtPrescriptionOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            $('#<%=txtPayerAmount.ClientID %>').change(function () {
                if ($(this).val() == "") {
                    $('#<%=txtPayerAmount.ClientID %>').val("0");
                }
                $(this).trigger('changeValue');
                var payerAmount = parseFloat($('#<%=txtPayerAmount.ClientID %>').attr('hiddenVal'));
                var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));
                $('#<%=txtPatientAmount.ClientID %>').val(lineAmount - payerAmount).trigger('changeValue');
            });

            $('#<%=txtPatientAmount.ClientID %>').change(function () {
                if ($(this).val() == "") {
                    $('#<%=txtPatientAmount.ClientID %>').val("0");
                }
                $(this).trigger('changeValue');
                var patientAmount = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
                var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));
                $('#<%=txtPayerAmount.ClientID %>').val(lineAmount - patientAmount).trigger('changeValue');
            });

            $('#btnSave').click(function () {
                if (IsValid(null, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            })

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            })

            $('#<%=btnPrintDrugLabel.ClientID %>').click(function () {
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var transactionNo = $('#<%=txtPrescriptionOrderNo.ClientID %>').val();
                var date = $('#<%=txtPrescriptionDate.ClientID %>').val();
                var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
                if (transactionID != "") {
                    var param = transactionID + '|' + transactionNo + '|' + date + '|' + time;
                    var url = ResolveUrl("~/Program/Prescription/UDD/MedicationOrder/PrintDrugLabelList.ascx");
                    openUserControlPopup(url, param, 'Cetak Etiket Obat', 800, 600);
                }
                else showToast('Warning', 'Belum ada transaksi resep yang dientry');
            });
        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                    $('#<%=btnPrintDrugLabel.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnIsAdminCanCancelAllTransaction.ClientID %>').val() == "0") {
                        if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
                            $('#<%=btnVoid.ClientID %>').show();
                        } else {
                            $('#<%=btnVoid.ClientID %>').hide();
                        }
                    } else {
                        if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN || $('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.WAIT_FOR_APPROVAL) {
                            $('#<%=btnVoid.ClientID %>').show();
                        } else {
                            $('#<%=btnVoid.ClientID %>').hide();
                        }
                    }
                    $('#<%=btnPrintDrugLabel.ClientID %>').show();
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtPrescriptionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function onCboChargeClassValueChanged() {
            showLoadingPanel();
            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var classID = cboChargeClass.GetValue();
            var trxDate = getTrxDate();

            Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
                if (result != null) {
                    $('#<%=hdnBasePrice.ClientID %>').val(result.BasePrice);
                    $('#<%=txtItemUnitPrice.ClientID %>').val(result.Price).trigger('changeValue');

                    $('#<%=hdnDiscountAmount.ClientID %>').val(result.DiscountAmount);
                    $('#<%=hdnCoverageAmount.ClientID %>').val(result.CoverageAmount);
                    $('#<%=hdnIsDicountInPercentage.ClientID %>').val(result.IsDiscountInPercentage ? '1' : '0');
                    $('#<%=hdnIsCoverageInPercentage.ClientID %>').val(result.IsCoverageInPercentage ? '1' : '0');
                }
                else {
                    $('#<%=hdnBasePrice.ClientID %>').val('0')
                    $('#<%=txtItemUnitPrice.ClientID %>').val('0').trigger('changeValue');

                    $('#<%=hdnDiscountAmount.ClientID %>').val('0');
                    $('#<%=hdnCoverageAmount.ClientID %>').val('0');
                    $('#<%=hdnIsDicountInPercentage.ClientID %>').val('0');
                    $('#<%=hdnIsCoverageInPercentage.ClientID %>').val('0');
                }

                calculate();

            });
            hideLoadingPanel();
        }

        //#region Embalace
        $('#lblEmbalace.lblLink').live('click', function () {
            openSearchDialog('embalace', "IsDeleted = 0", function (value) {
                $('#<%=txtEmbalaceCode.ClientID %>').val(value);
                ontxtEmbalaceCodeChanged(value);
            });
        });

        $('#<%=txtEmbalaceCode.ClientID %>').live('change', function () {
            ontxtEmbalaceCodeChanged($(this).val());
        });

        function ontxtEmbalaceCodeChanged(value) {
            var filterExpression = "IsDeleted = 0 AND EmbalaceCode = '" + value + "'";
            Methods.getObject('GetEmbalaceHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtEmbalaceName.ClientID %>').val(result.EmbalaceName);
                    $('#<%=hdnEmbalaceID.ClientID %>').val(result.EmbalaceID);
                    $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val(result.IsUsingRangePricing);
                    if (!result.IsUsingRangePricing) {
                        $('#<%=txtEmbalaceQty.ClientID %>').removeAttr('readonly');
                        $('#<%=txtEmbalaceQty.ClientID %>').val($('#<%=txtItemQty.ClientID %>').val()).trigger('changeValue');
                    } else {
                        $('#<%=txtEmbalaceQty.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtEmbalaceQty.ClientID %>').val('0').trigger('changeValue');
                    }
                    getEmbalaceTariff();
                }
                else {
                    $('#<%=txtEmbalaceName.ClientID %>').val('');
                    $('#<%=txtEmbalaceCode.ClientID %>').val('');
                    $('#<%=hdnEmbalaceID.ClientID %>').val('');
                    $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val('0');
                    $('#<%=txtEmbalaceQty.ClientID %>').val('0').trigger('changeValue');
                }
            });
        }

        $('#<%=txtItemQty.ClientID %>').live('change', function () {
            var itemQty = parseFloat($('#<%=txtItemQty.ClientID %>').val());

            if (itemQty != '' && itemQty > 0) {
                calculate();
            }
            else {
                $('#<%=txtItemQty.ClientID %>').val('0');
                $('#<%=txtEmbalaceQty.ClientID %>').val('0');
                $('#<%=txtEmbalaceAmount.ClientID %>').val('0.00');
                calculate();
            }
        });

        $('#<%=txtEmbalaceQty.ClientID %>').live('change', function () {
            var embalaceID = $('#<%=hdnEmbalaceID.ClientID %>').val();
            var qty = parseFloat($(this).val());
            if (qty < 0 || $(this).val() == "") {
                $(this).val(0);
            }
            if (embalaceID != "" && $(this).val() != "") {
                getEmbalaceTariff();
            }

        });

        function getEmbalaceTariff() {
            var isUsingRangePricing = $('#<%=hdnEmbalaceIsUsingRangePricing.ClientID %>').val();
            var qty = 0;
            var filterExpression = "EmbalaceID = " + $('#<%=hdnEmbalaceID.ClientID %>').val();
            var embalacePrice = 0;
            var prescriptionFeeAmount = parseFloat($('#<%=hdnPrescriptionFeeAmount.ClientID %>').val());
            if (isUsingRangePricing == "true") {
                qty = parseFloat($('#<%=txtItemQty.ClientID %>').val());
                filterExpression += " AND StartingQty <= " + qty + " AND EndingQty >= " + qty;
                Methods.getObject('GetEmbalaceDtList', filterExpression, function (result) {
                    if (result != null) {
                        embalacePrice = (result.Tariff + prescriptionFeeAmount);
                    }
                    else {
                        embalacePrice = (0 + prescriptionFeeAmount);
                    }
                });
            } else {
                qty = parseFloat($('#<%=txtEmbalaceQty.ClientID %>').val());
                Methods.getObject('GetEmbalaceHdList', filterExpression, function (result) {
                    if (result != null) {
                        embalacePrice = qty * (result.Tariff + prescriptionFeeAmount);
                    }
                    else {
                        embalacePrice = qty * (0 + prescriptionFeeAmount);
                    }
                });
            }
            $('#<%=txtEmbalaceAmount.ClientID %>').val(embalacePrice).trigger('changeValue');
            calculate();
        }
        //#endregion

        function calculate() {
            //#region calculate Total Price
            var embalace = parseFloat($('#<%=txtEmbalaceAmount.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var tariff = parseFloat($('#<%=txtItemUnitPrice.ClientID %>').attr('hiddenVal').replace('.00', '').split(',').join(''));
            var qty = parseFloat($('#<%=txtItemQty.ClientID %>').val().replace('.00', '').split(',').join(''));
            var fixTariff = parseFloat(tariff * qty);
            $('#<%=txtItemPrice.ClientID %>').val(fixTariff).trigger('changeValue');
            //#endregion

            //#region calculate Discount
            var discountAmount = parseFloat($('#<%=hdnDiscountAmount.ClientID %>').val());
            var isDicountInPercentage = ($('#<%=hdnIsDicountInPercentage.ClientID %>').val() == '1');

            var discountTotal = 0;
            if (discountAmount > 0) {
                if (isDicountInPercentage)
                    discountTotal = (fixTariff * discountAmount) / 100;
                else {
                    discountTotal = discountAmount * qty;
                }
                if (discountTotal > fixTariff)
                    discountTotal = fixTariff;
            }
            $('#<%=txtDiscountAmount.ClientID %>').val(discountTotal).trigger('changeValue');
            //#endregion

            //#region calculate Total
            var total = fixTariff - discountTotal + embalace;

            var coverageAmount = parseFloat($('#<%=hdnCoverageAmount.ClientID %>').val());
            var isCoverageInPercentage = ($('#<%=hdnIsCoverageInPercentage.ClientID %>').val() == '1');
            var totalPayer = 0;
            if (isCoverageInPercentage) {
                totalPayer = (total * coverageAmount) / 100;
            }
            else {
                totalPayer = coverageAmount * qty;
            }
            if (total > 0 && totalPayer > total)
                totalPayer = total;
            var totalPatient = total - totalPayer;

            $('#<%=txtPayerAmount.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=txtPatientAmount.ClientID %>').val(totalPatient).trigger('changeValue');
            $('#<%=txtLineAmount.ClientID %>').val(total).trigger('changeValue');
            //#endregion
        }

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);

            $('#<%=txtLocation.ClientID %>').val(entity.LocationName);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtItemQty.ClientID %>').val(entity.ChargedQuantity);
            $('#<%=txtItemUnit.ClientID %>').val(entity.ItemUnit);
            $('#<%=hdnBasePrice.ClientID %>').val(entity.BaseTariff);
            $('#<%=txtItemUnitPrice.ClientID %>').val(entity.Tariff).trigger('changeValue');
            $('#<%=txtItemPrice.ClientID %>').val(entity.Tariff * entity.ChargedQuantity).trigger('changeValue');
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
            $('#<%=txtPatientAmount.ClientID %>').val(entity.PatientAmount).trigger('changeValue');
            $('#<%=txtPayerAmount.ClientID %>').val(entity.PayerAmount).trigger('changeValue');
            $('#<%=txtLineAmount.ClientID %>').val(entity.LineAmount).trigger('changeValue');
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);

            $('#<%=txtEmbalaceName.ClientID %>').val(entity.EmbalaceName);
            $('#<%=txtEmbalaceCode.ClientID %>').val(entity.EmbalaceCode);
            $('#<%=hdnEmbalaceID.ClientID %>').val(entity.EmbalaceID);
            $('#<%=txtEmbalaceQty.ClientID %>').val(entity.EmbalaceQty);
            $('#<%=hdnPrescriptionFeeAmount.ClientID %>').val(entity.PrescriptionFeeAmount);
            var embalaceR = parseFloat(entity.EmbalaceAmount) + parseFloat(entity.PrescriptionFeeAmount);
            $('#<%=txtEmbalaceAmount.ClientID %>').val(embalaceR).trigger('changeValue');

            cboChargeClass.SetValue(entity.ChargeClassID);
            onCboChargeClassValueChanged();

            $('#containerEntry').show();
        });

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'switch') {
                if (param[1] == 'fail')
                    showToast('Switch Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        $('.imgSwitch.imgLink').die('click');
        $('.imgSwitch.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            cbpProcess.PerformCallback('switch|' + obj.ID);
        });

        function setRightPanelButtonEnabled() {
            var transactionID = $('#<%:hdnTransactionID.ClientID %>').val();
            if (transactionID != '0') {
                $('#btnPrescriptionChecklist').removeAttr('enabled');
            }
            else {
                $('#btnPrescriptionChecklist').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'prescriptionChecklist') {
                return $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%:hdnPrescriptionOrderID.ClientID %>').val() + '|' + $('#<%:hdnIsReviewPrescriptionMandatoryForProposedTransaction.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var isPrintMustProposedCharges = $('#<%=hdnIsRightPanelPrintMustProposedCharges.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();
            var PrescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var result = true;

            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Please Save Transaction First!';
                result = false;
            }
            else {
                if (code == 'PM-00201' || code == 'PM-00236' || code == 'PM-002361' || code == 'PM-002362' || code == 'PM-00239' ||
                    code == 'PM-00201' || code == 'PH-00015' || code == 'PH-00012' || code == 'PH-00017' || code == 'PH-00087' ||
                    code == 'PH-00018' || code == 'PH-00024' || code == 'PH-00037' || code == 'PH-00039' || code == 'PH-00046' ||
                    code == 'PH-00054' || code == 'PM-90027' || code == 'PM-002364' || code == 'PH-00108') {
                    if (isPrintMustProposedCharges == "1" && GCTransactionStatus == Constant.TransactionStatus.OPEN) {
                        isAllowPrint = "0";
                    } else {
                        isAllowPrint = "1";
                    }
                    if (isAllowPrint == "1") {
                        filterExpression.text = transactionID;
                    } else {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    }
                    return true;
                }
                if (code == 'PH-00030' || code == 'PH-00025') {
                    if (PrescriptionOrderID == '' || PrescriptionOrderID == '0') {
                        if (isPrintMustProposedCharges == "1" && GCTransactionStatus == Constant.TransactionStatus.OPEN) {
                            isAllowPrint = "0";
                        } else {
                            isAllowPrint = "1";
                        }
                        if (isAllowPrint == "1") {
                            filterExpression.text = transactionID;
                        } else {
                            errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                            return false;
                        }
                        return true;
                    }
                    else {
                        errMessage.text = 'Hanya Resep UDD yang bisa cetak';
                        result = false;
                    }
                }
                else {
                    filterExpression.text = transactionID;
                }
            }
            return result;
        }
    </script>
    <input type="hidden" id="hdnPrescriptionOrderID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionReturnOrderID" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionReturnOrderDtID" value="" runat="server" />
    <input type="hidden" id="hdnEmbalaceID" runat="server" value="" />
    <input type="hidden" id="hdnEmbalaceIsUsingRangePricing" runat="server" value="0" />
    <input type="hidden" id="hdnPrescriptionFeeAmount" runat="server" value="0" />
    <input type="hidden" id="hdnReferenceNo" value="" runat="server" />
    <input type="hidden" id="hdnLocationID" value="" runat="server" />
    <input type="hidden" id="hdnLogisticLocationID" value="" runat="server" />
    <input type="hidden" id="hdnGCTransactionStatus" value="" runat="server" />
    <input type="hidden" id="hdnIsAdminCanCancelAllTransaction" value="" runat="server" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <input type="hidden" id="hdnDefaultJenisTransaksiBPJS" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblPrescriptionOrderNo">
                                <%=GetLabel("No. Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrescriptionOrderNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" />
                            <%=GetLabel("Tanggal") %>
                            -
                            <%=GetLabel("Jam") %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server"
                                            ReadOnly="true" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrescriptionTime" Width="80px" CssClass="time" runat="server"
                                            Style="text-align: center" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Farmasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDispenseryUnitName" Width="300px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Transaksi BPJS")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBPJSTransType" ClientInstanceName="cboBPJSTransType" Width="300px"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align:top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNprmal" id="lblOrderNo">
                                <%=GetLabel("Informasi Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderNo" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="containerEntry" style="margin-top: 4px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsTrx" style="margin: 0">
                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                        <table class="tblEntryDetail">
                            <colgroup>
                                <col width="10px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Lokasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocation" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <%=GetLabel("Item Name") %>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemCode" Width="100px" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemName" Width="400px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Quantity") %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemQty" Width="100px" CssClass="number" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemUnit" Width="150px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Charge Class") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboChargeClass" ClientInstanceName="cboChargeClass" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboChargeClassValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Harga Satuan") %>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtItemUnitPrice" CssClass="txtCurrency" Width="100px"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal lblLink" id="lblEmbalace">
                                        <%=GetLabel("Embalase")%></label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtEmbalaceCode" Width="100px" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtEmbalaceName" Width="400px" ReadOnly="true" TabIndex="999" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jumlah Embalase")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtEmbalaceQty" CssClass="number" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Harga") %>
                                </td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnBasePrice" />
                                    <input type="hidden" runat="server" id="hdnDiscountAmount" />
                                    <input type="hidden" runat="server" id="hdnCoverageAmount" />
                                    <input type="hidden" runat="server" id="hdnIsDicountInPercentage" />
                                    <input type="hidden" runat="server" id="hdnIsCoverageInPercentage" />
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Harga") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Diskon") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("EMBALASE + R/") %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" runat="server" class="txtCurrency" id="txtItemPrice" readonly="readonly" />
                                            </td>
                                            <td>
                                                <input type="text" runat="server" class="txtCurrency" id="txtDiscountAmount" readonly="readonly" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtEmbalaceAmount" CssClass="txtCurrency" ReadOnly="true"
                                                    TabIndex="999" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Total") %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Pasien") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Instansi") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Total") %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" class="txtCurrency" ID="txtPatientAmount" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" class="txtCurrency" ID="txtPayerAmount" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" class="txtCurrency" ID="txtLineAmount" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="3">
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
                    </fieldset>
                </div>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:ListView ID="lvwView" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" rules="all">
                                                <tr>
                                                    <th rowspan="2" style="width: 70px;">
                                                    </th>
                                                    <th rowspan="2" align="left">
                                                        <div>
                                                            <%=GetLabel("Nama Item")%></div>
                                                    </th>
                                                    <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                        <div>
                                                            <%=GetLabel("Harga Satuan")%></div>
                                                    </th>
                                                    <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                        <div>
                                                            <%=GetLabel("Jumlah")%></div>
                                                    </th>
                                                    <th rowspan="2" align="left" style="padding: 3px; width: 80px;">
                                                        <div>
                                                            <%=GetLabel("Kelas Tagihan")%></div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div>
                                                            <%=GetLabel("Jumlah") %></div>
                                                    </th>
                                                    <th rowspan="2" style="padding: 3px; width: 40px;">
                                                        <div>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 120px;">
                                                        <div>
                                                            <%=GetLabel("Instansi") %></div>
                                                    </th>
                                                    <th style="width: 120px;">
                                                        <div>
                                                            <%=GetLabel("Pasien") %></div>
                                                    </th>
                                                    <th style="width: 120px;">
                                                        <div>
                                                            <%=GetLabel("Total") %></div>
                                                    </th>
                                                </tr>
                                                <tr align="center" style="height: 50px; vertical-align: middle;">
                                                    <td colspan="9">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th rowspan="2" style="width: 70px;">
                                                    </th>
                                                    <th rowspan="2" align="left" style="padding: 3px;">
                                                        <div>
                                                            <%=GetLabel("Nama Item")%></div>
                                                    </th>
                                                    <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                        <div>
                                                            <%=GetLabel("Harga Satuan")%></div>
                                                    </th>
                                                    <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                        <div>
                                                            <%=GetLabel("Jumlah")%></div>
                                                    </th>
                                                    <th rowspan="2" align="left" style="padding: 3px; width: 80px;">
                                                        <div>
                                                            <%=GetLabel("Kelas Tagihan")%></div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div>
                                                            <%=GetLabel("Jumlah") %></div>
                                                    </th>
                                                    <th rowspan="2" style="padding: 3px; width: 40px;">
                                                        <div>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 120px;">
                                                        <div>
                                                            <%=GetLabel("Instansi") %></div>
                                                    </th>
                                                    <th style="width: 120px;">
                                                        <div>
                                                            <%=GetLabel("Pasien") %></div>
                                                    </th>
                                                    <th style="width: 120px;">
                                                        <div>
                                                            <%=GetLabel("Total") %></div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="trFooter">
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td style="padding: 3px;">
                                                        <div style="text-align: right; padding: 0px 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </td>
                                                    <td style="padding: 3px;">
                                                        <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPayer" runat="server">
                                                            Instansi
                                                        </div>
                                                    </td>
                                                    <td style="padding: 3px;">
                                                        <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPatient" runat="server">
                                                            Pasien
                                                        </div>
                                                    </td>
                                                    <td style="padding: 3px;">
                                                        <div style="text-align: right; padding: 0px 3px" id="tdTotalAll" runat="server">
                                                            Total
                                                        </div>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <img class="imgEdit <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                        src='<%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    &nbsp;
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" style="display: none" />
                                                    <img class="imgServiceVerified" <%#: Eval("IsVerified").ToString() == "True" ?  "" : "style='display:none'" %>
                                                        title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                    <input type="hidden" value="<%#:Eval("LocationName") %>" bindingfield="LocationName" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ChargedQuantity") %>" bindingfield="ChargedQuantity" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("ChargeClassID") %>" bindingfield="ChargeClassID" />
                                                    <input type="hidden" value="<%#:Eval("BaseTariff") %>" bindingfield="BaseTariff" />
                                                    <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                                    <input type="hidden" value="<%#:Eval("DiscountAmount") %>" bindingfield="DiscountAmount" />
                                                    <input type="hidden" value="<%#:Eval("EmbalaceID") %>" bindingfield="EmbalaceID" />
                                                    <input type="hidden" value="<%#:Eval("EmbalaceCode") %>" bindingfield="EmbalaceCode" />
                                                    <input type="hidden" value="<%#:Eval("EmbalaceName") %>" bindingfield="EmbalaceName" />
                                                    <input type="hidden" value="<%#:Eval("EmbalaceQty") %>" bindingfield="EmbalaceQty" />
                                                    <input type="hidden" value="<%#:Eval("EmbalaceAmount") %>" bindingfield="EmbalaceAmount" />
                                                    <input type="hidden" value="<%#:Eval("PrescriptionFeeAmount") %>" bindingfield="PrescriptionFeeAmount" />
                                                    <input type="hidden" value="<%#:Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                                    <input type="hidden" value="<%#:Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                                    <input type="hidden" value="<%#:Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                </td>
                                                <td style="padding: 3px;">
                                                    <div>
                                                        <%#: Eval("ItemName1")%></div>
                                                    <div>
                                                </td>
                                                <td style="padding: 3px;" align="right">
                                                    <div>
                                                        <%#: Eval("Tariff", "{0:N}")%></div>
                                                </td>
                                                <td style="padding: 3px;" align="right">
                                                    <div>
                                                        <%#: Eval("ChargedQuantity")%>
                                                        <%#: Eval("ItemUnit")%></div>
                                                </td>
                                                <td style="padding: 3px;">
                                                    <div>
                                                        <%#: Eval("ChargeClassName")%></div>
                                                </td>
                                                <td align="right" style="padding: 3px;">
                                                    <%#:Eval("PayerAmount", "{0:N}")%>
                                                </td>
                                                <td align="right" style="padding: 3px;">
                                                    <%#:Eval("PatientAmount", "{0:N}")%>
                                                </td>
                                                <td align="right" style="padding: 3px;">
                                                    <%#:Eval("LineAmount", "{0:N}")%>
                                                </td>
                                                <td <%# IsEditable.ToString() == "True" ?  "" : "style='display:none'" %> valign="middle">
                                                    <img style="margin-left: 2px" class="imgSwitch imgLink" title='<%=GetLabel("Switch")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
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
