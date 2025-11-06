<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationBirthRecordCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationBirthRecordCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_registrationbirthrecordctl">
    registerCollapseExpandHandler();

    setDatePicker('<%=txtDOBCtlAdd.ClientID %>');
    $('#<%=txtDOBCtlAdd.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#<%=chkIsSSNTemporaryCtlAdd.ClientID %>').prop('checked', true);
    var oSSN = $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').val();

    var getDateTimeNow = new Date();
    var oYear = getDateTimeNow.getFullYear();
    var oMonth = (getDateTimeNow.getMonth() + 1);
    if (oMonth < 10) {
        oMonth = "0" + oMonth;
    }
    var oDate = getDateTimeNow.getDate();
    if (oDate < 10) {
        oDate = "0" + oDate;
    }
    var oHour = getDateTimeNow.getHours();
    if (oHour < 10) {
        oHour = "0" + oHour;
    }
    var oMinute = getDateTimeNow.getMinutes();
    if (oMinute < 10) {
        oMinute = "0" + oMinute;
    }
    var oSecond = getDateTimeNow.getSeconds();
    if (oSecond < 10) {
        oSecond = "0" + oSecond;
    }

    var newTempSSN = "99" + oYear + oMonth + oDate + oHour + oMinute + oSecond;

    $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').val(newTempSSN);
    $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').attr('disabled', 'disabled');

    $(function () {
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

        $('#leftPageNavPanel ul li').first().click();
    });

    //#region Mother Registration No
    function getMotherRegistrationNoFilterExpression() {
        var filterExpression = "<%:OnGetMotherRegistrationNoFilterExpression() %>";
        return filterExpression;
    }

    $('#<%=lblMotherRegNo.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('consultvisit', getMotherRegistrationNoFilterExpression(), function (value) {
            $('#<%=hdnMotherVisitIDCtlAdd.ClientID %>').val(value);
            onTxtMotherVisitIDChanged(value);
        });
    });

    function onTxtMotherVisitIDChanged(value) {
        var filterExpression = getMotherRegistrationNoFilterExpression() + " AND VisitID = '" + value + "'";
        Methods.getObject('GetvConsultVisitList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnMotherVisitIDCtlAdd.ClientID %>').val(result.VisitID);
                $('#<%:hdnMotherMRNCtlAdd.ClientID %>').val(result.MRN);
                $('#<%:hdnMotherNameCtlAdd.ClientID %>').val(result.PatientName);
                $('#<%:txtMotherRegNoCtlAdd.ClientID %>').val(result.RegistrationNo);
                $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val(result.ChargeClassID);
                $('#<%:hdnChargeClassBPJSCodeCtlAdd.ClientID %>').val(result.ChargeClassBPJSCode);
                $('#<%:hdnChargeClassBPJSTypeCtlAdd.ClientID %>').val(result.ChargeClassBPJSType);
                $('#<%:txtChargeClassCodeCtlAdd.ClientID %>').val(result.ChargeClassCode);
                $('#<%:txtChargeClassNameCtlAdd.ClientID %>').val(result.ChargeClassName);

                $('#<%:txtAddressCtlAdd.ClientID %>').val(result.StreetName);
                $('#<%=hdnZipCodeCtlAdd.ClientID %>').val(result.ZipCodeID);
                $('#<%=txtZipCodeCtlAdd.ClientID %>').val(result.ZipCode);
                $('#<%=txtRTDataCtlAdd.ClientID %>').val(result.RT);
                $('#<%=txtRWDataCtlAdd.ClientID %>').val(result.RW);
                $('#<%=txtCityCtlAdd.ClientID %>').val(result.City);
                $('#<%=txtCountyCtlAdd.ClientID %>').val(result.County);
                $('#<%=txtDistrictCtlAdd.ClientID %>').val(result.District);
                $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val(result.GCState.split('^')[1]);
                $('#<%=txtProvinceNameCtlAdd.ClientID %>').val(result.State);

                $('#<%=txtAddressDomicileCtlAdd.ClientID %>').val(result.StreetName);
                $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val(result.ZipCodeID);
                $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val(result.ZipCode);
                $('#<%=txtRTDomicileDataCtlAdd.ClientID %>').val(result.RT);
                $('#<%=txtRWDomicileDataCtlAdd.ClientID %>').val(result.RW);
                $('#<%=txtCityDomicileCtlAdd.ClientID %>').val(result.City);
                $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val(result.County);
                $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val(result.District);
                $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val(result.GCState.split('^')[1]);
                $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val(result.State);

                $('#<%:txtTelephoneNo1CtlAdd.ClientID %>').val(result.PhoneNo1);
                $('#<%:txtMobilePhone1CtlAdd.ClientID %>').val(result.MobilePhoneNo1);

                if (result.GCReligion != '') {
                    $('#<%=txtReligionCodeCtlAdd.ClientID %>').val(result.GCReligion.split('^')[1]);
                    $('#<%=txtReligionNameCtlAdd.ClientID %>').val(result.Religion);
                }
                else {
                    $('#<%=txtReligionCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtReligionNameCtlAdd.ClientID %>').val('');
                }
            }
            else {
                $('#<%:hdnMotherNameCtlAdd.ClientID %>').val('');
                $('#<%:hdnMotherVisitIDCtlAdd.ClientID %>').val('');
                $('#<%:hdnMotherMRNCtlAdd.ClientID %>').val('');
                $('#<%:hdnClassIDCtlAdd.ClientID %>').val('');
                $('#<%:txtClassCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtClassNameCtlAdd.ClientID %>').val('');
                $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val('');
                $('#<%:hdnChargeClassBPJSCodeCtlAdd.ClientID %>').val('');
                $('#<%:hdnChargeClassBPJSTypeCtlAdd.ClientID %>').val('');
                $('#<%:txtChargeClassCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtChargeClassNameCtlAdd.ClientID %>').val('');

                $('#<%:txtAddressCtlAdd.ClientID %>').val('');
                $('#<%=hdnZipCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtZipCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtRTDataCtlAdd.ClientID %>').val('');
                $('#<%=txtRWDataCtlAdd.ClientID %>').val('');
                $('#<%=txtCityCtlAdd.ClientID %>').val('');
                $('#<%=txtCountyCtlAdd.ClientID %>').val('');
                $('#<%=txtDistrictCtlAdd.ClientID %>').val('');
                $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtProvinceNameCtlAdd.ClientID %>').val('');

                $('#<%=txtAddressDomicileCtlAdd.ClientID %>').val('');
                $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val('');
                $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val('');
                $('#<%=txtRTDomicileDataCtlAdd.ClientID %>').val('');
                $('#<%=txtRWDomicileDataCtlAdd.ClientID %>').val('');
                $('#<%=txtCityDomicileCtlAdd.ClientID %>').val('');
                $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val('');
                $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val('');
                $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val('');

                $('#<%:txtTelephoneNo1CtlAdd.ClientID %>').val('');
                $('#<%:txtMobilePhone1CtlAdd.ClientID %>').val('');

                $('#<%=txtReligionCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtReligionNameCtlAdd.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=chkIsSSNTemporaryCtlAdd.ClientID %>').die('change');
    $('#<%=chkIsSSNTemporaryCtlAdd.ClientID %>').live('change', function () {
        if ($('#<%=chkIsSSNTemporaryCtlAdd.ClientID %>').is(':checked')) {
            var oSSN = $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').val();

            var getDateTimeNow = new Date();
            var oYear = getDateTimeNow.getFullYear();
            var oMonth = (getDateTimeNow.getMonth() + 1);
            if (oMonth < 10) {
                oMonth = "0" + oMonth;
            }
            var oDate = getDateTimeNow.getDate();
            if (oDate < 10) {
                oDate = "0" + oDate;
            }
            var oHour = getDateTimeNow.getHours();
            if (oHour < 10) {
                oHour = "0" + oHour;
            }
            var oMinute = getDateTimeNow.getMinutes();
            if (oMinute < 10) {
                oMinute = "0" + oMinute;
            }
            var oSecond = getDateTimeNow.getSeconds();
            if (oSecond < 10) {
                oSecond = "0" + oSecond;
            }

            var newTempSSN = "99" + oYear + oMonth + oDate + oHour + oMinute + oSecond;

            $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').val(newTempSSN);
            $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').attr('disabled', 'disabled');
        } else {
            $('#<%=txtIdentityCardNoCtlAdd.ClientID %>').removeAttr('disabled');
        }
    });

    function oncboSalutationCtlAddValueChanged() {
    }

    //#region DOB
    $('#<%=txtDOBCtlAdd.ClientID %>').change(function () {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
        var dateSelected = $('#<%=txtDOBCtlAdd.ClientID %>').val();

        var from = dateSelected.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);
        //untuk ambil data
        $('#<%=hdnDOBCtlAdd.ClientID %>').val(from[2] + "-" + from[1] + "-" + from[0]);
        var to = dateToday.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        if (f > t) {
            $('#<%=txtDOBCtlAdd.ClientID %>').val(dateToday);
        }
        var age = Methods.getAgeFromDatePickerFormat($(this).val());
        $('#<%=txtAgeInYearCtlAdd.ClientID %>').val(age.years);
        $('#<%=txtAgeInMonthCtlAdd.ClientID %>').val(age.months);
        $('#<%=txtAgeInDayCtlAdd.ClientID %>').val(age.days);
        oncboSalutationCtlAddValueChanged();

    });

    $('#<%=txtAgeInYearCtlAdd.ClientID %>').change(function () {
        getDOBFromAgeCtlAdd();
    });

    $('#<%=txtAgeInMonthCtlAdd.ClientID %>').change(function () {
        getDOBFromAgeCtlAdd();
    });

    $('#<%=txtAgeInDayCtlAdd.ClientID %>').change(function () {
        getDOBFromAgeCtlAdd();
    });

    function getDOBFromAgeCtlAdd() {
        var now = Methods.stringToDate('<%=GetTodayDate() %>');
        var ageInYear = parseInt($('#<%=txtAgeInYearCtlAdd.ClientID %>').val());
        var ageInMonth = parseInt($('#<%=txtAgeInMonthCtlAdd.ClientID %>').val());
        var ageInDay = parseInt($('#<%=txtAgeInDayCtlAdd.ClientID %>').val());

        now.setYear(now.getFullYear() - ageInYear);
        now.setMonth(now.getMonth() - ageInMonth);
        now.setDate(now.getDate() - ageInDay);

        var dateStr = Methods.dateToDatePickerFormat(now);
        $('#<%=txtDOBCtlAdd.ClientID %>').val(dateStr);
    }
    //#endregion

    //#region Religion
    function onGetSCReligionFilterExpression() {
        var filterExpression = "<%:OnGetSCReligionFilterExpression() %>";
        return filterExpression;
    }

    $('#lblReligion.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCReligionFilterExpression(), function (value) {
            $('#<%=txtReligionCodeCtlAdd.ClientID %>').val(value);
            onTxtReligionCodeChanged(value);
        });
    });

    $('#<%=txtReligionCodeCtlAdd.ClientID %>').change(function () {
        onTxtReligionCodeChanged($(this).val());
    });

    function onTxtReligionCodeChanged(value) {
        var filterExpression = onGetSCReligionFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtReligionNameCtlAdd.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtReligionNameCtlAdd.ClientID %>').val('');
                $('#<%=txtReligionCodeCtlAdd.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Nationality
    function onGetSCNationalityFilterExpression() {
        var filterExpression = "<%:OnGetSCNationalityFilterExpression() %>";
        return filterExpression;
    }

    $('#lblNationality.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCNationalityFilterExpression(), function (value) {
            $('#<%=txtNationalityCodeCtlAdd.ClientID %>').val(value);
            onTxtNationalityCodeChanged(value);
        });
    });

    $('#<%=txtNationalityCodeCtlAdd.ClientID %>').change(function () {
        onTxtNationalityCodeChanged($(this).val());
    });

    function onTxtNationalityCodeChanged(value) {
        var filterExpression = onGetSCNationalityFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtNationalityNameCtlAdd.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtNationalityCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtNationalityNameCtlAdd.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Zip Code
    $('#<%=lblZipCode.ClientID %>').live('click', function (evt) {
        openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
            onTxtZipCodeChanged(value);
        });
    });

    $('#<%=txtZipCodeCtlAdd.ClientID %>').change(function () {
        onTxtZipCodeChangedValue($(this).val());
    });

    function onTxtZipCodeChanged(value) {
        if (value != '') {
            var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeCtlAdd.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeCtlAdd.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityCtlAdd.ClientID %>').val(result.City);
                    $('#<%=txtCountyCtlAdd.ClientID %>').val(result.County);
                    $('#<%=txtDistrictCtlAdd.ClientID %>').val(result.District);
                    $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceNameCtlAdd.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtZipCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtCityCtlAdd.ClientID %>').val('');
                    $('#<%=txtCountyCtlAdd.ClientID %>').val('');
                    $('#<%=txtDistrictCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceNameCtlAdd.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtZipCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtCityCtlAdd.ClientID %>').val('');
            $('#<%=txtCountyCtlAdd.ClientID %>').val('');
            $('#<%=txtDistrictCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceNameCtlAdd.ClientID %>').val('');
        }
    }

    function onTxtZipCodeChangedValue(value) {
        if (value != '') {
            var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeCtlAdd.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeCtlAdd.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityCtlAdd.ClientID %>').val(result.City);
                    $('#<%=txtCountyCtlAdd.ClientID %>').val(result.County);
                    $('#<%=txtDistrictCtlAdd.ClientID %>').val(result.District);
                    $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceNameCtlAdd.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtZipCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtCityCtlAdd.ClientID %>').val('');
                    $('#<%=txtCountyCtlAdd.ClientID %>').val('');
                    $('#<%=txtDistrictCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceNameCtlAdd.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtZipCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtCityCtlAdd.ClientID %>').val('');
            $('#<%=txtCountyCtlAdd.ClientID %>').val('');
            $('#<%=txtDistrictCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceNameCtlAdd.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Domicile Zip Code
    $('#<%=lblZipCodeDomicile.ClientID %>').live('click', function (evt) {
        openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
            onTxtZipCodeDomicileChanged(value);
        });
    });

    $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').change(function () {
        onTxtZipCodeDomicileChangedValue($(this).val());
    });

    function onTxtZipCodeDomicileChanged(value) {
        if (value != '') {
            var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityDomicileCtlAdd.ClientID %>').val(result.City);
                    $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val(result.County);
                    $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val(result.District);
                    $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtCityDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtCityDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val('');
        }
    }

    function onTxtZipCodeDomicileChangedValue(value) {
        if (value != '') {
            var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityDomicileCtlAdd.ClientID %>').val(result.City);
                    $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val(result.County);
                    $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val(result.District);
                    $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtCityDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtCityDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val('');
            $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val('');
        }
    }
    //#endregion

    //#region City
    $('#lblCity.lblLink').click(function () {
        openSearchDialog('city', '', function (value) {
            $('#<%=txtCityCtlAdd.ClientID %>').val(value);
        });
    });
    //#endregion

    //#region Domicile City
    $('#lblCityDomicile.lblLink').click(function () {
        openSearchDialog('city', '', function (value) {
            $('#<%=txtCityDomicileCtlAdd.ClientID %>').val(value);
        });
    });
    //#endregion

    //#region Province
    function onGetSCProvinceFilterExpression() {
        var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
        return filterExpression;
    }

    $('#lblProvince.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
            $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val(value);
            onTxtProvinceCodeChanged(value);
        });
    });

    $('#<%=txtProvinceCodeCtlAdd.ClientID %>').change(function () {
        onTxtProvinceCodeChanged($(this).val());
    });

    function onTxtProvinceCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtProvinceNameCtlAdd.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtProvinceNameCtlAdd.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Domicile Province

    $('#lblDomicileProvince.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
            $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val(value);
            onTxtProvinceDomicileCodeChanged(value);
        });
    });

    $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').change(function () {
        onTxtProvinceDomicileCodeChanged($(this).val());
    });

    function onTxtProvinceDomicileCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val('');
                $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=chkCopyKTP.ClientID %>').live('change', function () {
        if ($(this.checked)) {
            var address = $('#<%=txtAddressCtlAdd.ClientID %>').val();
            var zipCodeHdn = $('#<%=hdnZipCodeCtlAdd.ClientID %>').val();
            var zipCode = $('#<%=txtZipCodeCtlAdd.ClientID %>').val();
            var rt = $('#<%=txtRTDataCtlAdd.ClientID %>').val();
            var rw = $('#<%=txtRWDataCtlAdd.ClientID %>').val();
            var city = $('#<%=txtCityCtlAdd.ClientID %>').val();
            var county = $('#<%=txtCountyCtlAdd.ClientID %>').val();
            var district = $('#<%=txtDistrictCtlAdd.ClientID %>').val();
            var provinceCode = $('#<%=txtProvinceCodeCtlAdd.ClientID %>').val();
            var provinceName = $('#<%=txtProvinceNameCtlAdd.ClientID %>').val();

            $('#<%=txtAddressDomicileCtlAdd.ClientID %>').val(address);
            $('#<%=hdnZipCodeDomicileCtlAdd.ClientID %>').val(zipCodeHdn);
            $('#<%=txtZipCodeDomicileCtlAdd.ClientID %>').val(zipCode);
            $('#<%=txtRTDomicileDataCtlAdd.ClientID %>').val(rt);
            $('#<%=txtRWDomicileDataCtlAdd.ClientID %>').val(rw);
            $('#<%=txtCityDomicileCtlAdd.ClientID %>').val(city);
            $('#<%=txtCountyDomicileCtlAdd.ClientID %>').val(county);
            $('#<%=txtDistrictDomicileCtlAdd.ClientID %>').val(district);
            $('#<%=txtProvinceDomicileCodeCtlAdd.ClientID %>').val(provinceCode);
            $('#<%=txtProvinceDomicileNameCtlAdd.ClientID %>').val(provinceName);
        }
    });

    //#region Physician
    function onGetPhysicianFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '') {
            serviceUnitID = '0';
        }

        var filterExpression = 'IsDeleted = 0 AND IsAvailable = 1 AND IsHasPhysicianRole = 1';
        if (serviceUnitID != '0') {
            filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ") AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
        }

        return filterExpression;
    }

    $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%:txtPhysicianCodeCtlAdd.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%:txtPhysicianCodeCtlAdd.ClientID %>').live('change', function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onHdnPhysicianIDChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicID = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                cboSpecialtyCtlAdd.SetValue(result.SpecialtyID);

                $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val(result.ParamedicID);
                $('#<%:txtPhysicianCodeCtlAdd.ClientID %>').val(result.ParamedicCode);
                $('#<%:txtPhysicianNameCtlAdd.ClientID %>').val(result.ParamedicName);

                var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                if (healthcareServiceUnitID == '' || healthcareServiceUnitID == '0') {
                    var filterExpression = getServiceUnitFilterFilterExpression();
                    var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
                    Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
                        if (result != null) {
                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                            $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val(result.ServiceUnitCode);
                            var parameter2 = "ServiceUnitCode = '" + result.ServiceUnitCode + "'";
                            $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val(result.ServiceUnitName);
                            Methods.getObject('GetServiceUnitMasterList', parameter2, function (result2) {
                                if (result2 != null) {
                                    $('#<%=hdnBPJSPoli.ClientID %>').val(result2.BPJSPoli);
                                }
                                else $('#<%=hdnBPJSPoli.ClientID %>').val('');
                            });
                            if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                                var filterExpression = onGetVisitTypeFilterExpression();
                                Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                    if (result != null) {
                                        $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                                        $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                                        $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                                    }
                                    else {
                                        $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                                        $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                                        $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                                    }
                                });
                            }
                        }
                        else {
                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                            $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                            $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
                        }
                    });
                }
                else {
                    if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                        var filterExpression = onGetVisitTypeFilterExpression();
                        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                                $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                                $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                            }
                            else {
                                $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                                $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                                $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                            }
                        });
                    }
                }
            }
            else {
                cboSpecialtyCtlAdd.SetValue('');
                $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val('');
                $('#<%:txtPhysicianCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtPhysicianNameCtlAdd.ClientID %>').val('');
            }
        });
        if ($('#<%=hdnRoomIDCtlAdd.ClientID %>').val() == '') getRoom('');
    }

    function onTxtPhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                var filterExpressionLeave = "IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                    if (resultLeave == null) {
                        cboSpecialtyCtlAdd.SetValue(result.SpecialtyID);

                        $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val(result.ParamedicID);
                        $('#<%:txtPhysicianNameCtlAdd.ClientID %>').val(result.ParamedicName);

                        if ($('#<%:hdnIsBridgingToBPJS.ClientID %>').val() == "1") {
                            if (result.BPJSReferenceInfo == "" || result.BPJSReferenceInfo == null) {
                                $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val('');
                                showToast("BPJS Bridging", "Dokter " + result.ParamedicName + " belum dimapping dengan Referensi BPJS (VClaim dan HFIS) !");
                            }
                            else {
                                $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val(result.BPJSReferenceInfo);
                            }
                        }

                        var otomatisIsiServiceUnit = $('#<%:hdnPilihDokterOtomatisIsiUnit.ClientID %>').val();

                        var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                        if (healthcareServiceUnitID == '' || healthcareServiceUnitID == '0') {
                            var filterExpression = getServiceUnitFilterFilterExpression();
                            var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
                            if (otomatisIsiServiceUnit == "0") {
                                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                                $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
                            } else {
                                Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
                                    if (result != null) {
                                        $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                                        $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val(result.ServiceUnitCode);
                                        $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val(result.ServiceUnitName);

                                        if (result.GCClinicGroup == Constant.ClinicGroup.BPJS)
                                            $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                        else
                                            $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');

                                        var parameter2 = "ServiceUnitCode = '" + result.ServiceUnitCode + "'";
                                        Methods.getObject('GetServiceUnitMasterList', parameter2, function (result2) {
                                            if (result2 != null) {
                                                $('#<%=hdnBPJSPoli.ClientID %>').val(result2.BPJSPoli);
                                            }
                                            else $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                        });
                                        if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                                            var filterExpression = onGetVisitTypeFilterExpression();
                                            Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                                if (result != null) {
                                                    $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                                                    $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                                                    $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                                                }
                                                else {
                                                    $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                                                    $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                                                    $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                                                }
                                            });
                                        }
                                    }
                                    else {
                                        $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                        $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                                        $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
                                        $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                    }
                                });
                            }
                        }
                        else {
                            if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                                var filterExpression = onGetVisitTypeFilterExpression();
                                Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                    if (result != null) {
                                        $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                                        $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                                        $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                                    }
                                    else {
                                        $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                                        $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                                        $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                                    }
                                });
                            }
                        }
                    }
                    else {
                        var departmentID = 'INPATIENT';
                        var isLeave = $('#<%:hdnRegistrasiSelainRajalMemperhatikanCutiDokter.ClientID %>').val();
                        if (isLeave == '1') {
                            cboSpecialtyCtlAdd.SetValue('');
                            $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val('');
                            $('#<%:txtPhysicianCodeCtlAdd.ClientID %>').val('');
                            $('#<%:txtPhysicianNameCtlAdd.ClientID %>').val('');
                            var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                            showToast("INFORMASI", info);
                        }
                        else {
                            cboSpecialtyCtlAdd.SetValue(result.SpecialtyID);

                            $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val(result.ParamedicID);
                            $('#<%:txtPhysicianNameCtlAdd.ClientID %>').val(result.ParamedicName);

                            if ($('#<%:hdnIsBridgingToBPJS.ClientID %>').val() == "1") {
                                if (result.BPJSReferenceInfo == "" || result.BPJSReferenceInfo == null) {
                                    $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val('');
                                    showToast("BPJS Bridging", "Dokter " + result.ParamedicName + " belum dimapping dengan Referensi BPJS (VClaim dan HFIS) !");
                                }
                                else {
                                    $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val(result.BPJSReferenceInfo);
                                }
                            }

                            var otomatisIsiServiceUnit = $('#<%:hdnPilihDokterOtomatisIsiUnit.ClientID %>').val();

                            var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                            if (healthcareServiceUnitID == '' || healthcareServiceUnitID == '0') {
                                var filterExpression = getServiceUnitFilterFilterExpression();
                                var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
                                if (otomatisIsiServiceUnit == "0") {
                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                    $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                                    $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
                                } else {
                                    Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
                                        if (result != null) {
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                                            $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val(result.ServiceUnitCode);
                                            $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val(result.ServiceUnitName);

                                            if (result.GCClinicGroup == Constant.ClinicGroup.BPJS)
                                                $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                            else
                                                $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');

                                            var parameter2 = "ServiceUnitCode = '" + result.ServiceUnitCode + "'";
                                            Methods.getObject('GetServiceUnitMasterList', parameter2, function (result2) {
                                                if (result2 != null) {
                                                    $('#<%=hdnBPJSPoli.ClientID %>').val(result2.BPJSPoli);
                                                }
                                                else $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                            });
                                            if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                                                var filterExpression = onGetVisitTypeFilterExpression();
                                                Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                                    if (result != null) {
                                                        $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                                                        $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                                                        $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                                                    }
                                                    else {
                                                        $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                                                        $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                                                        $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                                                    }
                                                });
                                            }
                                        }
                                        else {
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                            $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                            $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                                            $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
                                            $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                        }
                                    });
                                }
                            }
                            else {
                                if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                                    var filterExpression = onGetVisitTypeFilterExpression();
                                    Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                        if (result != null) {
                                            $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                                            $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                                            $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                                        }
                                        else {
                                            $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                                            $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                                            $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                                        }
                                    });
                                }
                            }

                        }
                    }
                });
            }
            else {
                cboSpecialtyCtlAdd.SetValue('');
                $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val('');
                $('#<%:txtPhysicianCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtPhysicianNameCtlAdd.ClientID %>').val('');
            }
        });
        if ($('#<%=hdnRoomIDCtlAdd.ClientID %>').val() == '') getRoom('');
    }
    //#endregion

    //#region Service Unit
    var serviceUnitUserCount = parseInt('<%:serviceUnitUserCount %>');
    function getServiceUnitFilterFilterExpression() {
        var filterExpression = '';
        var classID = $('#<%:hdnClassIDCtlAdd.ClientID %>').val();
        if (classID != '')
            filterExpression = 'HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitRoom WHERE ClassID = ' + classID + ')';

        var paramedicID = $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val();
        if (paramedicID != '') {
            if (filterExpression != '')
                filterExpression += ' AND ';
            filterExpression += '(HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = ' + paramedicID + '))';
        }

        if (filterExpression != '') {
            filterExpression += ' AND ';
        }
        filterExpression += 'IsUsingRegistration = 1';
        return filterExpression;
    }

    $('#<%:lblServiceUnit.ClientID %>.lblLink').live('click', function () {
        var parameter = "<%:GetServiceUnitUserParameter() %>" + getServiceUnitFilterFilterExpression();
        openSearchDialog('serviceunitroleuser', parameter, function (value) {
            $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val(value);
            onTxtClinicCodeChanged(value);
        });
    });

    $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').live('change', function () {
        onTxtClinicCodeChanged($(this).val());
    });

    function onTxtClinicCodeChanged(value) {
        var filterExpression = getServiceUnitFilterFilterExpression();
        if (filterExpression != '')
            filterExpression += ' AND ';
        filterExpression += "ServiceUnitCode = '" + value + "'";
        var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
        Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
            if (result != null) {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val(result.ServiceUnitName);

                var parameter2 = "ServiceUnitCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvHealthcareServiceUnitList', parameter2, function (result2) {
                    if (result2 != null) {
                        $('#<%=hdnBPJSPoli.ClientID %>').val(result2.BPJSPoli);

                        if (result2.DepartmentID != "INPATIENT") {
                            if (result2.IsChargeClassEditableForNonInpatient == "1") {
                                $('#<%=trChargeClass.ClientID %>').removeAttr('style');
                            } else {
                                $('#<%=trChargeClass.ClientID %>').attr('style', 'display:none');
                            }
                        } else {
                            $('#<%=trChargeClass.ClientID %>').removeAttr('style');
                        }
                    }
                    else {
                        $('#<%=hdnBPJSPoli.ClientID %>').val('');
                        $('#<%=trChargeClass.ClientID %>').attr('style', 'display:none');
                    }
                });

                if (result.GCClinicGroup == Constant.ClinicGroup.BPJS)
                    $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                else
                    $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');
            }
            else {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
            }
            getRoom('');

            var paramedicID = $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val();
            if (paramedicID != '' && paramedicID != '0') {
                if ($('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val() == '') {
                    var filterExpression = onGetVisitTypeFilterExpression();
                    Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                            $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(result.VisitTypeCode);
                            $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
                        }
                        else {
                            $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                            $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                            $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
                        }
                    });
                }
            }
        });
    }
    //#endregion

    //#region Class Care
    function getClassCareFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = 'IsDeleted = 0';
        if (serviceUnitID != '')
            filterExpression = 'ClassID IN (SELECT ClassID FROM vServiceUnitRoom WHERE HealthcareServiceUnitID = ' + serviceUnitID + ' AND IsDeleted = 0) AND IsDeleted = 0';
        return filterExpression;
    }

    $('#<%:lblClass.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('classcare', getClassCareFilterExpression(), function (value) {
            $('#<%:txtClassCodeCtlAdd.ClientID %>').val(value);
            onTxtClassCodeChanged(value);
        });
    });

    $('#<%:txtClassCodeCtlAdd.ClientID %>').live('change', function () {
        onTxtClassCodeChanged($(this).val());
    });

    function onTxtClassCodeChanged(value) {
        var filterExpression = getClassCareFilterExpression() + " AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnClassIDCtlAdd.ClientID %>').val(result.ClassID);
                $('#<%:txtClassNameCtlAdd.ClientID %>').val(result.ClassName);

                if (result.IsUsedInChargeClass) {
                    $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val(result.ClassID);
                    $('#<%:hdnChargeClassBPJSCodeCtlAdd.ClientID %>').val(result.BPJSClassCode);
                    $('#<%:hdnChargeClassBPJSTypeCtlAdd.ClientID %>').val(result.BPJSClassType);
                    $('#<%:txtChargeClassCodeCtlAdd.ClientID %>').val(result.ClassCode);
                    $('#<%:txtChargeClassNameCtlAdd.ClientID %>').val(result.ClassName);
                }
                else {
                    $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSCodeCtlAdd.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSTypeCtlAdd.ClientID %>').val('');
                    $('#<%:txtChargeClassCodeCtlAdd.ClientID %>').val('');
                    $('#<%:txtChargeClassNameCtlAdd.ClientID %>').val('');
                }
            }
            else {
                $('#<%:hdnClassIDCtlAdd.ClientID %>').val('');
                $('#<%:txtClassCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtClassNameCtlAdd.ClientID %>').val('');
                $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val('');
                $('#<%:hdnChargeClassBPJSCodeCtlAdd.ClientID %>').val('');
                $('#<%:hdnChargeClassBPJSTypeCtlAdd.ClientID %>').val('');
                $('#<%:txtChargeClassCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtChargeClassNameCtlAdd.ClientID %>').val('');
            }
            if ($('#<%:hdnHealthcareServiceUnitID.ClientID %>').val() != '') {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtServiceUnitNameCtlAdd.ClientID %>').val('');
                $('#<%:hdnIsServiceUnitHasParamedic.ClientID %>').val('0');
                $('#<%:hdnIsServiceUnitHasVisitType.ClientID %>').val('0');
            }
            $('#<%:hdnRoomIDCtlAdd.ClientID %>').val('');
            $('#<%:txtRoomCodeCtlAdd.ClientID %>').val('');
            $('#<%:txtRoomNameCtlAdd.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Visit Type
    function onGetVisitTypeFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '')
            serviceUnitID = '0';
        var paramedicID = $('#<%:hdnParamedicIDCtlAdd.ClientID %>').val();
        if (paramedicID == '')
            paramedicID = '0';
        var filterExpression = serviceUnitID + ';' + paramedicID + ';';
        return filterExpression;
    }

    $('#<%:lblVisitType.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('paramedicvisittype', onGetVisitTypeFilterExpression(), function (value) {
            $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val(value);
            onTxtVisitTypeCodeChanged(value);
        });
    });

    $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').live('change', function () {
        onTxtVisitTypeCodeChanged($(this).val());
    });

    function onTxtVisitTypeCodeChanged(value) {
        var filterExpression = onGetVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val(result.VisitTypeID);
                $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val(result.VisitTypeName);
            }
            else {
                $('#<%:hdnVisitTypeIDCtlAdd.ClientID %>').val('');
                $('#<%:txtVisitTypeCodeCtlAdd.ClientID %>').val('');
                $('#<%:txtVisitTypeNameCtlAdd.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Room

    function getRoomFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var classID = $('#<%:hdnClassIDCtlAdd.ClientID %>').val();
        var filterExpression = '';

        if (serviceUnitID != '') {
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
        }

        if (classID != '0' && classID != '') {
            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "ClassID = " + classID;
        }

        if (filterExpression != '') {
            filterExpression += " AND ";
        }
        filterExpression += "IsDeleted = 0 AND DepartmentID = 'INPATIENT'";

        return filterExpression;
    }

    $('#<%:lblRoom.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceunitroom', getRoomFilterExpression(), function (value) {
            $('#<%:txtRoomCodeCtlAdd.ClientID %>').val(value);
            onTxtRoomCodeChanged(value);
        });
    });

    $('#<%:txtRoomCodeCtlAdd.ClientID %>').live('change', function () {
        onTxtRoomCodeChanged($(this).val());
    });

    function onTxtRoomCodeChanged(value) {
        var filterExpression = getRoomFilterExpression() + " AND RoomCode = '" + value + "'";
        getRoom(filterExpression);
    }

    function getRoom(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getRoomFilterExpression();
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID != "") {
            Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                if (result.length == 1) {
                    $('#<%:hdnRoomIDCtlAdd.ClientID %>').val(result[0].RoomID);
                    $('#<%:txtRoomNameCtlAdd.ClientID %>').val(result[0].RoomName);
                    $('#<%:txtRoomCodeCtlAdd.ClientID %>').val(result[0].RoomCode);
                    if ($('#<%:hdnClassIDCtlAdd.ClientID %>').val() == '') {
                        $('#<%:hdnClassIDCtlAdd.ClientID %>').val(result[0].ClassID);
                        $('#<%:txtClassCodeCtlAdd.ClientID %>').val(result[0].ClassCode);
                        $('#<%:txtClassNameCtlAdd.ClientID %>').val(result[0].ClassName);
                        $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val(result[0].ChargeClassID);
                        $('#<%:hdnChargeClassBPJSCodeCtlAdd.ClientID %>').val(result[0].ChargeClassBPJSCode);
                        $('#<%:hdnChargeClassBPJSTypeCtlAdd.ClientID %>').val(result[0].ChargeClassBPJSType);
                        $('#<%:txtChargeClassCodeCtlAdd.ClientID %>').val(result[0].ChargeClassCode);
                        $('#<%:txtChargeClassNameCtlAdd.ClientID %>').val(result[0].ChargeClassName);
                    }
                }
                else {
                    $('#<%:hdnRoomIDCtlAdd.ClientID %>').val('');
                    $('#<%:txtRoomCodeCtlAdd.ClientID %>').val('');
                    $('#<%:txtRoomNameCtlAdd.ClientID %>').val('');
                }
                $('#<%:hdnBedIDCtlAdd.ClientID %>').val('');
                $('#<%:txtBedCodeCtlAdd.ClientID %>').val('');
            });
        } else {
            $('#<%:hdnRoomIDCtlAdd.ClientID %>').val('');
            $('#<%:txtRoomCodeCtlAdd.ClientID %>').val('');
            $('#<%:txtRoomNameCtlAdd.ClientID %>').val('');
            $('#<%:hdnBedIDCtlAdd.ClientID %>').val('');
            $('#<%:txtBedCodeCtlAdd.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Bed
    $('#<%:lblBed.ClientID %>.lblLink').live('click', function () {
        var roomID = $('#<%:hdnRoomIDCtlAdd.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND GCBedStatus = '0116^U'";
            openSearchDialog('bed', filterExpression, function (value) {
                $('#<%:txtBedCodeCtlAdd.ClientID %>').val(value);
                onTxtBedCodeChanged(value);
            });
        }
    });

    $('#<%:txtBedCodeCtlAdd.ClientID %>').live('change', function () {
        onTxtBedCodeChanged($(this).val());
    });

    function onTxtBedCodeChanged(value) {
        var roomID = $('#<%:hdnRoomIDCtlAdd.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND ";
            filterExpression += "BedCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvBedList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnBedIDCtlAdd.ClientID %>').val(result.BedID);
                    $('#<%:hdnExtensionNoCtlAdd.ClientID %>').val(result.ExtensionNo);
                }
                else {
                    $('#<%:hdnBedIDCtlAdd.ClientID %>').val('');
                    $('#<%:txtBedCodeCtlAdd.ClientID %>').val('');
                    $('#<%:hdnExtensionNoCtlAdd.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%:hdnBedIDCtlAdd.ClientID %>').val('');
            $('#<%:txtBedCodeCtlAdd.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    function getPayerContractFilterExpression() {
        var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerID.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
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
                                            var classID = $('#<%:hdnClassIDCtlAdd.ClientID %>').val();
                                            var chargeClassID = $('#<%:hdnChargeClassIDCtlAdd.ClientID %>').val();
                                            cboControlClassCare.SetValue(chargeClassID);
                                        }
                                    }
                                    else {
                                        $('#<%:trControlClassCare.ClientID %>').hide();
                                        cboControlClassCare.SetValue('');
                                    }
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
    //#endregion

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        getPayerCompany('');
        if ($('#<%:hdnContractID.ClientID %>').val() != '') {
            getCoverageType('');
        }
    }

    function setTblPayerCompanyVisibility() {
        var customerType = cboRegistrationPayer.GetValue();
        if (customerType == "<%:GetCustomerTypePersonal() %>") {
            $('#<%:tblPayerCompany.ClientID %>').hide();
            $('#<%:chkUsingCOB.ClientID %>').hide();
        }
        else {
            if (customerType == "<%:GetCustomerTypeHealthcare() %>") {
                $('#<%:trEmployee.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:trEmployee.ClientID %>').attr('style', 'display:none');
            }
            $('#<%:tblPayerCompany.ClientID %>').show();
            $('#<%:chkUsingCOB.ClientID %>').show();
            $('#<%:trCoverageLimitPerDay.ClientID %>').show();
        }
    }

    //#region Coverage Type
    function getCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%:hdnContractCoverageMemberCount.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
        var payerID = parseInt($('#<%:hdnPayerID.ClientID %>').val());

        var filterExpression = '';
        if (contractCoverageRowCount > 0)
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

    function onCboSpecialtyCtlAddValueChanged() {
    }

    function onBeforeSaveRecordEntryPopup() {
        return true;
    }
</script>
<style>
    .ScrollStyle
    {
        max-height: 150px;
        overflow-y: scroll;
    }
</style>
<div style="height: auto">
    <input type="hidden" id="hdnPayerIDCtlAdd" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnDOBCtlAdd" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnIsPatientNewBornMandatoryMotherRegistrationNo"
        value="" />
    <input type="hidden" id="hdnIsBridgingToBPJS" runat="server" />
    <input type="hidden" runat="server" id="hdnDateToday" value="" />
    <input type="hidden" runat="server" id="hdnPhysicianBPJSReferenceInfo" value="" />
    <input type="hidden" runat="server" id="hdnPilihDokterOtomatisIsiUnit" value="" />
    <input type="hidden" runat="server" id="hdnRegistrasiSelainRajalMemperhatikanCutiDokter"
        value="" />
    <input type="hidden" runat="server" id="hdnExtensionNoCtlAdd" value="" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnIsUsedReferenceQueueNo" value="0" runat="server" />
    <input type="hidden" id="hdnIsQueueNoUsingAppointment" runat="server" />
    <input type="hidden" id="hdnIsOutpatientUsingRoom" value="0" runat="server" />
    <input type="hidden" runat="server" id="hdnIsRegistrationBabyLinkRegistrationMother"
        value="" />
    <input type="hidden" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
    <input type="hidden" id="hdnIsControlAdministrationCharges" runat="server" />
    <input type="hidden" id="hdnIsControlAdmCost" runat="server" />
    <input type="hidden" runat="server" id="hdnAdminID" value="" />
    <input type="hidden" id="hdnIsControlPatientCardPayment" runat="server" />
    <input type="hidden" id="hdnItemCardFee" runat="server" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <input type="hidden" id="hdnIsBridgingToMobileJKN" value="0" runat="server" />
    <input type="hidden" id="hdnIsSendNotifToMobileJKN" value="0" runat="server" />
    <input type="hidden" runat="server" id="hdnIsBridgingToMedinfrasMobileApps" value="" />
    <input type="hidden" id="hdnIsBridgingToIPTV" value="0" runat="server" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Data Ibu" class="w3-hover-red">Data Ibu</li>
                        <li contentid="divPage2" title="Data Bayi" class="w3-hover-red">Data Bayi</li>
                        <li contentid="divPage3" title="Pendaftaran" class="w3-hover-red">Pendaftaran</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr id="trMotherRegNo" runat="server">
                            <td class="tdLabel">
                                <div style="position: relative;">
                                    <label class="lblLink lblKey lblMandatory" id="lblMotherRegNo" runat="server">
                                        <%:GetLabel("No. Registrasi Ibu")%></label></div>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnMotherVisitIDCtlAdd" value="" runat="server" />
                                <input type="hidden" id="hdnMotherMRNCtlAdd" value="" runat="server" />
                                <input type="hidden" id="hdnMotherNameCtlAdd" value="" runat="server" />
                                <asp:TextBox ID="txtMotherRegNoCtlAdd" Width="175px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent ScrollStyle" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td style="padding: 5px; vertical-align: top;">
                                <h4 class="h4expanded">
                                    <%=GetLabel("Data Pasien")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col style="width: 65%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Tipe Kartu Identitas")%></label>
                                            </td>
                                            <td>
                                                <table width="100%">
                                                    <colgroup>
                                                        <col width="50%" />
                                                        <col width="50%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="cboIdentityCardTypeCtlAdd" ClientInstanceName="cboIdentityCardTypeCtlAdd"
                                                                runat="server" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkIsSSNTemporaryCtlAdd" runat="server" /><%:GetLabel(" Nomor Kartu Identitas Sementara")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory lblNormal" id="lblSSN" runat="server">
                                                    <%=GetLabel("No Kartu Identitas")%></label>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col width="80%" />
                                                        <col width="20%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtIdentityCardNoCtlAdd" Width="100%" MaxLength="20" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Nama Belakang")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFamilyNameCtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Nama di Kartu") %>
                                                </label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNameCtlAdd" Width="100%" runat="server" MaxLength="28" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jenis Kelamin")%></label><br />
                                                / Gol. Darah
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 40%" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="cboGenderCtlAdd" ClientInstanceName="cboGenderCtlAdd" Width="100%"
                                                                runat="server">
                                                                <ClientSideEvents ValueChanged="function(s,e) { oncboSalutationCtlAddValueChanged(); }" />
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="cboBloodTypeCtlAdd" ClientInstanceName="cboBloodTypeCtlAdd"
                                                                Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="cboBloodRhesusCtlAdd" ClientInstanceName="cboBloodRhesusCtlAdd"
                                                                Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory lblLink" id="lblReligion">
                                                    <%=GetLabel("Agama")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtReligionCodeCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReligionNameCtlAdd" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblNationality">
                                                    <%=GetLabel("Kewarganegaraan")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNationalityCodeCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNationalityNameCtlAdd" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Tempat/Tanggal Lahir")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 50%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtBirthPlaceCtlAdd" Width="100%" MaxLength="50" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDOBCtlAdd" Width="108px" runat="server" CssClass="datepicker" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Umur")%>
                                                    (thn-bln-hari)</label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 32%" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 32%" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 32%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtAgeInYearCtlAdd" CssClass="number" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAgeInMonthCtlAdd" CssClass="number" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAgeInDayCtlAdd" CssClass="number" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table width="100%">
                                                    <colgroup>
                                                        <col width="50%" />
                                                        <col width="50%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chkIsAlive" Width="5%" runat="server" />
                                                            <%=GetLabel("Hidup")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4expanded">
                                    <%=GetLabel("Alamat KTP")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col style="width: 65%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jalan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddressCtlAdd" Width="100%" MaxLength="500" runat="server" TextMode="MultiLine"
                                                    Rows="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory lblLink" id="lblZipCode" runat="server">
                                                    <%=GetLabel("Kode Pos")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 40%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <input type="hidden" runat="server" id="hdnZipCodeCtlAdd" value="" />
                                                            <asp:TextBox ID="txtZipCodeCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal">
                                                                <%=GetLabel("RT/RW")%></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRTDataCtlAdd" Width="100%" MaxLength="3" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRWDataCtlAdd" Width="100%" MaxLength="3" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Desa / Kelurahan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountyCtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kecamatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDistrictCtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory lblLink" id="lblCity">
                                                    <%=GetLabel("Kota")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCityCtlAdd" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblProvince">
                                                    <%=GetLabel("Provinsi")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceCodeCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceNameCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4expanded">
                                    <%=GetLabel("Data Kontak Pasien")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col style="width: 65%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("No Telepon 1")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelephoneNo1CtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No Telepon 2")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelephoneNo2CtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("No HP 1")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobilePhone1CtlAdd" Width="100%" MaxLength="20" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No HP 2")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobilePhone2CtlAdd" Width="100%" MaxLength="20" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Email")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmailCtlAdd" CssClass="email" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4expanded">
                                    <%=GetLabel("Alamat Domisili")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col style="width: 65%" />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkCopyKTP" runat="server" /><%:GetLabel(" Salin Alamat KTP")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jalan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddressDomicileCtlAdd" Width="100%" MaxLength="500" runat="server"
                                                    TextMode="MultiLine" Rows="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblZipCodeDomicile" runat="server">
                                                    <input type="hidden" runat="server" id="hdnZipCodeDomicileCtlAdd" value="" />
                                                    <%=GetLabel("Kode Pos")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 40%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtZipCodeDomicileCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            &nbsp
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal">
                                                                <%=GetLabel("RT/RW")%></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRTDomicileDataCtlAdd" Width="100%" MaxLength="3" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRWDomicileDataCtlAdd" Width="100%" MaxLength="3" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Desa / Kelurahan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountyDomicileCtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kecamatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDistrictDomicileCtlAdd" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblCityDomicile">
                                                    <%=GetLabel("Kota")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCityDomicileCtlAdd" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblDomicileProvince">
                                                    <%=GetLabel("Provinsi")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceDomicileCodeCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceDomicileNameCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--                                <h4 class="h4expanded">
                                    <%=GetLabel("Data Pembayar")%></h4>--%>
                                <%--                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 35%" />
                                            <col style="width: 65%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Pembayar")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboPayerCtlAdd" ClientInstanceName="cboPayerCtlAdd" Width="100%"
                                                    runat="server">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerCtlAddValueChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr id="trPayerCompany" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblPayerCompany">
                                                    <%=GetLabel("Instansi")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 30%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtPayerCompanyCodeCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPayerCompanyNameCtlAdd" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left ScrollStyle"
                    style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <h4 class="h4expanded">
                            <%:GetLabel("Data Kunjungan")%></h4>
                        <tr id="trPhysician" runat="server">
                            <td class="tdLabel" style="width: 30%">
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%:GetLabel("Dokter/Tenaga Medis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnParamedicIDCtlAdd" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianNameCtlAdd" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trClass" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblClass">
                                    <%:GetLabel("Kelas Perawatan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnClassIDCtlAdd" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtClassCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClassNameCtlAdd" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trServiceUnit" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                    <%:GetServiceUnitLabel()%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnIsServiceUnitHasParamedic" value="" runat="server" />
                                <input type="hidden" id="hdnIsServiceUnitHasVisitType" value="" runat="server" />
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <input type="hidden" id="hdnIsPoliExecutive" value="0" runat="server" />
                                <input type="hidden" id="hdnBPJSPoli" value="" runat="server" />
                                <input type="hidden" id="hdnPoliRujukan" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitNameCtlAdd" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trRoom" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblRoom">
                                    <%:GetLabel("Kamar")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRoomIDCtlAdd" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRoomNameCtlAdd" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trBed" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblBed">
                                    <%:GetLabel("Tempat Tidur")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBedIDCtlAdd" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBedCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNewBorn" runat="server" Enabled="false" Checked="true" /><%:GetLabel("Bayi Baru Lahir")%>
                                            <asp:CheckBox ID="chkIsVisitorRestriction" runat="server" /><%:GetLabel("Tidak mau dikunjungi (Rahasia)")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trChargeClass" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblChargeClass">
                                    <%:GetLabel("Kelas Tagihan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnChargeClassBPJSCodeCtlAdd" value="" runat="server" />
                                <input type="hidden" id="hdnChargeClassBPJSTypeCtlAdd" value="" runat="server" />
                                <input type="hidden" id="hdnChargeClassIDCtlAdd" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtChargeClassCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChargeClassNameCtlAdd" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%:GetLabel("Spesialisasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSpecialtyCtlAdd" ClientInstanceName="cboSpecialtyCtlAdd"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){
                                    onCboSpecialtyCtlAddValueChanged();
                                }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                                    <%:GetLabel("Jenis Kunjungan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnVisitTypeIDCtlAdd" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeCodeCtlAdd" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeNameCtlAdd" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <h4 class="h4expanded">
                        <%:GetLabel("Data Pembayar")%></h4>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                            <col style="width: 100px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 3px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboRegistrationPayer" ClientInstanceName="cboRegistrationPayer"
                                                Width="100%" runat="server">
                                                <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <img id="imgCOB" runat="server" width="25" src='' alt='' title="COB" />
                                        </td>
                                        <td id="chkUsingCOB" runat="server" style="display: none">
                                            <div style="display: none">
                                                <asp:CheckBox ID="chkIsUsingCOB" Checked="false" runat="server" /><%:GetLabel("Peserta COB")%></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table class="tblEntryContent" runat="server" style="width: 100%;" id="tblPayerCompany">
                        <tr>
                            <td style="width: 30%" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPayerCompany">
                                    <%:GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPayerID" value="" runat="server" />
                                <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                                <input type="hidden" id="hdnIsBlacklistPayer" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
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
                        <tr>
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
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Masa Berlaku Kontrak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContractPeriod" Width="120px" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("No. Peserta")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParticipantNo" Width="100%" runat="server" />
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
                        <tr id="trGuaranteeLetterExists" runat="server">
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsGuaranteeLetterExists" runat="server" /><%:GetLabel("Memiliki Kontrol Surat Jaminan")%>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
