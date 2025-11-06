<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MDInitialAssessmentEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDInitialAssessmentEntry1" %>

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
            setDatePicker('<%=txtAsessmentDate.ClientID %>');
            $('#<%=txtAsessmentDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtAsessmentDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtAsessmentTime1.ClientID %>').change(function () {
                if ($('#<%=txtAsessmentTime1.ClientID %>').val() >= 0 && $('#<%=txtAsessmentTime1.ClientID %>').val() < 24 && $('#<%=txtAsessmentTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtAsessmentTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtAsessmentTime2.ClientID %>').change(function () {
                if ($('#<%=txtAsessmentTime2.ClientID %>').val() >= 0 && $('#<%=txtAsessmentTime2.ClientID %>').val() < 60 && $('#<%=txtAsessmentTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtAsessmentTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

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
            $('#<%=txtFamilyHistory.ClientID %>').keydown(function () {
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

            $('#<%=rblIsHasFinancialProblem.ClientID %> input').change(function () {
                var value = $("#<%= rblIsHasFinancialProblem.ClientID %> input:checked").val();
                if (value == "1") {
                    $('#<%=txtFinancialProblemRemarks.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#<%=txtFinancialProblemRemarks.ClientID %>').attr('readonly', 'readonly'); ;
                }
            });

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDiagnosisID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnAllergyID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();


            $('.btnApplyAllergy').click(function () {
                submitAllergy();
                $('#<%=txtAllergenName.ClientID %>').focus();
            });

            $('.btnCancelAllergy').click(function () {
                ResetAllergyEntryControls();
            });

            function submitAllergy() {
                if (($('#<%=txtAllergenName.ClientID %>').val() != '' && $('#<%=txtReaction.ClientID %>').val() != '')) {
                    if ($('#<%=hdnAllergyProcessMode.ClientID %>').val() == "1")
                        cbpAllergy.PerformCallback('add');
                    else
                        cbpAllergy.PerformCallback('edit');
                }
                else {
                    displayErrorMessageBox("Alergi Pasien", "Nama dan reaksi alergi harus diisi !");
                }
            }

            $('#<%=txtReaction.ClientID %>').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    submitAllergy();
                }
            });

            $('.btnApplyDiagnosis').click(function () {
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
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

        //#region Allergy

        var pageCount = parseInt('<%=gridAllergyPageCount %>');
        $(function () {
            setPaging($("#llergyPaging"), pageCount, function (page) {
                cbpAllergyView.PerformCallback('changepage|' + page);
            });
        });

        function getSelectedAllergy() {
            return $('#<%=grdAllergyView.ClientID %> tr.selected');
        }

        function onCbpAllergyEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnAllergyProcessMode.ClientID %>').val('1');

                ResetAllergyEntryControls();
                cbpAllergyView.PerformCallback('refresh');

                if (param[2] == "1") {
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').attr("disabled", "disabled");
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').prop("checked", false);
                }
                else {
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').prop("checked", true);
                }

                if (typeof RefreshPatientBanner == 'function')
                    RefreshPatientBanner();
            }
            else if (param[0] == '0') {
                showToast("ERROR", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
        }

        function GetCurrentSelectedAllergy(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdAllergyView.ClientID %> tr').index($tr);
            $('#<%=grdAllergyView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdAllergyView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetAllergyEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedAllergy(param);

            cboAllergenType.SetValue(selectedObj.GCAllergenType);
            $('#<%=txtAllergenName.ClientID %>').val(selectedObj.Allergen);
            $('#<%=txtReaction.ClientID %>').val(selectedObj.Reaction);
        }


        $('.imgEditAllergy.imgLink').die('click');
        $('.imgEditAllergy.imgLink').live('click', function () {
            SetAllergyEntityToControl(this);
            $('#<%=hdnAllergyProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteAllergy.imgLink').die('click');
        $('.imgDeleteAllergy.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedAllergy(this);

            var message = "Are you sure to delete this allergy record for this patient with allergen name <b>'" + selectedObj.Allergen + "'</b> ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpAllergy.PerformCallback('delete');
                }
            });
        });

        function ResetAllergyEntryControls(s) {
            cboAllergenType.SetValue(Constant.AllergenType.DRUG);
            $('#<%=txtAllergenName.ClientID %>').val('');
            $('#<%=txtReaction.ClientID %>').val('');
        }

        function onCbpAllergyViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

                setPaging($("#allergyPaging"), pageCount, function (page) {
                    cbpAllergyView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
        }
        //#endregion


        //#region Vital Sign
        var pageCount = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            var assessmentID = $('#<%=hdnID.ClientID %>').val();
            var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var testOrderID = "0";
                var date = $('#<%=txtAsessmentDate.ClientID %>').val();
                var time = $('#<%=txtAsessmentTime1.ClientID %>').val() + ":" + $('#<%=txtAsessmentTime2.ClientID %>').val();
                var paramedicID = $(this).attr('preoperativeSurgeryNurseID');
                var remarks = $(this).attr('remarks');
                var healthcareServiceUnitID = $(this).attr('healthcareServiceUnitID');
                var vitalSignID = "0";
                var param = Constant.VitalSignAssessmentType.HSU_NURSING_ASSESSMENT + "|" + assessmentID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID + "|" + healthcareServiceUnitID + "|" + vitalSignID;
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/ServiceUnitVitalSignEntry.ascx");
                openUserControlPopup(url, param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("Asesmen Penunjang", "Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });


        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            var assessmentID = $('#<%=hdnID.ClientID %>').val();
            var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
            onBeforeOpenTrxPopup();
            var testOrderID = "0";
            var date = $('#<%=txtAsessmentDate.ClientID %>').val();
            var time = $('#<%=txtAsessmentTime1.ClientID %>').val() + ":" + $('#<%=txtAsessmentTime2.ClientID %>').val();
            var paramedicID = $(this).attr('preoperativeSurgeryNurseID');
            var remarks = $(this).attr('remarks');
            var healthcareServiceUnitID = $(this).attr('healthcareServiceUnitID');
            var vitalSignID = $('#<%=hdnVitalSignRecordID.ClientID %>').val();
            var param = Constant.VitalSignAssessmentType.HSU_NURSING_ASSESSMENT + "|" + assessmentID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID + "|" + healthcareServiceUnitID + "|" + vitalSignID;
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/ServiceUnitVitalSignEntry.ascx");
            openUserControlPopup(url, param, "Tanda Vital & Indikator Lainnya", 700, 500);
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
            }
            else {
                displayErrorMessageBox("DELETE : Vital Sign", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
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
            var assessmentID = $('#<%=hdnID.ClientID %>').val();
            var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
            if (assessmentID != '0' && assessmentID != '') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|0|" + assessmentID;
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/ReviewOfSystem/ROSEntry2Ctl.ascx", param, "Pemeriksaan Fisik", 700, 500);
            }
            else {
                displayMessageBox("Asesment Penunjang Medis", "Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }


        });

        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var assessmentID = $('#<%=hdnID.ClientID %>').val();
            var param = recordID + "|0|0|0|0|0|" + assessmentID;
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/ReviewOfSystem/ROSEntry2Ctl.ascx", param, "Pemeriksaan Fisik", 700, 500);
        });

        $('.imgDeleteROS.imgLink').die('click');
        $('.imgDeleteROS.imgLink').live('click', function () {
            $('#<%=hdnReviewOfSystemID.ClientID %>').val($(this).attr('recordID'));
            var message = "Hapus pengkajian pemeriksaan fisik untuk asesmen ini ?";
            displayConfirmationMessageBox("Pemeriksaan Fisik", message, function (result) {
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
            }
            else {
                displayErrorMessageBox("DELETE - ROS", param[1]);
            }
        }

        function onRefreshROSGrid() {
            cbpROSView.PerformCallback('refresh');
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
            var serviceDateInString = $('#<%=txtAsessmentDate.ClientID %>').val();
            var serviceTime = $('#<%=txtAsessmentTime1.ClientID %>').val() + ":" + $('#<%=txtAsessmentTime2.ClientID %>').val();
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
                getDischargePlanningFormValues();
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

        function onAfterVisitHistoryLookUp(param) {
            var text = param.replace("&nbsp", "");
            $('#<%=txtMedicalHistory.ClientID %>').val(text);
        }

        function onAfterCustomClickSuccessSetRecordID(param) {
            $('#<%=hdnID.ClientID %>').val(param);
            showLoadingPanel();
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
        <input type="hidden" runat="server" id="hdnMenuType" value="" />
        <input type="hidden" runat="server" id="hdnDeptType" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" id="hdnIsPatientAllergyExists" runat="server" />
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
        <input type="hidden" runat="server" id="hdnPsychosocialLayout" value="" />
        <input type="hidden" runat="server" id="hdnPsychosocialValue" value="" />
        <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
        <input type="hidden" runat="server" id="hdnEducationValue" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalLayout" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalValue" value="" />
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
                            <li contentid="divPage6" title="Riwayat Alergi" class="w3-hover-red">Riwayat Alergi</li>
                            <li contentid="divPage7" title="Riwayat Penyakit Keluarga" class="w3-hover-red">Riwayat
                                Penyakit Keluarga</li>
                            <li contentid="divPage8" title="Ketergantungan Fungsional" class="w3-hover-red">Ketergantungan
                                Fungsional</li>
                            <li contentid="divPage9" title="Kesehatan Lingkungan dan Status Ekonomi" class="w3-hover-red">
                                Kesehatan Lingkungan dan Status Ekonomi</li>
                            <li contentid="divPage10" title="Psikososial Spiritual dan Kultural" class="w3-hover-red">
                                Psikososial Spiritual dan Kultural</li>
                            <li contentid="divPage11" title="Kebutuhan Informasi dan Edukasi" class="w3-hover-red">
                                Kebutuhan Informasi dan Edukasi</li>
                            <%--                            <li contentid="divPage12" title="Asesmen RAPUH" class="w3-hover-red">Asesmen RAPUH (Frailty
                                syndrome)</li>--%>
                            <li contentid="divPage13" title="Asesmen Tambahan (Populasi Khusus)" class="w3-hover-red">
                                Asesmen Tambahan (Populasi Khusus)</li>
                            <li contentid="divPage14" title="Masalah Keperawatan" class="w3-hover-red">Masalah Keperawatan</li>
                            <li contentid="divPage15" title="Catatan Planning" class="w3-hover-red">Catatan Planning</li>
                            <li contentid="divPage16" title="Catatan Instruksi" class="w3-hover-red">Catatan Instruksi</li>
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
                                                <asp:TextBox ID="txtAsessmentDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                                            <asp:TextBox ID="txtAsessmentTime1" Width="40px" CssClass="number" runat="server"
                                                                Style="text-align: center" MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAsessmentTime2" Width="40px" CssClass="number" runat="server"
                                                                Style="text-align: center" MaxLength="2" max="59" min="0" />
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
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto Anamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo Anamnesis" Checked="false" />
                                        </td>
                                        <td class="tdLabel" style="width: 200px">
                                            <label class="lblNormal" id="lblFamilyRelation">
                                                <%=GetLabel("Hubungan dengan Pasien")%></label>
                                        </td>
                                        <td style="width: 130px">
                                            <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                Width="100%" />
                                        </td>
                                        <td style="padding-left: 10px">
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
                                                    (Terakhir tanggal : dd-MMM-yyyy)</label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsNeedChronicInitialAssessment" runat="server" Text=" Penyakit Kronis"
                                                    Checked="false" />
                                            </td>
                                            <td>
                                                <label id="lblLastChronicInitialAssessmentDate" runat="server" class="blink-alert">
                                                    (Terakhir tanggal : dd-MMM-yyyy)</label>
                                            </td>
                                        </tr>
                                    </table>
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
                                                                <%=GetLabel("Belum ada pengkajian tanda vital untuk asesmen ini") %>
                                                                <br />
                                                                <span class="lblLink" id="lblAddVitalSign">
                                                                    <%= GetLabel("+ Tambah Tanda Vital")%></span>
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
                            <colgroup>
                                <col width="175px" />
                                <col width="200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="3">
                                    <div style="position: relative;">
                                        <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                                            ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server">
                                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage6">
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
                                                                                        alt="" recordid="<%#:Eval("ID") %>" />
                                                                                </td>
                                                                                <td style="width: 1px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <img class="imgDeleteROS imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" recordid="<%#:Eval("ID") %>" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
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
                                                                <%=GetLabel("Belum ada pengkajian pemeriksaan fisik untuk asesmen ini") %>
                                                                <br />
                                                                <span class="lblLink" id="lblAddROS">
                                                                    <%= GetLabel("+ Tambah Pemeriksaan Fisik")%></span>
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
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <tr>
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
                                            <td colspan="6">
                                                <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text=" Tidak ada Alergi"
                                                    Checked="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Tipe Alergi")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType"
                                                    Width="100px">
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Jenis/Nama")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAllergenName" runat="server" Width="150px" />
                                            </td>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Reaksi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReaction" runat="server" Width="150px" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <table border="0" cellpadding="0" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <img class="btnApplyAllergy imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td>
                                                            <img class="btnCancelAllergy imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                    <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                                        ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                                                    <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                                                    <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                                                                    <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEditAllergy imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDeleteAllergy imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data alergi pasien dalam episode ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="allergyPaging">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <label class="lblNormal" id="Label2">
                                        <%=GetLabel("Riwayat Penyakit Keluarga") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFamilyHistory" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="10" />
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
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Ketergantungan Fungsional")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboFunctionalType" ClientInstanceName="cboFunctionalType"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFunctionalTypeRemarks" CssClass="txtFunctionalTypeRemarks" Width="98%"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="175px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Terdapat Masalah ekonomi")%></label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblIsHasFinancialProblem" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Table">
                                        <asp:ListItem Text=" Ya" Value="1" Class="chkIsHasFinancialProblem" />
                                        <asp:ListItem Text=" Tidak" Value="0" Class="chkIsHasFinancialProblem" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Masalah ekonomi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFinancialProblemRemarks" CssClass="txtFunctionalTypeRemarks"
                                        Width="98%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kondisi perumahan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboHousingCondition" ClientInstanceName="cboHousingCondition"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHousingConditionRemarks" CssClass="txtHousingConditionRemarks"
                                        Width="98%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage10" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <div id="divFormContent2" runat="server" style="height: 500px; overflow-y: auto;">
                        </div>
                    </div>
                    <div id="divPage11" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <div id="divFormContent3" runat="server" style="height: 500px; overflow-y: auto;">
                        </div>
                    </div>
                    <div id="divPage12" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage13" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <div id="divFormContent5" runat="server" style="height: 500px; overflow-y: auto;">
                        </div>
                    </div>
                    <div id="divPage14" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                    <div id="divPage15" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <label class="lblNormal" id="Label1">
                                        <%=GetLabel("Catatan Planning") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlanningNotes" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="10" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage16" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <label class="lblNormal" id="Label3">
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
        <dxcp:ASPxCallbackPanel ID="cbpAllergy" runat="server" Width="100%" ClientInstanceName="cbpAllergy"
            ShowLoadingPanel="false" OnCallback="cbpAllergy_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteROS" runat="server" Width="100%" ClientInstanceName="cbpDeleteROS"
            ShowLoadingPanel="false" OnCallback="cbpDeleteROS_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteROSEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
            ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
