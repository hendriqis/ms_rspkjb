<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="OPInitialAssessmentEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OPInitialAssessmentEntry1" %>

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

            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                        PromptUserToSave();
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                    else {
                        showToast('Warning', 'Keluhan Pasien Harus diisi.');
                    }
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

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDiagnosisID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();


            $('.btnApplyDiagnosis').click(function () {
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
                $('#<%=hdnEntryDiagnoseID.ClientID %>').val('');
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
                $('#<%=hdnEntryDiagnoseID.ClientID %>').val('');
            });

            function submitDiagnosis() {
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

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                if (contentID == 'divPage2') {
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

            //#region Psikososial Spiritual dan Kultural
            $('#<%=divFormContent2.ClientID %>').html($('#<%=hdnPsychosocialLayout.ClientID %>').val());
            if ($('#<%=hdnPsychosocialValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnPsychosocialValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.optPsychosocial').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            //#endregion

            //#region Kebutuhan Edukasi
            $('#<%=divFormContent3.ClientID %>').html($('#<%=hdnEducationLayout.ClientID %>').val());
            if ($('#<%=hdnEducationValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnEducationValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent3.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.optEducation').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            //#endregion

            //#region Asessment Tambahan
            $('#<%=divFormContent5.ClientID %>').html($('#<%=hdnAdditionalLayout.ClientID %>').val());
            if ($('#<%=hdnAdditionalLayout.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnAdditionalValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent5.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.optPopulation').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            //#endregion
            $('#leftPageNavPanel ul li').first().click();
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
                ledDiagnose.SetValue('');
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshDiagnosisGrid() {
            cbpDiagnosisView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {

            if ($('#<%=hdnID.ClientID %>').val() == '') {
                $('#<%=hdnID.ClientID %>').val(retval);
            }
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                getPsychosocialFormValues();
                getEducationFormValues();
                getAdditionalFormValues();
                if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
                else {
                    showToast('Warning', 'Keluhan Pasien Harus diisi.');
                }
            }
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
        });

        $('#imgDeleteBodyDiagram').live('click', function () {
            var message = "Are you sure to delete this body diagram ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteBodyDiagram.PerformCallback();
                }
            });
        });

        //#region Get Form Values
        function getPsychosocialFormValues() {
            var controlValues = '';
            $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent2.ClientID %>').find('.optPsychosocial').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent2.ClientID %>').find('.chkNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=' + $(this).attr('nursingProblemCode');
                else
                    controlValues += $(this).attr('controlID') + '=-';
            });
            $('#<%=divFormContent2.ClientID %>').find('.txtNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });

            $('#<%=hdnPsychosocialValue.ClientID %>').val(controlValues);

            return controlValues;
        }

        function getEducationFormValues() {
            var controlValues = '';
            $('#<%=divFormContent3.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent3.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent3.ClientID %>').find('.optEducation').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent3.ClientID %>').find('.chkNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=' + $(this).attr('nursingProblemCode');
                else
                    controlValues += $(this).attr('controlID') + '=-';
            });

            $('#<%=divFormContent3.ClientID %>').find('.txtNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });

            $('#<%=hdnEducationValue.ClientID %>').val(controlValues);

            return controlValues;
        }

        function getAdditionalFormValues() {
            var controlValues = '';
            $('#<%=divFormContent5.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent5.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent5.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent5.ClientID %>').find('.optPopulation').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent5.ClientID %>').find('.chkNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=' + $(this).attr('nursingProblemCode');
                else
                    controlValues += $(this).attr('controlID') + '=-';
            });

            $('#<%=hdnAdditionalValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        //#endregion  

        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                getPsychosocialFormValues();
                getEducationFormValues();
                //     getDischargePlanningFormValues();
                getAdditionalFormValues();
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
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
            displayConfirmationMessageBox("SAVE", message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }

        $('#lblMedicalHistory').die('click');
        $('#lblMedicalHistory').live('click', function (evt) {
            var param = "";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/PastMedicalLookupCtl1.ascx", param, "Riwayat Kunjungan", 700, 500);
        });

        function onAfterVisitHistoryLookUp(param) {
            var text = param.replace("&nbsp", "");
            $('#<%=txtMedicalHistory.ClientID %>').val(text);
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
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="0" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" runat="server" id="hdnMenuType" value="" />
        <input type="hidden" runat="server" id="hdnDeptType" value="" />
        <input type="hidden" runat="server" id="hdnPsychosocialLayout" value="" />
        <input type="hidden" runat="server" id="hdnPsychosocialValue" value="" />
        <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
        <input type="hidden" runat="server" id="hdnEducationValue" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalLayout" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalValue" value="" />
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
        <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
        <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
        <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
        <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
        <input type="hidden" runat="server" id="hdnDisableServiceDateTime" value="" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 20%" />
                <col style="width: 80%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentid="divPage1" title="Keluhan Utama Pasien" class="w3-hover-red">Keluhan Utama
                                Pasien</li>
                            <li contentid="divPage2" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">
                                Tanda Vital dan Indikator Lainnya</li>
                            <li contentid="divPage3" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan
                                Fisik</li>
                            <li contentid="divPage4" title="Riwayat Penyakit Dahulu" class="w3-hover-red">Riwayat
                                Penyakit Dahulu</li>
                            <li contentid="divPage5" title="Riwayat Penggunaan Obat" class="w3-hover-red">Riwayat
                                Penggunaan Obat</li>
                            <li contentid="divPage6" title="Body Diagram" class="w3-hover-red">Body Diagram</li>
                            <li contentid="divPage7" title="Ketergantungan Fungsional" class="w3-hover-red">Ketergantungan
                                Fungsional</li>
                            <li contentid="divPage8" title="Psikososial Spiritual dan Kultural" class="w3-hover-red">
                                Psikososial Spiritual dan Kultural</li>
                            <li contentid="divPage9" title="Kebutuhan Informasi dan Edukasi" class="w3-hover-red">
                                Kebutuhan Informasi dan Edukasi</li>
                            <li contentid="divPage10" title="Asesmen Tambahan (Populasi Khusus)" class="w3-hover-red">
                                Asesmen Tambahan (Populasi Khusus)</li>
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
                            <tr id="trServiceUnit" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Penunjang Medis")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicalDiagnostic" ClientInstanceName="cboMedicalDiagnostic"
                                        runat="server" Width="350px">
                                    </dxe:ASPxComboBox>
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
                                        <td style="padding-left: 10px">
                                            <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text=" Tidak ada Alergi"
                                                Checked="false" />
                                        </td>
                                        <td style="padding-left: 10px">
                                            <asp:CheckBox ID="chkIsFastTrack" runat="server" Text=" Fast Track" />
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
                                    <asp:TextBox ID="txtHPISummary" runat="server" Width="99%" TextMode="Multiline" Rows="6" />
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
                                                <label id="lblLastAcuteInitialAssessmentDate" runat="server" class="blink-alert">
                                                    (Terakhir Tanggal : dd-MMM-yyyy)</label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsNeedChronicInitialAssessment" runat="server" Text=" Penyakit Kronis"
                                                    Checked="false" />
                                            </td>
                                            <td>
                                                <label id="lblLastChronicInitialAssessmentDate" runat="server" class="blink-alert">
                                                    (Terakhir Tanggal : dd-MMM-yyyy)</label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsGeriatricPatient" runat="server" Text="Pasien Geriatri" Checked="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                            <tr>
                                <td>
                                    <div style="position: relative;">
                                        <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                                            ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server">
                                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
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
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
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
                    <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                                                <label class="lblNormal lblLink" id="lblMedicalHistory">
                                                    <%=GetLabel("Riwayat Penyakit Dahulu") %>
                                                </label>
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
                    <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin-top: 2px">
                            <tr>
                                <td>
                                    <table runat="server" border="0" cellpadding="0" cellspacing="0" style="float: left;
                                        margin-top: 0px;">
                                        <tr>
                                            <td>
                                                <span class="lblLink" id="lblAddBodyDiagram" width="25px">
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
                                                <img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" alt=""
                                                    class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" />
                                            </td>
                                            <td>
                                                <img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" alt=""
                                                    class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" />
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
                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                EndCallback="function(s,e){ onCbpBodyDiagramViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server">
                                                    <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
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
                                </td>
                            </tr>
                        </table>
                        <table id="tblEmpty" style="display: none; width: 100%" runat="server">
                            <tr class="trEmpty">
                                <td align="center" valign="middle">
                                    <%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %>
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
                        <div id="divFormContent2" runat="server" style="height: 500px; overflow-y: auto;">
                        </div>
                    </div>
                    <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <div id="divFormContent3" runat="server" style="height: 500px; overflow-y: auto;">
                        </div>
                    </div>
                    <div id="divPage10" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <div id="divFormContent5" runat="server" style="height: 500px; overflow-y: auto;">
                        </div>
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
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
            ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
