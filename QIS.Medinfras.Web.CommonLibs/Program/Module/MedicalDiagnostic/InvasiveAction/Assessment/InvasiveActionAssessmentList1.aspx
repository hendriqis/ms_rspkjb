<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="InvasiveActionAssessmentList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InvasiveActionAssessmentList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
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
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_InvasiveActionAssessmentList">
        $(function () {
            $('#leftPanel ul li').click(function () {
                $('#leftPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                showContent(contentID);
            });

            $('#<%=btnRefresh.ClientID %>').click(function (evt) {
                onRefreshControl();
            });

            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnLinkedID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.TransactionNo').html());
                $('#<%=hdnItemName.ClientID %>').val($(this).find('.itemName').html());
                $('#<%=hdnChargesParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                $('#<%=hdnChargesParamedicName.ClientID %>').val($(this).find('.paramedicName').html());

                if ($('#<%=hdnLinkedID.ClientID %>').val() != "") {
                    cbpViewDt.PerformCallback('refresh');
                    setTimeout(function () { cbpViewDt2.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt3.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt4.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt5.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt6.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt8.PerformCallback('refresh'); }, 1000);
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Asesmen Pra Bedah
            $('#lblAddAsessment').die('click');
            $('#lblAddAsessment').live('click', function (evt) {
                var allow = true;
                if (typeof onBeforeBasePatientPageListAdd == 'function') {
                    allow = onBeforeBasePatientPageListAdd();
                }
                if (allow) {
                    $('#<%=hdnSelectedTab.ClientID %>').val('preSurgeryAssessment');
                    cbpMPListProcess.PerformCallback('add');
                }
            });
            $('.imgViewPreSurgeryAssessment.imgLink').die('click');
            $('.imgViewPreSurgeryAssessment.imgLink').live('click', function () {
                var url = ResolveUrl("~/libs/Controls/EMR/ProcedureReporting/ViewPreProcedureAssesmentCtl1.ascx");
                var recordID = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var linkedID = $(this).attr('linkedID');
                var transactionNo = $('#<%=hdnTransactionNo.ClientID %>').val();
                var itemName = $('#<%=hdnItemName.ClientID %>').val();
                var param = visitID + '|' + linkedID + '|' + recordID + '|' + transactionNo + '|' + itemName;
                openUserControlPopup(url, param, "Asesmen Pra Tindakan", 1024, 500);
            });
            //#endregion

            //#region Asesmen Pra Anestesi dan Pra Sedasi
            $('#lblAddAnesthesy').die('click');
            $('#lblAddAnesthesy').live('click', function (evt) {
                cbpMPListProcess.PerformCallback('add');
            });
            $('.imgViewPreAnesthesyAssessment.imgLink').die('click');
            $('.imgViewPreAnesthesyAssessment.imgLink').live('click', function () {
                var url = ResolveUrl("~/libs/Controls/EMR/ProcedureReporting/ViewProcedurePreAnesthesyAssesmentCtl1.ascx");
                var recordID = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var linkedID = $(this).attr('linkedID');
                var transactionNo = $('#<%=hdnTransactionNo.ClientID %>').val();
                var itemName = $('#<%=hdnItemName.ClientID %>').val();
                var param = visitID + '|' + linkedID + '|' + recordID + '|' + transactionNo + '|' + itemName;
                openUserControlPopup(url, param, "Asesmen Pra Anestesi", 1024, 500);
            });
            //#endregion

            //#region Asesmen Status Anestesi
            $('#lblAddAnesthesyStatus').die('click');
            $('#lblAddAnesthesyStatus').live('click', function (evt) {
                if ($('#<%=hdnInputSurgeryAssessmentFirst.ClientID %>').val() == "1") {
                    if ($('#<%=hdnIsSurgeryPreAssessmentExists.ClientID %>').val() == "1")
                        cbpMPListProcess.PerformCallback('add');
                    else
                        displayErrorMessageBox('Asesmen Status Anestesi', 'Maaf, Asesmen Status Anestesi hanya bisa dilakukan jika sudah ada Asesmen Pra Bedah');
                }
                else {
                    cbpMPListProcess.PerformCallback('add');
                }
            });
            $('.imgViewAnesthesyStatus.imgLink').die('click');
            $('.imgViewAnesthesyStatus.imgLink').live('click', function () {
                var url = ResolveUrl("~/libs/Controls/EMR/ProcedureReporting/ViewProcedureAnesthesyStatusCtl1.ascx");
                var recordID = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var linkedID = $(this).attr('linkedID');
                var transactionNo = $('#<%=hdnTransactionNo.ClientID %>').val();
                var itemName = $('#<%=hdnItemName.ClientID %>').val();
                var param = visitID + '|' + linkedID + '|' + recordID + '|' + transactionNo + '|' + itemName;
                openUserControlPopup(url, param, "Status Anestesi", 1024, 500);
            });
            //#endregion

            //#region Asesmen Status Anestesi
            $('.imgAddIntraVitalSign.imgLink').die('click');
            $('.imgAddIntraVitalSign.imgLink').live('click', function (evt) {
                addIntraVitalSign();
            });

            $('#lblAddIntraVitalSign').die('click');
            $('#lblAddIntraVitalSign').live('click', function (evt) {
                addIntraVitalSign();
            });

            function addIntraVitalSign() {
                var allow = true;
                if (allow) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                    var param = "06" + "||" + "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "||" + $('#<%=hdnLinkedID.ClientID %>').val(); // hdnLinkedID = TestOrderID
                    openUserControlPopup(url, param, "Pemantauan Status Fungsiologis", 700, 500, "X487^002");
                }
            }

            $('.imgEditAnesthesyVital.imgLink').die('click');
            $('.imgEditAnesthesyVital.imgLink').live('click', function () {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                    var param = "06" + "||" + recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "||" + $('#<%=hdnLinkedID.ClientID %>').val(); // hdnLinkedID = TestOrderID
                    openUserControlPopup(url, param, "Pemantauan Status Fungsiologis", 700, 500, "X487^002");
                }
            });

            $('.imgDeleteAnesthesyVital.imgLink').die('click');
            $('.imgDeleteAnesthesyVital.imgLink').live('click', function () {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var message = "Hapus informasi pemantauan status fisiologis untuk pasien ini ?";
                    displayConfirmationMessageBox("Status Anestesi", message, function (result) {
                        if (result) {
                            var param = "01|" + recordID;
                            cbpDeleteIntraAnesthesy.PerformCallback(param);
                        }
                    });
                }
            });

            $('.imgCopyAnesthesyVital.imgLink').die('click');
            $('.imgCopyAnesthesyVital').live('click', function () {
                var recordID = $(this).attr('recordID');
                var message = "Lakukan copy pemantauan status fisiologi ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var param = "06" + "||" + recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "||" + $('#<%=hdnLinkedID.ClientID %>').val(); // hdnLinkedID = TestOrderID
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/CopyVitalSignCtl1.ascx");
                        openUserControlPopup(url, param, 'Copy Pemantauan Status Fungsiologis', 700, 500, "X487^002");
                    }
                });
            });

            $('.imgAddIntraMedication.imgLink').die('click');
            $('.imgAddIntraMedication.imgLink').live('click', function (evt) {
                addIntraMedication();
            });

            $('#lblAddIntraMedication').die('click');
            $('#lblAddIntraMedication').live('click', function (evt) {
                addIntraMedication();
            });

            function addIntraMedication() {
                var allow = true;
                if (allow) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Surgery/AddAnesthesyMedicationCtl1.ascx");
                    var param = "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnLinkedID.ClientID %>').val(); // hdnLinkedID = TestOrderID
                    openUserControlPopup(url, param, "Status Anestesi - Maintenance", 700, 500, "X487^002");
                }
            }

            $('.imgEditIntraMedication.imgLink').die('click');
            $('.imgEditIntraMedication.imgLink').live('click', function () {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Surgery/AddAnesthesyMedicationCtl1.ascx");
                    var param = recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnLinkedID.ClientID %>').val(); // hdnLinkedID = TestOrderID
                    openUserControlPopup(url, param, "Status Anestesi - Maintenance", 700, 500, "X487^002");
                }
            });

            $('.imgDeleteIntraMedication.imgLink').die('click');
            $('.imgDeleteIntraMedication.imgLink').live('click', function () {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var message = "Hapus informasi administrasi pemberian obat (maintenance anesthesy) untuk pasien ini ?";
                    displayConfirmationMessageBox("Status Anestesi", message, function (result) {
                        if (result) {
                            var param = "02|" + recordID;
                            cbpDeleteIntraAnesthesy.PerformCallback(param);
                        }
                    });
                }
            });


            $('.imgAddIntake.imgLink').die('click');
            $('.imgAddIntake.imgLink').live('click', function (evt) {
                var allow = true;
                if (typeof onBeforeBasePatientPageListAdd == 'function') {
                    allow = onBeforeBasePatientPageListAdd();
                }
                if (allow) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceIntakeEntry.ascx");
                    var param = "05|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|0|1|0";
                    openUserControlPopup(url, param, "Pemantauan Intake", 700, 500, "X487^001");
                }
            });

            $('.imgCopyIntakeOutput.imgLink').die('click');
            $('.imgCopyIntakeOutput.imgLink').live('click', function (evt) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceIntakeEntry.ascx");
                var recordID = $(this).attr('recordID');
                var fluidGroup = $(this).attr('fluidGroup');
                var logTime = $(this).attr('logTime');
                var param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|1|1";
                var title = "Pemantauan Intake";
                if (fluidGroup == "X459^02") {
                    url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceOutputEntry.ascx");
                    param = "05|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|3|1";
                    title = "Pemantauan Output";
                }
                openUserControlPopup(url, param, title, 700, 500, "X487^001");
            });

            $('.imgEditIntakeOutput.imgLink').die('click');
            $('.imgEditIntakeOutput.imgLink').live('click', function (evt) {
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).attr('createdBy'));
                if (onBeforeEdit()) {
                    var recordID = $(this).attr('recordID');
                    var fluidGroup = $(this).attr('fluidGroup');
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceIntakeEntry.ascx");
                    var param = "05|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|1|0";
                    title = "Pemantauan Intake";
                    if (fluidGroup == "X459^02") {
                        url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceOutputEntry.ascx");
                        param = "05|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|3|0";
                        title = "Pemantauan Output";
                    }
                    openUserControlPopup(url, param, title, 700, 500, "X487^001");
                }
            });

            $('.imgDeleteIntakeOutput.imgLink').die('click');
            $('.imgDeleteIntakeOutput.imgLink').live('click', function () {
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).attr('createdBy'));
                if (onBeforeEdit()) {
                    var recordID = $(this).attr('recordID');
                    var message = "Hapus informasi pemantauan Intake Output untuk pasien ini ?";
                    displayConfirmationMessageBox("Pemantauan - Intake/Output", message, function (result) {
                        if (result) {
                            cbpDeleteIntraFluidBalance.PerformCallback(recordID);
                        }
                    });
                }
            });

            $('.imgAddOutput.imgLink').die('click');
            $('.imgAddOutput.imgLink').live('click', function (evt) {
                var allow = true;
                if (typeof onBeforeBasePatientPageListAdd == 'function') {
                    allow = onBeforeBasePatientPageListAdd();
                }
                if (allow) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceOutputEntry.ascx");
                    var param = "05|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|0|3|0";
                    openUserControlPopup(url, param, "Pemantauan Output", 700, 500, "X487^001");
                }
            });
            //#endregion

            //#region Instruksi Pasca Bedah
            $('.imgAddInstruction.imgLink').die('click');
            $('.imgAddInstruction.imgLink').live('click', function (evt) {
                addInstruction();
            });

            $('#lblAddInstruction').die('click');
            $('#lblAddInstruction').live('click', function (evt) {
                addInstruction();
            });

            function addInstruction() {
                var allow = true;
                if (allow) {
                    if ($('#<%=hdnInputSurgeryAssessmentFirst.ClientID %>').val() == "1") {
                        if ($('#<%=hdnIsSurgeryPreAssessmentExists.ClientID %>').val() == "1")
                            cbpMPListProcess.PerformCallback('add');
                        else
                            displayErrorMessageBox('Instruksi Paska Bedah', 'Maaf, Instruksi Paska Bedah hanya bisa dilakukan jika sudah ada Asesmen Pra Bedah');
                    }
                    else {
                        cbpMPListProcess.PerformCallback('add');
                    }
                }
            }

            $('.imgEditInstruction.imgLink').die('click');
            $('.imgEditInstruction.imgLink').live('click', function () {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
                $('#<%=hdnPostSurgeryInstructionID.ClientID %>').val(recordID);
                var allow = true;
                if (typeof onBeforeBasePatientPageListEdit == 'function') {
                    allow = onBeforeBasePatientPageListEdit();
                }
                if (allow) cbpMPListProcess.PerformCallback('edit');
            });

            $('.imgDeleteInstruction.imgLink').die('click');
            $('.imgDeleteInstruction.imgLink').live('click', function () {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                $('#<%=hdnPostSurgeryInstructionID.ClientID %>').val(recordID);
                $('#<%=hdnSelectedTab.ClientID %>').val('postSurgeryInstruction');
                var message = "Hapus instruksi paska bedah terintegrasi untuk pasien ini ?";
                displayConfirmationMessageBox("Instruksi Paska bedah terintegrasi", message, function (result) {
                    if (result) {
                        cbpMPListProcess.PerformCallback('delete');
                    }
                });
            });

            //#region Paska Bedah Terintegrasi
            $('.imgViewInstruction.imgLink').die('click');
            $('.imgViewInstruction.imgLink').live('click', function () {
                var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPostSurgeryInstructionCtl1.ascx");
                var recordID = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var testorderID = $(this).attr('testorderID');
                var param = visitID + '|' + testorderID + '|' + recordID;
                openUserControlPopup(url, param, "Instruksi Paska Bedah", 1024, 500);
            });
            //#endregion
            //#endregion

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                if ($contentID == "anesthesyStatus") {
                    $('#leftPanel ul li').first().click();
                }
            });
            //#endregion

            registerCollapseExpandHandler();
        });

        $('.imgEditAssessment.imgLink').die('click');
        $('.imgEditAssessment.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteAssessment.imgLink').die('click');
        $('.imgDeleteAssessment.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) {
                var assessmentID = $(this).attr('assessmentID');
                $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
                $('#<%=hdnSelectedTab.ClientID %>').val('preSurgeryAssessment');
                var message = "Hapus assesmen pra bedah untuk pasien ini ?";
                displayConfirmationMessageBox("Asesmen Pra Bedah", message, function (result) {
                    if (result) {
                        cbpMPListProcess.PerformCallback('delete');
                    }
                });
            }
        });

        $('.imgEditAnesthesy.imgLink').die('click');
        $('.imgEditAnesthesy.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            var recordID = $(this).attr('recordID');
            $('#<%=hdnAssessmentID.ClientID %>').val(recordID);
            $('#<%=hdnSelectedTab.ClientID %>').val('preSurgeryAnesthesy');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteAnesthesy.imgLink').die('click');
        $('.imgDeleteAnesthesy.imgLink').live('click', function () {
            var assessmentID = $(this).attr('assessmentID');
            $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
            $('#<%=hdnSelectedTab.ClientID %>').val('preSurgeryAnesthesy');
            var message = "Hapus assesmen pra anestesi untuk pasien ini ?";
            displayConfirmationMessageBox("Asesmen Pra Anestesi", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });

        $('.imgEditAnesthesyStatus.imgLink').die('click');
        $('.imgEditAnesthesyStatus.imgLink').live('click', function () {
            var assessmentID = $(this).attr('assessmentID');
            $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
            var paramedicID = $(this).attr('paramedicID');
            $('#<%=hdnSelectedTab.ClientID %>').val('anesthesyStatus');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteAnesthesyStatus.imgLink').die('click');
        $('.imgDeleteAnesthesyStatus.imgLink').live('click', function () {
            var assessmentID = $(this).attr('assessmentID');
            $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
            $('#<%=hdnSelectedTab.ClientID %>').val('anesthesyStatus');
            var message = "Hapus status anestesi untuk pasien ini ?";
            displayConfirmationMessageBox("Status Anestesi", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });

        //#region Surgery Safety Check List
        $('.imgDeleteSurgicalSafetyCheck.imgLink').die('click');
        $('.imgDeleteSurgicalSafetyCheck.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Surgical Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Surgical Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteSurgicalCheckList.PerformCallback(recordID);
                }
            });
        });

        $('#lblAddSurgicalSafetyChecklistForm').die('click');
        $('#lblAddSurgicalSafetyChecklistForm').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignInFormEntry.ascx");
                var param = "0" + "||||||||||" + $('#<%=hdnLinkedID.ClientID %>').val();
                openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
            }
        });

        $('#lblAddSignInSafetyChecklistForm').die('click');
        $('#lblAddSignInSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignInFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "||||||||||" + $('#<%=hdnLinkedID.ClientID %>').val();
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
        });

        $('#lblAddTimeOutSafetyChecklistForm').die('click');
        $('#lblAddTimeOutSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgeryTimeOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "||||||||||" + $('#<%=hdnLinkedID.ClientID %>').val();
            openUserControlPopup(url, param, "Surgical Safety Check List - TIME OUT", 900, 500);
        });

        $('#lblAddSignOutSafetyChecklistForm').die('click');
        $('#lblAddSignOutSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "||||||||||" + $('#<%=hdnLinkedID.ClientID %>').val();
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN OUT", 700, 500);
        });

        $('.imgEditSignIn.imgLink').die('click');
        $('.imgEditSignIn.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignInFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signInDate = $(this).attr('signInDate');
            var signInTime = $(this).attr('signInTime');
            var paramedicID = $(this).attr('paramedicID');
            var physicianID = $(this).attr('physicianID');
            var anesthetistID = $(this).attr('anesthetistID');
            var nurseID = $(this).attr('nurseID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var isVerified = $(this).attr('isSignInVerified');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                if (isVerified == "True") {
                    displayErrorMessageBox("Surgical Safety Check List", "Sudah dilakukan verifikasi oleh Dokter Operator/Anestesi tidak dapat dilakukan perubahan");
                    return false;
                }
                else {
                    var param = recordID + "|" + testOrderID + "|" + signInDate + "|" + signInTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + nurseID + "|" + layout + "|" + values + "|" + $('#<%=hdnLinkedID.ClientID %>').val();
                    openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
                }
            }
            else {
                displayErrorMessageBox('Surgical Safefy Check List', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewSignIn.imgLink').die('click');
        $('.imgViewSignIn.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgerySignInCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signInDate = $(this).attr('signInDate');
            var signInTime = $(this).attr('signInTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + signInDate + "|" + signInTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
        });

        $('.imgEditTimeOut.imgLink').die('click');
        $('.imgEditTimeOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgeryTimeOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var timeOutDate = $(this).attr('timeOutDate');
            var timeOutTime = $(this).attr('timeOutTime');
            var paramedicID = $(this).attr('paramedicID');
            var physicianID = $(this).attr('physicianID');
            var anesthetistID = $(this).attr('anesthetistID');
            var nurseID = $(this).attr('nurseID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var isVerified = $(this).attr('isTimeOutVerified');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                if (isVerified == "True") {
                    displayErrorMessageBox("Surgical Safety Check List", "Sudah dilakukan verifikasi oleh Dokter Operator/Anestesi tidak dapat dilakukan perubahan");
                    return false;
                }
                else {
                    var param = recordID + "|" + testOrderID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + nurseID + "|" + layout + "|" + values + "|" + $('#<%=hdnLinkedID.ClientID %>').val();
                    openUserControlPopup(url, param, "Surgical Safety Check List - TIME OUT", 900, 500);
                }
            }
            else {
                displayErrorMessageBox('Surgical Safefy Check List', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewTimeOut.imgLink').die('click');
        $('.imgViewTimeOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgeryTimeOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var timeOutDate = $(this).attr('timeOutDate');
            var timeOutTime = $(this).attr('timeOutTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - TIME OUT", 900, 500);
        });

        $('.imgEditSignOut.imgLink').die('click');
        $('.imgEditSignOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signOutDate = $(this).attr('signOutDate');
            var signOutTime = $(this).attr('signOutTime');
            var surgeryEndDate = $(this).attr('surgeryEndDate');
            var surgeryEndTime = $(this).attr('surgeryEndTime');
            var paramedicID = $(this).attr('paramedicID');
            var physicianID = $(this).attr('physicianID');
            var anesthetistID = $(this).attr('anesthetistID');
            var nurseID = $(this).attr('nurseID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var isVerified = $(this).attr('isSignOutVerified');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                if (isVerified == "True") {
                    displayErrorMessageBox("Surgical Safety Check List", "Sudah dilakukan verifikasi oleh Dokter Operator/Anestesi tidak dapat dilakukan perubahan");
                    return false;
                }
                else {
                    var param = recordID + "|" + testOrderID + "|" + signOutDate + "|" + signOutTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + nurseID + "|" + layout + "|" + values + "|" + surgeryEndDate + "|" + surgeryEndTime + "|" + $('#<%=hdnLinkedID.ClientID %>').val();
                    openUserControlPopup(url, param, "Surgical Safety Check List - SIGN OUT", 700, 500);
                }
            }
            else {
                displayErrorMessageBox('Surgical Safefy Check List', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewSignOut.imgLink').die('click');
        $('.imgViewSignOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgerySignOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signOutDate = $(this).attr('signOutDate');
            var signOutTime = $(this).attr('signOutTime');
            var surgeryEndDate = $(this).attr('surgeryEndDate');
            var surgeryEndTime = $(this).attr('surgeryEndTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + signOutDate + "|" + signOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values + "|" + surgeryEndDate + "|" + surgeryEndTime;
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN OUT", 700, 500);
        });

        $('.imgDeleteSignIn.imgLink').die('click');
        $('.imgDeleteSignIn.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Sign In dari Surgical Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Surgical Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteCheckListInfo.PerformCallback("1" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteTimeOut.imgLink').die('click');
        $('.imgDeleteTimeOut.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Time Out dari Surgical Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Surgical Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteCheckListInfo.PerformCallback("2" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteSignOut.imgLink').die('click');
        $('.imgDeleteSignOut.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Sign Out dari Surgical Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Surgical Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteCheckListInfo.PerformCallback("3" + "|" + recordID);
                }
            });
        });
        //#endregion

        //#region Laporan Operasi
        $('#lblAddReport').die('click');
        $('#lblAddReport').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) cbpMPListProcess.PerformCallback('add');
        });

        $('.imgAddReport2.imgLink').die('click');
        $('.imgAddReport2.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) cbpMPListProcess.PerformCallback('add');
        });

        $('.imgEditReport.imgLink').die('click');
        $('.imgEditReport.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            var recordID = $(this).attr('recordID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            $('#<%=hdnSurgeryReportID.ClientID %>').val(recordID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteReport.imgLink').die('click');
        $('.imgDeleteReport.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            $('#<%=hdnSurgeryReportID.ClientID %>').val(recordID);
            var message = "Hapus laporan operasi untuk untuk pasien ini ?";
            displayConfirmationMessageBox("Laporan Operasi", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });
        //#endregion

        //#region e-Document
        $('#lblAddDocument').die('click');
        $('#lblAddDocument').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
                var param = "||" + $('#<%=hdnLinkedID.ClientID %>').val();
                openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
            }
        });

        $('.imgAddDocument.imgLink').die('click');
        $('.imgAddDocument.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
            var param = "||" + $('#<%=hdnLinkedID.ClientID %>').val();
            openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
        });

        $('.imgEditDocument.imgLink').die('click');
        $('.imgEditDocument.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
            var param = recordID + "||" + $('#<%=hdnLinkedID.ClientID %>').val();
            openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
        });

        $('.imgDeleteDocument.imgLink').die('click');
        $('.imgDeleteDocument.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var fileName = $(this).attr('fileName');
            var message = "Hapus file/dokumen " + fileName + " dari Pengkajian Kamar Operasi untuk pasien ini ?";
            displayConfirmationMessageBox("e-Document", message, function (result) {
                if (result) {
                    cbpDeleteDocument.PerformCallback(recordID);
                }
            });
        });

        $('.lnkViewDocument a').die('click');
        $('.lnkViewDocument a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
            window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
        });
        //#endregion

        //#region Surgical Safety Checklist
        $('.imgViewSignIn.imgLink').die('click');
        $('.imgViewSignIn.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgerySignInCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signInDate = $(this).attr('signInDate');
            var signInTime = $(this).attr('signInTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + signInDate + "|" + signInTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
        });

        $('.imgViewTimeOut.imgLink').die('click');
        $('.imgViewTimeOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgeryTimeOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var timeOutDate = $(this).attr('timeOutDate');
            var timeOutTime = $(this).attr('timeOutTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - TIME OUT", 700, 500);
        });        
        //#endregion

        //#region Checklist Form
        $('#lblAddChecklist').die('click');
        $('#lblAddChecklist').live('click', function (evt) {
            addChecklist();
        });

        $('.imgAddChecklist.imgLink').die('click');
        $('.imgAddChecklist.imgLink').live('click', function (evt) {
            addChecklist();
        });

        function addChecklist() {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/PreProcedureChecklistFormEntry.ascx");
                var param = "0" + "|" + "X530^001" + "|" + "X531^001" + "||" + $('#<%=hdnVisitID.ClientID %>').val() + "||||||" + $('#<%=hdnRegistrationID.ClientID %>').val() + '|' + $('#<%=hdnLinkedID.ClientID %>').val();
                openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
            }
        }

        $('.imgEditChecklist.imgLink').die('click');
        $('.imgEditChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var checklistDate = $(this).attr('checklistDate');
            var checklistTime = $(this).attr('checklistTime');
            var paramedicID = $(this).attr('paramedicID');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/PreProcedureChecklistFormEntry.ascx");
                var param = recordID + "|" + "X530^001" + "|" + "X531^001" + "||" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegistrationID.ClientID %>').val() + "|" + $('#<%=hdnLinkedID.ClientID %>').val();
                openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
            }
            else {
                displayErrorMessageBox('Checklist Persiapan Operasi', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgDeleteChecklist.imgLink').die('click');
        $('.imgDeleteChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var message = "Hapus informasi checklist persiapan kamar operasi untuk pasien ini ?";
                displayConfirmationMessageBox("Checklist Persiapan Kamar Operasi", message, function (result) {
                    if (result) {
                        cbpDeleteChecklist.PerformCallback(recordID);
                    }
                });
            }
            else {
                displayErrorMessageBox('Checklist Persiapan Operasi', 'Maaf, tidak diijinkan menghapus pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewChecklist.imgLink').die('click');
        $('.imgViewChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var checklistDate = $(this).attr('checklistDate');
            var checklistTime = $(this).attr('checklistTime');
            var paramedicName = $(this).attr('paramedicName');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/ViewPreProcedureChecklistCtl.ascx");
            var param = recordID + "|" + "X530^001" + "|" + "X531^001" + "||" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicName + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegistrationID.ClientID %>').val() + "|" + $('#<%=hdnLinkedID.ClientID %>').val();
            openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
        });

        $('.imgCopyChecklist.imgLink').die('click');
        $('.imgCopyChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var checklistDate = $(this).attr('checklistDate');
            var checklistTime = $(this).attr('checklistTime');
            var paramedicID = $(this).attr('paramedicID');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            var message = "Lakukan copy form checklist persiapan pasien ?";
            displayConfirmationMessageBox('COPY FORM CHECKLIST :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/CopyPreProcedureChecklistEntry.ascx");
                    var param = "0" + "|" + "X530^001" + "|" + "X531^001" + "||" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegistrationID.ClientID %>').val() + "|" + $('#<%=hdnLinkedID.ClientID %>').val();
                    openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
                }
            });
        });
        //#endregion

        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
                    displayErrorMessageBox('Warning', 'Maaf, Assessment sudah ditandai selesai atau sudah diverifikasi sehingga tidak bisa diubah.');
                    return false;
                }
                else
                    return true;
            }
            else {
                displayErrorMessageBox('EDIT', 'Maaf, Assessment hanya bisa diubah/dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onBeforeBasePatientPageListDelete() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('DELETE', 'Maaf, Assessment hanya bisa diubah/dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onBeforeEditDelete(paramedicID) {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, pengkajian hanya bisa diubah/dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshDocumentGrid() {
            cbpViewDt5.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup() {
            setTimeout(function () { cbpViewDt2.PerformCallback('refresh'); }, 1000);
            setTimeout(function () { cbpViewDt8.PerformCallback('refresh'); }, 1000);
        }

        function onAfterSaveEditRecordEntryPopup() {
            setTimeout(function () { cbpViewDt2.PerformCallback('refresh'); }, 1000);
            setTimeout(function () { cbpViewDt8.PerformCallback('refresh'); }, 1000);
        }

        $('#imgVisitNote.imgLink').live('click', function () {
            $(this).closest('tr').click();
            var id = $('#<%=hdnLinkedID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/VisitNotesHistoryCtl.ascx");
            openUserControlPopup(url, id, 'History Catatan Perawat', 900, 500);
        });

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
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            var ispreSurgeryAssessmentExists = s.cpIsSurgeryPreAssessmentExists;

            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });

                $('#<%=hdnIsSurgeryPreAssessmentExists.ClientID %>').val(ispreSurgeryAssessmentExists);
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt2EndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2"), pageCount1, function (page) {
                    cbpViewDt2.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount3 = parseInt(param[1]);

                if (pageCount3 > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount3, function (page) {
                    cbpViewDt3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt4EndCallback(s) {
            $('#containerImgLoadingViewDt4').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt4"), pageCount1, function (page) {
                    cbpViewDt4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt5EndCallback(s) {
            $('#containerImgLoadingViewDt5').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt5"), pageCount1, function (page) {
                    cbpViewDt5.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt6EndCallback(s) {
            $('#containerImgLoadingViewDt6').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount6 = parseInt(param[1]);

                if (pageCount6 > 0)
                    $('#<%=grdViewDt6.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt6"), pageCount6, function (page) {
                    cbpViewDt6.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt6.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt8EndCallback(s) {
            $('#containerImgLoadingViewDt8').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt8.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt8"), pageCount1, function (page) {
                    cbpViewDt8.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt8.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt10EndCallback(s) {
            $('#containerImgLoadingViewDt10').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt10.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt10"), pageCount1, function (page) {
                    cbpViewDt10.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnLinkedID = $('#<%=hdnLinkedID.ClientID %>').val();

            if (hdnLinkedID == '' || hdnLinkedID == '0') {
                if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00160' || code == 'PM-00524') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'MR000013' || code == 'MR000017') {
                    filterExpression.text = visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                    return false;
                }
            }
            else if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019') {
                filterExpression.text = visitID;
                return true;
            }
            else if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00522' || code == 'PM-00523' || code == 'PM-00159') {
                filterExpression.text = registrationID;
                return true;
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }

        function onCbpCompletedEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('COMPLETED : FAILED', param[1]);
            }
        }

        function onCbpDeleteDocumentEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt5.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('e-Document', param[1]);
            }
        }

        function onCbpDeleteChecklistEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt8.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Checklist Persiapan Kamar Operasi', param[1]);
            }
        }

        function onCbpDeleteCheckListInfoEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Surgical Check List', param[1]);
            }
        }

        function onCbpDeleteSurgicalCheckListEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Surgical Check List', param[1]);
            }
        }

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
        #leftPanel
        {
            border: 1px solid #6E6E6E;
            width: 100%;
            height: 100%;
            position: relative;
        }
        #leftPanel > ul
        {
            margin: 0;
            padding: 2px;
            border-bottom: 1px groove black;
        }
        #leftPanel > ul > li
        {
            list-style-type: none;
            font-size: 15px;
            display: list-item;
            border: 1px solid #fdf5e6 !important;
            padding: 5px 8px;
            cursor: pointer;
            background-color: #87CEEB !important;
        }
        #leftPanel > ul > li.selected
        {
            background-color: #ff5722 !important;
            color: White;
        }
        .divContent
        {
            padding-left: 3px;
            min-height: 490px;
        }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnModuleID" value="" />
    <input type="hidden" runat="server" id="hdnLinkedID" value="0" />
    <input type="hidden" runat="server" id="hdnChargesParamedicID" value="0" />
    <input type="hidden" runat="server" id="hdnChargesParamedicName" value="0" />
    <input type="hidden" runat="server" id="hdnSurgeryReportID" value="0" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnPostSurgeryInstructionID" value="0" />
    <input type="hidden" runat="server" id="hdnTransactionNo" value="" />
    <input type="hidden" runat="server" id="hdnItemName" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnRevisedParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnInputSurgeryAssessmentFirst" value="0" />
    <input type="hidden" runat="server" id="hdnIsSurgeryPreAssessmentExists" value="0" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevised" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevision" value="0" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrl" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 370px" />
            <col />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative; width: 100%">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PatientChargesDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="TransactionNo" HeaderStyle-CssClass="hiddenColumn transactionNo"
                                                ItemStyle-CssClass="hiddenColumn TransactionNo" />
                                            <asp:BoundField DataField="ItemName" HeaderStyle-CssClass="hiddenColumn itemName"
                                                ItemStyle-CssClass="hiddenColumn itemName" />
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn paramadicID"
                                                ItemStyle-CssClass="hiddenColumn paramedicID" />
                                            <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn paramadicName"
                                                ItemStyle-CssClass="hiddenColumn paramedicName" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Tanggal ") %>
                                                        -
                                                        <%=GetLabel("Jam Transaksi") %>,
                                                        <%=GetLabel("No. Transaksi") %></div>
                                                    <div style="font-weight: bold">
                                                        <span style="color: blue">
                                                            <%=GetLabel("Dibuat Oleh") %></span></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%#: Eval("TransactionDate")%>,
                                                                    <%#: Eval("TransactionNo")%></div>
                                                                <div style="font-weight: bold">
                                                                    <span>
                                                                        <%#: Eval("ItemName") %></span></div>
                                                                <div style="font-weight: bold">
                                                                    <span style="color: blue">
                                                                        <%#: Eval("ParamedicName") %></span></div>
                                                            </td>
                                                            <%--                                                            <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                <div>
                                                                    <input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color: Red;
                                                                        color: White; width: 100px;" /></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %>>
                                                                <div>
                                                                    <input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color: Green;
                                                                        color: White; width: 100px" /></div>
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order Penunjang untuk pasien ini")%>
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
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="preSurgeryAssessment">
                                        <%=GetLabel("Asesmen Pra Tindakan")%></li>
                                    <li contentid="preSurgeryAnesthesy">
                                        <%=GetLabel("Asesmen Pra Anestesi")%></li>
                                    <li contentid="preSurgicalSafetyChecklist">
                                        <%=GetLabel("Surgical Safety Checklist")%></li>
                                    <li contentid="anesthesyStatus">
                                        <%=GetLabel("Status Anestesi")%></li>
                                    <li contentid="surgeryReport">
                                        <%=GetLabel("Laporan Tindakan")%></li>
                                    <li contentid="surgeryDocument">
                                        <%=GetLabel("e-Document")%></li>
                                    <li contentid="preProcedureChecklist">
                                        <%=GetLabel("Checklist Persiapan Operasi")%></li>
                                    <li contentid="postSurgeryInstruction" style="display: none">
                                        <%=GetLabel("Instruksi Pasca Bedah")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="preSurgeryAssessment">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreSurgeryAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Tindakan")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreSurgicalAssessmentID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgEditAssessment imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" assessmentid="<%#:Eval("PreSurgicalAssessmentID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteAssessment imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" assessmentid="<%#:Eval("PreSurgicalAssessmentID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PreSurgicalAssessmentID" HeaderStyle-CssClass="keyField"
                                                            ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PreSurgicalAssessmentID") %>" bindingfield="PreSurgicalAssessmentID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis" DataField="PreDiagnoseText" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada asesmen pra tindakan untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddAsessment">
                                                                <%= GetLabel("+ Asesmen Pra Tindakan")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="preSurgeryAnesthesy" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreAnesthesyAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreAnesthesyAssessmentID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgEditAnesthesy imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("PreAnesthesyAssessmentID") %>"
                                                                                linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteAnesthesy imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("PreAnesthesyAssessmentID") %>"
                                                                                linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PreAnesthesyAssessmentID" HeaderStyle-CssClass="keyField"
                                                            ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PreAnesthesyAssessmentID") %>" bindingfield="PreAnesthesyAssessmentID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="ASA" DataField="cfASAStatus" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Teknik Anestesi" DataField="cfAnesthesiaType" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada asesmen pra anestesi dan sedasi untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddAnesthesy">
                                                                <%= GetLabel("+ Asesmen Pra Anestesi")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt3">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="anesthesyStatus" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt6" runat="server" Width="100%" ClientInstanceName="cbpViewDt6"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt6_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt6').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt6EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt6" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewAnesthesyStatus imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" visitid="<%#:Eval("VisitID") %>"
                                                                                paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("AnesthesyStatusID") %>"
                                                                                linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgEditAnesthesyStatus imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" assessmentid="<%#:Eval("AnesthesyStatusID") %>"
                                                                                linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteAnesthesyStatus imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" assessmentid="<%#:Eval("AnesthesyStatusID") %>"
                                                                                linkedid="<%#:Eval("PatientChargesDtID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AnesthesyStatusID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("AnesthesyStatusID") %>" bindingfield="AnesthesyStatusID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="ASA" DataField="cfASAStatus" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Teknik Anestesi" DataField="cfAnesthesiaType" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada status anestesi untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddAnesthesyStatus">
                                                                <%= GetLabel("+ Status Anestesi")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt6">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt6">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="surgeryReport" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddReport2 imgLink" title='<%=GetLabel("+ Laporan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" linkedid="<%#:Eval("TransactionDtID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditReport imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("PatientSurgeryID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteReport imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("PatientSurgeryID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PatientSurgeryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PatientSurgeryID") %>" bindingfield="PatientSurgeryID" />
                                                                <input type="hidden" value="<%#:Eval("cfReportDate") %>" bindingfield="cfReportDate" />
                                                                <input type="hidden" value="<%#:Eval("ReportTime") %>" bindingfield="ReportTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfReportDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="ReportTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis" DataField="PreOperativeDiagnosisText"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Post Diagnosis" DataField="PostOperativeDiagnosisText"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada laporan tindakan untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddReport">
                                                                <%= GetLabel("+ Laporan Tindakan")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt4">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt4">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="surgeryDocument" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt5" runat="server" Width="100%" ClientInstanceName="cbpViewDt5"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt5_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt5EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt5" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddDocument imgLink" title='<%=GetLabel("+ Document")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                    filename="<%#:Eval("FileName") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditDocument imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                filename="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteDocument imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                filename="<%#:Eval("FileName") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn"
                                                            ItemStyle-CssClass="hiddenColumn fileName" />
                                                        <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada informasi e=document untuk tindakan operasi di pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddDocument">
                                                                <%= GetLabel("+ Document")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt5">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt5">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="preSurgicalSafetyChecklist" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent7" runat="server">
                                            <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px"
                                                            ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 4px">
                                                                    <tr>
                                                                        <td style="text-align: center">
                                                                            <img class="imgDeleteSurgicalSafetyCheck imgLink" title='<%=GetLabel("Delete Check List")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" recordid="<%#:Eval("ID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="SIGN IN" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width: 60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfSignInDateTime") %></div>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("SignInParamedicName") %></div>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Sign In Surgical Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddSignInSafetyChecklistForm" recordid="<%#:Eval("ID") %>"
                                                                                    testorderid="<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Sign In Check List")%></span>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditSignIn imgLink" title='<%=GetLabel("Edit Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                            layout="<%#:Eval("SignInLayout") %>" values="<%#:Eval("SignInValues") %>" paramedicid="<%#:Eval("SignInParamedicID") %>"
                                                                                            physicianid="<%#:Eval("SignOutPhysicianID") %>" anesthetistid="<%#:Eval("SignOutAnesthetistID") %>"
                                                                                            nurseid="<%#:Eval("SignOutNurseID") %>" signindate="<%#:Eval("cfSignInDate") %>"
                                                                                            signintime="<%#:Eval("SignInTime") %>" issigninverified="<%#:Eval("cfIsSignInVerified") %>"
                                                                                            istimeoutverified="<%#:Eval("cfIsTimeOutVerified") %>" issignoutverified="<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteSignIn imgLink" title='<%=GetLabel("Delete Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" issigninverified="<%#:Eval("cfIsSignInVerified") %>"
                                                                                            istimeoutverified="<%#:Eval("cfIsTimeOutVerified") %>" issignoutverified="<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewSignIn imgLink" title='<%=GetLabel("Lihat Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                            layout="<%#:Eval("SignInLayout") %>" values="<%#:Eval("SignInValues") %>" paramedicname="<%#:Eval("SignInParamedicName") %>"
                                                                                            physicianname="<%#:Eval("SignOutPhysicianName") %>" anesthetistname="<%#:Eval("SignOutAnesthetistName") %>"
                                                                                            nursename="<%#:Eval("SignOutNurseName") %>" signindate="<%#:Eval("cfSignInDate") %>"
                                                                                            signintime="<%#:Eval("SignInTime") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <table border="1" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                                    <colgroup>
                                                                                        <col style="width: 275px" />
                                                                                        <col />
                                                                                    </colgroup>
                                                                                    <tr>
                                                                                        <td colspan="2" style="text-align: center">
                                                                                            STATUS VERIFIKASI
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("SignOutPhysicianName") %>
                                                                                            <div <%# Eval("IsVerifiedBySignInPhysician1").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                <span style="font-style: italic">
                                                                                                    <%#:Eval("cfSignInVerifiedByPhysician1Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: center">
                                                                                            <div <%# Eval("IsVerifiedBySignInPhysician1").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                Belum
                                                                                            </div>
                                                                                            <div <%# Eval("IsVerifiedBySignInPhysician1").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                                Sudah
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("SignOutAnesthetistName") %>
                                                                                            <div <%# Eval("IsVerifiedBySignInPhysician2").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                <span style="font-style: italic">
                                                                                                    <%#:Eval("cfSignInVerifiedByPhysician2Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: center">
                                                                                            <div <%# Eval("IsVerifiedBySignInPhysician2").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                Belum
                                                                                            </div>
                                                                                            <div <%# Eval("IsVerifiedBySignInPhysician2").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                                Sudah
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="TIME OUT" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width: 60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfTimeOutDateTime") %></div>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("TimeOutParamedicName") %></div>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Time Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddTimeOutSafetyChecklistForm" recordid="<%#:Eval("ID") %>"
                                                                                    testorderid="<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Time Out Check List")%></span>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditTimeOut imgLink" title='<%=GetLabel("Edit Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                            layout="<%#:Eval("TimeOutLayout") %>" values="<%#:Eval("TimeOutValues") %>" paramedicid="<%#:Eval("TimeOutParamedicID") %>"
                                                                                            physicianid="<%#:Eval("SignOutPhysicianID") %>" anesthetistid="<%#:Eval("SignOutAnesthetistID") %>"
                                                                                            nurseid="<%#:Eval("SignOutNurseID") %>" timeoutdate="<%#:Eval("cfTimeOutDate") %>"
                                                                                            timeouttime="<%#:Eval("TimeOutTime") %>" issigninverified="<%#:Eval("cfIsSignInVerified") %>"
                                                                                            istimeoutverified="<%#:Eval("cfIsTimeOutVerified") %>" issignoutverified="<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteTimeOut imgLink" title='<%=GetLabel("Delete Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" issigninverified="<%#:Eval("cfIsSignInVerified") %>"
                                                                                            istimeoutverified="<%#:Eval("cfIsTimeOutVerified") %>" issignoutverified="<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewTimeOut imgLink" title='<%=GetLabel("Lihat Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                            layout="<%#:Eval("TimeOutLayout") %>" values="<%#:Eval("TimeOutValues") %>" paramedicname="<%#:Eval("TimeOutParamedicName") %>"
                                                                                            physicianname="<%#:Eval("SignOutPhysicianName") %>" anesthetistname="<%#:Eval("SignOutAnesthetistName") %>"
                                                                                            nursename="<%#:Eval("SignOutNurseName") %>" timeoutdate="<%#:Eval("cfTimeOutDate") %>"
                                                                                            timeouttime="<%#:Eval("TimeOutTime") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <table border="1" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                                    <colgroup>
                                                                                        <col style="width: 275px" />
                                                                                        <col />
                                                                                    </colgroup>
                                                                                    <tr>
                                                                                        <td colspan="2" style="text-align: center">
                                                                                            STATUS VERIFIKASI
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("SignOutPhysicianName") %>
                                                                                            <div <%# Eval("IsVerifiedByTimeOutPhysician1").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                <span style="font-style: italic">
                                                                                                    <%#:Eval("cfTimeOutVerifiedByPhysician1Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: center">
                                                                                            <div <%# Eval("IsVerifiedByTimeOutPhysician1").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                Belum
                                                                                            </div>
                                                                                            <div <%# Eval("IsVerifiedByTimeOutPhysician1").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                                Sudah
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("SignOutAnesthetistName") %>
                                                                                            <div <%# Eval("IsVerifiedByTimeOutPhysician2").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                <span style="font-style: italic">
                                                                                                    <%#:Eval("cfTimeOutVerifiedByPhysician2Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: center">
                                                                                            <div <%# Eval("IsVerifiedByTimeOutPhysician2").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                Belum
                                                                                            </div>
                                                                                            <div <%# Eval("IsVerifiedByTimeOutPhysician2").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                                Sudah
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="SIGN OUT" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width: 60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfSignOutDateTime") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("SignOutNurseName") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Sign Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddSignOutSafetyChecklistForm" recordid="<%#:Eval("ID") %>"
                                                                                    testorderid="<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Sign Out Check List")%></span>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditSignOut imgLink" title='<%=GetLabel("Edit Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                            layout="<%#:Eval("SignOutLayout") %>" values="<%#:Eval("SignOutValues") %>" paramedicid="<%#:Eval("SignOutNurseID") %>"
                                                                                            physicianid="<%#:Eval("SignOutPhysicianID") %>" anesthetistid="<%#:Eval("SignOutAnesthetistID") %>"
                                                                                            nurseid="<%#:Eval("SignOutNurseID") %>" signoutdate="<%#:Eval("cfSignOutDate") %>"
                                                                                            signouttime="<%#:Eval("SignOutTime") %>" surgeryenddate="<%#:Eval("cfSurgeryEndDate") %>"
                                                                                            surgeryendtime="<%#:Eval("SurgeryEndTime") %>" issigninverified="<%#:Eval("cfIsSignInVerified") %>"
                                                                                            istimeoutverified="<%#:Eval("cfIsTimeOutVerified") %>" issignoutverified="<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteSignOut imgLink" title='<%=GetLabel("Delete Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" issigninverified="<%#:Eval("cfIsSignInVerified") %>"
                                                                                            istimeoutverified="<%#:Eval("cfIsTimeOutVerified") %>" issignoutverified="<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewSignOut imgLink" title='<%=GetLabel("Lihat Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                            layout="<%#:Eval("SignOutLayout") %>" values="<%#:Eval("SignOutValues") %>" paramedicname="<%#:Eval("SignInParamedicName") %>"
                                                                                            physicianname="<%#:Eval("SignOutPhysicianName") %>" anesthetistname="<%#:Eval("SignOutAnesthetistName") %>"
                                                                                            nursename="<%#:Eval("SignOutNurseName") %>" signoutdate="<%#:Eval("cfSignOutDate") %>"
                                                                                            signouttime="<%#:Eval("SignOutTime") %>" surgeryenddate="<%#:Eval("cfSurgeryEndDate") %>"
                                                                                            surgeryendtime="<%#:Eval("SurgeryEndTime") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <table border="1" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                                    <colgroup>
                                                                                        <col style="width: 275px" />
                                                                                        <col />
                                                                                    </colgroup>
                                                                                    <tr>
                                                                                        <td colspan="2" style="text-align: center">
                                                                                            STATUS VERIFIKASI
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("SignOutPhysicianName") %>
                                                                                            <div <%# Eval("IsVerifiedBySignOutPhysician1").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                <span style="font-style: italic">
                                                                                                    <%#:Eval("cfSignOutVerifiedByPhysician1Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: center">
                                                                                            <div <%# Eval("IsVerifiedBySignOutPhysician1").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                Belum
                                                                                            </div>
                                                                                            <div <%# Eval("IsVerifiedBySignOutPhysician1").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                                Sudah
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <%#:Eval("SignOutAnesthetistName") %>
                                                                                            <div <%# Eval("IsVerifiedBySignOutPhysician2").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                <span style="font-style: italic">
                                                                                                    <%#:Eval("cfSignOutVerifiedByPhysician2Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align: center">
                                                                                            <div <%# Eval("IsVerifiedBySignOutPhysician2").ToString() == "True" ? "Style='display:none'":"" %>>
                                                                                                Belum
                                                                                            </div>
                                                                                            <div <%# Eval("IsVerifiedBySignOutPhysician2").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                                Sudah
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada formulir Surgical Safety Check List untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddSurgicalSafetyChecklistForm">
                                                                <%= GetLabel("+ Formulir Surgical Safety Check List")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="postSurgeryInstruction" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt10" runat="server" Width="100%" ClientInstanceName="cbpViewDt10"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt10_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt10').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt10EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent11" runat="server">
                                            <asp:Panel runat="server" ID="Panel10" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt10" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px"
                                                            ItemStyle-Width="60px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddInstruction imgLink" title='<%=GetLabel("+ Instruksi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordid="<%#:Eval("PostSurgeryInstructionID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewInstruction imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordid="<%#:Eval("PostSurgeryInstructionID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgEditInstruction imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordid="<%#:Eval("PostSurgeryInstructionID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteInstruction imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordid="<%#:Eval("PostSurgeryInstructionID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PostSurgeryInstructionID" HeaderStyle-CssClass="keyField"
                                                            ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="hiddenColumn testOrderID"
                                                            ItemStyle-CssClass="hiddenColumn testOrderID" />
                                                        <asp:BoundField DataField="cfInstructionDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="cfInstructionDate" ItemStyle-CssClass="cfInstructionDate"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="InstructionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="InstructionTime" ItemStyle-CssClass="InstructionTime"
                                                            HeaderStyle-Width="60px" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dibuat oleh" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <%=GetLabel("Tidak ada informasi instruksi pasca bedah terintegrasi untuk pasien ini")%>
                                                            <br />
                                                            <span class="lblLink" id="lblAddInstruction">
                                                                <%= GetLabel("+ Instruksi")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt10">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt10">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="preProcedureChecklist" style="position: relative;
                                display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt8" runat="server" Width="100%" ClientInstanceName="cbpViewDt8"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt8_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt8').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt8EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent9" runat="server">
                                            <input type="hidden" value="" id="hdnFileString" runat="server" />
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt8" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px"
                                                            ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddChecklist imgLink" title='<%=GetLabel("+ Checklist")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditChecklist imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                checklistdate="<%#:Eval("cfChecklistDatePickerFormat") %>" checklisttime="<%#:Eval("ChecklistTime") %>"
                                                                                paramedicid="<%#:Eval("ParamedicID") %>" formlayout="<%#:Eval("ChecklistFormLayout") %>"
                                                                                formvalue="<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteChecklist imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgViewChecklist imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("ID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" checklistdate="<%#:Eval("cfChecklistDatePickerFormat") %>"
                                                                                checklisttime="<%#:Eval("ChecklistTime") %>" paramedicid="<%#:Eval("ParamedicID") %>"
                                                                                paramedicname="<%#:Eval("ParamedicName") %>" formlayout="<%#:Eval("ChecklistFormLayout") %>"
                                                                                formvalue="<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgCopyChecklist imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>' alt="" recordid="<%#:Eval("ID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" checklistdate="<%#:Eval("cfChecklistDatePickerFormat") %>"
                                                                                checklisttime="<%#:Eval("ChecklistTime") %>" paramedicid="<%#:Eval("ParamedicID") %>"
                                                                                paramedicname="<%#:Eval("ParamedicName") %>" formlayout="<%#:Eval("ChecklistFormLayout") %>"
                                                                                formvalue="<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:BoundField DataField="cfChecklistDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="ChecklistTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime"
                                                            ItemStyle-CssClass="assessmentTime" />
                                                        <asp:BoundField DataField="ParamedicID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn"
                                                            ItemStyle-CssClass="paramedicID hiddenColumn" />
                                                        <asp:BoundField DataField="GCChecklistType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                        <asp:BoundField DataField="ChecklistFormLayout" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                        <asp:BoundField DataField="ChecklistFormValue" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                        <asp:BoundField DataField="cfChecklistDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                            ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada data checklist persiapan untuk nomor order ini") %>
                                                        <br />
                                                        <span class="lblLink" id="lblAddChecklist">
                                                            <%= GetLabel("+ Checklist Persiapan Operasi")%></span>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt8">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt8">
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
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpCompleted" runat="server" Width="100%" ClientInstanceName="cbpCompleted"
            ShowLoadingPanel="false" OnCallback="cbpCompleted_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCompletedEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteIntraAnesthesy" runat="server" Width="100%"
            ClientInstanceName="cbpDeleteIntraAnesthesy" ShowLoadingPanel="false" OnCallback="cbpDeleteIntraAnesthesy_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteIntraAnesthesyEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteDocument" runat="server" Width="100%" ClientInstanceName="cbpDeleteDocument"
            ShowLoadingPanel="false" OnCallback="cbpDeleteDocument_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteDocumentEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteChecklist" runat="server" Width="100%" ClientInstanceName="cbpDeleteChecklist"
            ShowLoadingPanel="false" OnCallback="cbpDeleteChecklist_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteChecklistEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteCheckListInfo" runat="server" Width="100%" ClientInstanceName="cbpDeleteCheckListInfo"
            ShowLoadingPanel="false" OnCallback="cbpDeleteCheckListInfo_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteCheckListInfoEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteSurgicalCheckList" runat="server" Width="100%" ClientInstanceName="cbpDeleteSurgicalCheckList"
            ShowLoadingPanel="false" OnCallback="cbpDeleteSurgicalCheckList_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteSurgicalCheckListEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
