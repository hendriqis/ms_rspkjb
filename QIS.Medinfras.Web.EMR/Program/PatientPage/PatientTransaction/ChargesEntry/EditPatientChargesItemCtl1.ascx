<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPatientChargesItemCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.EditPatientChargesItemCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbSave.png")%>' alt="" /><div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        $('#<%=txtLineAmount.ClientID %>').attr('readonly', 'readonly');
    });

    $('#<%=chkIsVariable.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=txtTariff.ClientID %>').removeAttr('readonly');
            $('#<%=txtPatientTotal.ClientID %>').removeAttr('readonly');
            $('#<%=txtPayerTotal.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%=txtTariff.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtPatientTotal.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtPayerTotal.ClientID %>').attr('readonly', 'readonly');
        }

        calculateLineAmount();
    });

    $('#<%=chkIsCITO.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            var isCITOInPercentage = ($('#<%=hdnIsCITOInPercentageMaster.ClientID %>').val() == '1');
            var CITOAmount = parseFloat($('#<%=hdnCITOAmountMaster.ClientID %>').val());
            var CITO = 0;
            if (isCITOInPercentage) {
                var tariff = parseFloat($('#<%=txtTariff.ClientID %>').val().split(',').join(''));
                CITO = parseFloat((tariff * CITOAmount) / 100);
            }
            else {
                var qty = parseFloat($('#<%=txtQty.ClientID %>').val().split(',').join(''));
                CITO = parseFloat(CITOAmount * qty);
            }
            $('#<%=txtCITOAmount.ClientID %>').val(CITO).trigger('changeValue');
        }
        else {
            $('#<%=txtCITOAmount.ClientID %>').val('0').trigger('changeValue');
        }
        calculateLineAmount();
    });

    $('#<%=chkIsDiscount.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=txtDiscountPctAmountComp2.ClientID %>').removeAttr('readonly');
            $('#<%=txtDiscountAmountComp2.ClientID %>').removeAttr('readonly');
            cboDiscountReason.SetEnabled(true);
        }
        else {
            $('#<%=txtDiscountPctAmountComp2.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDiscountPctAmountComp2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDiscountAmountComp2.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDiscountAmountComp2.ClientID %>').attr('readonly', 'readonly');
            cboDiscountReason.SetEnabled(false);
        }
        calculateLineAmount();
    });

    $('#<%=chkIsDiscount1.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=txtDiscountPctAmountComp1.ClientID %>').removeAttr('readonly');
            $('#<%=txtDiscountAmountComp1.ClientID %>').removeAttr('readonly');
            cboDiscountReason.SetEnabled(true);
        }
        else {
            $('#<%=txtDiscountPctAmountComp1.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDiscountPctAmountComp1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtDiscountAmountComp1.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtDiscountAmountComp1.ClientID %>').attr('readonly', 'readonly');
            cboDiscountReason.SetEnabled(false);
        }
        calculateLineAmount();
    });

    $('#btnMPEntryProcess').live('click', function () {
        var message = "Save the changes for <b>" + $('#<%:txtItemName.ClientID %>').val() + "</b> ?";
        showToastConfirmation(message, function (result) {
            if (result) cbpPopupProcess.PerformCallback('process');
        });
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                showToast("Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshControl == 'function')
                    onRefreshControl();
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    $('#<%=txtQty.ClientID %>').live('change', function () {
        $(this).blur();

        var qty = parseFloat($('#<%=txtQty.ClientID %>').val());

        var isQtyChecked = "0";
        if ($('#<%=txtQty.ClientID %>').val().includes(".")) {
            var qtyCheckDesimalList = $('#<%=txtQty.ClientID %>').val().split(".");
            var qtyCheckDesimal = qtyCheckDesimalList[1];
            if (qtyCheckDesimal.length > 2) {
                isQtyChecked = "1";
            }
        }

        if (isQtyChecked == "1") {
            alert("Maksimal digit desimal belakang koma adalah 2 digit.");
            $('#<%=txtQty.ClientID %>').val("1");
        } else {
            calculateLineAmount();
        }
    });

    $('#<%=txtTariff.ClientID %>').live('change', function () {
        $(this).blur();
        calculateLineAmount();
    });

    $('#<%=txtDiscountPctAmountComp1.ClientID %>').live('change', function () {
        $(this).blur();
        var oIsDiscountApplyToTariffComp2Only = $('#<%=hdnIsDiscountApplyToTariffComp2Only.ClientID %>').val();

        var tariff = parseFloat(FormatMoneyToNumeric($('#<%=txtTariff.ClientID %>').val()));
        var tariffComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp1.ClientID %>').val()));
        var tariffComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp2.ClientID %>').val()));

        var discComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp1.ClientID %>').val()));

        var discPctComp1 = parseFloat($('#<%=txtDiscountPctAmountComp1.ClientID %>').val().replace('.00', '').split(',').join(''));
        if (discPctComp1 < 0 || discPctComp1 > 100) {
            $('#<%=txtDiscountPctAmountComp1.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtDiscountAmountComp1.ClientID %>').val(0).trigger('changeValue');
        }
        else {
            var discComp1 = ((tariffComp1 * discPctComp1) / 100).toFixed(2);
            $('#<%=txtDiscountAmountComp1.ClientID %>').val(discComp1).trigger('changeValue');
        }

        calculateLineAmount();

        event.preventDefault();
    });

    $('#<%=txtDiscountAmountComp1.ClientID %>').live('change', function () {
        $(this).blur();
        var oIsDiscountApplyToTariffComp2Only = $('#<%=hdnIsDiscountApplyToTariffComp2Only.ClientID %>').val();

        var tariff = parseFloat(FormatMoneyToNumeric($('#<%=txtTariff.ClientID %>').val()));
        var tariffComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp1.ClientID %>').val()));
        var tariffComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp2.ClientID %>').val()));

        var discComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp1.ClientID %>').val()));

        var discComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp1.ClientID %>').val()));
        if (discComp1 > tariffComp2) {
            $('#<%=txtDiscountPctAmountComp1.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtDiscountAmountComp1.ClientID %>').val(0).trigger('changeValue');
        }
        else {
            var discPctComp1 = ((discComp1 / tariffComp1) * 100).toFixed(2);
            $('#<%=txtDiscountPctAmountComp1.ClientID %>').val(discPctComp1).trigger('changeValue');
        }

        calculateLineAmount();

        event.preventDefault();
    });

    $('#<%=txtDiscountPctAmountComp2.ClientID %>').live('change', function () {
        $(this).blur();
        var oIsDiscountApplyToTariffComp2Only = $('#<%=hdnIsDiscountApplyToTariffComp2Only.ClientID %>').val();

        var tariff = parseFloat(FormatMoneyToNumeric($('#<%=txtTariff.ClientID %>').val()));
        var tariffComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp1.ClientID %>').val()));
        var tariffComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp2.ClientID %>').val()));

        var discComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp1.ClientID %>').val()));

        var discPctComp2 = parseFloat($('#<%=txtDiscountPctAmountComp2.ClientID %>').val().replace('.00', '').split(',').join(''));
        if (discPctComp2 < 0 || discPctComp2 > 100) {
            $('#<%=txtDiscountPctAmountComp2.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtDiscountAmountComp2.ClientID %>').val(0).trigger('changeValue');
        }
        else {
            var discComp2 = ((tariffComp2 * discPctComp2) / 100).toFixed(2);
            $('#<%=txtDiscountAmountComp2.ClientID %>').val(discComp2).trigger('changeValue');
        }
        calculateLineAmount();

        event.preventDefault();
    });

    $('#<%=txtDiscountAmountComp2.ClientID %>').live('change', function () {
        $(this).blur();
        var oIsDiscountApplyToTariffComp2Only = $('#<%=hdnIsDiscountApplyToTariffComp2Only.ClientID %>').val();

        var tariff = parseFloat(FormatMoneyToNumeric($('#<%=txtTariff.ClientID %>').val()));
        var tariffComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp1.ClientID %>').val()));
        var tariffComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp2.ClientID %>').val()));

        var discComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp1.ClientID %>').val()));

        var discComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp2.ClientID %>').val()));
        if (discComp2 > tariffComp2) {
            $('#<%=txtDiscountPctAmountComp2.ClientID %>').val(0).trigger('changeValue');
            $('#<%=txtDiscountAmountComp2.ClientID %>').val(0).trigger('changeValue');
        }
        else {
            var discPctComp2 = ((discComp2 / tariffComp2) * 100).toFixed(2);
            $('#<%=txtDiscountPctAmountComp2.ClientID %>').val(discPctComp2).trigger('changeValue');
        }

        calculateLineAmount();

        event.preventDefault();
    });

    function calculateLineAmount() {
        var tariff = parseFloat(FormatMoneyToNumeric($('#<%=txtTariff.ClientID %>').val()));
        var tariffComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp1.ClientID %>').val()));
        var tariffComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtTariffComp2.ClientID %>').val()));

        var cito = parseFloat(FormatMoneyToNumeric($('#<%=txtCITOAmount.ClientID %>').val()));

        var discComp1 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp1.ClientID %>').val()));
        var discComp2 = parseFloat(FormatMoneyToNumeric($('#<%=txtDiscountAmountComp2.ClientID %>').val()));
        var discount = discComp1 + discComp2;

        var qty = parseFloat($('#<%=txtQty.ClientID %>').val());

        var total = parseFloat((tariff * qty) + cito - (discount * qty));

        var coverageAmount = parseInt($('#<%=hdnCoverageAmount.ClientID %>').val());
        var isCoverageInPercentage = ($('#<%=hdnIsCoverageInPercentage.ClientID %>').val() == '1');
        var totalPayer = 0;

        if (isCoverageInPercentage)
            totalPayer = (total * coverageAmount) / 100;
        else {
            var qty = parseFloat($('#<%=txtQty.ClientID %>').val());
            totalPayer = coverageAmount * qty;
        }

        if (total > 0 && totalPayer > total) {
            totalPayer = total;
        }
        var totalPatient = total - totalPayer;

        $('#<%=txtLineAmount.ClientID %>').val(total).trigger('changeValue');
        $('#<%=txtPatientTotal.ClientID %>').val(totalPatient).trigger('changeValue');
        $('#<%=txtPayerTotal.ClientID %>').val(totalPayer).trigger('changeValue');
    }

</script>
<input type="hidden" runat="server" id="hdnTransactionID" value="" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" value="" id="hdnVisitID" runat="server" />
<input type="hidden" value="" id="hdnTransactionDate" runat="server" />
<input type="hidden" value="" id="hdnTransactionTime" runat="server" />
<input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnServiceUnitName" runat="server" />
<input type="hidden" value="" id="hdnItemID" runat="server" />
<input type="hidden" value="" id="hdnRevenueSharingID" runat="server" />
<input type="hidden" value="" id="hdnItemUnit" runat="server" />
<input type="hidden" value="" id="hdnCoverageAmount" runat="server" />
<input type="hidden" value="" id="hdnCITOAmount" runat="server" />
<input type="hidden" value="" id="hdnIsCITOInPercentage" runat="server" />
<input type="hidden" value="" id="hdnCITOAmountMaster" runat="server" />
<input type="hidden" value="" id="hdnIsCITOInPercentageMaster" runat="server" />
<input type="hidden" value="" id="hdnComplicationAmount" runat="server" />
<input type="hidden" value="" id="hdnIsComplicationInPercentage" runat="server" />
<input type="hidden" value="" id="hdnItemFilterExpression" runat="server" />
<input type="hidden" value="" id="hdnTariff" runat="server" />
<input type="hidden" value="" id="hdnTariffComp1" runat="server" />
<input type="hidden" value="" id="hdnTariffComp2" runat="server" />
<input type="hidden" value="" id="hdnTariffComp3" runat="server" />
<input type="hidden" value="" id="hdnBaseTariff" runat="server" />
<input type="hidden" value="" id="hdnBaseTariffComp1" runat="server" />
<input type="hidden" value="" id="hdnBaseTariffComp2" runat="server" />
<input type="hidden" value="" id="hdnBaseTariffComp3" runat="server" />
<input type="hidden" value="" id="hdnDiscountAmount" runat="server" />
<input type="hidden" value="" id="hdnDiscountAmount1" runat="server" />
<input type="hidden" value="" id="hdnDiscountAmount2" runat="server" />
<input type="hidden" value="" id="hdnDiscountAmount3" runat="server" />
<input type="hidden" value="" id="hdnMasterItemTariff" runat="server" />
<input type="hidden" value="" id="hdnMasterItemTariffComp1" runat="server" />
<input type="hidden" value="" id="hdnMasterItemTariffComp2" runat="server" />
<input type="hidden" value="" id="hdnMasterItemTariffComp3" runat="server" />
<input type="hidden" value="" id="hdnMasterItemBaseTariff" runat="server" />
<input type="hidden" value="" id="hdnMasterItemBaseTariffComp1" runat="server" />
<input type="hidden" value="" id="hdnMasterItemBaseTariffComp2" runat="server" />
<input type="hidden" value="" id="hdnMasterItemBaseTariffComp3" runat="server" />
<input type="hidden" value="" id="hdnMasterItemTariffCoverage" runat="server" />
<input type="hidden" value="" id="hdnMasterItemTariffDiscount" runat="server" />
<input type="hidden" value="" id="hdnIsDiscountInPercentage" runat="server" />
<input type="hidden" value="" id="hdnIsCoverageInPercentage" runat="server" />
<input type="hidden" id="hdnIsDiscountApplyToTariffComp2Only" runat="server" value="0" />
<input type="hidden" id="hdnIsAllowChangeChargesQty" runat="server" value="1" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<input type="hidden" id="hdnIsAllowPreviewTariffEditCtl" runat="server" value="0" />
<div>
    <div>
        <table class="tblEntryDetail" style="width: 100%">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col style="width: 80px" />
                <col style="width: 20px" />
                <col style="width: 180px" />
                <col style="width: 100px" />
                <col style="width: 120px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory" id="lblServiceItem">
                        <%=GetLabel("Item")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <input type="hidden" value="" id="hdnServicePhysicianID" runat="server" />
                    <label class="lblMandatory" id="lblPhysician">
                        <%=GetLabel("Dokter")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtParamedicCode" ReadOnly="true" Width="100%" runat="server" />
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                    </label>
                </td>
                <td colspan="6">
                    <input type="hidden" id="hdnDefaultTariffComp" runat="server" value="1" />
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 20px" />
                            <col style="width: 100px" />
                            <col style="width: 20px" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsVariable" runat="server" />
                            </td>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Variable Tariff")%></label>
                            </td>
                            <td style="display: none">
                                <asp:CheckBox ID="chkIsUnbilledItem" runat="server" />
                            </td>
                            <td style="display: none" class="tdLabel">
                                <label>
                                    <%=GetLabel("Tidak Ditagihkan")%></label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Kelas Tagihan")%></label>
                </td>
                <td colspan="4">
                    <dxe:ASPxComboBox ID="cboChargeClassID" ClientInstanceName="cboChargeClassID" Width="100%"
                        runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td id="tdTariffLabel" runat="server">
                    <label>
                        <%=GetLabel("Harga Satuan")%></label>
                </td>
                <td id="tdTariff" runat="server">
                    <asp:TextBox ID="txtTariff" Width="100%" CssClass="txtCurrency" runat="server" ReadOnly="true" />
                </td>
                <td class="tdLabel" style="padding-right: 5px; text-align: right" id="tdCITOAmountLabel" runat="server">
                    <label>
                        <%=GetLabel("CITO")%></label>
                </td>
                <td>
                    <asp:CheckBox ID="chkIsCITO" runat="server" />
                </td>
                <td id="tdCITOAmount" runat="server">
                    <asp:TextBox ID="txtCITOAmount" ReadOnly="true" Width="100%" CssClass="txtCurrency"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td id="tdTariffComp2Label" runat="server">
                    <label>
                        <%=GetLabel("Tariff Komp Dokter")%></label>
                </td>
                <td id="tdTariffComp2" runat="server">
                    <asp:TextBox ID="txtTariffComp2" Width="100%" CssClass="txtCurrency" runat="server"
                        ReadOnly="true" />
                </td>
                <td class="tdLabel" colspan="2" id="tdTariffComp1Label" runat="server" style="padding-left: 5px;">
                    <label>
                        <%=GetLabel("Tariff Komp RS")%></label>
                </td>
                <td id="tdTariffComp1" runat="server" style="padding-right: 5px;">
                    <asp:TextBox ID="txtTariffComp1" Width="100%" CssClass="txtCurrency" runat="server"
                        ReadOnly="true" />
                </td>
                <td class="tdLabel" style="padding-left: 5px; text-align: right" id="tdPatientTotalLabel" runat="server">
                    <label>
                        <%=GetLabel("Pasien")%></label>
                </td>
                <td id="tdPatientTotal" runat="server">
                    <asp:TextBox ID="txtPatientTotal" ReadOnly="true" Width="100%" CssClass="txtCurrency"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Diskon Komp Dokter")%></label>
                    <asp:CheckBox ID="chkIsDiscount" runat="server" />
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="2">
                        <tr>
                            <td style="width: 40px">
                                <asp:TextBox ID="txtDiscountPctAmountComp2" Width="100%" CssClass="txtCurrency" runat="server" />
                            </td>
                            <td style="width: 10px; padding-left: 2px">
                                <label>
                                    <%=GetLabel("%")%></label>
                            </td>
                            <td id="tdDiscountAmountComp2" runat="server">
                                <asp:TextBox ID="txtDiscountAmountComp2" Width="100%" CssClass="txtCurrency" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="tdLabel" style="padding-left: 5px;" colspan="2">
                    <label>
                        <%=GetLabel("Diskon Komp RS")%></label>
                        <asp:CheckBox ID="chkIsDiscount1" runat="server" />
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="2">
                        <tr>
                            <td style="width: 40px">
                                <asp:TextBox ID="txtDiscountPctAmountComp1" Width="100%" CssClass="txtCurrency" runat="server" />
                            </td>
                            <td style="width: 10px; padding-left: 2px">
                                <label>
                                    <%=GetLabel("%")%></label>
                            </td>
                            <td id="tdDiscountAmountComp1" runat="server">
                                <asp:TextBox ID="txtDiscountAmountComp1" Width="100%" CssClass="txtCurrency" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Jumlah")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtQty" Width="100%" CssClass="number" runat="server" />
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td class="tdLabel" style="padding-right: 5px; text-align: right" id="tdPayerTotalLabel" runat="server">
                    <label>
                        <%=GetLabel("Instansi")%></label>
                </td>
                <td id="tdPayerTotal" runat="server">
                    <asp:TextBox ID="txtPayerTotal" ReadOnly="true" Width="100%" CssClass="txtCurrency"
                        runat="server" />
                </td>
            </tr>
            <tr style="display: none">
                <td>
                </td>
                <td>
                </td>
                <td class="tdLabel" style="padding-right: 5px; text-align: right" id="tdComplicationAmountLabel" runat="server">
                    <label>
                        <%=GetLabel("Penyulit")%></label>
                </td>
                <td>
                    <asp:CheckBox ID="chkIsComplication" runat="server" />
                </td>
                <td id="tdComplicationAmount" runat="server">
                    <asp:TextBox ID="txtComplicationAmount" ReadOnly="true" Width="100px" CssClass="txtCurrency"
                        runat="server" />
                </td>
                <td class="tdLabel" style="padding-right: 5px; text-align: right">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Alasan Discount")%></label>
                </td>
                <td colspan="4">
                    <dxe:ASPxComboBox ID="cboDiscountReason" ClientInstanceName="cboDiscountReason" Width="100%"
                        runat="server">
                    </dxe:ASPxComboBox>
                </td>
                <td class="tdLabel" style="padding-right: 5px; text-align: right" id="tdLineAmountLabel" runat="server">
                    <label>
                        <%=GetLabel("Total")%></label>
                </td>
                <td id="tdLineAmount" runat="server">
                    <asp:TextBox ID="txtLineAmount" ReadOnly="true" Width="100%" CssClass="txtCurrency"
                        runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
