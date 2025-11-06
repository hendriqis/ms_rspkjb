<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="CollectivePatientReceive.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CollectivePatientReceive" %>

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
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
            setDatePicker('<%=txtPaymentDate.ClientID %>');
        }

        $('#<%=txtPeriodFrom.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodFrom.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtPeriodTo.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodTo.ClientID %>').val(validateDateToFrom(start, end));
        });

        //#region Process
        $('#<%=btnProcess.ClientID %>').live('click', function () {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Harap Pilih Data Terlebih Dahulu');
            }
            else {
                var paymentType = cboPaymentType.GetValue();
                var paymentMethod = cboPaymentMethod.GetValue();
                var cardNo1 = $('#<%=txtCardNumber1.ClientID %>').val();
                var cardNo4 = $('#<%=txtCardNumber4.ClientID %>').val();
                var holderName = $('#<%=txtHolderName.ClientID %>').val();
                var batchNo = $('#<%=txtBatchNo.ClientID %>').val();
                var referenceNo = $('#<%=txtReferenceNo.ClientID %>').val();
                var approvalCode = $('#<%=txtApprovalCode.ClientID %>').val();

                if (paymentType == 'X034^002') {
                    if (paymentMethod == 'X035^002' || paymentMethod == 'X035^003') {
                        if (cardNo1 == '' || cardNo4 == '' || holderName == '' || batchNo == '' || referenceNo == '' || approvalCode == '') {
                            showToast('Warning', 'Harap Lengkapi Tipe dan Metode Pembayaran Terlebih Dahulu');
                        }
                        else {
                            showToastConfirmation('Apakah yakin akan proses billing dan payment ini ?', function (result) {
                                if (result) {
                                    onCustomButtonClick('process');
                                }
                            });
                        }
                    }
                    else if (paymentMethod == 'X035^004') {
                        if (referenceNo == '') {
                            showToast('Warning', 'Harap Lengkapi Tipe dan Metode Pembayaran Terlebih Dahulu');
                        }
                        else {
                            showToastConfirmation('Apakah yakin akan proses billing dan payment ini ?', function (result) {
                                if (result) {
                                    onCustomButtonClick('process');
                                }
                            });
                        }
                    }
                    else if (paymentMethod == 'X035^008') {
                        if (referenceNo == '') {
                            showToast('Warning', 'Harap Lengkapi Tipe dan Metode Pembayaran Terlebih Dahulu');
                        }
                        else {
                            showToastConfirmation('Apakah yakin akan proses billing dan payment ini ?', function (result) {
                                if (result) {
                                    onCustomButtonClick('process');
                                }
                            });
                        }
                    }
                    else if (paymentMethod == 'X035^013') {
                        if (referenceNo == '') {
                            showToast('Warning', 'Harap Lengkapi Tipe dan Metode Pembayaran Terlebih Dahulu');
                        }
                        else {
                            showToastConfirmation('Apakah yakin akan proses billing dan payment ini ?', function (result) {
                                if (result) {
                                    onCustomButtonClick('process');
                                }
                            });
                        }
                    }
                    else {
                        showToastConfirmation('Apakah yakin akan proses billing dan payment ini ?', function (result) {
                            if (result) {
                                onCustomButtonClick('process');
                            }
                        });
                    }
                }
                else {
                    showToastConfirmation('Apakah yakin akan proses billing dan payment ini ?', function (result) {
                        if (result) {
                            onCustomButtonClick('process');
                        }
                    });
                }
            }
        });
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=divTotalRecordSelected.ClientID %>').html('0');
            $('#<%=divTotalBillSelected.ClientID %>').html('0.00');

            $('#<%=trEDC.ClientID %>').attr('style', 'display:none');
            $('#<%=trCardType.ClientID %>').attr('style', 'display:none');
            $('#<%=trCardProvider.ClientID %>').attr('style', 'display:none');
            $('#<%=trCardNo.ClientID %>').attr('style', 'display:none');
            $('#<%=trHolderName.ClientID %>').attr('style', 'display:none');
            $('#<%=trBatchNo.ClientID %>').attr('style', 'display:none');
            $('#<%=trReferenceNo.ClientID %>').attr('style', 'display:none');
            $('#<%=trApprovalCode.ClientID %>').attr('style', 'display:none');
            $('#<%=trBank.ClientID %>').attr('style', 'display:none');
            $('#<%=trBankVirtual.ClientID %>').attr('style', 'display:none');

            $('#<%=txtCardNumber1.ClientID %>').val('');
            $('#<%=txtCardNumber4.ClientID %>').val('');
            $('#<%=txtHolderName.ClientID %>').val('');
            $('#<%=txtBatchNo.ClientID %>').val('');
            $('#<%=txtReferenceNo.ClientID %>').val('');
            $('#<%=txtApprovalCode.ClientID %>').val('');

            cboPaymentType.SetSelectedIndex(0);
            cboCashierGroup.SetSelectedIndex(0);
            cboShift.SetSelectedIndex(0);
            cboPaymentMethod.SetSelectedIndex(0);
            cboEDCMachine.SetSelectedIndex(0);
            cboCardType.SetSelectedIndex(0);
            cboCardProvider.SetSelectedIndex(0);
            cboBank.SetSelectedIndex(0);
            cboBankVirtual.SetSelectedIndex(0);

            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            var countReg = 0;
            var amount = 0;
            $('.chkIsSelected input').each(function () {
                if (isChecked) {
                    countReg = countReg + 1;

                    var $tr = $(this).closest('tr');
                    var token = ",";
                    var newToken = "";
                    var value = $tr.find('.hdnLineAmount').val().split(token).join(newToken);
                    var paymentAmount = parseFloat(parseFloat(value));
                    amount += paymentAmount;
                }
                $(this).prop('checked', isChecked);
                $(this).change();
            });

            $('#<%=divTotalRecordSelected.ClientID %>').html(countReg);
            $('#<%=divTotalBillSelected.ClientID %>').html(formatMoneyDiv(amount));
        });

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            var countReg = 0;
            var amount = 0;
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    countReg = countReg + 1;

                    var $tr = $(this).closest('tr');
                    var token = ",";
                    var newToken = "";
                    var value = $tr.find('.hdnLineAmount').val().split(token).join(newToken);
                    var paymentAmount = parseFloat(parseFloat(value));
                    amount += paymentAmount;
                }
            });

            $('#<%=divTotalRecordSelected.ClientID %>').html(countReg);
            $('#<%=divTotalBillSelected.ClientID %>').html(formatMoneyDiv(amount));
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('.grdview .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html().trim();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        function formatMoneyDiv(value) {
            var text = '0';
            if (value == '') {
                text = '0.00';
            }
            else {
                value = parseFloat(value).toFixed(2);
                text = parseFloat(value).formatMoney(2, '.', ',');
            }
            return text;
        }

        //#region BusinessPartners
        function onGetBusinessPartnerFilterExpression() {
            var filterExpression = "GCBusinessPartnerType = 'X017^002' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblBusinessPartner.lblLink').live('click', function () {
            openSearchDialog('businesspartners', onGetBusinessPartnerFilterExpression(), function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                ontxtBusinessPartnerCodeChanged(value);
            });
        });

        $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
            ontxtBusinessPartnerCodeChanged($(this).val());
        });

        function ontxtBusinessPartnerCodeChanged(value) {
            var filterExpression = onGetBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region MCU Package Item
        function onGetMCUPackageItemFilterExpression() {
            var filterExpression = "ItemID IN (SELECT ItemID FROM ItemService WHERE GCItemType = 'X001^007' AND IsPackageItem = 1) AND IsDeleted = 0 AND GCItemStatus = 'X181^001'";
            return filterExpression;
        }

        $('#lblMCUPackage.lblLink').live('click', function () {
            openSearchDialog('item', onGetMCUPackageItemFilterExpression(), function (value) {
                $('#<%=txtMCUPackageItemCode.ClientID %>').val(value);
                ontxtMCUPackageItemCodeCodeChanged(value);
            });
        });

        $('#<%=txtMCUPackageItemCode.ClientID %>').live('change', function () {
            ontxtMCUPackageItemCodeCodeChanged($(this).val());
        });

        function ontxtMCUPackageItemCodeCodeChanged(value) {
            var filterExpression = onGetMCUPackageItemFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMCUPackageItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtMCUPackageItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%=hdnMCUPackageItemID.ClientID %>').val('');
                    $('#<%=txtMCUPackageItemCode.ClientID %>').val('');
                    $('#<%=txtMCUPackageItemName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        var interval = 600000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            window.clearInterval(intervalID);
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            //cbpView.PerformCallback('refresh');
            intervalID = window.setInterval(function () {
                onRefreshGridView();
            }, interval);
        }

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onCboPaymentTypeValueChanged() {
            var paymentType = cboPaymentType.GetValue();

            cboPaymentMethod.SetSelectedIndex(0);
            cboEDCMachine.SetSelectedIndex(0);
            cboCardType.SetSelectedIndex(0);
            cboCardProvider.SetSelectedIndex(0);
            cboBank.SetSelectedIndex(0);
            cboBankVirtual.SetSelectedIndex(0);

            $('#<%=trEDC.ClientID %>').attr('style', 'display:none');
            $('#<%=trCardType.ClientID %>').attr('style', 'display:none');
            $('#<%=trCardProvider.ClientID %>').attr('style', 'display:none');
            $('#<%=trCardNo.ClientID %>').attr('style', 'display:none');
            $('#<%=trHolderName.ClientID %>').attr('style', 'display:none');
            $('#<%=trBatchNo.ClientID %>').attr('style', 'display:none');
            $('#<%=trReferenceNo.ClientID %>').attr('style', 'display:none');
            $('#<%=trApprovalCode.ClientID %>').attr('style', 'display:none');
            $('#<%=trBank.ClientID %>').attr('style', 'display:none');
            $('#<%=trBankVirtual.ClientID %>').attr('style', 'display:none');

            $('#<%=txtCardNumber1.ClientID %>').val('');
            $('#<%=txtCardNumber4.ClientID %>').val('');
            $('#<%=txtHolderName.ClientID %>').val('');
            $('#<%=txtBatchNo.ClientID %>').val('');
            $('#<%=txtReferenceNo.ClientID %>').val('');
            $('#<%=txtApprovalCode.ClientID %>').val('');

            if (paymentType == 'X034^002') {
                $('#<%=trPaymentMethod.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trPaymentMethod.ClientID %>').attr('style', 'display:none');
            }
        }

        function onCboPaymentMethodValueChanged() {
            var paymentMethod = cboPaymentMethod.GetValue();

            $('#<%=txtCardNumber1.ClientID %>').val('');
            $('#<%=txtCardNumber4.ClientID %>').val('');
            $('#<%=txtHolderName.ClientID %>').val('');
            $('#<%=txtBatchNo.ClientID %>').val('');
            $('#<%=txtReferenceNo.ClientID %>').val('');
            $('#<%=txtApprovalCode.ClientID %>').val('');

            if (paymentMethod == 'X035^002' || paymentMethod == 'X035^003') {
                $('#<%=trEDC.ClientID %>').removeAttr('style');
                $('#<%=trCardType.ClientID %>').removeAttr('style');
                $('#<%=trCardProvider.ClientID %>').removeAttr('style');
                $('#<%=trCardNo.ClientID %>').removeAttr('style');
                $('#<%=trHolderName.ClientID %>').removeAttr('style');
                $('#<%=trBatchNo.ClientID %>').removeAttr('style');
                $('#<%=trReferenceNo.ClientID %>').removeAttr('style');
                $('#<%=trApprovalCode.ClientID %>').removeAttr('style');
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankVirtual.ClientID %>').attr('style', 'display:none');
            }
            else if (paymentMethod == 'X035^004') {
                $('#<%=trEDC.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardType.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardProvider.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trHolderName.ClientID %>').attr('style', 'display:none');
                $('#<%=trBatchNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trReferenceNo.ClientID %>').removeAttr('style');
                $('#<%=trApprovalCode.ClientID %>').attr('style', 'display:none');
                $('#<%=trBank.ClientID %>').removeAttr('style');
                $('#<%=trBankVirtual.ClientID %>').attr('style', 'display:none');
            }
            else if (paymentMethod == 'X035^008') {
                $('#<%=trEDC.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardType.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardProvider.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trHolderName.ClientID %>').attr('style', 'display:none');
                $('#<%=trBatchNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trReferenceNo.ClientID %>').removeAttr('style');
                $('#<%=trApprovalCode.ClientID %>').attr('style', 'display:none');
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankVirtual.ClientID %>').attr('style', 'display:none');
            }
            else if (paymentMethod == 'X035^013') {
                $('#<%=trEDC.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardType.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardProvider.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trHolderName.ClientID %>').attr('style', 'display:none');
                $('#<%=trBatchNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trReferenceNo.ClientID %>').removeAttr('style');
                $('#<%=trApprovalCode.ClientID %>').attr('style', 'display:none');
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankVirtual.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trEDC.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardType.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardProvider.ClientID %>').attr('style', 'display:none');
                $('#<%=trCardNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trHolderName.ClientID %>').attr('style', 'display:none');
                $('#<%=trBatchNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trReferenceNo.ClientID %>').attr('style', 'display:none');
                $('#<%=trApprovalCode.ClientID %>').attr('style', 'display:none');
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankVirtual.ClientID %>').attr('style', 'display:none');
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var totalRecord = $('#<%=hdnTotalRecordAll.ClientID %>').val();
            var totalBill = $('#<%=hdnTotalBillAll.ClientID %>').val();

            $('#<%=divTotalRecordAll.ClientID %>').html(totalRecord);
            $('#<%=divTotalBillAll.ClientID %>').html(totalBill);
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="0" id="hdnPembuatanTagihanTidakAdaOutstandingOrder" runat="server" />
    <input type="hidden" value="0" id="hdnBlokPembuatanTagihanSaatAdaTransaksiOpen" runat="server" />
    <input type="hidden" value="" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" />
    <input type="hidden" value="" id="hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim"
        runat="server" />
    <input type="hidden" value="" id="hdnIsGrouperAmountClaimDefaultZero" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <h4>
                        <%=GetLabel("Filter By : ")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 35%" />
                                <col style="width: 15%" />
                            </colgroup>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Registrasi") %></label>
                                </td>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                            </td>
                                            <td style="width: 30px; text-align: center">
                                                s/d
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien") %></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                        Style="width: 350px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal lblLink" id="lblBusinessPartner">
                                        <%=GetLabel("Penjamin Bayar")%></label>
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
                                                <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
                                                <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblNormal lblLink" id="lblMCUPackage">
                                        <%=GetLabel("Paket MCU")%></label>
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
                                                <input type="hidden" id="hdnMCUPackageItemID" runat="server" value="" />
                                                <asp:TextBox ID="txtMCUPackageItemCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMCUPackageItemName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="2">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="560px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="NIK" FieldName="SSN" />
                                            <qis:QISIntellisenseHint Text="DOB" FieldName="DateOfBirth" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="2">
                                    <table width="100%">
                                        <colgroup>
                                            <col style="width: 25%" />
                                            <col style="width: 25%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;" id="divEntrySummary" runat="server">
                                                    <div class="pageTitle" style="text-align: center">
                                                        <b>
                                                            <%=GetLabel("TOTAL SELURUH")%></b>
                                                    </div>
                                                    <div style="background-color: #EAEAEA;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="1px">
                                                            <colgroup>
                                                                <col width="25%" />
                                                                <col width="25%" />
                                                            </colgroup>
                                                            <tr>
                                                                <td id="Td2" runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Registrasi")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divTotalRecordAll" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                                <td id="Td3" runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Tagihan")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divTotalBillAll" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 100%;" id="div1" runat="server">
                                                    <div class="pageTitle" style="text-align: center">
                                                        <b>
                                                            <%=GetLabel("TOTAL DIPILIH")%></b>
                                                    </div>
                                                    <div style="background-color: #EAEAEA;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="1px">
                                                            <colgroup>
                                                                <col width="25%" />
                                                                <col width="25%" />
                                                            </colgroup>
                                                            <tr>
                                                                <td id="Td4" runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Registrasi")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divTotalRecordSelected" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                                <td id="Td5" runat="server">
                                                                    <div style="text-align: center; width: 150px">
                                                                        <b>
                                                                            <%=GetLabel("Tagihan")%></b>
                                                                    </div>
                                                                    <div runat="server" id="divTotalBillSelected" style="text-align: center; font-weight: bold;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="vertical-align: top">
                    <h4>
                        <%=GetLabel("Tipe dan Metode Pembayaran : ")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 15%" />
                                <col style="width: 35%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Pembayaran") %></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" Width="120px" ID="txtPaymentDate" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tipe Pembayaran")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboPaymentType" ClientInstanceName="cboPaymentType" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboPaymentTypeValueChanged(); }" />
                                    </dxe:ASPxComboBox>
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
                            <tr id="trPaymentMethod" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Metode Pembayaran")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboPaymentMethodValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trEDC" style="display: none" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("EDC")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboEDCMachine" ClientInstanceName="cboEDCMachine" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr id="trCardType" style="display: none" runat="server">
                                <td>
                                    <label id="lblTipeKartu" runat="server">
                                        <%=GetLabel("Tipe Kartu")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCardType" ClientInstanceName="cboCardType" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr id="trCardProvider" style="display: none" runat="server">
                                <td id="Td6" class="tdBankPenerbit" runat="server">
                                    <label id="lblBankPenerbit" runat="server">
                                        <%=GetLabel("Bank Penerbit")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCardProvider" ClientInstanceName="cboCardProvider" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr id="trCardNo" style="display: none" runat="server">
                                <td id="Td1" class="tdNoKartu" runat="server">
                                    <label id="lblNoKartu" runat="server">
                                        <%=GetLabel("No. Kartu")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber1" Width="100%" runat="server" Style="text-align: center" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber2" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                    runat="server" Style="text-align: center" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber3" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                    runat="server" Style="text-align: center" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber4" Width="100%" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trHolderName" style="display: none" runat="server">
                                <td id="Td7" class="tdPemegangKartu" runat="server">
                                    <label id="lblPemegangKartu" runat="server">
                                        <%=GetLabel("Pemegang Kartu")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHolderName" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trBatchNo" style="display: none" runat="server">
                                <td>
                                    <label id="lblNoBatch" runat="server">
                                        <%=GetLabel("No. Batch")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBatchNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trReferenceNo" style="display: none" runat="server">
                                <td>
                                    <label id="lblReferenceNo" runat="server">
                                        <%=GetLabel("No. Referensi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trApprovalCode" style="display: none" runat="server">
                                <td id="Td8" class="tdApprovalCode" runat="server">
                                    <label id="lblApprovalCode" runat="server">
                                        <%=GetLabel("Appr Code")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApprovalCode" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trBank" style="display: none" runat="server">
                                <td>
                                    <label id="lblBank" runat="server">
                                        <%=GetLabel("Bank")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trBankVirtual" style="display: none" runat="server">
                                <td>
                                    <label id="lblBankVirtual" runat="server">
                                        <%=GetLabel("Bank")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboBankVirtual" ClientInstanceName="cboBankVirtual" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <input type="hidden" value="" id="hdnTotalRecordAll" runat="server" />
                                <input type="hidden" value="" id="hdnTotalBillAll" runat="server" />
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                    overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No. Registrasi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Tanggal Registrasi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No. Rekam Medis")%>
                                                    </th>
                                                    <th style="width: 350px">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th style="width: 350px">
                                                        <%=GetLabel("Penjamin Bayar")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Paket MCU")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Total Pasien")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Total Instansi")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Total Tagihan")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Terakhir Dikirim")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="12">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdview grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                    </th>
                                                    <th style="width: 20px; text-align: center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No. Registrasi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Tanggal Registrasi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No. Rekam Medis")%>
                                                    </th>
                                                    <th style="width: 350px">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th style="width: 350px">
                                                        <%=GetLabel("Penjamin Bayar")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Paket MCU")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Total Pasien")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Total Instansi")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Total Tagihan")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("VisitID")%>
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("RegistrationNo")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>'
                                                        bindingfield="hdnRegistrationID" />
                                                    <input type="hidden" class="hdnLineAmount" value='<%#: Eval("LineAmount") %>' bindingfield="hdnLineAmount" />
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("cfRegistrationDateInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center">
                                                        <%#: Eval("MedicalNo")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("PatientName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("BusinessPartnerName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("ItemName1")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfPatientAmountInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfPayerAmountInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right">
                                                        <%#: Eval("cfLineAmountInString")%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
