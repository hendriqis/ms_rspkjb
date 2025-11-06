<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="TestPartnerTransactionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TestPartnerTransactionEntry" %>

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
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCustomToolbarVisibility();
            calculateTotal();

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            if ($('#<%=hdnTransactionID.ClientID %>').val() == null || $('#<%=hdnTransactionID.ClientID %>').val() == "" || $('#<%=hdnTransactionID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtTransactionDate.ClientID %>');
                $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            }

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                    cbpProcess.PerformCallback('save');
                }
                else {
                    return false;
                }
            });
        }

        function setCustomToolbarVisibility() {
            if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
                $('#lblAddData').show();
            } else {
                $('#lblAddData').hide();
            }

            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showDeleteConfirmation(function (data) {
                    var param = 'voidbyreason;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        //#region Add, Edit, Delete
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnIsEdit.ClientID %>').val('0');
                $('#<%=hdnDtID.ClientID %>').val('');
                $('#<%=hdnRegistrationID.ClientID %>').val('');
                $('#<%=hdnPatientChargesID.ClientID %>').val('');
                $('#<%=hdnPatientChargesDtID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');

                $('#<%=txtRegistrationNo.ClientID %>').val('');
                $('#<%=txtPatientName.ClientID %>').val('');
                $('#<%=txtPatientChargesNo.ClientID %>').val('');
                $('#<%=txtPatientChargesDate.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtLineAmount.ClientID %>').val('0');
                $('#<%=txtPartnerAmount.ClientID %>').val('0');

                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $('#<%=hdnIsEdit.ClientID %>').val('1');
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnDtID.ClientID %>').val(entity.ID);

            $('#<%=hdnRegistrationID.ClientID %>').val(entity.RegistrationID);
            $('#<%=hdnPatientChargesID.ClientID %>').val(entity.PatientChargesID);
            $('#<%=hdnPatientChargesDtID.ClientID %>').val(entity.PatientChargesDtID);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);

            $('#<%=txtRegistrationNo.ClientID %>').val(entity.RegistrationNo);
            $('#<%=txtPatientName.ClientID %>').val("(" + entity.MedicalNo + ") " + entity.PatientName);
            $('#<%=txtPatientChargesNo.ClientID %>').val(entity.PatientChargesTransactionNo);
            $('#<%=txtPatientChargesDate.ClientID %>').val(entity.cfPatientChargesTransactionDateInString);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtLineAmount.ClientID %>').val(entity.LineAmount).trigger('changeValue');
            $('#<%=txtPartnerAmount.ClientID %>').val(entity.PartnerAmount).trigger('changeValue');

            $('#containerEntry').show();
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnDtID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });
        //#endregion

        //#region TransactionNo
        $('#lblTransactionNo.lblLink').live('click', function () {
            var filterExpression = "1=1";
            openSearchDialog('testpartnertransactionhd', filterExpression, function (value) {
                $('#<%=txtTransactionNo.ClientID %>').val(value);
                ontxtTransactionNoChanged(value);
            });
        });

        $('#<%=txtTransactionNo.ClientID %>').live('change', function () {
            ontxtTransactionNoChanged($(this).val());
        });

        function ontxtTransactionNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        //#region Business Partner / Test Partner
        function onBusinessPartnerFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND GCBusinessPartnerType IN ('" + Constant.BusinessPartnerType.RUJUKAN_KE_PIHAK_KETIGA + "')";
            return filterExpression;
        }

        $('#<%=lblBusinessPartner.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('businesspartners', onBusinessPartnerFilterExpression(), function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                ontxtBusinessPartnerCodeChanged(value);
            });
        });

        $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
            ontxtBusinessPartnerCodeChanged($(this).val());
        });

        function ontxtBusinessPartnerCodeChanged(value) {
            var filterExpression = onBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetvBusinessPartners1List', filterExpression, function (result) {
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

        //#region Product Line
        function getProductLineFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=lblProductLine.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                $('#<%=txtProductLineCode.ClientID %>').val(value);
                onTxtProductLineCodeChanged(value);
            });
        });

        $('#<%=txtProductLineCode.ClientID %>').live('change', function () {
            onTxtProductLineCodeChanged($(this).val());
        });

        function onTxtProductLineCodeChanged(value) {
            var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
            Methods.getObject('GetProductLineList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                    $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                }
                else {
                    $('#<%=hdnProductLineID.ClientID %>').val('');
                    $('#<%=txtProductLineCode.ClientID %>').val('');
                    $('#<%=txtProductLineName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Charges Item
        function onChargesItemFilterExpression() {
            var filterExpression = "GCTransactionStatus = 'X121^005'";
            filterExpression += " AND ID NOT IN (SELECT dt.PatientChargesDtID FROM TestPartnerTransactionDt dt WHERE dt.IsDeleted = 0 AND dt.TransactionID IN (SELECT hd.TransactionID FROM TestPartnerTransactionHd hd WHERE hd.GCTransactionStatus <> 'X121^999'))";

            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('patientchargesdt15', onChargesItemFilterExpression(), function (value) {
                onPatientChargesDtIDChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            var filterCharges = onChargesItemFilterExpression() + " AND ItemCode = '" + $(this).val() + "'";
            Methods.getObject('GetvPatientChargesDt15List', filterCharges, function (result) {
                if (result != null) {
                    onPatientChargesDtIDChanged(result.PatientChargesDtID);
                }
            });
        });

        function onPatientChargesDtIDChanged(value) {
            var filterCharges = onChargesItemFilterExpression() + " AND ID = " + value;
            Methods.getObject('GetvPatientChargesDt15List', filterCharges, function (result) {
                if (result != null) {
                    $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                    $('#<%=hdnPatientChargesID.ClientID %>').val(result.TransactionID);
                    $('#<%=hdnPatientChargesDtID.ClientID %>').val(result.ID);
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);

                    $('#<%=txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                    $('#<%=txtPatientName.ClientID %>').val(result.cfPatientName);
                    $('#<%=txtPatientChargesNo.ClientID %>').val(result.TransactionNo);
                    $('#<%=txtPatientChargesDate.ClientID %>').val(result.cfTransactionDateInString);
                    $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=txtLineAmount.ClientID %>').val(result.LineAmount).trigger('changeValue');

                    var value = new Date(result.cfTransactionDateInString);
                    var valueDay = value.getDate();
                    if ((1 + value.getDate()) < 10) {
                        valueDay = "0" + valueDay;
                    }
                    var valueMonth = (1 + value.getMonth());
                    if ((1 + value.getMonth()) < 10) {
                        valueMonth = "0" + valueMonth;
                    }
                    var valueYear = value.getFullYear();
                    var chargesHdDate = valueDay + "-" + valueMonth + "-" + valueYear;
                    var chargesHdDateIn112 = chargesHdDate.substring(10, 6) + chargesHdDate.substring(5, 3) + chargesHdDate.substring(0, 2);
                    var filterRefItem = "IsDeleted = 0 AND BusinessPartnerID = " + $('#<%=hdnBusinessPartnerID.ClientID %>').val()
                                            + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val()
                                            + " AND CONVERT(VARCHAR(8), StartDate, 112) <= '" + chargesHdDateIn112 + "'";

                    var filterMaxID = filterRefItem;

                    filterRefItem += " AND ID = (SELECT MAX(tpit.ID) FROM vTestPartnerItemTariff tpit WITH(NOLOCK) WHERE " + filterMaxID + ")";

                    Methods.getObject('GetvTestPartnerItemTariffList', filterRefItem, function (resultTPI) {
                        if (resultTPI != null) {
                            $('#<%=txtPartnerAmount.ClientID %>').val(resultTPI.Amount).trigger('changeValue');
                        } else {
                            $('#<%=txtPartnerAmount.ClientID %>').val('0').trigger('changeValue');
                        }
                    });
                }
                else {
                    $('#<%=hdnRegistrationID.ClientID %>').val('');
                    $('#<%=hdnPatientChargesID.ClientID %>').val('');
                    $('#<%=hdnPatientChargesDtID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');

                    $('#<%=txtRegistrationNo.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                    $('#<%=txtPatientChargesNo.ClientID %>').val('');
                    $('#<%=txtPatientChargesDate.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtLineAmount.ClientID %>').val('0');
                    $('#<%=txtPartnerAmount.ClientID %>').val('0');
                }
            });
        }
        //#endregion    

        //#region VAT

        $('#<%:chkVAT.ClientID %>').die('change');
        $('#<%:chkVAT.ClientID %>').live('change', function () {
            var ppnAllowChanged = $('#<%:hdnIsPpnAllowChanged.ClientID %>').val();
            var defaultVATPct = $('#<%:hdnVATPercentage.ClientID %>').val();

            if (ppnAllowChanged == "1") {
                if ($(this).is(':checked')) {
                    if (parseFloat($('#<%=txtVATPct.ClientID%>').val()) == 0) {
                        $('#<%=txtVATPct.ClientID%>').val(defaultVATPct);
                    }

                    $('#<%=txtVATPct.ClientID%>').removeAttr('readonly');
                    $('#<%=txtVATAmount.ClientID%>').attr('readonly', 'readonly');

                    calculateVAT("fromPctg");
                } else {
                    $('#<%=txtVATPct.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtVATAmount.ClientID%>').removeAttr('readonly');

                    calculateVAT("fromTxt");
                }
            } else {
                if (parseFloat($('#<%=txtVATPct.ClientID%>').val()) == 0) {
                    $('#<%=txtVATPct.ClientID%>').val(defaultVATPct);
                }
                calculateVAT("fromPctg");
            }
        });

        $('#<%=txtVATPct.ClientID%>').die('change');
        $('#<%=txtVATPct.ClientID%>').live('change', function () {
            if ($('#<%=chkVAT.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculateVAT("fromPctg");
            } else {
                $(this).trigger('changeValue');
                calculateVAT("fromTxt");
            }
        });

        $('#<%=txtVATAmount.ClientID%>').die('change');
        $('#<%=txtVATAmount.ClientID%>').live('change', function () {
            if ($('#<%=chkVAT.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculateVAT("fromPctg");
            } else {
                $(this).trigger('changeValue');
                calculateVAT("fromTxt");
            }
        });

        function calculateVAT(kode) {
            var totalPA = parseFloat($('#<%=txtTotalPartnerAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

            if (kode == "fromPctg") {
                var pctg = parseFloat($('#<%=txtVATPct.ClientID %>').val().replace('.00', '').split(',').join(''));
                var totalVAT = totalPA * (pctg / 100);

                $('#<%=txtVATAmount.ClientID %>').val(totalVAT).trigger('changeValue');
                $('#<%=txtVATPct.ClientID %>').val(pctg).trigger('changeValue');

            }
            else if (kode == "fromTxt") {
                var VAT = parseFloat($('#<%=txtVATAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
                var pctg = VAT / (totalPA / 100);

                $('#<%=txtVATAmount.ClientID %>').val(VAT).trigger('changeValue');
                $('#<%=txtVATPct.ClientID %>').val(pctg).trigger('changeValue');

            }

            calculateTotal();
        }

        //#endregion

        //#region PPH

        $('#<%:chkPPHPercent.ClientID %>').die('change');
        $('#<%:chkPPHPercent.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtPPHPct.ClientID%>').removeAttr('readonly');
                $('#<%=txtPPHAmount.ClientID%>').attr('readonly', 'readonly');

                calculatePPH("fromPctg");
            } else {
                $('#<%=txtPPHPct.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtPPHAmount.ClientID%>').removeAttr('readonly');

                calculatePPH("fromTxt");
            }
        });

        $('#<%=txtPPHPct.ClientID%>').die('change');
        $('#<%=txtPPHPct.ClientID%>').live('change', function () {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
            }
        });

        $('#<%=txtPPHAmount.ClientID%>').die('change');
        $('#<%=txtPPHAmount.ClientID%>').live('change', function () {
            if ($('#<%=chkPPHPercent.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculatePPH("fromPctg");
            } else {
                $(this).trigger('changeValue');
                calculatePPH("fromTxt");
            }
        });

        function calculatePPH(kode) {
            var totalPA = parseFloat($('#<%=txtTotalPartnerAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            if (kode == "fromPctg") {
                var pctg = parseFloat($('#<%=txtPPHPct.ClientID %>').val().replace('.00', '').split(',').join(''));
                var totalPPH = totalPA * (pctg / 100);

                $('#<%=txtPPHAmount.ClientID %>').val(totalPPH).trigger('changeValue');
                $('#<%=txtPPHPct.ClientID %>').val(pctg).trigger('changeValue');

            }
            else if (kode == "fromTxt") {
                var pph = parseFloat($('#<%=txtPPHAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
                var pctg = pph / (totalPA / 100);

                $('#<%=txtPPHAmount.ClientID %>').val(pph).trigger('changeValue');
                $('#<%=txtPPHPct.ClientID %>').val(pctg).trigger('changeValue');

            }

            calculateTotal();
        }

        //#endregion

        function calculateTotal() {
            var TotalPartnerAmount = parseFloat($('#<%=txtTotalPartnerAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

            if ($('#<%=chkVAT.ClientID %>').is(':checked')) {
                var PPN = parseFloat($('#<%=txtVATPct.ClientID %>').val().replace('.00', '').split(',').join('')) / 100 * parseFloat(TotalPartnerAmount);
                $('#<%=txtVATAmount.ClientID %>').val(PPN).trigger('changeValue');
            }
            else {
                $('#<%=txtVATAmount.ClientID %>').val('0').trigger('changeValue');
            }
            var PPN = parseFloat($('#<%=txtVATAmount.ClientID %>').attr('hiddenVal'));

            var PPH = parseFloat($('#<%=txtPPHAmount.ClientID %>').val().replace('.00', '').split(',').join(''));

            var NettAmount = TotalPartnerAmount + PPN + PPH;
            $('#<%=txtNettPartnerTransactionAmount.ClientID %>').val(NettAmount).trigger('changeValue');
        }

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var oTestPartnerTransactionID = s.cpTestPartnerTransactionID;
                    onAfterSaveRecordDtSuccess(oTestPartnerTransactionID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    cbpView.PerformCallback('refresh');
                }
            }
            calculateTotal();

            if ($('#<%=txtTransactionNo.ClientID %>').val() != '') {
                onLoadObject($('#<%=txtTransactionNo.ClientID %>').val());
            }
        }

        function onCbpViewEndCallback(s) {
            calculateTotal();
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function onAfterSaveRecordDtSuccess(TestPartnerTransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                $('#<%=hdnTransactionID.ClientID %>').val(TestPartnerTransactionID);
                var filterExpression = 'TransactionID = ' + TestPartnerTransactionID;
                Methods.getObject('GetTestPartnerTransactionHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    onLoadObject(result.TransactionNo);
                });
            }
            else {
                cbpView.PerformCallback('refresh');
            }
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsAllowVoid" runat="server" value="" />
    <input type="hidden" id="hdnIsEdit" runat="server" value="0" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="0" />
    <input type="hidden" id="hdnIsUsedProductLine" runat="server" value="0" />
    <input type="hidden" id="hdnIsPpnAllowChanged" runat="server" value="" />
    <input type="hidden" id="hdnVATPercentageFromSetvar" runat="server" value="" />
    <input type="hidden" id="hdnVATPercentage" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 60%" />
            <col style="width: 40%" />
        </colgroup>
        <tr id="trTransactionHd">
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblTransactionNo">
                                <%=GetLabel("No. Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory" runat="server" id="lblTransactionDate">
                                <%=GetLabel("Tanggal Transaksi")%></label>
                        </td>
                        <td style="padding-right: 1px; width: 140px">
                            <asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
                            <label class="lblMandatory lblLink" runat="server" id="lblBusinessPartner">
                                <%=GetLabel("Test Partner")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBusinessPartnerCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trProductLine" runat="server" style="display: none">
                        <td>
                            <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                            <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                <%=GetLabel("Product Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Catatan") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trTransactionDt">
            <td colspan="2">
                <div id="containerEntry" style="margin-top: 4px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Edit atau Tambah Data")%></div>
                    <fieldset id="fsTrxPopup" style="margin: 0">
                        <input type="hidden" value="" id="hdnDtID" runat="server" />
                        <table style="width: 100%" class="tblEntryDetail">
                            <colgroup>
                                <col style="width: 60%" />
                                <col style="width: 40%" />
                            </colgroup>
                            <tr>
                                <td style="vertical-align: top">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 150px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <input type="hidden" id="hdnPatientChargesID" runat="server" value="" />
                                                <input type="hidden" id="hdnPatientChargesDtID" runat="server" value="" />
                                                <input type="hidden" id="hdnItemID" runat="server" value="" />
                                                <label class="lblMandatory lblLink" id="lblItem">
                                                    <%=GetLabel("Item")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
                                                <label class="lblNormal" id="lblRegistration">
                                                    <%=GetLabel("No. Registrasi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientName" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("No - Tanggal Transaksi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientChargesNo" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientChargesDate" Width="120px" runat="server" ReadOnly="true"
                                                    Style="text-align: center" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("Nilai Transaksi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLineAmount" Width="100%" runat="server" ReadOnly="true" Style="text-align: right"
                                                    CssClass="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("Nilai Rujukan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPartnerAmount" Width="100%" runat="server" ReadOnly="true" Style="text-align: right"
                                                    CssClass="txtCurrency" />
                                            </td>
                                        </tr>
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
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="display: none">
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
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("TransactionID") %>" bindingfield="TransactionID" />
                                                <input type="hidden" value="<%#:Eval("RegistrationID") %>" bindingfield="RegistrationID" />
                                                <input type="hidden" value="<%#:Eval("RegistrationNo") %>" bindingfield="RegistrationNo" />
                                                <input type="hidden" value="<%#:Eval("MRN") %>" bindingfield="MRN" />
                                                <input type="hidden" value="<%#:Eval("MedicalNo") %>" bindingfield="MedicalNo" />
                                                <input type="hidden" value="<%#:Eval("PatientName") %>" bindingfield="PatientName" />
                                                <input type="hidden" value="<%#:Eval("PatientChargesDtID") %>" bindingfield="PatientChargesDtID" />
                                                <input type="hidden" value="<%#:Eval("PatientChargesID") %>" bindingfield="PatientChargesID" />
                                                <input type="hidden" value="<%#:Eval("PatientChargesTransactionNo") %>" bindingfield="PatientChargesTransactionNo" />
                                                <input type="hidden" value="<%#:Eval("PatientChargesTransactionDate") %>" bindingfield="PatientChargesTransactionDate" />
                                                <input type="hidden" value="<%#:Eval("cfPatientChargesTransactionDateInString") %>"
                                                    bindingfield="cfPatientChargesTransactionDateInString" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                <input type="hidden" value="<%#:Eval("PartnerAmount") %>" bindingfield="PartnerAmount" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Registration Info")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <label>
                                                    <%#:Eval("RegistrationNo")%></label>
                                                <br />
                                                <label style="font-weight: bold">
                                                    (<%#:Eval("MedicalNo")%>)
                                                    <%#:Eval("PatientName")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Transaction Info")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <label>
                                                    <b>
                                                        <%#:Eval("ItemName1")%></b> <i>(<%#:Eval("ItemCode")%>)</i></label>
                                                <br />
                                                <label>
                                                    <%#:Eval("PatientChargesTransactionNo")%>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfLineAmountInString" HeaderText="Nilai Transaksi" HeaderStyle-Width="170px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="cfPartnerAmountInString" HeaderText="Nilai Rujukan" HeaderStyle-Width="170px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
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
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px; margin-left: 300px">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                </div>
            </td>
        </tr>
        <tr id="trSummaryHd">
            <td>
            </td>
            <td valign="top">
                <table style="width: 100%;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 30px" />
                        <col style="width: 100px" />
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr id="trTotalOrder" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jumlah Nilai Partner")%></label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPartnerAmount" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("PPN ")%>
                            </label>
                        </td>
                        <td id="tdVATPercent" runat="server">
                            <asp:CheckBox ID="chkVAT" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVATPct" CssClass="txtCurrency" ReadOnly="true" Width="50px" runat="server" />
                            <%=GetLabel("%")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVATAmount" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("PPh ")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPPHPercent" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPPHPct" ReadOnly="true" CssClass="txtCurrency" Width="50px" runat="server" />
                            <%=GetLabel("%")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPPHAmount" ReadOnly="true" CssClass="txtCurrency" Width="180px"
                                runat="server" hiddenVal="0" />
                        </td>
                    </tr>
                    <tr id="trTotalOrderSaldo" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Nilai Partner")%></label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNettPartnerTransactionAmount" CssClass="txtCurrency" ReadOnly="true"
                                Width="180px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <div style="width: 50%;">
                        <div class="lblComponent" style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Informasi Transaksi") %></div>
                        <div style="background-color: #EAEAEA;">
                            <table width="100%" cellpadding="0" cellspacing="0">
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
                                        <%=GetLabel("Disetujui Oleh")%>
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
                                        <%=GetLabel("Disetujui Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidReason" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Alasan Batal")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidReason">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidBy" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Dibatalkan Oleh")%>
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
                                        <%=GetLabel("Dibatalkan Pada")%>
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
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
