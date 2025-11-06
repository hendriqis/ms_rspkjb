<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoverageTypeServiceUnitUploadCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.CoverageTypeServiceUnitUploadCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnServiceUnitID.ClientID %>').val('');
        $('#<%=txtServiceUnitCode.ClientID %>').val('');
        $('#<%=txtServiceUnitName.ClientID %>').val('');

        $('#<%=chkIsMarkupInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1Comp1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount1Comp1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1Comp2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount1Comp2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1Comp3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount1Comp3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage1.ClientID %>').prop('checked', false);
        $('#<%=txtCashBackAmount1.ClientID %>').val('0').trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2Comp1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount2Comp1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2Comp2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount2Comp2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2Comp3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount2Comp3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage2.ClientID %>').prop('checked', false);
        $('#<%=txtCashBackAmount2.ClientID %>').val('0').trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3Comp1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount3Comp1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3Comp2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount3Comp2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3Comp3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount3Comp3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage3.ClientID %>').prop('checked', false);
        $('#<%=txtCashBackAmount3.ClientID %>').val('0').trigger('changeValue');
        
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
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
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

        var markupAmount1 = $row.find('.tdMarkupAmount1').html();
        var discountAmount1 = $row.find('.tdDiscountAmount1').html();
        var discountAmount1Comp1 = $row.find('.DiscountAmount1Comp1').val();
        var discountAmount1Comp2 = $row.find('.DiscountAmount1Comp2').val();
        var discountAmount1Comp3 = $row.find('.DiscountAmount1Comp3').val();
        var coverageAmount1 = $row.find('.tdCoverageAmount1').html();
        var cashBackAmount1 = $row.find('.tdCashBackAmount1').html();

        var markupAmount2 = $row.find('.tdMarkupAmount2').html();
        var discountAmount2 = $row.find('.tdDiscountAmount2').html();
        var discountAmount2Comp1 = $row.find('.DiscountAmount2Comp1').val();
        var discountAmount2Comp2 = $row.find('.DiscountAmount2Comp2').val();
        var discountAmount2Comp3 = $row.find('.DiscountAmount2Comp3').val();
        var coverageAmount2 = $row.find('.tdCoverageAmount2').html();
        var cashBackAmount2 = $row.find('.tdCashBackAmount2').html();

        var markupAmount3 = $row.find('.tdMarkupAmount3').html();
        var discountAmount3 = $row.find('.tdDiscountAmount3').html();
        var discountAmount3Comp1 = $row.find('.DiscountAmount3Comp1').val();
        var discountAmount3Comp2 = $row.find('.DiscountAmount3Comp2').val();
        var discountAmount3Comp3 = $row.find('.DiscountAmount3Comp3').val();
        var coverageAmount3 = $row.find('.tdCoverageAmount3').html();
        var cashBackAmount3 = $row.find('.tdCashBackAmount3').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnServiceUnitID.ClientID %>').val(serviceUnitID);
        $('#<%=txtServiceUnitCode.ClientID %>').val(serviceUnitCode);
        $('#<%=txtServiceUnitName.ClientID %>').val(serviceUnitName);

        $('#<%=chkIsMarkupInPercentage1.ClientID %>').prop('checked', markupAmount1.indexOf("%") > -1);
        $('#<%=txtMarkupAmount1.ClientID %>').val(markupAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1.ClientID %>').prop('checked', discountAmount1.indexOf("%") > -1);
        $('#<%=txtDiscountAmount1.ClientID %>').val(discountAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1Comp1.ClientID %>').prop('checked', discountAmount1Comp1.indexOf("%") > -1);
        $('#<%=txtDiscountAmount1Comp1.ClientID %>').val(discountAmount1Comp1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1Comp2.ClientID %>').prop('checked', discountAmount1Comp2.indexOf("%") > -1);
        $('#<%=txtDiscountAmount1Comp2.ClientID %>').val(discountAmount1Comp2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage1Comp3.ClientID %>').prop('checked', discountAmount1Comp3.indexOf("%") > -1);
        $('#<%=txtDiscountAmount1Comp3.ClientID %>').val(discountAmount1Comp3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage1.ClientID %>').prop('checked', coverageAmount1.indexOf("%") > -1);
        $('#<%=txtCoverageAmount1.ClientID %>').val(coverageAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage1.ClientID %>').prop('checked', cashBackAmount1.indexOf("%") > -1);
        $('#<%=txtCashBackAmount1.ClientID %>').val(cashBackAmount1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage2.ClientID %>').prop('checked', markupAmount2.indexOf("%") > -1);
        $('#<%=txtMarkupAmount2.ClientID %>').val(markupAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2.ClientID %>').prop('checked', discountAmount2.indexOf("%") > -1);
        $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2Comp1.ClientID %>').prop('checked', discountAmount2Comp1.indexOf("%") > -1);
        $('#<%=txtDiscountAmount2Comp1.ClientID %>').val(discountAmount2Comp1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2Comp2.ClientID %>').prop('checked', discountAmount2Comp2.indexOf("%") > -1);
        $('#<%=txtDiscountAmount2Comp2.ClientID %>').val(discountAmount2Comp2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage2Comp3.ClientID %>').prop('checked', discountAmount2Comp3.indexOf("%") > -1);
        $('#<%=txtDiscountAmount2Comp3.ClientID %>').val(discountAmount2Comp3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage2.ClientID %>').prop('checked', coverageAmount2.indexOf("%") > -1);
        $('#<%=txtCoverageAmount2.ClientID %>').val(coverageAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage2.ClientID %>').prop('checked', cashBackAmount2.indexOf("%") > -1);
        $('#<%=txtCashBackAmount2.ClientID %>').val(cashBackAmount2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=chkIsMarkupInPercentage3.ClientID %>').prop('checked', markupAmount3.indexOf("%") > -1);
        $('#<%=txtMarkupAmount3.ClientID %>').val(markupAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3.ClientID %>').prop('checked', discountAmount3.indexOf("%") > -1);
        $('#<%=txtDiscountAmount3.ClientID %>').val(discountAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3Comp1.ClientID %>').prop('checked', discountAmount3Comp1.indexOf("%") > -1);
        $('#<%=txtDiscountAmount3Comp1.ClientID %>').val(discountAmount3Comp1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3Comp2.ClientID %>').prop('checked', discountAmount3Comp2.indexOf("%") > -1);
        $('#<%=txtDiscountAmount3Comp2.ClientID %>').val(discountAmount3Comp2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage3Comp3.ClientID %>').prop('checked', discountAmount3Comp3.indexOf("%") > -1);
        $('#<%=txtDiscountAmount3Comp3.ClientID %>').val(discountAmount3Comp3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage3.ClientID %>').prop('checked', coverageAmount3.indexOf("%") > -1);
        $('#<%=txtCoverageAmount3.ClientID %>').val(coverageAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage3.ClientID %>').prop('checked', cashBackAmount3.indexOf("%") > -1);
        $('#<%=txtCashBackAmount3.ClientID %>').val(cashBackAmount3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    //#region Service Unit
    function onGetCoverageTypeServiceUnitCodeFilterExpression() {
        var filterExpression = "DepartmentID = '" + $('#<%=hdnDepartmentID.ClientID %>').val() + "' AND ServiceUnitID NOT IN (SELECT ServiceUnitID FROM CoverageTypeServiceUnit WHERE CoverageTypeID = " + $('#<%=hdnCoverageTypeID.ClientID %>').val() + " AND IsDeleted = 0) AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblServiceUnit.lblLink').click(function () {
        openSearchDialog('serviceunit', onGetCoverageTypeServiceUnitCodeFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            onTxtCoverageTypeServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
        onTxtCoverageTypeServiceUnitCodeChanged($(this).val());
    });

    function onTxtCoverageTypeServiceUnitCodeChanged(value) {
        var filterExpression = onGetCoverageTypeServiceUnitCodeFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
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
</script>

<div style="height:450px; overflow-y:auto">
    <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Tipe Jaminan Unit Pelayanan ")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe Jaminan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCoverageType" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Instalasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDepartment" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 600px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnServiceUnitID" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="100%" runat="server" />
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
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td colspan="3" style="padding-bottom: 2px">
                                                <div class="lblComponent">
                                                    <%=GetLabel("Pelayanan")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="3" style="padding-bottom: 2px">
                                                <div class="lblComponent">
                                                    <%=GetLabel("Peralatan dan Bahan Medis")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="3" style="padding-bottom: 2px">
                                                <div class="lblComponent">
                                                    <%=GetLabel("Barang Umum")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("%")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Jumlah")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("%")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Jumlah")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("%")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Jumlah")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kenaikan Harga")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsMarkupInPercentage1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMarkupAmount1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsMarkupInPercentage2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMarkupAmount2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsMarkupInPercentage3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMarkupAmount3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
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
                                        <%=GetLabel("Diskon")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-style: italic; font-size: small; padding-left: 15px">
                                        <%=GetLabel("Diskon Komponen 1")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage1Comp1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount1Comp1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage2Comp1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount2Comp1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage3Comp1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount3Comp1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-style: italic; font-size: small; padding-left: 15px">
                                        <%=GetLabel("Diskon Komponen 2")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage1Comp2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount1Comp2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage2Comp2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount2Comp2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage3Comp2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount3Comp2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-style: italic; font-size: small; padding-left: 15px">
                                        <%=GetLabel("Diskon Komponen 3")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage1Comp3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount1Comp3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage2Comp3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount2Comp3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage3Comp3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount3Comp3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
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
                                        <%=GetLabel("Tipe Jaminan")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCoverageInPercentage1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCoverageAmount1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCoverageInPercentage2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCoverageAmount2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCoverageInPercentage3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCoverageAmount3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
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
                                        <%=GetLabel("Cashback")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                            <col style="width: 3px" />
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCashBackInPercentage1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCashBackAmount1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCashBackInPercentage2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCashBackAmount2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCashBackInPercentage3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCashBackAmount3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
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
                                                <th style="width:70px" rowspan="2" align="center">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="lft"><%=GetLabel("Unit Pelayanan")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Pelayanan")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Peralatan dan Bahan Medis")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Barang Umum")%></th>
                                            </tr>
                                            <tr>  
                                                <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                 
                                                <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                    
                                                <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="14">
                                                    <%=GetLabel("Data Tidak Tersedia")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2" align="center">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="left"><%=GetLabel("Unit Pelayanan")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Pelayanan")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Peralatan dan Bahan Medis")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Barang Umum")%></th>    
                                            </tr>
                                            <tr>  
                                                <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                 
                                                <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                    
                                                <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
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
                                                <input type="hidden" class="DiscountAmount1Comp1" value="<%#: Eval("DisplayDiscountAmount1Comp1")%>" />
                                                <input type="hidden" class="DiscountAmount1Comp2" value="<%#: Eval("DisplayDiscountAmount1Comp2")%>" />
                                                <input type="hidden" class="DiscountAmount1Comp3" value="<%#: Eval("DisplayDiscountAmount1Comp3")%>" />
                                                <input type="hidden" class="DiscountAmount2Comp1" value="<%#: Eval("DisplayDiscountAmount2Comp1")%>" />
                                                <input type="hidden" class="DiscountAmount2Comp2" value="<%#: Eval("DisplayDiscountAmount2Comp2")%>" />
                                                <input type="hidden" class="DiscountAmount2Comp3" value="<%#: Eval("DisplayDiscountAmount2Comp3")%>" />
                                                <input type="hidden" class="DiscountAmount3Comp1" value="<%#: Eval("DisplayDiscountAmount3Comp1")%>" />
                                                <input type="hidden" class="DiscountAmount3Comp2" value="<%#: Eval("DisplayDiscountAmount3Comp2")%>" />
                                                <input type="hidden" class="DiscountAmount3Comp3" value="<%#: Eval("DisplayDiscountAmount3Comp3")%>" />
                                            </td>
                                            <td class="tdServiceUnitName"><%#: Eval("ServiceUnitName")%></td>

                                            <td class="tdMarkupAmount1" align="right"><%#: Eval("DisplayMarkupAmount1")%></td>
                                            <td class="tdDiscountAmount1" align="right"><%#: Eval("DisplayDiscountAmount1")%></td>
                                            <td class="tdCoverageAmount1" align="right"><%#: Eval("DisplayCoverageAmount1")%></td>
                                            <td class="tdCashBackAmount1" align="right"><%#: Eval("DisplayCashBackAmount1")%></td>

                                            <td class="tdMarkupAmount2" align="right"><%#: Eval("DisplayMarkupAmount2")%></td>
                                            <td class="tdDiscountAmount2" align="right"><%#: Eval("DisplayDiscountAmount2")%></td>
                                            <td class="tdCoverageAmount2" align="right"><%#: Eval("DisplayCoverageAmount2")%></td>
                                            <td class="tdCashBackAmount2" align="right"><%#: Eval("DisplayCashBackAmount2")%></td>

                                            <td class="tdMarkupAmount3" align="right"><%#: Eval("DisplayMarkupAmount3")%></td>
                                            <td class="tdDiscountAmount3" align="right"><%#: Eval("DisplayDiscountAmount3")%></td>
                                            <td class="tdCoverageAmount3" align="right"><%#: Eval("DisplayCoverageAmount3")%></td>
                                            <td class="tdCashBackAmount3" align="right"><%#: Eval("DisplayCashBackAmount3")%></td>
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
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

