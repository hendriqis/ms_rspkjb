<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    ValidateRequest="false" AutoEventWireup="true" CodeBehind="ERInitialAssessment1.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ERInitialAssessment1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
        
            setDatePicker('<%=txtTriageDate.ClientID %>');
            $('#<%=txtTriageDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtEmergencyCaseDate.ClientID %>');
            $('#<%=txtEmergencyCaseDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));            
            
            $('#<%=txtEmergencyCaseDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtEmergencyCaseTime1.ClientID %>').change(function () {
                if ($('#<%=txtEmergencyCaseTime1.ClientID %>').val() >= 0 && $('#<%=txtEmergencyCaseTime1.ClientID %>').val() < 24 && $('#<%=txtEmergencyCaseTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtEmergencyCaseTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtEmergencyCaseTime2.ClientID %>').change(function () {
                if ($('#<%=txtEmergencyCaseTime2.ClientID %>').val() >= 0 && $('#<%=txtEmergencyCaseTime2.ClientID %>').val() < 60 && $('#<%=txtEmergencyCaseTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtEmergencyCaseTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceDate.ClientID %>').change(function () {
                HourDifference();
                var hdnER0010 = $('#<%=hdnER0010.ClientID %>').val();
                if(hdnER0010 == '1') {
                    $('#<%=txtTriageDate.ClientID %>').val($('#<%=txtServiceDate.ClientID %>').val());
                }
            });

            $('#<%=txtServiceTime1.ClientID %>').change(function () {
                var hdnER0010 = $('#<%=hdnER0010.ClientID %>').val();
                if ($('#<%=txtServiceTime1.ClientID %>').val() >= 0 && $('#<%=txtServiceTime1.ClientID %>').val() < 60 && $('#<%=txtServiceTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                    if(hdnER0010 == '1') {
                        $('#<%=txtTriageTime1.ClientID %>').val($('#<%=txtServiceTime1.ClientID %>').val());
                    }
                } else {
                    $('#<%=txtServiceTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    if(hdnER0010 == '1') {
                        $('#<%=txtTriageTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    }
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceTime2.ClientID %>').change(function () {
                var hdnER0010 = $('#<%=hdnER0010.ClientID %>').val();
                if ($('#<%=txtServiceTime2.ClientID %>').val() >= 0 && $('#<%=txtServiceTime2.ClientID %>').val() < 60 && $('#<%=txtServiceTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                    if(hdnER0010 == '1') {
                        $('#<%=txtTriageTime2.ClientID %>').val($('#<%=txtServiceTime2.ClientID %>').val());
                    }
                } else {
                    $('#<%=txtServiceTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    if(hdnER0010 == '1') {
                        $('#<%=txtTriageTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    }
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });
            
            $('#<%=txtTriageDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtTriageTime1.ClientID %>').change(function () {
                if ($('#<%=txtTriageTime1.ClientID %>').val() >= 0 && $('#<%=txtTriageTime1.ClientID %>').val() < 60 && $('#<%=txtTriageTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtTriageTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtTriageTime2.ClientID %>').change(function () {
                if ($('#<%=txtTriageTime2.ClientID %>').val() >= 0 && $('#<%=txtTriageTime2.ClientID %>').val() < 60 && $('#<%=txtTriageTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtTriageTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtRegistrationTime.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDiagnosisID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            $('#<%=grdEducationView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdEducationView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnPatientEducationID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdEducationView.ClientID %> tr:eq(1)').click();

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            $('.btnApplyDiagnosis').click(function () {
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
                $('#<%=hdnEntryDiagnoseID.ClientID %>').val('');
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
                $('#<%=hdnEntryDiagnoseID.ClientID %>').val('');
            });

            function submitDiagnosis()
            {
                if ($('#<%=hdnEntryDiagnoseID.ClientID %>').val() != '') {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1")
                        cbpDiagnosis.PerformCallback('add');
                    else
                        cbpDiagnosis.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "Masalah/Diagnosa keperawatan belum dipilih !");
                }
            }

            if ($('#<%=hdnDischargePlanningValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnDischargePlanningValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent4.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.optDischarge').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }

            HourDifference();

            registerCollapseExpandHandler();
        });

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

        //#region Vital Sign
        var pageCountVitalSign = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCountVitalSign, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            }, 8);
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() != "0") {
                onBeforeOpenTrxPopup();
                var visitNoteID = <%=HttpUtility.HtmlEncode(GetVisitNoteID())%>;
                var param = "0||" + visitNoteID + "|1|1";
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("Asesment Awal","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });


        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var visitNoteID = <%=HttpUtility.HtmlEncode(GetVisitNoteID())%>;
            var param = "0|" +$('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Vital Sign & Indicator", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Are you sure to delete this vital sign record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteVitalSign.PerformCallback();
                }
            });
        });

        function onCbpVitalSignViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

                setPaging($("#vitalSignPaging"), pageCount, function (page) {
                    cbpVitalSignView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
        }

        function onCbpVitalSignDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpVitalSignView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
        }
        //#endregion
        
        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                $('#<%=hdnID.ClientID %>').val(retval);
            }
        }

        function onAfterCustomClickSuccessSetRecordID(retval) {
            if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '' || $('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '0') {
                $('#<%=hdnPatientVisitNoteID.ClientID %>').val(retval);
            }
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry'))
                getDischargePlanningFormValues();
                onCustomButtonClick('save');
        });


        function HourDifference() {
            var EmergencyCaseDateInString = $('#<%=txtEmergencyCaseDate.ClientID %>').val();
            var EmergencyCaseTime = $('#<%=txtEmergencyCaseTime1.ClientID %>').val() + ":" + $('#<%=txtEmergencyCaseTime2.ClientID %>').val();

            //registration difference
            var registrationDateInString = $('#<%=txtRegistrationDate.ClientID %>').val();
            var registrationTime = $('#<%=txtRegistrationTime.ClientID %>').val();

            //service difference
            var serviceDateInString = $('#<%=txtServiceDate.ClientID  %>').val();
            var serviceTime = $('#<%=txtServiceTime1.ClientID %>').val() + ":" + $('#<%=txtServiceTime2.ClientID %>').val();

            var EmergencyCaseDate = Methods.getDatePickerDate(EmergencyCaseDateInString);
            var registrationDate = Methods.getDatePickerDate(registrationDateInString);
            var serviceDate = Methods.getDatePickerDate(serviceDateInString);

            var dateDiff = Methods.calculateDateDifference(registrationDate, serviceDate);
            $h1 = parseInt(serviceTime.substring(0, 2), 10);
            $m1 = parseInt(serviceTime.substring(3, 5), 10);

            $h2 = parseInt(registrationTime.substring(0, 2), 10);
            $m2 = parseInt(registrationTime.substring(3, 5), 10);

            var serviceDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2, "1");

            $('#<%=txtServiceDateDiff.ClientID %>').val(serviceDateDiff);


            dateDiff = Methods.calculateDateDifference(EmergencyCaseDate, registrationDate);

            $h1 = parseInt(registrationTime.substring(0, 2), 10);
            $m1 = parseInt(registrationTime.substring(3, 5), 10);

            $h2 = parseInt(EmergencyCaseTime.substring(0, 2), 10);
            $m2 = parseInt(EmergencyCaseTime.substring(3, 5), 10);

            var registrationDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2, "0");

            $('#<%=txtRegistrationDateDiff.ClientID %>').val(registrationDateDiff);
        }

        function countHour(days, h1, m1, h2, m2, index) {
            if ($m1 < $m2) {
                $m = $m1 + 60 - $m2;
                $h1 -= 1;
            }
            else $m = $m1 - $m2;

            if (days > 0)
                $h1 = days * 24 + $h1;
            $h = $h1 - $h2;

            if (index == "1") {
                $('#<%=hdnTimeElapsed1hour.ClientID %>').val($h);
                $('#<%=hdnTimeElapsed1minute.ClientID %>').val($m);
            }
            else {
                $('#<%=hdnTimeElapsed0hour.ClientID %>').val($h);
                $('#<%=hdnTimeElapsed0minute.ClientID %>').val($m);
            }
            return $h + " Jam " + $m + " Menit";
        }

        function onCboOnsetChanged(s) {
            $txt = $('#<%=txtOnset.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboProvocationChanged(s) {
            $txt = $('#<%=txtProvocation.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboQualityChanged(s) {
            $txt = $('#<%=txtQuality.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboSeverityChanged(s) {
            $txt = $('#<%=txtSeverity.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboTimeChanged(s) {
            $txt = $('#<%=txtTime.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboRelievedByChanged(s) {
            $txt = $('#<%=txtRelievedBy.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER || cboVisitReason.GetValue() == Constant.VisitReason.ACCIDENT)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        $('#btnBedQuickPicks').live('click', function () {
            var url = ResolveUrl('~/Controls/BedQuickPicksCtl.ascx');
            openUserControlPopup(url, '', 'Pilih Tempat Tidur', 1150, 550);
        });
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();

            if (code == 'ER090314') {
                filterExpression.text = registrationID;
                    return true;
                }
            
        }

        function onAfterClickBedQuickPicks(healthcareServiceUnitID, serviceUnitCode, serviceUnitName, roomID, roomCode, roomName, classID, classCode, className, chargeClassID, chargeClassBPJSCode, chargeClassCode, chargeClassName, bedID, bedCode, chargeClassBPJSType) {
            $('#<%:hdnClassID.ClientID %>').val(classID);
            $('#<%:txtClassCode.ClientID %>').val(classCode);
            $('#<%:txtClassName.ClientID %>').val(className);
            $('#<%:hdnChargeClassID.ClientID %>').val(classID);
            $('#<%:txtChargeClassCode.ClientID %>').val(classCode);
            $('#<%:txtChargeClassName.ClientID %>').val(className);
            $('#<%:hdnRoomID.ClientID %>').val(roomID);
            $('#<%:txtRoomCode.ClientID %>').val(roomCode);
            $('#<%:txtRoomName.ClientID %>').val(roomName);
            $('#<%:hdnBedID.ClientID %>').val(bedID);
            $('#<%:txtBedCode.ClientID %>').val(bedCode);
        }

        //#region Nursing Diagnosis
        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
        }

        function GetCurrentSelectedDiagnosis(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdDiagnosisView.ClientID %> tr').index($tr);
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdDiagnosisView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetDiagnosisEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnosis(param);

            ledDiagnose.SetValue(selectedObj.ProblemID);
        }

        function ResetDiagnosisEntryControls(s) {
            ledDiagnose.SetValue('');
        }

        $('.imgEditDiagnosis.imgLink').die('click');
        $('.imgEditDiagnosis.imgLink').live('click', function () {
            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val("0");
            SetDiagnosisEntityToControl(this);
        });

        $('.imgDeleteDiagnosis.imgLink').die('click');
        $('.imgDeleteDiagnosis.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnosis(this);

            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val("0");
            var message = "Hapus masalah keperawatan <b>'" + selectedObj.ProblemName + "'</b> untuk pasien ini ?";
            displayConfirmationMessageBox('MASALAH KEPERAWATAN', message, function (result) {
                if (result) {
                    cbpDiagnosis.PerformCallback('delete');
                }
            });
        });

        var pageCount = parseInt('<%=gridDiagnosisPageCount %>');
        $(function () {
            setPaging($("#diagnosisPaging"), pageCount, function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpDiagnosisViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var isMainDiagnosisExists = s.cpRetval;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

                setPaging($("#diagnosisPaging"), pageCount, function (page) {
                    cbpDiagnosisView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }

        function onCbpDiagnosisEndCallback(s) {
            var param = s.cpResult.split('|');
            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('1');
            if (param[0] == '1') {
                ResetDiagnosisEntryControls();
                cbpDiagnosisView.PerformCallback('refresh');
            }
            else if (param[0] == '0') {
                displayErrorMessageBox('MASALAH KEPERAWATAN', param[2]);
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Patient Education
        $('#lblAddEducation').die('click');
        $('#lblAddEducation').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var visitNoteID = <%=HttpUtility.HtmlEncode(GetVisitNoteID()) %>;
            var param = "|" + visitNoteID + "|1|1";            
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/PatientEducation/PatientEducationCtl.ascx", param, "Edukasi Pasien", 700, 500);
        });

        $('.imgEditEducation.imgLink').die('click');
        $('.imgEditEducation.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var visitNoteID = <%=HttpUtility.HtmlEncode(GetVisitNoteID())%>;
            var param = $('#<%=hdnPatientEducationID.ClientID %>').val() + "|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/PatientEducation/PatientEducationCtl.ascx", param, "Edukasi Pasien", 700, 500);
        });

        $('.imgDeleteEducation.imgLink').die('click');
        $('.imgDeleteEducation.imgLink').live('click', function () {
            var message = "Hapus data kebutuhan edukasi pasien ?";
            displayConfirmationMessageBox('Kebutuhan Edukasi Pasien', message, function (result) {
                if (result) {
                    cbpDeleteEducation.PerformCallback();
                }
            });
        });

        function onRefreshEducationGrid() {
            cbpEducationView.PerformCallback('refresh');
        }

        var pageCount = parseInt('<%=gridEducationPageCount %>');
        $(function () {
            setPaging($("#educationPaging"), pageCount, function (page) {
                cbpEducationView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpEducationViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdEducationView.ClientID %> tr:eq(1)').click();

                setPaging($("#educationPaging"), pageCount, function (page) {
                    cbpEducationView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdEducationView.ClientID %> tr:eq(1)').click();
        }

        function onCbpDeleteEducationEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpEducationView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox("ERROR", param[1]);
            }
        }
        //#endregion

        //#region Survey Primer
        function onCboAirwayChanged(s) {
            var filterExpression = "StandardCodeID = '" + cboAirway.GetValue() + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtScoreAirway.ClientID %>').val(result.TagProperty);
                }
                else {
                    $('#<%:txtScoreAirway.ClientID %>').val('');
                }
            });
            calculateTRIAGEScore();
        }

        function onCboBreathingChanged(s) {
            var filterExpression = "StandardCodeID = '" + cboBreathing.GetValue() + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtScoreBreathing.ClientID %>').val(result.TagProperty);
                }
                else {
                    $('#<%:txtScoreBreathing.ClientID %>').val('');
                }
            });
            calculateTRIAGEScore();
        }

        function onCboCirculationChanged(s) {
            var filterExpression = "StandardCodeID = '" + cboCirculation.GetValue() + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtScoreCirculation.ClientID %>').val(result.TagProperty);
                }
                else {
                    $('#<%:txtScoreCirculation.ClientID %>').val('');
                }
            });
            calculateTRIAGEScore();
        }

        function onCboDisabilityChanged(s) {
            var filterExpression = "StandardCodeID = '" + cboDisability.GetValue() + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtScoreDisability.ClientID %>').val(result.TagProperty);
                }
                else {
                    $('#<%:txtScoreDisability.ClientID %>').val('');
                }
            });
            calculateTRIAGEScore();
        }

        function onCboExposureChanged(s) {
            var filterExpression = "StandardCodeID = '" + cboExposure.GetValue() + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtScoreExposure.ClientID %>').val(result.TagProperty);
                }
                else {
                    $('#<%:txtScoreExposure.ClientID %>').val('');
                }
            });
            calculateTRIAGEScore();
        }

        function calculateTRIAGEScore() {
            var p1 = 0;
            var p2 = 0;
            var p3 = 0;
            var p4 = 0;
            var p5 = 0;

            if ($('#<%=txtScoreAirway.ClientID %>').val())
                p1 = parseInt($('#<%=txtScoreAirway.ClientID %>').val());

            if ($('#<%=txtScoreBreathing.ClientID %>').val())
                p2 = parseInt($('#<%=txtScoreBreathing.ClientID %>').val());

            if ($('#<%=txtScoreCirculation.ClientID %>').val())
                p3 = parseInt($('#<%=txtScoreCirculation.ClientID %>').val());

            if ($('#<%=txtScoreDisability.ClientID %>').val())
                p4 = parseInt($('#<%=txtScoreDisability.ClientID %>').val());

            if ($('#<%=txtScoreExposure.ClientID %>').val())
                p5 = parseInt($('#<%=txtScoreExposure.ClientID %>').val());

            var total = p1 + p2 + p3 + p4 + p5;
            $('#<%=txtTotalScoreTriage.ClientID %>').val(total);

            if (total >= 1 && total <= 4) {
                cboTriage.SetValue("X079^005");
            }
            else if (total >= 5 && total <= 6) {
                cboTriage.SetValue("X079^004");
            }
            else if (total >= 7 && total <= 8) {
                cboTriage.SetValue("X079^003");
            }
            else if (total == 9) {
                cboTriage.SetValue("X079^002");
            }
            else if (total >= 10 && total <= 13) {
                cboTriage.SetValue("X079^001");
            }
            else if (total >= 14 && total <= 17) {
                cboTriage.SetValue("X079^006");
            }
        }
        //#enregion

        //#region RAPUH SCORE CALCULATION
        function onCboRAPUH_R_Changed(s) {
            if (cboRAPUH_R.GetValue() != null) {
                if (cboRAPUH_R.GetValue().indexOf('^01') > -1) {
                    $('#<%=txtRAPUH_R.ClientID %>').val('1');
                }
                else {
                    $('#<%=txtRAPUH_R.ClientID %>').val('0');
                }
            }
            calculateRAPUHScore();
        }
        function onCboRAPUH_A_Changed(s) {
            if (cboRAPUH_A.GetValue() != null) {
                if (cboRAPUH_A.GetValue().indexOf('^01') > -1) {
                    $('#<%=txtRAPUH_A.ClientID %>').val('1');
                }
                else if (cboRAPUH_A.GetValue().indexOf('^02') > -1) {
                    $('#<%=txtRAPUH_A.ClientID %>').val('1');
                }
                else if (cboRAPUH_A.GetValue().indexOf('^03') > -1) {
                    $('#<%=txtRAPUH_A.ClientID %>').val('0');
                }
                else {
                    $('#<%=txtRAPUH_A.ClientID %>').val('0');
                }
            }
            calculateRAPUHScore();
        }
        function onCboRAPUH_P_Changed(s) {
            if (cboRAPUH_P.GetValue() != null) {
                if (cboRAPUH_P.GetValue().indexOf('^01') > -1) {
                    $('#<%=txtRAPUH_P.ClientID %>').val('0');
                }
                else {
                    $('#<%=txtRAPUH_P.ClientID %>').val('1');
                }
            }
            calculateRAPUHScore();
        }
        function onCboRAPUH_U_Changed(s) {
            if (cboRAPUH_U.GetValue() != null) {
                if (cboRAPUH_U.GetValue().indexOf('^01') > -1) {
                    $('#<%=txtRAPUH_U.ClientID %>').val('1');
                }
                else {
                    $('#<%=txtRAPUH_U.ClientID %>').val('0');
                }
            }
            calculateRAPUHScore();
        }
        function onCboRAPUH_H_Changed(s) {
            if (cboRAPUH_H.GetValue() != null) {
                if (cboRAPUH_H.GetValue().indexOf('^01') > -1) {
                    $('#<%=txtRAPUH_H.ClientID %>').val('0');
                }
                else {
                    $('#<%=txtRAPUH_H.ClientID %>').val('1');
                }
            }
            calculateRAPUHScore();
        }

        function calculateRAPUHScore() {
            var p1 = 0;
            var p2 = 0;
            var p3 = 0;
            var p4 = 0;
            var p5 = 0;

            if ($('#<%=txtRAPUH_R.ClientID %>').val())
                p1 = parseInt($('#<%=txtRAPUH_R.ClientID %>').val());

            if ($('#<%=txtRAPUH_A.ClientID %>').val())
                p2 = parseInt($('#<%=txtRAPUH_A.ClientID %>').val());

            if ($('#<%=txtRAPUH_P.ClientID %>').val())
                p3 = parseInt($('#<%=txtRAPUH_P.ClientID %>').val());

            if ($('#<%=txtRAPUH_U.ClientID %>').val())
                p4 = parseInt($('#<%=txtRAPUH_U.ClientID %>').val());

            if ($('#<%=txtRAPUH_H.ClientID %>').val())
                p5 = parseInt($('#<%=txtRAPUH_H.ClientID %>').val());

            var total = p1 + p2 + p3 + p4 + p5;
            $('#<%=txtRAPUHScore.ClientID %>').val(total);

            if (total <= 0) {
                cboRAPUHScore.SetValue("X096^01");
            }
            else if (total >= 1 && total <= 2) {
                cboRAPUHScore.SetValue("X096^02");
            }
            else {
                cboRAPUHScore.SetValue("X096^03");
            }
        }

        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        $('#h4TandaVital').live('click', function () {
            onRefreshVitalSignGrid();
        });

        function getDischargePlanningFormValues() {
            var controlValues = '';
            $('#<%=divFormContent4.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent4.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent4.ClientID %>').find('.optDischarge').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent4.ClientID %>').find('.chkNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=' + $(this).attr('nursingProblemCode');
                else
                    controlValues += $(this).attr('controlID') + '=-';
            });

            $('#<%=hdnDischargePlanningValue.ClientID %>').val(controlValues);

            return controlValues;
        }

    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsNotAllowNurseFillChiefComplaint" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnTimeNow1" value="00" />
        <input type="hidden" runat="server" id="hdnTimeNow2" value="00" />
        <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="0" />
        <input type="hidden" id="hdnRegistrationID" runat="server" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnIsEmergencyUsingRoom" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
        <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
        <input type="hidden" runat="server" id="hdnER0010" value="" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;" id="tdChiefComplaint" runat="server">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Anamnesa Perawat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="5"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto Anamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo Anamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text=" Tidak ada Alergi"
                                                Checked="false" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:CheckBox ID="chkIsFastTrack" runat="server" Text=" Fast Track" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:CheckBox ID="chkIsGeriatricPatient" runat="server" Text="Pasien Geriatri" Checked="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Location")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocation" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Onset")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboOnset" ClientInstanceName="cboOnset" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboOnsetChanged(s); }" Init="function(s,e){ onCboOnsetChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOnset" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Provocation")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboProvocation" ClientInstanceName="cboProvocation"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboProvocationChanged(s); }"
                                        Init="function(s,e){ onCboProvocationChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProvocation" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Quality")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboQuality" ClientInstanceName="cboQuality"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboQualityChanged(s); }"
                                        Init="function(s,e){ onCboQualityChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuality" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Severity")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboSeverityChanged(s); }"
                                        Init="function(s,e){ onCboSeverityChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSeverity" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Time")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboTime" ClientInstanceName="cboTime" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTimeChanged(s); }" Init="function(s,e){ onCboTimeChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTime" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Relieved By")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboRelievedBy" ClientInstanceName="cboRelievedBy"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRelievedByChanged(s); }"
                                        Init="function(s,e){ onCboRelievedByChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRelievedBy" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Alasan Kunjungan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%"
                                    runat="server">
                                    <ClientSideEvents Init="function(s,e){ onCboVisitReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOtherVisitNotesLabel" runat="server">
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVisitNotes" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Perawat")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trBedQuickPicks" runat="server">
                            <td />
                            <td>
                                <input type="button" id="btnBedQuickPicks" value='<%:("Pilih Tempat Tidur") %>' class="w3-btn w3-hover-blue" />
                            </td>
                        </tr>
                        <tr id="trRoom" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblRoom">
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
                                            <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trBed" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblBed">
                                    <%:GetLabel("Tempat Tidur")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBedID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 0px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBedCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trClass" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory" runat="server" id="lblClass">
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
                                            <asp:TextBox ID="txtClassCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClassName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trChargeClass" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory" runat="server" id="lblChargeClass">
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
                                            <asp:TextBox ID="txtChargeClassCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChargeClassName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="5">
                                <h4 class="h4expanded">
                                    <%=GetLabel("SURVEY PRIMER")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col width="150px" />
                                            <col width="200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Airway")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboAirway" ClientInstanceName="cboAirway" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAirwayChanged(s); }"
                                                        Init="function(s,e){ onCboAirwayChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScoreAirway" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                    ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Breathing")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboBreathing" ClientInstanceName="cboBreathing"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboBreathingChanged(s); }"
                                                        Init="function(s,e){ onCboBreathingChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScoreBreathing" Width="40px" CssClass="number" runat="server"
                                                    Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Circulation")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboCirculation" ClientInstanceName="cboCirculation"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboCirculationChanged(s); }"
                                                        Init="function(s,e){ onCboCirculationChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScoreCirculation" Width="40px" CssClass="number" runat="server"
                                                    Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Disability")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboDisability" ClientInstanceName="cboDisability"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboDisabilityChanged(s); }"
                                                        Init="function(s,e){ onCboDisabilityChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScoreDisability" Width="40px" CssClass="number" runat="server"
                                                    Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Exposure")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboExposure" ClientInstanceName="cboExposure"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboExposureChanged(s); }"
                                                        Init="function(s,e){ onCboExposureChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtScoreExposure" Width="40px" CssClass="number" runat="server"
                                                    Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalScoreTriage" Width="40px" CssClass="number" runat="server"
                                                    Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="5">
                                <h4 class="h4expanded">
                                    <%=GetLabel("Riwayat Penyakit Dahulu dan Terapi")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                            <col width="150px" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top">
                                                <label id="lblMedicalHistory">
                                                    <%=GetLabel("Riwayat Penyakit Dahulu")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtMedicalHistory" runat="server" TextMode="MultiLine" Rows="3"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top">
                                                <label id="lblMedicationHistory">
                                                    <%=GetLabel("Riwayat Pengobatan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtMedicationHistory" runat="server" TextMode="MultiLine" Rows="3"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td valign="top">
                                <h4 class="h4expanded">
                                    <%=GetLabel("TRIASE")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col width="150px" />
                                            <col width="150px" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                                                    <%:GetLabel("Jenis Kunjungan")%></label>
                                            </td>
                                            <td colspan="3">
                                                <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
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
                                                            <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%=GetLabel("Waktu Kejadian") %></div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmergencyCaseDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 40px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 40px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtEmergencyCaseTime1" Width="40px" CssClass="number" runat="server"
                                                                Style="text-align: center" MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmergencyCaseTime2" Width="40px" CssClass="number" runat="server"
                                                                Style="text-align: center" MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%=GetLabel("Waktu Registrasi")%></div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationTime" Width="80px" runat="server" CssClass="time"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationDateDiff" ReadOnly="true" Style="text-align: center"
                                                    Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%=GetLabel("Waktu Pelayanan")%></div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 40px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 40px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceTime1" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtServiceTime2" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceDateDiff" ReadOnly="true" Style="text-align: center" Width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Cara Datang")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox runat="server" ID="cboAdmissionRoute" ClientInstanceName="cboAdmissionRoute"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Jenis Kasus")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox runat="server" ID="cboVisitCaseType" ClientInstanceName="cboVisitCaseType"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Triage")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%"
                                                    ClientEnabled="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%=GetLabel("Waktu Triage") %></div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTriageDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 40px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 40px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTriageTime1" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTriageTime2" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Triage Oleh")%></label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTriageByParamedicName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkIsTrueEmergency" runat="server" Text=" True Emergency" Checked="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Keadaan Datang")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                                    Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <%=GetLabel("Lokasi dan Mekanisme Trauma") %>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtEmergencyCase" runat="server" Width="100%" TextMode="Multiline"
                                                    Rows="10" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Asesment Fungsional")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                        <colgroup>
                                            <col width="175px" />
                                            <col width="200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Ketergantungan Fungsional")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboFunctionalType" ClientInstanceName="cboFunctionalType"
                                                    Width="100%">
                                                    <%--                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboFunctionalTypeChanged(s); }"
                                            Init="function(s,e){ onCboFunctionalTypeChanged(s); }" />--%>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFunctionalTypeRemarks" CssClass="txtFunctionalTypeRemarks" Width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Kebutuhan Edukasi")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                        <colgroup>
                                            <col width="175px" />
                                            <col width="200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkIsNeedPatientEducation" runat="server" Text=" Kebutuhan Edukasi"
                                                    Checked="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                        <tr>
                                            <td>
                                                <div style="position: relative;">
                                                    <dxcp:ASPxCallbackPanel ID="cbpEducationView" runat="server" Width="100%" ClientInstanceName="cbpEducationView"
                                                        ShowLoadingPanel="false" OnCallback="cbpEducationView_Callback">
                                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                            EndCallback="function(s,e){ onCbpEducationViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent4" runat="server">
                                                                <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                                    <asp:GridView ID="grdEducationView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                        OnRowDataBound="grdEducationView_RowDataBound">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                <ItemTemplate>
                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img class="imgEditEducation imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                    alt="" />
                                                                                            </td>
                                                                                            <td style="width: 1px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td>
                                                                                                <img class="imgDeleteEducation imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                    alt="" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                <HeaderTemplate>
                                                                                    <div style="text-align: right">
                                                                                        <span class="lblLink" id="lblAddEducation">
                                                                                            <%= GetLabel("+ Tambah Edukasi Pasien")%></span>
                                                                                    </div>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <div>
                                                                                        <b>
                                                                                            <%#: Eval("cfEducationDate")%>,
                                                                                            <%#: Eval("EducationTime") %>,
                                                                                            <%#: Eval("ParamedicName") %>
                                                                                        </b>
                                                                                    </div>
                                                                                    <div>
                                                                                        <asp:Repeater ID="rptPatientEducationDt" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                                    <span style="color: Black"><strong>
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "EducationType") %>
                                                                                                        : </strong></span>&nbsp; <span style="color: Blue">
                                                                                                            <%#: DataBinder.Eval(Container.DataItem, "Remarks")%></span>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <br style="clear: both" />
                                                                                            </FooterTemplate>
                                                                                        </asp:Repeater>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            <%=GetLabel("Tidak ada data edukasi pasien untuk pasien ini") %>
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
                                                            <div id="educationPaging">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Riwayat Psikososial - Spiritual")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                        <colgroup>
                                            <col width="175px" />
                                            <col width="200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="labelColumn" colspan="2">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Hubungan dengan Anggota Keluarga :")%></label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblFamilyRelationship" CssClass="rblFamilyRelationship"
                                                    runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                    <asp:ListItem Text=" Baik" Value="1" />
                                                    <asp:ListItem Text=" Kurang Baik" Value="0" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelColumn">
                                                <label class="lblNormal" style="padding-left: 10px">
                                                    <%=GetLabel("Jelaskan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtFamilyRelationshipRemarks" CssClass="txtFamilyRelationshipRemarks"
                                                    runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelColumn" colspan="2">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Memerlukan kebutuhan privasi tambahan :")%></label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblIsNeedAdditionalPrivacy" CssClass="rblIsNeedAdditionalPrivacy"
                                                    runat="server" RepeatDirection="Horizontal" CellPadding="14">
                                                    <asp:ListItem Text=" Ya" Value="1" />
                                                    <asp:ListItem Text=" Tidak" Value="0" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelColumn">
                                                <label class="lblNormal" style="padding-left: 10px">
                                                    <%=GetLabel("Jelaskan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtNeedAdditionalPrivacyRemarks" CssClass="txtNeedAdditionalPrivacyRemarks"
                                                    runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Status Psikologis")%></label>
                                            </td>
                                            <td colspan="2">
                                                <dxe:ASPxComboBox runat="server" ID="cboPsychologyStatus" ClientInstanceName="cboPsychologyStatus"
                                                    Width="100%">
                                                    <%--                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPsychologyStatusChanged(s); }"
                                            Init="function(s,e){ onCboPsychologyStatusChanged(s); }" />--%>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelColumn" colspan="2">
                                                <label class="lblNormal" style="padding-left: 10px">
                                                    <%=GetLabel("Kecenderungan bunuh diri dilaporkan ke")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCommitSuicideRemarks" CssClass="txtCommitSuicideRemarks" Width="100%"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelColumn" colspan="2">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Terdapat masalah ekonomi :")%></label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblHasFinancialProblem" CssClass="rblHasFinancialProblem"
                                                    runat="server" RepeatDirection="Horizontal" CellPadding="14">
                                                    <asp:ListItem Text=" Ya" Value="1" />
                                                    <asp:ListItem Text=" Tidak" Value="0" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelColumn">
                                                <label class="lblNormal" style="padding-left: 10px">
                                                    <%=GetLabel("Jelaskan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtFinancialProblemRemarks" CssClass="txtFinancialProblemRemarks"
                                                    runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Asesment RAPUH")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsHasRAPUHAssessment" runat="server" Text="  Asesment RAPUH"
                                                    Checked="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" title="Dengan diri sendiri atau tanpa bantuan alat mengalami kesulitan untuk naik 10 anak tangga dan tanpa istirahat diantaranya?">
                                                    <%=GetLabel("Resistensi")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboRAPUH_R" ClientInstanceName="cboRAPUH_R"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_R_Changed(s); }"
                                                        Init="function(s,e){ onCboRAPUH_R_Changed(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-left: 5px;">
                                                            <%=GetLabel("Skor") %>
                                                        </td>
                                                        <td style="padding-left: 5px; width: 60px">
                                                            <asp:TextBox ID="txtRAPUH_R" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" title="Seberapa sering dalam 4 minggu merasa kelelahan?">
                                                    <%=GetLabel("Aktifitas")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboRAPUH_A" ClientInstanceName="cboRAPUH_A"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_A_Changed(s); }"
                                                        Init="function(s,e){ onCboRAPUH_A_Changed(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-left: 5px;">
                                                            <%=GetLabel("Skor") %>
                                                        </td>
                                                        <td style="padding-left: 5px; width: 60px">
                                                            <asp:TextBox ID="txtRAPUH_A" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" title="Apakah Dokter pernah mengatakan bahwa pasien mempunyai hipertensi, diabetes, kanker, penyakit paru kronis, serangan jantung, gagal jantung, kongestif, nyeri dada, asma, nyeri sendi, stroke dan penyakit ginjal?">
                                                    <%=GetLabel("Penyakit lebih dari 5")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboRAPUH_P" ClientInstanceName="cboRAPUH_P"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_P_Changed(s); }"
                                                        Init="function(s,e){ onCboRAPUH_P_Changed(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-left: 5px;">
                                                            <%=GetLabel("Skor") %>
                                                        </td>
                                                        <td style="padding-left: 5px; width: 60px">
                                                            <asp:TextBox ID="txtRAPUH_P" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" title="Dengan diri sendiri dan tanpa bantuan apakah pasien mengalami kesulitan berjalan sejauh 100-200 meter?">
                                                    <%=GetLabel("Usaha Berjalan")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboRAPUH_U" ClientInstanceName="cboRAPUH_U"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_U_Changed(s); }"
                                                        Init="function(s,e){ onCboRAPUH_U_Changed(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-left: 5px;">
                                                            <%=GetLabel("Skor") %>
                                                        </td>
                                                        <td style="padding-left: 5px; width: 60px">
                                                            <asp:TextBox ID="txtRAPUH_U" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" title="Berapa berat badan pasien dengan mengenakan baju tanpa alas kaki saat ini dan 1 tahun yang lalu?">
                                                    <%=GetLabel("Hilangnya berat badan")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboRAPUH_H" ClientInstanceName="cboRAPUH_H"
                                                    Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_H_Changed(s); }"
                                                        Init="function(s,e){ onCboRAPUH_H_Changed(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-left: 5px;">
                                                            <%=GetLabel("Skor") %>
                                                        </td>
                                                        <td style="padding-left: 5px; width: 60px">
                                                            <asp:TextBox ID="txtRAPUH_H" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" title="Berapa berat badan pasien dengan mengenakan baju tanpa alas kaki saat ini dan 1 tahun yang lalu?">
                                                    <%=GetLabel("Total Nilai RAPUH")%></label>
                                            </td>
                                            <td colspan="2">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtRAPUHScore" runat="server" Width="100px" CssClass="number" />
                                                        </td>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Kesimpulan")%></label>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxComboBox runat="server" ID="cboRAPUHScore" ClientInstanceName="cboRAPUHScore"
                                                                Width="100%">
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Masalah Keperawatan")%></h4>
                                <div class="containerTblEntryContent containerEntryPanel1">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                        <tr>
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
                                                                <%=GetLabel("Masalah Keperawatan")%></label>
                                                        </td>
                                                        <td colspan="4">
                                                            <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                                            <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                                                <colgroup>
                                                                    <col style="width: 95%" />
                                                                    <col style="width: 5%" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td>
                                                                        <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                                                            Width="99%" ValueText="ProblemID" FilterExpression="IsDeleted = 0" DisplayText="ProblemName"
                                                                            MethodName="GetNursingProblemList">
                                                                            <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                                                            <Columns>
                                                                                <qis:QISSearchTextBoxColumn Caption="Masalah Keperawatan (Kode)" FieldName="ProblemName"
                                                                                    Description="i.e. Intoleransi Aktifitas" Width="500px" />
                                                                            </Columns>
                                                                        </qis:QISSearchTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="1" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <img class="btnApplyDiagnosis imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                                <td>
                                                                                    <img class="btnCancelDiagnosis imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                                                        ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                                    <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                <ItemTemplate>
                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                    <input type="hidden" value="<%#:Eval("ProblemCode") %>" bindingfield="ProblemCode" />
                                                                                    <input type="hidden" value="<%#:Eval("ProblemName") %>" bindingfield="ProblemName" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img class="imgEditDiagnosis imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                    alt="" />
                                                                                            </td>
                                                                                            <td style="width: 1px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td>
                                                                                                <img class="imgDeleteDiagnosis imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                    alt="" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                <HeaderTemplate>
                                                                                    <%=GetLabel("Masalah Keperawatan")%>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <div>
                                                                                        <%#: Eval("ProblemName")%></div>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                <ItemTemplate>
                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                    <input type="hidden" value="<%#:Eval("ProblemID") %>" bindingfield="ProblemID" />
                                                                                    <input type="hidden" value="<%#:Eval("ProblemCode") %>" bindingfield="ProblemCode" />
                                                                                    <input type="hidden" value="<%#:Eval("ProblemName") %>" bindingfield="ProblemName" />
                                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            <%=GetLabel("Belum ada informasi masalah keperawatan untuk pasien ini") %>
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dxcp:ASPxCallbackPanel>
                                                    <div class="containerPaging">
                                                        <div class="wrapperPaging">
                                                            <div id="diagnosisPaging">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed" id="h4TandaVital">
                                    <%=GetLabel("Pemeriksaan Tanda Vital")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2">
                                                <div>
                                                    <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                                                        ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                                    <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                        OnRowDataBound="grdVitalSignView_RowDataBound">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img class="imgEditVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                    alt="" />
                                                                                            </td>
                                                                                            <td style="width: 1px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td>
                                                                                                <img class="imgDeleteVitalSign imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                    alt="" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                <HeaderTemplate>
                                                                                    <div style="text-align: right">
                                                                                        <span class="lblLink" id="lblAddVitalSign">
                                                                                            <%= GetLabel("+ Tambah Tanda Vital")%></span>
                                                                                    </div>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <div>
                                                                                        <b>
                                                                                            <%#: Eval("ObservationDateInString")%>,
                                                                                            <%#: Eval("ObservationTime") %>,
                                                                                            <%#: Eval("ParamedicName") %>
                                                                                        </b>
                                                                                        <br />
                                                                                        <span style="font-style: italic">
                                                                                            <%#: Eval("Remarks") %>
                                                                                        </span>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div>
                                                                                        <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <div style="padding-left: 20px; float: left; width: 350px;">
                                                                                                    <strong>
                                                                                                        <div style="width: 110px; float: left;" class="labelColumn">
                                                                                                            <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                                                        <div style="width: 20px; float: left;">
                                                                                                            :</div>
                                                                                                    </strong>
                                                                                                    <div style="float: left;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <br style="clear: both" />
                                                                                            </FooterTemplate>
                                                                                        </asp:Repeater>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            <%=GetLabel("No Data To Display") %>
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dxcp:ASPxCallbackPanel>
                                                    <div class="containerPaging">
                                                        <div class="wrapperPaging">
                                                            <div id="vitalSignPaging">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Perencanaan Pemulangan Pasien")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblContentArea">
                                        <tr>
                                            <td>
                                                <div id="divFormContent4" runat="server" style="height: 200px; overflow-y: scroll;">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td>
                                                <input type="button" class="btnTest w3-btn w3-hover-blue" value="Display HTML" style="background-color: Red;
                                                    color: White; width: 120px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Catatan Planning")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label class="lblNormal" id="Label2">
                                                    <%=GetLabel("Catatan Planning") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPlanningNotes" runat="server" Width="100%" TextMode="Multiline"
                                                    Rows="10" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <h4 class="h4collapsed">
                                    <%=GetLabel("Catatan Instruksi")%></h4>
                                <div class="containerTblEntryContent">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label class="lblNormal" id="Label1">
                                                    <%=GetLabel("Catatan Instruksi") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInstructionText" runat="server" Width="100%" TextMode="Multiline"
                                                    Rows="10" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
            ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteEducation" runat="server" Width="100%" ClientInstanceName="cbpDeleteEducation"
            ShowLoadingPanel="false" OnCallback="cbpDeleteEducation_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteEducationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
