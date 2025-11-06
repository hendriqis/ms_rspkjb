<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="MedicalResumeEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.MedicalResumeEntry1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
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
            <%=GetLabel("Batal Perubahan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    Resume Medis</div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT" && $('#<%=txtHospitalIndication.ClientID %>').val() == "") {
                        showToast('Warning', 'Indikasi Rawat Inap Harus diisi.');
                    }
                    else {
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            showLoadingPanel();
                            document.location = document.referrer;
                        }
                        else {
                            showToast('Warning', 'Anamnesa/Riwayat Penyakit Harus diisi.'); 
                        }
                    }
                }
            });

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

            setDatePicker('<%=txtResumeDate.ClientID %>');
            $('#<%=txtResumeDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtPlanFollowUpVisitDate.ClientID %>');
            $('#<%=txtPlanFollowUpVisitDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%:txtSubjectiveResumeText.ClientID %>').focus();


            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtResumeTime.ClientID %>').val())) {
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                        if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT" && $('#<%=txtHospitalIndication.ClientID %>').val() == "") {
                            showToast('Warning', 'Indikasi Rawat Inap Harus diisi.');
                        }
                        else {
                            if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                                onCustomButtonClick('save');
                                $('#<%=hdnIsChanged.ClientID %>').val('0');
                            }
                            else {
                                showToast('Warning', 'Anamnesa/Riwayat Penyakit Harus diisi.'); 
                            }
                        }
                    }
                    else {
                        showToast('Warning', 'Format Waktu yang diinput salah');                      
                    }
                }
            });

            function validateTime(timeValue) {
                var result = true;
                if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
                    result = false;
                }
                else {
                    var sHours = timeValue.split(':')[0];
                    var sMinutes = timeValue.split(':')[1];

                    if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                        result = false;
                    }
                    else if (parseInt(sHours) == 0)
                        sHours = "00";
                    else if (sHours < 10)
                        sHours = "0" + sHours;

                    if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                        result = false;
                    }
                    else if (parseInt(sMinutes) == 0)
                        sMinutes = "00";
                    else if (sMinutes < 10)
                        sMinutes = "0" + sMinutes;
                }
                return result;
            }

            $('#btnAddTemplate.imgLink').click(function () {
                if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtSubjectiveResumeText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^01|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#btnSubjectiveResumeTest.imgLink').click(function () {
                if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtSubjectiveResumeText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^02|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Are you sure to discard your changes ?";
                    showToastConfirmation(message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload();
                        }
                    });
                }
            });

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            $('#<%=grdROSView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdROSView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnReviewOfSystemID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdROSView.ClientID %> tr:eq(1)').click();

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDiagnosisID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            $('.btnApplyDiagnosis').click(function () {
                onBeforeOpenTrxPopup();
                submitDiagnosis();
                $('#trDiagnosis').attr('style', 'display:none');
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
                $('#trDiagnosis').attr('style', 'display:none');
            });

            function submitDiagnosis()
            {
                if ((cboDiagnosisType.GetValue() != '' && $('#<%=txtDiagnosisText.ClientID %>').val() != '')) {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1")
                        cbpDiagnosis.PerformCallback('add');
                    else
                        cbpDiagnosis.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill Diagnosis Type and Name field !");
                }
            }

            $('#<%=txtSubjectiveResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtSubjectiveResumeText.ClientID %>').blur(function () {
                ontxtSubjectiveResumeTextChanged($(this).val());
            });

            $('#<%=txtHospitalIndication.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            
            $('#<%=txtComorbiditiesText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtSubjectiveResumeText.ClientID %>').blur(function () {
                onTxtHPIChanged($(this).val());
            });

            $('#<%=txtMedicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtMedicationResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPrescriptionResidualEffectText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtObjectiveResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanningResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            
            $('#<%=txtDischargeMedicationResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtMedicalResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtSurgeryResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtDischargeMedicalSummary.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtInstructionResumeText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtDiagnosisText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitDiagnosis();
              }
            }); 

            registerCollapseExpandHandler();
            onCboPatientOutcomeChanged();
            onCboDischargeRoutineChanged();

            $('#leftPageNavPanel ul li').first().click();
        });

        //#region Chief Complaint
        $('#lblIndication').die('click');
        $('#lblIndication').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function ontxtSubjectiveResumeTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^01'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Chief Complaint Text from your template text ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtSubjectiveResumeText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region History of Present Illness
        $('#lblHPI').die('click');
        $('#lblHPI').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function onTxtHPIChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^02'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveResumeText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di Riwayat Penyakit sekarang dengan teks dari template ? ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtSubjectiveResumeText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
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
            var linkID = $('#<%=hdnMedicalResumeID.ClientID %>').val();
            if (linkID != '0' && linkID != '') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|0|" + linkID + "|0|0|0|";
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("Resume Medis","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromVitalSignLookup').die('click');
        $('#lblAddFromVitalSignLookup').live('click', function (evt) {
            var linkID = $('#<%=hdnMedicalResumeID.ClientID %>').val();
            if (linkID != '0' && linkID != '') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|" + linkID;
                openUserControlPopup("~/libs/Controls/EMR/Lookup/VitalSignLookupCtl1.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("Resume Medis","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var linkID = $('#<%=hdnMedicalResumeID.ClientID %>').val();
            var param = "0|" +$('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|0|0|0|0|" + linkID + "|0|0|0|";    
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Vital Sign & Indicator", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Hapus pemeriksaan Tanda Vital untuk Resume Medis ini ?";
            displayConfirmationMessageBox("Resume Medis",message, function (result) {
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

        //#region Review of System In
        var pageCount = parseInt('<%=gridROSINPageCount %>');
        $(function () {
            setPaging($("#rosInPaging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpROSInViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdROSInView.ClientID %> tr:eq(1)').click();

                setPaging($("#rosInPaging"), pageCount, function (page) {
                    cbpROSInView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdROSInView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshROSInGrid() {
            cbpROSInView.PerformCallback('refresh');
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
            var linkID = $('#<%=hdnMedicalResumeID.ClientID %>').val();
            if (linkID != '0' && linkID != '') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|" + linkID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", param, "Pemeriksaan Fisik", 700, 500);
            }
            else {
                displayMessageBox("Resume Medis","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromROSLookup').die('click');
        $('#lblAddFromROSLookup').live('click', function (evt) {
            var linkID = $('#<%=hdnMedicalResumeID.ClientID %>').val();
            if (linkID != '0' && linkID != '') {
                onBeforeOpenTrxPopup();
                if ($('#<%=hdnIsChanged.ClientID %>').val()=="0") {    
                    var param = "0|0|0|" + linkID;
                    openUserControlPopup("~/libs/Controls/EMR/Lookup/ROSLookupCtl1.ascx", param, "Pemeriksaan Fisik", 700, 500);
                }
                else
                {
                    displayMessageBox("Resume Medis","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
                }
            }
            else {
                displayMessageBox("Resume Medis","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });
        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedROS(this);
            $('#<%=hdnReviewOfSystemID.ClientID %>').val(selectedObj.ID);
            var linkID = $('#<%=hdnMedicalResumeID.ClientID %>').val();
            var param = $('#<%=hdnReviewOfSystemID.ClientID %>').val() + "|0|0|0|0|" + linkID;       
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", param, "Pemeriksaan Fisik", 700, 500);
        });

        $('.imgDeleteROS.imgLink').die('click');
        $('.imgDeleteROS.imgLink').live('click', function () {
            var message = "Are you sure to delete this physical examination record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteROS.PerformCallback();
                }
            });
        });

        var pageCount = parseInt('<%=gridROSPageCount %>');
        $(function () {
            setPaging($("#rosPaging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
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

        function onCbpROSDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpROSView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        function onRefreshROSGrid() {
            cbpROSView.PerformCallback('refresh');
        }
        //#endregion

        //#region Diagnosis
        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtDiagnosisText.ClientID %>').val($('#<%=hdnEntryDiagnoseText.ClientID %>').val());

            ledDiagnose2.SetValue(diagnoseID);
            $('#<%=hdnEntryDiagnoseID2.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText2.ClientID %>').val(led.GetDisplayText());
        }

        function onLedDiagnose2LostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID2.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText2.ClientID %>').val(led.GetDisplayText());
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
            cboDiagnosisType.SetValue($(param).attr('gcDiagnoseType'));
            cboDiagnosisStatus.SetValue($(param).attr('gcDiagnoseStatus'));
            ledDiagnose.SetValue($(param).attr('diagnoseID'));
            $('#<%=txtDiagnosisText.ClientID %>').val($(param).attr('diagnosisText'));

            cboDiagnosisTypeEKlaim.SetValue($(param).attr('claimGCDiagnoseType'));
            ledDiagnose2.SetValue($(param).attr('claimDiagnoseID'));
        }

        function ResetDiagnosisEntryControls(s) {
            if ($('#<%=hdnIsMainDiagnosisExists.ClientID %>').val() == "0")
                cboDiagnosisType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
            else
                cboDiagnosisType.SetValue(Constant.DiagnosisType.COMPLICATION);

            cboDiagnosisStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
            ledDiagnose.SetValue('');
            $('#<%=txtDiagnosisText.ClientID %>').val('');

            cboDiagnosisTypeEKlaim.SetValue('');
            ledDiagnose2.SetValue('');
        }

        $('.imgAddDiagnosis.imgLink').die('click');
        $('.imgAddDiagnosis.imgLink').live('click', function (evt) {
            ResetDiagnosisEntryControls();
            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val("1");
            $('#trDiagnosis').removeAttr('style');
        });

        $('.imgEditDiagnosis.imgLink').die('click');
        $('.imgEditDiagnosis.imgLink').live('click', function () {
            SetDiagnosisEntityToControl(this);
            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('0');
            $('#trDiagnosis').removeAttr('style');
        });

        $('.imgDeleteDiagnosis.imgLink').die('click');
        $('.imgDeleteDiagnosis.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnosis(this);

            var message = "Are you sure to delete this diagnosis <b>'" + selectedObj.DiagnosisText + "'</b> for this patient ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
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

            $('#<%=hdnIsMainDiagnosisExists.ClientID %>').val(isMainDiagnosisExists);
        }

        function onCbpDiagnosisEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('1');

                ResetDiagnosisEntryControls();
                cbpDiagnosisView.PerformCallback('refresh');
            }
            else if (param[0] == '0') {
                showToast("ERROR", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Medication

        $('#lblEpisodeMedication').die('click');
        $('#lblEpisodeMedication').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeMedicationPicksCtl1.ascx", visitID, "Terapi selama Perawatan", 1000, 500);
            }
        });

        $('#lblDischargePrescription').die('click');
        $('#lblDischargePrescription').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/DischargeMedicationPicksCtl1.ascx", visitID, "Resep Pulang", 700, 500);
            }
        });

        $('#lblPrescriptionResidualEffect').die('click');
        $('#lblPrescriptionResidualEffect').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/ResidualMedicationPicksCtl1.ascx", visitID, "Obat - obat Residual", 900, 500);
            }
        });
        //#endregion Medication

        //#region Progress Note
        $('#lblProgressNote').die('click');
        $('#lblProgressNote').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeProgressNotePicksCtl1.ascx", visitID+"|"+paramedicID, "Riwayat Catatan Perkembangan : CPPT", 700, 500);
            }
        });

        function onAfterLookUpProgressNote(param) {
            if ($('#<%=txtMedicalResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtMedicalResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;

                var message1 = "Ganti catatan yang sudah dientry di Catatan Perkembangan Pasien ?";
                var message2 = "<i>"+newText+"</i>";
                displayConfirmationMessageBox('Catatan Perkembangan Pasien', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtMedicalResumeText.ClientID %>').val(newText);
                    }
                });
            }
            else
            {
                $('#<%=txtMedicalResumeText.ClientID %>').val(param);
            }
        }
        //#endregion Progress Note

        //#region Pemeriksaan Fisik
        $('#lblObjectiveResumeText').die('click');
        $('#lblObjectiveResumeText').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND ParamedicID = " + $('#<%=hdnParamedicID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011') AND ObjectiveText IS NOT NULL";
            openSearchDialog('planningNote', filterExpression, function (value) {
                $('#<%=hdnVisitNoteID.ClientID %>').val(value);
                onSearchPatientVisitNote(value);
            });
        });

        function onSearchPatientVisitNote(value) {
            var filterExpression = "ID = " + value;
            Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtObjectiveResumeText.ClientID %>').val(result.ObjectiveText);
                }
                else {
                    $('#<%=txtObjectiveResumeText.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Pemeriksaan Penunjang

        $('#lblLaboratory').die('click');
        $('#lblLaboratory').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = "X001^004"+"|"+visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeTestOrderCtl1.ascx", param, "Pemeriksaan Penunjang - Laboratorium", 1000, 500);
            }
        });

        $('#lblImaging').die('click');
        $('#lblImaging').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = "X001^005"+"|"+visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeTestOrderCtl1.ascx", param, "Pemeriksaan Penunjang - Radiologi", 1000, 500);
            }
        });

        $('#lblOtherDiagnostic').die('click');
        $('#lblOtherDiagnostic').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = "X001^006"+"|"+visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeTestOrderCtl1.ascx", param, "Pemeriksaan Penunjang - Lain - lain", 1000, 500);
            }
        });
        //#endregion Pemeriksaan Penunjang


        //#region Prosedur Terapi dan Tindakan

        $('#lblSurgeryProcedure').die('click');
        $('#lblSurgeryProcedure').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeSurgeryProcedureCtl1.ascx", param, "Tindakan Kamar Operasi", 1000, 500);
            }
        });

        $('#lblProcedure').die('click');
        $('#lblProcedure').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                var param = visitID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/MedicalResume/EpisodeProcedureCtl1.ascx", param, "Tindakan", 1000, 500);
            }
        });

        //#endregion Prosedur Terapi dan Tindakan

        function onCboPatientOutcomeChanged() {
            if (cboPatientOutcome.GetValue() != null && (cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_AFTER_48)) {
                $('#trDeathInfo').removeAttr('style');
            }
            else {
                $('#trDeathInfo').attr('style', 'display:none');
            }
        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#trDischargeOtherReason').removeAttr('style');
            }
            else {
                $('#trDischargeOtherReason').attr('style', 'display:none');
            }
        }

        function onCboDischargeRoutineChanged() {
            if (cboDischargeRoutine.GetValue() != null && (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT)) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    $("#tblDischarge tr.trAppointment").hide();
                    $("#tblReferralNotes").hide();
                    $('#trReferrerGroup').removeAttr('style');
                    $('#trReferrer').removeAttr('style');
                    $('#trDischargeReason').removeAttr('style');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                }
                else {
                    $("#tblDischarge tr.trAppointment").show();
                    $("#tblReferralNotes").show();
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                }
            }
            else {
                $("#tblDischarge tr.trAppointment").hide();
                $("#tblReferralNotes").hide();
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
            }
        }

        //#region Referrer
        function onCboReferrerGroupValueChanged() {
            if ($('#<%=hdnGCReferrerGroup.ClientID %>').val() != cboReferrerGroup.GetValue()) {
                $('#<%=hdnReferrerID.ClientID %>').val('');
                $('#<%=txtReferrerCode.ClientID %>').val('');
                $('#<%=txtReferrerName.ClientID %>').val('');    
            }
            $('#<%=hdnGCReferrerGroup.ClientID %>').val(cboReferrerGroup.GetValue());
            return "GCReferrerGroup = '" + cboReferrerGroup.GetValue() + "'";
        }

        $('#<%:lbReferrerCode.ClientID %>.lblLink').live('click', function () {
            var filterExpression = onCboReferrerGroupValueChanged() + " AND IsDeleted = 0"
            openSearchDialog('referrer2', filterExpression, function (value) {
                $('#<%=txtReferrerCode.ClientID %>').val(value);
                onTxtReferrerCodeChanged(value);
            });
        });

        $('#<%=txtReferrerCode.ClientID %>').live('change', function () {
            onTxtReferrerCodeChanged($(this).val());
        });

        function onTxtReferrerCodeChanged(value) {
            var filterExpression = onCboReferrerGroupValueChanged() + " AND BusinessPartnerCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtReferrerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtReferrerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnReferrerID.ClientID %>').val('');
                    $('#<%=txtReferrerCode.ClientID %>').val('');
                    $('#<%=txtReferrerName.ClientID %>').val('');
                }
            });
        }
        //#endregion


        $('#lblPhysicianInstruction').die('click');
        $('#lblPhysicianInstruction').live('click', function (evt) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (visitID != '0' && visitID != '') {
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/Instruction/PhysicianInstructionQuickPicks1Ctl.ascx", "", "Template Instruksi dan Rencana Tindak Lanjut", 700, 600);
            }
        });


        function onBeforeLoadRightPanelContent(code) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (code == 'healthyinformation' || code == 'medicalSickLeave' || code == 'medicalSickLeaveBilingual') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                showToastConfirmation(message, function (result) {
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
            var message = "Your record is not saved yet, Do you want to save ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion     
                
        function onAfterLookUpDiagnosticTest(param) {
            if ($('#<%=txtPlanningResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtPlanningResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;
                $('#<%=txtPlanningResumeText.ClientID %>').val(newText);
            }
            else
            {
                $('#<%=txtPlanningResumeText.ClientID %>').val(param);
            }
        }

        function onAfterLookUpSurgeryProcedure(param) {
            if ($('#<%=txtSurgeryResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtSurgeryResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;
                $('#<%=txtSurgeryResumeText.ClientID %>').val(newText);
            }
            else
            {
                $('#<%=txtSurgeryResumeText.ClientID %>').val(param);
            }
        }

        function onAfterLookUpProcedure(param) {
            if ($('#<%=txtSurgeryResumeText.ClientID %>').val() != "") {
                var previousText = $('#<%=txtSurgeryResumeText.ClientID %>').val();
                var newText = previousText + "\n"+param;
                $('#<%=txtSurgeryResumeText.ClientID %>').val(newText);
            }
            else
            {
                $('#<%=txtSurgeryResumeText.ClientID %>').val(param);
            }
        }

        function onAfterLookUpEpisodePrescription(param) {
            if ($('#<%=txtMedicationResumeText.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Ringkasan Terapi Obat Selama Perawatan ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Terapi Obat Selama Perawatan', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtMedicationResumeText.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtMedicationResumeText.ClientID %>').val(param);
            }
        } 
        
        function onAfterLookUpDischargePrescription(param) {
            if ($('#<%=txtDischargeMedicationResumeText.ClientID %>').val() != "") {                
                var message1 = "Ganti catatan yang sudah dientry di Ringkasan Terapi Obat Pulang ?";
                var message2 = "<i>"+param+"</i>";
                var previousText = $('#<%=txtDischargeMedicationResumeText.ClientID %>').val();
                var newText = previousText + "\n" + param;
                displayConfirmationMessageBox('Terapi Obat Pulang', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                    $('#<%=txtDischargeMedicationResumeText.ClientID %>').val(newText);
                    }
                });
            }
            else
            {
                $('#<%=txtDischargeMedicationResumeText.ClientID %>').val(param);
            }
        }

        function onAfterLookUpPrescriptionResidualEffect(param) {
            if ($('#<%=txtPrescriptionResidualEffectText.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Ringkasan Obat Residual ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Obat Residual', message1 + "<br/><br/>" + message2, function (result) {
                    if (result) {
                        $('#<%=txtPrescriptionResidualEffectText.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtPrescriptionResidualEffectText.ClientID %>').val(param);
            }
        } 

        function onAfterSelectFromInstructionTemplate(param) {
            if ($('#<%=txtInstructionResumeText.ClientID %>').val() != "") {
                var message1 = "Ganti catatan yang sudah dientry di Instruksi dan Rencana Tindak Lanjut ?";
                var message2 = "<i>"+param+"</i>";
                displayConfirmationMessageBox('Instruksi dan Rencana Tindak Lanjut', message1+"<br/><br/>"+message2, function (result) {
                    if (result) {
                        $('#<%=txtInstructionResumeText.ClientID %>').val(param);
                    }
                });
            }
            else
            {
                $('#<%=txtInstructionResumeText.ClientID %>').val(param);
            }
        } 
        
        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnMedicalResumeID.ClientID %>').val(retval);
        }                                      

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();

            if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019' || code == 'PM-90008' || code == 'PM-90009' ||
                code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013' || code == 'PM-90014' || code == 'PM-90015' || 
                code == 'PM-90019' || code == 'PM-90020' || code == 'PM-90022' || code == 'PM-90023' || code == "PM-90056" || code == "PM-90085") {
                filterExpression.text = visitID;
                return true;
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnMedicalResumeID" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnTestOrderHealthcareServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnImagingTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticTestOrderID" runat="server" />
        <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
        <input type="hidden" runat="server" id="hdnPastMedicalID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" id="hdnIsMainDiagnosisExists" runat="server" value='0' />
        <input type="hidden" value="1" id="hdnInstructionProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnInstructionID" runat="server" />
        <input type="hidden" value="0" id="hdnPatientVisitNoteID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnRevisionParamedicID" runat="server" />
        <input type="hidden" id="hdnGCReferrerGroup" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnVisitNoteID" value="0" />
        <input type="hidden" runat="server" id="hdnIsCasemixRevision" value="0" />
        <input type="hidden" runat="server" id="hdnIsCasemixAllowChange" value="0" />
        <input type="hidden" runat="server" id="hdnIsHasResidualEffect" value="0" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <div id="leftPageNavPanel" class="w3-border">
                            <ul>
                                <li contentid="divPage1" title="Ringkasan Riwayat Penyakit" class="w3-hover-red">Ringkasan
                                    Riwayat Penyakit</li>
                                <li contentid="divPage11" title="Pemeriksaan Fisik Saat Masuk" class="w3-hover-red"
                                    id="divPage11_tab" runat="server" style="display: none;">Pemeriksaan Fisik Saat
                                    Masuk</li>
                                <li contentid="divPage2" id="divPage2_tab" class="w3-hover-red" runat="server"></li>
                                <li contentid="divPage3" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">
                                    Tanda Vital dan Indikator Lainnya</li>
                                <li contentid="divPage4" title="Pemeriksaan Penunjang" class="w3-hover-red">Pemeriksaan
                                    Penunjang</li>
                                <li contentid="divPage5" title="Diagnosa" class="w3-hover-red">Diagnosa</li>
                                <li contentid="divPage6" title="Ringkasan Terapi Pengobatan" class="w3-hover-red">Ringkasan
                                    Terapi Pengobatan</li>
                                <li contentid="divPage7" title="Perkembangan selama Perawatan" class="w3-hover-red">
                                    Perkembangan selama Perawatan</li>
                                <li contentid="divPage8" title="Prosedur Terapi dan Tindakan" class="w3-hover-red">Prosedur
                                    Terapi dan Tindakan</li>
                                <li contentid="divPage9" title="Kondisi dan Cara Pulang" class="w3-hover-red">Kondisi
                                    dan Cara Pulang</li>
                                <li contentid="divPage10" title="Instruksi dan Rencana Tindak Lanjut" class="w3-hover-red">
                                    Instruksi dan Rencana Tindak Lanjut</li>
                            </ul>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Waktu")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtResumeDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtResumeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                        MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trHospitalIndication" runat="server">
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width: 190px">
                                                    <label class="lblMandatory lblLink" id="lblIndication">
                                                        <%=GetLabel("Indikasi Rawat Inap")%></label>
                                                </td>
                                                <td style="display: none">
                                                    <img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>'
                                                        src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHospitalIndication" runat="server" TextMode="MultiLine" Rows="2"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width: 190px">
                                                    <label class="lblMandatory lblLink" id="lblHPI">
                                                        <%=GetLabel("Anamnesa/Riwayat Penyakit")%></label>
                                                </td>
                                                <td style="display: none">
                                                    <img class="imgLink" id="btnSubjectiveResumeTest" title='<%=GetLabel("Add to My Template")%>'
                                                        src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubjectiveResumeText" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width: 190px">
                                                    <label class="lblNormal" id="Label2">
                                                        <%=GetLabel("Komorbiditas")%></label>
                                                </td>
                                                <td style="display: none">
                                                    <img class="imgLink" id="Img1" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>'
                                                        alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComorbiditiesText" runat="server" TextMode="MultiLine" Rows="2"
                                            Width="100%" />
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
                                                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage5" Style="height: 300px">
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
                                                                                <%=GetLabel("Belum ada pemeriksaan fisik untuk resume medis ini") %>
                                                                                <div style="text-align: right">
                                                                                    <span class="lblLink" id="lblAddROS">
                                                                                        <%= GetLabel("+ Tambah Pemeriksaan Fisik")%></span>
                                                                                </div>
                                                                                <div style="text-align: right">
                                                                                    <span class="lblLink" id="lblAddFromROSLookup">
                                                                                        <%= GetLabel("+ Salin Pemeriksaan Fisik")%></span>
                                                                                </div>
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
                                                                <div id="rosPaging" style="display: none">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblObjectiveResumeText" title="Salin Text Objective dari CPPT">
                                            <%=GetLabel("Catatan Tambahan Pemeriksaan Fisik") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="5" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage11" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                            <tr>
                                                <td>
                                                    <div style="position: relative;">
                                                        <dxcp:ASPxCallbackPanel ID="cbpROSInView" runat="server" Width="100%" ClientInstanceName="cbpROSInView"
                                                            ShowLoadingPanel="false" OnCallback="cbpROSInView_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent2" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage5" Style="height: 300px">
                                                                        <asp:GridView ID="grdROSInView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                            OnRowDataBound="grdROSINView_RowDataBound">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                    <%-- <ItemTemplate>
                                                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                    </ItemTemplate>--%>
                                                                                </asp:TemplateField>
                                                                                <%--  <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
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
                                                                                </asp:TemplateField>--%>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
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
                                                                                            <asp:Repeater ID="rptReviewOfSystemINDt" runat="server">
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
                                                                                <%=GetLabel("Belum ada pemeriksaan fisik untuk resume medis ini") %>
                                                                                <%-- <div style="text-align: right">
                                                                                    <span class="lblLink" id="lblAddROS"><%= GetLabel("+ Tambah Pemeriksaan Fisik")%></span>
                                                                                </div>
                                                                                <div style="text-align: right">
                                                                                    <span class="lblLink" id="lblAddFromROSLookup">
                                                                                        <%= GetLabel("+ Salin Pemeriksaan Fisik")%></span>
                                                                                </div>--%>
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
                                                                <div id="rosInPaging" style="display: none">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
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
                                                                    <%=GetLabel("Belum ada pemeriksaan tanda vital untuk resume medis ini") %>
                                                                    <div style="text-align: right">
                                                                        <span class="lblLink" id="lblAddVitalSign">
                                                                            <%= GetLabel("+ Tambah Tanda Vital")%></span>
                                                                    </div>
                                                                    <div style="text-align: right">
                                                                        <span class="lblLink" id="lblAddFromVitalSignLookup">
                                                                            <%= GetLabel("+ Lookup Tanda Vital")%></span>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="vitalSignPaging" style="display: none">
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
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align: left">
                                                    <label class="lblNormal" id="Label1">
                                                        <%=GetLabel("Catatan Pemeriksaan Penunjang") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <label class="lblLink" id="lblLaboratory">
                                                        <%=GetLabel("Laboratorium") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <label class="lblLink" id="lblImaging">
                                                        <%=GetLabel("Radiologi") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <label class="lblLink" id="lblOtherDiagnostic">
                                                        <%=GetLabel("Lain-lain") %></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                <tr id="trDiagnosis" style="display: none">
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <colgroup>
                                                <col width="180px" />
                                                <col width="150px" />
                                                <col width="180px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tipe Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisType" ClientInstanceName="cboDiagnosisType"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ICD X")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                                        Width="99%" ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName"
                                                        MethodName="GetDiagnosisList">
                                                        <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                                Description="i.e. Cholera" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Status Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisStatus" ClientInstanceName="cboDiagnosisStatus"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Diagnosa Text")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiagnosisText" runat="server" Width="370px" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
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
                                            <tr>
                                                <td colspan="5">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="trEKlaim1" runat="server" style="display: none">
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tipe Diagnosa (e-Klaim)")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisTypeEKlaim" ClientInstanceName="cboDiagnosisTypeEKlaim"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ICD X")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseID2" runat="server" />
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseText2" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledDiagnose2" ClientInstanceName="ledDiagnose2" runat="server"
                                                        Width="99%" ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName"
                                                        MethodName="GetDiagnosisList">
                                                        <ClientSideEvents ValueChanged="function(s){ onLedDiagnose2LostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                                Description="i.e. Cholera" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
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
                                                                            <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                            <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                            <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddDiagnosis imgLink" title='<%=GetLabel("+ Diagnosa")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditDiagnosis imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" diagnoseid="<%#:Eval("DiagnoseID") %>" diagnosistext="<%#:Eval("DiagnosisText") %>"
                                                                                            gcdiagnosetype="<%#:Eval("GCDiagnoseType") %>" gcdiagnosestatus="<%#:Eval("GCDifferentialStatus") %>"
                                                                                            claimdiagnoseid="<%#:Eval("ClaimDiagnosisID") %>" claimgcdiagnosetype="<%#:Eval("GCDiagnoseTypeClaim") %>" />
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
                                                                            <%=GetLabel("Diagnose Information")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("DifferentialDateInString")%>,
                                                                                <%#: Eval("DifferentialTime")%>,
                                                                                <%#: Eval("cfParamedicNameEarlyDiagnosis")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <span style="color: Blue; font-size: 1.1em">
                                                                                    <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                                            </div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("ICDBlockName")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <b>
                                                                                    <%#: Eval("DiagnoseType")%></b> -
                                                                                <%#: Eval("DifferentialStatus")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("Remarks")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                            <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                            <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                            <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                            <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                            <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                                            <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                                            <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                                            <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                                            <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
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
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblEpisodeMedication">
                                            <%=GetLabel("Ringkasan Terapi Pengobatan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="6" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblDischargePrescription">
                                            <%=GetLabel("Terapi Obat Pulang") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="4" />
                                    </td>
                                </tr>
                                <tr id="trResidualEffect" runat="server" style="display: none">
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblPrescriptionResidualEffect">
                                            <%=GetLabel("Obat Residual") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrescriptionResidualEffectText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="4" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblProgressNote">
                                            <%=GetLabel("Perkembangan Selama Perawatan") %>
                                        </label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicalResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td style="text-align: left">
                                                    <label class="lblNormal" id="Label3">
                                                        <%=GetLabel("Prosedur Terapi dan Tindakan") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <label class="lblLink" id="lblSurgeryProcedure">
                                                        <%=GetLabel("Kamar Operasi") %></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <label class="lblLink" id="lblProcedure" title="Daftar Tindakan yang membutuhkan pengkodean Prosedur">
                                                        <%=GetLabel("Tindakan") %></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSurgeryResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="18" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="190px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Kondisi Klinis saat Pulang") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDischargeMedicalSummary" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Keadaan Keluar")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="350px"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Cara Keluar")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDischargeRoutine" ClientInstanceName="cboDischargeRoutine"
                                            Width="350px" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboDischargeRoutineChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trDeathInfo" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Jam Meninggal")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDateOfDeath" CssClass="datepicker" Width="120px" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTimeOfDeath" CssClass="time" Width="80px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trReferrerGroup" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Rujuk Ke")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboReferrerGroup" Width="100%" runat="server" ClientInstanceName="cboReferrerGroup">
                                            <ClientSideEvents Init="function(s,e){ onCboReferrerGroupValueChanged(s); }" ValueChanged="function(s,e){ onCboReferrerGroupValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trReferrer" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lbReferrerCode" runat="server">
                                            <%:GetLabel("Rumah Sakit / Faskes")%></label>
                                    </td>
                                    <td colspan="5">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                                    <asp:TextBox ID="txtReferrerCode" Width="150px" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReferrerName" Width="330px" ReadOnly="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trDischargeReason" style="display: none">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Alasan Pasien Dirujuk")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboDischargeReason" Width="100%" runat="server" ClientInstanceName="cboDischargeReason">
                                            <ClientSideEvents Init="function(s,e){ onCboDischargeReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeReasonValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trDischargeOtherReason" style="display: none">
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDischargeOtherReason" Width="100%" runat="server" />
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
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Surat Keterangan Sakit ")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblIsHasSickLetter" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoOfDays" Width="60px" runat="server" CssClass="number" Text="0" />
                                                </td>
                                                <td class="tdLabel">
                                                    <label>
                                                        <%=GetLabel("hari ")%></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Rencana Kontrol Kembali ")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPlanFollowUpVisitDate" CssClass="datepicker" Width="120px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal lblLink" id="lblPhysicianInstruction">
                                            <%=GetLabel("Instruksi dan Rencana Tindak Lanjut") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstructionResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="tblContentArea" style="display: none">
                <colgroup>
                    <col style="width: 45%" />
                    <col style="width: 55%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <h4 class="h4collapsed">
                            <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                        <div class="containerTblEntryContent">
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
                            <%=GetLabel("Diagnosa")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
                ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteROS" runat="server" Width="100%" ClientInstanceName="cbpDeleteROS"
                ShowLoadingPanel="false" OnCallback="cbpDeleteROS_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpROSDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
                ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
