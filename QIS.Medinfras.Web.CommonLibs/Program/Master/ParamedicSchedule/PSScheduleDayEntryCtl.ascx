<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PSScheduleDayEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PSScheduleDayEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PSScheduleWithDateEntryCtl">
    $('#lblEntryPopupAddDataScheduleDay').live('click', function () {
        $('#<%=hdnIsChangeDifferentQueueNo.ClientID %>').val("0");
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

        $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
        $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", true);
        $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);

        $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
        $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", true);
        $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);

        $('#<%=txtVisitDuration1.ClientID %>').val('');
        $('#<%=txtVisitDuration1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtVisitDuration2.ClientID %>').val('');
        $('#<%=txtVisitDuration2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtVisitDuration3.ClientID %>').val('');
        $('#<%=txtVisitDuration3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtVisitDuration4.ClientID %>').val('');
        $('#<%=txtVisitDuration4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtVisitDuration5.ClientID %>').val('');
        $('#<%=txtVisitDuration5.ClientID %>').attr('readonly', 'readonly');


        $('#<%=txtMaximumAppointment1.ClientID %>').val('');
        $('#<%=txtMaximumAppointment1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment2.ClientID %>').val('');
        $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment3.ClientID %>').val('');
        $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment4.ClientID %>').val('');
        $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment5.ClientID %>').val('');
        $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('');
        $('#<%=txtMaximumAppointment1BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('');
        $('#<%=txtMaximumAppointment2BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('');
        $('#<%=txtMaximumAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('');
        $('#<%=txtMaximumAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('');
        $('#<%=txtMaximumAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtMobileAppointment1.ClientID %>').val('');
        $('#<%=txtMobileAppointment1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('');
        $('#<%=txtMobileAppointment1BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment2.ClientID %>').val('');
        $('#<%=txtMobileAppointment2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('');
        $('#<%=txtMobileAppointment2BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment3.ClientID %>').val('');
        $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('');
        $('#<%=txtMobileAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment4.ClientID %>').val('');
        $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('');
        $('#<%=txtMobileAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment5.ClientID %>').val('');
        $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('');
        $('#<%=txtMobileAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
        $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').attr('readonly', 'readonly');


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

        $('#<%=txtMaximumWaitingList1.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList2.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList3.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList4.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
        $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtWaiting1.ClientID %>').val('');
        $('#<%=txtWaiting1.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting1BPJS.ClientID %>').val('');
        $('#<%=txtWaiting1BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting2.ClientID %>').val('');
        $('#<%=txtWaiting2.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting2BPJS.ClientID %>').val('');
        $('#<%=txtWaiting2BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting3.ClientID %>').val('');
        $('#<%=txtWaiting3.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting3BPJS.ClientID %>').val('');
        $('#<%=txtWaiting3BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting4.ClientID %>').val('');
        $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting4BPJS.ClientID %>').val('');
        $('#<%=txtWaiting4BPJS.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting5.ClientID %>').val('');
        $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtWaiting5BPJS.ClientID %>').val('');
        $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
    
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    //#region Function CheckBox
    $('#<%=chkIsAllowWaitingList1.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList1.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
            $('#<%=txtWaiting1.ClientID %>').val('0');
            $('#<%=txtWaiting1BPJS.ClientID %>').val('0');
        } else {
            $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList1.ClientID %>').val('');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('');
            $('#<%=txtWaiting1.ClientID %>').val('');
            $('#<%=txtWaiting1BPJS.ClientID %>').val('');

        }
    });

    $('#<%=chkIsAllowWaitingList2.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList2.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting2.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
            $('#<%=txtWaiting1.ClientID %>').val('0');
            $('#<%=txtWaiting1BPJS.ClientID %>').val('0');

        } else {
            $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList2.ClientID %>').val('');
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('');
            $('#<%=txtWaiting2.ClientID %>').val('');
            $('#<%=txtWaiting2BPJS.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAllowWaitingList3.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList3.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting3.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('0');
            $('#<%=txtWaiting3.ClientID %>').val('0');
            $('#<%=txtWaiting3BPJS.ClientID %>').val('0');
        } else {
            $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList3.ClientID %>').val('');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('');
            $('#<%=txtWaiting3.ClientID %>').val('');
            $('#<%=txtWaiting3BPJS.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAppointmentByTimeSlot.ClientID %>').change(function () {
        if ($('#<%=chkIsAppointmentByTimeSlot.ClientID %>').is(':checked')) {
            if ($('#<%=hdnTotalSession.ClientID %>').val() == "1") {
                $('#<%=txtVisitDuration1.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('0');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "2") {
                $('#<%=txtVisitDuration1.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration2.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('0');
                $('#<%=txtVisitDuration2.ClientID %>').val('0');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "3") {
                $('#<%=txtVisitDuration1.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration2.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration3.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('0');
                $('#<%=txtVisitDuration2.ClientID %>').val('0');
                $('#<%=txtVisitDuration3.ClientID %>').val('0');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "4") {
                $('#<%=txtVisitDuration1.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration2.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration3.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration4.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('0');
                $('#<%=txtVisitDuration2.ClientID %>').val('0');
                $('#<%=txtVisitDuration3.ClientID %>').val('0');
                $('#<%=txtVisitDuration4.ClientID %>').val('0');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "5") {
                $('#<%=txtVisitDuration1.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration2.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration3.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration4.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration5.ClientID %>').removeAttr('readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('0');
                $('#<%=txtVisitDuration2.ClientID %>').val('0');
                $('#<%=txtVisitDuration3.ClientID %>').val('0');
                $('#<%=txtVisitDuration4.ClientID %>').val('0');
                $('#<%=txtVisitDuration5.ClientID %>').val('0');   
            }
        } else {
            if ($('#<%=hdnTotalSession.ClientID %>').val() == "1") {
                
                $('#<%=txtVisitDuration1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "2") {
                
                $('#<%=txtVisitDuration1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('');
                $('#<%=txtVisitDuration2.ClientID %>').val('');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "3") {
                
                $('#<%=txtVisitDuration1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration3.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('');
                $('#<%=txtVisitDuration2.ClientID %>').val('');
                $('#<%=txtVisitDuration3.ClientID %>').val('');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "4") {
                
                $('#<%=txtVisitDuration1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration3.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration4.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('');
                $('#<%=txtVisitDuration2.ClientID %>').val('');
                $('#<%=txtVisitDuration3.ClientID %>').val('');
                $('#<%=txtVisitDuration4.ClientID %>').val('');
            }
            else if ($('#<%=hdnTotalSession.ClientID %>').val() == "5") {
                
                $('#<%=txtVisitDuration1.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration2.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration3.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration4.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration5.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtVisitDuration1.ClientID %>').val('');
                $('#<%=txtVisitDuration2.ClientID %>').val('');
                $('#<%=txtVisitDuration3.ClientID %>').val('');
                $('#<%=txtVisitDuration4.ClientID %>').val('');
                $('#<%=txtVisitDuration5.ClientID %>').val(''); 
            }
        }
    });

    $('#<%=chkIsAllowWaitingList4.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList4.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting4.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('0');
            $('#<%=txtWaiting4.ClientID %>').val('0');
            $('#<%=txtWaiting4BPJS.ClientID %>').val('0');
        } else {
            $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList3.ClientID %>').val('');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('');
            $('#<%=txtWaiting3.ClientID %>').val('');
            $('#<%=txtWaiting3BPJS.ClientID %>').val('');
        }
    });

    $('#<%=chkIsAllowWaitingList5.ClientID %>').change(function () {
        if ($('#<%=chkIsAllowWaitingList5.ClientID %>').is(':checked')) {
            $('#<%=txtMaximumWaitingList5.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting5.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting5BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('0');
            $('#<%=txtWaiting5.ClientID %>').val('0');
            $('#<%=txtWaiting5BPJS.ClientID %>').val('0');
        } else {
            $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly');
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly');
            $('#<%=txtWaiting5.ClientID %>').attr('readonly');
            $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
            $('#<%=txtWaiting5.ClientID %>').val('');
            $('#<%=txtWaiting5BPJS.ClientID %>').val('');
        }
    });

    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');

            var ParamedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            var HealthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var OperationalTimeIDEntry = $row.find('.hdnOperationalTimeIDEntry').val();
            var ScheduleDay = $row.find('.hdnDayNumber').val();
            $('#<%=hdnDayNumber.ClientID %>').val(ScheduleDay);
            $('#<%=hdnOperationalTimeID.ClientID %>').val(OperationalTimeIDEntry);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('#<%=chkIsAllowDifferentQueueNo.ClientID %>').change(function () {
        $('#<%=hdnIsChangeDifferentQueueNo.ClientID %>').val("1");
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

        $('#<%=hdnRoomID.ClientID %>').val(RoomIDEntry);

        $('#<%=hdnOperationalTime.ClientID %>').val("0");
        $('#<%=hdnRoomIDOld.ClientID %>').val(RoomIDEntry);
        $('#<%=hdnRoomIDNew.ClientID %>').val(RoomIDEntry);

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

        var MaximumAppointment1 = $row.find('.tdMaximumAppointment1').val();
        var MaximumAppointment2 = $row.find('.tdMaximumAppointment2').val();
        var MaximumAppointment3 = $row.find('.tdMaximumAppointment3').val();
        var MaximumAppointment4 = $row.find('.tdMaximumAppointment4').val();
        var MaximumAppointment5 = $row.find('.tdMaximumAppointment5').val();

        var MaximumAppointmentBPJS1 = $row.find('.tdMaximumAppointmentBPJS1').val();
        var MaximumAppointmentBPJS2 = $row.find('.tdMaximumAppointmentBPJS2').val();
        var MaximumAppointmentBPJS3 = $row.find('.tdMaximumAppointmentBPJS3').val();
        var MaximumAppointmentBPJS4 = $row.find('.tdMaximumAppointmentBPJS4').val();
        var MaximumAppointmentBPJS5 = $row.find('.tdMaximumAppointmentBPJS5').val();

        var MobileAppointment1 = $row.find('.tdMobileAppointment1').val();
        var MobileAppointment2 = $row.find('.tdMobileAppointment2').val();
        var MobileAppointment3 = $row.find('.tdMobileAppointment3').val();
        var MobileAppointment4 = $row.find('.tdMobileAppointment4').val();
        var MobileAppointment5 = $row.find('.tdMobileAppointment5').val();

        var MobileAppointment1BPJS = $row.find('.tdMobileAppointment1BPJS').val();
        var MobileAppointment2BPJS = $row.find('.tdMobileAppointment2BPJS').val();
        var MobileAppointment3BPJS = $row.find('.tdMobileAppointment3BPJS').val();
        var MobileAppointment4BPJS = $row.find('.tdMobileAppointment4BPJS').val();
        var MobileAppointment5BPJS = $row.find('.tdMobileAppointment5BPJS').val();

        var ReservedQueueStartNo1 = $row.find('.tdReservedQueueStartNo1').val();
        var ReservedQueueStartNo2 = $row.find('.tdReservedQueueStartNo2').val();
        var ReservedQueueStartNo3 = $row.find('.tdReservedQueueStartNo3').val();
        var ReservedQueueStartNo4 = $row.find('.tdReservedQueueStartNo4').val();
        var ReservedQueueStartNo5 = $row.find('.tdReservedQueueStartNo5').val();

        var ReservedQueueEndNo1 = $row.find('.tdReservedQueueEndNo1').val();
        var ReservedQueueEndNo2 = $row.find('.tdReservedQueueEndNo2').val();
        var ReservedQueueEndNo3 = $row.find('.tdReservedQueueEndNo3').val();
        var ReservedQueueEndNo4 = $row.find('.tdReservedQueueEndNo4').val();

        var ReservedQueueStartNoBPJS1 = $row.find('.tdReservedQueueStartNoBPJS1').val();
        var ReservedQueueStartNoBPJS2 = $row.find('.tdReservedQueueStartNoBPJS2').val();
        var ReservedQueueStartNoBPJS3 = $row.find('.tdReservedQueueStartNoBPJS3').val();
        var ReservedQueueStartNoBPJS4 = $row.find('.tdReservedQueueStartNoBPJS4').val();
        var ReservedQueueStartNoBPJS5 = $row.find('.tdReservedQueueStartNoBPJS5').val();
        var ReservedQueueEndNo5 = $row.find('.tdReservedQueueEndNo5').val();

        var ReservedQueueEndNoBPJS1 = $row.find('.tdReservedQueueEndNoBPJS1').val();
        var ReservedQueueEndNoBPJS2 = $row.find('.tdReservedQueueEndNoBPJS2').val();
        var ReservedQueueEndNoBPJS3 = $row.find('.tdReservedQueueEndNoBPJS3').val();
        var ReservedQueueEndNoBPJS4 = $row.find('.tdReservedQueueEndNoBPJS4').val();
        var ReservedQueueEndNoBPJS5 = $row.find('.tdReservedQueueEndNoBPJS5').val();

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

        var MaximumWaitingList1BPJS = $row.find('.tdMaximumWaitingListBPJS1').val();
        var MaximumWaitingList2BPJS = $row.find('.tdMaximumWaitingListBPJS2').val();
        var MaximumWaitingList3BPJS = $row.find('.tdMaximumWaitingListBPJS3').val();
        var MaximumWaitingList4BPJS = $row.find('.tdMaximumWaitingListBPJS4').val();
        var MaximumWaitingList5BPJS = $row.find('.tdMaximumWaitingListBPJS5').val();

        var MobileWaiting1 = $row.find('.tdMobileWaiting1').val();
        var MobileWaiting1BPJS = $row.find('.tdMobileWaiting1BPJS').val();
        var MobileWaiting2 = $row.find('.tdMobileWaiting2').val();
        var MobileWaiting2BPJS = $row.find('.tdMobileWaiting2BPJS').val();
        var MobileWaiting3 = $row.find('.tdMobileWaiting3').val();
        var MobileWaiting3BPJS = $row.find('.tdMobileWaiting3BPJS').val();
        var MobileWaiting4 = $row.find('.tdMobileWaiting4').val();
        var MobileWaiting4BPJS = $row.find('.tdMobileWaiting4BPJS').val();
        var MobileWaiting5 = $row.find('.tdMobileWaiting5').val();
        var MobileWaiting5BPJS = $row.find('.tdMobileWaiting5BPJS').val();

        var isAllowDifferentQueueNo = false;
        if ($row.find('.tdIsAllowDifferentQueueNo').val() == "True"){
            isAllowDifferentQueueNo = true;
        }
        $('#<%=hdnIsChangeDifferentQueueNo.ClientID %>').val("0");
        $('#<%=chkIsAllowDifferentQueueNo.ClientID %>').prop('checked', isAllowDifferentQueueNo);

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
            } else {
                $('#<%=txtMaximumAppointment1.ClientID %>').val('');
            }

            if (MaximumAppointmentBPJS1 != '0') {
                $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val(MaximumAppointmentBPJS1);
            } else {
                $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('');
            }

            if (MaximumAppointment2 != '0') {
                $('#<%=txtMaximumAppointment2.ClientID %>').val(MaximumAppointment2);
            } else {
                $('#<%=txtMaximumAppointment2.ClientID %>').val('');
            }

            if (MaximumAppointmentBPJS2 != '0') {
                $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val(MaximumAppointmentBPJS2);
            } else {
                $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('');
            }
            if (MaximumAppointment3 != '0') {
                $('#<%=txtMaximumAppointment3.ClientID %>').val(MaximumAppointment3);
            } else {
                $('#<%=txtMaximumAppointment3.ClientID %>').val('');
            }

            if (MaximumAppointmentBPJS3 != '0') {
                $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val(MaximumAppointmentBPJS3);
            } else {
                $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('');
            }
            if (MaximumAppointment4 != '0') {
                $('#<%=txtMaximumAppointment4.ClientID %>').val(MaximumAppointment4);
            } else {
                $('#<%=txtMaximumAppointment4.ClientID %>').val('');
            }

            if (MaximumAppointmentBPJS4 != '0') {
                $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val(MaximumAppointmentBPJS4);
            } else {
                $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('');
            }
            if (MaximumAppointment5 != '0') {
                $('#<%=txtMaximumAppointment5.ClientID %>').val(MaximumAppointment5);
            } else {
                $('#<%=txtMaximumAppointment5.ClientID %>').val('');
            }

            if (MaximumAppointmentBPJS1 != '0') {
                $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val(MaximumAppointmentBPJS5);
            } else {
                $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('');
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
            $('#<%:chkIsBPJS1.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJS1.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumAppointment1BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment1BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').removeAttr('readonly');
            $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", true);
            $('#<%=txtMaximumAppointment1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumAppointment1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList1.ClientID %>').attr("disabled", true);
        }

        if (StartTime2 != "") {
            $('#<%:chkIsBPJS2.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJS2.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumAppointment2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");

            $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1BPJS.ClientID %>').attr('readonly', 'readonly');

        } else {
            $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", true);
            $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumAppointment2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList2.ClientID %>').attr("disabled", true);
        }

        if (StartTime3 != "") {
            $('#<%:chkIsBPJS3.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJS3.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumAppointment3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting3.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtWaiting3BPJS.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", true);
            $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList3.ClientID %>').attr("disabled", true);
        }

        if (StartTime4 != "") {
            $('#<%:chkIsBPJS4.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJS4.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumAppointment4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo4.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo4.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting4.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtWaiting4BPJS.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList4.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", true);
            $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList4.ClientID %>').attr("disabled", true);
        }

        if (StartTime5 != "") {
            $('#<%:chkIsBPJS5.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsNonBPJS5.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumAppointment5.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumAppointment5BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment5.ClientID %>').removeAttr('readonly');
            $('#<%=txtMobileAppointment5BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo5.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo5.ClientID %>').removeAttr('readonly');
            $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting5.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtWaiting5BPJS.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList5.ClientID %>').removeAttr("disabled");
        } else {
            $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", true);
            $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", true);
            $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMobileAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%:chkIsAllowWaitingList5.ClientID %>').attr("disabled", true);
        }
        //#endregion

        //#region Maximum Appointment
        if (MaximumAppointment1 != '0') {
            $('#<%=txtMaximumAppointment1.ClientID %>').val(MaximumAppointment1);
        } else {
            $('#<%=txtMaximumAppointment1.ClientID %>').val('');
        }

        if (MaximumAppointmentBPJS1 != '0') {
            $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val(MaximumAppointmentBPJS1);
        } else {
            $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('');
        }

        if (MaximumAppointment2 != '0') {
            $('#<%=txtMaximumAppointment2.ClientID %>').val(MaximumAppointment2);
        } else {
            $('#<%=txtMaximumAppointment2.ClientID %>').val('');
        }

        if (MaximumAppointmentBPJS2 != '0') {
            $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val(MaximumAppointmentBPJS2);
        } else {
            $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('');
        }

        if (MaximumAppointment3 != '0') {
            $('#<%=txtMaximumAppointment3.ClientID %>').val(MaximumAppointment3);
        } else {
            $('#<%=txtMaximumAppointment3.ClientID %>').val('');
        }

        if (MaximumAppointmentBPJS3 != '0') {
            $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val(MaximumAppointmentBPJS3);
        } else {
            $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('');
        }

        if (MaximumAppointment4 != '0') {
            $('#<%=txtMaximumAppointment4.ClientID %>').val(MaximumAppointment4);
        } else {
            $('#<%=txtMaximumAppointment4.ClientID %>').val('');
        }

        if (MaximumAppointmentBPJS4 != '0') {
            $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val(MaximumAppointmentBPJS4);
        } else {
            $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('');
        }

        if (MaximumAppointment5 != '0') {
            $('#<%=txtMaximumAppointment5.ClientID %>').val(MaximumAppointment5);
        } else {
            $('#<%=txtMaximumAppointment5.ClientID %>').val('');
        }

        if (MaximumAppointmentBPJS5 != '0') {
            $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val(MaximumAppointmentBPJS5);
        } else {
            $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('');
        }
        //#endregion

        //#region Online Appointment 
        if (MobileAppointment1 != '0') {
            $('#<%=txtMobileAppointment1.ClientID %>').val(MobileAppointment1);
        } else {
            $('#<%=txtMobileAppointment1.ClientID %>').val('');
        }
        if (MobileAppointment1BPJS != '0') {
            $('#<%=txtMobileAppointment1BPJS.ClientID %>').val(MobileAppointment1BPJS);
        } else {
            $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('');
        }

        if (MobileAppointment2 != '0') {
            $('#<%=txtMobileAppointment2.ClientID %>').val(MobileAppointment2);
        } else {
            $('#<%=txtMobileAppointment2.ClientID %>').val('');
        }
        if (MobileAppointment2BPJS != '0') {
            $('#<%=txtMobileAppointment2BPJS.ClientID %>').val(MobileAppointment2BPJS);
        } else {
            $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('');
        }

        if (MobileAppointment3 != '0') {
            $('#<%=txtMobileAppointment3.ClientID %>').val(MobileAppointment3);
        } else {
            $('#<%=txtMobileAppointment3.ClientID %>').val('');
        }
        if (MobileAppointment3BPJS != '0') {
            $('#<%=txtMobileAppointment3BPJS.ClientID %>').val(MobileAppointment3BPJS);
        } else {
            $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('');
        }

        if (MobileAppointment4 != '0') {
            $('#<%=txtMobileAppointment4.ClientID %>').val(MobileAppointment4);
        } else {
            $('#<%=txtMobileAppointment4.ClientID %>').val('');
        }
        if (MobileAppointment4BPJS != '0') {
            $('#<%=txtMobileAppointment4BPJS.ClientID %>').val(MobileAppointment4BPJS);
        } else {
            $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('');
        }
        if (MobileAppointment5 != '0') {
            $('#<%=txtMobileAppointment5.ClientID %>').val(MobileAppointment5);
        } else {
            $('#<%=txtMobileAppointment5.ClientID %>').val('');
        }
        if (MobileAppointment5BPJS != '0') {
            $('#<%=txtMobileAppointment5BPJS.ClientID %>').val(MobileAppointment1BPJS);
        } else {
            $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('');
        }
        //#endregion

        //#region Is Allow Waiting List
        if (IsAllowWaitingList1 == 'True') {
            $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', IsAllowWaitingList1);
            $('#<%:chkIsAllowWaitingList1.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=chkIsAllowWaitingList1.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting1.ClientID %>').val('');
            $('#<%=txtWaiting1BPJS.ClientID %>').val('');
        }

        if (IsAllowWaitingList2 == 'True') {
            $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', IsAllowWaitingList2);
            $('#<%:chkIsAllowWaitingList2.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting2.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting2BPJS.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=chkIsAllowWaitingList2.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting2.ClientID %>').val('');
            $('#<%=txtWaiting2BPJS.ClientID %>').val('');
        }

        if (IsAllowWaitingList3 == 'True') {
            $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', IsAllowWaitingList3);
            $('#<%:chkIsAllowWaitingList3.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting3.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting3BPJS.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=chkIsAllowWaitingList3.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting3.ClientID %>').val('');
            $('#<%=txtWaiting3BPJS.ClientID %>').val('');
        }

        if (IsAllowWaitingList4 == 'True') {
            $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', IsAllowWaitingList4);
            $('#<%:chkIsAllowWaitingList4.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly');
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting4.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting4BPJS.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=chkIsAllowWaitingList4.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting4.ClientID %>').val('');
            $('#<%=txtWaiting4BPJS.ClientID %>').val('');

        }

        if (IsAllowWaitingList5 == 'True') {
            $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', IsAllowWaitingList5);
            $('#<%:chkIsAllowWaitingList5.ClientID %>').removeAttr("disabled");
            $('#<%=txtMaximumWaitingList5.ClientID %>').removeAttr('readonly',);
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').removeAttr('readonly',);
            $('#<%=txtWaiting5.ClientID %>').removeAttr('readonly');
            $('#<%=txtWaiting5BPJS.ClientID %>').removeAttr('readonly');
        } else {
            $('#<%=chkIsAllowWaitingList5.ClientID %>').prop('checked', false);
            $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtWaiting5.ClientID %>').val('');
            $('#<%=txtWaiting5BPJS.ClientID %>').val('');
        }
        //#endregion

        //#region Maximum Waiting List
        if (MaximumWaitingList1 != '0') {
            $('#<%=txtMaximumWaitingList1.ClientID %>').val(MaximumWaitingList1);
        } else {
            $('#<%=txtMaximumWaitingList1.ClientID %>').val('');
        }
        if (MaximumWaitingList1BPJS != '0') {
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val(MaximumWaitingList1BPJS);
        } else {
            $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('');
        }

        if (MaximumWaitingList2 != '0') {
            $('#<%=txtMaximumWaitingList2.ClientID %>').val(MaximumWaitingList2);
        } else {
            $('#<%=txtMaximumWaitingList2.ClientID %>').val('');
        }
        if (MaximumWaitingList2BPJS != '0') {
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val(MaximumWaitingList2BPJS);
        } else {
            $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('');
        }

        if (MaximumWaitingList3 != '0') {
            $('#<%=txtMaximumWaitingList3.ClientID %>').val(MaximumWaitingList3);
        } else {
            $('#<%=txtMaximumWaitingList3.ClientID %>').val('');
        }
        if (MaximumWaitingList3BPJS != '0') {
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val(MaximumWaitingList3BPJS);
        } else {
            $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('');
        }

        if (MaximumWaitingList4 != '0') {
            $('#<%=txtMaximumWaitingList4.ClientID %>').val(MaximumWaitingList4);
        } else {
            $('#<%=txtMaximumWaitingList4.ClientID %>').val('');
        }
        if (MaximumWaitingList4BPJS != '0') {
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val(MaximumWaitingList4BPJS);
        } else {
            $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('');
        }

        if (MaximumWaitingList5 != '0') {
            $('#<%=txtMaximumWaitingList5.ClientID %>').val(MaximumWaitingList5);
        } else {
            $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
        }
        if (MaximumWaitingList5BPJS != '0') {
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val(MaximumWaitingList5BPJS);
        } else {
            $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
        }
        //#endregion

        //#region Mobile Appointment Percentage
        if (MobileWaiting1 != '0') {
            $('#<%=txtWaiting1.ClientID %>').val(MobileWaiting1);
        } else {
            $('#<%=txtWaiting1.ClientID %>').val('');
        }
        if (MobileWaiting1BPJS != '0') {
            $('#<%=txtWaiting1BPJS.ClientID %>').val(MobileWaiting1BPJS);
        } else {
            $('#<%=txtWaiting1BPJS.ClientID %>').val('');
        }

        if (MobileWaiting2 != '0') {
            $('#<%=txtWaiting2.ClientID %>').val(MobileWaiting2);
        } else {
            $('#<%=txtWaiting2.ClientID %>').val('');
        }
        if (MobileWaiting2BPJS != '0') {
            $('#<%=txtWaiting2BPJS.ClientID %>').val(MobileWaiting2BPJS);
        } else {
            $('#<%=txtWaiting2BPJS.ClientID %>').val('');
        }
        if (MobileWaiting3 != '0') {
            $('#<%=txtWaiting3.ClientID %>').val(MobileWaiting3);
        } else {
            $('#<%=txtWaiting3.ClientID %>').val('');
        }
        if (MobileWaiting3BPJS != '0') {
            $('#<%=txtWaiting3BPJS.ClientID %>').val(MobileWaiting3BPJS);
        } else {
            $('#<%=txtWaiting3BPJS.ClientID %>').val('');
        }
        if (MobileWaiting4 != '0') {
            $('#<%=txtWaiting4.ClientID %>').val(MobileWaiting4);
        } else {
            $('#<%=txtWaiting4.ClientID %>').val('');
        }
        if (MobileWaiting4BPJS != '0') {
            $('#<%=txtWaiting4BPJS.ClientID %>').val(MobileWaiting4BPJS);
        } else {
            $('#<%=txtWaiting4BPJS.ClientID %>').val('');
        }
        if (MobileWaiting5 != '0') {
            $('#<%=txtWaiting5.ClientID %>').val(MobileWaiting5);
        } else {
            $('#<%=txtWaiting5.ClientID %>').val('');
        }
        if (MobileWaiting5BPJS != '0') {
            $('#<%=txtWaiting5BPJS.ClientID %>').val(MobileWaiting5BPJS);
        } else {
            $('#<%=txtWaiting5BPJS.ClientID %>').val('');
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


        if (ReservedQueueStartNoBPJS1 != '0') {
            $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val(ReservedQueueStartNoBPJS1);
        } else {
            $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
        }
        if (ReservedQueueStartNoBPJS2 != '0') {
            $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val(ReservedQueueStartNoBPJS2);
        } else {
            $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
        }
        if (ReservedQueueStartNoBPJS3 != '0') {
            $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val(ReservedQueueStartNoBPJS3);
        } else {
            $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
        }
        if (ReservedQueueStartNoBPJS4 != '0') {
            $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val(ReservedQueueStartNoBPJS4);
        } else {
            $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
        }
        if (ReservedQueueStartNoBPJS5 != '0') {
            $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val(ReservedQueueStartNoBPJS5);
        } else {
            $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val('');
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

        if (ReservedQueueEndNoBPJS1 != '0') {
            $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val(ReservedQueueEndNoBPJS1);
        } else {
            $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
        }
        if (ReservedQueueEndNoBPJS2 != '0') {
            $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val(ReservedQueueEndNoBPJS2);
        } else {
            $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
        }
        if (ReservedQueueEndNoBPJS3 != '0') {
            $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val(ReservedQueueEndNoBPJS3);
        } else {
            $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
        }
        if (ReservedQueueEndNoBPJS4 != '0') {
            $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val(ReservedQueueEndNoBPJS4);
        } else {
            $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
        }
        if (ReservedQueueEndNoBPJS5 != '0') {
            $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val(ReservedQueueEndNoBPJS5);
        } else {
            $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
        }
        //#endregion

        //#region Is BPJS Appointment
        if (IsBPJSAppointment1 == 'True') {
            $('#<%=chkIsBPJS1.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment2 == 'True') {
            $('#<%=chkIsBPJS2.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment3 == 'True') {
            $('#<%=chkIsBPJS3.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment4 == 'True') {
            $('#<%=chkIsBPJS4.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
        }
        if (IsBPJSAppointment5 == 'True') {
            $('#<%=chkIsBPJS5.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);
        }
        //#endregion

        //#region Is Non BPJS Appointment
        if (IsNonBPJSAppointment1 == 'True') {
            $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment2 == 'True') {
            $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment3 == 'True') {
            $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment4 == 'True') {
            $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
        }
        if (IsNonBPJSAppointment5 == 'True') {
            $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);
        }
        //#endregion

        $('#containerPopupEntryData').show();
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
        });
    });

    $('#<%=txtOperationalTimeCodeCtl.ClientID %>').change(function () {
        onTtxtOperationalTimeCodeCtlChanged($(this).val());
    });


    function onTtxtOperationalTimeCodeCtlChanged(value) {
        var filterExpression = "IsDeleted = 0 AND OperationalTimeCode ='" + value + "'";
        Methods.getObject('GetOperationalTimeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOperationalTime.ClientID %>').val("1");
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
                    $('#<%=hdnTotalSession.ClientID %>').val('1');

                    $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').attr('readonly', 'readonly');

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

//                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly', 'readonly');

//                    $('#<%=txtWaiting1.ClientID %>').val('0');
//                    $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2.ClientID %>').val('');
//                    $('#<%=txtWaiting2.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting3.ClientID %>').val('');
//                    $('#<%=txtWaiting3.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting4.ClientID %>').val('');
//                    $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5.ClientID %>').val('');
//                    $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 == "" && StartTime4 == "" && StartTime5 == "") {
                    $('#<%=hdnTotalSession.ClientID %>').val('2');

                    $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);


                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').attr('readonly', 'readonly');

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

//                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly', 'readonly');

//                    $('#<%=txtWaiting1.ClientID %>').val('');
//                    $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2.ClientID %>').val('');
//                    $('#<%=txtWaiting2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting3.ClientID %>').val('');
//                    $('#<%=txtWaiting3.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting4.ClientID %>').val('');
//                    $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5.ClientID %>').val('');
//                    $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 != "" && StartTime4 == "" && StartTime5 == "") {
                    $('#<%=hdnTotalSession.ClientID %>').val('3');
                    
                    $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');


                    $('#<%=txtMobileAppointment1.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').removeAttr('readonly');

                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').attr('readonly', 'readonly');


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

//                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly', 'readonly');

//                    $('#<%=txtWaiting1.ClientID %>').val('0');
//                    $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2.ClientID %>').val('0');
//                    $('#<%=txtWaiting2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').removeAttr('readonly');

//                    $('#<%=txtWaiting3.ClientID %>').val('0');
//                    $('#<%=txtWaiting3.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting4.ClientID %>').val('');
//                    $('#<%=txtWaiting4.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5.ClientID %>').val('');
//                    $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');

                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 != "" && StartTime4 != "" && StartTime5 == "") {
                    $('#<%=hdnTotalSession.ClientID %>').val('4');

                    $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('');
                    $('#<%=txtMaximumAppointment5.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').attr('readonly', 'readonly');

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

//                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').attr('readonly', 'readonly');

//                    $('#<%=txtWaiting1.ClientID %>').removeAttr('0');
//                    $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2.ClientID %>').val('0');
//                    $('#<%=txtWaiting2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting3.ClientID %>').val('0');
//                    $('#<%=txtWaiting3.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting4.ClientID %>').val('0');
//                    $('#<%=txtWaiting4.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting5.ClientID %>').val('');
//                    $('#<%=txtWaiting5.ClientID %>').attr('readonly', 'readonly');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').val('');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').attr('readonly', 'readonly');

                }
                else if (StartTime1 != "" && StartTime2 != "" && StartTime3 != "" && StartTime4 != "" && StartTime5 != "") {
                    $('#<%=hdnTotalSession.ClientID %>').val('5');

                    $('#<%:chkIsBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS3.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS4.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsBPJS5.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsBPJS5.ClientID %>').prop('checked', false);

                    $('#<%:chkIsNonBPJS1.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS1.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS2.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS2.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS3.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS3.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS4.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS4.ClientID %>').prop('checked', false);
                    $('#<%:chkIsNonBPJS5.ClientID %>').attr("disabled", false);
                    $('#<%=chkIsNonBPJS5.ClientID %>').prop('checked', false);

                    $('#<%=txtMaximumAppointment1.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment4.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment5.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').val('0');
                    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').removeAttr('readonly');

                    $('#<%=txtMobileAppointment1.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment4.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment5.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment5.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').val('0');
                    $('#<%=txtMobileAppointment5BPJS.ClientID %>').removeAttr('readonly');

                    $('#<%=txtReservedQueueStartNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueStartNo5BPJS.ClientID %>').removeAttr('readonly');

                    $('#<%=txtReservedQueueEndNo1.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo1BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo2BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo3BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo4BPJS.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5.ClientID %>').removeAttr('readonly');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').val('');
                    $('#<%=txtReservedQueueEndNo5BPJS.ClientID %>').removeAttr('readonly');


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

//                    $('#<%=txtMaximumWaitingList1.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList3.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList3BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList4.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList4BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList5.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').val('0');
//                    $('#<%=txtMaximumWaitingList5BPJS.ClientID %>').removeAttr('readonly');

//                    $('#<%=txtWaiting1.ClientID %>').val('0');
//                    $('#<%=txtWaiting1.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting1BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2.ClientID %>').val('0');
//                    $('#<%=txtWaiting2.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting2BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting3.ClientID %>').val('0');
//                    $('#<%=txtWaiting3.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting3BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting4.ClientID %>').val('0');
//                    $('#<%=txtWaiting4.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting4BPJS.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting5.ClientID %>').val('0');
//                    $('#<%=txtWaiting5.ClientID %>').removeAttr('readonly');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').val('0');
//                    $('#<%=txtWaiting5BPJS.ClientID %>').removeAttr('readonly');
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

    $('#<%=txtMaximumAppointment1.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });
    $('#<%=txtMaximumAppointment1BPJS.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });

    $('#<%=txtMaximumAppointment2.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });
    $('#<%=txtMaximumAppointment2BPJS.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });

    $('#<%=txtMaximumAppointment3.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });
    $('#<%=txtMaximumAppointment3BPJS.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });

    $('#<%=txtMaximumAppointment4.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });
    $('#<%=txtMaximumAppointment4BPJS.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });

    $('#<%=txtMaximumAppointment5.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });
    $('#<%=txtMaximumAppointment5BPJS.ClientID %>').change(function () {
        $('#<%=hdnMaxAppointmentChanged.ClientID %>').val("1");
    });

    $('#<%=txtRemarks.ClientID %>').change(function () {
        $('#<%=hdnIsRemarksChanged.ClientID %>').val("1");
    });

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
                $('#<%:hdnRoomIDNew.ClientID %>').val(result.RoomID);
                $('#<%:hdnRoomID.ClientID %>').val(result.RoomID);
                $('#<%:txtRoomCode.ClientID %>').val(result.RoomCode);
                $('#<%:txtRoomName.ClientID %>').val(result.RoomName);
            }
            else {
                $('#<%:hdnRoomIDNew.ClientID %>').val('');
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
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <input type="hidden" id="hdnRoomIDOld" value="" runat="server" />
    <input type="hidden" id="hdnRoomIDNew" value="" runat="server" />
    <input type="hidden" id="hdnOperationalTime" value="" runat="server" />
    <input type="hidden" id="hdnMaxAppointmentChanged" value="0" runat="server" />
    <input type="hidden" id="hdnIsChkBPJSChanged" value="0" runat="server" />
    <input type="hidden" id="hdnIsRemarksChanged" value="0" runat="server" />
    <input type="hidden" id="hdnTotalSession" value="0" runat="server" />
    <input type="hidden" id="hdnIsAllowDifferentQueueNo" value="0" runat="server" />
    <input type="hidden" id="hdnIsChangeDifferentQueueNo" value="0" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 1px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
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
                        <table class="tblEntryDetail1" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
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
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("By Time Slot")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsAppointmentByTimeSlot" Width="100px" Checked="false" runat="server" />
                                </td>
                            </tr>
                            <tr id="trIsAllowDifferentQueueNo" runat="server">
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Antrian Registrasi Terpisah")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsAllowDifferentQueueNo" Width="100px" Checked="false" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <div style="overflow-x:auto">
                            <table class="tblEntryDetail1" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%;display:none" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                            </colgroup>
                                            <tr>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jam Pelayanan")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jenis Pasien")%></label>
                                                </td>
                                                <td align="center" colspan="1" style="font-weight: bold; font-style: italic; display: none">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Interval")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Max. Appointment")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal" title="Maximum Appointment (Online)">
                                                        <%=GetLabel("Online Appointment")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("No.Antrian Non-BPJS")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("No.Antrian BPJS")%></label>
                                                </td>
                                                <td align="center" rowspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Waiting List")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Maximum Waiting")%></label>
                                                </td>
                                                <td align="center" colspan="2" style="font-weight: bold; font-style: italic">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Waiting Online")%></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Mulai")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Akhir")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Jenis Pasien : Non-BPJS">
                                                        <%=GetLabel("Non-BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Jenis Pasien BPJS">
                                                        <%=GetLabel("BPJS")%></label>
                                                </td>
                                                <td align="center" style="display:none">
                                                    <label class="lblNormal" title="">
                                                        <%=GetLabel("")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Appointment Non-BPJS">
                                                        <%=GetLabel("Non-BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Appointment BPJS">
                                                        <%=GetLabel("BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Appointment (Online) Non-BPJS">
                                                        <%=GetLabel("Non-BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Appointment (Online) BPJS">
                                                        <%=GetLabel("BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Awal")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Akhir")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Awal")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Akhir")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Waiting List Non-BPJS">
                                                        <%=GetLabel("Non-BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Waiting List BPJS">
                                                        <%=GetLabel("BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Waiting List (Online) Non-BPJS">
                                                        <%=GetLabel("Non-BPJS")%></label>
                                                </td>
                                                <td align="center">
                                                    <label class="lblNormal" title="Maximum Waiting List (Online) BPJS">
                                                        <%=GetLabel("BPJS")%></label>
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
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%; display:none" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                                <col style="width: 5%" />
                                            </colgroup>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtStartTime1" CssClass="time start" Width="60px" ReadOnly="true"
                                                        runat="server" MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtEndTime1" CssClass="time end" Width="60px" ReadOnly="true" runat="server"
                                                        MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsNonBPJS1" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsBPJS1" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center" style="display: none">
                                                    <asp:TextBox ID="txtVisitDuration1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment1BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment1BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo1BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo1BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsAllowWaitingList1" Width="100%" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList1BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting1" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting1BPJS" CssClass="number" Width="60px" runat="server" />
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
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%; display:none" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                            </colgroup>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtStartTime2" CssClass="time start" Width="60px" ReadOnly="true"
                                                        runat="server" MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtEndTime2" CssClass="time end" Width="60px" ReadOnly="true" runat="server"
                                                        MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsNonBPJS2" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsBPJS2" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center" style="display:none">
                                                    <asp:TextBox ID="txtVisitDuration2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment2BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment2BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo2BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo2BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsAllowWaitingList2" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList2BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting2" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting2BPJS" CssClass="number" Width="60px" runat="server" />
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
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%;display:none" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                            </colgroup>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtStartTime3" CssClass="time start" Width="60px" ReadOnly="true"
                                                        runat="server" MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtEndTime3" CssClass="time end" Width="60px" ReadOnly="true" runat="server"
                                                        MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsNonBPJS3" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsBPJS3" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center" style="display:none">
                                                    <asp:TextBox ID="txtVisitDuration3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment3BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment3BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo3BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo3BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsAllowWaitingList3" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList3BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting3" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting3BPJS" CssClass="number" Width="60px" runat="server" />
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
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%;display:none" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                            </colgroup>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtStartTime4" CssClass="time start" Width="60px" ReadOnly="true"
                                                        runat="server" MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtEndTime4" CssClass="time end" Width="60px" ReadOnly="true" runat="server"
                                                        MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsNonBPJS4" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsBPJS4" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center" style="display:none">
                                                    <asp:TextBox ID="txtVisitDuration4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment4BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment4BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo4BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo4BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsAllowWaitingList4" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList4BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting4" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting4BPJS" CssClass="number" Width="60px" runat="server" />
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
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%;display:none" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                                <col style="width: 5.5%" />
                                            </colgroup>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtStartTime5" CssClass="time start" Width="60px" ReadOnly="true"
                                                        runat="server" MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtEndTime5" CssClass="time end" Width="60px" ReadOnly="true" runat="server"
                                                        MaxLength="5" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsNonBPJS5" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsBPJS5" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center" style="display:none">
                                                    <asp:TextBox ID="txtVisitDuration5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumAppointment5BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMobileAppointment5BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueStartNo5BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtReservedQueueEndNo5BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsAllowWaitingList5" Width="60px" Checked="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMaximumWaitingList5BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting5" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaiting5BPJS" CssClass="number" Width="60px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table class="tblEntryDetail1" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
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
                                                <input type="hidden" class="tdMobileAppointment1BPJS" value="<%#: Eval("MobileAppointmentBPJS1")%>" />
                                                <input type="hidden" class="tdMobileAppointment2BPJS" value="<%#: Eval("MobileAppointmentBPJS2")%>" />
                                                <input type="hidden" class="tdMobileAppointment3BPJS" value="<%#: Eval("MobileAppointmentBPJS3")%>" />
                                                <input type="hidden" class="tdMobileAppointment4BPJS" value="<%#: Eval("MobileAppointmentBPJS4")%>" />
                                                <input type="hidden" class="tdMobileAppointment5BPJS" value="<%#: Eval("MobileAppointmentBPJS5")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo1" value="<%#: Eval("ReservedQueueStartNo1")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo2" value="<%#: Eval("ReservedQueueStartNo2")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo3" value="<%#: Eval("ReservedQueueStartNo3")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo4" value="<%#: Eval("ReservedQueueStartNo4")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNo5" value="<%#: Eval("ReservedQueueStartNo5")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNoBPJS1" value="<%#: Eval("ReservedQueueStartNoBPJS1")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNoBPJS2" value="<%#: Eval("ReservedQueueStartNoBPJS2")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNoBPJS3" value="<%#: Eval("ReservedQueueStartNoBPJS3")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNoBPJS4" value="<%#: Eval("ReservedQueueStartNoBPJS4")%>" />
                                                <input type="hidden" class="tdReservedQueueStartNoBPJS5" value="<%#: Eval("ReservedQueueStartNoBPJS5")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo1" value="<%#: Eval("ReservedQueueEndNo1")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo2" value="<%#: Eval("ReservedQueueEndNo2")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo3" value="<%#: Eval("ReservedQueueEndNo3")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo4" value="<%#: Eval("ReservedQueueEndNo4")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNo5" value="<%#: Eval("ReservedQueueEndNo5")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNoBPJS1" value="<%#: Eval("ReservedQueueEndNoBPJS1")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNoBPJS2" value="<%#: Eval("ReservedQueueEndNoBPJS2")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNoBPJS3" value="<%#: Eval("ReservedQueueEndNoBPJS3")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNoBPJS4" value="<%#: Eval("ReservedQueueEndNoBPJS4")%>" />
                                                <input type="hidden" class="tdReservedQueueEndNoBPJS5" value="<%#: Eval("ReservedQueueEndNoBPJS5")%>" />
                                                <input type="hidden" class="tdMaximumAppointment1" value="<%#: Eval("MaximumAppointment1")%>" />
                                                <input type="hidden" class="tdMaximumAppointment2" value="<%#: Eval("MaximumAppointment2")%>" />
                                                <input type="hidden" class="tdMaximumAppointment3" value="<%#: Eval("MaximumAppointment3")%>" />
                                                <input type="hidden" class="tdMaximumAppointment4" value="<%#: Eval("MaximumAppointment4")%>" />
                                                <input type="hidden" class="tdMaximumAppointment5" value="<%#: Eval("MaximumAppointment5")%>" />
                                                <input type="hidden" class="tdMaximumAppointmentBPJS1" value="<%#: Eval("MaximumAppointmentBPJS1")%>" />
                                                <input type="hidden" class="tdMaximumAppointmentBPJS2" value="<%#: Eval("MaximumAppointmentBPJS2")%>" />
                                                <input type="hidden" class="tdMaximumAppointmentBPJS3" value="<%#: Eval("MaximumAppointmentBPJS3")%>" />
                                                <input type="hidden" class="tdMaximumAppointmentBPJS4" value="<%#: Eval("MaximumAppointmentBPJS4")%>" />
                                                <input type="hidden" class="tdMaximumAppointmentBPJS5" value="<%#: Eval("MaximumAppointmentBPJS5")%>" />
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
                                                <input type="hidden" class="tdMaximumWaitingListBPJS1" value="<%#: Eval("MaximumWaitingListBPJS1")%>" />
                                                <input type="hidden" class="tdMaximumWaitingListBPJS2" value="<%#: Eval("MaximumWaitingListBPJS2")%>" />
                                                <input type="hidden" class="tdMaximumWaitingListBPJS3" value="<%#: Eval("MaximumWaitingListBPJS3")%>" />
                                                <input type="hidden" class="tdMaximumWaitingListBPJS4" value="<%#: Eval("MaximumWaitingListBPJS4")%>" />
                                                <input type="hidden" class="tdMaximumWaitingListBPJS5" value="<%#: Eval("MaximumWaitingListBPJS5")%>" />
                                                <input type="hidden" class="tdMobileWaiting1" value="<%#: Eval("MobileWaitingList1")%>" />
                                                <input type="hidden" class="tdMobileWaiting2" value="<%#: Eval("MobileWaitingList2")%>" />
                                                <input type="hidden" class="tdMobileWaiting3" value="<%#: Eval("MobileWaitingList3")%>" />
                                                <input type="hidden" class="tdMobileWaiting4" value="<%#: Eval("MobileWaitingList4")%>" />
                                                <input type="hidden" class="tdMobileWaiting5" value="<%#: Eval("MobileWaitingList5")%>" />
                                                <input type="hidden" class="tdMobileWaitingBPJS1" value="<%#: Eval("MobileWaitingListBPJS1")%>" />
                                                <input type="hidden" class="tdMobileWaitingBPJS2" value="<%#: Eval("MobileWaitingListBPJS2")%>" />
                                                <input type="hidden" class="tdMobileWaitingBPJS3" value="<%#: Eval("MobileWaitingListBPJS3")%>" />
                                                <input type="hidden" class="tdMobileWaitingBPJS4" value="<%#: Eval("MobileWaitingListBPJS4")%>" />
                                                <input type="hidden" class="tdMobileWaitingBPJS5" value="<%#: Eval("MobileWaitingListBPJS5")%>" />
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
                                                <input type="hidden" class="tdIsAllowDifferentQueueNo" value="<%#: Eval("IsAllowDifferentQueueNo")%>" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="80px" DataField="cfDayName1" HeaderText="Hari"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="RoomName" ItemStyle-CssClass="tdRoomName"
                                            HeaderText="Kamar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" CssClass="tdRoomName"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="250px" DataField="OperationalTimeName" ItemStyle-CssClass="tdOperationalTimeName"
                                            HeaderText="Operational Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
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
