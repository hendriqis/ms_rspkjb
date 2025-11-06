<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="OPProgressNote1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.OPProgressNote1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerateNote" runat="server" crudmode="C" style="display: none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Generate")%></div>
    </li>
    <li id="btnVerifyNote" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Verify All")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table>
        <tr>
            <td>
                <%=GetLabel("Notes View Type") %>
            </td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="All" Value="0" Selected="True" />
                    <asp:ListItem Text="Physician Notes Only" Value="1" />
                    <asp:ListItem Text="Nursing and Other Paramedic Notes Only" Value="2" />
                    <asp:ListItem Text="My Notes Only" Value="3" />
                    <asp:ListItem Text="Need my Readback Only" Value="4" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $btnReadback = null;
        var iColIndex = 0;
        var iRowIndex = 0;
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

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

            setDatePicker('<%=txtNoteDate.ClientID %>');
            $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnGenerateNote.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/PatientPage/ProgressNotes/GenerateSOAPNotesCtl.ascx");
                openUserControlPopup(url, "", 'Generate SOAP Notes', 800, 600);
            });

            $('#<%=btnVerifyNote.ClientID %>').click(function () {
                if ($('#<%=hdnIsDPJPPhysician.ClientID %>').val() == "0") {
                    showToast("ERROR", 'Error Message : ' + "Sorry, you are not DPJP Physician for this patient !");
                }
                else {
                    var message = "Are you sure to verify all unverified patient notes ?";
                    showToastConfirmation(message, function (result) {
                        if (result) cbpVerifyAll.PerformCallback('verify');
                    });
                }
            });

            $('#<%=ddlViewType.ClientID %>').change(function () {
                onPatientListEntryCancelEntryRecord();
                cbpView.PerformCallback('refresh');
            });

            //#region Readback
            $('.btnReadback').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                iRowIndex = $(this).closest("tr").prevAll("tr").length;
                $('#<%=hdnRowIndex.ClientID %>').val(iRowIndex);
                cbpReadback.PerformCallback($('#<%=hdnID.ClientID %>').val());
            });
            //#endregion

            //#region Verify
            $('.btnVerified').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                iRowIndex = $(this).closest("tr").prevAll("tr").length;
                $('#<%=hdnRowIndex.ClientID %>').val(iRowIndex);
                cbpVerify.PerformCallback($('#<%=hdnID.ClientID %>').val());
            });
            //#endregion

            //#region Subjective Template
            $('#btnAddSubjectiveTemplate.imgLink').click(function () {
                if ($('#<%=txtSubjectiveText.ClientID %>').val() != '') {
                    var text = $('#<%=txtSubjectiveText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^03|" + text, "Physician Subjective Template Text", 700, 500);
                }
            });

            $('#<%:txtSubjectiveText.ClientID %>').blur(function () {
                onTxtSubjectiveTextChanged($(this).val());
            });
            //#endregion

            //#region Objective Template
            $('#btnAddObjectiveTemplate.imgLink').click(function () {
                if ($('#<%=txtObjectiveText.ClientID %>').val() != '') {
                    var text = $('#<%=txtObjectiveText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^04|" + text, "Physician Objective Template Text", 700, 500);
                }
            });

            $('#<%:txtObjectiveText.ClientID %>').blur(function () {
                onTxtObjectiveTextChanged($(this).val());
            });
            //#endregion

            //#region Assessment Template
            $('#btnAddAssessmentTemplate.imgLink').click(function () {
                if ($('#<%=txtAssessmentText.ClientID %>').val() != '') {
                    var text = $('#<%=txtAssessmentText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^05|" + text, "Physician Objective Template Text", 700, 500);
                }
            });

            $('#<%:txtAssessmentText.ClientID %>').blur(function () {
                onTxtAssessmentTextChanged($(this).val());
            });
            //#endregion

            //#region Planning Template
            $('#btnAddPlanningTemplate.imgLink').click(function () {
                if ($('#<%=txtPlanningText.ClientID %>').val() != '') {
                    var text = $('#<%=txtPlanningText.ClientID %>').val();
                    openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^06|" + text, "Physician Objective Template Text", 700, 500);
                }
            });

            $('#<%:txtPlanningText.ClientID %>').blur(function () {
                onTxtPlanningTextChanged($(this).val());
            });
            //#endregion

            registerCollapseExpandHandler();

            $('#<%=txtSubjectiveText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtObjectiveText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtAssessmentText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtPlanningText.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
        });

        //#region Before Save
            function onBeforeChangePage(evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Your record is not saved yet, Do you want to save?";
                    showToastConfirmation(message, function (result) {
                        if (result) {
                            onSaveAddRecord(evt);
                        }
                        else {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                        }
                    });
                }
                else {
                    gotoNextPage();
                }
            }

            function onBeforeBackToListPage() {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    backToPatientList();
                }
            }

            function PromptUserToSave(evt) {
                var message = "Your record is not saved yet, Do you want to save?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        onSaveAddRecord(evt);
                    }
                    else {
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
        //#endregion 

        //#region Subjective
        function onTxtSubjectiveTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtSubjectiveText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di subjective dengan teks dari template ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtSubjectiveText.ClientID %>').val(obj.TemplateText);
                                    $('#<%=txtObjectiveText.ClientID %>').focus();
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region Objective
        function onTxtObjectiveTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtObjectiveText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di objective dengan teks dari template ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtObjectiveText.ClientID %>').val(obj.TemplateText);
                                    $('#<%=txtAssessmentText.ClientID %>').focus();
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region Assessment
        function onTxtAssessmentTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtAssessmentText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di assessment dengan teks dari template ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtAssessmentText.ClientID %>').val(obj.TemplateText);
                                    $('#<%=txtPlanningText.ClientID %>').focus();
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        //#region Planning
        function onTxtPlanningTextChanged(value) {
            if (value.length <= 6 && value.slice(-1) == ";") {
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtPlanningText.ClientID %>').val() != '') {
                            var message = "Ganti catatan di Planning dengan teks dari template ?";
                            showToastConfirmation(message, function (result) {
                                if (result) {
                                    $('#<%=txtPlanningText.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }
        //#endregion

        $('#lblObjective').die('click');
        $('#lblObjective').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSign/VitalSignLookupCtl1.ascx", "0|", "Vital Sign & Indicator", 700, 500);
        });

        function onCboPhysicianInstructionSourceChanged(s) {
            if (s.GetValue() != null && s.GetValue().indexOf('^01') > -1) {
                $('#lblPhysicianNoteID').addClass('lblLink');
            }
            else {
                $('#lblPhysicianNoteID').removeClass('lblLink');
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        }

        $('#lblPhysicianNoteID.lblLink').live('click', function () {
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^010','X011^011') AND IsEdited = 0";
            openSearchDialog('planningNote', filterExpression, function (value) {
                onTxtPlanningNoteChanged(value);
            });
        });

        function onTxtPlanningNoteChanged(value) {
            var filterExpression = "ID = " + value;
            Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                    $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
                }
                else {
                    $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                    $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
                }
            });
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            alert('onAfterSaveRecordPatientPageEntry');
            cbpView.PerformCallback('refresh');
        }

        function onCboPhysicianChanged(s) {
            if (s.GetValue() != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
            }
            else
                $('#<%=hdnParamedicID.ClientID %>').val('');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (iRowIndex == 0) 
                    iRowIndex = 1;
    
                if (pageCount > 0)
                    $("#<%=grdView.ClientID %> tr:eq(" + iRowIndex + ")").click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        function onBeforeEditRecord(entity, errMessage) {
            if (entity.ParamedicID != $('#<%=hdnDefaultParamedicID.ClientID %>').val()) {
                errMessage.text = 'This record is belong to another physician, you are not allowed to edit or delete it';
                return false;
            }
            else {
                if (entity.IsNeedConfirmation == true) {
                    if (entity.IsConfirmed == true) {
                        errMessage.text = 'This notes is already confirmed by Other Physician, you are not allowed to edit or delete it';
                        return false;
                    }
                    else 
                    {
                        if (entity.IsVerified == true) {
                            errMessage.text = 'This notes is already verified by Other Physician, you are not allowed to edit or delete it';
                            return false;                            
                        }
                    }
                }
            }
            return true;
        }

        function onBeforeDeleteRecord(entity, errMessage) {
            if (entity.ParamedicID != $('#<%=hdnDefaultParamedicID.ClientID %>').val()) {
                errMessage.text = 'This record is belong to another physician, you are not allowed to edit or delete it';
                return false;
            }
            else {
                if (entity.IsNeedConfirmation == true) {
                    if (entity.IsConfirmed == true) {
                        errMessage.text = 'This notes is already confirmed by Other Physician, you are not allowed to edit or delete it';
                        return false;
                    }
                    else 
                    {
                        if (entity.IsVerified == true) {
                            errMessage.text = 'This notes is already verified by Other Physician, you are not allowed to edit or delete it';
                            return false;                            
                        }
                    }
                }
            }
            return true;
        }        

        function onAfterSaveRecord(param) {
            $('#<%=hdnIsChanged.ClientID %>').val('0');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cboPhysician.SetValue(entity.ParamedicID);
                $('#<%=txtNoteDate.ClientID %>').val(entity.NoteDateInDatePickerFormat);
                $('#<%=txtNoteTime.ClientID %>').val(entity.NoteTime);
                $('#<%=txtSubjectiveText.ClientID %>').val(entity.SubjectiveText);
                $('#<%=txtObjectiveText.ClientID %>').val(entity.ObjectiveText);
                $('#<%=txtAssessmentText.ClientID %>').val(entity.AssessmentText);
                $('#<%=txtPlanningText.ClientID %>').val(entity.PlanningText);
                cboPhysicianInstructionSource.SetValue(entity.GCPhysicianInstructionSource);
                $('#<%=hdnPlanningNoteID.ClientID %>').val(entity.LinkedNoteID);
                $('#<%=chkIsNeedConfirmation.ClientID %>').prop('checked', (entity.IsNeedConfirmation == 'True'));
                $('#<%=hdnParamedicID.ClientID %>').val(entity.ConfirmationPhysicianID);
                cboSpecialistPhysician.SetValue(entity.ConfirmationPhysicianID);
            }
            else {
                var currentdate = getDateNowDatePickerFormat();
                var currenttime = getTimeNow();
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboPhysician.SetValue($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtNoteDate.ClientID %>').val(currentdate);
                $('#<%=txtNoteTime.ClientID %>').val(currenttime);
                $('#<%=txtSubjectiveText.ClientID %>').val('');
                $('#<%=txtObjectiveText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val('');
                $('#<%=txtPlanningText.ClientID %>').val('');
                cboPhysicianInstructionSource.SetValue('');
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=chkIsNeedConfirmation.ClientID %>').prop('checked', false);
                cboSpecialistPhysician.SetValue('');
            }
            $('#<%=txtSubjectiveText.ClientID %>').focus();
        }
        //#endregion

        //#region Vital Sign Paging
        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "0|", "Vital Sign & Indicator", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
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
            }
            else {
                showToast("ERROR", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
        }

        function onAfterAddVitalSign(param) {
            if (param != '') {
                var newText = param;
                if ($('#<%=txtObjectiveText.ClientID %>').val() != "") {
                    newText = $('#<%=txtObjectiveText.ClientID %>').val() + "\n" + newText;
                }
                $('#<%=txtObjectiveText.ClientID %>').val(newText);
            }
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

        function onAfterAddReviewOfSystem(param) {
            if (param != '') {
                var newText = param;
                if ($('#<%=txtObjectiveText.ClientID %>').val() != "") {
                    newText = $('#<%=txtObjectiveText.ClientID %>').val() + "\n" + newText;
                }
                $('#<%=txtObjectiveText.ClientID %>').val(newText);
            }
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

            var param = "LB|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
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

        var pageCount = parseInt('<%=gridLaboratoryPageCount %>');
        $(function () {
            setPaging($("#laboratoryPaging"), pageCount, function (page) {
                cbpLaboratoryView.PerformCallback('changepage|' + page);
            });
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

        function onAfterAddLabTestOrder(param) {
            if (param != '') {
                var item = param.split(";").join("\n");
                var newText = item;
                if ($('#<%=txtPlanningText.ClientID %>').val() != "") {
                    newText = $('#<%=txtPlanningText.ClientID %>').val() + "\n" + item;
                }
                $('#<%=txtPlanningText.ClientID %>').val(newText);
            }
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

        function onAfterAddImagingTestOrder(param) {
            if (param != '') {
                var item = param.split(";").join("\n");
                var newText = item;
                if ($('#<%=txtPlanningText.ClientID %>').val() != "") {
                    newText = $('#<%=txtPlanningText.ClientID %>').val() + "\n" + item;
                }
                $('#<%=txtPlanningText.ClientID %>').val(newText);
            }
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

        function onCbpReadbackEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                showToast('READBACK : FAILED', 'Error Message : ' + param[1]);
            }
        }

        function onCbpVerifyEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                showToast('VERIFY : FAILED', 'Error Message : ' + param[1]);
            }
        }

        function onCbpVerifyAllEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpView.PerformCallback('refresh');
            }
            else {
                showToast('VERIFY : FAILED', 'Error Message : ' + param[1]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table class="tblEntryDetail">
        <colgroup>
            <col style="width: 55%" />
            <col style="width: 45%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Date")%>
                                -
                                <%=GetLabel("Time")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory">
                                <%=GetLabel("Physician")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician"
                                Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="margin-bottom: 10px; vertical-align: top">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td class="tdLabel" valign="top" style="width:120px">
                                        <label id="lblSubjective" class="lblMandatory lblLink">
                                            <%=GetLabel("Subjective")%></label>
                                    </td>
                                    <td><img class="imgLink" id="btnAddSubjectiveTemplate" title='<%=GetLabel("Add to My Subjective Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSubjectiveText" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="6" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label id="lblObjective" class="lblMandatory lblLink">
                                                        <%=GetLabel("Objective")%></label>
                                                </td>
                                                <td><img class="imgLink" id="btnAddObjectiveTemplate" title='<%=GetLabel("Add to My Objective Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50px; display: none">
                                        <label class="lblNormal">
                                            <%=GetLabel("TTV")%></label>
                                    </td>
                                    <td style="display: none">
                                        <img class="imgLink" id="btnLookupVitalSign" title='<%=GetLabel("Select")%>' src='<%= ResolveUrl("~/Libs/Images/button/search.png")%>'
                                            alt="" />
                                    </td>
                                    <td style="display: none">
                                        <img class="imgLink" id="btnAddVitalSign" title='<%=GetLabel("Add")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>'
                                            alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50px; display: none">
                                        <label class="lblNormal">
                                            <%=GetLabel("ROS")%></label>
                                    </td>
                                    <td style="display: none">
                                        <img class="btnAddROS imgLink" title='<%=GetLabel("Add")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                            alt="" />
                                    </td>
                                    <td style="display: none">
                                        <img class="btnLookupROS imgLink" title='<%=GetLabel("Select")%>' src='<%= ResolveUrl("~/Libs/Images/button/search.png")%>'
                                            alt="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtObjectiveText" runat="server" Width="100%" TextMode="Multiline"
                                Rows="6" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td class="tdLabel" valign="top" style="width:120px">
                                        <label id="lblAssessment" class="lblMandatory lblLink">
                                            <%=GetLabel("Assessment")%></label>
                                    </td>
                                    <td><img class="imgLink" id="btnAddAssessmentTemplate" title='<%=GetLabel("Add to My Assessment Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAssessmentText" runat="server" Width="100%" TextMode="Multiline"
                                Rows="6" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width:120px">
                                                    <label id="Label1" class="lblMandatory lblLink">
                                                        <%=GetLabel("Planning")%></label>
                                                </td>
                                                <td style="display:none"><img class="imgLink" id="btnAddPlanningTemplate" title='<%=GetLabel("Add to My Planning Template")%>' src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td style="width: 50px">
                                        <label class="lblNormal">
                                            <%=GetLabel("LAB")%></label>
                                    </td>
                                    <td>
                                        <img class="btnAddLabOrder imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                            alt="" />
                                    </td>
                                    <td>
                                        <img class="btnLookupLabOrder imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                            alt="" />
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td style="width: 50px">
                                        <label class="lblNormal">
                                            <%=GetLabel("IMG")%></label>
                                    </td>
                                    <td>
                                        <img class="btnAddImagingOrder imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                            alt="" />
                                    </td>
                                    <td>
                                        <img class="btnLookupImagingOrder imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                            alt="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPlanningText" runat="server" Width="100%" TextMode="Multiline"
                                Rows="6" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top">
                <h4 class="h4collapsed" id="physicianNotesH4" runat="server" style="display:none">
                    <%=GetLabel("Instruksi dari Dokter Spesialis")%></h4>
                <div class="containerTblEntryContent containerEntryPanel1" style="display:none">
                    <table id="tblNotesInstruction" runat="server" border="0" cellpadding="1" cellspacing="0"
                        style="width: 100%">
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Instruksi dari Dokter Spesialis")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                                    runat="server" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }"
                                        Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblLink" id="lblPhysicianNoteID">
                                    <%=GetLabel("Catatan Instruksi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="350px" runat="server"
                                    TextMode="MultiLine" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked="false" />
                                <%:GetLabel("Perlu Konfirmasi/Readback")%>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboSpecialistPhysician" ClientInstanceName="cboSpecialistPhysician"
                                    runat="server" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }"
                                        Init="function(s,e){ onCboPhysicianChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4collapsed" style="display:none">
                    <%=GetLabel("Pemeriksaan Tanda Vital")%></h4>
                <div class="containerTblEntryContent" style="display:none">
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
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
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
                <h4 class="h4collapsed" style="display:none">
                    <%=GetLabel("Pemeriksaan Fisik")%></h4>
                <div class="containerTblEntryContent" style="display:none">
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
                                    <div class="imgLoadingGrdView" id="Div1">
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
                <h4 class="h4collapsed" style="display:none">
                    <%=GetLabel("Pemeriksaan : Laboratorium")%></h4>
                <div class="containerTblEntryContent containerEntryPanel1" style="display:none">
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
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
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
                <h4 class="h4collapsed" style="display:none">
                    <%=GetLabel("Pemeriksaan : Radiologi")%></h4>
                <div class="containerTblEntryContent containerEntryPanel1" style="display:none">
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
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
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
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
    <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnImagingTestOrderID" runat="server" />
    <input type="hidden" value="0" id="hdnIsDPJPPhysician" runat="server" />
    <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 600px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("NoteDate") %>" bindingfield="NoteDate" />
                                        <input type="hidden" value="<%#:Eval("NoteDateInDatePickerFormat") %>" bindingfield="NoteDateInDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                        <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                        <input type="hidden" value="<%#:Eval("SubjectiveText") %>" bindingfield="SubjectiveText" />
                                        <input type="hidden" value="<%#:Eval("ObjectiveText") %>" bindingfield="ObjectiveText" />
                                        <input type="hidden" value="<%#:Eval("AssessmentText") %>" bindingfield="AssessmentText" />
                                        <input type="hidden" value="<%#:Eval("PlanningText") %>" bindingfield="PlanningText" />
                                        <input type="hidden" value="<%#:Eval("GCPhysicianInstructionSource") %>" bindingfield="GCPhysicianInstructionSource" />
                                        <input type="hidden" value="<%#:Eval("LinkedNoteID") %>" bindingfield="LinkedNoteID" />
                                        <input type="hidden" value="<%#:Eval("ConfirmationPhysicianID") %>" bindingfield="ConfirmationPhysicianID" />
                                        <input type="hidden" value="<%#:Eval("IsNeedConfirmation") %>" bindingfield="IsNeedConfirmation" />
                                        <input type="hidden" value="<%#:Eval("IsConfirmed") %>" bindingfield="IsConfirmed" />
                                        <input type="hidden" value="<%#:Eval("IsVerified") %>" bindingfield="IsVerified" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Date" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Time" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfPPA" HeaderText="PPA" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="SOAP" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <span style="color: blue; font-style: italic">
                                                <%#:Eval("ParamedicName") %>
                                            </span>: <span style="font-style: italic">
                                                <%#:Eval("cfLastUpdatedDate") %></span>
                                        </div>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align: top">
                                            <%#:Eval("cfLastUpdatedDate") %>,
                                        </div>
                                        <div>
                                            <b>
                                                <%#:Eval("cfLastUpdatedByName") %></b>
                                        </div>
                                        <div>
                                            <img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                cursor: pointer; min-width: 30px; float: left;' title="Using Notification" /></div>
                                        <div id="divReadback" align="center" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %>'>
                                            <br />
                                            <div>
                                                <input type="button" id="btnReadback" runat="server" class="btnReadback" value="READBACK"
                                                    style="height: 30px; width: 100px; background-color: Red; color: White;" /></div>
                                            <div id="divNeedConfirmation" runat="server" style="text-align: left; font-weight: bold">
                                                <span style='color: red'>Need Readback :
                                                    <br />
                                                </span><span style='color: Blue'>
                                                    <%#:Eval("ConfirmationPhysicianName") %></span>
                                            </div>
                                        </div>
                                        <div id="divVerified" align="center" style='<%# Eval("IsVerified").ToString() == "True" ? "display:none;": "" %>'>
                                            <br />
                                            <div>
                                                <input type="button" id="btnVerified" runat="server" class="btnVerified" value="VERIFY"
                                                    style="height: 30px; width: 100px; background-color: Red; color: White;" />
                                            </div>
                                        </div>
                                        <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                            <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                <span style='color: red;'>Verified :</span>
                                                <br />
                                                <span style='color: Blue;'>
                                                    <%#:Eval("cfVerifiedDateTime") %>
                                                    <br />
                                                    <%#:Eval("VerifiedPhysicianName") %></span>
                                            </div>
                                        </div>
                                        <div id="divConfirmationInfo" runat="server" style="margin-top: 10px; text-align: left">
                                            <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                <span style='color: red;'>Readback :</span>
                                                <br />
                                                <span style='color: Blue;'>
                                                    <%#:Eval("cfConfirmationDateTime") %>
                                                    <br />
                                                    <%#:Eval("ConfirmationPhysicianName") %></span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Integration Notes to display")%>
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
                <div id="paging">
                </div>
            </div>
        </div>
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
        <dxcp:ASPxCallbackPanel ID="cbpDeleteTestOrder" runat="server" Width="100%" ClientInstanceName="cbpDeleteTestOrder"
            ShowLoadingPanel="false" OnCallback="cbpDeleteTestOrder_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteTestOrderEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
        ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel ID="cbpReadback" runat="server" Width="100%" ClientInstanceName="cbpReadback"
        ShowLoadingPanel="false" OnCallback="cbpReadback_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpReadbackEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel ID="cbpVerify" runat="server" Width="100%" ClientInstanceName="cbpVerify"
        ShowLoadingPanel="false" OnCallback="cbpVerify_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpVerifyEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel ID="cbpVerifyAll" runat="server" Width="100%" ClientInstanceName="cbpVerifyAll"
        ShowLoadingPanel="false" OnCallback="cbpVerifyAll_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpVerifyAllEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
