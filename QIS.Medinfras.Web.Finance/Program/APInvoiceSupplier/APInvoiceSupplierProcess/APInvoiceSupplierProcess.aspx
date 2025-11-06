<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/SupplierPage/MPBaseSupplierPageTrx.master"
    AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcess.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcess" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/APInvoiceSupplier/APInvoiceSupplierToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnApprove" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPurchaseInvoiceDate.ClientID %>');
            
            var oIsAllowBackdate = $('#<%=hdnIsAllowBackDate.ClientID %>').val();
            var oIsAllowFuturedate = $('#<%=hdnIsAllowFutureDate.ClientID %>').val();

            if (oIsAllowBackdate != "1") {
                $('#<%=txtPurchaseInvoiceDate.ClientID %>').datepicker('option', 'minDate', '0');
            }

            if (oIsAllowFuturedate != "1") {
                $('#<%=txtPurchaseInvoiceDate.ClientID %>').datepicker('option', 'maxDate', '0');
            }

            setDatePicker('<%=txtDueDate.ClientID %>');

            setDatePicker('<%=txtDocumentDate.ClientID %>');

            setDatePicker('<%=txtInvoiceDate.ClientID %>');
            $('#<%=txtInvoiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtTaxInvoiceDate.ClientID %>');
            $('#<%=txtTaxInvoiceDate.ClientID %>').datepicker('option', 'maxDate', '0');
            
            if ($('#<%=hdnPurchaseInvoiceID.ClientID %>').val() > 0 && $('#<%=hdnPurchaseInvoiceID.ClientID %>').val() != "") {
                var hdnPurchaseInvoiceDate = $('#<%=hdnPurchaseInvoiceDate.ClientID %>').val();
                $('#<%=txtPurchaseInvoiceDate.ClientID %>').val(hdnPurchaseInvoiceDate);
            }

            if ($('#<%=hdnIsEditableCustom.ClientID %>').val() == '1') {
                if ($('#<%=hdnGCBusinessPartnerType.ClientID %>').val() == 'X017^007') { // khusus salin hutang test partner transaction
                    $('#lblAddInvoice').show();
                    $('#lblCopyPurchaseReceive').hide();
                    $('#lblCopyPurchaseOrderTerm').hide();
                    $('#lblCopyPurchaseOrderConsignment').hide();
                    $('#lblCopyCreditNote').hide();
                    $('#lblCopyTestPartnerTransaction').show();
                } else {
                    $('#lblAddInvoice').show();
                    $('#lblCopyPurchaseReceive').show();
                    $('#lblCopyPurchaseOrderTerm').show();
                    $('#lblCopyPurchaseOrderConsignment').show();
                    $('#lblCopyCreditNote').show();
                    $('#lblCopyTestPartnerTransaction').hide();
                }
            }
            else {
                $('#lblAddInvoice').hide();
                $('#lblCopyPurchaseReceive').hide();
                $('#lblCopyPurchaseOrderTerm').hide();
                $('#lblCopyPurchaseOrderConsignment').hide();
                $('#lblCopyCreditNote').hide();
                $('#lblCopyTestPartnerTransaction').hide();
            }

            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus == 'X121^999') {
                $('#<%=txtFinalDiscountPIPctg.ClientID %>').val("0.00");
                $('#<%=txtFinalDIscountPI.ClientID %>').val("0.00");
                $('#<%=hdnFinalDiscountPctg.ClientID %>').setVal("0.00");
                $('#<%=hdnFinalDiscountText.ClientID %>').setVal("0.00");
                $('#<%=txtStampPI.ClientID %>').val("0.00");
                $('#<%=hdnStampPI.ClientID %>').val("0.00");
                $('#<%=txtChargesPI.ClientID %>').val("0.00");
                $('#<%=hdnChargesPI.ClientID %>').val("0.00");
            }

            setCustomToolbarVisibility();

            $('#<%=txtCreditNoteAmount.ClientID %>').val($('#<%=hdnTotalCNAmount.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtFinalDiscountPIPctg.ClientID %>').val($('#<%=hdnFinalDiscountPctg.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtFinalDIscountPI.ClientID %>').val($('#<%=hdnFinalDiscountText.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtStampPI.ClientID %>').val($('#<%=hdnStampPI.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtChargesPI.ClientID %>').val($('#<%=hdnChargesPI.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtPPHPIPctg1.ClientID %>').val($('#<%=hdnPPHPctg.ClientID %>').val()).trigger('changeValue');
            $('#<%=txtPPHPI1.ClientID %>').val($('#<%=hdnPPHText.ClientID %>').val()).trigger('changeValue');

            if ($('#<%=chkDiscountPercent.ClientID %>').is(':checked')) {
                $('#<%=txtFinalDiscountPIPctg.ClientID %>').change();
                $('#<%=txtFinalDiscountPIPctg.ClientID%>').removeAttr('readonly');
                $('#<%=txtFinalDIscountPI.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtFinalDIscountPI.ClientID %>').change();
                $('#<%=txtFinalDiscountPIPctg.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtFinalDIscountPI.ClientID%>').removeAttr('readonly');
            }

            $('#btnSave').click(function (evt) {
                var ProductLineDt = $('#<%=hdnProductLineDtID.ClientID %>').val();
                var dept = cboDepartment.GetValue();
                var serviceUnit = $('#<%=hdnServiceUnitID.ClientID %>').val();
                if (ProductLineDt == null || ProductLineDt == "" || ProductLineDt == 0) {
                    if ($('#<%=hdnIsUsedProductLine.ClientID %>').val() == "0") {
                        showToast('Save Failed', 'Product Line harus diisi.');
                    } else {
                        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                            cbpProcess.PerformCallback('save');
                        }
                    }
                } else if (dept == null || dept == "") {
                    showToast('Save Failed', 'Department harus diisi.');
                } else if (serviceUnit == null || serviceUnit == "" || serviceUnit == 0) {
                    showToast('Save Failed', 'Service Unit harus diisi.');
                } else {
                    if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                        cbpProcess.PerformCallback('save');
                    }
                }
            });

            $('#btnClose').click(function () {
                $('#containerEntry').hide();
            });

            //#region Add
            $('#lblAddInvoice').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=txtTransactionAmount.ClientID %>').removeAttr('readonly');
                    $('#<%=txtDiscountAmount.ClientID %>').removeAttr('readonly');
                    $('#<%=txtTransactionAmount.ClientID %>').val('').trigger('changeValue');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=txtInvoiceNo.ClientID %>').val('');
                    $('#<%=txtTaxInvoiceNoPref.ClientID %>').val('010');
                    $('#<%=txtTaxInvoiceNo.ClientID %>').val('');
                    $('#<%=txtRemarksDt.ClientID %>').val('');
                    $('#<%=hdnProductLineDtID.ClientID %>').val('');
                    $('#<%=txtProductLineDtCode.ClientID %>').val('');
                    $('#<%=txtProductLineDtName.ClientID %>').val('');
                    cboDepartment.SetValue('');
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                    $('#<%=txtDiscountAmount.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtVAT.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtStampAmount.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtChargesAmount.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtDownPayment.ClientID %>').val('').trigger('changeValue');
                    $('#<%=txtCreditNote.ClientID %>').val('').trigger('changeValue');

                    $('#containerEntry').show();
                }
            });
            //#endregion

            //#region Perhitungan
            $('#<%=txtRoundingHdAmount.ClientID %>').live('change', function () {
                $(this).blur();
                calculateTotal();
            });

            $('#<%=txtCreditNoteAmount.ClientID %>').live('change', function () {
                $(this).blur();
                calculateTotal();
            });

            $('#<%=chkPPN.ClientID %>').live('change', function () {
                var isEditable = $('#<%=hdnIsPpnAllowChanged.ClientID %>').val();
                if ($(this).is(':checked')) {
                    if (isEditable == '1') {
                        $('#<%:txtPpnPercentage.ClientID %>').removeAttr('readonly');
                    }
                    else {
                        $('#<%:txtPpnPercentage.ClientID %>').attr('readonly', 'readonly');
                    }
                }
                else {
                    $('#<%:txtPpnPercentage.ClientID %>').attr('readonly', 'readonly');
                }
                calculateTotal();
            });

            $('#<%=txtPpnPercentage.ClientID %>').die('change');
            $('#<%=txtPpnPercentage.ClientID %>').live('change', function () {
                $('#<%:hdnPPNPctg.ClientID %>').val($('#<%=txtPpnPercentage.ClientID %>').val());
                $(this).trigger('changeValue');
                calculateTotal();
            });

            $('#<%=chkPPHPercent1.ClientID %>').die('change');
            $('#<%=chkPPHPercent1.ClientID %>').live('change', function () {
                if ($('#<%=chkPPHPercent1.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');

                    $('#<%=txtPPHPIPctg1.ClientID %>').change();
                    $('#<%=txtPPHPIPctg1.ClientID%>').removeAttr('readonly');
                    $('#<%=txtPPHPI1.ClientID%>').attr('readonly', 'readonly');

                    calculatePPH("fromPctg");
                    calculateTotal();
                } else {
                    $('#<%=txtPPHPI1.ClientID %>').change();
                    $('#<%=txtPPHPIPctg1.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtPPHPI1.ClientID%>').removeAttr('readonly');

                    $(this).trigger('changeValue');
                    calculatePPH("fromTxt");
                    calculateTotal();
                }
            });

            $('#<%=txtFinalDiscountPIPctg.ClientID %>').die('change');
            $('#<%=txtFinalDiscountPIPctg.ClientID %>').live('change', function () {
                if ($('#<%=chkDiscountPercent.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');
                    calculateFinalDiscount("fromPctg");
                    if ($('#<%=chkPPHPercent1.ClientID %>').is(':checked')) {
                        calculatePPH("fromPctg");
                    }
                    calculateTotal();
                }
            });

            $('#<%=txtFinalDIscountPI.ClientID %>').die('change');
            $('#<%=txtFinalDIscountPI.ClientID %>').live('change', function () {
                if (!$('#<%=chkDiscountPercent.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');
                    calculateFinalDiscount("fromTxt");
                    if ($('#<%=chkPPHPercent1.ClientID %>').is(':checked')) {
                        calculatePPH("fromPctg");
                    }
                    calculateTotal();
                }
            });

            $('#<%=txtPPHPIPctg1.ClientID %>').die('change');
            $('#<%=txtPPHPIPctg1.ClientID %>').live('change', function () {
                if ($('#<%=chkPPHPercent1.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');
                    calculatePPH("fromPctg");
                    calculateTotal();
                } else {
                    $(this).trigger('changeValue');
                    calculatePPH("fromTxt");
                    calculateTotal();
                }
            });

            $('#<%=txtPPHPI1.ClientID %>').die('change');
            $('#<%=txtPPHPI1.ClientID %>').live('change', function () {
                if ($('#<%=chkPPHPercent1.ClientID %>').is(':checked')) {
                    $(this).trigger('changeValue');
                    calculatePPH("fromPctg");
                    calculateTotal();
                } else {
                    $(this).trigger('changeValue');
                    calculatePPH("fromTxt");
                    calculateTotal();
                }
            });

            $('#<%=txtChargesPI.ClientID %>').die('change');
            $('#<%=txtChargesPI.ClientID %>').live('change', function () {
                $(this).blur();
                calculateTotal();
            });

            $('#<%=txtStampPI.ClientID %>').die('change');
            $('#<%=txtStampPI.ClientID %>').live('change', function () {
                $(this).blur();
                calculateTotal();
            });
            //#endregion

            $('#<%=txtDueDate.ClientID %>').change(function () {
                var dueDate = $('#<%=txtDueDate.ClientID %>').val();
                $('#<%=hdnDueDate.ClientID %>').val(dueDate);
            });

            $('#lblCopyPurchaseReceive').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var dueDate = $('#<%=hdnDueDate.ClientID %>').val();
                    var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
                    var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var param = id + "|" + dueDate + "|" + productLineID;
                    var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessCtl.ascx');
                    openUserControlPopup(url, param, 'Pilih Penerimaan Pembelian', 1300, 500);
                }
            });

            $('#lblCopyPurchaseOrderTerm').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var dueDate = $('#<%=hdnDueDate.ClientID %>').val();
                    var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
                    var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var param = id + "|" + productLineID;
                    var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessPOTermCtl.ascx');
                    openUserControlPopup(url, param, 'Pilih Termin Pemesanan', 1200, 500);
                }
            });

            $('#lblCopyPurchaseOrderConsignment').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();

                    var isAPFromOrder = $('#<%=hdnIsAPConsignmentFromOrder.ClientID %>').val();
                    var isAPFromOrderPerDetailReceive = $('#<%=hdnIsAPConsignmentFromOrderPerDetailReceive.ClientID %>').val();

                    if (isAPFromOrder == "0") {
                        var dueDate = $('#<%=hdnDueDate.ClientID %>').val();
                        var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
                        var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                        var param = id + "|" + dueDate + "|" + productLineID;

                        var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessConsignmentFromReceiveCtl.ascx');
                        openUserControlPopup(url, param, 'Pilih Penerimaan Konsinyasi', 1200, 500);
                    } else {
                        var dueDate = $('#<%=hdnDueDate.ClientID %>').val();
                        var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
                        var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                        var param = id + "|" + dueDate + "|" + productLineID;

                        if (isAPFromOrderPerDetailReceive == "0") {
                            var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessConsignmentFromOrderCtl.ascx');
                            openUserControlPopup(url, param, 'Pilih Pemesanan Konsinyasi', 1200, 500);
                        }
                        else {
                            var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessConsignmentFromOrderPerDetailReceiveCtl.ascx');
                            openUserControlPopup(url, param, 'Pilih Pemesanan Konsinyasi', 1200, 500);
                        }
                    }
                }
            });

            $('#lblCopyCreditNote').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
                    var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var param = id + "|" + productLineID;
                    var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessCopyCreditNoteCtl.ascx');
                    openUserControlPopup(url, param, 'Pilih Nota Kredit', 1200, 500);
                }
            });

            $('#lblCopyTestPartnerTransaction').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
                    var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var param = id + "|" + productLineID;
                    var url = ResolveUrl('~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessCopyTestPartnerTransactionCtl.ascx');
                    openUserControlPopup(url, param, 'Pilih Transaksi Rujukan ke Pihak Ketiga', 1200, 500);
                }
            });

            $('#<%=txtPurchaseInvoiceDate.ClientID %>').live('change', function () {
                var invoiceDate = $('#<%=txtPurchaseInvoiceDate.ClientID %>').val();
                var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
                var isAllowBackdate = $('#<%=hdnIsAllowBackDate.ClientID %>').val();
                var isAllowFuturedate = $('#<%=hdnIsAllowFutureDate.ClientID %>').val();

                var from = invoiceDate.split("-");
                var f = new Date(from[2], from[1] - 1, from[0]);

                var to = dateToday.split("-");
                var t = new Date(to[2], to[1] - 1, to[0]);

                if (isAllowBackdate != "1") {
                    if (f < t) {
                        $('#<%=txtPurchaseInvoiceDate.ClientID %>').val(dateToday);
                    }
                }

                if (isAllowFuturedate != "1") {
                    if (f > t) {
                        $('#<%=txtInvoiceDate.ClientID %>').val(dateToday);
                    }
                }
            });

            $('#<%=txtTaxInvoiceNo.ClientID %>').live('change', function () {
                var value = $(this).val();
                var valueSplit = value.split("/");
                if (valueSplit[0] == "http:" || valueSplit[0] == "https:") {
                    $(this).val(valueSplit[6]);
                }
            });

            //#region Purchase Invoice No
            $('#lblPurchaseInvoiceNo.lblLink').live('click', function () {
                openSearchDialog('purchaseinvoicehd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtPurchaseInvoiceNo.ClientID %>').val(value);
                    onTxtPurchaseInvoiceNoChanged(value);
                });
            });

            $('#<%=txtPurchaseInvoiceNo.ClientID %>').change(function () {
                onTxtPurchaseInvoiceNoChanged($(this).val());
            });

            function onTxtPurchaseInvoiceNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Product Line HD
            function getProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblProductLine.ClientID %>.lblLink').click(function () {
                openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnProductLineItemType.ClientID %>').val(result.GCItemType);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                        $('#<%=hdnProductLineItemType.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            setRightPanelButtonEnabled();
            calculateTotal();

        }

        $('#<%:chkDiscountPercent.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtFinalDiscountPIPctg.ClientID %>').change();
                $('#<%=txtFinalDiscountPIPctg.ClientID%>').removeAttr('readonly');
                $('#<%=txtFinalDIscountPI.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtFinalDIscountPI.ClientID %>').change();
                $('#<%=txtFinalDiscountPIPctg.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtFinalDIscountPI.ClientID%>').removeAttr('readonly');
            }
        });

        function oncboPPHType1ValueChanged(evt) {
            if ($('#<%=chkPPHPercent1.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
                calculateTotal();
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
                calculateTotal();
            }
        }

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

        function onAfterSaveEditRecordEntryPopup() {
            calculateTotal();
            cbpView.PerformCallback('refresh');
            cbpViewCN.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            if (transactionStatus != 'X121^001') {
                $('#<%=btnApprove.ClientID %>').hide();
                $('#<%=btnVoidByReason.ClientID %>').hide();
            }
            else if (transactionStatus == 'X121^001') {
                $('#<%=btnApprove.ClientID %>').show();
                if (isVoid == 1) {
                    $('#<%=btnVoidByReason.ClientID %>').show();
                } else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                }
            }
        }

        //#region edit and delete
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
            if (entity.PurchaseOrderTermID != 0) {
                var id = entity.ID + '|' + entity.PurchaseOrderTermID;

                displayErrorMessageBox('INFORMATION', "Detail hutang dari PO Termin hanya dapat ditambah / dihapus saja (tidak dapat diubah)");
            }
            else if (entity.TestPartnerTransactionID != 0) {
                var id = entity.ID + '|' + entity.TestPartnerTransactionID;

                displayErrorMessageBox('INFORMATION', "Detail hutang dari Transaksi Rujukan ke Pihak Ketiga hanya dapat ditambah / dihapus saja (tidak dapat diubah)");
            }
            else if (entity.PurchaseReceiveNo != "") {
                var piID = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();

                if (entity.CreditNoteID != 0) {
                    var id = entity.ID + '|' + entity.CreditNoteID;
                    var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessEditCreditNoteCtl.ascx");
                    openUserControlPopup(url, id, 'Detail Credit Note Information', 700, 550);
                }
                else {
                    if (entity.PurchaseOrderID == 0) {
                        var id = entity.ID + '|' + entity.PurchaseReceiveID + '|' + piID;
                        var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessEditCtl.ascx");
                        openUserControlPopup(url, id, 'Detail Purchase Receive Information', 1200, 550);
                    } else {
                        var id = entity.ID + '|' + entity.PurchaseOrderID + '|' + piID;
                        var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessEditOrderCtl.ascx");
                        openUserControlPopup(url, id, 'Detail Purchase Order Information', 1200, 550);
                    }
                }
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtInvoiceNo.ClientID %>').val(entity.ReferenceNo);
                $('#<%=txtInvoiceDate.ClientID %>').val(entity.ReferenceDatePickerFormat);

                var invoiceNo = entity.TaxInvoiceNo.split("|");
                if (invoiceNo.length > 1) {
                    $('#<%=txtTaxInvoiceNoPref.ClientID %>').val(invoiceNo[0]);
                    $('#<%=txtTaxInvoiceNo.ClientID %>').val(invoiceNo[1]);
                }
                else {
                    $('#<%=txtTaxInvoiceNoPref.ClientID %>').val('');
                    $('#<%=txtTaxInvoiceNo.ClientID %>').val(invoiceNo[0]);
                }

                $('#<%=txtTaxInvoiceDate.ClientID %>').val(entity.TaxInvoiceDatePickerFormat);
                $('#<%=hdnProductLineDtID.ClientID %>').val(entity.ProductLineID);
                $('#<%=txtProductLineDtCode.ClientID %>').val(entity.ProductLineCode);
                $('#<%=txtProductLineDtName.ClientID %>').val(entity.ProductLineName);
                cboDepartment.SetValue(entity.DepartmentID);
                $('#<%=hdnServiceUnitID.ClientID %>').val(entity.ServiceUnitID);
                $('#<%=txtServiceUnitCode.ClientID %>').val(entity.ServiceUnitCode);
                $('#<%=txtServiceUnitName.ClientID %>').val(entity.ServiceUnitName);
                $('#<%=txtTransactionAmount.ClientID %>').val(entity.TransactionAmount).trigger('changeValue');
                $('#<%=txtDiscountAmount.ClientID %>').val(entity.FinalDiscountAmount).trigger('changeValue');
                $('#<%=txtVAT.ClientID %>').val(entity.VATAmount).trigger('changeValue');
                $('#<%=txtStampAmount.ClientID %>').val(entity.StampAmount).trigger('changeValue');
                $('#<%=txtChargesAmount.ClientID %>').val(entity.ChargesAmount).trigger('changeValue');
                $('#<%=txtDownPayment.ClientID %>').val(entity.DownPaymentAmount).trigger('changeValue');
                $('#<%=txtCreditNote.ClientID %>').val(entity.CreditNoteAmount).trigger('changeValue');
                $('#<%=txtRemarksDt.ClientID %>').val(entity.Remarks);

                $('#containerEntry').show();
            }
        });

        $('.imgEditCN.imgLink').die('click');
        $('.imgEditCN.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            if (entity.CreditNoteID != 0) {
                var id = entity.ID + '|' + entity.CreditNoteID;
                var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessEditCreditNoteCtl.ascx");
                openUserControlPopup(url, id, 'Detail Credit Note Information', 700, 550);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtInvoiceNo.ClientID %>').val(entity.ReferenceNo);
                $('#<%=txtInvoiceDate.ClientID %>').val(entity.ReferenceDatePickerFormat);

                var invoiceNo = entity.TaxInvoiceNo.split("|");
                if (invoiceNo.length > 1) {
                    $('#<%=txtTaxInvoiceNoPref.ClientID %>').val(invoiceNo[0]);
                    $('#<%=txtTaxInvoiceNo.ClientID %>').val(invoiceNo[1]);
                }
                else {
                    $('#<%=txtTaxInvoiceNoPref.ClientID %>').val('');
                    $('#<%=txtTaxInvoiceNo.ClientID %>').val(invoiceNo[0]);
                }

                $('#<%=txtTaxInvoiceDate.ClientID %>').val(entity.TaxInvoiceDatePickerFormat);
                $('#<%=hdnProductLineDtID.ClientID %>').val(entity.ProductLineID);
                $('#<%=txtProductLineDtCode.ClientID %>').val(entity.ProductLineCode);
                $('#<%=txtProductLineDtName.ClientID %>').val(entity.ProductLineName);
                cboDepartment.SetValue(entity.DepartmentID);
                $('#<%=hdnServiceUnitID.ClientID %>').val(entity.ServiceUnitID);
                $('#<%=txtServiceUnitCode.ClientID %>').val(entity.ServiceUnitCode);
                $('#<%=txtServiceUnitName.ClientID %>').val(entity.ServiceUnitName);
                $('#<%=txtTransactionAmount.ClientID %>').val(entity.TransactionAmount).trigger('changeValue');
                $('#<%=txtDiscountAmount.ClientID %>').val(entity.FinalDiscountAmount).trigger('changeValue');
                $('#<%=txtVAT.ClientID %>').val(entity.VATAmount).trigger('changeValue');
                $('#<%=txtStampAmount.ClientID %>').val(entity.StampAmount).trigger('changeValue');
                $('#<%=txtChargesAmount.ClientID %>').val(entity.ChargesAmount).trigger('changeValue');
                $('#<%=txtDownPayment.ClientID %>').val(entity.DownPaymentAmount).trigger('changeValue');
                $('#<%=txtCreditNote.ClientID %>').val(entity.CreditNoteAmount).trigger('changeValue');
                $('#<%=txtRemarksDt.ClientID %>').val(entity.Remarks);

                $('#containerEntry').show();
            }
        });
        //#endregion

        function calculateFinalDiscount(kode) {
            var totalTrans = parseFloat($('#<%=hdnTotalAmount.ClientID %>').val());
            var totalCN = parseFloat($('#<%=hdnTotalCNAmount.ClientID %>').val());
            var totalTC = totalTrans - totalCN;

            if (kode == "fromPctg") {
                var disc = parseFloat($('#<%=txtFinalDiscountPIPctg.ClientID %>').attr('hiddenVal'));
                if (disc == 0) {
                    disc = parseFloat($('#<%=txtFinalDiscountPIPctg.ClientID %>').val());
                }
                var totalDisc = totalTC * (disc / 100);
                $('#<%=txtFinalDIscountPI.ClientID %>').val(totalDisc).trigger('changeValue');
            }
            else if (kode == "fromTxt") {
                var disc = parseFloat($('#<%=txtFinalDIscountPI.ClientID %>').attr('hiddenVal'));
                if (disc == 0) {
                    disc = parseFloat($('#<%=txtFinalDIscountPI.ClientID %>').val());
                }
                var pctg = disc / (totalTC / 100);
                $('#<%=txtFinalDiscountPIPctg.ClientID %>').val(pctg).trigger('changeValue');
            }
        }

        function calculatePPH(kode) {
            var id = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
            var totalTC = 0;
            if (id != '0') {
                Methods.getObject("GetvPurchaseInvoiceHdList", "PurchaseInvoiceID = '" + id + "'", function (result) {
                    if (result != null) {
                        totalTC = result.TotalDPP;
                    }
                });
            }
                      
            var totalDisc = parseFloat($('#<%=txtFinalDIscountPI.ClientID %>').attr('hiddenVal'));
            if (kode == "fromPctg") {
                var pctg1 = parseFloat($('#<%=txtPPHPIPctg1.ClientID %>').attr('hiddenVal'));
                var totalTrans = totalTC - totalDisc;
                var totalPPH1 = totalTrans * (pctg1 / 100);

                $('#<%=txtPPHPI1.ClientID %>').val(totalPPH1).trigger('changeValue');
                $('#<%=txtPPHPIPctg1.ClientID %>').val(pctg1).trigger('changeValue');

                if (pctg1 > 0) {
                    $('#<%=txtPPHMode.ClientID %>').val("Plus");
                } else if (pctg1 < 0) {
                    $('#<%=txtPPHMode.ClientID %>').val("Minus");
                } else {
                    $('#<%=txtPPHMode.ClientID %>').val("");
                }
            }
            else if (kode == "fromTxt") {
                var pph = parseFloat($('#<%=txtPPHPI1.ClientID %>').attr('hiddenVal'));
                var pctg = pph / (totalTC / 100);
                $('#<%=txtPPHPI1.ClientID %>').val(pph).trigger('changeValue');
                $('#<%=txtPPHPIPctg1.ClientID %>').val(pctg).trigger('changeValue');

                if (pph > 0) {
                    $('#<%=txtPPHMode.ClientID %>').val("Plus");
                } else if (pph < 0) {
                    $('#<%=txtPPHMode.ClientID %>').val("Minus");
                } else {
                    $('#<%=txtPPHMode.ClientID %>').val("");
                }
            }

            var PPH = parseFloat($('#<%=txtPPHPI1.ClientID %>').attr('hiddenVal'));
        }

        function calculateTotal() {
            var totalTrans = parseFloat($('#<%=hdnTotalAmount.ClientID %>').val());
            $('#<%=txtTotalAmount.ClientID %>').val(totalTrans).trigger('changeValue');

            var totalCN = parseFloat($('#<%=hdnTotalCNAmount.ClientID %>').val());
            $('#<%=txtCreditNoteAmount.ClientID %>').val(totalCN).trigger('changeValue');

            var totalTC = totalTrans - totalCN;

            if ($('#<%=chkDiscountPercent.ClientID %>').is(':checked')) {
                calculateFinalDiscount("fromPctg");
            } else {
                calculateFinalDiscount("fromTxt");
            }
            var Discount = parseFloat($('#<%=txtFinalDIscountPI.ClientID %>').attr('hiddenVal'));

            if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
                var temp = totalTC - parseFloat($('#<%=txtFinalDIscountPI.ClientID %>').attr("hiddenVal"));
                var PPN = parseFloat($('#<%=hdnPPNPctg.ClientID %>').val()) / 100 * parseFloat(temp);
                $('#<%=txtPPNPI.ClientID %>').val(PPN).trigger('changeValue');
            }
            else {
                $('#<%=txtPPNPI.ClientID %>').val('0').trigger('changeValue');
            }
            var PPN = parseFloat($('#<%=txtPPNPI.ClientID %>').attr('hiddenVal'));

            var PPH = parseFloat($('#<%=txtPPHPI1.ClientID %>').attr('hiddenVal'));

            var materai = parseFloat($('#<%=txtStampPI.ClientID %>').attr('hiddenVal'));
            var ongkos = parseFloat($('#<%=txtChargesPI.ClientID %>').attr('hiddenVal'));
            var roundingHd = parseFloat($('#<%=txtRoundingHdAmount.ClientID %>').attr('hiddenVal'));

            var totalHarga = totalTrans - totalCN - Discount + PPH + materai + ongkos + PPN + roundingHd;
            $('#<%=txtGrandTotalPI.ClientID %>').val(totalHarga).trigger('changeValue');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=hdnPurchaseInvoiceID.ClientID %>').val() == '0' || $('#<%=hdnPurchaseInvoiceID.ClientID %>').val() == '') {
                $('#<%=hdnPurchaseInvoiceID.ClientID %>').val(param);
                var filterExpression = 'PurchaseInvoiceID = ' + param;
                Methods.getObject('GetPurchaseInvoiceHdList', filterExpression, function (result) {
                    $('#<%=txtPurchaseInvoiceNo.ClientID %>').val(result.PurchaseInvoiceNo);
                    onLoadObject(result.PurchaseInvoiceNo);
                    onAfterCustomSaveSuccess();
                    $('#<%=txtPPHPIPctg1.ClientID %>').change();
                    calculateTotal();
                });
            }
            else {
                $('#<%=txtPPHPIPctg1.ClientID %>').change();
                calculateTotal();
            }
            cbpView.PerformCallback('refresh');
            cbpViewCN.PerformCallback('refresh');
        }

        function onAfterSaveRecordDtSuccess(PurchaseInvoiceID) {
            var purchaseInvoiceNo;
            if ($('#<%=hdnPurchaseInvoiceID.ClientID %>').val() == '0' || $('#<%=hdnPurchaseInvoiceID.ClientID %>').val() == '') {
                $('#<%=hdnPurchaseInvoiceID.ClientID %>').val(PurchaseInvoiceID);
                var filterExpression = 'PurchaseInvoiceID = ' + PurchaseInvoiceID;
                Methods.getObject('GetPurchaseInvoiceHdList', filterExpression, function (result) {
                    $('#<%=txtPurchaseInvoiceNo.ClientID %>').val(result.PurchaseInvoiceNo);
                    onLoadObject(result.PurchaseInvoiceNo);
                    onAfterCustomSaveSuccess();
                });
            }
        }

        function cbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            calculateTotal();
        }

        function cbpViewCNEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewCN.PerformCallback('changepage|' + page);
                });
            }
            calculateTotal();
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            $('#containerEntry').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var PurchaseInvoiceID = s.cpPurchaseInvoiceID;
                    onAfterSaveRecordDtSuccess(PurchaseInvoiceID);
                    $('#lblAddInvoice').click();
                    calculateTotal();
                    cbpView.PerformCallback('refresh');
                    cbpViewCN.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    calculateTotal();
                    cbpView.PerformCallback('refresh');
                    cbpViewCN.PerformCallback('refresh');
                }
            }

            if ($('#<%=txtPurchaseInvoiceNo.ClientID %>').val() != '') {
                onLoadObject($('#<%=txtPurchaseInvoiceNo.ClientID %>').val());
            }
        }

        //#region Paging
        var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
        //#endregion

        $('.lblPurchaseReceiveNo').die('click');
        $('.lblPurchaseReceiveNo').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);
            var id = entity.ID + '|' + entity.PurchaseReceiveID + '|' + entity.PurchaseOrderID;

            if (entity.PurchaseOrderID != 0 && entity.PurchaseOrderID != null) {
                var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessDtOrderCtl.ascx");
                openUserControlPopup(url, id, 'Detail Pemesanan Konsinyasi', 1200, 600);
            } else {
                var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessDtReceiveCtl.ascx");
                openUserControlPopup(url, id, 'Detail Penerimaan Pembelian', 1200, 600);
            }
        });

        $('.lblPurchaseReturnNo.lblLink').die('click');
        $('.lblPurchaseReturnNo.lblLink').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);
            var prID = entity.PurchaseReturnID;

            var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierProcess/APInvoiceSupplierProcessReturnDtCtl.ascx");
            openUserControlPopup(url, prID, 'Detail Retur', 1200, 500);
        });

        //#region Product Line DT
        $('#lblProductLineDt.lblLink').live('click', function () {
            openSearchDialog('productlineitemtype', 'IsDeleted = 0', function (value) {
                $('#<%=txtProductLineDtCode.ClientID %>').val(value);
                onTxtProductLineDtCodeChanged(value);
            });
        });

        $('#<%=txtProductLineDtCode.ClientID %>').live('change', function () {
            onTxtProductLineDtCodeChanged($(this).val());
        });

        function onTxtProductLineDtCodeChanged(value) {
            var filterExpression = "ProductLineCode = '" + value + "'";
            Methods.getObject('GetProductLineList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProductLineDtID.ClientID %>').val(result.ProductLineID);
                    $('#<%=txtProductLineDtName.ClientID %>').val(result.ProductLineName);
                }
                else {
                    $('#<%=hdnProductLineDtID.ClientID %>').val('');
                    $('#<%=txtProductLineDtCode.ClientID %>').val('');
                    $('#<%=txtProductLineDtName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onCboDepartmentValueChanged(evt) {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
        }

        //#region Service Unit
        $('#lblServiceUnit.lblLink').live('click', function () {
            var DepartmentID = cboDepartment.GetValue();
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0";
            openSearchDialog('serviceunit', filterExpression, function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
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
            });
        }
        //#endregion

        function setRightPanelButtonEnabled() {
            if ($('#<%=hdnPurchaseInvoiceID.ClientID %>').val() != '') {
                $('#btninfoSupplierPayment').removeAttr('enabled');
            }
            else {
                $('#btninfoSupplierPayment').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoSupplierPayment') {
                var param = $('#<%:hdnPurchaseInvoiceID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnPurchaseInvoiceID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseInvoiceID = $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();

            if (printStatus == 'true' && code != 'FN-00042' || code != 'FN-00099') {
                if (purchaseInvoiceID == '' || purchaseInvoiceID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else if (code == 'FN-00010' || code == 'FN-00026' || code == 'FN-00041' || code == 'FN-00099' || code == 'FN-00197' || code == 'FN-00216' || code == 'FN-00267') {
                    filterExpression.text = "PurchaseInvoiceID = " + purchaseInvoiceID;
                    return true;
                }
                else if (code == 'FN-00033') {
                    filterExpression.text = purchaseInvoiceID;
                    return true;
                }
            }
            else if (transactionStatus == Constant.TransactionStatus.OPEN && code == 'FN-00042' || code == 'FN-00099') {
                if (purchaseInvoiceID == '' || purchaseInvoiceID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "PurchaseInvoiceID = " + purchaseInvoiceID;
                    return true;
                }
            }
            else {
                if (purchaseInvoiceID == '' || purchaseInvoiceID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                }
                else {
                    if (code != 'FN-00042') {
                        errMessage.text = "Data Doesn't Approved or Closed";
                    }
                    else {
                        errMessage.text = "Data Has Been Approved or Closed";
                    }
                }
                return false;
            }
        }

    </script>
    <input type="hidden" value="" id="hdnPurchaseInvoiceDate" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAPConsignmentFromOrder" runat="server" />
    <input type="hidden" value="" id="hdnIsAPConsignmentFromOrderPerDetailReceive" runat="server" />
    <input type="hidden" value="" id="hdnIsDiscountAppliedToAveragePrice" runat="server" />
    <input type="hidden" value="" id="hdnIsDiscountAppliedToUnitPrice" runat="server" />
    <input type="hidden" value="" id="hdnIsProcessInvoiceCanChangeAveragePrice" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseInvoiceID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnGCBusinessPartnerType" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditableCustom" runat="server" />
    <input type="hidden" value="" id="hdnTotalDPPHeader" runat="server" />
    <input type="hidden" value="" id="hdnPPNPctg" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnStampPI" runat="server" />
    <input type="hidden" value="" id="hdnFinalDiscountPctg" runat="server" />
    <input type="hidden" value="" id="hdnFinalDiscountText" runat="server" />
    <input type="hidden" value="" id="hdnPPHPctg" runat="server" />
    <input type="hidden" value="" id="hdnPPHText" runat="server" />
    <input type="hidden" value="" id="hdnChargesPI" runat="server" />
    <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnDueDate" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoidByReason" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsPpnAllowChanged" runat="server" />
    <input type="hidden" value="0" id="hdnDateToday" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowBackDate" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowFutureDate" runat="server" />
    <div style="overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 60%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 135px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPurchaseInvoiceNo" class="lblLink">
                                    <%=GetLabel("No. Tanda Terima Faktur")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseInvoiceNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Penyelesaian") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtDocumentDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Tukar Faktur") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtPurchaseInvoiceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td class="tdLabel">
                                <%=GetLabel("Jatuh Tempo Faktur s/d") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtDueDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr id="trProductLine" runat="server" style="display: none">
                            <td>
                                <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <input type="hidden" id="hdnProductLineItemType" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 135px" />
                            <col style="width: 150px" />
                            <col style="width: 135px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <%=GetLabel("Catatan") %>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
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
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Tambah Faktur Tanpa Penerimaan")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("No. Faktur/Kirim")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("No. Faktur Pajak")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTaxInvoiceNoPref" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTaxInvoiceNo" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Tanggal") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInvoiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Tanggal Faktur Pajak") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTaxInvoiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Jumlah") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransactionAmount" runat="server" Width="150px" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr id="trProductLineDt" runat="server" style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblProductLineDt">
                                                        <%=GetLabel("Product Line")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnProductLineDtID" value="" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtProductLineDtCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtProductLineDtName" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Asal Pasien")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%"
                                                        runat="server">
                                                        <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblServiceUnit">
                                                        <%=GetLabel("Unit Pelayanan")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
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
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <%--<tr>
                                                <td>
                                                    <%=GetLabel("Diskon Per Item") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiscTransAmount" runat="server" Width="150px" ReadOnly="true"
                                                        CssClass="txtCurrency" />
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Diskon Final") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiscountAmount" runat="server" Width="150px" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("PPN") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVAT" runat="server" Width="150px" ReadOnly="true" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Materai") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStampAmount" runat="server" Width="150px" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Ongkos Kirim") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtChargesAmount" runat="server" Width="150px" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Uang Muka") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDownPayment" runat="server" Width="150px" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Nota Kredit") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCreditNote" runat="server" Width="150px" ReadOnly="true" CssClass="txtCurrency" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%=GetLabel("Pembulatan") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRoundingDtAmount" runat="server" Width="150px" CssClass="txtCurrency" />
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
                                    <h4 style="text-align: center">
                                        <%=GetLabel("[+] Penerimaan/Faktur Pembelian") %></h4>
                                    <input type="hidden" value="0" id="hdnTotalAmount" runat="server" />
                                    <input type="hidden" value="0" id="hdnTotalAmountBeforeDP" runat="server" />
                                    <table class="grdPurchaseInvoice grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th>
                                            </th>
                                            <th class="keyField">
                                            </th>
                                            <th style="width: 100px" align="left">
                                                <%=GetLabel("No Penerimaan") %>
                                            </th>
                                            <th style="width: 80px" align="left">
                                                <%=GetLabel("No Faktur") %>
                                            </th>
                                            <th style="width: 110px" align="left">
                                                <%=GetLabel("No Faktur Pajak") %>
                                                <br />
                                                <%=GetLabel("Tanggal Pajak") %>
                                            </th>
                                            <th style="width: 110px" align="left">
                                                <%=GetLabel("Transaksi Rujukan") %>
                                            </th>
                                            <th style="width: 100px" align="right">
                                                <%=GetLabel("Jumlah") %>
                                            </th>
                                            <%--<th style="width: 80px" align="right">
                                                <%=GetLabel("Diskon Transaksi") %>
                                            </th>--%>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("Diskon Final") %>
                                            </th>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("PPN") %>
                                            </th>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("PPH") %>
                                            </th>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("Materai") %>
                                            </th>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("Ongkos Kirim") %>
                                            </th>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("Uang Muka") %>
                                            </th>
                                            <th style="width: 120px" align="right">
                                                <%=GetLabel("Nota Kredit") %>
                                            </th>
                                            <th style="width: 120px" align="right">
                                                <%=GetLabel("SubTotal") %>
                                            </th>
                                            <th style="width: 80px" align="right">
                                                <%=GetLabel("Pembulatan") %>
                                            </th>
                                            <th align="right" style="width: 120px">
                                                <%=GetLabel("Total") %>
                                            </th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div>
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
                                                            <input type="hidden" bindingfield="PurchaseInvoiceID" value='<%#: Eval("PurchaseInvoiceID")%>' />
                                                            <input type="hidden" bindingfield="CreditNoteID" value='<%#: Eval("CreditNoteID")%>' />
                                                            <input type="hidden" bindingfield="PurchaseOrderTermID" value='<%#: Eval("PurchaseOrderTermID")%>' />
                                                            <input type="hidden" bindingfield="PurchaseOrderID" value='<%#: Eval("PurchaseOrderID")%>' />
                                                            <input type="hidden" bindingfield="PurchaseOrderNo" value='<%#: Eval("PurchaseOrderNo")%>' />
                                                            <input type="hidden" bindingfield="PurchaseReceiveID" value='<%#: Eval("PurchaseReceiveID")%>' />
                                                            <input type="hidden" bindingfield="PurchaseReceiveNo" value='<%#: Eval("PurchaseReceiveNo")%>' />
                                                            <input type="hidden" bindingfield="ReferenceNo" value='<%#: Eval("ReferenceNo")%>' />
                                                            <input type="hidden" bindingfield="ReferenceDatePickerFormat" value='<%#: Eval("ReferenceDatePickerFormat")%>' />
                                                            <input type="hidden" bindingfield="ProductLineID" value='<%#: Eval("ProductLineID")%>' />
                                                            <input type="hidden" bindingfield="ProductLineCode" value='<%#: Eval("ProductLineCode")%>' />
                                                            <input type="hidden" bindingfield="ProductLineName" value='<%#: Eval("ProductLineName")%>' />
                                                            <input type="hidden" bindingfield="DepartmentID" value='<%#: Eval("DepartmentID")%>' />
                                                            <input type="hidden" bindingfield="DepartmentName" value='<%#: Eval("DepartmentName")%>' />
                                                            <input type="hidden" bindingfield="ServiceUnitID" value='<%#: Eval("ServiceUnitID")%>' />
                                                            <input type="hidden" bindingfield="ServiceUnitCode" value='<%#: Eval("ServiceUnitCode")%>' />
                                                            <input type="hidden" bindingfield="ServiceUnitName" value='<%#: Eval("ServiceUnitName")%>' />
                                                            <input type="hidden" bindingfield="TaxInvoiceNo" value='<%#: Eval("TaxInvoiceNo")%>' />
                                                            <input type="hidden" bindingfield="TaxInvoiceDatePickerFormat" value='<%#: Eval("TaxInvoiceDatePickerFormat")%>' />
                                                            <input type="hidden" bindingfield="VATAmount" value='<%#: Eval("VATAmount")%>' />
                                                            <input type="hidden" bindingfield="TransactionAmount" value='<%#: Eval("TransactionAmount")%>' />
                                                            <input type="hidden" bindingfield="DownPaymentAmount" value='<%#: Eval("DownPaymentAmount")%>' />
                                                            <input type="hidden" bindingfield="CreditNoteAmount" value='<%#: Eval("CreditNoteAmount")%>' />
                                                            <input type="hidden" bindingfield="DiscountAmount" value='<%#: Eval("DiscountAmount")%>' />
                                                            <input type="hidden" bindingfield="FinalDiscountAmount" value='<%#: Eval("FinalDiscountAmount")%>' />
                                                            <input type="hidden" bindingfield="StampAmount" value='<%#: Eval("StampAmount")%>' />
                                                            <input type="hidden" bindingfield="ChargesAmount" value='<%#: Eval("ChargesAmount")%>' />
                                                            <input type="hidden" bindingfield="CustomSubTotal" value='<%#: Eval("LineAmount")%>' />
                                                            <input type="hidden" bindingfield="ID" value='<%#: Eval("ID")%>' />
                                                            <input type="hidden" bindingfield="TotalDPP" value='<%#: Eval("cfTotalDPP")%>' />
                                                            <input type="hidden" bindingfield="Remarks" value='<%#: Eval("Remarks")%>' />
                                                            <input type="hidden" bindingfield="TestPartnerTransactionID" value='<%#: Eval("TestPartnerTransactionID")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <label class="lblLink lblPurchaseReceiveNo">
                                                            <%#:Eval("cfReceiveOrderNo") %></label>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("ReferenceNo")%>
                                                        <br />
                                                        <%#:Eval("ReferenceDateInString")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("TaxInvoiceNo")%>
                                                        <br />
                                                        <%#:Eval("TaxInvoiceDateInString")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("TestPartnerTransactionNo")%>
                                                        <br />
                                                        <%#:Eval("TestPartnerTransactionDateInString")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("TransactionAmount","{0:N2}")%>
                                                    </td>
                                                    <%--<td align="right">
                                                        <%#:Eval("DiscountAmount","{0:N2}")%>
                                                    </td>--%>
                                                    <td align="right">
                                                        <%#:Eval("FinalDiscountAmount","{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("VATAmount","{0:N2}") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PPHAmount","{0:N2}") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("StampAmount", "{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("ChargesAmount", "{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("DownPaymentAmount", "{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("CreditNoteAmount", "{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("cfNetBeforeRounding", "{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("RoundingAmount", "{0:N2}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("LineAmount", "{0:N2}")%>
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
                        <span class="lblLink" id="lblAddInvoice" style="margin-right: 100px;">
                            <%= GetLabel("Tambah Faktur tanpa Penerimaan")%>
                        </span><span class="lblLink" id="lblCopyPurchaseReceive" style="margin-right: 100px;">
                            <%= GetLabel("Salin Penerimaan Pembelian")%>
                        </span><span class="lblLink" id="lblCopyPurchaseOrderTerm" style="margin-right: 100px;">
                            <%= GetLabel("Salin Pemesanan dgn Termin")%>
                        </span><span class="lblLink" id="lblCopyPurchaseOrderConsignment" style="margin-right: 100px;">
                            <%= GetLabel("Salin Konsinyasi")%>
                        </span><span class="lblLink" id="lblCopyCreditNote">
                            <%= GetLabel("Salin Nota Kredit")%>
                        </span><span class="lblLink" id="lblCopyTestPartnerTransaction" style="display: none">
                            <%= GetLabel("Salin Transaksi Rujukan ke Pihak Ketiga")%>
                        </span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpViewCN" runat="server" Width="100%" ClientInstanceName="cbpViewCN"
                        ShowLoadingPanel="false" OnCallback="cbpViewCN_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ cbpViewCNEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="height: 250px; overflow-y: scroll; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <h4 style="text-align: center">
                                        <%=GetLabel("[-] Nota Kredit") %></h4>
                                    <input type="hidden" value="0" id="hdnTotalCNAmount" runat="server" />
                                    <table class="grdCreditNote grdSelected" cellspacing="0" width="100%" rules="all">
                                        <colgroup>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <th style="width: 40px" align="center">
                                            </th>
                                            <th class="keyField">
                                            </th>
                                            <th style="width: 150px" align="left">
                                                <%=GetLabel("No Nota Kredit") %>
                                            </th>
                                            <th style="width: 150px" align="left">
                                                <%=GetLabel("No Retur") %>
                                            </th>
                                            <th style="width: 100px" align="center">
                                                <%=GetLabel("Tipe Nota Kredit") %>
                                            </th>
                                            <th style="width: 150px" align="right">
                                                <%=GetLabel("Nilai (Setelah PPN)") %>
                                            </th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwCN">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="5">
                                                        <%=GetLabel("No Data To Display") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <input type="hidden" bindingfield="ID" value='<%#: Eval("ID")%>' />
                                                    <input type="hidden" bindingfield="CreditNoteID" value='<%#: Eval("CreditNoteID")%>' />
                                                    <input type="hidden" bindingfield="PurchaseReturnID" value='<%#: Eval("PurchaseReturnID")%>' />
                                                    <td align="center">
                                                        <div>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <img class="imgEditCN <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>'
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
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("CreditNoteNo") %></label>
                                                        <br />
                                                        <%#:Eval("CreditNoteDateInString")%>
                                                    </td>
                                                    <td align="left">
                                                        <label class="lblLink lblPurchaseReturnNo">
                                                            <%#:Eval("PurchaseReturnNo") %></label>
                                                        <br />
                                                        <%#:Eval("ReturnDateInString")%>
                                                    </td>
                                                    <td align="center">
                                                        <%#:Eval("CreditNoteType")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("CreditNoteAmount","{0:N2}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
                <td>
                    <div id="containerTotal">
                        <table class="tblContentArea" style="width: 100%;">
                            <colgroup>
                                <col style="width: 100%" />
                            </colgroup>
                            <tr>
                                <td valign="top" style="float: right">
                                    <div id="containerTotalFaktur">
                                        <fieldset id="fsTotalFaktur">
                                            <table style="width: 100%;" border="0">
                                                <colgroup>
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td valign="top" align="right">
                                                        <table style="width: 100%;">
                                                            <colgroup>
                                                                <col style="width: 40px" />
                                                                <col style="width: 230px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Total Penerimaan")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTotalAmount" CssClass="txtCurrency" ReadOnly="true" Width="100%"
                                                                        runat="server" hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                    [-]
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Total Nota Kredit")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCreditNoteAmount" CssClass="txtCurrency" ReadOnly="true" Width="100%"
                                                                        runat="server" hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <hr style="margin: 0 0 0 0;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                    [-]
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td class="tdLabel" style="text-align: right; width: 100px;">
                                                                                <label class="lblNormal">
                                                                                    <%=GetLabel("Diskon Final")%></label>&nbsp;
                                                                            </td>
                                                                            <td style="width: 10px">
                                                                                <asp:CheckBox ID="chkDiscountPercent" Checked="true" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txtFinalDiscountPIPctg txtCurrency" ID="txtFinalDiscountPIPctg"
                                                                                    Width="60px" runat="server" hiddenVal="0" />
                                                                                %
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFinalDIscountPI" CssClass="txtCurrency" Width="100%" runat="server"
                                                                        hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <hr style="margin: 0 0 0 0;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                    [+]
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td class="tdLabel" style="text-align: right; width: 100px;">
                                                                                <label class="lblNormal">
                                                                                    <%=GetLabel("PPN")%></label>&nbsp;
                                                                            </td>
                                                                            <td style="width: 10px">
                                                                                <asp:CheckBox ID="chkPPN" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txtPpnPercentage txtCurrency" ID="txtPpnPercentage" ReadOnly="true"
                                                                                    Width="60px" runat="server" hiddenVal="0" />
                                                                                %
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPPNPI" CssClass="txtCurrency" Width="100%" ReadOnly="true" runat="server"
                                                                        hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <hr style="margin: 0 0 0 0;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right;">
                                                                    <dxe:ASPxComboBox ID="cboPPHType1" ClientInstanceName="cboPPHType1" runat="server"
                                                                        Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s,e) { oncboPPHType1ValueChanged(e); }" />
                                                                    </dxe:ASPxComboBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                    [+/-]
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td class="tdLabel" style="text-align: right; width: 100px;">
                                                                                <asp:TextBox ID="txtPPHMode" Width="80px" ReadOnly="true" runat="server" />
                                                                                <%--<dxe:ASPxComboBox ID="cboPPHOptions1" ClientInstanceName="cboPPHOptions1" runat="server"
                                                                                    Width="80px">
                                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHOptions1ValueChanged(e); }"
                                                                                        SelectedIndexChanged="calculateTotal" />
                                                                                </dxe:ASPxComboBox>--%>
                                                                            </td>
                                                                            <td style="width: 10px">
                                                                                <asp:CheckBox ID="chkPPHPercent1" Checked="true" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txtPPHPIPctg1 txtCurrency" ID="txtPPHPIPctg1" Width="60px" runat="server"
                                                                                    hiddenVal="0" />
                                                                                %
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPPHPI1" CssClass="txtCurrency" Width="100%" runat="server" hiddenVal="0"
                                                                        ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <hr style="margin: 0 0 0 0;" />
                                                                </td>
                                                            </tr>
                                                            <tr style="display: none">
                                                                <td class="tdLabel" style="text-align: right;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Tipe PPH 2")%></label>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxComboBox ID="cboPPHType2" ClientInstanceName="cboPPHType2" runat="server"
                                                                        Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s,e) { oncboPPHType2ValueChanged(e); }" />
                                                                    </dxe:ASPxComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="display: none">
                                                                <td style="text-align: right;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td class="tdLabel" style="text-align: right;">
                                                                                <label class="lblNormal">
                                                                                    <%=GetLabel("PPH 2")%></label>
                                                                                <asp:CheckBox ID="chkPPHPercent2" Checked="true" runat="server" />
                                                                            </td>
                                                                            <td style="width: 10px">
                                                                            </td>
                                                                            <td>
                                                                                <%--<dxe:ASPxComboBox ID="cboPPHOptions2" ClientInstanceName="cboPPHOptions2" runat="server"
                                                                                    Width="60px">
                                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHOptions2ValueChanged(e); }"
                                                                                        SelectedIndexChanged="calculateTotal" />
                                                                                </dxe:ASPxComboBox>--%>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txtPPHPIPctg2 txtCurrency" ID="txtPPHPIPctg2" Width="60px" runat="server"
                                                                                    hiddenVal="0" />
                                                                                %
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPPHPI2" CssClass="txtCurrency" Width="100%" runat="server" hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr style="display: none">
                                                                <td class="tdLabel" style="text-align: right;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Tipe PPH 3")%></label>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxComboBox ID="cboPPHType3" ClientInstanceName="cboPPHType3" runat="server"
                                                                        Width="100%">
                                                                        <ClientSideEvents ValueChanged="function(s,e) { oncboPPHType3ValueChanged(e); }" />
                                                                    </dxe:ASPxComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="display: none">
                                                                <td style="text-align: right;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td class="tdLabel" style="text-align: right;">
                                                                                <label class="lblNormal">
                                                                                    <%=GetLabel("PPH 3")%></label>
                                                                                <asp:CheckBox ID="chkPPHPercent3" Checked="true" runat="server" />
                                                                            </td>
                                                                            <td style="width: 10px">
                                                                            </td>
                                                                            <td>
                                                                                <%--<dxe:ASPxComboBox ID="cboPPHOptions3" ClientInstanceName="cboPPHOptions3" runat="server"
                                                                                    Width="60px">
                                                                                    <ClientSideEvents ValueChanged="function(s,e) { oncboPPHOptions3ValueChanged(e); }"
                                                                                        SelectedIndexChanged="calculateTotal" />
                                                                                </dxe:ASPxComboBox>--%>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox class="txtPPHPIPctg3 txtCurrency" ID="txtPPHPIPctg3" Width="60px" runat="server"
                                                                                    hiddenVal="0" />
                                                                                %
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPPHPI3" CssClass="txtCurrency" Width="100%" runat="server" hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Biaya Administrasi/Kirim")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtChargesPI" CssClass="txtCurrency" Width="100%" runat="server"
                                                                        hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                    [+]
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Materai")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtStampPI" CssClass="txtCurrency" Width="100%" runat="server" hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <hr style="margin: 0 0 0 0;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                    [+/-]
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Pembulatan")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRoundingHdAmount" CssClass="txtCurrency" Width="100%" runat="server"
                                                                        hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <hr style="margin: 0 0 0 0;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="text-align: left">
                                                                </td>
                                                                <td class="tdLabel" style="text-align: right">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Total Faktur")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGrandTotalPI" CssClass="txtCurrency" ReadOnly="true" Width="100%"
                                                                        runat="server" hiddenVal="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
