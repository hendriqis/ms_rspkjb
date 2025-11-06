<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientEntryReservationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientEntryReservationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PatientEntryReservationCtl">

    $(document).ready(function () {
        var hdnIsMobilePhoneNumeric = $('#<%=hdnIsMobilePhoneNumeric.ClientID %>').val();
        if (hdnIsMobilePhoneNumeric == "1") {
            $('#<%=txtMobilePhone1.ClientID %>').TextNumericOnly();
            $('#<%=txtMobilePhone2.ClientID %>').TextNumericOnly();
        }
    });

    $('#<%=grdPatientNotesView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
        $('#<%=grdPatientNotesView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnPatientNotesID.ClientID %>').val($(this).find('.keyField').html());
    });
    $('#<%=grdPatientNotesView.ClientID %> tr:eq(1)').click();

    $('#<%=txtPatientNotes.ClientID %>').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            submitPatientNotes();
        }
    });

    setDatePicker('<%=txtDOB.ClientID %>');
    $('#<%=txtDOB.ClientID %>').datepicker('option', 'maxDate', '0');
    registerCollapseExpandHandler();

    $('#<%=txtCardName.ClientID %>').keyup(function () {
        var cardName = $(this).val();
        if (cardName.length > 28) {
            cardName = cardName.substring(0, cardName.length - 1);
            $(this).val(cardName);
            showToast('Warning', 'Maximal 28 Character untuk Nama di Kartu');
        }
    });

    $("#btnSearchBPJSUsingIDCard").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtIdentityCardNo.ClientID %>').val() == '')
            alert("Nomor NIK harus diisi!");
        else {
            var noNIK = $('#<%=txtIdentityCardNo.ClientID %>').val();
            var tglSEP = Methods.dateToYMD(new Date());
            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                BPJSService.getPesertaByNIK(noNIK, tglSEP, function (result) {
                    GetPesertaHandler(result);
                });
            } else {
                BPJSService.getPesertaByNIKMedinfrasAPI(noNIK, tglSEP, function (result) {
                    GetPesertaHandler(result);
                });
            }
        }
    });

    function GetPesertaHandler(result) {
        try {
            resetPEBPJSField();
            var resultInfo = result.split('|');
            if (resultInfo[0] == "1") {
                var obj = jQuery.parseJSON(resultInfo[1]);
                if (obj.response.peserta != null) {
                    $('#<%=txtNHSRegistrationNo.ClientID %>').val(obj.response.peserta.noKartu);
                    $('#<%=txtPENamaPeserta.ClientID %>').val(obj.response.peserta.nama);
                    $('#<%=txtPEJenisPeserta.ClientID %>').val(obj.response.peserta.jenisPeserta.keterangan);
                    $('#<%=txtPEKelas.ClientID %>').val(obj.response.peserta.hakKelas.kode + ' - ' + obj.response.peserta.hakKelas.keterangan);
                    $('#<%=txtPEPpkRujukan.ClientID %>').val(obj.response.peserta.provUmum.kdProvider + ' - ' + obj.response.peserta.provUmum.nmProvider);
                    $('#<%=txtIdentityCardNo.ClientID %>').val(obj.response.peserta.nik);
                    $('#<%=txtCardName.ClientID %>').val(obj.response.peserta.nama);
                    if ($('#<%=txtMobilePhone1.ClientID %>').val() == '') {
                        $('#<%=txtMobilePhone1.ClientID %>').val(obj.response.peserta.mr.noTelepon);
                    }
                    if ($('#<%=hdnPatientLastName.ClientID %>').val() == '0') {
                        var nama = obj.response.peserta.nama.split(' ');
                        if (nama.length == 1) {
                            $('#<%=txtFamilyName.ClientID %>').val(nama[0]);
                        }
                        else if (nama.length == 2) {
                            $('#<%=txtFirstName.ClientID %>').val(nama[0]);
                            $('#<%=txtFamilyName.ClientID %>').val(nama[1]);
                        }
                        else {
                            var tempNama = '';
                            $('#<%=txtFirstName.ClientID %>').val(nama[0]);
                            $('#<%=txtMiddleName.ClientID %>').val(nama[1]);
                            for (var a = 2; a < nama.length; a++) {
                                tempNama += nama[a];
                            }
                            $('#<%=txtFamilyName.ClientID %>').val(tempNama);
                        }
                    } else {
                        $('#<%=txtFamilyName.ClientID %>').val(obj.response.peserta.nama);
                    }
                    cboIdentityCardType.SetValue('X097^001');
                    var tglLahir = obj.response.peserta.tglLahir;
                    var tgl = tglLahir.split(" ");
                    var arrDate = tgl[0].split("-");
                    var strDate = arrDate[0] + arrDate[1] + arrDate[2]
                    var date = Methods.stringToDate(strDate);
                    $('#<%=txtDOB.ClientID %>').val(Methods.dateToDMY(date)).change();
                    var gender = '';
                    if (obj.response.peserta.sex == 'L') {
                        gender = '0003^M';
                    }
                    else {
                        gender = '0003^F';
                    }
                    cboGender.SetValue(gender);
                    var kodeInstansiBPJS = '040';
                    var SCRegistrationPayerBPJS = 'X004^500';
                    cboPayer.SetValue(SCRegistrationPayerBPJS);
                    onCboPayerValueChanged();
                    $('#<%=txtPayerCompanyCode.ClientID %>').val(kodeInstansiBPJS);
                    onPatientEntryTxtPayerCompanyCodeChanged(kodeInstansiBPJS);
                }
                else BPJSPEFailTrigger('BPJS-Response', obj.metaData.message);
            }
            else BPJSPEFailTrigger('BPJS-Bridging Gagal', resultInfo[2]);
        }
        catch (err) {
            showToast('BPJS-Bridging', 'Error Message : ' + err.Description);
            resetPEBPJSField();
        }
    }

    //#region BPJSSearch
    $("#btnSearchPesertaCtl").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNHSRegistrationNo.ClientID %>').val() == '')
            alert("Nomor Peserta harus diisi!");
        else {
            var noPeserta = $('#<%=txtNHSRegistrationNo.ClientID %>').val();
            var tglSEP = Methods.dateToYMD(new Date());
            try {
                if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                    BPJSService.getPeserta(noPeserta, tglSEP, function (result) {
                        GetPesertaHandler(result);
                    });
                } else {
                    BPJSService.getPesertaMedinfrasAPI(noPeserta, tglSEP, function (result) {
                        GetPesertaHandler(result);
                    });
                }
            }
            catch (err) {
                BPJSPEFailTrigger('BPJS-Bridging Gagal', err.Message);
            }
        }
    });

    function resetPEBPJSField() {
        $('#<%=txtNHSRegistrationNo.ClientID %>').val('');
        $('#<%=txtPENamaPeserta.ClientID %>').val('');
        $('#<%=txtPEJenisPeserta.ClientID %>').val('');
        $('#<%=txtPEKelas.ClientID %>').val('');
        $('#<%=txtPEPpkRujukan.ClientID %>').val('');
        $('#<%=txtIdentityCardNo.ClientID %>').val('');
        $('#<%=txtFirstName.ClientID %>').val('');
        $('#<%=txtMiddleName.ClientID %>').val('');
        $('#<%=txtFamilyName.ClientID %>').val('');
        cboIdentityCardType.SetText('');
        $('#<%=txtDOB.ClientID %>').val('');
        $('#<%=txtAgeInYear.ClientID %>').val('');
        $('#<%=txtAgeInMonth.ClientID %>').val('');
        $('#<%=txtAgeInDay.ClientID %>').val('');
        cboGender.SetText('');
        cboPayer.SetText('Pribadi');
        onCboPayerValueChanged();
        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
        onPatientEntryTxtPayerCompanyCodeChanged('');
        lblPEBPJSStatusINACTIVE.style.display = "none";
    }

    function BPJSPEFailTrigger(message1, message2) {
        showToast(message1, 'Error Message : ' + message2);
        resetPEBPJSField();
        lblPEBPJSStatusINACTIVE.style.display = "inline-block";
    }
    //#endregion

    //#region Inhealth
    $("#btnInhealthSearchPesertaCtl").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtInhealthParticipantNo.ClientID %>').val() == '')
            showToast('Warning', "Nomor Kartu Inhealth harus diisi!");
        else {
            $('#<%=hdnIsAdd.ClientID %>').val('0')
            var token = $('#<%=hdnTokenInhealth.ClientID %>').val();
            var kodeprovider = $('#<%=hdnKodeProviderInhealth.ClientID %>').val();
            var nokainhealth = $('#<%=txtInhealthParticipantNo.ClientID %>').val();
            var tglpelayanan = $('#<%=hdnTodayDate.ClientID %>').val();
            var jenispelayanan = '';
            if ($('#<%=hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT) {
                jenispelayanan = '2';
            }
            else {
                jenispelayanan = '1';
            }
            var poli = 'UGD';
            if (jenispelayanan != "0") {
                InhealthService.eligibilitaspeserta_API(nokainhealth, tglpelayanan, jenispelayanan, poli, function (result) {
                    GetPesertaHandlerInhealth(result);
                });
            }
            else {
                showToast('WARNING', 'Harap pilih jenis pelayanan pasien Inhealth');
            }
        }
    });

    function GetPesertaHandlerInhealth(result) {
        try {
            var resp = result.split('|');
            if (resp[0] == "1") {
                var obj = jQuery.parseJSON(resp[2]);
                var errorCode = obj.ERRORCODE;
                if (errorCode == '00') {
                    displayMessageBox('Status Eligibilitas Peserta : SUKSES', 'Cek Peserta dengan nama ' + obj.NMPST + ' (' + obj.NOKAPST + ') berhasil.');
                    $('#<%=txtInhealthParticipantNo.ClientID %>').val(obj.NOKAPST);
                    $('#<%=txtFamilyName.ClientID %>').val(obj.NMPST);
                    $('#<%=txtCardName.ClientID %>').val(obj.NMPST);
                    $('#<%=txtNHSRegistrationNo.ClientID %>').val(obj.NOKAPSTBPJS);
                    var year = obj.TGLLAHIR.substring(0, 4);
                    var month = obj.TGLLAHIR.substring(5, 7);
                    var day = obj.TGLLAHIR.substring(8, 10);
                    var dob = year + month + day;
                    var date = Methods.stringToDate(dob);
                    $('#<%=txtDOB.ClientID %>').val(Methods.dateToDMY(date)).change();

                    if (obj.JENISKELAMIN == "P") {
                        cboGender.SetValue(Constant.Gender.FEMALE);
                    }
                    else {
                        cboGender.SetValue(Constant.Gender.MALE);
                    }
                }
                else {
                    displayMessageBox('Status Eligibilitas Peserta : GAGAL', obj.ERRORDESC);
                    ResetDataPeserta();
                }
            }
            else {
                displayMessageBox('Status Eligibilitas Peserta : GAGAL', resp[1]);
            }
        }
        catch (err) {
            ResetDataPeserta();
        }
    }

    function ResetDataPeserta() {
        $('#<%=txtInhealthParticipantNo.ClientID %>').val('');
        $('#<%=txtFamilyName.ClientID %>').val('');
        $('#<%=txtCardName.ClientID %>').val('');
        $('#<%=txtNHSRegistrationNo.ClientID %>').val('');
        $('#<%=txtDOB.ClientID %>').val('');
    }
    //#endregion

    $("#btnGetIHSNumber").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtIdentityCardNo.ClientID %>').val() == '')
            alert("Nomor Identitas Pasien (NIK) harus diisi!");
        else {
            var NIK = $('#<%=txtIdentityCardNo.ClientID %>').val();
            try {
                IHSService.getIHSNumberByNIK(NIK, function (result) {
                    GetIHSDataHandler(result);
                });
            }
            catch (err) {
                displayErrorMessageBox("Integrasi SATUSEHAT", err.Message);
            }
        }
    });

    function GetIHSDataHandler(result) {
        try {
            var resultInfo = result.split('|');
            if (resultInfo[0] == "1") {
                $('#<%=txtIHSNumber.ClientID %>').val(resultInfo[1]);
            }
            else {
                $('#<%=txtIHSNumber.ClientID %>').val('');
                displayErrorMessageBox('Integrasi SatuSehat', resultInfo[2]);
            }
        }
        catch (err) {
            displayErrorMessageBox('Integrasi SATUSEHAT', 'Error Message : ' + err.Description);
        }
    }

    if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
        $('#<%=txtMRNPatientEntryCtl.ClientID %>').attr('readonly', 'readonly');
    }


    function onGetEntryPopupReturnValue() {
        var result = $('#<%=txtMRNPatientEntryCtl.ClientID %>').val();
        return result;
    }

    function onBeforeSaveRecord(param) {
        var dob = Methods.getDatePickerDate($('#<%=txtDOB.ClientID %>').val());
        var dobString = Methods.dateToString(dob);
        var datenow = ('<%:dateNow %>');
        var isBlokDoubleRM = $('#<%=hdnIsBlockDoublePatientData.ClientID %>').val();

        if (dobString == datenow) {
            showToast('Information', 'Usia pasien 0 tahun');
        }
        var mrn = $('#<%=hdnMRN.ClientID %>').val();
        var dateSelected = $('#<%=txtDOB.ClientID %>').val();
        var from = dateSelected.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);
        /// untuk ambil data
        $('#<%=hdnDOB.ClientID %>').val(from[2] + "-" + from[1] + "-" + from[0]);
        var filterExpression = "";
        var GCIdentityNoType = cboIdentityCardType.GetValue();
        var SSN = $('#<%=txtIdentityCardNo.ClientID %>').val();
        var IsExis = true;
        if (mrn == "") {
            var fullName = getFullNameText();
            var DOB = $('#<%=hdnDOB.ClientID %>').val();

            filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + DOB + "' AND GCGender = '" + cboGender.GetValue() + "' AND IsDeleted=0";
        }
        else {
            var fullName = getFullNameText();
            var DOB = $('#<%=hdnDOB.ClientID %>').val();

            filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + DOB + "' AND GCGender = '" + cboGender.GetValue() + "'  AND MRN <> '" + mrn + "' AND IsDeleted=0";
        }

        if (SSN != "") {
            filterExpression += " AND SSN ='" + SSN + "'";
        }

        Methods.getObject('GetvPatientList', filterExpression, function (result) {
            if (result != null) {
                if (isBlokDoubleRM == "1") {
                    messageDoublePatient = 'Ditemukan ada pasien dengan data yang sama :<br/>' +
                                                'No.RM : ' + result.MedicalNo + '<br/>Nama Lengkap : ' + result.PatientName + '<br/> No Kartu Identitas  : ' + result.SSN + '<br/>Tanggal Lahir : ' + Methods.getJSONDateValue(result.DateOfBirth) + '<br/>Jenis Kelamin : ' + result.Gender + '<br/>Alamat : ' + result.HomeAddress + '<br/>';
                    displayMessageBox('Failed', messageDoublePatient);
                    IsExis = false;
                    return IsExis;
                }
                else {
                    IsExis = true;
                    return IsExis;
                }
            }
        });

        return IsExis;
    }

    function getFullNameText() {
        var firstName = $('#<%=txtFirstName.ClientID %>').val();
        var middleName = $('#<%=txtMiddleName.ClientID %>').val();
        var familyName = $('#<%=txtFamilyName.ClientID %>').val();
        var cardName = "";

        if (firstName != "" && firstName != null) {
            if (middleName != "" && middleName != null) {
                cardName = firstName + " " + middleName + " " + familyName;
            } else {
                cardName = firstName + " " + familyName;
            }
        } else {
            cardName = familyName;
        }

        return cardName;
    }

    function getDataPatient() {
        var fullName = getFullNameText();
        var DOB = $('#<%=hdnDOB.ClientID %>').val();
        var IsExis = true;
        var filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + DOB + "' AND GCGender = '" + cboGender.GetValue() + "'";
        Methods.getObject('GetvPatientList', filterExpression, function (result) {
            if (result != null) {

                var messageDoubleRegistration = 'sudah ada pasien dengan data yang sama :<br/>' +
                                                'Nomor Rekamedis : ' + result.MedicalNo + '<br/>Nama Lengkap : ' + result.PatientName + '<br/>Nomor NIK :' + result.SSN + '<br/>Tanggal Lahir : ' + Methods.getJSONDateValue(result.DateOfBirth) + '<br/>Jenis Kelamin :' + result.Gender + '<br/>Alamat :' + result.HomeAddress + '<br/>' +
                                                'Apakah Ingin Melanjutkan Proses ?<br/>';

                showToastConfirmation(messageDoubleRegistration, function (confirm) {
                    IsExis = false;
                });

            }
        });

        return IsExis;
    }

    function onAfterSaveAddRecordEntryPopup(param) {
        if (typeof onAfterSaveGenerateMR == 'function') {
            onAfterSaveGenerateMR();
        }

        $('#hdnRightPanelContentCode').val('patientIdentity');
        return $('#<%=txtMRNPatientEntryCtl.ClientID %>').val(param);
    }

    function entityToControl(entity, entityAddress, entityOfficeAddress) {
        if (entity != null) {
            setEntryPopupIsAdd(false);

            $('#<%=hdnMRN.ClientID %>').val(entity.MRN);
            $('#<%=txtMRNPatientEntryCtl.ClientID %>').val(entity.MedicalNo);

            //guest 
            onBindGuestEdit();

            //#region Patient Data
            cboSalutation.SetValue(entity.GCSalutation);
            cboTitle.SetValue(entity.GCTitle);
            $('#<%=txtFirstName.ClientID %>').val(entity.FirstName);
            $('#<%=txtMiddleName.ClientID %>').val(entity.MiddleName);
            $('#<%=txtFamilyName.ClientID %>').val(entity.LastName);
            $('#<%=txtPreferredName.ClientID %>').val(entity.PreferredName);
            cboIdentityCardType.SetValue(entity.GCIdentityNoType);
            $('#<%=chkIsSSNTemporary.ClientID %>').prop('checked', entity.IsSSNTemporary);
            $('#<%=txtIdentityCardNo.ClientID %>').val(entity.SSN);
            $('#<%=txtFamilyCardNo.ClientID %>').val(entity.FamilyCardNo);
            $('#<%=txtNHSRegistrationNo.ClientID %>').val(entity.NHSRegistrationNo);
            $('#<%=txtInhealthParticipantNo.ClientID %>').val(entity.InhealthParticipantNo);
            $('#<%=txtIHSNumber.ClientID %>').val(entity.IHSNumber);
            $('#<%=txtEKlaimMedicalNo.ClientID %>').val(entity.EKlaimMedicalNo);
            $('#<%=txtSITBRegisterNo.ClientID %>').val(entity.SITBRegisterNo);
            $('#<%=txtCardName.ClientID %>').val(entity.CardName);
            $('#<%=txtName2.ClientID %>').val(entity.name2);
            cboSuffix.SetValue(entity.GCSuffix);
            cboGender.SetValue(entity.GCGender);
            if (entity.GCEthnic != '')
                $('#<%=txtEthnicCode.ClientID %>').val(entity.GCEthnic.split('^')[1]);
            else
                $('#<%=txtEthnicCode.ClientID %>').val('');
            $('#<%=txtEthnicName.ClientID %>').val(entity.Ethnic);
            $('#<%=txtBirthPlace.ClientID %>').val(entity.CityOfBirth);
            cboBloodType.SetValue(entity.GCBloodType);
            cboBloodRhesus.SetValue(entity.BloodRhesus);
            $('#<%=txtDOB.ClientID %>').val(Methods.getJSONDateValue(entity.DateOfBirth));
            $('#<%=txtAgeInYear.ClientID %>').val(entity.AgeInYear);
            $('#<%=txtAgeInMonth.ClientID %>').val(entity.AgeInMonth);
            $('#<%=txtAgeInDay.ClientID %>').val(entity.AgeInDay);
            //#endregion

            //#region Data Keluarga
            $('#<%=txtFatherName.ClientID %>').val(entity.FatherName);
            $('#<%=txtMotherName.ClientID %>').val(entity.MotherName);
            $('#<%=txtSpouseName.ClientID %>').val(entity.SpouseName);
            var filterExpression = "MRN = '" + entity.MRN + "' AND GCFamilyRelation = '" + '0063^CHD' + "' AND IsDeleted = 0";
            Methods.getObject('GetPatientFamilyList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtChildName.ClientID %>').val(result.Name);
                }
                else {
                    $('#<%=txtChildName.ClientID %>').val('');
                }
            });
            //#endregion

            //#region Patient Address
            $('#<%=txtAddress.ClientID %>').val(entity.StreetName);
            $('#<%=txtRTData.ClientID %>').val(entity.RT);
            $('#<%=txtRWData.ClientID %>').val(entity.RW);
            $('#<%=txtCounty.ClientID %>').val(entity.County);
            $('#<%=txtDistrict.ClientID %>').val(entity.District);
            $('#<%=txtCity.ClientID %>').val(entity.City);
            if (entity.GCState != '')
                $('#<%=txtProvinceCode.ClientID %>').val(entity.GCState.split('^')[1]);
            else
                $('#<%=txtProvinceCode.ClientID %>').val('');
            $('#<%=txtProvinceName.ClientID %>').val(entity.State);
            $('#<%=hdnZipCode.ClientID %>').val(entity.ZipCodeID);
            $('#<%=txtZipCode.ClientID %>').val(entity.ZipCode);
            //#endregion

            //#region Patient Other Address
            $('#<%=txtAddressDomicile.ClientID %>').val(entity.OtherStreetName);
            $('#<%=txtRTDomicileData.ClientID %>').val(entity.OtherRT);
            $('#<%=txtRWDomicileData.ClientID %>').val(entity.OtherRW);
            $('#<%=txtCountyDomicile.ClientID %>').val(entity.OtherCounty);
            $('#<%=txtDistrictDomicile.ClientID %>').val(entity.OtherDistrict);
            $('#<%=txtCityDomicile.ClientID %>').val(entity.OtherCity);
            if (entity.GCState != '')
                $('#<%=txtProvinceDomicileCode.ClientID %>').val(entity.OtherGCState.split('^')[1]);
            else
                $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
            $('#<%=txtProvinceDomicileName.ClientID %>').val(entity.OtherState);
            $('#<%=hdnZipCodeDomicile.ClientID %>').val(entity.OtherZipCodeID);
            $('#<%=txtZipCodeDomicile.ClientID %>').val(entity.OtherZipCode);
            //#endregion

            //#region Patient Contact
            $('#<%=txtTelephoneNo1.ClientID %>').val(entity.PhoneNo1);
            $('#<%=txtTelephoneNo2.ClientID %>').val(entity.PhoneNo2);
            $('#<%=txtMobilePhone1.ClientID %>').val(entity.MobilePhoneNo1);
            $('#<%=txtMobilePhone2.ClientID %>').val(entity.MobilePhoneNo2);
            $('#<%=txtEmail.ClientID %>').val(entity.EmailAddress);
            //#endregion

            //#region Additonal Information
            if (entity.GCEducation != '') {
                $('#<%=txtEducationCode.ClientID %>').val(entity.GCEducation.split('^')[1]);
                $('#<%=txtEducationName.ClientID %>').val(entity.Education);
            }
            else {
                $('#<%=txtEducationCode.ClientID %>').val('');
                $('#<%=txtEducationName.ClientID %>').val('');
            }
            if (entity.GCReligion != '') {
                $('#<%=txtReligionCode.ClientID %>').val(entity.GCReligion.split('^')[1]);
                $('#<%=txtReligionName.ClientID %>').val(entity.Religion);
            }
            else {
                $('#<%=txtReligionCode.ClientID %>').val('');
                $('#<%=txtReligionName.ClientID %>').val('');
            }
            if (entity.GCMaritalStatus != '') {
                $('#<%=txtMaritalStatusCode.ClientID %>').val(entity.GCMaritalStatus.split('^')[1]);
                $('#<%=txtMaritalStatusName.ClientID %>').val(entity.MaritalStatus);
            }
            else {
                $('#<%=txtMaritalStatusCode.ClientID %>').val('');
                $('#<%=txtMaritalStatusName.ClientID %>').val('');
            }
            if (entity.GCNationality != '') {
                $('#<%=txtNationalityCode.ClientID %>').val(entity.GCNationality.split('^')[1]);
                $('#<%=txtNationalityName.ClientID %>').val(entity.Nationality);
            }
            else {
                $('#<%=txtNationalityCode.ClientID %>').val('');
                $('#<%=txtNationalityName.ClientID %>').val('');
            }
            if (entity.GCPatientCategory != '') {
                $('#<%=txtPatientCategoryCode.ClientID %>').val(entity.GCPatientCategory.split('^')[1]);
                $('#<%=txtPatientCategoryName.ClientID %>').val(entity.PatientCategory);
            }
            else {
                $('#<%=txtPatientCategoryCode.ClientID %>').val('');
                $('#<%=txtPatientCategoryName.ClientID %>').val('');
            }

            if (entity.GCCommunicationRestriction != '') {
                $('#<%=txtlanguageCode.ClientID %>').val(entity.GCCommunicationRestriction.split('^')[1]);
                $('#<%=txtlanguageName.ClientID %>').val(entity.Communication);
            }
            else {
                $('#<%=txtlanguageCode.ClientID %>').val('');
                $('#<%=txtlanguageName.ClientID %>').val('');
            }
            $('#<%=hdnEmployeeID.ClientID %>').val(entity.EmployeeID);
            $('#<%=txtEmployeeCode.ClientID %>').val(entity.EmployeeCode);
            $('#<%=txtEmployeeName.ClientID %>').val(entity.EmployeeName);
            //#endregion

            //#region Patient Payer
            $('#<%=hdnPayerID.ClientID %>').val(entity.BusinessPartnerID);
            $('#<%=txtPayerCompanyCode.ClientID %>').val(entity.BusinessPartnerCode);
            $('#<%=txtPayerCompanyName.ClientID %>').val(entity.BusinessPartnerName);
            //#endregion

            //#region Patient Job
            if (entity.GCOccupation != '') {
                $('#<%=txtPatientJobCode.ClientID %>').val(entity.GCOccupation.split('^')[1]);
                $('#<%=txtPatientJobName.ClientID %>').val(entity.Occupation);
            }
            else {
                $('#<%=txtPatientJobCode.ClientID %>').val('');
                $('#<%=txtPatientJobName.ClientID %>').val('');
            }
            $('#<%=txtPatientJobOffice.ClientID %>').val(entity.Company);
            $('#<%=txtPatientJobOfficeAddress.ClientID %>').val(entity.OfficeStreetName);
            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(entity.OfficeCounty);
            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(entity.OfficeDistrict);
            $('#<%=txtPatientJobOfficeCity.ClientID %>').val(entity.OfficeCity);
            if (entity.OfficeGCState != '')
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(entity.OfficeGCState.split('^')[1]);
            else
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(entity.OfficeState);
            $('#<%=hdnOfficeZipCode.ClientID %>').val(entity.OfficeZipCodeID);
            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(entity.OfficeZipCode);
            $('#<%=txtRTOfficeData.ClientID %>').val(entity.OfficeRT);
            $('#<%=txtRWOfficeData.ClientID %>').val(entity.OfficeRW);
            $('#<%=txtPatientJobOfficeTelephone.ClientID %>').val(entity.OfficePhoneNo1);
            $('#<%=txtPatientJobOfficeEmail.ClientID %>').val(entity.OfficeEmail);
            $('#<%=txtCorporateAccountNoCtl.ClientID %>').val(entity.CorporateAccountNo);
            $('#<%=txtCorporateAccountNameCtl.ClientID %>').val(entity.CorporateAccountName);
            $('#<%=txtCorporateAccountDepartmentCtl.ClientID %>').val(entity.CorporateAccountDepartment);
            //#endregion

            //#region Patient Status
            $('#<%=chkIsAlive.ClientID %>').prop('checked', entity.IsAlive);
            $('#<%=chkIsBlackList.ClientID %>').prop('checked', entity.IsBlacklist);
            cboGCBlacklistReason.SetValue(entity.GCBlacklistReason);
            $('#<%=txtOtherBlackListReason.ClientID %>').show();
            $('#<%=txtOtherBlackListReason.ClientID %>').val(entity.OtherBlacklistReason);
            $('#<%=chkIsDonor.ClientID %>').prop('checked', entity.IsDonor);
            $('#<%=chkIsG6PD.ClientID %>').prop('checked', entity.IsG6PD);
            $('#<%=chkIsHasAllergy.ClientID %>').prop('checked', entity.IsHasAllergy);
            $('#<%=chkIsIlliteracy.ClientID %>').prop('checked', entity.IsIlliteracy);
            $('#<%=chkIsSmoking.ClientID %>').prop('checked', entity.IsSmoking);
            $('#<%=chkIsCataract.ClientID %>').prop('checked', entity.IsCataract);
            $('#<%=chkIsHasPhysicalLimitation.ClientID %>').prop('checked', entity.IsHasPhysicalLimitation);
            cboPhysicalLimitation.SetValue(entity.GCPhysicalLimitationType);
            $('#<%=chkIsGeriatricPatient.ClientID %>').prop('checked', entity.IsGeriatricPatient);
            $('#<%=chkIsVIP.ClientID %>').prop('checked', entity.IsVIP);
            cboVIPGroup.SetValue(entity.GCVIPGroup);
            $('#<%=chkIsHasCommunicationRestriction.ClientID %>').prop('checked', entity.IsHasCommunicationRestriction);
            cboCommunication.SetValue(entity.GCCommunicationRestriction);
            //#endregion

            //#region Other Information
            $('#<%=txtNotes.ClientID %>').val(entity.Notes);
            //#endregion

            //#region Merge MRN Information
            var filterExpression = "ToMRN = '" + entity.MRN + "'";
            Methods.getObject('GetvMrnmergehistoryList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=trInformationMergeMRN.ClientID %>').removeAttr('style');
                    $('#<%=txtMRNPatientEntryCtlMerge.ClientID %>').val(result.FromMedicalNo);
                }
                else {
                    $('#<%=trInformationMergeMRN.ClientID %>').attr('style', 'display:none');
                    $('#<%=txtMRNPatientEntryCtlMerge.ClientID %>').val('');
                }
            });

            //#endregion

        }
        else {
            setEntryPopupIsAdd(true);
            $('#<%=hdnMRN.ClientID %>').val('');
            $('#<%=txtMRNPatientEntryCtl.ClientID %>').val('');

            //#region Patient Data
            cboSalutation.SetValue('');
            cboTitle.SetValue('');
            $('#<%=txtFirstName.ClientID %>').val('');
            $('#<%=txtMiddleName.ClientID %>').val('');
            $('#<%=txtFamilyName.ClientID %>').val('');
            $('#<%=txtPreferredName.ClientID %>').val('');
            cboIdentityCardType.SetValue('');
            $('#<%=txtIdentityCardNo.ClientID %>').val('');
            $('#<%=txtFamilyCardNo.ClientID %>').val('');
            $('#<%=txtNHSRegistrationNo.ClientID %>').val('');
            $('#<%=txtInhealthParticipantNo.ClientID %>').val('');
            $('#<%=txtIHSNumber.ClientID %>').val('');
            $('#<%=txtCardName.ClientID %>').val('');
            $('#<%=txtName2.ClientID %>').val('');
            cboSuffix.SetValue('');
            cboGender.SetValue('');
            $('#<%=txtEthnicCode.ClientID %>').val('');
            $('#<%=txtEthnicName.ClientID %>').val('');
            $('#<%=txtBirthPlace.ClientID %>').val('');
            cboBloodType.SetValue('');
            cboBloodRhesus.SetValue('');
            $('#<%=txtDOB.ClientID %>').val('');
            $('#<%=txtAgeInYear.ClientID %>').val('');
            $('#<%=txtAgeInMonth.ClientID %>').val('');
            $('#<%=txtAgeInDay.ClientID %>').val('');
            //#endregion

            //#region Data Keluarga
            $('#<%=txtFatherName.ClientID %>').val('');
            $('#<%=txtMotherName.ClientID %>').val('');
            $('#<%=txtSpouseName.ClientID %>').val('');
            $('#<%=txtChildName.ClientID %>').val('');
            //#endregion

            //#region Patient Address
            $('#<%=txtAddress.ClientID %>').val('');
            $('#<%=txtRTData.ClientID %>').val('');
            $('#<%=txtRWData.ClientID %>').val('');
            $('#<%=txtCounty.ClientID %>').val('');
            $('#<%=txtDistrict.ClientID %>').val('');
            $('#<%=txtCity.ClientID %>').val('');
            $('#<%=txtProvinceCode.ClientID %>').val('');
            $('#<%=txtProvinceName.ClientID %>').val('');
            $('#<%=hdnZipCode.ClientID %>').val('');
            $('#<%=txtZipCode.ClientID %>').val('');
            //#endregion

            //#region Patient Other Address
            $('#<%=txtAddressDomicile.ClientID %>').val('');
            $('#<%=txtRTDomicileData.ClientID %>').val('');
            $('#<%=txtRWDomicileData.ClientID %>').val('');
            $('#<%=txtCountyDomicile.ClientID %>').val('');
            $('#<%=txtDistrictDomicile.ClientID %>').val('');
            $('#<%=txtCityDomicile.ClientID %>').val('');
            $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
            $('#<%=txtProvinceDomicileName.ClientID %>').val('');
            $('#<%=hdnZipCodeDomicile.ClientID %>').val('');
            $('#<%=txtZipCodeDomicile.ClientID %>').val('');
            //#endregion

            //#region Patient Contact
            $('#<%=txtTelephoneNo1.ClientID %>').val('');
            $('#<%=txtTelephoneNo2.ClientID %>').val('');
            $('#<%=txtMobilePhone1.ClientID %>').val('');
            $('#<%=txtMobilePhone2.ClientID %>').val('');
            $('#<%=txtEmail.ClientID %>').val('');
            //#endregion

            //#region Additonal Information
            $('#<%=txtEducationCode.ClientID %>').val('');
            $('#<%=txtEducationName.ClientID %>').val('');
            $('#<%=txtReligionCode.ClientID %>').val('');
            $('#<%=txtReligionName.ClientID %>').val('');
            $('#<%=txtMaritalStatusCode.ClientID %>').val('');
            $('#<%=txtMaritalStatusName.ClientID %>').val('');
            $('#<%=txtNationalityCode.ClientID %>').val('');
            $('#<%=txtNationalityName.ClientID %>').val('');
            $('#<%=txtPatientCategoryCode.ClientID %>').val('');
            $('#<%=txtPatientCategoryName.ClientID %>').val('');
            $('#<%=hdnEmployeeID.ClientID %>').val('');
            $('#<%=txtEmployeeCode.ClientID %>').val('');
            $('#<%=txtEmployeeName.ClientID %>').val('');
            $('#<%=txtlanguageCode.ClientID %>').val('');
            $('#<%=txtlanguageName.ClientID %>').val('');
            //#endregion

            //#region Patient Payer
            $('#<%=hdnPayerID.ClientID %>').val('');
            $('#<%=txtPayerCompanyCode.ClientID %>').val('');
            $('#<%=txtPayerCompanyName.ClientID %>').val('');
            //#endregion

            //#region Patient Job
            $('#<%=txtPatientJobCode.ClientID %>').val('');
            $('#<%=txtPatientJobName.ClientID %>').val('');
            $('#<%=txtPatientJobOffice.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeAddress.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
            $('#<%=hdnOfficeZipCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
            $('#<%=txtRTOfficeData.ClientID %>').val('');
            $('#<%=txtRWOfficeData.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeTelephone.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeEmail.ClientID %>').val('');
            $('#<%=txtCorporateAccountNoCtl.ClientID %>').val('');
            $('#<%=txtCorporateAccountNameCtl.ClientID %>').val('');
            $('#<%=txtCorporateAccountDepartmentCtl.ClientID %>').val('');
            //#endregion

            //#region Patient Status
            $('#<%=chkIsAlive.ClientID %>').prop('checked', true);
            $('#<%=chkIsBlackList.ClientID %>').prop('checked', false);
            cboGCBlacklistReason.SetValue('');
            $('#<%=txtOtherBlackListReason.ClientID %>').hide();
            $('#<%=txtOtherBlackListReason.ClientID %>').val('');
            $('#<%=chkIsDonor.ClientID %>').prop('checked', false);
            $('#<%=chkIsG6PD.ClientID %>').prop('checked', false);
            $('#<%=chkIsHasAllergy.ClientID %>').prop('checked', false);
            $('#<%=chkIsIlliteracy.ClientID %>').prop('checked', false);
            $('#<%=chkIsSmoking.ClientID %>').prop('checked', false);
            $('#<%=chkIsCataract.ClientID %>').prop('checked', false);
            $('#<%=chkIsHasPhysicalLimitation.ClientID %>').prop('checked', false);
            cboPhysicalLimitation.SetValue('');
            $('#<%=chkIsGeriatricPatient.ClientID %>').prop('checked', false);
            $('#<%=chkIsVIP.ClientID %>').prop('checked', false);
            cboVIPGroup.SetValue('');
            $('#<%=chkIsHasCommunicationRestriction.ClientID %>').prop('checked', false);
            cboCommunication.SetValue('');
            //#endregion

            //#region Other Information
            $('#<%=txtNotes.ClientID %>').val('');
            //#endregion
        }
    }

    $('#<%=chkIsSSNTemporary.ClientID %>').die('change');
    $('#<%=chkIsSSNTemporary.ClientID %>').live('change', function () {
        if ($('#<%=chkIsSSNTemporary.ClientID %>').is(':checked')) {
            var oSSN = $('#<%=txtIdentityCardNo.ClientID %>').val();

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

            $('#<%=txtIdentityCardNo.ClientID %>').val(newTempSSN);
            $('#<%=txtIdentityCardNo.ClientID %>').attr('disabled', 'disabled');
        } else {
            $('#<%=txtIdentityCardNo.ClientID %>').removeAttr('disabled');
        }
    });

    //#region No RM
    $('#<%=lblMRN.ClientID %>.lblLink').click(function () {
        if ($('#<%:chkIsUsedReused.ClientID %>').is(':checked')) {
            openSearchDialog('mergedpatient', '', function (value) {
                $('#<%=txtMRNPatientEntryCtl.ClientID %>').val(value);
                onPatientEntrytxtMRNPatientEntryCtlChanged(value);
            });
        }
        else {
            openSearchDialog('patient', '', function (value) {
                $('#<%=txtMRNPatientEntryCtl.ClientID %>').val(value);
                onPatientEntrytxtMRNPatientEntryCtlChanged(value);
            });
        }
    });
    $('#<%=txtMRNPatientEntryCtl.ClientID %>').change(function () {
        onPatientEntrytxtMRNPatientEntryCtlChanged($(this).val());
    });
    function onPatientEntrytxtMRNPatientEntryCtlChanged(value) {
        var filterExpression = "MedicalNo = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvPatientList', filterExpression, function (entity) {
            entityToControl(entity);
        });
    }
    //#endregion

    //#region Patient Name
    $('#<%=txtFirstName.ClientID %>').change(function () {
        onPatientEntryTxtNameChanged($(this).val());
    });
    $('#<%=txtMiddleName.ClientID %>').change(function () {
        onPatientEntryTxtNameChanged($(this).val());
    });
    $('#<%=txtFamilyName.ClientID %>').change(function () {
        onPatientEntryTxtNameChanged($(this).val());
    });
    function onPatientEntryTxtNameChanged(value) {
        var firstName = $('#<%=txtFirstName.ClientID %>').val();
        var middleName = $('#<%=txtMiddleName.ClientID %>').val();
        var familyName = $('#<%=txtFamilyName.ClientID %>').val();
        var cardName = "";

        if (firstName != "" && firstName != null) {
            if (middleName != "" && middleName != null) {
                cardName = firstName + " " + middleName + " " + familyName;
            } else {
                cardName = firstName + " " + familyName;
            }
        } else {
            cardName = familyName;
        }

        $('#<%=txtCardName.ClientID %>').val(cardName);
    }
    //#endregion

    //#region Employee
    function getPatientEntryEmployeeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblPatientEntryEmployee.lblLink').die('click');
    $('#lblPatientEntryEmployee.lblLink').live('click', function () {
        openSearchDialog('employee', getPatientEntryEmployeeFilterExpression(), function (value) {
            $('#<%=txtEmployeeCode.ClientID %>').val(value);
            onPatientEntryTxtEmployeeCodeChanged(value);
        });
    });

    $('#<%=txtEmployeeCode.ClientID %>').change(function () {
        onPatientEntryTxtEmployeeCodeChanged($(this).val());
    });

    function onPatientEntryTxtEmployeeCodeChanged(value) {
        var filterExpression = getPatientEntryEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
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

    //#region Province
    function onGetSCProvinceFilterExpression() {
        var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
        return filterExpression;
    }

    $('#lblProvince.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
            $('#<%=txtProvinceCode.ClientID %>').val(value);
            onTxtProvinceCodeChanged(value);
        });
    });

    $('#<%=txtProvinceCode.ClientID %>').change(function () {
        onTxtProvinceCodeChanged($(this).val());
    });

    function onTxtProvinceCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtProvinceName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtProvinceCode.ClientID %>').val('');
                $('#<%=txtProvinceName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Office City
    $('#lblPatientJobOfficeCity.lblLink').click(function () {
        openSearchDialog('city', '', function (value) {
            $('#<%=txtPatientJobOfficeCity.ClientID %>').val(value);
        });
    });
    //#endregion

    //#region City
    $('#lblCity.lblLink').click(function () {
        openSearchDialog('city', '', function (value) {
            $('#<%=txtCity.ClientID %>').val(value);
        });
    });
    //#endregion

    //#region Domicile City
    $('#lblCityDomicile.lblLink').click(function () {
        openSearchDialog('city', '', function (value) {
            $('#<%=txtCityDomicile.ClientID %>').val(value);
        });
    });
    //#endregion

    //#region Office Province
    $('#lblOfficeProvince.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(value);
            onTxtOfficeProvinceCodeChanged(value);
        });
    });

    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').change(function () {
        onTxtOfficeProvinceCodeChanged($(this).val());
    });

    function onTxtOfficeProvinceCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Domicile Province

    $('#lblDomicileProvince.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
            $('#<%=txtProvinceDomicileCode.ClientID %>').val(value);
            onTxtProvinceDomicileCodeChanged(value);
        });
    });

    $('#<%=txtProvinceDomicileCode.ClientID %>').change(function () {
        onTxtProvinceDomicileCodeChanged($(this).val());
    });

    function onTxtProvinceDomicileCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtProvinceDomicileName.ClientID %>').val(result.StandardCodeName);
            else {
                $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
                $('#<%=txtProvinceDomicileName.ClientID %>').val('');
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

    $('#<%=txtZipCode.ClientID %>').change(function () {
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
            $('#<%=txtCity.ClientID %>').val('');
            $('#<%=txtCounty.ClientID %>').val('');
            $('#<%=txtDistrict.ClientID %>').val('');
            $('#<%=txtProvinceCode.ClientID %>').val('');
            $('#<%=txtProvinceName.ClientID %>').val('');
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
            $('#<%=txtCity.ClientID %>').val('');
            $('#<%=txtCounty.ClientID %>').val('');
            $('#<%=txtDistrict.ClientID %>').val('');
            $('#<%=txtProvinceCode.ClientID %>').val('');
            $('#<%=txtProvinceName.ClientID %>').val('');
        }
    }
    //#endregion

    $('#<%=chkCopyKTP.ClientID %>').live('change', function () {
        if ($(this.checked)) {
            var address = $('#<%=txtAddress.ClientID %>').val();
            var zipCodeHdn = $('#<%=hdnZipCode.ClientID %>').val();
            var zipCode = $('#<%=txtZipCode.ClientID %>').val();
            var rt = $('#<%=txtRTData.ClientID %>').val();
            var rw = $('#<%=txtRWData.ClientID %>').val();
            var city = $('#<%=txtCity.ClientID %>').val();
            var county = $('#<%=txtCounty.ClientID %>').val();
            var district = $('#<%=txtDistrict.ClientID %>').val();
            var provinceCode = $('#<%=txtProvinceCode.ClientID %>').val();
            var provinceName = $('#<%=txtProvinceName.ClientID %>').val();

            $('#<%=txtAddressDomicile.ClientID %>').val(address);
            $('#<%=hdnZipCodeDomicile.ClientID %>').val(zipCodeHdn);
            $('#<%=txtZipCodeDomicile.ClientID %>').val(zipCode);
            $('#<%=txtRTDomicileData.ClientID %>').val(rt);
            $('#<%=txtRWDomicileData.ClientID %>').val(rw);
            $('#<%=txtCityDomicile.ClientID %>').val(city);
            $('#<%=txtCountyDomicile.ClientID %>').val(county);
            $('#<%=txtDistrictDomicile.ClientID %>').val(district);
            $('#<%=txtProvinceDomicileCode.ClientID %>').val(provinceCode);
            $('#<%=txtProvinceDomicileName.ClientID %>').val(provinceName);
        }
    });

    //#region Domicile Zip Code
    $('#<%=lblZipCodeDomicile.ClientID %>').live('click', function (evt) {
        openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
            onTxtZipCodeDomicileChanged(value);
        });
    });

    $('#<%=txtZipCodeDomicile.ClientID %>').change(function () {
        onTxtZipCodeDomicileChangedValue($(this).val());
    });

    function onTxtZipCodeDomicileChanged(value) {
        if (value != '') {
            var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeDomicile.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeDomicile.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityDomicile.ClientID %>').val(result.City);
                    $('#<%=txtCountyDomicile.ClientID %>').val(result.County);
                    $('#<%=txtDistrictDomicile.ClientID %>').val(result.District);
                    $('#<%=txtProvinceDomicileCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceDomicileName.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeDomicile.ClientID %>').val('');
                    $('#<%=txtZipCodeDomicile.ClientID %>').val('');
                    $('#<%=txtCityDomicile.ClientID %>').val('');
                    $('#<%=txtCountyDomicile.ClientID %>').val('');
                    $('#<%=txtDistrictDomicile.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileName.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeDomicile.ClientID %>').val('');
            $('#<%=txtZipCodeDomicile.ClientID %>').val('');
            $('#<%=txtCityDomicile.ClientID %>').val('');
            $('#<%=txtCountyDomicile.ClientID %>').val('');
            $('#<%=txtDistrictDomicile.ClientID %>').val('');
            $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
            $('#<%=txtProvinceDomicileName.ClientID %>').val('');
        }
    }

    function onTxtZipCodeDomicileChangedValue(value) {
        if (value != '') {
            var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnZipCodeDomicile.ClientID %>').val(result.ID);
                    $('#<%=txtZipCodeDomicile.ClientID %>').val(result.ZipCode);
                    $('#<%=txtCityDomicile.ClientID %>').val(result.City);
                    $('#<%=txtCountyDomicile.ClientID %>').val(result.County);
                    $('#<%=txtDistrictDomicile.ClientID %>').val(result.District);
                    $('#<%=txtProvinceDomicileCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtProvinceDomicileName.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnZipCodeDomicile.ClientID %>').val('');
                    $('#<%=txtZipCodeDomicile.ClientID %>').val('');
                    $('#<%=txtCityDomicile.ClientID %>').val('');
                    $('#<%=txtCountyDomicile.ClientID %>').val('');
                    $('#<%=txtDistrictDomicile.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
                    $('#<%=txtProvinceDomicileName.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnZipCodeDomicile.ClientID %>').val('');
            $('#<%=txtZipCodeDomicile.ClientID %>').val('');
            $('#<%=txtCityDomicile.ClientID %>').val('');
            $('#<%=txtCountyDomicile.ClientID %>').val('');
            $('#<%=txtDistrictDomicile.ClientID %>').val('');
            $('#<%=txtProvinceDomicileCode.ClientID %>').val('');
            $('#<%=txtProvinceDomicileName.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Office Zip Code
    $('#<%=txtPatientJobOfficeCounty.ClientID %>').change(function () {
        var filterExpression = "County = '" + $('#<%=txtPatientJobOfficeCounty.ClientID %>').val() + "' AND IsDeleted = 0";
        Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOfficeZipCode.ClientID %>').val(result.ID);
                $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(result.ZipCode);
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(result.County);
                $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(result.District);
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.Province);
            }
            else {
                $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
            }
        });
    });

    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').change(function () {
        var filterExpression = "District = '" + $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val() + "' AND IsDeleted = 0";
        Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOfficeZipCode.ClientID %>').val(result.ID);
                $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(result.ZipCode);
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(result.County);
                $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(result.District);
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.Province);
            }
            else {
                $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
            }
        });
    });

    $('#lblOfficeZipCode.lblLink').click(function () {
        openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
            onTxtOfficeZipCodeChanged(value);
        });
    });

    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').change(function () {
        onTxtOfficeZipCodeChangedValue($(this).val());
    });

    function onTxtOfficeZipCodeChanged(value) {
        if (value != '') {
            var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnOfficeZipCode.ClientID %>').val(result.ID);
                    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(result.ZipCode);
                    $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                    $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(result.County);
                    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(result.District);
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnOfficeZipCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
        }
    }

    function onTxtOfficeZipCodeChangedValue(value) {
        if (value != '') {
            var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnOfficeZipCode.ClientID %>').val(result.ID);
                    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(result.ZipCode);
                    $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                    $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(result.County);
                    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(result.District);
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                    $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.Province);
                }
                else {
                    $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnOfficeZipCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
        }
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

    //#region Patient Category
    function onGetSCPatientCategoryFilterExpression() {
        var filterExpression = "<%:OnGetSCPatientCategoryFilterExpression() %>";
        return filterExpression;
    }

    $('#lblPatientCategory.lblLink').click(function () {
        openSearchDialog('stdcode', onGetSCPatientCategoryFilterExpression(), function (value) {
            $('#<%=txtPatientCategoryCode.ClientID %>').val(value);
            onTxtPatientCategoryCodeChanged(value);
        });
    });

    $('#<%=txtPatientCategoryCode.ClientID %>').change(function () {
        onTxtPatientCategoryCodeChanged($(this).val());
    });

    function onTxtPatientCategoryCodeChanged(value) {
        var filterExpression = onGetSCPatientCategoryFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtPatientCategoryName.ClientID %>').val(result.StandardCodeName);

                if (result.TagProperty != "") {
                    var tempParam = result.TagProperty.split('|');
                    var isEmployee = tempParam[0].substring(tempParam[0].length - 1, tempParam[0].length);
                    if (isEmployee == "1") {
                        $('#<%=txtEmployeeCode.ClientID %>').removeAttr('readonly');
                        $('#lblPatientEntryEmployee').attr('class', 'lblLink');
                        $('#<%=hdnEmployeeID.ClientID %>').val('');
                        $('#<%=txtEmployeeCode.ClientID %>').val('');
                        $('#<%=txtEmployeeName.ClientID %>').val('');
                    } else {
                        $('#lblPatientEntryEmployee').attr('class', 'lblDisabled');
                        $('#<%=hdnEmployeeID.ClientID %>').val('');
                        $('#<%=txtEmployeeCode.ClientID %>').val('');
                        $('#<%=txtEmployeeName.ClientID %>').val('');
                    }
                } else {
                    $('#lblPatientEntryEmployee').attr('class', 'lblDisabled');
                    $('#<%=hdnEmployeeID.ClientID %>').val('');
                    $('#<%=txtEmployeeCode.ClientID %>').val('');
                    $('#<%=txtEmployeeName.ClientID %>').val('');
                }
            }
            else {
                $('#<%=txtPatientCategoryCode.ClientID %>').val('');
                $('#<%=txtPatientCategoryName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    $('#lblPayerCompany.lblLink').click(function () {
        openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
            $('#<%=txtPayerCompanyCode.ClientID %>').val(value);
            onPatientEntryTxtPayerCompanyCodeChanged(value);
        });
    });

    $('#<%=txtPayerCompanyCode.ClientID %>').change(function () {
        onPatientEntryTxtPayerCompanyCodeChanged($(this).val());
    });

    function onPatientEntryTxtPayerCompanyCodeChanged(value) {
        var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
            }
            else {
                $('#<%=hdnPayerID.ClientID %>').val('');
                $('#<%=txtPayerCompanyCode.ClientID %>').val('');
                $('#<%=txtPayerCompanyName.ClientID %>').val('');
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

    //#region DOB
    $('#<%=txtDOB.ClientID %>').change(function () {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
        var dateSelected = $('#<%=txtDOB.ClientID %>').val();

        var from = dateSelected.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);
        //untuk ambil data
        $('#<%=hdnDOB.ClientID %>').val(from[2] + "-" + from[1] + "-" + from[0]);
        var to = dateToday.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        if (f > t) {
            $('#<%=txtDOB.ClientID %>').val(dateToday);
        }
        var age = Methods.getAgeFromDatePickerFormat($(this).val());
        $('#<%=txtAgeInYear.ClientID %>').val(age.years);
        $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
        $('#<%=txtAgeInDay.ClientID %>').val(age.days);
        oncboSalutationValueChanged();

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
    //#endregion

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        $('#<%=hdnPayerID.ClientID %>').val('');
        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
        $('#<%=txtPayerCompanyName.ClientID %>').val('');
    }

    function setTblPayerCompanyVisibility() {
        if (cboPayer.GetValue() == Constant.CustomerType.PERSONAL)
            $('#<%=trPayerCompany.ClientID %>').hide();
        else
            $('#<%=trPayerCompany.ClientID %>').show();
    }

    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $('#btnUploadFile').click(function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    $('#<%:chkIsVIP.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=tdVIPGroup.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=tdVIPGroup.ClientID %>').attr('style', 'display:none');
            $('#<%=trVIPGroupOther.ClientID %>').attr('style', 'display:none');
        }
    });

    $('#<%:chkIsHasPhysicalLimitation.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=tdPhysicalLimitation.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=tdPhysicalLimitation.ClientID %>').attr('style', 'display:none');
        }
    });

    $('#<%:chkIsHasCommunicationRestriction.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=tdCommunication.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=tdCommunication.ClientID %>').attr('style', 'display:none');
        }
    });

    function oncboVIPGroupChanged() {
        if (cboVIPGroup.GetValue() != 'X289^999') {
            $('#<%=trVIPGroupOther.ClientID %>').attr('style', 'display:none');
            $('#<%=txtOtherVIPGroup.ClientID %>').val('');
        }
        else {
            $('#<%=trVIPGroupOther.ClientID %>').removeAttr('style');
        }
    }

    $('#<%:chkIsBlackList.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=tdGCBlacklistReason.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=tdGCBlacklistReason.ClientID %>').attr('style', 'display:none');
            $('#<%=trOtherBlackListReason.ClientID %>').attr('style', 'display:none');
        }
    });

    function onCboGCBlacklistReasonValueChanged() {
        var defaultOther = Constant.PatientBlackListReason.OTHER;
        if (cboGCBlacklistReason.GetValue() == defaultOther) {
            $('#<%=trOtherBlackListReason.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=trOtherBlackListReason.ClientID %>').attr('style', 'display:none');
            $('#<%=txtOtherBlackListReason.ClientID %>').val('');
        }
    }

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

    function oncboSalutationValueChanged() {
        var ageInYear = parseInt($('#<%=txtAgeInYear.ClientID %>').val());
        var salutation = cboSalutation.GetValue();

        if (cboGender.GetValue() == '0003^F') {
            $('#<%=chkIsPregnant.ClientID %>').removeAttr('disabled');
        }
        else {
            $('#<%=chkIsPregnant.ClientID %>').attr("disabled", true);
            $('#<%=chkIsPregnant.ClientID %>').prop('checked', false);
        }
    }

    $('#<%=txtAgeInYear.ClientID %>').change(function () {
        oncboSalutationValueChanged();
    });

    function onCbpPopupProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'PatientExis') {
            if (param[1] == 'fail') {
                $('#<%=hdnIsAxisPatient.ClientID %>').val("1");
                /// showToast('Save Failed', "Sudah ada data yang sama");
            } else {
                $('#<%=hdnIsAxisPatient.ClientID %>').val("0");
            }

        }
    }

    //#region Guest

    $('#<%=btnGuest.ClientID %>').click(function () {
        var filterexpresion = "MRN IS  Null and IsDeleted=0";
        openSearchDialog('guest1', filterexpresion, function (value) {
            onGuestChanged(value);
        });
    });

    function onGuestChanged(value) {
        var filterExpression = "GuestID = '" + value + "' AND IsDeleted=0";
        Methods.getObject('GetvGuestList', filterExpression, function (result) {
            if (result != null) {
                onBidingGuest(result)
            }

        });
    }

    function onBindGuestEdit() {
        var mrn = $('#<%=hdnMRN.ClientID %>').val();
        if (mrn != "") {

            var filterExpression = "MRN = '" + mrn + "' AND IsDeleted=0";
            Methods.getObject('GetvGuestList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGuestID.ClientID %>').val(result.GuestID);
                    $('#<%=txtGuestNo.ClientID %>').val(result.GuestNo);

                }

            });

            $('#<%=btnGuest.ClientID %>').hide();
            $('#<%=txtGuestNo.ClientID %>').attr('disabled', 'disabled');
        }
    }

    function onBidingGuest(row) {
        $('#<%=hdnGuestID.ClientID %>').val(row.GuestID);
        $('#<%=txtGuestNo.ClientID %>').val(row.GuestNo);
        $('#<%=txtFirstName.ClientID %>').val(row.FirstName);
        $('#<%=txtMiddleName.ClientID %>').val(row.MiddleName);
        $('#<%=txtFamilyName.ClientID %>').val(row.LastName);
        cboGender.SetValue(row.GCGender);
        cboBloodType.SetValue(row.GCBloodType);
        cboBloodRhesus.SetValue(row.BloodRhesus);
        cboSalutation.SetValue(row.GCSalutation);
        cboTitle.SetValue(row.GCTitle);
        cboSuffix.SetValue(row.GCSuffix);
        cboIdentityCardType.SetValue(row.GCIdentityNoType);
        $('#<%=txtIdentityCardNo.ClientID %>').val(row.SSN);

        $('#<%=txtBirthPlace.ClientID %>').val(row.CityOfBirth);
        $('#<%=txtDOB.ClientID %>').val(row.DateOfBirthInStringDatePickerFormat);
        if (row.DateOfBirthInStringDatePickerFormat != "") {
            var age = Methods.getAgeFromDatePickerFormat(row.DateOfBirthInStringDatePickerFormat);
            $('#<%=txtAgeInYear.ClientID %>').val(age.years);
            $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
            $('#<%=txtAgeInDay.ClientID %>').val(age.days);
        }

        if (row.GCReligion != "") {
            var ReligionData = row.GCReligion.split('^');
            $('#<%=txtReligionCode.ClientID %>').val(ReligionData[1]);
            $('#<%=txtReligionName.ClientID %>').val(row.Religion);
        }

        if (row.GCEducation != "") {
            var EducationData = row.GCEducation.split('^');
            $('#<%=txtEducationCode.ClientID %>').val(EducationData[1]);
            $('#<%=txtEducationName.ClientID %>').val(row.Education);
        }
        if (row.GCLanguage != "") {
            var LanguageData = row.GCLanguage.split('^');
            $('#<%=txtlanguageCode.ClientID %>').val(LanguageData[1]);
            $('#<%=txtlanguageName.ClientID %>').val(row.Language);
        }
        if (row.GCEthnic != "") {
            var EthnicData = row.GCEthnic.split('^');
            $('#<%=txtEthnicCode.ClientID %>').val(EthnicData[1]);
            $('#<%=txtEthnicName.ClientID %>').val(row.Ethnic);
        }
        if (row.GCMaritalStatus) {

            var MaritalStatusData = row.GCMaritalStatus.split('^');
            $('#<%=txtMaritalStatusCode.ClientID %>').val(MaritalStatusData[1]);
            $('#<%=txtMaritalStatusName.ClientID %>').val(row.MaritalStatus);
        }
        if (row.GCNationality != null) {
            var NationalityData = row.GCNationality.split('^');

            $('#<%=txtNationalityCode.ClientID %>').val(NationalityData[1]);
            $('#<%=txtNationalityName.ClientID %>').val(row.Nationality);

        }
        $('#<%=txtAddress.ClientID %>').val(row.StreetName);
        $('#<%=txtCounty.ClientID %>').val(row.County);
        $('#<%=txtDistrict.ClientID %>').val(row.District);
        $('#<%=txtCounty.ClientID %>').val(row.County);
        $('#<%=txtCity.ClientID %>').val(row.City);

        $('#<%=txtTelephoneNo1.ClientID %>').val(row.PhoneNo);
        $('#<%=txtMobilePhone1.ClientID %>').val(row.MobilePhoneNo);
        $('#<%=txtEmail.ClientID %>').val(row.EmailAddress);
    }

    //#endregion 


    function submitPatientNotes() {
        if ($('#<%=hdnPatientNotesProcessMode.ClientID %>').val() == "1") {
            cbpPatientNotes.PerformCallback('add');
        }
        else {
            cbpPatientNotes.PerformCallback('edit');
        }
    }

    $('.btnApplyPatientNotes').click(function () {
        submitPatientNotes();
        $('#<%=txtPatientNotes.ClientID %>').focus();
    });

    $('.btnCancelPatientNotes').click(function () {
        ResetPatientNotesEntryControls();
    });

    //#region Catatan Pasien
    function GetCurrentSelectedPatientNotes(s) {
        var $tr = $(s).closest('tr').parent().closest('tr');
        var idx = $('#<%=grdPatientNotesView.ClientID %> tr').index($tr);
        $('#<%=grdPatientNotesView.ClientID %> tr:eq(' + idx + ')').click();

        $row = $('#<%=grdPatientNotesView.ClientID %> tr.selected');
        var selectedObj = {};

        $row.find('input[type=hidden]').each(function () {
            selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });

        return selectedObj;
    }

    function SetPatientNotesEntityToControl(param) {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedPatientNotes(param);

        $('#<%=txtPatientNotes.ClientID %>').val(selectedObj.Notes);
    }

    function ResetPatientNotesEntryControls(s) {
        //cboInstructionType.SetValue('');
        $('#<%=txtPatientNotes.ClientID %>').val('');
    }

    $('.imgEditPatientNotes.imgLink').die('click');
    $('.imgEditPatientNotes.imgLink').live('click', function () {
        SetPatientNotesEntityToControl(this);
        console.log(this);
        $('#<%=hdnPatientNotesProcessMode.ClientID %>').val('0');
    });

    $('.imgDeletePatientNotes.imgLink').die('click');
    $('.imgDeletePatientNotes.imgLink').live('click', function () {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedPatientNotes(this);

        var message = "Hapus Catatan Pasien ?";
        showToastConfirmation(message, function (result) {
            if (result) {
                cbpPatientNotes.PerformCallback('delete');
            }
        });
    });

    var pageCount = parseInt('<%=gridPatientNotesPageCount %>');
    $(function () {
        setPaging($("#PatientNotesPaging"), pageCount, function (page) {
            cbpPatientNotesView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPatientNotesViewEndCallback(s) {
        var param = s.cpResult.split('|');
        var summaryText = s.cpSummary;
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPatientNotesView.ClientID %> tr:eq(1)').click();


            $('#<%=hdnPatientNotesID.ClientID %>').val(param[2]);


            setPaging($("#diagnosticPaging"), pageCount, function (page) {
                cbpPatientNotesView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPatientNotesView.ClientID %> tr:eq(1)').click();

        $('#<%=hdnPatientNotesID.ClientID %>').val(summaryText);
    }

    function onCbpPatientNotesEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit")
                $('#<%=hdnPatientNotesProcessMode.ClientID %>').val('1');

            ResetPatientNotesEntryControls();
            cbpPatientNotesView.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            showToast("ERROR", 'Error Message : ' + param[2]);
        }
        else
            $('#<%=grdPatientNotesView.ClientID %> tr:eq(1)').click();
    }

    //#endregion

</script>
<div style="height: 445px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnDOB" value="" />
    <input type="hidden" runat="server" id="hdnReservationID" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnPatientNotesID" value="" />
    <input type="hidden" runat="server" id="hdnOfficeZipCode" value="" />
    <input type="hidden" id="hdnPayerID" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnPatientLastName" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingToMedinfrasMobileApps" value="" />
    <input type="hidden" runat="server" id="hdnIsBlockDoublePatientData" value="" />
    <input type="hidden" runat="server" id="hdnTokenInhealth" value="" />
    <input type="hidden" runat="server" id="hdnKodeProviderInhealth" value="" />
    <input type="hidden" runat="server" id="hdnUsername" value="" />
    <input type="hidden" runat="server" id="hdnTodayDate" value="" />
    <input type="hidden" runat="server" id="hdnDateToday" value="" />
    <input type="hidden" runat="server" id="hdnIsAdd" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingBPJSVClaimVersion" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnIsAxisPatient" value="0" />
    <input type="hidden" runat="server" id="hdnGuestID" value="0" />
    <input type="hidden" runat="server" id="hdnIsMobilePhoneNumeric" value="0" />
    <input type="hidden" runat="server" id="hdnIsGenerateRMFromGuest" value="0" />
    <input type="hidden" value="1" id="hdnPatientNotesProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnIsPatientNewBornNotTakeFromMotherName" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 49%" />
            <col style="width: 3px" />
            <col style="width: 49%" />
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
                                <label class="lblLink" runat="server" id="lblMRN">
                                    <%=GetLabel("No Rekam Medis")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <colgroup>
                                        <col width="120px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMRNPatientEntryCtl" Width="99%" runat="server" />
                                        </td>
                                        <td id="tdReusedRM" style="padding-left: 10px" runat="server">
                                            <asp:CheckBox ID="chkIsUsedReused" Checked="false" runat="server" /><%:GetLabel("Nomor Kosong")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("No. Pengunjung")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <colgroup>
                                        <col width="120px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGuestNo" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            <input type="button" value="Salin Data Pengunjung" class="btn" id="btnGuest" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trInformationMergeMRN" style="display: none" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblMergeMRN">
                                    <%=GetLabel("Gabungan Dari")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <colgroup>
                                        <col width="120px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMRNPatientEntryCtlMerge" Width="99%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label1">
                                    <%=GetLabel("No Peserta BPJS")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80%" />
                                        <col width="20%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNHSRegistrationNo" Width="100%" runat="server" />
                                        </td>
                                        <td style="padding-left: 5px" id="tdPEValidateBPJS" runat="server">
                                            <input type="button" id="btnSearchPesertaCtl" title="Ambil Data Peserta BPJS" value='<%= GetLabel("Ambil Data")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label4">
                                    <%=GetLabel("No RM E-Klaim")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80%" />
                                        <col width="20%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEKlaimMedicalNo" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label5">
                                    <%=GetLabel("No Register SITB")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80%" />
                                        <col width="20%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSITBRegisterNo" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label2">
                                    <%=GetLabel("No Peserta Inhealth")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80%" />
                                        <col width="20%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtInhealthParticipantNo" Width="100%" runat="server" />
                                        </td>
                                        <td style="padding-left: 5px" id="tdPEValidateInhealth" runat="server">
                                            <input type="button" id="btnInhealthSearchPesertaCtl" title="Ambil Data Peserta Inhealth"
                                                value='<%= GetLabel("Ambil Data")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label3" title="No. Rekam Medis Pasien di Platform SATUSEHAT">
                                    <%=GetLabel("IHS Number")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80%" />
                                        <col width="20%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIHSNumber" Width="100%" runat="server" />
                                        </td>
                                        <td style="padding-left: 10px" id="tdGetIHSNumber" runat="server">
                                            <input type="button" id="btnGetIHSNumber" value='<%=GetLabel("IHS Number") %>' title="No. Rekam Medis Pasien di Platform SATUSEHAT" />
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
                                <div style="display: none">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                <table width="100%">
                                    <colgroup>
                                        <col width="50%" />
                                        <col width="50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboIdentityCardType" ClientInstanceName="cboIdentityCardType"
                                                runat="server" Width="100%" />
                                        </td>
                                        <td>
                                            <input type="hidden" value="" id="hdnUploadedFile1" runat="server" />
                                            <input type="button" id="btnUploadFile" value="Upload" style="width: 100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsSSNTemporary" runat="server" /><%:GetLabel(" Nomor Kartu Identitas Sementara")%>
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
                                            <asp:TextBox ID="txtIdentityCardNo" Width="100%" MaxLength="20" runat="server" />
                                        </td>
                                        <td style="padding-left: 5px" id="tdValidateBPJSUsingIDCard" runat="server">
                                            <input type="button" id="btnSearchBPJSUsingIDCard" title="Ambil Data Peserta BPJS dari NIK"
                                                value='<%= GetLabel("Ambil Data")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Kartu Keluarga")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80%" />
                                        <col width="20%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFamilyCardNo" Width="100%" MaxLength="30" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal" id="lblSalutation" runat="server">
                                    <%=GetLabel("Sapaan")%></label><br />
                                / Gelar Depan
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 49%" />
                                        <col style="width: 3px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboSalutation" ClientInstanceName="cboSalutation" Width="100%"
                                                runat="server">
                                                <ClientSideEvents ValueChanged="function(s,e) { oncboSalutationValueChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboTitle" ClientInstanceName="cboTitle" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal" id="lblPatientName" runat="server">
                                    <%=GetLabel("Nama Pasien")%></label>
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
                                            <asp:TextBox ID="txtFirstName" Width="100%" MaxLength="50" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleName" Width="99%" MaxLength="50" runat="server" />
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
                                <asp:TextBox ID="txtFamilyName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Pasien (Khusus)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName2" Width="100%" MaxLength="200" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Panggilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreferredName" Width="100%" MaxLength="35" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama di Kartu") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCardName" Width="100%" runat="server" MaxLength="28" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffix" ClientInstanceName="cboSuffix" Width="100%" runat="server" />
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
                                            <dxe:ASPxComboBox ID="cboGender" ClientInstanceName="cboGender" Width="100%" runat="server">
                                                <ClientSideEvents ValueChanged="function(s,e) { oncboSalutationValueChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
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
                                            <asp:TextBox ID="txtReligionCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReligionName" Width="100%" runat="server" ReadOnly="true" />
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
                                            <asp:TextBox ID="txtNationalityCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNationalityName" Width="100%" runat="server" ReadOnly="true" />
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
                                            <asp:TextBox ID="txtBirthPlace" Width="100%" MaxLength="50" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDOB" Width="108px" runat="server" CssClass="datepicker" />
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
                                            <asp:TextBox ID="txtAgeInYear" CssClass="number" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeInMonth" CssClass="number" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeInDay" CssClass="number" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" />
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Keluarga")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Pasangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpouseName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Anak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChildName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Pekerjaan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblPatientJob" runat="server">
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
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kantor")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOffice" Width="100%" MaxLength="100" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Alamat Kantor")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeAddress" Width="100%" MaxLength="500" runat="server"
                                    TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblOfficeZipCode">
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
                                            <asp:TextBox ID="txtPatientJobOfficeZipCode" Width="100%" runat="server" />
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
                                            <asp:TextBox ID="txtRTOfficeData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRWOfficeData" Width="100%" MaxLength="3" runat="server" />
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
                                <asp:TextBox ID="txtPatientJobOfficeCounty" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeDistrict" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPatientJobOfficeCity">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeCity" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblOfficeProvince">
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
                                            <asp:TextBox ID="txtPatientJobOfficeProvinceCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientJobOfficeProvinceName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Telepon Kantor")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeTelephone" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email Kantor")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeEmail" CssClass="email" Width="100%" MaxLength="50"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountNoCtl" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountNameCtl" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Divisi Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountDepartmentCtl" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Lain")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("No RM Lama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldMedicalNo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded" id="classSavePatientNotes" runat="server">
                    <%=GetLabel("Catatan Pasien")%></h4>
                <div class="containerTblEntryContent" id="btnSavePatientNotes" runat="server">
                    <div style="position: relative;">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <colgroup>
                                            <col width="100px" />
                                            <col width="165px" />
                                            <col width="100px" />
                                            <col width="100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td style="padding-left: 5px; width: 300px">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Catatan Pasien")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientNotes" runat="server" Width="250px" TextMode="MultiLine"
                                                    Rows="2" />
                                            </td>
                                            <td style="padding-left: 5px" colspan="2">
                                                <table border="0" cellpadding="0" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <img class="btnApplyPatientNotes imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td>
                                                            <img class="btnCancelPatientNotes imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpPatientNotesView" runat="server" Width="100%" ClientInstanceName="cbpPatientNotesView"
                                        ShowLoadingPanel="false" OnCallback="cbpPatientNotesView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpPatientNotesViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent10" runat="server">
                                                <asp:Panel runat="server" ID="Panel9" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdPatientNotesView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="1px">
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEditPatientNotes imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDeletePatientNotes imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("Notes") %>" bindingfield="Notes" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Notes" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Belum ada Catatan yang diberikan")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="PatientNotesPaging">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td>
                &nbsp;
            </td>
            <td style="padding: 5px; vertical-align: top;">
                <h4 class="h4expanded">
                    <%=GetLabel("Foto Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table width="25%">
                        <colgroup>
                            <col width="150px" />
                        </colgroup>
                        <tr>
                            <td rowspan="4" style="height: 100px; width: 150px; border: 1px solid ActiveBorder;"
                                align="center">
                                <input type="hidden" id="Hidden1" runat="server" value="" />
                                <img src="" alt="" runat="server" id="imgPreview" width="150" height="150" />
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
                                <asp:TextBox ID="txtAddress" Width="100%" MaxLength="500" runat="server" TextMode="MultiLine"
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
                                            <input type="hidden" runat="server" id="hdnZipCode" value="" />
                                            <asp:TextBox ID="txtZipCode" Width="100%" runat="server" />
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
                                            <asp:TextBox ID="txtRTData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRWData" Width="100%" MaxLength="3" runat="server" />
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
                                <asp:TextBox ID="txtCounty" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrict" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblCity">
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
                                            <asp:TextBox ID="txtProvinceName" Width="100%" runat="server" />
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
                                <asp:TextBox ID="txtTelephoneNo1" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Telepon 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNo2" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No HP 1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone1" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No HP 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone2" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" CssClass="email" Width="100%" MaxLength="50" runat="server" />
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
                                <asp:TextBox ID="txtAddressDomicile" Width="100%" MaxLength="500" runat="server"
                                    TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblZipCodeDomicile" runat="server">
                                    <input type="hidden" runat="server" id="hdnZipCodeDomicile" value="" />
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
                                            <asp:TextBox ID="txtZipCodeDomicile" Width="100%" runat="server" />
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
                                            <asp:TextBox ID="txtRTDomicileData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRWDomicileData" Width="100%" MaxLength="3" runat="server" />
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
                                <asp:TextBox ID="txtCountyDomicile" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictDomicile" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblCityDomicile">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityDomicile" Width="100%" runat="server" />
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
                                            <asp:TextBox ID="txtProvinceDomicileCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceDomicileName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Pembayar")%></h4>
                <div class="containerTblEntryContent">
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
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
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
                    </table>
                </div>
                <h4 id="hdPEBPJSInformation" runat="server" class="h4expanded">
                    <%:GetLabel("Data Peserta BPJS :")%>
                    <label id="lblPEBPJSStatusINACTIVE" style="color: Red; font-weight: bold; display: none">
                        <%:GetLabel("TIDAK AKTIF")%></label>
                </h4>
                <div class="containerTblEntryContent">
                    <table id="tblPEBPJSInformation" class="tblEntryContent" runat="server" width="100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="table-layout: fixed; width: 180px;">
                                <label class="lblNormal">
                                    <%:GetLabel("Nama Peserta")%></label>
                            </td>
                            <td style="table-layout: fixed; width: 450px;">
                                <asp:TextBox ID="txtPENamaPeserta" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Peserta")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPEJenisPeserta" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kelas Tanggungan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPEKelas" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Faskes / PPK 1") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPEPpkRujukan" runat="server" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Tambahan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblEducation" runat="server">
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
                                <label class="lblLink" id="lblPatientCategory">
                                    <%=GetLabel("Kategori Pasien")%></label>
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
                                            <asp:TextBox ID="txtPatientCategoryCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientCategoryName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPatientEntryEmployee">
                                    <%=GetLabel("Pegawai")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
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
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Status Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="2" style="font-weight: bold;">
                                            <%=GetLabel("Klinis") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasAllergy" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Alergi")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsG6PD" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("G6PD")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCataract" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pasien Katarak")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsAlive" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Hidup")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasPhysicalLimitation" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Keterbatasan Fisik")%>
                                        </td>
                                        <td id="tdPhysicalLimitation" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboPhysicalLimitation" ClientInstanceName="cboPhysicalLimitation"
                                                Width="100px">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsGeriatricPatient" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pasien Geriatri")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsPregnant" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Hamil")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="3" style="font-weight: bold;">
                                            <%=GetLabel("Sosial") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsSmoking" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Merokok")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsIlliteracy" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Buta Huruf")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsDonor" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Donor")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsBlackList" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pasien Bermasalah / Penanganan khusus")%>
                                        </td>
                                        <td id="tdGCBlacklistReason" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboGCBlacklistReason" ClientInstanceName="cboGCBlacklistReason"
                                                Width="100px">
                                                <ClientSideEvents ValueChanged="function(s,e){ onCboGCBlacklistReasonValueChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr id="trOtherBlackListReason" runat="server" style="display: none">
                                        <td>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOtherBlackListReason" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsVIP" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("VIP")%>
                                        </td>
                                        <td id="tdVIPGroup" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboVIPGroup" ClientInstanceName="cboVIPGroup"
                                                Width="100px">
                                                <ClientSideEvents ValueChanged="function(s,e){ oncboVIPGroupChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasCommunicationRestriction" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Keterbatasan Komunikasi")%>
                                        </td>
                                        <td id="tdCommunication" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboCommunication" ClientInstanceName="cboCommunication"
                                                Width="100px">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trVIPGroupOther" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOtherReason">
                                    <%=GetLabel("VIP Group (Other) : ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherVIPGroup" Width="180px" runat="server" />
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
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPopupProcessEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server">
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpPatientNotes" runat="server" Width="100%" ClientInstanceName="cbpPatientNotes"
        ShowLoadingPanel="false" OnCallback="cbpPatientNotes_Callback">
        <ClientSideEvents EndCallback="function(s,e){ onCbpPatientNotesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
