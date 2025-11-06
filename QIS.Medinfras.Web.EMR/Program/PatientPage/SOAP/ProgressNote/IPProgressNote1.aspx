<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="IPProgressNote1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.IPProgressNote1" %>

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
    <li id="btnVerifyAll" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Verify All")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">

    <table>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 125px" />
                        <col style="width: 145px" />
                        <col style="width: 10px" />
                        <col style="width: 145px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td style="text-align:center">
                            <%=GetLabel("s/d") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <%=GetLabel("Jenis Tampilan") %>
            </td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="Catatan Semua PPA" Value="0" Selected="True" />
                    <asp:ListItem Text="Catatan Dokter" Value="1" />
                    <asp:ListItem Text="Catatan Saya" Value="2" />
                    <asp:ListItem Text="Catatan Perawat dan Tenaga Medis Lainnya" Value="3" />
                    <asp:ListItem Text="Yang perlu konfirmasi" Value="4" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript" id="_ipProgressNote1">
        $btnReadback = null;
        var iColIndex = 0;
        var iRowIndex = 0;
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                ////// alert($("#id tr").index(this));
                var index = $('#<%=grdView.ClientID %> tr').index(this);
                $('#<%=hdnLastIndexSelected.ClientID %>').val(index);
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtNoteDate.ClientID %>');
            $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                onRefreshControl();
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                onRefreshControl();
            });

            $('#<%=btnGenerateNote.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/PatientPage/ProgressNotes/GenerateSOAPNotesCtl.ascx");
                openUserControlPopup(url, "", 'Generate SOAP Notes', 800, 600);
            });

            $('#<%=btnVerifyAll.ClientID %>').click(function () {
                if ($('#<%=hdnIsDPJPPhysician.ClientID %>').val() == "0") {
                    displayErrorMessageBox("VERIFIKASI", "Maaf, hanya DPJP Utama yang bisa melakukan verifikasi catatan.");
                }
                else {
                    var message = "Lakukan proses verikasi untuk semua catatan ?";
                    displayConfirmationMessageBox("VERIFIKASI", message, function (result) {
                        if (result) {
                            if ($('#<%=hdnIsUseSignature.ClientID %>').val() == "0") {
                                cbpVerifyAll.PerformCallback('verify');
                            }
                            else {
                                var ppa = $tr.find('.paramedicName').html();
                                var noteDate = $tr.find('.cfNoteDate').html();
                                var noteTime = $tr.find('.noteTime').html();
                                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "5");
                                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl.ascx");
                                openUserControlPopup(url, data, 'Tanda Tangan Digital - Verifikasi DPJP Utama', 400, 500);
                            }
                        }
                    });
                }
            });

            $('#<%=btnCopyNote.ClientID %>').live('click', function () {
                var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND ParamedicID = " + $('#<%=hdnDefaultParamedicID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^004','X011^011') AND SubjectiveText IS NOT NULL";
                openSearchDialog('planningNote', filterExpression, function (value) {
                    $('#<%=hdnPatientVisitNoteID.ClientID %>').val(value);
                    onSearchPatientVisitNote(value);
                });
            });

            function onSearchPatientVisitNote(value) {
                var filterExpression = "ID = " + value;
                Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtSubjectiveText.ClientID %>').val(result.SubjectiveText);
                        $('#<%=txtObjectiveText.ClientID %>').val(result.ObjectiveText);
                        $('#<%=txtAssessmentText.ClientID %>').val(result.AssessmentText);
                        $('#<%=txtPlanningText.ClientID %>').val(result.PlanningText);
                        $('#<%=txtInstructionSummary.ClientID %>').val(result.InstructionText);
                    }
                    else {
                        $('#<%=txtSubjectiveText.ClientID %>').val('');
                        $('#<%=txtObjectiveText.ClientID %>').val('');
                        $('#<%=txtAssessmentText.ClientID %>').val('');
                        $('#<%=txtPlanningText.ClientID %>').val('');
                        $('#<%=txtInstructionSummary.ClientID %>').val('');
                    }
                });
            }

            $('#<%=ddlViewType.ClientID %>').change(function () {
                $('#<%=hdnLastIndexSelected.ClientID %>').val('');
                onPatientListEntryCancelEntryRecord();
                cbpView.PerformCallback('refresh');
            });

            $('.btnView').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/IPNurseInitialAssessmentCtl1.ascx");
                openUserControlPopup(url, visitID, 'Nurse Initial Assessment', 1300, 600);
            });

            //#region Signature
            $('.btnSignature').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "1");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.lblParamedicName').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var signature1 = $tr.find('.signature1').html();
                var signature2 = $tr.find('.signature2').html();
                var signature3 = $tr.find('.signature3').html();
                var signature4 = $tr.find('.signature4').html();
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "1" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });
            //#endregion

            //#region Readback
            $('.btnReadback').live('click', function () {
                $tr = $(this).parent().parent().parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                iRowIndex = $(this).closest("tr").prevAll("tr").length;
                $('#<%=hdnRowIndex.ClientID %>').val(iRowIndex);

                if ($('#<%=hdnIsUseSignature.ClientID %>').val() == "0") {
                    cbpReadback.PerformCallback($('#<%=hdnID.ClientID %>').val());
                }
                else {
                    var ppa = $tr.find('.paramedicName').html();
                    var noteDate = $tr.find('.cfNoteDate').html();
                    var noteTime = $tr.find('.noteTime').html();
                    var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "2");
                    var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl.ascx");
                    openUserControlPopup(url, data, 'Tanda Tangan Digital - Verifikasi DPJP Utama', 400, 500);
                }
            });
            $('.btnNote1').live('click', function () {
                $tr = $(this).parent().parent().parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                iRowIndex = $(this).closest("tr").prevAll("tr").length;
                $('#<%=hdnRowIndex.ClientID %>').val(iRowIndex);

                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var confirmationRemarks = $tr.find('.confirmationRemarks').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "2" + "|" + confirmationRemarks);

                var url = ResolveUrl("~/Libs/Controls/ConfirmWithNoteCtl.ascx");
                openUserControlPopup(url, data, 'Konfirmasi', 400, 300);
            });
            //#endregion

            //#region Verify
            $('.btnVerified').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                iRowIndex = $(this).closest("tr").prevAll("tr").length;
                $('#<%=hdnRowIndex.ClientID %>').val(iRowIndex);

                if ($('#<%=hdnIsUseSignature.ClientID %>').val() == "0") {
                    cbpVerify.PerformCallback($('#<%=hdnID.ClientID %>').val());
                }
                else {
                    var ppa = $tr.find('.paramedicName').html();
                    var noteDate = $tr.find('.cfNoteDate').html();
                    var noteTime = $tr.find('.noteTime').html();
                    var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "4");
                    var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl.ascx");
                    openUserControlPopup(url, data, 'Tanda Tangan Digital - Verifikasi DPJP Utama', 400, 500);
                }
            });
            $('.btnNote2').live('click', function () {
                $tr = $(this).parent().parent().parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                iRowIndex = $(this).closest("tr").prevAll("tr").length;
                $('#<%=hdnRowIndex.ClientID %>').val(iRowIndex);

                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var remarks = $tr.find('.verificationRemarks').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "4" + "|" + remarks);

                var url = ResolveUrl("~/Libs/Controls/ConfirmWithNoteCtl.ascx");
                openUserControlPopup(url, data, 'Verifikasi', 400, 300);
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

            $('#<%=txtInstructionSummary.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            if ($('#<%=hdnIsShowVerifyButton.ClientID %>').val() == "1") {
                $('#<%=btnVerifyAll.ClientID %>').show();
            }
            else {
                $('#<%=btnVerifyAll.ClientID %>').hide();
            }
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
            $('#<%=hdnIsHasNeedConfirmation.ClientID %>').val(s.cpRetval);
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                //////$("#<%=grdView.ClientID %> tr:eq(" + iRowIndex + ")").click();
                    SelectLastIndex();

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

        function onBeforeBasePatientPageListAdd() {
            $('#<%=hdnLastIndexSelected.ClientID %>').val('');
            if ($('#<%=hdnIsAllowAdd.ClientID %>').val() == "0") {
                var messageBody = "Kajian Awal belum terisi atau tidak ada. Mohon dilengkapi terlebih dahulu.";
                displayErrorMessageBox('EM0034', messageBody);
                return false;
            }
            else
                return true;
        }

        function onBeforeEditRecord(entity, errMessage) {
            if (entity.GCPatientNoteType != Constant.SOAPNoteType.INPATIENT_INITIAL_ASSESSMENT) {
                if (entity.ParamedicID != $('#<%=hdnDefaultParamedicID.ClientID %>').val()) {
                    errMessage.text = 'Maaf, tidak diijinkan mengedit catatan user lain.';
                    return false;
                }
                else {
                    if (entity.IsNeedConfirmation == true) {
                        if (entity.IsConfirmed == true) {
                            errMessage.text = 'Maaf, Catatan sudah dikonfirmasi  atau diverifikasi oleh Dokter, tidak bisa diubah lagi.';
                            return false;
                        }
                        else {
                            if (entity.IsVerified == true) {
                                errMessage.text = 'Maaf, Catatan sudah dikonfirmasi atau diverifikasi oleh Dokter, tidak bisa diubah lagi.';
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else {
                errMessage.text = 'Catatan Pengkajian Awal tidak bisa diubah melalui menu ini';
                return false;
            }
            return true;
        }

        function onBeforeDeleteRecord(entity, errMessage) {
            if (entity.GCPatientNoteType != Constant.SOAPNoteType.INPATIENT_INITIAL_ASSESSMENT) {
                if (entity.ParamedicID != $('#<%=hdnDefaultParamedicID.ClientID %>').val()) {
                    errMessage.text = 'Maaf, tidak diijinkan mengedit catatan user lain.';
                    return false;
                }
                else {
                    if (entity.IsNeedConfirmation == true) {
                        if (entity.IsConfirmed == true) {
                            errMessage.text = 'Maaf, Catatan sudah dikonfirmasi atau diverifikasi oleh Dokter, tidak bisa diubah lagi.';
                            return false;
                        }
                        else {
                            if (entity.IsVerified == true) {
                                errMessage.text = 'Maaf, Catatan sudah dikonfirmasi atau diverifikasi oleh Dokter, tidak bisa diubah lagi.';
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else {
                errMessage.text = 'Catatan Pengkajian Awal tidak bisa diubah melalui menu ini';
                return false;
            }
            return true;
        }

        function onAfterSaveRecord(param) {
            $('#<%=hdnIsChanged.ClientID %>').val('0');
            $('#containerPatientPageEntry').hide();
            
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
                $('#<%=txtInstructionSummary.ClientID %>').val(entity.InstructionText);
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
                $('#<%=txtInstructionSummary.ClientID %>').val('');
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
            var clinicalNotes = $('#<%=txtAssessmentText.ClientID %>').val();
            var chiefComplaint = $('#<%=txtSubjectiveText.ClientID %>').val();
            var param = "X001^004|0|" + clinicalNotes + "|" + chiefComplaint;
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
        });

//        $('#lblAddLabOrder2').die('click');
//        $('#lblAddLabOrder2').live('click', function (evt) {
//            var clinicalNotes = $('#<%=txtAssessmentText.ClientID %>').val();
//            var chiefComplaint = $('#<%=txtSubjectiveText.ClientID %>').val();
//            var param = "X001^004|0|" + clinicalNotes + "|" + chiefComplaint;
//            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
//            openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx', param, title, 1200, 600);
//        });

        $('.imgAddLabOrderDt.imgLink').die('click');
        $('.imgAddLabOrderDt.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedLaboratory(this);
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val(selectedObj.TestOrderID);
            var clinicalNotes = $('#<%=txtAssessmentText.ClientID %>').val();
            var chiefComplaint = $('#<%=txtSubjectiveText.ClientID %>').val();
            var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            var param = "X001^004|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + clinicalNotes + "|" + chiefComplaint;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
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
            var clinicalNotes = $('#<%=txtAssessmentText.ClientID %>').val();
            var chiefComplaint = $('#<%=txtSubjectiveText.ClientID %>').val();
            var title = "Imaging Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            var param = "X001^005|0|" + clinicalNotes + "|" + chiefComplaint;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
        });

        $('.imgAddImagingOrderDt.imgLink').die('click');
        $('.imgAddImagingOrderDt.imgLink').live('click', function () {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedImaging(this);
            $('#<%=hdnImagingTestOrderID.ClientID %>').val(selectedObj.TestOrderID);

            var clinicalNotes = $('#<%=txtAssessmentText.ClientID %>').val();
            var chiefComplaint = $('#<%=txtSubjectiveText.ClientID %>').val();
            var title = "Imaging Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
            var param = "X001^005|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val() + "|" + clinicalNotes + "|" + chiefComplaint;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, title, 1200, 600);
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
                displayErrorMessageBox('KONFIRMASI', param[1]);
            }
        }

        function onCbpVerifyEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('VERIFIKASI', param[1]);
            }
        }
        function SelectLastIndex() {
            var lastIndex = $('#<%=hdnLastIndexSelected.ClientID %>').val();
            if (lastIndex == "") {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            } else {
                $('#<%=grdView.ClientID %> tr:eq(' + lastIndex + ')').click();
                $('#<%=grdView.ClientID %> tr:eq(' + lastIndex + ')').focus();
                // Get row position by index
                //////////var ypos = $('#<%=pnlView.ClientID %> tr:eq(' + lastIndex + ')').offset().down;
                var ypos = $('#<%=grdView.ClientID %> tr:eq(' + lastIndex + ')').offset().top;
                // Go to row
                $('#<%=pnlView.ClientID %>').animate({
                    scrollTop: $('#<%=pnlView.ClientID %>').scrollTop() + ypos - 525
                }, 1000);
            }

        }
       
        function onCbpVerifyAllEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('VERIFIKASI', param[1]);
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsHasNeedConfirmation.ClientID %>').val() == '1') {
                if ($('#<%=hdnEM0058.ClientID %>').val() == '0') {
                    var message = "Masih ada catatan yang perlu dikonfirmasi, lanjutkan meninggalkan halaman ini ?";
                    displayConfirmationMessageBox('Catatan Terintegrasi', message, function (result) {
                        if (result) {
                            gotoNextPage();
                        }
                    });
                }
                else {
                    var message = "Masih ada catatan yang perlu dikonfirmasi, Silahkan lakukan konfirmasi dulu sebelum meninggalkan halaman ini.";
                    displayMessageBox('Catatan Terintegrasi', message);
                }
            }
            else {
                gotoNextPage();
            }
        }
        function onBeforeBackToListPage() {
            if ($('#<%=hdnIsHasNeedConfirmation.ClientID %>').val() == '1') {
                if ($('#<%=hdnEM0058.ClientID %>').val() == '0') {
                    var message = "Masih ada catatan yang perlu dikonfirmasi, lanjutkan meninggalkan halaman ini ?";
                    displayConfirmationMessageBox('Catatan Terintegrasi', message, function (result) {
                        if (result) {
                            backToPatientList();
                        }
                    });
                }
                else {
                    var message = "Masih ada catatan yang perlu dikonfirmasi, Silahkan lakukan konfirmasi dulu sebelum meninggalkan halaman ini.";
                    displayMessageBox('Catatan Terintegrasi', message);
                }
            }
            else {
                backToPatientList();
            }
        }

        $('#imgVisitNote.imgLink').live('click', function () {
            $(this).closest('tr').click();
            var id = $('#<%=hdnID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/VisitNotesHistoryCtl.ascx");
            openUserControlPopup(url, id, 'History Catatan Perawat', 900, 500);
        });

       
    </script>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnLastIndexSelected" runat="server" />
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
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server" />
                        </td>
                        <td />
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
                        <td id="tdCopyButton" style="vertical-align:top;">
                            <input type="button" id="btnCopyNote" runat="server" value="Salin SOAP" class="btnCopyNote w3-btn w3-hover-blue"
                                style="width: 100px;background-color: Red; color: White;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                            <label class="lblMandatory">
                                <%=GetLabel("Subjective")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSubjectiveText" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td colspan="3">
                                        <label id="lblObjective" class="lblMandatory" style="font-weight: bold; margin-bottom: 10px">
                                            <%=GetLabel("Objective")%></label>
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
                        <td colspan="3">
                            <asp:TextBox ID="txtObjectiveText" runat="server" Width="100%" TextMode="Multiline"
                                Rows="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory" style="font-weight: bold;">
                                <%=GetLabel("Assessment")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAssessmentText" runat="server" Width="100%" TextMode="Multiline"
                                Rows="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td colspan="3">
                                        <label class="lblMandatory" style="font-weight: bold; margin-bottom: 10px">
                                            <%=GetLabel("Planning")%></label>
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
                        <td colspan="3">
                            <asp:TextBox ID="txtPlanningText" runat="server" Width="100%" TextMode="Multiline"
                                Rows="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblMandatory" style="font-weight: bold;">
                                <%=GetLabel("Instruksi")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInstructionSummary" runat="server" Width="100%" TextMode="Multiline"
                                Rows="5" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top">
                <h4 class="h4collapsed" style="display:none">
                    <%=GetLabel("Instruksi Dokter Spesialis kepada Dokter Jaga")%></h4>
                <div class="containerTblEntryContent containerEntryPanel1">
                    <table id="tblNotesInstruction" runat="server" border="0" cellpadding="1" cellspacing="0"
                        style="width: 100%">
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara pemberian Instruksi")%></label>
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
                                    <%=GetLabel("Catatan Instruksi (CPPT)")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="350px" runat="server"
                                    TextMode="MultiLine" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked="false" />
                                <%:GetLabel("Perlu Konfirmasi")%>
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
<%--                                                                <HeaderTemplate>
                                                                    <div style="text-align: center">
                                                                        <span class="lblLink" id="lblAddLabOrder2"><%= GetLabel("+ Order (Form)")%></span>
                                                                    </div>                                                                            
                                                                </HeaderTemplate>--%>
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
                                                                            <td style="display:none">
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
                                                                            <td style="display:none">
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
                <h4 class="h4collapsed" style="display:none">
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
                                                    <asp:GridView ID="grdInstructionView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                                            <asp:BoundField DataField="PatientInstructionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
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
                                                            <asp:BoundField DataField="cfInstructionDate" HeaderText="Tanggal" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                                            <asp:BoundField DataField="InstructionTime" HeaderText="Jam" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="InstructionGroup" HeaderText="Jenis Instruksi" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Description" HeaderText="Instruksi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("No Data To Display")%>
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
                                </td>
                            </tr>
                        </table>
                    </div>
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
    <input type="hidden" id="hdnPatientInformation" runat="server" />
    <input type="hidden" value="0" id="hdnIsUseSignature" runat="server" />
    <input type="hidden" value="0" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" value="" id="hdnInstructionID" runat="server" />
    <input type="hidden" value="" id="hdnInstructionText" runat="server" />
    <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
    <input type="hidden" runat="server" id="hdnIsShowVerifyButton" value="0" />
    <input type="hidden" value="0" id="hdnIsHasNeedConfirmation" runat="server" />
    <input type="hidden" value="1" id="hdnIsAllowAdd" runat="server" />
    <input type="hidden" value="0" id="hdnEM0058" runat="server" />
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
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                <asp:BoundField DataField="GCPatientNoteType" HeaderStyle-CssClass="controlColumn" ItemStyle-CssClass="controlColumn" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("GCPatientNoteType") %>" bindingfield="GCPatientNoteType" />
                                        <input type="hidden" value="<%#:Eval("NoteDate") %>" bindingfield="NoteDate" />
                                        <input type="hidden" value="<%#:Eval("NoteDateInDatePickerFormat") %>" bindingfield="NoteDateInDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                        <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                        <input type="hidden" value="<%#:Eval("SubjectiveText") %>" bindingfield="SubjectiveText" />
                                        <input type="hidden" value="<%#:Eval("ObjectiveText") %>" bindingfield="ObjectiveText" />
                                        <input type="hidden" value="<%#:Eval("AssessmentText") %>" bindingfield="AssessmentText" />
                                        <input type="hidden" value="<%#:Eval("PlanningText") %>" bindingfield="PlanningText" />
                                        <input type="hidden" value="<%#:Eval("InstructionText") %>" bindingfield="InstructionText" />
                                        <input type="hidden" value="<%#:Eval("GCPhysicianInstructionSource") %>" bindingfield="GCPhysicianInstructionSource" />
                                        <input type="hidden" value="<%#:Eval("LinkedNoteID") %>" bindingfield="LinkedNoteID" />
                                        <input type="hidden" value="<%#:Eval("ConfirmationPhysicianID") %>" bindingfield="ConfirmationPhysicianID" />
                                        <input type="hidden" value="<%#:Eval("IsNeedConfirmation") %>" bindingfield="IsNeedConfirmation" />
                                        <input type="hidden" value="<%#:Eval("IsConfirmed") %>" bindingfield="IsConfirmed" />
                                        <input type="hidden" value="<%#:Eval("IsVerified") %>" bindingfield="IsVerified" />
                                        <input type="hidden" value="<%#:Eval("ConfirmationRemarks") %>" bindingfield="ConfirmationRemarks" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Jam" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:BoundField DataField="cfPPA" HeaderText="PPA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfPPA" />
                                <asp:TemplateField HeaderText="SOAP" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <span style="color: blue; font-style: italic; vertical-align: top">
                                                <%#:Eval("ParamedicName") %>
                                                - <b>
                                                    <%#:Eval("DepartmentID") %>
                                                    (<%#:Eval("ServiceUnitName") %>)
                                                    <%#:Eval("cfParamedicMasterType") %>
                                                  </b>
                                                  <span style="float: right; <%# Eval("IsEdited").ToString() == "False" ? "display:none": "" %>">
                                                            <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                                                alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                                                  </span>
                                           </span>
                                        </div>
                                        <div style="height: 130px; overflow-y: auto; margin-top: 15px;">
                                            <%#Eval("cfNoteSOAP").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                        <div id="divView" runat="server">
                                            <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Lihat Detail Kajian Awal"
                                                style='width: 180px; background-color: Green; color: White;'  />
                                        </div>
                                        <div style="margin-top: 10px; text-align: left">
                                            <table border="0" cellpadding="1" cellspacing="0">
                                               <tr id="divNursingNotesInfo" runat="server" >
                                                    <td>
                                                       <asp:CheckBox ID="chkIsWritten" runat="server" Enabled="false" Checked='<%# Eval("IsWrite")%>' /> TULIS
                                                       <asp:CheckBox ID="chkIsReadback" runat="server" Enabled="false" Checked='<%# Eval("IsReadback")%>'/> BACA
                                                       <asp:CheckBox ID="chkIsConfirmed" runat="server" Enabled="false" Checked='<%# Eval("IsConfirmed")%>'/> KONFIRMASI
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divNoteButton">
                                            <table border="0" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <input class="btnNote1 w3-btn w3-hover-blue" type="button" id="btnNote1" runat="server" value="CONFIRM NOTE"
                                                            style="width: 150px; background-color: Green; color: White;" />
                                                    </td>
                                                    <td class="tdVerifyNote">
                                                        <input class="btnNote2 w3-btn w3-hover-blue" type="button" id="btnNote2" runat="server" value="VERIFY NOTE"
                                                            style="width: 150px; background-color: Green; color: White;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "InstructionText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align: top">
                                            <%#:Eval("cfCreatedDate") %>,
                                        </div>
                                        <div>
                                            <b><label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'>
                                                <%#:Eval("cfCreatedByName") %></label></b>                                            
                                        </div>
                                        <div id="divParamedicSignature" runat="server" style='margin-top: 5px; text-align: left'>
                                            <input type="button" id="btnSignature" runat="server" class="btnSignature" value="Ttd" title="Tanda Tangan"
                                                style='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>' />
                                        </div>
                                        <div>
                                            <img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                cursor: pointer; min-width: 30px; float: left;' title="Using Notification" /></div>
                                            <div id="divReadback" align="center" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %>'>
                                                <br />
                                                <div>
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <input class="btnReadback w3-btn w3-hover-blue" type="button" id="btnReadback" runat="server" value="CONFIRM"
                                                                    style="width: 150px; background-color: Red; color: White;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="divNeedConfirmation" runat="server" style="text-align: left; font-weight: bold">
                                                    <span style='color: red'>Need Confirmation :
                                                        <br />
                                                    </span><span style='color: Blue'>
                                                        <%#:Eval("ConfirmationPhysicianName") %></span>
                                                </div>
                                            </div>
                                            <div id="divConfirmationInfo" runat="server" style="margin-top: 10px; text-align: left">
                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                    <span style='color: red;'>Confirmed : </span>
                                                    <span style='color: Blue;'>
                                                        <%#:Eval("cfConfirmationDateTime") %>, <%#:Eval("ConfirmationPhysicianName") %></span>
                                                        <div id="divConfirmationRemarks">
                                                            <br />
                                                            <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                                readonly><%#: DataBinder.Eval(Container.DataItem, "ConfirmationRemarks") %> </textarea>
                                                        </div>
                                                </div>
                                            </div>
                                            <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                    <span style='color: red;'>Verified :</span>
                                                    <br />
                                                    <span style='color: Blue;'>
                                                        <%#:Eval("cfVerifiedDateTime") %>, <%#:Eval("VerifiedPhysicianName") %></span>
                                                        <div id="divVerificationRemarks">
                                                            <br />
                                                            <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                                readonly><%#: DataBinder.Eval(Container.DataItem, "VerificationRemarks") %> </textarea>
                                                        </div>
                                                </div>
                                            </div>
                                            <div id="divVerified" align="center" style='<%# Eval("IsVerified").ToString() == "True" ? "display:none;": "" %>'>
                                                <br />
                                                <div>
                                                    <input class="btnVerified w3-btn w3-hover-blue" type="button" id="btnVerified" runat="server" value="VERIFY"
                                                        style="width: 150px; background-color: Red; color: White;" />
                                                </div>
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Signature1" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature1" />
                                <asp:BoundField DataField="Signature2" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature2" />
                                <asp:BoundField DataField="Signature3" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature3" />
                                <asp:BoundField DataField="Signature4" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature4" />
                                <asp:BoundField DataField="ConfirmationRemarks" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn confirmationRemarks" />
                                <asp:BoundField DataField="VerificationRemarks" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn verificationRemarks" />
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan terintegrasi untuk periode tanggal yang dipilih")%>
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
