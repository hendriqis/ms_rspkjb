<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="ProcedureAnesthesyStatusEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ProcedureAnesthesyStatusEntry1" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    Status Anestesi</div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .formContainer 
        {
          flex: 1;
          overflow-x: scroll;
          max-width:1200px;
        }        
    </style>
    <script type="text/javascript" id="dxss_anesthesyStatus1">
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

            setDatePicker('<%=txtAssessmentDate.ClientID %>');
            $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtStartDate.ClientID %>');
            $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtStartFastingDate.ClientID %>');
            $('#<%=txtStartFastingDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtStartAnesthesyDate.ClientID %>');
            $('#<%=txtStartAnesthesyDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtStartIncisionDate.ClientID %>');
            $('#<%=txtStartIncisionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtPremedicationDate.ClientID %>');
            $('#<%=txtPremedicationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtASAStatusRemarks.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtAssessmentTime.ClientID %>').val())) {
                            var assesmentDate = $('#<%:txtAssessmentDate.ClientID %>').val();
                            var assesmentTime = $('#<%:txtAssessmentTime.ClientID %>').val();
                            var startAnesthesyDate = $('#<%:txtStartAnesthesyDate.ClientID %>').val();
                            var startAnesthesyTime = $('#<%:txtStartAnesthesyTime.ClientID %>').val();
                            var anesthesyDuration = $('#<%:txtAnesthesyDuration.ClientID %>').val();
                            var startDate = $('#<%:txtStartDate.ClientID %>').val();
                            var startTime = $('#<%:txtStartTime.ClientID %>').val();
                            var duration = $('#<%:txtDuration.ClientID %>').val();
                            var stopSurgeryDate = $('#<%:txtStopSurgeryDate.ClientID %>').val();
                            var stopSurgeryTime = $('#<%:txtStopSurgeryTime.ClientID %>').val();
                            var stopAnesthesyDate = $('#<%:txtStopAnesthesyDate.ClientID %>').val();
                            var stopAnesthesyTime = $('#<%:txtStopAnesthesyTime.ClientID %>').val();

                            var isGeneralAnesthesy = $('#<%:chkIsGeneralAnesthesy.ClientID %>').is(':checked');
                            var isLocal = $('#<%:chkIsLocal.ClientID %>').is(':checked');
                            var isSedation = $('#<%:chkIsSedation.ClientID %>').is(':checked');
                            var isRegional = $('#<%:chkIsRegional.ClientID %>').is(':checked');

                            if(assesmentDate != '' && assesmentTime != '' && startAnesthesyDate != '' && startAnesthesyTime != '' && anesthesyDuration != ''
                                && startDate != '' && startTime != '' && duration != '' && stopSurgeryDate != '' && stopSurgeryTime != '' && stopAnesthesyDate != ''
                                && stopAnesthesyTime != '') {
                                if(isGeneralAnesthesy || isLocal || isSedation || isRegional) {
                                    getFormContent1Values();
                                    getFormContent2Values();
                                    getFormContent3Values();
                                    getFormContent4Values();
                                    getFormContent5Values();
                                    getFormContent6Values();
                                    onCustomButtonClick('save');
                                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                                }
                                else {
                                    displayErrorMessageBox('Asesmen', 'Harap melengkapi seluruh isian data yang mandatory');
                                }
                        }
                        else {
                            displayErrorMessageBox('Asesmen', 'Harap melengkapi seluruh isian data yang mandatory');     
                        }
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
                if ($('#<%=txtASAStatusRemarks.ClientID %>').val() != '') {
                    onBeforeOpenTrxPopup();
                    var text = $('#<%=txtASAStatusRemarks.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^01|" + text, "Physician Template Text", 700, 500);
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

            $('#<%=txtASAStatusRemarks.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtASAStatusRemarks.ClientID %>').blur(function () {
                ontxtASAStatusRemarksChanged($(this).val());
            });

            $('#<%=txtReaction.ClientID %>').keypress(function (e) {
             var key = e.which;
             if(key == 13)  // the enter key code
              {
                submitAllergy();
              }
            }); 

            //#region Form Values
            if ($('#<%=hdnMonitoringCheckListValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnMonitoringCheckListValue.ClientID %>').val().split(';');
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
            if ($('#<%=hdnIVCheckListValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnIVCheckListValue.ClientID %>').val().split(';');
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
            if ($('#<%=hdnAccessoriesListValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnAccessoriesListValue.ClientID %>').val().split(';');
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
            if ($('#<%=hdnPatientPositionValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnPatientPositionValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            if ($('#<%=hdnAirwayManagementValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnAirwayManagementValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            if ($('#<%=hdnRegionalAnestheticValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnRegionalAnestheticValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent6.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent6.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent6.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent6.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            //#endregion

            registerCollapseExpandHandler();

            $('#<%=rblIsUsePremedication.ClientID %>').change();
            $('#leftPageNavPanel ul li').first().click();
        });

        //#region Chief Complaint
        $('#lblChiefComplaint').die('click');
        $('#lblChiefComplaint').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Physician Template Text Lookup)");
        });

        function ontxtASAStatusRemarksChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^01'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtASAStatusRemarks.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Chief Complaint Text from your template text ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtASAStatusRemarks.ClientID %>').val(obj.TemplateText);
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
            displayConfirmationMessageBox("Alergi Pasien",message, function (result) {
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
                var param = "0|0|0|0|0|0|0|0|0|" + assessmentID + "|";
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);    
            }
            else {
                displayMessageBox("Status Anestesi","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('#lblAddFromVitalSignLookup').die('click');
        $('#lblAddFromVitalSignLookup').live('click', function (evt) {
            var assessmentID = $('#<%=hdnAssessmentID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var param = "0|0|0|0|0|0|0|" + assessmentID;
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
            var param = "0|" +$('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|0|0|0|0|0|0|0|" + assessmentID;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Hapus pengkajian tanda vital dan indikator lainnya untuk pasien ini ?";
            displayConfirmationMessageBox("Status Anestesi - Tanda Vital", message, function (result) {
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

         //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                getFormContent1Values();
                getFormContent2Values();
                getFormContent3Values();
                getFormContent4Values();
                getFormContent5Values();
                getFormContent6Values();
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                displayConfirmationMessageBox("Asesment",message, function (result) {
                    if (result) {
                        getFormContent1Values();
                        getFormContent2Values();
                        getFormContent3Values();
                        getFormContent4Values();
                        getFormContent5Values();
                        getFormContent6Values();
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
                    getFormContent1Values();
                    getFormContent2Values();
                    getFormContent3Values();
                    getFormContent4Values();
                    getFormContent5Values();
                    getFormContent6Values();
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion     
        
        //#region Get Form Values
        function getFormContent1Values() {
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

            $('#<%=hdnMonitoringCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }  

        function getFormContent2Values() {
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

            $('#<%=hdnIVCheckListValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
                 
        function getFormContent3Values() {
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

            $('#<%=hdnAccessoriesListValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
        function getFormContent4Values() {
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
            $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });

            $('#<%=hdnPatientPositionValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
        function getFormContent5Values() {
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
            $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });

            $('#<%=hdnAirwayManagementValue.ClientID %>').val(controlValues);

            return controlValues;
        }   
        function getFormContent6Values() {
            var controlValues = '';
            $('#<%=divFormContent6.ClientID %>').find('.ddlForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent6.ClientID %>').find('.txtForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });
            $('#<%=divFormContent6.ClientID %>').find('.optForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });
            $('#<%=divFormContent6.ClientID %>').find('.chkForm').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=1';
                else
                    controlValues += $(this).attr('controlID') + '=0';
            });

            $('#<%=hdnRegionalAnestheticValue.ClientID %>').val(controlValues);

            return controlValues;
        }  
        //#endregion    

    function onGetLocalHiddenFieldValue(param) {
        $('#<%=hdnAssessmentID.ClientID %>').val(param);
    }

    //#region Duration
    $('#<%=txtDuration.ClientID %>').die('change');
    $('#<%=txtDuration.ClientID %>').live('change', function () {
        var duration = parseInt($('#<%=txtDuration.ClientID %>').val());
        if (duration < 0) {
            $('#<%=txtDuration.ClientID %>').val("0");
        };
        var dt = Methods.calculateEndDate($('#<%=txtStartDate.ClientID %>').val(),$('#<%=txtStartTime.ClientID %>').val(), $('#<%=txtDuration.ClientID %>').val());
        $('#<%=txtStopSurgeryDate.ClientID %>').val(Methods.dateToDatePickerFormat(dt));
        $('#<%=txtStopSurgeryTime.ClientID %>').val(Methods.getTimeFromDate(dt));
    });

    $('#<%=txtAnesthesyDuration.ClientID %>').die('change');
    $('#<%=txtAnesthesyDuration.ClientID %>').live('change', function () {
        var duration = parseInt($('#<%=txtAnesthesyDuration.ClientID %>').val());
        if (duration < 0) {
            $('#<%=txtAnesthesyDuration.ClientID %>').val("0");
        };
        var dt = Methods.calculateEndDate($('#<%=txtStartAnesthesyDate.ClientID %>').val(),$('#<%=txtStartAnesthesyTime.ClientID %>').val(), $('#<%=txtAnesthesyDuration.ClientID %>').val());
        $('#<%=txtStopAnesthesyDate.ClientID %>').val(Methods.dateToDatePickerFormat(dt));
        $('#<%=txtStopAnesthesyTime.ClientID %>').val(Methods.getTimeFromDate(dt));
    });
    //#endregion

    $('#<%=rblIsASAChanged.ClientID %> input').die('change');
    $('#<%=rblIsASAChanged.ClientID %> input').live('change', function () {
        if ($(this).val() == "1") {       
           $('#<%=txtASAStatusRemarks.ClientID %>').prop("disabled", false);
        }
        else
        {
           $('#<%=txtASAStatusRemarks.ClientID %>').prop("disabled", true);
           $('#<%=txtASAStatusRemarks.ClientID %>').val("");
        }
    });

    $('#<%=rblIsAnesthesiaTypeChanged.ClientID %> input').die('change');
    $('#<%=rblIsAnesthesiaTypeChanged.ClientID %> input').live('change', function () {
        if ($(this).val() == "1") {       
           $('#<%=txtAnesthesiaTypeChangeRemarks.ClientID %>').prop("disabled", false);
        }
        else
        {
           $('#<%=txtAnesthesiaTypeChangeRemarks.ClientID %>').prop("disabled", true);
           $('#<%=txtAnesthesiaTypeChangeRemarks.ClientID %>').val("");
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
            $('#trPremedication').removeAttr('style');
        }
        else {
            $('#trPremedication').attr('style', 'display:none');
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
        <input type="hidden" runat="server" id="hdnMonitoringCheckListLayout" value="" />
        <input type="hidden" runat="server" id="hdnMonitoringCheckListValue" value="" />
        <input type="hidden" runat="server" id="hdnIVCheckListLayout" value="" />
        <input type="hidden" runat="server" id="hdnIVCheckListValue" value="" />
        <input type="hidden" runat="server" id="hdnAccessoriesListLayout" value="" />
        <input type="hidden" runat="server" id="hdnAccessoriesListValue" value="" />
        <input type="hidden" runat="server" id="hdnPatientPositionLayout" value="" />
        <input type="hidden" runat="server" id="hdnPatientPositionValue" value="" />
        <input type="hidden" runat="server" id="hdnAirwayManagementLayout" value="" />
        <input type="hidden" runat="server" id="hdnAirwayManagementValue" value="" />
        <input type="hidden" runat="server" id="hdnRegionalAnestheticLayout" value="" />
        <input type="hidden" runat="server" id="hdnRegionalAnestheticValue" value="" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnIsPatientAllergyExists" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 200px" />
                    <col />
                </colgroup>
                <tr>
                <td style="vertical-align:top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentID="divPage1" title="Asesmen Pra Induksi" class="w3-hover-red">Asesmen Pra Induksi</li>
                            <li contentID="divPage3" title="Riwayat Alergi" class="w3-hover-red">Riwayat Alergi</li>
                            <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>                                               
                            <li contentID="divPage5" title="Monitors" class="w3-hover-red">Monitors</li>      
                            <li contentID="divPage6" title="Intravenous Lines" class="w3-hover-red">Intravenous Lines</li>      
                            <li contentID="divPage7" title="Accessories" class="w3-hover-red">Accessories</li>
                            <li contentID="divPage8" title="Posisi Pasien" class="w3-hover-red">Patient Position(s) Used During Case</li>
                            <li contentID="divPage9" title="Airway Management" class="w3-hover-red">Airway Management</li>
                            <li contentID="divPage10" title="Regional Anesthetic(s)" class="w3-hover-red">Regional Anesthetic(s)</li>
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
                                                    <asp:TextBox ID="txtAssessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtAssessmentTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label id="lblOrderNo">
                                            <%:GetLabel("Nomor Transaksi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnPatientChargesDtID" value="" runat="server" />
                                                    <asp:TextBox ID="txtTransactionNo" Width="150px" runat="server" Enabled="false" />
                                                </td>
                                                <td class="tdLabel">
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>  
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Rencana Tindakan") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProcedureGroupSummary" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="3" ReadOnly="true" />
                                    </td>
                                </tr>  
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Mulai Puasa")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStartFastingDate" CssClass="datepicker" Style="text-align: center" Width="120px" runat="server" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblNormal" for="txtStartFastingTime">
                                                        <%=GetLabel("Jam Mulai Puasa")%></label>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtStartFastingTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td> 
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                                  
                                <tr>
                                    <td class="tdLabel"">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal Mulai Anestesi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStartAnesthesyDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jam Mulai Anestesi")%></label>
                                                </td>
                                                <td style="padding-left : 5px">
                                                    <asp:TextBox ID="txtStartAnesthesyTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                                <td class="tdLabel"">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Lama Anestesi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAnesthesyDuration" Width="60px" TextMode="Number" CssClass="number" runat="server" min="0" /> menit
                                                </td>   
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Mulai Tindakan")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStartDate" CssClass="datepicker" Width="120px" Style="text-align: center" runat="server" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jam Mulai Tindakan")%></label>
                                                </td>
                                                <td style="padding-left : 5px">
                                                    <asp:TextBox ID="txtStartTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Lama Tindakan") %></label>
                                                </td>
                                                <td>   
                                                    <asp:TextBox ID="txtDuration" Width="60px" TextMode="Number" CssClass="number" runat="server" /> menit
                                                </td>    
                                            </tr>
                                        </table>
                                    </td>
                                </tr>    
                                <tr>
                                    <td class="tdLabel"">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal Selesai Tindakan")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStopSurgeryDate" CssClass="datepicker" Width="120px" runat="server" ReadOnly="true" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jam Selesai Tindakan")%></label>
                                                </td>
                                                <td style="padding-left : 5px">
                                                    <asp:TextBox ID="txtStopSurgeryTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal Selesai Anestesi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStopAnesthesyDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jam Selesai Anestesi")%></label>
                                                </td>
                                                <td style="padding-left : 5px">
                                                    <asp:TextBox ID="txtStopAnesthesyTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Premedikasi")%></label>
                                    </td>
                                    <td colspan="2">
                                        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col style="width:100px" />
                                                <col style="width:120px" />
                                                <col style="width:150px" />
                                                <col style="width:140px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblIsUsePremedication" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                    </asp:RadioButtonList>  
                                                </td>
                                            </tr>
                                        </table>                                             
                                    </td>
                                </tr>
                                <tr id="trPremedication">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Mulai Premedikasi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPremedicationDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jam Premedikasi")%></label>
                                                </td>
                                                <td style="padding-left : 5px;">
                                                    <asp:TextBox ID="txtPremedicationTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Obat Premedikasi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPremedication" Width="99%" runat="server"/>
                                                </td>
                                                <td style="padding-left : 5px;">
                                                    <asp:RadioButtonList ID="rblGCPremedicationType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text=" Oral" Value="X456^01" />
                                                        <asp:ListItem Text=" IM" Value="X456^02" />
                                                        <asp:ListItem Text=" IV" Value="X456^03" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display:none">
                                    <td class="tdLabel"">
                                        <label class="lblNormal">
                                            <%=GetLabel("Mulai Insisi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStartIncisionDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td class="tdLabel" style="padding-left : 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jam Mulai Insisi")%></label>
                                                </td>
                                                <td style="padding-left : 5px">
                                                    <asp:TextBox ID="txtStartIncisionTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>   
                                <tr>
                                    <td class="tdLabel"><%=GetLabel("Status Fisik ASA Terakhir") %></td>
                                    <td>
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
                                                <td style="padding-left:40px">
                                                    <asp:RadioButtonList ID="rblIsASAChanged" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text=" Tidak berubah" Value="0" />
                                                        <asp:ListItem Text=" Berubah (Catatan Perubahan)" Value="1" />
                                                    </asp:RadioButtonList>
                                                </td>  
                                            </tr>
                                        </table>
                                    </td>  
                                </tr>                                                                                                                                                       
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label class="lblNormal" id="lblASARemarks">
                                                        <%=GetLabel("Catatan Perubahan Status ASA")%></label>
                                                </td>
                                                <td style="display:none"><img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtASAStatusRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 120px">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Teknik Anestesi")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col width="180px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
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
                                                <td class="tdLabel" style="width: 100px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tipe Regional")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRegionalAnesthesiaType" ClientInstanceName="cboRegionalAnesthesiaType"
                                                        Width="150px" />
                                                </td>
                                                <td style="padding-left:10px;" colspan="2">
                                                    <asp:RadioButtonList ID="rblIsAnesthesiaTypeChanged" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text=" Tidak berubah" Value="0" />
                                                        <asp:ListItem Text=" Berubah" Value="1" />
                                                    </asp:RadioButtonList>
                                                </td> 
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label class="lblNormal" id="lblAnesthesiaTypeChangeRemarks">
                                                        <%=GetLabel("Catatan Perubahan Teknik Anestesi")%></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAnesthesiaTypeChangeRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:CheckBox ID="chkIsTimeOutSafetyCheck" runat="server" Text=" TIME OUT (Correct : Patient, Procedure, Side/Site, Position, Special Equipment" Checked="false" />
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
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="450px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="vertical-align:top">
                                        <div id="divFormContent2" runat="server" style="height: 480px;overflow-y: auto;"></div>
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
                                        <div id="divFormContent3" runat="server" style="height: 480px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="vertical-align:top">
                                        <div id="divFormContent4" runat="server" style="height: 480px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage9" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="vertical-align:top">
                                        <div id="divFormContent5" runat="server" style="height: 480px;overflow-y: auto;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage10" class="w3-border divPageNavPanelContent w3-animate-left formContainer" style="display:none">
                            <table class="tblContentArea">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="vertical-align:top; width: 100%">
                                        <div id="divFormContent6" runat="server" style="height: 480px;overflow-y: auto;"></div>
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
    </div>
</asp:Content>
