<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="IntegratedNotesList1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.IntegratedNotesList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerateNote" runat="server" CRUDMode="C"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Generate")%></div></li>
    <li id="btnVerifyAll" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Verify All")%></div>
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
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erevaluationnotes">
        var iColIndex = 0;
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.cfIsEditable').html());
                $('#<%=hdnIsNoteFromOtherProcess.ClientID %>').val($(this).find('.isNoteFromOtherProcess').html());
                $('#<%=hdnPatientNoteType.ClientID %>').val($(this).find('.controlColumn').html());
                $('#<%=hdnRowIndex.ClientID %>').val($(this).closest("tr").prevAll("tr").length);

                var index = $('#<%=grdView.ClientID %> tr').index(this);
                $('#<%=hdnLastIndexSelected.ClientID %>').val(index);
            });

            $('#<%=btnGenerateNote.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/PatientPage/_PopupEntry/SOAP/GenerateSOAPNotesCtl.ascx");
                openUserControlPopup(url, "", 'Generate SOAP Notes', 800, 600);
            });

            $('.btnView').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var deptID = $(this).closest('tr').find('.departmentID').html();
                var paramedicMasterType = $(this).attr("paramedicMasterType");
                var chiefComplaintID = $(this).attr("ChiefComplaintID");
                var nurseChiefComplaintID = $(this).attr("NurseChiefComplaintID");
                var param = visitID + "|" + id;


                if (paramedicMasterType == Constant.ParamedicType.Physician) {
                    if (deptID == Constant.Facility.EMERGENCY) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/ERPhysicianInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.INPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/IPPhysicianInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.OUTPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/OPPhysicianInitialAssessmentCtl1.ascx");
                    }
                    else {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/MDPhysicianInitialAssessmentCtl1.ascx");
                    }
                    param = visitID + "|" + id + "|" + chiefComplaintID;
                }
                else {
                    if (deptID == Constant.Facility.EMERGENCY) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ERNurseInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.INPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/IPNurseInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.OUTPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/OPNurseInitialAssessmentCtl1.ascx");
                    }
                    else {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/MDNurseInitialAssessmentCtl1.ascx");
                    }
                    param = visitID + "|" + id + "|" + nurseChiefComplaintID;
                }
                openUserControlPopup(url, param, 'Detail Kajian', 1300, 600);
            });

            $('.btnViewDetail').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var testOrderID = "";
                var isPostSurgeryInstruction = $(this).attr("isPostSurgeryInstruction");
                var postSurgeryInstructionID = $(this).attr("postSurgeryInstructionID");
                var param = visitID + "|" + id;
                var url = "";
                var title = "Detail Catatan";

                if (isPostSurgeryInstruction == "True") {
                    param = visitID + "|" + testOrderID + "|" + postSurgeryInstructionID;
                    url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPostSurgeryInstructionCtl1.ascx");
                    title = "Instruksi Paska Bedah";
                }
                else {
                    param = visitID + "|" + testOrderID + "|" + postSurgeryInstructionID;
                    url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPostSurgeryInstructionCtl1.ascx");
                    title = "Instruksi Paska Bedah";
                }
                openUserControlPopup(url, param, title, 1300, 600);
            });

            $('.btnViewDetail2').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var testOrderID = "";
                var nutritionAssessmentID = $(this).attr("nutritionAssessmentID");
                var param = visitID + "|" + id;
                var url = "";
                var title = "Detail Catatan";

                param = visitID + "|" + nutritionAssessmentID;
                url = ResolveUrl("~/libs/Controls/EMR/Nutritionist/Assessment/NTNutritionistAssessmentCtl1.ascx");
                title = "Detail Kajian";
                openUserControlPopup(url, param, title, 1300, 600);
            });

            //#region Verify
            $('.btnVerified').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                $('#<%=hdnRowIndex.ClientID %>').val($(this).closest("tr").prevAll("tr").length);

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
            //#endregion

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
                $('#<%=hdnRowIndex.ClientID %>').val($(this).closest("tr").prevAll("tr").length);

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
                $('#<%=hdnRowIndex.ClientID %>').val($(this).closest("tr").prevAll("tr").length);

                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var confirmationRemarks = $tr.find('.confirmationRemarks').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "2" + "|" + confirmationRemarks);

                var url = ResolveUrl("~/Libs/Controls/ConfirmWithNoteCtl.ascx");
                openUserControlPopup(url, data, 'Konfirmasi', 400, 300);
            });
            $('.btnNote2').live('click', function () {
                $tr = $(this).parent().parent().parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                $('#<%=hdnRowIndex.ClientID %>').val($(this).closest("tr").prevAll("tr").length);

                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var remarks = $tr.find('.verificationRemarks').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "4" + "|" + remarks);

                var url = ResolveUrl("~/Libs/Controls/ConfirmWithNoteCtl.ascx");
                openUserControlPopup(url, data, 'Verifikasi', 400, 300);
            });
            //#endregion

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

            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $("#<%=grdView.ClientID %> tr:eq(1)").click();
        });

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

        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnPatientNoteType.ClientID %>').val() == Constant.SOAPNoteType.SOAP_SUMMARY_NOTES) {
                if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnCurrentSessionID.ClientID %>').val()) {
                    if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
                        var messageBody = "Maaf, Catatan sudah dikonfirmasi atau diverifikasi oleh Dokter, tidak bisa diubah lagi.";
                        displayErrorMessageBox('Catatan Terintegrasi', messageBody);
                        return false;
                    }
                    else if ($('#<%=hdnIsNoteFromOtherProcess.ClientID %>').val() == "True") {
                        displayErrorMessageBox('Catatan Terintegrasi', 'Catatan ini merupakan salinan dari proses lain, hanya bisa diubah dari proses asal');
                        return false;
                    }
                    else
                        return true;
                }
                else {
                    var messageBody = "Maaf, tidak diijinkan mengedit catatan user lain.";
                    displayErrorMessageBox('Catatan Terintegrasi', messageBody);
                    return false;
                }
            }
            else {
                var messageBody = "Maaf, Catatan Pengkajian Awal Dokter tidak bisa diubah melalui menu ini";
                displayErrorMessageBox('Catatan Terintegrasi', messageBody);
                return false;
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        $('#imgVisitNote.imgLink').live('click', function () {
            $(this).closest('tr').click();
            var id = $('#<%=hdnID.ClientID %>').val();
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
            $('#<%=hdnIsHasNeedConfirmation.ClientID %>').val(s.cpRetval);
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    SelectLastIndex();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            SelectLastIndex();
            //////$("#<%=grdView.ClientID %> tr:eq(" + $('#<%=hdnRowIndex.ClientID %>').val() + ")").click();
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            var patientReferralFormID = "";

            if (hdnID == '' || hdnID == '0') {
                if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00160' || code == 'PM-00524' || code == 'PM-00618' || code == 'PM-00645' || code == 'MR000045' || code == 'PM-00700') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'MR000013' || code == 'MR000017' || code == 'PM-00618') {
                    filterExpression.text = visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                    return false;
                }
            }
            else if (code == 'MR000003' || code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019' || code == 'MR000037' || code == 'MR000039'
                     || code == 'PM-90009' || code == 'PM-00618') {
                filterExpression.text = visitID;
                return true;
            }
            else if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00522' || code == 'PM-00523' || code == 'PM-00524' || code == 'PM-00564' || code == 'PM-00159'
                     || code == 'PM-00570' || code == 'PM-00618' || code == 'PM-00644' || code == 'PM-00645' || code == 'PM-00661' || code == 'MR000045' || code == 'PM-00700') {
                filterExpression.text = registrationID;
                return true;
            } 
            else if (code == 'PM-00571' || code == 'PM-00583') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            }
            else if (code == 'PM-00146' || code == 'PM-00147') {
                Methods.getObject("GetvPatientReferralFormList", "VisitID = " + visitID, function (result) {
                    if (result != null) {
                        patientReferralFormID = result.ID;
                    }
                });

                if (patientReferralFormID == null || patientReferralFormID == "" || patientReferralFormID == "0") {
                    errMessage.text = 'Pasien tidak memiliki Konsultasi Rawat Bersama';
                    return false;
                }
                else {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'keteranganIstirahat1' || code == 'kesehatanMata1' || code == 'keteranganSehat1' || code == 'keteranganDokter'
                || code == 'rujukanrslain' || code == 'keteranganVaksin' || code == 'keteranganButaWarna' || code == 'keteranganSehat2') {
                return $('#<%:hdnVisitID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
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
                cbpView.PerformCallback('changepage|' + $('#<%=hdnPageIndex.ClientID %>').val());
                //////$("#<%=grdView.ClientID %> tr:eq(" + $('#<%=hdnRowIndex.ClientID %>').val() + ")").click();
            }
            else {
                displayErrorMessageBox('VERIFIKASI', param[1]);
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
        function oncboDisplay(){
            $('#<%=hdnLastIndexSelected.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
       }
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style>
     
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsNoteFromOtherProcess" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnPageIndex" value="1" />
    <input type="hidden" value="1" id="hdnRowIndex" runat="server"/>
    <input type="hidden" value="0" id="hdnIsUseSignature" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasNeedConfirmation" runat="server" />
    <input type="hidden" runat="server" id="hdnIsShowVerifyButton" value="0" />
    <input type="hidden" value="0" id="hdnIsDPJPPhysician" runat="server" />
    <input type="hidden" value="1" id="hdnIsAllowAdd" runat="server" />
     <input type="hidden" value="" id="hdnLastIndexSelected" runat="server" />
    <input type="hidden" value="0" id="hdnEM0058" runat="server" />
    <div style="position: relative; width: 100%" id="area">
        <div id="filterArea">
            <table style="margin-top: 10px; margin-bottom: 10px">
                <tr>
                    <td class="tdLabel" style="width: 150px">
                        <label>
                            <%=GetLabel("Pilihan Tampilan :")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                            Width="300px">
                            <ClientSideEvents ValueChanged="function() { oncboDisplay();  }" />
                        </dxe:ASPxComboBox>
                    </td>
                    <td class="tdLabel">
                        <label class="lblNormal" style="font-weight: bold">
                            <%=GetLabel("Tanggal ")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <%=GetLabel("s/d") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true"
                            OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="DepartmentID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn departmentID" />
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                <asp:BoundField DataField="GCPatientNoteType" HeaderStyle-CssClass="controlColumn"
                                    ItemStyle-CssClass="controlColumn" />
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:TemplateField HeaderText="PPA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div>
                                            <%#:Eval("cfPPA") %>
                                        </div>
                                        <div>
                                            <img class="imgNeedConfirmation" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                alt="" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                cursor: pointer; min-width: 30px;' title="Perlu Konfirmasi" />
                                        </div>
                                        <div>
                                            <img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/notification.png")%>'
                                                alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                cursor: pointer; min-width: 30px; float: left;' title="Notifikasi Catatan Terintegrasi Ke Unit Pelayanan" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                                            <%#Eval("NoteText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                        <div id="divView" runat="server" style='margin-top: 5px;'>
                                            <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Detail Kajian Awal"
                                                style='width: 150px; background-color: Green; color: White;' recordID = '<%#:Eval("ID") %>' paramedicMasterType = '<%#:Eval("GCParamedicMasterType") %>' 
                                                chiefComplaintID = '<%#Eval("ChiefComplaintID") %>' nurseChiefComplaintID = '<%#Eval("NurseChiefComplaintID") %>'  />
                                        </div>
                                        <div id="divViewDetail" runat="server" style='margin-top: 5px;'>
                                            <input type="button" id="btnViewDetail" runat="server" class="btnViewDetail w3-btn w3-hover-blue" value="Lihat Detail"
                                                style='width: 150px; background-color: Green; color: White;' recordID = '<%#:Eval("ID") %>' isPostSurgeryInstruction = '<%#Eval("cfIsPostSurgeryInstruction") %>' postSurgeryInstructionID = '<%#Eval("PostSurgeryInstructionID") %>'  />
                                        </div>
                                        <div id="divViewDetail2" runat="server" style='margin-top: 5px;'>
                                            <input type="button" id="btnViewDetail2" runat="server" class="btnViewDetail2 w3-btn w3-hover-blue" value="Lihat Detail"
                                                style='width: 150px; background-color: Green; color: White;' recordID = '<%#:Eval("ID") %>' paramedicMasterType = '<%#:Eval("GCParamedicMasterType") %>' 
                                                paramedicID = '<%#Eval("ParamedicID") %>' nutritionAssessmentID = '<%#Eval("NutritionAssessmentID")%>'  />
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
                                                <tr id="divConfirmationInfo" runat="server" >
                                                    <td>
                                                        <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                            <span style='color: red;'>Konfirmasi : </span>
                                                            <span style='color: Blue;'>                                                    
                                                                <%#:Eval("cfConfirmationDateTime") %>, <%#:Eval("ConfirmationPhysicianName") %></span>
                                                                <div id="divConfirmationRemarks">
                                                                    <br />
                                                                    <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "ConfirmationRemarks") %> </textarea>
                                                                </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="float: right; <%# Eval("IsPanicValueReporting").ToString() == "False" ? "display:none": "" %>">
                                                            <asp:CheckBox ID="chkIsPanicValueReporting" runat="server" Enabled="false" Checked='<%# Eval("IsPanicValueReporting")%>' /> Pelaporan Nilai Kritis
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="float: right; <%# Eval("IsRMOHandsover").ToString() == "False" ? "display:none": "" %>">
                                                            <asp:CheckBox ID="chkIsRMOHandsover" runat="server" Enabled="false" Checked='<%# Eval("IsRMOHandsover")%>' /> Catatan Hand over Dokter Jaga
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %>'>
                                            <br />
                                            <span style='color: red;'>Konfirmasi Notifikasi : </span>
                                            <br />
                                            <span style='color: Blue;'>
                                                <%#:Eval("cfNotificationConfirmedDateTime") %>
                                                <br />
                                                <%#:Eval("NotificationUserName") %></span>
                                            <br />
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
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
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
                                                    <span style='color: red'>Perlu Konfirmasi :
                                                        <br />
                                                    </span><span style='color: Blue'>
                                                        <%#:Eval("ConfirmationPhysicianName") %></span>
                                                </div>
                                            </div>
                                            <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                    <span style='color: red;'>Diverifikasi :</span>
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
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn cfIsEditable" />
                                <asp:BoundField DataField="cfIsNoteFromOtherProcess" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isNoteFromOtherProcess" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan terintegrasi untuk pasien ini") %>
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
