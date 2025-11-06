<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecalculationPatientBillUpdatePayerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RecalculationPatientBillUpdatePayerCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_patiententryctl">
    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblPayerCompany').click(function () {
        openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
            $('#<%=txtPayerCompanyCode.ClientID %>').val(value);
            onTxtPayerCompanyCodeChanged(value);
        });
    });

    $('#<%=txtPayerCompanyCode.ClientID %>').change(function () {
        onTxtPayerCompanyCodeChanged($(this).val());
    });

    function onTxtPayerCompanyCodeChanged(value) {
        var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                $('#<%=hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);

                Methods.getValue('GetCustomerContractRowCount', getPayerContractFilterExpression(), function (result) {
                    if (result == 1) {
                        var filterExpression = getPayerContractFilterExpression();
                        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnContractID.ClientID %>').val(result.ContractID);
                                $('#<%=txtContractNo.ClientID %>').val(result.ContractNo);
                                $('#<%=hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                                if (result.IsControlCoverageLimit) {
                                    $('#<%=trCoverageLimit.ClientID %>').show();
                                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                                        $('#<%=trCoverageLimitPerDay.ClientID %>').show();
                                }
                                else {
                                    $('#<%=trCoverageLimit.ClientID %>').hide();
                                    $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
                                    $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                                }

                                onAfterContractNoChanged();
                            }
                        });
                    }
                    else {
                        $('#<%=hdnContractID.ClientID %>').val('');
                        $('#<%=txtContractNo.ClientID %>').val('');
                        $('#<%=hdnContractCoverageCount.ClientID %>').val('');
                        $('#<%=trCoverageLimit.ClientID %>').hide();
                        $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                        $('#<%=trCoverageLimitPerDay.ClientID %>').hide();

                        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnPayerID.ClientID %>').val('');
                $('#<%=txtPayerCompanyCode.ClientID %>').val('');
                $('#<%=hdnGCTariffScheme.ClientID %>').val('');

                $('#<%=hdnContractID.ClientID %>').val('');
                $('#<%=txtContractNo.ClientID %>').val('');
                $('#<%=hdnContractCoverageCount.ClientID %>').val('');
                $('#<%=trCoverageLimit.ClientID %>').hide();
                $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
                $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');

                $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                $('#<%=txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        $('#<%=hdnPayerID.ClientID %>').val('');
        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
        $('#<%=txtPayerCompanyName.ClientID %>').val('');

        $('#<%=hdnPayerID.ClientID %>').val('');
        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
        $('#<%=txtPayerCompanyName.ClientID %>').val('');
        $('#<%=hdnContractID.ClientID %>').val('');
        $('#<%=txtContractNo.ClientID %>').val('');
        $('#<%=hdnContractCoverageCount.ClientID %>').val('');
        $('#<%=trCoverageLimit.ClientID %>').hide();
        $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
        $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');

        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
        $('#<%=txtCoverageTypeName.ClientID %>').val('');
    }

    //#region Payer Contract
    function getPayerContractFilterExpression() {
        var filterExpression = "BusinessPartnerID = " + $('#<%=hdnPayerID.ClientID %>').val() + "<%=GetCustomerContractFilterExpression() %>";
        return filterExpression;
    }

    $('#<%=lblContract.ClientID %>.lblLink').click(function () {
        if ($('#<%=hdnPayerID.ClientID %>').val() != '') {
            openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                $('#<%=txtContractNo.ClientID %>').val(value);
                onTxtPayerContractNoChanged(value);
            });
        }
    });

    $('#<%=txtContractNo.ClientID %>').change(function () {
        if ($('#<%=hdnPayerID.ClientID %>').val() != '')
            onTxtPayerContractNoChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtPayerContractNoChanged(value) {
        var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnContractID.ClientID %>').val(result.ContractID);
                $('#<%=hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                if (result.IsControlCoverageLimit) {
                    $('#<%=trCoverageLimit.ClientID %>').show();
                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                        $('#<%=trCoverageLimitPerDay.ClientID %>').show();
                }
                else {
                    $('#<%=trCoverageLimit.ClientID %>').hide();
                    $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
                    $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                }

                onAfterContractNoChanged();
            }
            else {
                $('#<%=hdnContractID.ClientID %>').val('');
                $('#<%=txtContractNo.ClientID %>').val('');
                $('#<%=hdnContractCoverageCount.ClientID %>').val('');
                $('#<%=trCoverageLimit.ClientID %>').hide();
                $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
                $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                $('#<%=txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }

    function onAfterContractNoChanged() {
        var filterExpression = 'ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val();
        Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
            $('#<%=hdnContractCoverageMemberCount.ClientID %>').val(result);
            if (result == 1) {
                filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
                Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                        $('#<%=txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                        $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                    }
                    else {
                        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    }
                });
            }
            else {
                var contractCoverageRowCount = parseInt($('#<%=hdnContractCoverageCount.ClientID %>').val());
                if (contractCoverageRowCount == 1) {
                    var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%=hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
                    Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                            $('#<%=txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                            $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                        }
                        else {
                            $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%=txtCoverageTypeName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%=txtCoverageTypeName.ClientID %>').val('');
                }
            }
        });
    }
    //#endregion

    //#region Coverage Type
    function getCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%=hdnContractCoverageMemberCount.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%=hdnContractCoverageCount.ClientID %>').val());
        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%=hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#<%=lblCoverageType.ClientID %>.lblLink').click(function () {
        if ($('#<%=hdnContractID.ClientID %>').val() != '') {
            openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                $('#<%=txtCoverageTypeCode.ClientID %>').val(value);
                onTxtCoverageTypeCodeChanged(value);
            });
        }
    });

    $('#<%=txtCoverageTypeCode.ClientID %>').change(function () {
        if ($('#<%=hdnContractID.ClientID %>').val() != '')
            onTxtCoverageTypeCodeChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtCoverageTypeCodeChanged(value) {
        var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
            }
            else {
                $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                $('#<%=txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function setTblPayerCompanyVisibility() {
        if (cboPayer.GetValue() == 'X004^999') {
            $('#<%=trPayerCompany.ClientID %>').hide();
            $('#<%=trPayerContract.ClientID %>').hide();
            $('#<%=trCoverageType.ClientID %>').hide();
            $('#<%=trParticipant.ClientID %>').hide();
        }
        else {
            $('#<%=trPayerCompany.ClientID %>').show();
            $('#<%=trPayerContract.ClientID %>').show();
            $('#<%=trCoverageType.ClientID %>').show();
            $('#<%=trParticipant.ClientID %>').show();
        }
    }
</script>
<div class="pageTitle"><%=GetLabel("Update Patient Payer")%></div>
<div>
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:49%"/>
        </colgroup>
        <tr>
            <td>
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Registrasi")%></label></td>
                        <td><asp:TextBox ID="txtRegistrationNo" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No RM")%></label></td>
                        <td><asp:TextBox ID="txtMRN" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                        <td><asp:TextBox ID="txtPatientName" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Pembayar")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPayer" ClientInstanceName="cboPayer" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trPayerCompany" runat="server">
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPayerCompany"><%=GetLabel("Instansi")%></label></td>
                        <td>
                            <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                            <input type="hidden" id="hdnPayerID" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPayerContract" runat="server">
                        <td style="width:30%" class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblContract"><%=GetLabel("Kontrak")%></label></td>
                        <td>
                            <input type="hidden" id="hdnContractID" value="" runat="server" />
                            <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                            <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                            <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                        </td>
                    </tr>                    
                    <tr id="trCoverageType" runat="server">
                        <td style="width:30%" class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblCoverageType"><%=GetLabel("Tipe Coverage")%></label></td>
                        <td>
                            <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtCoverageTypeCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtCoverageTypeName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trParticipant" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Partisipan")%></label></td>
                        <td><asp:TextBox ID="txtParticipantNo" Width="100%" runat="server" /></td>
                    </tr>
                    <tr id="trCoverageLimit" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Batas Tanggungan")%></label></td>
                        <td><asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                    <tr id="trCoverageLimitPerDay" runat="server">
                        <td class="tdLabel">&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsCoverageLimitPerDay" runat="server"/><%=GetLabel("Batas Tanggungan Per Hari")%></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>