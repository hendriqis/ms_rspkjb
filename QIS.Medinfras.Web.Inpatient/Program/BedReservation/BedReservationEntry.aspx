<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="BedReservationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.BedReservationEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Process") %></div>
    </li>
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        //#region onLoad
        function onLoad() {
            setCustomToolbarVisibility();
            
            $('#<%:chkIsPregnant.ClientID %>').attr('disabled', 'disabled');
            $('#<%:chkIsParturition.ClientID %>').attr('disabled', 'disabled');
            $('#<%:chkIsNewBorn.ClientID %>').attr('disabled', 'disabled');
            $('#<%=txtPlanningRegistrationDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtReservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            setDatePicker('<%=txtReservationDate.ClientID %>');
            setDatePicker('<%=txtPlanningRegistrationDate.ClientID %>');
            
            if ($('#<%:chkIsHasMRN.ClientID %>').is(':checked')) {
                $('#btnDataGuest').attr('style', 'display:none');
                $('#<%:trMRN.ClientID %>').removeAttr('style');
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').removeAttr('enabled');
            }
            else {
                $('#btnDataGuest').removeAttr('style');
                $('#<%:trMRN.ClientID %>').attr('style', 'display:none');
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').attr('enabled', 'false');
            }

            var displayOption = $('#<%=rblDataSource.ClientID %>').find(":checked").val();
            if (displayOption == 0) { //tidak dirawat
                $('#<%:trRegistration.ClientID %>').attr('style', 'display:none');
            }
            else { //sedang dirawat
                $('#<%:trRegistration.ClientID %>').removeAttr('style');
            }

            var reservationNoORI = $('#<%:txtReservationNo.ClientID %>').val();
            if (reservationNoORI == "") {
                var reservationNo = $('#<%=hdnReservationNo.ClientID %>').val();
                if (reservationNo != "") {
                    $('#<%:hdnIsFromRegistration.ClientID %>').val(1);
                    $('#<%:txtReservationNo.ClientID %>').val(reservationNo);
                    onReservationNoChanged(reservationNo);
                }
            }

            var registrationNoORI = $('#<%:txtRegistrationNo.ClientID %>').val();
            if (registrationNoORI == "") {
                var RegistrationNo = $('#<%=hdnRegistrationNo.ClientID %>').val();
                if (RegistrationNo != "") {
                    $('#<%:hdnIsFromRegistration.ClientID %>').val(1);
                    $('#<%:txtRegistrationNo.ClientID %>').val(RegistrationNo);
                    onTxtRegistrationNoChanged(RegistrationNo);
                }
            }
            $('#<%:lblMRN.ClientID %>').attr('class', 'lblNormal');
            $('#<%:trIsHasMRN.ClientID %>').attr('style', 'display:none');
            $('#<%:trRegistration.ClientID %>').removeAttr('style');
            $('#<%:txtMRN.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtRegistrationNo.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtPatientName.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtPreferredName.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtGender.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtDOB.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtAgeInYear.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtAgeInMonth.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtAgeInDay.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtAddress.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtClassCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtClassName.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtServiceUnitName.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtRoomCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtRoomName.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtBedCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtChargeClassCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtChargeClassName.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtRemarks.ClientID %>').attr('', '');
            $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
            $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
            $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);
        }
        //#endregion

        //#region setCustomToolbarVisibility
        function setCustomToolbarVisibility() {
            var gc = $('#<%:hdnGCReservationStatus.ClientID %>').val();
            if ($('#<%:hdnGCReservationStatus.ClientID %>').val() == Constant.BedReservation.OPEN) {
                $('#<%=btnVoid.ClientID %>').show();
                $('#<%=btnProcess.ClientID %>').show();
            } else if ($('#<%:hdnGCReservationStatus.ClientID %>').val() == Constant.BedReservation.COMPLETE) {
                $('#<%=btnVoid.ClientID %>').hide();
                $('#<%=btnProcess.ClientID %>').hide();
            } else if ($('#<%:hdnGCReservationStatus.ClientID %>').val() == Constant.BedReservation.CANCELLED) {
                $('#<%=btnVoid.ClientID %>').hide();
                $('#<%=btnProcess.ClientID %>').hide();
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
                $('#<%=btnProcess.ClientID %>').hide();
            }
        }
        //#endregion

        //#region Button
        $('#<%=btnProcess.ClientID %>').live('click', function (evt) {
            var reservationNo = $('#<%:txtReservationNo.ClientID %>').val();
            if (reservationNo != "") {
                var registrationNo = $('#<%:txtRegistrationNo.ClientID %>').val();
                if (registrationNo != "") {
                    showToastConfirmation("Lanjut untuk Pasien Pindah?", function (resultTransfer) {
                        if (resultTransfer) {
                            if ($('#<%:hdnReservationID.ClientID %>').val() != "") {
                                var url = ResolveUrl('~/Program/BedReservation/ReservationTransferEntry.ascx');
                                var reservationID = $('#<%=hdnReservationID.ClientID %>').val();
                                var id = reservationID;
                                openUserControlPopup(url, id, 'Transfer Pasien', 1000, 500);
                            } else {
                                showToast('Warning', 'Reservasi tidak dapat dilakukan');
                            }
                        }
                    });
                } else {
                    showToastConfirmation("Lanjut untuk Registrasi Pasien?", function (resultReservasi) {
                        if (resultReservasi) {
                            if ($('#<%:hdnReservationID.ClientID %>').val() != "") {
                                var url = "";
                                var reservationID = $('#<%:hdnReservationID.ClientID %>').val();
                                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/RegistrationEntry.aspx?id=INPATIENT&resid=" + reservationID);
                                showLoadingPanel();
                                window.location.href = url;
                            } else {
                                showToast('Warning', 'Pasien Belum Mempunyai Reservasi');
                            }
                        }
                    });
                }
            } else {
                showToast('Warning', 'Simpan Reservasi Terlebih Dahulu');
            }
        });

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        $('#btnDataGuest').live('click', function () {
            var id = $('#<%:hdnGuestID.ClientID %>').val();
            var url = ResolveUrl('~/Program/BedReservation/GuestEntryReservationCtl.ascx');
            openUserControlPopup(url, id, 'Identitas Pasien Pengunjung', 980, 600, 'guestIdentity');
        });

        $('#btnDataPasien').live('click', function () {
            var id = $('#<%:hdnMRN.ClientID %>').val();
            var reservationID = $('#<%:hdnReservationID.ClientID %>').val();
            var url = ResolveUrl('~/Program/BedReservation/PatientEntryReservationCtl.ascx');
            if (id != null && id != "") {
                var param = id + "|" + reservationID;
                openUserControlPopup(url, param, 'Patient Identity', 980, 600, 'patientIdentity');
            } else {
                alert("Maaf tidak bisa menambah No RM pada menu ini.");
            }
        });
        //#endregion

        //#region Is Has MRN
        $('#<%:chkIsHasMRN.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').removeAttr('enabled');
                $('#btnDataGuest').attr('style', 'display:none');
                $('#<%:trMRN.ClientID %>').removeAttr('style');
                $('#<%:hdnGuestID.ClientID %>').val('');
                $('#<%:hdnMRN.ClientID %>').val('');
                $('#<%:hdnRegistrationID.ClientID %>').val('');
                $('#<%:txtRegistrationNo.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').val('');
                $('#<%:txtPreferredName.ClientID %>').val('');
                $('#<%:txtGender.ClientID %>').val('');
                $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                $('#<%:txtDOB.ClientID %>').val('');
                $('#<%:txtAgeInYear.ClientID %>').val('');
                $('#<%:txtAgeInMonth.ClientID %>').val('');
                $('#<%:txtAgeInDay.ClientID %>').val('');
                $('#<%:txtAddress.ClientID %>').val('');
                $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);

                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');

                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
            }
            else {
                if ($('#btnPatientIdentity').length > 0) $('#btnPatientIdentity').attr('enabled', 'false');
                $('#btnDataGuest').removeAttr('style');
                $('#<%:trMRN.ClientID %>').attr('style', 'display:none');
                $('#<%:hdnGuestID.ClientID %>').val('');
                $('#<%:hdnMRN.ClientID %>').val('');
                $('#<%:hdnRegistrationID.ClientID %>').val('');
                $('#<%:txtRegistrationNo.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').val('');
                $('#<%:txtPreferredName.ClientID %>').val('');
                $('#<%:txtGender.ClientID %>').val('');
                $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                $('#<%:txtDOB.ClientID %>').val('');
                $('#<%:txtAgeInYear.ClientID %>').val('');
                $('#<%:txtAgeInMonth.ClientID %>').val('');
                $('#<%:txtAgeInDay.ClientID %>').val('');
                $('#<%:txtAddress.ClientID %>').val('');
                $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);

                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');

                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
            }
        });

        //#endregion

        //#region MRN
        $('#<%:lblMRN.ClientID %>.lblLink').live('click', function () {
            var filterExpression = "";
            var filterExpressionPatientNotInCare = "<%:OnGetPatientFilterExpression() %>";
            if ($('#<%:hdnRMPatientWalkin.ClientID %>').val() != '' && $('#<%:hdnRMPatientWalkin.ClientID %>').val() != '0') {
                filterExpression = "MedicalNo != '" + $('#<%:hdnRMPatientWalkin.ClientID %>').val() + "' AND IsDeleted = 0 AND ";
            }
            filterExpression += filterExpressionPatientNotInCare;
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
            var filterExpressionPatientNotInCare = "<%:OnGetPatientFilterExpression() %>";
            if ($('#<%:hdnRMPatientWalkin.ClientID %>').val() != '' && $('#<%:hdnRMPatientWalkin.ClientID %>').val() != '0') {
                filterExpression = "MedicalNo != '" + $('#<%:hdnRMPatientWalkin.ClientID %>').val() + "' AND IsDeleted = 0 AND ";
            }
            var filterExpression = "MedicalNo = '" + mrn + "' AND IsDeleted = 0 AND ";
            filterExpression += filterExpressionPatientNotInCare;
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                SetPatientInformationToControl(result);
            });
        }
        //#endregion

        //#region SetPatientInformationToControl
        function SetPatientInformationToControl(result) {
            if (result != null) {
                $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                $('#<%:hdnMRN.ClientID %>').val(result.MRN);
                $('#<%:hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                var filterExpressionCheckRegistration = "MRN = '" + result.MRN + "' AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "'";
                var regID = $('#<%:hdnRegistrationID.ClientID %>').val();
                $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                $('#<%:txtPreferredName.ClientID %>').val(result.PreferredName);
                $('#<%:txtGender.ClientID %>').val(result.Gender);
                $('#<%:hdnGCGender.ClientID %>').val(result.GCGender);
                $('#<%:txtDOB.ClientID %>').val(result.DateOfBirthInString);

                var age = Methods.getAgeFromDatePickerFormat(result.cfDateOfBirthInString1);
                $('#<%=txtAgeInYear.ClientID %>').val(age.years);
                $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
                $('#<%=txtAgeInDay.ClientID %>').val(age.days);

                $('#<%:txtAddress.ClientID %>').val(result.HomeAddress);
                $('#<%:hdnIsBlacklist.ClientID %>').val(result.IsBlackList);
                if (getIsAdd()) {
                    if (result.GCGender == '<%:GetGenderFemale() %>') {
                        $('#<%:chkIsPregnant.ClientID %>').removeAttr("disabled");
                        $('#<%:chkIsParturition.ClientID %>').removeAttr("disabled");
                    }
                    else {
                        $('#<%:chkIsPregnant.ClientID %>').attr("disabled", true);
                        $('#<%:chkIsParturition.ClientID %>').attr("disabled", true);
                        $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                        $('#<%:chkIsParturition.ClientID %>').prop("checked", false);

                        if ($('#<%:hdnIsBlacklist.ClientID %>').val() == 'true') {
                            showToast('Warning', 'Pasien dengan Nomor Rekam Medis <b>' + $('#<%:txtMRN.ClientID %>').val() + "</b> termasuk kategori Pasien Bermasalah.");
                        }
                    }
                }
            }
            else {
                $('#<%:hdnMRN.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:hdnIsBlacklist.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').val('');
                $('#<%:txtPreferredName.ClientID %>').val('');
                $('#<%:txtGender.ClientID %>').val('');
                $('#<%:txtDOB.ClientID %>').val('');
                $('#<%:txtAgeInYear.ClientID %>').val('');
                $('#<%:txtAgeInMonth.ClientID %>').val('');
                $('#<%:txtAgeInDay.ClientID %>').val('');
                $('#<%:txtAddress.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Bed
        $('#btnBedQuickPicks').live('click', function () {
            var url = ResolveUrl('~/Controls/BedQuickPicksReservationCtl.ascx');
            openUserControlPopup(url, '', 'Pilih Bed', 1150, 600);
        });

        function onAfterClickBedQuickPicks(healthcareServiceUnitID, serviceUnitCode, serviceUnitName, roomID, roomCode, roomName, classID, classCode, className, chargeClassID, chargeClassBPJSCode, chargeClassCode, chargeClassName, bedID, bedCode) {
            $('#<%:hdnChargeClassID.ClientID %>').val(chargeClassID);
            $('#<%:txtChargeClassCode.ClientID %>').val(chargeClassCode);
            $('#<%:txtChargeClassName.ClientID %>').val(chargeClassName);
            $('#<%:hdnClassID.ClientID %>').val(classID);
            $('#<%:txtClassCode.ClientID %>').val(classCode);
            $('#<%:txtClassName.ClientID %>').val(className);
            $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(healthcareServiceUnitID);
            $('#<%:txtServiceUnitCode.ClientID %>').val(serviceUnitCode);
            $('#<%:txtServiceUnitName.ClientID %>').val(serviceUnitName);
            $('#<%:hdnRoomID.ClientID %>').val(roomID);
            $('#<%:txtRoomCode.ClientID %>').val(roomCode);
            $('#<%:txtRoomName.ClientID %>').val(roomName);
            $('#<%:hdnBedID.ClientID %>').val(bedID);
            $('#<%:txtBedCode.ClientID %>').val(bedCode);
        }
        //#endregion

        //#region Reservation No
        $('#lblReservationNo.lblLink').live('click', function () {
            var filterExpression = "";
            openSearchDialog('bedreservation', filterExpression, function (value) {
                $('#<%:txtReservationNo.ClientID %>').val(value);
                onReservationNoChanged(value);
            });
        });

        $('#<%:txtReservationNo.ClientID %>').live('change', function () {
            onReservationNoChanged($(this).val());
        });

        function onReservationNoChanged(value) {
            var filterExpression = "ReservationNo = '" + value + "'";
            Methods.getObject('GetvBedReservationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                    $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                    $('#<%:txtPreferredName.ClientID %>').val(result.PreferredName);
                    $('#<%:txtGender.ClientID %>').val(result.Gender);
                    $('#<%:txtDOB.ClientID %>').val(result.cfDateOfBirthInString);
                    $('#<%:txtAgeInYear.ClientID %>').val(result.AgeInYear);
                    $('#<%:txtAgeInMonth.ClientID %>').val(result.AgeInMonth);
                    $('#<%:txtAgeInDay.ClientID %>').val(result.AgeInDay);
                    $('#<%:txtAddress.ClientID %>').val(result.PatientAddress);
                    $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    $('#<%:txtClassCode.ClientID %>').val(result.ClassCode);
                    $('#<%:txtClassName.ClientID %>').val(result.ClassName);
                    $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    $('#<%:txtRoomCode.ClientID %>').val(result.RoomCode);
                    $('#<%:txtRoomName.ClientID %>').val(result.RoomName);
                    $('#<%:txtBedCode.ClientID %>').val(result.BedCode);
                    $('#<%:txtChargeClassCode.ClientID %>').val(result.ChargeClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ChargeClassName);
                    $('#<%:txtRemarks.ClientID %>').val(result.Remarks);

                    $('#<%:trBedQuickPicks.ClientID %>').attr('style', 'display:none');

                    $('#<%:lblNoReg.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:lblClass.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:lblRoom.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:lblBed.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:lblChargeClass.ClientID %>').attr('class', 'lblDisabled');

                    $('#<%:txtReservationDate.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtReservationHour.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtPlanningRegistrationDate.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtPlanningRegistrationHour.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtClassCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtRoomCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtBedCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtChargeClassCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtRemarks.ClientID %>').attr('readonly', 'readonly');
                    $('#dataSource input:radio').attr({ disabled: 'true' });

                    $('#<%:hdnReservationID.ClientID %>').val(result.ReservationID);
                    $('#<%:hdnGCReservationStatus.ClientID %>').val(result.GCReservationStatus);

                    onLoadObject(value);
                } else {
                    $('#<%:hdnRegistrationID.ClientID %>').val('');
                    $('#<%:txtRegistrationNo.ClientID %>').val('');
                    $('#<%:hdnGuestID.ClientID %>').val('');
                    $('#<%:txtMRN.ClientID %>').val('');
                    $('#<%:txtPatientName.ClientID %>').val('');
                    $('#<%:txtPreferredName.ClientID %>').val('');
                    $('#<%:txtGender.ClientID %>').val('');
                    $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                    $('#<%:txtDOB.ClientID %>').val('');
                    $('#<%:txtAgeInYear.ClientID %>').val('');
                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                    $('#<%:txtAgeInDay.ClientID %>').val('');
                    $('#<%:txtAddress.ClientID %>').val('');
                    $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                    $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);
                }
                setCustomToolbarVisibility();
            });
        }
        //#endregion

        //#region Registration No
        function getRegistrationNoFilterExpression() {
            var filterExpression = "<%:OnGetRegistrationNoFilterExpression() %>";
            return filterExpression;
        }

        $('#<%:lblNoReg.ClientID %>.lblLink').live('click', function () {
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
            Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:txtMRN.ClientID %>').attr("readonly", "readonly");
                    $('#<%:hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                    $('#<%:txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                    $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                    $('#<%:txtPreferredName.ClientID %>').val(result.PreferredName);
                    $('#<%:txtGender.ClientID %>').val(result.Gender);
                    $('#<%:chkIsPregnant.ClientID %>').prop("checked", result.IsPregnant);
                    $('#<%:txtDOB.ClientID %>').val(result.DateOfBirthInString);

                    var dob = result.cfDateOfBirthInStringInDatePicker;
                    var age = Methods.getAgeFromDatePickerFormat(result.cfDateOfBirthInStringInDatePicker);
                    $('#<%=txtAgeInYear.ClientID %>').val(age.years);
                    $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
                    $('#<%=txtAgeInDay.ClientID %>').val(age.days);

                    $('#<%:txtAddress.ClientID %>').val(result.StreetName);
                    $('#<%:chkIsParturition.ClientID %>').prop("checked", result.IsParturition);
                    $('#<%:chkIsNewBorn.ClientID %>').prop("checked", result.IsNewBorn);
                }
                else {
                    $('#<%:hdnRegistrationID.ClientID %>').val('');
                    $('#<%:txtRegistrationNo.ClientID %>').val('');
                    $('#<%:hdnGuestID.ClientID %>').val('');
                    $('#<%:txtMRN.ClientID %>').val('');
                    $('#<%:txtPatientName.ClientID %>').val('');
                    $('#<%:txtPreferredName.ClientID %>').val('');
                    $('#<%:txtGender.ClientID %>').val('');
                    $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                    $('#<%:txtDOB.ClientID %>').val('');
                    $('#<%:txtAgeInYear.ClientID %>').val('');
                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                    $('#<%:txtAgeInDay.ClientID %>').val('');
                    $('#<%:txtAddress.ClientID %>').val('');
                    $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                    $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);
                }
            });
        }
        //#endregion

        //#region Tipe Pasien
        $('#<%=rblDataSource.ClientID %>').live('change', function () {
            var displayOption = $('#<%=rblDataSource.ClientID %>').find(":checked").val();

            if (displayOption == 0) { //tidak dirawat
                $('#btnDataPasien').removeAttr('style');
                $('#<%:lblMRN.ClientID %>').attr('class', 'lblLink');
                $('#<%:trIsHasMRN.ClientID %>').removeAttr('style');
                $('#<%:trRegistration.ClientID %>').attr('style', 'display:none');
                $('#<%:txtMRN.ClientID %>').removeAttr('readonly');
                $('#<%:hdnGuestID.ClientID %>').val('');
                $('#<%:hdnMRN.ClientID %>').val('');
                $('#<%:hdnRegistrationID.ClientID %>').val('');
                $('#<%:txtRegistrationNo.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').val('');
                $('#<%:txtPreferredName.ClientID %>').val('');
                $('#<%:txtGender.ClientID %>').val('');
                $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                $('#<%:txtDOB.ClientID %>').val('');
                $('#<%:txtAgeInYear.ClientID %>').val('');
                $('#<%:txtAgeInMonth.ClientID %>').val('');
                $('#<%:txtAgeInDay.ClientID %>').val('');
                $('#<%:txtAddress.ClientID %>').val('');
                $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);

                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');

                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');

                $('#<%:hdnIsNewPatient.ClientID %>').val(0);
                $('#<%:hdnIsFromRegistration.ClientID %>').val(0);
            }
            else { //sedang dirawat
                $('#btnDataPasien').attr('style', 'display:none');
                $('#<%:lblMRN.ClientID %>').attr('class', 'lblNormal');
                $('#<%:trIsHasMRN.ClientID %>').attr('style', 'display:none');
                $('#<%:trRegistration.ClientID %>').removeAttr('style');
                $('#<%:txtMRN.ClientID %>').attr('readonly', 'readonly');
                $('#<%:hdnGuestID.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtPreferredName.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtGender.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtDOB.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtAgeInYear.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtAgeInMonth.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtAgeInDay.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtAddress.ClientID %>').attr('readonly', 'readonly');
                $('#<%:hdnMRN.ClientID %>').val('');
                $('#<%:hdnRegistrationID.ClientID %>').val('');
                $('#<%:txtRegistrationNo.ClientID %>').val('');
                $('#<%:txtMRN.ClientID %>').val('');
                $('#<%:txtPatientName.ClientID %>').val('');
                $('#<%:txtPreferredName.ClientID %>').val('');
                $('#<%:txtGender.ClientID %>').val('');
                $('#<%:chkIsPregnant.ClientID %>').prop("checked", false);
                $('#<%:txtDOB.ClientID %>').val('');
                $('#<%:txtAgeInYear.ClientID %>').val('');
                $('#<%:txtAgeInMonth.ClientID %>').val('');
                $('#<%:txtAgeInDay.ClientID %>').val('');
                $('#<%:txtAddress.ClientID %>').val('');
                $('#<%:chkIsParturition.ClientID %>').prop("checked", false);
                $('#<%:chkIsNewBorn.ClientID %>').prop("checked", false);

                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');

                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');

                $('#<%:hdnIsNewPatient.ClientID %>').val(0);
                $('#<%:hdnIsFromRegistration.ClientID %>').val(1);
            }
        });
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
                        $('#<%:txtChargeClassCode.ClientID %>').val(result.ClassCode);
                        $('#<%:txtChargeClassName.ClientID %>').val(result.ClassName);
                    }
                    else {
                        $('#<%:hdnChargeClassID.ClientID %>').val('');
                        $('#<%:txtChargeClassCode.ClientID %>').val('');
                        $('#<%:txtChargeClassName.ClientID %>').val('');
                    }
                }
                else {
                    $('#<%:hdnClassID.ClientID %>').val('');
                    $('#<%:txtClassCode.ClientID %>').val('');
                    $('#<%:txtClassName.ClientID %>').val('');
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
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

        //#region Service Unit
        var serviceUnitUserCount = parseInt('<%:serviceUnitUserCount %>');
        function getServiceUnitFilterFilterExpression() {
            var filterExpression = '';
            var classID = $('#<%:hdnClassID.ClientID %>').val();

            if (classID != '')
                filterExpression = 'HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitRoom WHERE ClassID = ' + classID + ')';

            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            if (paramedicID != '') {
                if (filterExpression != '')
                    filterExpression += ' AND ';
                filterExpression += '(HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = ' + paramedicID + '))';
            }

            if (filterExpression != '')
                filterExpression += ' AND ';
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
                    $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Room
        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var classID = $('#<%:hdnClassID.ClientID %>').val();
            var filterExpression = '';
            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }
            if (classID != '0' && classID != '') {
                if (filterExpression != '') filterExpression += " AND ";
                filterExpression += 'ClassID = ' + classID;
            }
            if (filterExpression != '') filterExpression += " AND ";
            filterExpression += "IsDeleted = 0";
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
        }
        //#endregion

        //#region Bed
        $('#<%:lblBed.ClientID %>.lblLink').live('click', function () {
            var roomID = $('#<%:hdnRoomID.ClientID %>').val();
            var filterExpression = '';
            if (roomID != '') {
                filterExpression = "RoomID = " + roomID;
                openSearchDialog('bed', filterExpression, function (value) {
                    $('#<%:txtBedCode.ClientID %>').val(value);
                    onTxtBedCodeChanged(value);
                });
            }
            else {
                showToast('Warning', 'Pilih kamar terlebih dahulu.');
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

        //#region Charge Class
        function onGetChargeClassFilterExpression() {
            return "IsUsedInChargeClass = 1 AND IsDeleted = 0";
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
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ClassName);
                }
                else {
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region LoadGuest
        function LoadGuest(GCSalutation, GCTitle, FirstName, MiddleName, LastName, GCSuffix, GCGender, DateOfBirth,
            StreetName, County, District, City, PhoneNo, MobilePhoneNo, EmailAddress, GCIdentityNoType, SSN,
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
            $('#<%:hdnGuestPhoneNo.ClientID %>').val(PhoneNo);
            $('#<%:hdnGuestMobilePhoneNo.ClientID %>').val(MobilePhoneNo);
            $('#<%:hdnGuestEmailAddress.ClientID %>').val(EmailAddress);
            $('#<%:hdnGuestGCIdentityNoType.ClientID %>').val(GCIdentityNoType);
            $('#<%:hdnGuestSSN.ClientID %>').val(SSN);
            $('#<%:hdnGuestSuffix.ClientID %>').val(Suffix);
            $('#<%:hdnGuestTitle.ClientID %>').val(Title);
            $('#<%:txtDOB.ClientID %>').val(DateOfBirth);

            var dateSelected = $('#<%=txtDOB.ClientID %>').val();
            var age = Methods.getAgeFromDatePickerFormat(dateSelected);
            $('#<%=txtAgeInYear.ClientID %>').val(age.years);
            $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
            $('#<%=txtAgeInDay.ClientID %>').val(age.days);

            $('#<%:txtGender.ClientID %>').val(Gender);
            $('#<%:txtAddress.ClientID %>').val(StreetName + ' ' + County + ' ' + District + ' ' + City);
            $('#<%:txtPatientName.ClientID %>').val(FirstName + ' ' + MiddleName + ' ' + LastName);
            $('#<%:txtGender.ClientID %>').val(Gender);
            $('#<%:hdnIsNewPatient.ClientID %>').val(0);
        }
        //#endregion

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var filterExpression = 'IsDeleted = 0 AND IsHasPhysicianRole = 1';
            if (serviceUnitID != '0')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ") AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
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

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    var filterExpressionLeave = "IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
                    Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                        if (resultLeave == null) {
                            $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                            $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                        }
                        else {
                            var isLeave = $('#<%:hdnRegistrasiSelainRajalMemperhatikanCutiDokter.ClientID %>').val();
                            if (isLeave == '1') {
                                $('#<%:hdnParamedicID.ClientID %>').val('');
                                $('#<%:txtPhysicianCode.ClientID %>').val('');
                                $('#<%:txtPhysicianName.ClientID %>').val('');
                                var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                                showToast("INFORMASI", info);
                            }
                            else {
                                $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                                $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                            }
                        }
                    });
                }
                else {
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type) {
            var typeSplit = type.split(';');
            hideLoadingPanel();
            if (typeSplit[0] == 'void') {
                var no = $('#<%:txtReservationNo.ClientID %>').val();
                onReservationNoChanged(no);
            }
        }

        function InitializeGuest() {
            SetGuestData();
        }

        function onAfterSaveRightPanelContent(code, value, isAdd) {
            if (code == 'patientIdentity') {
                isOpenPatientIdentityPopupFromAppointment = false;
                $('#<%:txtMRN.ClientID %>').val(value);
                $('#<%:hdnIsNewPatient.ClientID %>').val(1);

                onTxtMRNChanged(value);
            }
        }
    </script>
    <input type="hidden" runat="server" id="hdnPageTitle" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowBackDate" value="0" />
    <input type="hidden" runat="server" id="hdnIsAllowVoid" />
    <input type="hidden" runat="server" id="hdnIsAllowRegistration" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="1" />
    <input type="hidden" runat="server" id="hdnIsFromRegistration" value="1" />
    <input type="hidden" runat="server" id="hdnExtensionNo" value="" />
    <input type="hidden" runat="server" id="hdnGCReservationStatus" value="" />
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
    <input type="hidden" runat="server" id="hdnGuestPhoneNo" value="" />
    <input type="hidden" runat="server" id="hdnGuestMobilePhoneNo" value="" />
    <input type="hidden" runat="server" id="hdnGuestEmailAddress" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCIdentityNoType" value="" />
    <input type="hidden" runat="server" id="hdnGuestSSN" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationNo" value="" />
    <input type="hidden" runat="server" id="hdnReservationID" value="" />
    <input type="hidden" runat="server" id="hdnReservationNo" value="" />
    <input type="hidden" runat="server" id="hdnIsNewPatient" value="0" />
    <input type="hidden" runat="server" id="hdnPatientSearchDialogType" value="patient1" />
    <input type="hidden" runat="server" id="hdnIsBlacklist" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnRMPatientWalkin" value="" />
    <input type="hidden" runat="server" id="hdnGCGender" value="" />
    <input type="hidden" runat="server" id="hdnRegistrasiSelainRajalMemperhatikanCutiDokter"
        value="" />
    <input type="hidden" runat="server" id="hdnIsAdd" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink lblKey" id="lblReservationNo">
                                    <%:GetLabel("No. Reservasi")%></label></div>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtReservationNo" Width="60%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal / Jam Reservasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReservationDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtReservationHour" CssClass="time" runat="server" Width="60px"
                                Style="text-align: center" MaxLength="9" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal / Jam Rencana Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlanningRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtPlanningRegistrationHour" CssClass="time" runat="server" Width="60px"
                                Style="text-align: center" MaxLength="9" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblNormal lblMandatory">
                                    <%:GetLabel("Reservasi Dari")%></label></div>
                        </td>
                        <td id="dataSource" colspan="2">
                            <asp:RadioButtonList ID="rblDataSource" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Sedang Dirawat" Value="1" Selected="True" />
                                <asp:ListItem Text="Tidak Dirawat" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trRegistration" runat="server">
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink lblMandatory" id="lblNoReg" runat="server">
                                    <%:GetLabel("No. Registrasi")%></label></div>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRegistrationNo" Width="60%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Pasien")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr id="trIsHasMRN" runat="server">
                        <td>
                            <asp:CheckBox ID="chkIsHasMRN" Checked="true" runat="server" /><%:GetLabel("Gunakan Status RM")%>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <input type="button" id="btnDataGuest" title='<%:GetLabel("Data Tamu") %>' value="Data Tamu"
                                            style="display: none" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trMRN" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblMRN" runat="server">
                                <%:GetLabel("No. Rekam Medis (No.RM)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtMRN" Width="175px" runat="server" />
                                    </td>
                                    <td style="display:none">
                                        <input type="button" id="btnDataPasien" title='<%:GetLabel("Data Pasien") %>' value="Data Pasien"
                                            style="display: none" />
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
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Panggilan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPreferredName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td>
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
                                        <asp:CheckBox ID="chkIsPregnant" runat="server" Style="margin-left: 5px" /><%:GetLabel("Hamil")%>
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
                        <td>
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
                        <td>
                            <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%:GetLabel("Data Tempat Tidur")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr id="trBedQuickPicks" runat="server">
                        <td style="width: 30%">
                            <input type="button" id="btnBedQuickPicks" value='<%:("Pilih Tempat Tidur") %>' />
                        </td>
                    </tr>
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
                <label class="lblLink lblMandatory" id="lblClass" runat="server">
                    <%:GetLabel("Kelas")%></label>
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
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trServiceUnit" runat="server">
            <td class="tdLabel">
                <label class="lblLink" runat="server" id="lblServiceUnit">
                    <%:GetLabel("Ruang Perawatan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnIsServiceUnitHasParamedic" value="" runat="server" />
                <input type="hidden" id="hdnIsServiceUnitHasVisitType" value="" runat="server" />
                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
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
                <label class="lblLink" runat="server" id="lblRoom">
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
                <label class="lblLink" runat="server" id="lblBed">
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
                            <asp:CheckBox ID="chkIsParturition" runat="server" /><%:GetLabel("Partus")%>&nbsp;
                            <asp:CheckBox ID="chkIsNewBorn" runat="server" /><%:GetLabel("Bayi Baru Lahir")%>
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
    </table>
    <h4>
        <%:GetLabel("Data Lain Lain")%></h4>
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 200px" />
            <col />
        </colgroup>
        <tr id="trLainLain" runat="server">
            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                <label class="lblNormal lblMandatory">
                    <%:GetLabel("Catatan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
            </td>
        </tr>
    </table>
    </td> </tr> </table>
</asp:Content>
