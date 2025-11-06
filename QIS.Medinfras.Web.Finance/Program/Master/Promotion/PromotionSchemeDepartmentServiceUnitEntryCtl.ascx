<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionSchemeDepartmentServiceUnitEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.PromotionSchemeDepartmentServiceUnitEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_promotionTypeDepartmentEntryCtl">
    $(function () {
        $('#<%=txtTariff1.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariff2.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariff3.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariff1Comp1.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariff1Comp2.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariff1Comp3.ClientID %>').attr("disabled", "disabled");

        $('#<%=chkIsUsePromotionPrice1.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=txtTariff1.ClientID %>').removeAttr("disabled");
                $('#<%=txtTariff1Comp1.ClientID %>').removeAttr("disabled");
                $('#<%=txtTariff1Comp2.ClientID %>').removeAttr("disabled");
                $('#<%=txtTariff1Comp3.ClientID %>').removeAttr("disabled");
            }
            else {
                $('#<%=txtTariff1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariff1Comp1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariff1Comp2.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariff1Comp3.ClientID %>').attr("disabled", "disabled");

                $('#<%=txtTariff1.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTariff1Comp1.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTariff1Comp2.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTariff1Comp3.ClientID %>').val('0').trigger('changeValue');
            }
        });

        $('#<%=chkIsUsePromotionPrice2.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=txtTariff2.ClientID %>').removeAttr("disabled");
            }
            else {
                $('#<%=txtTariff2.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariff2.ClientID %>').val('0').trigger('changeValue');
            }
        });

        $('#<%=chkIsUsePromotionPrice3.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=txtTariff3.ClientID %>').removeAttr("disabled");
            }
            else {
                $('#<%=txtTariff3.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariff3.ClientID %>').val('0').trigger('changeValue');
            }
        });
    });


    $('#<%=txtTariff1.ClientID %>').change(function () {
        $(this).blur();
        var total = parseFloat($(this).attr('hiddenVal'));

        if (isNaN(total)) {
            $('#<%=txtTariff1.ClientID %>').val('0').trigger('changeValue');
            total = 0;
        }
        else {
            if (total < 0) {
                $('#<%:txtTariff1.ClientID %>').val('0').trigger('changeValue');
                total = 0;
            }
        }

        var component1 = parseFloat($('#<%=txtTariff1Comp1.ClientID %>').attr('hiddenVal'));
        var component2 = parseFloat($('#<%=txtTariff1Comp2.ClientID %>').attr('hiddenVal'));
        var component3 = parseFloat($('#<%=txtTariff1Comp3.ClientID %>').attr('hiddenVal'));

        if (isNaN(component1)) component1 = 0;
        if (isNaN(component2)) component2 = 0;
        if (isNaN(component3)) component3 = 0;

        var component1 = total - (component2 + component3);
        if (component1 < 0 || component1 == null) {
            $('#<%=txtTariff1Comp1.ClientID %>').val(total).trigger('changeValue');
            $('#<%=txtTariff1Comp2.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtTariff1Comp3.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            $('#<%=txtTariff1Comp1.ClientID %>').val(component1).trigger('changeValue');
            $('#<%=txtTariff1Comp2.ClientID %>').val(component2).trigger('changeValue');
            $('#<%=txtTariff1Comp3.ClientID %>').val(component3).trigger('changeValue');
        }
    });

    $('#<%=txtTariff1Comp1.ClientID %>').change(function () {
        $(this).blur();
        var tariffComp1 = parseFloat($(this).attr('hiddenVal'));

        if (isNaN(tariffComp1)) {
            $('#<%:txtTariff1Comp1.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (tariffComp1 < 0) {
                $('#<%:txtTariff1Comp1.ClientID %>').val('0').trigger('changeValue');
            }
        }

        calculateTariffTotal();
    });

    $('#<%=txtTariff1Comp2.ClientID %>').change(function () {
        $(this).blur();
        var tariffComp2 = parseFloat($(this).attr('hiddenVal'));
        
        if (isNaN(tariffComp2)) {
            $('#<%:txtTariff1Comp2.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (tariffComp2 < 0) {
                $('#<%:txtTariff1Comp2.ClientID %>').val('0').trigger('changeValue');
            }
        }

        calculateTariffTotal();
    });

    $('#<%=txtTariff1Comp3.ClientID %>').change(function () {
        $(this).blur();
        var tariffComp3 = parseFloat($(this).attr('hiddenVal'));

        if (isNaN(tariffComp3)) {
            $('#<%:txtTariff1Comp3.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (tariffComp3 < 0) {
                $('#<%:txtTariff1Comp3.ClientID %>').val('0').trigger('changeValue');
            }
        }

        calculateTariffTotal();
    });

    function calculateTariffTotal() {
        var total = 0;
        var comp1 = parseFloat($('#<%=txtTariff1Comp1.ClientID %>').attr('hiddenVal'));
        var comp2 = parseFloat($('#<%=txtTariff1Comp2.ClientID %>').attr('hiddenVal'));
        var comp3 = parseFloat($('#<%=txtTariff1Comp3.ClientID %>').attr('hiddenVal'));

        if (isNaN(comp1)) comp1 = 0;
        if (isNaN(comp2)) comp2 = 0;
        if (isNaN(comp3)) comp3 = 0;

        if (comp1 < 0) comp1 = 0;
        if (comp2 < 0) comp2 = 0;
        if (comp3 < 0) comp3 = 0;

        total = comp1 + comp2 + comp3;
        $('#<%=txtTariff1.ClientID %>').val(total).trigger('changeValue');
    }

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtServiceUnitCode.ClientID %>').val('');
        $('#<%=txtServiceUnitName.ClientID %>').val('');

        $('#<%=chkIsUsePromotionPrice1.ClientID %>').prop('checked', false);
        $('#<%=txtTariff1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtTariff1Comp1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtTariff1Comp2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtTariff1Comp3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');

        $('#<%=chkIsUsePromotionPrice2.ClientID %>').prop('checked', false);
        $('#<%=txtTariff2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');

        $('#<%=chkIsUsePromotionPrice3.ClientID %>').prop('checked', false);
        $('#<%=txtTariff3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
        
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        displayConfirmationMessageBox('DELETE','Are you sure want to delete?', function (result) {
            if (result) {
                $('#<%=hdnID.ClientID %>').val(ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        var serviceUnitID = $row.find('.hdnServiceUnitID').val();
        var serviceUnitCode = $row.find('.hdnServiceUnitCode').val();
        var serviceUnitName = $row.find('.tdServiceUnitName').html();

        var revenueSharingID = $row.find('.hdnRevenueSharingID').val();
        var revenueSharingCode = $row.find('.hdnRevenueSharingCode').val();
        var revenueSharingName = $row.find('.tdRevenueSharingName').html();

        var tariff1 = $row.find('.tdTariff1').html();
        var tariff1Comp1 = $row.find('.hdnTariff1Comp1').val();
        var tariff1Comp2 = $row.find('.hdnTariff1Comp2').val();
        var tariff1Comp3 = $row.find('.hdnTariff1Comp3').val();
        var discountAmount1 = $row.find('.tdDiscountAmount1').html();

        var tariff2 = $row.find('.tdTariff2').html();
        var discountAmount2 = $row.find('.tdDiscountAmount2').html();

        var tariff3 = $row.find('.tdTariff3').html();
        var discountAmount3 = $row.find('.tdDiscountAmount3').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnServiceUnitID.ClientID %>').val(serviceUnitID);
        $('#<%=txtServiceUnitCode.ClientID %>').val(serviceUnitCode);
        $('#<%=txtServiceUnitName.ClientID %>').val(serviceUnitName);

        $('#<%=chkIsUsePromotionPrice1.ClientID %>').prop('checked', tariff1 != '-');
        if (tariff1 != '-') {
            $('#<%=txtTariff1.ClientID %>').removeAttr("disabled");
            $('#<%=txtTariff1Comp1.ClientID %>').removeAttr("disabled");
            $('#<%=txtTariff1Comp2.ClientID %>').removeAttr("disabled");
            $('#<%=txtTariff1Comp3.ClientID %>').removeAttr("disabled");
        }
        $('#<%=txtTariff1.ClientID %>').val(tariff1.replace('-', '0')).trigger('changeValue');
        $('#<%=txtTariff1Comp1.ClientID %>').val(tariff1Comp1.replace('-', '0')).trigger('changeValue');
        $('#<%=txtTariff1Comp2.ClientID %>').val(tariff1Comp2.replace('-', '0')).trigger('changeValue');
        $('#<%=txtTariff1Comp3.ClientID %>').val(tariff1Comp3.replace('-', '0')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', discountAmount1.indexOf("%") > -1);
        $('#<%=txtDiscountAmount1.ClientID %>').val(discountAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=chkIsUsePromotionPrice2.ClientID %>').prop('checked', tariff2 != '-');
        if (tariff2 != '-') {
            $('#<%=txtTariff2.ClientID %>').removeAttr("disabled");
        }
        $('#<%=txtTariff2.ClientID %>').val(tariff2.replace('-', '0')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', discountAmount2.indexOf("%") > -1);
        $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=chkIsUsePromotionPrice3.ClientID %>').prop('checked', tariff3 != '-');
        if (tariff3 != '-') {
            $('#<%=txtTariff3.ClientID %>').removeAttr("disabled");
        }
        $('#<%=txtTariff3.ClientID %>').val(tariff3.replace('-', '0')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3.ClientID %>').prop('checked', discountAmount3.indexOf("%") > -1);
        $('#<%=txtDiscountAmount3.ClientID %>').val(discountAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=hdnRevenueSharingID.ClientID %>').val(revenueSharingID);
        $('#<%=txtRevenueSharingCode.ClientID %>').val(revenueSharingCode);
        $('#<%=txtRevenueSharingName.ClientID %>').val(revenueSharingName);

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                displayErrorMessageBox('SAVE', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                displayErrorMessageBox('DELETE', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    //#region Service Unit
    function onGetServiceUnitFilterExpression() {
        var filterExpression = "DepartmentID = '" + $('#<%=hdnDepartmentID.ClientID %>').val() + "' AND ServiceUnitID NOT IN (SELECT ServiceUnitID FROM PromotionSchemeServiceUnit WHERE PromotionSchemeID = " + $('#<%=hdnPromotionSchemeID.ClientID %>').val() + " AND IsDeleted = 0) AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblServiceUnit.lblLink').click(function () {
        openSearchDialog('serviceunit', onGetServiceUnitFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            onTxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
        onTxtServiceUnitCodeChanged($(this).val());
    });

    function onTxtServiceUnitCodeChanged(value) {
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
        });
    }
    //#endregion

        //#region Revenue Sharing
        function onGetRevenueSharingMasterCodeFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblRevenueSharing.lblLink').live('click', function () {
            openSearchDialog('revenuesharing', onGetRevenueSharingMasterCodeFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                onTxtRevenueSharingMasterCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            onTxtRevenueSharingMasterCodeChanged($(this).val());
        });

        function onTxtRevenueSharingMasterCodeChanged(value) {
            var filterExpression = onGetRevenueSharingMasterCodeFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
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
        }
        //#endregion

        $('#<%:txtTariff2.ClientID %>').live('change', function () {
            var value = $('#<%:txtTariff2.ClientID %>').val();
            if (isNaN(value)) {
                $('#<%:txtTariff2.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtTariff2.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtTariff3.ClientID %>').live('change', function () {
            var value = $('#<%:txtTariff3.ClientID %>').val();
            if (isNaN(value)) {
                $('#<%:txtTariff3.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtTariff3.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtDiscountAmount1.ClientID %>').live('change', function () {
            var value = $('#<%:txtDiscountAmount1.ClientID %>').val();
            var isPercentage = $('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:chkIsDiscountInPercentage1.ClientID %>').live('change', function () {
            var value = $('#<%:txtDiscountAmount1.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkIsDiscountInPercentage1.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtDiscountAmount2.ClientID %>').live('change', function () {
            var value = $('#<%:txtDiscountAmount2.ClientID %>').val();
            var isPercentage = $('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:chkIsDiscountInPercentage2.ClientID %>').live('change', function () {
            var value = $('#<%:txtDiscountAmount2.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkIsDiscountInPercentage2.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtDiscountAmount3.ClientID %>').live('change', function () {
            var value = $('#<%:txtDiscountAmount3.ClientID %>').val();
            var isPercentage = $('#<%:chkIsDiscountInPercentage3.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:chkIsDiscountInPercentage3.ClientID %>').live('change', function () {
            var value = $('#<%:txtDiscountAmount3.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkIsDiscountInPercentage3.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnPromotionSchemeID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnServiceUnitID" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Skema Promo")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtPromotionScheme" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Instalasi")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtDepartment" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <input type="hidden" id="hdnTariffComp1Text" runat="server" value="" />
                    <input type="hidden" id="hdnTariffComp2Text" runat="server" value="" />
                    <input type="hidden" id="hdnTariffComp3Text" runat="server" value="" />
                    <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width:180px"/>
                                <col style="width:100px"/>
                                <col style="width:3px"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
                                <td><asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="100%" runat="server" /></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table cellpadding="1" cellspacing="0">
                                        <colgroup>
                                            <col style="width:180px"/>
                                            <col style="width:20px"/>
                                            <col style="width:140px"/>
                                            <col style="width:20px"/>
                                            <col style="width:140px"/>
                                            <col style="width:20px"/>
                                            <col style="width:140px"/>
                                        </colgroup>
                                        <tr>
                                            <td colspan="6">

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jenis Item")%></label></td>
                                            <td colspan="2" style="padding-bottom:2px"><div class="lblComponent"><%=GetLabel("Tindakan / Pelayanan")%></div></td>
                                            <td colspan="2" style="padding-bottom:2px"><div class="lblComponent"><%=GetLabel("Obat dan Persediaan Medis")%></div></td>
                                            <td colspan="2" style="padding-bottom:2px"><div class="lblComponent"><%=GetLabel("Barang Umum")%></div></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Penggunaan Harga Khusus")%></label></td>
                                            <td style="text-align:center"><asp:CheckBox ID="chkIsUsePromotionPrice1" runat="server" /></td>
                                            <td><asp:TextBox ID="txtTariff1" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td style="text-align:center"><asp:CheckBox ID="chkIsUsePromotionPrice2" runat="server" /></td>
                                            <td><asp:TextBox ID="txtTariff2" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td style="text-align:center"><asp:CheckBox ID="chkIsUsePromotionPrice3" runat="server" /></td>
                                            <td><asp:TextBox ID="txtTariff3" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="text-align:right"><label class="lblNormal"><%=GetTariffComponent1Text()%></label></td>
                                            <td>&nbsp</td>
                                            <td><asp:TextBox ID="txtTariff1Comp1" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="text-align:right"><label class="lblNormal"><%=GetTariffComponent2Text()%></label></td>
                                            <td>&nbsp</td>
                                            <td><asp:TextBox ID="txtTariff1Comp2" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="text-align:right"><label class="lblNormal"><%=GetTariffComponent3Text()%></label></td>
                                            <td>&nbsp</td>
                                            <td><asp:TextBox ID="txtTariff1Comp3" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                            <td>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td><div class="lblComponent"><%=GetLabel("%")%></div></td>
                                            <td><div class="lblComponent" style="text-align:right"><%=GetLabel("Nilai")%></div></td>
                                            <td><div class="lblComponent"><%=GetLabel("%")%></div></td>
                                            <td><div class="lblComponent" style="text-align:right" ><%=GetLabel("Nilai")%></div></td>
                                            <td><div class="lblComponent"><%=GetLabel("%")%></div></td>
                                            <td><div class="lblComponent" style="text-align:right"><%=GetLabel("Nilai")%></div></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Pemberian Diskon")%></label></td>
                                            <td style="text-align:center"><asp:CheckBox ID="chkIsDiscountInPercentage1" runat="server" /></td>
                                            <td><asp:TextBox ID="txtDiscountAmount1" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td style="text-align:center"><asp:CheckBox ID="chkIsDiscountInPercentage2" runat="server" /></td>
                                            <td><asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                            <td style="text-align:center"><asp:CheckBox ID="chkIsDiscountInPercentage3" runat="server" /></td>
                                            <td><asp:TextBox ID="txtDiscountAmount3" CssClass="txtCurrency" runat="server" Width="100%" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style='display:none'>
                                <td class="tdLabel"><label class="lblLink" id="lblRevenueSharing"><%=GetLabel("Formula Jasa Medis")%></label></td>
                                <td><asp:TextBox ID="txtRevenueSharingCode" CssClass="required" Width="100%" runat="server" /></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" /></td>
                            </tr>
                        </table>
                        <div style="padding-bottom:10px">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' class="btnEntryPopupSave w3-btn w3-hover-blue" />
                                        </td>
                                        <td>
                                            <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="left"><%=GetLabel("Unit Pelayanan")%></th>
                                                <th style="width:150px" rowspan="2" align="left"><%=GetLabel("Formula Jasa Medis")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Tindakan / Pelayanan")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Peralatan dan Bahan Medis")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Barang Umum")%></th>    
                                            </tr>
                                            <tr>  
                                                <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                 
                                                <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    
                                                <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th></th>                                          
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="12">
                                                    <%=GetLabel("Konfigurasi Promo untuk Instalasi tidak tersedia")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2" align="center">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="left"><%=GetLabel("Unit Pelayanan")%></th>
                                                <th style="width:150px" rowspan="2" align="left"><%=GetLabel("Formula Jasa Medis")%></th>
                                                <th colspan="2" align="center"><%=GetLabel("Tindakan / Pelayanan")%></th>
                                                <th colspan="2" align="center"><%=GetLabel("Peralatan dan Bahan Medis")%></th>
                                                <th colspan="3" align="center"><%=GetLabel("Barang Umum")%></th>    
                                            </tr>
                                            <tr>  
                                                <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                 
                                                <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    
                                                <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>    
                                                <th></th>                                         
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt=""  /></td>
                                                        <td style="width:1px">&nbsp;</td>
                                                        <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt=""  /></td>
                                                    </tr>
                                                </table>

                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnServiceUnitID" value="<%#: Eval("ServiceUnitID")%>" />
                                                <input type="hidden" class="hdnServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                <input type="hidden" class="hdnRevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                <input type="hidden" class="hdnRevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                <input type="hidden" class="hdnTariff1Comp1" value="<%#: Eval("Tariff1Comp1")%>" />
                                                <input type="hidden" class="hdnTariff1Comp2" value="<%#: Eval("Tariff1Comp2")%>" />
                                                <input type="hidden" class="hdnTariff1Comp3" value="<%#: Eval("Tariff1Comp3")%>" />
                                            </td>
                                            <td class="tdServiceUnitName"><%#: Eval("ServiceUnitName")%></td> 
                                            <td class="tdRevenueSharingName"><%#: Eval("RevenueSharingName")%></td> 
                                            <td class="tdTariff1" align="right"><%#: Eval("DisplayTariff1")%></td>
                                            <td class="tdDiscountAmount1" align="right"><%#: Eval("DisplayDiscountAmount1")%></td>
                                            <td class="tdTariff2" align="right"><%#: Eval("DisplayTariff2")%></td>
                                            <td class="tdDiscountAmount2" align="right"><%#: Eval("DisplayDiscountAmount2")%></td>
                                            <td class="tdTariff3" align="right"><%#: Eval("DisplayTariff3")%></td>
                                            <td class="tdDiscountAmount3" align="right"><%#: Eval("DisplayDiscountAmount3")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>

