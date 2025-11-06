<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ItemLaboratoryEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.ItemLaboratoryEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region ItemGroup
            $('#lblItemGroup.lblLink').click(function () {
                var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                openSearchDialog('itemgroup', filterExpression, function (value) {
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
                    //  onButtonOutpatientInformationShow();
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

            //#region Product Line
            $('#lblProductLine.lblLink').click(function () {
                openSearchDialog('productline', 'IsDeleted = 0', function (value) {
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
                    onTxtRevenueChanged(value);
                });
            });

            $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
                onTxtRevenueChanged($(this).val());
            });

            function onTxtRevenueChanged(value) {
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

            //#region GCRLReportGroup
            $('#lblGCRLReportGroup.lblLink').click(function () {
                var filter = "ParentID = 'X378' AND TagProperty = 'MR090929' AND IsDeleted = 0";
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

            $('#<%=chkIsPackageItem.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=chkIsPackageAllInOne.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsPackageAllInOne.ClientID %>').removeAttr('checked');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('disabled');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('checked');
                }
                else {
                    $('#<%=chkIsPackageAllInOne.ClientID %>').removeAttr('disabled');
                    $('#<%=chkIsPackageAllInOne.ClientID %>').removeAttr('checked');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').attr('disabled', 'true');
                    $('#<%=chkIsAccumulatedPrice.ClientID %>').removeAttr('checked');
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

            registerCollapseExpandHandler();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsEKlaimParameterMandatory" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Pemeriksaan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Pemeriksaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCode" Width="120px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Item Lama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldItemCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Pemeriksaan #1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName1" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Pemeriksaan #2")%></label>
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Satuan Hasil")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemUnit" runat="server" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Item")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemStatus" Width="130px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblItemGroup">
                                    <%=GetLabel("Kelompok Pemeriksaan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
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
                                        <col style="width: 120px" />
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
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
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
                                        <col style="width: 120px" />
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
                                <label class="lblLink" id="lblGCRLReportGroup">
                                    <%=GetLabel("Kelompok Laporan RL")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnGCRLReportGroup" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
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
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDefaultParamedic">
                                    <%=GetLabel("Default Dokter")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnDefaultParamedicID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
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
                                <label class="lblLink lblMandatory" id="lblEKlaimParameter" runat="server">
                                    <%=GetLabel("Parameter E-Klaim")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnEKlaimParameterID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
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
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kelompok Patologi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatologyGroup" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kelompok Tes Lab")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboLabTestCategory" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Test eHAC")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboeHACTestType" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                &nbsp
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsBloodBankOrder" runat="server" />
                                <%=GetLabel("Order Bank Darah")%>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Darah (Bank Darah)")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCBloodBankType" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Durasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDuration" runat="server" CssClass="number" Width="80px" />
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Urutan Cetak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrintOrder" CssClass="number" Width="100px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Precondition Notes")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <tr>
                            <td>
                                <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtPreconditionNotes"
                                    runat="server" CssClass="htmlEditor" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Item Status")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%" style="font-family: calibri;">
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
                                <%=GetLabel("Tarif Variabel Komponen 1")%>
                            </td>
                              <td>
                                <asp:CheckBox ID="chkIsAllowDiscountTariffComp1" runat="server" />
                                <%=GetLabel("Diskon Tarif Komponen 1")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowVariableTariffComp2" runat="server" />
                                <%=GetLabel("Tarif Variabel Komponen 2")%>
                            </td>
                             <td>
                                <asp:CheckBox ID="chkIsAllowDiscountTariffComp2" runat="server" />
                                <%=GetLabel("Diskon Tarif Komponen 2")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowVariableTariffComp3" runat="server" />
                                <%=GetLabel("Tarif Variabel Komponen 3")%>
                            </td>
                             <td>
                                <asp:CheckBox ID="chkIsAllowDiscountTariffComp3" runat="server" />
                                <%=GetLabel("Diskon Tarif Komponen 3")%>
                            </td>
                        </tr>
                        <tr>
		                    <td colspan="3" width="100%" style="background: #cccccc; height: 1px;"></td>
	                    </tr>
                         <tr>
                            <td>
                                <asp:CheckBox ID="chkIsPrintWithDoctorName" runat="server" />
                                <%=GetLabel("Cetak Tagihan Dengan Nama Dokter")%>
                            </td>
                             <td>
                                <asp:CheckBox ID="chkIsQtyAllowChangeForDoctor" runat="server" />
                                <%=GetLabel("Izinkan Dokter Ubah Qty Transaksi di EMR ")%>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:CheckBox ID="chkIsAllowRevenueSharing" runat="server" />
                                <%=GetLabel("Diperhitungkan Jasa Medis ")%>
                            </td>
                           
                        </tr>
                         <tr>
                            <td>
                                <asp:CheckBox ID="chkIsRevenueSharingFee1" runat="server" />
                                <%=GetLabel("Pajak Jasa Medis Komponen 1 ")%>
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
		                    <td colspan="3" width="100%" style="background: #cccccc; height: 1px;"></td>
	                    </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsSubContractItem" runat="server" />
                                <%=GetLabel("Pelayanan dengan Pihak Ketiga")%>
                            </td>
                        </tr>
                        <tr>
		                    <td colspan="3" width="100%" style="background: #cccccc; height: 1px;"></td>
	                    </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsUnbilledItem" runat="server" />
                                <%=GetLabel("Tidak Ditagihkan")%>
                            </td>
                        </tr>
                        <tr>
		                    <td colspan="3" width="100%" style="background: #cccccc; height: 1px;"></td>
	                    </tr>
                         <tr>
                            <td>
                                <asp:CheckBox ID="chkIsBPJS" runat="server" />
                                <%=GetLabel("Formularium BPJS")%>
                            </td>
                        </tr>
                         <tr>
		                    <td colspan="3" width="100%" style="background: #cccccc; height: 1px;"></td>
	                    </tr>
                          
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsTestItem" runat="server" />
                                <%=GetLabel("Test Item (Pemeriksaan dengan Hasil)")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsBridgingItem" runat="server" />
                                <%=GetLabel("Bridging terhadap LIS")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkIsPPRA" runat="server" />
                                <%=GetLabel("Pemeriksaan untuk PPRA")%>
                            </td>
                             <td>
                                <asp:CheckBox ID="chkIsSpotCheckItem" runat="server" />
                                <%=GetLabel("Pemeriksaan untuk MCU Onsite")%>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:CheckBox ID="chkIsPGxTest" runat="server" />
                                <%=GetLabel("Pemeriksaan Pharmacogenomics")%>
                            </td>
                        </tr>
                        <tr>
		                    <td colspan="3" width="100%" style="background: #cccccc; height: 1px;"></td>
	                    </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsIncludeInAdminCalculation" runat="server" />
                                <%=GetLabel("Diperhitungkan dalam Administrasi")%>
                            </td>
                        </tr>
                       
                        <tr style="display:none">
                            <td>
                                <asp:CheckBox ID="chkIsBridgingToeHAC" runat="server" />
                                <%=GetLabel("Is Bridging To eHAC")%>
                            </td>
                        </tr>
                       
                        <%--<tr>
                            <td>
                                <asp:CheckBox ID="chkIsPackageItem" runat="server" Visible="false" Text='<%=GetLabel("Package Item")%>' />
                            </td>
                        </tr>--%>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Paket Item")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsPackageItem" runat="server" />
                                <%=GetLabel("Paket Item")%>
                            </td>
                        </tr>
                        <tr>
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
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Custom Attribute")%></h4>
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
                                    <asp:TextBox ID="txtTagField" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table> </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
