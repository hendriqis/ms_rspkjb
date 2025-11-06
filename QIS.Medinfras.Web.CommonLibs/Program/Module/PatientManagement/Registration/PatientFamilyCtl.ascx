<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientFamilyCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientFamilyCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PatientFamilyCtl">
    $('#lblPatientVisitAddData').die('click');
    $('#lblPatientVisitAddData').live('click', function () {
        $('#<%=chkIsPatient.ClientID %>').removeAttr('disabled');
        $('#<%=chkAutomaticallyLinkFamily.ClientID %>').removeAttr('disabled');
        $('#<%=txtFamilyMRN.ClientID %>').removeAttr('readonly');
        $('#lblFamilyMRN').attr('class', 'lblLink');

        $('#trFamilyRelationLink').attr('style', 'diplay:none');

        $('#<%=chkIsPatient.ClientID %>').prop('checked', false);
        $('#<%=chkAutomaticallyLinkFamily.ClientID %>').prop('checked', false);
        $('#<%=hdnFamilyID.ClientID %>').val('');
        $('#<%=hdnFamilyMRN.ClientID %>').val('');
        $('#<%=txtFamilyMRN.ClientID %>').val('');

        cboFamilyRelation.SetValue('');
        cboFamilyRelationLink.SetValue('');
        cboSalutation.SetValue('');
        cboTitle.SetValue('');
        $('#<%=txtFirstName.ClientID %>').val('');
        $('#<%=txtMiddleName.ClientID %>').val('');
        $('#<%=txtLastName.ClientID %>').val('');
        cboSuffix.SetValue('');
        cboTitle.SetValue('');

        cboGender.SetValue('');
        cboOccupation.SetValue('');

        setDatePicker('<%:txtDOB.ClientID %>');
        $('#<%:txtDOB.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        $('#<%=txtDOB.ClientID %>').val('');
        $('#<%=txtBirthPlace.ClientID %>').val('');

        $('#<%=hdnAddressID.ClientID %>').val('');
        $('#<%=txtAddress.ClientID %>').val('');
        $('#<%=txtCounty.ClientID %>').val('');
        $('#<%=txtDistrict.ClientID %>').val('');
        $('#<%=txtCity.ClientID %>').val('');
        $('#<%=txtProvinceCode.ClientID %>').val($('#<%=hdnGCState.ClientID %>').val());
        $('#<%=txtProvinceName.ClientID %>').val($('#<%=hdnState.ClientID %>').val());
        $('#<%=hdnZipCode.ClientID %>').val('');
        $('#<%=txtZipCode.ClientID %>').val('');
        $('#<%=txtTelephoneNo.ClientID %>').val($('#<%=hdnPhoneArea.ClientID %>').val());
        $('#<%=txtEmail.ClientID %>').val('');

        $('#<%=chkIsPatient.ClientID %>').change();
        $('#<%=chkAutomaticallyLinkFamily.ClientID %>').change();

        $('#containerPatientFamilyEntryData').show();
    });

    $('#btnPatientFamilyCancel').die('click');
    $('#btnPatientFamilyCancel').live('click', function () {
        $('#containerPatientFamilyEntryData').hide();
    });

    $('#btnPatientFamilySave').die('click');
    $('#btnPatientFamilySave').live('click', function (evt) {
        if (IsValid(evt, 'fsPatientFamily', 'mpPatientFamily')) {
            if ($('#<%=chkIsPatient.ClientID %>').prop('checked', false)) {
                if (IsValid(evt, 'fsRelationFamilyLink', 'mpRelationFamilyLink')) {
                    //                    $('#<%=chkIsPatient.ClientID %>').prop('checked', true);
                    //                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').prop('checked', true);
                    cbpPatientFamily.PerformCallback('save');
                }
                else {
                    $('#<%=chkIsPatient.ClientID %>').prop('checked', true);
                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').prop('checked', true);
                }
            }
            else {
                cbpPatientFamily.PerformCallback('save');
            }
        }
        return false;
    });

    $('.imgPatientVisitDelete.imgLink').die('click');
    $('.imgPatientVisitDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).parent().parent();
            var id = $row.find('.hdnFamilyID').val();
            $('#<%=hdnFamilyID.ClientID %>').val(id);
            cbpPatientFamily.PerformCallback('delete');
        }
    });

    $('.imgPatientVisitEdit.imgLink').die('click')
    $('.imgPatientVisitEdit.imgLink').live('click', function () {
        $row = $(this).parent().parent();

        var familyID = $row.find('.hdnFamilyID').val();
        var familyMedicalNo = $row.find('.hdnFamilyMedicalNo').val();
        var familyMRN = $row.find('.hdnFamilyMRN').val();
        var GCFamilyRelation = $row.find('.hdnGCFamilyRelation').val();
        var GCSalutation = $row.find('.hdnGCSalutation').val();
        var GCTitle = $row.find('.hdnGCTitle').val();
        var firstName = $row.find('.hdnFirstName').val();
        var middleName = $row.find('.hdnMiddleName').val();
        var lastName = $row.find('.hdnLastName').val();
        var GCSuffix = $row.find('.hdnGCSuffix').val();
        var dob = $row.find('.hdnDateOfBirth').val();
        var cityOfBirth = $row.find('.hdnCityOfBirth').val();
        var gcgender = $row.find('.hdnGCGender').val();
        var gcoccupation = $row.find('.hdnGCOccupation').val();

        var phoneNo1 = $row.find('.hdnPhoneNo1').val();
        var email = $row.find('.hdnEmail').val();
        var streetName = $row.find('.hdnStreetName').val();
        var county = $row.find('.hdnCounty').val();
        var district = $row.find('.hdnDistrict').val();
        var city = $row.find('.hdnCity').val();
        var GCState = $row.find('.hdnGCState').val();
        var state = $row.find('.hdnState').val();
        var zipCodeID = $row.find('.hdnZipCodeID').val();
        var zipCode = $row.find('.hdnZipCode').val();

        $('#<%=hdnFamilyID.ClientID %>').val(familyID);
        $('#<%=hdnFamilyMRN.ClientID %>').val(familyMRN);
        $('#<%=txtFamilyMRN.ClientID %>').val(familyMedicalNo);
        cboFamilyRelation.SetValue(GCFamilyRelation);
        cboSalutation.SetValue(GCSalutation);
        cboTitle.SetValue(GCTitle);
        $('#<%=txtFirstName.ClientID %>').val(firstName);
        $('#<%=txtMiddleName.ClientID %>').val(middleName);
        $('#<%=txtLastName.ClientID %>').val(lastName);
        cboSuffix.SetValue(GCSuffix);

        $('#<%=txtDOB.ClientID %>').val(dob);
        setDatePicker('<%:txtDOB.ClientID %>');
        $('#<%:txtDOB.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        $('#<%=txtBirthPlace.ClientID %>').val(cityOfBirth);

        cboGender.SetValue(gcgender);
        cboOccupation.SetValue(gcoccupation);

        $('#<%=txtAddress.ClientID %>').val(streetName);
        $('#<%=txtCounty.ClientID %>').val(county);
        $('#<%=txtDistrict.ClientID %>').val(district);
        var displayGCState = '';
        if (GCState != '')
            displayGCState = GCState.split('^')[1];
        $('#<%=txtProvinceCode.ClientID %>').val(displayGCState);
        $('#<%=txtProvinceName.ClientID %>').val(state);
        $('#<%=hdnZipCode.ClientID %>').val(zipCodeID);
        $('#<%=txtZipCode.ClientID %>').val(zipCode);
        $('#<%=txtTelephoneNo.ClientID %>').val(phoneNo1);
        $('#<%=txtEmail.ClientID %>').val(email);

        $('#<%=chkIsPatient.ClientID %>').prop('checked', (familyMRN != '0' && familyMRN != ''));
        $('#<%=chkIsPatient.ClientID %>').change();
        $('#<%=chkIsPatient.ClientID %>').attr('disabled', 'disabled');
        $('#<%=txtFamilyMRN.ClientID %>').attr('readonly', 'readonly');
        $('#lblFamilyMRN').attr('class', 'lblDisabled');
        $('#trFamilyRelationLink').attr('style', 'display:none');

        if ((familyMRN != '0' && familyMRN != '')) {
            var filterFamily = "FamilyMRN = " + familyMRN + " AND MRN = " + $('#<%=hdnMRN.ClientID %>').val() + " AND GCFamilyRelation = '" + GCFamilyRelation + "' AND IsDeleted = 0";
            Methods.getObject('GetPatientFamilyList', filterFamily, function (result) {
                if (result != null) {
                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').prop('checked', true);
                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').change();
                    cboFamilyRelationLink.SetValue(result.GCFamilyRelation);
                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').attr('disabled', 'disabled');
                }
                else {
                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').prop('checked', false);
                    $('#<%=chkAutomaticallyLinkFamily.ClientID %>').change();
                    cboFamilyRelationLink.SetValue('');
                }
            });
        }
        $('#containerPatientFamilyEntryData').show();
    });

    $(function () {
        $('#<%=chkIsPatient.ClientID %>').live('change', function () {
            var isPatient = $(this).is(":checked");
            if (isPatient) {
                $('#lblFamilyMRN').attr('class', 'lblLink');
                $('#trAutomaticallyLinkFamily').removeAttr('style');
                $('#<%=txtFamilyMRN.ClientID %>').removeAttr('readonly');
                cboSalutation.SetEnabled(false);
                cboTitle.SetEnabled(false);
                $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtLastName.ClientID %>').attr('readonly', 'readonly');
                cboSuffix.SetEnabled(false);
                cboGender.SetEnabled(false);
                cboOccupation.SetEnabled(false);
                $('#<%=txtDOB.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtBirthPlace.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#lblFamilyMRN').attr('class', 'lblDisabled');
                $('#<%=txtFamilyMRN.ClientID %>').attr('readonly', 'readonly');
                $('#trAutomaticallyLinkFamily').attr('style', 'display:none');
                cboSalutation.SetEnabled(true);
                cboTitle.SetEnabled(true);
                $('#<%=txtFirstName.ClientID %>').removeAttr('readonly');
                $('#<%=txtMiddleName.ClientID %>').removeAttr('readonly');
                $('#<%=txtLastName.ClientID %>').removeAttr('readonly');
                cboSuffix.SetEnabled(true);
                cboGender.SetEnabled(true);
                cboOccupation.SetEnabled(true);
                $('#<%=txtDOB.ClientID %>').removeAttr('readonly');
                $('#<%=txtBirthPlace.ClientID %>').removeAttr('readonly');
            }
            $('#trFamilyRelationLink').attr('style', 'display:none');
        });

        $('#<%=chkAutomaticallyLinkFamily.ClientID %>').live('change', function () {
            var isAutomaticallyLink = $(this).is(":checked");
            if (isAutomaticallyLink) {
                $('#trFamilyRelationLink').removeAttr('style');
            }
            else {
                $('#trFamilyRelationLink').attr('style', 'display:none');
            }
        });

        //#region No RM

        $('#lblFamilyMRN.lblLink').die('click');
        $('#lblFamilyMRN.lblLink').live('click', function () {
            var filterExpression = "MRN != " + $('#<%=hdnMRN.ClientID %>').val();
            openSearchDialog('patient', filterExpression, function (value) {
                $('#<%=txtFamilyMRN.ClientID %>').val(value);
                onPatientEntryTxtMRNChanged(value);
            });
        });

        $('#<%=txtFamilyMRN.ClientID %>').die('change');
        $('#<%=txtFamilyMRN.ClientID %>').live('change', function () {
            onPatientEntryTxtMRNChanged($(this).val());
        });

        function onPatientEntryTxtMRNChanged(value) {
            var filterExpression = "MRN != " + $('#<%=hdnMRN.ClientID %>').val() + " AND MedicalNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (entity) {
                if (entity != null) {
                    $('#<%=hdnFamilyMRN.ClientID %>').val(entity.MRN);
                    $('#<%=txtFamilyMRN.ClientID %>').val(entity.MedicalNo);

                    //#region Patient Data
                    cboSalutation.SetValue(entity.GCSalutation);
                    cboTitle.SetValue(entity.GCTitle);
                    $('#<%=txtFirstName.ClientID %>').val(entity.FirstName);
                    $('#<%=txtMiddleName.ClientID %>').val(entity.MiddleName);
                    $('#<%=txtLastName.ClientID %>').val(entity.LastName);
                    cboSuffix.SetValue(entity.GCSuffix);
                    cboGender.SetValue(entity.GCGender);
                    cboOccupation.SetValue(entity.GCOccupation);
                    $('#<%=txtDOB.ClientID %>').val(entity.cfDateOfBirthInString1);
                    $('#<%=txtBirthPlace.ClientID %>').val(entity.CityOfBirth);
                    //#endregion

                    //#region Patient Address
                    $('#<%=hdnAddressID.ClientID %>').val(entity.HomeAddressID);
                    $('#<%=txtAddress.ClientID %>').val(entity.StreetName);
                    $('#<%=txtCounty.ClientID %>').val(entity.County);
                    $('#<%=txtDistrict.ClientID %>').val(entity.District);
                    $('#<%=txtCity.ClientID %>').val(entity.City);
                    if (entity.GCState != "0347^0347") {
                        $('#<%=txtProvinceCode.ClientID %>').val(entity.GCState.split('^')[1]);
                    }
                    else {
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                    }
                    $('#<%=txtProvinceName.ClientID %>').val(entity.State);
                    $('#<%=hdnZipCode.ClientID %>').val(entity.ZipCodeID);
                    $('#<%=txtZipCode.ClientID %>').val(entity.ZipCode);
                    $('#<%=txtTelephoneNo.ClientID %>').val(entity.PhoneNo1);
                    $('#<%=txtEmail.ClientID %>').val(entity.Email);
                    //#endregion
                }
                else {
                    setEntryPopupIsAdd(true);
                    $('#<%=hdnFamilyMRN.ClientID %>').val('');
                    $('#<%=txtFamilyMRN.ClientID %>').val('');

                    //#region Patient Data
                    cboSalutation.SetValue('');
                    cboTitle.SetValue('');
                    $('#<%=txtFirstName.ClientID %>').val('');
                    $('#<%=txtMiddleName.ClientID %>').val('');
                    $('#<%=txtLastName.ClientID %>').val('');
                    cboSuffix.SetValue('');
                    //#endregion

                    //#region Patient Address
                    $('#<%=hdnAddressID.ClientID %>').val('');
                    $('#<%=txtAddress.ClientID %>').val('');
                    $('#<%=txtCounty.ClientID %>').val('');
                    $('#<%=txtDistrict.ClientID %>').val('');
                    $('#<%=txtCity.ClientID %>').val('');
                    $('#<%=txtProvinceCode.ClientID %>').val('');
                    $('#<%=txtProvinceName.ClientID %>').val('');
                    $('#<%=hdnZipCode.ClientID %>').val('');
                    $('#<%=txtZipCode.ClientID %>').val('');
                    $('#<%=txtTelephoneNo.ClientID %>').val('');
                    $('#<%=txtEmail.ClientID %>').val('');
                    //#endregion
                }
            });
        }
        //#endregion

        //#region Province
        function onGetSCProvinceFilterExpression() {
            var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
            return filterExpression;
        }

        $('#lblProvince.lblLink').die('click');
        $('#lblProvince.lblLink').live('click', function () {
            openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
                $('#<%=txtProvinceCode.ClientID %>').val(value);
                onTxtProvinceCodeChanged(value);
            });
        });

        $('#<%=txtProvinceCode.ClientID %>').die('change');
        $('#<%=txtProvinceCode.ClientID %>').live('change', function () {
            onTxtProvinceCodeChanged($(this).val());
        });

        function onTxtProvinceCodeChanged(value) {
            var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtProvinceName.ClientID %>').val(result.StandardCodeName);
                else
                    $('#<%=txtProvinceName.ClientID %>').val('');
            });
        }
        //#endregion

        //#region Zip Code
        $('#lblZipCode.lblLink').die('click');
        $('#lblZipCode.lblLink').live('click', function () {
            openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                onTxtZipCodeChanged(value);
            });
        });

        $('#<%=txtZipCode.ClientID %>').die('change');
        $('#<%=txtZipCode.ClientID %>').live('change', function () {
            onTxtZipCodeChangedValue($(this).val());
        });

        function onTxtZipCodeChanged(value) {
            if (value != '') {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtCounty.ClientID %>').val(result.County);
                        $('#<%=txtDistrict.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                        $('#<%=txtCity.ClientID %>').val('');
                        $('#<%=txtCounty.ClientID %>').val('');
                        $('#<%=txtDistrict.ClientID %>').val('');
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCode.ClientID %>').val('');
                $('#<%=txtZipCode.ClientID %>').val('');
            }
        }

        function onTxtZipCodeChangedValue(value) {
            if (value != '') {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtCounty.ClientID %>').val(result.County);
                        $('#<%=txtDistrict.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                        $('#<%=txtCity.ClientID %>').val('');
                        $('#<%=txtCounty.ClientID %>').val('');
                        $('#<%=txtDistrict.ClientID %>').val('');
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCode.ClientID %>').val('');
                $('#<%=txtZipCode.ClientID %>').val('');
            }
        }
        //#endregion

    });

    function onCbpPatientFamilyEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPatientFamilyEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnMRN" />
    <input type="hidden" value="" runat="server" id="hdnGCState" />
    <input type="hidden" value="" runat="server" id="hdnState" />
    <input type="hidden" value="" runat="server" id="hdnPhoneArea" />
    <input type="hidden" value="0" runat="server" id="hdnIsBridgingToMedinfrasMobileApps" />
    <div class="pageTitle">
        <%=GetLabel("Keluarga Pasien")%></div>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="160px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPatientFamilyEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnFamilyID" runat="server" value="" />
                    <fieldset id="fsPatientFamily" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 50%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <h4>
                                        <%=GetLabel("Data Keluarga")%></h4>
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblFamilyMRN">
                                                    <%=GetLabel("No.RM Keluarga")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" runat="server" id="hdnFamilyMRN" value="" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col />
                                                        <col style="width: 3px" />
                                                        <col style="width: 5px" />
                                                        <col style="width: 60px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFamilyMRN" CssClass="required NoRM" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsPatient" runat="server" />
                                                        </td>
                                                        <td>
                                                            <%=GetLabel("Is Patient") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Relasi")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox id="cboFamilyRelation" clientinstancename="cboFamilyRelation" width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Sapaan")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox id="cboSalutation" clientinstancename="cboSalutation" width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Gelar Depan")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox id="cboTitle" clientinstancename="cboTitle" width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Nama")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 49%" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFirstName" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMiddleName" Width="100%" runat="server" />
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
                                                <asp:TextBox ID="txtLastName" Width="100%" CssClass="required" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Gelar Belakang")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox id="cboSuffix" clientinstancename="cboSuffix" width="100%" runat="server" />
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
                                                        <col style="width: 48%" />
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
                                                            <asp:TextBox ID="txtDOB" Width="98px" runat="server" CssClass="datepicker" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Jenis Kelamin")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox id="cboGender" clientinstancename="cboGender" width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="lblPatientJob" runat="server">
                                                    <%=GetLabel("Pekerjaan")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox id="cboOccupation" clientinstancename="cboOccupation" width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trAutomaticallyLinkFamily" style="display: none">
                                            <td class="tdLabel">
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col />
                                                        <col style="width: 100px" />
                                                        <col style="width: 1px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td style="font-size: smaller; color: Maroon; font-style: italic; font-weight: normal;
                                                            text-align: left" class="blink-alert" colspan="2">
                                                            <label>
                                                                <%=GetLabel("Otomatis terbentuk data keluarga relasi ke pasien?")%></label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkAutomaticallyLinkFamily" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trFamilyRelationLink">
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Relasi ke Pasien")%></label>
                                            </td>
                                            <td>
                                                <fieldset id="fsFamilyRelationLink" style="margin: 0">
                                                    <dxe:aspxcombobox id="cboFamilyRelationLink" clientinstancename="cboFamilyRelationLink"
                                                        width="100%" runat="server" />
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <h4>
                                        <%=GetLabel("Alamat Keluarga")%></h4>
                                    <input type="hidden" id="hdnAddressID" runat="server" />
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
                                                <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblZipCode">
                                                    <%=GetLabel("Kode Pos")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" runat="server" id="hdnZipCode" value="" />
                                                <asp:TextBox ID="txtZipCode" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Desa / Kelurahan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCounty" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kecamatan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDistrict" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Kota")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCity" Width="100%" runat="server" />
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
                                                            <asp:TextBox ID="txtProvinceCode" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtProvinceName" ReadOnly="true" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Telepon")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTelephoneNo" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Email")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" CssClass="email" Width="100%" runat="server" />
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
                                                <input type="button" id="btnPatientFamilySave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnPatientFamilyCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:aspxcallbackpanel id="cbpPatientFamily" runat="server" width="100%" clientinstancename="cbpPatientFamily"
                    showloadingpanel="false" oncallback="cbpPatientFamily_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientFamilyEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdFamilyName" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <img class="imgLink imgPatientVisitEdit" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-right: 2px;" />
                                                <img class="imgLink imgPatientVisitDelete" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Nama")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("FullName")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Relasi")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("FamilyRelation")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="300px">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Alamat")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <input type="hidden" class="hdnFamilyID" value="<%#: Eval("FamilyID")%>" />
                                                    <input type="hidden" class="hdnFamilyMedicalNo" value="<%#: Eval("FamilyMedicalNo")%>" />
                                                    <input type="hidden" class="hdnFamilyMRN" value="<%#: Eval("FamilyMRN")%>" />
                                                    <input type="hidden" class="hdnGCFamilyRelation" value="<%#: Eval("GCFamilyRelation")%>" />
                                                    <input type="hidden" class="hdnGCSalutation" value="<%#: Eval("GCSalutation")%>" />
                                                    <input type="hidden" class="hdnGCTitle" value="<%#: Eval("GCTitle")%>" />
                                                    <input type="hidden" class="hdnFirstName" value="<%#: Eval("FirstName")%>" />
                                                    <input type="hidden" class="hdnMiddleName" value="<%#: Eval("MiddleName")%>" />
                                                    <input type="hidden" class="hdnLastName" value="<%#: Eval("LastName")%>" />
                                                    <input type="hidden" class="hdnGCSuffix" value="<%#: Eval("GCSuffix")%>" />
                                                    <input type="hidden" class="hdnPhoneNo1" value="<%#: Eval("PhoneNo1")%>" />
                                                    <input type="hidden" class="hdnEmail" value="<%#: Eval("Email")%>" />
                                                    <input type="hidden" class="hdnDateOfBirth" value="<%#: Eval("cfDOBInDatePickerFormat")%>" />
                                                    <input type="hidden" class="hdnCityOfBirth" value="<%#: Eval("CityOfBirth")%>" />
                                                    <input type="hidden" class="hdnGCGender" value="<%#: Eval("GCGender")%>" />
                                                    <input type="hidden" class="hdnGender" value="<%#: Eval("Gender")%>" />
                                                    <input type="hidden" class="hdnGCOccupation" value="<%#: Eval("GCOccupation")%>" />
                                                    <input type="hidden" class="hdnOccupation" value="<%#: Eval("Occupation")%>" />
                                                    <input type="hidden" class="hdnStreetName" value="<%#: Eval("StreetName")%>" />
                                                    <input type="hidden" class="hdnCounty" value="<%#: Eval("County")%>" />
                                                    <input type="hidden" class="hdnDistrict" value="<%#: Eval("District")%>" />
                                                    <input type="hidden" class="hdnCity" value="<%#: Eval("City")%>" />
                                                    <input type="hidden" class="hdnGCState" value="<%#: Eval("GCState")%>" />
                                                    <input type="hidden" class="hdnState" value="<%#: Eval("State")%>" />
                                                    <input type="hidden" class="hdnZipCodeID" value="<%#: Eval("ZipCodeID")%>" />
                                                    <input type="hidden" class="hdnZipCode" value="<%#: Eval("ZipCode")%>" />
                                                    <div class="divVisitTypeName">
                                                        <%#: Eval("HomeAddress")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:aspxcallbackpanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblPatientVisitAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
