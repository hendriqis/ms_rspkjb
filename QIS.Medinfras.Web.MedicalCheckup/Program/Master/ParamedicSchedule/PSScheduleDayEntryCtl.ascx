<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PSScheduleDayEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.PSScheduleDayEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PSScheduleWithDateEntryCtl">
    $('#lblEntryPopupAddDataScheduleDay').live('click', function () {
        $('#<%=hdnDayNumber.ClientID %>').val('');
        $('#<%=hdnRoomID.ClientID %>').val('');
        $('#<%=txtRoomCode.ClientID %>').val('');
        $('#<%=txtRoomName.ClientID %>').val('');
        $('#<%=txtRemarks.ClientID %>').val('');
        $('#<%=hdnOperationalTimeID.ClientID %>').val('');
        $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val('');
        $('#<%=txtOperationalTimeNameCtl.ClientID %>').val('');

        $('#<%=txtStartTime1.ClientID %>').val('');
        $('#<%=txtStartTime2.ClientID %>').val('');
        $('#<%=txtStartTime3.ClientID %>').val('');
        $('#<%=txtStartTime4.ClientID %>').val('');
        $('#<%=txtStartTime5.ClientID %>').val('');

        $('#<%=txtEndTime1.ClientID %>').val('');
        $('#<%=txtEndTime2.ClientID %>').val('');
        $('#<%=txtEndTime3.ClientID %>').val('');
        $('#<%=txtEndTime4.ClientID %>').val('');
        $('#<%=txtEndTime5.ClientID %>').val('');

        $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
        $('#<%=txtMaximumAppointment1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
        $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
        $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
        $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
        $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtMobileAppointment1.ClientID %>').val('');
        $('#<%=txtMobileAppointment1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment2.ClientID %>').val('');
        $('#<%=txtMobileAppointment2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment3.ClientID %>').val('');
        $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment4.ClientID %>').val('');
        $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment5.ClientID %>').val('');
        $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');

        $('#<%:chkIsAllowWaitingList1.ClientID %>').attr("disabled", true);
        $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
        $('#<%:chkIsAllowWaitingList2.ClientID %>').attr("disabled", true);
        $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
        $('#<%:chkIsAllowWaitingList3.ClientID %>').attr("disabled", true);
        $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
        $('#<%:chkIsAllowWaitingList4.ClientID %>').attr("disabled", true);
        $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
        $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
        $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);

        $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
        $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
        $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
        $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
        $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
        $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
        $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
        $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
        $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
        $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
        $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');
    
        $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');

        $('#<%:chkIsBPJSAppointment1.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJSAppointment2.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJSAppointment3.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJSAppointment4.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJSAppointment5.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);

        $('#<%:chkIsNonBPJSAppointment1.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJSAppointment2.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJSAppointment3.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJSAppointment4.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJSAppointment5.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);

        $('#containerPopupEntryData').show();
    });

    //    $('#btnEntryPopupCancel').live('click', function () {
    //        $('#containerPopupEntryData').hide();
    //    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    //#region Function CheckBox
    $('#<%=chkIsAllowWaitingList1.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList1.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
            $('#<%=txtCalculateResultWaiting1.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAllowWaitingList2.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList2.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
            $('#<%=txtCalculateResultWaiting2.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAllowWaitingList3.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList3.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
            $('#<%=txtCalculateResultWaiting3.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAllowWaitingList4.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList4.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
            $('#<%=txtCalculateResultWaiting4.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAllowWaitingList5.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList5.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
            $('#<%=txtCalculateResultWaiting5.ClientID %>').val('');
        }
    });
    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');

            var ParamedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            var HealthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var ScheduleDay = $row.find('.hdnDayNumber').val();
            $('#<%=hdnDayNumber.ClientID %>').val(ScheduleDay);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var ScheduleDay = $row.find('.hdnDayNumber').val();
        var ScheduleDateInString = $row.find('.tdScheduleDate').html();
        cboDay.SetValue(ScheduleDay);

        var RoomIDEntry = $row.find('.hdnRoomIDEntry').val();
        var RoomCodeEntry = $row.find('.hdnRoomCodeEntry').val();
        var RoomNameEntry = $row.find('.tdRoomName').html();

        var Remarks = $row.find('.hdnParamedicScheduleRemarks').val();

        var OperationalTimeIDEntry = $row.find('.hdnOperationalTimeIDEntry').val();
        var OperationalTimeCodeEntry = $row.find('.hdnOperationalTimeCodeEntry').val();
        var OperationalTimeNameEntry = $row.find('.tdOperationalTimeName').html();

        var IsAppointmentByTimeSlot1 = $row.find('.tdIsAppointmentByTimeSlot1').val();
        var IsAppointmentByTimeSlot2 = $row.find('.tdIsAppointmentByTimeSlot2').val();
        var IsAppointmentByTimeSlot3 = $row.find('.tdIsAppointmentByTimeSlot3').val();
        var IsAppointmentByTimeSlot4 = $row.find('.tdIsAppointmentByTimeSlot4').val();
        var IsAppointmentByTimeSlot5 = $row.find('.tdIsAppointmentByTimeSlot5').val();

        var StartTime1 = $row.find('.tdStartTime1').val();
        var StartTime2 = $row.find('.tdStartTime2').val();
        var StartTime3 = $row.find('.tdStartTime3').val();
        var StartTime4 = $row.find('.tdStartTime4').val();
        var StartTime5 = $row.find('.tdStartTime5').val();

        var EndTime1 = $row.find('.tdEndTime1').val();
        var EndTime2 = $row.find('.tdEndTime2').val();
        var EndTime3 = $row.find('.tdEndTime3').val();
        var EndTime4 = $row.find('.tdEndTime4').val();
        var EndTime5 = $row.find('.tdEndTime5').val();

        var MaximumAppointment1 = $row.find('.tdMaximumAppointment1').val();
        var MaximumAppointment2 = $row.find('.tdMaximumAppointment2').val();
        var MaximumAppointment3 = $row.find('.tdMaximumAppointment3').val();
        var MaximumAppointment4 = $row.find('.tdMaximumAppointment4').val();
        var MaximumAppointment5 = $row.find('.tdMaximumAppointment5').val();

        var MobileAppointment1 = $row.find('.tdMobileAppointment1').val();
        var MobileAppointment2 = $row.find('.tdMobileAppointment2').val();
        var MobileAppointment3 = $row.find('.tdMobileAppointment3').val();
        var MobileAppointment4 = $row.find('.tdMobileAppointment4').val();
        var MobileAppointment5 = $row.find('.tdMobileAppointment5').val();

        var IsAllowWaitingList1 = $row.find('.tdIsAllowWaitingList1').val();
        var IsAllowWaitingList2 = $row.find('.tdIsAllowWaitingList2').val();
        var IsAllowWaitingList3 = $row.find('.tdIsAllowWaitingList3').val();
        var IsAllowWaitingList4 = $row.find('.tdIsAllowWaitingList4').val();
        var IsAllowWaitingList5 = $row.find('.tdIsAllowWaitingList5').val();

        var MaximumWaitingList1 = $row.find('.tdMaximumWaitingList1').val();
        var MaximumWaitingList2 = $row.find('.tdMaximumWaitingList2').val();
        var MaximumWaitingList3 = $row.find('.tdMaximumWaitingList3').val();
        var MaximumWaitingList4 = $row.find('.tdMaximumWaitingList4').val();
        var MaximumWaitingList5 = $row.find('.tdMaximumWaitingList5').val();

        var MobileWaitingPercentage1 = $row.find('.tdMobileWaitingPercentage1').val();
        var MobileWaitingPercentage2 = $row.find('.tdMobileWaitingPercentage2').val();
        var MobileWaitingPercentage3 = $row.find('.tdMobileWaitingPercentage3').val();
        var MobileWaitingPercentage4 = $row.find('.tdMobileWaitingPercentage4').val();
        var MobileWaitingPercentage5 = $row.find('.tdMobileWaitingPercentage5').val();

        var ReservedQueueStartNo1 = $row.find('.tdReservedQueueStartNo1').val();
        var ReservedQueueStartNo2 = $row.find('.tdReservedQueueStartNo2').val();
        var ReservedQueueStartNo3 = $row.find('.tdReservedQueueStartNo3').val();
        var ReservedQueueStartNo4 = $row.find('.tdReservedQueueStartNo4').val();
        var ReservedQueueStartNo5 = $row.find('.tdReservedQueueStartNo5').val();

        var ReservedQueueEndNo1 = $row.find('.tdReservedQueueEndNo1').val();
        var ReservedQueueEndNo2 = $row.find('.tdReservedQueueEndNo2').val();
        var ReservedQueueEndNo3 = $row.find('.tdReservedQueueEndNo3').val();
        var ReservedQueueEndNo4 = $row.find('.tdReservedQueueEndNo4').val();
        var ReservedQueueEndNo5 = $row.find('.tdReservedQueueEndNo5').val();

        var IsBPJSAppointment1 = $row.find('.tdIsBPJSAppointment1').val();
        var IsBPJSAppointment2 = $row.find('.tdIsBPJSAppointment2').val();
        var IsBPJSAppointment3 = $row.find('.tdIsBPJSAppointment3').val();
        var IsBPJSAppointment4 = $row.find('.tdIsBPJSAppointment4').val();
        var IsBPJSAppointment5 = $row.find('.tdIsBPJSAppointment5').val();

        var IsNonBPJSAppointment1 = $row.find('.tdIsNonBPJSAppointment1').val();
        var IsNonBPJSAppointment2 = $row.find('.tdIsNonBPJSAppointment2').val();
        var IsNonBPJSAppointment3 = $row.find('.tdIsNonBPJSAppointment3').val();
        var IsNonBPJSAppointment4 = $row.find('.tdIsNonBPJSAppointment4').val();
        var IsNonBPJSAppointment5 = $row.find('.tdIsNonBPJSAppointment5').val();

        $('#<%=hdnDayNumber.ClientID %>').val(ScheduleDay);
        $('#<%=hdnOperationalTimeID.ClientID %>').val(OperationalTimeIDEntry);
        $('#<%=txtRemarks.ClientID %>').val(Remarks);

        if (RoomCodeEntry != "") {
            $('#<%=txtRoomCode.ClientID %>').val(RoomCodeEntry);
            $('#<%=txtRoomName.ClientID %>').val(RoomNameEntry);
        } else {
            $('#<%=txtRoomCode.ClientID %>').val('');
            $('#<%=txtRoomName.ClientID %>').val('');
        }

        $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val(OperationalTimeCodeEntry);
        $('#<%=txtOperationalTimeNameCtl.ClientID %>').val(OperationalTimeNameEntry);

        if (IsAppointmentByTimeSlot1 == 'True' || IsAppointmentByTimeSlot2 == 'True' || IsAllowWaitingList3 == 'True' || IsAllowWaitingList4 == 'True' || IsAllowWaitingList5 == 'True') {
            $('#<%=chkIsAppointmentByTimeSlot.ClientID %>').prop('checked', IsAppointmentByTimeSlot1);

            if (MaximumAppointment1 != '0') {
                $('#<%=txtMaximumAppointment1.ClientID %>').val(MaximumAppointment1);
                $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
            } else {
                $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                $('#<%=txtMaximumAppointment1.ClientID %>').attr('readonly', 'readonly');
            }

            if (MaximumAppointment2 != '0') {
                $('#<%=txtMaximumAppointment2.ClientID %>').val(MaximumAppointment2);
                $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
            } else {
                $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
            }

            if (MaximumAppointment3 != '0') {
                $('#<%=txtMaximumAppointment3.ClientID %>').val(MaximumAppointment3);
                $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
            } else {
                $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
            }

            if (MaximumAppointment4 != '0') {
                $('#<%=txtMaximumAppointment4.ClientID %>').val(MaximumAppointment4);
                $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
            } else {
                $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
            }

            if (MaximumAppointment5 != '0') {
                $('#<%=txtMaximumAppointment5.ClientID %>').val(MaximumAppointment5);
                $('#<%=txtMaximumAppointment5.ClientID %>').removeAttr('readonly', 'readonly');
            } else {
                $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');
            }

        } else {
            $('#<%=chkIsAppointmentByTimeSlot.ClientID %>').prop('checked', false);
        }

        $('#<%=txtStartTime1.ClientID %>').val(StartTime1);
        $('#<%=txtStartTime2.ClientID %>').val(StartTime2);
        $('#<%=txtStartTime3.ClientID %>').val(StartTime3);
        $('#<%=txtStartTime4.ClientID %>').val(StartTime4);
        $('#<%=txtStartTime5.ClientID %>').val(StartTime5);

        $('#<%=txtEndTime1.ClientID %>').val(EndTime1);
        $('#<%=txtEndTime2.ClientID %>').val(EndTime2);
        $('#<%=txtEndTime3.ClientID %>').val(EndTime3);
        $('#<%=txtEndTime4.ClientID %>').val(EndTime4);
        $('#<%=txtEndTime5.ClientID %>').val(EndTime5);

        //#region StartTime
        if (StartTime1 != "") {
            $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=txtMaximumAppointment1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList1.ClientID %>').attr("disabled", true);
            $('#<%:chkIsBPJSAppointment1.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJSAppointment1.ClientID %>').attr("disabled", true);
        }

        if (StartTime2 != "") {
            $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsBPJSAppointment2.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJSAppointment2.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList2.ClientID %>').attr("disabled", true);
            $('#<%:chkIsBPJSAppointment2.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJSAppointment2.ClientID %>').attr("disabled", true);
        }

        if (StartTime3 != "") {
            $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsBPJSAppointment3.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJSAppointment3.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList3.ClientID %>').attr("disabled", true);
            $('#<%:chkIsBPJSAppointment3.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJSAppointment3.ClientID %>').attr("disabled", true);
        }

        if (StartTime4 != "") {
            $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList4.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsBPJSAppointment4.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJSAppointment4.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList4.ClientID %>').attr("disabled", true);
            $('#<%:chkIsBPJSAppointment4.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJSAppointment4.ClientID %>').attr("disabled", true);
        }

        if (StartTime5 != "") {
            $('#<%=txtMaximumAppointment5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileAppointment5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList5.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsBPJSAppointment5.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJSAppointment5.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
            $('#<%:chkIsBPJSAppointment5.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJSAppointment5.ClientID %>').attr("disabled", true);
        }
        //#endregion

        //#region Maximum Appointment
        if (MaximumAppointment1 != '0') {
            $('#<%=txtMaximumAppointment1.ClientID %>').val(MaximumAppointment1);
            $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
        }

        if (MaximumAppointment2 != '0') {
            $('#<%=txtMaximumAppointment2.ClientID %>').val(MaximumAppointment2);
            $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
        }

        if (MaximumAppointment3 != '0') {
            $('#<%=txtMaximumAppointment3.ClientID %>').val(MaximumAppointment3);
            $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
        }

        if (MaximumAppointment4 != '0') {
            $('#<%=txtMaximumAppointment4.ClientID %>').val(MaximumAppointment4);
            $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
        }

        if (MaximumAppointment5 != '0') {
            $('#<%=txtMaximumAppointment5.ClientID %>').val(MaximumAppointment5);
            $('#<%=txtMaximumAppointment5.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
        }
        //#endregion

        //#region Mobile Appointment Percentage
        if (MobileAppointment1 != '0') {
            $('#<%=txtMobileAppointment1.ClientID %>').val(MobileAppointment1);
            $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumAppointment1 * (MobileAppointment1 / 100);
            $('#<%=txtCalculateResult1.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileAppointment1.ClientID %>').val('');
        }
        if (MobileAppointment2 != '0') {
            $('#<%=txtMobileAppointment2.ClientID %>').val(MobileAppointment2);
            $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumAppointment2 * (MobileAppointment2 / 100);
            $('#<%=txtCalculateResult2.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileAppointment2.ClientID %>').val('');
        }
        if (MobileAppointment3 != '0') {
            $('#<%=txtMobileAppointment3.ClientID %>').val(MobileAppointment3);
            $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumAppointment3 * (MobileAppointment3 / 100);
            $('#<%=txtCalculateResult3.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileAppointment3.ClientID %>').val('');
        }
        if (MobileAppointment4 != '0') {
            $('#<%=txtMobileAppointment4.ClientID %>').val(MobileAppointment4);
            $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumAppointment4 * (MobileAppointment4 / 100);
            $('#<%=txtCalculateResult4.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileAppointment4.ClientID %>').val('');
        }
        if (MobileAppointment5 != '0') {
            $('#<%=txtMobileAppointment5.ClientID %>').val(MobileAppointment5);
            $('#<%=txtMobileAppointment5.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumAppointment5 * (MobileAppointment5 / 100);
            $('#<%=txtCalculateResult5.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileAppointment5.ClientID %>').val('');
        }
        //#endregion

        //#region Is Allow Waiting List
        if (IsAllowWaitingList1 == 'True') {
            $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', IsAllowWaitingList1);
            $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
        }

        if (IsAllowWaitingList2 == 'True') {
            $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', IsAllowWaitingList2);
            $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
        }

        if (IsAllowWaitingList3 == 'True') {
            $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', IsAllowWaitingList3);
            $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');

        }

        if (IsAllowWaitingList4 == 'True') {
            $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', IsAllowWaitingList4);
            $('#<%:chkIsAllowWaitingList4.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');

        }

        if (IsAllowWaitingList5 == 'True') {
            $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', IsAllowWaitingList5);
            $('#<%:chkIsAllowWaitingList5.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList5.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
        }
        //#endregion

        //#region Maximum Waiting List
        if (MaximumWaitingList1 != '0') {
            $('#<%=txtMaximumWaitingList1.ClientID %>').val(MaximumWaitingList1);
            $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
        }

        if (MaximumWaitingList2 != '0') {
            $('#<%=txtMaximumWaitingList2.ClientID %>').val(MaximumWaitingList2);
            $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
        }

        if (MaximumWaitingList3 != '0') {
            $('#<%=txtMaximumWaitingList3.ClientID %>').val(MaximumWaitingList3);
            $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
        }

        if (MaximumWaitingList4 != '0') {
            $('#<%=txtMaximumWaitingList4.ClientID %>').val(MaximumWaitingList4);
            $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
        }

        if (MaximumWaitingList5 != '0') {
            $('#<%=txtMaximumWaitingList5.ClientID %>').val(MaximumWaitingList5);
            $('#<%=txtMaximumWaitingList5.ClientID %>').removeAttr('readonly', 'readonly');
        } else {
            $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
        }
        //#endregion

        //#region Mobile Appointment Percentage
        if (MobileWaitingPercentage1 != '0') {
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').val(MobileWaitingPercentage1);
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumWaitingList1 * (MobileWaitingPercentage1 / 100);
            $('#<%=txtCalculateResultWaiting1.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
        }
        if (MobileWaitingPercentage2 != '0') {
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').val(MobileWaitingPercentage2);
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumWaitingList2 * (MobileWaitingPercentage2 / 100);
            $('#<%=txtCalculateResultWaiting2.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
        }
        if (MobileWaitingPercentage3 != '0') {
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').val(MobileWaitingPercentage3);
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumWaitingList3 * (MobileWaitingPercentage3 / 100);
            $('#<%=txtCalculateResultWaiting3.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
        }
        if (MobileWaitingPercentage4 != '0') {
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').val(MobileWaitingPercentage4);
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumWaitingList4 * (MobileWaitingPercentage4 / 100);
            $('#<%=txtCalculateResultWaiting4.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
        }
        if (MobileWaitingPercentage5 != '0') {
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').val(MobileWaitingPercentage5);
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').removeAttr('readonly', 'readonly');
            var calculate = MaximumWaitingList5 * (MobileWaitingPercentage5 / 100);
            $('#<%=txtCalculateResultWaiting5.ClientID %>').val(calculate);
        } else {
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
        }
        //#endregion

        //#region Reserved Queue Start No
        if (ReservedQueueStartNo1 != '0') {
            $('#<%=txtReservedQueueStartNo1.ClientID %>').val(ReservedQueueStartNo1);
        } else {
            $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
        }
        if (ReservedQueueStartNo2 != '0') {
            $('#<%=txtReservedQueueStartNo2.ClientID %>').val(ReservedQueueStartNo2);
        } else {
            $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
        }
        if (ReservedQueueStartNo3 != '0') {
            $('#<%=txtReservedQueueStartNo3.ClientID %>').val(ReservedQueueStartNo3);
        } else {
            $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
        }
        if (ReservedQueueStartNo4 != '0') {
            $('#<%=txtReservedQueueStartNo4.ClientID %>').val(ReservedQueueStartNo4);
        } else {
            $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
        }
        if (ReservedQueueStartNo5 != '0') {
            $('#<%=txtReservedQueueStartNo5.ClientID %>').val(ReservedQueueStartNo5);
        } else {
            $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
        }
        //#endregion

        //#region Reserved Queue End No
        if (ReservedQueueEndNo1 != '0') {
            $('#<%=txtReservedQueueEndNo1.ClientID %>').val(ReservedQueueEndNo1);
        } else {
            $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
        }
        if (ReservedQueueEndNo2 != '0') {
            $('#<%=txtReservedQueueEndNo2.ClientID %>').val(ReservedQueueEndNo2);
        } else {
            $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
        }
        if (ReservedQueueEndNo3 != '0') {
            $('#<%=txtReservedQueueEndNo3.ClientID %>').val(ReservedQueueEndNo3);
        } else {
            $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
        }
        if (ReservedQueueEndNo4 != '0') {
            $('#<%=txtReservedQueueEndNo4.ClientID %>').val(ReservedQueueEndNo4);
        } else {
            $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
        }
        if (ReservedQueueEndNo5 != '0') {
            $('#<%=txtReservedQueueEndNo5.ClientID %>').val(ReservedQueueEndNo5);
        } else {
            $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
        }
        //#endregion

        //#region Is BPJS Appointment
        if (IsBPJSAppointment1 == 'True') {
            $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', IsBPJSAppointment1);
            $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment2 == 'True') {
            $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', IsBPJSAppointment2);
            $('#<%:chkIsBPJSAppointment2.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment3 == 'True') {
            $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', IsBPJSAppointment3);
            $('#<%:chkIsBPJSAppointment3.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment4 == 'True') {
            $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', IsBPJSAppointment4);
            $('#<%:chkIsBPJSAppointment4.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment5 == 'True') {
            $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', IsBPJSAppointment5);
            $('#<%:chkIsBPJSAppointment5.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);
        }
        //#endregion

        //#region Is Non BPJS Appointment
        if (IsNonBPJSAppointment1 == 'True') {
            $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', IsNonBPJSAppointment1);
            $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment2 == 'True') {
            $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', IsNonBPJSAppointment2);
            $('#<%:chkIsNonBPJSAppointment2.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment3 == 'True') {
            $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', IsNonBPJSAppointment3);
            $('#<%:chkIsNonBPJSAppointment3.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment4 == 'True') {
            $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', IsNonBPJSAppointment4);
            $('#<%:chkIsNonBPJSAppointment4.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment5 == 'True') {
            $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', IsNonBPJSAppointment5);
            $('#<%:chkIsNonBPJSAppointment5.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);
        }
        //#endregion

        $('#containerPopupEntryData').show();

        getCalculateResult();
        getCalculateResultWaitingList();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                $('#containerPopupEntryData').hide();
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        $('#containerImgLoadingView').hide();
        hideLoadingPanel();
    }

    //#region Operational Time
    $('#lblOperationalTime.lblLink').click(function () {
        openSearchDialog('operationaltime', 'IsDeleted = 0', function (value) {
            $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val(value);
            onTtxtOperationalTimeCodeCtlChanged(value);
            getCalculateResult();
            getCalculateResultWaitingList();
        });
    });

    $('#<%=txtOperationalTimeCodeCtl.ClientID %>').change(function () {
        onTtxtOperationalTimeCodeCtlChangedValue($(this).val());
    });

    function onTtxtOperationalTimeCodeCtlChanged(value) {
        var filterExpression = "IsDeleted = 0 AND OperationalTimeID ='" + value + "'";
        Methods.getObject('GetOperationalTimeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOperationalTimeID.ClientID %>').val(result.OperationalTimeID);
                $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val(result.OperationalTimeCode);
                $('#<%=txtOperationalTimeNameCtl.ClientID %>').val(result.OperationalTimeName);

                var StartTime1 = result.StartTime1;
                var StartTime2 = result.StartTime2;
                var StartTime3 = result.StartTime3;
                var StartTime4 = result.StartTime4;
                var StartTime5 = result.StartTime5;
                var EndTime1 = result.EndTime1;
                var EndTime2 = result.EndTime2;
                var EndTime3 = result.EndTime3;
                var EndTime4 = result.EndTime4;
                var EndTime5 = result.EndTime5;

                $('#<%=txtStartTime1.ClientID %>').val(StartTime1);
                $('#<%=txtStartTime2.ClientID %>').val(StartTime2);
                $('#<%=txtStartTime3.ClientID %>').val(StartTime3);
                $('#<%=txtStartTime4.ClientID %>').val(StartTime4);
                $('#<%=txtStartTime5.ClientID %>').val(StartTime5);

                $('#<%=txtEndTime1.ClientID %>').val(EndTime1);
                $('#<%=txtEndTime2.ClientID %>').val(EndTime2);
                $('#<%=txtEndTime3.ClientID %>').val(EndTime3);
                $('#<%=txtEndTime4.ClientID %>').val(EndTime4);
                $('#<%=txtEndTime5.ClientID %>').val(EndTime5);

                if (StartTime1 != "" && StartTime2 == "" && StartTime3 == "" && StartTime4 == "" && StartTime5 == "") {
                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);

                    $('#<%:chkIsAllowWaitingList2.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);

                    $('#<%:chkIsBPJSAppointment2.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment2.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);
                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 == "" && StartTime4 == "" && StartTime5 == "") {
                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);

                    $('#<%:chkIsAllowWaitingList3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);

                    $('#<%:chkIsBPJSAppointment3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);
                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 != "" && StartTime4 == "" && StartTime5 == "") {
                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);

                    $('#<%:chkIsAllowWaitingList4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment3.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);

                    $('#<%:chkIsBPJSAppointment4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment3.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);
                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 != "" && StartTime4 != "" && StartTime5 == "") {
                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList4.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);

                    $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment3.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment4.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);

                    $('#<%:chkIsBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment3.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment4.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);
                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 != "" && StartTime4 != "" && StartTime5 != "") {
                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList4.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsAllowWaitingList5.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage1.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').val('');
                    $('#<%=txtMobileWaitingPercentage5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').removeAttr('readonly', 'readonly');

                    $('#<%:chkIsBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment3.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment4.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJSAppointment5.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsBPJSAppointment5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJSAppointment1.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsNonBPJSAppointment1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment2.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment3.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment4.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJSAppointment5.ClientID %>').removeAttr("disabled", true);
                    $('#<%=chkIsNonBPJSAppointment5.ClientID %>').prop('checked', false);
                }
            }
            else {
                $('#<%=hdnOperationalTimeID.ClientID %>').val('');
                $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val('');
                $('#<%=txtOperationalTimeNameCtl.ClientID %>').val('');
            }
        });
    }

    function onTtxtOperationalTimeCodeCtlChangedValue(value) {
        var filterExpression = "IsDeleted = 0 AND OperationalTimeCode ='" + value + "'";
        Methods.getObject('GetOperationalTimeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOperationalTimeID.ClientID %>').val(result.OperationalTimeID);
                $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val(result.OperationalTimeCode);
                $('#<%=txtOperationalTimeNameCtl.ClientID %>').val(result.OperationalTimeName);
            }
            else {
                $('#<%=hdnOperationalTimeID.ClientID %>').val('');
                $('#<%=txtOperationalTimeCodeCtl.ClientID %>').val('');
                $('#<%=txtOperationalTimeNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Room
    function getRoomFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = '';
        if (serviceUnitID != '') {
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
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
        Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
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
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion

    //#region calculate (Maximum Appointment * Percentage)
    function getCalculateResult() {
        var result = '';
        var finalResult = '';

        //#region calculate for 1st session
        $(function () {
            $('#<%=txtMobileAppointment1.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileAppointment1.ClientID %>').val() == 0 || $('#<%=txtMobileAppointment1.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumAppointment1.ClientID %>').val();
                        return $('#<%=txtCalculateResult1.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumAppointment1.ClientID %>').val() * ($('#<%=txtMobileAppointment1.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResult1.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 2nd session
        $(function () {
            $('#<%=txtMobileAppointment2.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileAppointment2.ClientID %>').val() == 0 || $('#<%=txtMobileAppointment2.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumAppointment2.ClientID %>').val();
                        return $('#<%=txtCalculateResult2.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumAppointment2.ClientID %>').val() * ($('#<%=txtMobileAppointment2.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResult2.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 3rd session
        $(function () {
            $('#<%=txtMobileAppointment3.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileAppointment3.ClientID %>').val() == 0 || $('#<%=txtMobileAppointment3.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumAppointment3.ClientID %>').val();
                        return $('#<%=txtCalculateResult3.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumAppointment3.ClientID %>').val() * ($('#<%=txtMobileAppointment3.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResult3.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 4th session
        $(function () {
            $('#<%=txtMobileAppointment4.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileAppointment4.ClientID %>').val() == 0 || $('#<%=txtMobileAppointment4.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumAppointment4.ClientID %>').val();
                        return $('#<%=txtCalculateResult4.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumAppointment4.ClientID %>').val() * ($('#<%=txtMobileAppointment4.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResult4.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 5th session
        $(function () {
            $('#<%=txtMobileAppointment5.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileAppointment5.ClientID %>').val() == 0 || $('#<%=txtMobileAppointment5.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumAppointment5.ClientID %>').val();
                        return $('#<%=txtCalculateResult5.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumAppointment5.ClientID %>').val() * ($('#<%=txtMobileAppointment5.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResult5.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion
    }
    //#endregion

    //#region calculate (Maximum Waiting list * Percentage)
    function getCalculateResultWaitingList() {
        var result = '';
        var finalResult = '';

        //#region calculate for 1st session
        $(function () {
            $('#<%=txtMobileWaitingPercentage1.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileWaitingPercentage1.ClientID %>').val() == 0 || $('#<%=txtMobileWaitingPercentage1.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumWaitingList1.ClientID %>').val();
                        return $('#<%=txtCalculateResultWaiting1.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumWaitingList1.ClientID %>').val() * ($('#<%=txtMobileWaitingPercentage1.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResultWaiting1.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 2nd session
        $(function () {
            $('#<%=txtMobileWaitingPercentage2.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileWaitingPercentage2.ClientID %>').val() == 0 || $('#<%=txtMobileWaitingPercentage2.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumWaitingList2.ClientID %>').val();
                        return $('#<%=txtCalculateResultWaiting2.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumWaitingList2.ClientID %>').val() * ($('#<%=txtMobileWaitingPercentage2.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResultWaiting2.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 3rd session
        $(function () {
            $('#<%=txtMobileWaitingPercentage3.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileWaitingPercentage3.ClientID %>').val() == 0 || $('#<%=txtMobileWaitingPercentage3.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumWaitingList3.ClientID %>').val();
                        return $('#<%=txtCalculateResultWaiting3.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumWaitingList3.ClientID %>').val() * ($('#<%=txtMobileWaitingPercentage3.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResultWaiting3.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 4th session
        $(function () {
            $('#<%=txtMobileWaitingPercentage4.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileWaitingPercentage4.ClientID %>').val() == 0 || $('#<%=txtMobileWaitingPercentage4.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumWaitingList4.ClientID %>').val();
                        return $('#<%=txtCalculateResultWaiting4.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumWaitingList4.ClientID %>').val() * ($('#<%=txtMobileWaitingPercentage4.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResultWaiting4.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion

        //#region calculate for 5th session
        $(function () {
            $('#<%=txtMobileWaitingPercentage5.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13) {
                    if ($('#<%=txtMobileWaitingPercentage5.ClientID %>').val() == 0 || $('#<%=txtMobileWaitingPercentage5.ClientID %>').val() == '') {
                        finalResult = $('#<%=txtMaximumWaitingList5.ClientID %>').val();
                        return $('#<%=txtCalculateResultWaiting5.ClientID %>').val(finalResult);
                    }
                    else {
                        result = $('#<%=txtMaximumWaitingList5.ClientID %>').val() * ($('#<%=txtMobileWaitingPercentage5.ClientID %>').val() / 100);
                        finalResult = Math.round(result);
                        return $('#<%=txtCalculateResultWaiting5.ClientID %>').val(finalResult);
                    }
                };
            });
        });
        //#endregion
    }
    //#endregion

</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnRoomID" value="" runat="server" />
    <input type="hidden" id="hdnOperationalTimeID" value="" runat="server" />
    <input type="hidden" id="hdnDatePickerToday" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="" runat="server" />
    <input type="hidden" id="hdnCheckAppointmentBeforeChangeSchedule" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 1px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Klinik")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnDayNumber" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Hari")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" Width="108px" ID="cboDay" ClientInstanceName="cboDay" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" runat="server" id="lblRoom">
                                        <%:GetLabel("Kamar")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 108px" />
                                            <col style="width: 10px" />
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
                                                <asp:TextBox ID="txtRoomName" Width="50%" runat="server" disabled />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblOperationalTime">
                                        <%=GetLabel("Operational Time")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 108px" />
                                            <col style="width: 10px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtOperationalTimeCodeCtl" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOperationalTimeNameCtl" Width="80%" disabled runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblnormal" id="lblRemarks">
                                        <%=GetLabel("Keterangan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" Width="100%" TextMode="MultiLine" Rows="4" runat="server" />
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("By Time Slot")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsAppointmentByTimeSlot" Width="100px" Checked="false" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Mulai")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal" >
                                                    <%=GetLabel("Akhir")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Max Appointment")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Appointment Mobile App (%)")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Appointment Kuota Akhir")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Waiting List")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Maximum Waiting")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Waiting Mobile App (%)")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Waiting Kuota Akhir")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Awal Antrian")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Akhir Antrian")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("BPJS")%></label>
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Non-BPJS")%></label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Session #1")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartTime1" CssClass="time start" Width="80px" ReadOnly="true"
                                                    runat="server" MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndTime1" CssClass="time end" Width="80px" ReadOnly="true" runat="server"
                                                    MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumAppointment1" CssClass="maximum appointment" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileAppointment1" CssClass="mobile app percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResult1" CssClass="calculate result" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsAllowWaitingList1" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumWaitingList1" CssClass="maximum waiting" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileWaitingPercentage1" CssClass="mobile app waiting percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResultWaiting1" CssClass="calculate result waiting" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueStartNo1" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueEndNo1" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsBPJSAppointment1" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsNonBPJSAppointment1" Width="80px" Checked="false" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Session #2")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartTime2" CssClass="time start" Width="80px" ReadOnly="true"
                                                    runat="server" MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndTime2" CssClass="time end" Width="80px" ReadOnly="true" runat="server"
                                                    MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumAppointment2" CssClass="maximum appointment" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileAppointment2" CssClass="mobile app percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResult2" CssClass="calculate result" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsAllowWaitingList2" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumWaitingList2" CssClass="maximum waiting" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileWaitingPercentage2" CssClass="mobile app waiting percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResultWaiting2" CssClass="calculate result waiting" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueStartNo2" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueEndNo2" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsBPJSAppointment2" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsNonBPJSAppointment2" Width="80px" Checked="false" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Session #3")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartTime3" CssClass="time start" Width="80px" ReadOnly="true"
                                                    runat="server" MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndTime3" CssClass="time end" Width="80px" ReadOnly="true" runat="server"
                                                    MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumAppointment3" CssClass="maximum appointment" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileAppointment3" CssClass="mobile app percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResult3" CssClass="calculate result" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsAllowWaitingList3" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumWaitingList3" CssClass="maximum waiting" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileWaitingPercentage3" CssClass="mobile app waiting percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResultWaiting3" CssClass="calculate result waiting" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueStartNo3" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueEndNo3" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsBPJSAppointment3" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsNonBPJSAppointment3" Width="80px" Checked="false" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Session #4")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartTime4" CssClass="time start" Width="80px" ReadOnly="true"
                                                    runat="server" MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndTime4" CssClass="time end" Width="80px" ReadOnly="true" runat="server"
                                                    MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumAppointment4" CssClass="maximum appointment" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileAppointment4" CssClass="mobile app percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResult4" CssClass="calculate result" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsAllowWaitingList4" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumWaitingList4" CssClass="maximum waiting" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileWaitingPercentage4" CssClass="mobile app waiting percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResultWaiting4" CssClass="calculate result waiting" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueStartNo4" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueEndNo4" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsBPJSAppointment4" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsNonBPJSAppointment4" Width="80px" Checked="false" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Session #5")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                            <col style="width: 7.7%" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartTime5" CssClass="time start" Width="80px" ReadOnly="true"
                                                    runat="server" MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndTime5" CssClass="time end" Width="80px" ReadOnly="true" runat="server"
                                                    MaxLength="5" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumAppointment5" CssClass="maximum appointment" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileAppointment5" CssClass="mobile app percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResult5" CssClass="calculate result" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsAllowWaitingList5" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMaximumWaitingList5" CssClass="maximum waiting" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMobileWaitingPercentage5" CssClass="mobile app waiting percentage" Width="80px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCalculateResultWaiting5" CssClass="calculate result waiting" Width="80px"
                                                    runat="server" ReadOnly="true" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueStartNo5" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtReservedQueueEndNo5" CssClass="queue start no" Width="50px"
                                                    runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsBPJSAppointment5" Width="80px" Checked="false" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsNonBPJSAppointment5" Width="80px" Checked="false" runat="server" />
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
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
<%--                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; position: relative;
                                font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 10px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnDayNumber" value="<%#: Eval("DayNumber")%>" />
                                                <input type="hidden" class="hdnRoomIDEntry" value="<%#: Eval("RoomID")%>" />
                                                <input type="hidden" class="hdnRoomCodeEntry" value="<%#: Eval("RoomCode")%>" />
                                                <input type="hidden" class="hdnRoomNameEntry" value="<%#: Eval("RoomName")%>" />
                                                <input type="hidden" class="hdnParamedicScheduleRemarks" value="<%#: Eval("Remarks")%>" />
                                                <input type="hidden" class="hdnOperationalTimeIDEntry" value="<%#: Eval("OperationalTimeID")%>" />
                                                <input type="hidden" class="hdnOperationalTimeCodeEntry" value="<%#: Eval("OperationalTimeCode")%>" />
                                                <input type="hidden" class="tdStartTime1" value="<%#: Eval("StartTime1")%>" />
                                                <input type="hidden" class="tdStartTime2" value="<%#: Eval("StartTime2")%>" />
                                                <input type="hidden" class="tdStartTime3" value="<%#: Eval("StartTime3")%>" />
                                                <input type="hidden" class="tdStartTime4" value="<%#: Eval("StartTime4")%>" />
                                                <input type="hidden" class="tdStartTime5" value="<%#: Eval("StartTime5")%>" />
                                                <input type="hidden" class="tdEndTime1" value="<%#: Eval("EndTime1")%>" />
                                                <input type="hidden" class="tdEndTime2" value="<%#: Eval("EndTime2")%>" />
                                                <input type="hidden" class="tdEndTime3" value="<%#: Eval("EndTime3")%>" />
                                                <input type="hidden" class="tdEndTime4" value="<%#: Eval("EndTime4")%>" />
                                                <input type="hidden" class="tdEndTime5" value="<%#: Eval("EndTime5")%>" />
                                                <input type="hidden" class="tdIsAppointmentByTimeSlot1" value="<%#: Eval("IsAppointmentByTimeSlot1")%>" />
                                                <input type="hidden" class="tdIsAppointmentByTimeSlot2" value="<%#: Eval("IsAppointmentByTimeSlot2")%>" />
                                                <input type="hidden" class="tdIsAppointmentByTimeSlot3" value="<%#: Eval("IsAppointmentByTimeSlot3")%>" />
                                                <input type="hidden" class="tdIsAppointmentByTimeSlot4" value="<%#: Eval("IsAppointmentByTimeSlot4")%>" />
                                                <input type="hidden" class="tdIsAppointmentByTimeSlot5" value="<%#: Eval("IsAppointmentByTimeSlot5")%>" />
                                                <input type="hidden" class="tdMobileAppointment1" value="<%#: Eval("MobileAppointment1")%>" />
                                                <input type="hidden" class="tdMobileAppointment2" value="<%#: Eval("MobileAppointment2")%>" />
                                                <input type="hidden" class="tdMobileAppointment3" value="<%#: Eval("MobileAppointment3")%>" />
                                                <input type="hidden" class="tdMobileAppointment4" value="<%#: Eval("MobileAppointment4")%>" />
                                                <input type="hidden" class="tdMobileAppointment5" value="<%#: Eval("MobileAppointment5")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo1" value="<%#: Eval("ReservedQueueStartNo1")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo2" value="<%#: Eval("ReservedQueueStartNo2")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo3" value="<%#: Eval("ReservedQueueStartNo3")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo4" value="<%#: Eval("ReservedQueueStartNo4")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo5" value="<%#: Eval("ReservedQueueStartNo5")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo1" value="<%#: Eval("ReservedQueueEndNo1")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo2" value="<%#: Eval("ReservedQueueEndNo2")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo3" value="<%#: Eval("ReservedQueueEndNo3")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo4" value="<%#: Eval("ReservedQueueEndNo4")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo5" value="<%#: Eval("ReservedQueueEndNo5")%>" />
                                                <input type="hidden" class="tdMaximumAppointment1" value="<%#: Eval("MaximumAppointment1")%>" />
                                                <input type="hidden" class="tdMaximumAppointment2" value="<%#: Eval("MaximumAppointment2")%>" />
                                                <input type="hidden" class="tdMaximumAppointment3" value="<%#: Eval("MaximumAppointment3")%>" />
                                                <input type="hidden" class="tdMaximumAppointment4" value="<%#: Eval("MaximumAppointment4")%>" />
                                                <input type="hidden" class="tdMaximumAppointment5" value="<%#: Eval("MaximumAppointment5")%>" />
                                                <input type="hidden" class="tdIsAllowWaitingList1" value="<%#: Eval("IsAllowWaitingList1")%>" />
                                                <input type="hidden" class="tdIsAllowWaitingList2" value="<%#: Eval("IsAllowWaitingList2")%>" />
                                                <input type="hidden" class="tdIsAllowWaitingList3" value="<%#: Eval("IsAllowWaitingList3")%>" />
                                                <input type="hidden" class="tdIsAllowWaitingList4" value="<%#: Eval("IsAllowWaitingList4")%>" />
                                                <input type="hidden" class="tdIsAllowWaitingList5" value="<%#: Eval("IsAllowWaitingList5")%>" />
                                                <input type="hidden" class="tdMaximumWaitingList1" value="<%#: Eval("MaximumWaitingList1")%>" />
                                                <input type="hidden" class="tdMaximumWaitingList2" value="<%#: Eval("MaximumWaitingList2")%>" />
                                                <input type="hidden" class="tdMaximumWaitingList3" value="<%#: Eval("MaximumWaitingList3")%>" />
                                                <input type="hidden" class="tdMaximumWaitingList4" value="<%#: Eval("MaximumWaitingList4")%>" />
                                                <input type="hidden" class="tdMaximumWaitingList5" value="<%#: Eval("MaximumWaitingList5")%>" />
                                                <input type="hidden" class="tdMobileWaitingPercentage1" value="<%#: Eval("MobileWaitingList1")%>" />
                                                <input type="hidden" class="tdMobileWaitingPercentage2" value="<%#: Eval("MobileWaitingList2")%>" />
                                                <input type="hidden" class="tdMobileWaitingPercentage3" value="<%#: Eval("MobileWaitingList3")%>" />
                                                <input type="hidden" class="tdMobileWaitingPercentage4" value="<%#: Eval("MobileWaitingList4")%>" />
                                                <input type="hidden" class="tdMobileWaitingPercentage5" value="<%#: Eval("MobileWaitingList5")%>" />
                                                <input type="hidden" class="tdIsBPJSAppointment1" value="<%#: Eval("IsBPJS1")%>" />
                                                <input type="hidden" class="tdIsBPJSAppointment2" value="<%#: Eval("IsBPJS2")%>" />
                                                <input type="hidden" class="tdIsBPJSAppointment3" value="<%#: Eval("IsBPJS3")%>" />
                                                <input type="hidden" class="tdIsBPJSAppointment4" value="<%#: Eval("IsBPJS4")%>" />
                                                <input type="hidden" class="tdIsBPJSAppointment5" value="<%#: Eval("IsBPJS5")%>" />
                                                <input type="hidden" class="tdIsNonBPJSAppointment1" value="<%#: Eval("IsNonBPJS1")%>" />
                                                <input type="hidden" class="tdIsNonBPJSAppointment2" value="<%#: Eval("IsNonBPJS2")%>" />
                                                <input type="hidden" class="tdIsNonBPJSAppointment3" value="<%#: Eval("IsNonBPJS3")%>" />
                                                <input type="hidden" class="tdIsNonBPJSAppointment4" value="<%#: Eval("IsNonBPJS4")%>" />
                                                <input type="hidden" class="tdIsNonBPJSAppointment5" value="<%#: Eval("IsNonBPJS5")%>" />
                                            </ItemTemplate>

<HeaderStyle Width="50px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="80px" DataField="cfDayName1" HeaderText="Hari"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="RoomName" ItemStyle-CssClass="tdRoomName"
                                            HeaderText="Kamar" HeaderStyle-HorizontalAlign="Center" 
                                            ItemStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" CssClass="tdRoomName"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="250px" DataField="OperationalTimeName" ItemStyle-CssClass="tdOperationalTimeName"
                                            HeaderText="Operational Time" HeaderStyle-HorizontalAlign="Center" 
                                            ItemStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" CssClass="tdOperationalTimeName"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>

<EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="Div1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddDataScheduleDay">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
