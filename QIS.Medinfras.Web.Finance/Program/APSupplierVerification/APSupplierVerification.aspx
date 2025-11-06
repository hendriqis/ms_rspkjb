<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="APSupplierVerification.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.APSupplierVerification" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnCloseNew" runat="server" crudmode="R" title="VoidNewPCI">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Void&New")%></div>
    </li>
    <li id="btnCloseVoid" runat="server" crudmode="R" title="VoidPCI">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnVoidByReason" runat="server" crudmode="R" title="VoidByReasonPMT">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnReopenPayment" runat="server" crudmode="R" title="ReopenPMT">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Reopen")%></div>
    </li>
    <li id="btnUnPaid" runat="server" crudmode="R" title="UnpaidPMT">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("UnPaid")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var total = 0;

        function onLoad() {
            setCustomToolbarVisibility();

            setDatePicker('<%=txtVerificationDate.ClientID %>');
            setDatePicker('<%=txtReferenceDate.ClientID %>');
            setDatePicker('<%=txtDueDateFrom.ClientID %>');
            setDatePicker('<%=txtDueDateTo.ClientID %>');

            if ($('#<%=hdnCounterNoPakaiTanggal.ClientID %>').val() != "2") // pakai tanggal rencana bayar
            {
                setDatePicker('<%=txtPlanningPaymentDate.ClientID %>');
            }

            $('#<%=txtDueDateFrom.ClientID %>').live('change', function (evt) {
                var paymentNo = $('#<%=txtPaymentNo.ClientID %>').val();
                if (paymentNo == null || paymentNo == "") {
                    cbpProcessDetail.PerformCallback('refresh');
                }
            });

            $('#<%=txtDueDateTo.ClientID %>').live('change', function (evt) {
                var paymentNo = $('#<%=txtPaymentNo.ClientID %>').val();
                if (paymentNo == null || paymentNo == "") {
                    cbpProcessDetail.PerformCallback('refresh');
                }
            });

            $('.chkIsSelected input').die('change');
            $('.chkIsSelected input').live('change', function () {
                var isChecked = $(this).is(":checked");
                $txtPembayaran = $(this).closest('tr').find('.txtPembayaran');
                $customSisaHutang = $(this).closest('tr').find('.CustomSisaHutang');

                if (isChecked) {
                    $txtPembayaran.removeAttr('readonly');
                    $txtPembayaran.attr('max', $customSisaHutang);
                }
                else {
                    $txtPembayaran.attr('readonly', 'readonly');
                }

                calculateTotalVerification();
            });

            $('.chkIsSelected input').live('change', function () {
                $('.chkSelectAll input').prop('checked', false);
                calculateTotalVerification();
            });

            $('.chkSelectAll input').live('change', function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
                calculateTotalVerification();
            });

            if ($('#<%=hdnIsAdd.ClientID %>').val() == "1") {
                $('#<%=panelAddPurchaseInvoice.ClientID %>').show();
                $('#<%=panelSupplierPaymentDt.ClientID %>').hide();
                $('#<%=divInformationSupplierPayment.ClientID %>').hide();
            }
            else {
                $('#<%=panelAddPurchaseInvoice.ClientID %>').hide();
                $('#<%=panelSupplierPaymentDt.ClientID %>').show();
                $('#<%=divInformationSupplierPayment.ClientID %>').show();
            }

            if ($('#<%=hdnIsEditable.ClientID %>').val() == "1") {
                $('#lblAddPurchaseInvoice').show();
            }
            else {
                $('#lblAddPurchaseInvoice').hide();
            }

            $('#lblAddPurchaseInvoice').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var id = $('#<%=hdnSupplierPaymentID.ClientID %>').val();
                    var url = ResolveUrl('~/Program/APSupplierVerification/APSupplierVerificationAddCtl.ascx');
                    openUserControlPopup(url, id, 'Tambah Tukar Faktur', 1200, 500);
                }
            });

            $('.txtPembayaran').each(function () {
                $(this).trigger('changeValue');
            });

            $('.txtPembayaran').live('change', function () {
                $tr = $(this).closest('tr');
                var sisaHutang = parseFloat($tr.find('.CustomSisaHutang').val());
                var jmlhBayar = parseFloat($(this).val());

                if (jmlhBayar > sisaHutang) {
                    $(this).val(sisaHutang).trigger('changeValue');
                }
            });

            $('.txtVerificationAmount').live('change', function () {
                $(this).trigger('changeValue');
            });

            //#region Supplier Payment No
            $('#lblSupplierPaymentNo.lblLink').live('click', function () {
                openSearchDialog('supplierpaymenthd', "", function (value) {
                    $('#<%=txtPaymentNo.ClientID %>').val(value);
                    onTxtSupplierPaymentNoChanged(value);
                });
            });

            $('#<%=txtPaymentNo.ClientID %>').live('change', function () {
                onTxtSupplierPaymentNoChanged($(this).val());
            });

            function onTxtSupplierPaymentNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

        }

        $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
            showDeleteConfirmation(function (data) {
                var param = 'justvoid;' + data.GCDeleteReason + ';' + data.Reason;
                onCustomButtonClick(param);
            });
        });

        $('#<%=btnCloseNew.ClientID %>').live('click', function () {
            getCheckedPurchaseInvoice();
            var selectedPI = $('#<%=hdnSelectedMember.ClientID %>').val();
            if (selectedPI != "") {
                onCustomButtonClick('closenew');
            } else {
                displayErrorMessageBox("ERROR", "Pilih nomor tukar faktur (invoice) terlebih dahulu.");
            }
        });

        $('#<%=btnCloseVoid.ClientID %>').live('click', function () {
            getCheckedPurchaseInvoice();
            var selectedPI = $('#<%=hdnSelectedMember.ClientID %>').val();
            if (selectedPI != "") {
                showDeleteConfirmation(function (data) {
                    var param = 'closevoid;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            } else {
                displayErrorMessageBox("ERROR", "Pilih nomor tukar faktur (invoice) terlebih dahulu.");
            }
        });

        $('#<%=btnReopenPayment.ClientID %>').live('click', function () {
            onCustomButtonClick('reopenpayment');
        });

        $('#<%=btnUnPaid.ClientID %>').live('click', function () {
            var voucherNo = $('#<%=txtVoucherNo.ClientID %>').val();
            if (voucherNo != "") {
                displayErrorMessageBox("ERROR", "Nomor tukar faktur (invoice) ini sudah memiliki nomor voucher " + voucherNo);
            } else {
                onCustomButtonClick('unpaid');
            }
        });

        function setCustomToolbarVisibility() {
            var paymentNo = $('#<%=txtPaymentNo.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            if (paymentNo != "") {
                $('#<%=btnCloseNew.ClientID %>').hide();
                $('#<%=btnCloseVoid.ClientID %>').hide();
                if (isVoid == 1) {
                    if ($('#<%=hdnTransactionStatus.ClientID %>').val() == "X121^001") {
                        $('#<%=btnVoidByReason.ClientID %>').show();
                        $('#<%=btnReopenPayment.ClientID %>').hide();
                        $('#<%=btnUnPaid.ClientID %>').hide();
                    }
                    else if ($('#<%=hdnTransactionStatus.ClientID %>').val() == "X121^002") {
                        $('#<%=btnVoidByReason.ClientID %>').hide();
                        $('#<%=btnReopenPayment.ClientID %>').show();
                        $('#<%=btnUnPaid.ClientID %>').hide();
                    }
                    else if ($('#<%=hdnTransactionStatus.ClientID %>').val() == "X121^003") {
                        $('#<%=btnVoidByReason.ClientID %>').hide();
                        $('#<%=btnReopenPayment.ClientID %>').hide();
                        $('#<%=btnUnPaid.ClientID %>').show();
                    }
                    else {
                        $('#<%=btnVoidByReason.ClientID %>').hide();
                        $('#<%=btnReopenPayment.ClientID %>').hide();
                        $('#<%=btnUnPaid.ClientID %>').hide();
                    }
                } else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                    $('#<%=btnReopenPayment.ClientID %>').hide();
                    $('#<%=btnUnPaid.ClientID %>').hide();
                }
            } else {
                $('#<%=btnCloseNew.ClientID %>').show();
                $('#<%=btnCloseVoid.ClientID %>').show();
                $('#<%=btnVoidByReason.ClientID %>').hide();
                $('#<%=btnReopenPayment.ClientID %>').hide();
                $('#<%=btnUnPaid.ClientID %>').hide();
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type.includes("justvoid")) {
                showToast('Void Success', 'Proses Void Sudah Berhasil Dilakukan');
                onLoadObject(retval);
            }
            else if (type.includes("reopenpayment")) {
                showToast('Re-Open Success', 'Proses Re-Open Sudah Berhasil Dilakukan');
                onLoadObject(retval);
            }
            else {
                var paymentNo = $('#<%=txtPaymentNo.ClientID %>').val();
                if (paymentNo == null || paymentNo == "") {
                    cbpProcessDetail.PerformCallback('refresh');
                }
            }

            $('#<%=hdnSelectedMember.ClientID %>').val('');
            $('#<%=hdnSelectedPayment.ClientID %>').val('');
            $('#<%=hdnSelectedSupplier.ClientID %>').val('');
        }

        $('#<%=txtStampAmount.ClientID %>').live('change', function (evt) {
            calculateTotalVerification();
        });

        function onCbpProcessDetailEndCallback(s) {
            hideLoadingPanel();
            $('.txtPembayaran').each(function () {
                $(this).trigger('changeValue');
            });
            $('.txtVerificationAmount').val(0).trigger('changeValue');
            var param = s.cpResult.split('|');
            if (param == 'success') {
                onLoadObject($('#<%=txtPaymentNo.ClientID %>').val());
            }
        }

        function onAfterSaveEditRecordEntryPopup() {
            onLoadObject(retval);
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onLoadObject($('#<%=txtPaymentNo.ClientID %>').val());
        }

        function onCboPaymentMethodValueChanged(evt) {
            var value = cboPaymentMethod.GetValue();
            if (value == '<%=GetSupplierPaymentMethodTransfer() %>' || value == '<%=GetSupplierPaymentMethodGiro() %>' || value == '<%=GetSupplierPaymentMethodCheque() %>') {
                $('#<%=trBank.ClientID %>').removeAttr('style');
                $('#<%=trBankRef.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankRef.ClientID %>').attr('style', 'display:none');
            }
        }

        $('#chkSelectAllInvoice').die('change');
        $('#chkSelectAllInvoice').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('#<%=chkIsAll.ClientID %>').die();
        $('#<%=chkIsAll.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDueDateFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtDueDateTo.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=txtDueDateFrom.ClientID %>').removeAttr('readonly');
                $('#<%=txtDueDateTo.ClientID %>').removeAttr('readonly');
            }
            onRefreshGrid();
        });

        function getCheckedPurchaseInvoice() {
            var lstSelectedPurchaseInvoice = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedPayment = $('#<%=hdnSelectedPayment.ClientID %>').val().split(',');
            var lstSelectedSupplier = $('#<%=hdnSelectedSupplier.ClientID %>').val().split(',');
            var result = '';
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var payment = parseFloat($tr.find('.txtPembayaran').attr('hiddenVal'));
                    var businessPartner = $tr.find('.BusinessPartnerID').val();
                    var idx = lstSelectedPurchaseInvoice.indexOf(key);
                    if (idx < 0) {
                        lstSelectedPurchaseInvoice.push(key);
                        lstSelectedPayment.push(payment);
                        lstSelectedSupplier.push(businessPartner);
                    }
                    else {
                        lstSelectedPayment[idx] = payment;
                        lstSelectedSupplier[idx] = businessPartner;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstSelectedPurchaseInvoice.indexOf(key);
                    if (idx > -1) {
                        lstSelectedPurchaseInvoice.splice(idx, 1);
                        lstSelectedPayment.splice(idx, 1);
                        lstSelectedSupplier.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedPurchaseInvoice.join(','));
            $('#<%=hdnSelectedPayment.ClientID %>').val(lstSelectedPayment.join(','));
            $('#<%=hdnSelectedSupplier.ClientID %>').val(lstSelectedSupplier.join(','));
        }

        function calculateTotalVerification() {
            var lstSelectedPayment = 0;
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var payment = parseFloat($tr.find('.txtPembayaran').attr('hiddenVal'));
                    lstSelectedPayment += payment;
                }
            });
            var stampAmount = parseFloat($('#<%=txtStampAmount.ClientID %>').val());
            lstSelectedPayment += stampAmount;
            $('#<%=txtVerificationAmount.ClientID %>').val(lstSelectedPayment).trigger('changeValue');
        }

        function onBeforeSaveRecord(errMessage) {
            if ($('#<%=hdnSupplierPaymentID.ClientID %>').val() == '0') {
                getCheckedPurchaseInvoice();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                    return true;
                }
                else {
                    showToast('Process Failed', 'Please Select Purchase Invoice First');
                    return false;
                }
            } else {
                return true;
            }
        }

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            getCheckedPurchaseInvoice();
            cbpProcessDetail.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        $('.lblPurchaseInvoiceNo').die('click');
        $('.lblPurchaseInvoiceNo').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').val();
            var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierVerification/APInvoiceSupplierVerificationDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail Information', 1100, 500);
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var id = $row.find('.keyField').val();
                    $('#<%=hdnEntryID.ClientID %>').val(id);
                    cbpProcessDetail.PerformCallback('delete');
                }
            });
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var supplierPaymentID = $('#<%=hdnSupplierPaymentID.ClientID %>').val();
            var dueDateFrom = Methods.DatePickerToDateFormat($('#<%:txtDueDateFrom.ClientID %>').val());
            var dueDateTo = Methods.DatePickerToDateFormat($('#<%:txtDueDateTo.ClientID %>').val());

            if (code == "FN-00093") {
                filterExpression.text = "DueDate BETWEEN '" + dueDateFrom + "' AND '" + dueDateTo + "'";
                return true
            } else {
                if (supplierPaymentID == '' || supplierPaymentID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "SupplierPaymentID = " + supplierPaymentID;
                    return true;
                }
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPayment" runat="server" value="" />
    <input type="hidden" id="hdnSelectedSupplier" runat="server" value="" />
    <input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
    <input type="hidden" id="hdnSupplierPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnIsAdd" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="1" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnIsAllowVoidByReason" value="" runat="server" />
    <input type="hidden" id="hdnCounterNoPakaiTanggal" value="" runat="server" />
    <input type="hidden" id="hdnUsingApprovalVerification" value="" runat="server" />
    <input type="hidden" id="hdnStampAmount" runat="server" value="" />
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblSupplierPaymentNo" class="lblLink">
                                    <%=GetLabel("No. Pembayaran")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentNo" Width="200px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Verifikasi") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtVerificationDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Rencana Bayar") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtPlanningPaymentDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" Width="200px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboPaymentMethodValueChanged(e); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trBank" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Bank")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trBankRef" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No. Cek/Giro") %></label>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtBankReferenceNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Materai")%>
                            </td>
                            <td>
                                <asp:TextBox class="txtCurrency" ID="txtStampAmount" Width="200px" runat="server"
                                    Text="0" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Total Verifikasi")%>
                            </td>
                            <td>
                                <asp:TextBox class="txtCurrency" ID="txtVerificationAmount" Width="200px" ReadOnly="true"
                                    Text="0" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 25%" />
                            <col />
                        </colgroup>
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
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Referensi")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Referensi") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td style="padding-right: 1px;">
                                <asp:TextBox ID="txtRemarks" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Jatuh Tempo")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 145px" />
                                        <col style="width: 3px" />
                                        <col style="width: 145px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDueDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDueDateTo" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsAll" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Quick Search")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="PurchaseInvoiceNo" FieldName="PurchaseInvoiceNo" />
                                        <qis:QISIntellisenseHint Text="BusinessPartnerName" FieldName="BusinessPartnerName" />
                                        <qis:QISIntellisenseHint Text="SupplierInvoiceDate(dd-mm-yyyy)" FieldName="SupplierInvoiceDate" />
                                        <qis:QISIntellisenseHint Text="DueDate(dd-mm-yyyy)" FieldName="DueDate" />
                                        <qis:QISIntellisenseHint Text="ProductLineName" FieldName="ProductLineName" />
                                        <qis:QISIntellisenseHint Text="BankName" FieldName="BankName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="panelAddPurchaseInvoice" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAllInvoice" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("No. Tukar Faktur")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Supplier")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Bank")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tgl. Tukar Faktur")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tgl. Jatuh Tempo")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Total Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Terbayar")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Sisa Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Pembayaran")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="10">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAllInvoice" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("No. Tukar Faktur")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Supplier")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Bank")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tgl. Tukar Faktur")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tgl. Jatuh Tempo")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Total Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Terbayar")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Sisa Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Pembayaran")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PurchaseInvoiceID")%>' />
                                                        <input type="hidden" class="BusinessPartnerID" id="BusinessPartnerID" runat="server"
                                                            value='<%#: Eval("BusinessPartnerID")%>' />
                                                        <input type="hidden" class="CustomSisaHutang" id="CustomSisaHutang" runat="server"
                                                            value='<%#: Eval("CustomSisaHutang")%>' />
                                                    </td>
                                                    <td style="width: 180px;">
                                                        <div>
                                                            <label class="lblLink lblPurchaseInvoiceNo">
                                                                <%#: Eval("PurchaseInvoiceNo") %></label></div>
                                                        <div style="font-size: smaller; max-width: 200px;">
                                                            No. Faktur:
                                                            <%#: Eval("PurchaseReceiveReferenceNo") %></div>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("BusinessPartnerName")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("BankName")%>
                                                    </td>
                                                    <td align="center" style="width: 120px;">
                                                        <%#: Eval("PInvoiceDateInString")%>
                                                    </td>
                                                    <td align="center" style="width: 120px;">
                                                        <%#: Eval("DueDateInString")%>
                                                    </td>
                                                    <td align="right" style="width: 170px;">
                                                        <%#: Eval("TotalNetTransactionAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right" style="width: 170px;">
                                                        <%#: Eval("PaymentAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right" style="width: 170px;">
                                                        <%#: Eval("CustomSisaHutang", "{0:N}")%>
                                                    </td>
                                                    <td align="center" style="width: 170px;">
                                                        <input type="hidden" class="hdnPaymentAmount" value='<%#: Eval("PaymentAmount", "{0:N}")%>' />
                                                        <asp:TextBox ID="txtPembayaran" Width="80%" runat="server" ReadOnly="true" CssClass="txtPembayaran txtCurrency" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panelSupplierPaymentDt" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>"
                                                                        title='<%=GetLabel("Delete")%>' src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <input type="hidden" class="keyField" value='<%#:Eval("PurchaseInvoiceID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No. Tukar Faktur" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <div>
                                                            <label class="lblLink lblPurchaseInvoiceNo">
                                                                <%#: Eval("PurchaseInvoiceNo") %></label></div>
                                                        <div style="font-size: smaller; max-width: 200px;">
                                                            No. Faktur:
                                                            <%#: Eval("PurchaseReceiveReferenceNo") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BusinessPartnerName" HeaderText="Supplier" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PurchaseInvoiceDateInString" HeaderText="Tgl. Tukar Faktur"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                                <asp:BoundField DataField="DueDateInString" HeaderText="Tgl. Jatuh Tempo" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                                <asp:BoundField DataField="TotalNetTransactionAmount" HeaderText="Total Hutang" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" ItemStyle-Width="250px" />
                                                <asp:BoundField DataField="PaymentAmount" HeaderText="Jumlah Pembayaran" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" ItemStyle-Width="250px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div style="width: 100%; text-align: center;">
                            <span class="lblLink" id="lblAddPurchaseInvoice" style="margin-right: 100px;">
                                <%= GetLabel("Tambah Tukar Faktur")%>
                            </span>
                        </div>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divInformationSupplierPayment" runat="server">
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
                                <tr id="trProposedBy" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Proposed Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divProposedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trProposedDate" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Proposed Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divProposedDate">
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
