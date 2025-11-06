<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RegistrationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" id="hdnDiagnosticType" value="" runat="server" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnItemCardFee" runat="server" />
    <input type="hidden" id="hdnIsControlPatientCardPayment" runat="server" />
    <input type="hidden" id="hdnIsControlAdmCost" runat="server" />
    <input type="hidden" id="hdnDefaultGCAdmissionSource" runat="server" />
    <input type="hidden" id="hdnLastParamedicID" runat="server" />
    <input type="hidden" id="hdnLastParamedicCode" runat="server" />
    <input type="hidden" id="hdnLastParamedicName" runat="server" />
    <input type="hidden" id="hdnLastSpecialty" runat="server" />
    <input type="hidden" id="hdnIsBridgingToBPJS" runat="server" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <input type="hidden" id="hndIsBlockPatientAlreadyHasRegistrationDateParamedicClinicSame"
        value="" runat="server" />
    <input type="hidden" id="hdnIsQueueNoUsingAppointment" runat="server" />
    <input type="hidden" id="hdnIsAdd" runat="server" />
    <input type="hidden" id="hdnIsControlAdministrationCharges" runat="server" />
    <input type="hidden" id="hdnIsWarningPatientHaveAR" runat="server" />
    <input type="hidden" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
    <input type="hidden" id="hdnIsOutpatientUsingRoom" value="0" runat="server" />
    <input type="hidden" id="hdnIsEmergencyUsingRoom" value="0" runat="server" />
    <input type="hidden" id="hdnIsCheckNewPatient" value="0" runat="server" />
    <input type="hidden" id="hdnIsAuthorizedToJump" value="0" runat="server" />
    <input type="hidden" id="hdnIsBridgingToQumatic" value="0" runat="server" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <input type="hidden" id="hdnIsBridgingToMobileJKN" value="0" runat="server" />
    <input type="hidden" id="hdnIsSendNotifToMobileJKN" value="0" runat="server" />
    <input type="hidden" id="hdnIsCheckScheduleBeforeRegistrationUsingConfirmation" value="0"
        runat="server" />
    <input type="hidden" id="hdnIsReferralVisit" value="0" runat="server" />
    <input type="hidden" id="hdnIsBridgingToIPTV" value="0" runat="server" />
    <input type="hidden" id="hdnIsUsedReferenceQueueNo" value="0" runat="server" />
    <input type="hidden" id="hdnIsUsingMultiVisitSchedule" value="0" runat="server" />
    <input type="hidden" id="hdnRadioteraphyUnitID" value="0" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClinicTransaction" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Transaction") %></div>
    </li>
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            if ($('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.INPATIENT && $('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.EMERGENCY) {
                $('#<%:trIsKontrolBPJS.ClientID %>').removeAttr('style');
            }

            if ($('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val() == "" && $('#<%:txtPhysicianCode.ClientID %>').val() != "") {
                var physicianCode = $('#<%:txtPhysicianCode.ClientID %>').val();
                Methods.getObject("GetvParamedicMasterList", "ParamedicCode = '" + physicianCode + "'", function (result) {
                    if (result != null) {
                        $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val(result.BPJSReferenceInfo);
                    }
                });
            }
            setRightPanelButtonEnabled();
            if (typeof onTxtAppointmentNoChanged === 'function') {
                if ($('#<%=txtAppointmentNo.ClientID %>').val() != '') {
                    onTxtAppointmentNoChanged($('#<%=txtAppointmentNo.ClientID %>').val());
                }
            }

            if (typeof onTxtReservationNoChanged === 'function') {
                if ($('#<%=txtReservationNo.ClientID %>').val() != '') {
                    onTxtReservationNoChanged($('#<%=txtReservationNo.ClientID %>').val());
                }
            }

            $('#<%=btnClinicTransaction.ClientID %>').click(function () {
                if ($('#<%=hdnVisitID.ClientID %>').val() != "") {
                    onCustomButtonClick('transaction');
                }
                else showToast('Warning', 'Pilih nomor registrasi terlebih dahulu.');
            });

            //#region Province
            function GetSCProvinceFilterExpression() {
                var filterExpression = "<%:GetSCProvinceFilterExpression() %>";
                return filterExpression;
            }

            $('#<%:lblAccidentProvince.ClientID %>.lblLink').click(function () {
                openSearchDialog('stdcode', GetSCProvinceFilterExpression(), function (value) {
                    $('#<%=txtKodePropinsi.ClientID %>').val(value);
                    onTxtKodePropinsiChanged(value);
                });
            });

            $('#<%=txtKodePropinsi.ClientID %>').change(function () {
                onTxtKodePropinsiChanged($(this).val());
            });

            function onTxtKodePropinsiChanged(value) {
                var filterExpression = GetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGCState.ClientID %>').val(result.StandardCodeID);
                        $('#<%=hdnKodePropinsiBPJS.ClientID %>').val(result.TagProperty);
                        $('#<%=txtNamaPropinsi.ClientID %>').val(result.StandardCodeName);
                    }
                    else {
                        $('#<%=hdnGCState.ClientID %>').val('');
                        $('#<%=hdnKodePropinsiBPJS.ClientID %>').val('');
                        $('#<%=txtNamaPropinsi.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Kota/Kabupaten
            function GetCityFilterExpression() {
                var filterExpression = " GCState = '" + $('#<%=hdnGCState.ClientID %>').val() + "'";
                return filterExpression;
            }

            $('#<%:lblAccidentCity.ClientID %>.lblLink').click(function () {
                openSearchDialog('kabupaten', GetCityFilterExpression(), function (value) {
                    $('#<%=txtKodeKabupaten.ClientID %>').val(value);
                    onTxtKodeKabupatenChanged(value);
                });
            });

            $('#<%=txtKodeKabupaten.ClientID %>').change(function () {
                onTxtKodeKabupatenChanged($(this).val());
            });

            function onTxtKodeKabupatenChanged(value) {
                var filterExpression = GetCityFilterExpression() + " AND KodeKabupaten = '" + value + "'";
                Methods.getObject('GetKabupatenList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnKabupatenID.ClientID %>').val(value);
                        $('#<%=txtNamaKabupaten.ClientID %>').val(result.NamaKabupaten);

                        if (result.BPJSReferenceInfo != null) {
                            var bpjsReferenceInfo = result.BPJSReferenceInfo.split('|');

                            if (bpjsReferenceInfo[0] != '') {
                                $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(bpjsReferenceInfo[0]);
                            }
                            else {
                                $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(result.KodeKabupaten);
                            }
                        }
                        else {
                            $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(result.KodeKabupaten);
                        }
                    }
                    else {
                        $('#<%=hdnKabupatenID.ClientID %>').val('0');
                        $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val('');
                        $('#<%=txtKodeKabupaten.ClientID %>').val('');
                        $('#<%=txtNamaKabupaten.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Kecamatan
            function GetDistrictFilterExpression() {
                var filterExpression = " KabupatenID = " + $('#<%=hdnKabupatenID.ClientID %>').val();
                return filterExpression;
            }

            $('#<%:lblAccidentDistrict.ClientID %>.lblLink').click(function () {
                openSearchDialog('kecamatan', GetDistrictFilterExpression(), function (value) {
                    $('#<%=txtKodeKecamatan.ClientID %>').val(value);
                    onTxtKodeKecamatanChanged(value);
                });
            });

            $('#<%=txtKodeKabupaten.ClientID %>').change(function () {
                onTxtKodeKecamatanChanged($(this).val());
            });

            function onTxtKodeKecamatanChanged(value) {
                var filterExpression = GetDistrictFilterExpression() + " AND KodeKecamatan = '" + value + "'";
                Methods.getObject('GetKecamatanList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnKecamatanID.ClientID %>').val(value);
                        $('#<%=txtNamaKecamatan.ClientID %>').val(result.NamaKecamatan);

                        if (result.BPJSReferenceInfo != null) {
                            var bpjsReferenceInfo = result.BPJSReferenceInfo.split('|');

                            if (bpjsReferenceInfo[0] != '') {
                                $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(bpjsReferenceInfo[0]);
                            }
                            else {
                                $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(result.KodeKecamatan);
                            }
                        }
                        else {
                            $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(result.KodeKecamatan);
                        }
                    }
                    else {
                        $('#<%=hdnKecamatanID.ClientID %>').val('0');
                        $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val('');
                        $('#<%=txtKodeKecamatan.ClientID %>').val('');
                        $('#<%=txtNamaKecamatan.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        });

        function checkAppointmentBeforeSaveRecordMaster() {
            var mrn = $('#<%:hdnMRN.ClientID %>').val();
            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var appointmentID = $('#<%:hdnAppointmentID.ClientID %>').val();

            var registrationDate = $('#<%:txtRegistrationDate.ClientID %>').val();
            var registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
            var registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);

            if (appointmentID != '') {
                return "";
            }
            else {
                var result2 = "";
                var filterExpression = "MRN = '" + mrn + "' AND ParamedicID = '" + paramedicID + "' AND HealthcareServiceUnitID = '" + healthcareServiceUnitID + "' AND StartDate = '" + registrationDateFormatString + "' AND  GCAppointmentStatus IN ('" + Constant.AppointmentStatus.STARTED + "','" + Constant.AppointmentStatus.CONFIRMED + "')";
                Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                    if (result != null) {
                        result2 = "Sudah ada perjanjian untuk <b>" + result.ParamedicName + "</b> di klinik <b>" + result.ServiceUnitName + "</b> di nomor <b>" + result.AppointmentNo + "</b> hari ini";
                    }
                });
                return result2;
            }
        }

        function validateRegistrationOnSameDay() {
            var mrn = $('#<%:hdnMRN.ClientID %>').val();
            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();

            var registrationDate = $('#<%:txtRegistrationDate.ClientID %>').val();
            var registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
            var registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);

            var resultFinal = "";

            if (departmentID == Constant.Facility.OUTPATIENT) {
                var filterExpression = "MRN = '" + mrn + "' AND ParamedicID = '" + paramedicID + "' AND HealthcareServiceUnitID = '" + healthcareServiceUnitID + "' AND VisitDate = '" + registrationDateFormatString + "' AND GCVisitStatus = 'X020^002'";
                Methods.getListObject('GetvConsultVisit1List', filterExpression, function (result) {
                    for (i = 0; i < result.length; i++) {
                        if (resultFinal == '') {
                            resultFinal = "Pasien dengan MRN <b>" + result[i].MedicalNo + "</b> sudah ada pendaftaran hari ini di klinik <b>" + result[i].ServiceUnitName + "</b> dengan dokter <b>" + result[i].ParamedicName + "</b> (<b>" + result[i].RegistrationNo + "</b>)";
                        }
                        else {
                            var info = "Pasien dengan MRN <b>" + result[i].MedicalNo + "</b> sudah ada pendaftaran hari ini di klinik <b>" + result[i].ServiceUnitName + "</b> dengan dokter <b>" + result[i].ParamedicName + "</b> (<b>" + result[i].RegistrationNo + "</b>)";
                            resultFinal = resultFinal + "<br>" + info;
                        }
                    }
                });
            }

            return resultFinal;
        }

        function onBeforeSaveRecordMaster(errMessage) {
            var registrationHour = $('#<%:txtRegistrationHour.ClientID %>').val();
            var isTemporaryLocation = $('#<%:chkIsTemporaryLocation.ClientID %>').is(':checked');
            var classRequestID = $('#<%=hdnClassRequestID.ClientID %>').val();
            var resultCheckAppointment = checkAppointmentBeforeSaveRecordMaster();
            var resultValidateRegistrationSameDay = validateRegistrationOnSameDay();
            var isBlockRegistrationSameDay = $('#<%=hndIsBlockPatientAlreadyHasRegistrationDateParamedicClinicSame.ClientID %>').val();

            if (registrationHour.length != 5) {
                errMessage.text = "Format Jam yang diinput salah";
                return false;
            }

            if (resultValidateRegistrationSameDay != '' && isBlockRegistrationSameDay == '1') {
                errMessage.text = resultValidateRegistrationSameDay;
                return false;
            }

            if (resultCheckAppointment != "") {
                errMessage.text = resultCheckAppointment;
                return false;
            }

            var today = new Date();
            var checkConfirmRegDate = "1";

            var oRegDate = $('#<%:txtRegistrationDate.ClientID %>').val();
            var oRegDate_day = oRegDate.substring(0, 2);
            var oRegDate_month = oRegDate.substring(3, 5);
            var oRegDate_year = oRegDate.substring(6, 10);
            oRegDate = oRegDate_month + "/" + oRegDate_day + "/" + oRegDate_year;
            oRegDate = new Date(Date.parse(oRegDate));

            var attr = $('#<%:txtRegistrationDate.ClientID %>').attr('readonly');

            if (typeof attr !== 'undefined' && attr !== false) { }
            else {
                if (oRegDate.toLocaleDateString() < today.toLocaleDateString()) {
                    var resultBackDateConf = confirm("Tanggal pendaftaran yang terpilih lebih kecil dari tanggal hari ini, lanjutkan proses simpan?");
                    if (resultBackDateConf == false) {
                        checkConfirmRegDate = "0";
                    }
                }
            }

            if (checkConfirmRegDate == "1") {
                if ($('#<%:hdnPayerID.ClientID %>').val() != '') {
                    var test = cboRegistrationPayer.GetValue();
                    var SCRegistrationPayerBPJS = 'X004^500';
                    if (cboRegistrationPayer.GetValue() == SCRegistrationPayerBPJS) {
                        if ($('#<%=hdnIsBridgingToBPJS.ClientID %>').val() == '1' && $('#<%=hdnIsBPJSChecked.ClientID %>').val() == '0' && $('#<%=txtNHSRegistrationNo.ClientID %>').val() != '') {
                            errMessage.text = "Harap Melakukan Check Status Peserta Terlebih Dahulu";
                            return false;
                        }
                        else {
                            if (isTemporaryLocation == true) {
                                if (classRequestID == '' || classRequestID == null) {
                                    errMessage.text = "Harap Isi Kelas Permintaan Terlebih Dahulu";
                                    return false;
                                }
                                else {
                                    return true;
                                }
                            }
                            else {
                                return true;
                            }
                        }
                    } else {
                        if (isTemporaryLocation == true) {
                            if (classRequestID == '' || classRequestID == null) {
                                errMessage.text = "Harap Isi Kelas Permintaan Terlebih Dahulu";
                                return false;
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            return true;
                        }
                    }
                } else {
                    if (isTemporaryLocation == true) {
                        if (classRequestID == '' || classRequestID == null) {
                            errMessage.text = "Harap Isi Kelas Permintaan Terlebih Dahulu";
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                    else {
                        return true;
                    }
                }
            } else {
                errMessage.text = "Batal simpan pendaftaran karena tanggal pendaftaran terpilih lebih kecil dari tanggal hari ini.";
                return false;
            }
        }

        function onAfterCustomClickSuccess(type, paramUrl) {
            var url = ResolveUrl(paramUrl);
            showLoadingPanel();
            window.location.href = url;
        }

        function onChangeHideLoadingPanel() {
            return false;
        }

        var isOpenPatientIdentityPopupFromAppointment = false;
        var isAfterSearchBPJSReferral = false;
        var isAfterSearchBPJSurkon = false;
        var isChangePayer = false;
        var isOpenPatientIdentityPopUpFromInhealthPraRegistration = false;

        function onAfterPopupControlClosing() {
            if (isOpenPatientIdentityPopupFromAppointment) {
                $('#<%:hdnAppointmentID.ClientID %>').val('');
                $('#<%:txtAppointmentNo.ClientID %>').val('');
                $('#<%:hdnReservationID.ClientID %>').val('');
                $('#<%:txtReservationNo.ClientID %>').val('');
                $('#<%:lblMRN.ClientID %>').attr('class', 'lblLink lblMandatory');
                $('#<%:txtMRN.ClientID %>').removeAttr('readonly');
                isOpenPatientIdentityPopupFromAppointment = false;
            }
            else if (isAfterSearchBPJSReferral) {
                isAfterSearchBPJSReferral = false;
            }
            else if (isAfterSearchBPJSurkon) {
                isAfterSearchBPJSurkon = false;
            }
            else if (isChangePayer) {
                var value = $('#<%:txtRegistrationNo.ClientID %>').val();
                var filterExpression = getRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    if (result != null) {
                        onLoadObject(value);
                    }
                    else {
                        $('#<%:txtRegistrationNo.ClientID %>').val('');
                    }
                });
            }
        }

        var maxBackDate = parseInt('<%:maxBackDate %>');
        var maxNextDate = parseInt('<%:maxNextDate %>');
        function onLoad() {
            setCustomToolbarVisibility();
            if ($('#<%:chkIsHasMRN.ClientID %>').is(':checked')) {
                $('#btnDataPasien').attr('style', 'display:none');
                $('#<%:trMRN.ClientID %>').removeAttr('style');
                $('#<%:trGuestNo.ClientID %>').attr('style', 'display:none');
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').removeAttr('enabled');
                $('#btnPatientFamily').removeAttr('enabled');
                $('#sendNotificationToJKN').removeAttr('enabled');
                $('#btnEmergencyContact').removeAttr('enabled');
            }
            else {
                $('#btnDataPasien').removeAttr('style');
                $('#<%:trMRN.ClientID %>').attr('style', 'display:none');
                $('#<%:trGuestNo.ClientID %>').removeAttr('style');
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').attr('enabled', 'false');
                $('#btnPatientFamily').attr('enabled', 'false');
                $('#sendNotificationToJKN').attr('enabled', 'false');
                $('#btnEmergencyContact').attr('enabled', 'false');
            }

            var attr = $('#<%:txtRegistrationDate.ClientID %>').attr('readonly');
            if (typeof attr !== 'undefined' && attr !== false) { }
            else {
                setDatePicker('<%:txtRegistrationDate.ClientID %>');
                $('#<%:txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', maxNextDate);
                $('#<%:txtRegistrationDate.ClientID %>').datepicker('option', 'minDate', '-' + maxBackDate);
            }
            var mrn = $('#<%:txtMRN.ClientID %>').val();
            if (mrn != '') {
                //                $('#<%:txtMRN.ClientID %>').trigger('change');

            }
            setTblPayerCompanyVisibility();
            var vt = $('#<%:txtVisitTypeCode.ClientID %>').val();
            if (vt != '') {
                $('#<%:txtVisitTypeCode.ClientID %>').trigger('change');
            }

            var attrTglRujukan = $('#<%:txtTglRujukan.ClientID %>').attr('readonly');
            if (typeof attrTglRujukan !== 'undefined' && attr !== false) { }
            else {
                setDatePicker('<%:txtTglRujukan.ClientID %>');
                $('#<%:txtTglRujukan.ClientID %>').datepicker('option', 'maxDate', 0);
            }

            var attrRefDate = $('#<%:txtReferrerDate.ClientID %>').attr('readonly');
            if (typeof attrRefDate !== 'undefined' && attrRefDate !== false) { }
            else {
                setDatePicker('<%:txtReferrerDate.ClientID %>');
                $('#<%:txtReferrerDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            }
        }

        function setRightPanelButtonEnabled() {
            if (getIsAdd()) {
                if ($('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.INPATIENT && $('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.EMERGENCY) {
                    $('#<%:trIsKontrolBPJS.ClientID %>').removeAttr('style');
                }
                $('#btnAddMCUPackage').attr('enabled', 'false');
                $('#btnUploadDocument').attr('enabled', 'false');
                $('#btnInfoPatientDocument').attr('enabled', 'false');
                $('#btnPatientVisit').attr('enabled', 'false');
                $('#btnInfoHistoryRegistration').attr('enabled', 'false');
                $('#btnPatientFamily').attr('enabled', 'false');
                $('#btnPaymentLetter').attr('enabled', 'false');
                $('#btnPayerDetail').attr('enabled', 'false');
                $('#btnLinkedToRegistration').attr('enabled', 'false');
                $('#btnGenerateSEP').attr('enabled', 'false');
                $('#btnRegistrationClaimHistory').attr('enabled', 'false');
                $('#btnGenerateSEPManual').attr('enabled', 'false');
                $('#sendNotificationToJKN').attr('enabled', 'false');
                $('#btnGenerateSJP').attr('enabled', 'false');
                $('#btnGenerateSJPManual').attr('enabled', 'false');
                $('#btnRegistrationEdit').attr('enabled', 'false');
                $('#btnServiceUnitEdit').attr('enabled', 'false');
                $('#btnRegistrationNotes').attr('enabled', 'false');
                $('#btnReferrerNotes').attr('enabled', 'false');
                $('#btnScanPatientIdentity').attr('enabled', 'false');
                $('#btnPrintLabelPatientRegistration').attr('enabled', 'false');
                $('#btnQueueEdit').attr('enabled', 'false');
                $('#btnAccompanyData').attr('enabled', 'false');
                $('#btnTakePictures').attr('enabled', 'false');
                $('#btnGuestTakePictures').attr('enabled', 'false');
                $('#btnGenerateDataRujukan').attr('enabled', 'false');
                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.DIAGNOSTIC) {
                    $('#btnEmergencyContact').attr('enabled', 'false');
                }
                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.EMERGENCY) {
                    $('#btnEmergencyContact').attr('enabled', 'false');
                }
                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT) {
                    $('#btnParamedicTeam').attr('enabled', 'false');
                    $('#btnEmergencyContact').attr('enabled', 'false');
                    $('#btnChargeClassEdit').attr('enabled', 'false');
                    $('#btnBedChange').attr('enabled', 'false');
                }
                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.OUTPATIENT) {
                    $('#btnEmergencyContact').attr('enabled', 'false');
                }
                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.PHARMACY) {
                    $('#btnEmergencyContact').attr('enabled', 'false');
                }
                $('#btnPayerDetail').attr('enabled', 'false');
                if ($('#<%:hdnMRN.ClientID %>').val() == '' || $('#<%:hdnMRN.ClientID %>').val() == '0') {
                    $('#btnInfoOutstandingInvoice').attr('enabled', 'false');
                }
                $('#btnDraftAppointment').attr('enabled', 'false');
            }
            else {
                if (cboRegistrationPayer.GetValue() == Constant.CustomerType.BPJS) {                    
                    $('#btnGenerateSEP').removeAttr('enabled');
                    $('#btnRegistrationClaimHistory').removeAttr('enabled');
                    $('#btnGenerateSEPManual').removeAttr('enabled');
                    $('#btnGenerateDataRujukan').removeAttr('enabled');
                    $('#btnSuratRencanaInap').removeAttr('enabled');
                    if ($('#<%:txtAppointmentNo.ClientID %>').val() != '' && $('#<%:txtRegistrationNo.ClientID %>').val() != '') {
                        $('#sendNotificationToJKN').removeAttr('enabled');
                    }
                    else {
                        $('#sendNotificationToJKN').attr('enabled', 'false');
                    }
                } else {
                    var countBPJS = 0;
                    if ($('#<%:hdnRegistrationID.ClientID %>').val() != null && $('#<%:hdnRegistrationID.ClientID %>').val() != "") {
                        var filterRegPayer = "IsDeleted = 0 AND RegistrationID = " + $('#<%:hdnRegistrationID.ClientID %>').val();
                        Methods.getListObject('GetvRegistrationPayerList', filterRegPayer, function (resultRegPayer) {
                            for (i = 0; i < resultRegPayer.length; i++) {
                                if (resultRegPayer[i].GCCustomerType == Constant.CustomerType.BPJS) {
                                    countBPJS += 1;
                                }
                            }
                        });
                    }
                    if (countBPJS > 0) {
                        $('#btnGenerateSEP').removeAttr('enabled');
                        $('#btnRegistrationClaimHistory').removeAttr('enabled');
                        $('#btnGenerateSEPManual').removeAttr('enabled');
                        $('#btnGenerateDataRujukan').removeAttr('enabled');
                        $('#btnSuratRencanaInap').removeAttr('enabled');
                        if ($('#<%:txtAppointmentNo.ClientID %>').val() != '' && $('#<%:txtRegistrationNo.ClientID %>').val() != '') {
                            $('#sendNotificationToJKN').removeAttr('enabled');
                        }
                        else {
                            $('#sendNotificationToJKN').attr('enabled', 'false');
                        }
                    } else {
                        $('#btnGenerateSEP').attr('enabled', 'false');
                        $('#btnRegistrationClaimHistory').attr('enabled', 'false');
                        $('#btnGenerateSEPManual').attr('enabled', 'false');
                        $('#btnGenerateDataRujukan').attr('enabled', 'false');
                        $('#btnSuratRencanaInap').attr('enabled', 'false');     
                        $('#sendNotificationToJKN').attr('enabled', 'false');
                    }
                }

                if (cboRegistrationPayer.GetValue() == Constant.CustomerType.INHEALTH) {
                    $('#btnGenerateSJP').removeAttr('enabled');
                    $('#btnGenerateSJPManual').removeAttr('enabled');
                } else {
                    var countInhealth = 0;
                    if ($('#<%:hdnRegistrationID.ClientID %>').val() != null && $('#<%:hdnRegistrationID.ClientID %>').val() != "") {
                        var filterRegPayer = "IsDeleted = 0 AND RegistrationID = " + $('#<%:hdnRegistrationID.ClientID %>').val();
                        Methods.getListObject('GetvRegistrationPayerList', filterRegPayer, function (resultRegPayer) {
                            for (i = 0; i < resultRegPayer.length; i++) {
                                if (resultRegPayer[i].GCCustomerType == Constant.CustomerType.INHEALTH) {
                                    countInhealth += 1;
                                }
                            }
                        });
                    }
                    if (countInhealth > 0) {
                        $('#btnGenerateSJP').removeAttr('enabled');
                        $('#btnGenerateSJPManual').removeAttr('enabled');
                    } else {
                        $('#btnGenerateSJP').attr('enabled', 'false');
                        $('#btnGenerateSJPManual').attr('enabled', 'false');
                    }
                }

                if ($('#<%:chkIsHasMRN.ClientID %>').is(':checked')) {
                    $('#btnUploadDocument').removeAttr('enabled');
                    $('#btnInfoPatientDocument').removeAttr('enabled');
                }

                $('#btnLinkedToRegistration').removeAttr('enabled');
                $('#btnAddMCUPackage').removeAttr('enabled');
                $('#btnPatientVisit').removeAttr('enabled');
                $('#btnInfoHistoryRegistration').removeAttr('enabled');
                $('#btnPaymentLetter').removeAttr('enabled');
                $('#btnPayerDetail').removeAttr('enabled');
                $('#btnRegistrationEdit').removeAttr('enabled');
                $('#btnServiceUnitEdit').removeAttr('enabled');
                $('#btnRegistrationNotes').removeAttr('enabled');
                $('#btnReferrerNotes').removeAttr('enabled');
                $('#btnScanPatientIdentity').removeAttr('enabled');
                $('#btnPrintLabelPatientRegistration').removeAttr('enabled');
                $('#btnQueueEdit').removeAttr('enabled');
                $('#btnEmergencyContact').removeAttr('enabled');
                $('#btnDraftAppointment').removeAttr('enabled');
                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT) {
                    $('#btnParamedicTeam').removeAttr('enabled');
                    $('#btnChargeClassEdit').removeAttr('enabled');
                    $('#btnAccompanyData').removeAttr('enabled');
                    if ($('#<%:hdnRegistrationStatus.ClientID %>').val() == Constant.RegistrationStatus.OPEN) {
                        $('#btnBedChange').removeAttr('enabled');
                    } else {
                        $('#btnBedChange').attr('enabled', 'false');
                    }
                    $('#btnLinkedToRegistration').attr('enabled', 'false');
                }
                if (cboRegistrationPayer.GetValue() != "<%:GetCustomerTypePersonal() %>")
                    $('#btnPayerDetail').removeAttr('enabled');
                else
                    $('#btnPayerDetail').attr('enabled', 'false');

                if ($('#<%:txtGuestNo.ClientID %>').val() != '') {
                    $('#btnPatientIdentity').attr('enabled', 'false');
                    $('#btnPatientFamily').attr('enabled', 'false');
                    $('#btnEmergencyContact').attr('enabled', 'false');
                    $('#btnTakePictures').attr('enabled', 'false');
                }
                else {
                    $('#btnPatientIdentity').removeAttr('enabled');
                    $('#btnPatientFamily').removeAttr('enabled');
                    $('#btnEmergencyContact').removeAttr('enabled');
                    $('#btnTakePictures').removeAttr('enabled');
                }

                if ($('#<%:txtRegistrationNo.ClientID %>').val() != '') 
                {
                    $('#btnGuestTakePictures').removeAttr('enabled'); 
                }
                
                $('#btnInfoOutstandingInvoice').removeAttr('enabled');
            }
        }

        function setIconVisibility() {
            var mrn = $('#<%:hdnMRN.ClientID %>').val();
            var filterExpression = "MRN = '" + mrn + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    var isVIP = result.IsVIP;
                    var vipGroup = result.cfVIPGroup;
                    if (isVIP == false) {
                        $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                    }
                    else {
                        $('#<%:tdIsVIP.ClientID %>').removeAttr('style');
                        document.getElementById('hdnTitleVIP').title = vipGroup;
                    }
                }
            });
        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            var isLinkedToInpatient = $('#<%:hdnIsLinkedToInpatient.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                    $('#<%=btnClinicTransaction.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnRegistrationStatus.ClientID %>').val() != Constant.RegistrationStatus.CANCELLED && $('#<%:hdnRegistrationStatus.ClientID %>').val() != Constant.RegistrationStatus.CLOSED) {
                        if (isLinkedToInpatient == 1)
                            $('#<%=btnVoid.ClientID %>').hide();
                        else
                            $('#<%=btnVoid.ClientID %>').show();
                        var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        if (departmentID != "PHARMACY" && departmentID != "DIAGNOSTIC") {
                            $('#<%=btnClinicTransaction.ClientID %>').hide();
                        } else {
                            $('#<%=btnClinicTransaction.ClientID %>').show();
                        }
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                        $('#<%=btnClinicTransaction.ClientID %>').hide();
                    }
                }
            }
            else {
                $('#<%=btnVoid.ClientID %>').hide();
                if (getIsAdd()) {
                    $('#<%=btnClinicTransaction.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnRegistrationStatus.ClientID %>').val() != Constant.RegistrationStatus.CANCELLED && $('#<%:hdnRegistrationStatus.ClientID %>').val() != Constant.RegistrationStatus.CLOSED) {
                        var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        if (departmentID != "PHARMACY" && departmentID != "DIAGNOSTIC") {
                            $('#<%=btnClinicTransaction.ClientID %>').hide();
                        } else {
                            $('#<%=btnClinicTransaction.ClientID %>').show();
                        }
                    } else {
                        $('#<%=btnClinicTransaction.ClientID %>').hide();
                    }
                }
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Registration/RegistrationVoidCtl.ascx');
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var id = registrationID + '|' + visitID + '|' + departmentID;
            openUserControlPopup(url, id, 'Void Registration', 400, 230);
        });



        //#region AppointmentNo
        function getAppointmentNoFilterExpression() {
            var departmentID = $('#<%:hdnDepartmentIDFilterAppointment.ClientID %>').val();
            var HealthcareServiceUnitIDLaboratory = $('#<%:hdnHealthcareServiceUnitIDLaboratory.ClientID %>').val();
            var HealthcareServiceUnitIDRadiology = $('#<%:hdnHealthcareServiceUnitIDRadiology.ClientID %>').val();
            var HealthcareServiceUnitIDRadioteraphy = $('#<%:hdnRadioteraphyUnitID.ClientID %>').val();
            var regDate = $('#<%:txtRegistrationDate.ClientID %>').val();
            var regTemp = Methods.DatePickerToDateFormat(regDate);
            var isRadioteraphy = $('#<%:hdnIsRadioteraphy.ClientID %>').val();

            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + $('#<%:hdnDepartmentID.ClientID %>').val() + "' AND  GCAppointmentStatus IN ('" + Constant.AppointmentStatus.STARTED + "','" + Constant.AppointmentStatus.CONFIRMED + "') AND StartDate = '" + regTemp + "'";
            if (departmentID == 'IMAGING') {
                filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND HealthcareServiceUnitID = '" + HealthcareServiceUnitIDRadiology + "' AND GCAppointmentStatus = '" + Constant.AppointmentStatus.STARTED + "' AND StartDate = '" + regTemp + "'";
            }
            else if (departmentID == 'LABORATORY') {
                filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND HealthcareServiceUnitID = '" + HealthcareServiceUnitIDLaboratory + "' AND GCAppointmentStatus = '" + Constant.AppointmentStatus.STARTED + "' AND StartDate = '" + regTemp + "'";
            }
            else if (departmentID == 'DIAGNOSTIC') {
                if (isRadioteraphy == "1") {
                    filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND HealthcareServiceUnitID = '" + HealthcareServiceUnitIDRadioteraphy + "' AND GCAppointmentStatus = '" + Constant.AppointmentStatus.STARTED + "' AND StartDate = '" + regTemp + "'";
                }
                else if (HealthcareServiceUnitIDRadioteraphy != null && HealthcareServiceUnitIDRadioteraphy != "") {
                    filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + $('#<%:hdnDepartmentID.ClientID %>').val() + "' AND GCAppointmentStatus = '" + Constant.AppointmentStatus.STARTED + "' AND StartDate = '" + regTemp + "'" + " AND HealthcareServiceUnitID NOT IN (" + HealthcareServiceUnitIDLaboratory + ',' + HealthcareServiceUnitIDRadiology + ',' + HealthcareServiceUnitIDRadioteraphy + ")";
                } else {
                    filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + $('#<%:hdnDepartmentID.ClientID %>').val() + "' AND GCAppointmentStatus = '" + Constant.AppointmentStatus.STARTED + "' AND StartDate = '" + regTemp + "'" + " AND HealthcareServiceUnitID NOT IN (" + HealthcareServiceUnitIDLaboratory + ',' + HealthcareServiceUnitIDRadiology + ")";
                }
            }

            return filterExpression;
        }

        $('#<%:lblAppointmentNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('appointment', getAppointmentNoFilterExpression(), function (value) {
                $('#<%:txtAppointmentNo.ClientID %>').val(value);
                onTxtAppointmentNoChanged(value);
            });
        });
        $('#<%:txtAppointmentNo.ClientID %>').live('change', function () {
            onTxtAppointmentNoChanged($(this).val());
        });
        function onTxtAppointmentNoChanged(value) {
            var filterExpression = getAppointmentNoFilterExpression() + " AND AppointmentNo = '" + value + "'";
            Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                if (result != null) {
                    var isHasCharges = false;
                    var isHasTestOrder = false;
                    var isHasTestOrderHd = false;
                    var isHasServiceOrder = false;
                    var isHasPrescriptionOrder = false;

                    var filterExpressionDraftCharges = "AppointmentID = '" + result.AppointmentID + "' AND GCTransactionStatus = '" + Constant.TransactionStatus.OPEN + "' AND GCTransactionDetailStatus = '" + Constant.TransactionStatus.OPEN + "' AND IsDeleted = 0";
                    var filterExpressionDraftTestOrderHd = "AppointmentID = '" + result.AppointmentID + "' AND GCTransactionStatus = '" + Constant.TransactionStatus.OPEN + "'";
                    var filterExpressionDraftTestOrder = "AppointmentID = '" + result.AppointmentID + "' AND GCTransactionStatus = '" + Constant.TransactionStatus.OPEN + "' AND GCDraftTestOrderStatus = '" + Constant.OrderStatus.OPEN + "' AND IsDeleted = 0";
                    var filterExpressionDraftServiceOrder = "AppointmentID = '" + result.AppointmentID + "' AND GCTransactionStatus = '" + Constant.TransactionStatus.OPEN + "' AND GCDraftServiceOrderStatus = '" + Constant.OrderStatus.OPEN + "' AND IsDeleted = 0";
                    var filterExpressionDraftPrescriptionOrder = "AppointmentID = '" + result.AppointmentID + "' AND GCTransactionStatus = '" + Constant.TransactionStatus.OPEN + "' AND GCDraftPrescriptionOrderStatus = '" + Constant.OrderStatus.OPEN + "' AND IsDeleted = 0";

                    Methods.getObject('GetvDraftPatientChargesDtList', filterExpressionDraftCharges, function (result) {
                        if (result != null) {
                            isHasCharges = true;
                        }
                        else {
                            isHasCharges = false;
                        }
                    });

                    Methods.getObject('GetvDraftTestOrderDtList', filterExpressionDraftTestOrder, function (result) {
                        if (result != null) {
                            isHasTestOrder = true;
                        }
                        else {
                            isHasTestOrder = false;
                        }
                    });

                    //                    Methods.getObject('GetvDraftTestOrderHdList', filterExpressionDraftTestOrderHd, function (result) {
                    //                        if (result != null) {
                    //                            isHasTestOrderHd = true;
                    //                        }
                    //                        else {
                    //                            isHasTestOrderHd = false;
                    //                        }
                    //                    });

                    Methods.getObject('GetvDraftServiceOrderDtList', filterExpressionDraftServiceOrder, function (result) {
                        if (result != null) {
                            isHasServiceOrder = true;
                        }
                        else {
                            isHasServiceOrder = false;
                        }
                    });

                    Methods.getObject('GetvDraftPrescriptionOrderDtList', filterExpressionDraftPrescriptionOrder, function (result) {
                        if (result != null) {
                            isHasPrescriptionOrder = true;
                        }
                        else {
                            isHasPrescriptionOrder = false;
                        }
                    });

                    if (isHasCharges == true || isHasTestOrderHd == true || isHasTestOrder == true || isHasServiceOrder == true || isHasPrescriptionOrder == true) {
                        $('#<%:hdnIsAppointmentHaveDraft.ClientID %>').val("1");
                    }
                    else {
                        $('#<%:hdnIsAppointmentHaveDraft.ClientID %>').val("0");
                    }

                    $('#<%:lblMRN.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtMRN.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:hdnAppointmentID.ClientID %>').val(result.AppointmentID);

                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val(result.PhysicianBPJSReferenceInfo);

                    cboSpecialty.SetValue(result.SpecialtyID);
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    $('#<%:hdnBPJSPoli.ClientID %>').val(result.BPJSPoli);
                    $('#<%:hdnIsServiceUnitHasParamedic.ClientID %>').val(result.IsHasParamedic ? '1' : '0');
                    $('#<%:hdnIsServiceUnitHasVisitType.ClientID %>').val(result.IsHasVisitType ? '1' : '0');
                    if ($('#<%:hdnIsQueueNoUsingAppointment.ClientID %>').val() == "1") {
                        if ($('#<%:hdnIsUsedReferenceQueueNo.ClientID %>').val() == "1") {
                            $('#<%:txtQueueNo.ClientID %>').val(result.cfReferenceQueueNo);
                        }
                        else {
                            $('#<%:txtQueueNo.ClientID %>').val(result.cfQueueNo);
                        }
                    }
                    $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                    $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                    $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                    $('#<%:hdnRoomID.ClientID %>').val(result.RoomID);
                    $('#<%:txtRoomName.ClientID %>').val(result.RoomName);
                    $('#<%:txtRoomCode.ClientID %>').val(result.RoomCode);
                    if (result.MRN == 0) {
                        showToast('Warning', '<%:GetErrorMessageHasMedicalNo() %>');
                        isOpenPatientIdentityPopupFromAppointment = true;
                        var id = 'app|' + result.AppointmentID;
                        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/PatientEntryCtl.ascx");
                        openUserControlPopup(url, id, 'Patient Identity', 1100, 550, 'patientIdentity');
                    }
                    else {
                        $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                        onTxtMRNChanged($('#<%:txtMRN.ClientID %>').val());
                    }

                    cboRegistrationPayer.SetValue(result.GCCustomerType);
                    setTblPayerCompanyVisibility();

                    if (cboRegistrationPayer.GetValue() != "<%:GetCustomerTypePersonal() %>") {
                        $('#<%:hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                        $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    }

                    $('#<%:txtPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                    $('#<%:txtParticipantNo.ClientID %>').val(result.CorporateAccountNo);
                    $('#<%:txtEmployeeCode.ClientID %>').val(result.EmployeeCode);
                    $('#<%:txtEmployeeCode.ClientID %>').val(result.EmployeeName);
                    $('#<%:txtCoverageLimit.ClientID %>').val(result.cfCoverageLimitAmountInString);
                    $('#<%=chkIsUsingCOB.ClientID %>').prop('checked', result.IsUsingCOB);
                    $('#<%=chkIsCoverageLimitPerDay.ClientID %>').prop('checked', result.IsCoverageLimitPerDay);
                    $('#<%:txtContractNo.ClientID %>').val(result.ContractNo);
                    $('#<%:txtContractPeriod.ClientID %>').val(result.cfContractEndDateInString);
                    $('#<%:txtVisitNotes.ClientID %>').val(result.Notes);

                    if (result.FromVisitID != "null") {
                        onRegistrationBPJSNoChanged(result.FromVisitID);
                        if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                            if ($('#<%=hdnIsUsingMultiVisitSchedule.ClientID %>').val() == '1') {
                                Methods.getObject("GetvHealthcareServiceUnitCustomList", "HealthcareServiceUnitID = " + result.HealthcareServiceUnitID + " AND IsDeleted = 0", function (resultHsuSch) {
                                    if (resultHsuSch != null) {
                                        if (resultHsuSch.IsAllowMultiVisitSchedule) {
                                            Methods.getObject("GetvPatientDiagnosisList", "VisitID = " + result.FromVisitID + " AND IsDeleted = 0 AND GCDiagnoseType = '" + Constant.DiagnosisType.MAIN_DIAGNOSIS + "'", function (resultDiagSch) {
                                                if (resultDiagSch != null) {
                                                    if (resultDiagSch.DiagnoseID == '' && resultDiagSch.DiagnosisText != '') {
                                                        $('#<%:txtDiagnoseText.ClientID %>').val(resultDiagSch.DiagnosisText);
                                                        $('#<%:txtDiagnoseCode.ClientID %>').val('');
                                                        $('#<%:txtDiagnoseName.ClientID %>').val('');
                                                    }
                                                    else if (resultDiagSch.DiagnoseID != '') {
                                                        $('#<%:txtDiagnoseText.ClientID %>').val(resultDiagSch.DiagnoseName);
                                                        $('#<%:txtDiagnoseCode.ClientID %>').val(resultDiagSch.DiagnoseID);
                                                        $('#<%:txtDiagnoseName.ClientID %>').val(resultDiagSch.DiagnoseName);
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    }
                    else {
                        if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                            $('#<%:txtNoSEP.ClientID %>').val('');
                        }
                    }

                    cboReferral.SetValue(result.GCReferrerGroup);
                    $('#<%:hdnReferrerID.ClientID %>').val(result.ReferrerID);
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ReferrerParamedicID);
                    if (result.ReferrerID != 0 && result.ReferrerID != null && result.ReferrerID != "") {
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ReferrerCode);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ReferrerName);
                    } else {
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ReferrerParamedicCode);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ReferrerParamedicName);
                    }

                    if (result.ReferenceNo.includes('|')) {
                        $('#<%:txtReferralNo.ClientID %>').val(result.ReferenceNo.split('|')[0]);
                    }
                    else {
                        $('#<%:txtReferralNo.ClientID %>').val(result.ReferenceNo);
                    }

                    if (result.IsReferralVisit == true) {
                        $('#<%:hdnIsReferralVisit.ClientID %>').val("1");
                    }
                    else {
                        $('#<%:hdnIsReferralVisit.ClientID %>').val("0");
                    }

                    var oGCResultDeliveryPlan = result.GCResultDeliveryPlan;
                    var oResultDeliveryPlan = result.ResultDeliveryPlan;
                    var oResultDeliveryPlanOthers = result.ResultDeliveryPlanOthers;

                    cboResultDeliveryPlan.SetEnabled(false);
                    if (oGCResultDeliveryPlan != null && oGCResultDeliveryPlan != '') {
                        cboResultDeliveryPlan.SetValue(oGCResultDeliveryPlan);
                        if (oGCResultDeliveryPlan == "X546^999") {
                            $('#<%=txtResultDeliveryPlanOthers.ClientID %>').val(oResultDeliveryPlanOthers);
                            $('#<%=txtResultDeliveryPlanOthers.ClientID %>').removeAttr('readonly');
                        } else {
                            $('#<%=txtResultDeliveryPlanOthers.ClientID %>').val("");
                            $('#<%:txtResultDeliveryPlanOthers.ClientID %>').attr('readonly', 'true');
                        }
                    } else {
                        cboResultDeliveryPlan.SetValue("");
                        $('#<%=txtResultDeliveryPlanOthers.ClientID %>').val("");
                        $('#<%:txtResultDeliveryPlanOthers.ClientID %>').attr('readonly', 'true');
                    }

                }
                else {
                    $('#<%:hdnAppointmentID.ClientID %>').val('');
                    $('#<%:txtAppointmentNo.ClientID %>').val('');
                    $('#<%:txtQueueNo.ClientID %>').val('');
                    $('#<%:lblMRN.ClientID %>').attr('class', 'lblLink lblMandatory');
                    $('#<%:txtMRN.ClientID %>').removeAttr('readonly');
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                    $('#<%:hdnIsServiceUnitHasParamedic.ClientID %>').val('0');
                    $('#<%:hdnIsServiceUnitHasVisitType.ClientID %>').val('0');
                    cboSpecialty.SetValue('');
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                    $('#<%:hdnVisitTypeID.ClientID %>').val('');
                    $('#<%:txtVisitTypeCode.ClientID %>').val('');
                    $('#<%:txtVisitTypeName.ClientID %>').val('');
                    $('#<%:hdnMRN.ClientID %>').val('');
                    $('#<%:txtMRN.ClientID %>').val('');
                    $('#<%:txtPatientName.ClientID %>').val('');
                    $('#<%:txtPreferredName.ClientID %>').val('');
                    $('#<%:txtGender.ClientID %>').val('');
                    $('#<%:txtDOB.ClientID %>').val('');
                    $('#<%:txtAgeInYear.ClientID %>').val('');
                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                    $('#<%:txtAgeInDay.ClientID %>').val('');
                    $('#<%:txtAddress.ClientID %>').val('');
                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                    $('#<%:txtPatientNotes.ClientID %>').val('');

                    cboReferral.SetValue("");
                    $('#<%:hdnReferrerID.ClientID %>').val("0");
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val("0");
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val("");
                    $('#<%:txtReferralDescriptionName.ClientID %>').val("");
                    $('#<%:txtReferralNo.ClientID %>').val("");
                    $('#<%:hdnIsReferralVisit.ClientID %>').val("0");

                    cboResultDeliveryPlan.SetEnabled(true);
                    cboResultDeliveryPlan.SetValue("");
                    $('#<%=txtResultDeliveryPlanOthers.ClientID %>').val("");
                    $('#<%:txtResultDeliveryPlanOthers.ClientID %>').attr('readonly', 'true');

                    cboRegistrationPayer.SetValue("<%:GetCustomerTypePersonal() %>");
                    setTblPayerCompanyVisibility();
                    $('#<%:hdnPayerID.ClientID %>').val('');
                    $('#<%:hdnContractID.ClientID %>').val('');
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                    $('#<%:txtPayerCompanyName.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                    $('#<%:txtParticipantNo.ClientID %>').val('');
                    $('#<%:txtEmployeeCode.ClientID %>').val('');
                    $('#<%:txtEmployeeCode.ClientID %>').val('');
                    $('#<%:txtCoverageLimit.ClientID %>').val('');
                    $('#<%=chkIsUsingCOB.ClientID %>').prop('checked', false);
                    $('#<%=chkIsCoverageLimitPerDay.ClientID %>').prop('checked', false);
                    $('#<%:txtContractNo.ClientID %>').val('');
                    $('#<%:txtContractPeriod.ClientID %>').val('');
                }
            });
        }

        function onRegistrationBPJSNoChanged(value) {
            var filterExpression = "VisitID = " + value + "";
            Methods.getObject('GetConsultVisitList', filterExpression, function (result) {
                if (result != null) {
                    if (result.RegistrationID != "") {
                        var filterExpressionBPJS = "RegistrationID = " + result.RegistrationID + "";

                        Methods.getObject('GetvRegistrationBPJSList', filterExpressionBPJS, function (resultBPJS) {
                            if (resultBPJS != null) {
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(resultBPJS.KodeRujukan);
                                $('#<%:txtReferralDescriptionName.ClientID %>').val(resultBPJS.NamaRujukan);
                                $('#<%:txtReferralNo.ClientID %>').val(resultBPJS.NoRujukan);
                                $('#<%:txtDiagnoseCode.ClientID %>').val(resultBPJS.KodeDiagnosa);
                                $('#<%:txtDiagnoseName.ClientID %>').val(resultBPJS.NamaDiagnosa);
                                $('#<%:txtDiagnoseText.ClientID %>').val(resultBPJS.NamaDiagnosa);
                                if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                                    $('#<%:txtNoSEP.ClientID %>').val('');
                                }
                            }
                            else {
                                if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                                    $('#<%:txtNoSEP.ClientID %>').val('');
                                }
                            }
                        });
                    }
                    else {
                        if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                            $('#<%:txtNoSEP.ClientID %>').val('');
                        }
                    }
                }
                else {
                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'DIAGNOSTIC') {
                        $('#<%:txtNoSEP.ClientID %>').val('');
                    }
                }
            });
        }
        //#endregion

        function oncboResultDeliveryPlanValueChanged(s) {
            var oValue = cboResultDeliveryPlan.GetValue();
            if (oValue == "X546^999") {
                $('#<%:txtResultDeliveryPlanOthers.ClientID %>').val("");
                $('#<%=txtResultDeliveryPlanOthers.ClientID %>').removeAttr('readonly');
            } else {
                $('#<%:txtResultDeliveryPlanOthers.ClientID %>').val("");
                $('#<%:txtResultDeliveryPlanOthers.ClientID %>').attr('readonly', 'true');
            }
        }

        //#region Reservation No
        function getReservationNoFilterExpression() {
            var filterExpression = "GCReservationStatus NOT IN ('" + Constant.BedReservation.CANCELLED + "','" + Constant.BedReservation.COMPLETE + "')";
            return filterExpression;
        }

        $('#<%:lblReservationID.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('bedreservation', getReservationNoFilterExpression(), function (value) {
                $('#<%:txtReservationNo.ClientID %>').val(value);
                onTxtReservationNoChanged(value);
            });
        });

        function onTxtReservationNoChanged(value) {
            var filterExpression = getReservationNoFilterExpression() + " AND ReservationNo = '" + value + "'";
            Methods.getObject('GetvBedReservationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:lblMRN.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtMRN.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtReservationNo.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:hdnReservationID.ClientID %>').val(result.ReservationID);
                    $('#<%:txtReservationNo.ClientID %>').val(result.ReservationNo);
                    $('#<%:hdnMRN.ClientID %>').val(result.MRN);
                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                    $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                    $('#<%:txtPreferredName.ClientID %>').val(result.PreferredName);
                    $('#<%:txtGender.ClientID %>').val(result.Gender);
                    $('#<%:txtDOB.ClientID %>').val(result.cfDateOfBirthInString);
                    $('#<%:txtAgeInYear.ClientID %>').val(result.AgeInYear);
                    $('#<%:txtAgeInMonth.ClientID %>').val(result.AgeInMonth);
                    $('#<%:txtAgeInDay.ClientID %>').val(result.AgeInDay);
                    $('#<%:txtAddress.ClientID %>').val(result.StreetName);
                    $('#<%:hdnClassID.ClientID %>').val(result.ClassID);
                    $('#<%:txtClassCode.ClientID %>').val(result.ClassCode);
                    $('#<%:txtClassName.ClientID %>').val(result.ClassName);
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    $('#<%:hdnRoomID.ClientID %>').val(result.RoomID);
                    $('#<%:txtRoomName.ClientID %>').val(result.RoomName);
                    $('#<%:txtRoomCode.ClientID %>').val(result.RoomCode);
                    if (result.MRN == 0) {
                        showToast('Warning', '<%:GetErrorMessageHasMedicalNo() %>');
                        isOpenPatientIdentityPopupFromAppointment = true;
                        var id = 'rev|' + result.ReservationID;
                        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/PatientEntryCtl.ascx");
                        openUserControlPopup(url, id, 'Patient Identity', 1100, 550, 'patientIdentity');
                    }
                    else {
                        $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                        onTxtMRNChanged($('#<%:txtMRN.ClientID %>').val());
                    }
                    $('#<%:txtBedCode.ClientID %>').val(result.BedCode);
                    $('#<%:hdnChargeClassID.ClientID %>').val(result.ChargeClassID);
                    $('#<%:txtChargeClassCode.ClientID %>').val(result.ChargeClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ChargeClassName);
                }
                else {
                    $('#<%:hdnReservationID.ClientID %>').val('');
                    $('#<%:txtReservationNo.ClientID %>').val('');
                    $('#<%:hdnMRN.ClientID %>').val('');
                    $('#<%:txtMRN.ClientID %>').val('');
                    $('#<%:txtPatientName %>').val('');
                    $('#<%:txtPreferredName.ClientID %>').val('');
                    $('#<%:txtGender.ClientID %>').val('');
                    $('#<%:txtDOB.ClientID %>').val('');
                    $('#<%:txtAgeInYear.ClientID %>').val('');
                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                    $('#<%:txtAgeInDay.ClientID %>').val('');
                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                    $('#<%:txtPatientNotes.ClientID %>').val('');
                    $('#<%:txtAddress.ClientID %>').val('');
                    $('#<%:hdnClassID.ClientID %>').val('');
                    $('#<%:txtClassCode.ClientID %>').val('');
                    $('#<%:txtClassName.ClientID %>').val('');
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                    $('#<%:hdnRoomID.ClientID %>').val('');
                    $('#<%:txtRoomName.ClientID %>').val('');
                    $('#<%:txtRoomCode.ClientID %>').val('');
                    $('#<%:txtBedCode.ClientID %>').val('');
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Registration No
        function getRegistrationNoFilterExpression() {
            var filterExpression = "<%:OnGetRegistrationNoFilterExpression() %>";
            return filterExpression;
        }

        $('#lblNoReg.lblLink').live('click', function () {
            openSearchDialog('registration', getRegistrationNoFilterExpression(), function (value) {
                $('#<%:txtRegistrationNo.ClientID %>').val(value);
                onTxtRegistrationNoChanged(value);
            });
        });
        $('#<%:txtRegistrationNo.ClientID %>').live('change', function () {
            onTxtRegistrationNoChanged($(this).val());
        });
        function onTxtRegistrationNoChanged(value) {
            var filterExpression = getRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
            Methods.getObject('GetvRegistration1List', filterExpression, function (result) {
                if (result != null) {
                    var isVIP = result.IsVIP;
                    var vipGroup = result.cfVIPGroup;
                    onLoadObject(value);
                    if (isVIP == false) {
                        $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                    }
                    else {
                        $('#<%:tdIsVIP.ClientID %>').removeAttr('style');
                        document.getElementById('hdnTitleVIP').title = vipGroup;
                    }
                }
                else {
                    $('#<%:txtRegistrationNo.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Mother Registration No
        function getMotherRegistrationNoFilterExpression() {
            var filterExpression = "<%:OnGetMotherRegistrationNoFilterExpression() %>";
            return filterExpression;
        }

        $('#<%=lblMotherRegNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('consultvisit', getMotherRegistrationNoFilterExpression(), function (value) {
                $('#<%=hdnMotherVisitID.ClientID %>').val(value);
                onTxtMotherVisitIDChanged(value);
            });
        });

        function onTxtMotherVisitIDChanged(value) {
            var filterExpression = getMotherRegistrationNoFilterExpression() + " AND VisitID = '" + value + "'";
            Methods.getObject('GetvConsultVisitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnMotherVisitID.ClientID %>').val(result.VisitID);
                    $('#<%:hdnMotherMRN.ClientID %>').val(result.MRN);
                    $('#<%:hdnMotherName.ClientID %>').val(result.PatientName);
                    $('#<%:txtMotherRegNo.ClientID %>').val(result.RegistrationNo);
                    $('#<%:hdnChargeClassID.ClientID %>').val(result.ChargeClassID);
                    $('#<%:hdnChargeClassBPJSCode.ClientID %>').val(result.ChargeClassBPJSCode);
                    $('#<%:hdnChargeClassBPJSType.ClientID %>').val(result.ChargeClassBPJSType);
                    $('#<%:txtChargeClassCode.ClientID %>').val(result.ChargeClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ChargeClassName);
                }
                else {
                    $('#<%:hdnMotherName.ClientID %>').val('');
                    $('#<%:hdnMotherVisitID.ClientID %>').val('');
                    $('#<%:hdnMotherMRN.ClientID %>').val('');
                    $('#<%:hdnClassID.ClientID %>').val('');
                    $('#<%:txtClassCode.ClientID %>').val('');
                    $('#<%:txtClassName.ClientID %>').val('');
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSCode.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSType.ClientID %>').val('');
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region MRN
        $('#<%:lblMRN.ClientID %>.lblLink').live('click', function () {
            var filterExpression = "";
            if ($('#<%:hdnRMPatientWalkin.ClientID %>').val() != '' && $('#<%:hdnRMPatientWalkin.ClientID %>').val() != '0') {
                filterExpression = "MedicalNo != '" + $('#<%:hdnRMPatientWalkin.ClientID %>').val() + "' AND IsDeleted = 0";
            }
            openSearchDialog($('#<%:hdnPatientSearchDialogType.ClientID %>').val(), filterExpression, function (value) {
                $('#<%:txtMRN.ClientID %>').val(value);
                onTxtMRNChanged(value);
            });
        });

        $('#<%:txtMRN.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val());
        });
        function onTxtMRNChanged(value) {
            var mrn = FormatMRN(value);
            var filterExpression = "MedicalNo = '" + mrn + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                SetPatientInformationToControl(result);

                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                var noPeserta = result.CorporateAccountNo;
                if (regID == "" || regID == "0") {
                    if (result.GCCustomerType != "") {
                        cboRegistrationPayer.SetValue(result.GCCustomerType);
                        setTblPayerCompanyVisibility();

                        if (result.BusinessPartnerID != null && result.BusinessPartnerID != 0) {
                            var filterCustomer = "BusinessPartnerID = " + result.BusinessPartnerID;
                            getPayerCompany(filterCustomer);
                        }
                    }
                }

                onCheckCustomerMember($('#<%:hdnPayerID.ClientID %>').val(), mrn, noPeserta);
            });
        }
        //#endregion

        //#region Guest
        $('#<%:lblGuestNo.ClientID %>.lblLink').live('click', function () {
            var filterExpression = "IsDeleted = 0";
            openSearchDialog($('#<%:hdnGuestSearchDialogType.ClientID %>').val(), filterExpression, function (value) {
                $('#<%:hdnGuestID.ClientID %>').val(value);
                onGuestIDChanged(value);
            });
        });

        function onGuestIDChanged(value) {
            var filterExpression = "GuestID = '" + value + "'";
            Methods.getObject('GetvGuestList', filterExpression, function (result) {
                setGuestInformationToControl(result);
            });
        }

        $('#<%:txtGuestNo.ClientID %>').live('change', function () {
            onTxtGuestNoChanged($(this).val());
        });

        function onTxtGuestNoChanged(value) {
            var filterExpression = "GuestNo = '" + value + "'";
            Methods.getObject('GetvGuestList', filterExpression, function (result) {
                setGuestInformationToControl(result);
            });
        }
        //#endregion

        //#region NHSRegistrationNo
        $('#<%:txtNHSRegistrationNo.ClientID %>').live('change', function () {
            onTxtNHSRegistrationNo($(this).val());
        });

        function onTxtNHSRegistrationNo(value) {
            var filterExpression = "NHSRegistrationNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                SetPatientInformationToControl(result);

                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                if (regID == "" || regID == "0") {
                    if (result.GCCustomerType != "") {
                        cboRegistrationPayer.SetValue(result.GCCustomerType);
                        setTblPayerCompanyVisibility();

                        if (result.BusinessPartnerID != null && result.BusinessPartnerID != 0) {
                            var filterCustomer = "BusinessPartnerID = " + result.BusinessPartnerID;
                            getPayerCompany(filterCustomer);
                        }
                    }
                }
            });
        }
        //#endregion

        //#region PatientInhealthNo
        $('#<%:txtInhealthParticipantNo.ClientID %>').live('change', function () {
            ontxtInhealthParticipantNoChanged($(this).val());
        });

        function ontxtInhealthParticipantNoChanged(value) {
            var filterExpression = "InhealthParticipantNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                SetPatientInformationToControl(result);

                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                if (regID == "" || regID == "0") {
                    if (result.GCCustomerType != "") {
                        cboRegistrationPayer.SetValue(result.GCCustomerType);
                        setTblPayerCompanyVisibility();

                        if (result.BusinessPartnerID != null && result.BusinessPartnerID != 0) {
                            var filterCustomer = "BusinessPartnerID = " + result.BusinessPartnerID;
                            getPayerCompany(filterCustomer);
                        }
                    }
                }
            });
        }
        //#endregion

        //#region GenerateSEP
        $('#<%=btnGenerateSEP.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsGenerateSEP', 'mpGenerateSEP')) {
                if ($('#<%=txtNoSEP.ClientID %>').val() == '' && $('#<%=txtNHSRegistrationNo.ClientID %>').val() != '') {
                    var noKartu = $('#<%=txtNHSRegistrationNo.ClientID %>').val();
                    var date = Methods.getDatePickerDate($('#<%=txtRegistrationDate.ClientID %>').val());
                    var tglSEP = Methods.dateToYMD(date) + ' ' + $('#<%=txtRegistrationHour.ClientID %>').val();
                    date = Methods.getDatePickerDate($('#<%=txtRegistrationDate.ClientID %>').val());
                    var jnsPelayanan = '';

                    var klsRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();

                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'INPATIENT') {
                        jnsPelayanan = '1';
                    }
                    else {
                        jnsPelayanan = '2';
                    }

                    //////  Ditutup oleh RN (20190528) karna mengikuti cara baca binding kelasSEP yg ada di menu right panel Task - SEP

                    //////                    var klsRawat = '';

                    //////                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'INPATIENT') {
                    //////                        var tempKlsRawat = $('#<%=txtSEPClass.ClientID %>').val();
                    //////                        var hakKelas = tempKlsRawat.split(" - ")[0];
                    //////                        klsRawat = $('#<%:hdnChargeClassBPJSType.ClientID %>').val();
                    //////                        if ($('#<%:hdnChargeClassBPJSType.ClientID %>').val() >= hakKelas) {
                    //////                            klsRawat = $('#<%:hdnChargeClassBPJSType.ClientID %>').val();
                    //////                        }
                    //////                        else {
                    //////                            klsRawat = hakKelas;
                    //////                        }
                    //////                        jnsPelayanan = '1';
                    //////                    }
                    //////                    else {
                    //////                        var tempKlsRawat = $('#<%=txtSEPClass.ClientID %>').val();
                    //////                        klsRawat = tempKlsRawat.split(" - ")[0];
                    //////                        jnsPelayanan = '2';
                    //////                    }

                    var medicalNo = $('#<%=txtMRN.ClientID %>').val();
                    var asalRujukan = '1';
                    if (cboReferral.GetValue() == Constant.ReferrerGroup.RUMAH_SAKIT) {
                        asalRujukan = '2';
                    }
                    var dateRujukan = $('#<%:hdnTglRujukan.ClientID %>').val();
                    var noRujukan = $('#<%=txtReferralNo.ClientID %>').val();
                    if ($('#<%=hdnKodeProvider.ClientID %>').val() != "") {
                        var ppkRujukan = $('#<%=hdnKodeProvider.ClientID %>').val();
                    } else {
                        var ppkRujukan = $('#<%=txtReferralNo.ClientID %>').val();
                    }
                    var catatan = $('#<%=txtVisitNotes.ClientID %>').val();
                    var diagnosa = $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val();
                    var bpjsPoli = $('#<%=hdnBPJSPoli.ClientID %>').val();
                    var poliTujuan = bpjsPoli.split("|")[0];
                    var poliEksekutif = $('#<%:hdnIsPoliExecutive.ClientID %>').val();
                    var cob = '0';
                    if ($('#<%:chkIsUsingCOB.ClientID %>').is(':checked')) {
                        cob = '1';
                    }

                    var noSKDP = $('#<%:hdnNoSKDP.ClientID %>').val();
                    var kodeDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
                    var katarak = $('#<%:hdnIsCataract.ClientID %>').val();

                    var lakaLantas = "0";
                    var lokasiLaka = "";
                    var penjamin = "";
                    var suplesi = "0";
                    if ($('#<%:chkIsSuplesi.ClientID %>').is(':checked')) {
                        suplesi = '1';
                    }
                    var noSepSuplesi = $('#<%=txtNoSEPSuplesi.ClientID %>').val();
                    var kodeKecamatan = $('#<%:hdnKodeKecamatanBPJS.ClientID %>').val();
                    var kodeKabupaten = $('#<%:hdnKodeKabupatenBPJS.ClientID %>').val();
                    var kodePropinsi = $('#<%:hdnKodePropinsiBPJS.ClientID %>').val();

                    if (cboVisitReason.GetValue() == Constant.VisitReason.ACCIDENT) {
                        if ($('#<%:chkBPJSAccidentPayer1.ClientID %>').is(':checked')) {
                            penjamin = '1';
                        }
                        if ($('#<%:chkBPJSAccidentPayer2.ClientID %>').is(':checked')) {
                            if (penjamin != '') {
                                penjamin = penjamin + ',' + '2';
                            }
                            else {
                                penjamin = '2';
                            }
                        }
                        if ($('#<%:chkBPJSAccidentPayer3.ClientID %>').is(':checked')) {
                            if (penjamin != '') {
                                penjamin = penjamin + ',' + '3';
                            }
                            else {
                                penjamin = '3';
                            }
                        }
                        if ($('#<%:chkBPJSAccidentPayer4.ClientID %>').is(':checked')) {
                            if (penjamin != '') {
                                penjamin = penjamin + ',' + '4';
                            }
                            else {
                                penjamin = '4';
                            }
                        }

                        lakaLantas = "1";
                        lokasiLaka = $('#<%=txtAccidentLocation.ClientID %>').val();
                    }
                    else {
                        penjamin = '1';
                    }

                    var mobilePhoneNo = $('#<%=hdnMobilePhoneNo1.ClientID %>').val();

                    BPJSService.generateNoSEP(noKartu, tglSEP, dateRujukan, noRujukan, ppkRujukan, jnsPelayanan, catatan, diagnosa, poliTujuan, klsRawat, lakaLantas, lokasiLaka, medicalNo, asalRujukan, cob, poliEksekutif, mobilePhoneNo, penjamin,
                    katarak, suplesi, noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, noSKDP, kodeDPJP, function (result) {
                        try {
                            var obj = result.split('|');
                            if (obj[0] == "1") {
                                $('#<%=txtNoSEP.ClientID %>').val(obj[1]);
                                cbpBPJSProcess.PerformCallback('save');
                            }
                            else {
                                showToast('PEMBUATAN SEP : GAGAL', obj[2]);
                            }

                        } catch (err) {
                            showToast('PEMBUATAN SEP : GAGAL', err);
                            $('#<%=txtNoSEP.ClientID %>').val('');
                        }
                    });
                }
            }
        });
        //#endregion

        //#region BPJSReferral
        $('#<%:chkIsKontrolBPJS.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=trSurkonBPJS.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trSurkonBPJS.ClientID %>').attr('style', 'display:none');
            }
        });

        $('#<%:lblSurkonBPJS.ClientID %>.lblLink').live('click', function () {
            var noRujukan = $('#<%=txtReferralNo.ClientID %>').val();
            var noKartu = $('#<%=txtNHSRegistrationNo.ClientID %>').val();
            var asalRujukan = cboReferral.GetValue();

            if (noKartu != '') {
                isAfterSearchBPJSurkon = true;
                openUserControlPopup("~/Libs/Program/Module/PatientManagement/Registration/SearchBPJSSurkonCtl.ascx", noKartu + '|' + asalRujukan, "Data Rujukan Berdasarkan Nomor Kartu", 900, 550);
            }
            else {
                showToast("DATA SURAT KONTROL : No. Kartu", 'Pencarian Data Surat Kontrol berdasarkan Nomor Kartu harus menyertai nomor kartu peserta!');
            }
        });

        $('#<%=btnReferral.ClientID %>').live('click', function () {
            var noRujukan = $('#<%=txtReferralNo.ClientID %>').val();
            var noKartu = $('#<%=txtNHSRegistrationNo.ClientID %>').val();
            var asalRujukan = cboReferral.GetValue();

            if (!cboReferral.GetValue())
                showToast("DATA RUJUKAN", 'Asal Rujukan Pasien BPJS Harus diisi!');
            else {
                if (noRujukan == '') {
                    if (noKartu != '') {
                        isAfterSearchBPJSReferral = true;
                        openUserControlPopup("~/Libs/Program/Module/PatientManagement/Registration/SearchBPJSReferralCtl.ascx", noKartu + '|' + asalRujukan, "Data Rujukan Berdasarkan Nomor Kartu", 900, 550);
                    }
                    else {
                        showToast("DATA RUJUKAN : No. Kartu", 'Pencarian Data Rujukan berdasarkan Nomor Kartu harus menyertai nomor kartu peserta!');
                    }
                }
                else {
                    if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                        OnGetBPJSReferralInformation(noRujukan, asalRujukan);
                    } else {
                        OnGetBPJSReferralInformationAPI(noRujukan, asalRujukan);
                    }
                }
            }
        });

        //#region OnGetBPJSReferralInformation
        function OnGetBPJSReferralInformation(noRujukan, asalRujukan) {
            BPJSService.getRujukan(noRujukan, asalRujukan, function (result) {
                try {
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        var oData = jQuery.parseJSON(resultInfo[1]);

                        var kodeUnit = oData.response.rujukan.poliRujukan.kode.trim();
                        var namaUnit = oData.response.rujukan.poliRujukan.nama.trim();
                        var noPeserta = oData.response.rujukan.peserta.noKartu;

                        var kodeProvider = oData.response.rujukan.provPerujuk.kode.trim();
                        $('#<%=hdnKodeProvider.ClientID %>').val(kodeProvider);
                        if ($('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.INPATIENT && $('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.EMERGENCY) {
                            $('#<%:trIsKontrolBPJS.ClientID %>').removeAttr('style');
                        }
                        var namaProvider = oData.response.rujukan.provPerujuk.nama.trim();
                        var tipeProvider = cboReferral.GetValue();
                        var filterExpression = getReferralDescriptionFilterExpression() + " AND IsDeleted = 0 AND CommCode = '" + kodeProvider + "'";
                        Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.BusinessPartnerCode);
                                $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                            }
                            else {
                                $('#<%:hdnReferrerID.ClientID %>').val('');
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                                $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                                BPJSService.insertFaskes(kodeProvider, namaProvider, tipeProvider, function (result2) {
                                    if (result2 != null) {
                                        try {
                                            var result2 = result2.split('|');
                                            if (result2[0] == "1") {
                                                var oDataNew = jQuery.parseJSON(result2[1]);
                                                $('#<%:hdnReferrerID.ClientID %>').val(oDataNew.BusinessPartnerID);
                                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(oDataNew.BusinessPartnerCode);
                                                $('#<%:txtReferralDescriptionName.ClientID %>').val(oDataNew.BusinessPartnerName);
                                            }
                                            else {
                                                showToast('DATA RUJUKAN : ERROR', result2Info[2]);
                                                GetSearchPeserta();
                                            }
                                        } catch (err) {
                                            alert(err);
                                        }
                                    }
                                });
                            }
                        });

                        $('#<%=hdnTglRujukan.ClientID %>').val(oData.response.rujukan.tglKunjungan.trim());
                        $('#<%=txtTglRujukan.ClientID %>').val(Methods.BPJSDateStringToDate(oData.response.rujukan.tglKunjungan.trim()));

                        var isValid = "1";
                        var isFirstVisit = "1";

                        //Check Visit Count using current referral number
                        var filterExpression1 = "NoRujukan = '" + noRujukan + "' AND AppointmentNo IS NOT NULL";
                        var filterExpression2 = "NoRujukan = '" + noRujukan + "'";
                        Methods.getObject('GetvRegistrationBPJSInfoList', filterExpression1, function (oRegistrationBPJS) {
                            if (oRegistrationBPJS != null) {
                                isFirstVisit = "0";
                                isAfterSearchBPJSReferral = true;
//                                openUserControlPopup("~/Libs/Program/Module/PatientManagement/Registration/FollowupBPJSReferralCtl.ascx", noPeserta + '|' + asalRujukan + '|' + noRujukan, "Kontrol Kunjungan Berikutnya", 1200, 550);
                                showToast("DATA RUJUKAN", 'Nomor Rujukan ' + noRujukan + ' sudah pernah digunakan.');
                            } else {
                                Methods.getObject('GetvRegistrationBPJSInfoList', filterExpression2, function (oRegistrationBPJS2) {
                                    if (oRegistrationBPJS2 != null) {
                                        isFirstVisit = "0";
                                        isAfterSearchBPJSReferral = true;
                                        showToast("DATA RUJUKAN", 'Nomor Rujukan ' + noRujukan + ' sudah pernah digunakan.');
                                    }
                                });
                            }
                        });

                        if ($('#<%:txtServiceUnitCode.ClientID %>').val() != "") {
                            var referenceInfo = $('#<%=hdnBPJSPoli.ClientID %>').val().split('|');
                            var kodePoli = referenceInfo[0].trim();
                            var namaPoli = referenceInfo[1].trim();
                            if (kodeUnit != kodePoli) {
                                showToastConfirmation("DATA RUJUKAN, Poli Rujukan dari Nomor Rujukan " + noRujukan + " tidak sesuai dengan pendaftaran pasien ! (" + kodeUnit + " - " + namaUnit + "), Lanjutkan?", function (result) {
                                    if (result) {
                                        isValid = "1";
                                    } else {
                                        isValid = "0";
                                        $('#<%:hdnReferrerID.ClientID %>').val('');
                                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                                        $('#<%=txtReferralNo.ClientID %>').val('');
                                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                                        $('#<%=txtDiagnoseText.ClientID %>').val('');
                                        $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                                    }
                                });
                            }
                        }
                        else {
                            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
                            var filterExpressionSU = "BPJSPoli = '" + kodeUnit + '|' + namaUnit + "' AND ServiceUnitID IN (SELECT ServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = '" + deptID + "' AND IsDeleted = 0 AND IsUsingRegistration = 1)";
                            Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpressionSU, function (oServiceUnit) {
                                if (oServiceUnit != null) {
                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(oServiceUnit.HealthcareServiceUnitID);
                                    $('#<%:txtServiceUnitCode.ClientID %>').val(oServiceUnit.ServiceUnitCode);
                                    $('#<%:txtServiceUnitName.ClientID %>').val(oServiceUnit.ServiceUnitName);
                                    $('#<%=hdnBPJSPoli.ClientID %>').val(oServiceUnit.BPJSPoli);
                                    $('#<%:txtSubSpesialisCode.ClientID %>').val(kodeUnit);
                                    $('#<%:txtSubSpesialisName.ClientID %>').val(namaUnit);

                                    if (oServiceUnit.GCClinicGroup == Constant.ClinicGroup.BPJS)
                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                    else
                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');
                                }
                                else {
                                    var filterExpressionSubKlinik = "BPJSPoli = '" + kodeUnit + '|' + namaUnit + "'";
                                    Methods.getObject('GetHealthcareServiceUnitSpecialtyList', filterExpressionSubKlinik, function (oSubKlinik) {
                                        if (oSubKlinik != null) {
                                            Methods.getObject('GetvHealthcareServiceUnitCustomList', 'HealthcareServiceUnitID = ' + oSubKlinik.HealthcareServiceUnitID, function (oServiceUnitSub) {
                                                if (oServiceUnitSub != null) {
                                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(oServiceUnitSub.HealthcareServiceUnitID);
                                                    $('#<%:txtServiceUnitCode.ClientID %>').val(oServiceUnitSub.ServiceUnitCode);
                                                    $('#<%:txtServiceUnitName.ClientID %>').val(oServiceUnitSub.ServiceUnitName);
                                                    $('#<%=hdnBPJSPoli.ClientID %>').val(oServiceUnitSub.BPJSPoli);
                                                    $('#<%:txtSubSpesialisCode.ClientID %>').val(kodeUnit);
                                                    $('#<%:txtSubSpesialisName.ClientID %>').val(namaUnit);

                                                    if (oServiceUnitSub.GCClinicGroup == Constant.ClinicGroup.BPJS)
                                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                                    else
                                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');
                                                }
                                                else {
                                                    showToast("SILAHKAN COBA LAGI", 'Unit pelayanan rujukan bukan ada di department ' + deptID);
                                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                                    $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                                                    $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                                }
                                            });
                                        } 
                                        else {
                                            showToast("SILAHKAN COBA LAGI", 'Unit pelayanan rujukan bukan ada di department ' + deptID);
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                            $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                            $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                            $('#<%:txtServiceUnitName.ClientID %>').val('');
                                            $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                        }
                                    });
                                }
                            });
                        }

                        if ($('#<%:txtNHSRegistrationNo.ClientID %>').val() == '') {
                            $('#<%:txtNHSRegistrationNo.ClientID %>').val(noPeserta);
                            $('#<%:txtNHSRegistrationNo.ClientID %>').change();
                            if ($('#<%:txtNHSRegistrationNo.ClientID %>').val() == "") {
                                $('#<%=txtReferralNo.ClientID %>').val('')
                                isValid = "0";
                                BPJSFailTrigger('DATA RUJUKAN : GAGAL', "Pasien dengan Nomor Kartu " + noPeserta + " belum terdaftar sebagai pasien");
                            }
                        }
                        else {
                            var nokartu = $('#<%:txtNHSRegistrationNo.ClientID %>').val();
                            if (nokartu != noPeserta) {
                                $('#<%=txtReferralNo.ClientID %>').val('')
                                isValid = "0";
                                BPJSFailTrigger('DATA RUJUKAN : GAGAL', "Data Rujukan tidak sesuai dengan Nomor Peserta " + nokartu);
                                GetSearchPeserta();
                            }
                        }


                        if (isValid == "1") {
                            var filterExpressionDiagnose = "INACBGLabel = '" + oData.response.rujukan.diagnosa.kode + "'";
                            Methods.getObject('GetDiagnoseList', filterExpressionDiagnose, function (resultCheck) {
                                if (resultCheck != null) {
                                    $('#<%=txtDiagnoseCode.ClientID %>').val(resultCheck.DiagnoseID);
                                    $('#<%=txtDiagnoseName.ClientID %>').val(resultCheck.DiagnoseName);
                                    $('#<%=txtDiagnoseText.ClientID %>').val(oData.response.rujukan.diagnosa.nama);
                                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val(resultCheck.INACBGLabel);

                                    onTxtDiagnoseCodeChanged(resultCheck.DiagnoseID);
                                }
                                else {
                                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                                    $('#<%=txtDiagnoseText.ClientID %>').val('');
                                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                                }
                            });
                            $('#<%=txtVisitNotes.ClientID %>').val(oData.response.rujukan.keluhan);
                        }
                    }
                    else {
                        BPJSFailTrigger('DATA RUJUKAN : GAGAL', resultInfo[2]);
                        GetSearchPeserta();
                    }
                } catch (err) {
                    BPJSFailTrigger('DATA RUJUKAN : GAGAL', resultInfo[2]);
                    $('#<%=txtReferralNo.ClientID %>').val('');
                    GetSearchPeserta();
                }
            });
        }
        //#endregion

        //#region OnGetBPJSReferralInformationAPI
        function OnGetBPJSReferralInformationAPI(noRujukan, asalRujukan) {
            BPJSService.getRujukanMedinfrasAPI(noRujukan, asalRujukan, function (result) {
                try {
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        var oData = jQuery.parseJSON(resultInfo[1]);

                        var kodeUnit = oData.response.rujukan.poliRujukan.kode.trim();
                        var namaUnit = oData.response.rujukan.poliRujukan.nama.trim();
                        var noPeserta = oData.response.rujukan.peserta.noKartu;
                        var kodeProvider = oData.response.rujukan.provPerujuk.kode.trim();
                        $('#<%=hdnKodeProvider.ClientID %>').val(kodeProvider);
                        $('#<%:trIsKontrolBPJS.ClientID %>').removeAttr('style');
                        var namaProvider = oData.response.rujukan.provPerujuk.nama.trim();
                        var tipeProvider = cboReferral.GetValue();
                        var filterExpression = getReferralDescriptionFilterExpression() + " AND IsDeleted = 0 AND CommCode = '" + kodeProvider + "'";
                        Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.CommCode);
                                $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                            }
                            else {
                                $('#<%:hdnReferrerID.ClientID %>').val('');
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                                $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                                BPJSService.insertFaskes(kodeProvider, namaProvider, tipeProvider, function (result2) {
                                    if (result2 != null) {
                                        try {
                                            var result2 = result2.split('|');
                                            if (result2[0] == "1") {
                                                var oDataNew = jQuery.parseJSON(result2[1]);
                                                $('#<%:hdnReferrerID.ClientID %>').val(oDataNew.BusinessPartnerID);
                                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(oDataNew.BusinessPartnerCode);
                                                $('#<%:txtReferralDescriptionName.ClientID %>').val(oDataNew.BusinessPartnerName);
                                            }
                                            else {
                                                showToast('DATA RUJUKAN : ERROR', result2Info[2]);
                                                GetSearchPeserta();
                                            }
                                        } catch (err) {
                                            alert(err);
                                        }
                                    }
                                });
                            }
                        });

                        $('#<%=hdnTglRujukan.ClientID %>').val(oData.response.rujukan.tglKunjungan.trim());
                        $('#<%=txtTglRujukan.ClientID %>').val(Methods.BPJSDateStringToDate(oData.response.rujukan.tglKunjungan.trim()));

                        var isValid = "1";
                        var isFirstVisit = "1";

                        //Check Visit Count using current referral number
                        var filterExpression1 = "NoRujukan = '" + noRujukan + "' AND AppointmentNo IS NOT NULL";
                        var filterExpression2 = "NoRujukan = '" + noRujukan + "'";
                        Methods.getObject('GetvRegistrationBPJSInfoList', filterExpression1, function (oRegistrationBPJS) {
                            if (oRegistrationBPJS != null) {
                                isFirstVisit = "0";
                                isAfterSearchBPJSReferral = true;
//                                openUserControlPopup("~/Libs/Program/Module/PatientManagement/Registration/FollowupBPJSReferralCtl.ascx", noPeserta + '|' + asalRujukan + '|' + noRujukan, "Kontrol Kunjungan Berikutnya", 1200, 550);
                                showToast("DATA RUJUKAN", 'Nomor Rujukan ' + noRujukan + ' sudah pernah digunakan.');
                            } else {
                                Methods.getObject('GetvRegistrationBPJSInfoList', filterExpression2, function (oRegistrationBPJS2) {
                                    if (oRegistrationBPJS2 != null) {
                                        isFirstVisit = "0";
                                        isAfterSearchBPJSReferral = true;
                                        showToast("DATA RUJUKAN", 'Nomor Rujukan ' + noRujukan + ' sudah pernah digunakan.');
                                    }
                                });
                            }
                        });

                        if ($('#<%:txtServiceUnitCode.ClientID %>').val() != "") {
                            var referenceInfo = $('#<%=hdnBPJSPoli.ClientID %>').val().split('|');
                            if (referenceInfo != '') {
                                var kodePoli = referenceInfo[0].trim();
                                var namaPoli = referenceInfo[1].trim();
                                if (kodeUnit != kodePoli) {
                                    showToastConfirmation("DATA RUJUKAN, Poli Rujukan dari Nomor Rujukan " + noRujukan + " tidak sesuai dengan pendaftaran pasien ! (" + kodeUnit + " - " + namaUnit + "), Lanjutkan?", function (result) {
                                        if (result) {
                                            isValid = "1";
                                        } else {
                                            isValid = "0";
                                            $('#<%:hdnReferrerID.ClientID %>').val('');
                                            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                                            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                                            $('#<%=txtReferralNo.ClientID %>').val('');
                                            $('#<%=txtDiagnoseCode.ClientID %>').val('');
                                            $('#<%=txtDiagnoseName.ClientID %>').val('');
                                            $('#<%=txtDiagnoseText.ClientID %>').val('');
                                            $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                                        }
                                    });
                                }
                            }
                        }
                        else {
                            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
                            var filterExpressionSU = "BPJSPoli = '" + kodeUnit + '|' + namaUnit + "' AND DepartmentID = '" + deptID + "' AND IsDeleted = 0 AND IsUsingRegistration = 1";
                            Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpressionSU, function (oServiceUnit) {
                                if (oServiceUnit != null) {
                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(oServiceUnit.HealthcareServiceUnitID);
                                    $('#<%:txtServiceUnitCode.ClientID %>').val(oServiceUnit.ServiceUnitCode);
                                    $('#<%:txtServiceUnitName.ClientID %>').val(oServiceUnit.ServiceUnitName);
                                    $('#<%=hdnBPJSPoli.ClientID %>').val(oServiceUnit.BPJSPoli);
                                    $('#<%=hdnPoliRujukan.ClientID %>').val(oServiceUnit.BPJSPoli);
                                    $('#<%:txtSubSpesialisCode.ClientID %>').val(kodeUnit);
                                    $('#<%:txtSubSpesialisName.ClientID %>').val(namaUnit);

                                    if (oServiceUnit.GCClinicGroup == Constant.ClinicGroup.BPJS)
                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                    else
                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');

                                    //Get Healthcare Service Unit ID
                                    var filterExpressionHSU = "HealthcareID = '" + AppSession.healthcareID + "' AND ServiceUnitCode = '" + oServiceUnit.ServiceUnitCode + "'";
                                    Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpressionHSU, function (oHealthcareServiceUnit) {
                                        if (oHealthcareServiceUnit != null) {
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(oHealthcareServiceUnit.HealthcareServiceUnitID);
                                        }
                                    });
                                }
                                else {
                                    Methods.getObject('GetHealthcareServiceUnitSpecialtyList', filterExpressionSubKlinik, function (oSubKlinik) {
                                        if (oSubKlinik != null) {
                                            Methods.getObject('GetvHealthcareServiceUnitCustomList', 'HealthcareServiceUnitID = ' + oSubKlinik.HealthcareServiceUnitID, function (oServiceUnitSub) {
                                                if (oServiceUnitSub != null) {
                                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(oServiceUnitSub.HealthcareServiceUnitID);
                                                    $('#<%:txtServiceUnitCode.ClientID %>').val(oServiceUnitSub.ServiceUnitCode);
                                                    $('#<%:txtServiceUnitName.ClientID %>').val(oServiceUnitSub.ServiceUnitName);
                                                    $('#<%=hdnBPJSPoli.ClientID %>').val(oServiceUnitSub.BPJSPoli);
                                                    $('#<%:txtSubSpesialisCode.ClientID %>').val(kodeUnit);
                                                    $('#<%:txtSubSpesialisName.ClientID %>').val(namaUnit);

                                                    if (oServiceUnitSub.GCClinicGroup == Constant.ClinicGroup.BPJS)
                                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                                    else
                                                        $('#<%:hdnIsPoliExecutive.ClientID %>').val('1');
                                                }
                                                else {
                                                    showToast("SILAHKAN COBA LAGI", 'Unit pelayanan rujukan bukan ada di department ' + deptID);
                                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                                    $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                                                    $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                                }
                                            });
                                        }
                                        else {
                                            showToast("SILAHKAN COBA LAGI", 'Unit pelayanan rujukan bukan ada di department ' + deptID);
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                            $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                            $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                            $('#<%:txtServiceUnitName.ClientID %>').val('');
                                            $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                        }
                                    });
                                }
                            });
                        }

                        if ($('#<%:txtNHSRegistrationNo.ClientID %>').val() == '') {
                            $('#<%:txtNHSRegistrationNo.ClientID %>').val(noPeserta);
                            $('#<%:txtNHSRegistrationNo.ClientID %>').change();
                            if ($('#<%:txtNHSRegistrationNo.ClientID %>').val() == "") {
                                $('#<%=txtReferralNo.ClientID %>').val('')
                                isValid = "0";
                                BPJSFailTrigger('DATA RUJUKAN : GAGAL', "Pasien dengan Nomor Kartu " + noPeserta + " belum terdaftar sebagai pasien");
                            }
                        }
                        else {
                            var nokartu = $('#<%:txtNHSRegistrationNo.ClientID %>').val();
                            if (nokartu != noPeserta) {
                                $('#<%=txtReferralNo.ClientID %>').val('')
                                isValid = "0";

                                BPJSFailTrigger('DATA RUJUKAN : GAGAL', "Data Rujukan tidak sesuai dengan Nomor Peserta " + nokartu);
                                GetSearchPeserta();
                            }
                        }


                        if (isValid == "1") {
                            var filterExpressionDiagnose = "BPJSReferenceInfo = '" + oData.response.rujukan.diagnosa.kode + "'";
                            Methods.getObject('GetDiagnoseList', filterExpressionDiagnose, function (resultCheck) {
                                if (resultCheck != null) {
                                    $('#<%=txtDiagnoseCode.ClientID %>').val(resultCheck.DiagnoseID);
                                    $('#<%=txtDiagnoseName.ClientID %>').val(resultCheck.DiagnoseName);
                                    $('#<%=txtDiagnoseText.ClientID %>').val(oData.response.rujukan.diagnosa.nama);
                                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val(resultCheck.BPJSReferenceInfo);

                                    onTxtDiagnoseCodeChanged(resultCheck.DiagnoseID);
                                }
                                else {
                                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                                    $('#<%=txtDiagnoseText.ClientID %>').val('');
                                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                                }
                            });
                            $('#<%=txtVisitNotes.ClientID %>').val(oData.response.rujukan.keluhan);
                        }
                    }
                    else {
                        BPJSFailTrigger('DATA RUJUKAN : GAGAL', resultInfo[2]);
                        //                        GetSearchPeserta();
                    }
                } catch (err) {
                    BPJSFailTrigger('DATA RUJUKAN : GAGAL', resultInfo[2]);
                    $('#<%=txtReferralNo.ClientID %>').val('');
                    GetSearchPeserta();
                }
            });
        }
        //#endregion
        //#endregion

        //#region BPJSSearch
        $('#<%=btnSearchPeserta.ClientID %>').live('click', function () {
            GetSearchPeserta();
        });

        function GetSearchPeserta() {
            var noPeserta = $('#<%=txtNHSRegistrationNo.ClientID %>').val();
            var date = Methods.getDatePickerDate($('#<%=txtRegistrationDate.ClientID %>').val());
            var tglSEP = Methods.dateToYMD(date);
            if (noPeserta == '')
                showToast("DATA PESERTA : GAGAL", 'No. Kartu Peserta BPJS harus diisi !');
            else {
                $('#<%:hdnIsBPJSChecked.ClientID %>').val('1');
                try {
                    if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                        BPJSService.getPeserta(noPeserta, tglSEP, function (result) {
                            resetBPJSField();
                            var resultInfo = result.split('|');
                            if (resultInfo[0] == "1") {
                                //success
                                var obj = jQuery.parseJSON(resultInfo[1]);
                                if (obj.response.peserta != null) {
                                    $('#<%=txtNamaPeserta.ClientID %>').val(obj.response.peserta.nama);
                                    $('#<%=txtJenisPeserta.ClientID %>').val(obj.response.peserta.jenisPeserta.keterangan);
                                    $('#<%=txtKelas.ClientID %>').val(obj.response.peserta.hakKelas.kode + ' - ' + obj.response.peserta.hakKelas.keterangan);
                                    $('#<%=txtNamaFaskes.ClientID %>').val(obj.response.peserta.provUmum.kdProvider + ' - ' + obj.response.peserta.provUmum.nmProvider.trim());
                                    $('#<%=txtStatusPeserta.ClientID %>').val(obj.response.peserta.statusPeserta.kode + ' - ' + obj.response.peserta.statusPeserta.keterangan.trim());
                                    $('#<%=txtDinsos.ClientID %>').val(obj.response.peserta.informasi.dinsos);
                                    $('#<%=txtNoSKTM.ClientID %>').val(obj.response.peserta.informasi.noSKTM);
                                    $('#<%=txtPRB.ClientID %>').val(obj.response.peserta.informasi.prolanisPRB);
                                    var value = obj.response.peserta.provUmum.kdProvider;
                                    var SCRegistrationPayerBPJS = 'X004^500';
                                    $('#<%:txtReferralNo.ClientID %>').removeAttr('readonly');
                                    cboRegistrationPayer.SetValue(SCRegistrationPayerBPJS);
                                    onCboPayerValueChanged();
                                    getPayerCompany('');
                                    if ($('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.INPATIENT && $('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.EMERGENCY) {
                                        $('#<%:trIsKontrolBPJS.ClientID %>').removeAttr('style');
                                    }
                                    //var kodeTipeCoverage = 'TJ04';
                                    if ($('#<%:hdnContractID.ClientID %>').val() != '') {
                                        getCoverageType('');
                                    }
                                    $('#<%=txtParticipantNo.ClientID %>').val(noPeserta);

                                    if (obj.response.peserta.statusPeserta.kode.trim() != "0") {
                                        showToast("STATUS PESERTA : TIDAK VALID", obj.response.peserta.statusPeserta.keterangan.trim());
                                        lblBPJSStatusINACTIVE.style.display = "block";
                                        lblBPJSStatusINACTIVE.innerHTML = obj.response.peserta.statusPeserta.keterangan.trim();
                                    }
                                    else {
                                        lblBPJSStatusINACTIVE.style.display = "none";
                                        lblBPJSStatusINACTIVE.innerHTML = "TIDAK VALID";
                                    }
                                }
                                else BPJSFailTrigger('DATA PESERTA : GAGAL', "Data Peserta invalid");
                            }
                            else {
                                //validate-fail
                                BPJSFailTrigger('DATA PESERTA : GAGAL', resultInfo[2]);
                            }
                        });
                    } else {
                    BPJSService.getPesertaMedinfrasAPI(noPeserta, tglSEP, function (result) {
                        resetBPJSField();
                        var resultInfo = result.split('|');
                        if (resultInfo[0] == "1") {
                            //success
                            var obj = jQuery.parseJSON(resultInfo[1]);
                            if (obj.response.peserta != null) {
                                $('#<%=txtNamaPeserta.ClientID %>').val(obj.response.peserta.nama);
                                $('#<%=txtJenisPeserta.ClientID %>').val(obj.response.peserta.jenisPeserta.keterangan);
                                $('#<%=txtKelas.ClientID %>').val(obj.response.peserta.hakKelas.kode + ' - ' + obj.response.peserta.hakKelas.keterangan);
                                $('#<%=txtNamaFaskes.ClientID %>').val(obj.response.peserta.provUmum.kdProvider + ' - ' + obj.response.peserta.provUmum.nmProvider.trim());
                                $('#<%=txtStatusPeserta.ClientID %>').val(obj.response.peserta.statusPeserta.kode + ' - ' + obj.response.peserta.statusPeserta.keterangan.trim());
                                $('#<%=txtDinsos.ClientID %>').val(obj.response.peserta.informasi.dinsos);
                                $('#<%=txtNoSKTM.ClientID %>').val(obj.response.peserta.informasi.noSKTM);
                                $('#<%=txtPRB.ClientID %>').val(obj.response.peserta.informasi.prolanisPRB);
                                var value = obj.response.peserta.provUmum.kdProvider;
                                var SCRegistrationPayerBPJS = 'X004^500';
                                $('#<%:txtReferralNo.ClientID %>').removeAttr('readonly');
                                if ($('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.INPATIENT && $('#<%:hdnDepartmentID.ClientID %>').val() != Constant.Facility.EMERGENCY) {
                                    $('#<%:trIsKontrolBPJS.ClientID %>').removeAttr('style');
                                }
                                cboRegistrationPayer.SetValue(SCRegistrationPayerBPJS);
                                onCboPayerValueChanged();
                                Methods.getObject('GetSettingParameterDtList', "ParameterCode = 'FN0046'", function (resultBPJS) {
                                    if (resultBPJS != null) {
                                        if (resultBPJS.ParameterValue != 0) {
                                            getPayerCompany('BusinessPartnerID = ' + resultBPJS.ParameterValue);
                                        }
                                        else {
                                            getPayerCompany('');
                                        }
                                    }
                                    else {
                                        getPayerCompany('');
                                    }
                                });
                                //var kodeTipeCoverage = 'TJ04';
                                if ($('#<%:hdnContractID.ClientID %>').val() != '') {
                                    getCoverageType('');
                                }
                                $('#<%=txtParticipantNo.ClientID %>').val(noPeserta);

                                if (obj.response.peserta.statusPeserta.kode.trim() != "0") {
                                    showToast("STATUS PESERTA : TIDAK VALID", obj.response.peserta.statusPeserta.keterangan.trim());
                                    lblBPJSStatusINACTIVE.style.display = "block";
                                    lblBPJSStatusINACTIVE.innerHTML = obj.response.peserta.statusPeserta.keterangan.trim();
                                }
                                else {
                                    lblBPJSStatusINACTIVE.style.display = "none";
                                    lblBPJSStatusINACTIVE.innerHTML = "TIDAK VALID";
                                }
                            }
                            else BPJSFailTrigger('DATA PESERTA : GAGAL', "Data Peserta invalid");
                        }
                        else {
                            //validate-fail
                            BPJSFailTrigger('DATA PESERTA : GAGAL', resultInfo[2]);
                        }
                    });
                    }
                } catch (err) {
                    BPJSFailTrigger('DATA PESERTA : GAGAL', err.Message);
                }
            }
        }

        function resetBPJSField() {
            $('#<%=txtNamaPeserta.ClientID %>').val('');
            $('#<%=txtJenisPeserta.ClientID %>').val('');
            $('#<%=txtKelas.ClientID %>').val('');
            $('#<%=txtNamaFaskes.ClientID %>').val('');
            $('#<%=txtStatusPeserta.ClientID %>').val('');
            $('#<%=txtParticipantNo.ClientID %>').val('');
            cboRegistrationPayer.SetText('Pribadi');
            onCboPayerValueChanged();
            lblBPJSStatusINACTIVE.style.display = "none";
        }

        function BPJSFailTrigger(message1, message2) {
            showToast(message1, 'Error Message : ' + message2);
            lblBPJSStatusINACTIVE.style.display = "block";
            resetBPJSField();
        }
        //#endregion

        //#region SetPatientInformationToControl
        function SetPatientInformationToControl(result) {
            if (result != null) {
                $('#btnInfoHistoryRegistration').removeAttr('enabled');
                $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                $('#<%:hdnMRN.ClientID %>').val(result.MRN);
                $('#<%:hdnMobilePhoneNo1.ClientID %>').val(result.MobilePhoneNo1);
                $('#<%=txtStatusPeserta.ClientID %>').val('');
                var filterExpressionCheckRegistration = "MRN = '" + result.MRN + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "'";
                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();

                var messageRetention = 'Pasien Dengan MRN ' + '<b>' + $('#<%:txtMRN.ClientID %>').val() + '</b>' + ' termasuk kategori sudah di retensi';
                if (result.GCPatientStatus == Constant.PatientStatus.RETENTION || result.GCPatientStatus == Constant.PatientStatus.ARCHIEVED) {
                    showToast('Warning', messageRetention);
                }

                var isHasOutstandingInvoice = 0;
                var filterExpressionCheckOutstandingInvoicePatient = "MRN = " + $('#<%:hdnMRN.ClientID %>').val();
                var messageHasOutstandingInvoice = 'Pasien Dengan MRN ' + '<b>' + $('#<%:txtMRN.ClientID %>').val() + '</b>' + ' masih memiliki piutang pribadi yang masih outstanding' + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';
                Methods.getObject('GetvPatientHasOutstandingInvoicePaymentList', filterExpressionCheckOutstandingInvoicePatient, function (result) {
                    if (result != null) {
                        isHasOutstandingInvoice = 1;
                    }
                });

                if (regID == '') {
                    Methods.getObject('GetRegistrationList', filterExpressionCheckRegistration, function (resultCheck) {
                        if (resultCheck != null) {
                            $('#<%:chkCardFee.ClientID %>').prop('checked', false);
                            $('#<%:hdnIsNewPatient.ClientID %>').val('0');
                        }
                        else {
                            var date = new Date(parseInt(result.RegisteredDate.substr(6)));
                            var dateString = Methods.dateToDMY(date);
                            if (dateString == $('#<%:txtRegistrationDate.ClientID %>').val()) {
                                $('#<%:chkCardFee.ClientID %>').prop('checked', true);
                                $('#<%:hdnIsNewPatient.ClientID %>').val('1');
                            }
                            else {
                                $('#<%:chkCardFee.ClientID %>').prop('checked', false);
                                $('#<%:hdnIsNewPatient.ClientID %>').val('0');
                            }
                        }
                    });
                }
                if (result.IdentityNoType != null && result.IdentityNoType != "" && result.SSN != null && result.SSN != "") {
                    $('#<%:txtIdentityCardNo.ClientID %>').val("(" + result.IdentityNoType + ") " + result.SSN);
                } else if (result.SSN != null && result.SSN != "") {
                    $('#<%:txtIdentityCardNo.ClientID %>').val(result.SSN);
                } else {
                    $('#<%:txtIdentityCardNo.ClientID %>').val("");
                }

                if (result.IsHasPhysicalLimitation) {
                    $('#<%:chkIsFastTrack.ClientID %>').prop("checked", true);
                }
                else {
                    $('#<%:chkIsFastTrack.ClientID %>').prop("checked", false);
                }

                $('#<%:txtPatientNotes.ClientID %>').val(result.Notes);
                $('#<%:txtNHSRegistrationNo.ClientID %>').val(result.NHSRegistrationNo);
                if ($('#<%:txtNHSRegistrationNo.ClientID %>').val() != '') {
                    $('#<%:txtParticipantNo.ClientID %>').val(result.NHSRegistrationNo);
                }
                $('#<%:txtInhealthParticipantNo.ClientID %>').val(result.InhealthParticipantNo);
                $('#<%:txtCorporateAccountNo.ClientID %>').val(result.CorporateAccountNo);
                $('#<%:txtIHSNumber.ClientID %>').val(result.IHSNumber);
                $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                $('#<%:txtPreferredName.ClientID %>').val(result.PreferredName);
                $('#<%:txtGender.ClientID %>').val(result.Gender);
                $('#<%:hdnGCGender.ClientID %>').val(result.GCGender);
                $('#<%=txtNamaPeserta.ClientID %>').val(result.NamaPesertaBPJS);
                $('#<%=txtJenisPeserta.ClientID %>').val(result.JenisPesertaBPJS);
                $('#<%=txtKelas.ClientID %>').val(result.KodeKelasBPJS);
                $('#<%=txtNamaFaskes.ClientID %>').val(result.KodePPK1BPJS + ' - ' + result.NamaPPK1BPJS);
                //                if (result.KodePPK1BPJS != null && result.KodePPK1BPJS != '') {
                //                    cboReferral.SetValue('X105^006');
                //                    onCboReferralValueChanged();
                //                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.KodePPK1BPJS);
                //                    onTxtReferralDescriptionCodeChanged(result.KodePPK1BPJS);
                //                }
                $('#<%:txtDOB.ClientID %>').val(result.DateOfBirthInString);
                $('#<%:txtAgeInYear.ClientID %>').val(result.AgeInYear);
                $('#<%:txtAgeInMonth.ClientID %>').val(result.AgeInMonth);
                $('#<%:txtAgeInDay.ClientID %>').val(result.AgeInDay);
                $('#<%:txtAddress.ClientID %>').val(result.HomeAddress);
                $('#<%:txtHandphoneNo.ClientID %>').val(result.MobilePhoneNo1);
                $('#<%:txtEmailAddress.ClientID %>').val(result.EmailAddress);
                $('#<%:txtPatientJob.ClientID %>').val(result.Occupation);
                $('#<%:txtPatientJobOffice.ClientID %>').val(result.Company);
                if (result.IsCataract)
                    $('#<%:hdnIsCataract.ClientID %>').val("1");
                else
                    $('#<%:hdnIsCataract.ClientID %>').val("0");
                $('#<%:hdnIsBlacklist.ClientID %>').val(result.IsBlacklist);
                var blacklistReason = result.cfBlacklistReason;
                var messageBlackList = 'Pasien Dengan MRN ' + '<b>' + $('#<%:txtMRN.ClientID %>').val() + '</b>' + ' adalah Pasien Bermasalah (' + '<b>' + blacklistReason + '</b>' + ').' + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                if (getIsAdd()) {
                    if (result.GCGender == '<%:GetGenderFemale() %>') {
                        $('#<%:chkIsPregnant.ClientID %>').removeAttr("disabled");
                        $('#<%:chkIsParturition.ClientID %>').removeAttr("disabled");

                        if (result.IsPregnant) {
                        $('#<%:chkIsPregnant.ClientID %>').prop("checked", true);
                        }
                    }
                    else {
                        $('#<%:chkIsPregnant.ClientID %>').attr("disabled", true);
                        $('#<%:chkIsParturition.ClientID %>').attr("disabled", true);
                        $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                        $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                    }
                    var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
                    var isAlive = result.IsAlive;
                    var isVIP = result.IsVIP;
                    var vipGroup = result.cfVIPGroup;
                    if (deptID == Constant.Facility.PHARMACY || deptID == Constant.Facility.LABORATORY || deptID == Constant.Facility.DIAGNOSTIC) {
                        var filterParamWalkin = "ParameterCode = '" + Constant.SettingParameter.RM_DEFAULT_PATIENT_WALKIN + "'";
                        Methods.getObject('GetSettingParameterDtList', filterParamWalkin, function (resultwalkin) {
                            if (resultwalkin != '' && resultwalkin != null) {
                                var mrn = $('#<%:txtMRN.ClientID %>').val();
                                if (mrn != resultwalkin.ParameterValue) {
                                    if (isAlive == false) {
                                        showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> sudah meninggal.");
                                    };
                                    if (isVIP == false) {
                                        $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                    }
                                    else {
                                        $('#<%:tdIsVIP.ClientID %>').removeAttr('style');
                                        document.getElementById('hdnTitleVIP').title = vipGroup;
                                    }
                                    var isValidateDuplicatePatient = onCheckDuplicatePatientData(result.MRN, result.PatientName, result.cfDateOfBirthInYYYYMMDD, result.GCGender, result.StreetName, result.ZipCodeID, result.County, result.District, result.City);
                                    var resultCheckIsValidateDuplicatePatient = isValidateDuplicatePatient.split('|');

                                    if (resultCheckIsValidateDuplicatePatient[0] == 'false') {
                                        showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> sudah ada di sistem dengan Nomor Rekam Medis <b>" + resultCheckIsValidateDuplicatePatient[1] + "</b>");
                                    };

                                    if ($('#<%:hdnIsBlacklist.ClientID %>').val() == 'true') {
                                        showToastConfirmation(messageBlackList, function (resultMessageBlackList) {
                                            if (resultMessageBlackList) {
                                                var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                                if (departmentID == Constant.Facility.OUTPATIENT) {
                                                    var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                                    var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                                    Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                                        if (result != null) {
                                                            $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                            $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                            $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                            $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                            $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                                        }
                                                        else {
                                                            $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                            $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                            $('#<%:tdLastPayerName.ClientID %>').html('');
                                                            $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                            $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                                        }
                                                    });
                                                }
                                                var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                                var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                                if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                    if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                        filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                                    }
                                                    else {
                                                        filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                                    }
                                                }
                                                var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                                if (fromRegistrationID != '' && fromRegistrationID != '0')
                                                    filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                                Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                                    if (result.length > 0) {
                                                        var messageDoubleRegistration = '';
                                                        for (i = 0; i < result.length; i++) {
                                                            if (messageDoubleRegistration == '') {
                                                                messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                            }
                                                            else {
                                                                var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                                messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                            }
                                                        }
                                                        messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                                        showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                            if (resultDoubleRegistration.toString() != 'true') {
                                                                $('#<%:hdnMRN.ClientID %>').val('');
                                                                $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                                $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                                $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                                $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                                $('#<%:txtMRN.ClientID %>').val('');
                                                                $('#<%:txtPatientName.ClientID %>').val('');
                                                                $('#<%:txtPreferredName.ClientID %>').val('');
                                                                $('#<%:txtGender.ClientID %>').val('');
                                                                $('#<%:txtDOB.ClientID %>').val('');
                                                                $('#<%:txtAgeInYear.ClientID %>').val('');
                                                                $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                                $('#<%:txtAgeInDay.ClientID %>').val('');
                                                                $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                                $('#<%:txtAddress.ClientID %>').val('');
                                                                $('#<%:txtPatientNotes.ClientID %>').val('');
                                                                $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                            else {
                                                $('#<%:hdnMRN.ClientID %>').val('');
                                                $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                $('#<%:txtMRN.ClientID %>').val('');
                                                $('#<%:txtPatientName.ClientID %>').val('');
                                                $('#<%:txtPreferredName.ClientID %>').val('');
                                                $('#<%:txtGender.ClientID %>').val('');
                                                $('#<%:txtDOB.ClientID %>').val('');
                                                $('#<%:txtAgeInYear.ClientID %>').val('');
                                                $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                $('#<%:txtAgeInDay.ClientID %>').val('');
                                                $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                $('#<%:txtAddress.ClientID %>').val('');
                                                $('#<%:txtPatientNotes.ClientID %>').val('');
                                                $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                            }
                                        });
                                    }
                                    else if (isHasOutstandingInvoice == 1) {
                                        showToastConfirmation(messageHasOutstandingInvoice, function (resultHasOutsandingInvoice) {
                                            if (resultHasOutsandingInvoice) {
                                                var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                                if (departmentID == Constant.Facility.OUTPATIENT) {
                                                    var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                                    var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                                    Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                                        if (result != null) {
                                                            $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                            $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                            $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                            $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                            $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                                        }
                                                        else {
                                                            $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                            $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                            $('#<%:tdLastPayerName.ClientID %>').html('');
                                                            $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                            $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                                        }
                                                    });
                                                }
                                                var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                                var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                                if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                    if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                        filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                                    }
                                                    else {
                                                        filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                                    }
                                                }
                                                var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                                if (fromRegistrationID != '' && fromRegistrationID != '0')
                                                    filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                                Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                                    if (result.length > 0) {
                                                        var messageDoubleRegistration = '';
                                                        for (i = 0; i < result.length; i++) {
                                                            if (messageDoubleRegistration == '') {
                                                                messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                            }
                                                            else {
                                                                var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                                messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                            }
                                                        }
                                                        messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                                        showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                            if (resultDoubleRegistration.toString() != 'true') {
                                                                $('#<%:hdnMRN.ClientID %>').val('');
                                                                $('#<%:txtMRN.ClientID %>').val('');
                                                                $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                                $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                                $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                                $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                                $('#<%:txtMRN.ClientID %>').val('');
                                                                $('#<%:txtPatientName.ClientID %>').val('');
                                                                $('#<%:txtPreferredName.ClientID %>').val('');
                                                                $('#<%:txtGender.ClientID %>').val('');
                                                                $('#<%:txtDOB.ClientID %>').val('');
                                                                $('#<%:txtAgeInYear.ClientID %>').val('');
                                                                $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                                $('#<%:txtAgeInDay.ClientID %>').val('');
                                                                $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                                $('#<%:txtAddress.ClientID %>').val('');
                                                                $('#<%:txtPatientNotes.ClientID %>').val('');
                                                                $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                            else {
                                                $('#<%:hdnMRN.ClientID %>').val('');
                                                $('#<%:txtMRN.ClientID %>').val('');
                                                $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                $('#<%:txtMRN.ClientID %>').val('');
                                                $('#<%:txtPatientName.ClientID %>').val('');
                                                $('#<%:txtPreferredName.ClientID %>').val('');
                                                $('#<%:txtGender.ClientID %>').val('');
                                                $('#<%:txtDOB.ClientID %>').val('');
                                                $('#<%:txtAgeInYear.ClientID %>').val('');
                                                $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                $('#<%:txtAgeInDay.ClientID %>').val('');
                                                $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                $('#<%:txtAddress.ClientID %>').val('');
                                                $('#<%:txtPatientNotes.ClientID %>').val('');
                                                $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                            }
                                        });
                                    }
                                    else {
                                        var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                        if (departmentID == Constant.Facility.OUTPATIENT) {
                                            var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                            var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                            Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                                if (result != null) {
                                                    $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                    $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                    $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                    $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                    $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                                }
                                                else {
                                                    $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                    $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                    $('#<%:tdLastPayerName.ClientID %>').html('');
                                                    $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                    $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                                }
                                            });
                                        }
                                        var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                        var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                        if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                            if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                            }
                                            else {
                                                filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                            }
                                        }
                                        var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                        if (fromRegistrationID != '' && fromRegistrationID != '0')
                                            filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                        Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                            if (result.length > 0) {
                                                var messageDoubleRegistration = '';
                                                for (i = 0; i < result.length; i++) {
                                                    if (messageDoubleRegistration == '') {
                                                        messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                    }
                                                    else {
                                                        var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                        messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                    }
                                                }
                                                messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                                showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                    if (resultDoubleRegistration.toString() != 'true') {
                                                        $('#<%:hdnMRN.ClientID %>').val('');
                                                        $('#<%:txtMRN.ClientID %>').val('');
                                                        $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                        $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                        $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                        $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                        $('#<%:txtMRN.ClientID %>').val('');
                                                        $('#<%:txtPatientName.ClientID %>').val('');
                                                        $('#<%:txtPreferredName.ClientID %>').val('');
                                                        $('#<%:txtGender.ClientID %>').val('');
                                                        $('#<%:txtDOB.ClientID %>').val('');
                                                        $('#<%:txtAgeInYear.ClientID %>').val('');
                                                        $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                        $('#<%:txtAgeInDay.ClientID %>').val('');
                                                        $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                        $('#<%:txtAddress.ClientID %>').val('');
                                                        $('#<%:txtPatientNotes.ClientID %>').val('');
                                                        $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                    }
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                            else {
                                if (isAlive == false) {
                                    showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> sudah meninggal.");
                                };
                                if (isVIP == false) {
                                    $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                }
                                else {
                                    $('#<%:tdIsVIP.ClientID %>').removeAttr('style');
                                    document.getElementById('hdnTitleVIP').title = vipGroup;
                                }
                                var isValidateDuplicatePatient = onCheckDuplicatePatientData(result.MRN, result.PatientName, result.cfDateOfBirthInYYYYMMDD, result.GCGender, result.StreetName, result.ZipCodeID, result.County, result.District, result.City);
                                var resultCheckIsValidateDuplicatePatient = isValidateDuplicatePatient.split('|');

                                if (resultCheckIsValidateDuplicatePatient[0] == 'false') {
                                    showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> sudah ada di sistem dengan Nomor Rekam Medis <b>" + resultCheckIsValidateDuplicatePatient[1] + "</b>");
                                };

                                if ($('#<%:hdnIsBlacklist.ClientID %>').val() == 'true') {
                                    showToastConfirmation(messageBlackList, function (resultMessageBlackList) {
                                        if (resultMessageBlackList) {
                                            var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                            if (departmentID == Constant.Facility.OUTPATIENT) {
                                                var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                                var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                                    if (result != null) {
                                                        $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                        $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                        $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                        $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                        $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                                    }
                                                    else {
                                                        $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                        $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                        $('#<%:tdLastPayerName.ClientID %>').html('');
                                                        $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                        $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                                    }
                                                });
                                            }
                                            var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                            var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                            var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                            if (fromRegistrationID != '' && fromRegistrationID != '0')
                                                filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                            Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                                if (result.length > 0) {
                                                    var messageDoubleRegistration = '';
                                                    for (i = 0; i < result.length; i++) {
                                                        if (messageDoubleRegistration == '') {
                                                            messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                        }
                                                        else {
                                                            var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                            messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                        }
                                                    }
                                                    messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                                    showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                        if (resultDoubleRegistration.toString() != 'true') {
                                                            $('#<%:hdnMRN.ClientID %>').val('');
                                                            $('#<%:txtMRN.ClientID %>').val('');
                                                            $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                            $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                            $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                            $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                            $('#<%:txtMRN.ClientID %>').val('');
                                                            $('#<%:txtPatientName.ClientID %>').val('');
                                                            $('#<%:txtPreferredName.ClientID %>').val('');
                                                            $('#<%:txtGender.ClientID %>').val('');
                                                            $('#<%:txtDOB.ClientID %>').val('');
                                                            $('#<%:txtAgeInYear.ClientID %>').val('');
                                                            $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                            $('#<%:txtAgeInDay.ClientID %>').val('');
                                                            $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                            $('#<%:txtAddress.ClientID %>').val('');
                                                            $('#<%:txtPatientNotes.ClientID %>').val('');
                                                            $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                        }
                                                    });
                                                }
                                            });
                                        }
                                        else {
                                            $('#<%:hdnMRN.ClientID %>').val('');
                                            $('#<%:txtMRN.ClientID %>').val('');
                                            $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                            $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                            $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                            $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                            $('#<%:txtMRN.ClientID %>').val('');
                                            $('#<%:txtPatientName.ClientID %>').val('');
                                            $('#<%:txtPreferredName.ClientID %>').val('');
                                            $('#<%:txtGender.ClientID %>').val('');
                                            $('#<%:txtDOB.ClientID %>').val('');
                                            $('#<%:txtAgeInYear.ClientID %>').val('');
                                            $('#<%:txtAgeInMonth.ClientID %>').val('');
                                            $('#<%:txtAgeInDay.ClientID %>').val('');
                                            $('#<%:txtHandphoneNo.ClientID %>').val('');
                                            $('#<%:txtAddress.ClientID %>').val('');
                                            $('#<%:txtPatientNotes.ClientID %>').val('');
                                            $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                        }
                                    });
                                }
                                else if (isHasOutstandingInvoice == 1) {
                                    showToastConfirmation(messageHasOutstandingInvoice, function (resultHasOutsandingInvoice) {
                                        if (resultHasOutsandingInvoice) {
                                            var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                            if (departmentID == Constant.Facility.OUTPATIENT) {
                                                var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                                var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                                    if (result != null) {
                                                        $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                        $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                        $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                        $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                        $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                                    }
                                                    else {
                                                        $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                        $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                        $('#<%:tdLastPayerName.ClientID %>').html('');
                                                        $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                        $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                                    }
                                                });
                                            }
                                            var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                            var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                            if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                                    filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                                }
                                                else {
                                                    filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                                }
                                            }
                                            var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                            if (fromRegistrationID != '' && fromRegistrationID != '0')
                                                filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                            Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                                if (result.length > 0) {
                                                    var messageDoubleRegistration = '';
                                                    for (i = 0; i < result.length; i++) {
                                                        if (messageDoubleRegistration == '') {
                                                            messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                        }
                                                        else {
                                                            var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                            messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                        }
                                                    }
                                                    messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                                    showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                        if (resultDoubleRegistration.toString() != 'true') {
                                                            $('#<%:hdnMRN.ClientID %>').val('');
                                                            $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                            $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                            $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                            $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                            $('#<%:txtMRN.ClientID %>').val('');
                                                            $('#<%:txtPatientName.ClientID %>').val('');
                                                            $('#<%:txtPreferredName.ClientID %>').val('');
                                                            $('#<%:txtGender.ClientID %>').val('');
                                                            $('#<%:txtDOB.ClientID %>').val('');
                                                            $('#<%:txtAgeInYear.ClientID %>').val('');
                                                            $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                            $('#<%:txtAgeInDay.ClientID %>').val('');
                                                            $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                            $('#<%:txtAddress.ClientID %>').val('');
                                                            $('#<%:txtPatientNotes.ClientID %>').val('');
                                                            $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                        }
                                                    });
                                                }
                                            });
                                        }
                                        else {
                                            $('#<%:hdnMRN.ClientID %>').val('');
                                            $('#<%:txtMRN.ClientID %>').val('');
                                            $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                            $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                            $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                            $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                            $('#<%:txtMRN.ClientID %>').val('');
                                            $('#<%:txtPatientName.ClientID %>').val('');
                                            $('#<%:txtPreferredName.ClientID %>').val('');
                                            $('#<%:txtGender.ClientID %>').val('');
                                            $('#<%:txtDOB.ClientID %>').val('');
                                            $('#<%:txtAgeInYear.ClientID %>').val('');
                                            $('#<%:txtAgeInMonth.ClientID %>').val('');
                                            $('#<%:txtAgeInDay.ClientID %>').val('');
                                            $('#<%:txtHandphoneNo.ClientID %>').val('');
                                            $('#<%:txtAddress.ClientID %>').val('');
                                            $('#<%:txtPatientNotes.ClientID %>').val('');
                                            $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                        }
                                    });
                                }
                                else {
                                    var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                    if (departmentID == Constant.Facility.OUTPATIENT) {
                                        var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                        var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                        Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                            if (result != null) {
                                                $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                            }
                                            else {
                                                $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                $('#<%:tdLastPayerName.ClientID %>').html('');
                                                $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                            }
                                        });
                                    }
                                    var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                    var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                    var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                    if (fromRegistrationID != '' && fromRegistrationID != '0')
                                        filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                    Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                        if (result.length > 0) {
                                            var messageDoubleRegistration = '';
                                            for (i = 0; i < result.length; i++) {
                                                if (messageDoubleRegistration == '') {
                                                    messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                }
                                                else {
                                                    var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                    messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                }
                                            }
                                            messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                            showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                if (resultDoubleRegistration.toString() != 'true') {
                                                    $('#<%:hdnMRN.ClientID %>').val('');
                                                    $('#<%:txtMRN.ClientID %>').val('');
                                                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                    $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                    $('#<%:txtMRN.ClientID %>').val('');
                                                    $('#<%:txtPatientName.ClientID %>').val('');
                                                    $('#<%:txtPreferredName.ClientID %>').val('');
                                                    $('#<%:txtGender.ClientID %>').val('');
                                                    $('#<%:txtDOB.ClientID %>').val('');
                                                    $('#<%:txtAgeInYear.ClientID %>').val('');
                                                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                    $('#<%:txtAgeInDay.ClientID %>').val('');
                                                    $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                    $('#<%:txtAddress.ClientID %>').val('');
                                                    $('#<%:txtPatientNotes.ClientID %>').val('');
                                                    $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                }
                                            });
                                        }
                                    });
                                }
                            }
                        });

                    }
                    else {
                        if (isAlive == false) {
                            showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> sudah meninggal.");
                        };
                        if (isVIP == false) {
                            $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                        }
                        else {
                            $('#<%:tdIsVIP.ClientID %>').removeAttr('style');
                            document.getElementById('hdnTitleVIP').title = vipGroup;
                        }
                        var isValidateDuplicatePatient = onCheckDuplicatePatientData(result.MRN, result.PatientName, result.cfDateOfBirthInYYYYMMDD, result.GCGender, result.StreetName, result.ZipCodeID, result.County, result.District, result.City);
                        var resultCheckIsValidateDuplicatePatient = isValidateDuplicatePatient.split('|');

                        if (resultCheckIsValidateDuplicatePatient[0] == 'false') {
                            showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> sudah ada di sistem dengan Nomor Rekam Medis <b>" + resultCheckIsValidateDuplicatePatient[1] + "</b>");
                        };

                        if ($('#<%:hdnIsBlacklist.ClientID %>').val() == 'true') {
                            showToastConfirmation(messageBlackList, function (resultMessageBlackList) {
                                if (resultMessageBlackList) {
                                    var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                    if (departmentID == Constant.Facility.OUTPATIENT) {
                                        var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                        var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                        Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                            if (result != null) {
                                                $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                            }
                                            else {
                                                $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                $('#<%:tdLastPayerName.ClientID %>').html('');
                                                $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                            }
                                        });
                                    }
                                    var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                    var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                    if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                        if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                            filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                        }
                                        else {
                                            filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                        }
                                    }
                                    var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                    if (fromRegistrationID != '' && fromRegistrationID != '0')
                                        filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                    Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                        if (result.length > 0) {
                                            var messageDoubleRegistration = '';
                                            for (i = 0; i < result.length; i++) {
                                                if (messageDoubleRegistration == '') {
                                                    messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                }
                                                else {
                                                    var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                    messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                }
                                            }
                                            messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                            showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                if (resultDoubleRegistration.toString() != 'true') {
                                                    $('#<%:hdnMRN.ClientID %>').val('');
                                                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                    $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                    $('#<%:txtMRN.ClientID %>').val('');
                                                    $('#<%:txtPatientName.ClientID %>').val('');
                                                    $('#<%:txtPreferredName.ClientID %>').val('');
                                                    $('#<%:txtGender.ClientID %>').val('');
                                                    $('#<%:txtDOB.ClientID %>').val('');
                                                    $('#<%:txtAgeInYear.ClientID %>').val('');
                                                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                    $('#<%:txtAgeInDay.ClientID %>').val('');
                                                    $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                    $('#<%:txtAddress.ClientID %>').val('');
                                                    $('#<%:txtPatientNotes.ClientID %>').val('');
                                                    $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                }
                                            });
                                        }
                                    });
                                    if ($('#<%=hdnIsWarningPatientHaveAR.ClientID %>').val() == '1') {
                                        filterExpression = "GCPaymentType = 'X034^003' AND GCTransactionStatus NOT IN ('" + Constant.TransactionStatus.VOID + "','" + Constant.TransactionStatus.CLOSED + "') AND RegistrationID IN (SELECT RegistrationID FROM Registration WHERE MRN = " + result.MRN + " AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "')";
                                        Methods.getObject('GetPatientPaymentHdList', filterExpression, function (result2) {
                                            if (result2 != null) {
                                                showToast('Warning', 'Masih Ada Piutang yang belum di bayar dengan NO : ' + result2.PaymentNo + "</b>");
                                            }
                                        });
                                    }
                                }
                                else {
                                    $('#<%:hdnMRN.ClientID %>').val('');
                                    $('#<%:txtMRN.ClientID %>').val('');
                                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                    $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                    $('#<%:txtMRN.ClientID %>').val('');
                                    $('#<%:txtPatientName.ClientID %>').val('');
                                    $('#<%:txtPreferredName.ClientID %>').val('');
                                    $('#<%:txtGender.ClientID %>').val('');
                                    $('#<%:txtDOB.ClientID %>').val('');
                                    $('#<%:txtAgeInYear.ClientID %>').val('');
                                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                                    $('#<%:txtAgeInDay.ClientID %>').val('');
                                    $('#<%:txtHandphoneNo.ClientID %>').val('');
                                    $('#<%:txtAddress.ClientID %>').val('');
                                    $('#<%:txtPatientNotes.ClientID %>').val('');
                                    $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                }
                            });
                        }
                        else if (isHasOutstandingInvoice == 1) {
                            showToastConfirmation(messageHasOutstandingInvoice, function (resultHasOutsandingInvoice) {
                                if (resultHasOutsandingInvoice) {
                                    var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                                    if (departmentID == Constant.Facility.OUTPATIENT) {
                                        var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                        var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                        Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                            if (result != null) {
                                                $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                                $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                                $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                                $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                                $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                            }
                                            else {
                                                $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                                $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                                $('#<%:tdLastPayerName.ClientID %>').html('');
                                                $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                                $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                            }
                                        });
                                    }
                                    var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                    var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                    if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                        if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                            filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                        }
                                        else {
                                            filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                        }
                                    }
                                    var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                                    if (fromRegistrationID != '' && fromRegistrationID != '0')
                                        filterExpression += " AND RegistrationID != " + fromRegistrationID;
                                    Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                        if (result.length > 0) {
                                            var messageDoubleRegistration = '';
                                            for (i = 0; i < result.length; i++) {
                                                if (messageDoubleRegistration == '') {
                                                    messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                }
                                                else {
                                                    var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                                    messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                                }
                                            }
                                            messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                            showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                                if (resultDoubleRegistration.toString() != 'true') {
                                                    $('#<%:hdnMRN.ClientID %>').val('');
                                                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                                    $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                                    $('#<%:txtMRN.ClientID %>').val('');
                                                    $('#<%:txtPatientName.ClientID %>').val('');
                                                    $('#<%:txtPreferredName.ClientID %>').val('');
                                                    $('#<%:txtGender.ClientID %>').val('');
                                                    $('#<%:txtDOB.ClientID %>').val('');
                                                    $('#<%:txtAgeInYear.ClientID %>').val('');
                                                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                                                    $('#<%:txtAgeInDay.ClientID %>').val('');
                                                    $('#<%:txtHandphoneNo.ClientID %>').val('');
                                                    $('#<%:txtAddress.ClientID %>').val('');
                                                    $('#<%:txtPatientNotes.ClientID %>').val('');
                                                    $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                                }
                                            });
                                        }
                                    });
                                }
                                else {
                                    $('#<%:hdnMRN.ClientID %>').val('');
                                    $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                    $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                    $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                    $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                    $('#<%:txtMRN.ClientID %>').val('');
                                    $('#<%:txtPatientName.ClientID %>').val('');
                                    $('#<%:txtPreferredName.ClientID %>').val('');
                                    $('#<%:txtGender.ClientID %>').val('');
                                    $('#<%:txtDOB.ClientID %>').val('');
                                    $('#<%:txtAgeInYear.ClientID %>').val('');
                                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                                    $('#<%:txtAgeInDay.ClientID %>').val('');
                                    $('#<%:txtHandphoneNo.ClientID %>').val('');
                                    $('#<%:txtAddress.ClientID %>').val('');
                                    $('#<%:txtPatientNotes.ClientID %>').val('');
                                    $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                }
                            });
                        }
                        else {
                            var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                            if (departmentID == Constant.Facility.OUTPATIENT) {
                                var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                                var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate < '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "' ORDER BY RegistrationDate DESC";
                                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                                    if (result != null) {
                                        $('#<%:trLastVisitDataEmpty.ClientID %>').attr('style', 'display:none');
                                        $('#<%:trLastVisitData.ClientID %>').removeAttr('style');
                                        $('#<%:tdLastPayerName.ClientID %>').html(result.BusinessPartnerName);
                                        $('#<%:tdLastServiceUnitParamedic.ClientID %>').html(result.ServiceUnitName + '<br>' + result.ParamedicName);
                                        $('#<%:tdLastRegistrationDate.ClientID %>').html(result.RegistrationDateInString);
                                    }
                                    else {
                                        $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                                        $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                                        $('#<%:tdLastPayerName.ClientID %>').html('');
                                        $('#<%:tdLastServiceUnitParamedic.ClientID %>').html('');
                                        $('#<%:tdLastRegistrationDate.ClientID %>').html('');
                                    }
                                });
                            }
                            var registrationDate = Methods.getDatePickerDate($('#<%:txtRegistrationDate.ClientID %>').val());
                            var filterExpression = "MRN = " + result.MRN + " AND RegistrationDate = '" + Methods.dateToYMD(registrationDate) + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                            if ($('#<%:hdnWarningPatientOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                if ($('#<%:hdnWarningPatientPersonalOutstandingRegDiffDay.ClientID %>').val() == '1') {
                                    filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND BusinessPartnerID = 1";
                                }
                                else {
                                    filterExpression = "MRN = " + result.MRN + " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
                                }
                            }
                            var fromRegistrationID = $('#<%:hdnFromRegistrationID.ClientID %>').val();
                            if (fromRegistrationID != '' && fromRegistrationID != '0')
                                filterExpression += " AND RegistrationID != " + fromRegistrationID;

                            Methods.getListObject('GetvRegistrationList', filterExpression, function (result) {
                                if (result.length > 0) {
                                    var messageDoubleRegistration = '';
                                    for (i = 0; i < result.length; i++) {
                                        if (messageDoubleRegistration == '') {
                                            messageDoubleRegistration = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                        }
                                        else {
                                            var info = 'Masih Ada Pendaftaran Yang Belum Ditutup Untuk No RM <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> di Unit Pelayanan <b>" + result[i].ServiceUnitName + "</b>" + ' dengan <b>' + result[i].ParamedicName + '</b>' + ' (<b>' + result[i].RegistrationNo + '</b>)';
                                            messageDoubleRegistration = messageDoubleRegistration + '<br>' + info;
                                        }
                                    }
                                    messageDoubleRegistration = messageDoubleRegistration + '<br>' + 'Apakah Ingin Melanjutkan Proses ?';

                                    showToastConfirmation(messageDoubleRegistration, function (resultDoubleRegistration) {
                                        if (resultDoubleRegistration.toString() != 'true') {
                                            $('#<%:hdnMRN.ClientID %>').val('');
                                            $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                            $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                                            $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                                            $('#<%:txtIdentityCardNo.ClientID %>').val('');
                                            $('#<%:hdnIsBlacklist.ClientID %>').val('');
                                            $('#<%:txtMRN.ClientID %>').val('');
                                            $('#<%:txtPatientName.ClientID %>').val('');
                                            $('#<%:txtPreferredName.ClientID %>').val('');
                                            $('#<%:txtGender.ClientID %>').val('');
                                            $('#<%:txtDOB.ClientID %>').val('');
                                            $('#<%:txtAgeInYear.ClientID %>').val('');
                                            $('#<%:txtAgeInMonth.ClientID %>').val('');
                                            $('#<%:txtAgeInDay.ClientID %>').val('');
                                            $('#<%:txtHandphoneNo.ClientID %>').val('');
                                            $('#<%:txtAddress.ClientID %>').val('');
                                            $('#<%:txtPatientNotes.ClientID %>').val('');
                                            $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                                        }
                                    });
                                }
                            });
                            if ($('#<%=hdnIsWarningPatientHaveAR.ClientID %>').val() == '1') {
                                filterExpression = "GCPaymentType = 'X034^003' AND GCTransactionStatus NOT IN ('" + Constant.TransactionStatus.VOID + "','" + Constant.TransactionStatus.CLOSED + "') AND RegistrationID IN (SELECT RegistrationID FROM Registration WHERE MRN = " + result.MRN + " AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "')";
                                Methods.getObject('GetPatientPaymentHdList', filterExpression, function (result2) {
                                    if (result2 != null) {
                                        showToast('Warning', 'Masih Ada Piutang yang belum di bayar dengan NO : ' + result2.PaymentNo + "</b>");
                                    }
                                });
                            }
                        }
                    }
                }
            }
            else {
                cboReferral.SetValue('');
                onCboReferralValueChanged();
                $('#btnInfoHistoryRegistration').attr('enabled', 'false');
                $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                $('#<%:hdnReferrerID.ClientID %>').val('');
                $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                $('#<%:hdnMRN.ClientID %>').val('');
                $('#<%:txtIdentityCardNo.ClientID %>').val('');
                $('#<%:txtNHSRegistrationNo.ClientID %>').val('');
                $('#<%:txtInhealthParticipantNo.ClientID %>').val('');
                $('#<%:hdnIsBlacklist.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').val('');
                $('#<%:txtPreferredName.ClientID %>').val('');
                $('#<%:txtGender.ClientID %>').val('');
                $('#<%:txtDOB.ClientID %>').val('');
                $('#<%:txtAgeInYear.ClientID %>').val('');
                $('#<%:txtAgeInMonth.ClientID %>').val('');
                $('#<%:txtAgeInDay.ClientID %>').val('');
                $('#<%:txtHandphoneNo.ClientID %>').val('');
                $('#<%:txtAddress.ClientID %>').val('');
                $('#<%:txtPatientNotes.ClientID %>').val('');
                cboRegistrationPayer.SetValue("<%:GetCustomerTypePersonal() %>");
                $('#<%:hdnPayerID.ClientID %>').val('');
                $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                $('#<%:txtPayerCompanyName.ClientID %>').val('');
                $('#<%=txtNamaPeserta.ClientID %>').val('');
                $('#<%=txtJenisPeserta.ClientID %>').val('');
                $('#<%=txtKelas.ClientID %>').val('');
                $('#<%=txtNamaFaskes.ClientID %>').val('');
                $('#<%=txtStatusPeserta.ClientID %>').val('');
                $('#btnPayerNotesDetail').attr('enabled', 'false');
                $('#<%:trLastVisitData.ClientID %>').attr('style', 'display:none');
                $('#<%:trLastVisitDataEmpty.ClientID %>').removeAttr('style');
                $('#<%:chkIsVisitorRestriction.ClientID %>').prop("checked", false);
                $('#<%:tdIsVIP.ClientID %>').attr('style', 'display:none');
                $('#<%:txtIHSNumber.ClientID %>').val('');
            }
        }
        //#endregion

        //#region SetGuestInformationToControl
        function setGuestInformationToControl(result) {
            if (result != null) {
                $('#<%:hdnGuestID.ClientID %>').val(result.GuestID);
                $('#<%:txtGuestNo.ClientID %>').val(result.GuestNo);
                $('#<%:hdnGuestGCSalutation.ClientID %>').val(result.GCSalutation);
                $('#<%:hdnGuestGCTitle.ClientID %>').val(result.GCTitle);
                $('#<%:hdnGuestFirstName.ClientID %>').val(result.FirstName);
                $('#<%:hdnGuestMiddleName.ClientID %>').val(result.MiddleName);
                $('#<%:hdnGuestLastName.ClientID %>').val(result.LastName);
                $('#<%:hdnGuestGCSuffix.ClientID %>').val(result.GCSuffix);
                $('#<%:hdnGuestGCGender.ClientID %>').val(result.GCGender);
                $('#<%:hdnGuestDateOfBirth.ClientID %>').val(result.DateOfBirth);
                $('#<%:hdnGuestStreetName.ClientID %>').val(result.StreetName);
                $('#<%:hdnGuestCounty.ClientID %>').val(result.County);
                $('#<%:hdnGuestDistrict.ClientID %>').val(result.District);
                $('#<%:hdnGuestCity.ClientID %>').val(result.City);
                $('#<%:hdnGuestStreetNameDomicile.ClientID %>').val(result.StreetNameDomicile);
                $('#<%:hdnGuestCountyDomicile.ClientID %>').val(result.CountyDomicile);
                $('#<%:hdnGuestDistrictDomicile.ClientID %>').val(result.DistrictDomicile);
                $('#<%:hdnGuestCityDomicile.ClientID %>').val(result.CityDomicile);
                $('#<%:hdnGuestPhoneNo.ClientID %>').val(result.PhoneNo);
                $('#<%:hdnGuestMobilePhoneNo.ClientID %>').val(result.MobilePhoneNo);
                $('#<%:hdnGuestEmailAddress.ClientID %>').val(result.EmailAddress);
                $('#<%:hdnGuestGCIdentityNoType.ClientID %>').val(result.GCIdentityNoType);
                $('#<%:hdnGuestSSN.ClientID %>').val(result.SSN);
                $('#<%:hdnGuestSuffix.ClientID %>').val(result.Suffix);
                $('#<%:hdnGuestTitle.ClientID %>').val(result.Title);
                $('#<%:txtAgeInYear.ClientID %>').val(result.AgeInYear);
                $('#<%:txtAgeInMonth.ClientID %>').val(result.AgeInMonth);
                $('#<%:txtAgeInDay.ClientID %>').val(result.AgeInDay);
                $('#<%:txtGender.ClientID %>').val(result.cfGender);
                $('#<%:txtDOB.ClientID %>').val(result.DateOfBirthInString);
                $('#<%:txtAddress.ClientID %>').val(result.StreetName + ' ' + result.County + ' ' + result.District + ' ' + result.City);
                $('#<%:txtPatientName.ClientID %>').val(result.FirstName + ' ' + result.MiddleName + ' ' + result.LastName);
                $('#<%:txtEmailAddress.ClientID %>').val(result.EmailAddress);
                $('#<%:txtCorporateAccountNo.ClientID %>').val(result.CorporateAccountNo);
                $('#<%:txtIHSNumber.ClientID %>').val('');
                $('#<%:txtPatientNotes.ClientID %>').val('');
            }
        }
        //#endregion

        //#region check duplicate patient
        function onCheckDuplicatePatientData(mrn, patientName, dateOfBirth, gender, streetName, zipCodeID, county, district, city) {
            var filterExpressionCekDuplicate =
                            "MRN != " + mrn
                            + " AND PatientName = '" + patientName
                            + "' AND DateOfBirth = '" + dateOfBirth
                            + "' AND GCGender = '" + gender
                            + "' AND StreetName = '" + streetName
                            + "' AND ISNULL(County,'') = '" + county
                            + "' AND ISNULL(District,'') = '" + district
                            + "' AND ISNULL(City,'') = '" + city
                            + "' AND IsDeleted = 0";

            if (zipCodeID != "" && zipCodeID != null) {
                filterExpressionCekDuplicate += " AND ISNULL(ZipCodeID,'') = '" + zipCodeID + "'";
            }

            var resultFinal = true + '|';
            Methods.getObject('GetvPatientList', filterExpressionCekDuplicate, function (result) {
                if (result != null) {
                    resultFinal = false + '|' + result.MedicalNo;
                }
            });

            return resultFinal;
        }
        //#endregion

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
                $('#<%:txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onHdnPhysicianIDChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicID = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    cboSpecialty.SetValue(result.SpecialtyID);

                    $('#<%:hdnLastParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:hdnLastParamedicCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:hdnLastParamedicName.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnLastSpecialty.ClientID %>').val(cboSpecialty.GetValue());
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);

                    var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                    if (healthcareServiceUnitID == '' || healthcareServiceUnitID == '0') {
                        var filterExpression = getServiceUnitFilterFilterExpression();
                        var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
                        Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
                            if (result != null) {
                                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                                $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                                var parameter2 = "ServiceUnitCode = '" + result.ServiceUnitCode + "'";
                                $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                                Methods.getObject('GetServiceUnitMasterList', parameter2, function (result2) {
                                    if (result2 != null) {
                                        $('#<%=hdnBPJSPoli.ClientID %>').val(result2.BPJSPoli);
                                    }
                                    else $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                });
                                if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                                    var filterExpression = onGetVisitTypeFilterExpression();
                                    Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                        if (result != null) {
                                            $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                            $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                            $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                        }
                                        else {
                                            $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                            $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                            $('#<%:txtVisitTypeName.ClientID %>').val('');
                                        }
                                    });
                                }
                            }
                            else {
                                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                $('#<%:txtServiceUnitName.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                            var filterExpression = onGetVisitTypeFilterExpression();
                            Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                    $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                }
                                else {
                                    $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%:txtVisitTypeName.ClientID %>').val('');
                                }
                            });
                        }
                    }
                }
                else {
                    cboSpecialty.SetValue('');
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
            //disni
            if ($('#<%=hdnRoomID.ClientID %>').val() == '') getRoom('');
        }

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    var filterExpressionLeave = "IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
                    Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                        if (resultLeave == null) {
                            cboSpecialty.SetValue(result.SpecialtyID);

                            $('#<%:hdnLastParamedicID.ClientID %>').val(result.ParamedicID);
                            $('#<%:hdnLastParamedicCode.ClientID %>').val(result.ParamedicCode);
                            $('#<%:hdnLastParamedicName.ClientID %>').val(result.ParamedicName);
                            $('#<%:hdnLastSpecialty.ClientID %>').val(cboSpecialty.GetValue());
                            $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                            $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);

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
                                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                                } else {
                                    Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
                                        if (result != null) {
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                                            $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                                            $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

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
                                            if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                                                var filterExpression = onGetVisitTypeFilterExpression();
                                                Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                                    if (result != null) {
                                                        $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                                        $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                                        $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                                    }
                                                    else {
                                                        $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                                        $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                                        $('#<%:txtVisitTypeName.ClientID %>').val('');
                                                    }
                                                });
                                            }
                                        }
                                        else {
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                            $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                            $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                            $('#<%:txtServiceUnitName.ClientID %>').val('');
                                            $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                        }
                                    });
                                }
                            }
                            else {
                                if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                                    var filterExpression = onGetVisitTypeFilterExpression();
                                    Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                        if (result != null) {
                                            $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                            $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                            $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                        }
                                        else {
                                            $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                            $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                            $('#<%:txtVisitTypeName.ClientID %>').val('');
                                        }
                                    });
                                }
                                if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.OUTPATIENT) {
                                    var filterExpression = 'ParamedicID = ' + $('#<%:hdnParamedicID.ClientID %>').val() + ' AND HealthcareServiceUnitID = ' + healthcareServiceUnitID + ' AND DayNumber = (DATEPART(dw, GETDATE()) + 5) % 7 + 1';
                                    Methods.getObject('GetvParamedicScheduleList', filterExpression, function (result) {
                                        if (result != null) {
                                            $('#<%:hdnRoomID.ClientID %>').val(result.RoomID);
                                            $('#<%:txtRoomCode.ClientID %>').val(result.RoomCode);
                                            $('#<%:txtRoomName.ClientID %>').val(result.RoomName);
                                        }
                                        else {
                                            $('#<%:hdnRoomID.ClientID %>').val('');
                                            $('#<%:txtRoomCode.ClientID %>').val('');
                                            $('#<%:txtRoomName.ClientID %>').val('');
                                        }
                                    });
                                }
                            }
                        }
                        else {
                            var departmentID = $('#<%:hdnDepartmentID.ClientID %>').val();
                            var isLeave = $('#<%:hdnRegistrasiSelainRajalMemperhatikanCutiDokter.ClientID %>').val();
                            if (isLeave == '1') {
                                cboSpecialty.SetValue('');
                                $('#<%:hdnParamedicID.ClientID %>').val('');
                                $('#<%:txtPhysicianCode.ClientID %>').val('');
                                $('#<%:txtPhysicianName.ClientID %>').val('');
                                var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                                showToast("INFORMASI", info);
                            }
                            else {
                                if (departmentID == Constant.Facility.OUTPATIENT) {
                                    cboSpecialty.SetValue('');
                                    $('#<%:hdnParamedicID.ClientID %>').val('');
                                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                                    $('#<%:txtPhysicianName.ClientID %>').val('');
                                    var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                                    showToast("INFORMASI", info);
                                }
                                else {
                                    cboSpecialty.SetValue(result.SpecialtyID);

                                    $('#<%:hdnLastParamedicID.ClientID %>').val(result.ParamedicID);
                                    $('#<%:hdnLastParamedicCode.ClientID %>').val(result.ParamedicCode);
                                    $('#<%:hdnLastParamedicName.ClientID %>').val(result.ParamedicName);
                                    $('#<%:hdnLastSpecialty.ClientID %>').val(cboSpecialty.GetValue());
                                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);

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
                                            $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                            $('#<%:txtServiceUnitName.ClientID %>').val('');
                                        } else {
                                            Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
                                                if (result != null) {
                                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                                                    $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                                                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

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
                                                    if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                                                        var filterExpression = onGetVisitTypeFilterExpression();
                                                        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                                            if (result != null) {
                                                                $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                                                $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                                                $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                                            }
                                                            else {
                                                                $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                                                $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                                                $('#<%:txtVisitTypeName.ClientID %>').val('');
                                                            }
                                                        });
                                                    }
                                                }
                                                else {
                                                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                                                    $('#<%:hdnIsPoliExecutive.ClientID %>').val('0');
                                                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                                                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                                                    $('#<%=hdnBPJSPoli.ClientID %>').val('');
                                                }
                                            });
                                        }
                                    }
                                    else {
                                        if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                                            var filterExpression = onGetVisitTypeFilterExpression();
                                            Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                                                if (result != null) {
                                                    $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                                    $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                                    $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                                }
                                                else {
                                                    $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                                    $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                                    $('#<%:txtVisitTypeName.ClientID %>').val('');
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    });
                }
                else {
                    cboSpecialty.SetValue('');
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
            if ($('#<%=hdnRoomID.ClientID %>').val() == '') getRoom('');
        }
        //#endregion

        function onCboSpecialtyValueChanged() {
            $('#<%:hdnLastSpecialty.ClientID %>').val(cboSpecialty.GetValue());
        }

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
                $('#<%:txtClassCode.ClientID %>').val(value);
                onTxtClassCodeChanged(value);
            });
        });

        $('#<%:txtClassCode.ClientID %>').live('change', function () {
            onTxtClassCodeChanged($(this).val());
        });

        function onTxtClassCodeChanged(value) {
            var filterExpression = getClassCareFilterExpression() + " AND ClassCode = '" + value + "'";
            Methods.getObject('GetClassCareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnClassID.ClientID %>').val(result.ClassID);
                    $('#<%:txtClassName.ClientID %>').val(result.ClassName);

                    if (result.IsUsedInChargeClass) {
                        $('#<%:hdnChargeClassID.ClientID %>').val(result.ClassID);
                        $('#<%:hdnChargeClassBPJSCode.ClientID %>').val(result.BPJSClassCode);
                        $('#<%:hdnChargeClassBPJSType.ClientID %>').val(result.BPJSClassType);
                        $('#<%:txtChargeClassCode.ClientID %>').val(result.ClassCode);
                        $('#<%:txtChargeClassName.ClientID %>').val(result.ClassName);
                    }
                    else {
                        $('#<%:hdnChargeClassID.ClientID %>').val('');
                        $('#<%:hdnChargeClassBPJSCode.ClientID %>').val('');
                        $('#<%:hdnChargeClassBPJSType.ClientID %>').val('');
                        $('#<%:txtChargeClassCode.ClientID %>').val('');
                        $('#<%:txtChargeClassName.ClientID %>').val('');
                    }
                }
                else {
                    $('#<%:hdnClassID.ClientID %>').val('');
                    $('#<%:txtClassCode.ClientID %>').val('');
                    $('#<%:txtClassName.ClientID %>').val('');
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSCode.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSType.ClientID %>').val('');
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                }
                if ($('#<%:hdnHealthcareServiceUnitID.ClientID %>').val() != '') {
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                    $('#<%:hdnIsServiceUnitHasParamedic.ClientID %>').val('0');
                    $('#<%:hdnIsServiceUnitHasVisitType.ClientID %>').val('0');
                }
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
            });
        }
        //#endregion

        //#region Request Class Care
        $('#<%:lblClassRequest.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('classcare', "IsDeleted = 0 AND IsUsedInChargeClass = 1", function (value) {
                $('#<%:txtClassRequestCode.ClientID %>').val(value);
                onTxtClassRequestCodeChanged(value);
            });
        });

        $('#<%:txtClassRequestCode.ClientID %>').live('change', function () {
            onTxtClassRequestCodeChanged($(this).val());
        });

        function onTxtClassRequestCodeChanged(value) {
            var filterExpression = "IsDeleted = 0 AND ClassCode = '" + value + "'";
            Methods.getObject('GetClassCareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnClassRequestID.ClientID %>').val(result.ClassID);
                    $('#<%:txtClassRequestCode.ClientID %>').val(result.ClassCode);
                    $('#<%:txtClassRequestName.ClientID %>').val(result.ClassName);
                }
                else {
                    $('#<%:hdnClassRequestID.ClientID %>').val('');
                    $('#<%:txtClassRequestCode.ClientID %>').val('');
                    $('#<%:txtClassRequestName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Charge Class
        function onGetChargeClassFilterExpression() {
            return "IsUsedInChargeClass = 1 AND IsDeleted = 0"
        }

        $('#<%:lblChargeClass.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('classcare', onGetChargeClassFilterExpression(), function (value) {
                $('#<%:txtChargeClassCode.ClientID %>').val(value);
                onTxtChargeClassCodeChanged(value);
            });
        });

        $('#<%:txtChargeClassCode.ClientID %>').live('change', function () {
            onTxtChargeClassCodeChanged($(this).val());
        });

        function onTxtChargeClassCodeChanged(value) {
            var filterExpression = onGetChargeClassFilterExpression() + " AND ClassCode = '" + value + "'";
            Methods.getObject('GetClassCareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnChargeClassID.ClientID %>').val(result.ClassID);
                    $('#<%:hdnChargeClassBPJSCode.ClientID %>').val(result.BPJSClassCode);
                    $('#<%:hdnChargeClassBPJSType.ClientID %>').val(result.BPJSClassType);
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ClassName);
                }
                else {
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSCode.ClientID %>').val('');
                    $('#<%:hdnChargeClassBPJSType.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Service Unit
        var serviceUnitUserCount = parseInt('<%:serviceUnitUserCount %>');
        function getServiceUnitFilterFilterExpression() {
            var filterExpression = '';
            if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT) {
                var classID = $('#<%:hdnClassID.ClientID %>').val();
                if (classID != '')
                    filterExpression = 'HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitRoom WHERE ClassID = ' + classID + ')';
            }
            else if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.DIAGNOSTIC) {
                filterExpression = '<%:filterExpressionOtherMedicalDiagnostic %>';
            }


            if ($('#<%:hdnDiagnosticType.ClientID %>').val() == "0") {
                //                filterExpression = 'HealthcareServiceUnitID = ' + $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                filterExpression = "IsLaboratoryUnit = 1";
            }
            else if ($('#<%:hdnDiagnosticType.ClientID %>').val() == "1") {
                filterExpression = 'HealthcareServiceUnitID = ' + $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            }

            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
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
                $('#<%:txtServiceUnitCode.ClientID %>').val(value);
                onTxtClinicCodeChanged(value);
            });
        });

        $('#<%:txtServiceUnitCode.ClientID %>').live('change', function () {
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
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

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
                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                }
                getRoom('');

                var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
                if (paramedicID != '' && paramedicID != '0') {
                    if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                        var filterExpression = onGetVisitTypeFilterExpression();
                        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                            }
                            else {
                                $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                $('#<%:txtVisitTypeName.ClientID %>').val('');
                            }
                        });
                        if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.OUTPATIENT) {
                            var filterExpression = 'ParamedicID = ' + paramedicID + ' AND HealthcareServiceUnitID = ' + $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val() + ' AND DayNumber = (DATEPART(dw, GETDATE()) + 5) % 7 + 1';
                            Methods.getObject('GetvParamedicScheduleList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnRoomID.ClientID %>').val(result.RoomID);
                                    $('#<%:txtRoomCode.ClientID %>').val(result.RoomCode);
                                    $('#<%:txtRoomName.ClientID %>').val(result.RoomName);
                                }
                                else {
                                    $('#<%:hdnRoomID.ClientID %>').val('');
                                    $('#<%:txtRoomCode.ClientID %>').val('');
                                    $('#<%:txtRoomName.ClientID %>').val('');
                                }
                            });
                        }
                    }
                }
            });
        }
        //#endregion

        //#region Item MCU
        function getItemMasterMCUFilterExpression() {
            var filterExpression = "<%:GetItemMasterFilterExpression() %>";
            return filterExpression;
        }

        $('#<%:lblItemMCU.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('item', getItemMasterMCUFilterExpression(), function (value) {
                $('#<%:txtItemCode.ClientID %>').val(value);
                onTxtItemMasterCodeChanged(value);
            });
        });

        $('#<%:txtItemCode.ClientID %>').live('change', function () {
            onTxtItemMasterCodeChanged($(this).val());
        });

        function onTxtItemMasterCodeChanged(value) {
            var filterExpression = getItemMasterMCUFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%:txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%:hdnItemID.ClientID %>').val('');
                    $('#<%:txtItemCode.ClientID %>').val('');
                    $('#<%:txtItemName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Item AIO
        function getItemMasterAIOFilterExpression() {
            var filterExpression = "<%:GetItemMasterAIOFilterExpression() %>";
            return filterExpression;
        }

        $('#<%:lblItemAIO.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('item', getItemMasterAIOFilterExpression(), function (value) {
                $('#<%:txtItemAIOCode.ClientID %>').val(value);
                onTxtItemMasterAIOCodeChanged(value);
            });
        });

        $('#<%:txtItemAIOCode.ClientID %>').live('change', function () {
            onTxtItemMasterAIOCodeChanged($(this).val());
        });

        function onTxtItemMasterAIOCodeChanged(value) {
            var filterExpression = getItemMasterAIOFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnItemAIOID.ClientID %>').val(result.ItemID);
                    $('#<%:txtItemAIOName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%:hdnItemAIOID.ClientID %>').val('');
                    $('#<%:txtItemAIOCode.ClientID %>').val('');
                    $('#<%:txtItemAIOName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Room

        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
            var classID = $('#<%:hdnClassID.ClientID %>').val();
            var filterExpression = '';

            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }

            if (deptID == "INPATIENT") {
                if (classID != '0' && classID != '') {
                    if (filterExpression != '') {
                        filterExpression += " AND ";
                    }
                    filterExpression += "ClassID = " + classID;
                }
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0 AND DepartmentID = '" + deptID + "'";

            return filterExpression;
        }

        $('#<%:lblRoom.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('serviceunitroom', getRoomFilterExpression(), function (value) {
                $('#<%:txtRoomCode.ClientID %>').val(value);
                onTxtRoomCodeChanged(value);
            });
        });

        $('#<%:txtRoomCode.ClientID %>').live('change', function () {
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
                        $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                        $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                        $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                        if ($('#<%:hdnClassID.ClientID %>').val() == '') {
                            $('#<%:hdnClassID.ClientID %>').val(result[0].ClassID);
                            $('#<%:txtClassCode.ClientID %>').val(result[0].ClassCode);
                            $('#<%:txtClassName.ClientID %>').val(result[0].ClassName);
                            $('#<%:hdnChargeClassID.ClientID %>').val(result[0].ChargeClassID);
                            $('#<%:hdnChargeClassBPJSCode.ClientID %>').val(result[0].ChargeClassBPJSCode);
                            $('#<%:hdnChargeClassBPJSType.ClientID %>').val(result[0].ChargeClassBPJSType);
                            $('#<%:txtChargeClassCode.ClientID %>').val(result[0].ChargeClassCode);
                            $('#<%:txtChargeClassName.ClientID %>').val(result[0].ChargeClassName);
                        }
                    }
                    else {
                        $('#<%:hdnRoomID.ClientID %>').val('');
                        $('#<%:txtRoomCode.ClientID %>').val('');
                        $('#<%:txtRoomName.ClientID %>').val('');
                    }
                    $('#<%:hdnBedID.ClientID %>').val('');
                    $('#<%:txtBedCode.ClientID %>').val('');
                });
            } else {
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Bed
        $('#<%:lblBed.ClientID %>.lblLink').live('click', function () {
            var roomID = $('#<%:hdnRoomID.ClientID %>').val();
            var filterExpression = '';
            if (roomID != '') {
                filterExpression = "RoomID = " + roomID + " AND GCBedStatus = '0116^U'";
                openSearchDialog('bed', filterExpression, function (value) {
                    $('#<%:txtBedCode.ClientID %>').val(value);
                    onTxtBedCodeChanged(value);
                });
            }
        });

        $('#<%:txtBedCode.ClientID %>').live('change', function () {
            onTxtBedCodeChanged($(this).val());
        });

        function onTxtBedCodeChanged(value) {
            var roomID = $('#<%:hdnRoomID.ClientID %>').val();
            var filterExpression = '';
            if (roomID != '') {
                filterExpression = "RoomID = " + roomID + " AND ";
                filterExpression += "BedCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvBedList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnBedID.ClientID %>').val(result.BedID);
                        $('#<%:hdnExtensionNo.ClientID %>').val(result.ExtensionNo);
                    }
                    else {
                        $('#<%:hdnBedID.ClientID %>').val('');
                        $('#<%:txtBedCode.ClientID %>').val('');
                        $('#<%:hdnExtensionNo.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Visit Type
        function onGetVisitTypeFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            if (paramedicID == '')
                paramedicID = '0';
            var filterExpression = serviceUnitID + ';' + paramedicID + ';';
            return filterExpression;
        }

        $('#<%:lblVisitType.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedicvisittype', onGetVisitTypeFilterExpression(), function (value) {
                $('#<%:txtVisitTypeCode.ClientID %>').val(value);
                onTxtVisitTypeCodeChanged(value);
            });
        });

        $('#<%:txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtVisitTypeCodeChanged($(this).val());
        });

        function onTxtVisitTypeCodeChanged(value) {
            var filterExpression = onGetVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
            Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                    $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                }
                else {
                    $('#<%:hdnVisitTypeID.ClientID %>').val('');
                    $('#<%:txtVisitTypeCode.ClientID %>').val('');
                    $('#<%:txtVisitTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Referral Description
        function getReferralDescriptionFilterExpression() {
            var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1";
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
                filterExpression = getReferralParamedicFilterExpression() + " AND IsDeleted = 0 AND IsAvailable = 1 AND ParamedicCode = '" + value + "'";
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

        //#region Payer Company
        function getPayerCompanyFilterExpression() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
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
                                Methods.getObject('GetvCustomerContract1List', filterExpression, function (result) {
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
                                        onCheckCustomerMember($('#<%:hdnPayerID.ClientID %>').val(), $('#<%:txtMRN.ClientID %>').val(), $('#<%:txtParticipantNo.ClientID %>').val());
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
                Methods.getObject('GetvCustomerMemberList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:txtParticipantNo.ClientID %>').val(result.MemberNo);
                        $('#<%:txtParticipantNo.ClientID %>').attr('readonly', 'readonly');
                    }
                    else {
                        if (regID == null || regID == 0) {
                            $('#<%:txtParticipantNo.ClientID %>').val('');
                            $('#<%:txtParticipantNo.ClientID %>').removeAttr('readonly');
                        }
                        else {
                            $('#<%:txtParticipantNo.ClientID %>').val(noPeserta);
                            $('#<%:txtParticipantNo.ClientID %>').attr('readonly', 'readonly');
                        }
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

        $('#btnPayerNotesDetail').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var id = $('#<%:hdnPayerID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/CustomerNotesDetailCtl.ascx");
                openUserControlPopup(url, id, 'Notes', 500, 400);
            }
        });

        //        $('#btnContractSummary').live('click', function () {
        //            if ($(this).attr('enabled') == null) {
        //                var id = $('#<%:hdnContractID.ClientID %>').val();
        //                var url = ResolveUrl("~/Libs/Program/Information/CustomerContractSummaryViewCtl.ascx");
        //                openUserControlPopup(url, id, 'Ringkasan Kontrak', 700, 600);
        //            }
        //        });

        //        $('#btnCustomerContractDocumentInfo').live('click', function () {
        //            if (cboRegistrationPayer.GetText() != 'Pribadi') {
        //                if ($(this).attr('enabled') == null) {
        //                    var payer = $('#<%:hdnPayerID.ClientID %>').val();
        //                    var contract = $('#<%:hdnContractID.ClientID %>').val();

        //                    var filterExpression = "ContractID = " + contract;
        //                    Methods.getObject('GetContractDocumentList', filterExpression, function (result) {
        //                        if (result != null) {
        //                            var url = ResolveUrl("~/Libs/Program/Information/InformationCustomerPayerCtl.ascx");
        //                            var id = payer + "|" + contract;
        //                            openUserControlPopup(url, id, 'Informasi Instansi', 700, 600);
        //                        }
        //                        else {
        //                            showToast("INFORMASI KONTRAK REKANAN", "Tidak memiliki dokumen kontrak");
        //                        }
        //                    });
        //                }
        //            } else {
        //                showToast("INFORMASI KONTRAK REKANAN", "Tidak ada kontrak");
        //            }
        //        });

        $('#btnCustomerContractDocumentInfo').live('click', function () {
            if (cboRegistrationPayer.GetText() != 'Pribadi') {
                if ($(this).attr('enabled') == null) {
                    var payer = $('#<%:hdnPayerID.ClientID %>').val();
                    var contract = $('#<%:hdnContractID.ClientID %>').val();
                    var url = ResolveUrl("~/Libs/Program/Information/InformationCustomerPayerCtl.ascx");
                    var id = payer + "|" + contract;
                    openUserControlPopup(url, id, 'Informasi Instansi', 700, 600);
                }
            } else {
                showToast("INFORMASI KONTRAK REKANAN", "Tidak ada kontrak");
            }
        });

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

        //#region Diagnose
        $('#<%:lblDiagnose.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                onTxtDiagnoseCodeChanged(value);
            });
        });

        $('#<%=txtDiagnoseCode.ClientID %>').live('change', function () {
            onTxtDiagnoseCodeChanged($(this).val());
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' OR BPJSReferenceInfo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=txtDiagnoseText.ClientID %>').val(result.DiagnoseName);
                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val(result.BPJSReferenceInfo);
                }
                else {
                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                    $('#<%=txtDiagnoseText.ClientID %>').val('');
                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                }
            });
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

        //#region Promotion
        function getPromotionFilterExpression() {
            var dateNow = $('#<%:txtRegistrationDate.ClientID %>').val();
            var registrationDateNowInDatePicker = Methods.getDatePickerDate(dateNow);
            var registrationDateNowFormatString = Methods.dateToString(registrationDateNowInDatePicker);

            var filterExpression = "GCPromotionType = 'X415^001' AND IsDeleted = 0 AND ('" + registrationDateNowFormatString + "' BETWEEN StartDate AND EndDate) AND PromotionSchemeID IN (SELECT PromotionSchemeID FROM HealthcarePromotionScheme WHERE HealthcareID = '" + AppSession.healthcareID + "')";
            return filterExpression;
        }

        $('#<%:lblPromotion.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('promotion', getPromotionFilterExpression(), function (value) {
                $('#<%:txtPromotionCode.ClientID %>').val(value);
                onTxtPromotionCodeChanged(value);
            });
        });

        $('#<%:txtPromotionCode.ClientID %>').live('change', function () {
            onTxtPromotionCodeChanged($(this).val());
        });

        function onTxtPromotionCodeChanged(value) {
            var filterExpression = getPromotionFilterExpression() + " AND PromotionSchemeCode = '" + value + "'";
            getPromotion(filterExpression);
        }

        function getPromotion(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getPromotionFilterExpression();
            Methods.getObject('GetPromotionSchemeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnPromotionID.ClientID %>').val(result.PromotionSchemeID);
                    $('#<%:txtPromotionCode.ClientID %>').val(result.PromotionSchemeCode);
                    $('#<%:txtPromotionName.ClientID %>').val(result.PromotionSchemeName);
                }
                else {
                    $('#<%:hdnPromotionID.ClientID %>').val('');
                    $('#<%:txtPromotionCode.ClientID %>').val('');
                    $('#<%:txtPromotionName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region From Registration No
        function getFromRegistrationNoFilterExpression() {
            var departmentID = '';
            var admissionSource = cboAdmissionSource.GetValue();
            switch (admissionSource) {
                case Constant.AdmissionSource.EMERGENCY: departmentID = Constant.Facility.EMERGENCY; break;
                case Constant.AdmissionSource.DIAGNOSTIC: departmentID = Constant.Facility.DIAGNOSTIC; break;
                default: departmentID = Constant.Facility.OUTPATIENT; break;
            }
            var filterExpression = "DepartmentID = '" + departmentID + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.OPEN + "','" + Constant.RegistrationStatus.CLOSED + "','" + Constant.RegistrationStatus.CANCELLED + "')";
            filterExpression += " AND LinkedToRegistrationID IS NULL";

            return filterExpression;
        }

        $('#<%:lblFromRegistrationNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('registration', getFromRegistrationNoFilterExpression(), function (value) {
                $('#<%:txtFromRegistrationNo.ClientID %>').val(value);
                onTxtFromRegistrationNoChanged(value);
            });
        });

        $('#<%:txtFromRegistrationNo.ClientID %>').live('change', function () {
            onTxtFromRegistrationNoChanged($(this).val());
        });

        function onTxtFromRegistrationNoChanged(value) {
            var filterExpression = getFromRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
            Methods.getObject('GetvRegistration7List', filterExpression, function (result) {
                if (result != null) {
                    var blok24jam = $('#<%:hdnRegistrasiAsalDiblokJikaLebih24Jam.ClientID %>').val();

                    var regDate = $('#<%:txtRegistrationDate.ClientID %>').val();
                    var regTime = $('#<%:txtRegistrationHour.ClientID %>').val();
                    var regTemp = Methods.DatePickerToDateFormat(regDate) + "" + regTime.substring(0, 2) + "" + regTime.substring(3, 5);
                    var regTempDate = Methods.stringToDateTime(regTemp);

                    var regFromDate = Methods.DatePickerToDateFormat(result.RegistrationDateInDatePicker);
                    var regFromTime = result.RegistrationTime;
                    var regFromTemp = regFromDate + "" + regFromTime.substring(0, 2) + "" + regFromTime.substring(3, 5);
                    var regFromTempDate = Methods.stringToDateTime(regFromTemp);

                    var diff = Methods.calculateDateTimeDifference(regTempDate, regFromTempDate);

                    if (diff > 24) {
                        var regFromDate = 'Registrasi Asal <b>' + result.RegistrationNo + ' (' + Methods.dateToDMY(regFromTempDate) + ' ' + result.RegistrationTime + '</b>) sudah lebih dari 24 jam lalu.';
                        if (blok24jam == "1") {
                            showToast('GAGAL', regFromDate);

                            $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                            $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                            $('#<%:lblMRN.ClientID %>').attr('class', 'lblLink lblMandatory');
                            $('#<%:txtMRN.ClientID %>').removeAttr('readonly');
                            $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');
                            $('#<%:txtServiceRegFromInfo.ClientID %>').val('');
                            $('#<%=trServiceRegFromInfo.ClientID %>').attr('style', 'display:none');
                        } else {
                            showToastConfirmation(regFromDate, function (resultFromDate) {
                                if (resultFromDate) {
                                    $('#<%:hdnFromRegistrationID.ClientID %>').val(result.RegistrationID);
                                    $('#<%:txtRoomType.ClientID %>').val(result.RoomType);
                                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                                    $('#<%:lblMRN.ClientID %>').attr('class', 'lblDisabled');
                                    $('#<%:txtMRN.ClientID %>').attr('readonly', 'readonly');
                                    $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val(result.IsNewPatient ? '1' : '0');
                                    if ($('#<%:hdnParamedicID.ClientID %>').val() == "" || $('#<%:hdnParamedicID.ClientID %>').val() == "0") {
                                        if (result.ReferralID != 0) {
                                            $('#<%:hdnParamedicID.ClientID %>').val(result.ReferralPhysicianID);
                                            onHdnPhysicianIDChanged(result.ReferralPhysicianID);
                                        }
                                    }
                                    onTxtMRNChanged(result.MedicalNo);
                                    $('#<%=txtDiagnoseCode.ClientID %>').val(result.DiagnoseID);
                                    onTxtDiagnoseCodeChanged(result.DiagnoseID);
                                    $('#<%=txtDiagnoseText.ClientID %>').val(result.DiagnosisText)

                                    if (result.GCReferrerGroup != '') {
                                        cboReferral.SetValue(result.GCReferrerGroup);
                                    }
                                    else {
                                        cboReferral.SetValue(Constant.ReferrerGroup.DOKTERRS);
                                    }
                                    onCboReferralValueChanged();
                                    if (result.ReferrerCode != '') {
                                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ReferrerCode);
                                    }
                                    else {
                                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ParamedicCode);
                                    }

                                    onTxtReferralDescriptionCodeChanged($('#<%:txtReferralDescriptionCode.ClientID %>').val());
                                    $('#<%=txtReferralNo.ClientID %>').val(result.ReferralNo);
                                    $('#<%=txtTglRujukan.ClientID %>').val(result.cfTanggalRujukan2);
                                    $('#<%:chkIsParturition.ClientID %>').prop('checked', result.IsParturition);

                                    if (result.cfServiceRegFromInfo != '') {
                                        $('#<%:txtServiceRegFromInfo.ClientID %>').val(result.cfServiceRegFromInfo);
                                        $('#<%=trServiceRegFromInfo.ClientID %>').removeAttr('style');
                                    }
                                    else {
                                        $('#<%=trServiceRegFromInfo.ClientID %>').attr('style', 'display:none');
                                    }
                                }
                                else {
                                    $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                                    $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                                    $('#<%:lblMRN.ClientID %>').attr('class', 'lblLink lblMandatory');
                                    $('#<%:txtMRN.ClientID %>').removeAttr('readonly');
                                    $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');
                                    $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                                    $('#<%:txtServiceRegFromInfo.ClientID %>').val('');
                                    $('#<%=trServiceRegFromInfo.ClientID %>').attr('style', 'display:none');
                                }
                            });
                        }
                    } else {
                        $('#<%:hdnFromRegistrationID.ClientID %>').val(result.RegistrationID);
                        $('#<%:txtRoomType.ClientID %>').val(result.RoomType);
                        $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                        $('#<%:lblMRN.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%:txtMRN.ClientID %>').attr('readonly', 'readonly');
                        $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val(result.IsNewPatient ? '1' : '0');
                        if ($('#<%:hdnParamedicID.ClientID %>').val() == "" || $('#<%:hdnParamedicID.ClientID %>').val() == "0") {
                            if (result.ReferralID != 0) {
                                $('#<%:hdnParamedicID.ClientID %>').val(result.ReferralPhysicianID);
                                onHdnPhysicianIDChanged(result.ReferralPhysicianID);
                            }
                        }
                        onTxtMRNChanged(result.MedicalNo);
                        $('#<%=txtDiagnoseCode.ClientID %>').val(result.DiagnoseID);
                        onTxtDiagnoseCodeChanged(result.DiagnoseID);
                        $('#<%=txtDiagnoseText.ClientID %>').val(result.DiagnosisText)
                        if (result.GCReferrerGroup != '') {
                            cboReferral.SetValue(result.GCReferrerGroup);
                        }
                        else {
                            cboReferral.SetValue(Constant.ReferrerGroup.DOKTERRS);
                        }
                        onCboReferralValueChanged();
                        if (result.ReferrerCode != '') {
                            $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ReferrerCode);
                        }
                        else {
                            $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ParamedicCode);
                        }
                        onTxtReferralDescriptionCodeChanged($('#<%:txtReferralDescriptionCode.ClientID %>').val());
                        $('#<%=txtReferralNo.ClientID %>').val(result.ReferralNo);
                        $('#<%=txtTglRujukan.ClientID %>').val(result.cfTanggalRujukan2);
                        $('#<%:chkIsParturition.ClientID %>').prop('checked', result.IsParturition);

                        if (result.cfServiceRegFromInfo != '') {
                            $('#<%:txtServiceRegFromInfo.ClientID %>').val(result.cfServiceRegFromInfo);
                            $('#<%=trServiceRegFromInfo.ClientID %>').removeAttr('style');
                        }
                        else {
                            $('#<%=trServiceRegFromInfo.ClientID %>').attr('style', 'display:none');
                        }
                    }
                }
                else {
                    $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                    $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                    $('#<%:lblMRN.ClientID %>').attr('class', 'lblLink lblMandatory');
                    $('#<%:txtMRN.ClientID %>').removeAttr('readonly');
                    $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');
                    $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                    $('#<%:txtServiceRegFromInfo.ClientID %>').val('');
                    $('#<%=trServiceRegFromInfo.ClientID %>').attr('style', 'display:none');
                }
            });
        }
        //#endregion

        //#region To Registration No
        function getToRegistrationNoFilterExpression() {
            var filterExpression = "GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.OPEN + "','" + Constant.RegistrationStatus.CLOSED + "','" + Constant.RegistrationStatus.CANCELLED + "') AND RegistrationID IN (SELECT RegistrationID FROM Registration WHERE LinkedToRegistrationID IS NULL AND GCRegistrationStatus != 'X020^006')";
            return filterExpression;
        }

        $('#<%:lblToRegistrationNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('registration', getToRegistrationNoFilterExpression(), function (value) {
                $('#<%:txtToRegistrationNo.ClientID %>').val(value);
                onTxtToRegistrationNoChanged(value);
            });
        });
        $('#<%:txtToRegistrationNo.ClientID %>').live('change', function () {
            onTxtToRegistrationNoChanged($(this).val());
        });
        function onTxtToRegistrationNoChanged(value) {
            var filterExpression = getToRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
            Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnToRegistrationID.ClientID %>').val(result.RegistrationID);
                }
                else {
                    $('#<%:hdnToRegistrationID.ClientID %>').val('');
                    $('#<%:txtToRegistrationNo.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Partus & New Born
        $('#<%:chkIsParturition.ClientID %>').live('change', function () {
            $chkIsNewBorn = $('#<%:chkIsNewBorn.ClientID %>');
            if ($(this).is(':checked')) {
                $chkIsNewBorn.attr("disabled", true);
                $('#<%:hdnMotherMRN %>').val("");
                $('#<%:txtMotherRegNo %>').val("");
            }
            else {
                $chkIsNewBorn.removeAttr("disabled");

            }
        });

        $('#<%:chkIsNewBorn.ClientID %>').live('change', function () {
            $chkIsParturition = $('#<%:chkIsParturition.ClientID %>');
            $chkIsPregnant = $('#<%:chkIsPregnant.ClientID %>');
            if ($('#<%:hdnGCGender.ClientID %>').val() == '<%:GetGenderFemale() %>') {
                if ($(this).is(':checked')) {
                    $chkIsParturition.attr("disabled", true);
                    $chkIsPregnant.attr("disabled", true);
                    $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                    $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                    $('#<%=trMotherRegNo.ClientID %>').show();
                }
                else {
                    $chkIsParturition.removeAttr("disabled");
                    $chkIsPregnant.removeAttr("disabled");
                    $('#<%:hdnMotherMRN.ClientID %>').val("");
                    $('#<%:txtMotherRegNo.ClientID %>').val("");
                    $('#<%=trMotherRegNo.ClientID %>').hide();
                }
            }
            else {
                if ($(this).is(':checked')) {
                    $('#<%=trMotherRegNo.ClientID %>').show();
                }
                else {
                    $('#<%:hdnMotherMRN.ClientID %>').val("");
                    $('#<%:txtMotherRegNo.ClientID %>').val("");
                    $('#<%=trMotherRegNo.ClientID %>').hide();
                }
            }
        });

        $('#<%:chkIsPregnant.ClientID %>').live('change', function () {
            $chkIsPregnant = $('#<%:chkIsPregnant.ClientID %>');
            if ($(this).is(':checked')) {
                $chkIsNewBorn.attr("disabled", true);
                $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);
            }
            else {
                $chkIsNewBorn.removeAttr("disabled");

            }
        });
        //#endregion

        //#region temporary location
        $('#<%:chkIsTemporaryLocation.ClientID %>').live('change', function () {
            $chkIsTemporaryLocation = $('#<%:chkIsTemporaryLocation.ClientID %>');
            if ($(this).is(':checked')) {
                $('#<%:trClassRequest.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:trClassRequest.ClientID %>').attr('style', 'display:none');
            }

            $('#<%:hdnClassRequestID.ClientID %>').val('');
            $('#<%:txtClassRequestCode.ClientID %>').val('');
            $('#<%:txtClassRequestName.ClientID %>').val('');
        });
        //#endregion

        //#region Is Has MRN
        $('#<%:chkIsHasMRN.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').removeAttr('enabled');
                $('#btnDataPasien').attr('style', 'display:none');
                $('#<%:trMRN.ClientID %>').removeAttr('style');
                $('#<%:chkCardFee.ClientID %>').removeAttr('disabled');
                $('#<%:trGuestNo.ClientID %>').attr('style', 'display:none');
            }
            else {
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').attr('enabled', 'false');
                $('#<%:chkCardFee.ClientID %>').prop('checked', false);
                $('#<%:chkCardFee.ClientID %>').attr("disabled", true);
                $('#btnDataPasien').removeAttr('style');
                $('#<%:trMRN.ClientID %>').attr('style', 'display:none');
                $('#<%:trGuestNo.ClientID %>').removeAttr('style');
                $('#<%:txtGuestNo.ClientID %>').val('');
            }
        });

        $('#btnDataPasien').live('click', function () {
            var id = $('#<%:hdnGuestID.ClientID %>').val();
            var registrationNo = $('#<%:txtRegistrationNo.ClientID %>').val();
            var param = id + "|" + registrationNo;
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Registration/GuestEntryCtl.ascx');
            openUserControlPopup(url, param, 'Identitas Pasien Pemeriksaan APS', 980, 600, 'guestIdentity');
        });
        //#endregion

        $('#btnBedQuickPicks').live('click', function () {
            var url = ResolveUrl('~/Controls/BedQuickPicksCtl.ascx');
            openUserControlPopup(url, '', 'Pilih Tempat Tidur', 1200, 550);
        });

        $('#<%:chkParamedicHasSchedule.ClientID %>').live('change', function () {
            $chkParamedicHasSchedule = $('#<%:chkParamedicHasSchedule.ClientID %>');

            $('#<%:hdnParamedicID.ClientID %>').val('');
            $('#<%:txtPhysicianCode.ClientID %>').val('');
            $('#<%:txtPhysicianName.ClientID %>').val('');
            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%:txtServiceUnitCode.ClientID %>').val('');
            $('#<%:txtServiceUnitName.ClientID %>').val('');

            if ($(this).is(':checked')) {
                $('#<%:lblPhysician.ClientID %>').removeAttr('class');
                $('#<%:lblServiceUnit.ClientID %>').removeAttr('class');
                $('#<%:txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%:btnParamedicSelection.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:lblPhysician.ClientID %>').attr('class', 'lblLink');
                $('#<%:lblServiceUnit.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtPhysicianCode.ClientID %>').removeAttr('readonly');
                $('#<%:txtServiceUnitCode.ClientID %>').removeAttr('readonly');
                $('#<%:btnParamedicSelection.ClientID %>').attr('style', 'display:none');
            }
        });

        $('#<%:btnParamedicSelection.ClientID %>').live('click', function () {
            var regDate = $('#<%:txtRegistrationDate.ClientID %>').val();
            var regTime = $('#<%:txtRegistrationHour.ClientID %>').val();
            var regTemp = Methods.DatePickerToDateFormat(regDate) + "" + regTime.substring(0, 2) + "" + regTime.substring(3, 5);
            var regTempDate = Methods.stringToDateTime(regTemp);
            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
            var paramDeptID = $('#<%:hdnDepartmentIDFilterAppointment.ClientID %>').val();
            var diagnosticType = $('#<%:hdnDiagnosticType.ClientID %>').val();
            var hsuID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterOther = '<%:filterExpressionOtherMedicalDiagnostic %>';
            var id = regDate + '|' + regTime + '|' + regTempDate + '|' + deptID + '|' + diagnosticType + '|' + hsuID + '|' + filterOther + '|' + paramDeptID;
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Registration/ParamedicEntryPopupCtl.ascx');
            openUserControlPopup(url, id, 'Pilih Klinik / Dokter', 980, 150, 'ParamedicEntryPopup');
        });

        function onAfterClickBedQuickPicks(healthcareServiceUnitID, serviceUnitCode, serviceUnitName, roomID, roomCode, roomName, classID, classCode, className, chargeClassID, chargeClassBPJSCode, chargeClassCode, chargeClassName, bedID, bedCode, chargeClassBPJSType) {
            if ($('#<%:hdnMotherMRN.ClientID %>').val() == "") {
                $('#<%:hdnChargeClassBPJSCode.ClientID %>').val(chargeClassBPJSCode);
                $('#<%:hdnChargeClassBPJSType.ClientID %>').val(chargeClassBPJSType);
                $('#<%:hdnChargeClassID.ClientID %>').val(chargeClassID);
                if ($('#<%:hdnIsAutomaticInputChargeClass.ClientID %>').val() == '1') {
                    $('#<%:txtChargeClassCode.ClientID %>').val(chargeClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(chargeClassName);
                    $('#<%:txtClassCode.ClientID %>').val(classCode);
                    $('#<%:txtClassName.ClientID %>').val(className);
                } else {
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                    $('#<%:txtClassCode.ClientID %>').val('');
                    $('#<%:txtClassName.ClientID %>').val('');
                }
            }
            $('#<%:hdnClassID.ClientID %>').val(classID);
            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(healthcareServiceUnitID);
            $('#<%:txtServiceUnitCode.ClientID %>').val(serviceUnitCode);
            $('#<%:txtServiceUnitName.ClientID %>').val(serviceUnitName);
            $('#<%:hdnRoomID.ClientID %>').val(roomID);
            $('#<%:txtRoomCode.ClientID %>').val(roomCode);
            $('#<%:txtRoomName.ClientID %>').val(roomName);
            $('#<%:hdnBedID.ClientID %>').val(bedID);
            $('#<%:txtBedCode.ClientID %>').val(bedCode);

            if ($('#<%:hdnIsControlClassCare.ClientID %>').val() == "1") {
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

        function onCboReferralValueChanged(s) {
            $('#<%:hdnReferrerID.ClientID %>').val('');
            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
                $('#<%:txtReferralNo.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtReferralNo.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtReferralNo.ClientID %>').val('');
            }
        }

        function onCboAdmissionSourceValueChanged(s) {
            $('#<%:hdnFromRegistrationID.ClientID %>').val('');
            $('#<%:txtFromRegistrationNo.ClientID %>').val('');
            if (s.GetValue() != Constant.AdmissionSource.INPATIENT) {
                $('#<%:lblFromRegistrationNo.ClientID %>').attr('class', 'lblLink lblMandatory');
                $('#<%:txtFromRegistrationNo.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblFromRegistrationNo.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtFromRegistrationNo.ClientID %>').attr('readonly', 'readonly');
            }
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER)
                $('#<%:txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%:txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
            if (cboVisitReason.GetValue() == Constant.VisitReason.ACCIDENT) {
                $('#<%=trAccidentLocation1.ClientID %>').attr('style', '');
                $('#<%=trAccidentLocation2.ClientID %>').attr('style', '');
                $('#<%=trAccidentLocation3.ClientID %>').attr('style', '');
                $('#<%=trAccidentLocation4.ClientID %>').attr('style', '');
                $('#<%=trSuplesi.ClientID %>').attr('style', '');
                $('#<%=trAccidentPayor.ClientID %>').attr('style', '');
            }
            else {
                $('#<%=trAccidentLocation1.ClientID %>').attr('style', 'display:none');
                $('#<%=trAccidentLocation2.ClientID %>').attr('style', 'display:none');
                $('#<%=trAccidentLocation3.ClientID %>').attr('style', 'display:none');
                $('#<%=trAccidentLocation4.ClientID %>').attr('style', 'display:none');
                $('#<%=trSuplesi.ClientID %>').attr('style', 'display:none');
                $('#<%=trAccidentPayor.ClientID %>').attr('style', 'display:none');
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
            var isUsingPromo = $('#<%:hdnIsRegistrationUsingPromotionScheme.ClientID %>').val();
            var customerType = cboRegistrationPayer.GetValue();
            if (customerType == "<%:GetCustomerTypePersonal() %>") {
                $('#<%:tblPayerCompany.ClientID %>').hide();
                $('#<%:chkUsingCOB.ClientID %>').hide();

                if (isUsingPromo == '1') {
                    $('#<%:tblPromotion.ClientID %>').show();
                }
                else {
                    $('#<%:tblPromotion.ClientID %>').hide();
                }
            }
            else {
                $('#<%:tblPromotion.ClientID %>').hide();
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

        function onOverrideLoadRightPanelUrl(code, data) {
            if ($('#<%:hdnIsCheckNewPatient.ClientID %>').val() == '1') {
                if (code == 'patientIdentity' && ($('#<%:hdnMRN.ClientID %>').val() == '0' || ($('#<%:hdnMRN.ClientID %>').val() == ''))) {
                    var param = "";
                    if ($('#<%:txtMotherRegNo.ClientID %>').val() != "") {
                        if ($('#<%:hdnMRN.ClientID %>').val() == "") {
                            var filterExpression = "RegistrationNo = '" + $('#<%:txtMotherRegNo.ClientID %>').val() + "'";
                            Methods.getObject('GetRegistrationList', filterExpression, function (result) {
                                param = "mother|" + result.MRN;
                            });
                        }
                    }
                    $('#hdnRightPanelContentParam').val(param);
                    data.url = '~/Libs/Program/Module/PatientManagement/Registration/CheckNewPatientCtl.ascx';
                    data.width = 1200;
                    data.height = 550;
                    data.title = 'Pencarian Data Pasien';
                }
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            isChangePayer = false;
            if (code == 'patientIdentity') {
                var param = $('#<%:hdnMRN.ClientID %>').val() + "|" + $('#<%:hdnRegistrationID.ClientID %>').val();
                if ($('#<%:txtMotherRegNo.ClientID %>').val() != "") {
                    if ($('#<%:hdnMRN.ClientID %>').val() == "") {
                        var filterExpression = "RegistrationNo = '" + $('#<%:txtMotherRegNo.ClientID %>').val() + "'";
                        Methods.getObject('GetRegistrationList', filterExpression, function (result) {
                            param = "mother|" + result.MRN;
                        });
                    }
                }
                return param;
            }
            else if (code == 'resultDeliveryPlanEdit') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val();
                return param;
            }
            else if (code == 'suratRencanaInap') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val();
                return param;
            }
            else if (code == 'guestIdentity') {
                var param = $('#<%:hdnGuestID.ClientID %>').val();
                return param;
            }
            else if (code == 'patientFamily' || code == 'patientPhoto' || code == 'infoOutstandingPiutangPribadi' || code == 'infoHistoryRegistration') {
                var param = $('#<%:hdnMRN.ClientID %>').val();
                return param;
            }
            else if (code == 'guestPhoto') {
                var param = $('#<%:txtRegistrationNo.ClientID %>').val();
                return param;
            }
            else if (code == 'paymentLetter') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + "|pl";
                return param;
            }
            else if (code == 'generateSJP') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + "|" + $('#<%:hdnDepartmentID.ClientID %>').val() + "|" + $('#<%:txtInhealthParticipantNo.ClientID %>').val();
                return param;
            }
            else if (code == 'generateDataRujukan') {
                var noKartu = $('#<%=txtNHSRegistrationNo.ClientID %>').val();
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var param = noKartu + '|' + registrationID;
                return param;
            }
            else if (code == 'sendBARNotificationMessage') {
                if ($('#<%:hdnRegistrationID.ClientID %>').val() != '' || $('#<%:hdnRegistrationID.ClientID %>').val() != '0') {
                    var messageType = "00";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = registrationID;
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else if (code == 'sendIHSEncounter') {
                if ($('#<%:hdnRegistrationID.ClientID %>').val() != '' || $('#<%:hdnRegistrationID.ClientID %>').val() != '0') {
                    var messageType = "04";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = registrationID;
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else if (code == 'sendIHSCondition') {
                if ($('#<%:hdnRegistrationID.ClientID %>').val() != '' || $('#<%:hdnRegistrationID.ClientID %>').val() != '0') {
                    var messageType = "05";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = registrationID;
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else if (code == 'uploadDocument') {
                var mrn = $('#<%:hdnMRN.ClientID %>').val();
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var regDate = $('#<%:txtRegistrationDate.ClientID %>').val();
                var param = mrn + "|" + visitID + "|" + regDate;
                return param;
            }
            else if (code == 'infoPatientDocument') {
                var mrn = $('#<%:hdnMRN.ClientID %>').val();
                var medicalNo = $('#<%:txtMRN.ClientID %>').val();
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var param = mrn + "|" + medicalNo + "|" + visitID;
                return param;
            }
            else if (code == 'registrationEdit') {
                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                var regDate = $('#<%:txtRegistrationDate.ClientID %>').val();
                var regTime = $('#<%:txtRegistrationHour.ClientID %>').val();
                var param = regID + '|' + regDate + '|' + regTime;
                return param;
            }
            else if (code == 'sendNotificationToJKN') {
                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                var apmID = $('#<%:hdnAppointmentID.ClientID %>').val();
                var param = "02" + '|' + apmID + '|' + regID;
                return param;
            }
            else if (code == 'payerDetail') {
                isChangePayer = true;
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'bpjsPatientHistory') {
                return $('#<%:txtNHSRegistrationNo.ClientID %>').val();
            }
            else if (code == 'sendVitalSignToVS4') {

                return "ADT^A01|" + $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'draftChargesOrder') {
                var isAppointmentHasDraft = $('#<%:hdnIsAppointmentHaveDraft.ClientID %>').val();
                var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
                var appointmentID = $('#<%:hdnAppointmentID.ClientID %>').val();

                if (appointmentID == '0') {
                    appointmentID = "";
                }

                return appointmentID;
            }
            else if (code == 'generalConsentForm') {
                var MRN = $('#<%:hdnMRN.ClientID %>').val();
                var medicalNo = $('#<%:txtMRN.ClientID %>').val();
                var patientName = $('#<%:txtPatientName.ClientID %>').val();
                var dateOfBirth = $('#<%:txtDOB.ClientID %>').val();
                var registrationNo = $('#<%:txtRegistrationNo.ClientID %>').val();
                var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();

                return registrationID + "|" + registrationNo + "|" + MRN + "|" + medicalNo + "|" + patientName + "|" + dateOfBirth;
            }
            else if (code == 'registrationClaimHistory') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onAfterSaveRightPanelContent(code, value, isAdd) {
            if (code == 'patientIdentity') {
                isOpenPatientIdentityPopupFromAppointment = false;
                $('#<%:txtMRN.ClientID %>').val(value);

                onTxtMRNChanged(value);
            } 
            else if (code == 'resultDeliveryPlanEdit') {
                var filterExpression = "RegistrationID = " + value;
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    cboResultDeliveryPlan.SetValue(result.GCResultDeliveryPlan);
                    $('#<%:txtResultDeliveryPlanOthers.ClientID %>').val(result.ResultDeliveryPlanOthers);
                });
            }
            else if (code == 'registrationEdit') {
                var filterExpression = "RegistrationID = " + $('#<%:hdnRegistrationID.ClientID %>').val();
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                    $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                    $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                    if (result.GCReferrerGroup != '') {
                        cboReferral.SetValue(result.GCReferrerGroup);
                    }
                    cboReferral.SetEnabled(false);
                    $('#<%:hdnReferrerID.ClientID %>').val(result.ReferrerID);
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ReferrerParamedicID);
                    if (result.ReferrerParamedicID != "" && result.ReferrerParamedicID != "0") {
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ReferrerParamedicCode);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ReferrerParamedicName);
                    }
                    else {
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(result.ReferrerCommCode);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ReferrerName);
                    }

                    $('#<%:txtDiagnoseCode.ClientID %>').val(result.DiagnoseID);
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    $('#<%:txtDiagnoseText.ClientID %>').val(result.DiagnosisText);

                    $('#<%:chkIsParturition.ClientID %>').prop('checked', result.IsParturition);
                    $('#<%:chkIsVisitorRestriction.ClientID %>').prop('checked', result.IsVisitorRestriction);
                    $('#<%:chkIsPregnant.ClientID %>').prop('checked', result.IsPregnant);
                    $('#<%:chkIsNewBorn.ClientID %>').prop('checked', result.IsNewBorn);
                    if (result.IsNewBorn == true) {
                        filterExpression = "IsDeleted = 0 AND MRN = " + result.MRN;
                        Methods.getObject('GetvPatientBirthRecordList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnMotherVisitID.ClientID %>').val(result.MotherVisitID);
                                $('#<%:hdnMotherMRN.ClientID %>').val(result.MotherMRN);
                                $('#<%:hdnMotherName.ClientID %>').val(result.MotherFirstName);
                                $('#<%:txtMotherRegNo.ClientID %>').val(result.MotherRegistrationNo);
                            }
                        });
                    }
                    cboSpecialty.SetValue(result.SpecialtyID);
                });
            }
            else if (code == 'chargeClassEdit') {
                var filterExpression = "RegistrationID = " + $('#<%:hdnRegistrationID.ClientID %>').val();
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    $('#<%:hdnChargeClassID.ClientID %>').val(result.ChargeClassID);
                    $('#<%:hdnChargeClassBPJSCode.ClientID %>').val(result.ChargeClassBPJSCode);
                    $('#<%:hdnChargeClassBPJSType.ClientID %>').val(result.ChargeClassBPJSType);
                    $('#<%:txtChargeClassCode.ClientID %>').val(result.ChargeClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ChargeClassName);
                });
            }
            else if (code == 'guestIdentity') {
                if (value != "") {
                    $('#<%:txtGuestNo.ClientID %>').val(value);
                    onTxtGuestNoChanged(value);
                }
            }
            else if (code == 'generateSEP') {
                var filterExpression = "RegistrationID = " + $('#<%:hdnRegistrationID.ClientID %>').val();
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    $('#<%=txtNoSEP.ClientID %>').val(result.NoSEP);
                    $('#<%=txtDiagnoseCode.ClientID %>').val(result.DiagnoseID);
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=txtReferralNo.ClientID %>').val(result.ReferralNo);
                    cboVisitReason.SetValue(result.GCVisitReason);
                    $('#<%=txtAccidentLocation.ClientID %>').val(result.AccidentLocation);
                    if (cboVisitReason.GetValue() == Constant.VisitReason.ACCIDENT) {
                        $('#<%=trAccidentLocation1.ClientID %>').attr('style', '');
                        $('#<%=trAccidentLocation2.ClientID %>').attr('style', '');
                        $('#<%=trAccidentLocation3.ClientID %>').attr('style', '');
                        $('#<%=trAccidentLocation4.ClientID %>').attr('style', '');
                        $('#<%=trSuplesi.ClientID %>').attr('style', '');
                    }
                    else {
                        $('#<%=trAccidentLocation1.ClientID %>').attr('style', 'display:none');
                        $('#<%=trAccidentLocation2.ClientID %>').attr('style', 'display:none');
                        $('#<%=trAccidentLocation3.ClientID %>').attr('style', 'display:none');
                        $('#<%=trAccidentLocation4.ClientID %>').attr('style', 'display:none');
                        $('#<%=trSuplesi.ClientID %>').attr('style', 'display:none');
                    }
                });
            }
            else if (code == 'getDataRujukan') {
                $('#<%:txtReferralNo.ClientID %>').val(value);
                var asalRujukan = '1';
                var asalRujukanInString = cboReferral.GetValue();
                if (cboReferral.GetValue() == Constant.ReferrerGroup.RUMAH_SAKIT) {
                    asalRujukan = '2';
                }
                if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                    OnGetBPJSReferralInformation(value, asalRujukan);
                } else {
                    OnGetBPJSReferralInformationAPI(value, asalRujukanInString);
                }
            }
            else if (code == 'getDataSurkonBPJS') {
                $('#<%:txtSurkonBPJS.ClientID %>').val(value);
            }
            else if (code == 'getReferralFollowupVisit') {
                $('#<%:txtAppointmentNo.ClientID %>').val(value);
                onTxtAppointmentNoChanged(value);
            }
            else if (code == 'serviceUnitEdit') {
                var filterExpression = "RegistrationID = " + value;
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                });
            }
            else if (code == "generalConsent") {
                onLoadObject(value);
            }
            else if (code == 'changeReferralManual') {
                $('#<%:txtRegistrationNo.ClientID %>').val(value);
                onTxtRegistrationNoChanged($('#<%:txtRegistrationNo.ClientID %>').val());
            }
            else if (code == 'registrationClaimHistory') {
                $('#<%:txtRegistrationNo.ClientID %>').val(value);
                onTxtRegistrationNoChanged($('#<%:txtRegistrationNo.ClientID %>').val());
            }
            else if (code == "searchPraRegistration") {
                if ($('#<%:hdnIsBridgingToInhealth.ClientID %>').val() == "1") {
                    var noPraRegis = value;
                    InhealthService.detailpraregistrasi_API(noPraRegis, function (result) {
                        if (result != null) {
                            var resp = result.split('|');
                            if (resp[0] == "1") {
                                var obj = jQuery.parseJSON(resp[2]);
                                var errorCode = obj.ERRORCODE;
                                if (errorCode == "00") {
                                    $('#<%:txtVisitNotes.ClientID %>').val(obj.CATATANKHUSUS);
                                    $('#<%:txtDiagnoseCode.ClientID %>').val(obj.KDDIAG);
                                    $('#<%:txtDiagnoseName.ClientID %>').val(obj.NMDIAG);
                                    $('#<%:txtDiagnoseText.ClientID %>').val(obj.NMDIAG);
                                    $('#<%:hdnInhealthPraRegistrationNo.ClientID %>').val(obj.NOPRAREGISTRASI);
                                    Methods.getObject('GetSettingParameterDtList', "ParameterCode = 'FN0073'", function (resultInhealth) {
                                        if (resultInhealth != null) {
                                            if (resultInhealth.ParameterValue != '') {
                                                cboRegistrationPayer.SetValue(resultInhealth.ParameterValue);
                                                onCboPayerValueChanged();
                                            }
                                        }
                                    });
                                    Methods.getObject("GetvHealthcareServiceUnitList", "InhealthPoli LIKE '%" + obj.KDPOLI + "%'", function (resultHSU) {
                                        if (resultHSU != null) {
                                            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(resultHSU.HealthcareServiceUnitID);
                                            $('#<%:txtServiceUnitCode.ClientID %>').val(resultHSU.ServiceUnitCode);
                                            $('#<%:txtServiceUnitName.ClientID %>').val(resultHSU.ServiceUnitName);
                                        }
                                    });
                                    Methods.getObject("GetPatientList", "InhealthParticipantNo = '" + obj.NOKAPST + "' AND IsDeleted = 0", function (resultPatient) {
                                        if (resultPatient == null) {
                                            showToast('Warning', '<%:GetErrorMessageHasMedicalNo() %>');
                                            var id = 'inhealth|' + obj.NOKAPST + "|" + obj.NOHP + "|" + obj.EMAIL;
                                            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/PatientEntryCtl.ascx");
                                            openUserControlPopup(url, id, 'Patient Identity', 1100, 550, 'patientIdentity');
                                        }
                                        else {
                                            $('#<%:txtMRN.ClientID %>').val(resultPatient.MedicalNo);

                                            var mrn = FormatMRN(resultPatient.MedicalNo);
                                            var filterExpression = "MedicalNo = '" + mrn + "' AND IsDeleted = 0";
                                            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                                                SetPatientInformationToControl(result);
                                                $('#<%:txtParticipantNo.ClientID %>').val(result.InhealthParticipantNo);
                                            });
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if (param != "" && $('#<%:hdnRegistrationID.ClientID %>').val() != null && $('#<%:hdnRegistrationID.ClientID %>').val() != "") {
                onLoadObject(param);
            }
        }

        function onAfterSaveParamedicFromCtlSchedule(oServiceUnitCode, oParamedicCode) {
            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%:hdnParamedicID.ClientID %>').val('');
            $('#<%:txtServiceUnitCode.ClientID %>').val(oServiceUnitCode).trigger('change');
            $('#<%:txtPhysicianCode.ClientID %>').val(oParamedicCode).trigger('change');
        }

        $('#<%=txtRegistrationDate.ClientID %>').live('change', function () {
            var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");

            if ($('#<%=txtRegistrationDate.ClientID %>').val() == '') {
                $('#<%=txtRegistrationDate.ClientID %>').val(dateToday);
            }

            var dateSelected = $('#<%=txtRegistrationDate.ClientID %>').val();
            var registrationDateInDatePicker = Methods.getDatePickerDate(dateSelected);

            if (new Date(registrationDateInDatePicker).toString() !== 'Invalid Date') {
                var from = dateSelected.split("-");
                var f = new Date(from[2], from[1] - 1, from[0]);

                var to = dateToday.split("-");
                var t = new Date(to[2], to[1] - 1, to[0]);

                if (f > t) {
                    showToast('Warning', 'Tidak bisa pilih tanggal melewati hari ini!');
                    $('#<%=txtRegistrationDate.ClientID %>').val(dateToday);
                }
            }
            else {
                $('#<%=txtRegistrationDate.ClientID %>').val(dateToday);
            }

            if (isCheckedFilter) {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:hdnParamedicID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('').trigger('change');
                $('#<%:txtPhysicianCode.ClientID %>').val('').trigger('change');
            }
        });

        $('#<%=txtRegistrationHour.ClientID %>').live('change', function () {
            var hourMinutes = $('#<%=txtRegistrationHour.ClientID %>').val();
            var isCheckedFilter = $('#<%=chkParamedicHasSchedule.ClientID %>').is(":checked");

            var date = new Date();
            var hourInString = date.getHours().toString();
            var minutesInString = date.getMinutes().toString();
            if (hourInString.length == 1) {
                hourInString = '0' + hourInString;
            }
            if (minutesInString.length == 1) {
                minutesInString = '0' + minutesInString;
            }
            var formattedTime = hourInString + ":" + minutesInString;

            if (hourMinutes == '') {
                $('#<%=txtRegistrationHour.ClientID %>').val(formattedTime);
            }

            var sHours = hourMinutes.split(':')[0];
            var sMinutes = hourMinutes.split(':')[1];

            if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                $('#<%=txtRegistrationHour.ClientID %>').val(formattedTime);
            }

            if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                $('#<%=txtRegistrationHour.ClientID %>').val(formattedTime);
            }

            if (isCheckedFilter) {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:hdnParamedicID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('').trigger('change');
                $('#<%:txtPhysicianCode.ClientID %>').val('').trigger('change');
            }
        });

        function onAfterSaveEditRecordEntryPopup(param) {
            if (param != "" && $('#<%:hdnRegistrationID.ClientID %>').val() != null && $('#<%:hdnRegistrationID.ClientID %>').val() != "") {
                onLoadObject(param);
            }
        }

        function onAfterSaveAddRecord(param) {
            var isAppointmentHasDraft = $('#<%:hdnIsAppointmentHaveDraft.ClientID %>').val();
            var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
            var appointmentID = $('#<%:hdnAppointmentID.ClientID %>').val();

            if (deptID == Constant.Facility.PHARMACY || deptID == Constant.Facility.LABORATORY || deptID == Constant.Facility.IMAGING || deptID == Constant.Facility.DIAGNOSTIC || deptID == Constant.Facility.OUTPATIENT) {
                if (isAppointmentHasDraft == '1') {
                    userControlType = 'processdraft';
                    var id = appointmentID;
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/DraftAppointmentProcessCtl.ascx");
                    openUserControlPopup(url, id, 'Draft Transaction and Order', 1250, 550);
                }
                else {
                    $('#<%:txtRegistrationNo.ClientID %>').val(param);
                    setIsAdd(false);
                }
            }
            else if (deptID == Constant.Facility.INPATIENT) {
                var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
                if (isBridgingAplicares == "1") {
                    UpdateRoomAplicares();
                }
            }
        }

        //#region APLICARES

        function UpdateRoomAplicares() {
            var paramRoomID = $('#<%=hdnRoomID.ClientID %>').val();
            var paramClassID = $('#<%=hdnClassID.ClientID %>').val();
            var filterSUR = "RoomID = " + paramRoomID + " AND ClassID = " + paramClassID + " AND IsDeleted = 0 AND IsAplicares = 1";
            Methods.getListObject('GetServiceUnitRoomList', filterSUR, function (resultSUR) {
                for (i = 0; i < resultSUR.length; i++) {
                    var hsuID = resultSUR[i].HealthcareServiceUnitID;
                    var classID = resultSUR[i].ClassID;
                    var filterExpression = "HealthcareServiceUnitID = " + hsuID + " AND ClassID = " + classID;
                    Methods.getObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
                        if (result != null) {
                            var kodeKelas = result.AplicaresClassCode;
                            var kodeRuang = result.ServiceUnitCode;
                            var namaRuang = result.ServiceUnitName;
                            var jumlahKapasitas = result.CountBedAll;
                            var jumlahKosong = result.CountBedEmpty;
                            var jumlahKosongPria = result.CountBedEmptyMale;
                            var jumlahKosongWanita = result.CountBedEmptyFemale;
                            var jumlahKosongPriaWanita = result.CountBedEmptyMale + result.CountBedEmptyFemale;

                            if (result.CountIsSendToAplicares == 0) {
                                AplicaresService.createRoom(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                    if (resultRoom != null) {
                                        try {
                                            AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                                if (resultUpdate != null) {
                                                    try {
                                                        var resultUpdate = resultUpdate.split('|');
                                                        if (resultUpdate[0] == "1") {
                                                            showToast('INFORMATION', "SUCCESS");
                                                        }
                                                        else {
                                                            showToast('FAILED', resultUpdate[2]);
                                                        }
                                                    } catch (error) {
                                                        showToast('FAILED', error);
                                                    }
                                                }
                                            });
                                        } catch (err) {
                                            showToast('FAILED', err);
                                        }
                                    }
                                });
                            } else {
                                AplicaresService.updateRoomStatus(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                    if (resultRoom != null) {
                                        try {
                                            AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                                if (resultUpdate != null) {
                                                    try {
                                                        var resultUpdate = resultUpdate.split('|');
                                                        if (resultUpdate[0] == "1") {
                                                            showToast('INFORMATION', "SUCCESS");
                                                        }
                                                        else {
                                                            showToast('FAILED', resultUpdate[2]);
                                                        }
                                                    } catch (error) {
                                                        showToast('FAILED', error);
                                                    }
                                                }
                                            });
                                        } catch (err) {
                                            showToast('FAILED', err);
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }

        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            if (registrationID == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
            else {
                if (code == 'PM-00101' || code == 'PM-00102' || code == 'PM-00103' || code == 'PM-00104' || code == 'PM-00105' || code == 'PM-00524'
                    || code == 'PM-00107' || code == 'PM-00108' || code == 'PM-00110' || code == 'ER000002' || code == 'PM-00111'
                    || code == "PM-00112" || code == "PM-00113" || code == "PM-00114" || code == "PM-00115" || code == "PM-00116"
                    || code == "PM-00117" || code == 'MC-00001' || code == 'PM-00126' || code == 'PM-00129' || code == 'PM-00132'
                    || code == 'PM-00133' || code == 'PM-00130' || code == 'PM-00134' || code == 'PM-00135' || code == 'PM-00136'
                    || code == 'PM-00144' || code == 'PM-00148' || code == 'PM-00508' || code == 'MR000012' || code == 'PM-00513'
                    || code == 'PM-00613' || code == 'PM-00155' || code == 'PM-00157' || code == 'PM-00162' || code == 'PM-00165'
                    || code == 'PM-00167' || code == 'PM-00169' || code == 'PM-00170' || code == 'PM-00175' || code == "PM-00178"
                    || code == 'PM-00179' || code == 'PM-00181' || code == 'PM-00185' || code == 'PM-00186' || code == 'PM-00188'
                    || code == 'PM-00192' || code == 'PM-00194' || code == 'MR000031' || code == 'PM-00357' || code == 'PM-00359'
                    || code == 'PM-00360' || code == 'PM-00362' || code == 'PM-00392' || code == 'PM-00394' || code == 'PM-00395'
                    || code == 'PM-00579' || code == 'MR-00016' || code == 'PM-00117' || code == 'PM-90025' || code == 'PM-90026'
                    || code == 'PM-90030' || code == 'PM-00659' || code == 'PM-00481' || code == 'IP-00115' || code == 'PM-00665'
                    || code == 'PM-00663' || code == 'PM-00665' || code == 'PM-00200' || code == 'PM-00128' || code == 'PM-90060'
                    || code == 'PM-90061' || code == 'PM-00662' || code == 'PM-90042' || code == 'PM-90071' || code == 'PM-00727'
                    || code == 'PM-00728' || code == 'PM-90083' || code == 'PM-90056') {
                    filterExpression.text = registrationID;
                    if (code == 'PM-00111' || code == "PM-00112" || code == "PM-00178" || code == 'PM-00524' || code == 'PM-90026' || code == 'PM-00128'
                        || code == 'PM-90042' || code == 'PM-90071') {
                        if (cboRegistrationPayer.GetValue() != Constant.CustomerType.BPJS) {
                            errMessage.text = 'Hanya Pasien BPJS yang dapat cetak surat ini';
                            return false;
                        }
                        else if (code == 'PM-00111' || code == 'PM-90026' || code == 'PM-90042') {
                            cbpBPJSProcess.PerformCallback('update');
                            return true;
                        }
                    }
                    return true;
                }
                else if (code == 'PM-00137' || code == 'PM-00428' || code == 'PM-90039') {
                    var MRN = $('#<%:hdnMRN.ClientID %>').val();
                    filterExpression.text = MRN;
                    return true;
                }
                else if (code == 'PM-00617') {
                    var MRN = $('#<%:hdnMRN.ClientID %>').val();
                    filterExpression.text = "MRN = " + MRN;
                    return true;
                }
                else if (code == 'PM-00843') {
                    filterExpression.text = visitID;
                    return true;
                }
                else if (code == 'PM-00131' || code == 'PM-00429' || code == 'PM-00430' || code == 'PM-00514' || code == 'PM-00583' || code == 'MR000016' || code == 'PM-00178' || code == 'MR000065') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'PM-00511' || code == 'PM-00434' || code == 'PM-00623' || code == 'PM-00566') {
                    filterExpression.text = "RegistrationID = " + registrationID;
                    return true;
                }
                else if (code == 'PM-00143' || code == 'PM-00426' || code == 'PM-00427' || code == 'PM-00504' || code == 'PM-00507' || code == 'PM-00512' || code == 'PM-00151' || code == 'PM-00420') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else {
                    var MRN = $('#<%:hdnMRN.ClientID %>').val();
                    filterExpression.text = MRN;
                    return true;
                }
            }
        }

        function LoadGuest(GCSalutation, GCTitle, FirstName, MiddleName, LastName, GCSuffix, GCGender, DateOfBirth,
            StreetName, County, District, City, StreetNameDomicile, CountyDomicile, DistrictDomicile, CityDomicile, PhoneNo, MobilePhoneNo, EmailAddress, GCIdentityNoType, SSN,
            Suffix, Title, AgeInYear, AgeInMonth, AgeInDay, Gender) {
            $('#<%:hdnGuestGCSalutation.ClientID %>').val(GCSalutation);
            $('#<%:hdnGuestGCTitle.ClientID %>').val(GCTitle);
            $('#<%:hdnGuestFirstName.ClientID %>').val(FirstName);
            $('#<%:hdnGuestMiddleName.ClientID %>').val(MiddleName);
            $('#<%:hdnGuestLastName.ClientID %>').val(LastName);
            $('#<%:hdnGuestGCSuffix.ClientID %>').val(GCSuffix);
            $('#<%:hdnGuestGCGender.ClientID %>').val(GCGender);
            $('#<%:hdnGuestDateOfBirth.ClientID %>').val(DateOfBirth);
            $('#<%:hdnGuestStreetName.ClientID %>').val(StreetName);
            $('#<%:hdnGuestCounty.ClientID %>').val(County);
            $('#<%:hdnGuestDistrict.ClientID %>').val(District);
            $('#<%:hdnGuestCity.ClientID %>').val(City);
            $('#<%:hdnGuestStreetNameDomicile.ClientID %>').val(StreetNameDomicile);
            $('#<%:hdnGuestCountyDomicile.ClientID %>').val(CountyDomicile);
            $('#<%:hdnGuestDistrictDomicile.ClientID %>').val(DistrictDomicile);
            $('#<%:hdnGuestCityDomicile.ClientID %>').val(CityDomicile);
            $('#<%:hdnGuestPhoneNo.ClientID %>').val(PhoneNo);
            $('#<%:hdnGuestMobilePhoneNo.ClientID %>').val(MobilePhoneNo);
            $('#<%:hdnGuestEmailAddress.ClientID %>').val(EmailAddress);
            $('#<%:hdnGuestGCIdentityNoType.ClientID %>').val(GCIdentityNoType);
            $('#<%:hdnGuestSSN.ClientID %>').val(SSN);
            $('#<%:hdnGuestSuffix.ClientID %>').val(Suffix);
            $('#<%:hdnGuestTitle.ClientID %>').val(Title);
            $('#<%:txtAgeInYear.ClientID %>').val(AgeInYear);
            $('#<%:txtAgeInMonth.ClientID %>').val(AgeInMonth);
            $('#<%:txtAgeInDay.ClientID %>').val(AgeInDay);
            $('#<%:txtGender.ClientID %>').val(Gender);
            $('#<%:txtDOB.ClientID %>').val(DateOfBirth);
            $('#<%:txtAddress.ClientID %>').val(StreetName + ' ' + County + ' ' + District + ' ' + City);
            $('#<%:txtPatientName.ClientID %>').val(FirstName + ' ' + MiddleName + ' ' + LastName);
        }

        function InitializeGuest() {
            SetGuestData();
        }

        function onBeforeOpenTransaction() {
            return ($('#<%=hdnVisitID.ClientID %>').val() != "");
        }

        function onCbpBPJSProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else if (param[1] == 'success') {
                    var noSep = $('#<%=txtNoSEP.ClientID %>').val();
                    var noTrans = $('#<%=txtRegistrationNo.ClientID %>').val();
                    var message = "Pembuatan SEP Berhasil, Nomor SEP Pasien : <b>" + noSep + "</b>";
                    showToast('Information', message);
                    var id = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/GenerateSEPCtl.ascx");
                    openUserControlPopup(url, id, 'Pembuatan SEP', 900, 550);
                }
            }
        }

        function onAfterSavePatientPhoto() {
            var MRN = $('#<%:hdnMRN.ClientID %>').val();
            var filterExpression = 'MRN = ' + MRN;
            hideLoadingPanel();
            pcRightPanelContent.Hide();
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" value="0" runat="server" />
    <input type="hidden" id="hdnDepartmentIDFilterAppointment" value="0" runat="server" />
    <input type="hidden" id="hdnIsNewPatient" value="0" runat="server" />
    <input type="hidden" id="hdnIsAllowBackDate" value="0" runat="server" />
    <input type="hidden" id="hdnIsAllowNextDate" value="0" runat="server" />
    <input type="hidden" id="hdnMRN" value="" runat="server" />
    <input type="hidden" id="hdnIsBlacklist" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnIsUsingResultDeliveryPlan" runat="server" />
    <input type="hidden" id="hdnIsRegistrationUsingPromotionScheme" runat="server" />
    <input type="hidden" runat="server" id="hdnGuestID" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCSalutation" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCTitle" value="" />
    <input type="hidden" runat="server" id="hdnGuestFirstName" value="" />
    <input type="hidden" runat="server" id="hdnGuestMiddleName" value="" />
    <input type="hidden" runat="server" id="hdnGuestLastName" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCSuffix" value="" />
    <input type="hidden" runat="server" id="hdnGuestSuffix" value="" />
    <input type="hidden" runat="server" id="hdnGuestTitle" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCGender" value="" />
    <input type="hidden" runat="server" id="hdnGuestDateOfBirth" value="" />
    <input type="hidden" runat="server" id="hdnGuestStreetName" value="" />
    <input type="hidden" runat="server" id="hdnGuestCounty" value="" />
    <input type="hidden" runat="server" id="hdnGuestDistrict" value="" />
    <input type="hidden" runat="server" id="hdnGuestCity" value="" />
    <input type="hidden" runat="server" id="hdnGuestStreetNameDomicile" value="" />
    <input type="hidden" runat="server" id="hdnGuestCountyDomicile" value="" />
    <input type="hidden" runat="server" id="hdnGuestDistrictDomicile" value="" />
    <input type="hidden" runat="server" id="hdnGuestCityDomicile" value="" />
    <input type="hidden" runat="server" id="hdnGuestPhoneNo" value="" />
    <input type="hidden" runat="server" id="hdnGuestMobilePhoneNo" value="" />
    <input type="hidden" runat="server" id="hdnGuestEmailAddress" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCIdentityNoType" value="" />
    <input type="hidden" runat="server" id="hdnGuestSSN" value="" />
    <input type="hidden" runat="server" id="hdnExtensionNo" value="" />
    <input type="hidden" runat="server" id="hdnAdminID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationStatus" value="" />
    <input type="hidden" runat="server" id="hdnRMPatientWalkin" value="" />
    <input type="hidden" runat="server" id="hdnWarningPatientOutstandingRegDiffDay" value="" />
    <input type="hidden" runat="server" id="hdnWarningPatientPersonalOutstandingRegDiffDay" value="" />
    <input type="hidden" runat="server" id="hdnPatientSearchDialogType" value="patient1" />
    <input type="hidden" runat="server" id="hdnGuestSearchDialogType" value="guest1" />
    <input type="hidden" runat="server" id="hdnGCGender" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowVoid" value="" />
    <input type="hidden" runat="server" id="hdnIsLinkedToInpatient" value="" />
    <input type="hidden" runat="server" id="hdnIsBPJSChecked" value="0" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitIDLaboratory" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitIDRadiology" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitICUID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitPICUID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitNICUID" value="" />
    <input type="hidden" runat="server" id="hdnMobilePhoneNo1" value="-" />
    <input type="hidden" runat="server" id="hdnIsCataract" value="0" />
    <input type="hidden" runat="server" id="hdnPhysicianBPJSReferenceInfo" value="" />
    <input type="hidden" runat="server" id="hdnNoSKDP" value="" />
    <input type="hidden" runat="server" id="hdnKodeDPJP" value="" />
    <input type="hidden" runat="server" id="hdnNoRujukan" value="" />
    <input type="hidden" runat="server" id="hdnTglRujukan" value="" />
    <input type="hidden" runat="server" id="hdnChargeClassSEP" value="" />
    <input type="hidden" runat="server" id="hdnPilihDokterOtomatisIsiUnit" value="" />
    <input type="hidden" runat="server" id="hdnRegistrasiAsalDiblokJikaLebih24Jam" value="" />
    <input type="hidden" runat="server" id="hdnRegistrasiSelainRajalMemperhatikanCutiDokter"
        value="" />
    <input type="hidden" runat="server" id="hdnKodeProvider" value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatus" value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInInpatientRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInOutpatientRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInEmergencyRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInMCURegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInLaboratoryRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInImagingRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInDiagnosticRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedPatientOwnerStatusInPharmacyRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedInputAIOPackageInOutpatientRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedInputAIOPackageInLaboratoryRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedInputAIOPackageInImagingRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedInputAIOPackageInDiagnosticRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedInputAIOPackageInPharmacyRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnIsUsedInputAIOPackageInInpatientRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnInhealthCustomerType" value="0" />
    <input type="hidden" runat="server" id="hdnIsBridgingToMedinfrasMobileApps" value="" />
    <input type="hidden" runat="server" id="hdnScheduleValidationBeforeRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnTokenInhealth" value="" />
    <input type="hidden" runat="server" id="hdnKodeProviderInhealth" value="" />
    <input type="hidden" runat="server" id="hdnUsername" value="" />
    <input type="hidden" runat="server" id="hdnTodayDate" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationButtonTransactionDirectMenu"
        value="" />
    <input type="hidden" runat="server" id="hdnIsPatientNewBornMandatoryMotherRegistrationNo"
        value="" />
    <input type="hidden" runat="server" id="hdnIsRegistrationBabyLinkRegistrationMother"
        value="" />
    <input type="hidden" runat="server" id="hdnIsInpatientRegistrationBlockOpenRegistration"
        value="" />
    <input type="hidden" runat="server" id="hdnDefaultBindingPatientOwnerStatus" value="" />
    <input type="hidden" runat="server" id="hdnIsParamedicInRegistrationUseSchedule"
        value="" />
    <input type="hidden" value="0" id="hdnDateToday" runat="server" />
    <input type="hidden" value="0" id="hdnIsBridgingBPJSVClaimVersion" runat="server" />
    <input type="hidden" value="0" id="hdnIsBridgingToInhealth" runat="server" />
    <input type="hidden" value="0" id="hdnInhealthPraRegistrationNo" runat="server" />
    <input type="hidden" value="0" id="hdnDefaultLastParamedicID" runat="server" />
    <input type="hidden" value="0" id="hdnIsRadioteraphy" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutomaticInputChargeClass" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 0px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 175px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Pendaftaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                            <%--<asp:RegularExpressionValidator ID="regexStartTime" ControlToValidate="txtRegistrationDate"
     ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$" ErrorMessage="" runat="server" />--%>
                        </td>
                        <td style="padding-left: 30px;">
                            <%:GetLabel("Jam")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationHour" CssClass="time" runat="server" Width="120px"
                                Style="text-align: center" MaxLength="5" />
                        </td>
                    </tr>
                    <tr id="trAppointment" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblAppointmentNo">
                                <%:GetLabel("No. Perjanjian")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnAppointmentID" value="" runat="server" />
                            <input type="hidden" id="hdnIsAppointmentHaveDraft" value="" runat="server" />
                            <asp:TextBox ID="txtAppointmentNo" Width="175px" runat="server" />
                        </td>
                        <td style="padding-left: 30px;">
                            <%:GetLabel("No. Antrian")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQueueNo" ReadOnly="true" Width="120px" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr id="trReservation" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblReservationID">
                                <%:GetLabel("No. Reservasi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnReservationID" value="" runat="server" />
                            <input type="hidden" id="hdnIsReservationHaveDraft" value="" runat="server" />
                            <asp:TextBox ID="txtReservationNo" Width="175px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink lblKey" id="lblNoReg">
                                    <%:GetLabel("No. Registrasi")%></label></div>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationNo" Width="175px" runat="server" />
                                    </td>
                                    <td id="tdIsAutoAppointment" runat="server" style="display: none">
                                        <img src='<%= ResolveUrl("~/Libs/Images/Icon/appointment.png")%>' alt="" title="<%=GetLabel("Dari Appointment")%>"
                                            width="28" height="28" style="padding-left: 5px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding-left: 30px;">
                            <%:GetLabel("No. Ticket")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoTicket" runat="server" Width="120px" Style="text-align: center"
                                MaxLength="5" />
                        </td>
                    </tr>
                    <tr id="trEstimatedService" runat="server" style="display: none">
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="padding-left: 30px;">
                            <%:GetLabel("Estimasi")%>
                            <%:GetLabel("Pelayanan")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstimatedService" Width="120px" runat="server" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%:GetLabel("Data Pasien")%></h4>
                        </td>
                    </tr>
                    <tr id="trIsHasMRN" runat="server">
                        <td>
                            <asp:CheckBox ID="chkIsHasMRN" Checked="true" runat="server" /><%:GetLabel("Gunakan Status RM")%>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <input type="button" id="btnDataPasien" title='<%:GetLabel("Data Pasien") %>' value="Data Pasien"
                                            style="display: none;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trMRN" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblMRN">
                                <%:GetLabel("No. Rekam Medis (RM)")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtMRN" Width="175px" runat="server" />
                                    </td>
                                    <td id="tdIsVIP" runat="server" style="display: none">
                                        &nbsp;<img id="hdnTitleVIP" height="35" src='<%= ResolveUrl("~/Libs/Images/Status/VIP.png")%>'
                                            alt='' />
                                    </td>
                                    <td id="tdIsAlive" runat="server" style="display: none">
                                        &nbsp;<img height="35" src='<%= ResolveUrl("~/Libs/Images/Status/RIP.png")%>' alt='' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trGuestNo" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblGuestNo">
                                <%:GetLabel("No. Pengunjung")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtGuestNo" Width="175px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkCardFee" runat="server" /><%:GetLabel("Cetak Kartu Pasien")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsNeedPastoralCare" runat="server" /><%:GetLabel("Pelayanan Rohani")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsFastTrack" runat="server" /><%:GetLabel("Fast Track")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPartus" runat="server">
                        <td>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsPregnant" runat="server" /><%:GetLabel("Hamil")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsParturition" runat="server" /><%:GetLabel("Partus")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trMotherRegNo" style="display: none" runat="server">
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink lblKey" id="lblMotherRegNo" runat="server">
                                    <%:GetLabel("No. Registrasi Ibu")%></label></div>
                        </td>
                        <td colspan="3">
                            <input type="hidden" id="hdnMotherVisitID" value="" runat="server" />
                            <input type="hidden" id="hdnMotherMRN" value="" runat="server" />
                            <input type="hidden" id="hdnMotherName" value="" runat="server" />
                            <asp:TextBox ID="txtMotherRegNo" Width="175px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trSSN" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="Label2">
                                <%:GetLabel("No. Kartu Identitas")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtIdentityCardNo" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trNHSRegistrationNo" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="lblNHSRegistrationNo">
                                <%:GetLabel("No. Kartu Peserta BPJS")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtNHSRegistrationNo" Width="175px" runat="server" />
                                    </td>
                                    <td id="tdValidateBPJS" runat="server" style="padding-left: 5px">
                                        <input type="button" id="btnSearchPeserta" value="Status Peserta" style="margin-left: 5 0px;
                                            width: 150px;" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPatientInhealthNo" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="lblInhealthParticipantNo">
                                <%:GetLabel("No. Kartu Peserta Inhealth")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtInhealthParticipantNo" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="Label4">
                                <%:GetLabel("No. Karyawan")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCorporateAccountNo" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="Label5">
                                <%:GetLabel("IHS Number")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtIHSNumber" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Panggilan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPreferredName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtGender" Width="175px" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%:GetLabel("No HP")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHandphoneNo" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Lahir")%>
                                /
                                <%:GetLabel("Umur")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 118px" />
                                    <col style="width: 50px" />
                                    <col style="width: 10%" />
                                    <col style="width: 10%" />
                                    <col style="width: 10%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDOB" Width="118px" runat="server" Style="margin-right: 3px; text-align: left" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgeInYear" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Tahun")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgeInMonth" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Bulan")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgeInDay" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Hari")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%:GetLabel("Alamat")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr id="trEmailAddress" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="Label3">
                                <%:GetLabel("Alamat Email")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEmailAddress" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Pekerjaan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPatientJob" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Kantor")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPatientJobOffice" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%:GetLabel("Catatan Pasien")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPatientNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <h4 id="hdBPJSInformation" runat="server">
                                <%:GetLabel("Data Peserta BPJS")%>
                                <label id="lblBPJSStatusINACTIVE" style="color: Red; font-weight: bold; display: none">
                                    <%:GetLabel("TIDAK AKTIF")%></label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table id="tblBPJSInformation" runat="server" width="100%">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" style="table-layout: fixed; width: 212px;">
                                        <label class="lblNormal">
                                            <%:GetLabel("Nama Peserta")%></label>
                                    </td>
                                    <td style="table-layout: fixed; width: 520px;">
                                        <asp:TextBox ID="txtNamaPeserta" Width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jenis Peserta")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJenisPeserta" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kelas Tanggungan") %></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtKelas" ReadOnly="true" runat="server" Width="100px" />
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Faskes / PPK") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNamaFaskes" ReadOnly="true" runat="server" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kelas SEP")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSEPClass" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Status Peserta") %></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStatusPeserta" ReadOnly="true" runat="server" Width="100%" />
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
                                            <%=GetLabel("Dinas Sosial") %></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDinsos" ReadOnly="true" runat="server" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. SKTM") %></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtNoSKTM" ReadOnly="true" runat="server" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Prolanis PRB") %></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPRB" ReadOnly="true" runat="server" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Sub Spesialis") %></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 20px" />
                                                <col style="width: 80px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtSubSpesialisCode" ReadOnly="true" runat="server" Width="100%" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSubSpesialisName" ReadOnly="true" runat="server" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAccidentLocation1" runat="server" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Lokasi Kejadian") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccidentLocation" runat="server" Width="100%" />
                                    </td>
                                </tr>
                                <tr id="trAccidentLocation2" runat="server" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccidentDistrict" runat="server">
                                            <%=GetLabel("Kecamatan")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="80px" />
                                                <col width="3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnKecamatanID" value="" runat="server" />
                                                    <input type="hidden" id="hdnKodeKecamatanBPJS" value="" runat="server" />
                                                    <asp:TextBox ID="txtKodeKecamatan" Width="100%" runat="server" />
                                                </td>
                                                <td />
                                                <td>
                                                    <asp:TextBox ID="txtNamaKecamatan" Width="100%" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAccidentLocation3" runat="server" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccidentCity" runat="server">
                                            <%=GetLabel("Kota/Kabupaten")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="80px" />
                                                <col width="3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnKabupatenID" value="" runat="server" />
                                                    <input type="hidden" id="hdnKodeKabupatenBPJS" value="" runat="server" />
                                                    <asp:TextBox ID="txtKodeKabupaten" Width="100%" runat="server" />
                                                </td>
                                                <td />
                                                <td>
                                                    <asp:TextBox ID="txtNamaKabupaten" Width="100%" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAccidentLocation4" runat="server" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccidentProvince" runat="server">
                                            <%=GetLabel("Propinsi")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="80px" />
                                                <col width="3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnGCState" value="" runat="server" />
                                                    <input type="hidden" id="hdnKodePropinsiBPJS" value="" runat="server" />
                                                    <asp:TextBox ID="txtKodePropinsi" Width="100%" runat="server" />
                                                </td>
                                                <td />
                                                <td>
                                                    <asp:TextBox ID="txtNamaPropinsi" Width="100%" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trSuplesi" runat="server" style="display: none">
                                    <td class="tdLabel" style="vertical-align: top">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. SEP Suplesi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtNoSEPSuplesi" runat="server" Width="175px" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsSuplesi" runat="server" Style="margin-left: 5px" Text="Suplesi" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAccidentPayor" runat="server" style="display: none">
                                    <td class="tdLabel" style="vertical-align: top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Penjamin KLL") %></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkBPJSAccidentPayer1" runat="server" Style="margin-left: 5px"
                                                        Text="Jasa Raharja" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkBPJSAccidentPayer2" runat="server" Style="margin-left: 5px"
                                                        Text="BPJS Ketenagakerjaan" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkBPJSAccidentPayer3" runat="server" Style="margin-left: 5px"
                                                        Text="TASPEN,PT" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkBPJSAccidentPayer4" runat="server" Style="margin-left: 5px"
                                                        Text="ASABRI,PT" />
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
                                            <%:GetLabel("No. SEP")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 175px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtNoSEP" Width="175px" runat="server" />
                                                </td>
                                                <td id="td1" runat="server" style="padding-left: 10px">
                                                    <div style="display: none;">
                                                        <input type="button" id="btnGenerateSEP" value="Pembuatan SEP" style="margin-left: 5px;
                                                            width: 150px; display: none;" runat="server" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Petugas")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrar" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trAdmissionSource" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%:GetLabel("Cara Pendaftaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboAdmissionSource" ClientInstanceName="cboAdmissionSource"
                                Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboAdmissionSourceValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trAdmissionRegistrationNo" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblFromRegistrationNo" runat="server">
                                <%:GetLabel("No Registrasi Asal")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFromRegistrationIsNewPatient" value="0" runat="server" />
                            <input type="hidden" id="hdnFromRegistrationID" value="" runat="server" />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromRegistrationNo" Width="100%" runat="server" />
                                    </td>
                                    <td style="padding-left: 10px; padding-right: 5px; text-align: right;">
                                        <label class="lblNormal" id="Label1" runat="server">
                                            <%:GetLabel("Jenis Kamar")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRoomType" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trServiceRegFromInfo" style="display: none;" runat="server">
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceRegFromInfo" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4 id="h4LastVisitData" runat="server">
                    <%:GetLabel("Data Kunjungan Terakhir")%></h4>
                <table class="grdNormal" width="100%" cellpadding="0" cellspacing="0" id="tblLastVisitData"
                    runat="server">
                    <tr>
                        <th style="width: 70px" align="center">
                            <%:GetLabel("Tanggal") %>
                        </th>
                        <th align="left" style="width: 200px">
                            <%:GetLabel("Unit Pelayanan") %>
                            <br />
                            <%:GetLabel("Dokter Pemeriksa") %>
                        </th>
                        <th align="left" style="width: 150px">
                            <%:GetLabel("Penjamin Bayar") %>
                        </th>
                        <th align="center" style="width: 30px">
                        </th>
                    </tr>
                    <tr style="display: none" id="trLastVisitData" runat="server">
                        <td align="center" id="tdLastRegistrationDate" runat="server">
                        </td>
                        <td align="left" id="tdLastServiceUnitParamedic" runat="server">
                        </td>
                        <td align="left" id="tdLastPayerName" runat="server">
                        </td>
                        <td align="center">
                            <img id="imgCoverage" runat="server" width="30" src='' alt='' />
                        </td>
                    </tr>
                    <tr class="trEmpty" id="trLastVisitDataEmpty" runat="server">
                        <td colspan="4">
                            <%:GetLabel("Belum ada riwayat kunjungan")%>
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Kunjungan")%></h4>
                <table class="tblEntryContent" id="tblVisitData" runat="server" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trBedQuickPicks" runat="server">
                        <td style="width: 30%">
                            <input type="button" id="btnBedQuickPicks" value='<%:("Pilih Tempat Tidur") %>' />
                        </td>
                    </tr>
                    <tr id="trParamedicHasSchedule" runat="server">
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkParamedicHasSchedule" runat="server" /><%:GetLabel(" Tampilkan hanya Dokter yang Punya Jadwal")%>
                            <input type="button" id="btnParamedicSelection" title='<%:GetLabel("Dokter/Klinik") %>'
                                value="Dokter/Klinik" style='display: none' runat="server" />
                        </td>
                    </tr>
                    <tr id="trPhysician" runat="server">
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                <%:GetLabel("Dokter / Tenaga Medis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnParamedicID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
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
                            <input type="hidden" id="hdnClassID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClassName" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td id="tdTemporaryLocation" runat="server">
                                        <asp:CheckBox ID="chkIsTemporaryLocation" runat="server" /><%:GetLabel("Pasien Titipan")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trClassRequest" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblClassRequest">
                                <%:GetLabel("Kelas Permintaan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnClassRequestID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtClassRequestCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClassRequestName" Width="100%" runat="server" />
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
                                        <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
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
                            <input type="hidden" id="hdnRoomID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRoomName" Width="100%" runat="server" />
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
                            <input type="hidden" id="hdnBedID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBedCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsNewBorn" runat="server" /><%:GetLabel("Bayi Baru Lahir")%>
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
                            <input type="hidden" id="hdnChargeClassBPJSCode" value="" runat="server" />
                            <input type="hidden" id="hdnChargeClassBPJSType" value="" runat="server" />
                            <input type="hidden" id="hdnChargeClassID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtChargeClassCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargeClassName" Width="100%" runat="server" />
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
                            <dxe:ASPxComboBox ID="cboSpecialty" ClientInstanceName="cboSpecialty" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){
                                    onCboSpecialtyValueChanged();
                                }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trPatientOwnerStatus" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal lblMandatory">
                                <%=GetLabel("Status Pasien")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPatientOwnerStatus" ClientInstanceName="cboPatientOwnerStatus"
                                Width="100%" runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr id="trItemMCU" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblItemMCU">
                                <%:GetLabel("Paket MCU")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnItemID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trItemAIO" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblItemAIO">
                                <%:GetLabel("Paket AIO")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnItemAIOID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemAIOCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemAIOName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                                <%:GetLabel("Jenis Kunjungan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
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
                                        <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Rujukan")%></label>
                        </td>
                        <td>
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
                        <td>
                            <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                            <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
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
                                        <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal") %>
                            -
                            <%=GetLabel("Jam Rujukan") %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtReferrerDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReferrerTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <fieldset id="fsGenerateSEP" style="margin: 0">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%:GetLabel("Tanggal Rujukan BPJS")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTglRujukan" Width="100px" runat="server" Style="text-align: center"
                                    CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("No. Rujukan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferralNo" Width="230px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("")%></label>
                            </td>
                            <td id="tdReferral" runat="server" style="padding-left: 5px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td>
                                            <input type="button" id="btnReferral" value="Data Rujukan" style="width: 100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trIsKontrolBPJS" runat="server">
                            <td>
                            </td>
                            <td colspan="2">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsKontrolBPJS" runat="server" /><%:GetLabel("Kunjungan Kontrol BPJS")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trSurkonBPJS" runat="server" style="display:none">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblSurkonBPJS" runat="server">
                                    <%:GetLabel("No. Rencana Kontrol BPJS")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurkonBPJS" Width="230px" runat="server" />
                            </td>
                            <td id="td2" runat="server" style="padding-left: 5px" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDiagnose" runat="server">
                                    <%=GetLabel("Diagnosa")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80px" />
                                        <col width="3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnBPJSDiagnoseCode" value="" runat="server" />
                                            <asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" />
                                        </td>
                                        <td />
                                        <td>
                                            <asp:TextBox ID="txtDiagnoseName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Diagnosa Text")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnoseText" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trResultDeliveryPlan" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Rencana Pengambilan Hasil")%></label>
                        </td>
                        <td colspan="3">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboResultDeliveryPlan" ClientInstanceName="cboResultDeliveryPlan"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s){ oncboResultDeliveryPlanValueChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultDeliveryPlanOthers" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trAdmissionCondition" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Keadaan Datang")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trVisitReason" runat="server">
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblNormal">
                                <%:GetLabel("Alasan Kunjungan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblNormal" id="lblOtherVisitNotesLabel" runat="server">
                            </label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVisitNotes" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblNormal">
                                <%:GetLabel("Nomor Visa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientVisaNumber" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Pembayar")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                        <col style="width: 100px" />
                    </colgroup>
                    <%-- RN (20201029) : untuk fitur Linked To Registration dipindahkan di Right Panel Task--%>
                    <tr id="trToRegistrationNo" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblToRegistrationNo" runat="server">
                                <%:GetLabel("Ke Registrasi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnToRegistrationID" value="" runat="server" />
                            <asp:TextBox ID="txtToRegistrationNo" Width="150px" runat="server" />
                        </td>
                    </tr>
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
                <table class="tblEntryContent" runat="server" style="width: 100%;" id="tblPromotion">
                    <tr>
                        <td style="width: 30%" class="tdLabel">
                            <label class="lblLink" runat="server" id="lblPromotion">
                                <%:GetLabel("Skema Promo")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnPromotionID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPromotionCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPromotionName" Width="100%" runat="server" />
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
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 600px;">
                                    <div class="pageTitle" style="text-align: center">
                                        <%=GetLabel("Informasi")%></div>
                                    <div style="background-color: #EAEAEA;">
                                        <table width="600px" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="30px" />
                                            </colgroup>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Status Pendaftaran") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divRegistrationStatus" style="font-size: medium; font-style: oblique">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Oleh") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Pada") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Oleh") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Pada")%>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpBPJSProcess" runat="server" Width="100%" ClientInstanceName="cbpBPJSProcess"
        ShowLoadingPanel="false" OnCallback="cbpBPJSProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpBPJSProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
