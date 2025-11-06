<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentFormEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentFormEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_vitalsignentryctl">
   
    $(document).ready(function () {
        var hdnIsMobilePhoneNumeric = $('#<%=hdnIsMobilePhoneNumeric.ClientID %>').val();
        if (hdnIsMobilePhoneNumeric == "1") {
            $('#<%=txtMobilePhone.ClientID %>').TextNumericOnly();
        }
    });

    function openLink(evt, animName) {
        var i, x, tablinks;
        x = document.getElementsByClassName("city");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablink");
        for (i = 0; i < x.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
        }
        document.getElementById(animName).style.display = "block";
        evt.currentTarget.className += " w3-red";
    }

    $(function () {
        setTblPayerCompanyVisibility();

        setDatePicker('<%:txtDOBMainAppt.ClientID %>');
        $('#<%:txtDOBMainAppt.ClientID %>').datepicker('option', 'maxDate', 0);

        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        //#region Is New Patient
        $('#<%=chkIsNewPatient.ClientID %>').live('change', function (evt) {
            if ($(this).is(":checked")) {
                $('#lblMRN').attr('class', 'lblDisabled');
                $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtMRN.ClientID %>').removeClass('error');
                $('#<%=txtFirstName.ClientID %>').removeAttr('readonly');
                $('#<%=txtMiddleName.ClientID %>').removeAttr('readonly');
                $('#<%=txtFamilyName.ClientID %>').removeAttr('readonly');
                $('#<%=txtAddress.ClientID %>').removeAttr('readonly');
                $('#<%=txtDOBMainAppt.ClientID %>').removeAttr('readonly');
                $('#<%=txtCorporateAccountNo.ClientID %>').removeAttr('readonly');
                $('#<%=txtCorporateAccountName.ClientID %>').removeAttr('readonly');

                $('#<%=hdnMRN.ClientID %>').val('');
                $('#<%=txtMRN.ClientID %>').val('');
                $('#<%=txtFirstName.ClientID %>').val('');
                $('#<%=txtMiddleName.ClientID %>').val('');
                $('#<%=txtFamilyName.ClientID %>').val('');
                $('#<%=txtAddress.ClientID %>').val('');
                $('#<%=txtPhoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
                $('#<%=txtDOBMainAppt.ClientID %>').val('');
                $('#<%=txtCorporateAccountNo.ClientID %>').val('');
                $('#<%=txtCorporateAccountName.ClientID %>').val('');

                cboSalutationAppo.SetEnabled(true);
                cboGenderAppointment.SetEnabled(true);
                cboGenderAppointment.SetValue('');
            }
            else {
                $('#lblMRN').attr('class', 'lblLink');
                $('#<%=txtMRN.ClientID %>').removeAttr('readonly');
                $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtDOBMainAppt.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtCorporateAccountNo.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtCorporateAccountName.ClientID %>').attr('readonly', 'readonly');

                $('#<%=txtMRN.ClientID %>').val('');
                $('#<%=txtFirstName.ClientID %>').val('');
                $('#<%=txtMiddleName.ClientID %>').val('');
                $('#<%=txtFamilyName.ClientID %>').val('');
                $('#<%=txtAddress.ClientID %>').val('');
                $('#<%=txtPhoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
                $('#<%=txtDOBMainAppt.ClientID %>').val('');
                $('#<%=txtCorporateAccountNo.ClientID %>').val('');
                $('#<%=txtCorporateAccountName.ClientID %>').val('');

                cboSalutationAppo.SetValue('');
                cboSalutationAppo.SetEnabled(false);
                cboGenderAppointment.SetEnabled(false);
                cboGenderAppointment.SetValue('');
            }
        });
        //#endregion

        //#region MRN
        $('#lblMRN.lblLink').live('click', function () {
            openSearchDialog('patient', '', function (value) {
                $('#<%=txtMRN.ClientID %>').val(value);
                onTxtMRNChanged(value);
            });
        });
        $('#<%=txtMRN.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val());
        });
        function onTxtMRNChanged(value) {
            var mrn = FormatMRN(value);
            var filterExpression = "MedicalNo = '" + mrn + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    var mrn = result.MRN;
                    var salutation = result.Salutation;
                    var firstName = result.FirstName;
                    var middleName = result.MiddleName;
                    var lastName = result.LastName;
                    var homeAddress = result.HomeAddress;
                    var phoneNo1 = result.PhoneNo1;
                    var mobilePhoneNo1 = result.MobilePhoneNo1;
                    var emailAddress = result.EmailAddress;
                    var masterCorporateAccountNo = result.CorporateAccountNo;
                    var masterCorporateAccountName = result.CorporateAccountName;
                    var gender = result.Gender;
                    var dob = result.cfDateOfBirthInString1;

                    $('#<%=hdnMRN.ClientID %>').val(mrn);
                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);

                    if (salutation != '' && salutation != null) {
                        cboSalutationAppo.SetValue(salutation);
                    }

                    $('#<%=txtFirstName.ClientID %>').val(firstName);
                    $('#<%=txtMiddleName.ClientID %>').val(middleName);
                    $('#<%=txtFamilyName.ClientID %>').val(lastName);
                    $('#<%=txtAddress.ClientID %>').val(homeAddress);
                    $('#<%=txtPhoneNo.ClientID %>').val(phoneNo1);
                    $('#<%=txtMobilePhone.ClientID %>').val(mobilePhoneNo1);
                    $('#<%=txtEmail.ClientID %>').val(emailAddress);
                    $('#<%=txtDOBMainAppt.ClientID %>').val(dob);
                    $('#<%=txtCorporateAccountNo.ClientID %>').val(masterCorporateAccountNo);
                    $('#<%=txtCorporateAccountName.ClientID %>').val(masterCorporateAccountName);
                    cboGenderAppointment.SetValue(gender);

                    cboGenderAppointment.SetEnabled(false);

                    if (result.GCCustomerType != "") {
                        var appID = $('#<%:hdnAppointmentID.ClientID %>').val();
                        if (appID == "" || appID == "0") {
                            cboAppointmentPayer.SetValue(result.GCCustomerType);
                            setTblPayerCompanyVisibility();
                            $('#<%:hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                            $('#<%:txtPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                            $('#<%:txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                        }
                    }
                }
                else {
                    $('#<%=hdnMRN.ClientID %>').val('');
                    $('#<%=txtMRN.ClientID %>').val('');

                    $('#<%=txtFirstName.ClientID %>').val('');
                    $('#<%=txtMiddleName.ClientID %>').val('');
                    $('#<%=txtFamilyName.ClientID %>').val('');
                    $('#<%=txtAddress.ClientID %>').val('');
                    $('#<%=txtPhoneNo.ClientID %>').val('');
                    $('#<%=txtMobilePhone.ClientID %>').val('');
                    $('#<%=txtEmail.ClientID %>').val('');
                    $('#<%=txtDOBMainAppt.ClientID %>').val('');
                    $('#<%=txtCorporateAccountNo.ClientID %>').val('');
                    $('#<%=txtCorporateAccountName.ClientID %>').val('');

                    cboAppointmentPayer.SetValue('');
                }
            });
        }
        //#endregion

        //#region Room
        function onGetServiceUnitRoomFilterExpression() {
            var filterExpression = 'HealthcareServiceUnitID = ' + cboServiceUnit.GetValue() + ' AND IsDeleted = 0';
            return filterExpression;
        }

        $('#lblRoom.lblLink').live('click', function () {
            openSearchDialog('serviceunitroom', onGetServiceUnitRoomFilterExpression(), function (value) {
                $('#<%=txtRoomCode.ClientID %>').val(value);
                onTxtServiceUnitRoomCodeChanged(value);
            });
        });

        $('#<%=txtRoomCode.ClientID %>').live('change', function () {
            onTxtServiceUnitRoomCodeChanged($(this).val());
        });

        function onTxtServiceUnitRoomCodeChanged(value) {
            var filterExpression = onGetServiceUnitRoomFilterExpression() + " AND RoomCode = '" + value + "'";
            Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                    $('#<%=txtRoomName.ClientID %>').val(result.RoomName);
                }
                else {
                    $('#<%=hdnRoomID.ClientID %>').val('');
                    $('#<%=txtRoomCode.ClientID %>').val('');
                    $('#<%=txtRoomName.ClientID %>').val('');
                }
            });
        }
        //#endregion 

        //#region Visit Type
        $('#lblVisitType').live('click', function () {
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (paramedicID != '') {
                var filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
                Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                    var filterExpression = '';
                    if (result > 0) {
                        filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + ")";
                        openSearchDialog('visittype', filterExpression, function (value) {
                            $('#<%=txtVisitTypeCode.ClientID %>').val(value);
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
                                $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                                onTxtVisitTypeCodeChanged(value);
                            });
                        });
                    }
                });
            }
            else {
                showToast('Warning', 'Silahkan Pilih Dokter Terlebih Dahulu');
            }
        });

        $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtVisitTypeCodeChanged($(this).val());
        });

        function onTxtVisitTypeCodeChanged(value) {
            var filterExpression = '';

            var serviceUnitID = cboServiceUnit.GetValue();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
            Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                var filterExpression = '';
                if (result > 0) {
                    filterExpression += "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + " AND VisitTypeCode = '" + value + "'";
                    Methods.getObject('GetvParamedicVisitTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                            $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                            $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                            $('#<%=hdnVisitDuration.ClientID %>').val(result.VisitDuration);
                        }
                        else {
                            $('#<%=hdnVisitTypeID.ClientID %>').val('');
                            $('#<%=txtVisitTypeCode.ClientID %>').val('');
                            $('#<%=txtVisitTypeName.ClientID %>').val('');
                            $('#<%=txtVisitDuration.ClientID %>').val('');
                            $('#<%=hdnVisitDuration.ClientID %>').val('');
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
                                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                    $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                                    $('#<%=hdnVisitDuration.ClientID %>').val(result.VisitDuration);
                                }
                                else {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                                    $('#<%=txtVisitDuration.ClientID %>').val('');
                                    $('#<%=hdnVisitDuration.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            filterExpression = "VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                            Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                    $('#<%=txtVisitDuration.ClientID %>').val('15');
                                    $('#<%=hdnVisitDuration.ClientID %>').val('15');
                                }
                                else {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                                    $('#<%=txtVisitDuration.ClientID %>').val('');
                                    $('#<%=hdnVisitDuration.ClientID %>').val('');
                                }
                            });
                        }
                    });
                }
            });
        }

        $('#btnPlusVisitDuration').live('click', function () {
            var hdnVisitDuration = parseFloat($('#<%=hdnVisitDuration.ClientID %>').val());
            var visitDuration = parseFloat($('#<%=txtVisitDuration.ClientID %>').val());
            if ($('#<%=txtVisitDuration.ClientID %>').val() != '') {
                var value = visitDuration + hdnVisitDuration
                $('#<%=txtVisitDuration.ClientID %>').val(value);
            }
            else {
                showToast('Warning', 'Harap pilih jenis kunjungan dahulu!');
            }
        });

        $('#btnMinVisitDuration').live('click', function () {
            var hdnVisitDuration = parseFloat($('#<%=hdnVisitDuration.ClientID %>').val());
            var visitDuration = parseFloat($('#<%=txtVisitDuration.ClientID %>').val());

            if (visitDuration != 0 && visitDuration > hdnVisitDuration) {
                var value = parseFloat($('#<%=txtVisitDuration.ClientID %>').val());
                value -= hdnVisitDuration;
                $('#<%=txtVisitDuration.ClientID %>').val(value);
            }
            else if ($('#<%=hdnVisitDuration.ClientID %>').val() != '' && $('#<%=txtVisitDuration.ClientID %>').val() != '') {
                if ($('#<%=hdnVisitDuration.ClientID %>').val() != $('#<%=txtVisitDuration.ClientID %>').val()) {
                    showToast('Warning', 'Harap pilih jenis kunjungan dahulu!');
                }
            }
            else {
                showToast('Warning', 'Harap pilih jenis kunjungan dahulu!');
            }
        });

        //#endregion

        //#region Referral Description
        function getReferralDescriptionFilterExpression() {
            var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        function getReferralParamedicFilterExpression() {
            var filterExpression = "GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'";
            return filterExpression;
        }

        $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
            var referral = cboReferral.GetValue();
            if (referral == Constant.ReferrerGroup.DOKTERRS) {
                openSearchDialog('referrerparamedic', getReferralParamedicFilterExpression(), function (value) {
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                    onTxtReferralDescriptionCodeChanged(value);
                });
            } else {
                openSearchDialog('referrer', getReferralDescriptionFilterExpression(), function (value) {
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                    onTxtReferralDescriptionCodeChanged(value);
                });
            }
        });

        $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
            onTxtReferralDescriptionCodeChanged($(this).val());
        });

        function onTxtReferralDescriptionCodeChanged(value) {
            var filterExpression = "";
            var referral = cboReferral.GetValue();
            if (referral == Constant.ReferrerGroup.DOKTERRS) {
                filterExpression = getReferralParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ParamedicName);

                        $('#<%:hdnReferrerID.ClientID %>').val('');
                    }
                    else {
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    }
                });
            } else {
                filterExpression = getReferralDescriptionFilterExpression() + " AND CommCode = '" + value + "'";
                Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);

                        $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                    }
                    else {
                        $('#<%:hdnReferrerID.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    }
                });
            }
        }
        //#endregion

        $('#<%=chkIsNewPatient.ClientID %>').change();
        $('#leftPageNavPanel ul li').first().click();
    });

    function onBeforeSaveRecordEntryPopup() {
        var values = getFormValues();
        return true;
    }

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        getPayerCompany('');
        if ($('#<%:hdnContractID.ClientID %>').val() != '') {
            getCoverageType('');
        }
    }

    function setTblPayerCompanyVisibility() {
        var customerType = cboAppointmentPayer.GetValue();
        $('#<%:trControlClassCare.ClientID %>').hide();
        if (customerType == "<%:GetCustomerTypePersonal() %>") {
            $('#<%:trPayerCompany1.ClientID %>').hide();
            $('#<%:trPayerCompany2.ClientID %>').hide();
            $('#<%:trPayerCompany3.ClientID %>').hide();
            $('#<%:trPayerCompany4.ClientID %>').hide();
            $('#<%:trPayerCompany5.ClientID %>').hide();
            $('#<%:trEmployee.ClientID %>').hide();            
            $('#<%:trCoverageLimit.ClientID %>').hide();
            $('#<%:trCoverageLimitPerDay.ClientID %>').hide();            
        }
        else {
            $('#<%:trPayerCompany1.ClientID %>').show();
            $('#<%:trPayerCompany2.ClientID %>').show();
            $('#<%:trPayerCompany3.ClientID %>').show();
            $('#<%:trPayerCompany4.ClientID %>').show();
            $('#<%:trPayerCompany5.ClientID %>').show();

            if (customerType == "<%:GetCustomerTypeHealthcare() %>")
                $('#<%:trEmployee.ClientID %>').removeAttr('style');
            else
                $('#<%:trEmployee.ClientID %>').attr('style', 'display:none');
        }
    }

    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboAppointmentPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
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
        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';

            if (result != null) {
                if (result.IsBlackList == false) {
                    $('#<%:hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%:hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                    $('#btnPayerNotesDetail').removeAttr('enabled');

                    if (result.GCCustomerType == $('#<%:hdnTipeCustomerBPJS.ClientID %>').val()) {
                        $('#<%:hdnIsBPJS.ClientID %>').val("1");
                    }
                    else {
                        $('#<%:hdnIsBPJS.ClientID %>').val("0");
                    }

                    var filterExpression = getPayerContractFilterExpression();
                    Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                        if (result == 1) {
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                                    $('#<%:txtContractNo.ClientID %>').val(result.ContractNo);
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
                                        if (result.ControlClassID != '0')
                                            cboControlClassCare.SetValue(result.ControlClassID);
                                        else
                                            cboControlClassCare.SetValue('');
                                    }
                                    else {
                                        $('#<%:trControlClassCare.ClientID %>').hide();
                                        cboControlClassCare.SetValue('');
                                    }
                                    onAfterContractNoChanged();
                                    onCheckCustomerMember($('#<%:hdnPayerID.ClientID %>').val(), $('#<%:txtMRN.ClientID %>').val());
                                }
                            });
                        }
                        else {
                            $('#<%:hdnContractID.ClientID %>').val('');
                            $('#<%:txtContractNo.ClientID %>').val('');
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
                    cboRegistrationPayer.SetValue("<%:GetCustomerTypePersonal() %>");
                    $('#<%:chkUsingCOB.ClientID %>').hide();
                    $('#<%:tblPayerCompany.ClientID %>').hide();
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

    function onCheckCustomerMember(payerID, medicalNoID) {
        if (payerID != '' && medicalNoID != '') {
            var filterExpression = "MedicalNo = '" + medicalNoID + "' AND BusinessPartnerID = '" + payerID + "'";
            Methods.getObject('GetvCustomerMemberList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:txtParticipantNo.ClientID %>').val(result.MemberNo);
                    $('#<%:txtParticipantNo.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%:txtParticipantNo.ClientID %>').val('');
                    $('#<%:txtParticipantNo.ClientID %>').removeAttr('readonly');
                }
            });
        }
    }
    //#endregion

    //#region Coverage Type
    function getCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%:hdnContractCoverageMemberCount.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";
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

    //#region Payer Contract
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
<style type="text/css">
</style>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentMode" value="" />
    <input type="hidden" runat="server" id="hdnSessionID" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" runat="server" id="hdnAppointmentID" value='0' />
    <input type="hidden" runat="server" id="hdnQueueNo" value='0' />
    <input type="hidden" runat="server" id="hdnAppointmentDate" value='' />
    <input type="hidden" runat="server" id="hdnGCAppointmentStatus" value='' />
    <input type="hidden" runat="server" id="hdnPayerID" value='0' />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value='0' />
    <input type="hidden" runat="server" id="hdnParamedicID" value='0' />
    <input type="hidden" id="hdnIsBridgingToGateway" runat="server" />
    <input type="hidden" id="hdnIsAppintmentAllowBackDate" runat="server" />
    <input type="hidden" id="hdnTipeCustomerBPJS" runat="server" />
    <input type="hidden" id="hdnBPJSKetenagakerjaanID" runat="server" />
    <input type="hidden" id="hdnIsBridgingToQumatic" runat="server" />
    <input type="hidden" id="hdnApiKeyQumatic" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" runat="server" />
    <input type="hidden" id="hdnIsUsingValidationMaxAppointment" runat="server" />
    <input type="hidden" id="hdnDefaultServiceUnitInterval" runat="server" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnIsBPJS" runat="server" value = "0" />
    
     <input type="hidden" id="hdnIsMobilePhoneNumeric" runat="server" value = "0" />
    <input type="hidden" runat="server" id="hdnFormType" value="" />
    <input type="hidden" runat="server" id="hdnDivHTML" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />

    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align:top">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="vertical-align:top">
                            <div id="divQueueNo" runat="server" class="divQueueNo w3-blue w3-padding-16 w3-monospace w3-xxxlarge" style="text-align:center; text-shadow:1px 1px 0 #444; width:100%;">000</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">
                            <div id="leftPageNavPanel" class="w3-border">
                                <ul>
                                    <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                                    <li contentID="divPage2" title="Data Perjanjian" class="w3-hover-red">Data Perjanjian</li>
                                    <li contentID="divPage3" title="Data Pembayar" class="w3-hover-red">Data Pembayar</li>
                                </ul>     
                            </div> 
                        </td>
                    </tr>
                </table>

            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNewPatient" runat="server" /><%=GetLabel("Pasien Baru")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMRN">
                                    <%=GetLabel("No. Rekam Medis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnMRN" value="" runat="server" />
                                <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Pasien")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 11%">
                                            <dxe:ASPxComboBox ID="cboSalutationAppo" ClientInstanceName="cboSalutationAppo" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td style="width: 3px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFirstName" Width="100%" runat="server" />
                                        </td>
                                        <td style="width: 3px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtMiddleName" Width="100%" runat="server" />
                                        </td>
                                        <td style="width: 3px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="txtFamilyName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trGender" runat="server">
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGenderAppointment" ClientInstanceName="cboGenderAppointment"
                                    Width="30%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trDOB" runat="server">
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Lahir")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDOBMainAppt" ReadOnly="true" Width="30%" runat="server" CssClass="datepicker"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alamat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No. Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor HP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" CssClass="email" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountName" Width="200px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblAppointmentNo">
                                    <%=GetLabel("No. Perjanjian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentNo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td style="text-align: right;">
                                <label class="lblNormal" id="lblQueueNo">
                                    <%=GetLabel("No. Antrian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQueueNo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentDate" ReadOnly="true" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtServiceUnit" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblRoom">
                                    <%=GetLabel("Ruang")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnRoomID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCode" CssClass="required" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dokter/Tenaga Medis")%></label>
                            </td>
                            <td style="display: none">
                                <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPhysician" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblVisitType">
                                    <%=GetLabel("Jenis Kunjungan")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                                <input type="hidden" id="hdnVisitDuration" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Durasi")%></label>
                            </td>
                            <td colspan="3">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVisitDuration" Width="100%" runat="server"  ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="padding-left:3px">
                                            <input type="button" id="btnMinVisitDuration" style="width: 32px;" value='<%= GetLabel("-")%>' />
                                            &nbsp;
                                            <input type="button" id="btnPlusVisitDuration" style="width: 32px;" value='<%= GetLabel("+")%>' />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trAppointmentMethod" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Perjanjian")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboAppointmentMethod" ClientInstanceName="cboAppointmentMethod"
                                    Width="100%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Rujukan")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" runat="server" id="lblReferralDescription">
                                    <%:GetLabel("Deskripsi Rujukan")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan / Alasan Kunjungan")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                    <table class="tblEntryContent" runat="server" style="width: 100%;" id="tblPayerCompany">
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%:GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboAppointmentPayer" ClientInstanceName="cboAppointmentPayer"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td id="chkUsingCOB" runat="server">
                                <asp:CheckBox ID="chkIsUsingCOB" Checked="false" runat="server" /><%:GetLabel("Peserta COB")%>
                            </td>
                        </tr>

                        <tr id = "trPayerCompany1">
                            <td style="width: 30%" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPayerCompany">
                                    <%:GetLabel("Instansi")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="Hidden1" value="" runat="server" />
                                <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                                <input type="hidden" id="hdnIsBlacklistPayer" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                        <col style="width: 3px" />
                                        <col style="width: 20px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <input type="button" id="btnPayerNotesDetail" value="..." style="display:none" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id = "trPayerCompany2">
                            <td style="width: 30%" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblContract">
                                    <%:GetLabel("Kontrak")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnContractID" value="" runat="server" />
                                <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                                <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                                <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id = "trPayerCompany3">
                            <td style="width: 30%" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                    <%:GetLabel("Tipe Coverage")%></label>
                            </td>
                            <td colspan="2">
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
                        <tr id = "trPayerCompany4">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("No. Peserta")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtParticipantNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trEmployee" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblEmployee">
                                    <%:GetLabel("Pegawai")%></label>
                            </td>
                            <td colspan="2">
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
                                            <asp:TextBox ID="txtEmployeeName" ReadOnly="true" Width="100%" runat="server" />
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
                            <td colspan="2">
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
                        <tr id = "trPayerCompany5">
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            </td>
                            <td>
                                <input type="button" id="btnContractSummary" value="Ringkasan Kontrak" style="display:none" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <table class="tblContentArea" border="0">
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="height: 450px;overflow-y: auto;"></div>
            </td>
        </tr>
    </table>
</div>
