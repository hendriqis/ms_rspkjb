<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    ValidateRequest="false" AutoEventWireup="true" CodeBehind="ERInitialAssessmentEntry1.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ERInitialAssessmentEntry1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back to List")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Discard Changes")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
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
            });

            $('#<%=txtServiceTime1.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime1.ClientID %>').val() >= 0 && $('#<%=txtServiceTime1.ClientID %>').val() < 24) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceTime2.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime2.ClientID %>').val() >= 0 && $('#<%=txtServiceTime2.ClientID %>').val() < 60) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
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

            $('.btnApplyDiagnosis').click(function () {
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
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

            HourDifference();

            registerCollapseExpandHandler();

            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Lanjutkan proses pembatalan perubahan yang sudah dilakukan ?";
                    displayConfirmationMessageBox("BATAL PERUBAHAN", message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload();
                        }
                    });
                }
            });

            //#region Keluhan Utama
            $('#<%=txtChiefComplaint.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Riwayat Penyakit Sekarang
            $('#<%=txtHPISummary.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Riwayat Penyakit Dahulu
            $('#<%=txtMedicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Riwayat Penggunaan Obat
            $('#<%=txtMedicationHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Catatan Planning
            $('#<%=txtPlanningNotes.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Catatan Instruksi
            $('#<%=txtInstructionText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                if (contentID == 'divPage3') {
                    cbpVitalSignView.PerformCallback('refresh');
                }

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

            $('#leftPageNavPanel ul li').first().click();

            //#region Vital Sign View
            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
            //#endregion

            //#region ROS View
            $('#<%=grdROSView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdROSView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnReviewOfSystemID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdROSView.ClientID %> tr:eq(1)').click();
            //#endregion
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
            });
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            if (onCheckPatientVisitNote() == "1") {
                var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
                var param = "0||" + visitNoteID + "|1|1";
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("WARNING", "Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
            var param = "0|" + $('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Vital Sign & Indicator", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Are you sure to delete this vital sign record ?";
            displayConfirmationMessageBox("DELETE : Vital Sign", message, function (result) {
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
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                displayErrorMessageBox("DELETE : Vital Sign", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Review of System
        function GetCurrentSelectedROS(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdROSView.ClientID %> tr').index($tr);
            $('#<%=grdROSView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdROSView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddROS').die('click');
        $('#lblAddROS').live('click', function (evt) {
            if (onCheckPatientVisitNote() == "1") {
                var param = "0|0|1|1";
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/ReviewOfSystem/ROSEntry1Ctl.ascx", param, "Review of System", 700, 500);
            }
            else {
                displayMessageBox("WARNING", "Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedROS(this);
            $('#<%=hdnReviewOfSystemID.ClientID %>').val(selectedObj.ID);
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/ReviewOfSystem/ROSEntry1Ctl.ascx", $('#<%=hdnReviewOfSystemID.ClientID %>').val(), "Review of System", 700, 500);
        });

        $('.imgDeleteROS.imgLink').die('click');
        $('.imgDeleteROS.imgLink').live('click', function () {
            var message = "Are you sure to delete this physical examination record ?";
            displayConfirmationMessageBox("DELETE - ROS", message, function (result) {
                if (result) {
                    cbpDeleteROS.PerformCallback();
                }
            });
        });

        var pageCount = parseInt('<%=gridROSPageCount %>');
        $(function () {
            setPaging($("#rosPaging"), pageCount, function (page) {
                cbpROSView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpROSViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdROSView.ClientID %> tr:eq(1)').click();

                setPaging($("#rosPaging"), pageCount, function (page) {
                    cbpROSView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdROSView.ClientID %> tr:eq(1)').click();
        }

        function onCbpDeleteROSEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpROSView.PerformCallback('refresh');
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                displayErrorMessageBox("DELETE - ROS", param[1]);
            }
        }

        function onRefreshROSGrid() {
            cbpROSView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Diagnose
        $('#lblDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                onTxtDiagnoseCodeChanged(value);
            });
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnDiagnoseID.ClientID %>').val(result.DiagnoseID);
                    $('#<%=txtDiagnose.ClientID %>').val(result.DiagnoseName + ' (' + result.DiagnoseID + ')');
                }
                else {
                    $('#<%=hdnDiagnoseID.ClientID %>').val('');
                    $('#<%=txtDiagnose.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry'))
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
        });


        function HourDifference() {
            var EmergencyCaseDateInString = $('#<%=txtEmergencyCaseDate.ClientID %>').val();
            var EmergencyCaseTime = $('#<%=txtEmergencyCaseTime1.ClientID %>').val() + ":" + $('#<%=txtEmergencyCaseTime2.ClientID %>').val();

            //service difference
            var serviceDateInString = $('#<%=txtServiceDate.ClientID  %>').val();
            var serviceTime = $('#<%=txtServiceTime1.ClientID %>').val() + ":" + $('#<%=txtServiceTime2.ClientID %>').val();

            var EmergencyCaseDate = Methods.getDatePickerDate(EmergencyCaseDateInString);
            var serviceDate = Methods.getDatePickerDate(serviceDateInString);

            var dateDiff = Methods.calculateDateDifference(EmergencyCaseDate, serviceDate);
            $h1 = parseInt(serviceTime.substring(0, 2), 10);
            $m1 = parseInt(serviceTime.substring(3, 5), 10);

            $h2 = parseInt(EmergencyCaseTime.substring(0, 2), 10);
            $m2 = parseInt(EmergencyCaseTime.substring(3, 5), 10);

            var serviceDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2, "1");

            $('#<%=txtServiceDateDiff.ClientID %>').val(serviceDateDiff);


            //registration difference
            var registrationDateInString = $('#<%=txtRegistrationDate.ClientID %>').val();
            var registrationTime = $('#<%=txtRegistrationTime.ClientID %>').val();
            var registrationDate = Methods.getDatePickerDate(registrationDateInString);
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
        
        function onRefreshDiagnosisGrid() {
            cbpDiagnosisView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
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
        //#endregion 

        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Perubahan yang dilakukan belum disimpan, Apakah perubahan tersebut disimpan ?";
                displayConfirmationMessageBox("SAVE",message, function (result) {
                    if (result) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
            else {
                gotoNextPage();
            }
        }

        function onBeforeChangeParentMenu() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Perubahan yang dilakukan belum disimpan, Apakah perubahan tersebut disimpan ?";
                displayConfirmationMessageBox("SAVE", message, function (result) {
                    if (result) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
            else {
                gotoNextParentMenu();
            }
        }

        function exitPatientPage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Perubahan yang dilakukan belum disimpan, Apakah perubahan tersebut disimpan ?";
                displayConfirmationMessageBox("SAVE", message, function (result) {
                    if (result) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
            else {
                backToPatientList();
            }
        }

        function onBeforeBackToListPage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                PromptUserToSave();
            }
            else {
                backToPatientList();
            }
        }

        function PromptUserToSave() {
            var message = "Perubahan yang dilakukan terhadap kajian awal belum disimpan, disimpan ?";
            displayConfirmationMessageBox("SAVE",message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }

         function onAfterCustomClickSuccessSetRecordID(retval) {
            if (retval.includes(';')) {
                var patientVisitNoteID = retval.split(';')[0];
                var nurseChiefComplaintID = retval.split(';')[1];
                if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '' || $('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '0') {
                    $('#<%=hdnPatientVisitNoteID.ClientID %>').val(patientVisitNoteID);
                }
                if ($('#<%=hdnChiefComplaintID.ClientID %>').val() == '' || $('#<%=hdnChiefComplaintID.ClientID %>').val() == '0') {
                    $('#<%=hdnChiefComplaintID.ClientID %>').val(nurseChiefComplaintID);
                }
            }
            else {
                if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '' || $('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '0') {
                    $('#<%=hdnPatientVisitNoteID.ClientID %>').val(retval);
                }
            }
        }

        function onCheckPatientVisitNote() {
            if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '' || $('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '0') {
                return "0";
            }
            else {
                return "1";
            }
        }
        //#endregion 
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnRegistrationDate" value="00" />
        <input type="hidden" runat="server" id="hdnRegistrationTime" value="00" />
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
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnRevisedByParamedicID" value="" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnIsEmergencyUsingRoom" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 20%" />
                <col style="width: 80%" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentid="divPage1" title="Keluhan Utama Pasien" class="w3-hover-red">Keluhan Utama Pasien</li>
                            <li contentid="divPage2" title="Triase & Survey Primer" class="w3-hover-red">Triase & Survey Primer</li>
                            <li contentid="divPage3" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>
                            <li contentid="divPage4" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>
                            <li contentid="divPage5" title="Riwayat Penyakit Dahulu" class="w3-hover-red">Riwayat Penyakit Dahulu</li>
                            <li contentid="divPage6" title="Riwayat Penggunaan Obat" class="w3-hover-red">Riwayat Penggunaan Obat</li>
                            <li contentid="divPage7" title="Ketergantungan Fungsional" class="w3-hover-red">Ketergantungan Fungsional</li>
                            <li contentid="divPage8" title="Psikososial Spiritual dan Kultural" class="w3-hover-red">Psikososial Spiritual dan Kultural</li>
                            <li contentid="divPage9" title="Kebutuhan Informasi dan Edukasi" class="w3-hover-red">Kebutuhan Informasi dan Edukasi</li>
                            <li contentid="divPage10" title="Asesmen Tambahan (Populasi Khusus)" class="w3-hover-red">Asesmen Tambahan (Populasi Khusus)</li>
                            <li contentid="divPage11" title="Masalah Keperawatan" class="w3-hover-red">Masalah Keperawatan</li>
                            <li contentid="divPage12" title="Catatan Planning" class="w3-hover-red">Catatan Planning</li>
                            <li contentid="divPage13" title="Catatan Instruksi" class="w3-hover-red">Catatan Instruksi</li>
                        </ul>
                    </div>
                </td>
                <td style="vertical-align: top">
                    <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
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
                                            <td style="width: 130px">
                                                <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto Anamnesis" Checked="false" />
                                            </td>
                                            <td style="width: 130px">
                                                <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo Anamnesis" Checked="false" />
                                            </td>
                                            <td class="tdLabel" style="width: 180px">
                                                <label class="lblNormal" id="lblFamilyRelation">
                                                    <%=GetLabel("Hubungan dengan Pasien")%></label>
                                            </td>
                                            <td style="width: 130px">
                                                <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                    Width="100%" />
                                            </td>
                                            <td style="padding-left:10px">
                                                <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text=" Tidak ada Alergi"
                                                    Checked="false" />
                                            </td>
                                            <td style="padding-left:10px">
                                                <asp:CheckBox ID="chkIsFastTrack" runat="server" Text=" Fast Track" />
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
                                <td class="tdLabel" style="width: 150px; vertical-align: top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Riwayat Penyakit Sekarang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHPISummary" runat="server" Width="100%" TextMode="Multiline" Rows="6" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblDiagnose">
                                        <%=GetLabel("Diagnosa Masuk")%></label>
                                </td>
                                <td>
                                    <input type="hidden" runat="server" value="" id="hdnDiagnoseID" />
                                    <asp:TextBox ID="txtDiagnose" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pembaharuan Assessment Awal")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <colgroup>
                                            <col width="25%" />
                                            <col width="25%" />
                                            <col width="25%" />
                                            <col width="25%" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsNeedAcuteInitialAssessment" runat="server" Text=" Penyakit Akut"
                                                    Checked="false" />
                                            </td>
                                            <td>
                                                <label id="lblLastAcuteInitialAssessmentDate" runat="server" class="blink-alert">(Terakhir tanggal : dd-MMM-yyyy)</label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsNeedChronicInitialAssessment" runat="server" Text=" Penyakit Kronis"
                                                    Checked="false" />
                                            </td>
                                            <td>
                                                <label id="lblLastChronicInitialAssessmentDate" runat="server" class="blink-alert">(Terakhir tanggal : dd-MMM-yyyy)</label>
                                            </td>
                                        </tr>
                                    </table>
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
                                    <asp:TextBox ID="txtVisitNotes" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Perawat")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                                </td>
                            </tr>
                            
                        </table>
                    </div>
                    <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col />
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <h4 class="h4expanded">
                                        <%=GetLabel("TRIASE")%></h4>
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
                                                <td>
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Triage")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
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
                                </td>
                                <td valign="top">
                                    <h4 class="h4expanded">
                                        <%=GetLabel("SURVEY PRIMER")%></h4>
                                        <table class="tblEntryContent" style="width: 100%">
                                            <colgroup>
                                                <col style="width: 40%" />
                                                <col style="width: 60%" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Airway")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboAirway" ClientInstanceName="cboAirway" Width="100%" />
                                                </td>
                                                <td />
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Breathing")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboBreathing" ClientInstanceName="cboBreathing"
                                                        Width="100%" />
                                                </td>
                                                <td />
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Circulation")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboCirculation" ClientInstanceName="cboCirculation"
                                                        Width="100%" />
                                                </td>
                                                <td />
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Disability")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDisability" ClientInstanceName="cboDisability"
                                                        Width="100%" />
                                                </td>
                                                <td />
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Exposure")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboExposure" ClientInstanceName="cboExposure"
                                                        Width="100%" />
                                                </td>
                                                <td />
                                            </tr>
                                            <tr id="trBedQuickPicks" runat="server">
                                                <td />
                                                <td>
                                                    <input type="button" id="btnBedQuickPicks" value='<%:("Pilih Tempat Tidur") %>' class="w3-btn w3-hover-blue" />
                                                </td>
                                            </tr>
                                            <tr id="trRoom" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblNormal lblMandatory" runat="server" id="lblRoom">
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
                                                    <label class="lblNormal lblMandatory" runat="server" id="lblBed">
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
                                                    <label class="lblNormal lblMandatory" runat="server" id="lblClass">
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
                                                    <label class="lblNormal lblMandatory" runat="server" id="lblChargeClass">
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
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <div style="position: relative;">
                                        <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                                            ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server">
                                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                        <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage"
                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                            OnRowDataBound="grdROSView_RowDataBound">
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
                                                                                    <img class="imgEditROS imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                                <td style="width: 1px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <img class="imgDeleteROS imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <div style="text-align: right">
                                                                            <span class="lblLink" id="lblAddROS">
                                                                                <%= GetLabel("+ Tambah Pemeriksaan Fisik")%></span>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div>
                                                                            <b>
                                                                                <%#: Eval("ObservationDateInString")%>,
                                                                                <%#: Eval("ObservationTime") %>,
                                                                                <%#: Eval("ParamedicName") %>
                                                                            </b>
                                                                        </div>
                                                                        <div>
                                                                            <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                                                <ItemTemplate>
                                                                                    <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                        <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                            <strong>
                                                                                                <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                                                : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
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
                                                                <%=GetLabel("Tidak ada data pemeriksaan fisik untuk pasien ini") %>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="Div1">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                        <div class="containerPaging">
                                            <div class="wrapperPaging">
                                                <div id="rosPaging">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col />
                            </colgroup>
                            <tr>
                                <td style="vertical-align: top">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                    <%=GetLabel("Riwayat Penyakit Dahulu") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMedicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                                    Rows="5" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Penggunaan Obat") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicationHistory" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="5" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage10" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_A_Changed(s); }"
                                                        Init="function(s,e){ onCboRAPUH_A_Changed(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage11" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage12" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage13" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
            ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteEducation" runat="server" Width="100%" ClientInstanceName="cbpDeleteEducation"
            ShowLoadingPanel="false" OnCallback="cbpDeleteEducation_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteEducationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
        <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteROS" runat="server" Width="100%" ClientInstanceName="cbpDeleteROS"
            ShowLoadingPanel="false" OnCallback="cbpDeleteROS_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteROSEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
