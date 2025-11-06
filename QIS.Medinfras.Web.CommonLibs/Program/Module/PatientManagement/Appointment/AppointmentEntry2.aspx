<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="AppointmentEntry2.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentEntry2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnAppointmentSave" runat="server" crudmode="R" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnAppointmentChangeAppointment1" runat="server" crudmode="R" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Change Appointment")%></div>
    </li>
    <li id="btnCreateAppointment" runat="server" crudmode="R" style="display: none;">
        <img src="<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>" alt="" /><div>
            <%=GetLabel("Create Appointment") %></div>
    </li>
    <li id="btnAppointmentVoid1" runat="server" crudmode="R" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnAppointmentVoidByParamedic" runat="server" crudmode="R" style="display:none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void By Paramedic")%></div>
    </li>
    <li id="btnAppointmentProcessRegistration1" runat="server" crudmode="R" style="display:none;">
        <img src="<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>" alt="" /><div>
            <%=GetLabel("Registrasi") %></div>
    </li>
    <li id="btnReschedule" runat="server" crudmode="R" style="display:none;">
        <img src="<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>" alt="" /><div>
            <%=GetLabel("Reschedule All") %></div>
    </li>
    <li id="bntReopen" runat="server" crudmode="R" style="display: none;">
        <img src="<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>" alt="" /><div>
            <%=GetLabel("Re-open") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var lastAppointmentID = -1;
        $(function () {
            setRightPanelButtonEnabled();
            registerCollapseExpandHandler();
            setDatePickerIsAllowBackDate();
            $('#btnPayerNotesDetail').attr('enabled', 'false');
            $('#ctxMenuEdit').click(function () {
                var className = $(this).attr('class');
                if (!(typeof className !== 'undefined' && className !== false))
                    $('#<%=btnAppointmentChangeAppointment.ClientID %>').click();
            });
            $('#ctxMenuVoid:not(.disabled)').click(function () {
                var className = $(this).attr('class');
                if (!(typeof className !== 'undefined' && className !== false))
                    $('#<%=btnAppointmentVoid.ClientID %>').click();
            });

            $('.btnEditAppointment2').live('click', function () {
                //                var visitID = $(this).attr('visitID');
                //                var testOrderID = $(this).attr('orderID');
                //                var isEnabled = $(this).attr('enabled');
                //                var patientInfo = $(this).attr('patientInfo');
                //                var patientLocation = $(this).attr('patientLocation');

                //                if (isEnabled == "True") {
                //                    var url = ResolveUrl("~/Libs/Controls/Process/SurgeryOrderEntryCtl1.ascx");
                //                    var id = visitID + '|' + testOrderID;

                //                    openUserControlPopup(url, id, 'Proses Penjadwalan Order Kamar Operasi', 900, 600);
                //                }
                //                else {
                //                    displayMessageBox("JADWAL KAMAR OPERASI", "Order tidak bisa dilakukan perubahan lagi karena sudah diproses");
                //                }

                displayMessageBox("Edit Appointment", "Menampilkan Pop-up Edit Appointment");
            });
            //#region Room Code

            var attr = $('#<%:txtDOBMainAppt.ClientID %>').attr('readonly');
            setDatePicker('<%:txtDOBMainAppt.ClientID %>');

            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();
            today = dd + '-' + mm + 'yyyy' + yyyy;
            $('#<%:txtDOBMainAppt.ClientID %>').val(today);

            $('#<%:txtDOBMainAppt.ClientID %>').datepicker('option', 'maxDate', 0);


            $('#<%=btnAddAppointment.ClientID %>').live('click', function (evt) {
                if ($('.grdAppointment li.selected').length == 0) {
                    displayErrorMessageBox('Warning', 'Tidak bisa membuat appointment karena tidak ada slot yang dipilih. Harap pilih slot.');
                } else {
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentFormEntry.ascx");
                    var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                    var appointmentMode = "2"; //No-Time Slot Mode
                    //var queueNo = $(this).attr('queueNo');

                    var queueNo = "0";
                    var selectedDate = $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val();
                    var healthcareServiceUnitID = cboServiceUnit.GetValue();
                    var healthcareServiceUnitName = cboServiceUnit.GetText();
                    var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
                    var paramedicName = $('#<%=hdnParamedicName.ClientID %>').val();
                    var txtAppointmentHour = $('#<%=txtAppointmentHour.ClientID %>').val();
                    var session = cboSession.GetValue();
                    var param = departmentID + "|" + appointmentMode + "|" + "1" + "|" + queueNo + "|" + selectedDate + "|" + healthcareServiceUnitID + "|" + healthcareServiceUnitName + "|" + paramedicID + "|" + paramedicName + "|" + txtAppointmentHour + "|" + session;
                    openUserControlPopup(url, param, 'Perjanjian Pasien', 900, 600);
                }


            });


            $('#<%=btnAppointmentSave.ClientID %>').click(function (evt) {
                if ($('.grdAppointment li.selected').length == 0) {
                    displayErrorMessageBox('Warning', 'Tidak bisa membuat appointment karena tidak ada slot yang dipilih. Harap pilih slot.');
                }
                else {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != '' && ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() == Constant.AppointmentStatus.COMPLETE || $('#<%=hdnGCAppointmentStatus.ClientID %>').val() == Constant.AppointmentStatus.DELETED)) {
                        showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                    }
                    else if (IsValid(evt, 'fsAppointment', 'mpAppointment')) {
                        lastAppointmentID = 0;
                        if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                            onCustomButtonClick('save');
                        }
                        else {
                            if ($('#<%=hdnIsWaitingList.ClientID %>').val() == '0') {
                                if ($('#<%=hdnIsValidAppoMessage.ClientID %>').val() == '1') {
                                    onCustomButtonClick('save');
                                }
                                else {
                                    if ($('#<%=hdnIsUsingValidationMaxAppointment.ClientID %>').val() == '0') {
                                        var message = 'Maksimum perjanjian (untuk sesi yang dipilih) adalah ' + '<b>' + $('#<%=hdnMaxAppoMessage.ClientID %>').val() + '</b>' + ' dan jumlah perjanjian (untuk sesi yang dipilih) yang ada saat ini adalah ' + '<b>' + $('#<%=hdnTotalAppoMessage.ClientID %>').val() + '</b>' + '. Apakah ingin melanjutkan proses?'
                                        if ($('#<%=hdnMaxAppoMessage.ClientID %>').val() > 0) {
                                            showToastConfirmation(message, function (result) {
                                                if (result) {
                                                    onCustomButtonClick('save');
                                                }
                                            });
                                        }
                                        else {
                                            onCustomButtonClick('save');
                                        }
                                    }
                                    else {
                                        if ($('#<%=hdnMaxAppoMessage.ClientID %>').val() > 0 || $('#<%=hdnMaxAppoMessage.ClientID %>').val() == 0) {
                                            onCustomButtonClick('save');
                                        }
                                        else {
                                            showToast('WARNING', 'Kuota dokter sudah penuh');
                                        }
                                    }
                                }
                            }
                            else {
                                if ($('#<%=hdnIsValidWaitingMessage.ClientID %>').val() == '1') {
                                    onCustomButtonClick('save');
                                }
                                else {
                                    var message = 'Maksimum Waiting List (untuk sesi yang dipilih) adalah ' + '<b>' + $('#<%=hdnMaxWaitingMessage.ClientID %>').val() + '</b>' + ' dan jumlah Waiting List (untuk sesi yang dipilih) yang ada saat ini adalah ' + '<b>' + $('#<%=hdnTotalWaitingMessage.ClientID %>').val() + '</b>' + '. Apakah ingin melanjutkan proses?';
                                    if ($('#<%=hdnMaxWaitingMessage.ClientID %>').val() > 0) {
                                        showToastConfirmation(message, function (result) {
                                            if (result) {
                                                onCustomButtonClick('save');
                                            }
                                        });
                                    }
                                    else {
                                        onCustomButtonClick('save');
                                    }
                                }
                            }
                        }
                    }
                }
            });

            $('#<%=btnAppointmentProcessRegistration.ClientID %>').live('click', function () {
                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.COMPLETE) {
                        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
                        var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                        var url = "";
                        if (departmentID == 'OP') {
                            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/RegistrationEntry.aspx?id=OUTPATIENT&appid=" + appointmentID);
                        }
                        else if (departmentID == 'IS') {
                            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/RegistrationEntry.aspx?id=IMAGING&appid=" + appointmentID);
                        }
                        else if (departmentID == 'LB') {
                            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/RegistrationEntry.aspx?id=LABORATORY&appid=" + appointmentID);
                        }
                        else if (departmentID == 'MD') {
                            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/RegistrationEntry.aspx?id=DIAGNOSTIC&appid=" + appointmentID);
                        }
                        showLoadingPanel();
                        window.location.href = url;
                    }
                    else {
                        displayErrorMessageBox("Perjanjian Pasien", '<%=GetErrMessageCompletedAppointment() %>');
                    }
                }
                else {
                    displayErrorMessageBox("Perjanjian Pasien", '<%=GetErrMessageSelectAppointmentFirst() %>');
                }
            });

            $('#<%=btnAppointmentVoid.ClientID %>').live('click', function () {
                if ($('.grdAppointment li.selected').length == 0) {
                    displayErrorMessageBox('Warning', 'Silahkan pilih appointment yang akan di void');
                } else {
                    if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                        if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.STARTED) {
                            showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                        }
                        else {
                            showToastConfirmation('Are You Sure Want To Void?', function (result) {
                                if (result) {
                                    var id = $('#<%=hdnAppointmentID.ClientID %>').val();
                                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/v2/AppointmentVoidV2Ctl.ascx");
                                    openUserControlPopup(url, id, 'Void Appointment', 800, 350);
                                }
                            });
                        }
                    }
                    else {
                        showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
                    }
                }

            });

            $('#<%=btnAppointmentVoidByParamedic.ClientID %>').click(function () {
                if ($('#<%=hdnParamedicID.ClientID %>').val() != '') {
                    showToastConfirmation('Apakah yakin akan membatalkan SELURUH PERJANJIAN PASIEN dari Dokter/Paramedis ini?', function (result) {
                        if (result) {
                            onCustomButtonClick('voidAll');
                            cbpAppointment.PerformCallback();
                        }
                    });
                }
                else {
                    showToast('Warning', 'Pilih Dokter/Paramedis terlebih dahulu.');
                }
            });

            function checkTime(i) {
                if (i < 10) {
                    i = "0" + i;
                }
                return i;
            }

            $('#<%=btnAppointmentChangeAppointment.ClientID %>').live('click', function () {

                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    var isAllow = "1";
                    if ($('#<%=hdnIsAllowRescheduleBackDate.ClientID %>').val() == "0") {
                        var time = '00:00';
                        if ($('#<%=hdnIsWaitingList.ClientID %>').val() == '0') {
                            time = $.trim($tr.find('.tdTime').html());
                        }

                        var dateNow = $('#<%=hdnDateTodayTime.ClientID %>').val();
                        var today = new Date('<%=TodayYear() %>', '<%=TodayMonth() %>' - 1, '<%=TodayDay() %>', 0, 0, 0);

                        var chosenDateInString = $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val();
                        var dateDay = chosenDateInString.substring(0, 2);
                        var dateMonth = chosenDateInString.substring(3, 5);
                        var dateYear = chosenDateInString.substring(6);
                        var chosenTimeHour = time.substring(0, 2);
                        var chosenTimeMinute = time.substring(3);

                        var chosenDate = new Date(dateYear, dateMonth - 1, dateDay, 0, 0, 0);

                        if (chosenDate.getTime() < today.getTime()) {
                            isAllow = "0";
                        }
                        else {
                            isAllow = "1";
                        }
                    }

                    if (isAllow == "1") {
                        if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() == Constant.AppointmentStatus.COMPLETE) {
                            displayMessageBox('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                        }
                        if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() == "0278^004" || $('#<%=hdnGCAppointmentStatus.ClientID %>').val() == "0278^001") {
                            displayMessageBox('Warning', 'Appointment sudah dibatalkan');
                        }
                        else {
                            var id;
                            var isVoidAndByNoTimeSlot = $('#<%=hdnIsByNoTimeSlot.ClientID %>').val();
                            var appointmentStatus = $('#<%=hdnGCAppointmentStatus.ClientID %>').val();

                            if (isVoidAndByNoTimeSlot == "1" && appointmentStatus == Constant.AppointmentStatus.DELETED) {
                                id = $('#<%=hdnAppointmentID.ClientID %>').val() + '|1|' + $('#<%=hdnDepartmentID.ClientID %>').val();
                            }
                            else {
                                id = $('#<%=hdnAppointmentID.ClientID %>').val() + '|0|' + $('#<%=hdnDepartmentID.ClientID %>').val();
                            }

                            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/v2/AppointmentChangeDateV2Ctl.ascx");
                            openUserControlPopup(url, id, 'Change Appointment Date', 1200, 650);
                        }
                    }
                    else {
                        displayMessageBox('Warning', 'Tidak bisa mengubah appointment sebelum tanggal hari ini');
                    }
                }
                else {
                    displayMessageBox('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
                }
            });

            $('#<%=bntReopen.ClientID %>').click(function () {
                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() == Constant.AppointmentStatus.COMPLETE) {
                        showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                    }
                    else {
                        onCustomButtonClick('reopen');
                        cbpAppointment.PerformCallback();
                    }
                }
                else {
                    showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
                }
            });

            $('#<%=btnCreateAppointment.ClientID %>').click(function () {
                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.STARTED) {
                        showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                    }
                    else {
                        var id = $('#<%=hdnAppointmentID.ClientID %>').val() + '|0|' + $('#<%=hdnDepartmentID.ClientID %>').val();
                        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentChangeDateCtl1.ascx");
                        openUserControlPopup(url, id, 'Create Appointment Date', 1200, 650);
                    }
                }
                else {
                    showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
                }
            });

            $('#<%=btnReschedule.ClientID %>').click(function () {
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRescheduleCtl.ascx");
                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                openUserControlPopup(url, departmentID, 'Reschedule All Appointment', 1200, 650);
            });

            function setDatePickerIsAllowBackDate() {
                var isAllowBackDate = $('#<%=hdnIsAppintmentAllowBackDate.ClientID %>').val();
                if (isAllowBackDate == '1') {
                    $("#calAppointment").datepicker({
                        defaultDate: "w",
                        changeMonth: true,
                        changeYear: true,
                        dateFormat: "dd-mm-yy",
                        //minDate: "0",
                        onSelect: function (dateText, inst) {
                            $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                            cbpPhysician.PerformCallback('refresh');
                            $('#<%=txtAppointmentDate.ClientID %>').val(dateText);
                        }
                    });
                }
                else {
                    $("#calAppointment").datepicker({
                        defaultDate: "w",
                        changeMonth: true,
                        changeYear: true,
                        dateFormat: "dd-mm-yy",
                        minDate: "0",
                        onSelect: function (dateText, inst) {
                            $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                            cbpPhysician.PerformCallback('refresh');
                            $('#<%=txtAppointmentDate.ClientID %>').val(dateText);
                        }
                    });
                }
            }

            $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
                $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnIsChangeParamedicOrHealthcare.ClientID %>').val('1');
                $('#<%=txtPhysicianCode.ClientID %>').val($(this).find('.tdParamedicCode').html());
                $('#<%=txtPhysician.ClientID %>').val($(this).find('.tdParamedicName').html());

                $('#<%=hdnParamedicCode.ClientID %>').val($(this).find('.tdParamedicCode').html());
                $('#<%=hdnParamedicName.ClientID %>').val($(this).find('.tdParamedicName').html());
                $('#<%=hdnParamedicInitial.ClientID %>').val($(this).find('.tdParamedicInitial').html());
                $('#<%=hdnParamedicType.ClientID %>').val($(this).find('.tdParamedicType').html());
                $('#<%=hdnParamedicSpecialty.ClientID %>').val($(this).find('.tdParamedicSpecialty').html());
                $('#<%=hdnParamedicStatus.ClientID %>').val($(this).find('.tdParamedicStatus').html());
                $('#<%=hdnParamedicImageUrl.ClientID %>').val($(this).find('.tdPhysicianImageUrl').html());


                $('#<%=lblParamedicCode.ClientID %>').html($(this).find('.tdParamedicCode').html());
                $('#<%=lblParamedicName.ClientID %>').html($(this).find('.tdParamedicName').html());

                var serviceUnitID = cboServiceUnit.GetValue();
                var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
                if (paramedicID != '') {
                    $('#changeRoomAppointment').removeAttr('enabled');
                    var filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
                    Methods.getObject("GetvParamedicVisitTypeList", filterExpression, function (result) {
                        if (result != null) {
                            if (result.VisitTypeCode == "VT001") {
                                $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                $('#<%=txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
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
                cbpAppointment.PerformCallback();
            });
            $('#<%=grdPhysician.ClientID %> > tbody > tr:eq(1)').click();

            //#region Grd Appointment

            $('.grdAppointment > tbody > tr:gt(0):not(.trDetail):not(.trEmpty) td.tdAppointment li').live('click', function (evt) {
                //if ($('#<%=grdAppointment.ClientID %> > tbody > tr.selected').html() != $(this).html()) {
                $tr = $(this).closest('tr');
                var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
                if (appointmentID > -2) {
                    $('.grdAppointment li.selected').removeClass('selected');
                    $(this).addClass('selected');
                    var time = '00:00';
                    if ($('#<%=hdnIsWaitingList.ClientID %>').val() == '0') {
                        time = $.trim($tr.find('.tdTime').html());
                    }
                    $('#<%=txtAppointmentHour.ClientID %>').val(time);
                    if (appointmentID > -1) {
                        var GCAppointmentStatus = $(this).find('.hdnGCAppointmentStatus').val();
                        var filterExpression = "AppointmentID = " + appointmentID;
                        Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                            var gcAppointmentStatus = result.GCAppointmentStatus;
                            var appointmentID = result.AppointmentID;
                            var appointmentNo = result.AppointmentNo;
                            var queueNo = result.QueueNo;
                            var queueNocf = result.cfQueueNo;
                            var streetName = result.StreetName;
                            var phoneNo = result.PhoneNo;
                            var mobilePhoneNo = result.MobilePhoneNo;
                            var emailAddress = result.EmailAddress;
                            var masterCorporateAccountNo = result.CorporateAccountNo;
                            var masterCorporateAccountName = result.CorporateAccountName;
                            var salutation = result.Salutation;
                            var firstName = result.FirstName;
                            var middleName = result.MiddleName;
                            var lastName = result.LastName;
                            var medicalNo = result.MedicalNo;
                            var notes = result.Notes;
                            var visitDuration = result.VisitDuration;
                            var visitTypeID = result.VisitTypeID;
                            var visitTypeCode = result.VisitTypeCode;
                            var visitTypeName = result.VisitTypeName;
                            var roomID = result.RoomID;
                            var roomCode = result.RoomCode;
                            var roomName = result.RoomName;
                            var gender = result.Gender;
                            var gcGender = result.GCGender;
                            var dateOfBirthInString = result.cfDateOfBirthInString1;
                            var mrn = result.MRN;
                            var businessPartnerID = result.BusinessPartnerID;
                            var businessPartnerCode = result.BusinessPartnerCode;
                            var businessPartnerName = result.BusinessPartnerName;
                            var gcCustomerType = result.GCCustomerType;
                            var coverageTypeCode = result.CoverageTypeCode;
                            var coverageTypeName = result.CoverageTypeName;
                            var corporateAccountNo = result.CorporateAccountNo;
                            var employeeCode = result.EmployeeCode;
                            var employeeName = result.EmployeeName;
                            var coverageLimitAmountInString = result.cfCoverageLimitAmountInString;
                            var isUsingCob = result.IsUsingCOB;
                            var isCoverageLimitPerDay = result.IsCoverageLimitPerDay;
                            var contractNo = result.ContractNo;
                            var appointmentMethod = result.GCAppointmentMethod;
                            cboAppointmentMethod.SetEnabled(false);
                            var gcReferrerGroup = result.GCReferrerGroup;
                            var referrerParamedicID = result.ReferrerParamedicID;
                            var referrerParamedicCode = result.ReferrerParamedicCode;
                            var referrerParamedicName = result.ReferrerParamedicName;
                            var referrerID = result.ReferrerID;
                            var referrerCode = result.ReferrerCode;
                            var referrerName = result.ReferrerName;
                            var guestID = result.GuestID;
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

                            if (appointmentMethod != '' && appointmentMethod != null) {
                                cboAppointmentMethod.SetValue(appointmentMethod);
                            }
                            else {
                                cboAppointmentMethod.SetValue('');
                            }
                            if (gcAppointmentStatus == Constant.AppointmentStatus.DELETED) {
                                $('#<%=bntReopen.ClientID %>').removeAttr('style');
                            }
                            else {
                                $('#<%=bntReopen.ClientID %>').attr('style', 'display:none');
                            }
                            $('#<%=trChange.ClientID %>').removeAttr('style');
                            $('#draftCharges').removeAttr('enabled');
                            $('#draftOrder').removeAttr('enabled');
                            $('#draftServiceOrderEmergency').removeAttr('enabled');
                            $('#draftServiceOrderOutpatient').removeAttr('enabled');
                            $('#draftPrescriptionOrder').removeAttr('enabled');
                            $('#<%=hdnGCAppointmentStatus.ClientID %>').val(gcAppointmentStatus);
                            $('#<%=hdnAppointmentID.ClientID %>').val(appointmentID);
                            $('#<%=txtAppointmentNo.ClientID %>').val(appointmentNo);
                            $('#<%=txtQueueNo.ClientID %>').val(queueNocf);
                            $('#<%=txtAddress.ClientID %>').val(streetName);
                            $('#<%=txtPhoneNo.ClientID %>').val(phoneNo);
                            $('#<%=txtMobilePhone.ClientID %>').val(mobilePhoneNo);
                            $('#<%=txtEmail.ClientID %>').val(emailAddress);
                            $('#<%=txtDOBMainAppt.ClientID %>').val(dateOfBirthInString);
                            $('#<%=txtCorporateAccountNo.ClientID %>').val(masterCorporateAccountNo);
                            $('#<%=txtCorporateAccountName.ClientID %>').val(masterCorporateAccountName);
                            if (salutation != '' && salutation != null) {
                                cboSalutationAppo.SetValue(salutation);
                            }
                            else {
                                cboSalutationAppo.SetValue('');
                            }
                            $('#<%=txtFirstName.ClientID %>').val(firstName);
                            $('#<%=txtMiddleName.ClientID %>').val(middleName);
                            $('#<%=txtFamilyName.ClientID %>').val(lastName);
                            $('#<%=txtMRN.ClientID %>').val(medicalNo);
                            $('#<%=txtRemarks.ClientID %>').val(notes);
                            $('#<%=txtVisitDuration.ClientID %>').val(visitDuration);
                            $('#<%=hdnVisitTypeID.ClientID %>').val(visitTypeID);

                            $('#<%=txtVisitTypeCode.ClientID %>').val(visitTypeCode);
                            $('#<%=txtVisitTypeName.ClientID %>').val(visitTypeName);
                            $('#<%=hdnRoomID.ClientID %>').val(roomID);
                            $('#<%=txtRoomCode.ClientID %>').val(roomCode);
                            $('#<%=txtRoomName.ClientID %>').val(roomName);
                            cboGenderAppointment.SetValue(gcGender);
                            $('#<%=chkIsNewPatient.ClientID %>').attr("disabled", true);
                            $('#lblMRN').attr('class', 'lblDisabled');
                            if (medicalNo == '') {
                                $('#<%=chkIsNewPatient.ClientID %>').prop('checked', true);
                                $('#<%=hdnMRN.ClientID %>').val('');
                                $('#patientIdentity').attr('enabled', 'false');
                                cboGenderAppointment.SetEnabled(true);
                                Methods.getObject("GetvGuestList", "GuestID = " + guestID, function (resultGuest) {
                                    if (resultGuest != null) {
                                        $('#<%=txtDOBMainAppt.ClientID %>').val(resultGuest.DateOfBirthInStringDatePickerFormat);
                                    };
                                });
                            }
                            else {
                                $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);
                                $('#<%=hdnMRN.ClientID %>').val(mrn);
                                $('#patientIdentity').removeAttr('enabled');
                                cboGenderAppointment.SetEnabled(false);
                            }
                            $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');

                            $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtDOBMainAppt.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtCorporateAccountNo.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtCorporateAccountName.ClientID %>').attr('readonly', 'readonly');
                            $('#lblMRN').attr('class', 'lblDisabled');
                            $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtMRN.ClientID %>').removeClass('error');
                            $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtDOBMainAppt.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtCorporateAccountNo.ClientID %>').attr('readonly', 'readonly');
                            $('#<%=txtCorporateAccountName.ClientID %>').attr('readonly', 'readonly');
                            cboSalutationAppo.SetEnabled(false);

                            cboAppointmentPayer.SetValue(gcCustomerType);
                            cboAppointmentPayer.SetEnabled(false);
                            setTblPayerCompanyVisibility();

                            $('#<%:lblPayerCompany.ClientID %>').attr('class', 'lblDisabled');
                            $('#<%:txtPayerCompanyCode.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtPayerCompanyCode.ClientID %>').val(businessPartnerCode);
                            $('#<%:txtPayerCompanyName.ClientID %>').val(businessPartnerName);
                            $('#<%:lblCoverageType.ClientID %>').attr('class', 'lblDisabled');
                            $('#<%:txtCoverageTypeCode.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtCoverageTypeCode.ClientID %>').val(coverageTypeCode);
                            $('#<%:txtCoverageTypeName.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtCoverageTypeName.ClientID %>').val(coverageTypeName);
                            $('#<%:txtParticipantNo.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtParticipantNo.ClientID %>').val(corporateAccountNo);
                            $('#<%:lblEmployee.ClientID %>').attr('class', 'lblDisabled');
                            $('#<%:txtEmployeeCode.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtEmployeeCode.ClientID %>').val(employeeCode);
                            $('#<%:txtEmployeeCode.ClientID %>').val(employeeName);
                            $('#<%:txtCoverageLimit.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtCoverageLimit.ClientID %>').val(coverageLimitAmountInString);
                            $('#<%=chkIsUsingCOB.ClientID %>').attr("disabled", true);
                            $('#<%=chkIsUsingCOB.ClientID %>').prop('checked', isUsingCob);
                            $('#<%=chkIsCoverageLimitPerDay.ClientID %>').attr("disabled", true);
                            $('#<%=chkIsCoverageLimitPerDay.ClientID %>').prop('checked', isCoverageLimitPerDay);
                            $('#<%:lblContract.ClientID %>').attr('class', 'lblDisabled');
                            $('#<%:txtContractNo.ClientID %>').attr('readonly', 'readonly');
                            $('#<%:txtContractNo.ClientID %>').val(contractNo);

                            cboReferral.SetValue(gcReferrerGroup);
                            $('#<%:hdnReferrerID.ClientID %>').val(referrerID);
                            $('#<%:hdnReferrerParamedicID.ClientID %>').val(referrerParamedicID);
                            if (referrerID != 0 && referrerID != null && referrerID != "") {
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(referrerCode);
                                $('#<%:txtReferralDescriptionName.ClientID %>').val(referrerName);
                            } else {
                                $('#<%:txtReferralDescriptionCode.ClientID %>').val(referrerParamedicCode);
                                $('#<%:txtReferralDescriptionName.ClientID %>').val(referrerParamedicName);
                            }

                            if (gcAppointmentStatus == Constant.AppointmentStatus.COMPLETE) {
                                $('#<%=txtVisitTypeCode.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtRemarks.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtPhoneNo.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtMobilePhone.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtEmail.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtDOBMainAppt.ClientID %>').attr('readonly', 'readonly');
                            }
                            else {
                                $('#<%=txtVisitTypeCode.ClientID %>').removeAttr('readonly');
                                $('#<%=txtRemarks.ClientID %>').removeAttr('readonly');
                                $('#<%=txtPhoneNo.ClientID %>').removeAttr('readonly');
                                $('#<%=txtMobilePhone.ClientID %>').removeAttr('readonly');
                                $('#<%=txtEmail.ClientID %>').removeAttr('readonly');
                                $('#<%=txtDOBMainAppt.ClientID %>').removeAttr('readonly');
                            }
                        });
                    } else { //else if (lastAppointmentID > -1) {
                        $('#<%=bntReopen.ClientID %>').attr('style', 'display:none');

                        $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);
                        $('#<%=chkIsNewPatient.ClientID %>').removeAttr('disabled');

                        $('#<%:tblPayerCompany.ClientID %>').hide();
                        $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                        $('#<%:txtPayerCompanyName.ClientID %>').val('');
                        $('#<%:trEmployee.ClientID %>').attr('style', 'display:none');
                        $('#<%:lblPayerCompany.ClientID %>').attr('class', 'lblLink');
                        cboAppointmentPayer.SetEnabled(true);
                        $('#<%:txtPayerCompanyCode.ClientID %>').removeAttr('readonly');
                        cboAppointmentPayer.SetValue(Constant.CustomerType.PERSONAL);
                        $('#<%:lblCoverageType.ClientID %>').attr('class', 'lblLink');
                        $('#<%:txtCoverageTypeCode.ClientID %>').removeAttr('readonly');
                        $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%:txtCoverageTypeName.ClientID %>').removeAttr('readonly');
                        $('#<%:txtCoverageTypeName.ClientID %>').val('');
                        $('#<%:txtParticipantNo.ClientID %>').removeAttr('readonly');
                        $('#<%:txtParticipantNo.ClientID %>').val('');
                        $('#<%:lblEmployee.ClientID %>').attr('class', 'lblLink');
                        $('#<%:txtEmployeeCode.ClientID %>').removeAttr('readonly');
                        $('#<%:txtEmployeeCode.ClientID %>').val('');
                        $('#<%:txtEmployeeCode.ClientID %>').val('');
                        $('#<%:txtCoverageLimit.ClientID %>').removeAttr('readonly');
                        $('#<%:txtCoverageLimit.ClientID %>').val('');
                        $('#<%=chkIsUsingCOB.ClientID %>').removeAttr('disabled');
                        $('#<%=chkIsUsingCOB.ClientID %>').prop('checked', false);
                        $('#<%=chkIsCoverageLimitPerDay.ClientID %>').removeAttr('disabled');
                        $('#<%=chkIsCoverageLimitPerDay.ClientID %>').prop('checked', false);
                        $('#<%:lblContract.ClientID %>').attr('class', 'lblLink');
                        $('#<%:txtContractNo.ClientID %>').removeAttr('readonly');
                        $('#<%:txtContractNo.ClientID %>').val('');
                        $('#draftCharges').attr('enabled', 'false');
                        $('#draftOrder').attr('enabled', 'false');
                        $('#draftServiceOrderEmergency').attr('enabled', 'false');
                        $('#draftServiceOrderOutpatient').attr('enabled', 'false');
                        $('#draftPrescriptionOrder').attr('enabled', 'false');
                        $('#<%=hdnGCAppointmentStatus.ClientID %>').val('');
                        $('#<%=hdnAppointmentID.ClientID %>').val('');
                        $('#<%=txtAppointmentNo.ClientID %>').val('');
                        $('#<%=txtQueueNo.ClientID %>').val('');
                        $('#<%=txtAddress.ClientID %>').val('');
                        $('#<%=txtPhoneNo.ClientID %>').val('');
                        $('#<%=txtMobilePhone.ClientID %>').val('');
                        $('#<%=txtEmail.ClientID %>').val('');
                        $('#<%=txtDOBMainAppt.ClientID %>').val('');
                        $('#<%=txtCorporateAccountNo.ClientID %>').val('');
                        $('#<%=txtCorporateAccountName.ClientID %>').val('');
                        $('#<%=hdnMRN.ClientID %>').val('');

                        $('#<%=txtFirstName.ClientID %>').val('');
                        $('#<%=txtMiddleName.ClientID %>').val('');
                        $('#<%=txtFamilyName.ClientID %>').val('');
                        $('#<%=txtMRN.ClientID %>').val('');
                        $('#<%=txtRemarks.ClientID %>').val('');
                        $('#<%=txtVisitDuration.ClientID %>').val('');
                        $('#<%=hdnVisitTypeID.ClientID %>').val('');
                        $('#<%=txtVisitTypeCode.ClientID %>').val('');
                        $('#<%=txtVisitTypeName.ClientID %>').val('');

                        cboResultDeliveryPlan.SetEnabled(true);
                        cboResultDeliveryPlan.SetValue("");
                        $('#<%=txtResultDeliveryPlanOthers.ClientID %>').val("");
                        $('#<%:txtResultDeliveryPlanOthers.ClientID %>').attr('readonly', 'true');

                        $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtDOBMainAppt.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtCorporateAccountNo.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtCorporateAccountName.ClientID %>').attr('readonly', 'readonly');

                        $('#<%=txtVisitTypeCode.ClientID %>').removeAttr('readonly');
                        $('#<%=txtRemarks.ClientID %>').removeAttr('readonly');
                        $('#<%=txtPhoneNo.ClientID %>').removeAttr('readonly');
                        $('#<%=txtMobilePhone.ClientID %>').removeAttr('readonly');
                        $('#<%=txtEmail.ClientID %>').removeAttr('readonly');

                        $('#<%=txtMRN.ClientID %>').removeAttr('readonly');
                        $('#lblMRN').attr('class', 'lblLink');
                        $('#<%=txtMRN.ClientID %>').removeAttr('readonly');

                        $('#<%=trChange.ClientID %>').attr('style', 'display:none');
                        $('#patientIdentity').attr('enabled', 'false');

                        cboSalutationAppo.SetValue('');
                        cboSalutationAppo.SetEnabled(false);
                        cboGenderAppointment.SetValue('');
                        cboAppointmentMethod.SetEnabled(true);

                        cboReferral.SetValue("");
                        $('#<%:hdnReferrerID.ClientID %>').val("0");
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val("0");
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val("");
                        $('#<%:txtReferralDescriptionName.ClientID %>').val("");

                    }

                    $('#<%=divWarning.ClientID %>').attr('style', 'display:none');
                    lastAppointmentID = appointmentID;
                }
            });

            $('#<%=grdAppointment.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
                $imgExpand = $(this).find('img');
                if ($imgExpand.is(":visible")) {
                    var id = $(this).parent().find('.keyField').html();

                    $hdnIsExpand = $(this).find('.hdnIsExpand');
                    var isVisible = true;
                    if ($hdnIsExpand.val() == '0') {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                        $hdnIsExpand.val('1');
                        isVisible = false;
                    }
                    else {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                        $hdnIsExpand.val('0');
                        isVisible = true;
                    }

                    $('#<%=grdAppointment.ClientID %> input[parentid=' + id + ']').each(function () {
                        if (!isVisible)
                            $(this).closest('tr').show('slow');
                        else
                            $(this).closest('tr').hide('fast');
                    });
                }
            });

            $('.grdAppointment > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live("contextmenu", function (e) {
                if (e.button === 2) {
                    e.preventDefault();
                    $(this).click();
                    var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
                    var GCAppointmentStatus = $(this).find('.hdnGCAppointmentStatus').val();
                    if (appointmentID > -2) {
                        if (appointmentID < 0 || GCAppointmentStatus != Constant.AppointmentStatus.STARTED) {
                            $('#ctxMenuEdit').attr('class', 'disabled');
                            $('#ctxMenuVoid').attr('class', 'disabled');
                        }
                        else {
                            $('#ctxMenuEdit').removeAttr('class');
                            $('#ctxMenuVoid').removeAttr('class');
                        }
                        showContextMenu($("#ctxMenuAppointment"), e);
                    }
                }
            });

            $('#<%=grdAppointmentNoTimeSlot.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
                $imgExpand = $(this).find('img');
                if ($imgExpand.is(":visible")) {
                    var id = $(this).parent().find('.keyField').html();

                    $hdnIsExpand = $(this).find('.hdnIsExpand');
                    var isVisible = true;
                    if ($hdnIsExpand.val() == '0') {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                        $hdnIsExpand.val('1');
                        isVisible = false;
                    }
                    else {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                        $hdnIsExpand.val('0');
                        isVisible = true;
                    }

                    $('#<%=grdAppointmentNoTimeSlot.ClientID %> input[parentid=' + id + ']').each(function () {
                        if (!isVisible)
                            $(this).closest('tr').show('slow');
                        else
                            $(this).closest('tr').hide('fast');
                    });
                }
            });

            $('.grdAppointmentNoTimeSlot > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live("contextmenu", function (e) {
                if (e.button === 2) {
                    e.preventDefault();
                    $(this).click();
                    var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
                    var GCAppointmentStatus = $(this).find('.hdnGCAppointmentStatus').val();
                    if (appointmentID > -2) {
                        if (appointmentID < 0 || GCAppointmentStatus != Constant.AppointmentStatus.STARTED) {
                            $('#ctxMenuEdit').attr('class', 'disabled');
                            $('#ctxMenuVoid').attr('class', 'disabled');
                        }
                        else {
                            $('#ctxMenuEdit').removeAttr('class');
                            $('#ctxMenuVoid').removeAttr('class');
                        }
                        showContextMenu($("#ctxMenuAppointment"), e);
                    }
                }
            });

            $(document).click(function (event) {
                $("#ctxMenuAppointment").hide();
            });

            $(window).blur(function () {
                $("#ctxMenuAppointment").hide();
            });

            //#endregion


        });

        $('#ulTabClinicTransaction li').live('click', function () {
            $('.grdAppointment li.selected').removeClass('selected');
            var name = $(this).attr('contentid');
            if (name == 'containerWaitingList') {
                $('#<%=hdnIsWaitingList.ClientID %>').val('1');
                $('#<%=hdnIsByTimeSlot.ClientID %>').val('0');
                $('#<%=hdnIsByNoTimeSlot.ClientID %>').val('0');
                $('#<%=btnAppointmentChangeAppointment.ClientID %>').attr('style', 'display:none');
                $('#<%=btnAppointmentProcessRegistration.ClientID %>').attr('style', 'display:none');
                $('#<%=btnCreateAppointment.ClientID %>').removeAttr('style');
            }
            else if (name == 'containerAppointmentNoTimeSlot') {
                $('#<%=hdnIsWaitingList.ClientID %>').val('0');
                $('#<%=hdnIsByTimeSlot.ClientID %>').val('0');
                $('#<%=hdnIsByNoTimeSlot.ClientID %>').val('1');
                $('#<%=btnAppointmentSave.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentChangeAppointment.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentProcessRegistration.ClientID %>').removeAttr('style');
                $('#<%=btnCreateAppointment.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%=hdnIsWaitingList.ClientID %>').val('0');
                $('#<%=hdnIsByTimeSlot.ClientID %>').val('1');
                $('#<%=hdnIsByNoTimeSlot.ClientID %>').val('0');
                $('#<%=btnAppointmentSave.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentChangeAppointment.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentProcessRegistration.ClientID %>').removeAttr('style');
                $('#<%=btnCreateAppointment.ClientID %>').attr('style', 'display:none');
            }

            $(this).addClass('selected');
            $('#' + name).removeAttr('style');
            $('#ulTabClinicTransaction li').each(function () {
                var tempNameContainer = $(this).attr('contentid');
                if (tempNameContainer != name) {
                    $(this).removeClass('selected');
                    $('#' + tempNameContainer).attr('style', 'display:none');
                }
                else {
                    $(this).attr('style', 'overflow-y:auto;');
                }
            });
        });

        function registerGrdAppointmentHandler() {
            var timer = null;
            $('.tdAppointmentInformation').hover(function () {
                $appointmentID = parseInt($(this).closest('tr').find('.hdnAppointmentID').val());
                if ($appointmentID > 0) {
                    $td = $(this);
                    timer = setTimeout(function () {
                        $td.find('.divAppointmentInformationDt').show('fast');
                    }, 300);
                }
            }, function () {
                if ($appointmentID > 0) {
                    if (timer) {
                        clearTimeout(timer);
                    }
                    $(this).find('.divAppointmentInformationDt').hide();
                }
            });
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpAppointment.PerformCallback();
        }

        function onCbpAppointmentEndCallback(s) {
            var idx = parseInt(s.cpResult);
            $('#<%=hdnIsChangeParamedicOrHealthcare.ClientID %>').val('');
            $('#<%=bntReopen.ClientID %>').attr('style', 'display:none');

            if (idx == 0)
                idx = 1;
            $('#<%=hdnIsWaitingList.ClientID %>').val('0');
            $('#<%=grdAppointment.ClientID %> > tbody > tr:eq(' + idx + ') td.tdAppointment li').click();

            $('.grdAppointment li.selected').removeClass('selected');
            var name = '';

            if ($('#<%=hdnIsShowTimeSlotContainerEntry.ClientID %>').val() == "1") {
                name = 'containerAppointment';
            }
            else {
                name = 'containerAppointmentNoTimeSlot';
            }

            if ($('#<%=hdnRoomIDDefault.ClientID %>').val() != '') {
                $('#<%=hdnRoomID.ClientID %>').val($('#<%=hdnRoomIDDefault.ClientID %>').val());
                $('#<%=txtRoomCode.ClientID %>').val($('#<%=hdnRoomCodeDefault.ClientID %>').val());
                $('#<%=txtRoomName.ClientID %>').val($('#<%=hdnRoomNameDefault.ClientID %>').val());
                $('#<%=hdnRoomIDDefault.ClientID %>').val('');
            }

            if (name == 'containerWaitingList') {
                $('#<%=hdnIsByTimeSlot.ClientID %>').val('0');
                $('#<%=hdnIsByNoTimeSlot.ClientID %>').val('0');
                $('#<%=hdnIsWaitingList.ClientID %>').val('1');
                $('#<%=btnAppointmentSave.ClientID %>').attr('style', 'display:none');
                $('#<%=btnAppointmentChangeAppointment.ClientID %>').attr('style', 'display:none');
                $('#<%=btnAppointmentProcessRegistration.ClientID %>').attr('style', 'display:none');
                $('#<%=btnCreateAppointment.ClientID %>').removeAttr('style');
            }
            else if (name == 'containerAppointmentNoTimeSlot') {
                $('#<%=hdnIsByTimeSlot.ClientID %>').val('0');
                $('#<%=hdnIsByNoTimeSlot.ClientID %>').val('1');
                $('#<%=hdnIsWaitingList.ClientID %>').val('0');
                $('#<%=btnAppointmentSave.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentChangeAppointment.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentProcessRegistration.ClientID %>').removeAttr('style');
                $('#<%=btnCreateAppointment.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%=hdnIsByTimeSlot.ClientID %>').val('1');
                $('#<%=hdnIsByNoTimeSlot.ClientID %>').val('0');
                $('#<%=hdnIsWaitingList.ClientID %>').val('0');
                $('#<%=btnAppointmentSave.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentChangeAppointment.ClientID %>').removeAttr('style');
                $('#<%=btnAppointmentProcessRegistration.ClientID %>').removeAttr('style');
                $('#<%=btnCreateAppointment.ClientID %>').attr('style', 'display:none');
            }

            $(this).addClass('selected');
            $('#' + name).removeAttr('style');
            $('#ulTabClinicTransaction li').each(function () {
                var tempNameContainer = $(this).attr('contentid');
                if (tempNameContainer != name) {
                    $(this).removeClass('selected');
                    $('#' + tempNameContainer).attr('style', 'display:none');
                }
                else {
                    $(this).addClass('selected');
                }
            });

            LoadParamedicInformation();

            hideLoadingPanel();
            registerGrdAppointmentHandler();
            registerCollapseExpandHandler();

            $('#<%=trChange.ClientID %>').attr('style', 'display:none');
            $('#<%=divWarning.ClientID %>').attr('style', 'display:none');
        }

        function LoadParamedicInformation() {
            $('#<%=imgParamedicProfilePicture.ClientID %>').attr("src", $('#<%=hdnParamedicImageUrl.ClientID %>').val());
            $('#<%=lblParamedicCode.ClientID %>').html($('#<%=hdnParamedicCode.ClientID %>').val());
            $('#<%=lblParamedicName.ClientID %>').html($('#<%=hdnParamedicName.ClientID %>').val());
            $('#<%=lblParamedicInitial.ClientID %>').html($('#<%=hdnParamedicInitial.ClientID %>').val());
            $('#<%=lblParamedicType.ClientID %>').html($('#<%=hdnParamedicType.ClientID %>').val());
            $('#<%=lblParamedicSpecialty.ClientID %>').html($('#<%=hdnParamedicSpecialty.ClientID %>').val());
            $('#<%=lblEmploymentStatus.ClientID %>').html($('#<%=hdnParamedicStatus.ClientID %>').val());
        }

        function onAfterCustomClickSuccess(type) {
            if (type == 'save')
                cbpAppointment.PerformCallback();
        }

        //#region Paging
        var pageCountMaster = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingPhysican"), pageCountMaster, function (page) {
                cbpPhysician.PerformCallback('changepage|' + page);
            });
        });

        function onCbpPhysicianEndCallback(s) {
            hideLoadingPanel();

            $('#<%=hdnGCAppointmentStatus.ClientID %>').val('');
            $('#<%=hdnAppointmentID.ClientID %>').val('');
            $('#<%=txtAppointmentNo.ClientID %>').val('');
            $('#<%=txtQueueNo.ClientID %>').val('');
            $('#<%=txtAddress.ClientID %>').val('');
            $('#<%=txtDOBMainAppt.ClientID %>').val('');
            $('#<%=txtCorporateAccountNo.ClientID %>').val('');
            $('#<%=txtCorporateAccountName.ClientID %>').val('');
            $('#<%=txtPhoneNo.ClientID %>').val('');
            $('#<%=txtMobilePhone.ClientID %>').val('');
            $('#<%=txtEmail.ClientID %>').val('');
            cboSalutationAppo.SetValue('');
            $('#<%=txtFirstName.ClientID %>').val('');
            $('#<%=txtMiddleName.ClientID %>').val('');
            $('#<%=txtFamilyName.ClientID %>').val('');
            $('#<%=txtMRN.ClientID %>').val('');
            $('#<%=txtRemarks.ClientID %>').val('');
            $('#<%=txtVisitDuration.ClientID %>').val('');
            $('#<%=hdnVisitTypeID.ClientID %>').val('');
            $('#<%=txtVisitTypeCode.ClientID %>').val('');
            $('#<%=txtVisitTypeName.ClientID %>').val('');
            $('#<%=trChange.ClientID %>').attr('style', 'display:none');
            $('#<%=divWarning.ClientID %>').attr('style', 'display:none');
            $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCountMaster = parseInt(param[1]);
                if (pageCountMaster > 0)
                    $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();
                setPaging($("#pagingPhysican"), pageCountMaster, function (page) {
                    cbpPhysician.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();

            $('#<%=lblParamedicCode.ClientID %>').html($('#<%=hdnParamedicCode.ClientID %>').val());
            $('#<%=lblParamedicName.ClientID %>').html($('#<%=hdnParamedicName.ClientID %>').val());
        }
        //#endregion

        function onCboServiceUnitValueChanged() {
            $('#<%=txtServiceUnit.ClientID %>').val(cboServiceUnit.GetText());
            $('#changeRoomAppointment').attr('enabled', 'false');
            cbpPhysician.PerformCallback('refresh');
        }

        function oncboSessionValueChanged() {
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (paramedicID != '') {
                cbpAppointment.PerformCallback();
            }
            else {
                showToast('Warning', 'Silahkan Pilih Dokter Terlebih Dahulu');
            }
        }

        function setRightPanelButtonEnabled() {
            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            var selectedDate = $('#<%:hdnCalAppointmentSelectedDate.ClientID %>').val();
            var serviceUnitID = cboServiceUnit.GetValue();
            var mrn = $('#<%=hdnMRN.ClientID %>').val();
            var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();

            if (paramedicID != "" && selectedDate != "" && serviceUnitID != "") {
                $('#btnInfoVoidAppointment').removeAttr('enabled');
                $('#changeRoomAppointment').removeAttr('enabled');
            }
            else {
                $('#btnInfoVoidAppointment').attr('enabled', 'false');
                $('#changeRoomAppointment').attr('enabled', 'false');
            }

            if (mrn != "" && appointmentID != "") {
                $('#patientIdentity').removeAttr('enabled');
            }
            else {
                $('#patientIdentity').attr('enabled', 'false');
            }

            if (appointmentID != "") {
                $('#draftCharges').removeAttr('enabled');
                $('#draftOrder').removeAttr('enabled');
                $('#draftServiceOrderEmergency').removeAttr('enabled');
                $('#draftServiceOrderOutpatient').removeAttr('enabled');
                $('#draftPrescriptionOrder').removeAttr('enabled');
            }
            else {
                $('#draftCharges').attr('enabled', 'false');
                $('#draftOrder').attr('enabled', 'false');
                $('#draftServiceOrderEmergency').attr('enabled', 'false');
                $('#draftServiceOrderOutpatient').attr('enabled', 'false');
                $('#draftPrescriptionOrder').attr('enabled', 'false');
            }
        }

        function onRefreshAfterChangeAppointment() {
            cbpAppointment.PerformCallback();
        }

        function onCboSessionCtlValueChanged() {
            cbpAppointmentChangeAppointment.PerformCallback();
        }

        $('#btnChange').live('click', function () {
            $('#<%=divWarning.ClientID %>').removeAttr('style');
            var isNew = $('#<%=chkIsNewPatient.ClientID %>').is(":checked");

            $('#<%=chkIsNewPatient.ClientID %>').attr("disabled", false);
            $('#lblMRN').attr('class', 'lblLink');

            if (isNew) {
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
                cboSalutationAppo.SetEnabled(true);
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
                cboSalutationAppo.SetEnabled(false);

            }
        });

        function onBeforeLoadRightPanelContent(code) {
            var param = '';
            if (code == 'patientIdentity') {
                param = $('#<%:hdnMRN.ClientID %>').val() + '|';
            }
            else if (code == 'infoDokterPraktek') {
                param = $('#<%:hdnDepartmentID.ClientID %>').val();
            }
            else if (code == 'infoVoidAppointment') {
                var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
                var selectedDate = $('#<%:hdnCalAppointmentSelectedDate.ClientID %>').val();
                var serviceUnitID = cboServiceUnit.GetValue();

                param = paramedicID + "|" + selectedDate + "|" + serviceUnitID;
            }
            else if (code == 'changeRoomAppointment') {
                var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
                var selectedDate = $('#<%:hdnCalAppointmentSelectedDate.ClientID %>').val();
                var serviceUnitID = cboServiceUnit.GetValue();
                var startTime = $('#<%:hdnStartTime.ClientID %>').val();
                var endTime = $('#<%:hdnEndTime.ClientID %>').val();
                var session = cboSession.GetText();

                param = paramedicID + "|" + selectedDate + "|" + serviceUnitID + "|" + startTime + "|" + endTime + "|" + session;
            }
            else if (code == 'draftOrder' || code == 'draftCharges' || code == 'draftPrescriptionOrder') {
                param = $('#<%=hdnAppointmentID.ClientID %>').val() + "|" + $('#<%=hdnDepartmentID.ClientID %>').val();
            }
            else if (code == 'draftServiceOrderEmergency' || code == 'draftServiceOrderOutpatient') {
                param = $('#<%=hdnAppointmentID.ClientID %>').val() + "|" + $('#<%=hdnParamedicID.ClientID %>').val();
            }
            return param;
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
            var mrn = $('#<%=hdnMRN.ClientID %>').val();
            var selectedDate = $('#<%:hdnCalAppointmentSelectedDate.ClientID %>').val();
            if (code == 'PM-00505') {
                if (appointmentID != '' && appointmentID != null) {
                    filterExpression.text = appointmentID;
                    return true;
                }
                else {
                    errMessage.text = 'Harap Pilih Appointment Terlebih Dahulu';
                    return false;
                }
            }
            else if (code == 'PM-00506') {
                filterExpression.text = '0|' + selectedDate;
                return true;
            }
            else if (code == 'PM-00144' || code == 'PM-00172') {
                filterExpression.text = appointmentID;
                return true;
            }
            else if (code == 'PM-00149' || code == 'PM-00150') {
                filterExpression.text = appointmentID;
                return true;
            }
            else if (code == 'PM-00617') {
                filterExpression.text = "MRN = " + mrn;
                return true;
            }
            else if (code == "PM-00176") {
                filterExpression.text = mrn;
                return true;
            }
        }

        function onAfterSaveRightPanelContent(code, value, isAdd) {
            if (code == 'patientIdentity') {
                var GCAppointmentStatus = $(this).find('.hdnGCAppointmentStatus').val();
                var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
                var filterExpression = "AppointmentID = " + appointmentID;
                Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                    var gcAppointmentStatus = result.GCAppointmentStatus;
                    var appointmentID = result.AppointmentID;
                    var appointmentNo = result.AppointmentNo;
                    var queueNo = result.QueueNo;
                    var queueNocf = result.cfQueueNo;
                    var streetName = result.StreetName;
                    var phoneNo = result.PhoneNo;
                    var mobilePhoneNo = result.MobilePhoneNo;
                    var emailAddress = result.EmailAddress;
                    var masterCorporateAccountNo = result.CorporateAccountNo;
                    var masterCorporateAccountName = result.CorporateAccountName;
                    var salutation = result.Salutation;
                    var firstName = result.FirstName;
                    var middleName = result.MiddleName;
                    var lastName = result.LastName;
                    var medicalNo = result.MedicalNo;
                    var notes = result.Notes;
                    var visitDuration = result.VisitDuration;
                    var visitTypeID = result.VisitTypeID;
                    var visitTypeCode = result.VisitTypeCode;
                    var visitTypeName = result.VisitTypeName;
                    var roomID = result.RoomID;
                    var roomCode = result.RoomCode;
                    var roomName = result.RoomName;
                    var gender = result.Gender;
                    var gcGender = result.GCGender;
                    var dateOfBirthInString = result.cfDateOfBirthInString1;
                    var mrn = result.MRN;
                    var businessPartnerID = result.BusinessPartnerID;
                    var businessPartnerCode = result.BusinessPartnerCode;
                    var businessPartnerName = result.BusinessPartnerName;
                    var gcCustomerType = result.GCCustomerType;
                    var coverageTypeCode = result.CoverageTypeCode;
                    var coverageTypeName = result.CoverageTypeName;
                    var corporateAccountNo = result.CorporateAccountNo;
                    var employeeCode = result.EmployeeCode;
                    var employeeName = result.EmployeeName;
                    var coverageLimitAmountInString = result.cfCoverageLimitAmountInString;
                    var isUsingCob = result.IsUsingCOB;
                    var isCoverageLimitPerDay = result.IsCoverageLimitPerDay;
                    var contractNo = result.ContractNo;
                    var appointmentMethod = result.GCAppointmentMethod;
                    cboAppointmentMethod.SetEnabled(false);
                    if (appointmentMethod != '' && appointmentMethod != null) {
                        cboAppointmentMethod.SetValue(appointmentMethod);
                    }
                    else {
                        cboAppointmentMethod.SetValue('');
                    }
                    if (gcAppointmentStatus == Constant.AppointmentStatus.DELETED) {
                        $('#<%=bntReopen.ClientID %>').removeAttr('style');
                    }
                    else {
                        $('#<%=bntReopen.ClientID %>').attr('style', 'display:none');
                    }
                    $('#<%=trChange.ClientID %>').removeAttr('style');
                    $('#draftCharges').removeAttr('enabled');
                    $('#draftOrder').removeAttr('enabled');
                    $('#draftServiceOrderEmergency').removeAttr('enabled');
                    $('#draftServiceOrderOutpatient').removeAttr('enabled');
                    $('#draftPrescriptionOrder').removeAttr('enabled');
                    $('#<%=hdnGCAppointmentStatus.ClientID %>').val(gcAppointmentStatus);
                    $('#<%=hdnAppointmentID.ClientID %>').val(appointmentID);
                    $('#<%=txtAppointmentNo.ClientID %>').val(appointmentNo);
                    $('#<%=txtQueueNo.ClientID %>').val(queueNocf);
                    $('#<%=txtAddress.ClientID %>').val(streetName);
                    $('#<%=txtPhoneNo.ClientID %>').val(phoneNo);
                    $('#<%=txtMobilePhone.ClientID %>').val(mobilePhoneNo);
                    $('#<%=txtEmail.ClientID %>').val(emailAddress);
                    $('#<%=txtCorporateAccountNo.ClientID %>').val(masterCorporateAccountNo);
                    $('#<%=txtCorporateAccountName.ClientID %>').val(masterCorporateAccountName);
                    if (salutation != '' && salutation != null) {
                        cboSalutationAppo.SetValue(salutation);
                    }
                    else {
                        cboSalutationAppo.SetValue('');
                    }
                    $('#<%=txtFirstName.ClientID %>').val(firstName);
                    $('#<%=txtMiddleName.ClientID %>').val(middleName);
                    $('#<%=txtFamilyName.ClientID %>').val(lastName);
                    $('#<%=txtMRN.ClientID %>').val(medicalNo);
                    $('#<%=txtRemarks.ClientID %>').val(notes);
                    $('#<%=txtVisitDuration.ClientID %>').val(visitDuration);
                    $('#<%=hdnVisitTypeID.ClientID %>').val(visitTypeID);

                    $('#<%=txtVisitTypeCode.ClientID %>').val(visitTypeCode);
                    $('#<%=txtVisitTypeName.ClientID %>').val(visitTypeName);
                    $('#<%=hdnRoomID.ClientID %>').val(roomID);
                    $('#<%=txtRoomCode.ClientID %>').val(roomCode);
                    $('#<%=txtRoomName.ClientID %>').val(roomName);
                    cboGenderAppointment.SetValue(gcGender);
                    $('#<%=txtDOBMainAppt.ClientID %>').val(dateOfBirthInString);
                    $('#<%=chkIsNewPatient.ClientID %>').attr("disabled", true);
                    $('#lblMRN').attr('class', 'lblDisabled');
                    if (medicalNo == '') {
                        $('#<%=chkIsNewPatient.ClientID %>').prop('checked', true);
                        $('#<%=hdnMRN.ClientID %>').val('');
                        $('#patientIdentity').attr('enabled', 'false');
                        cboGenderAppointment.SetEnabled(true);
                        Methods.getObject("GetvGuestList", "GuestID = " + guestID, function (resultGuest) {
                            if (resultGuest != null) {
                                $('#<%=txtDOBMainAppt.ClientID %>').val(resultGuest.DateOfBirthInStringDatePickerFormat);
                            };
                        });
                    }
                    else {
                        $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);
                        $('#<%=hdnMRN.ClientID %>').val(mrn);
                        $('#patientIdentity').removeAttr('enabled');
                        cboGenderAppointment.SetEnabled(false);
                    }
                    $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtCorporateAccountNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtCorporateAccountName.ClientID %>').attr('readonly', 'readonly');
                    $('#lblMRN').attr('class', 'lblDisabled');
                    $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMRN.ClientID %>').removeClass('error');
                    $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtDOBMainAppt.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPhoneNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobilePhone.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtEmail.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtCorporateAccountNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtCorporateAccountName.ClientID %>').attr('readonly', 'readonly');
                    cboSalutationAppo.SetEnabled(false);

                    cboAppointmentPayer.SetValue(gcCustomerType);
                    cboAppointmentPayer.SetEnabled(false);
                    setTblPayerCompanyVisibility();

                    $('#<%:lblPayerCompany.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtPayerCompanyCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtPayerCompanyCode.ClientID %>').val(businessPartnerCode);
                    $('#<%:txtPayerCompanyName.ClientID %>').val(businessPartnerName);
                    $('#<%:lblCoverageType.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtCoverageTypeCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(coverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtCoverageTypeName.ClientID %>').val(coverageTypeName);
                    $('#<%:txtParticipantNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtParticipantNo.ClientID %>').val(corporateAccountNo);
                    $('#<%:lblEmployee.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtEmployeeCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtEmployeeCode.ClientID %>').val(employeeCode);
                    $('#<%:txtEmployeeCode.ClientID %>').val(employeeName);
                    $('#<%:txtCoverageLimit.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtCoverageLimit.ClientID %>').val(coverageLimitAmountInString);
                    $('#<%=chkIsUsingCOB.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsUsingCOB.ClientID %>').prop('checked', isUsingCob);
                    $('#<%=chkIsCoverageLimitPerDay.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsCoverageLimitPerDay.ClientID %>').prop('checked', isCoverageLimitPerDay);
                    $('#<%:lblContract.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtContractNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtContractNo.ClientID %>').val(contractNo);

                    if (gcAppointmentStatus == Constant.AppointmentStatus.COMPLETE) {
                        $('#<%=txtVisitTypeCode.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtRemarks.ClientID %>').attr('readonly', 'readonly');
                    }
                    else {
                        $('#<%=txtVisitTypeCode.ClientID %>').removeAttr('readonly');
                        $('#<%=txtRemarks.ClientID %>').removeAttr('readonly');
                    }
                });

                $('#<%=trChange.ClientID %>').attr('style', 'display:none');
            }
            else if (code == 'changeRoomAppointment') {
                cbpAppointment.PerformCallback();
            }
        }

        function onCboReferralValueChanged(s) {
        }

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

        $('#btnPayerNotesDetail').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var id = $('#<%:hdnPayerID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/CustomerNotesDetailCtl.ascx");
                openUserControlPopup(url, id, 'Notes', 500, 400);
            }
        });

        $('#btnContractSummary').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var id = $('#<%:hdnContractID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Information/CustomerContractSummaryViewCtl.ascx");
                openUserControlPopup(url, id, 'Ringkasan Kontrak', 700, 600);
            }
        });

        $('#<%=lblParamedicCode.ClientID %>').html($('#<%=hdnParamedicCode.ClientID %>').val());
        $('#<%=lblParamedicName.ClientID %>').html($('#<%=hdnParamedicName.ClientID %>').val());

        function setTblPayerCompanyVisibility() {
           
        }

    </script>
    <style type="text/css">
        .tdAppointmentInformation
        {
            position: relative;
            cursor: pointer;
        }
        .tdAppointmentInformation .divAppointmentInformationDt
        {
            z-index: 11;
            font-size: 10px;
            display: none;
            padding: 5px;
            position: absolute;
            top: 15px;
            width: 300px;
            border: 1px solid #AAA;
            text-align: left;
            background-color: White;
        }
        .tdAppointmentInformation .divAppointmentInformationDt td
        {
            color: #000;
        }
        .grdAppointment > tbody > tr > td
        {
            vertical-align: top;
            padding: 0px;
        }
        .tdAppointment
        {
            padding: 0;
            margin: 0;
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
    <div id="ctxMenuAppointment" class="context-menu">
        <ol>
            <li id="ctxMenuEdit"><a href="#">
                <%=GetLabel("Edit")%></a> </li>
            <li id="ctxMenuVoid"><a href="#">
                <%=GetLabel("Delete")%></a> </li>
        </ol>
    </div>
    <input type="hidden" id="hdnDefaultServiceUnitInterval" runat="server" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsBridgingToGateway" runat="server" />
    <input type="hidden" id="hdnIsAppintmentAllowBackDate" runat="server" />
    <input type="hidden" id="hdnBPJSKesehatanID" runat="server" />
    <input type="hidden" id="hdnBPJSKetenagakerjaanID" runat="server" />
    <input type="hidden" id="hdnIsBridgingToQumatic" runat="server" />
    <input type="hidden" id="hdnApiKeyQumatic" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" runat="server" />
    <input type="hidden" id="hdnIsUsingValidationMaxAppointment" runat="server" />
    <input type="hidden" value="0" id="hdnDateTodayTime" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowRescheduleBackDate" runat="server" />
    <input type="hidden" id="hdnParamedicCode" runat="server" />
    <input type="hidden" id="hdnParamedicName" runat="server" />
    <input type="hidden" id="hdnParamedicInitial" runat="server" />
    <input type="hidden" id="hdnParamedicImageUrl" runat="server" />
    <input type="hidden" id="hdnParamedicSpecialty" runat="server" />
    <input type="hidden" id="hdnParamedicType" runat="server" />
    <input type="hidden" id="hdnParamedicStatus" runat="server" />
    <input type="hidden" id="hdnIsUsingResultDeliveryPlan" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 250px; max-width: 250px" />
            <col />
            <col style="max-width: 500px" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top; border-right: 1px solid #AAA;">
                <div style="height: 500px; overflow-y: auto; overflow-x: hidden;">
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 250px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDate" />
                                <center>
                                    <div id="calAppointment">
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                                <input type="hidden" id="hdnIsChangeParamedicOrHealthcare" runat="server" />
                                <input type="hidden" id="hdnParamedicID" runat="server" />
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpPhysician" runat="server" Width="100%" ClientInstanceName="cbpPhysician"
                                        ShowLoadingPanel="false" OnCallback="cbpPhysician_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPhysicianEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 200px">
                                                    <asp:GridView ID="grdPhysician" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="ParamedicCode" HeaderText="Physician Code" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdParamedicCode" />
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-CssClass="tdParamedicName" />
                                                            <asp:BoundField DataField="ParamedicCode" HeaderText="Image" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdParamedicImageUrl" />
                                                            <asp:BoundField DataField="EmploymentStatus" HeaderText="Employment Status" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdParamedicStatus" />
                                                            <asp:BoundField DataField="SpecialtyName" HeaderText="Specialty" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdParamedicSpecialty" />
                                                            <asp:BoundField DataField="ParamedicMasterType" HeaderText="ParamedicType" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdParamedicType" />
                                                            <asp:BoundField DataField="Initial" HeaderText="Initial" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdParamedicInitial" />
                                                            <asp:BoundField DataField="PhysicianImageUrl" HeaderText="Image" HeaderStyle-CssClass="hiddenColumn"
                                                                ItemStyle-CssClass="hiddenColumn tdPhysicianImageUrl" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="blink-alert">
                                                                <%=GetLabel("Tidak ada dokter atau tenaga medis yang tersedia di tanggal yang dipilih")%></div>
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
                                            <div id="pagingPhysican">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="vertical-align: top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpAppointment" runat="server" Width="100%" ClientInstanceName="cbpAppointment"
                        ShowLoadingPanel="false" OnCallback="cbpAppointment_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpAppointmentEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table class="tblPatientBannerInfo" cellpadding="0" cellspacing="0" width="100%">
                                                <col style="width: 60px" />
                                                <col style="width: 120px" />
                                                <col style="width: 180px" />
                                                <col style="width: 120px" />
                                                <col style="width: 120px" />
                                                <col style="width: 100px" />
                                                <col />
                                                <tr style="font-size: 1.2em">
                                                    <td rowspan="4">
                                                        <img id="imgParamedicProfilePicture" class="imgParamedicProfilePicture" runat="server"
                                                            src='~/Libs/Images/physician.png' alt="" width="55" />
                                                        <input type="hidden" id="hdnParamedicGender" runat="server" class="hdnParamedicGender" />
                                                    </td>
                                                    <td colspan="4" style="padding-left: 20px">
                                                        <label id="lblParamedicName" runat="server" style="font-weight: bold">
                                                            DOKTER/TENAGA MEDIS</label>
                                                    </td>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Sesi Jadwal")%>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboSession" ClientInstanceName="cboSession" Width="100%" runat="server">
                                                            <ClientSideEvents ValueChanged="function(s,e) { oncboSessionValueChanged(s); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Tipe Medis")%>
                                                    </td>
                                                    <td valign="top">
                                                        <label id="lblParamedicType" runat="server">
                                                        </label>
                                                    </td>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Kode")%>
                                                    </td>
                                                    <td valign="top">
                                                        <label id="lblParamedicCode" runat="server">
                                                        </label>
                                                    </td>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Catatan Jadwal :")%>
                                                    </td>
                                                    <td valign="top" rowspan="3">
                                                        <asp:TextBox ID="txtRemarksSchedule" ReadOnly="true" Width="97%" TextMode="MultiLine"
                                                            Rows="2" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Spesialisasi")%>
                                                    </td>
                                                    <td valign="top">
                                                        <label id="lblParamedicSpecialty" runat="server">
                                                        </label>
                                                    </td>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Status Kepegawaian")%>
                                                    </td>
                                                    <td valign="top">
                                                        <label id="lblEmploymentStatus" runat="server">
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                        <%=GetLabel("Inisial")%>
                                                    </td>
                                                    <td valign="top">
                                                        <label id="lblParamedicInitial" runat="server">
                                                        </label>
                                                    </td>
                                                    <td valign="top" class="tdPatientBannerLabel">
                                                    </td>
                                                    <td valign="top">
                                                        <label id="lblNote" runat="server">
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr id="trInformation" runat="server">
                                                    <td colspan="7">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" class="tdPatientBannerLabel">
                                                                    <p>
                                                                        <%=GetLabel("Max. Appointment")%></p>
                                                                </td>
                                                                <td valign="top">
                                                                    <p>
                                                                        <span class="w3-badge w3-green">
                                                                            <label id="lblMaximumAppointment" runat="server" title="Kuota Maksimum Appointment (Non-BPJS+BPJS)">
                                                                                0</label></span></p>
                                                                </td>
                                                                <td valign="top" class="tdPatientBannerLabel" style="padding-left: 5px">
                                                                    <p>
                                                                        <%=GetLabel("Appointment : Non-BPJS")%></p>
                                                                </td>
                                                                <td valign="top">
                                                                    <p>
                                                                        <span class="w3-badge w3-blue">
                                                                            <label id="lblTotalAppointmentNonBPJS" runat="server" title="Total Appointment : Non-BPJS">
                                                                                0</label></span></p>
                                                                </td>
                                                                <td valign="top" class="tdPatientBannerLabel" style="padding-left: 5px">
                                                                    <p>
                                                                        <%=GetLabel("Appointment : BPJS")%></p>
                                                                </td>
                                                                <td valign="top">
                                                                    <p>
                                                                        <span class="w3-badge w3-blue">
                                                                            <label id="lblTotalAppointmentBPJS" runat="server" title="Total Appointment : BPJS">
                                                                                0</label></span></p>
                                                                </td>
                                                                <td valign="top" class="tdPatientBannerLabel" style="padding-left: 5px">
                                                                    <p>
                                                                        <%=GetLabel("Registrasi")%></p>
                                                                </td>
                                                                <td valign="top">
                                                                    <p>
                                                                        <span class="w3-badge w3-yellow">
                                                                            <label id="lblTotalRegistered" runat="server" title="Total Registrasi (Non-BPJS+BPJS)">
                                                                                0</label></span></p>
                                                                </td>
                                                                <td valign="top" class="tdPatientBannerLabel" style="padding-left: 5px">
                                                                    <p>
                                                                        <%=GetLabel("Batal")%></p>
                                                                </td>
                                                                <td valign="top">
                                                                    <p>
                                                                        <span class="w3-badge w3-red">
                                                                            <label id="lblCancelAppointment" runat="server" title="Total Batal Appointment (Non-BPJS+BPJS)">
                                                                                0</label></span></p>
                                                                </td>
                                                                <td valign="top" class="tdPatientBannerLabel" style="padding-left: 5px">
                                                                    <p>
                                                                        <%=GetLabel("Waiting List")%></p>
                                                                </td>
                                                                <td valign="top">
                                                                    <p>
                                                                        <span class="w3-badge w3-orange">
                                                                            <label id="lblMaximumWaitingList" runat="server" title="Total Waiting List (Non-BPJS+BPJS)">
                                                                                0</label></span></p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trLeaveInformation" style="display: none" runat="server">
                                                    <td colspan="7">
                                                        <div class="w3-panel w3-red">
                                                            <h3 class='blink'>
                                                                Informasi Cuti :</h3>
                                                            <p id="lblLeaveDateInfo" runat="server">
                                                                <i>Mulai dari tanggal : </i><b>{0}</b> <i>sampai dengan</i> <b>{1}</b></p>
                                                            <p id="lblLeaveReason" runat="server">
                                                                Keterangan Cuti</p>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div class="containerTblEntryContent">
                                    <table>
                                        <colgroup>
                                            <col width="5%" />
                                        </colgroup>
                                    </table>
                                </div>
                                <asp:Panel runat="server" ID="Panel1">
                                    <input type="hidden" runat="server" id="hdnIsWaitingList" value="0" />
                                    <input type="hidden" runat="server" id="hdnIsByTimeSlot" value="1" />
                                    <input type="hidden" runat="server" id="hdnIsByNoTimeSlot" value="0" />
                                    <input type="hidden" value="" id="hdnRoomIDDefault" runat="server" />
                                    <input type="hidden" value="" id="hdnRoomCodeDefault" runat="server" />
                                    <input type="hidden" value="" id="hdnRoomNameDefault" runat="server" />
                                    <input type="hidden" value="" id="hdnMaxAppoMessage" runat="server" />
                                    <input type="hidden" value="" id="hdnMaxWaitingMessage" runat="server" />
                                    <input type="hidden" value="" id="hdnTotalAppoMessage" runat="server" />
                                    <input type="hidden" value="" id="hdnTotalWaitingMessage" runat="server" />
                                    <input type="hidden" value="" id="hdnIsValidAppoMessage" runat="server" />
                                    <input type="hidden" value="" id="hdnIsValidWaitingMessage" runat="server" />
                                    <input type="hidden" value="" id="hdnIsShowTimeSlotContainerEntry" runat="server" />
                                    <input type="hidden" value="" id="hdnStartTime" runat="server" />
                                    <input type="hidden" value="" id="hdnEndTime" runat="server" />
                                    <div class="toolbarArea">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <ul id="ulMPTrxToolbar" runat="server">
                                                        <li id="btnAddAppointment" runat="server" crudmode="R">
                                                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbNew.png")%>' alt="" /><div>
                                                                <%=GetLabel("New Appointment")%></div>
                                                        </li>
                                                    </ul>
                                                </td>
                                                 <td>
                                                    <ul id="ul1" runat="server">
                                                        <li id="btnAppointmentChangeAppointment" runat="server" crudmode="R">
                                                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
                                                                <%=GetLabel("Change Appointment")%></div>
                                                        </li>
                                                    </ul>
                                                </td>
                                                <td>
                                                    <ul id="ul2" runat="server">
                                                        <li id="btnAppointmentProcessRegistration" runat="server" crudmode="R">
                                                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
                                                                <%=GetLabel("Registrasi")%></div>
                                                        </li>
                                                    </ul>
                                                </td>
                                                 <td>
                                                    <ul id="ul3" runat="server">
                                                        <li id="btnAppointmentVoid" runat="server" crudmode="R">
                                                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
                                                                <%=GetLabel("Void")%></div>
                                                        </li>
                                                    </ul>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="containerUlTabPage">
                                        <ul class="ulTabPage" id="ulTabClinicTransaction">
                                            <li class="selected" contentid="containerAppointment" id="containerAppointment" runat="server">
                                                <%=GetLabel("APPOINTMENT")%></li>
                                            <li contentid="containerAppointmentNoTimeSlot" id="containerAppointmentNoTimeSlot"
                                                runat="server">
                                                <%=GetLabel("APPOINTMENT (NO TIME SLOT)") %></li>
                                            <li contentid="containerWaitingList" id="containerWaitingList" runat="server">
                                                <%=GetLabel("WAITING LIST") %></li>
                                        </ul>
                                    </div>
                                    <div id="containerAppointment" class="containerAppointment">
                                        <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected grdAppointment"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdAppointment_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField ItemStyle-CssClass="tdExpand" HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <img class="imgExpand" <%#: Eval("ParentID").ToString() != "-1" ? "style='display:none;'" : "style='cursor:pointer'"%>
                                                            src='<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>' alt='' />
                                                        <input type="hidden" parentid='<%#: Eval("ParentID")%>' />
                                                        <input type="hidden" class="hdnIsExpand" value="1" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Time") %>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="tdTime"><%#: Eval("Time") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                    <ItemTemplate>
                                                        <asp:Repeater ID="rptAppointmentInformation" runat="server">
                                                            <HeaderTemplate>
                                                                <ol>
                                                                   
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li>
                                                                    <div class="tdAppointmentInformation">
                                                                        <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                        <input type="hidden" class="hdnGCAppointmentStatus" value='<%#: Eval("GCAppointmentStatus")%>' />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Status/VIP.png") %>' height="25px" title="Vip"
                                                                            <%# Eval("IsVip").ToString() == "False" ? "style='display:none;float:right'" : "style='float:left'"%> />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                            <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Status/cancel.png") %>' height="16px" title="Delete"
                                                                            <%# Eval("IsAppointmentDeleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                        <div class="divPatientName" style="text-align: left">
                                                                            <%#: Eval("PatientName") %></div>
                                                                        <div class="divAppointmentInformationDt">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img src="<%#: Eval("PatientImageUrl") %>" height="60" width="55" alt="" />
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div>
                                                                                            <%#: Eval("AppointmentNo") %></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("PatientName") %></b></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("StartTime") %>
                                                                                                -
                                                                                                <%#: Eval("EndTime") %></b></div>
                                                                                        <div>
                                                                                            <%#: Eval("VisitTypeName") %></div>
                                                                                        <div>
                                                                                            <%#: Eval("CreatedInfo") %></div>
                                                                                        <div>
                                                                                            <%#: Eval("LastUpdatedInfo") %></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("AppointmentStatus") %></b></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ol>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                    <div id="containerAppointmentNoTimeSlot" style="display: none" class="containerAppointmentNoTimeSlot">
                                        <asp:GridView ID="grdAppointmentNoTimeSlot" runat="server" CssClass="grdSelected grdAppointment"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdAppointmentNoTimeSlot_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <%=GetLabel("No.") %>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="tdTime">
                                                            <%#: Eval("Queue") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table border="0" cellpadding="1" cellspacing="1">
                                                            <colgroup>
                                                                <col style="width: 24px;" />
                                                                <col style="width: 24px;" />
                                                                <col style="width: 24px;" />
                                                                <col style="width: 24px;" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <div <%# Eval("IsEmpty").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                        <img class="btnAddAppointment2 imgLink" title='<%=GetLabel("Add Appointment")%>'
                                                                            src='<%=ResolveUrl("~/Libs/Images/Button/plus.png")%>' alt="" queueno='<%#: Eval("Queue") %>' />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div <%# Eval("IsEmpty").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                        <img class="btnEditAppointment2 imgLink" title='<%=GetLabel("Edit Appointment")%>'
                                                                            src='<%=ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div <%# Eval("IsEmpty").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                        <img class="btnDeleteAppointment2 imgLink" title='<%=GetLabel("Hapus Appointment")%>'
                                                                            src='<%=ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div <%# Eval("IsEmpty").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                        <img class="btnRegistration2 imgLink" title='<%=GetLabel("Registrasi")%>' src='<%=ResolveUrl("~/Libs/Images/Button/process.png")%>'
                                                                            alt="" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                    <ItemTemplate>
                                                        <asp:Repeater ID="rptAppointmentInformation" runat="server">
                                                            <HeaderTemplate>
                                                                <ol>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li>
                                                                    <div class="tdAppointmentInformation">
                                                                        <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                        <input type="hidden" class="hdnGCAppointmentStatus" value='<%#: Eval("GCAppointmentStatus")%>' />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Status/VIP.png") %>' height="25px" title="Vip"
                                                                            <%# Eval("IsVip").ToString() == "False" ? "style='display:none;float:right'" : "style='float:left'"%> />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                            <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Status/cancel.png") %>' height="16px" title="Delete"
                                                                            <%# Eval("IsAppointmentDeleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                        <b>
                                                                            <div style="text-align: left; font-style: italic">
                                                                                <%#: Eval("EstimatedTimeService") %></div>
                                                                        </b>
                                                                        <div class="divPatientName" style="text-align: left">
                                                                            <%#: Eval("PatientName") %></div>
                                                                        <div class="divAppointmentInformationDt">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img src="<%#: Eval("PatientImageUrl") %>" height="60" width="55" alt="" />
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div>
                                                                                            <%#: Eval("AppointmentNo") %></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("PatientName") %></b></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("StartTime") %>
                                                                                                -
                                                                                                <%#: Eval("EndTime") %></b></div>
                                                                                        <div>
                                                                                            <%#: Eval("VisitTypeName") %></div>
                                                                                        <div>
                                                                                            <%#: Eval("CreatedInfo") %></div>
                                                                                        <div>
                                                                                            <%#: Eval("LastUpdatedInfo") %></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("AppointmentStatus") %></b></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ol>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                    <div id="containerWaitingList" style="display: none" class="containerWaitingList">
                                        <asp:GridView ID="grdWaitingList" runat="server" CssClass="grdSelected grdAppointment"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdWaitingList_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="70px">
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Antrian") %>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="tdTime">
                                                            <%#: Eval("Queue") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                    <ItemTemplate>
                                                        <asp:Repeater ID="rptAppointmentInformation" runat="server">
                                                            <HeaderTemplate>
                                                                <ol>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li>
                                                                    <div class="tdAppointmentInformation">
                                                                        <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                        <input type="hidden" class="hdnGCAppointmentStatus" value='<%#: Eval("GCAppointmentStatus")%>' />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Status/VIP.png") %>' height="25px" title="Vip"
                                                                            <%# Eval("IsVip").ToString() == "False" ? "style='display:none;float:right'" : "style='float:left'"%> />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                            <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Status/cancel.png") %>' height="16px" title="Delete"
                                                                            <%# Eval("IsAppointmentDeleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                        <div class="divPatientName" style="text-align: left">
                                                                            <%#: Eval("PatientName") %></div>
                                                                        <div class="divAppointmentInformationDt">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img src="<%#: Eval("PatientImageUrl") %>" height="60" width="55" alt="" />
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div>
                                                                                            <%#: Eval("AppointmentNo") %></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("PatientName") %></b></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("StartTime") %>
                                                                                                -
                                                                                                <%#: Eval("EndTime") %></b></div>
                                                                                        <div>
                                                                                            <%#: Eval("VisitTypeName") %></div>
                                                                                        <div>
                                                                                            <%#: Eval("CreatedInfo") %></div>
                                                                                        <div>
                                                                                            <%#: Eval("LastUpdatedInfo") %></div>
                                                                                        <div>
                                                                                            <b>
                                                                                                <%#: Eval("AppointmentStatus") %></b></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ol>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top; background-color: Yellow; display: none">
                <fieldset id="fsAppointment" style="margin: 0">
                    <input type="hidden" id="hdnAppointmentID" runat="server" />
                    <input type="hidden" id="hdnGCAppointmentStatus" runat="server" />
                    <h4>
                        <%=GetLabel("Visit Information")%></h4>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 25%" />
                            <col style="width: 25%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblAppointmentNo">
                                    <%=GetLabel("Appointment No")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentNo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td style="text-align: right;">
                                <label class="lblNormal" id="lblQueueNo">
                                    <%=GetLabel("Queue No")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQueueNo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Appointment Date/Time")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentDate" ReadOnly="true" Width="100%" runat="server"
                                    CssClass="datepicker" />
                            </td>
                            <td style="text-align: right;">
                                <%=GetLabel("Hour")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentHour" runat="server" Width="100%" CssClass="time" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Service Unit")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtServiceUnit" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblRoom">
                                    <%=GetLabel("Room")%></label>
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
                                    <%=GetLabel("Physician")%></label>
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
                                    <%=GetLabel("Visit Type")%></label>
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
                                    <%=GetLabel("Visit Duration")%></label>
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
                                            <asp:TextBox ID="txtVisitDuration" Width="100%" runat="server" CssClass="number"
                                                ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
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
                                    <%=GetLabel("Appointment Method")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboAppointmentMethod" ClientInstanceName="cboAppointmentMethod"
                                    Width="100%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Remarks")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                        <tr id="trResultDeliveryPlan" runat="server" style="display:none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Rencana Pengambilan Hasil")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboResultDeliveryPlan" ClientInstanceName="cboResultDeliveryPlan"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ oncboResultDeliveryPlanValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResultDeliveryPlanOthers" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <h4>
                        <%=GetLabel("Patient Information")%></h4>
                    <table style="float: right">
                        <tr>
                            <td align="right">
                                <div id="divWarning" style="display: none;" runat="server">
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top" align="right">
                                                <label class="lblWarning">
                                                    <%=GetLabel("Anda Akan Mengubah Data Perjanjian") %></label>
                                            </td>
                                            <td rowspan="3">
                                                <img height="50" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top" align="right">
                                                <label class="lblWarning">
                                                    <%=GetLabel("Pastikan Pengisian Sudah Benar") %></label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr id="trChange" runat="server">
                            <td align="right">
                                <input type="button" id="btnChange" value='<%= GetLabel("GANTI PASIEN")%>' />
                            </td>
                        </tr>
                    </table>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNewPatient" runat="server" /><%=GetLabel("Is New Patient")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMRN">
                                    <%=GetLabel("MRN")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnMRN" value="" runat="server" />
                                <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Patient Name")%></label>
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
                                    <%=GetLabel("Gender")%></label>
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
                                    <%=GetLabel("Date Of Birth")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDOBMainAppt" ReadOnly="true" Width="30%" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Address")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Phone No")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Mobile Phone")%></label>
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
                                    <%=GetLabel("No Karyawan")%></label>
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
                    <h4>
                        <%=GetLabel("Payer Information")%></h4>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%:GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboAppointmentPayer" ClientInstanceName="cboAppointmentPayer"
                                                Width="100%" runat="server">
                                                <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td id="chkUsingCOB" runat="server">
                                            <asp:CheckBox ID="chkIsUsingCOB" Checked="false" runat="server" /><%:GetLabel("Peserta COB")%>
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
                                            <input type="button" id="btnPayerNotesDetail" value="..." />
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
                                <input type="hidden" id="hdnContractID" value="" runat="server" />
                                <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                                <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                                <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                    <%:GetLabel("Tipe Coverage")%></label>
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
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <%--<label class="lblNormal">
                                <%:GetLabel("Ringkasan Kontrak")%></label>--%>
                            </td>
                            <td>
                                <input type="button" id="btnContractSummary" value="Ringkasan Kontrak" />
                                <%--<asp:TextBox ID="txtContractSummary" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="2" />--%>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>
