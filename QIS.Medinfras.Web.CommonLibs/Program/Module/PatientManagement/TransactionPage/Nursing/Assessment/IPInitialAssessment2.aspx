 <%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="IPInitialAssessment2.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.IPNurseInitialAssessment2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Discard Changes")%></div>
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
                    getPhysicalFormValues();
                    getPsychosocialFormValues();
                    getEducationFormValues();
                    getDischargePlanningFormValues();
                    getAdditionalFormValues();
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
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
                    $('#<%=divFormContent1.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }

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

            if ($('#<%=hdnAdditionalValue.ClientID %>').val() != '') {
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
                }
            }
            //#endregion

            registerCollapseExpandHandler();
        });

        //#region Chief Complaint
        $('#lblChiefComplaint').die('click');
        $('#lblChiefComplaint').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Template Text Lookup)");
        });

        function onTxtChiefComplaintChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^01'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtChiefComplaint.ClientID %>').val() != '') {
                            var message = "Are you sure to replace the Chief Complaint Text from your template text ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtChiefComplaint.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region Diagnosis
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
        //#endregion

        //#region History of Present Illness
        $('#lblHPI').die('click');
        $('#lblHPI').live('click', function (evt) {
            alert("Sorry, this feature is currently in development process (Template Text Lookup)");
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
                }
                else {
                    showToast('SEND ORDER : FAILED', 'Error Message : ' + param[3]);
                }
            }
        }

        //#region Get Form Values
        function getPhysicalFormValues() {
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
            $('#<%=divFormContent1.ClientID %>').find('.chkNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                if ($(this).is(':checked'))
                    controlValues += $(this).attr('controlID') + '=' + $(this).attr('nursingProblemCode');
                else
                    controlValues += $(this).attr('controlID') + '=-';
            });
            $('#<%=divFormContent1.ClientID %>').find('.txtNursingProblem').each(function () {
                if (controlValues != '')
                    controlValues += ";";
                controlValues += $(this).attr('controlID') + '=' + $(this).val();
            });

            $('#<%=hdnPhysicalExamValue.ClientID %>').val(controlValues);

            return controlValues;
        }

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

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER || cboVisitReason.GetValue() == Constant.VisitReason.ACCIDENT)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        //#region Propose
        $('.btnTest').die('click');
        $('.btnTest').live('click', function () {
            alert(getPsychosocialFormValues());
        });
        //#endregion
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
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
        <input type="hidden" value="1" id="hdnInstructionProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnInstructionID" runat="server" />
        <input type="hidden" value="0" id="hdnPatientVisitNoteID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnIsPatientAllergyExists" runat="server" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
        <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
        <input type="hidden" runat="server" id="hdnPsychosocialLayout" value="" />
        <input type="hidden" runat="server" id="hdnPsychosocialValue" value="" />
        <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
        <input type="hidden" runat="server" id="hdnEducationValue" value="" />
        <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
        <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalLayout" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalValue" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 40%" />
                    <col style="width: 60%" />
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
                                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
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
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Alasan Masuk RS")%></label>
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
                                <td class="tdLabel" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="tdLabel" valign="top" style="width:120px">
                                                <label class="lblMandatory lblLink" id="lblChiefComplaint">
                                                    <%=GetLabel("Keluhan Utama")%></label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="3" Width="100%" />
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
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHPISummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="8" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="lblGCFamilyRelation">
                                        <%=GetLabel("Sumber Anamnesis")%></label>
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
                                        <td class="tdLabel">
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
                                    <label class="lblNormal">
                                        <%=GetLabel("Clinical Pathway")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboClinicalPathway" ClientInstanceName="cboClinicalPathway" Width="100%"
                                        runat="server">
