<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GuestEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GuestEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_guestentryctl">
    setDatePicker('<%=txtDOB.ClientID %>');
    $('#<%=txtDOB.ClientID %>').datepicker('option', 'maxDate', '0');
    //registerCollapseExpandHandler();

    function onBeforeSaveRecord(errMessage) {
        //if (IsValid(null, 'fsEntryPopup', 'mpEntryPopup')) {
        LoadGuest(
            cboSalutation.GetValue(),
            cboTitle.GetValue(),
            $('#<%=txtFirstName.ClientID %>').val(),
            $('#<%=txtMiddleName.ClientID %>').val(),
            $('#<%=txtFamilyName.ClientID %>').val(),
            cboSuffix.GetValue(),
            cboGender.GetValue(),
            $('#<%=txtDOB.ClientID %>').val(),
            $('#<%=txtAddress.ClientID %>').val(),
            $('#<%=txtCounty.ClientID %>').val(),
            $('#<%=txtDistrict.ClientID %>').val(),
            $('#<%=txtCity.ClientID %>').val(),
            $('#<%=txtAddressDomicile.ClientID %>').val(),
            $('#<%=txtCountyDomicile.ClientID %>').val(),
            $('#<%=txtDistrictDomicile.ClientID %>').val(),
            $('#<%=txtCityDomicile.ClientID %>').val(),
            $('#<%=txtTelephoneNo.ClientID %>').val(),
            $('#<%=txtMobilePhone.ClientID %>').val(),
            $('#<%=txtEmail.ClientID %>').val(),
            cboIdentityCardType.GetValue(),
            $('#<%=txtIdentityCardNo.ClientID %>').val(),
            cboSuffix.GetText(),
            cboTitle.GetText(),
            $('#<%=txtAgeInYear.ClientID %>').val(),
            $('#<%=txtAgeInMonth.ClientID %>').val(),
            $('#<%=txtAgeInDay.ClientID %>').val(),
            cboGender.GetText()
        );
        return true;
        //}
    }

    function onAfterSaveAddRecordEntryPopup(param) {
        return $('#<%=txtGuestNo.ClientID %>').val(param);
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=txtGuestNo.ClientID %>').val();
        return result;
    }

    function entityToControl(entity) {
        if (entity != null) {
            setEntryPopupIsAdd(false);

            //#region Patient Data
            cboSalutation.SetValue(entity.GCSalutation);
            cboTitle.SetValue(entity.GCTitle);
            $('#<%=txtGuestNo.ClientID %>').val(entity.GuestNo);
            $('#<%=txtFirstName.ClientID %>').val(entity.FirstName);
            $('#<%=txtMiddleName.ClientID %>').val(entity.MiddleName);
            $('#<%=txtFamilyName.ClientID %>').val(entity.LastName);

            cboSuffix.SetValue(entity.GCSuffix);
            cboGender.SetValue(entity.GCGender);

            $('#<%=txtDOB.ClientID %>').val(Methods.getJSONDateValue(entity.DateOfBirth));
            $('#<%=txtAgeInYear.ClientID %>').val(entity.AgeInYear);
            $('#<%=txtAgeInMonth.ClientID %>').val(entity.AgeInMonth);
            $('#<%=txtAgeInDay.ClientID %>').val(entity.AgeInDay);
            //#endregion

            //#region Patient Address
            $('#<%=txtAddress.ClientID %>').val(entity.StreetName);
            $('#<%=txtCounty.ClientID %>').val(entity.County);
            $('#<%=txtDistrict.ClientID %>').val(entity.District);
            $('#<%=txtCity.ClientID %>').val(entity.City);
            //#endregion

            //#region Patient Address Domicile
            $('#<%=txtAddressDomicile.ClientID %>').val(entity.StreetName);
            $('#<%=txtCountyDomicile.ClientID %>').val(entity.County);
            $('#<%=txtDistrictDomicile.ClientID %>').val(entity.District);
            $('#<%=txtCityDomicile.ClientID %>').val(entity.City);
            //#endregion

            //#region Patient Contact
            $('#<%=txtTelephoneNo.ClientID %>').val(entity.PhoneNo);
            $('#<%=txtMobilePhone.ClientID %>').val(entity.MobilePhoneNo);
            $('#<%=txtEmail.ClientID %>').val(entity.EmailAddress);
            cboIdentityCardType.SetValue(entity.GCIdentityNoType);
            $('#<%=txtIdentityCardNo.ClientID %>').val(entity.SSN);
            //#endregion
        }
        else {
            setEntryPopupIsAdd(true);

            //#region Patient Data
            cboSalutation.SetValue('');
            cboTitle.SetValue('');
            $('#<%=txtFirstName.ClientID %>').val('');
            $('#<%=txtMiddleName.ClientID %>').val('');
            $('#<%=txtFamilyName.ClientID %>').val('');
            cboSuffix.SetValue('');
            cboGender.SetValue('');

            $('#<%=txtDOB.ClientID %>').val('');
            $('#<%=txtAgeInYear.ClientID %>').val('');
            $('#<%=txtAgeInMonth.ClientID %>').val('');
            $('#<%=txtAgeInDay.ClientID %>').val('');
            //#endregion

            //#region Patient Address
            $('#<%=txtAddress.ClientID %>').val('');
            $('#<%=txtCounty.ClientID %>').val('');
            $('#<%=txtDistrict.ClientID %>').val('');
            $('#<%=txtCity.ClientID %>').val('');
            //#endregion

            //#region Patient Address Domicile
            $('#<%=txtAddressDomicile.ClientID %>').val('');
            $('#<%=txtCountyDomicile.ClientID %>').val('');
            $('#<%=txtDistrictDomicile.ClientID %>').val('');
            $('#<%=txtCityDomicile.ClientID %>').val('');
            //#endregion

            //#region Patient Contact
            $('#<%=txtTelephoneNo.ClientID %>').val('');
            $('#<%=txtMobilePhone.ClientID %>').val('');
            $('#<%=txtEmail.ClientID %>').val('');
            cboIdentityCardType.SetValue('');
            $('#<%=txtIdentityCardNo.ClientID %>').val('');
            //#endregion
        }
    }

    //#region DOB
    $('#<%=txtDOB.ClientID %>').change(function () {
        var age = Methods.getAgeFromDatePickerFormat($(this).val());
        $('#<%=txtAgeInYear.ClientID %>').val(age.years);
        $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
        $('#<%=txtAgeInDay.ClientID %>').val(age.days);
    });

    $('#<%=txtAgeInYear.ClientID %>').change(function () {
        getDOBFromAge();
    });

    $('#<%=txtAgeInMonth.ClientID %>').change(function () {
        getDOBFromAge();
    });

    $('#<%=txtAgeInDay.ClientID %>').change(function () {
        getDOBFromAge();
    });

    function getDOBFromAge() {
        var now = Methods.stringToDate('<%=GetTodayDate() %>');
        var ageInYear = parseInt($('#<%=txtAgeInYear.ClientID %>').val());
        var ageInMonth = parseInt($('#<%=txtAgeInMonth.ClientID %>').val());
        var ageInDay = parseInt($('#<%=txtAgeInDay.ClientID %>').val());

        now.setYear(now.getFullYear() - ageInYear);
        now.setMonth(now.getMonth() - ageInMonth);
        now.setDate(now.getDate() - ageInDay);

        var dateStr = Methods.dateToDatePickerFormat(now);
        $('#<%=txtDOB.ClientID %>').val(dateStr);
    }

    $('#<%=txtDOB.ClientID %>').change();
    //#endregion

    //#region Religion
    function onGetSCReligionFilterExpression() {
        var filterExpression = "<%:OnGetSCReligionFilterExpression() %>";
        return filterExpression;
    }

    $('#lblReligion.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCReligionFilterExpression(), function (value) {
            $('#<%=txtReligionCode.ClientID %>').val(value);
            onTxtReligionCodeChanged(value);
        });
    });

    $('#<%=txtReligionCode.ClientID %>').change(function () {
        onTxtReligionCodeChanged($(this).val());
    });

    function onTxtReligionCodeChanged(value) {
        var filterExpression = onGetSCReligionFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtReligionName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtReligionName.ClientID %>').val('');
                $('#<%=txtReligionCode.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=chkCopyKTP.ClientID %>').live('change', function () {
        if ($(this.checked)) {
            var address = $('#<%=txtAddress.ClientID %>').val();
            var city = $('#<%=txtCity.ClientID %>').val();
            var county = $('#<%=txtCounty.ClientID %>').val();
            var district = $('#<%=txtDistrict.ClientID %>').val();

            $('#<%=txtAddressDomicile.ClientID %>').val(address);
            $('#<%=txtCityDomicile.ClientID %>').val(city);
            $('#<%=txtCountyDomicile.ClientID %>').val(county);
            $('#<%=txtDistrictDomicile.ClientID %>').val(district);
        }
    });

    //#region Nationality
    function onGetSCNationalityFilterExpression() {
        var filterExpression = "<%:OnGetSCNationalityFilterExpression() %>";
        return filterExpression;
    }

    $('#lblNationality.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCNationalityFilterExpression(), function (value) {
            $('#<%=txtNationalityCode.ClientID %>').val(value);
            onTxtNationalityCodeChanged(value);
        });
    });

    $('#<%=txtNationalityCode.ClientID %>').change(function () {
        onTxtNationalityCodeChanged($(this).val());
    });

    function onTxtNationalityCodeChanged(value) {
        var filterExpression = onGetSCNationalityFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtNationalityName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtNationalityCode.ClientID %>').val('');
                $('#<%=txtNationalityName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Ethnic
    function onGetSCEthnicFilterExpression() {
        var filterExpression = "<%:OnGetSCEthnicFilterExpression() %>";
        return filterExpression;
    }

    $('#lblEthnic.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCEthnicFilterExpression(), function (value) {
            $('#<%=txtEthnicCode.ClientID %>').val(value);
            onTxtEthnicCodeChanged(value);
        });
    });

    $('#<%=txtEthnicCode.ClientID %>').change(function () {
        onTxtEthnicCodeChanged($(this).val());
    });

    function onTxtEthnicCodeChanged(value) {
        var filterExpression = onGetSCEthnicFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtEthnicName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtEthnicCode.ClientID %>').val('');
                $('#<%=txtEthnicName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Marital Status
    function onGetSCMaritalStatusFilterExpression() {
        var filterExpression = "<%:OnGetSCMaritalStatusFilterExpression() %>";
        return filterExpression;
    }

    $('#lblMaritalStatus.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCMaritalStatusFilterExpression(), function (value) {
            $('#<%=txtMaritalStatusCode.ClientID %>').val(value);
            onTxtMaritalStatusCodeChanged(value);
        });
    });

    $('#<%=txtMaritalStatusCode.ClientID %>').change(function () {
        onTxtMaritalStatusCodeChanged($(this).val());
    });

    function onTxtMaritalStatusCodeChanged(value) {
        var filterExpression = onGetSCMaritalStatusFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtMaritalStatusName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtMaritalStatusCode.ClientID %>').val('');
                $('#<%=txtMaritalStatusName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Language
    function onGetSCLanguageFilterExpression() {
        var filterExpression = "<%:OnGetSCLanguageFilterExpression() %>";
        return filterExpression;
    }

    $('#lblLanguage.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCLanguageFilterExpression(), function (value) {
            $('#<%=txtlanguageCode.ClientID %>').val(value);
            onTxtLanguageCodeChanged(value);
        });
    });

    $('#<%=txtlanguageCode.ClientID %>').change(function () {
        onTxtLanguageCodeChanged($(this).val());
    });

    function onTxtLanguageCodeChanged(value) {
        var filterExpression = onGetSCLanguageFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtlanguageName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtlanguageCode.ClientID %>').val('');
                $('#<%=txtlanguageName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Education
    function onGetSCEducationFilterExpression() {
        var filterExpression = "<%:OnGetSCEducationFilterExpression() %>";
        return filterExpression;
    }

    $('#<%=lblEducation.ClientID %>').live('click', function (evt) {
        openSearchDialog('stdcode', onGetSCEducationFilterExpression(), function (value) {
            $('#<%=txtEducationCode.ClientID %>').val(value);
            onTxtEducationCodeChanged(value);
        });
    });

    $('#<%=txtEducationCode.ClientID %>').change(function () {
        onTxtEducationCodeChanged($(this).val());
    });

    function onTxtEducationCodeChanged(value) {
        var filterExpression = onGetSCEducationFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtEducationName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtEducationCode.ClientID %>').val('');
                $('#<%=txtEducationName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Patient Job
    function onGetSCPatientJobFilterExpression() {
        var filterExpression = "<%:OnGetSCPatientJobFilterExpression() %>";
        return filterExpression;
    }

    $('#<%=lblPatientJob.ClientID %>').live('click', function (evt) {
        openSearchDialog('stdcode', onGetSCPatientJobFilterExpression(), function (value) {
            $('#<%=txtPatientJobCode.ClientID %>').val(value);
            onTxtPatientJobCodeChanged(value);
        });
    });

    $('#<%=txtPatientJobCode.ClientID %>').change(function () {
        onTxtPatientJobCodeChanged($(this).val());
    });

    function onTxtPatientJobCodeChanged(value) {
        var filterExpression = onGetSCPatientJobFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtPatientJobName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtPatientJobCode.ClientID %>').val('');
                $('#<%=txtPatientJobName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<input type="hidden" runat="server" id="hdnGuestID" />
<input type="hidden" runat="server" id="hdnRegistrationNo" />
<div style="height:445px;overflow-y:scroll;">
    <table class="tblContentArea">
        <colgroup>
            <col style="width:49%"/>
            <col style="width:3px"/>
            <col style="width:49%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <h4><%=GetLabel("Data Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblGuest">
                                    <%=GetLabel("No Pengunjung")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <colgroup>
                                        <col width="120px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGuestNo" Width="99%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sapaan")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboSalutation" ClientInstanceName="cboSalutation" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Gelar Depan")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboTitle" ClientInstanceName="cboTitle" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:49%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtFirstName" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtMiddleName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Belakang")%></label></td>
                            <td><asp:TextBox ID="txtFamilyName" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Gelar Belakang")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboSuffix" ClientInstanceName="cboSuffix" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jenis Kelamin")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboGender" ClientInstanceName="cboGender" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class=""><%=GetLabel("Gol. Darah")%></label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:32%"/>
                                        <col style="width:3px"/>
                                        <col style="width:32%"/>
                                        <col style="width:3px"/>
                                        <col style="width:32%"/>
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboBloodType" ClientInstanceName="cboBloodType" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboBloodRhesus" ClientInstanceName="cboBloodRhesus" Width="100%"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="">
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
                                            <asp:TextBox ID="txtBirthPlace" Width="100%" MaxLength="50" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDOB" Width="100px" runat="server" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Umur")%> (yyyy-MM-dd)</label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:32%"/>
                                        <col style="width:3px"/>
                                        <col style="width:32%"/>
                                        <col style="width:3px"/>
                                        <col style="width:32%"/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtAgeInYear" CssClass="number" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtAgeInMonth" CssClass="number" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtAgeInDay" CssClass="number" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4><%=GetLabel("Informasi Tambahan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblReligion">
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
                                            <asp:TextBox ID="txtReligionCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReligionName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNationality">
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
                                            <asp:TextBox ID="txtNationalityCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNationalityName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblEthnic" class="lblLink">
                                    <%=GetLabel("Suku")%></label>
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
                                            <asp:TextBox ID="txtEthnicCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEthnicName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMaritalStatus">
                                    <%=GetLabel("Status Pernikahan")%></label>
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
                                            <asp:TextBox ID="txtMaritalStatusCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMaritalStatusName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblLanguage">
                                    <%=GetLabel("Bahasa")%></label>
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
                                            <asp:TextBox ID="txtlanguageCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlanguageName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblEducation" runat="server">
                                    <%=GetLabel("Pendidikan")%></label>
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
                                            <asp:TextBox ID="txtEducationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEducationName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPatientJob" runat="server">
                                    <%=GetLabel("Pekerjaan")%></label>
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
                                            <asp:TextBox ID="txtPatientJobCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientJobName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class=""><%=GetLabel("Nomor Karyawan")%></label></td>
                            <td><asp:TextBox ID="txtCorporateAccountNo" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class=""><%=GetLabel("Nama Karyawan")%></label></td>
                            <td><asp:TextBox ID="txtCorporateAccountName" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class=""><%=GetLabel("Divisi Karyawan")%></label></td>
                            <td><asp:TextBox ID="txtCorporateAccountDepartment" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
            </td>
            <td>&nbsp;</td>
            <td style="padding:5px;vertical-align:top;">                
                <h4><%=GetLabel("Data Alamat")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Jalan")%></label></td>
                            <td><asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Desa / Kelurahan")%></label></td>
                            <td><asp:TextBox ID="txtCounty" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kecamatan")%></label></td>
                            <td><asp:TextBox ID="txtDistrict" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kota")%></label></td>
                            <td><asp:TextBox ID="txtCity" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>

                <h4><%=GetLabel("Data Alamat Domisili")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkCopyKTP" runat="server" /><%:GetLabel("Salin KTP")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Jalan")%></label></td>
                            <td><asp:TextBox ID="txtAddressDomicile" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Desa / Kelurahan")%></label></td>
                            <td><asp:TextBox ID="txtCountyDomicile" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kecamatan")%></label></td>
                            <td><asp:TextBox ID="txtDistrictDomicile" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kota")%></label></td>
                            <td><asp:TextBox ID="txtCityDomicile" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
                
                <h4><%=GetLabel("Data Kontak Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("No Telepon")%></label></td>
                            <td><asp:TextBox ID="txtTelephoneNo" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No HP")%></label></td>
                            <td><asp:TextBox ID="txtMobilePhone" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Email")%></label></td>
                            <td><asp:TextBox ID="txtEmail" CssClass="email" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tipe Kartu Identitas")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboIdentityCardType" ClientInstanceName="cboIdentityCardType" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Kartu Identitas")%></label></td>
                            <td><asp:TextBox ID="txtIdentityCardNo" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>