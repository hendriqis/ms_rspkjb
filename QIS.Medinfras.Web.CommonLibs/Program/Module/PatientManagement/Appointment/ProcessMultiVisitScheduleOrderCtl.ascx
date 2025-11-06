<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcessMultiVisitScheduleOrderCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessMultiVisitScheduleOrderCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_registrationeditbusinesspartnerctl">

    var numInit = 0;

    function onCboPayerValueChangedCtl(s) {
        setTblPayerCompanyVisibilityCtl();
        getPayerCompanyCtl('');
        if ($('#<%:hdnContractIDCtl.ClientID %>').val() != '') {
            getCoverageTypeCtl('');
        }
    }

    setDatePicker('<%=txtAppointmentDate.ClientID %>');
    $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

    function setTblPayerCompanyVisibilityCtl() {
        var customerType = cboRegistrationPayerCtl.GetValue();
        if (customerType == "<%:GetCustomerTypePersonal() %>") {
            $('#<%:tblPayerCompanyCtl.ClientID %>').hide();
            $('#<%:chkUsingCOBCtl.ClientID %>').attr('style', 'display:none');
        }
        else {
            if (customerType == "<%:GetCustomerTypeHealthcare() %>") {
                $('#<%:trEmployeeCtl.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:trEmployeeCtl.ClientID %>').attr('style', 'display:none');
            }
            $('#<%:tblPayerCompanyCtl.ClientID %>').show();
            $('#<%:chkUsingCOBCtl.ClientID %>').removeAttr('style');
            $('#<%:trCoverageLimitPerDayCtl.ClientID %>').show();
        }
    }

    //#region Payer Company
    function getPayerCompanyFilterExpressionCtl() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayerCtl.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    $('#<%:lblPayerCompanyCtl.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('payer', getPayerCompanyFilterExpressionCtl(), function (value) {
            $('#<%:txtPayerCompanyCodeCtl.ClientID %>').val(value);
            ontxtPayerCompanyCodeCtlChanged(value);
        });
    });

    $('#<%:txtPayerCompanyCodeCtl.ClientID %>').live('change', function () {
        ontxtPayerCompanyCodeCtlChanged($(this).val());
    });

    function ontxtPayerCompanyCodeCtlChanged(value) {
        var filterExpression = getPayerCompanyFilterExpressionCtl() + " AND BusinessPartnerCode = '" + value + "'";
        getPayerCompanyCtl(filterExpression);
    }

    function getPayerCompanyCtl(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getPayerCompanyFilterExpressionCtl();
        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';

            if (result != null) {
                $('#<%:hdnIsBlacklistPayerCtl.ClientID %>').val(result.IsBlackList);
                if ($('#<%:hdnIsBlacklistPayerCtl.ClientID %>').val() == 'false') {
                    $('#<%:hdnPayerIDCtl.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtPayerCompanyCodeCtl.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyNameCtl.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%:hdnGCTariffSchemeCtl.ClientID %>').val(result.GCTariffScheme);

                    var filterExpression = getPayerContractFilterExpression();
                    Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                        if (result == 1) {
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnContractIDCtl.ClientID %>').val(result.ContractID);
                                    $('#<%:txtContractNoCtl.ClientID %>').val(result.ContractNo);
                                    $('#<%:hdnContractCoverageCountCtl.ClientID %>').val(result.ContractCoverageCount);

                                    if (result.IsControlCoverageLimit) {
                                        $('#<%:trCoverageLimitCtl.ClientID %>').show();
                                        if ($('#<%:hdnDepartmentIDCtl.ClientID %>').val() == Constant.Facility.INPATIENT)
                                            $('#<%:trCoverageLimitPerDayCtl.ClientID %>').show();
                                    }
                                    else {
                                        $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                                        $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                                        $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                                    }
                                    $('#<%:hdnIsControlClassCareCtl.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                    if (result.IsControlClassCare) {
                                        $('#<%:trControlClassCareCtl.ClientID %>').show();
                                        if (result.ControlClassID != '0') {
                                            cboControlClassCareCtl.SetValue(result.ControlClassID);
                                        }
                                        else {
                                            var deptID = $('#<%:hdnDepartmentIDCtl.ClientID %>').val();
                                            var classID = $('#<%:hdnClassIDCtl.ClientID %>').val();
                                            var chargeClassID = $('#<%:hdnChargeClassIDCtl.ClientID %>').val();

                                            if (chargeClassID == "") {
                                                if (deptID != "INPATIENT") {
                                                    chargeClassID = classID;
                                                }
                                            }
                                            cboControlClassCareCtl.SetValue(chargeClassID);
                                        }
                                    }
                                    else {
                                        $('#<%:trControlClassCareCtl.ClientID %>').hide();
                                        cboControlClassCareCtl.SetValue('');
                                    }
                                    onAfterContractNoChanged();
                                    onCheckCustomerMember($('#<%:hdnPayerIDCtl.ClientID %>').val(), $('#<%:txtMRNCtl.ClientID %>').val());
                                }
                            });
                        }
                        else {
                            $('#<%:hdnContractIDCtl.ClientID %>').val('');
                            $('#<%:txtContractNoCtl.ClientID %>').val('');
                            $('#<%:hdnContractCoverageCountCtl.ClientID %>').val('');
                            $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                            $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                            $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                            $('#<%:trControlClassCareCtl.ClientID %>').hide();

                            $('#<%:hdnIsControlClassCareCtl.ClientID %>').val('0');
                            $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                        }
                    });
                }
                else {
                    showToast(messageBlacklistPayer);
                    cboRegistrationPayerCtl.SetValue("<%:GetCustomerTypePersonal() %>");
                    $('#<%:chkUsingCOBCtl.ClientID %>').hide();
                    $('#<%:tblPayerCompanyCtl.ClientID %>').hide();
                }
            }
            else {
                $('#<%:hdnIsBlacklistPayerCtl.ClientID %>').val('0');
                $('#<%:hdnPayerIDCtl.ClientID %>').val('');
                $('#<%:txtPayerCompanyCodeCtl.ClientID %>').val('');
                $('#<%:txtPayerCompanyNameCtl.ClientID %>').val('');
                $('#<%:hdnGCTariffSchemeCtl.ClientID %>').val('');

                $('#<%:hdnContractIDCtl.ClientID %>').val('');
                $('#<%:txtContractNoCtl.ClientID %>').val('');
                $('#<%:hdnContractCoverageCountCtl.ClientID %>').val('');
                $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                $('#<%:trControlClassCareCtl.ClientID %>').hide();
                $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');

                $('#<%:hdnIsControlClassCareCtl.ClientID %>').val('0');
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
            }
        });
    }

    function onCheckCustomerMember(payerID, medicalNoID) {
        if (payerID != '' && medicalNoID != '') {
            var filterExpression = "MedicalNo = '" + medicalNoID + "' AND BusinessPartnerID = '" + payerID + "'";
            Methods.getObject('GetvCustomerMemberList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:txtParticipantNoCtl.ClientID %>').val(result.MemberNo);
                    $('#<%:txtParticipantNoCtl.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%:txtParticipantNoCtl.ClientID %>').val('');
                    $('#<%:txtParticipantNoCtl.ClientID %>').removeAttr('readonly');
                }
            });
        }
    }
    //#endregion

    //#region Physician
    function getParamedicFilterExpression() {
        var hsu = $('#<%:hdnHealthcareServiceUnitIDCtl.ClientID %>').val();
        var filterExpression = "IsDeleted = 0 AND HealthcareServiceUnitID = " + hsu;
        return filterExpression;
    }
    $('#<%:lblPhysicianCtl.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceUnitParamedicCodeMaster', getParamedicFilterExpression(), function (value) {
            $('#<%:txtParamedicCodeCtl.ClientID %>').val(value);
            ontxtParamedicCodeCtlChanged(value);
        });
    });

    $('#<%:txtParamedicCodeCtl.ClientID %>').change(function () {
        ontxtParamedicCodeCtlChanged($(this).val());
    });

    function ontxtParamedicCodeCtlChanged(value) {
        var filterExpression = getParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvServiceUnitParamedicList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnParamedicIDCtl.ClientID %>').val(result.ParamedicID);
                $('#<%:txtParamedicNameCtl.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%:hdnParamedicIDCtl.ClientID %>').val('');
                $('#<%:txtParamedicCodeCtl.ClientID %>').val('');
                $('#<%:txtParamedicNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region No Appointment
    function getToAppointmentFilterExpression() {
        var date = $('#<%=hdnCalAppointmentSelectedDateCtl.ClientID %>').val();
        var year = date.substring(6, 10);
        var month = date.substring(3, 5);
        var day = date.substring(0, 2);
        var date112 = year.toString() + month.toString() + day.toString();
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitIDCtl.ClientID %>').val();
        var paramedicID = $('#<%=hdnParamedicIDCtl.ClientID %>').val();
        var mrn = $('#<%=hdnMRNCtl.ClientID %>').val();
        var session = cboSessionCtl.GetValue();

        var filterExpression = "GCAppointmentStatus IN ('0278^008')";
        filterExpression += " AND CONVERT(VARCHAR, StartDate, 112) = '" + date112 + "'";
        filterExpression += " AND HealthcareServiceUnitID = " + serviceUnitID;
        filterExpression += " AND ParamedicID = " + paramedicID;
        filterExpression += " AND MRN = " + mrn;
        filterExpression += " AND Session = " + session;

        return filterExpression;
    }

    $('#<%:lblToAppointment.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('appointment', getToAppointmentFilterExpression(), function (value) {
            if (value != null) {
                $('#<%=hdnToAppointmentID.ClientID %>').val(value);
                Methods.getObject("GetvAppointmentList", "AppointmentNo = '" + value + "'", function (result) {
                    if (result != null) {
                        $('#<%=txtToAppointmentNo.ClientID %>').val(result.AppointmentNo);
                        var startTime = result.StartTime.split(':');
                        var endTime = result.EndTime.split(':');
                        var visitTypeID = result.VisitTypeID;
                        var visitTypeCode = result.VisitTypeCode;
                        var visitTypeName = result.VisitTypeName;
                        var visitDuration = result.VisitDuration;

                        $('#<%=hdnScheduleInHour.ClientID %>').val(startTime[0]);
                        $('#<%=txtScheduleInHour.ClientID %>').val(startTime[0]);
                        $('#<%=txtScheduleInMinute.ClientID %>').val(startTime[1]);
                        $('#<%=txtEstimatedEndTimeInHour.ClientID %>').val(endTime[0]);
                        $('#<%=txtEstimatedEndTimeInMinute.ClientID %>').val(endTime[1]);
                        $('#<%=hdnVisitDurationCtl.ClientID %>').val(visitDuration);
                        $('#<%=hdnVisitTypeIDCtl.ClientID %>').val(visitTypeID);
                        $('#<%=txtVisitTypeCodeCtl.ClientID %>').val(visitTypeCode);
                        $('#<%=txtVisitTypeNameCtl.ClientID %>').val(visitTypeName);
                    }
                    else {
                        $('#<%=hdnToAppointmentID.ClientID %>').val('');
                        $('#<%=txtToAppointmentNo.ClientID %>').val('');
                        $('#<%=hdnScheduleInHour.ClientID %>').val('');
                        $('#<%=txtScheduleInHour.ClientID %>').val('');
                        $('#<%=txtScheduleInMinute.ClientID %>').val('');
                        $('#<%=txtEstimatedEndTimeInHour.ClientID %>').val('');
                        $('#<%=txtEstimatedEndTimeInMinute.ClientID %>').val('');
                        $('#<%=hdnVisitDurationCtl.ClientID %>').val('');
                        $('#<%=hdnVisitTypeIDCtl.ClientID %>').val('');
                        $('#<%=txtVisitTypeCodeCtl.ClientID %>').val('');
                        $('#<%=txtVisitTypeNameCtl.ClientID %>').val('');
                    }
                });
            }
        });
    });
    //#endregion

    //#region Visit Type
    $('#<%:lblVisitTypeCtl.ClientID %>.lblLink').live('click', function () {
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitIDCtl.ClientID %>').val();
        var paramedicID = $('#<%=hdnParamedicIDCtl.ClientID %>').val();
        if (paramedicID != '') {
            var filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
            Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                var filterExpression = '';
                if (result > 0) {
                    filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + ")";
                    openSearchDialog('visittype', filterExpression, function (value) {
                        $('#<%=txtVisitTypeCodeCtl.ClientID %>').val(value);
                        onTxtVisitTypeCodeChanged(value);
                    });
                }
                else {
                    var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                    Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                        if (result.IsHasVisitType)
                            filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
                        else
                            filterExpression = '';
                        openSearchDialog('visittype', filterExpression, function (value) {
                            $('#<%=txtVisitTypeCodeCtl.ClientID %>').val(value);
                            onTxtVisitTypeCodeChanged(value);
                        });
                    });
                }
            });
        }
        else {
            displayMessageBox('Warning', 'Silahkan Pilih Dokter Terlebih Dahulu');
        }
    });

    $('#<%=txtVisitTypeCodeCtl.ClientID %>').live('change', function () {
        onTxtVisitTypeCodeChanged($(this).val());
    });

    function onTxtVisitTypeCodeChanged(value) {
        var filterExpression = '';

        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitIDCtl.ClientID %>').val();
        var paramedicID = $('#<%=hdnParamedicIDCtl.ClientID %>').val();
        filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
        Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
            var filterExpression = '';
            if (result > 0) {
                filterExpression += "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + " AND VisitTypeCode = '" + value + "'";
                Methods.getObject('GetvParamedicVisitTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnVisitTypeIDCtl.ClientID %>').val(result.VisitTypeID);
                        $('#<%=txtVisitTypeNameCtl.ClientID %>').val(result.VisitTypeName);
                        $('#<%=hdnVisitDurationCtl.ClientID %>').val(result.VisitDuration);
                    }
                    else {
                        $('#<%=hdnVisitTypeIDCtl.ClientID %>').val('');
                        $('#<%=txtVisitTypeCodeCtl.ClientID %>').val('');
                        $('#<%=txtVisitTypeNameCtl.ClientID %>').val('');
                        $('#<%=hdnVisitDurationCtl.ClientID %>').val('');
                    }
                });
            }
            else {
                var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                    if (result.IsHasVisitType) {
                        filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND VisitTypeCode = '" + value + "'";
                        Methods.getObject('GetvServiceUnitVisitTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnVisitTypeIDCtl.ClientID %>').val(result.VisitTypeID);
                                $('#<%=txtVisitTypeNameCtl.ClientID %>').val(result.VisitTypeName);
                                $('#<%=hdnVisitDurationCtl.ClientID %>').val(result.VisitDuration);
                            }
                            else {
                                $('#<%=hdnVisitTypeIDCtl.ClientID %>').val('');
                                $('#<%=txtVisitTypeCodeCtl.ClientID %>').val('');
                                $('#<%=txtVisitTypeNameCtl.ClientID %>').val('');
                                $('#<%=hdnVisitDurationCtl.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        filterExpression = "VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                        Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnVisitTypeIDCtl.ClientID %>').val(result.VisitTypeID);
                                $('#<%=txtVisitTypeNameCtl.ClientID %>').val(result.VisitTypeName);
                                $('#<%=hdnVisitDurationCtl.ClientID %>').val('15');
                            }
                            else {
                                $('#<%=hdnVisitTypeIDCtl.ClientID %>').val('');
                                $('#<%=txtVisitTypeCodeCtl.ClientID %>').val('');
                                $('#<%=txtVisitTypeNameCtl.ClientID %>').val('');
                                $('#<%=hdnVisitDurationCtl.ClientID %>').val('');
                            }
                        });
                    }
                });

                setEstimatedServiceTime();
            }
        });
    }
    //#endregion

    $('#<%=txtScheduleInMinute.ClientID %>').live('change', function () {
        setEstimatedServiceTime();
    });

    function setEstimatedServiceTime() {
        var duration = parseInt($('#<%=hdnVisitDurationCtl.ClientID %>').val());
        if (duration > 0) {
            var estimatedTime = new Date();
            estimatedTime.setHours(parseInt($('#<%=hdnScheduleInHour.ClientID %>').val()), parseInt($('#<%=txtScheduleInMinute.ClientID %>').val()) + duration, 0);
            var hour = estimatedTime.getHours().toString().length == 1 ? "0" + estimatedTime.getHours().toString() : estimatedTime.getHours().toString();
            var minute = estimatedTime.getMinutes().toString().length == 1 ? "0" + estimatedTime.getMinutes().toString() : estimatedTime.getMinutes().toString();
            var time = hour + ":" + minute;
            $('#<%=txtEstimatedEndTimeInHour.ClientID %>').val(hour);
            $('#<%=txtEstimatedEndTimeInMinute.ClientID %>').val(minute);
        }
        else {
            $('#<%=txtEstimatedEndTimeInHour.ClientID %>').val('');
            $('#<%=txtEstimatedEndTimeInMinute.ClientID %>').val('');
        }
    }

    //#region Employee
    function getEmployeeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }
    $('#<%:lblEmployeeCtl.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
            $('#<%:txtEmployeeCodeCtl.ClientID %>').val(value);
            ontxtEmployeeCodeCtlChanged(value);
        });
    });

    $('#<%:txtEmployeeCodeCtl.ClientID %>').change(function () {
        ontxtEmployeeCodeCtlChanged($(this).val());
    });

    function ontxtEmployeeCodeCtlChanged(value) {
        var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
        Methods.getObject('GetEmployeeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnEmployeeIDCtl.ClientID %>').val(result.EmployeeID);
                $('#<%:txtEmployeeNameCtl.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%:hdnEmployeeIDCtl.ClientID %>').val('');
                $('#<%:txtEmployeeCodeCtl.ClientID %>').val('');
                $('#<%:txtEmployeeNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Payer Contract
    function getPayerContractFilterExpression() {
        var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerIDCtl.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblContractCtl.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnPayerIDCtl.ClientID %>').val() != '') {
            openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                $('#<%:txtContractNoCtl.ClientID %>').val(value);
                onTxtPayerContractNoChanged(value);
            });
        }
    });

    $('#<%:txtContractNoCtl.ClientID %>').live('change', function () {
        if ($('#<%:hdnPayerIDCtl.ClientID %>').val() != '')
            onTxtPayerContractNoChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtPayerContractNoChanged(value) {
        var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnContractIDCtl.ClientID %>').val(result.ContractID);
                $('#<%:hdnContractCoverageCountCtl.ClientID %>').val(result.ContractCoverageCount);
                if (result.IsControlCoverageLimit) {
                    $('#<%:trCoverageLimitCtl.ClientID %>').show();
                    if ($('#<%:hdnDepartmentIDCtl.ClientID %>').val() == Constant.Facility.INPATIENT)
                        $('#<%:trCoverageLimitPerDayCtl.ClientID %>').show();
                }
                else {
                    $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                    $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                    $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                }

                $('#<%:hdnIsControlClassCareCtl.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                if (result.IsControlClassCare) {
                    $('#<%:trControlClassCareCtl.ClientID %>').show();
                    cboControlClassCareCtl.SetValue(result.ControlClassID);
                }
                else {
                    $('#<%:trControlClassCareCtl.ClientID %>').hide();
                    cboControlClassCareCtl.SetValue('');
                }

                onAfterContractNoChanged();
            }
            else {
                $('#<%:hdnContractIDCtl.ClientID %>').val('');
                $('#<%:txtContractNoCtl.ClientID %>').val('');
                $('#<%:hdnContractCoverageCountCtl.ClientID %>').val('');
                $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                $('#<%:trControlClassCareCtl.ClientID %>').hide();
                $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                $('#<%:hdnIsControlClassCareCtl.ClientID %>').val('0');
            }
        });
    }

    function onAfterContractNoChanged() {
        var MRN = $('#<%:hdnMRNCtl.ClientID %>').val();
        if (MRN != '') {
            var filterExpression = 'ContractID = ' + $('#<%:hdnContractIDCtl.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRNCtl.ClientID %>').val();
            Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
                $('#<%:hdnContractCoverageMemberCountCtl.ClientID %>').val(result);
                if (result == 1) {
                    filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractIDCtl.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRNCtl.ClientID %>').val() + ') AND IsDeleted = 0';
                    Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val(result.CoverageTypeID);
                            $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(result.CoverageTypeCode);
                            $('#<%:txtCoverageTypeNameCtl.ClientID %>').val(result.CoverageTypeName);
                        }
                        else {
                            $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                        }
                    });
                }
                else {
                    var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCountCtl.ClientID %>').val());
                    if (contractCoverageRowCount == 1) {
                        var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractIDCtl.ClientID %>').val() + ") AND IsDeleted = 0";
                        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val(result.CoverageTypeID);
                                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(result.CoverageTypeCode);
                                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val(result.CoverageTypeName);
                            }
                            else {
                                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                        $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                        $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                    }
                }
            });
        }
    }
    //#endregion

    //#region Coverage Type
    function getCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%:hdnContractCoverageMemberCountCtl.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCountCtl.ClientID %>').val());
        var payerID = parseInt($('#<%:hdnPayerIDCtl.ClientID %>').val());

        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractIDCtl.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRNCtl.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractIDCtl.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";

        filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
        return filterExpression;
    }

    $('#<%:lblCoverageTypeCtl.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnContractIDCtl.ClientID %>').val() != '') {
            openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(value);
                ontxtCoverageTypeCodeCtlChanged(value);
            });
        }
    });

    $('#<%:txtCoverageTypeCodeCtl.ClientID %>').live('change', function () {
        if ($('#<%:hdnContractIDCtl.ClientID %>').val() != '')
            ontxtCoverageTypeCodeCtlChanged($(this).val());
        else
            $(this).val('');
    });

    function ontxtCoverageTypeCodeCtlChanged(value) {
        var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
        getCoverageTypeCtl(filterExpression);
    }

    function getCoverageTypeCtl(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val(result.CoverageTypeID);
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(result.CoverageTypeCode);
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val(result.CoverageTypeName);
            }
            else {
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $("#calAppointmentChangeDate").datepicker({
        defaultDate: Methods.getDatePickerDate($('#<%=hdnCalAppointmentSelectedDateCtl.ClientID %>').val()),
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd-mm-yy",
        minDate: "0",
        onSelect: function (dateText, inst) {
            $('#<%=hdnCalAppointmentSelectedDateCtl.ClientID %>').val(dateText);
            CbpGetPhysicianSchedule.PerformCallback("get|" + dateText);
        }
    });

    function onLoad() {
        alert('a');
    }

    function onCbpGetPhysicianScheduleEndCallback(s) {
        $('#<%=trListAppointment.ClientID %>').attr('style', 'display:none');
        $('#<%=trToAppointment.ClientID %>').attr('style', 'display:none');
        hideLoadingPanel();
    }

    function oncbpViewDtCtlEndCallback(s) {
        hideLoadingPanel();
        $('#containerImgLoadingViewDt').hide();
        var parameter = s.cpSummary.split('|');
        var totalAppointment = parameter[0].split('=');
        var totalQuota = parameter[1].split('=');
        $('#<%=txtTotalQuota.ClientID %>').val(totalQuota[1]);
        $('#<%=txtTotalAppointment.ClientID %>').val(totalAppointment[1]);
    }

    function oncbpParamedicViewDetailEndCallback(s) {
        hideLoadingPanel();

        var parameter = s.cpResult.split('|');
        if (parameter[0] == "paramedic") {
            cboSessionCtl.ClearItems();
            var obj = jQuery.parseJSON(parameter[1]);
            for (var i = 0; i < obj.length; i++) {
                cboSessionCtl.AddItem(obj[i].StandardCodeName, obj[i].StandardCodeID);
            }
            cboSessionCtl.SetSelectedIndex(0);
            Methods.getObject("GetAppointmentList", getToAppointmentFilterExpression(), function (result) {
                if (result != null) {
                    $('#<%=trChkIsUsingSameAppointment.ClientID %>').removeAttr('style');
                }
                else {
                    $('#<%=trChkIsUsingSameAppointment.ClientID %>').attr('style', 'display:none');
                }
            });
            $('#<%=trListAppointment.ClientID %>').removeAttr('style');
            cbpViewDtCtl.PerformCallback('load|' + $('#<%=hdnParamedicIDCtl.ClientID %>').val() + '|' + cboSessionCtl.GetValue());
        }
    }


    $('#<%=chkIsUsingSameAppointment.ClientID %>').live('change', function (evt) {
        if ($(this).is(":checked")) {
            $('#<%=trToAppointment.ClientID %>').removeAttr('style');
            $('#<%=txtScheduleInMinute.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtVisitTypeCodeCtl.ClientID %>').attr('readonly', 'readonly');
            $('#<%=lblVisitTypeCtl.ClientID %>').removeClass('lblMandatory');
            $('#<%=lblVisitTypeCtl.ClientID %>').removeClass('lblLink');

            $('#<%=hdnToAppointmentID.ClientID %>').val('');
            $('#<%=txtToAppointmentNo.ClientID %>').val('');
            $('#<%=hdnScheduleInHour.ClientID %>').val('');
            $('#<%=txtScheduleInHour.ClientID %>').val('');
            $('#<%=txtScheduleInMinute.ClientID %>').val('');
            $('#<%=txtEstimatedEndTimeInHour.ClientID %>').val('');
            $('#<%=txtEstimatedEndTimeInMinute.ClientID %>').val('');
            $('#<%=hdnVisitTypeIDCtl.ClientID %>').val('');
            $('#<%=txtVisitTypeCodeCtl.ClientID %>').val('');
            $('#<%=txtVisitTypeNameCtl.ClientID %>').val('');
            $('#<%=hdnVisitDurationCtl.ClientID %>').val('');
        }
        else {
            $('#<%=trToAppointment.ClientID %>').attr('style', 'display:none');
            $('#<%=txtScheduleInMinute.ClientID %>').removeAttr('readonly');
            $('#<%=txtVisitTypeCodeCtl.ClientID %>').removeAttr('readonly');
            $('#<%=lblVisitTypeCtl.ClientID %>').addClass('lblMandatory');
            $('#<%=lblVisitTypeCtl.ClientID %>').addClass('lblLink');
        }
    });

    function onCboSessionCtlValueChanged() {
        Methods.getObject("GetAppointmentList", getToAppointmentFilterExpression(), function (result) {
            if (result != null) {
                $('#<%=trChkIsUsingSameAppointment.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trChkIsUsingSameAppointment.ClientID %>').attr('style', 'display:none');
            }
        });
        $('#<%=trListAppointment.ClientID %>').removeAttr('style');
        cbpViewDtCtl.PerformCallback('load|' + $('#<%=hdnParamedicIDCtl.ClientID %>').val() + '|' + cboSessionCtl.GetValue());
    }

    $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').die('click');
    $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
        $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnParamedicIDCtl.ClientID %>').val($(this).find('.keyField').html());
        $('#<%=chkIsUsingSameAppointment.ClientID %>').prop('checked', false);
        $('#<%=trToAppointment.ClientID %>').attr('style', 'display:none');
        $('#<%=txtScheduleInMinute.ClientID %>').removeAttr('readonly');
        $('#<%=txtVisitTypeCodeCtl.ClientID %>').removeAttr('readonly');
        $('#<%=lblVisitTypeCtl.ClientID %>').addClass('lblMandatory');
        $('#<%=lblVisitTypeCtl.ClientID %>').addClass('lblLink');
        cbpParamedicViewDetail.PerformCallback('paramedic|' + $(this).find('.keyField').html());
    });

    $('#<%=grdAppointment.ClientID %> tr:gt(0) td.tdAppointment').live('click', function () {
        var chkIsUsingSameAppointment = $('#<%=chkIsUsingSameAppointment.ClientID %>').is(":checked");
        if (!chkIsUsingSameAppointment) {
            $tr = $(this).closest('tr');
            var selectedStartTime = $(this).find('.hdnStartTime').val();
            $('#<%=txtScheduleInHour.ClientID %>').val(selectedStartTime.split(':')[0]);
            $('#<%=hdnScheduleInHour.ClientID %>').val(selectedStartTime.split(':')[0]);
            $('#<%=txtScheduleInMinute.ClientID %>').val(selectedStartTime.split(':')[1]);
            $('#<%=grdAppointment.ClientID %> .selected').removeClass('selected');
            $(this).addClass('selected');
            setEstimatedServiceTime();
        }
    });

