<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientDtPage/MPBasePatientDtPageTrx2.master"
    AutoEventWireup="true" CodeBehind="ARInvoicePatientEditEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePatientEditEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/ARInvoicePatient/ARInvoicePatientToolbarCtl.ascx" TagName="ToolbarCtl"
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
                setDatePicker('<%=txtDocumentDate.ClientID %>');
                setDatePicker('<%=txtARDocumentReceiveDate.ClientID %>');
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
            var filterExpression = "<%:OnGetARInvoiceHdFilterExpression() %>";
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

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
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
            var filter = "IsDeleted = 0 AND BusinessPartnerID = 1";
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

        function setRightPanelButtonEnabled() {
            var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();

            if (arInvoiceID != '' && arInvoiceID != null) {
                var filterExpression = "ARInvoiceID = " + arInvoiceID;
                Methods.getObject('GetARInvoiceReceivingList', filterExpression, function (result) {
                    if (result != null) {
                        if (transactionStatus == 'X121^004' || transactionStatus == 'X121^005') {
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

            } else {
                $('#btnARNote').attr('enabled', 'false');
                $('#btnAlocationRegistration').attr('enabled', 'false');
                $('#btnUpdateDocDate').attr('enabled', 'false');
                $('#btninfoReceivingInvoice').attr('enabled', 'false');
                $('#btnProportionalARInvoice').attr('enabled', 'false');
            }
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
        }

        function onAfterCustomClickSuccess() {
            onLoadObject($('#<%=txtARInvoiceNo.ClientID %>').val());
        }

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

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
                var url = ResolveUrl('~/Program/ARInvoicePatient/ARInvoicePatientEdit/ARInvoicePatientAddRevisionCtl.ascx');
                openUserControlPopup(url, arInvoiceID, 'Tambah Data', 1200, 500);
            }
        });

        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnARInvoiceID.ClientID %>').val();
            if (param != "" && param != 0) {
                if (code == "ARNote") {
                    return "5102|" + param;
                } else {
                    return param;
                }
            } else if (code == 'proportionalARInvoice') {
                var arInvoiceID = $('#<%:hdnARInvoiceID.ClientID %>').val();
                var arInvoiceNo = $('#<%:txtARInvoiceNo.ClientID %>').val();
                return arInvoiceID + "|" + arInvoiceNo;
            } else {
                showToast('Failed', 'Maaf, pilih No. Invoice terlebih dahulu.');
                return false;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var arInvoiceID = $('#<%=hdnARInvoiceID.ClientID %>').val();
            var gcTransactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();

            if (arInvoiceID == '' || arInvoiceID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            } else {
                if (gcTransactionStatus == Constant.TransactionStatus.APPROVED || gcTransactionStatus == Constant.TransactionStatus.PROCESSED || gcTransactionStatus == Constant.TransactionStatus.CLOSED) {
                    if (code == 'FN-00102' || code == 'FN-00103' || code == 'FN-00061' || code == 'FN-00062'
                            || code == 'FN-00174' || code == 'FN-00175' || code == 'FN-00182' || code == 'FN-00183'
                            || code == 'FN-00189' || code == 'FN-00190' || code == 'FN-00191' || code == 'FN-00192'
                            || code == 'FN-00193' || code == 'FN-00194' || code == 'FN-00195' || code == 'FN-00196') {
                        filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                        return true;
                    } else {
                        filterExpression.text = "ARInvoiceID = " + arInvoiceID;
                        return true;
                    }
                }
                else {
                    errMessage.text = 'Transaksi harus di-approve terlebih dahulu.';
                    return false;
                }
            }
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnBusinessPartnerVirtualAccountID" runat="server" value="" />
        <input type="hidden" id="hdnPaymentID" runat="server" />
        <input type="hidden" id="hdnPaymentDetailID" runat="server" />
        <input type="hidden" id="hdnARInvoiceDtID" runat="server" />
        <input type="hidden" id="hdnARInvoiceID" runat="server" />
        <input type="hidden" id="hdnRegistrationID" runat="server" />
        <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
        <input type="hidden" id="hdnIsEditable" runat="server" value="1" />
        <input type="hidden" id="hdnIsVoidByReason" runat="server" value="0" />
        <input type="hidden" id="hdnSetvarLeadTime" runat="server" value="0" />
        <input type="hidden" id="hdnSetvarHitungJatuhTempoDari" runat="server" value="0" />
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 36%" />
                <col style="width: 36%" />
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
                                <input type="hidden" id="hdnPageCount" runat="server" />
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
                                    <%=GetLabel("Cetak Atas Nama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrintAsName" Width="100%" runat="server" />
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
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="position: relative;" id="dView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <table class="grdARInvoiceHD grdSelected" cellspacing="0" width="100%" rules="all">
                                            <colgroup>
                                                <col style="width: 40px" />
                                                <col style="width: 100px" />
                                                <col style="width: 140px" />
                                                <col style="width: 80px" />
                                                <col />
                                                <col style="width: 140px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 40px" />
                                            </colgroup>
                                            <tr>
                                                <th>
                                                </th>
                                                <th class="keyField">
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Tgl Registrasi") %>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("No Registrasi") %>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("No RM") %>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Nama Pasien") %>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("No Piutang") %>
                                                </th>
                                                <th align="right">
                                                    <%=GetLabel("Jmlh Transaksi") %>
                                                </th>
                                                <th align="right">
                                                    <%=GetLabel("Jmlh Klaim") %>
                                                </th>
                                                <th align="right">
                                                    <%=GetLabel("Jmlh Penyesuaian") %>
                                                </th>
                                                <th align="right">
                                                    <%=GetLabel("Jmlh Diskon") %>
                                                </th>
                                                <th align="center">
                                                </th>
                                            </tr>
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="11">
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
                                                            <input type="hidden" class="hdnTransactionAmount" bindingfield="TransactionAmount"
                                                                value='<%#: Eval("TransactionAmount")%>' />
                                                            <input type="hidden" bindingfield="VarianceAmount" value='<%#: Eval("VarianceAmount")%>' />
                                                            <input type="hidden" bindingfield="ClaimedAmount" value='<%#: Eval("ClaimedAmount")%>' />
                                                            <input type="hidden" bindingfield="DiscountAmount" value='<%#: Eval("DiscountAmount")%>' />
                                                            <%#: Eval("ARInvoiceID")%>|<%#:Eval("RegistrationID") %>
                                                        </td>
                                                        <td align="center">
                                                            <%#:Eval("RegistrationDateInString")%>
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("RegistrationNo") %>
                                                        </td>
                                                        <td align="center">
                                                            <%#:Eval("MedicalNo") %>
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("PatientName") %>
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("PaymentNo") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("TransactionAmount","{0:N}") %>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtClaimedAmount" ReadOnly="true" runat="server" Width="95%" CssClass="txtCurrency txtClaimedAmount" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtVarianceAmount" runat="server" Width="95%" CssClass="txtCurrency txtVarianceAmount" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtDiscountAmount" runat="server" Width="95%" CssClass="txtCurrency txtDiscountAmount" />
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
