<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/CustomerPage/MPBaseCustomerPageTrx.master"
    AutoEventWireup="true" CodeBehind="ARInvoicePayerAdjustmentEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerAdjustmentEntry" %>

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

            setDatePicker('<%=txtInvoiceDate.ClientID %>');
            setDatePicker('<%=txtDueDate.ClientID %>');
            setDatePicker('<%=txtDocumentDate.ClientID %>');

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            cbpView.PerformCallback('refresh');
        }

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
            var filterExpression = "IsDeleted = 0 AND TransactionCode IN ('5110')";
            return filterExpression;
        }

        $('#lblTransactionAdjustment.lblLink').live('click', function () {
            openSearchDialog('transactionnonoperational', getTransactionNonOperationalFilterExpression(), function (value) {
                $('#<%=txtTransactionAdjustmentCode.ClientID %>').val(value);
                onTxtTransactionNonOperationalChanged(value);
            });
        });

        $('#<%=txtTransactionAdjustmentCode.ClientID %>').live('change', function () {
            onTxtTransactionNonOperationalChanged($(this).val());
        });

        function onTxtTransactionNonOperationalChanged(value) {
            var filterExpression = getTransactionNonOperationalFilterExpression() + " AND TransactionNonOperationalTypeCode ='" + value + "'";
            Methods.getObject('GetvTransactionNonOperationalTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnTransactionAdjustmentID.ClientID %>').val(result.TransactionNonOperationalTypeID);
                    $('#<%=txtTransactionAdjustmentCode.ClientID %>').val(result.TransactionNonOperationalTypeCode);
                    $('#<%=txtTransactionAdjustmentName.ClientID %>').val(result.TransactionNonOperationalTypeName);
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val(result.RevenueCostCenterCode);
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);
                }
                else {
                    $('#<%=hdnTransactionAdjustmentID.ClientID %>').val('');
                    $('#<%=txtTransactionAdjustmentCode.ClientID %>').val('');
                    $('#<%=txtTransactionAdjustmentName.ClientID %>').val('');
                    $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                    $('#<%=txtRevenueCostCenterName.ClientID %>').val('');
                }
            });
        }
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
            $('#<%=txtTransactionAdjustmentCode.ClientID %>').val("");
            $('#<%=txtTransactionAdjustmentName.ClientID %>').val("");
            $('#<%=txtClaimAmount.ClientID %>').val(0);
            $('#<%=txtRevenueCostCenterCode.ClientID %>').val("");
            $('#<%=txtRevenueCostCenterName.ClientID %>').val("");
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
            $('#<%=hdnTransactionAdjustmentID.ClientID %>').val(entity.TransactionNonOperationalTypeID);
            $('#<%=txtTransactionAdjustmentCode.ClientID %>').val(entity.TransactionNonOperationalTypeCode);
            $('#<%=txtTransactionAdjustmentName.ClientID %>').val(entity.TransactionNonOperationalTypeName);
            $('#<%=txtClaimAmount.ClientID %>').val(entity.ClaimedAmount).trigger('changeValue');
            $('#<%=hdnRevenueCostCenterID.ClientID %>').val(entity.RevenueCostCenterID);
            $('#<%=txtRevenueCostCenterCode.ClientID %>').val(entity.RevenueCostCenterCode);
            $('#<%=txtRevenueCostCenterName.ClientID %>').val(entity.RevenueCostCenterName);

            $('#containerEntry').show();
        });
        //#endregion

        //#region Button
        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                cbpProcess.PerformCallback('save');
        });

        $('#btnClose').live('click', function (evt) {
            $('#<%=hdnEntryID.ClientID %>').val(0);
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
            } else {
                $('#btnARNote').attr('enabled', 'false');
                $('#btnAlocationRegistration').attr('enabled', 'false');
                $('#btnUpdateDocDate').attr('enabled', 'false');
                $('#btninfoReceivingInvoice').attr('enabled', 'false');
                $('#btninfoARReceipt').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnARInvoiceID.ClientID %>').val();
            if (param != "" && param != 0) {
                if (code == "ARNote") {
                    return "5110|" + param;
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
            
        }

    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnARInvoiceID" runat="server" value="0" />
        <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
        <input type="hidden" id="hdnBusinessPartnerVirtualAccountID" runat="server" value="" />
        <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
        <input type="hidden" id="hdnIsEditable" runat="server" value="1" />
        <input type="hidden" id="hdnIsVoidByReason" runat="server" value="0" />
        <input type="hidden" id="hdnPageCount" value="" runat="server" />
        <input type="hidden" id="hdnIsAllowVoidByReason" runat="server" value="" />
        <input type="hidden" id="hdnPrintStatus" runat="server" value="" />
        <input type="hidden" id="hdnIsAllowPrintInvoiceAfterApprove" runat="server" value="0" />
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
                            <%=GetLabel("Transaksi Penyesuaian")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 60%" />
                                    <col style="width: 40%" />
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
                                                    <label class="lblLink lblMandatory" id="lblTransactionAdjustment">
                                                        <%=GetLabel("Transaksi Penyesuaian")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnTransactionAdjustmentID" runat="server" value="0" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 50%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTransactionAdjustmentCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTransactionAdjustmentName" ReadOnly="true" Width="200%"
                                                                    runat="server" />
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
                                                            <col style="width: 50%" />
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
                                                                <asp:TextBox ID="txtRevenueCostCenterName" ReadOnly="true" Width="200%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory" id="Label1">
                                                        <%=GetLabel("Nilai Klaim")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 50%" />
                                                            <col style="width: 3px" />
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
                                            <th align="center" style="width: 80px">
                                            </th>
                                            <th class="keyField">
                                            </th>
                                            <th align="left">
                                                <%=GetLabel("Transaksi Penyesuaian") %>
                                            </th>
                                            <th align="left">
                                                <%=GetLabel("RCC") %>
                                            </th>
                                            <th align="right" style="width: 200px">
                                                <%=GetLabel("Nilai Klaim") %>
                                            </th>
                                            <th align="center" style="width: 200px">
                                                <%=GetLabel("Informasi Dibuat") %>
                                            </th>
                                            <th align="center" style="width: 200px">
                                                <%=GetLabel("Informasi Diubah Terakhir") %>
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
                                                        <input type="hidden" bindingfield="ClaimedAmount" value='<%#: Eval("ClaimedAmount")%>' />
                                                        <input type="hidden" bindingfield="RevenueCostCenterID" value='<%#: Eval("RevenueCostCenterID")%>' />
                                                        <input type="hidden" bindingfield="RevenueCostCenterCode" value='<%#: Eval("RevenueCostCenterCode")%>' />
                                                        <input type="hidden" bindingfield="RevenueCostCenterName" value='<%#: Eval("RevenueCostCenterName")%>' />
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
                                                        <%#:Eval("ClaimedAmount","{0:N}")%>
                                                    </td>
                                                    <td align="center">
                                                        <%#:Eval("CreatedByName")%> <br />
                                                        <%#:Eval("cfCreatedDateInStringFull")%>
                                                    </td>
                                                    <td align="center">
                                                        <%#:Eval("LastUpdatedByName")%> <br />
                                                        <%#:Eval("cfLastUpdatedDateInStringFull")%>
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
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
        OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
