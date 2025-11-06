<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="AssessmentPreProcedureEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AssessmentPreProcedureEntry1" %>

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
                    Asesmen Pra Tindakan</div>
            </td>
        </tr>
    </table>
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
                    if ($('#<%=hdnProcedureGroupProcessMode.ClientID %>').val() == "1")
                        cbpProcedureGroup.PerformCallback('add');
                    else
                        cbpProcedureGroup.PerformCallback('edit');
                }
                else {
                    displayErrorMessageBox("ERROR", "Jenis prosedur operasi harus dipilih sebelum diproses !");
                }
            }
            //#endregion

            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtAnamnesisText.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtServiceTime.ClientID %>').val())) {
                        getDiagnosticTestChecklistFormValues();
                        getDocumentChecklistFormValues();
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

            $('#<%=grdROSView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdROSView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnReviewOfSystemID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdROSView.ClientID %> tr:eq(1)').click();


            $('#<%=grdProcedureGroupView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdProcedureGroupView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnOrderDtProcedureGroupID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

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
                    displayErrorMessageBox("Body Diagram", "You should fill allergen name and allergy reaction !");
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
                displayConfirmationMessageBox("Body Diagram", message, function (result) {
                    if (result) {
                        cbpDeleteBodyDiagram.PerformCallback();
                    }
                });
            });

            $('#<%=txtAnamnesisText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtAnamnesisText.ClientID %>').blur(function () {
                ontxtAnamnesisTextChanged($(this).val());
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

            $('#<%=txtReaction.ClientID %>').keypress(function (e) {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                var key = e.which;
                if(key == 13)  // the enter key code
                {
                submitAllergy();
                }
            }); 

            $('#<%=txtDiagnosisText.ClientID %>').keypress(function (e) {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                var key = e.which;
                if(key == 13)  // the enter key code
                {
                submitDiagnosis();
                }
            }); 

            //#region Form Values
            if ($('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val().split(';');
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

            if ($('#<%=hdnDocumentCheckListValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnDocumentCheckListValue.ClientID %>').val().split(';');
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
            //#endregion

            registerCollapseExpandHandler();

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
                displayErrorMessageBox("Alergi Pasien", 'Error Message : ' + param[2]);
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
                var param = "0|0|0|0|0|0|0|" + assessmentID + "|0|0|";
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);    
            }
            else {
                displayMessageBox("Asesmen Pra Bedah","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromVitalSignLookup').die('click');
        $('#lblAddFromVitalSignLookup').live('click', function (evt) {
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|0|0|0|0|" + assessmentID;
                 openUserControlPopup("~/libs/Controls/EMR/Lookup/VitalSignLookupCtl1.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);  
            }
            else {
                displayMessageBox("Asesmen Pra Bedah","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var selectedObj = {};
            selectedObj = GetCurrentSelectedVitalSign(this);
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            var testOrderID = $('#<%=hdnPatientChargesDtID.ClientID %>').val();
            var param = "0|" +$('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|0|0|0|0|0|" + assessmentID + "|0|0|";
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Hapus pengkajian tanda vital dan indikator lainnya untuk pasien ini ?";
            displayConfirmationMessageBox("Asesmen Pra Bedah - Tanda Vital", message, function (result) {
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
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|0|" + assessmentID;     
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", param, "Pemeriksaan Fisik", 700, 500);           
            }
            else {
                displayMessageBox("Asesmen Pra Bedah","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromROSLookup').die('click');
        $('#lblAddFromROSLookup').live('click', function (evt) {
            var linkID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (linkID != '0' && linkID != '') {
                onBeforeOpenTrxPopup();
                if ($('#<%=hdnIsChanged.ClientID %>').val()=="0") {    
                    var param = "0|0|0|0|0|" + linkID;
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
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            var testOrderID = $('#<%=hdnPatientChargesDtID.ClientID %>').val();
            var param = $('#<%=hdnReviewOfSystemID.ClientID %>').val() + "|0|0|0|0|0|" + assessmentID;   
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

        //#region Body Diagram
        $('#lblAddBodyDiagram').die('click');
        $('#lblAddBodyDiagram').live('click', function (evt) {
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var linkedID = $('#<%=hdnPatientChargesDtID.ClientID %>').val() + ";" + "01";
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPAdd1Ctl.ascx", linkedID, "Body Diagram", 1200, 600);
            }
            else {
                displayMessageBox("Asesmen Pra Bedah","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
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

        //#region Diagnosis
        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtDiagnosisText.ClientID %>').val($('#<%=hdnEntryDiagnoseText.ClientID %>').val());
        }

        function ResetDiagnosisEntryControls(s) {
            ledDiagnose.SetValue('');
            $('#<%=txtDiagnosisText.ClientID %>').val('');
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
                getDiagnosticTestChecklistFormValues();
                getDocumentChecklistFormValues();
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                displayConfirmationMessageBox("Asesment",message, function (result) {
                    if (result) {
                        getDiagnosticTestChecklistFormValues();
                        getDocumentChecklistFormValues();
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
                    getDiagnosticTestChecklistFormValues();
                    getDocumentChecklistFormValues();
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion     
        
        //#region Get Form Values
        function getDiagnosticTestChecklistFormValues() {
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

            $('#<%=hdnDiagnosticTestCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
        
        function getDocumentChecklistFormValues() {
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

            $('#<%=hdnDocumentCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }                 
        //#endregion    

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
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val('0');
    }

    $('.imgAddProcedureGroup.imgLink').die('click');
    $('.imgAddProcedureGroup.imgLink').live('click', function (evt) {
        ResetProcedureGroupEntryControls();
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val("1");
        $('#trProcedureGroup').removeAttr('style');
    });

    $('.imgEditProcedureGroup.imgLink').die('click');
    $('.imgEditProcedureGroup.imgLink').live('click', function () {
        SetProcedureGroupEntityToControl(this);
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val("0");
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
            if (pageCount > 0)
                $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

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
            if (param[1] == "edit")
                $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val('0');

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

    function onGetLocalHiddenFieldValue(param) {
        $('#<%=hdnAssessmentID.ClientID %>').val(param);
    }

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
        <input type="hidden" runat="server" id="hdnDiagnosticTestCheckListLayout" value="" />
        <input type="hidden" runat="server" id="hdnDiagnosticTestCheckListValue" value="" />
        <input type="hidden" runat="server" id="hdnDocumentCheckListLayout" value="" />
        <input type="hidden" runat="server" id="hdnDocumentCheckListValue" value="" />
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
                            <li contentID="divPage1" title="Anamnesis dan Riwayat Kesehatan" class="w3-hover-red">Anamnesis dan Riwayat Kesehatan</li>
                            <li contentID="divPage2" title="Pengkajian Riwayat Kesehatan" class="w3-hover-red">Pengkajian Riwayat Kesehatan</li>
                            <li contentID="divPage3" title="Riwayat Alergi" class="w3-hover-red">Riwayat Alergi</li>
                            <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>                                               
                            <li contentID="divPage5" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>      
                            <li contentID="divPage6" title="Hasil Pemeriksaan Penunjang yang telah teridentifikasi secara benar" class="w3-hover-red">Hasil Pemeriksaan Penunjang</li>     
                            <li contentID="divPage9" title="Analisis" class="w3-hover-red">Analisis</li>                      
                            <li contentID="divPage8" title="Penandaan Lokasi Tindakan" class="w3-hover-red">Penandaan Lokasi Tindakan</li>
                            <li contentID="divPage7" title="Checklist Dokumen Terkait" class="w3-hover-red">Kelengkapan Berkas/Dokumen Terkait</li>
                        </ul>     
                    </div> 
                </td>
                    <td style="vertical-align:top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
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
                                                    <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label id="lblOrderNo">
                                            <%:GetLabel("Nomor Order")%></label>
                                    </td>
                                    <td colspan="2">
                                        <input type="hidden" id="hdnPatientChargesDtID" value="" runat="server" />
                                        <asp:TextBox ID="txtTransactionNo" Width="225px" runat="server" Enabled="false" />
                                    </td>
                                </tr>   
                                <tr>
                                    <td>
                                         <label id="Label4">
                                            <%:GetLabel("Tindakan")%></label>                                       
                                    </td>
                                    <td colspan="2">
                                          <asp:TextBox ID="txtItemName" Width="100%" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label class="lblMandatory lblLink" id="lblChiefComplaint">
                                                        <%=GetLabel("Anamnesis")%></label>
                                                </td>
                                                <td><img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAnamnesisText" runat="server" TextMode="MultiLine" Rows="3" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label class="lblNormal lblLink" id="lblHPI">
                                                        <%=GetLabel("Keluhan Lain yang menyertai")%></label>
                                                </td>
                                                <td><img class="imgLink" id="btnAddHPITemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
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
                                <tr style="display:none">
                                    <td class="tdLabel">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkIsTerminal" runat="server" Text=" Pasien Terminal" Checked="false" />
                                            </td>
                                        </table>
                                    </td>
                                </tr>  
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table class="tblEntryContent" border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="200px" />
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
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Penyakit Keluarga") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFamilyHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Penggunaan Obat") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicationHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Tindakan Sebelumnya") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSurgeryHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
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
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
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
                                                                    <br />
                                                                    <span class="lblLink" id="lblAddROS">
                                                                        <%= GetLabel("+ Tambah Pemeriksaan Fisik")%></span>
                                                                    <br />
                                                                    <span class="lblLink" id="lblAddFromROSLookup">
                                                                            <%= GetLabel("+ Salin Pemeriksaan Fisik")%></span>
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
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="450px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td style="vertical-align:top">
                                        <div id="divFormContent1" runat="server" style="height: 400px;overflow-y: auto;"></div>
                                    </td>
                                    <td style="vertical-align:top">
                                        <table border"0" cellpadding="0" cellspacing="1" style="width:100%">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                    <label class="lblNormal" id="Label1">
                                                        <%=GetLabel("Catatan Hasil Penunjang") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiagnosticResultSummary" runat="server" Width="100%" TextMode="Multiline"
                                                        Rows="15" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="vertical-align:top">
                                        <div id="divFormContent2" runat="server" style="height: 400px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
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
                        <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                <colgroup>
                                    <col style="width: 55%" />
                                    <col style="width: 45%" />
                                </colgroup>
                                <tr>
                                   <td style="vertical-align:top">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                            <tr>
                                                <td>
                                                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="100px" />
                                                            <col width="150px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="padding-left: 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Pre Diagnosis (ICD X)")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                                                <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                                                <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                                                    Width="99%" ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName" Value="DiagnoseID"
                                                                    MethodName="GetvDiagnoseList">
                                                                    <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                                                    <Columns>
                                                                        <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                                            Description="i.e. Cholera" Width="500px" />
                                                                    </Columns>
                                                                </qis:QISSearchTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 5px">
                                                                <label class="lblNormal lblMandatory">
                                                                    <%=GetLabel("Pre Diagnosis Text")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtDiagnosisText" runat="server" Width="99%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Profilaxis")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtProfilaxis" runat="server" Width="99%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Posisi Pasien Saat Tindakan")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtPatientPositionSummary" runat="server" Width="99%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" style="padding-left: 5px"><%=GetLabel("Estimasi Lama Tindakan") %></td>
                                                            <td><asp:TextBox ID="txtEstimatedDuration" Width="60px" CssClass="number" runat="server" /> menit</td>                                              
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                                                <%=GetLabel("Alat Khusus/Darah") %>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtSurgeryItemSummary" runat="server" Width="100%" TextMode="Multiline"
                                                                    Rows="5" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                                                <label class="lblNormal" id="Label3">
                                                                    <%=GetLabel("Catatan Konsultasi Ahli lainnya") %></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtReferralSummary" runat="server" Width="100%" TextMode="Multiline"
                                                                    Rows="5" />
                                                            </td>
                                                        </tr>
                                                       <tr>
                                                            <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                                                <label class="lblNormal" id="Label2">
                                                                    <%=GetLabel("Catatan Lainnya") %></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:TextBox ID="txtOtherSummary" runat="server" Width="100%" TextMode="Multiline"
                                                                    Rows="5" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                   </td>
                                   <td style="vertical-align:top; display:none">
                                        <h4 class="h4collapsed">
                                            <%=GetLabel("Pengkodean Tindakan")%></h4>
                                        <div class="containerTblEntryContent">
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                                <tr id="trProcedureGroup" style="display:none">
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
                                                                            <col style="width:95%"/>
                                                                            <col style="width:5%" />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <qis:QISSearchTextBox ID="ledProcedureGroup" ClientInstanceName="ledProcedureGroup" runat="server"
                                                                                    Width="99%" ValueText="ProcedureGroupID" FilterExpression="IsDeleted = 0" DisplayText="ProcedureGroupName"
                                                                                    MethodName="GetProcedureGroupList">
                                                                                    <ClientSideEvents ValueChanged="function(s){ onLedProcedureGroupLostFocus(s); }" />
                                                                                    <Columns>
                                                                                        <qis:QISSearchTextBoxColumn Caption="Jenis Operasi (Kode)" FieldName="ProcedureGroupName"
                                                                                            Description="i.e. Appendictomy" Width="500px" />
                                                                                    </Columns>
                                                                                </qis:QISSearchTextBox>
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
                                                            <dxcp:ASPxCallbackPanel ID="cbpProcedureGroupView" runat="server" Width="100%" ClientInstanceName="cbpProcedureGroupView"
                                                                ShowLoadingPanel="false" OnCallback="cbpProcedureGroupView_Callback">
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
                                                                                            <img class="imgAddProcedureGroup imgLink" title='<%=GetLabel("+ Jenis Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                                alt=""/>
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
                                                                                            <div><%#: Eval("ProcedureGroupName")%></div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Kategori Operasi"  DataField="SurgeryClassification" />
                                                                                </Columns>
                                                                                <EmptyDataTemplate>
                                                                                    <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
                                                                                </EmptyDataTemplate>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dxcp:ASPxCallbackPanel>
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
            <dxcp:ASPxCallbackPanel ID="cbpDeleteROS" runat="server" Width="100%" ClientInstanceName="cbpDeleteROS"
                ShowLoadingPanel="false" OnCallback="cbpDeleteROS_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpROSDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
                ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
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
        <dxcp:ASPxCallbackPanel ID="cbpProcedureGroup" runat="server" Width="100%" ClientInstanceName="cbpProcedureGroup"
            ShowLoadingPanel="false" OnCallback="cbpProcedureGroup_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
