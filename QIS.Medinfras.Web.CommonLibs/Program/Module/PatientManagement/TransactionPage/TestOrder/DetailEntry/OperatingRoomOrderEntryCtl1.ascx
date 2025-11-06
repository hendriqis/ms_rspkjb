<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatingRoomOrderEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.OperatingRoomOrderEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_OperatingRoomOrderEntryCtl1">
    $(function () {
        setDatePicker('<%=txtOrderDate.ClientID %>');
        setDatePicker('<%=txtScheduleDate.ClientID %>');

        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        if ($('#<%=hdnVisitDepartmentID.ClientID %>').val() == "INPATIENT") {
            $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
            $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
        } else {
            var isChecked = $('#<%=chkIsNextVisit.ClientID %>').is(":checked");
            if (isChecked) {
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
            } else {
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '0');
            }
        }
        $('#<%=rblIsEmergency.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=chkIsCITO.ClientID %>').prop('checked', true);
            }
            else {
                $('#<%=chkIsCITO.ClientID %>').prop('checked', false);
            }
        });

        $('#<%=chkIsNextVisit.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=trNextVisit.ClientID %>').removeAttr("style");

                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
            }
            else {
                $('#<%=trNextVisit.ClientID %>').attr("style", "display:none");

                if ($('#<%=hdnVisitDepartmentID.ClientID %>').val() != "INPATIENT") {
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '0');
                } else {
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
                }
            }
        });

        $('#<%=grdProcedureGroupView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdProcedureGroupView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnOrderDtProcedureGroupID.ClientID %>').val($(this).find('.keyField').html());
        });
        $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

        $('#<%=grdParamedicTeamView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdParamedicTeamView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnOrderDtParamedicTeamID.ClientID %>').val($(this).find('.keyField').html());
        });

        $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();

        //#region Room
        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = '';

            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
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
            if (serviceUnitID != "") {
                Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                    if (result.length == 1) {
                        $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                        $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                        $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                    }
                    else {
                        $('#<%:hdnRoomID.ClientID %>').val('');
                        $('#<%:txtRoomCode.ClientID %>').val('');
                        $('#<%:txtRoomName.ClientID %>').val('');
                    }
                });
            } else {
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        //#region Procedure Group Button
        $('.btnApplyProcedureGroup').click(function () {
            submitProcedureGroup();
            $('#<%=ledProcedureGroup.ClientID %>').focus();
            $('#trProcedureGroup').attr('style', 'display:none');
        });

        $('.btnCancelProcedureGroup').click(function () {
            ResetProcedureGroupEntryControls();
            $('#trProcedureGroup').attr('style', 'display:none');
        });

        function submitProcedureGroup() {
            if ($('#<%=hdnEntryProcedureGroupID.ClientID %>').val() != '') {
                if ($('#<%=hdnProcedureGroupProcessMode.ClientID %>').val() == "1") {
                    var resultFinal = true;
                    var filterExpression = onGetScheduleFilterExpression();
                    Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                        if (result != null) {
                            if ($('#<%=hdnTestOrderID.ClientID %>').val() != '') {
                                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                                var filterExpression2 = "TestOrderID = '" + testOrderID + "' AND IsDeleted = 0";
                                Methods.getObject('GetTestOrderDtProcedureGroupList', filterExpression2, function (result2) {
                                    if (result2 != null) {
                                        cbpProcedureGroup.PerformCallback('add');
                                    }
                                    else {
                                        var message = "Masih ada outstanding jadwal kamar operasi, lanjutkan proses pembuatan jadwal lagi?";
                                        displayConfirmationMessageBox('CONFIRMATION', message, function (result3) {
                                            if (result3) {
                                                cbpProcedureGroup.PerformCallback('add');
                                            }
                                        });
                                        resultFinal = false;
                                    }
                                });
                            }
                            else {
                                var message = "Masih ada outstanding jadwal kamar operasi, lanjutkan proses pembuatan jadwal lagi?";
                                displayConfirmationMessageBox('CONFIRMATION', message, function (result3) {
                                    if (result3) {
                                        cbpProcedureGroup.PerformCallback('add');
                                    }
                                });
                                resultFinal = false;
                            }
                        }
                        else {
                            cbpProcedureGroup.PerformCallback('add');
                        }
                    });
                    return resultFinal;
                    cbpProcedureGroup.PerformCallback('add');
                }
                else {
                    cbpProcedureGroup.PerformCallback('edit');
                }
            }
            else {
                displayErrorMessageBox("ERROR", "Jenis prosedur operasi harus dipilih sebelum diproses !");
            }
        }
        //#endregion

        //#region Paramedic Team
        $('.btnApplyParamedicTeam').click(function () {
            submitParamedicTeam();
            $('#<%=ledParamedicTeam.ClientID %>').focus();
            $('#trParamedicTeam').attr('style', 'display:none');
        });

        $('.btnCancelParamedicTeam').click(function () {
            ResetParamedicTeamEntryControls();
            $('#trParamedicTeam').attr('style', 'display:none');
        });

        function submitParamedicTeam() {
            if ($('#<%=hdnEntryParamedicID.ClientID %>').val() != '' && cboParamedicType.GetValue() != null) {
                if ($('#<%=hdnParamedicTeamProcessMode.ClientID %>').val() == "1") {
                    var resultFinal2 = true;
                    var filterExpression3 = onGetScheduleFilterExpression();
                    Methods.getObject('GetTestOrderHdList', filterExpression3, function (result4) {
                        if (result4 != null) {
                            if ($('#<%=hdnTestOrderID.ClientID %>').val() != '') {
                                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                                var filterExpression4 = "TestOrderID = '" + testOrderID + "' AND IsDeleted = 0";
                                Methods.getObject('GetTestOrderDtParamedicTeamList', filterExpression4, function (result5) {
                                    if (result5 != null) {
                                        cbpParamedicTeam.PerformCallback('add');
                                    }
                                    else {
                                        var message = "Masih ada outstanding jadwal kamar operasi, lanjutkan proses pembuatan jadwal lagi?";
                                        displayConfirmationMessageBox('CONFIRMATION', message, function (result6) {
                                            if (result6) {
                                                cbpParamedicTeam.PerformCallback('add');
                                            }
                                        });
                                        resultFinal2 = false;
                                    }
                                });
                            }
                            else {
                                var message = "Masih ada outstanding jadwal kamar operasi, lanjutkan proses pembuatan jadwal lagi?";
                                displayConfirmationMessageBox('CONFIRMATION', message, function (result6) {
                                    if (result6) {
                                        cbpParamedicTeam.PerformCallback('add');
                                    }
                                });
                                resultFinal2 = false;
                            }
                        }
                        else {
                            cbpParamedicTeam.PerformCallback('add');
                        }
                    });
                    return resultFinal2;
                    cbpParamedicTeam.PerformCallback('add');
                }
                else {
                    cbpParamedicTeam.PerformCallback('edit');
                }
            }
            else {
                displayErrorMessageBox("ERROR", "Dokter/Tenaga Medis harus dipilih sebelum diproses !");
            }
        }
        //#endregion

        $('#<%=rblIsHasInfectiousDisease.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trInfectiousInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trInfectiousInfo.ClientID %>').attr("style", "display:none");
            }
        });

        $('#<%=rblIsHasComorbidities.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trComorbiditiesInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trComorbiditiesInfo.ClientID %>').attr("style", "display:none");
            }
        });

        $('#leftPageNavPanel ul li').first().click();

        $('#lblPhysicianNoteID').removeClass('lblLink');

    });

    //#region Procedure Group
    function onLedProcedureGroupLostFocus(s) {
        var procedureGroupID = s.GetValueText();
        $('#<%=hdnEntryProcedureGroupID.ClientID %>').val(procedureGroupID);
        $('#<%=hdnEntryProcedureGroupText.ClientID %>').val(s.GetDisplayText());
    }

    function GetCurrentSelectedProcedureGroup(s) {
        var $tr = $(s).closest('tr').parent().closest('tr');
        var idx = $('#<%=grdProcedureGroupView.ClientID %> tr').index($tr);
        $('#<%=grdProcedureGroupView.ClientID %> tr:eq(' + idx + ')').click();

        $row = $('#<%=grdProcedureGroupView.ClientID %> tr.selected');
        var selectedObj = {};

        $row.find('input[type=hidden]').each(function () {
            selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });
        return selectedObj;
    }

    function SetProcedureGroupEntityToControl(param) {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedProcedureGroup(param);
        ledProcedureGroup.SetValue(selectedObj.ProcedureGroupID);
        $('#<%=hdnEntryProcedureGroupID.ClientID %>').val(selectedObj.ProcedureGroupID);
    }

    function ResetProcedureGroupEntryControls(s) {
        ledProcedureGroup.SetValue('');
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val('1');
    }

    $('.imgAddProcedureGroup.imgLink').die('click');
    $('.imgAddProcedureGroup.imgLink').live('click', function (evt) {
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val("1");
        ResetProcedureGroupEntryControls();
        $('#trProcedureGroup').removeAttr('style');
    });

    $('.imgEditProcedureGroup.imgLink').die('click');
    $('.imgEditProcedureGroup.imgLink').live('click', function () {
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val("0");
        SetProcedureGroupEntityToControl(this);
        $('#trProcedureGroup').removeAttr('style');
    });

    $('.imgDeleteProcedureGroup.imgLink').die('click');
    $('.imgDeleteProcedureGroup.imgLink').live('click', function () {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedProcedureGroup(this);

        var message = "Hapus Jenis Operasi <b>'" + selectedObj.ProcedureGroupName + "'</b> untuk pasien ini ?";
        displayConfirmationMessageBox('JENIS OPERASI', message, function (result) {
            if (result) {
                cbpProcedureGroup.PerformCallback('delete');
            }
        });
    });

    var pageCount = parseInt('<%=gridProcedureGroupPageCount %>');
    $(function () {
        setPaging($("#procedureGroupPaging"), pageCount, function (page) {
            cbpProcedureGroupView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpProcedureGroupViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0) {
                $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
            }

            setPaging($("#procedureGroupPaging"), pageCount, function (page) {
                cbpProcedureGroupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
    }

    function onCbpProcedureGroupEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit") {
                $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val('0');
            }

            $('#<%=hdnTestOrderID.ClientID %>').val(param[2]);

            ResetProcedureGroupEntryControls();
            cbpProcedureGroupView.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            displayErrorMessageBox('JENIS OPERASI', param[2]);
        }
        else
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    //#region ParamedicTeam

    var pageCount2 = parseInt('<%=gridParamedicTeamPageCount %>');
    $(function () {
        setPaging($("#paramedicTeamPaging"), pageCount2, function (page) {
            cbpParamedicTeamView.PerformCallback('changepage|' + page);
        });
    });

    function getSelectedParamedicTeam() {
        return $('#<%=grdParamedicTeamView.ClientID %> tr.selected');
    }

    function oncbpParamedicTeamEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit") {
                $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val('0');
            }

            $('#<%=hdnTestOrderID.ClientID %>').val(param[2]);

            ResetParamedicTeamEntryControls();
            cbpParamedicTeamView.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            displayErrorMessageBox("Team Pelaksana", 'Error Message : ' + param[2]);
        }
        else
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
    }

    function onLedParamedicLostFocus(led) {
        var paramedicID = led.GetValueText();
        $('#<%=hdnEntryParamedicID.ClientID %>').val(paramedicID);
    }

    function GetCurrentSelectedParamedicTeam(s) {
        var $tr = $(s).closest('tr').parent().closest('tr');
        var idx = $('#<%=grdParamedicTeamView.ClientID %> tr').index($tr);
        $('#<%=grdParamedicTeamView.ClientID %> tr:eq(' + idx + ')').click();

        $row = $('#<%=grdParamedicTeamView.ClientID %> tr.selected');
        var selectedObj = {};

        $row.find('input[type=hidden]').each(function () {
            selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });

        return selectedObj;
    }

    function SetParamedicTeamEntityToControl(param) {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedParamedicTeam(param);

        ledParamedicTeam.SetValue(selectedObj.ParamedicID);
        cboParamedicType.SetValue(selectedObj.GCParamedicRole);

        $('#<%=hdnEntryParamedicID.ClientID %>').val(selectedObj.ParamedicID);
    }


    $('.imgAddParamedicTeam.imgLink').die('click');
    $('.imgAddParamedicTeam.imgLink').live('click', function (evt) {
        $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val("1");
        ResetParamedicTeamEntryControls();
        $('#trParamedicTeam').removeAttr('style');
    });

    $('.imgEditParamedicTeam.imgLink').die('click');
    $('.imgEditParamedicTeam.imgLink').live('click', function () {
        $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val('0');
        SetParamedicTeamEntityToControl(this);
        $('#trParamedicTeam').removeAttr('style');
    });

    $('.imgDeleteParamedicTeam.imgLink').die('click');
    $('.imgDeleteParamedicTeam.imgLink').live('click', function () {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedParamedicTeam(this);

        var message = "Hapus Dokter/Tenaga Medis <b>'" + selectedObj.ParamedicName + "'</b> dari order kamar operasi ?";
        displayConfirmationMessageBox('TEAM PELAKSANA', message, function (result) {
            if (result) {
                cbpParamedicTeam.PerformCallback('delete');
            }
        });
    });

    function ResetParamedicTeamEntryControls(s) {
        ledParamedicTeam.SetValue('');
        $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val('1');
    }

    function oncbpParamedicTeamViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();

            setPaging($("#paramedicPaging"), pageCount2, function (page) {
                cbpParamedicTeamView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onGetScheduleFilterExpression() {
        var visitID = $('#<%=hdnParameterVisitID.ClientID %>').val();
        var scheduleDate = $('#<%=txtScheduleDate.ClientID %>').val();
        var scheduleDateInDatePicker = Methods.getDatePickerDate(scheduleDate);
        var scheduleDateFormatString = Methods.dateToString(scheduleDateInDatePicker);
        var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = "VisitID = " + visitID + " AND HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND ScheduledDate = '" + scheduleDateFormatString + "' AND GCTransactionStatus NOT IN ('X121^999','X121^005') AND GCOrderStatus NOT IN ('X126^006','X121^005') AND TestOrderID != '" + testOrderID + "'";
        return filterExpression;
    }

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        var filterExpression = onGetScheduleFilterExpression();
        Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
            if (result != null) {
                errMessage.text = "Masih ada outstanding jadwal kamar operasi, lanjutkan proses pembuatan jadwal lagi?";
                resultFinal = false;
            }
        });
        return resultFinal;
    }

    function onCboGCInfectiousDiseaseChanged(s) {
        if (cboGCInfectiousDisease.GetValue() != '' && cboGCInfectiousDisease.GetValue() != null && cboGCInfectiousDisease.GetValue() == "X522^999") {
            $('#<%:txtOtherInfectiousDisease.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtOtherInfectiousDisease.ClientID %>').attr('readonly', 'readonly');
        }
    }

    function onCboGCComorbiditiesChanged(s) {
        if (cboGCComorbidities.GetValue() != '' && cboGCComorbidities.GetValue() != null && cboGCComorbidities.GetValue() == "X523^999") {
            $('#<%:txtOtherComorbidities.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtOtherComorbidities.ClientID %>').attr('readonly', 'readonly');
        }
    }
