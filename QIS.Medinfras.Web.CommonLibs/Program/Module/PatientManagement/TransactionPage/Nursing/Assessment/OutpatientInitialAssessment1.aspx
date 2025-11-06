<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="OutpatientInitialAssessment1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OutpatientInitialAssessment1" %>

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
            if ($('#<%=hdnDisableServiceDateTime.ClientID %>').val() == "1") {
                setDatePicker('<%=txtServiceDate.ClientID %>');
            }
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));


            $('#<%=txtServiceDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtServiceTime1.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime1.ClientID %>').val() >= 0 && $('#<%=txtServiceTime1.ClientID %>').val() < 24 && $('#<%=txtServiceTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceTime2.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime2.ClientID %>').val() >= 0 && $('#<%=txtServiceTime2.ClientID %>').val() < 60 && $('#<%=txtServiceTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

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
                    showToast("ERROR", "You should fill Diagnosis Type and Name field !");
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

        //#region Vital Sign
        var pageCountVitalSign = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCountVitalSign, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            }, 8);
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
                onBeforeOpenTrxPopup();
                var visitNoteID = <%=HttpUtility.HtmlEncode(GetVisitNoteID())%>;
                var param = "0||" + visitNoteID + "|1|1";
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
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
            var visitNoteID = '<%=HttpUtility.HtmlEncode(GetVisitNoteID()) %>';
            var param = "|" + visitNoteID + "|1|1";            
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/PatientEducation/PatientEducationCtl.ascx", param, "Edukasi Pasien", 700, 500);
        });

        $('.imgEditEducation.imgLink').die('click');
        $('.imgEditEducation.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var visitNoteID = '<%=HttpUtility.HtmlEncode(GetVisitNoteID())%>';
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


        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                $('#<%=hdnID.ClientID %>').val(retval);
            }
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry'))
                onCustomButtonClick('save');
        });

        function HourDifference() {
            var serviceDateInString = $('#<%=txtServiceDate.ClientID %>').val();
            var serviceTime = $('#<%=txtServiceTime1.ClientID %>').val() + ":" + $('#<%=txtServiceTime2.ClientID %>').val();
            var serviceDate = Methods.getDatePickerDate(serviceDateInString);

            //registration difference
            var registrationDateInString = $('#<%=hdnRegistrationDate.ClientID %>').val();
            var registrationTime = $('#<%=hdnRegistrationTime.ClientID %>').val();
            var registrationDate = Methods.getDatePickerDate(registrationDateInString);
            dateDiff = Methods.calculateDateDifference(registrationDate, serviceDate);

            $h1 = parseInt(serviceTime.substring(0, 2), 10);
            $m1 = parseInt(serviceTime.substring(3, 5), 10);

            $h2 = parseInt(registrationTime.substring(0, 2), 10);
            $m2 = parseInt(registrationTime.substring(3, 5), 10);


            var registrationDateDiff = countHour(dateDiff.days, dateDiff.months, $h1, $m1, $h2, $m2);

            $('#<%=txtServiceDateDiff.ClientID %>').val(registrationDateDiff);
        }

        function countHour(days, months, h1, m1, h2, m2) {
            var totalDaysInMonth = (months * 30) + days;
            var totalMinutes1 = (days * 24 * 60) + (h1 * 60) + m1;
            var totalMinutes2 = (h2 * 60) + m2;
            var diffMinutes = totalMinutes1 - totalMinutes2;

            var hours = Math.floor(diffMinutes / 60);
            var minutes = diffMinutes % 60;

            if (minutes < 0) {
                minutes += 60;
                hours -= 1;
            }

            var totalDays = Math.floor(hours / 24) + totalDaysInMonth;
            var totalHours = hours % 24;

            $('#<%=hdnTimeElapsed1day.ClientID %>').val(totalDays);
            $('#<%=hdnTimeElapsed1hour.ClientID %>').val(totalHours);
            $('#<%=hdnTimeElapsed1minute.ClientID %>').val(minutes);

            return totalDays + " Hari " + totalHours + " Jam " + minutes + " Menit";
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

        //#region Body Diagram
        $('#lblAddBodyDiagram').die('click');
        $('#lblAddBodyDiagram').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/BodyDiagram/BodyDiagramSOAPAdd1Ctl.ascx", "", "Body Diagram", 1200, 600);
        });

        function onCbpBodyDiagramViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'count') {
                if (param[1] != '0') {
                    $('#<%=divBodyDiagram.ClientID %>').show();
                    $('#<%=tblEmpty.ClientID %>').hide();
                }
                else {
                    $('#<%=divBodyDiagram.ClientID %>').hide();
                    $('#<%=tblEmpty.ClientID %>').show();
                }

                $('#<%=hdnPageCount.ClientID %>').val(param[1]);
                $('#<%=hdnPageIndex.ClientID %>').val('0');
            }
            else if (param[0] == 'index')
                $('#<%=hdnPageIndex.ClientID %>').val(param[1]);
            hideLoadingPanel();
        }

        function onRefreshBodyDiagram(filterExpression) {
            if (filterExpression == 'edit')
                cbpBodyDiagramView.PerformCallback('edit');
            else
                cbpBodyDiagramView.PerformCallback('refresh');
        }

        function onCbpBodyDiagramDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpBodyDiagramView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }
        //endregion

        $('#btnBodyDiagramContainerPrev').live('click', function () {
            if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                cbpBodyDiagramView.PerformCallback('prev');
        });
        $('#btnBodyDiagramContainerNext').live('click', function () {
            if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                cbpBodyDiagramView.PerformCallback('next');
        });

        $('#imgEditBodyDiagram').live('click', function () {
            onBeforeOpenTrxPopup();
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/BodyDiagram/BodyDiagramSOAPEdit1Ctl.ascx", $('#<%=hdnBodyDiagramID.ClientID %>').val(), "Body Diagram", 1200, 600);
            cbpBodyDiagramView.PerformCallback('refresh');
        });

        $('#imgDeleteBodyDiagram').live('click', function () {
            var message = "Are you sure to delete this body diagram ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteBodyDiagram.PerformCallback();
                }
            });
            cbpBodyDiagramView.PerformCallback('refresh');
        });

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

        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnRegistrationDate" value="00" />
        <input type="hidden" runat="server" id="hdnRegistrationTime" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1day" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="0" />
        <input type="hidden" runat="server" id="hdnIsNotAllowNurseFillChiefComplaint" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnTimeNow1" value="00" />
        <input type="hidden" runat="server" id="hdnTimeNow2" value="00" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" runat="server" id="hdnDisableServiceDateTime" value="" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 55%" />
                <col style="width: 45%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;" id="tdChiefComplaint" runat="server">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Waktu")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
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
                                </table>
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
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Keluhan Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="3"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                <asp:TextBox ID="txtHPISummary" runat="server" Width="99%" TextMode="Multiline" Rows="10" />
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
                    </table>
                </td>
                <td valign="top">
                    <h4 class="h4collapsed">
                        <%=GetLabel("Pembaharuan Assessment Awal")%></h4>
                    <div class="containerTblEntryContent">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="50%" />
                                <col width="50%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kebutuhan Pembaharuan Assessment Awal")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkIsNeedAcuteInitialAssessment" runat="server" Text=" Penyakit Akut"
                                                Checked="false" />
                                        </td>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkIsNeedChronicInitialAssessment" runat="server" Text=" Penyakit Kronis"
                                                Checked="false" />
                                        </td>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h4 class="h4collapsed">
                        <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                    <div class="containerTblEntryContent" style="display: none">
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
                    </div>
                    <h4 class="h4collapsed">
                        <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                    <div class="containerTblEntryContent" style="display: none">
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
                        <%=GetLabel("Body Diagram")%></h4>
                    <div class="containerTblEntryContent">
                        <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 1px">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" style="float: left; margin-top: 0px;">
                                        <tr>
                                            <td>
                                                <span class="lblLink" id="lblAddBodyDiagram">
                                                    <%= GetLabel("+ Tambah Body Diagram")%></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table id="tblBodyDiagramNavigation" runat="server" border="0" cellpadding="0" cellspacing="0"
                                        style="float: right; margin-top: 0px;">
                                        <tr>
                                            <td>
                                                <img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px"
                                                    alt="" class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" />
                                            </td>
                                            <td>
                                                <img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px"
                                                    alt="" class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="position: relative;" id="divBodyDiagram" runat="server">
                                        <dxcp:ASPxCallbackPanel ID="cbpBodyDiagramView" runat="server" Width="100%" ClientInstanceName="cbpBodyDiagramView"
                                            ShowLoadingPanel="false" OnCallback="cbpBodyDiagramView_Callback">
                                            <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server">
                                                    <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGrid">
                                                        <div class="templatePatientBodyDiagram">
                                                            <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />
                                                            <div class="containerImage boxShadow">
                                                                <img src='' alt="" id="imgBodyDiagram" runat="server" />
                                                            </div>
                                                            <div>
                                                                <table border="0" cellpadding="0" cellspacing="0" style="float: right; margin-top: 0px;">
                                                                    <tr>
                                                                        <td>
                                                                            <img src='<%=ResolveUrl("~/Libs/Images/Button/edit.png") %>' title="Edit" width="25px"
                                                                                alt="" class="imgLink" id="imgEditBodyDiagram" style="margin-left: 5px;" />
                                                                        </td>
                                                                        <td>
                                                                            <img src='<%=ResolveUrl("~/Libs/Images/Button/delete.png") %>' title="Delete" width="25px"
                                                                                alt="" class="imgLink" id="imgDeleteBodyDiagram" style="margin-left: 5px;" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <span class="spLabel">
                                                                <%=GetLabel("Diagram Name") %></span>:<span class="spValue" id="spnDiagramName" runat="server"></span><br />
                                                            <span class="spLabel">
                                                                <%=GetLabel("Remarks") %></span>:
                                                            <br />
                                                            <asp:Repeater ID="rptRemarks" runat="server">
                                                                <HeaderTemplate>
                                                                    <table>
                                                                        <colgroup width="20px" />
                                                                        <colgroup width="2px" />
                                                                        <colgroup width="15px" />
                                                                        <colgroup width="2px" />
                                                                        <colgroup width="60px" />
                                                                        <colgroup width="2px" />
                                                                        <colgroup width="*" />
                                                                        <colgroup width="16px" />
                                                                        <colgroup width="16px" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <img alt="" style="width: 16px; height: 16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>" />
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td>
                                                                            <%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%>
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td>
                                                                            <%#: DataBinder.Eval(Container.DataItem, "SymbolName")%>
                                                                        </td>
                                                                        <td>
                                                                            :
                                                                        </td>
                                                                        <td>
                                                                            <%#: DataBinder.Eval(Container.DataItem, "Remarks")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                            <br />
                                                            <span class="spLabel">
                                                                <%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
                                                            <span class="spLabel">
                                                                <%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime"
                                                                    runat="server"></span><br />
                                                        </div>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                    </div>
                                    <table id="tblEmpty" style="display: none; width: 100%" runat="server">
                                        <tr class="trEmpty">
                                            <td align="center" valign="middle">
                                                <%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %>
                                            </td>
                                        </tr>
                                    </table>
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
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
            ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteEducation" runat="server" Width="100%" ClientInstanceName="cbpDeleteEducation"
            ShowLoadingPanel="false" OnCallback="cbpDeleteEducation_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteEducationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
            ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