<%--                                        <ClientSideEvents Init="function(s,e){ onCboVisitReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />--%>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>                 
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <h4 class="h4collapsed">
                            <%=GetLabel("Diagnosa Pasien")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
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
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <%#: Eval("DifferentialDateInString")%>,
                                                                                <%#: Eval("DifferentialTime")%>,                                                                           
                                                                                <%#: Eval("ParamedicName")%></div>
                                                                            <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                <span Style="color: Blue; font-size: 1.1em">
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
                            <%=GetLabel("Riwayat Kesehatan Keluarga")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Kesehatan Keluarga") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFamilyHistory" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
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
                            <%=GetLabel("Masalah Keperawatan")%></h4>
                        <div class="containerTblEntryContent">
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Pemeriksaan Fisik")%></h4>
                        <div class="containerTblEntryContent">
                            <table class="tblContentArea">
                                <tr>
                                    <td>
                                        <div id="divFormContent1" runat="server" style="height: 400px;overflow-y: scroll;"></div>
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
                            <%=GetLabel("Pengkajian Psikososial Spiritual dan Kultural")%></h4>
                        <div class="containerTblEntryContent">
                            <table class="tblContentArea">
                                <tr>
                                    <td>
                                        <div id="divFormContent2" runat="server" style="height: 400px;overflow-y: scroll;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Kebutuhan Informasi dan Edukasi")%></h4>
                        <div class="containerTblEntryContent">
                            <table class="tblContentArea">
                                <tr>
                                    <td>
                                        <div id="divFormContent3" runat="server" style="height: 400px;overflow-y: scroll;"></div>
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
                                        <div id="divFormContent4" runat="server" style="height: 200px;overflow-y: scroll;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsNeedPatientEducation" runat="server" Text=" Kebutuhan Edukasi" Checked="false" />
                                    </td>
                                </tr>
<%--                               <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsHasAdditionalAssessment" runat="server" Text=" Asesmen Tambahan (Populasi Khusus)" Checked="false" />
                                    </td>
                               </tr>--%>
                                <tr style="display:none">
                                    <td>
                                        <input type="button" class="btnTest w3-btn w3-hover-blue" value="Display HTML" style="background-color:Red;color:White; width:120px;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Kebutuhan Asesment Tambahan (Populasi Khusus)")%></h4>
                        <div class="containerTblEntryContent">
                            <table class="tblContentArea">
                                <tr>
                                    <td>
                                        <div id="divFormContent5" runat="server" style="height: 400px;overflow-y: scroll;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    <h4 class="h4collapsed" style="display:none">
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
                                    <asp:TextBox ID="txtFunctionalTypeRemarks" CssClass="txtFunctionalTypeRemarks" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h4 class="h4collapsed" style="display:none">
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
                                    <asp:RadioButtonList ID="rblFamilyRelationship" CssClass="rblFamilyRelationship" runat="server"
                                        RepeatDirection="Horizontal" CellPadding="10">
                                        <asp:ListItem Text=" Baik" Value="1" />
                                        <asp:ListItem Text=" Kurang Baik" Value="0" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelColumn">
                                    <label class="lblNormal" style="padding-left:10px">
                                        <%=GetLabel("Jelaskan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtFamilyRelationshipRemarks" CssClass="txtFamilyRelationshipRemarks" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="labelColumn" colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Memerlukan kebutuhan privasi tambahan :")%></label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblIsNeedAdditionalPrivacy" CssClass="rblIsNeedAdditionalPrivacy" runat="server"
                                        RepeatDirection="Horizontal" CellPadding="14">
                                        <asp:ListItem Text=" Ya" Value="1" />
                                        <asp:ListItem Text=" Tidak" Value="0" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelColumn">
                                    <label class="lblNormal" style="padding-left:10px">
                                        <%=GetLabel("Jelaskan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNeedAdditionalPrivacyRemarks" CssClass="txtNeedAdditionalPrivacyRemarks" runat="server" Width="100%" />
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
                                    <label class="lblNormal" style="padding-left:10px">
                                        <%=GetLabel("Kecenderungan bunuh diri dilaporkan ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCommitSuicideRemarks" CssClass="txtCommitSuicideRemarks" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="labelColumn" colspan="2">
                                    <label class="lblNormal">
                                        <%=GetLabel("Terdapat masalah ekonomi :")%></label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblHasFinancialProblem" CssClass="rblHasFinancialProblem" runat="server"
                                        RepeatDirection="Horizontal" CellPadding="14">
                                        <asp:ListItem Text=" Ya" Value="1" />
                                        <asp:ListItem Text=" Tidak" Value="0" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelColumn">
                                    <label class="lblNormal" style="padding-left:10px">
                                        <%=GetLabel("Jelaskan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtFinancialProblemRemarks" CssClass="txtFinancialProblemRemarks" runat="server" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h4 class="h4collapsed" style="display:none">
                        <%=GetLabel("Asesment RAPUH")%></h4>
                    <div class="containerTblEntryContent">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIsHasRAPUHAssessment" runat="server" Text="  Asesment RAPUH" Checked="false" />
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
                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_R" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
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
                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_A" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
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
                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_P" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
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
                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_U" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
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
                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_H" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
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
                                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                        <tr>
                                            <td><asp:TextBox ID="txtRAPUHScore" runat="server" Width="100px" CssClass="number" /></td>
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
            <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
                ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