</script>
<style type="text/css">
    
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnParameterRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnParameterVisitID" value="" />
    <input type="hidden" runat="server" id="hdnParameterParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
    <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
    <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Jadwal Kamar Operasi" class="w3-hover-red">Jadwal Operasi</li>
                        <li contentid="divPage2" title="Jenis Operasi" class="w3-hover-red">Jenis Operasi</li>
                        <li contentid="divPage3" title="Team Pelaksana" class="w3-hover-red">Team Pelaksana</li>
                        <li contentid="divPage4" title="Permohonan Khusus" class="w3-hover-red">Permohonan Khusus</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:aspxcombobox id="cboParamedicID" width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Estimasi Lama Operasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" />
                                menit
                            </td>
                            <td style="padding-left: 5px">
                                <asp:CheckBox ID="chkIsUsedRequestTime" Width="180px" runat="server" Text=" Permintaan Jam Khusus" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Jadwal Operasi") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsEmergency" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Emergency" Value="1" />
                                    <asp:ListItem Text=" Elektif" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Rencana")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                        <td />
                                        <td>
                                            <asp:CheckBox ID="chkIsNextVisit" Width="200px" runat="server" Text=" Kunjungan Berikutnya" />
                                        </td>
                                        <td style="display: none">
                                            <div id="divScheduleInfo" runat="server" style="display: none">
                                                <input type="button" class="btnSchedule w3-btn w3-hover-blue" value="Info Jadwal"
                                                    style="background-color: Green; color: White; width: 100px" /></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trNextVisit" runat="server" style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Pilih Kunjungan Berikutnya") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblNextVisitType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" ODS" Value="1" />
                                    <asp:ListItem Text=" Rawat Inap" Value="2" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" runat="server" id="lblRoom">
                                    <%:GetLabel("Ruang Operasi")%></label>
                            </td>
                            <td colspan="2">
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
                                        <td colspan="">
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Riwayat Penyakit Infeksi") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasInfectiousDisease" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trInfectiousInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Penyakit Infeksi")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col style="width: 80px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:aspxcombobox runat="server" id="cboGCInfectiousDisease" clientinstancename="cboGCInfectiousDisease"
                                                width="99%" tooltip="Tipe Penyakit Infeksi">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCInfectiousDiseaseChanged(s); }" />
                                            </dxe:aspxcombobox>
                                        </td>
                                        <td class="tdLabel" style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherInfectiousDisease" CssClass="txtOtherInfectiousDisease"
                                                runat="server" Width="100%" ReadOnly />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Memiliki Komorbid") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasComorbidities" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trComorbiditiesInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Komorbid")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col style="width: 80px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:aspxcombobox runat="server" id="cboGCComorbidities" clientinstancename="cboGCComorbidities"
                                                width="99%" tooltip="Tipe Komorbid">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCComorbiditiesChanged(s); }" />
                                            </dxe:aspxcombobox>
                                        </td>
                                        <td class="tdLabel" style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherComorbidities" CssClass="txtOtherComorbidities" runat="server"
                                                Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                        <tr id="trProcedureGroup" style="display: none">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="150px" />
                                        <col width="150px" />
                                        <col width="100px" />
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Jenis Operasi")%></label>
                                        </td>
                                        <td colspan="4">
                                            <input type="hidden" value="" id="hdnEntryProcedureGroupID" runat="server" />
                                            <input type="hidden" value="" id="hdnEntryProcedureGroupText" runat="server" />
                                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                                <colgroup>
                                                    <col style="width: 95%" />
                                                    <col style="width: 5%" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <qis:qissearchtextbox id="ledProcedureGroup" clientinstancename="ledProcedureGroup"
                                                            runat="server" width="99%" valuetext="ProcedureGroupID" filterexpression="" displaytext="ProcedureGroupName"
                                                            methodname="GetProcedureGroupList">
                                                            <ClientSideEvents ValueChanged="function(s){ onLedProcedureGroupLostFocus(s); }" />
                                                            <Columns>
                                                                <qis:QISSearchTextBoxColumn Caption="Jenis Operasi (Kode)" FieldName="ProcedureGroupName"
                                                                    Description="i.e. Appendictomy" Width="500px" />
                                                            </Columns>
                                                        </qis:qissearchtextbox>
                                                    </td>
                                                    <td>
                                                        <table border="0" cellpadding="1" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="btnApplyProcedureGroup imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td>
                                                                    <img class="btnCancelProcedureGroup imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                        alt="" />
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
                        </tr>
                        <tr>
                            <td>
                                <div style="position: relative;">
                                    <dxcp:aspxcallbackpanel id="cbpProcedureGroupView" runat="server" width="100%" clientinstancename="cbpProcedureGroupView"
                                        showloadingpanel="false" oncallback="cbpProcedureGroupView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdProcedureGroupView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("ProcedureGroupID") %>" bindingfield="ProcedureGroupID" />
                                                                    <input type="hidden" value="<%#:Eval("ProcedureGroupCode") %>" bindingfield="ProcedureGroupCode" />
                                                                    <input type="hidden" value="<%#:Eval("ProcedureGroupName") %>" bindingfield="ProcedureGroupName" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                <HeaderTemplate>
                                                                    <img class="imgAddProcedureGroup imgLink" title='<%=GetLabel("+ Jenis Operasi")%>'
                                                                        src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>' alt="" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEditProcedureGroup imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDeleteProcedureGroup imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Jenis Operasi")%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <%#: Eval("ProcedureGroupName")%></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Kategori Operasi" DataField="SurgeryClassification" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:aspxcallbackpanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="procedureGroupPaging">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <div style="position: relative;">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <tr id="trParamedicTeam" style="display: none">
                                <td>
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <colgroup>
                                            <col width="100px" />
                                            <col width="100px" />
                                            <col width="100px" />
                                            <col width="100px" />
                                            <col width="100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Dokter/Tenaga Medis")%></label>
                                            </td>
                                            <td colspan="3">
                                                <input type="hidden" value="" id="hdnEntryParamedicID" runat="server" />
                                                <qis:qissearchtextbox id="ledParamedicTeam" clientinstancename="ledParamedicTeam"
                                                    runat="server" width="99%" valuetext="ParamedicID" filterexpression="IsDeleted = 0" displaytext="ParamedicName"
                                                    methodname="GetvParamedicMasterList">
                                                    <ClientSideEvents ValueChanged="function(s){ onLedParamedicLostFocus(s); }" />
                                                    <Columns>
                                                        <qis:QISSearchTextBoxColumn Caption="Nama Dokter/Tenaga Medis" FieldName="ParamedicName"
                                                            Description="i.e. samuel" Width="500px" />
                                                    </Columns>
                                                </qis:qissearchtextbox>
                                            </td>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Peran Team")%></label>
                                            </td>
                                            <td>
                                                <dxe:aspxcombobox runat="server" id="cboParamedicType" clientinstancename="cboParamedicType"
                                                    width="100px">
                                                </dxe:aspxcombobox>
                                            </td>
                                            <td style="padding-left: 5px">
                                                <table border="0" cellpadding="0" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <img class="btnApplyParamedicTeam imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td>
                                                            <img class="btnCancelParamedicTeam imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                    <dxcp:aspxcallbackpanel id="cbpParamedicTeamView" runat="server" width="100%" clientinstancename="cbpParamedicTeamView"
                                        showloadingpanel="false" oncallback="cbpParamedicTeamView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ oncbpParamedicTeamViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdParamedicTeamView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                                    <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                <HeaderTemplate>
                                                                    <img class="imgAddParamedicTeam imgLink" title='<%=GetLabel("+ Team")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                        alt="" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEditParamedicTeam imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDeleteParamedicTeam imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Tenaga Medis" HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" HeaderStyle-Width="150px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data team pelaksana untuk order ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:aspxcallbackpanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="paramedicPaging">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingSpecificItem" Width="180px" runat="server" Text=" Penggunaan Alat Tertentu" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Order") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="250px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
    <dxcp:aspxcallbackpanel id="cbpProcedureGroup" runat="server" width="100%" clientinstancename="cbpProcedureGroup"
        showloadingpanel="false" oncallback="cbpProcedureGroup_Callback">
        <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupEndCallback(s); }" />
    </dxcp:aspxcallbackpanel>
    <dxcp:aspxcallbackpanel id="cbpParamedicTeam" runat="server" width="100%" clientinstancename="cbpParamedicTeam"
        showloadingpanel="false" oncallback="cbpParamedicTeam_Callback">
        <ClientSideEvents EndCallback="function(s,e){ oncbpParamedicTeamEndCallback(s); }" />
    </dxcp:aspxcallbackpanel>
</div>
