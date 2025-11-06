<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="ERInitialAssessment2.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ERInitialAssessment2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Simpan")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Batal Perubahan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .templatePatientBodyDiagram
        {
            padding: 10px;
        }
        .templatePatientBodyDiagram .containerImage
        {
            float: left;
            display: table-cell;
            vertical-align: middle;
            border: 1px solid #AAA;
            width: 300px;
            height: 300px;
            margin-right: 20px;
            text-align: center;
            position: relative;
        }
        .templatePatientBodyDiagram .containerImage img
        {
            max-height: 300px;
            max-width: 300px;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }
        .templatePatientBodyDiagram .spLabel
        {
            display: inline-block;
            width: 120px;
            font-weight: bolder;
        }
        .templatePatientBodyDiagram .spValue
        {
            margin-left: 10px;
        }
    </style>
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtChiefComplaint.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtServiceTime.ClientID %>').val())) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
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

            function getVisitNoteID() {
                var value = '<%=GetVisitNoteID() %>';
                return value;
            }

            $('#btnAddTemplate.imgLink').click(function () {
                if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtChiefComplaint.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^01|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#btnAddHPITemplate.imgLink').click(function () {
                if ($('#<%=txtHPISummary.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtHPISummary.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^02|" + text, "Physician Template Text", 700, 500);
                }
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Are you sure to discard your changes ?";
                    showToastConfirmation(message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload(true);
                        }
                    });
                }
            });

            $('#<%=btnWriteInstructionCPPT.ClientID %>').click(function (evt) {
                if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                    var message = "Salin kajian awal ke Catatan Terintegrasi ?";
                    displayConfirmationMessageBox("Salin ke CPPT",message, function (result) {
                        if (result) {
                            cbpIntegrationNote.PerformCallback();
                        }
                    });
                }
                else
                {
                    var message = "Catatan Anamnesa masih belum terisi, mohon dilengkapi terlebih dahulu";
                    displayMessageBox('INSTRUKSI DOKTER', message);
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

            $('#<%=grdProcedureView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdProcedureView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnProcedureID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

            $('#<%=grdLaboratoryView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdLaboratoryView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=grdImagingView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdImagingView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnImagingTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=grdDiagnosticView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdDiagnosticView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=grdInstructionView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdInstructionView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnInstructionID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdInstructionView.ClientID %> tr:eq(1)').click();

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

            $('#btnBodyDiagramContainerPrev').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('prev');
            });
            $('#btnBodyDiagramContainerNext').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('next');
            });

            $('#imgEditBodyDiagram').live('click', function () {
                onBeforeOpenTrxPopup();
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPEdit1Ctl.ascx", $('#<%=hdnBodyDiagramID.ClientID %>').val(), "Body Diagram", 1200, 600);
            });

            $('#imgDeleteBodyDiagram').live('click', function () {
                var message = "Are you sure to delete this body diagram ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        cbpDeleteBodyDiagram.PerformCallback();
                    }
                });
            });

            $('.btnApplyDiagnosis').click(function () {
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
            });

            function submitDiagnosis()
            {
                if ((cboDiagnosisType.GetValue() != '' && $('#<%=txtDiagnosisText.ClientID %>').val() != '')) {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1") {
                        cbpDiagnosis.PerformCallback('add');
                    }
                    else {
                        cbpDiagnosis.PerformCallback('edit');
                    }
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
                else {
                    displayErrorMessageBox("KAJIAN AWAL IGD", "You should fill Diagnosis Type and Name field !");
                }
            }

            $('.btnApplyProcedure').click(function () {
                submitProcedure();
                $('#<%=ledProcedure.ClientID %>').focus();
            });

            $('.btnCancelProcedure').click(function () {
                ResetProcedureEntryControls();
            });

            function submitProcedure()
            {
                onBeforeOpenTrxPopup();
                if ($('#<%=txtProcedureText.ClientID %>').val() != '') {
                    if ($('#<%=hdnProcedureProcessMode.ClientID %>').val() == "1")
                        cbpProcedure.PerformCallback('add');
                    else
                        cbpProcedure.PerformCallback('edit');
                }
                else {
                    displayErrorMessageBox("KAJIAN AWAL IGD", "You should fill Procedure Name field !");
                }
            }

            function submitInstruction()
            {
                if ($('#<%=txtChiefComplaint.ClientID %>').val() == '') {
                    var message = "Catatan Anamnesa masih belum terisi, mohon dilengkapi terlebih dahulu";
                    displayMessageBox('INSTRUKSI DOKTER', message);
                }
                else {                
                    onBeforeOpenTrxPopup();
                    if ((cboInstructionType.GetValue() != '' && $('#<%=txtInstructionText.ClientID %>').val() != '')) {
                        if ($('#<%=hdnInstructionProcessMode.ClientID %>').val() == "1") {
                            cbpInstruction.PerformCallback('add');
                        }
                        else {
                            cbpInstruction.PerformCallback('edit');
                        }
                        $('#<%=hdnIsChanged.ClientID %>').val('1');
                    }
                    else {
                        var messageBody = "Jenis dan keterangan Instruksi Dokter harus diisi.";
                        displayErrorMessageBox('INSTRUKSI DOKTER', messageBody);
                    }
               }
            }

            $('.btnApplyInstruction').click(function () {
                submitInstruction();
                $('#<%=txtInstructionText.ClientID %>').focus();
            });

            $('.btnCancelInstruction').click(function () {
                ResetInstructionEntryControls();
            });

            $('#<%=txtChiefComplaint.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtChiefComplaint.ClientID %>').blur(function () {
                onTxtChiefComplaintChanged($(this).val());
            });

            $('#<%=txtHPISummary.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtHPISummary.ClientID %>').blur(function () {
                onTxtHPIChanged($(this).val());
            });

            $('#<%=txtMedicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtMedicationHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanningNotes.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtInstructionText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitInstruction();
              }
            }); 

            $('#<%=txtReaction.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitAllergy();
              }
            }); 

            $('#<%=txtDiagnosisText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitDiagnosis();
              }
            }); 
            
            $('#<%=txtProcedureText.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitProcedure();
              }
            }); 

            $('#<%=rblIsFoodIntakeChanged.ClientID %> input').change(function () {
                $('#<%=txtFoodIntakeScore.ClientID %>').val($(this).val());
                calculateMSTScore();
            });

            registerCollapseExpandHandler();
        });

        //#region Chief Complaint
        $('#lblChiefComplaint').die('click');
        $('#lblChiefComplaint').live('click', function (evt) {
            var visitNoteID = 0;
            var param = "X058^01|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/SOAPTemplateLookupCtl1.ascx", param, "SOAP Template Text", 700, 500);
        });

        function onTxtChiefComplaintChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                            var message1 = "Ganti catatan di keluhan pasien (anamnesa) dengan teks dari template ?";
                            var message2 = "<i>"+obj.TemplateText+"</i>";
                            displayConfirmationMessageBox('TEMPLATE TEXT', message1+"<br/><br/>"+message2, function (result) {
                                if (result) {
                                    $('#<%=txtChiefComplaint.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }

        function onAfterLookUpSOAPTemplate(value) {
            var valueInfo = value.split('|');
            $('#<%=txtChiefComplaint.ClientID %>').val(valueInfo[0]+';');
            onTxtChiefComplaintChanged($('#<%=txtChiefComplaint.ClientID %>').val());
        }    
        //#endregion

        //#region History of Present Illness
        $('#lblHPI').die('click');
        $('#lblHPI').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function onTxtHPIChanged(value) {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^02'";
            Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                if (obj != null)
                    if ($('#<%=txtHPISummary.ClientID %>').val() != '') {
                        var message = "Ganti catatan di Riwayat Penyakit sekarang dengan teks dari template ? ?";
                        showToastConfirmation(message, function (result) {
                            if (result) {
                                $('#<%=txtHPISummary.ClientID %>').val(obj.TemplateText);
                            }
                        });
                    }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        function onCboReferralValueChanged(s) {
            $('#<%:hdnReferrerID.ClientID %>').val('');
            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
            }
        }

        //#region Referral Description
        function getReferralDescriptionFilterExpression() {
            var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        function getReferralParamedicFilterExpression() {
            var filterExpression = "GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'";
            return filterExpression;
        }

        $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
            var referral = cboReferral.GetValue();
            if (referral == Constant.ReferrerGroup.DOKTERRS) {
                openSearchDialog('referrerparamedic', getReferralParamedicFilterExpression(), function (value) {
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                    onTxtReferralDescriptionCodeChanged(value);
                });
            } else {
                openSearchDialog('referrer', getReferralDescriptionFilterExpression(), function (value) {
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                    onTxtReferralDescriptionCodeChanged(value);
                });
            }
        });

        $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
            onTxtReferralDescriptionCodeChanged($(this).val());
        });

        function onTxtReferralDescriptionCodeChanged(value) {
            var filterExpression = "";
            var referral = cboReferral.GetValue();
            if (referral == Constant.ReferrerGroup.DOKTERRS) {
                filterExpression = getReferralParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    }
                });
            } else {
                filterExpression = getReferralDescriptionFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%:hdnReferrerID.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    }
                });
            }
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

                if (param[2]=="1") {
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').attr("disabled", "disabled");
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').prop( "checked", false );
                }
                else
                {
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').removeAttr("disabled");
                    $('#<%=chkIsPatientAllergyExists.ClientID %>').prop( "checked", true );
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
        var pageCountVitalSign = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|", "Vital Sign & Indicator", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            $row = $(this).closest('tr').parent().closest('tr');
            var obj = rowToObject($row);
            $('#<%=hdnVitalSignRecordID.ClientID %>').val(obj.ID);
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|" + $('#<%=hdnVitalSignRecordID.ClientID %>').val(), "Vital Sign & Indicator", 700, 500);
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
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                showToast("ERROR", 'Error Message : ' + param[1]);
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
            onBeforeOpenTrxPopup();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", "", "Pemeriksaan Fisik", 700, 500);
        });

        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedROS(this);
            $('#<%=hdnReviewOfSystemID.ClientID %>').val(selectedObj.ID);
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", $('#<%=hdnReviewOfSystemID.ClientID %>').val(), "Pemeriksaan Fisik", 700, 500);
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
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        function onRefreshROSGrid() {
            cbpROSView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Body Diagram
        $('#lblAddBodyDiagram').die('click');
        $('#lblAddBodyDiagram').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPAdd1Ctl.ascx", "", "Body Diagram", 1200, 600);
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
        //#endregion

        //#region Diagnosis
        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
            if ($('#<%=txtDiagnosisText.ClientID %>').val() == "") 
                $('#<%=txtDiagnosisText.ClientID %>').val($('#<%=hdnEntryDiagnoseText.ClientID %>').val());    
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

            cboDiagnosisType.SetValue(selectedObj.GCDiagnoseType);
            cboDiagnosisStatus.SetValue(selectedObj.GCDifferentialStatus);
            ledDiagnose.SetValue(selectedObj.DiagnoseID);
            $('#<%=txtDiagnosisText.ClientID %>').val(selectedObj.DiagnosisText);
        }

        function ResetDiagnosisEntryControls(s) {
            if ($('#<%=hdnIsMainDiagnosisExists.ClientID %>').val() == "0")
                cboDiagnosisType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
            else
                cboDiagnosisType.SetValue(Constant.DiagnosisType.COMPLICATION);

            cboDiagnosisStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
            ledDiagnose.SetValue('');
            $('#<%=txtDiagnosisText.ClientID %>').val('');
        }

        $('.imgEditDiagnosis.imgLink').die('click');
        $('.imgEditDiagnosis.imgLink').live('click', function () {
            SetDiagnosisEntityToControl(this);
            $('#<%=hdnDiagnosisProcessMode.ClientID %>').val('0');
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
            var diagnosis = s.cpDiagnosis;
            var isMainDiagnosisExists = s.cpRetval;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
                    
               
                    $('#<%=hdnDiagnosisSummary.ClientID %>').val(param[2]);
               

                setPaging($("#diagnosisPaging"), pageCount, function (page) {
                    cbpDiagnosisView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            if (isMainDiagnosisExists == "0")
                cboDiagnosisType.SetValue(Constant.DiagnosisType.MAIN_DIAGNOSIS);
            else
                cboDiagnosisType.SetValue(Constant.DiagnosisType.COMPLICATION);
            cboDiagnosisStatus.SetValue(Constant.DiagnosisStatus.UNDER_INVESTIGATION);
            ledDiagnose.SetValue('');
            $('#<%=txtDiagnosisText.ClientID %>').val('');

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

        //#region Procedure
        function onLedProcedureLostFocus(led) {
            var ProcedureID = led.GetValueText();
            $('#<%=hdnEntryProcedureID.ClientID %>').val(ProcedureID);
            $('#<%=hdnEntryProcedureText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtProcedureText.ClientID %>').val($('#<%=hdnEntryProcedureText.ClientID %>').val());
        }

        function GetCurrentSelectedProcedure(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdProcedureView.ClientID %> tr').index($tr);
            $('#<%=grdProcedureView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdProcedureView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetProcedureEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedProcedure(param);
            ledProcedure.SetValue(selectedObj.ProcedureID);
            $('#<%=txtProcedureText.ClientID %>').val(selectedObj.ProcedureText);
        }

        function ResetProcedureEntryControls(s) {
            ledProcedure.SetValue('');
            $('#<%=txtProcedureText.ClientID %>').val('');
        }

        $('.imgEditProcedure.imgLink').die('click');
        $('.imgEditProcedure.imgLink').live('click', function () {
            SetProcedureEntityToControl(this);
            $('#<%=hdnProcedureProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteProcedure.imgLink').die('click');
        $('.imgDeleteProcedure.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedProcedure(this);

            var message = "Are you sure to delete this Procedure <b>'" + selectedObj.ProcedureText + "'</b> for this patient ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpProcedure.PerformCallback('delete');
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
            });
        });

        var pageCount = parseInt('<%=gridProcedurePageCount %>');
        $(function () {
            setPaging($("#ProcedurePaging"), pageCount, function (page) {
                cbpProcedureView.PerformCallback('changepage|' + page);
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
        });

        function onCbpProcedureViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var isMainProcedureExists = s.cpRetval;
            var summaryText = s.cpSummary;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

                $('#<%=hdnProcedureSummary.ClientID %>').val(summaryText);

                setPaging($("#ProcedurePaging"), pageCount, function (page) {
                    cbpProcedureView.PerformCallback('changepage|' + page);
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                });
            }
            else
                $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnProcedureSummary.ClientID %>').val(summaryText);
        }

        function onCbpProcedureEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnProcedureProcessMode.ClientID %>').val('1');

                ResetProcedureEntryControls();
                cbpProcedureView.PerformCallback('refresh');
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else if (param[0] == '0') {
                showToast("ERROR", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Laboratory
        function GetCurrentSelectedLaboratory(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdLaboratoryView.ClientID %> tr').index($tr);
            $('#<%=grdLaboratoryView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdLaboratoryView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddLabOrder').die('click');
        $('#lblAddLabOrder').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Laboratory Test Order", 1200, 600);
        });

        $('#lblAddLabOrder2').die('click');
        $('#lblAddLabOrder2').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var width = 1150;
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var param = "X001^004|0|" + clinicalNotes + "|" + chiefComplaint;
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx', param, title, width, 600);
        });

        $('.imgAddLabOrderDt.imgLink').die('click');
        $('.imgAddLabOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Laboratory Test Order", 1200, 600);
        });

        $('.imgEditLabOrder.imgLink').die('click');
        $('.imgEditLabOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "LB|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Laboratory Order", 700, 500);
        });

        $('.imgDeleteLabOrder.imgLink').die('click');
        $('.imgDeleteLabOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Laboratory Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteTestOrder.PerformCallback('LB');
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
            });
        });

        $('.imgSendLabOrder.imgLink').die('click');
        $('.imgSendLabOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnLaboratoryTestOrderID.ClientID %>').val() == "") {
                showToast("ERROR", "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                showToastConfirmation(message, function (result) {
                    if (result) cbpSendOrder.PerformCallback('sendOrder|LB|' + $('#<%:hdnLaboratoryTestOrderID.ClientID %>').val());
                });
            }
        });

        var pageCount = parseInt('<%=gridLaboratoryPageCount %>');
        $(function () {
            setPaging($("#laboratoryPaging"), pageCount, function (page) {
                cbpLaboratoryView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpLaboratoryViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();
                    
               
                    $('#<%=hdnLaboratorySummary.ClientID %>').val(param[2]);
               

                setPaging($("#laboratoryPaging"), pageCount, function (page) {
                    cbpLaboratoryView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnLaboratorySummary.ClientID %>').val(summaryText);     
        }

        function onRefreshLaboratoryGrid() {
            cbpLaboratoryView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Imaging
        function GetCurrentSelectedImaging(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdImagingView.ClientID %> tr').index($tr);
            $('#<%=grdImagingView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdImagingView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddImagingOrder').die('click');
        $('#lblAddImagingOrder').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('#lblAddImagingOrder2').die('click');
        $('#lblAddImagingOrder2').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var width = 400;
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var healthcareServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var param = "X001^005|0|" + healthcareServiceUnitID + "|" + clinicalNotes + "|" + chiefComplaint;
            var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderHeaderCtl1.ascx', param, title, width, 500);
        });

        $('#lblAddImagingOrder3').die('click');
        $('#lblAddImagingOrder3').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var width = 1150;
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var imagingServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var param = "X001^005|0|" + clinicalNotes + "|" + chiefComplaint + "|" + imagingServiceUnitID;
            var title = "Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx', param, title, width, 600);
        });

        $('.imgAddImagingOrderDt.imgLink').die('click');
        $('.imgAddImagingOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('.imgEditImagingOrder.imgLink').die('click');
        $('.imgEditImagingOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "IS|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Imaging Order", 700, 500);
        });

        $('.imgDeleteImagingOrder.imgLink').die('click');
        $('.imgDeleteImagingOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Imaging Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteTestOrder.PerformCallback('IS');
                }
            });
        });

        $('.imgSendImagingOrder.imgLink').die('click');
        $('.imgSendImagingOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnImagingTestOrderID.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                showToastConfirmation(message, function (result) {
                    if (result) cbpSendOrder.PerformCallback('sendOrder|IS|' + $('#<%:hdnImagingTestOrderID.ClientID %>').val());
                });
            }
        });

        var pageCount = parseInt('<%=gridImagingPageCount %>');
        $(function () {
            setPaging($("#imagingPaging"), pageCount, function (page) {
                cbpImagingView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpImagingViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();
                    
               
                    $('#<%=hdnImagingSummary.ClientID %>').val(param[2]);
               

                setPaging($("#imagingPaging"), pageCount, function (page) {
                    cbpImagingView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnImagingSummary.ClientID %>').val(summaryText);     
        }

        function onRefreshImagingGrid() {
            cbpImagingView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Diagnostic
        function GetCurrentSelectedDiagnostic(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdDiagnosticView.ClientID %> tr').index($tr);
            $('#<%=grdDiagnosticView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdDiagnosticView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        $('#lblAddDiagnosticOrder').die('click');
        $('#lblAddDiagnosticOrder').live('click', function (evt) {
            onBeforeOpenTrxPopup();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = "0";
            var param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx", param, "Diagnostic Order", 1200, 600);
        });

        $('.imgAddDiagnosticOrderDt.imgLink').die('click');
        $('.imgAddDiagnosticOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = $('#<%=txtChiefComplaint.ClientID %>').val();
            var testOrderID = selectedObj.TestOrderID;

            var param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx", param, "Diagnostic Test Order", 1200, 600);
        });

        $('.imgEditDiagnosticOrder.imgLink').die('click');
        $('.imgEditDiagnosticOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "MD|" + $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Diagnostic Order", 700, 500);
        });

        $('.imgDeleteDiagnosticOrder.imgLink').die('click');
        $('.imgDeleteDiagnosticOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Diagnostic Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteTestOrder.PerformCallback('MD');
                }
            });
        });

        $('.imgSendDiagnosticOrder.imgLink').die('click');
        $('.imgSendDiagnosticOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedDiagnostic(this);
            $('#<%=hdnTestOrderHealthcareServiceUnitID.ClientID %>').val(selectedObj.HealthcareServiceUnitID);
            $('#<%=hdnDiagnosticTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            if ($('#<%:hdnDiagnosticTestOrderID.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        cbpSendOrder.PerformCallback('sendOrder|MD|' + $('#<%:hdnDiagnosticTestOrderID.ClientID %>').val());
                    }
                });
            }
        });

        var pageCount = parseInt('<%=gridDiagnosticPageCount %>');
        $(function () {
            setPaging($("#diagnosticPaging"), pageCount, function (page) {
                cbpDiagnosticView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpDiagnosticViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();
                    
               
                    $('#<%=hdnDiagnosticSummary.ClientID %>').val(param[2]);
               

                setPaging($("#diagnosticPaging"), pageCount, function (page) {
                    cbpDiagnosticView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnDiagnosticSummary.ClientID %>').val(summaryText);     
        }

        function onRefreshDiagnosticGrid() {
            cbpDiagnosticView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Physician Instruction
        function GetCurrentSelectedInstruction(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdInstructionView.ClientID %> tr').index($tr);
            $('#<%=grdInstructionView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdInstructionView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function SetInstructionEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedInstruction(param);

            cboInstructionType.SetValue(selectedObj.GCInstructionGroup);
            $('#<%=txtInstructionText.ClientID %>').val(selectedObj.Description);
        }

        function ResetInstructionEntryControls(s) {
            //cboInstructionType.SetValue('');
            $('#<%=txtInstructionText.ClientID %>').val('');
        }

        $('.imgEditInstruction.imgLink').die('click');
        $('.imgEditInstruction.imgLink').live('click', function () {
            SetInstructionEntityToControl(this);
            $('#<%=hdnInstructionProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteInstruction.imgLink').die('click');
        $('.imgDeleteInstruction.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedInstruction(this);

            var message = "Hapus instruksi Dokter <b>'" + selectedObj.Description + "'</b> ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onBeforeOpenTrxPopup();
                    cbpInstruction.PerformCallback('delete');
                }
            });
        });

        var pageCount = parseInt('<%=gridInstructionPageCount %>');
        $(function () {
            setPaging($("#instructionPaging"), pageCount, function (page) {
                cbpInstructionView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpInstructionViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var summaryText = s.cpSummary;
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdInstructionView.ClientID %> tr:eq(1)').click();
                    
               
                    $('#<%=hdnInstructionText.ClientID %>').val(param[2]);
               

                setPaging($("#diagnosticPaging"), pageCount, function (page) {
                    cbpInstructionView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdInstructionView.ClientID %> tr:eq(1)').click();

            $('#<%=hdnInstructionText.ClientID %>').val(summaryText);     
        }

        function onCbpInstructionEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                    $('#<%=hdnInstructionProcessMode.ClientID %>').val('1');

                ResetInstructionEntryControls();
                cbpInstructionView.PerformCallback('refresh');
            }
            else if (param[0] == '0') {
                showToast("ERROR", 'Error Message : ' + param[2]);
            }
            else
                $('#<%=grdInstructionView.ClientID %> tr:eq(1)').click();
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

       function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (visitID == '') {
                errMessage.text = 'Please Select Patient Visit First!';
                return false;
            }
            else {
                filterExpression.text = visitID;
                return true;
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

        function onCbpSendOrderEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'sendOrder') {
                if (param[2] == 'success') {
                    showToast('Send Success', 'The test order was successfully sent to Service Unit.');

                    if (param[1] == 'LB') {
                        onRefreshLaboratoryGrid();
                    }
                    else if (param[1] == 'IS') {
                        onRefreshImagingGrid();
                    }
                    else if (param[1] == 'MD') {
                        onRefreshDiagnosticGrid();
                    }
                }
                else {
                    showToast('SEND ORDER : FAILED', 'Error Message : ' + param[3]);
                }
            }
        }

        function onCbpIntegrationNoteEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'integrationNote') {
                if (param[2] != 'success') {
                    var messageBody = param[3];
                    displayErrorMessageBox('Salin ke Catatan Terintegrasi', messageBody);
                }
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
        
        function onGCWeightChangedGroup(s) {
            if (cboGCWeightChangedGroup.GetValue() != null) 
            {
                var param1 = cboGCWeightChangedGroup.GetValue().split('^');
                $('#<%=txtWeightChangedGroupScore.ClientID %>').val(parseInt(param1[1]));
            }
            else
            {
                $('#<%=txtWeightChangedGroupScore.ClientID %>').val('');
            }
            calculateMSTScore();
        }

        function onGCWeightChangedStatus(s) {
            if (cboGCWeightChangedStatus.GetValue() != null) {
                if (cboGCWeightChangedStatus.GetValue().indexOf('^01') > -1) {
                    $('#<%=txtWeightChangedStatusScore.ClientID %>').val('0');
                 }
                else if (cboGCWeightChangedStatus.GetValue().indexOf('^02') > -1) {
                     $('#<%=txtWeightChangedStatusScore.ClientID %>').val('2');
                }
                 else {
                     $('#<%=txtWeightChangedStatusScore.ClientID %>').val('');
                }
            }
            calculateMSTScore();
        }

        function onGCMSTDiagnosisChanged(s) {
            $txt = $('#<%=txtOtherMSTDiagnosis.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^99') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function calculateMSTScore()
        {
            var p1 = 0;
            var p2 = 0;
            var p3 = 0;

            if ($('#<%=txtWeightChangedGroupScore.ClientID %>').val())
                p1 = parseInt($('#<%=txtWeightChangedGroupScore.ClientID %>').val());

            if ($('#<%=txtWeightChangedStatusScore.ClientID %>').val())
                p2 = parseInt($('#<%=txtWeightChangedStatusScore.ClientID %>').val());

            if ($('#<%=txtFoodIntakeScore.ClientID %>').val())
                p3 = parseInt($('#<%=txtFoodIntakeScore.ClientID %>').val());

            var total = p1 + p2 + p3;
            $('#<%=txtTotalMST.ClientID %>').val(total);
        }     
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnVisitID" value="" />
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnMSTAssessmentID" value="" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnProcedureID" runat="server" />
        <input type="hidden" value="1" id="hdnProcedureProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnTestOrderHealthcareServiceUnitID" runat="server" />
        <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
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
        <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" value="" id="hdnProcedureSummary" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
        <input type="hidden" value="" id="hdnTimeToday" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentDateIsUsingRegDate" runat="server" /> 
        <input type="hidden" id="hdnDefaultInstructionType" runat="server" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
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
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                    MaxLength="5" />
                                                <asp:TextBox ID="txtRecordID" Width="80px" ReadOnly="true" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jenis Kunjungan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboVisitType" ClientInstanceName="cboVisitType"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width: 120px">
                                                <label class="lblMandatory lblLink" id="lblChiefComplaint">
                                                    <%=GetLabel("Anamnesa")%></label>
                                            </td>
                                            <td>
                                                <img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="15"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel" style="width: 150px; vertical-align: top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width: 120px">
                                                <label class="lblNormal lblLink" id="lblHPI">
                                                    <%=GetLabel("Riwayat Penyakit Sekarang")%></label>
                                            </td>
                                            <td>
                                                <img class="imgLink" id="btnAddHPITemplate" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHPISummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="10" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <td style="width: 40%">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Autoanamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 40%">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Alloanamnesis / Heteroanamnesis"
                                                Checked="false" />
                                        </td>
                                        <td style="width: 40%">
                                            <asp:CheckBox ID="chkIsFastTrack" runat="server" Text=" Fast Track" />
                                        </td>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblMandatory">
                                        <%=GetLabel("Triage") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <h4 class="h4expanded">
                            <%=GetLabel("Pengkajian Perawat")%></h4>
                        <div class="containerTblEntryContent">
                            <table id="tblNurseAnamnesis" style="width: 100%" runat="server">
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Anamnesa Perawat")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNurseAnamnesis" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Cara Datang")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox runat="server" ID="cboAdmissionRoute" ClientInstanceName="cboAdmissionRoute"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Keadaan Datang")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                            Width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Airway")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox runat="server" ID="cboAirway" ClientInstanceName="cboAirway" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Breathing")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox runat="server" ID="cboBreathing" ClientInstanceName="cboBreathing"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Circulation")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox runat="server" ID="cboCirculation" ClientInstanceName="cboCirculation"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Disability")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox runat="server" ID="cboDisability" ClientInstanceName="cboDisability"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Exposure")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox runat="server" ID="cboExposure" ClientInstanceName="cboExposure"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%:GetLabel("Rujukan")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" runat="server" id="lblReferralDescription">
                                            <%:GetLabel("Deskripsi Rujukan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                        <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 80px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" ReadOnly="true" />
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
                            </table>
                        </div>
                        <h4 class="h4collapsed">
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
                                                                            <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
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
                            <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                        <div class="containerTblEntryContent">
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
                        <h4 class="h4collapsed">
                            <%=GetLabel("Riwayat Alergi")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <div style="position: relative;">
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
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Fisik")%></h4>
                        <div class="containerTblEntryContent">
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
                        <h4 class="h4collapsed">
                            <%=GetLabel("MST")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="210px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                        <%=GetLabel("Apakah Pasien mengalami penurunan berat badan dalam waktu 6 bulan terakhir ?") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboGCWeightChangedStatus" ClientInstanceName="cboGCWeightChangedStatus"
                                            Width="210px">
                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCWeightChangedStatus(s); }"
                                                Init="function(s,e){ onGCWeightChangedStatus(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-left: 5px;">
                                                    <%=GetLabel("Skor") %>
                                                </td>
                                                <td style="padding-left: 5px; width: 60px">
                                                    <asp:TextBox ID="txtWeightChangedStatusScore" runat="server" Width="100%" ReadOnly="true"
                                                        CssClass="number" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                        <%=GetLabel("Jika Ya, berapa penurunan berat badan tersebut ?") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboGCWeightChangedGroup" ClientInstanceName="cboGCWeightChangedGroup"
                                            Width="210px">
                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCWeightChangedGroup(s); }"
                                                Init="function(s,e){ onGCWeightChangedGroup(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-left: 5px;">
                                                    <%=GetLabel("Skor") %>
                                                </td>
                                                <td style="padding-left: 5px; width: 60px">
                                                    <asp:TextBox ID="txtWeightChangedGroupScore" runat="server" Width="100%" ReadOnly="true"
                                                        CssClass="number" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                        <%=GetLabel("Apakah Asupan makanan berkurang karena tidak nafsu makan ?") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsFoodIntakeChanged" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Tidak" Value="0" />
                                            <asp:ListItem Text="Ya" Value="1" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-left: 5px;">
                                                    <%=GetLabel("Skor") %>
                                                </td>
                                                <td style="padding-left: 5px; width: 60px">
                                                    <asp:TextBox ID="txtFoodIntakeScore" runat="server" Width="100%" ReadOnly="true"
                                                        CssClass="number" />
                                                </td>
                                                <td style="width: 60px">
                                                </td>
                                                <td style="padding-left: 5px; width: 60px">
                                                    <asp:TextBox ID="txtTotalMST" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Pasien dengan Diagnosa Khusus ?") %>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsHasSpecificDiagnosis" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Ya" Value="1" />
                                            <asp:ListItem Text="Tidak" Value="0" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboGCMSTDiagnosis" ClientInstanceName="cboGCMSTDiagnosis"
                                            Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCMSTDiagnosisChanged(s); }"
                                                Init="function(s,e){ onGCMSTDiagnosisChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOtherMSTDiagnosis" runat="server" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                        <%=GetLabel("Apakah Sudah dibaca dan diketahui oleh tenaga gizi?")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsReadedByNutritionist" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Ya" Value="1" />
                                            <asp:ListItem Text="Tidak" Value="0" />
                                        </asp:RadioButtonList>
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
                            <%=GetLabel("Diagnosa")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <colgroup>
                                                <col width="160px" />
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Medical Problem")%></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtMedicalProblem" runat="server" Width="98%" />
                                                </td>
                                            </tr>
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
                                                        Width="99%" ValueText="DiagnoseID" FilterExpression="IsDeleted = 0 AND IsNutritionDiagnosis = 0"
                                                        DisplayText="DiagnoseName" MethodName="GetDiagnosisList">
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
                                                                            <%=GetLabel("Diagnose Information")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("DifferentialDateInString")%>,
                                                                                <%#: Eval("DifferentialTime")%>,
                                                                                <%#: Eval("GCDifferentialStatus")%>,
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
                        <h4 class="h4collapsed">
                            <%=GetLabel("Prosedur/Tindakan")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel2">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Prosedur/Tindakan")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryProcedureID" runat="server" />
                                                    <input type="hidden" value="" id="hdnEntryProcedureText" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledProcedure" ClientInstanceName="ledProcedure" runat="server"
                                                        Width="99%" ValueText="ProcedureID" FilterExpression="" DisplayText="ProcedureName"
                                                        MethodName="GetProceduresList">
                                                        <ClientSideEvents ValueChanged="function(s){ onLedProcedureLostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Procedure Name (Code)" FieldName="ProcedureName"
                                                                Description="i.e. Isolation" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Prosedur/Tindakan Text")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProcedureText" runat="server" Width="370px" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <img class="btnApplyProcedure imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td>
                                                                <img class="btnCancelProcedure imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpProcedureView" runat="server" Width="100%" ClientInstanceName="cbpProcedureView"
                                                ShowLoadingPanel="false" OnCallback="cbpProcedureView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent11" runat="server">
                                                        <asp:Panel runat="server" ID="Panel11" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdProcedureView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditProcedure imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteProcedure imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Procedure Information")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("ProcedureDateInString")%>,
                                                                                <%#: Eval("ProcedureTime")%>,
                                                                                <%#: Eval("ParamedicName")%></div>
                                                                            <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <span style="color: Blue; font-size: 1.1em">
                                                                                    <%#: Eval("ProcedureText")%></span> (<b><%#: Eval("ProcedureID")%></b>)
                                                                            </div>
                                                                            <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("Remarks")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                                            <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada informasi Prosedur/Tindakan untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="procedurePaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Catatan Tindakan/Terapi")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label2">
                                            <%=GetLabel("Catatan Tindakan/Terapi") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningNotes" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Catatan : Hasil Pemeriksaan Penunjang")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label3">
                                            <%=GetLabel("Catatan Hasil Pemeriksaan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiagnosticResultSummary" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Order Pemeriksaan : Laboratorium")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpLaboratoryView" runat="server" Width="100%" ClientInstanceName="cbpLaboratoryView"
                                                ShowLoadingPanel="false" OnCallback="cbpLaboratoryView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpLaboratoryViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                                        <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdLaboratoryView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdLaboratoryView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px" HeaderStyle-Width="100px">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: center; width: 100px">
                                                                                <span class="lblLink" id="lblAddLabOrder2">
                                                                                    <%= GetLabel("+ Order (Form)")%></span>
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddLabOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditLabOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteLabOrder imgLink" title='<%=GetLabel("Delete Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendLabOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <colgroup>
                                                                                    <col style="width: 70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%=GetLabel("Pemeriksaan") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: right">
                                                                                            <span class="lblLink" id="lblAddLabOrder">
                                                                                                <%= GetLabel("+ Tambah Order")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                            <%#: Eval("ServiceUnitName")%>,
                                                                                            <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                                <%#: Eval("TestOrderNo")%></span></div>
                                                                                        <div style="font-style: italic">
                                                                                            <asp:Repeater ID="rptLaboratoryDt" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <br style="clear: both" />
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data order pemeriksaan laboratorium untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="laboratoryPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Order Pemeriksaan : Radiologi")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpImagingView" runat="server" Width="100%" ClientInstanceName="cbpImagingView"
                                                ShowLoadingPanel="false" OnCallback="cbpImagingView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpImagingViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent8" runat="server">
                                                        <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdImagingView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdImagingView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: center">
                                                                                <span class="lblLink" id="lblAddImagingOrder2">
                                                                                    <%= GetLabel("+ Order (Tanpa Detail)")%></span>
                                                                                <br>
                                                                                <span class="lblLink" id="lblAddImagingOrder3">
                                                                                    <%= GetLabel("+ Order (Form)")%></span>
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddImagingOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditImagingOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteImagingOrder imgLink" title='<%=GetLabel("Delete Order")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendImagingOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <colgroup>
                                                                                    <col style="width: 70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%=GetLabel("Pemeriksaan") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: right">
                                                                                            <span class="lblLink" id="lblAddImagingOrder">
                                                                                                <%= GetLabel("+ Tambah Order")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                            <%#: Eval("ServiceUnitName")%>,
                                                                                            <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                                <%#: Eval("TestOrderNo")%></span></div>
                                                                                        <div style="font-style: italic">
                                                                                            <asp:Repeater ID="rptImagingDt" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <br style="clear: both" />
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data order pemeriksaan radiologi untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="imagingPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Order Pemeriksaan : Penunjang Medis Lain-lain")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpDiagnosticView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosticView"
                                                ShowLoadingPanel="false" OnCallback="cbpDiagnosticView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosticViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent9" runat="server">
                                                        <asp:Panel runat="server" ID="Panel8" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdDiagnosticView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdDiagnosticView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddDiagnosticOrderDt imgLink" title='<%=GetLabel("Add Detail")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>' alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditDiagnosticOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteDiagnosticOrder imgLink" title='<%=GetLabel("Delete Order")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendDiagnosticOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                <colgroup>
                                                                                    <col style="width: 70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%=GetLabel("Pemeriksaan") %></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="text-align: right">
                                                                                            <span class="lblLink" id="lblAddDiagnosticOrder">
                                                                                                <%= GetLabel("+ Tambah Order")%></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("TestOrderDateTimeInString")%>,
                                                                                            <%#: Eval("ServiceUnitName")%>,
                                                                                            <%#: Eval("ParamedicName") %>, <span style="font-style: italic; font-weight: bold">
                                                                                                <%#: Eval("TestOrderNo")%></span>
                                                                                        </div>
                                                                                        <div style="font-style: italic">
                                                                                            <asp:Repeater ID="rptDiagnosticDt" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <br style="clear: both" />
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data order pemeriksaan penunjang untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="diagnosticPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed" style="display: none">
                            <%=GetLabel("Planning : Resep Farmasi")%></h4>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Instruksi Dokter")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <div style="position: relative;">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0">
                                                <colgroup>
                                                    <col width="100px" />
                                                    <col width="165px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Jenis Instruksi")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ID="cboInstructionType" ClientInstanceName="cboInstructionType"
                                                            Width="165px">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Instruksi")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInstructionText" runat="server" Width="380px" />
                                                    </td>
                                                    <td style="padding-left: 5px" colspan="2">
                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <img class="btnApplyInstruction imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                        alt="" />
                                                                </td>
                                                                <td>
                                                                    <img class="btnCancelInstruction imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trWriteInstructionCPPT" runat="server">
                                                    <td colspan="2">
                                                        <input type="button" id="btnWriteInstructionCPPT" runat="server" class="btnWriteInstructionCPPT w3-btn w3-hover-blue"
                                                            value="Salin ke CPPT" style='margin-top: 5px; background-color: Red; color: White;' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpInstructionView" runat="server" Width="100%" ClientInstanceName="cbpInstructionView"
                                                ShowLoadingPanel="false" OnCallback="cbpInstructionView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpInstructionViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent10" runat="server">
                                                        <asp:Panel runat="server" ID="Panel9" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdInstructionView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditInstruction imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteInstruction imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="PatientInstructionID" HeaderStyle-CssClass="keyField"
                                                                        ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("PatientInstructionID") %>" bindingfield="PatientInstructionID" />
                                                                            <input type="hidden" value="<%#:Eval("cfInstructionDate") %>" bindingfield="cfInstructionDate" />
                                                                            <input type="hidden" value="<%#:Eval("cfInstructionDatePickerFormat") %>" bindingfield="cfInstructionDatePickerFormat" />
                                                                            <input type="hidden" value="<%#:Eval("InstructionTime") %>" bindingfield="InstructionTime" />
                                                                            <input type="hidden" value="<%#:Eval("PhysicianID") %>" bindingfield="PhysicianID" />
                                                                            <input type="hidden" value="<%#:Eval("PhysicianName") %>" bindingfield="PhysicianName" />
                                                                            <input type="hidden" value="<%#:Eval("GCInstructionGroup") %>" bindingfield="GCInstructionGroup" />
                                                                            <input type="hidden" value="<%#:Eval("Description") %>" bindingfield="Description" />
                                                                            <input type="hidden" value="<%#:Eval("AdditionalText") %>" bindingfield="AdditionalText" />
                                                                            <input type="hidden" value="<%#:Eval("ExecutedDateTime") %>" bindingfield="ExecutedDateTime" />
                                                                            <input type="hidden" value="<%#:Eval("ExecutedBy") %>" bindingfield="ExecutedBy" />
                                                                            <input type="hidden" value="<%#:Eval("ExecutedByName") %>" bindingfield="ExecutedByName" />
                                                                            <input type="hidden" value="<%#:Eval("IsCompleted") %>" bindingfield="IsCompleted" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="cfInstructionDate" HeaderText="Tanggal" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="InstructionTime" HeaderText="Jam" HeaderStyle-Width="50px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="InstructionGroup" HeaderText="Jenis Instruksi" HeaderStyle-Width="120px"
                                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="Description" HeaderText="Instruksi" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Belum ada instruksi yang diberikan")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="InstructionPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
            <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
                ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpDeleteROS" runat="server" Width="100%" ClientInstanceName="cbpDeleteROS"
                ShowLoadingPanel="false" OnCallback="cbpDeleteROS_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpROSDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
                ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
                ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpProcedure" runat="server" Width="100%" ClientInstanceName="cbpProcedure"
                ShowLoadingPanel="false" OnCallback="cbpProcedure_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpInstruction" runat="server" Width="100%" ClientInstanceName="cbpInstruction"
                ShowLoadingPanel="false" OnCallback="cbpInstruction_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpInstructionEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpDeleteTestOrder" runat="server" Width="100%" ClientInstanceName="cbpDeleteTestOrder"
                ShowLoadingPanel="false" OnCallback="cbpDeleteTestOrder_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteTestOrderEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
                ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpIntegrationNote" runat="server" Width="100%" ClientInstanceName="cbpIntegrationNote"
                ShowLoadingPanel="false" OnCallback="cbpIntegrationNote_Callback">
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpIntegrationNoteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
