<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryRecalculationBillUpdatePayerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryRecalculationBillUpdatePayerCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
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
                        Methods.getObject('GetvCustomerContract1List', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnContractID.ClientID %>').val(result.ContractID);
                                $('#<%=txtContractNo.ClientID %>').val(result.ContractNo);
                                $('#<%=hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                                if (result.IsControlCoverageLimit) {
                                    $('#<%=trCoverageLimit.ClientID %>').show();
                                    if (($('#<%=hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT) && (cboPayer.GetValue() != 'X004^500'))
                                        $('#<%=trCoverageLimitPerDay.ClientID %>').show();
                                }
                                else {
                                    $('#<%=trCoverageLimit.ClientID %>').hide();
                                    $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
                                    $('#<%=txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                                }
                                $('#<%=hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                if (result.IsControlClassCare) {
                                    $('#<%=trControlClassCare.ClientID %>').show();
                                    if (result.ControlClassID != "0") {
                                        cboControlClassCare.SetValue(result.ControlClassID);
                                    } else {
                                        cboControlClassCare.SetValue($('#<%=hdnChargeClassID.ClientID %>').val());
                                    }
                                }
                                else {
                                    $('#<%=trControlClassCare.ClientID %>').hide();
                                    cboControlClassCare.SetValue('');
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
                        $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
                        $('#<%=trControlClassCare.ClientID %>').hide();
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
                $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
                $('#<%=trControlClassCare.ClientID %>').hide();
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
        $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
        $('#<%=trControlClassCare.ClientID %>').hide();
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

                $('#<%=hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                if (result.IsControlClassCare) {
                    $('#<%=trControlClassCare.ClientID %>').show();
                    if (result.ControlClassID != "0") {
                        cboControlClassCare.SetValue(result.ControlClassID);
                    } else {
                        cboControlClassCare.SetValue($('#<%=hdnChargeClassID.ClientID %>').val());
                    }
                }
                else {
                    $('#<%=trControlClassCare.ClientID %>').hide();
                    cboControlClassCare.SetValue('');
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
                $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
                $('#<%=trControlClassCare.ClientID %>').hide();
            }
        });
    }

    function onAfterContractNoChanged() {
        var payerID = parseInt($('#<%:hdnPayerID.ClientID %>').val());
        var filterExpression = 'ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val();
        Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
            $('#<%=hdnContractCoverageMemberCount.ClientID %>').val(result);
            if (result == 1) {
                filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val() + ") AND IsDeleted = 0 AND CoverageTypeID IN (SELECT CoverageTypeID FROM HealthcareCoverageType WHERE HealthcareID = '" + AppSession.healthcareID + "')";
                filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
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
                    var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%=hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0 AND CoverageTypeID IN (SELECT CoverageTypeID FROM HealthcareCoverageType WHERE HealthcareID = '" + AppSession.healthcareID + "')";
                    filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
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
        var payerID = parseInt($('#<%:hdnPayerID.ClientID %>').val()); 

        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%=hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";

        filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
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

    //#region Employee
    function getEmployeeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }
    $('#<%=lblEmployee.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
            $('#<%=txtEmployeeCode.ClientID %>').val(value);
            onTxtEmployeeCodeChanged(value);
        });
    });

    $('#<%=txtEmployeeCode.ClientID %>').change(function () {
        onTxtEmployeeCodeChanged($(this).val());
    });

    function onTxtEmployeeCodeChanged(value) {
        var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
        Methods.getObject('GetEmployeeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnEmployeeID.ClientID %>').val(result.EmployeeID);
                $('#<%=txtEmployeeName.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%=hdnEmployeeID.ClientID %>').val('');
                $('#<%=txtEmployeeCode.ClientID %>').val('');
                $('#<%=txtEmployeeName.ClientID %>').val('');
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
            $('#<%=trGuaranteeLetterExists.ClientID %>').hide();            
        }
        else {
            $('#<%=trPayerCompany.ClientID %>').show();
            $('#<%=trPayerContract.ClientID %>').show();
            $('#<%=trCoverageType.ClientID %>').show();
            $('#<%=trParticipant.ClientID %>').show();
            $('#<%=trGuaranteeLetterExists.ClientID %>').show();
        }
        if (cboPayer.GetValue() == 'X004^400')
            $('#<%=trEmployee.ClientID %>').show();
        else
            $('#<%=trEmployee.ClientID %>').hide();

        if (cboPayer.GetValue() == 'X004^500') {
            $('#<%=trCoverageLimitPerDay.ClientID %>').hide();
        }
        else {
            $('#<%=trCoverageLimitPerDay.ClientID %>').show();
        }

    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnChargeClassID" value="" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 49%" />
        </colgroup>
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Pendaftaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pembayar")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPayer" ClientInstanceName="cboPayer" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trPayerCompany" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblPayerCompany">
                                <%=GetLabel("Instansi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                            <input type="hidden" id="hdnPayerID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 140px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPayerContract" runat="server">
                        <td style="width: 150px" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblContract">
                                <%=GetLabel("Kontrak")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnContractID" value="" runat="server" />
                            <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                            <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                            <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCoverageType" runat="server">
                        <td style="width: 150px" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                <%=GetLabel("Tipe Tanggungan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 140px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCoverageTypeCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCoverageTypeName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trEmployee" runat="server">
                        <td style="width: 150px" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblEmployee">
                                <%=GetLabel("Pegawai")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnEmployeeID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 140px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEmployeeCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmployeeName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trParticipant" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Peserta/Referensi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtParticipantNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trControlClassCare" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kelas Tanggungan")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                <colgroup>
                                    <col width="140px" />
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <input type="hidden" id="hdnIsControlClassCare" value="" runat="server" />
                                        <dxe:ASPxComboBox ID="cboControlClassCare" ClientInstanceName="cboControlClassCare"
                                            Width="100%" runat="server" />
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal"  style="padding-left:3px">
                                            <%=GetLabel("Kelas Rumah Sakit")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargeClass" Width="140px" runat="server" ReadOnly=true />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trCoverageLimit" runat="server">
                        <td style="width: 150px" class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Batas Tanggungan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="133px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCoverageLimitPerDay" runat="server">
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCoverageLimitPerDay" runat="server" /><%=GetLabel("Batas Tanggungan Per Hari")%>
                        </td>
                    </tr>
                    <tr id="trGuaranteeLetterExists" runat="server">
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsGuaranteeLetterExists" runat="server" /><%:GetLabel("Memiliki Kontrol Surat Jaminan")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
