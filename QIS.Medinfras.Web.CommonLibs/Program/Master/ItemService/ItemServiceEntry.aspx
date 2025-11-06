<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ItemServiceEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Harga Variabel
            var variableTariffComp1 = $('#<%=chkIsAllowVariableTariffComp1.ClientID %>').is(":checked");
            var variableTariffComp2 = $('#<%=chkIsAllowVariableTariffComp2.ClientID %>').is(":checked");
            var variableTariffComp3 = $('#<%=chkIsAllowVariableTariffComp3.ClientID %>').is(":checked");
            if (variableTariffComp1 || variableTariffComp2 || variableTariffComp3) {
                $('#<%=chkIsAllowVariable.ClientID %>').prop("checked", true);
            }
            //#endregion

            //#region Diskon
            var discountTariffComp1 = $('#<%=chkIsAllowDiscountTariffComp1.ClientID %>').is(":checked");
            var discountTariffComp2 = $('#<%=chkIsAllowDiscountTariffComp2.ClientID %>').is(":checked");
            var discountTariffComp3 = $('#<%=chkIsAllowDiscountTariffComp3.ClientID %>').is(":checked");
            if (discountTariffComp1 || discountTariffComp2 || discountTariffComp3) {
                $('#<%=chkIsAllowDiscount.ClientID %>').prop("checked", true);
            }
            //#endregion

            //#region Item Group
            $('#lblItemGroup.lblLink').click(function () {
                var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                if ($('#<%=hdnGCSubItemType.ClientID %>').val() == '') {
                    filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND GCSubItemType IS NULL AND IsDeleted = 0";
                }
                else {
                    filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND GCSubItemType = '" + $('#<%=hdnGCSubItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                }
                openSearchDialog('itemgroup1', filterExpression, function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "ItemGroupCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Product Line
            $('#lblProductLine.lblLink').click(function () {
                var filterPL = "IsDeleted = 0 AND GCItemType NOT IN ('X001^002','X001^003','X001^008','X001^009')";
                openSearchDialog('productline', filterPL, function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = "ProductLineCode = '" + value + "'";
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

            //#region Revenue Sharing
            $('#lblRevenueSharing.lblLink').click(function () {
                openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
                    $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                    onTxtRevenueSharingCodeChanged(value);
                });
            });

            $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
                onTxtRevenueSharingCodeChanged($(this).val());
            });

            function onTxtRevenueSharingCodeChanged(value) {
                var filterExpression = "RevenueSharingCode = '" + value + "'";
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

            //#region Default Paramedic
            $('#lblDefaultParamedic.lblLink').click(function () {
                openSearchDialog('paramedic', 'IsDeleted = 0', function (value) {
                    $('#<%=txtDefaultParamedicCode.ClientID %>').val(value);
                    onDefaultParamedicCodeChanged(value);
                });
            });

            $('#<%=txtDefaultParamedicCode.ClientID %>').change(function () {
                onDefaultParamedicCodeChanged($(this).val());
            });

            function onDefaultParamedicCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDefaultParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtDefaultParamedicName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnDefaultParamedicID.ClientID %>').val('');
                        $('#<%=txtDefaultParamedicCode.ClientID %>').val('');
                        $('#<%=txtDefaultParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Billing Group
            $('#lblBillingGroup.lblLink').click(function () {
                openSearchDialog('billinggroup', 'IsDeleted = 0', function (value) {
                    $('#<%=txtBillingGroupCode.ClientID %>').val(value);
                    onBillingGroupCodeChanged(value);
                });
            });

            $('#<%=txtBillingGroupCode.ClientID %>').change(function () {
                onBillingGroupCodeChanged($(this).val());
            });

            function onBillingGroupCodeChanged(value) {
                var filterExpression = "BillingGroupCode = '" + value + "'";
                Methods.getObject('GetBillingGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBillingGroupID.ClientID %>').val(result.BillingGroupID);
                        $('#<%=txtBillingGroupName.ClientID %>').val(result.BillingGroupName1);
                    }
                    else {
                        $('#<%=hdnBillingGroupID.ClientID %>').val('');
                        $('#<%=txtBillingGroupCode.ClientID %>').val('');
                        $('#<%=txtBillingGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region E-Klaim Parameter
            $('#<%:lblEKlaimParameter.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('eklaimparameter', 'IsDeleted = 0', function (value) {
                    $('#<%=txtEKlaimParameterCode.ClientID %>').val(value);
                    ontxtEKlaimParameterCodeChanged(value);
                });
            });

            $('#<%=txtEKlaimParameterCode.ClientID %>').change(function () {
                ontxtEKlaimParameterCodeChanged($(this).val());
            });

            function ontxtEKlaimParameterCodeChanged(value) {
                var filterExpression = "EKlaimParameterCode = '" + value + "'";
                Methods.getObject('GetEKlaimParameterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnEKlaimParameterID.ClientID %>').val(result.EKlaimParameterID);
                        $('#<%=txtEKlaimParameterName.ClientID %>').val(result.EKlaimParameterName);
                    }
                    else {
                        $('#<%=hdnEKlaimParameterID.ClientID %>').val('');
                        $('#<%=txtEKlaimParameterCode.ClientID %>').val('');
                        $('#<%=txtEKlaimParameterName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Modality
            $('#lblModalityID.lblLink').live('click', function () {
                openSearchDialog('modality', '1=1', function (value) {
                    $('#<%=txtModalityCode.ClientID %>').val(value);
                    onTxtModalityCodeChanged(value);
                });
            });

            $('#<%=txtModalityCode.ClientID %>').live('change', function () {
                onTxtModalityCodeChanged($(this).val());
            });

            function onTxtModalityCodeChanged(value) {
                var filterExpression = "ModalityCode = '" + value + "'";
                Methods.getObject('GetModalityList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnModalityID.ClientID %>').val(result.ModalityID);
                        $('#<%=txtModalityName.ClientID %>').val(result.ModalityName);
                        cboModalities.SetValue(result.GCModality);
                    }
                    else {
                        $('#<%=hdnModalityID.ClientID %>').val('');
                        $('#<%=txtModalityCode.ClientID %>').val('');
                        $('#<%=txtModalityName.ClientID %>').val('');
                        cboModalities.SetValue('');
                    }
                });
            }

            //#endregion

            //#region GCRLReportGroup
            $('#lblGCRLReportGroup.lblLink').click(function () {
                var filter = "ParentID = 'X378' AND IsDeleted = 0";
                openSearchDialog('rlreportgroup', filter, function (value) {
                    $('#<%=txtGCRLReportGroupCode.ClientID %>').val(value);
                    onTxtGCRLReportGroupCodeChanged(value);
                });
            });

            $('#<%=txtGCRLReportGroupCode.ClientID %>').change(function () {
                onTxtGCRLReportGroupCodeChanged($(this).val());
            });

            function onTxtGCRLReportGroupCodeChanged(value) {
                var filterExpression = "StandardCodeID = '" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGCRLReportGroup.ClientID %>').val(result.StandardCodeID);
                        $('#<%=txtGCRLReportGroupCode.ClientID %>').val(result.StandardCodeID);
                        $('#<%=txtGCRLReportGroupName.ClientID %>').val(result.StandardCodeName);
                    }
                    else {
                        $('#<%=hdnGCRLReportGroup.ClientID %>').val('');
                        $('#<%=txtGCRLReportGroupCode.ClientID %>').val('');
                        $('#<%=txtGCRLReportGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=chkIsPackageItem.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=chkIsPackageAllInOne.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsPackageAllInOne.ClientID %>').removeAttr('checked');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('disabled');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('checked');

                    $('#<%=chkIsPackageBalanceItem.ClientID %>').removeAttr('checked');
                    $('#<%=txtDefaultPackageBalanceItemQty.ClientID %>').val('0');
                }
                else {
                    $('#<%=chkIsPackageAllInOne.ClientID %>').removeAttr('disabled');
                    $('#<%=chkIsPackageAllInOne.ClientID %>').removeAttr('checked');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('checked');

                    $('#<%=chkIsPackageBalanceItem.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsPackageBalanceItem.ClientID %>').removeAttr('checked');
                    $('#<%=txtDefaultPackageBalanceItemQty.ClientID %>').val('');
                }
            });

            $('#<%=chkIsPackageAllInOne.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=chkIsPackageItem.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsPackageItem.ClientID %>').removeAttr('checked');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').attr('checked', true);
                }
                else {
                    $('#<%=chkIsPackageItem.ClientID %>').removeAttr('disabled');
                    $('#<%=chkIsPackageItem.ClientID %>').removeAttr('checked');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('checked');
                }
            });

            //#region Harga Variabel
            $('#<%=chkIsAllowVariableTariffComp1.ClientID %>').live('change', function () {
                onAllowVariableTariffChanged($(this).is(':checked'), $('#<%=chkIsAllowVariableTariffComp2.ClientID %>').is(":checked"), $('#<%=chkIsAllowVariableTariffComp3.ClientID %>').is(":checked"));
            });

            $('#<%=chkIsAllowVariableTariffComp2.ClientID %>').live('change', function () {
                onAllowVariableTariffChanged($('#<%=chkIsAllowVariableTariffComp1.ClientID %>').is(":checked"), $(this).is(':checked'), $('#<%=chkIsAllowVariableTariffComp3.ClientID %>').is(":checked"));
            });

            $('#<%=chkIsAllowVariableTariffComp3.ClientID %>').live('change', function () {
                onAllowVariableTariffChanged($('#<%=chkIsAllowVariableTariffComp1.ClientID %>').is(":checked"), $('#<%=chkIsAllowVariableTariffComp2.ClientID %>').is(":checked"), $(this).is(':checked'));
            });

            function onAllowVariableTariffChanged(comp1, comp2, comp3) {
                if (comp1 || comp2 || comp3) {
                    $('#<%=chkIsAllowVariable.ClientID %>').val("1");
                    $('#<%=chkIsAllowVariable.ClientID %>').prop('checked', true);
                }
                else if (!comp1 && !comp2 && !comp3) {
                    $('#<%=chkIsAllowVariable.ClientID %>').val("0");
                    $('#<%=chkIsAllowVariable.ClientID %>').prop('checked', false);
                }
            }
            //#endregion

            //#region Diskon
            $('#<%=chkIsAllowDiscountTariffComp1.ClientID %>').live('change', function () {
                onAllowDiscountTariffChanged($(this).is(':checked'), $('#<%=chkIsAllowDiscountTariffComp2.ClientID %>').is(":checked"), $('#<%=chkIsAllowDiscountTariffComp3.ClientID %>').is(":checked"));
            });

            $('#<%=chkIsAllowDiscountTariffComp2.ClientID %>').live('change', function () {
                onAllowDiscountTariffChanged($('#<%=chkIsAllowDiscountTariffComp1.ClientID %>').is(":checked"), $(this).is(':checked'), $('#<%=chkIsAllowDiscountTariffComp3.ClientID %>').is(":checked"));
            });

            $('#<%=chkIsAllowDiscountTariffComp3.ClientID %>').live('change', function () {
                onAllowDiscountTariffChanged($('#<%=chkIsAllowDiscountTariffComp1.ClientID %>').is(":checked"), $('#<%=chkIsAllowDiscountTariffComp2.ClientID %>').is(":checked"), $(this).is(':checked'));
            });

            function onAllowDiscountTariffChanged(comp1, comp2, comp3) {
                if (comp1 || comp2 || comp3) {
                    $('#<%=chkIsAllowDiscount.ClientID %>').val("1");
                    $('#<%=chkIsAllowDiscount.ClientID %>').prop('checked', true);
                }
                else if (!comp1 && !comp2 && !comp3) {
                    $('#<%=chkIsAllowDiscount.ClientID %>').val("0");
                    $('#<%=chkIsAllowDiscount.ClientID %>').prop('checked', false);
                }
            }

            $('#<%:chkIsPackageBalanceItem.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=tdDefaultPackageBalanceItemQty.ClientID %>').attr('style', '');
                }
                else {
                    $('#<%=tdDefaultPackageBalanceItemQty.ClientID %>').attr('style', 'display:none');
                }
            });
            //#endregion

            registerCollapseExpandHandler();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnSubMenuType" runat="server" value="" />
    <input type="hidden" id="hdnGCSubItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsEKlaimParameterMandatory" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Item")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCode" Width="100px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldItemCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Item #1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName1" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item #2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName2" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Alternatif")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlternateItemName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trModality" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblModalityID">
                                    <%=GetLabel("Modality")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnModalityID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtModalityCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtModalityName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trModalities" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Modalities")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboModalities" ClientInstanceName="cboModalities" runat="server"
                                    Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Satuan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemUnit" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Item")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemStatus" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr  id="trOperationType" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jenis Operasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSurgeryClassification" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr  id="trBloodComponent" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jenis Darah/Komponen")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBloodComponentType" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblItemGroup">
                                    <%=GetLabel("Kelompok Item")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblBillingGroup">
                                    <%=GetLabel("Kelompok Rincian Transaksi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBillingGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBillingGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillingGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblEKlaimParameter" runat="server">
                                    <%=GetLabel("Parameter E-Klaim")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnEKlaimParameterID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEKlaimParameterCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEKlaimParameterName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
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
                                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblRevenueSharing">
                                    <%=GetLabel("Formula Honor Dokter")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRevenueSharingID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRevenueSharingName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDefaultParamedic">
                                    <%=GetLabel("Default Dokter")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnDefaultParamedicID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDefaultParamedicCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDefaultParamedicName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblGCRLReportGroup">
                                    <%=GetLabel("Kelompok Laporan RL")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnGCRLReportGroup" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGCRLReportGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGCRLReportGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Inhealth JenPel Rawat Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKodeJenPelRajal" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Inhealth JenPel Rawat Inap")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKodeJenPelRanap" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                &nbsp
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsInhealthRanapAkomodasi" runat="server" />
                                <%=GetLabel("Akomodasi Inhealth")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Informasi Tambahan")%></h4>
                    <asp:Repeater ID="rptCustomAttribute" runat="server">
                        <HeaderTemplate>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                    </colgroup>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%#: Eval("Value") %></label>
                                </td>
                                <td>
                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnTagFieldCode" />
                                    <asp:TextBox ID="txtTagField" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table> </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Status Item")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col width="225px" />
                            <col width="225px" />
                            <col />
                        </colgroup>
                        <tr id="trTariffScheme" runat="server" style="display:none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Skema Tarif")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTariffScheme" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trDefaultKomponenTariff" runat="server" style="display: none">
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <%=GetLabel("Default Komponen Tarif")%>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="padding: 5px">
                                            <dxe:ASPxComboBox runat="server" Width="50px" ID="cboDefaultTariffComp" ClientInstanceName="cboDefaultTariffComp" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsRegistrationReceiptItem" runat="server" />
                                <%=GetLabel("Item Struk Pendaftaran")%>
                            </td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Label Struk Pendaftaran")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegistrationReceiptLabel" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%" style="background: #cccccc; height: 1px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowVariable" runat="server" />
                                <%=GetLabel("Tarif Variabel")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAllowDiscount" runat="server" />
                                <%=GetLabel("Diskon Tarif")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAllowCITO" runat="server" />
                                <%=GetLabel("Cito Tarif")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowVariableTariffComp1" runat="server" />
                                <%=GetLabel("Harga Variabel Comp 1")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAllowDiscountTariffComp1" runat="server" />
                                <%=GetLabel("Harga Diskon Comp 1")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsUsingSpecialMarkup" runat="server" />
                                <%=GetLabel("Tariff Khusus")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowVariableTariffComp2" runat="server" />
                                <%=GetLabel("Harga Variabel Comp 2")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAllowDiscountTariffComp2" runat="server" />
                                <%=GetLabel("Harga Diskon Comp 2")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowVariableTariffComp3" runat="server" />
                                <%=GetLabel("Harga Variabel Comp 3")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAllowDiscountTariffComp3" runat="server" />
                                <%=GetLabel("Harga Diskon Comp 3")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%" style="background: #cccccc; height: 1px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsPrintWithDoctorName" runat="server" />
                                <%=GetLabel("Cetak Tagihan dengan Nama Dokter")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAllowChangeQty" runat="server" />
                                <%=GetLabel("Izinkan Ubah Qty Transaksi ")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowRevenueSharing" runat="server" />
                                <%=GetLabel("Diperhitungkan Jasa Medis ")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsQtyAllowChangeForDoctor" runat="server" />
                                <%=GetLabel("Izinkan Dokter Ubah Qty Transaksi di EMR ")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsIncludeRevenueSharingTax" runat="server" />
                                <%=GetLabel("Pajak Jasa Medis")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsRevenueSharingFee1" runat="server" />
                                <%=GetLabel("Pajak Jasa Medis Komponen 1")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsRevenueSharingFee2" runat="server" />
                                <%=GetLabel("Pajak Jasa Medis Komponen 2")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsRevenueSharingFee3" runat="server" />
                                <%=GetLabel("Pajak Jasa Medis Komponen 3")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%" style="background: #cccccc; height: 1px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsSubContractItem" runat="server" />
                                <%=GetLabel("Pelayanan dengan Pihak Ketiga ")%>
                            </td>
                            <td id="tdBloodBankItem" runat="server">
                                <asp:CheckBox ID="chkIsBloodBankItem" runat="server" />
                                <%=GetLabel("Pelayanan Bank Darah")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAssetUtilization" runat="server" />
                                <%=GetLabel("Pelayanan dengan Utilisasi Aset ")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsUsingProcedureCoding" runat="server" />
                                <%=GetLabel("Pengkodean dan Pelaporan Prosedur Tindakan")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%" style="background: #cccccc; height: 1px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowComplication" runat="server" />
                                <%=GetLabel("Komplikasi")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsUnbilledItem" runat="server" />
                                <%=GetLabel("Tidak Ditagihkan")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%" style="background: #cccccc; height: 1px;">
                            </td>
                        </tr>
                        <tr id="trIsTestItem" runat="server">
                            <td>
                                <asp:CheckBox ID="chkIsTestItem" runat="server" />
                                <%=GetLabel("Test Item (Pemeriksaan dengan Hasil)")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%" style="background: #cccccc; height: 1px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsIncludeInAdminCalculation" runat="server" />
                                <%=GetLabel("Diperhitungkan dalam Administrasi ")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsSpecialItem" runat="server" />
                                <%=GetLabel("Item Khusus")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsBPJS" runat="server" />
                                <%=GetLabel("Item BPJS")%>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded" id="h4PaketKunjungan" runat="server" style="display: none">
                    <%=GetLabel("Paket Kunjungan")%></h4>
                <div class="containerTblEntryContent" id="divPaketKunjungan" runat="server" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsPackageBalanceItem" runat="server" />
                                <%=GetLabel("Paket Kunjungan")%>
                            </td>
                            <td id="tdDefaultPackageBalanceItemQty" style="display: none" runat="server">
                                <asp:TextBox ID="txtDefaultPackageBalanceItemQty" Width="10%" CssClass="number" runat="server" />
                                X
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Paket Item")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <tr id="trItemPackage">
                            <td>
                                <asp:CheckBox ID="chkIsPackageItem" runat="server" />
                                <%=GetLabel("Paket Item")%>
                            </td>
                        </tr>
                        <tr id="trItemPackageAIO" runat="server">
                            <td>
                                <asp:CheckBox ID="chkIsPackageAllInOne" runat="server" />
                                <%=GetLabel("Paket All in One")%>
                            </td>
                        </tr>
                        <tr id="trAccumulatedPrice">
                            <td>
                                <asp:CheckBox ID="chkIsAccumulatedPrice" runat="server" />
                                <%=GetLabel("Akumulasi Harga")%>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
