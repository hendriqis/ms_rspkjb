<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ProcedurePreAnesthesyEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcedurePreAnesthesyEntry1" %>

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
            <%=GetLabel("Kembali ke Daftar")%></div>
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
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    Asesmen Pra Anestesi</div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
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

            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');
            setDatePicker('<%=txtStartFastingDate.ClientID %>');
            $('#<%=txtStartFastingDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtAnamnesisText.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtServiceTime.ClientID %>').val())) {
                        getPhysicalExamFormValues();
                        getDiagnosticTestChecklistFormValues();
                        getAnesthesyPlanningFormValues();
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                    else {
                        displayErrorMessageBox('Asesmen', 'Format Waktu pengisian asesmen yang diinput salah');                      
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
                if ($('#<%=txtAnamnesisText.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtAnamnesisText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^01|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Are you sure to discard your changes ?";
                    displayConfirmationMessageBox("Asesmen Pra Anestesi",message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload();
                        }
                    });
                }
            });

            $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnAllergyID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            $('.btnApplyAllergy').click(function () {
                submitAllergy();
                $('#<%=txtAllergenName.ClientID %>').focus();
            });

            $('.btnCancelAllergy').click(function () {
                ResetAllergyEntryControls();
            });

            function submitAllergy()
            {
                if (($('#<%=txtAllergenName.ClientID %>').val() != '' && $('#<%=txtReaction.ClientID %>').val() != '')) {
                    if ($('#<%=hdnAllergyProcessMode.ClientID %>').val() == "1")
                        cbpAllergy.PerformCallback('add');
                    else
                        cbpAllergy.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill allergen name and allergy reaction !");
                }
            }

            $('#<%=txtAnamnesisText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtAnamnesisText.ClientID %>').blur(function () {
                ontxtAnamnesisTextChanged($(this).val());
            });

            $('#<%=txtPastSurgicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtMedicationHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });


            $('#<%:txtDiagnosticResultSummary.ClientID %>').die('blur');
            $('#<%:txtDiagnosticResultSummary.ClientID %>').blur(function () {
                ontxtDiagnosticResultSummaryTextChanged($(this).val());
            });


            $('#<%=txtReaction.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitAllergy();
              }
            }); 

            //#region Form Values
            if ($('#<%=hdnPhysicalExamValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnPhysicalExamValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            if ($('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            if ($('#<%=hdnAnesthesyPlanningValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnAnesthesyPlanningValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            //#endregion

            registerCollapseExpandHandler();

            $('#<%=chkAlloAnamnesis.ClientID %>').change();
            $('#<%=chkIsRegional.ClientID %>').change();

            $('#leftPageNavPanel ul li').first().click();
        });

        //#region Chief Complaint
        $('#lblChiefComplaint').die('click');
        $('#lblChiefComplaint').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function ontxtAnamnesisTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^01'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtAnamnesisText.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Chief Complaint Text from your template text ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtAnamnesisText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region Catatan Hasil Penunjang
        $('#btnAddTemplate2.imgLink').die('click');
        $('#btnAddTemplate2.imgLink').live('click', function (evt) {
            if ($('#<%=txtDiagnosticResultSummary.ClientID %>').val() != '') {
                onBeforeOpenTrxPopup();
                var text = $('#<%=txtDiagnosticResultSummary.ClientID %>').val();
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^10|" + text, "Physician Template Text", 700, 500);
            }           
        });

        $('#lblDiagnosticResultSummary').die('click');
        $('#lblDiagnosticResultSummary').live('click', function (evt) {
            var visitNoteID = 0;
            var param = "X058^10|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/SOAPTemplateLookupCtl1.ascx", param, "SOAP Template Text", 700, 500);
        });

        function ontxtDiagnosticResultSummaryTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^10'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtDiagnosticResultSummary.ClientID %>').val() != '') {
                            var message1 = "Ganti catatan di Catatan Hasil Penunjang dengan teks dari template ?";
                            var message2 = "<i>"+obj.TemplateText+"</i>";
                            displayConfirmationMessageBox('TEMPLATE TEXT', message1+"<br/><br/>"+message2, function (result) {
                                if (result) {
                                    $('#<%=txtDiagnosticResultSummary.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion



        function onAfterCustomClickSuccess(type, retval) {
        }


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

        function GetCurrentSelectedVitalSign(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdVitalSignView.ClientID %> tr').index($tr);
            $('#<%=grdVitalSignView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdVitalSignView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var testOrderID = $('#<%=hdnPatientChargesDtID.ClientID %>').val();
                var param = "0|0|0|0|0|0|0|0|" + assessmentID + "|0|0|";
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);    
            }
            else {
                displayMessageBox("Asesmen Pra Anestesi","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromVitalSignLookup').die('click');
        $('#lblAddFromVitalSignLookup').live('click', function (evt) {
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|0|0|0|" + assessmentID;
                 openUserControlPopup("~/libs/Controls/EMR/Lookup/VitalSignLookupCtl1.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);  
            }
            else {
                displayMessageBox("Status Anestesi","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });


        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedVitalSign(this);
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            var testOrderID = $('#<%=hdnPatientChargesDtID.ClientID %>').val();
            var param = "0|" +$('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|0|0|0|0|0|0|" + assessmentID + "|0";
            
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Hapus pengkajian tanda vital dan indikator lainnya untuk pasien ini ?";
            displayConfirmationMessageBox("Asesmen Pra Anestesi - Tanda Vital", message, function (result) {
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

        function onBeforeLoadRightPanelContent(code) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (code == 'healthyinformation' || code == 'medicalSickLeave' || code == 'medicalSickLeaveBilingual') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onCbpDeleteTestOrderEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == 'LB') {
                    cbpLaboratoryView.PerformCallback('refresh');
                }
                else if (param[1] == 'IS') {
                    cbpImagingView.PerformCallback('refresh');
                }
                else {
                    cbpDiagnosticView.PerformCallback('refresh');
                }
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                getPhysicalExamFormValues();
                getDiagnosticTestChecklistFormValues();
                getAnesthesyPlanningFormValues();
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                displayConfirmationMessageBox("Asesment",message, function (result) {
                    if (result) {
                        getPhysicalExamFormValues();
                        getDiagnosticTestChecklistFormValues();
                        getAnesthesyPlanningFormValues();
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
            displayConfirmationMessageBox("Assessment",message, function (result) {
                if (result) {
                    getPhysicalFormValues();
                    getDiagnosticTestChecklistFormValues();
                    getAnesthesyPlanningFormValues();
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion     
        
        //#region Get Form Values
        function getPhysicalExamFormValues() {
            var controlValues = '';
            $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });

            $('#<%=hdnPhysicalExamValue.ClientID %>').val(controlValues);

            return controlValues;
        }  

        function getDiagnosticTestChecklistFormValues() {
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

            $('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
                 
        function getAnesthesyPlanningFormValues() {
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
            $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });

            $('#<%=hdnAnesthesyPlanningValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
        //#endregion    

    function onGetLocalHiddenFieldValue(param) {
        $('#<%=hdnAssessmentID.ClientID %>').val(param);
    }

    function onBeforeOpenTrxPopup() {
        if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
            onCustomButtonClick('save');
            $('#<%=hdnIsChanged.ClientID %>').val('0');
        }
    }

    function onAfterLookUpSOAPTemplate(value) {
        var valueInfo = value.split('|');
        switch (valueInfo[1]) {
            case 'X058^10':
                $('#<%=txtDiagnosticResultSummary.ClientID %>').val(valueInfo[0]+';');
                ontxtDiagnosticResultSummaryTextChanged($('#<%=txtDiagnosticResultSummary.ClientID %>').val());
                break;
            default:
                displayErrorMessageBox("ERROR : TEMPLATE TEXT","Unhandled Template Text Type");
                break;
        }
    }   

     $('#<%=chkAlloAnamnesis.ClientID %>').die('change');
     $('#<%=chkAlloAnamnesis.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");
        
        if ($(this).is(':checked')) {
            cboFamilyRelation.SetEnabled(true);
        }
        else {
            cboFamilyRelation.SetEnabled(false);
            cboFamilyRelation.SetValue("");
        }
    });

     $('#<%=chkIsRegional.ClientID %>').die('change');
     $('#<%=chkIsRegional.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");
        
        if ($(this).is(':checked')) {
            cboRegionalAnesthesiaType.SetEnabled(true);
        }
        else {
            cboRegionalAnesthesiaType.SetEnabled(false);
            cboRegionalAnesthesiaType.SetValue("");
        }
    });

     $('#<%=rblIsUsePremedication.ClientID %>').die('change');
     $('#<%=rblIsUsePremedication.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        
        if (value=="1") {
            $('#<%=txtPremedication.ClientID %>').prop("disabled", false);
        }
        else {
            $('#<%=txtPremedication.ClientID %>').prop("disabled", true);
            $('#<%=txtPremedication.ClientID %>').val("");
        }
    });

    $('#<%=rblIsSedativePlan.ClientID %>').die('change');
    $('#<%=rblIsSedativePlan.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        
        if (value=="1") {
            $('#<%=txtSedativeMedication.ClientID %>').prop("disabled", false);
        }
        else {
            $('#<%=txtSedativeMedication.ClientID %>').prop("disabled", true);
            $('#<%=txtSedativeMedication.ClientID %>').val("");
        }
    });

    $('#<%=rblIsMaintenancePlan.ClientID %>').die('change');
    $('#<%=rblIsMaintenancePlan.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        
        if (value=="1") {
            $('#<%=txtMaintenanceMedication.ClientID %>').prop("disabled", false);
        }
        else {
            $('#<%=txtMaintenanceMedication.ClientID %>').prop("disabled", true);
            $('#<%=txtMaintenanceMedication.ClientID %>').val("");
        }
    });

    $('#<%=rblIsRegionalAnesthesy.ClientID %>').die('change');
    $('#<%=rblIsRegionalAnesthesy.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        
        if (value=="1") {
            $('#<%=txtRegionalAnesthesyMedication.ClientID %>').prop("disabled", false);
        }
        else {
            $('#<%=txtRegionalAnesthesyMedication.ClientID %>').prop("disabled", true);
            $('#<%=txtRegionalAnesthesyMedication.ClientID %>').val("");
        }
    });

    $('#<%=rblIsLocalAnesthesy.ClientID %>').die('change');
    $('#<%=rblIsLocalAnesthesy.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        
        if (value=="1") {
            $('#<%=txtLocalAnesthesyMedication.ClientID %>').prop("disabled", false);
        }
        else {
            $('#<%=txtLocalAnesthesyMedication.ClientID %>').prop("disabled", true);
            $('#<%=txtLocalAnesthesyMedication.ClientID %>').val("");
        }
    });
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
        <input type="hidden" runat="server" id="hdnMSTAssessmentID" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
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
        <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
        <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
        <input type="hidden" runat="server" id="hdnDiagnosticTestCheckListLayout" value="" />
        <input type="hidden" runat="server" id="hdnDiagnosticTestCheckListValue" value="" />
        <input type="hidden" runat="server" id="hdnAnesthesyPlanningLayout" value="" />
        <input type="hidden" runat="server" id="hdnAnesthesyPlanningValue" value="" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnIsPatientAllergyExists" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                <td style="vertical-align:top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentID="divPage1" title="Catatan Asesmen dan Riwayat Kesehatan" class="w3-hover-red">Rencana Tindakan dan Riwayat Kesehatan</li>
                            <li contentID="divPage3" title="Riwayat Alergi" class="w3-hover-red">Riwayat Alergi</li>
                            <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>                                               
                            <li contentID="divPage5" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>      
                            <li contentID="divPage6" title="Hasil Pemeriksaan Penunjang yang telah teridentifikasi secara benar" class="w3-hover-red">Hasil Pemeriksaan Penunjang</li>      
                            <li contentID="divPage7" title="Rencana Anestesi" class="w3-hover-red">Rencana Anestesi</li>
                        </ul>     
                    </div> 
                </td>
                    <td style="vertical-align:top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Waktu")%></label>
                                    </td>
                                    <td colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label id="lblItemName">
                                            <%:GetLabel("Tindakan")%>
                                        </label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtItemName" Width="100%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label id="lblOrderNo">
                                            <%:GetLabel("Nomor Transaksi")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPatientChargesDtID" value="" runat="server" />
                                        <asp:TextBox ID="txtTransactionNo" Width="225px" runat="server" Enabled="false" />
                                    </td>
                                    <td class="tdLabel" style="display:none">
                                        <label id="Label1">
                                            <%:GetLabel("Diagnosa Pre Op")%></label>
                                    </td>
                                    <td colspan="3" style="display:none">
                                        <asp:TextBox ID="txtPreOpDiagnosisInfo" Width="100%" runat="server" Enabled="false" />
                                    </td>
                                </tr>  
                                <tr style="display:none">
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Rencana Tindakan") %>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtProcedureGroupSummary" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" ReadOnly="true" />
                                    </td>
                                </tr>                                 
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Pembedahan dan Anestesi") %>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtPastSurgicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Penggunaan Obat") %>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtMedicationHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        &nbsp;
                                    </td>
                                    <td colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <td style="width: 50px">
                                                <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto" Checked="false" />
                                            </td>
                                            <td style="width: 50px">
                                                <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo"
                                                    Checked="false" />
                                            </td>
                                            <td class="tdLabel" style="width: 120px">
                                                <label class="lblNormal" id="lblFamilyRelation">
                                                    <%=GetLabel("Hubungan dengan Pasien")%></label>
                                            </td>
                                            <td style="width: 130px">
                                                <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                    Width="100%" />
                                            </td>
                                        </table>
                                    </td>
                                </tr>   
                                <tr>
                                    <td class="tdLabel">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkIsAsthma" runat="server" Text=" Memiliki Riwayat Asma" Checked="false" />
                                            </td>
                                        </table>
                                    </td>
                                </tr>  
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
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
                                                    <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
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
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
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
                                                                                <span style="font-style:italic">
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
                                                                        <br />
                                                                    <span class="lblLink" id="lblAddFromVitalSignLookup">
                                                                        <%= GetLabel("+ Salin Tanda Vital")%></span>
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
                        <div id="divPage5" class="divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="450px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td style="vertical-align:top" colspan="2">
                                        <div id="divFormContent1" runat="server" style="height: 480px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage6" class="divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="220px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="vertical-align:top">
                                        <div id="divFormContent2" runat="server" style="height: 480px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                                <tr style="display:none">
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:180px">
                                                    <label class="lblLink" id="lblDiagnosticResultSummary">
                                                        <%=GetLabel("Catatan Hasil Penunjang")%></label>
                                                </td>
                                                <td><img class="imgLink" id="btnAddTemplate2" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDiagnosticResultSummary" runat="server" TextMode="MultiLine" Rows="12" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage7" class="divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea" style="width:100%">
                                <colgroup>
                                    <col width="200px" />
                                    <col width="150px" />
                                    <col width="120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Mulai Puasa")%></label>
                                    </td>
                                    <td colspan="3">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                      <asp:TextBox ID="txtStartFastingDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtStartFastingTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><%=GetLabel("Status Fisik ASA") %></td>
                                    <td colspan="3">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                     <asp:RadioButtonList ID="rblGCASAStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text=" 1" Value="X455^1" />
                                                        <asp:ListItem Text=" 2" Value="X455^2" />
                                                        <asp:ListItem Text=" 3" Value="X455^3" />
                                                        <asp:ListItem Text=" 4" Value="X455^4" />
                                                        <asp:ListItem Text=" 5" Value="X455^5" />
                                                    </asp:RadioButtonList>                                               
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsASAStatusE" Checked="false" Text = " E" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>    
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Teknik Anestesi")%></label>
                                    </td>
                                    <td colspan="2">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsGeneralAnesthesy" runat="server" Text=" General/Umum" Checked="false" Width="120px" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsLocal" runat="server" Text=" Local" Checked="false" Width="80px" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsSedation" runat="server" Text=" Sedation" Checked="false" Width="80px" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsRegional" runat="server" Text=" Regional" Checked="false" Width="80px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td class="tdLabel" style="width: 120px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Regional Anestesi")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRegionalAnesthesiaType" ClientInstanceName="cboRegionalAnesthesiaType"
                                                        Width="150px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <label class="lblNormal" style="font-weight:bold">
                                            <%=GetLabel("Rencana obat yang digunakan :")%></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Premedikasi")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsUsePremedication" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Ya" Value="1" />
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                        </asp:RadioButtonList>                                               
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nama Obat")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPremedication" Width="99%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Sedatif")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsSedativePlan" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Ya" Value="1" />
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                        </asp:RadioButtonList>                                               
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nama Obat")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSedativeMedication" Width="99%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Maintenance")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsMaintenancePlan" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Ya" Value="1" />
                                            <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                        </asp:RadioButtonList>                                               
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nama Obat")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaintenanceMedication" Width="99%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Regional Anestesi")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsRegionalAnesthesy" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Ya" Value="1" />
                                            <asp:ListItem Text=" Tidak" Value="0" Selected = "True" />
                                        </asp:RadioButtonList>                                               
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nama Obat")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegionalAnesthesyMedication" Width="99%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Lokal Anestesi")%></label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsLocalAnesthesy" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Value="0">
                                            <asp:ListItem Text=" Ya" Value="1" />
                                            <asp:ListItem Text=" Tidak" Value="0" Selected = "True" />
                                        </asp:RadioButtonList>                                               
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nama Obat")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLocalAnesthesyMedication" Width="99%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="vertical-align:top">
                                        <div id="divFormContent3" runat="server" style="height: 220px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label class="lblNormal" id="lblAnesthesyRemarks">
                                                        <%=GetLabel("Catatan Anestesi")%></label>
                                                </td>
                                                <td style="display:none"><img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAnamnesisText" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpAllergy" runat="server" Width="100%" ClientInstanceName="cbpAllergy"
                ShowLoadingPanel="false" OnCallback="cbpAllergy_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
                ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteTestOrder" runat="server" Width="100%" ClientInstanceName="cbpDeleteTestOrder"
                ShowLoadingPanel="false" OnCallback="cbpDeleteTestOrder_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteTestOrderEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
