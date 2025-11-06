<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentRequestChangePayerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentRequestChangePayerCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        if ($('#<%=hdnPayerID.ClientID %>').val() == "1") {
            $('#<%=trPayerCompany.ClientID %>').hide();
            $('#<%=trPayerContract.ClientID %>').hide();
            $('#<%=trPayerContractAvailability.ClientID %>').hide();
            $('#<%=trPayerScheme.ClientID %>').hide();
        }
        else {
            $('#<%=trPayerCompany.ClientID %>').show();
            $('#<%=trPayerContract.ClientID %>').show();
            $('#<%=trPayerContractAvailability.ClientID %>').show();
            $('#<%=trPayerScheme.ClientID %>').show();
        }
        $('#<%:trEmployee.ClientID %>').attr('style', 'display:none');
    });

    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    $('#<%=lblPayerCompany.ClientID %> ').click(function () {
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
            }
            else {
                $('#<%=hdnPayerID.ClientID %>').val('');
                $('#<%=txtPayerCompanyCode.ClientID %>').val('');
                $('#<%=hdnGCTariffScheme.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        $('#<%=hdnPayerID.ClientID %>').val('');
        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
        $('#<%=txtPayerCompanyName.ClientID %>').val('');
    }

    function setTblPayerCompanyVisibility() {
        if (cboPayer.GetValue() == 'X004^999') {
            $('#<%=trPayerCompany.ClientID %>').hide();
            $('#<%=trPayerContract.ClientID %>').hide();
            $('#<%=trPayerContractAvailability.ClientID %>').hide();
            $('#<%=trPayerScheme.ClientID %>').hide();
        }
        else {
            $('#<%=trPayerCompany.ClientID %>').show();
            $('#<%=trPayerContract.ClientID %>').show();
            $('#<%=trPayerContractAvailability.ClientID %>').show();
            $('#<%=trPayerScheme.ClientID %>').show();
        }
    }

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        getPayerCompany('');
        if ($('#<%:hdnContractID.ClientID %>').val() != '') {
            getCoverageType('');
        }
    }
    function setTblPayerCompanyVisibility() {
        var customerType = cboPayer.GetValue();
        if (customerType == "<%:GetCustomerTypePersonal() %>") {
            $('#<%:trPayerCompany.ClientID %>').hide();
            $('#<%=trPayerContract.ClientID %>').hide();
            $('#<%=trPayerContractAvailability.ClientID %>').hide();
            $('#<%=trPayerScheme.ClientID %>').hide();
        }
        else {
            if (customerType == "<%:GetCustomerTypeHealthcare() %>") {
                $('#<%:trEmployee.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:trEmployee.ClientID %>').attr('style', 'display:none');
            }
            $('#<%:trPayerCompany.ClientID %>').show();
            $('#<%=trPayerContract.ClientID %>').show();
            $('#<%=trPayerContractAvailability.ClientID %>').show();
            $('#<%=trPayerScheme.ClientID %>').show();
            
            $('#<%:trCoverageLimitPerDay.ClientID %>').show();
        }
    }
    function getCoverageType(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
            }
            else {
                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                $('#<%:txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }
    function getPayerCompany(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getPayerCompanyFilterExpression();

        alert(filterExpression);

        Methods.getObject('GetvCustomerList', filterExpression, function (resultCS) {
            var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';
            if (resultCS != null) {
                $('#<%:hdnIsBlacklistPayer.ClientID %>').val(resultCS.IsBlackList);
                if ($('#<%:hdnIsBlacklistPayer.ClientID %>').val() == 'false') {
                    $('#<%:hdnPayerID.ClientID %>').val(resultCS.BusinessPartnerID);
                    $('#<%:txtPayerCompanyCode.ClientID %>').val(resultCS.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyName.ClientID %>').val(resultCS.BusinessPartnerName);
                    $('#<%:hdnGCTariffScheme.ClientID %>').val(resultCS.GCTariffScheme);
                    $('#btnPayerNotesDetail').removeAttr('enabled');
                    var filterExpression = getPayerContractFilterExpression();
                    Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                        if (result == 1) {
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                                    $('#<%:txtContractNo.ClientID %>').val(result.ContractNo);
                                    $('#<%:txtContractPeriod.ClientID %>').val(result.cfContractEndDateInString);
                                    $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);

                                    if (result.IsControlCoverageLimit) {
                                        $('#<%:trCoverageLimit.ClientID %>').show();
                                        if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                                            $('#<%:trCoverageLimitPerDay.ClientID %>').show();
                                    }
                                    else {
                                        $('#<%:trCoverageLimit.ClientID %>').hide();
                                        $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                                        $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                                    }
                                    $('#<%:hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                    if (result.IsControlClassCare) {
                                        $('#<%:trControlClassCare.ClientID %>').show();
                                        if (result.ControlClassID != '0') {
                                            cboControlClassCare.SetValue(result.ControlClassID);
                                        }
                                        else {
                                            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                            var classID = $('#<%:hdnClassID.ClientID %>').val();
                                            var chargeClassID = $('#<%:hdnChargeClassID.ClientID %>').val();

                                            if (chargeClassID == "") {
                                                if (deptID != "INPATIENT") {
                                                    chargeClassID = classID;
                                                }
                                            }
                                            cboControlClassCare.SetValue(chargeClassID);
                                        }
                                    }
                                    else {
                                        $('#<%:trControlClassCare.ClientID %>').hide();
                                        cboControlClassCare.SetValue('');
                                    }
                                    onAfterContractNoChanged();
                                    onCheckCustomerMember($('#<%:hdnPayerID.ClientID %>').val(), $('#<%:txtMRN.ClientID %>').val(), "");
                                }
                            });
                        }
                        else {
                            $('#<%:hdnContractID.ClientID %>').val('');
                            $('#<%:txtContractNo.ClientID %>').val('');
                            $('#<%:txtContractPeriod.ClientID %>').val('');
                            $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                            $('#<%:trCoverageLimit.ClientID %>').hide();
                            $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                            $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                            $('#<%:trControlClassCare.ClientID %>').hide();

                            $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                            $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%:txtCoverageTypeName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    showToast(messageBlacklistPayer);
                    cboPayer.SetValue("<%:GetCustomerTypePersonal() %>");

                    $('#<%:trPayerCompany.ClientID %>').hide();
                    $('#<%=trPayerContract.ClientID %>').hide();
                    $('#<%=trPayerContractAvailability.ClientID %>').hide();
                    $('#<%=trPayerScheme.ClientID %>').hide();
                }
            }
            else {
                $('#<%:hdnIsBlacklistPayer.ClientID %>').val('0');
                $('#<%:hdnPayerID.ClientID %>').val('');
                $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                $('#<%:txtPayerCompanyName.ClientID %>').val('');
                $('#btnPayerNotesDetail').attr('enabled', 'false');
                $('#<%:hdnGCTariffScheme.ClientID %>').val('');

                $('#<%:hdnContractID.ClientID %>').val('');
                $('#<%:txtContractNo.ClientID %>').val('');
                $('#<%:txtContractPeriod.ClientID %>').val('');
                $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                $('#<%:trCoverageLimit.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                $('#<%:trControlClassCare.ClientID %>').hide();
                $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');

                $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                $('#<%:txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }
    function getPayerContractFilterExpression() {
        var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerID.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblContract.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnPayerID.ClientID %>').val() != '') {
            openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                $('#<%:txtContractNo.ClientID %>').val(value);
                onTxtPayerContractNoChanged(value);
            });
        }
    });

    $('#<%:txtContractNo.ClientID %>').live('change', function () {
        if ($('#<%:hdnPayerID.ClientID %>').val() != '')
            onTxtPayerContractNoChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtPayerContractNoChanged(value) {
        var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                $('#<%:txtContractPeriod.ClientID %>').val(result.cfContractEndDateInString);
                $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                if (result.IsControlCoverageLimit) {
                    $('#<%:trCoverageLimit.ClientID %>').show();
                    if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                        $('#<%:trCoverageLimitPerDay.ClientID %>').show();
                }
                else {
                    $('#<%:trCoverageLimit.ClientID %>').hide();
                    $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                    $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                }

                $('#<%:hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                if (result.IsControlClassCare) {
                    $('#<%:trControlClassCare.ClientID %>').show();
                    cboControlClassCare.SetValue(result.ControlClassID);
                }
                else {
                    $('#<%:trControlClassCare.ClientID %>').hide();
                    cboControlClassCare.SetValue('');
                }

                onAfterContractNoChanged();
            }
            else {
                $('#<%:hdnContractID.ClientID %>').val('');
                $('#<%:txtContractNo.ClientID %>').val('');
                $('#<%:txtContractPeriod.ClientID %>').val('');
                $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                $('#<%:trCoverageLimit.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                $('#<%:trControlClassCare.ClientID %>').hide();
                $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                $('#<%:txtCoverageTypeName.ClientID %>').val('');
                $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
            }
        });
    }
    function onAfterContractNoChanged() {
        var MRN = $('#<%:hdnMRN.ClientID %>').val();
        var payerID = parseInt($('#<%:hdnPayerID.ClientID %>').val());
        if (MRN != '') {
            var filterExpression = 'ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val();
            Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
                $('#<%:hdnContractCoverageMemberCount.ClientID %>').val(result);
                if (result == 1) {
                    filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val() + ") AND IsDeleted = 0  AND CoverageTypeID IN (SELECT CoverageTypeID FROM HealthcareCoverageType WHERE HealthcareID = '" + AppSession.healthcareID + "')";
                    filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
                    Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                            $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                            $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                        }
                        else {
                            $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%:txtCoverageTypeName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
                    if (contractCoverageRowCount == 1) {
                        var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0 AND CoverageTypeID IN (SELECT CoverageTypeID FROM HealthcareCoverageType WHERE HealthcareID = '" + AppSession.healthcareID + "')";
                        filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
                        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                                $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                                $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                            }
                            else {
                                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                                $('#<%:txtCoverageTypeName.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%:txtCoverageTypeName.ClientID %>').val('');
                    }
                }
            });
        }
        else {
            var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0 AND CoverageTypeID IN (SELECT CoverageTypeID FROM HealthcareCoverageType WHERE HealthcareID = '" + AppSession.healthcareID + "')";
            filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
    }
    //#endregion
    //#region Coverage Type
    function getCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%:hdnContractCoverageMemberCount.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
        var payerID = parseInt($('#<%:hdnPayerID.ClientID %>').val());

        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";

        filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
        return filterExpression;
    }

    $('#<%:lblCoverageType.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnContractID.ClientID %>').val() != '') {
            openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                $('#<%:txtCoverageTypeCode.ClientID %>').val(value);
                onTxtCoverageTypeCodeChanged(value);
            });
        }
    });

    $('#<%:txtCoverageTypeCode.ClientID %>').live('change', function () {
        if ($('#<%:hdnContractID.ClientID %>').val() != '')
            onTxtCoverageTypeCodeChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtCoverageTypeCodeChanged(value) {
        var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
        getCoverageType(filterExpression);
    }

    function getCoverageType(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
            }
            else {
                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                $('#<%:txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }
    //#endregion
    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    $('#<%:lblPayerCompany.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
            $('#<%:txtPayerCompanyCode.ClientID %>').val(value);
            onTxtPayerCompanyCodeChanged(value);
        });
    });

    $('#<%:txtPayerCompanyCode.ClientID %>').live('change', function () {
        onTxtPayerCompanyCodeChanged($(this).val());
    });

    function onTxtPayerCompanyCodeChanged(value) {
        var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        getPayerCompany(filterExpression);
    }

    function getPayerCompany(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getPayerCompanyFilterExpression();

        Methods.getObject('GetvCustomerList', filterExpression, function (resultCS) {
            var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';
            if (resultCS != null) {
                $('#<%:hdnIsBlacklistPayer.ClientID %>').val(resultCS.IsBlackList);
                if ($('#<%:hdnIsBlacklistPayer.ClientID %>').val() == 'false') {
                    $('#<%:hdnPayerID.ClientID %>').val(resultCS.BusinessPartnerID);
                    $('#<%:txtPayerCompanyCode.ClientID %>').val(resultCS.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyName.ClientID %>').val(resultCS.BusinessPartnerName);
                    $('#<%:hdnGCTariffScheme.ClientID %>').val(resultCS.GCTariffScheme);
                    $('#btnPayerNotesDetail').removeAttr('enabled');
                    var filterExpression = getPayerContractFilterExpression();
                    Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                        if (result == 1) {
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                                    $('#<%:txtContractNo.ClientID %>').val(result.ContractNo);
                                    $('#<%:txtContractPeriod.ClientID %>').val(result.cfContractEndDateInString);
                                    $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);

                                    if (result.IsControlCoverageLimit) {
                                        $('#<%:trCoverageLimit.ClientID %>').show();
                                        if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                                            $('#<%:trCoverageLimitPerDay.ClientID %>').show();
                                    }
                                    else {
                                        $('#<%:trCoverageLimit.ClientID %>').hide();
                                        $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                                        $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                                    }
                                    $('#<%:hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                    if (result.IsControlClassCare) {
                                        $('#<%:trControlClassCare.ClientID %>').show();
                                        if (result.ControlClassID != '0') {
                                            cboControlClassCare.SetValue(result.ControlClassID);
                                        }
                                        else {
                                            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                            var classID = $('#<%:hdnClassID.ClientID %>').val();
                                            var chargeClassID = $('#<%:hdnChargeClassID.ClientID %>').val();

                                            if (chargeClassID == "") {
                                                if (deptID != "INPATIENT") {
                                                    chargeClassID = classID;
                                                }
                                            }
                                            cboControlClassCare.SetValue(chargeClassID);
                                        }
                                    }
                                    else {
                                        $('#<%:trControlClassCare.ClientID %>').hide();
                                        cboControlClassCare.SetValue('');
                                    }
                                    onAfterContractNoChanged();
                                    onCheckCustomerMember($('#<%:hdnPayerID.ClientID %>').val(), $('#<%:txtMRN.ClientID %>').val(),"");
                                }
                            });
                        }
                        else {
                            $('#<%:hdnContractID.ClientID %>').val('');
                            $('#<%:txtContractNo.ClientID %>').val('');
                            $('#<%:txtContractPeriod.ClientID %>').val('');
                            $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                            $('#<%:trCoverageLimit.ClientID %>').hide();
                            $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                            $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                            $('#<%:trControlClassCare.ClientID %>').hide();

                            $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                            $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%:txtCoverageTypeName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    showToast(messageBlacklistPayer);
                    cboPayer.SetValue("<%:GetCustomerTypePersonal() %>");

                    $('#<%:trPayerCompany.ClientID %>').hide();
                    $('#<%=trPayerContract.ClientID %>').hide();
                    $('#<%=trPayerContractAvailability.ClientID %>').hide();
                    $('#<%=trPayerScheme.ClientID %>').hide();
                }
            }
            else {
                $('#<%:hdnIsBlacklistPayer.ClientID %>').val('0');
                $('#<%:hdnPayerID.ClientID %>').val('');
                $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                $('#<%:txtPayerCompanyName.ClientID %>').val('');
                $('#btnPayerNotesDetail').attr('enabled', 'false');
                $('#<%:hdnGCTariffScheme.ClientID %>').val('');

                $('#<%:hdnContractID.ClientID %>').val('');
                $('#<%:txtContractNo.ClientID %>').val('');
                $('#<%:txtContractPeriod.ClientID %>').val('');
                $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                $('#<%:trCoverageLimit.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                $('#<%:trControlClassCare.ClientID %>').hide();
                $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');

                $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                $('#<%:txtCoverageTypeName.ClientID %>').val('');
            }
        });
    }


    function onCheckCustomerMember(payerID, medicalNoID, noPeserta) {
        if (payerID != '' && medicalNoID != '') {
            var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var filterExpression = "MedicalNo = '" + medicalNoID + "' AND BusinessPartnerID = '" + payerID + "'";
             
        }
    }
    //#endregion
    //#region Employee
    function getEmployeeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }
    $('#<%:lblEmployee.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
            $('#<%:txtEmployeeCode.ClientID %>').val(value);
            onTxtEmployeeCodeChanged(value);
        });
    });

    $('#<%:txtEmployeeCode.ClientID %>').change(function () {
        onTxtEmployeeCodeChanged($(this).val());
    });

    function onTxtEmployeeCodeChanged(value) {
        var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
        Methods.getObject('GetEmployeeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnEmployeeID.ClientID %>').val(result.EmployeeID);
                $('#<%:txtEmployeeName.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%:hdnEmployeeID.ClientID %>').val('');
                $('#<%:txtEmployeeCode.ClientID %>').val('');
                $('#<%:txtEmployeeName.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div>
    <input type="hidden" runat="server" id="hdnProcessType" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnChargeClassID" value="" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" id="hdnIsBlacklistPayer" runat="server" />
     <input type="hidden" id="hdnClassID" runat="server" />
    
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
                            <label class="lblLink" id="lblPayerCompany" runat="server">
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
                        <td style="width: 30%" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblContract">
                                <%:GetLabel("Kontrak")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 250px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <input type="hidden" id="hdnContractID" value="" runat="server" />
                                        <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                                        <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                                        <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input type="button" id="btnPayerNotesDetail" value="..." />
                                    </td>
                                    <td>
                                        <input type="button" id="btnCustomerContractDocumentInfo" value="Informasi Instansi"
                                            style="width: 100%;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPayerContractAvailability" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Masa Berlaku Kontrak")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtContractPeriod" Width="120px" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr id="trPayerScheme" runat="server">
                        <td style="width: 30%" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                <%:GetLabel("Skema Penjaminan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
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
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                   
                    <tr id="trEmployee" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblEmployee">
                                <%:GetLabel("Pegawai")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
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
                    <tr id="trControlClassCare" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%:GetLabel("Jatah Kelas")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnIsControlClassCare" value="" runat="server" />
                            <dxe:ASPxComboBox ID="cboControlClassCare" ClientInstanceName="cboControlClassCare"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCoverageLimit" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Batas Tanggungan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCoverageLimitPerDay" runat="server">
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCoverageLimitPerDay" runat="server" /><%:GetLabel("Coverage Limit Per Hari")%>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
</div>
