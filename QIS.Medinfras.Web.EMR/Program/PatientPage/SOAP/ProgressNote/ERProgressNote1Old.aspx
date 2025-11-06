<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="ERProgressNote1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ERProgressNote1" %>

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
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnPatientStatusSaveNew" runat="server" crudmode="R" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div>
            <%=GetLabel("Save and New")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Discard Changes")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .templatePatientBodyDiagram             { padding: 10px; }
        .templatePatientBodyDiagram .containerImage { float:left; display: table-cell; vertical-align:middle; border: 1px solid #AAA; width:300px; height:300px; margin-right: 20px; text-align:center; position:relative; }
        .templatePatientBodyDiagram .containerImage img { max-height:300px; max-width:300px; position:absolute;top:0;bottom:0;left:0;right:0;margin:auto; }
        .templatePatientBodyDiagram .spLabel    { display: inline-block; width: 120px; font-weight:bolder; }
        .templatePatientBodyDiagram .spValue    { margin-left:10px; }        
    </style>

    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            setDatePicker('<%=txtNoteDate.ClientID %>');
            $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtChiefComplaint.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
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

            $('.btnAllergyButton').click(function () {
                if (($('#<%=txtAllergenName.ClientID %>').val() != '' && $('#<%=txtReaction.ClientID %>').val() != '')) {
                    if ($('#<%=hdnAllergyProcessMode.ClientID %>').val() == "1")
                        cbpAllergy.PerformCallback('add');
                    else
                        cbpAllergy.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill allergen name and allergy reaction !");
                }
            });

            $('#btnBodyDiagramContainerPrev').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('prev');
            });
            $('#btnBodyDiagramContainerNext').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('next');
            });

            $('#imgEditBodyDiagram').live('click', function () {
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
                if ((cboDiagnosisType.GetValue() != '' && $('#<%=txtDiagnosisText.ClientID %>').val() != '')) {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1")
                        cbpDiagnosis.PerformCallback('add');
                    else
                        cbpDiagnosis.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill Diagnosis Type and Name field !");
                }
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
            });

            $('#<%=txtSubjective.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtObjective.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtAssessment.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanning.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            registerCollapseExpandHandler();
        });

        function onAfterCustomClickSuccess(type, retval) {
        }

        var allergyPageCount = parseInt('<%=gridAllergyPageCount %>');
        var vitalSignPageCount = parseInt('<%=gridVitalSignPageCount %>');
        var diagnosticPageCount = parseInt('<%=gridDiagnosticPageCount %>');
        var diagnosisPageCount = parseInt('<%=gridDiagnosisPageCount %>');
        var rosPageCount = parseInt('<%=gridROSPageCount %>');
        var laboratoryPageCount = parseInt('<%=gridLaboratoryPageCount %>');
        var imagingPageCount = parseInt('<%=gridImagingPageCount %>');

        $(function () {
            setPaging($("#allergyPaging"), allergyPageCount, function (page) {
                cbpAllergyView.PerformCallback('changepage|' + page);
            });
            setPaging($("#vitalSignPaging"), vitalSignPageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
            setPaging($("#rosPaging"), rosPageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
            setPaging($("#diagnosisPaging"), diagnosisPageCount, function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });
            setPaging($("#laboratoryPaging"), laboratoryPageCount, function (page) {
                cbpLaboratoryView.PerformCallback('changepage|' + page);
            });
            setPaging($("#imagingPaging"), imagingPageCount, function (page) {
                cbpImagingView.PerformCallback('changepage|' + page);
            });
            setPaging($("#diagnosticPaging"), diagnosticPageCount, function (page) {
                cbpDiagnosticView.PerformCallback('changepage|' + page);
            });

        });

        //#region Allergy
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

            var message = "Are you sure to delete this allergy record for this patient with allergen name <b>'" + selectedObj.Allergen +"'</b> ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpAllergy.PerformCallback('delete');
                }
            });
        });

        function ResetAllergyEntryControls(s) {
            cboAllergenType.SetValue('');
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

        //#region Vital Sign Paging
        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|", "Vital Sign & Indicator", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|"+$('#<%=hdnVitalSignRecordID.ClientID %>').val(), "Vital Sign & Indicator", 700, 500);
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
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", "", "Review of System", 700, 500);
        });

        $('.imgEditROS.imgLink').die('click');
        $('.imgEditROS.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedROS(this);
            $('#<%=hdnReviewOfSystemID.ClientID %>').val(selectedObj.ID);
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/ROSEntry1Ctl.ascx", $('#<%=hdnReviewOfSystemID.ClientID %>').val(), "Review of System", 700, 500);
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
        //endregion

        //#region Diagnosis
        function onLedDiagnoseLostFocus(led) {
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
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
                    cbpDiagnosis.PerformCallback('delete');
                }
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
            var param = "X001^004|0";
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Laboratory Test Order", 1200, 600);
        });

        $('.imgAddLabOrderDt.imgLink').die('click');
        $('.imgAddLabOrderDt.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "X001^004|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Laboratory Test Order", 1200, 600);
        });

        $('.imgEditLabOrder.imgLink').die('click');
        $('.imgEditLabOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "LB|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Laboratory Order", 700, 500);
        });

        $('.imgDeleteLabOrder.imgLink').die('click');
        $('.imgDeleteLabOrder.imgLink').live('click', function () {
            var message = "Are you sure to delete this Laboratory Test Order record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteTestOrder.PerformCallback('LB');
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

        function onCbpLaboratoryViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();

                setPaging($("#laboratoryPaging"), pageCount, function (page) {
                    cbpLaboratoryView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdLaboratoryView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshLaboratoryGrid() {
            cbpLaboratoryView.PerformCallback('refresh');
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
            var param = "X001^005|0";
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('.imgAddImagingOrderDt.imgLink').die('click');
        $('.imgAddImagingOrderDt.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "X001^005|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('.imgEditImagingOrder.imgLink').die('click');
        $('.imgEditImagingOrder.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var param = "LB|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val();
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

        function onCbpImagingViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();

                setPaging($("#imagingPaging"), pageCount, function (page) {
                    cbpImagingView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdImagingView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshImagingGrid() {
            cbpImagingView.PerformCallback('refresh');
        }
        //#endregion

        //#region Diagnostic
        function onCbpImagingViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();

                setPaging($("#diagnosticPaging"), pageCount, function (page) {
                    cbpDiagnosticView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosticView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onCbpDeleteTestOrderEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == 'LB') {
                    cbpLaboratoryView.PerformCallback('refresh');
                }
                else if (param[1] == 'IS') {
                    cbpImagingView.PerformCallback('refresh');
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
        //#region Change Page - Save
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
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnProcedureID" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />        
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
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
                                                <asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Subjective")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSubjective" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Objective")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObjective" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Assessment")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAssessment" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Planning")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlanning" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <h4 class="h4expanded">
                            <%=GetLabel("Chief Complaint")%></h4>
                        <div class="containerTblEntryContent">
                            <table id="tblHPI" style="width: 100%" runat="server">
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="15"
                                            Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Alergi")%></h4>
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
                                                    <td>
                                                         <label class="lblNormal"><%=GetLabel("Tipe Alergi")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType" Width="100px">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                         <label class="lblNormal"><%=GetLabel("Jenis/Nama")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAllergenName" runat="server" Width="150px" />
                                                    </td>
                                                    <td>
                                                         <label class="lblNormal"><%=GetLabel("Reaksi")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReaction" runat="server" Width="150px" />
                                                    </td>
                                                    <td>
                                                        <input id="btnAllergyButton" type="button" value='<%= GetLabel("Simpan")%>' style="width:80px" class="btnAllergyButton" />
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
                                                                            </div>
                                                                            <div>
                                                                                <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <div style="padding-left: 20px; float: left; width: 300px;">
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
                                                            <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdROSView_RowDataBound" >
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
                                                                                        <div style="padding-left:20px;float:left;width:300px;">
                                                                                            <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>><strong> <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %> : </strong></span>&nbsp;
                                                                                            <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>><%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <br style="clear:both"/>
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
                                            <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="rosPaging"></div>
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
                                        <table border="0" cellpadding="0" cellspacing="0" style="float: left;margin-top: 0px;">
                                            <tr>
                                                <td><span class="lblLink" id="lblAddBodyDiagram"><%= GetLabel("+ Tambah Body Diagram")%></span></td>
                                            </tr>
                                        </table>   
                                    </td>
                                    <td>
                                        <table id="tblBodyDiagramNavigation" runat="server" border="0" cellpadding="0" cellspacing="0" style="display:none;float: right;margin-top: 0px;">
                                            <tr>
                                                <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px" alt="" class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" /></td>
                                                <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px" alt="" class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" /></td>
                                            </tr>
                                        </table>  
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="position: relative;" id="divBodyDiagram" runat="server" >
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
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="float: right;margin-top: 0px;">
                                                                        <tr>
                                                                            <td><img src='<%=ResolveUrl("~/Libs/Images/Button/edit.png") %>' title="Edit" width="25px" alt="" class="imgLink" id="imgEditBodyDiagram" style="margin-left: 5px;" /></td>
                                                                            <td><img src='<%=ResolveUrl("~/Libs/Images/Button/delete.png") %>' title="Delete" width="25px" alt="" class="imgLink" id="imgDeleteBodyDiagram" style="margin-left: 5px;" /></td>
                                                                        </tr>
                                                                    </table>  
                                                                </div>
                                                                <span class="spLabel"><%=GetLabel("Diagram Name") %></span>:<span class="spValue" id="spnDiagramName" runat="server"></span><br />
                                                                <span class="spLabel"><%=GetLabel("Remarks") %></span>: <br />
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
                                                                        <td><img alt="" style="width:16px;height:16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>"/></td>
                                                                        <td>:</td>
                                                                        <td><%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%></td>
                                                                        <td>:</td>
                                                                        <td><%#: DataBinder.Eval(Container.DataItem, "SymbolName")%></td>
                                                                        <td>:</td>
                                                                        <td><%#: DataBinder.Eval(Container.DataItem, "Remarks")%></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                                <br />
                                                                <span class="spLabel"><%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
                                                                <span class="spLabel"><%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime" runat="server"></span><br />
                                                            </div>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel> 
                                        </div>
                                        <table id="tblEmpty" style="display:none;width:100%" runat="server">
                                            <tr class="trEmpty">
                                                <td align="center" valign="middle"><%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %></td>
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
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                            <colgroup>
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal"><%=GetLabel("Tipe Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisType" ClientInstanceName="cboDiagnosisType" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left:5px">
                                                     <label class="lblNormal"><%=GetLabel("ICD X")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                                    <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server" Width="99%"
                                                        ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName" MethodName="GetDiagnosisList" >
                                                        <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName" Description="i.e. Cholera" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal"><%=GetLabel("Status Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDiagnosisStatus" ClientInstanceName="cboDiagnosisStatus" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left:5px">
                                                     <label class="lblNormal"><%=GetLabel("Diagnosa Text")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiagnosisText" runat="server" Width="370px" />
                                                </td>
                                                <td style="padding-left:5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td><img class="btnApplyDiagnosis imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>' alt="" /></td>
                                                            <td><img class="btnCancelDiagnosis imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>' alt="" /></td>
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
                                                        <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height:300px">
                                                            <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" >
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("Diagnose Information")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div><%#: Eval("DifferentialDateInString")%>, <%#: Eval("DifferentialTime")%>, <%#: Eval("ParamedicName")%></div>
                                                                            <div>
                                                                                    <span style="color:Blue; font-size:1.1em"><%#: Eval("DiagnosisText")%></span>
                                                                                    (<b><%#: Eval("DiagnoseID")%></b>)
                                                                            </div>
                                                                            <div><%#: Eval("ICDBlockName")%></div>
                                                                            <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                                                                            <div><%#: Eval("Remarks")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
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
                                                    <div id="diagnosisPaging"></div>
                                                </div>
                                            </div> 
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Planning : Laboratory")%></h4>
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
                                                            <asp:GridView ID="grdLaboratoryView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdLaboratoryView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddLabOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>  />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditLabOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>/>
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteLabOrder imgLink" title='<%=GetLabel("Delete Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>/>
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
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                                <colgroup>
                                                                                    <col style="width:70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div><%=GetLabel("Pemeriksaan") %></div>                                                                                    
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
                                                                                        <div><%#: Eval("TestOrderDateTimeInString")%>, <%#: Eval("ServiceUnitName")%>, <%#: Eval("ParamedicName") %>, <span style="font-style:italic; font-weight:bold"><%#: Eval("TestOrderNo")%></span></div>
                                                                                        <div style="font-style:italic">
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
                                                    <div id="laboratoryPaging"></div>
                                                </div>
                                            </div> 
                                        </div>                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Planning : Imaging")%></h4>
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
                                                            <asp:GridView ID="grdImagingView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdImagingView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddImagingOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>  />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditImagingOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>/>
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteImagingOrder imgLink" title='<%=GetLabel("Delete Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>/>
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
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                                <colgroup>
                                                                                    <col style="width:70%" />
                                                                                    <col />
                                                                                </colgroup>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div><%=GetLabel("Pemeriksaan") %></div>                                                                                    
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
                                                                                        <div><%#: Eval("TestOrderDateTimeInString")%>, <%#: Eval("ServiceUnitName")%>, <%#: Eval("ParamedicName") %>, <span style="font-style:italic; font-weight:bold"><%#: Eval("TestOrderNo")%></span></div>
                                                                                        <div style="font-style:italic">
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
                                                    <div id="imagingPaging"></div>
                                                </div>
                                            </div> 
                                        </div>                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed" style="display:none">
                            <%=GetLabel("Planning : Penunjang Medis Lain-lain")%></h4>
                        <div class="containerTblEntryContent containerEntryPanel1" style="display:none">
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
                                                            <asp:GridView ID="grdDiagnosticView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgAddImagingOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>  />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditImagingOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>/>
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteImagingOrder imgLink" title='<%=GetLabel("Delete Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>/>
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
                                                                            <div><%=GetLabel("Order Date") %> - <%=GetLabel("Time") %>,  <span style="color:blue"><%=GetLabel("Ordered By") %></span></div>
                                                                            <div style="font-weight: bold"><%=GetLabel("Service Unit") %>, <%=GetLabel("Test Order No.") %>, <span style="font-style:italic"><%=GetLabel("Charges No.") %></span></div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div><%#: Eval("TestOrderDateTimeInString")%>,  <span style="color:blue"> <%#: Eval("ParamedicName") %></span></div>
                                                                                        <div style="font-weight: bold"><%#: Eval("ServiceUnitName")%>, <%#: Eval("TestOrderNo")%>, <span style="font-style:italic"><%#: Eval("ChargesNo")%></span></div>
                                                                                    </td>
                                                                                    <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> ><div><input type="button" class="btnPropose" value="Send Order" /></div></td>
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
                                                    <div id="diagnosticPaging"></div>
                                                </div>
                                            </div> 
                                        </div>                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h4 class="h4collapsed" style="display:none">
                            <%=GetLabel("Planning : Resep Farmasi")%></h4>
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
            <dxcp:ASPxCallbackPanel ID="cbpDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDiagnosis"
                ShowLoadingPanel="false" OnCallback="cbpDiagnosis_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisEndCallback(s); }" />
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