</script>
<style type="text/css">
    .tdAppointmentInformation
    {
        position: relative;
        cursor: pointer;
    }
    .grdAppointment > tbody > tr > td
    {
        vertical-align: top;
        padding: 10px;
    }
    .tdAppointment
    {
        padding: 0;
        margin: 0;
    }
    .tdAppointment.selected
    {
        background-color: #F39200;
        color: Black;
    }
    .tdAppointment ol
    {
        margin: 0;
        padding: 0;
        width: 100%;
        height: 30px;
        display: table;
        table-layout: fixed;
    }
    .tdAppointment ol.selected
    {
        background-color: #F39200;
        color: White;
    }
    .tdAppointment ol li
    {
        display: table-cell;
        border: 1px solid #E3E2E3;
        text-align: center;
    }
    .tdAppointment ol li.selected
    {
        background-color: #F39200;
        color: White;
    }
    .tdTime
    {
        padding: 5px;
    }
</style>
<input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
<input type="hidden" runat="server" id="hdnMRNCtl" value="" />
<input type="hidden" runat="server" id="hdnDepartmentIDCtl" value="" />
<input type="hidden" runat="server" id="hdnClassIDCtl" value="" />
<input type="hidden" runat="server" id="hdnChargeClassIDCtl" value="" />
<input type="hidden" runat="server" id="hdnGCCustomerTypeCtl" value="" />
<input type="hidden" runat="server" id="hdnGCTariffSchemePersonalCtl" />
<input type="hidden" runat="server" id="hdnParamedicID" />
<input type="hidden" runat="server" id="hdnScheduleIDCtl" />
<input type="hidden" runat="server" id="hdnHealthcareServiceUnitIDCtl" />
<input type="hidden" runat="server" id="hdnProcessTypeCtl" />
<input type="hidden" runat="server" id="hdnVisitDurationCtl" />
<input type="hidden" id="hdnParamedicIDCtl" value="" runat="server" />
<input type="hidden" id="hdnSelectedAppointmentID" runat="server" />
<input type="hidden" id="hdnScheduleInHour" runat="server" />
<input type="hidden" id="hdnTestOrderID" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="height: 50%" />
        <col style="height: 50%" />
    </colgroup>
    <tr>
        <td>
            <div style="height: 100%">
                <table class="tblContentArea">
                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td style="padding: 5px; vertical-align: top; border-right: 1px solid #AAA;">
                            <div style="height: 100%; overflow-y: scroll; overflow-x: hidden;">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table style="width: 100%">
                                                <colgroup>
                                                    <col style="width:auto; height:auto" />
                                                    <col style="width:auto; height:auto" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDateCtl" />
                                                        <div id="calAppointmentChangeDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trTimeSlotCtl" runat="server">
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboSessionCtl" ClientInstanceName="cboSessionCtl" Width="100%"
                                                            runat="server">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboSessionCtlValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <div style="position: relative;">
                                                <dxcp:ASPxCallbackPanel ID="CbpGetPhysicianSchedule" runat="server" Width="100%"
                                                    ClientInstanceName="CbpGetPhysicianSchedule" ShowLoadingPanel="false" OnCallback="CbpGetPhysicianSchedule_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                                        EndCallback="function(s,e){ onCbpGetPhysicianScheduleEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 200px">
                                                                <asp:GridView ID="grdPhysician" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter / Tenaga Medis" ItemStyle-CssClass="tdParamedicName" />
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
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingPhysicanChangeAppointment">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <div style="height: 100%">
                                                <table class="tblContentArea" width="100%" border="2" cellspacing="1" cellpadding="5">
                                                    <colgroup>
                                                        <col style="width: 50%" />
                                                        <col style="width: 50%" />
                                                    </colgroup>
                                                    <tr style="background-color:#4287f5">
                                                        <td style="text-align: center;font-weight:bold; font-size:small">
                                                             Total Kuota
                                                        </td>
                                                        <td style="text-align: center;font-weight:bold; font-size:small">
                                                             Total Appointment
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:TextBox ID="txtTotalQuota" ReadOnly="true" Width="100%" runat="server" style="text-align: center" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:TextBox ID="txtTotalAppointment" ReadOnly="true" Width="100%" runat="server" style="text-align: center" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div style="height: 300px; overflow-y: scroll; overflow-x: hidden">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col />
                                        <col style="width: 100px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("No. Order")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrderNoCtl" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("No. RM")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMRNCtl" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Nama Pasien")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientNameCtl" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Nama Tindakan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Tindakan ke - ")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSequenceNo" ReadOnly="true" Width="50px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Periode Penjadwalan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSchedulePeriode" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trChkIsUsingSameAppointment" runat="server" style="display:none">
                                        <td class="tdLabel">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingSameAppointment" runat="server" /><%:GetLabel("Gabungkan ke nomor booking lain")%>
                                        </td>
                                    </tr>
                                    <tr id="trToAppointment" runat="server" style="display:none">
                                        <td style="width: 30%" class="tdLabel">
                                            <label class="lblLink" runat="server" id="lblToAppointment">
                                                <%:GetLabel("No. Appointment")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnToAppointmentID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100%" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtToAppointmentNo" Width="50%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Tanggal Rencana Tindakan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppointmentDate" CssClass="datepicker" Width="100px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="tblEntryContent" runat="server" style="width: 100%" id="tblVisitInfoCtl">
                                    <tr style="display:none">
                                        <td style="width: 30%" class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblPhysicianCtl">
                                                <%:GetLabel("Dokter / Tenaga Medis")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtParamedicCodeCtl" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtParamedicNameCtl" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Rencana Mulai")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtScheduleInHour" ReadOnly="true" Width="40px" CssClass="number" runat="server" Style="text-align: center" MaxLength="2" max="59" min="0" />
                                            :
                                            <asp:TextBox ID="txtScheduleInMinute" Width="40px" CssClass="number" runat="server" Style="text-align: center" MaxLength="2" max="59" min="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblVisitTypeCtl">
                                                <%:GetLabel("Jenis Kunjungan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnVisitTypeIDCtl" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtVisitTypeCodeCtl" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVisitTypeNameCtl" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Perkiraan Selesai")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEstimatedEndTimeInHour" ReadOnly="true" Width="40px" CssClass="number" runat="server" Style="text-align: center" MaxLength="2" max="59" min="0" />
                                            :
                                            <asp:TextBox ID="txtEstimatedEndTimeInMinute" ReadOnly="true" Width="40px" CssClass="number" runat="server" Style="text-align: center" MaxLength="2" max="59" min="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Pembayar")%></label>
                                        </td>
                                        <td>
                                            <table>
                                                <colgroup>
                                                    <col style="width: 150px" />
                                                    <col style="width: 3px" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboRegistrationPayerCtl" ClientInstanceName="cboRegistrationPayerCtl"
                                                            Width="100%" runat="server">
                                                            <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChangedCtl(s); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td id="tdChkUsingCOBCtl" runat="server">
                                                        <asp:CheckBox ID="chkUsingCOBCtl" Checked="false" runat="server" Text="Peserta COB" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="tblEntryContent" runat="server" style="width: 100%;" id="tblPayerCompanyCtl">
                                    <tr>
                                        <td style="width: 30%" class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblPayerCompanyCtl">
                                                <%:GetLabel("Instansi")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnPayerIDCtl" value="" runat="server" />
                                            <input type="hidden" id="hdnGCTariffSchemeCtl" value="" runat="server" />
                                            <input type="hidden" id="hdnIsBlacklistPayerCtl" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtPayerCompanyCodeCtl" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPayerCompanyNameCtl" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblContractCtl">
                                                <%:GetLabel("Kontrak")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnContractIDCtl" value="" runat="server" />
                                            <input type="hidden" id="hdnContractCoverageCountCtl" value="" runat="server" />
                                            <input type="hidden" id="hdnContractCoverageMemberCountCtl" value="" runat="server" />
                                            <asp:TextBox ID="txtContractNoCtl" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%" class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblCoverageTypeCtl">
                                                <%:GetLabel("Skema Penjaminan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnCoverageTypeIDCtl" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtCoverageTypeCodeCtl" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCoverageTypeNameCtl" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("No. Peserta")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParticipantNoCtl" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trEmployeeCtl" runat="server">
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblEmployeeCtl">
                                                <%:GetLabel("Pegawai")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" runat="server" id="hdnEmployeeIDCtl" value="" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtEmployeeCodeCtl" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmployeeNameCtl" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trControlClassCareCtl" runat="server">
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%:GetLabel("Jatah Kelas")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnIsControlClassCareCtl" value="" runat="server" />
                                            <dxe:ASPxComboBox ID="cboControlClassCareCtl" ClientInstanceName="cboControlClassCareCtl"
                                                Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trCoverageLimitCtl" runat="server">
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%:GetLabel("Batas Tanggungan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverageLimitCtl" Text="0.00" CssClass="txtCurrency" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trCoverageLimitPerDayCtl" runat="server">
                                        <td class="tdLabel">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCoverageLimitPerDayCtl" runat="server" /><%:GetLabel("Coverage Limit Per Hari")%>
                                        </td>
                                    </tr>
                                </table>
                            </div> 
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr style="text-align: center;" id="trListAppointment" runat="server">
        <td style="text-align: center;">
            <div style="text-align: center; overflow-y: scroll; overflow-x: hidden; height: 400px">
                <dxcp:ASPxCallbackPanel ID="cbpViewDtCtl" runat="server" Width="100%" ClientInstanceName="cbpViewDtCtl"
                    ShowLoadingPanel="false" OnCallback="cbpViewDtCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ oncbpViewDtCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage7">
                                <div id="containerAppointment" class="containerAppointment">
                                    <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected grdAppointment"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdAppointment_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="tdAppointment">
                                                <HeaderTemplate>
                                                    <div class="tdTime" style="text-align: center">
                                                        <%=GetLabel("JAM PRAKTEK") %>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="hdnStartTime" value='<%#: Eval("StartTime")%>' />
                                                    <div class="tdTime w3-large">
                                                        <%#: Eval("DisplayTime") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <div class="tdTime" style="text-align: left">
                                                        <%=GetLabel("DATA APPOINTMENT") %>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Repeater ID="rptOrderInformation" runat="server">
                                                        <HeaderTemplate>
                                                            <ol style="list-style-type: none;">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <li style="padding-bottom: 2px">
                                                                <div class="tdAppointmentInformation" id="tdAppointmentInformation" runat="server">
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #e6e6e6">
                                                                        <colgroup>
                                                                            <col style="width: 100px;" />
                                                                            <col style="width: 150px" />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td style="background-color: #f2f2f2; text-align: center;">
                                                                                <div class="w3-bar w3-large" style="color: #0060df;">
                                                                                    <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                                    <%#: Eval("StartTime") %>
                                                                                    -
                                                                                    <%#: Eval("EndTime") %></div>
                                                                            </td>
                                                                            <td style="background-color: #f2f2f2; width: 80%">
                                                                                <div>
                                                                                    <span style="font-weight: bold; font-size: 11pt">
                                                                                        <%#: Eval("PatientName") %></span></div>
                                                                                <div>
                                                                                    <%#: Eval("MedicalNo") %>,
                                                                                    <%#: Eval("AppointmentNo")%> </div>
                                                                                <div>
                                                                                    <%#: Eval("ParamedicName")%></div>
                                                                                <div>
                                                                                    <%#: Eval("BusinessPartnerName")%></div>
                                                                                <div>
                                                                                    <%#: Eval("ListOrder")%></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </li>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </ul>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Belum ada penjadwalan untuk tanggal yang dipilih")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </td>
    </tr>
</table>
<dxcp:ASPxCallbackPanel ID="cbpParamedicViewDetail" runat="server" Width="100%" ClientInstanceName="cbpParamedicViewDetail" ShowLoadingPanel="false" OnCallback="cbpParamedicViewDetail_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpParamedicViewDetailEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>
