<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx3.master"
    AutoEventWireup="true" CodeBehind="PatientBirth.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientBirth" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFatherDOB.ClientID %>');
            $('#<%=txtFatherDOB.ClientID %>').datepicker('option', 'maxDate', '0');
            setRightPanelButtonEnabled();
        });

        function setRightPanelButtonEnabled() {
            var id = $('#<%=txtNoRegBayi.ClientID %>').val();
            if (id == '') {
                $('#btnRegistrationBirthRecord').removeAttr('enabled');
            }
            else {
                $('#btnRegistrationBirthRecord').attr('enabled', 'false');
            }
        }

        $('#lblNoRegBayi.lblLink').die('click');
        $('#lblNoRegBayi.lblLink').live('click', function () {
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('patientBirthRecord', filterExpression, function (value) {
                $('#<%=txtBabyMRN.ClientID %>').val(value);
                onTxtBabyMRNhanged(value);
            });
        });

        $('#<%=txtBabyMRN.ClientID %>').change(function () {
            onTxtBabyMRNhanged($(this).val());
        });

        function onTxtBabyMRNhanged(value) {
            var filterExpression = "IsDeleted = 0 AND MedicalNo = '" + value + "'";
            Methods.getObject('GetvPatientBirthRecordList', filterExpression, function (result) {
                if (result != null) {
                    //#region Baby
                    $('#<%=hdnBabyMRN.ClientID %>').val(result.MRN);
                    $('#<%=hdnVisitID.ClientID %>').val(result.VisitID);
                    $('#<%=hdnVisitIDBayi.ClientID %>').val(result.VisitID);
                    $('#<%=txtNoRegBayi.ClientID %>').val(result.RegistrationNo);
                    $('#<%=txtNoSKL.ClientID %>').val(result.ReferenceNo);
                    cboSalutationBaby.SetValue(result.GCSalutation);
                    $('#<%=txtFirstNameBaby.ClientID %>').val(result.FirstName);
                    $('#<%=txtMiddleNameBaby.ClientID %>').val(result.MiddleName);
                    $('#<%=txtLastNameBaby.ClientID %>').val(result.LastName);
                    $('#<%=txtPreferredNameBaby.ClientID %>').val(result.PreferredName);
                    $('#<%=txtGenderBaby.ClientID %>').val(result.Gender);
                    $('#<%=txtDOBBayi.ClientID %>').val(result.DateOfBirthInString);
                    $('#<%=txtBabyPhoto.ClientID %>').val(result.PictureFileName);
                    var ItemImagePath = $('#<%=hdnURLPictureDirectory.ClientID %>').val() + result.MedicalNo + "/" + result.MedicalNo + ".png";
                    $('#<%=imgPreview.ClientID %>').attr('src', ItemImagePath);
                    $('#<%=txtAddressBaby.ClientID %>').val(result.StreetName);
                    var filterExpressionCodeBaby = "AddressID = '" + result.BabyAddressID + "'";
                    Methods.getObject('GetAddressList', filterExpressionCodeBaby, function (result4) {
                        if (result4.ZipCode != null) {
                            var filterExpressionZipCodeBaby = "ID = '" + result4.ZipCode + "'";
                            Methods.getObject('GetZipCodesList', filterExpressionZipCodeBaby, function (result5) {
                                if (result5 != null) {
                                    $('#<%=txtZipCodeBaby.ClientID %>').val(result5.ZipCode);
                                    $('#<%=hdnZipCodeBaby.ClientID %>').val(result5.ID);
                                }
                                else {
                                    $('#<%=txtZipCodeBaby.ClientID %>').val('');
                                    $('#<%=hdnZipCodeBaby.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            $('#<%=txtZipCodeBaby.ClientID %>').val('');
                            $('#<%=hdnZipCodeBaby.ClientID %>').val('');
                        }
                    });
                    $('#<%=hdnAddressIDBaby.ClientID %>').val(result.BabyAddressID);
                    $('#<%=txtCountyBaby.ClientID %>').val(result.County);
                    $('#<%=txtDistrictBaby.ClientID %>').val(result.District);
                    $('#<%=txtCityBaby.ClientID %>').val(result.City);
                    $('#<%=txtProvinceCodeBaby.ClientID %>').val(result.GCState.split('^')[1]);
                    $('#<%=txtProvinceNameBaby.ClientID %>').val(result.State);
                    $('#<%=txtTelephoneNoBaby.ClientID %>').val(result.PhoneNo1);

                    $('#<%=txtBirthNo.ClientID %>').val(result.BirthNo);
                    $('#<%=txtChildNo.ClientID %>').val(result.ChildNo);
                    $('#<%=txtTimeOfBirth.ClientID %>').val(result.TimeOfBirth);
                    cboBornAt.SetValue(result.GCBornAt);
                    $('#<%=txtBirthPregnancyAge.ClientID %>').val(result.BirthPregnancyAge);
                    $('#<%=txtAPGARScore1.ClientID %>').val(result.APGARScore1);
                    $('#<%=txtAPGARScore2.ClientID %>').val(result.APGARScore2);
                    $('#<%=txtAPGARScore3.ClientID %>').val(result.APGARScore3);
                    $('#<%=txtLength.ClientID %>').val(result.Length);
                    $('#<%=txtWeight.ClientID %>').val(result.Weight);
                    $('#<%=txtWeightGram.ClientID %>').val(result.WeightGram);
                    $('#<%=txtHeadCircumference.ClientID %>').val(result.HeadCircumference);
                    $('#<%=txtChestCircumference.ClientID %>').val(result.ChestCircumference);
                    cboCaesarMethod.SetValue(result.GCCaesarMethod);
                    cboTwinSingle.SetValue(result.GCTwinSingle);
                    cboBornCondition.SetValue(result.GCBornCondition);
                    cboPartumDeathType.SetValue(result.GCPartumDeathType);
                    cboNeonatalPerinatalDeathType.SetValue(result.GCNeonatalPerinatalDeathType);
                    cboBirthMethod.SetValue(result.GCBirthMethod);
                    cboBirthComplication.SetValue(result.GCBirthComplicationType);
                    cboBirthCOD.SetValue(result.GCBirthCOD);

                    if (result.GCBornCondition == Constant.BornCondition.Meninggal) {
                        $('#trPartumDeathType').removeAttr('style');
                        $('#trNeonatalPerinatalDeathType').removeAttr('style');
                    }
                    else {
                        $('#trPartumDeathType').attr('style', 'display:none');
                        $('#trNeonatalPerinatalDeathType').attr('style', 'display:none');
                    }
                    //#endregion

                    //#region Data Ibu
                    $('#<%=hdnMotherMRN.ClientID %>').val(result.MotherMRN);
                    $('#<%=hdnVisitIDMom.ClientID %>').val(result.MotherVisitID);
                    $('#<%=hdnVisitIDIbu.ClientID %>').val(result.MotherVisitID);
                    $('#<%=txtNoRegIbu.ClientID %>').val(result.MotherRegistrationNo);
                    $('#<%=txtMotherMRN.ClientID %>').val(result.MotherMedicalNo);
                    cboSalutationMother.SetValue(result.MotherGCSalutation);
                    cboTitleMother.SetValue(result.MotherGCTitle);
                    cboSuffixMother.SetValue(result.MotherGCSuffix);
                    cboGCIdentityNoType.SetValue(result.MotherGCIdentityNoType);
                    $('#<%=txtFirstNameMother.ClientID %>').val(result.MotherFirstName);
                    $('#<%=txtMiddleNameMother.ClientID %>').val(result.MotherMiddleName);
                    $('#<%=txtLastNameMother.ClientID %>').val(result.MotherLastName);
                    $('#<%=txtDOBIbu.ClientID %>').val(result.MotherDateOfBirthInString);
                    $('#<%=txtAgeYearMom.ClientID %>').val(result.AgeMotherInYear);
                    $('#<%=txtAgeMonthMom.ClientID %>').val(result.AgeMotherInMonth);
                    $('#<%=txtAgeDayMom.ClientID %>').val(result.AgeMotherInDay);
                    $('#<%=txtIdentityNo.ClientID %>').val(result.MotherIdentityNo);
                    cboOccupationMother.SetValue(result.MotherGCOccupation);
                    var filterExpressionCode = "AddressID = '" + result.MotherAddressID + "'";
                    Methods.getObject('GetAddressList', filterExpressionCode, function (result2) {
                        if (result2.ZipCode != null) {
                            var filterExpressionZipCode = "ID = '" + result2.ZipCode + "'";
                            Methods.getObject('GetZipCodesList', filterExpressionZipCode, function (result3) {
                                if (result3 != null) {
                                    $('#<%=txtZipCodeMother.ClientID %>').val(result3.ZipCode);
                                    $('#<%=hdnZipCodeMother.ClientID %>').val(result3.ID);
                                }
                                else {
                                    $('#<%=txtZipCodeMother.ClientID %>').val('');
                                    $('#<%=hdnZipCodeMother.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            $('#<%=txtZipCodeMother.ClientID %>').val('');
                            $('#<%=hdnZipCodeMother.ClientID %>').val('');
                        }
                    });
                    $('#<%=txtCountyMother.ClientID %>').val(result.MotherCounty);
                    $('#<%=txtDistrictMother.ClientID %>').val(result.MotherDistrict);
                    $('#<%=txtCityMother.ClientID %>').val(result.MotherCity);
                    $('#<%=txtProvinceCodeMother.ClientID %>').val(result.MotherGCState.split('^')[1]);
                    $('#<%=txtProvinceNameMother.ClientID %>').val(result.MotherState);
                    $('#<%=txtTelephoneNoMother.ClientID %>').val(result.MotherPhoneNo1);

                    cboGCBirthFromHIVMother.SetValue(result.GCBirthFromHIVMother);
                    cboGCBirthFromSyphilisMother.SetValue(result.GCBirthFromSyphilisMother);
                    cboGCBirthFromHepatitisMother.SetValue(result.GCBirthFromHepatitisMother);

                    //#endregion

                    $('#<%=hdnParamedicID1.ClientID %>').val(result.ParamedicID1);
                    $('#<%=txtParamedicCode1.ClientID %>').val(result.ParamedicCode1);
                    $('#<%=txtParamedicName1.ClientID %>').val(result.ParamedicName1);

                    $('#<%=hdnParamedicID2.ClientID %>').val(result.ParamedicID2);
                    $('#<%=txtParamedicCode2.ClientID %>').val(result.ParamedicCode2);
                    $('#<%=txtParamedicName2.ClientID %>').val(result.ParamedicName2);

                    $('#<%=hdnParamedicID3.ClientID %>').val(result.ParamedicID3);
                    $('#<%=txtParamedicCode3.ClientID %>').val(result.ParamedicCode3);
                    $('#<%=txtParamedicName3.ClientID %>').val(result.ParamedicName3);

                    //#region Data Ayah
                    $('#<%=hdnFatherMRN.ClientID %>').val(result.FatherMRN);
                    $('#<%=txtFatherMRN.ClientID %>').val(result.FatherMedicalNo);
                    cboSalutationFather.SetValue(result.FatherGCSalutation);
                    cboTitleFather.SetValue(result.FatherGCTitle);
                    $('#<%=txtFirstNameFather.ClientID %>').val(result.FatherFirstName);
                    $('#<%=txtMiddleNameFather.ClientID %>').val(result.FatherMiddleName);
                    $('#<%=txtLastNameFather.ClientID %>').val(result.FatherLastName);
                    cboSuffixFather.SetValue(result.FatherGCSuffix);
                    cboOccupationFather.SetValue(result.FatherGCOccupation);

                    setDatePicker('<%=txtFatherDOB.ClientID %>');
                    $('#<%=txtFatherDOB.ClientID %>').datepicker('option', 'maxDate', '0');
                    $('#<%=txtFatherDOB.ClientID %>').val(result.FatherDateOfBirthInStringDatePicker);

                    $('#<%=txtAddressFather.ClientID %>').val(result.FatherStreetName);
                    $('#<%=txtZipCodeFather.ClientID %>').val(result.FatherZipCode);
                    $('#<%=txtCountyFather.ClientID %>').val(result.FatherCounty);
                    $('#<%=txtDistrictFather.ClientID %>').val(result.FatherDistrict);
                    $('#<%=txtCityFather.ClientID %>').val(result.FatherCity);
                    $('#<%=txtProvinceCodeFather.ClientID %>').val(result.FatherGCState.split('^')[1]);
                    $('#<%=txtProvinceNameFather.ClientID %>').val(result.FatherState);
                    $('#<%=txtTelephoneNoFather.ClientID %>').val(result.FatherPhoneNo1);

                    $('#<%=txtIdentityNoFather.ClientID %>').val(result.FatherIdentityNo);
                    cboGCIdentityNoTypeFather.SetValue(result.FatherGCIdentityNo);


                    //#endregion

                }
                else {
                    //#region Baby
                    $('#<%=hdnBabyMRN.ClientID %>').val('');
                    $('#<%=hdnVisitID.ClientID %>').val('');
                    $('#<%=hdnVisitIDBayi.ClientID %>').val('');
                    cboSalutationBaby.SetValue('');
                    $('#<%=txtNoRegBayi.ClientID %>').val('');
                    $('#<%=txtNoSKL.ClientID %>').val('');
                    $('#<%=txtFirstNameBaby.ClientID %>').val('');
                    $('#<%=txtMiddleNameBaby.ClientID %>').val('');
                    $('#<%=txtLastNameBaby.ClientID %>').val('');
                    $('#<%=txtPreferredNameBaby.ClientID %>').val('');
                    $('#<%=txtGenderBaby.ClientID %>').val('');
                    $('#<%=txtDOBBayi.ClientID %>').val('');

                    $('#<%=hdnAddressIDBaby.ClientID %>').val('');
                    $('#<%=txtAddressBaby.ClientID %>').val('');
                    $('#<%=txtZipCodeBaby.ClientID %>').val('');
                    $('#<%=txtCountyBaby.ClientID %>').val('');
                    $('#<%=txtDistrictBaby.ClientID %>').val('');
                    $('#<%=txtCityBaby.ClientID %>').val('');
                    $('#<%=txtProvinceCodeBaby.ClientID %>').val('');
                    $('#<%=txtProvinceNameBaby.ClientID %>').val('');
                    $('#<%=txtTelephoneNoBaby.ClientID %>').val('');

                    $('#<%=txtBirthNo.ClientID %>').val('');
                    $('#<%=txtChildNo.ClientID %>').val('');
                    $('#<%=txtTimeOfBirth.ClientID %>').val('');
                    cboBornAt.SetValue('');
                    $('#<%=txtBirthPregnancyAge.ClientID %>').val('');
                    $('#<%=txtAPGARScore1.ClientID %>').val('');
                    $('#<%=txtAPGARScore2.ClientID %>').val('');
                    $('#<%=txtAPGARScore3.ClientID %>').val('');
                    $('#<%=txtLength.ClientID %>').val('');
                    $('#<%=txtWeight.ClientID %>').val('');
                    $('#<%=txtWeightGram.ClientID %>').val('');
                    $('#<%=txtHeadCircumference.ClientID %>').val('');
                    $('#<%=txtChestCircumference.ClientID %>').val('');
                    cboCaesarMethod.SetValue('');
                    cboTwinSingle.SetValue('');
                    cboBornCondition.SetValue('');
                    cboPartumDeathType.SetValue('');
                    cboNeonatalPerinatalDeathType.SetValue('');
                    cboBirthMethod.SetValue('');
                    cboBirthComplication.SetValue('');
                    cboBirthCOD.SetValue('');

                    $('#trPartumDeathType').attr('style', 'display:none');
                    $('#trNeonatalPerinatalDeathType').attr('style', 'display:none');
                    //#endregion

                    //#region Data Ibu
                    $('#<%=hdnMotherMRN.ClientID %>').val('');
                    $('#<%=hdnVisitIDMom.ClientID %>').val('');
                    $('#<%=hdnVisitIDIbu.ClientID %>').val('');
                    $('#<%=txtNoRegIbu.ClientID %>').val('');
                    $('#<%=txtMotherMRN.ClientID %>').val('');
                    cboSalutationMother.SetValue('');
                    cboTitleMother.SetValue('');
                    cboSuffixMother.SetValue('');
                    cboGCIdentityNoType.SetValue('');
                    $('#<%=txtFirstNameMother.ClientID %>').val('');
                    $('#<%=txtMiddleNameMother.ClientID %>').val('');
                    $('#<%=txtLastNameMother.ClientID %>').val('');
                    $('#<%=txtDOBIbu.ClientID %>').val('');
                    $('#<%=txtAgeYearMom.ClientID %>').val('');
                    $('#<%=txtAgeMonthMom.ClientID %>').val('');
                    $('#<%=txtAgeDayMom.ClientID %>').val('');
                    $('#<%=txtIdentityNo.ClientID %>').val('');
                    cboOccupationMother.SetValue('');

                    $('#<%=hdnMotherMRN.ClientID %>').val('');
                    $('#<%=txtAddressMother.ClientID %>').val('');
                    $('#<%=txtZipCodeMother.ClientID %>').val('');
                    $('#<%=txtCountyMother.ClientID %>').val('');
                    $('#<%=txtDistrictMother.ClientID %>').val('');
                    $('#<%=txtCityMother.ClientID %>').val('');
                    $('#<%=txtProvinceCodeMother.ClientID %>').val('');
                    $('#<%=txtProvinceNameMother.ClientID %>').val('');
                    $('#<%=txtTelephoneNoMother.ClientID %>').val('');

                    cboGCBirthFromHIVMother.SetValue('');
                    cboGCBirthFromSyphilisMother.SetValue('');
                    cboGCBirthFromHepatitisMother.SetValue('');

                    //#endregion

                    $('#<%=hdnParamedicID1.ClientID %>').val('');
                    $('#<%=txtParamedicCode1.ClientID %>').val('');
                    $('#<%=txtParamedicName1.ClientID %>').val('');

                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode2.ClientID %>').val('');
                    $('#<%=txtParamedicName2.ClientID %>').val('');

                    $('#<%=hdnParamedicID3.ClientID %>').val('');
                    $('#<%=txtParamedicCode3.ClientID %>').val('');
                    $('#<%=txtParamedicName3.ClientID %>').val('');

                    //#region Data Ayah
                    cboSalutationFather.SetValue('');
                    cboTitleFather.SetValue('');
                    $('#<%=txtFirstNameFather.ClientID %>').val('');
                    $('#<%=txtMiddleNameFather.ClientID %>').val('');
                    $('#<%=txtLastNameFather.ClientID %>').val('');
                    cboSuffixFather.SetValue('');
                    cboOccupationFather.SetValue('');
                    $('#<%=txtFatherDOB.ClientID %>').val('');
                    setDatePicker('<%=txtFatherDOB.ClientID %>');
                    $('#<%=txtFatherDOB.ClientID %>').datepicker('option', 'maxDate', '0');

                    $('#<%=hdnAddressIDFather.ClientID %>').val('');
                    $('#<%=txtAddressFather.ClientID %>').val('');
                    $('#<%=txtZipCodeFather.ClientID %>').val('');
                    $('#<%=txtCountyFather.ClientID %>').val('');
                    $('#<%=txtDistrictFather.ClientID %>').val('');
                    $('#<%=txtCityFather.ClientID %>').val('');
                    $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                    $('#<%=txtProvinceNameFather.ClientID %>').val('');
                    $('#<%=txtTelephoneNoFather.ClientID %>').val('');
                    //#endregion
                }
            });
            setRightPanelButtonEnabled();
        }

        //#region Berat Bayi
        $('#<%=txtWeight.ClientID %>').die('change');
        $('#<%=txtWeight.ClientID %>').live('change', function () {
            var weight = parseFloat($('#<%=txtWeight.ClientID %>').val().replace('.00', '').split(',').join(''));
            var weightgram = parseFloat(weight * 1000);
            $('#<%=txtWeightGram.ClientID %>').val(weightgram).trigger('changeValue');
        });

        $('#<%=txtWeightGram.ClientID %>').die('change');
        $('#<%=txtWeightGram.ClientID %>').live('change', function () {
            var weightgram = parseFloat($('#<%=txtWeightGram.ClientID %>').val().replace('.00', '').split(',').join(''));
            var weight = parseFloat(weightgram / 1000);
            $('#<%=txtWeight.ClientID %>').val(weight).trigger('changeValue');
        });
        //#endregion

        //#region Dokter Persalinan
        function onGetParamedicMasterFilterExpression() {
            var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblParamedic1.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetParamedicMasterFilterExpression(), function (value) {
                $('#<%=txtParamedicCode1.ClientID %>').val(value);
                ontxtParamedicCodeChanged(value);
            });
        });

        $('#<%=txtParamedicCode1.ClientID %>').live('change', function () {
            ontxtParamedicCodeChanged($(this).val());
        });

        function ontxtParamedicCodeChanged(value) {
            var filterExpression = onGetParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID1.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName1.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%=hdnParamedicID1.ClientID %>').val('');
                    $('#<%=txtParamedicCode1.ClientID %>').val('');
                    $('#<%=txtParamedicName1.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Dokter Anak
        function onGetParamedicMaster2FilterExpression() {
            var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblParamedic2.lblLink').live('click', function () {
            var filterExpression = onGetParamedicMaster2FilterExpression();
            openSearchDialog('paramedic', filterExpression, function (value) {
                $('#<%=txtParamedicCode2.ClientID %>').val(value);
                onTxtPhysicianCode2Changed(value);
            });
        });

        $('#<%=txtParamedicCode2.ClientID %>').live('change', function () {
            onTxtPhysicianCode2Changed($(this).val());
        });

        function onTxtPhysicianCode2Changed(value) {
            var filterExpression = onGetParamedicMaster2FilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID2.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName2.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode2.ClientID %>').val('');
                    $('#<%=txtParamedicName2.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Bidan / Perawat
        function onGetParamedicMaster3FilterExpression() {
            var filterExpression = "GCParamedicMasterType IN ('X019^002','X019^003') AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblParamedic3.lblLink').live('click', function () {
            var filterExpression = onGetParamedicMaster3FilterExpression();
            openSearchDialog('paramedic', filterExpression, function (value) {
                $('#<%=txtParamedicCode3.ClientID %>').val(value);
                onTxtPhysicianCode3Changed(value);
            });
        });

        $('#<%=txtParamedicCode3.ClientID %>').live('change', function () {
            onTxtPhysicianCode3Changed($(this).val());
        });

        function onTxtPhysicianCode3Changed(value) {
            var filterExpression = onGetParamedicMaster3FilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID3.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName3.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnParamedicID3.ClientID %>').val('');
                    $('#<%=txtParamedicCode3.ClientID %>').val('');
                    $('#<%=txtParamedicName3.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Zip Code Baby
        $('#<%=lblZipCodeBaby.ClientID %>').live('click', function (evt) {
            openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                onTxtZipCodeBabyChanged(value);
            });
        });

        $('#<%=txtZipCodeBaby.ClientID %>').change(function () {
            onTxtZipCodeBabyChangedValue($(this).val());
        });

        //#region Has Medical No?
        $('#<%=chkIsFatherHasMRN.ClientID %>').live('change', function (evt) {
            if ($('#<%=chkIsFatherHasMRN.ClientID %>').is(":checked")) {
                $('#lblMRNFather').attr('class', 'lblLink');
            }
            else {

                $('#lblMRNFather').attr('class', 'lblDisabled');
                $('#<%=hdnFatherMRN.ClientID %>').val(0);
                $('#<%=txtFatherMRN.ClientID %>').val('');
                cboSalutationFather.SetValue('');
                cboTitleFather.SetValue('');
                $('#<%=txtFirstNameFather.ClientID %>').val("");
                $('#<%=txtMiddleNameFather.ClientID %>').val("");
                $('#<%=txtLastNameFather.ClientID %>').val("");
                $('#<%=txtFatherDOB.ClientID %>').val("");
                setDatePicker('<%=txtFatherDOB.ClientID %>');
                $('#<%=txtFatherDOB.ClientID %>').datepicker('option', 'maxDate', '0');
                cboSuffixFather.SetValue('');
                cboOccupationFather.SetValue('');
                $('#<%=txtAddressFather.ClientID %>').val("");
                $('#<%=txtZipCodeFather.ClientID %>').val("");
                $('#<%=txtCountyFather.ClientID %>').val("");
                $('#<%=txtDistrictFather.ClientID %>').val("");
                $('#<%=txtCityFather.ClientID %>').val("");
                $('#<%=txtProvinceCodeFather.ClientID %>').val("");
                $('#<%=txtProvinceNameFather.ClientID %>').val("");
                $('#<%=txtTelephoneNoFather.ClientID %>').val("");
            }
        });
        $('#<%=chkIsFatherHasMRN.ClientID %>').change();

        function onTxtZipCodeBabyChanged(value) {
            if (value != '') {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeBaby.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeBaby.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCityBaby.ClientID %>').val(result.City);
                        $('#<%=txtCountyBaby.ClientID %>').val(result.County);
                        $('#<%=txtDistrictBaby.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCodeBaby.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameBaby.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCodeBaby.ClientID %>').val('');
                        $('#<%=txtZipCodeBaby.ClientID %>').val('');
                        $('#<%=txtCityBaby.ClientID %>').val('');
                        $('#<%=txtCountyBaby.ClientID %>').val('');
                        $('#<%=txtDistrictBaby.ClientID %>').val('');
                        $('#<%=txtProvinceCodeBaby.ClientID %>').val('');
                        $('#<%=txtProvinceNameBaby.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeBaby.ClientID %>').val('');
                $('#<%=txtZipCodeBaby.ClientID %>').val('');
                $('#<%=txtCityBaby.ClientID %>').val('');
                $('#<%=txtCountyBaby.ClientID %>').val('');
                $('#<%=txtDistrictBaby.ClientID %>').val('');
                $('#<%=txtProvinceCodeBaby.ClientID %>').val('');
                $('#<%=txtProvinceNameBaby.ClientID %>').val('');
            }
        }

        function onTxtZipCodeBabyChangedValue(value) {
            if (value != '') {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeBaby.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeBaby.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCityBaby.ClientID %>').val(result.City);
                        $('#<%=txtCountyBaby.ClientID %>').val(result.County);
                        $('#<%=txtDistrictBaby.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCodeBaby.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameBaby.ClientID %>').val(result.Province);
                        $('#<%=txtTelephoneNoBaby.ClientID %>').val(result.NationPhoneCode);
                    }
                    else {
                        $('#<%=hdnZipCodeBaby.ClientID %>').val('');
                        $('#<%=txtZipCodeBaby.ClientID %>').val('');
                        $('#<%=txtCityBaby.ClientID %>').val('');
                        $('#<%=txtCountyBaby.ClientID %>').val('');
                        $('#<%=txtDistrictBaby.ClientID %>').val('');
                        $('#<%=txtProvinceCodeBaby.ClientID %>').val('');
                        $('#<%=txtProvinceNameBaby.ClientID %>').val('');
                        $('#<%=txtTelephoneNoBaby.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeBaby.ClientID %>').val('');
                $('#<%=txtZipCodeBaby.ClientID %>').val('');
                $('#<%=txtCityBaby.ClientID %>').val('');
                $('#<%=txtCountyBaby.ClientID %>').val('');
                $('#<%=txtDistrictBaby.ClientID %>').val('');
                $('#<%=txtProvinceCodeBaby.ClientID %>').val('');
                $('#<%=txtProvinceNameBaby.ClientID %>').val('');
                $('#<%=txtTelephoneNoBaby.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Zip Code Mother
        $('#<%=lblZipCodeMother.ClientID %>').live('click', function (evt) {
            openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                onTxtZipCodeMotherChanged(value);
            });
        });

        $('#<%=txtZipCodeMother.ClientID %>').change(function () {
            onTxtZipCodeMotherChangedValue($(this).val());
        });

        function onTxtZipCodeMotherChanged(value) {
            if (value != '') {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeMother.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeMother.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCityMother.ClientID %>').val(result.City);
                        $('#<%=txtCountyMother.ClientID %>').val(result.County);
                        $('#<%=txtDistrictMother.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCodeMother.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameMother.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCodeMother.ClientID %>').val('');
                        $('#<%=txtZipCodeMother.ClientID %>').val('');
                        $('#<%=txtCityMother.ClientID %>').val('');
                        $('#<%=txtCountyMother.ClientID %>').val('');
                        $('#<%=txtDistrictMother.ClientID %>').val('');
                        $('#<%=txtProvinceCodeMother.ClientID %>').val('');
                        $('#<%=txtProvinceNameMother.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeMother.ClientID %>').val('');
                $('#<%=txtZipCodeMother.ClientID %>').val('');
                $('#<%=txtCityMother.ClientID %>').val('');
                $('#<%=txtCountyMother.ClientID %>').val('');
                $('#<%=txtDistrictMother.ClientID %>').val('');
                $('#<%=txtProvinceCodeMother.ClientID %>').val('');
                $('#<%=txtProvinceNameMother.ClientID %>').val('');
            }
        }

        function onTxtZipCodeMotherChangedValue(value) {
            if (value != '') {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeMother.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeMother.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCityMother.ClientID %>').val(result.City);
                        $('#<%=txtCountyMother.ClientID %>').val(result.County);
                        $('#<%=txtDistrictMother.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCodeMother.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameMother.ClientID %>').val(result.Province);
                        $('#<%=txtTelephoneNoMother.ClientID %>').val(result.NationPhoneCode);
                    }
                    else {
                        $('#<%=hdnZipCodeMother.ClientID %>').val('');
                        $('#<%=txtZipCodeMother.ClientID %>').val('');
                        $('#<%=txtCityMother.ClientID %>').val('');
                        $('#<%=txtCountyMother.ClientID %>').val('');
                        $('#<%=txtDistrictMother.ClientID %>').val('');
                        $('#<%=txtProvinceCodeMother.ClientID %>').val('');
                        $('#<%=txtProvinceNameMother.ClientID %>').val('');
                        $('#<%=txtTelephoneNoMother.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeMother.ClientID %>').val('');
                $('#<%=txtZipCodeMother.ClientID %>').val('');
                $('#<%=txtCityMother.ClientID %>').val('');
                $('#<%=txtCountyMother.ClientID %>').val('');
                $('#<%=txtDistrictMother.ClientID %>').val('');
                $('#<%=txtProvinceCodeMother.ClientID %>').val('');
                $('#<%=txtProvinceNameMother.ClientID %>').val('');
                $('#<%=txtTelephoneNoMother.ClientID %>').val('');
            }
        }
        //#endregion
        //#region address
        $('#<%=chkCopyAlamatIbuKeBayi.ClientID %>').live('change', function () {
            var ischecked = $(this).is(':checked');
            if (!ischecked) {
                formAddressBaby(false);
            } else {
                formAddressBaby(true);
            }
        });
        function formAddressBaby(checked) {

            var address = $('#<%=txtAddressMother.ClientID %>').val();
            var zipcode = $('#<%=txtZipCodeMother.ClientID %>').val();
            var county = $('#<%=txtCountyMother.ClientID %>').val();
            var District = $('#<%=txtDistrictMother.ClientID %>').val();
            var City = $('#<%=txtCityMother.ClientID %>').val();
            var ProvinceCode = $('#<%=txtProvinceCodeMother.ClientID %>').val();
            var ProvinceName = $('#<%=txtProvinceNameMother.ClientID %>').val();
            var Telephone = $('#<%=txtTelephoneNoMother.ClientID %>').val();

            if (checked) {
                $('#<%=txtAddressBaby.ClientID %>').val(address);
                $('#<%=txtZipCodeBaby.ClientID %>').val(zipcode);
                $('#<%=txtCountyBaby.ClientID %>').val(county);
                $('#<%=txtDistrictBaby.ClientID %>').val(District);
                $('#<%=txtCityBaby.ClientID %>').val(City);
                $('#<%=txtProvinceCodeBaby.ClientID %>').val(ProvinceCode);
                $('#<%=txtProvinceNameBaby.ClientID %>').val(ProvinceName);
                $('#<%=txtTelephoneNoBaby.ClientID %>').val(Telephone);
            } else {
                $('#<%=txtAddressBaby.ClientID %>').val("");
                $('#<%=txtZipCodeBaby.ClientID %>').val("");
                $('#<%=txtCountyBaby.ClientID %>').val("");
                $('#<%=txtDistrictBaby.ClientID %>').val("");
                $('#<%=txtCityBaby.ClientID %>').val("");
                $('#<%=txtProvinceCodeBaby.ClientID %>').val("");
                $('#<%=txtProvinceNameBaby.ClientID %>').val("");
                $('#<%=txtTelephoneNoBaby.ClientID %>').val("");
            }


        }
        //#endregion
        $('#<%=chkCopyAlamatIbuKeAyah.ClientID %>').live('change', function () {
            var ischecked = $(this).is(':checked');
            if (!ischecked) {
                formAddressFather(false);
            } else {
                formAddressFather(true);
            }
        });
        function formAddressFather(checked) {

            var address = $('#<%=txtAddressMother.ClientID %>').val();
            var zipcode = $('#<%=txtZipCodeMother.ClientID %>').val();
            var county = $('#<%=txtCountyMother.ClientID %>').val();
            var District = $('#<%=txtDistrictMother.ClientID %>').val();
            var City = $('#<%=txtCityMother.ClientID %>').val();
            var ProvinceCode = $('#<%=txtProvinceCodeMother.ClientID %>').val();
            var ProvinceName = $('#<%=txtProvinceNameMother.ClientID %>').val();
            var Telephone = $('#<%=txtTelephoneNoMother.ClientID %>').val();

            if (checked) {
                $('#<%=txtAddressFather.ClientID %>').val(address);
                $('#<%=txtZipCodeFather.ClientID %>').val(zipcode);
                $('#<%=txtCountyFather.ClientID %>').val(county);
                $('#<%=txtDistrictFather.ClientID %>').val(District);
                $('#<%=txtCityFather.ClientID %>').val(City);
                $('#<%=txtProvinceCodeFather.ClientID %>').val(ProvinceCode);
                $('#<%=txtProvinceNameFather.ClientID %>').val(ProvinceName);
                $('#<%=txtTelephoneNoFather.ClientID %>').val(Telephone);
            } else {
                $('#<%=txtAddressFather.ClientID %>').val("");
                $('#<%=txtZipCodeFather.ClientID %>').val("");
                $('#<%=txtCountyFather.ClientID %>').val("");
                $('#<%=txtDistrictFather.ClientID %>').val("");
                $('#<%=txtCityFather.ClientID %>').val("");
                $('#<%=txtProvinceCodeFather.ClientID %>').val("");
                $('#<%=txtProvinceNameFather.ClientID %>').val("");
                $('#<%=txtTelephoneNoFather.ClientID %>').val("");
            }


        }
        //#endregion

        //#region Zip Code Father
        $('#<%=lblZipCodeFather.ClientID %>').live('click', function (evt) {
            openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                onTxtZipCodeFatherChanged(value);
            });
        });

        $('#<%=txtZipCodeFather.ClientID %>').change(function () {
            onTxtZipCodeFatherChangedValue($(this).val());
        });

        function onTxtZipCodeFatherChanged(value) {
            if (value != '') {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeFather.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeFather.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCityFather.ClientID %>').val(result.City);
                        $('#<%=txtCountyFather.ClientID %>').val(result.County);
                        $('#<%=txtDistrictFather.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCodeFather.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameFather.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCodeFather.ClientID %>').val('');
                        $('#<%=txtZipCodeFather.ClientID %>').val('');
                        $('#<%=txtCityFather.ClientID %>').val('');
                        $('#<%=txtCountyFather.ClientID %>').val('');
                        $('#<%=txtDistrictFather.ClientID %>').val('');
                        $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                        $('#<%=txtProvinceNameFather.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeFather.ClientID %>').val('');
                $('#<%=txtZipCodeFather.ClientID %>').val('');
                $('#<%=txtCityFather.ClientID %>').val('');
                $('#<%=txtCountyFather.ClientID %>').val('');
                $('#<%=txtDistrictFather.ClientID %>').val('');
                $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                $('#<%=txtProvinceNameFather.ClientID %>').val('');
            }
        }

        function onTxtZipCodeFatherChangedValue(value) {
            if (value != '') {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeFather.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeFather.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCityFather.ClientID %>').val(result.City);
                        $('#<%=txtCountyFather.ClientID %>').val(result.County);
                        $('#<%=txtDistrictFather.ClientID %>').val(result.District);
                        $('#<%=txtProvinceCodeFather.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameFather.ClientID %>').val(result.Province);
                        $('#<%=txtTelephoneNoFather.ClientID %>').val(result.NationPhoneCode);
                    }
                    else {
                        $('#<%=hdnZipCodeFather.ClientID %>').val('');
                        $('#<%=txtZipCodeFather.ClientID %>').val('');
                        $('#<%=txtCityFather.ClientID %>').val('');
                        $('#<%=txtCountyFather.ClientID %>').val('');
                        $('#<%=txtDistrictFather.ClientID %>').val('');
                        $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                        $('#<%=txtProvinceNameFather.ClientID %>').val('');
                        $('#<%=txtTelephoneNoFather.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeFather.ClientID %>').val('');
                $('#<%=txtZipCodeFather.ClientID %>').val('');
                $('#<%=txtCityFather.ClientID %>').val('');
                $('#<%=txtCountyFather.ClientID %>').val('');
                $('#<%=txtDistrictFather.ClientID %>').val('');
                $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                $('#<%=txtProvinceNameFather.ClientID %>').val('');
                $('#<%=txtTelephoneNoFather.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Province Baby
        function OnGetSCProvinceFilterExpression() {
            var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
            return filterExpression;
        }

        $('#<%=lblProvinceBaby.ClientID %>').live('click', function (evt) {
            openSearchDialog('stdcode', OnGetSCProvinceFilterExpression(), function (value) {
                $('#<%=txtProvinceCodeBaby.ClientID %>').val(value);
                onTxtProvinceCodeBabyChanged(value);
            });
        });

        $('#<%=txtProvinceCodeBaby.ClientID %>').change(function () {
            onTxtProvinceCodeBabyChanged($(this).val());
        });

        function onTxtProvinceCodeBabyChanged(value) {
            var filterExpression = OnGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtProvinceNameBaby.ClientID %>').val(result.StandardCodeName);
                else {
                    $('#<%=txtProvinceCodeBaby.ClientID %>').val('');
                    $('#<%=txtProvinceNameBaby.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Province Mother
        function OnGetSCProvinceFilterExpression() {
            var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
            return filterExpression;
        }

        $('#<%=lblProvinceMother.ClientID %>').live('click', function (evt) {
            openSearchDialog('stdcode', OnGetSCProvinceFilterExpression(), function (value) {
                $('#<%=txtProvinceCodeMother.ClientID %>').val(value);
                onTxtProvinceCodeMotherChanged(value);
            });
        });

        $('#<%=txtProvinceCodeMother.ClientID %>').change(function () {
            onTxtProvinceCodeMotherChanged($(this).val());
        });

        function onTxtProvinceCodeMotherChanged(value) {
            var filterExpression = OnGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtProvinceNameMother.ClientID %>').val(result.StandardCodeName);
                else {
                    $('#<%=txtProvinceCodeMother.ClientID %>').val('');
                    $('#<%=txtProvinceNameMother.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Province Father
        function OnGetSCProvinceFilterExpression() {
            var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
            return filterExpression;
        }

        $('#<%=lblProvinceFather.ClientID %>').live('click', function (evt) {
            openSearchDialog('stdcode', OnGetSCProvinceFilterExpression(), function (value) {
                $('#<%=txtProvinceCodeFather.ClientID %>').val(value);
                onTxtProvinceCodeFatherChanged(value);
            });
        });

        $('#<%=txtProvinceCodeFather.ClientID %>').change(function () {
            onTxtProvinceCodeFatherChanged($(this).val());
        });

        function onTxtProvinceCodeFatherChanged(value) {
            var filterExpression = OnGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtProvinceNameFather.ClientID %>').val(result.StandardCodeName);
                else {
                    $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                    $('#<%=txtProvinceNameFather.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region MRN Father
        function OnGetSCPatientGenderFilterExpression() {
            var filterExpression = "<%:OnGetSCPatientGenderFilterExpression() %>";
            return filterExpression;
        }
        $('#lblMRNFather.lblLink').live('click', function () {
            openSearchDialog('patient', OnGetSCPatientGenderFilterExpression(), function (value) {
                $('#<%=txtFatherMRN.ClientID %>').val(value);
                ontxtFatherMRNChanged(value);
            });
        });
        $('#<%=txtFatherMRN.ClientID %>').live('change', function () {
            ontxtFatherMRNChanged($(this).val());
        });
        function ontxtFatherMRNChanged(value) {
            var mrn = FormatMRN(value);
            var filterExpression = "MedicalNo = '" + mrn + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    var dob = result.cfDateOfBirthInString1;
                    $('#<%=hdnFatherMRN.ClientID %>').val(result.MRN);
                    $('#<%:txtFatherMRN.ClientID %>').val(result.MedicalNo);
                    if (result.GCSalutation != '' && result.GCSalutation != null) {
                        cboSalutationFather.SetValue(result.GCSalutation);
                    }
                    $('#<%=txtFirstNameFather.ClientID %>').val(result.FirstName);
                    $('#<%=txtMiddleNameFather.ClientID %>').val(result.MiddleName);
                    $('#<%=txtLastNameFather.ClientID %>').val(result.LastName);
                    $('#<%=txtFatherDOB.ClientID %>').val(dob);
                    $('#<%=hdnAddressIDFather.ClientID %>').val(result.HomeAddressID);
                    $('#<%=txtAddressFather.ClientID %>').val(result.HomeAddress);
                    $('#<%=txtTelephoneNoFather.ClientID %>').val(result.PhoneNo1);
                    $('#<%=txtCountyFather.ClientID %>').val(result.County);
                    $('#<%=txtDistrictFather.ClientID %>').val(result.District);
                    $('#<%=txtCityFather.ClientID %>').val(result.City);
                    $('#<%=txtZipCodeFather.ClientID %>').val(result.ZipCode);
                    $('#<%=txtProvinceCodeFather.ClientID %>').val(result.GCState.split('^')[1]);
                    $('#<%=txtProvinceNameFather.ClientID %>').val(result.State);
                    cboTitleFather.SetValue(result.GCTitle);
                    cboSuffixFather.SetValue(result.GCSuffix);
                    cboOccupationFather.SetValue(result.GCOccupation);
                }
            });
        }
        //#endregion

        function oncboBornConditionValueChanged() {
            if (cboBornCondition.GetValue() == Constant.BornCondition.Meninggal) {
                $('#trPartumDeathType').removeAttr('style');
                $('#trNeonatalPerinatalDeathType').removeAttr('style');
            }
            else {
                $('#trPartumDeathType').attr('style', 'display:none');
                $('#trNeonatalPerinatalDeathType').attr('style', 'display:none');
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var MRN = $('#<%=hdnBabyMRN.ClientID %>').val();
            var VisitIDMom = $('#<%=hdnVisitIDMom.ClientID %>').val();

            if (MRN == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            } else {
                if (code == 'PM-00614') {
                    filterExpression.text = VisitIDMom;
                    return true;
                } else {
                    filterExpression.text = 'MRN = ' + MRN;
                    return true;
                }
            }
        }

        $('#<%=FileUpload1.ClientID %>').live('change', function () {
            readURL(this);
            var fileName = $('#<%=FileUpload1.ClientID %>').val().replace("C:\\fakepath\\", "");

            $('#<%=txtBabyPhoto.ClientID %>').val(fileName);
            $('#<%=hdnFileName.ClientID %>').val(fileName);

            //fileName = fileName.split('.')[0];
            if ($('#<%=imgPreview.ClientID %>').width() > $('#<%=imgPreview.ClientID %>').height())
                $('#<%=imgPreview.ClientID %>').width('150px');
            else
                $('#<%=imgPreview.ClientID %>').height('150px');
        });

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                    $('#<%=imgPreview.ClientID %>').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
            else {
                $('#<%=imgPreview.ClientID %>').attr('src', input.value);
            }
        }

        $('#btnUploadFile').live('click', function () {
            document.getElementById('<%= FileUpload1.ClientID %>').click();
        });

        $('#btnDeleteFile').live('click', function () {
            readURL(this);
            var fileName = $('#<%=FileUpload1.ClientID %>').val();
            $('#<%=txtBabyPhoto.ClientID %>').val(fileName);
            $('#<%=hdnFileName.ClientID %>').val(fileName);
        });

        function onAfterSaveRecordPatientPageEntry(param) {
            var paramSplit = param.split("|");
            if (paramSplit[0] == 'savepatientbirth') {
                showToast('Information', 'Registrasi bayi berhasil disimpan dengan nomor <b>' + paramSplit[1]);
            }
        }
    </script>
    <input type="hidden" id="hdnURLPictureDirectory" value="" runat="server" />
    <input type="hidden" id="hdnBirthRecordID" value="0" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="0" runat="server" />
    <input type="hidden" id="hdnFileName" value="0" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="0" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Bayi")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnVisitID" value="0" runat="server" />
                    <input type="hidden" id="hdnBabyMRN" value="0" runat="server" />
                    <input type="hidden" id="hdnVisitIDBayi" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNoRegBayi">
                                    <%=GetLabel("No Pendaftaran Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoRegBayi" Width="100%" runat="server" TabIndex="9" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No RM Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBabyMRN" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. SKL")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoSKL" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sapaan Bayi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSalutationBaby" ClientInstanceName="cboSalutationBaby" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Bayi")%></label>
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
                                            <asp:TextBox ID="txtFirstNameBaby" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleNameBaby" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Belakang Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameBaby" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Panggilan Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreferredNameBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGenderBaby" Width="100%" runat="server" TabIndex="1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Lahir")%>
                                    /
                                    <%=GetLabel("Umur Bayi")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDOBBayi" Width="108px" runat="server" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Foto Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBabyPhoto" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="height: 150px; width: 150px; border: 1px solid ActiveBorder;"
                                align="center">
                                <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                                <div>
                                    <img src="" alt="" runat="server" id="imgPreview" width="170" height="150" />
                                </div>
                            </td>
                            <td>
                                <div style="display: none">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnUploadFile" value="Upload" />
                                        </td>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnDeleteFile" value="Delete" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%=GetLabel("extension") %>
                                            : jpg,jpeg,png.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%=GetLabel("Maximum upload size") %>
                                            : 10MB
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA ALAMAT BAYI")%></label></b>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCopyAlamatIbuKeBayi" runat="server" /><%:GetLabel("Salin Data Alamat Ibu")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <input type="hidden" id="hdnAddressIDBaby" runat="server" />
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressBaby" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCodeBaby" runat="server">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCodeBaby" value="" />
                                <asp:TextBox ID="txtZipCodeBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountyBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvinceBaby" runat="server">
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
                                            <asp:TextBox ID="txtProvinceCodeBaby" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceNameBaby" ReadOnly="true" Width="100%" runat="server" />
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
                                <asp:TextBox ID="txtTelephoneNoBaby" CssClass="required" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA KELAHIRAN BAYI")%></label></b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kelahiran ke")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthNo" TabIndex="3" Width="30%" runat="server" CssClass="number" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Anak ke")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 20%" />
                                        <col style="width: 30%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtChildNo" TabIndex="3" Width="80%" runat="server" CssClass="number" />
                                        </td>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Jam lahir")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTimeOfBirth" TabIndex="3" Width="80%" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tempat Lahir")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBornAt" ClientInstanceName="cboBornAt" Width="100%" runat="server"
                                    TabIndex="4" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Usia Kehamilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthPregnancyAge" Width="80px" runat="server" CssClass="number"
                                    TabIndex="5" />
                                <label>
                                    <%=GetLabel("Minggu")%>
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblParamedic1">
                                    <%=GetLabel("Dokter Persalinan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnParamedicID1" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtParamedicCode1" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParamedicName1" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblParamedic2">
                                    <%=GetLabel("Dokter Anak")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnParamedicID2" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtParamedicCode2" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParamedicName2" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblParamedic3">
                                    <%=GetLabel("Bidan/Perawat")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnParamedicID3" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtParamedicCode3" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParamedicName3" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore1" Width="100%" CssClass="number" runat="server" TabIndex="6" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore2" Width="100%" CssClass="number" runat="server" TabIndex="7" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 3")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore3" Width="100%" CssClass="number" runat="server" TabIndex="8" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNewBornGivenKangarooMethod" Width="100%" runat="server" Text=" Bayi BBLR dilakukan perawatan metode kanguru" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNewBornGivenEarlyInitiationBreastfeeding" Width="100%" runat="server" Text=" Bayi baru lahir yg dilakukan IMD" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNewBornGivenCongenitalHyperthyroidismScreening" Width="100%" runat="server" Text=" Bayi baru lahir yg dilakukan Skrining Hipertiroid Kongenital" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kuantitatif")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 18%" />
                            <col style="width: 20%" />
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Panjang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLength" Width="80px" runat="server" TabIndex="11" CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Berat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeight" Width="80px" runat="server" TabIndex="12" CssClass="number" />
                                <label>
                                    <%=GetLabel("kg")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeightGram" Width="80px" runat="server" TabIndex="12" CssClass="number" />
                                <label>
                                    <%=GetLabel("gr")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lingkar Kepala")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHeadCircumference" Width="80px" runat="server" TabIndex="13"
                                    CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lingkar Dada")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChestCircumference" Width="80px" runat="server" TabIndex="14"
                                    CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kualitatif")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara Caesar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCaesarMethod" ClientInstanceName="cboCaesarMethod" Width="100%"
                                    runat="server" TabIndex="15" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kembar / Tunggal")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTwinSingle" ClientInstanceName="cboTwinSingle" Width="100%"
                                    runat="server" TabIndex="16" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kondisi Kelahiran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBornCondition" Width="100%" runat="server" ClientInstanceName="cboBornCondition"
                                    TabIndex="17">
                                    <ClientSideEvents Init="function(s,e){ oncboBornConditionValueChanged(s); }" ValueChanged="function(s,e){ oncboBornConditionValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trPartumDeathType" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lahir Mati")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPartumDeathType" ClientInstanceName="cboPartumDeathType"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trNeonatalPerinatalDeathType" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kematian Neonatal dan Perinatal")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboNeonatalPerinatalDeathType" ClientInstanceName="cboNeonatalPerinatalDeathType"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Melahirkan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthMethod" ClientInstanceName="cboBirthMethod" Width="100%"
                                    runat="server" TabIndex="18" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Komplikasi Kelahiran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthComplication" ClientInstanceName="cboBirthComplication"
                                    Width="100%" runat="server" TabIndex="19" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kelainan Bawaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHerediateryDefectRemarks" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sebab Kematian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthCOD" ClientInstanceName="cboBirthCOD" Width="100%"
                                    runat="server" TabIndex="20" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Ibu")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnVisitIDMom" value="0" runat="server" />
                    <input type="hidden" id="hdnVisitIDIbu" value="0" runat="server" />
                    <input type="hidden" id="hdnMotherMRN" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblNoRegIbu">
                                    <%=GetLabel("No Pendaftaran Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoRegIbu" Width="100%" runat="server" TabIndex="9" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No RM Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherMRN" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sapaan Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSalutationMother" ClientInstanceName="cboSalutationMother"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Depan Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTitleMother" ClientInstanceName="cboTitleMother" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ibu")%></label>
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
                                            <asp:TextBox ID="txtFirstNameMother" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleNameMother" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Belakang Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameMother" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffixMother" ClientInstanceName="cboSuffixMother" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Lahir")%>
                                    /
                                    <%=GetLabel("Umur Ibu")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDOBIbu" Width="95%" runat="server" Style="margin-right: 3px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeYearMom" Width="95%" runat="server" Style="margin-right: 1px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Tahun")%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeMonthMom" Width="95%" runat="server" Style="margin-right: 1px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Bulan")%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeDayMom" Width="95%" runat="server" Style="margin-right: 1px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Hari")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Kartu Identitas")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCIdentityNoType" ClientInstanceName="cboGCIdentityNoType"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityNo" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pekerjaan Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboOccupationMother" ClientInstanceName="cboOccupationMother"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("RIWAYAT INFEKSIUS IBU")%></label></b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Bayi yang Lahir dari Ibu HIV")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCBirthFromHIVMother" Width="100%" ClientInstanceName="cboGCBirthFromHIVMother"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Bayi yang Lahir dari Ibu Sifilis")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCBirthFromSyphilisMother" Width="100%" ClientInstanceName="cboGCBirthFromSyphilisMother"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Bayi yang Lahir dari Ibu Hepatitis")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCBirthFromHepatitisMother" Width="100%" ClientInstanceName="cboGCBirthFromHepatitisMother"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA ALAMAT IBU")%></label></b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <input type="hidden" id="hdnAddressIDMother" runat="server" />
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressMother" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCodeMother" runat="server">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCodeMother" value="" />
                                <asp:TextBox ID="txtZipCodeMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountyMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvinceMother" runat="server">
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
                                            <asp:TextBox ID="txtProvinceCodeMother" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceNameMother" ReadOnly="true" Width="100%" runat="server" />
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
                                <asp:TextBox ID="txtTelephoneNoMother" CssClass="required" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Ayah")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnFatherMRN" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsFatherHasMRN" Checked="true" runat="server" /><%:GetLabel("Has Medical No?")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMRNFather">
                                    <%=GetLabel("No RM Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherMRN" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sapaan Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSalutationFather" ClientInstanceName="cboSalutationFather"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Depan Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTitleFather" ClientInstanceName="cboTitleFather" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ayah")%></label>
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
                                            <asp:TextBox ID="txtFirstNameFather" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleNameFather" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Belakang Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffixFather" ClientInstanceName="cboSuffixFather" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Lahir Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherDOB" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Kartu Identitas")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCIdentityNoTypeFather" ClientInstanceName="cboGCIdentityNoTypeFather"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityNoFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pekerjaan Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboOccupationFather" ClientInstanceName="cboOccupationFather"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA ALAMAT AYAH")%></label></b>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCopyAlamatIbuKeAyah" runat="server" /><%:GetLabel("Salin Data Alamat Ibu")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <input type="hidden" id="hdnAddressIDFather" runat="server" />
                                <label class="lblNormal">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressFather" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCodeFather" runat="server">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCodeFather" value="" />
                                <asp:TextBox ID="txtZipCodeFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountyFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvinceFather" runat="server">
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
                                            <asp:TextBox ID="txtProvinceCodeFather" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceNameFather" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNoFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
