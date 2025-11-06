<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingSummaryAdjustmentEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingSummaryAdjustmentEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnAdjustmentProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCustomToolbarVisibility();

            $('#btnEntryPopupCancel').live('click', function () {
                $('#containerPopupEntryData').hide();
            });

            $('#btnEntryPopupSave').live('click', function () {
                if (IsValid(null, 'fsEntryPopup', 'mpEntryPopup'))
                    cbpEntryPopupView.PerformCallback('save');
                return false;
            });
        }

        function setCustomToolbarVisibility() {
            var isUsed = $('#<%:hdnIsUsedButtonProcessAdjustment.ClientID %>').val();
            if (isUsed == "1") {
                $('#<%=btnAdjustmentProcess.ClientID %>').show();
            } else {
                $('#<%=btnAdjustmentProcess.ClientID %>').hide();
            }

            $('#lblEntryARInvoiceParamedic').hide();
        }

        $('#<%=btnAdjustmentProcess.ClientID %>').live('click', function () {
            if ($('#<%=hdnRSSummaryID.ClientID %>').val() == '') {
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            }
            else {
                onCustomButtonClick('process');
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Process Success', 'Proses penyesuaian honor dokter di nomor <b>' + retval + '</b> berhasil dilakukan.', function () {
                ontxtRSSummaryNoChanged($('#<%=txtRSSummaryNo.ClientID %>').val());
            });
        }

        //#region RS Summary No
        $('#lblRSSummaryNo.lblLink').live('click', function () {
            var filterExpression = "<%=OnGetRevenueSharingFilterExpression() %>";
            openSearchDialog('transrevenuesharingsummaryhd', filterExpression, function (value) {
                $('#<%=txtRSSummaryNo.ClientID %>').val(value);
                ontxtRSSummaryNoChanged(value);
            });
        });

        $('#<%=txtRSSummaryNo.ClientID %>').live('change', function () {
            ontxtRSSummaryNoChanged($(this).val());
        });

        function ontxtRSSummaryNoChanged(value) {
            var filterExpression = "<%=OnGetRevenueSharingFilterExpression() %>" + " AND RSSummaryNo = '" + value + "'";
            Methods.getObject('GetTransRevenueSharingSummaryHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRSSummaryID.ClientID %>').val(result.RSSummaryID);
                    $('#<%=hdnRSSummaryNo.ClientID %>').val(result.RSSummaryNo);
                    $('#<%=txtRSSummaryNo.ClientID %>').val(result.RSSummaryNo);
                    $('#<%=hdnGCTransactionStatus.ClientID %>').val(result.GCTransactionStatus);

                    var brutoAmount = 0;

                    var filterSRSDt = "RSSummaryID = " + result.RSSummaryID + " AND IsDeleted = 0";
                    Methods.getListObject('GetvTransRevenueSharingSummaryDtList', filterSRSDt, function (resultDt) {
                        for (i = 0; i < resultDt.length; i++) {
                            brutoAmount += parseFloat(resultDt[i].BrutoTransactionAmount);
                        }
                    });

                    var summaryAmount = parseFloat(result.TotalRevenueSharingAmount - result.TotalAdjustmentAmount);
                    var adjustmentAmount = parseFloat(result.TotalAdjustmentAmount);
                    var summaryEndAmount = parseFloat(result.TotalRevenueSharingAmount);

                    $('#<%=txtBrutoAmount.ClientID %>').val(brutoAmount).trigger('changeValue');
                    $('#<%=txtSummaryAmount.ClientID %>').val(summaryAmount).trigger('changeValue');
                    $('#<%=txtSummaryAdjustmentAmount.ClientID %>').val(adjustmentAmount).trigger('changeValue');
                    $('#<%=txtSummaryEndAmount.ClientID %>').val(summaryEndAmount).trigger('changeValue');
                }
                else {
                    $('#<%=hdnRSSummaryID.ClientID %>').val('');
                    $('#<%=hdnRSSummaryNo.ClientID %>').val('');
                    $('#<%=txtRSSummaryNo.ClientID %>').val('');

                    $('#<%=txtBrutoAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtSummaryAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtSummaryAdjustmentAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtSummaryEndAmount.ClientID %>').val('0').trigger('changeValue');
                }
            });

            cbpView.PerformCallback();
        };
        //#endregion

        $('#<%=rblAdjustment.ClientID%>').live('change', function () {
            var adjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var adjGroupPlus = "<%=OnGetAdjustmentGroupPlus() %>";

            if (adjustmentGroup == adjGroupPlus) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();

                $('#lblEntryARInvoiceParamedic').hide();
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();

                $('#lblEntryARInvoiceParamedic').show();
            }

            cbpView.PerformCallback();
        });

        $('#lblEntryARInvoiceParamedic').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var id = $('#<%=hdnRSSummaryID.ClientID %>').val();
                var maxAmount = $('#<%=txtSummaryEndAmount.ClientID %>').val().split(",").join("");
                var param = id + "|" + maxAmount;
                var isHasLinkedToCustomer = $('#<%=hdnIsParamedicHasLinkedToCustomer.ClientID %>').val();
                var url = ResolveUrl('~/Program/RevenueSharing/RevenueSharingSummary/RevenueSharingSummaryEntryARInvoiceCtl.ascx');

                if (isHasLinkedToCustomer == "1") {
                    openUserControlPopup(url, param, 'Pilih Piutang Dokter', 1200, 500);
                } else {
                    displayMessageBox('INFORMASI', "Dokter/Paramedis ini belum memiliki link ke Penjamin Bayar manapun.");
                    hideLoadingPanel();
                }
            }
        });

        $('#lblCopyAdjustmentTransaction').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var id = $('#<%=hdnRSSummaryID.ClientID %>').val();
                var maxAmount = $('#<%=txtSummaryEndAmount.ClientID %>').val().split(",").join("");
                var param = id + "|" + maxAmount;
                var url = ResolveUrl('~/Program/RevenueSharing/RevenueSharingSummary/RevenueSharingSummaryEntryAdjTransCtl.ascx');

                if (id != null && id != "" && id != "0") {
                    openUserControlPopup(url, param, 'Pilih Transaksi Penyesuaian', 1200, 500);
                } else {
                    displayMessageBox('INFORMASI', "Pilih nomor REKAP jasa medis terlebih dahulu.");
                    hideLoadingPanel();
                }
            }
        });

        //#region Detail Process
        $('#lblEntryPopupAddData').live('click', function (evt) {
            var gcTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (gcTransactionStatus == "X121^001" || gcTransactionStatus == "") {
                if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                    $('#<%=hdnID.ClientID %>').val('');
                    $('#<%=txtAdjustmentAmount.ClientID %>').val('0').trigger('changeValue');
                    cboAdjustmentTypeAdd.SetValue('');
                    cboAdjustmentTypeMin.SetValue('');
                    $('#<%=rblAdjustment.ClientID%>').change();
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
                    $('#<%=txtRemarks.ClientID %>').val('');
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                    $('#<%=txtRegistrationNo.ClientID %>').val('');
                    $('#<%=txtRegistrationDate.ClientID %>').val('');
                    $('#<%=txtDischargeDate.ClientID %>').val('');
                    $('#<%=txtReceiptNo.ClientID %>').val('');
                    $('#<%=txtInvoiceNo.ClientID %>').val('');
                    $('#<%=txtReferenceNo.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    $('#<%=txtMedicalNo.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                    $('#<%=txtTransactionNo.ClientID %>').val('');
                    $('#<%=txtTransactionDate.ClientID %>').val('');
                    $('#<%=txtItemName1.ClientID %>').val('');
                    $('#<%=txtChargedQty.ClientID %>').val('0');

                    $('#containerPopupEntryData').show();
                }
            } else {
                showToast('Failed', 'Transaksi tidak dapat diubah. Harap refresh halaman ini.');
            }
        });

        $('.imgDelete').live('click', function () {
            $tr = $(this).closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($tr);
                    $('#<%=hdnID.ClientID %>').val(entity.ID);
                    cbpEntryPopupView.PerformCallback('delete');
                }
            });
        });

        $('.imgEdit').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);

            var adjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var adjGroupPlus = "<%=OnGetAdjustmentGroupPlus() %>";

            $('#<%=hdnID.ClientID %>').val(entity.ID);
            $('#containerPopupEntryData').show();

            if (adjustmentGroup == adjGroupPlus) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();
                cboAdjustmentTypeAdd.SetValue(entity.GCRSAdjustmentType);
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();
                cboAdjustmentTypeMin.SetValue(entity.GCRSAdjustmentType);
            }
            $('#<%=txtAdjustmentAmount.ClientID %>').val(entity.AdjustmentAmount).trigger('changeValue');
            if (entity.IsTaxed == 'True') {
                $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', true);
            }
            else {
                $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
            }
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#<%=hdnRevenueSharingID.ClientID %>').val(entity.RevenueSharingID);
            $('#<%=txtRevenueSharingCode.ClientID %>').val(entity.RevenueSharingCode);
            $('#<%=txtRevenueSharingName.ClientID %>').val(entity.RevenueSharingName);
            $('#<%=txtRegistrationNo.ClientID %>').val(entity.RegistrationNo);
            $('#<%=txtRegistrationDate.ClientID %>').val(entity.txtRegistrationDate);
            $('#<%=txtDischargeDate.ClientID %>').val(entity.DischargeDate);
            $('#<%=txtReceiptNo.ClientID %>').val(entity.ReceiptNo);
            $('#<%=txtInvoiceNo.ClientID %>').val(entity.InvoiceNo);
            $('#<%=txtReferenceNo.ClientID %>').val(entity.ReferenceNo);
            $('#<%=txtBusinessPartnerName.ClientID %>').val(entity.BusinessPartnerName);
            $('#<%=txtMedicalNo.ClientID %>').val(entity.MedicalNo);
            $('#<%=txtPatientName.ClientID %>').val(entity.PatientName);
            $('#<%=txtTransactionNo.ClientID %>').val(entity.TransactionNo);
            $('#<%=txtTransactionDate.ClientID %>').val(entity.cfTransactionDateInDatePickerFormat);
            $('#<%=txtItemName1.ClientID %>').val(entity.ItemName1);
            $('#<%=txtChargedQty.ClientID %>').val(entity.ChargedQty).trigger('changeValue');
        });

        //#region RevenueSharing
        $('#<%=lblRevenueSharingDt.ClientID %>').live('click', function () {
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('revenuesharing', filterExpression, function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                ontxtRevenueSharingCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            ontxtRevenueSharingCodeChanged($(this).val());
        });

        function ontxtRevenueSharingCodeChanged(value) {
            var filterExpression = "IsDeleted = 0 AND RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        };
        //#endregion

        //#endregion

        function onAfterSaveAddRecordEntryPopup() {
            ontxtRSSummaryNoChanged($('#<%=txtRSSummaryNo.ClientID %>').val());
        }

        function onAfterSaveEditRecordEntryPopup() {
            ontxtRSSummaryNoChanged($('#<%=txtRSSummaryNo.ClientID %>').val());
        }

        function onCbpEntryPopupViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#containerPopupEntryData').hide();
                    if ($('#<%=txtRSSummaryNo.ClientID %>').val() == "") {
                        ontxtRSSummaryNoChanged(param[2]);
                    } else {
                        ontxtRSSummaryNoChanged($('#<%=txtRSSummaryNo.ClientID %>').val());
                    }
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    ontxtRSSummaryNoChanged($('#<%=txtRSSummaryNo.ClientID %>').val());
            }

            hideLoadingPanel();
        };

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var rsSummaryID = $('#<%=hdnRSSummaryID.ClientID %>').val();
            if (rsSummaryID == '' || rsSummaryID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            else if (code == 'FN-00098') {
                filterExpression.text = "RSSummaryID = " + rsSummaryID;
                return true;
            }
            else if (code == 'FN-00228') {
                filterExpression.text = rsSummaryID;
                return true;
            }
            else {
                filterExpression.text = "RSSummaryID = " + rsSummaryID;
                return true;
            }
        }

    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnIsUsedButtonProcessAdjustment" runat="server" />
        <input type="hidden" id="hdnIsParamedicHasLinkedToCustomer" runat="server" />
        <input type="hidden" id="hdnGCTransactionStatus" runat="server" />
        <fieldset id="fsEntryPopup" style="margin: 0">
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td style="width: 150px;">
                                    <input type="hidden" id="hdnRSSummaryID" runat="server" />
                                    <input type="hidden" id="hdnRSSummaryNo" runat="server" />
                                    <label id="lblRSSummaryNo" class="lblLink">
                                        <%=GetLabel("Nomor Rekap")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRSSummaryNo" Width="220px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rblAdjustment" RepeatDirection="Horizontal" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="right">
                        <table>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Transaksi Bruto") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBrutoAmount" Width="150px" ReadOnly="true" class="txtCurrency"
                                        Style="text-align: right" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Rekap") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSummaryAmount" Width="150px" ReadOnly="true" class="txtCurrency"
                                        Style="text-align: right" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Penyesuaian") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSummaryAdjustmentAmount" Width="150px" ReadOnly="true"
                                        class="txtCurrency" Style="text-align: right" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Akhir") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSummaryEndAmount" Width="150px" ReadOnly="true"
                                        class="txtCurrency" Style="text-align: right" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                            <input type="hidden" id="hdnID" runat="server" value="" />
                            <div class="pageTitle">
                                <%=GetLabel("Tambah atau Edit Detail")%></div>
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 40%" />
                                    <col style="width: 60%" />
                                </colgroup>
                                <tr>
                                    <td valign="top" align="left">
                                        <table>
                                            <colgroup>
                                                <col style="width: 170px" />
                                                <col />
                                            </colgroup>
                                            <tr style="display: none" id="trAdjustmentTypeAdd">
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentTypeAdd">
                                                        <%=GetLabel("Jenis Penambahan") %></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeAdd" ID="cboAdjustmentTypeAdd"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                            <tr style="display: none" id="trAdjustmentTypeMin">
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentTypeMin">
                                                        <%=GetLabel("Jenis Pengurangan") %></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeMin" ID="cboAdjustmentTypeMin"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRevenueSharingFee" runat="server" /><%:GetLabel(" Diperhitungkan Pajak")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentAmountBruto">
                                                        <%=GetLabel("Adjustment Amount Bruto") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentAmountBruto" CssClass="txtCurrency"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentAmount">
                                                        <%=GetLabel("Adjustment Amount") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentAmount" CssClass="txtCurrency" Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory" id="lblRemarks">
                                                        <%=GetLabel("Remarks") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Rows="2" Width="350px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />&nbsp<input
                                                        type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" align="left">
                                        <table>
                                            <colgroup>
                                                <col style="width: 200px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnRevenueSharingID" runat="server" />
                                                    <label class="lblLink lblRevenueSharingDt" id="lblRevenueSharingDt" runat="server">
                                                        <%=GetLabel("RevenueSharing") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRevenueSharingCode" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtRevenueSharingName" Width="300px" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("RegNo | RegDate | DischDate") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRegistrationNo" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtRegistrationDate" ToolTip="yyyyMMdd" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtDischargeDate" ToolTip="yyyyMMdd" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ReceiptNo") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReceiptNo" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("InvoiceNo") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInvoiceNo" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ReferenceNo") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReferenceNo" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("BusinessPartnerName") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBusinessPartnerName" Width="320px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Patient") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMedicalNo" ToolTip="xx-xx-xx-xx" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtPatientName" Width="350px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("TransNo | TransDate") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTransactionNo" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtTransactionDate" ToolTip="dd-MM-yyyy" CssClass="datepicker"
                                                        Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ItemName1 | ChargedQty") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtItemName1" Width="320px" />
                                                    <asp:TextBox runat="server" ID="txtChargedQty" CssClass="txtCurrency" Width="150px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="trCbpView">
                    <td colspan="2">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <input type="hidden" bindingfield="ID" value="<%#: Eval("ID") %>" />
                                                        <input type="hidden" bindingfield="GCRSAdjustmentType" value="<%#: Eval("GCRSAdjustmentType")%>" />
                                                        <input type="hidden" bindingfield="AdjustmentAmount" value="<%#: Eval("AdjustmentAmount")%>" />
                                                        <input type="hidden" bindingfield="IsTaxed" value="<%#: Eval("IsTaxed")%>" />
                                                        <input type="hidden" bindingfield="Remarks" value="<%#: Eval("Remarks")%>" />
                                                        <input type="hidden" bindingfield="RSAdjustmentID" value="<%#: Eval("RSAdjustmentID")%>" />
                                                        <input type="hidden" bindingfield="RSAdjustmentDtID" value="<%#: Eval("RSAdjustmentDtID")%>" />
                                                        <input type="hidden" bindingfield="RevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                        <input type="hidden" bindingfield="RevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                        <input type="hidden" bindingfield="RevenueSharingName" value="<%#: Eval("RevenueSharingName")%>" />
                                                        <input type="hidden" bindingfield="RegistrationNo" value="<%#: Eval("RegistrationNo")%>" />
                                                        <input type="hidden" bindingfield="RegistrationDate" value="<%#: Eval("RegistrationDate")%>" />
                                                        <input type="hidden" bindingfield="DischargeDate" value="<%#: Eval("DischargeDate")%>" />
                                                        <input type="hidden" bindingfield="ReceiptNo" value="<%#: Eval("ReceiptNo")%>" />
                                                        <input type="hidden" bindingfield="InvoiceNo" value="<%#: Eval("InvoiceNo")%>" />
                                                        <input type="hidden" bindingfield="ReferenceNo" value="<%#: Eval("ReferenceNo")%>" />
                                                        <input type="hidden" bindingfield="BusinessPartnerName" value="<%#: Eval("BusinessPartnerName")%>" />
                                                        <input type="hidden" bindingfield="MedicalNo" value="<%#: Eval("MedicalNo")%>" />
                                                        <input type="hidden" bindingfield="PatientName" value="<%#: Eval("PatientName")%>" />
                                                        <input type="hidden" bindingfield="TransactionNo" value="<%#: Eval("TransactionNo")%>" />
                                                        <input type="hidden" bindingfield="cfTransactionDateInDatePickerFormat" value="<%#: Eval("cfTransactionDateInDatePickerFormat")%>" />
                                                        <input type="hidden" bindingfield="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                        <input type="hidden" bindingfield="ChargedQty" value="<%#: Eval("ChargedQty")%>" />
                                                        <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="RSAdjustmentType" HeaderText="Tipe" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <div style="height: 100px; overflow-y: auto">
                                                            <table style="vertical-align: top">
                                                                <colgroup>
                                                                    <col style="width: 150px" />
                                                                    <col style="width: 10px" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("Remark") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <b>
                                                                            <label class="lblNormal">
                                                                                <%#: Eval("Remarks")%></label></b>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("RevenueSharingCode").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("RevenueSharing") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%=GetLabel("[") %><%#: Eval("RevenueSharingCode")%><%=GetLabel("] ") %>
                                                                            <%#: Eval("RevenueSharingName")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("RegistrationNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("RegNo | RegDate | DischDate") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("RegistrationNo")%><%=GetLabel(" | ") %><%#: Eval("RegistrationDate")%><%=GetLabel(" | ") %><%#: Eval("DischargeDate")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("ReceiptNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("ReceiptNo") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("ReceiptNo")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("InvoiceNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("InvoiceNo") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("InvoiceNo")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("ReferenceNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("ReferenceNo") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("ReferenceNo")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("BusinessPartnerName").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("BusinessPartnerName") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("BusinessPartnerName")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("PatientName").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("Patient") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%=GetLabel("[") %><%#: Eval("MedicalNo")%><%=GetLabel("] ") %><%#: Eval("PatientName")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("TransactionNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("TransactionNo | TransactionDate") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("TransactionNo")%><%=GetLabel(" | ") %><%#: Eval("cfTransactionDateInString")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("ItemName1").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("ItemName1 | ChargedQty") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("ItemName1")%><%=GetLabel(" | ") %><%#: Eval("cfChargedQtyInString")%></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="AdjustmentAmountBRUTO" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" DataFormatString="{0:N2}"
                                                    HeaderText="Adjustment Amount Bruto" />
                                                <asp:BoundField DataField="AdjustmentAmount" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" DataFormatString="{0:N2}"
                                                    HeaderText="Adjustment Amount" />
                                                <asp:BoundField DataField="RSAdjustmentNo" HeaderText="No Penyesuaian" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                            <span class="lblLink" id="lblEntryPopupAddData" style="margin-right: 100px;">
                                <%= GetLabel("Tambah Data")%></span> <span class="lblLink" id="lblEntryARInvoiceParamedic"
                                    style="margin-right: 100px;">
                                    <%= GetLabel("Tambah Piutang Dokter")%></span> <span class="lblLink" id="lblCopyAdjustmentTransaction"
                                        style="margin-right: 100px;">
                                        <%= GetLabel("Salin Transaksi Penyesuaian")%></span>
                        </div>
                        <dxcp:ASPxCallbackPanel runat="server" ID="cbpEntryPopupView" ClientInstanceName="cbpEntryPopupView"
                            OnCallback="cbpEntryPopupView_Callback" ShowLoadingPanel="false">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpEntryPopupViewEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
