<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentChangeDateCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentChangeDateCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntrySave">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_sappointmentchangedatectl">
    var numInit = 0;

    function onLoad() {
        cboAppointmentMethodFrom.SetEnabled(false);
        $("#calAppointmentChangeDate").datepicker({
            defaultDate: Methods.getDatePickerDate($('#<%=hdnCalAppointmentSelectedDateCtl.ClientID %>').val()),
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            minDate: "0",
            onSelect: function (dateText, inst) {
                $('#<%=hdnCalAppointmentSelectedDateCtl.ClientID %>').val(dateText);
                cbpPhysicianChangeAppointment.PerformCallback('refresh');
                $('#<%=txtNewAppointmentDate.ClientID %>').val(dateText);
            }
        });


        $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').die('click');
        $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnParamedicIDCtl.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=txtNewPhysician.ClientID %>').val($(this).find('.tdParamedicName').html());

            cbpAppointmentChangeAppointment.PerformCallback('changeParamedic');
        });
        var paramedicID = $('#<%=hdnOldParamedicID.ClientID %>').val();
        $('#<%=grdPhysician.ClientID %> > tbody > tr').each(function () {
            if ($(this).find('.keyField').html() == paramedicID)
                $(this).click();
        });

        $('#<%=grdAppointment.ClientID %> td.tdAppointment li').die('click');
        $('#<%=grdAppointment.ClientID %> td.tdAppointment li').live('click', function (evt) {
            $tr = $(this).closest('tr');
            var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
            if (appointmentID > -2) {
                $('#<%=grdAppointment.ClientID %> li.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=txtNewAppointmentTime.ClientID %>').val($tr.find('.tdTime').html());
                $('#<%=hdnSelectedAppointmentID.ClientID %>').val(appointmentID);
            }
        });

        $('#<%=grdAppointment.ClientID %> tr:gt(0) td.tdExpand').die('click');
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

        $('#<%=grdAppointmentNoTimeSlot.ClientID %> td.tdAppointment li').die('click');
        $('#<%=grdAppointmentNoTimeSlot.ClientID %> td.tdAppointment li').live('click', function (evt) {
            $tr = $(this).closest('tr');
            var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
            if (appointmentID > -2) {
                $('#<%=grdAppointmentNoTimeSlot.ClientID %> li.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnSelectedAppointmentID.ClientID %>').val(appointmentID);
            }
        });

        $('#<%=grdAppointmentNoTimeSlot.ClientID %> tr:gt(0) td.tdExpand').die('click');
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
    }

    $('#<%=grdWaitingList.ClientID %> td.tdAppointment li').die('click');
    $('#<%=grdWaitingList.ClientID %> td.tdAppointment li').live('click', function (evt) {
        $tr = $(this).closest('tr');
        var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
        if (appointmentID > -2) {
            $('#<%=grdWaitingList.ClientID %> li.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnSelectedAppointmentID.ClientID %>').val(appointmentID);
        }
    });

    function onCbpAppointmentChangeAppointmentEndCallback(s) {
        var idx = s.cpResult.split('|');
        if (idx[0] == '0') {
            idx[0] = 1;
            $('#<%=grdAppointment.ClientID %> > tbody > tr:eq(' + idx[0] + ') td.tdAppointment li').click();

            if (idx[1] == 'leave') {
                $('#<%:txtStartLeaveCtl.ClientID %>').val($('#<%=hdnStartLeaveCtlValidMessage.ClientID %>').val());
                $('#<%:txtEndLeaveCtl.ClientID %>').val($('#<%=hdnEndLeaveCtlValidMessage.ClientID %>').val());
                $('#<%:txtLeaveReasonCtl.ClientID %>').val($('#<%=hdnLeaveReasonCtlValidMessage.ClientID %>').val());

                $('#<%:trStartLeaveCtl.ClientID %>').removeAttr('style');
                $('#<%:trEndLeaveCtl.ClientID %>').removeAttr('style');
                $('#<%:trLeaveReasonCtl.ClientID %>').removeAttr('style');
                $('#<%:trTimeSlotCtl.ClientID %>').attr('style', 'display:none');
            }
        }
        else {
            $('#<%:trStartLeaveCtl.ClientID %>').attr('style', 'display:none');
            $('#<%:trEndLeaveCtl.ClientID %>').attr('style', 'display:none');
            $('#<%:trLeaveReasonCtl.ClientID %>').attr('style', 'display:none');
            $('#<%:trTimeSlotCtl.ClientID %>').removeAttr('style');
        }

        $('.grdAppointment li.selected').removeClass('selected');

        var nameCtl = '';
        if ($('#<%=hdnIsShowTimeSlotContainerEntryCtl.ClientID %>').val() == "1") {
            nameCtl = 'containerAppointmentCtl';
        }
        else {
            nameCtl = 'containerAppointmentNoTimeSlotCtl';
        }

        if (nameCtl == 'containerWaitingListCtl') {
            $('#<%=hdnIsWaitingListCtl.ClientID %>').val('1');
            $('#<%=hdnIsByTimeSlotCtl.ClientID %>').val('0');
            $('#<%=hdnIsByNoTimeSlotCtl.ClientID %>').val('0');
        }
        else if (nameCtl == 'containerAppointmentNoTimeSlotCtl') {
            $('#<%=hdnIsWaitingListCtl.ClientID %>').val('0');
            $('#<%=hdnIsByTimeSlotCtl.ClientID %>').val('0');
            $('#<%=hdnIsByNoTimeSlotCtl.ClientID %>').val('1');
        }
        else {
            $('#<%=hdnIsWaitingListCtl.ClientID %>').val('0');
            $('#<%=hdnIsByTimeSlotCtl.ClientID %>').val('1');
            $('#<%=hdnIsByNoTimeSlotCtl.ClientID %>').val('0');
        }

        $(this).addClass('selected');
        $('#' + nameCtl).removeAttr('style');
        $('#ulTabClinicTransactionCtl li').each(function () {
            var tempNameContainerCtl = $(this).attr('contentid');
            if (tempNameContainerCtl != nameCtl) {
                $(this).removeClass('selected');
                $('#' + tempNameContainerCtl).attr('style', 'display:none');
            }
            else {
                $(this).addClass('selected');
            }
        });

        hideLoadingPanel();
    }

    function onCboServiceUnitChangeAppointmentValueChanged() {
        $('#<%=txtNewServiceUnit.ClientID %>').val(cboServiceUnitChangeAppointment.GetText());
        cbpPhysicianChangeAppointment.PerformCallback('refresh');
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPhysicanChangeAppointment"), pageCount, function (page) {
            cbpPhysicianChangeAppointment.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPhysicianChangeAppointmentEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPhysicanChangeAppointment"), pageCount, function (page) {
                cbpPhysicianChangeAppointment.PerformCallback('changepage|' + page);
            });
        }
        else if (param[0] == 'save') {
            pcRightPanelContent.Hide();
            if (typeof onRefreshAfterChangeAppointment == 'function')
                onRefreshAfterChangeAppointment();
        }
        else
            $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    $('#ulTabClinicTransactionCtl li').live('click', function () {
        $('.grdAppointment li.selected').removeClass('selected');
        var nameCtl = $(this).attr('contentid');

        if (nameCtl == 'containerWaitingListCtl') {
            $('#<%=hdnIsWaitingListCtl.ClientID %>').val('1');
            $('#<%=hdnIsByTimeSlotCtl.ClientID %>').val('0');
            $('#<%=hdnIsByNoTimeSlotCtl.ClientID %>').val('0');
        }
        else if (nameCtl == 'containerAppointmentNoTimeSlotCtl') {
            $('#<%=hdnIsWaitingListCtl.ClientID %>').val('0');
            $('#<%=hdnIsByTimeSlotCtl.ClientID %>').val('0');
            $('#<%=hdnIsByNoTimeSlotCtl.ClientID %>').val('1');
        }
        else {
            $('#<%=hdnIsWaitingListCtl.ClientID %>').val('0');
            $('#<%=hdnIsByTimeSlotCtl.ClientID %>').val('1');
            $('#<%=hdnIsByNoTimeSlotCtl.ClientID %>').val('0');
        }

        $(this).addClass('selected');
        $('#' + nameCtl).removeAttr('style');
        $('#ulTabClinicTransactionCtl li').each(function () {
            var tempNameContainer = $(this).attr('contentid');
            if (tempNameContainer != nameCtl) {
                $(this).removeClass('selected');
                $('#' + tempNameContainer).attr('style', 'display:none');
            }
        });
    });

    function onCboSessionCtlValueChanged() {
        cbpAppointmentChangeAppointment.PerformCallback();
    }

    function oncboChangeReasonValueChanged() {
        if (cboChangeReason.GetValue() == Constant.DeleteReason.OTHER) {
            $('#trChangeOtherReason').removeAttr('style');
        }
        else {
            $('#trChangeOtherReason').attr('style', 'display:none');
        }
    }

    //#region Visit Type
    $('#lblVisitTypeTo').live('click', function () {
        var serviceUnitID = cboServiceUnitChangeAppointment.GetValue();
        var paramedicID = $('#<%=hdnParamedicIDCtl.ClientID %>').val();
        if (paramedicID != '') {
            var filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
            Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                var filterExpression = '';
                if (result > 0) {
                    filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + ")";
                    openSearchDialog('visittype', filterExpression, function (value) {
                        $('#<%=txtVisitTypeCodeTo.ClientID %>').val(value);
                        onTxtVisitTypeCodeToChanged(value);
                    });
                }
                else {
                    var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                    Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                        if (result.IsHasVisitType)
                            filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
                        else
                            filterExpression = '';
                        openSearchDialog('visittype', filterExpression, function (value) {
                            $('#<%=txtVisitTypeCodeTo.ClientID %>').val(value);
                            onTxtVisitTypeCodeToChanged(value);
                        });
                    });
                }
            });
        }
        else {
            showToast('Warning', 'Silahkan Pilih Dokter Terlebih Dahulu');
        }
    });

    $('#<%=txtVisitTypeCodeTo.ClientID %>').live('change', function () {
        onTxtVisitTypeCodeToChanged($(this).val());
    });

    function onTxtVisitTypeCodeToChanged(value) {
        var filterExpression = '';

        var serviceUnitID = cboServiceUnit.GetValue();
        var paramedicID = $('#<%=hdnParamedicIDCtl.ClientID %>').val();
        filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
        Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
            var filterExpression = '';
            if (result > 0) {
                filterExpression += "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + " AND VisitTypeCode = '" + value + "'";
                Methods.getObject('GetvParamedicVisitTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnVisitTypeIDTo.ClientID %>').val(result.VisitTypeID);
                        $('#<%=txtVisitTypeNameTo.ClientID %>').val(result.VisitTypeName);
                        $('#<%=hdnVisitDurationTo.ClientID %>').val(result.VisitDuration);
                        $('#<%=txtVisitDurationTo.ClientID %>').val(result.VisitDuration);
                    }
                    else {
                        $('#<%=hdnVisitTypeIDTo.ClientID %>').val('');
                        $('#<%=txtVisitTypeCodeTo.ClientID %>').val('');
                        $('#<%=txtVisitTypeNameTo.ClientID %>').val('');
                        $('#<%=hdnVisitDurationTo.ClientID %>').val('');
                        $('#<%=txtVisitDurationTo.ClientID %>').val('');
                    }
                });
            }
            else {
                var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                    if (result.IsHasVisitType) {
                        filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND VisitTypeCode = '" + value + "'";
                        Methods.getObject('GetvServiceUnitVisitTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnVisitTypeIDTo.ClientID %>').val(result.VisitTypeID);
                                $('#<%=txtVisitTypeNameTo.ClientID %>').val(result.VisitTypeName);
                                $('#<%=hdnVisitDurationTo.ClientID %>').val(result.VisitDuration);
                                $('#<%=txtVisitDurationTo.ClientID %>').val(result.VisitDuration);
                            }
                            else {
                                $('#<%=hdnVisitTypeIDTo.ClientID %>').val('');
                                $('#<%=txtVisitTypeCodeTo.ClientID %>').val('');
                                $('#<%=txtVisitTypeNameTo.ClientID %>').val('');
                                $('#<%=hdnVisitDurationTo.ClientID %>').val('');
                                $('#<%=txtVisitDurationTo.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        filterExpression = "VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                        Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnVisitTypeIDTo.ClientID %>').val(result.VisitTypeID);
                                $('#<%=txtVisitTypeNameTo.ClientID %>').val(result.VisitTypeName);
                                $('#<%=hdnVisitDurationTo.ClientID %>').val('15');
                            }
                            else {
                                $('#<%=hdnVisitTypeIDTo.ClientID %>').val('');
                                $('#<%=txtVisitTypeCodeTo.ClientID %>').val('');
                                $('#<%=txtVisitTypeNameTo.ClientID %>').val('');
                                $('#<%=hdnVisitDurationTo.ClientID %>').val('');
                            }
                        });
                    }
                });
            }
        });
    }
    //#endregion

    $('#btnPlusVisitDurationCtl').click(function (evt) {
        var hdnVisitDurationTo = parseFloat($('#<%=hdnVisitDurationTo.ClientID %>').val());
        var visitDurationCtl = parseFloat($('#<%=txtVisitDurationTo.ClientID %>').val());
        if ($('#<%=txtVisitDurationTo.ClientID %>').val() != '') {
            var valueCtl = visitDurationCtl + hdnVisitDurationTo
            $('#<%=txtVisitDurationTo.ClientID %>').val(valueCtl);
        }
        else {
            showToast('Warning', 'Harap pilih jenis kunjungan dahulu!');
        }
    });

    $('#btnMinVisitDurationCtl').click(function (evt) {
        var hdnVisitDurationTo = parseFloat($('#<%=hdnVisitDurationTo.ClientID %>').val());
        var visitDuration = parseFloat($('#<%=txtVisitDurationTo.ClientID %>').val());

        if (visitDuration != 0 && visitDuration > hdnVisitDurationTo) {
            var valueCtl = parseFloat($('#<%=txtVisitDurationTo.ClientID %>').val());
            valueCtl -= hdnVisitDurationTo;
            $('#<%=txtVisitDurationTo.ClientID %>').val(valueCtl);
        }
        else if ($('#<%=hdnVisitDurationTo.ClientID %>').val() != '' && $('#<%=txtVisitDurationTo.ClientID %>').val() != '') {
            if ($('#<%=hdnVisitDurationTo.ClientID %>').val() != $('#<%=txtVisitDurationTo.ClientID %>').val()) {
                showToast('Warning', 'Harap pilih jenis kunjungan dahulu!');
            }
        }
        else {
            showToast('Warning', 'Harap pilih jenis kunjungan dahulu!');
        }
    });

    //#region Room
    function onGetServiceUnitRoomFilterExpressionCtl() {
        var filterExpressionCtl = 'HealthcareServiceUnitID = ' + cboServiceUnitChangeAppointment.GetValue() + ' AND IsDeleted = 0';
        return filterExpressionCtl;
    }

    $('#lblRoomTo.lblLink').live('click', function () {
        openSearchDialog('serviceunitroom', onGetServiceUnitRoomFilterExpressionCtl(), function (value) {
            $('#<%=txtRoomCodeTo.ClientID %>').val(value);
            onTxtServiceUnitRoomCodeChangedCtl(value);
        });
    });

    $('#<%=txtRoomCodeTo.ClientID %>').live('change', function () {
        onTxtServiceUnitRoomCodeChangedCtl($(this).val());
    });

    function onTxtServiceUnitRoomCodeChangedCtl(value) {
        var filterExpressionCtl = onGetServiceUnitRoomFilterExpressionCtl() + " AND RoomCode = '" + value + "'";
        Methods.getObject('GetvServiceUnitRoomList', filterExpressionCtl, function (result) {
            if (result != null) {
                $('#<%=hdnRoomIDTo.ClientID %>').val(result.RoomID);
                $('#<%=txtRoomCodeTo.ClientID %>').val(result.RoomCode);
                $('#<%=txtRoomNameTo.ClientID %>').val(result.RoomName);
            }
            else {
                $('#<%=hdnRoomIDTo.ClientID %>').val('');
                $('#<%=txtRoomCodeTo.ClientID %>').val('');
                $('#<%=txtRoomNameTo.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnMPEntrySave').click(function (evt) {
        var appointmentID = parseInt($('#<%=hdnSelectedAppointmentID.ClientID %>').val());
        if (appointmentID < 0) {
            if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                if ($('#<%=hdnIsWaitingListCtl.ClientID %>').val() == '0') {
                    if ($('#<%=hdnGCCustomerTypeCtl.ClientID %>').val() != "X004^500") {
                        var maxAppo = parseInt($('#<%=hdnMaxAppoMessageCtl.ClientID %>').val());
                        var totalAppo = parseInt($('#<%=hdnTotalAppoMessageCtl.ClientID %>').val());
                        if (maxAppo > totalAppo) {
                            var message = 'Maksimum perjanjian (untuk sesi yang dipilih) adalah ' + '<b>' + $('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() + '</b>' + ' dan jumlah perjanjian (untuk sesi yang dipilih) yang ada saat ini adalah ' + '<b>' + $('#<%=hdnTotalAppoMessageCtl.ClientID %>').val() + '</b>' + '. Apakah ingin melanjutkan proses?'
                            if (maxAppo <= totalAppo) {
                                showToastConfirmation(message, function (result) {
                                    if (result) {
                                        cbpPhysicianChangeAppointment.PerformCallback('save');
                                    }
                                });
                            }
                            else {
                                cbpPhysicianChangeAppointment.PerformCallback('save');
                            }
                        }
                        else {
                            var message = 'Maksimum perjanjian (untuk sesi yang dipilih) adalah ' + '<b>' + $('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() + '</b>' + ' sudah memenuhi kuota jadwal dokter. Apakah ingin melanjutkan proses?'
                            if ($('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() > 0) {
                                showToastConfirmation(message, function (result) {
                                    if (result) {
                                        cbpPhysicianChangeAppointment.PerformCallback('save');
                                    }
                                });
                            }
                            else {
                                cbpPhysicianChangeAppointment.PerformCallback('save');
                            }
                        }
                        if ($('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() > 0) {
                            var message = 'Maksimum Appointment (Untuk Sesi Yang Dipilih) Adalah ' + '<b>' + $('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() + '</b>' + ' Dan Jumlah Appointment (Untuk Sesi Yang Dipilih) Yang Ada Saat Ini Adalah ' + '<b>' + $('#<%=hdnTotalAppoMessageCtl.ClientID %>').val() + '</b>' + ' , Apakah Ingin Melanjutkan Proses ?'
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    cbpPhysicianChangeAppointment.PerformCallback('save');
                                }
                            });
                        }
                        else {
                            cbpPhysicianChangeAppointment.PerformCallback('save');
                        }
                    }
                    else {
                        if ($('#<%=hdnIsValidAppoMessageBPJSCtl.ClientID %>').val() == '1') {
                            cbpPhysicianChangeAppointment.PerformCallback('save');
                        }
                        else {
                            var maxAppo = parseInt($('#<%=hdnMaxAppoMessageCtl.ClientID %>').val());
                            var totalAppo = parseInt($('#<%=hdnTotalAppoMessageCtl.ClientID %>').val());
                            if (maxAppo > totalAppo) {
                                var message = 'Maksimum perjanjian BPJS (untuk sesi yang dipilih) adalah ' + '<b>' + $('#<%=hdnMaxAppoMessageBPJSCtl.ClientID %>').val() + '</b>' + ' dan jumlah perjanjian BPJS (untuk sesi yang dipilih) yang ada saat ini adalah ' + '<b>' + $('#<%=hdnTotalAppoMessageBPJSCtl.ClientID %>').val() + '</b>' + '. Apakah ingin melanjutkan proses?'

                                if ($('#<%=hdnMaxAppoMessageBPJSCtl.ClientID %>').val() > 0) {
                                    showToastConfirmation(message, function (result) {
                                        if (result) {
                                            cbpPhysicianChangeAppointment.PerformCallback('save');
                                        }
                                    });
                                }
                                else {
                                    cbpPhysicianChangeAppointment.PerformCallback('save');
                                }
                            }
                            else {
                                var message = 'Maksimum perjanjian (untuk sesi yang dipilih) adalah ' + '<b>' + $('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() + '</b>' + ' sudah memenuhi kuota jadwal dokter. Apakah ingin melanjutkan proses?'
                                if ($('#<%=hdnMaxAppoMessageCtl.ClientID %>').val() > 0) {
                                    showToastConfirmation(message, function (result) {
                                        if (result) {
                                            cbpPhysicianChangeAppointment.PerformCallback('save');
                                        }
                                    });
                                }
                                else {
                                    cbpPhysicianChangeAppointment.PerformCallback('save');
                                }
                            }
                        }
                    }
                }
                else {
                    if ($('#<%=hdnIsValidWaitingMessageCtl.ClientID %>').val() == '1') {
                        cbpPhysicianChangeAppointment.PerformCallback('save');
                    }
                    else {
                        if ($('#<%=hdnMaxWaitingMessageCtl.ClientID %>').val() > 0) {
                            var message = 'Maksimum Waiting List (Untuk Sesi Yang Dipilih) Adalah ' + '<b>' + $('#<%=hdnMaxWaitingMessageCtl.ClientID %>').val() + '</b>' + ' Dan Jumlah Waiting List (Untuk Sesi Yang Dipilih) Yang Ada Saat Ini Adalah ' + '<b>' + $('#<%=hdnTotalWaitingMessageCtl.ClientID %>').val() + '</b>' + ' , Apakah Ingin Melanjutkan Proses ?'
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    cbpPhysicianChangeAppointment.PerformCallback('save');
                                }
                            });
                        }
                        else {
                            cbpPhysicianChangeAppointment.PerformCallback('save');
                        }
                    }
                }
            }
        }
        else {
            showToast('Save Failed', 'Error Message : ' + '<%=GetErrorMsgAppointmentSlot() %>');
            return false;
        }
    });
</script>
<input type="hidden" runat="server" id="hdnID" value="" />
<input type="hidden" id="hdnSelectedAppointmentID" runat="server" />
<input type="hidden" id="hdnDefaultServiceUnitInterval" runat="server" />
<input type="hidden" id="hdnDepartmentIDCtl" runat="server" />
<input type="hidden" id="hdnOldParamedicID" runat="server" />
<input type="hidden" id="hdnOldHealthcareServiceUnitID" runat="server" />
<input type="hidden" id="hdnMaxAppoitment" runat="server" />
<input type="hidden" id="hdnIsVoidAndNoTimeSlot" runat="server" />
<input type="hidden" id="hdnIsBridgingToGateway" runat="server" />
<input type="hidden" id="hdnChangeAppointmentCreateNewAppointment" runat="server" />
<input type="hidden" id="hdnGCCustomerTypeCtl" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 50%" />
        <col style="width: 50%" />
    </colgroup>
    <tr>
        <td style="padding: 5px; vertical-align: top; border-right: 1px solid #AAA;">
            <div style="height: 500px; overflow-y: scroll; overflow-x: hidden;">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td valign="top">
                            <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDateCtl" />
                            <div id="calAppointmentChangeDate">
                            </div>
                        </td>
                        <td valign="top">
                            <dxe:ASPxComboBox ID="cboServiceUnitChangeAppointment" ClientInstanceName="cboServiceUnitChangeAppointment"
                                Width="100%" runat="server">
                                <ClientSideEvents Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }" ValueChanged="function(s,e) { onCboServiceUnitChangeAppointmentValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                            <input type="hidden" id="hdnParamedicIDCtl" runat="server" />
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpPhysicianChangeAppointment" runat="server" Width="100%"
                                    ClientInstanceName="cbpPhysicianChangeAppointment" ShowLoadingPanel="false" OnCallback="cbpPhysicianChangeAppointment_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                        EndCallback="function(s,e){ onCbpPhysicianChangeAppointmentEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 200px">
                                                <asp:GridView ID="grdPhysician" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Physician Name" ItemStyle-CssClass="tdParamedicName" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No Data To Display")%>
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
                                        <div id="pagingPhysicanChangeAppointment">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="position: relative;">
                                <h4 class="h4expanded">
                                    <%=GetLabel("Appointment Info")%></h4>
                                <dxcp:ASPxCallbackPanel ID="cbpAppointmentChangeAppointment" runat="server" Width="100%"
                                    ClientInstanceName="cbpAppointmentChangeAppointment" ShowLoadingPanel="false"
                                    OnCallback="cbpAppointmentChangeAppointment_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                        EndCallback="function(s,e){ onCbpAppointmentChangeAppointmentEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2Ctl" runat="server">
                                            <asp:Panel runat="server" ID="Panel1Ctl">
                                                <div class="containerTblEntryContent">
                                                    <table>
                                                        <tr id="trStartLeaveCtl" style="display: none" runat="server">
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <b>
                                                                        <%=GetLabel("Cuti Dari") %>
                                                                    </b>
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStartLeaveCtl" ReadOnly="true" Width="120px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trEndLeaveCtl" style="display: none" runat="server">
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <b>
                                                                        <%=GetLabel("Cuti Sampai") %>
                                                                    </b>
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEndLeaveCtl" ReadOnly="true" Width="120px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trLeaveReasonCtl" style="display: none" runat="server">
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <b>
                                                                        <%=GetLabel("Alasan Cuti") %>
                                                                    </b>
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLeaveReasonCtl" ReadOnly="true" Width="400px" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <colgroup>
                                                            <col width="50%" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr id="trTimeSlotCtl" runat="server">
                                                                        <td class="tdLabel">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Sesi") %></label>
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="cboSessionCtl" ClientInstanceName="cboSessionCtl" Width="100%"
                                                                                runat="server">
                                                                                <ClientSideEvents ValueChanged="function(s,e) { onCboSessionCtlValueChanged(); }" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <input type="hidden" runat="server" id="hdnIsWaitingListCtl" value="0" />
                                                <input type="hidden" runat="server" id="hdnIsByTimeSlotCtl" value="1" />
                                                <input type="hidden" runat="server" id="hdnIsByNoTimeSlotCtl" value="0" />
                                                <input type="hidden" value="" id="hdnRoomIDDefaultCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnRoomCodeDefaultCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnRoomNameDefaultCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnMaxAppoMessageCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnMaxWaitingMessageCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnTotalAppoMessageCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnTotalWaitingMessageCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnIsValidAppoMessageCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnIsValidWaitingMessageCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnStartLeaveCtlValidMessage" runat="server" />
                                                <input type="hidden" value="" id="hdnEndLeaveCtlValidMessage" runat="server" />
                                                <input type="hidden" value="" id="hdnLeaveReasonCtlValidMessage" runat="server" />
                                                <input type="hidden" value="" id="hdnIsShowTimeSlotContainerEntryCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnMaxAppoMessageBPJSCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnTotalAppoMessageBPJSCtl" runat="server" />
                                                <input type="hidden" value="" id="hdnIsValidAppoMessageBPJSCtl" runat="server" />
                                                <div class="containerUlTabPageCtl">
                                                    <ul class="ulTabPage" id="ulTabClinicTransactionCtl">
                                                        <li class="selected" contentid="containerAppointmentCtl" id="containerAppointmentCtl"
                                                            runat="server">
                                                            <%=GetLabel("APPOINTMENT")%></li>
                                                        <li contentid="containerAppointmentNoTimeSlotCtl" id="containerAppointmentNoTimeSlotCtl"
                                                            runat="server">
                                                            <%=GetLabel("APPOINTMENT (NO TIME SLOT)") %></li>
                                                        <li contentid="containerWaitingListCtl" id="containerWaitingListCtl" runat="server">
                                                            <%=GetLabel("WAITING LIST") %></li>
                                                    </ul>
                                                </div>
                                                <div id="containerAppointmentCtl" class="containerAppointmentCtl">
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
                                                            <asp:TemplateField HeaderStyle-Width="70px">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Time") %>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div class="tdTime">
                                                                        <%#: Eval("Time") %></div>
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
                                                                                    <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                                        <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                    <img src='<%#ResolveUrl("~/Libs/Images/Status/cancel.png") %>' height="16px" title="Delete"
                                                                                        <%# Eval("IsAppointmentDeleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                    <div class="divPatientName">
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
                                                                                                        <%#: Eval("VisitTypeName") %></div>
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
                                                <div id="containerAppointmentNoTimeSlotCtl" style="display: none" class="containerAppointmentNoTimeSlotCtl">
                                                    <asp:GridView ID="grdAppointmentNoTimeSlot" runat="server" CssClass="grdSelected grdAppointment"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                        OnRowDataBound="grdAppointmentNoTimeSlot_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-Width="70px">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("No. Urut") %>
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
                                                                                    <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                                        <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                    <img src='<%#ResolveUrl("~/Libs/Images/Status/cancel.png") %>' height="16px" title="Delete"
                                                                                        <%# Eval("IsAppointmentDeleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                    <div class="divPatientName">
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
                                                <div id="containerWaitingListCtl" style="display: none" class="containerWaitingListCtl">
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
                                                                                    <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                                        <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                    <img src='<%#ResolveUrl("~/Libs/Images/Status/cancel.png") %>' height="16px" title="Delete"
                                                                                        <%# Eval("IsAppointmentDeleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                    <div class="divPatientName">
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
                                                                                                        <%#: Eval("VisitTypeName") %></div>
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
                    </tr>
                </table>
            </div>
        </td>
        <td style="padding: 5px; vertical-align: top">
            <table style="width: 100%;">
                <colgroup>
                    <col style="width: 30%" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Appointment No")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppointmentNo" ReadOnly="true" Width="160px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Appointment Status")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppointmentStatus" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Patient Name")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
            </table>
            <h4>
                <%=GetLabel("From") %></h4>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col style="width: 140px" />
                    <col style="width: 30px" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Date / Time")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppointmentDate" ReadOnly="true" Width="120px" runat="server"
                            CssClass="datepicker" />
                    </td>
                    <td style="padding-left: 30px; padding-right: 10px">
                        <%=GetLabel("Hour")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppointmentHour" ReadOnly="true" runat="server" Width="60px"
                            CssClass="time" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Clinic")%></label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="lblRoom">
                            <%=GetLabel("Room")%></label>
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
                                    <asp:TextBox ID="txtRoomCode" ReadOnly="true" Width="100%" runat="server" />
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
                        <label class="lblNormal">
                            <%=GetLabel("Physician")%></label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtPhysician" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Visit Type")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVisitType" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Visit Duration")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVisitDuration" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr id="trAppointmentMethodFrom" runat="server">
                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                        <label class="lblMandatory">
                            <%=GetLabel("Appointment Method")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboAppointmentMethodFrom" ClientInstanceName="cboAppointmentMethodFrom"
                            Width="100%" ReadOnly="true" runat="server">
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
            <h4>
                <%=GetLabel("To") %></h4>
            <fieldset id="fsEntryPopup" style="margin: 0">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 140px" />
                        <col style="width: 30px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Date / Time")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewAppointmentDate" ReadOnly="true" Width="120px" runat="server"
                                CssClass="datepicker" />
                        </td>
                        <td style="padding-left: 30px; padding-right: 10px">
                            <%=GetLabel("Hour")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewAppointmentTime" ReadOnly="true" runat="server" Width="60px"
                                CssClass="time" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Clinic")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewServiceUnit" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblRoomTo">
                                <%=GetLabel("Room")%></label>
                        </td>
                        <td colspan="3">
                            <input type="hidden" id="hdnRoomIDTo" runat="server" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRoomCodeTo" CssClass="required" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRoomNameTo" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Physician")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNewPhysician" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblVisitTypeTo">
                                <%=GetLabel("Visit Type")%></label>
                        </td>
                        <td colspan="3">
                            <input type="hidden" id="hdnVisitTypeIDTo" value="" runat="server" />
                            <input type="hidden" id="hdnVisitDurationTo" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtVisitTypeCodeTo" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVisitTypeNameTo" ReadOnly="true" Width="100%" runat="server" />
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
                                        <asp:TextBox ID="txtVisitDurationTo" Width="100px" runat="server" CssClass="number"
                                            ReadOnly="true" />
                                    </td>
                                    <td>
                                        <input type="button" id="btnMinVisitDurationCtl" value='<%= GetLabel("-")%>' />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input type="button" id="btnPlusVisitDurationCtl" value='<%= GetLabel("+")%>' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trAppointmentMethodTo" runat="server">
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Appointment Method")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboAppointmentMethodTo" ClientInstanceName="cboAppointmentMethodTo"
                                Width="100%" runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--<h4>
                <%=GetLabel("Reason") %></h4>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col style="width: 140px" />
                    <col style="width: 30px" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Reason")%></label>
                    </td>
                    <td colspan="3">
                        <dxe:ASPxComboBox ID="cboChangeReason" ClientInstanceName="cboChangeReason" Width="100%"
                            runat="server">
                            <ClientSideEvents ValueChanged="function(s,e) { oncboChangeReasonValueChanged(); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr id="trChangeOtherReason" style="display: none">
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:textbox id="txtOtherChangeReason" cssclass="required" runat="server" width="100%" />
                    </td>
                </tr>
            </table>--%>
        </td>
    </tr>
</table>
