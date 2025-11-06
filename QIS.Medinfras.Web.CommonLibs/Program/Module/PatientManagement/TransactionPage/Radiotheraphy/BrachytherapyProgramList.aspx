<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="BrachytherapyProgramList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.BrachytherapyProgramList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
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
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientallergylist">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').live('click', function () {
                cbpView.PerformCallback('refresh');
            });
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnProgramID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                $('#<%=hdnTotalFraction.ClientID %>').val($(this).find('.totalFraction').html());
                if ($('#<%=hdnProgramID.ClientID %>').val() != "") {
                    cbpViewDt2.PerformCallback('refresh');
                    setTimeout(function () { cbpViewDt1.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt3.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt4.PerformCallback('refresh'); }, 1000);
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                switch ($contentID) {
                    case "safetyCheck":
                        cbpViewDt2.PerformCallback('refresh');
                        break;
                    case "radiationLog":
                        cbpViewDt1.PerformCallback('refresh');
                        break;
                    case "procedureReport":
                        cbpViewDt4.PerformCallback('refresh');
                        break;
                    case "monitoringVitalSign":
                        cbpViewDt3.PerformCallback('refresh');
                        break;
                    default:
                        break;
                }
            });
            //#endregion
        });

        $('#<%=grdViewDt1.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt1.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnProgramLogID.ClientID %>').val($(this).find('.keyField').html());
        });

        $('#<%=grdViewDt3.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt3.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnIntraProgramLogID.ClientID %>').val($(this).find('.keyField').html());
            if ($('#<%=hdnIntraProgramLogID.ClientID %>').val() != "") {
                cbpViewDt3_1.PerformCallback('refresh');
            }
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshLogGrid() {
            cbpViewDt1.PerformCallback('refresh');
        }

        function onRefreshVitalSignGrid() {
            cbpViewDt3_1.PerformCallback('refresh');
        }

        function onRefreshReportGrid() {
            cbpViewDt4.PerformCallback('refresh');
        }

        //#region View Program
        $('.imgViewProgram.imgLink').die('click');
        $('.imgViewProgram.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewRTProgramCtl.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
            openUserControlPopup(url, param, "Program Brakiterapi", 800, 500);
        });
        //#endregion

        //#region Safety Check List
        function onRefreshSafetyCheckViewGrid() {
            cbpViewDt2.PerformCallback('refresh');
        }

        $('.imgDeleteSafetyCheck.imgLink').die('click');
        $('.imgDeleteSafetyCheck.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteSafetyCheckList.PerformCallback(recordID);
                }
            });
        });

        $('.imgAddSafetyCheckList.imgLink').die('click');
        $('.imgAddSafetyCheckList.imgLink').live('click', function (evt) {
            addSafetyCheckList();
        });

        $('#lblAddSafetyChecklistForm').die('click');
        $('#lblAddSafetyChecklistForm').live('click', function (evt) {
            addSafetyCheckList();
        });

        function addSafetyCheckList() {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/SignInFormEntry.ascx");
                var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|||||||||" + "||" + $('#<%=hdnTotalFraction.ClientID %>').val();
                openUserControlPopup(url, param, "Safety Check List - SIGN IN", 700, 500);
            }         
        }

        $('#lblAddSignInSafetyChecklistForm').die('click');
        $('#lblAddSignInSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/SignInFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var fractionNo = $(this).attr('fractionNo');
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|||||||||" + "|" + fractionNo + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
            openUserControlPopup(url, param, "Safety Check List - SIGN IN", 700, 500);
        });

        $('#lblAddTimeOutSafetyChecklistForm').die('click');
        $('#lblAddTimeOutSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/TimeOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var fractionNo = $(this).attr('fractionNo');
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|||||||||" + "|" + fractionNo + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
            openUserControlPopup(url, param, "Safety Check List - TIME OUT", 900, 500);
        });

        $('#lblAddSignOutSafetyChecklistForm').die('click');
        $('#lblAddSignOutSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/SignOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var fractionNo = $(this).attr('fractionNo');
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|||||||||||" + "|" + fractionNo + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
            openUserControlPopup(url, param, "Safety Check List - SIGN OUT", 700, 500);
        });

        $('.imgEditSignIn.imgLink').die('click');
        $('.imgEditSignIn.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/SignInFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var programID = $(this).attr('programID');
            var signInDate = $(this).attr('signInDate');
            var signInTime = $(this).attr('signInTime');
            var paramedicID = $(this).attr('paramedicID');
            var physicianID = $(this).attr('physicianID');
            var anesthetistID = $(this).attr('anesthetistID');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var isVerified = $(this).attr('isSignInVerified');
            var fractionNo = $(this).attr('fractionNo');
            var totalFraction = $('#<%=hdnTotalFraction.ClientID %>').val();
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                if (isVerified == "True") {
                    displayErrorMessageBox("Safety Check List", "Sudah dilakukan verifikasi oleh Dokter Operator/Anestesi tidak dapat dilakukan perubahan");
                    return false;
                }
                else {
                    var param = recordID + "|" + programID + "|" + signInDate + "|" + signInTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + paramedicID + "|" + layout + "|" + values + "||" + fractionNo + "|" + totalFraction;
                    openUserControlPopup(url, param, "Safety Check List - SIGN IN", 700, 500);
                }
            }
            else {
                displayErrorMessageBox('Safefy Check List', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewSignIn.imgLink').die('click');
        $('.imgViewSignIn.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewSignInCtl.ascx");
            var recordID = $(this).attr('recordID');
            var programID = $(this).attr('programID');
            var signInDate = $(this).attr('signInDate');
            var signInTime = $(this).attr('signInTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var paramedicName = $(this).attr('paramedicName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var fractionNo = $(this).attr('fractionNo');
            var param = recordID + "|" + programID + "|" + signInDate + "|" + signInTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + paramedicName + "|" + layout + "|" + values + "|" + fractionNo;
            openUserControlPopup(url, param, "Safety Check List - SIGN IN", 700, 500);
        });

        $('.imgEditTimeOut.imgLink').die('click');
        $('.imgEditTimeOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/TimeOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var programID = $(this).attr('programID');
            var timeOutDate = $(this).attr('timeOutDate');
            var timeOutTime = $(this).attr('timeOutTime');
            var paramedicID = $(this).attr('paramedicID');
            var physicianID = $(this).attr('physicianID');
            var anesthetistID = $(this).attr('anesthetistID');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var isVerified = $(this).attr('isTimeOutVerified');
            var fractionNo = $(this).attr('fractionNo');
            var totalFraction = $('#<%=hdnTotalFraction.ClientID %>').val();
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                if (isVerified == "True") {
                    displayErrorMessageBox("Safety Check List", "Sudah dilakukan verifikasi oleh Dokter Operator/Anestesi tidak dapat dilakukan perubahan");
                    return false;
                }
                else {
                    var param = recordID + "|" + programID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + paramedicID + "|" + layout + "|" + values + "||" + fractionNo + "|" + totalFraction;
                    openUserControlPopup(url, param, "Safety Check List - TIME OUT", 900, 500);
                }
            }
            else {
                displayErrorMessageBox('Safefy Check List', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewTimeOut.imgLink').die('click');
        $('.imgViewTimeOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewTimeOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var programID = $(this).attr('programID');
            var timeOutDate = $(this).attr('timeOutDate');
            var timeOutTime = $(this).attr('timeOutTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var paramedicName = $(this).attr('paramedicName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var fractionNo = $(this).attr('fractionNo');
            var param = recordID + "|" + programID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + paramedicName + "|" + layout + "|" + values + "|" + fractionNo;
            openUserControlPopup(url, param, "Safety Check List - TIME OUT", 900, 500);
        });

        $('.imgEditSignOut.imgLink').die('click');
        $('.imgEditSignOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/SignOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var programID = $(this).attr('programID');
            var signOutDate = $(this).attr('signOutDate');
            var signOutTime = $(this).attr('signOutTime');
            var surgeryEndDate = $(this).attr('surgeryEndDate');
            var surgeryEndTime = $(this).attr('surgeryEndTime');
            var paramedicID = $(this).attr('paramedicID');
            var physicianID = $(this).attr('physicianID');
            var anesthetistID = $(this).attr('anesthetistID');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var isVerified = $(this).attr('isSignOutVerified');
            var fractionNo = $(this).attr('fractionNo');
            var totalFraction = $('#<%=hdnTotalFraction.ClientID %>').val();
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                if (isVerified == "True") {
                    displayErrorMessageBox("Safety Check List", "Sudah dilakukan verifikasi oleh Dokter Operator/Anestesi tidak dapat dilakukan perubahan");
                    return false;
                }
                else {
                    var param = recordID + "|" + programID + "|" + signOutDate + "|" + signOutTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + paramedicID + "|" + layout + "|" + values + "|" + surgeryEndDate + "|" + surgeryEndTime + "||" + fractionNo + "|" + totalFraction;
                    openUserControlPopup(url, param, "Safety Check List - SIGN OUT", 700, 500);
                }
            }
            else {
                displayErrorMessageBox('Safefy Check List', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewSignOut.imgLink').die('click');
        $('.imgViewSignOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewSignOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var programID = $(this).attr('programID');
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
            var fractionNo = $(this).attr('fractionNo');
            var param = recordID + "|" + programID + "|" + signOutDate + "|" + signOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + paramedicName + "|" + layout + "|" + values + "|" + surgeryEndDate + "|" + surgeryEndTime + "|" + fractionNo;
            openUserControlPopup(url, param, "Safety Check List - SIGN OUT", 700, 500);
        });

        $('.imgDeleteSignIn.imgLink').die('click');
        $('.imgDeleteSignIn.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Sign In dari Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteCheckListInfo.PerformCallback("1" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteTimeOut.imgLink').die('click');
        $('.imgDeleteTimeOut.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Time Out dari Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteCheckListInfo.PerformCallback("2" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteSignOut.imgLink').die('click');
        $('.imgDeleteSignOut.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Sign Out dari Safety Check List untuk pasien ini ?";
            displayConfirmationMessageBox("Safety Check List", message, function (result) {
                if (result) {
                    cbpDeleteCheckListInfo.PerformCallback("3" + "|" + recordID);
                }
            });
        });
        //#endregion

        //#region Pemantauan

        function onBeforeEditIntraRecord(paramedicID) {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('MEDINFRAS', 'Maaf, tidak diijinkan mengedit/menghapus record user lain.');
                return false;

            }
        }
        $('#lblAddIntra').die('click');
        $('#lblAddIntra').live('click', function (evt) {
            addIntraMonitoringRecord();
        });

        $('.imgAddIntra.imgLink').die('click');
        $('.imgAddIntra.imgLink').live('click', function (evt) {
            addIntraMonitoringRecord();
        });

        function addIntraMonitoringRecord() {
            var allow = false;
            var title = "Pemantauan Observasi";
            var monitoringType = "X487^005";
            if ($('#<%=hdnIntraProgramLogID.ClientID %>').val() != "" && $('#<%=hdnIntraProgramLogID.ClientID %>').val() != "0") {
                allow = true;
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "09" + "|" + $('#<%=hdnIntraProgramLogID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, title, 700, 500, monitoringType);
            }
            else {
                displayErrorMessageBox('Data Pemantauan', "Data Pemantauan hanya bisa diisi jika ada record/data catatan radiasi.");
            }
        }

        $('.imgEditIntraVitalSign.imgLink').die('click');
        $('.imgEditIntraVitalSign.imgLink').live('click', function (evt) {
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditIntraRecord(paramedicID)) {
                var recordID = $(this).attr('recordID');
                var title = "Pemantauan Observasi";
                var monitoringType = "X487^005";

                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "09" + "|" + $('#<%=hdnIntraProgramLogID.ClientID %>').val() + "|" + recordID + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, title, 700, 500, monitoringType);
            }
        });

        $('.imgDeleteIntraVitalSign.imgLink').die('click');
        $('.imgDeleteIntraVitalSign.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditIntraRecord(paramedicID)) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi data pemantauan observasi untuk pasien ini ?";
                displayConfirmationMessageBox("Data Pemantauan Observasi", message, function (result) {
                    if (result) {
                        cbpDeleteIntra.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgCopyIntraVitalSign.imgLink').die('click');
        $('.imgCopyIntraVitalSign').live('click', function () {
            var allow = false;
            if ($('#<%=hdnIntraProgramLogID.ClientID %>').val() != "" && $('#<%=hdnIntraProgramLogID.ClientID %>').val() != "0") {
                allow = true;
            }
            if (allow) {
                var recordID = $(this).attr('recordID');
                var message = "Lakukan copy indikator data pemantauan observasi ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var monitoringType = "X487^005";
                        var param = "09" + "||" + recordID + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "||" + $('#<%=hdnIntraProgramLogID.ClientID %>').val();
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/CopyVitalSignCtl1.ascx");
                        openUserControlPopup(url, param, 'Copy Pemantauan Indikator', 700, 500, monitoringType);
                    }
                });
            }
            else {
                displayErrorMessageBox('Data Pemantauan', "Data Pemantauan hanya bisa diisi jika ada record/data catatan radiasi");
            }
        });
        //#endregion

        //#region Laporan Tindakan
        $('#lblAddReport').die('click');
        $('#lblAddReport').live('click', function (evt) {
            addReport();
        });

        $('.imgAddReport2.imgLink').die('click');
        $('.imgAddReport2.imgLink').live('click', function (evt) {
            addReport();
        });

        function addReport() {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ProcedureReportEntryCtl.ascx");
            var recordID = $(this).attr('recordID');
            var fractionNo = $(this).attr('fractionNo');
            var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val() + "|" + $('#<%=hdnIsEMR.ClientID %>').val();
            openUserControlPopup(url, param, "Laporan Tindakan Brakiterapi", 800, 500);         
        }

        $('.imgEditReport.imgLink').die('click');
        $('.imgEditReport.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ProcedureReportEntryCtl.ascx");
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDelete(paramedicID)) {
                var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val() + "|" + $('#<%=hdnIsEMR.ClientID %>').val();
                openUserControlPopup(url, param, "Laporan Tindakan Brakiterapi", 800, 500);
            } 
        });

        $('.imgDeleteReport.imgLink').die('click');
        $('.imgDeleteReport.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDelete(paramedicID)) {
                var message = "Hapus informasi laporan tindakan untuk pasien ini ?";
                displayConfirmationMessageBox("Laporan Tindakan", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpDeleteReport.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgViewReport.imgLink').die('click');
        $('.imgViewReport.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewProcedureReportCtl.ascx");
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
            openUserControlPopup(url, param, "Laporan Tindakan Brakiterapi", 800, 500);
        });
        //#endregion

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

        function onCbpViewDt1EndCallback(s) {
            $('#containerImgLoadingViewDt1').hide();

            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();
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
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount1, function (page) {
                    cbpViewDt3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt3_1EndCallback(s) {
            $('#containerImgLoadingViewDt3_1').hide();

            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt3_1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3_1"), pageCount1, function (page) {
                    cbpViewDt3_1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3_1.ClientID %> tr:eq(1)').click();
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

        function oncbpDeleteReportEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt4.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Laporan Tindakan', param[1]);
            }
        }

        function oncbpDeleteIntraEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3_1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Data Pemantauan Alat - Tanda Vital dan Indikator Lainnya', param[1]);
            }
        }

        $('.imgAddRequest.imgLink').die('click');
        $('.imgAddRequest.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
                openUserControlPopup(url, param, "Permintaan Simulasi", 800, 500, "");
            }
        });

        $('.imgEditRequest.imgLink').die('click');
        $('.imgEditRequest.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestEntryCtl.ascx");
                var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
                openUserControlPopup(url, param, "Permintaan Simulasi", 800, 500, "");
            }
        });

        $('.imgAddRadiotheraphyLog.imgLink').die('click');
        $('.imgAddRadiotheraphyLog.imgLink').live('click', function (evt) {
            addRadiotheraphyLog();
        });

        $('#lblAddProgramLog').die('click');
        $('#lblAddProgramLog').live('click', function (evt) {
            addRadiotheraphyLog();
        });

        function addRadiotheraphyLog() {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var allow = true;
                if (allow) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/BrachytherapyProgramLogEntryCtl.ascx");
                    var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
                    openUserControlPopup(url, param, "Catatan Harian Brakiterapi", 700, 550, "");
                }
            }
            else {
                displayErrorMessageBox("Catatan Harian Brakiterapi", "Catatan Harian Brakiterapi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        }

        $('.imgEditRadiotheraphyLog.imgLink').die('click');
        $('.imgEditRadiotheraphyLog.imgLink').live('click', function (evt) {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var allow = true;
                if (allow) {
                    var recordID = $(this).attr('recordID');
                    var paramedicID = $(this).attr('paramedicID');
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/BrachytherapyProgramLogEntryCtl.ascx");
                    var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
                    openUserControlPopup(url, param, "Catatan Harian Brakiterapi", 700, 500, "");
                }
            }
            else {
                displayErrorMessageBox("Catatan Harian Brakiterapi", "Catatan Harian Brakiterapi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgDeleteRadiotheraphyLog.imgLink').die('click');
        $('.imgDeleteRadiotheraphyLog.imgLink').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var message = "Hapus informasi catatan harian untuk pasien ini ?";
                    displayConfirmationMessageBox("Catatan Harian", message, function (result) {
                        if (result) {
                            var param = recordID;
                            cbpDeleteLog.PerformCallback(param);
                        }
                    });
                }
            }
            else {
                displayErrorMessageBox("Catatan Harian Brakiterapi", "Catatan Harian Brakiterapi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgCopyRadiotheraphyLog.imgLink').die('click');
        $('.imgCopyRadiotheraphyLog').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var message = "Lakukan copy catatan harian ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/BrachytherapyProgramLogEntryCtl.ascx");
                        var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + recordID;
                        openUserControlPopup(url, param, "Catatan Harian", 700, 500, "");
                    }
                });
            }
            else {
                displayErrorMessageBox("Catatan Harian Brakiterapi", "Catatan Harian Brakiterapi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgViewLog.imgLink').die('click');
        $('.imgViewLog.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewBrachytherapyProgramLogCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
            openUserControlPopup(url, param, "Catatan Harian", 800, 500, "");
        });

        function onCbpDeleteLogEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Penyinaran Onkologi Radiasi', param[1]);
            }
        }
        //#endregion

        function onBeforeEditDelete(paramedicID) {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, pengkajian hanya bisa diubah/dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onCbpDeleteCheckListInfoEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Safety Check List', param[1]);
            }
        }

        function onCbpDeleteSafetyCheckListEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Safety Check List', param[1]);
            }
        }

        $('.imgVerification1.imgLink').die('click');
        $('.imgVerification1.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            if (onBeforeVerification()) {
                var message = "Lakukan proses verifikasi oleh Dokter ?";
                displayConfirmationMessageBox("Verifikasi Dokter", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpVerification.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgCancelVerification1.imgLink').die('click');
        $('.imgCancelVerification1.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            if (onBeforeVerification()) {
                var message = "Lakukan proses batal verifikasi Dokter ?";
                displayConfirmationMessageBox("Batal Verifikasi Dokter", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpCancelVerification.PerformCallback(param);
                    }
                });
            }
        });

        function onBeforeVerification() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('Verifikasi Catatan Simulasi', 'Maaf, Verifikasi dan Batal Verifikasi hanya bisa dilakukan oleh Dokter yang mengajukan permintaan simulasi.');
                return false;
            }
        }

        function onCbpVerificationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Verifikasi Dokter', param[1]);
            }
        }

        function onCbpCancelVerificationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Batal Verifikasi Dokter', param[1]);
            }
        }
    </script>

    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnProgramID" runat="server" />
    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnProgramLogID" runat="server" />
    <input type="hidden" value="" id="hdnTotalFraction" runat="server" />
    <input type="hidden" value="0" id="hdnPageVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPageMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnSubMenuType" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <input type="hidden" value="" id="hdnSimulationRequestID" runat="server" />
    <input type="hidden" runat="server" id="hdnIntraProgramLogID" value="0" />
    <input type="hidden" runat="server" id="hdnHasIntraVitalSignRecord" value="0" />
    <input type="hidden" runat="server" id="hdnProcedureReportID" value="0" />
    <input type="hidden" value="0" id="hdnIsEMR" runat="server" />

    <table style="width: 100%">
        <colgroup>
            <col style="width: 26%" />
            <col style="width: 74%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ProgramID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn paramedicID" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                            <asp:BoundField DataField="cfTotalFraction" HeaderStyle-CssClass="hiddenColumn totalFraction" ItemStyle-CssClass="hiddenColumn totalFraction" />
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgViewProgram imgLink" title='<%=GetLabel("Lihat Program")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("ProgramID") %>"
                                                        visitid="<%#:Eval("VisitID") %>" programID="<%#:Eval("ProgramID") %>" />                                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfProgramDate" HeaderText="Tanggal" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:TemplateField HeaderText="Tipe Radioterapi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="font-weight:bold"><%#Eval("TherapyType")%></span>
                                                    </div>
                                                    <div style="<%# Eval("GCTherapyType").ToString() != "X582^002" ? "display:none": "" %>">
                                                        <span style="font-style:italic">Jenis : </span><span style="font-weight:bold"><%#Eval("BrachytherapyType")%></span>
                                                    </div>
                                                    <div style="<%# Eval("GCTherapyType").ToString() != "X582^002" ? "display:none": "" %>">
                                                        <span style="font-style:italic">Aplikator : </span><span style="font-weight:bold"><%#Eval("ApplicatorType")%></span>
                                                    </div>
                                                    <div>
                                                        <span style="font-style:italic">Dosis : </span><br />
                                                        <span style="font-weight:bold"><%#Eval("cfDosageInfo")%></span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Status" DataField="ProgramStatus" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="programStatus" />
                                            <asp:TemplateField HeaderText="Catatan Tambahan" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <div style="height: auto; max-height:150px; overflow-y: auto;">
                                                        <%#Eval("Remarks").ToString().Replace("\n","<br />")%><br />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada program radioterapi untuk pasien ini")%>
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
                                    <li contentid="safetyCheck"  class="selected">
                                        <%=GetLabel("Safety Check List")%></li>
                                    <li contentid="procedureReport">
                                        <%=GetLabel("Laporan Brakiterapi")%></li>
                                    <li contentid="radiationLog">
                                        <%=GetLabel("Catatan Radiasi")%></li>
                                    <li contentid="monitoringVitalSign">
                                        <%=GetLabel("Pemantauan Observasi")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="safetyCheck">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContentChecklist" runat="server">
                                            <asp:Panel runat="server" ID="PanelChecklist" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddSafetyCheckList imgLink" title='<%=GetLabel("+ Safety CheckList")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID="<%#:Eval("ID") %>" programID="<%#:Eval("ProgramID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-top:4px">
                                                                    <tr>
                                                                        <td style="text-align:center">
                                                                            <img class="imgDeleteSafetyCheck imgLink" title='<%=GetLabel("Delete Check List")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="FractionNo" HeaderText = "Fraksi Ke-" HeaderStyle-CssClass="fractionNo" ItemStyle-CssClass="fractionNo" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"   />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="SIGN IN" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfSignInDateTime") %></div>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("SignInParamedicName") %></div>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Sign In Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddSignInSafetyChecklistForm" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>"  fractionNo = "<%#:Eval("FractionNo") %>">
                                                                                    <%= GetLabel("+ Sign In Check List")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditSignIn imgLink" title='<%=GetLabel("Edit Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" layout = "<%#:Eval("SignInLayout") %>" values = "<%#:Eval("SignInValues") %>" 
                                                                                            paramedicID = "<%#:Eval("SignInParamedicID") %>" physicianID = "<%#:Eval("SignOutPhysicianID") %>" anesthetistID = "<%#:Eval("SignOutAnesthetistID") %>" paramedicID = "<%#:Eval("SignOutParamedicID") %>"
                                                                                            signInDate = "<%#:Eval("cfSignInDate") %>" signInTime = "<%#:Eval("SignInTime") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" fractionNo = "<%#:Eval("FractionNo") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteSignIn imgLink" title='<%=GetLabel("Delete Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewSignIn imgLink" title='<%=GetLabel("Lihat Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" layout = "<%#:Eval("SignInLayout") %>" values = "<%#:Eval("SignInValues") %>" 
                                                                                            paramedicName = "<%#:Eval("SignInParamedicName") %>" physicianName = "<%#:Eval("SignOutPhysicianName") %>" anesthetistName = "<%#:Eval("SignOutAnesthetistName") %>" nurseName = "<%#:Eval("SignOutParamedicName") %>"
                                                                                            signInDate = "<%#:Eval("cfSignInDate") %>" signInTime = "<%#:Eval("SignInTime") %>" fractionNo = "<%#:Eval("FractionNo") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                             <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <table border="1" cellpadding="1" cellspacing="0" style="width:100%">
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
                                                                                                <span style="font-style:italic"><%#:Eval("cfSignInVerifiedByPhysician1Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align:center">
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
                                                                                                <span style="font-style:italic"><%#:Eval("cfSignInVerifiedByPhysician2Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align:center">
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
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="TIME OUT" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfTimeOutDateTime") %></div>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("TimeOutParamedicName") %></div>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Time Out Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddTimeOutSafetyChecklistForm" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" fractionNo = "<%#:Eval("FractionNo") %>">
                                                                                    <%= GetLabel("+ Time Out Check List")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditTimeOut imgLink" title='<%=GetLabel("Edit Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" layout = "<%#:Eval("TimeOutLayout") %>" values = "<%#:Eval("TimeOutValues") %>" 
                                                                                            paramedicID = "<%#:Eval("TimeOutParamedicID") %>" physicianID = "<%#:Eval("SignOutPhysicianID") %>" anesthetistID = "<%#:Eval("SignOutAnesthetistID") %>" paramedicID = "<%#:Eval("SignOutParamedicID") %>"
                                                                                            timeOutDate = "<%#:Eval("cfTimeOutDate") %>" timeOutTime = "<%#:Eval("TimeOutTime") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" fractionNo = "<%#:Eval("FractionNo") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteTimeOut imgLink" title='<%=GetLabel("Delete Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewTimeOut imgLink" title='<%=GetLabel("Lihat Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" layout = "<%#:Eval("TimeOutLayout") %>" values = "<%#:Eval("TimeOutValues") %>" 
                                                                                            paramedicName = "<%#:Eval("TimeOutParamedicName") %>" physicianName = "<%#:Eval("SignOutPhysicianName") %>" anesthetistName = "<%#:Eval("SignOutAnesthetistName") %>" nurseName = "<%#:Eval("SignOutParamedicName") %>"
                                                                                            timeOutDate = "<%#:Eval("cfTimeOutDate") %>" timeOutTime = "<%#:Eval("TimeOutTime") %>" fractionNo = "<%#:Eval("FractionNo") %>"  />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                             <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <table border="1" cellpadding="1" cellspacing="0" style="width:100%">
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
                                                                                                <span style="font-style:italic"><%#:Eval("cfTimeOutVerifiedByPhysician1Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align:center">
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
                                                                                                <span style="font-style:italic"><%#:Eval("cfTimeOutVerifiedByPhysician2Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align:center">
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
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="SIGN OUT" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfSignOutDateTime") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("SignOutParamedicName") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Sign Out Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddSignOutSafetyChecklistForm" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>"  fractionNo = "<%#:Eval("FractionNo") %>">
                                                                                    <%= GetLabel("+ Sign Out Check List")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditSignOut imgLink" title='<%=GetLabel("Edit Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" layout = "<%#:Eval("SignOutLayout") %>" values = "<%#:Eval("SignOutValues") %>" 
                                                                                            paramedicID = "<%#:Eval("SignOutParamedicID") %>" physicianID = "<%#:Eval("SignOutPhysicianID") %>" anesthetistID = "<%#:Eval("SignOutAnesthetistID") %>" paramedicID = "<%#:Eval("SignOutParamedicID") %>"
                                                                                            signOutDate = "<%#:Eval("cfSignOutDate") %>" signOutTime = "<%#:Eval("SignOutTime") %>" surgeryEndDate = "<%#:Eval("cfSurgeryEndDate") %>" surgeryEndTime = "<%#:Eval("SurgeryEndTime") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" fractionNo = "<%#:Eval("FractionNo") %>"  />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteSignOut imgLink" title='<%=GetLabel("Delete Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>"/>
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewSignOut imgLink" title='<%=GetLabel("Lihat Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" programID = "<%#:Eval("ProgramID") %>" layout = "<%#:Eval("SignOutLayout") %>" values = "<%#:Eval("SignOutValues") %>" 
                                                                                            paramedicName = "<%#:Eval("SignInParamedicName") %>" physicianName = "<%#:Eval("SignOutPhysicianName") %>" anesthetistName = "<%#:Eval("SignOutAnesthetistName") %>" paramedicName = "<%#:Eval("SignOutParamedicName") %>"
                                                                                            signOutDate = "<%#:Eval("cfSignOutDate") %>" signOutTime = "<%#:Eval("SignOutTime") %>" surgeryEndDate = "<%#:Eval("cfSurgeryEndDate") %>" surgeryEndTime = "<%#:Eval("SurgeryEndTime") %>" fractionNo = "<%#:Eval("FractionNo") %>"  />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                             <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <table border="1" cellpadding="1" cellspacing="0" style="width:100%">
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
                                                                                                <span style="font-style:italic"><%#:Eval("cfSignOutVerifiedByPhysician1Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align:center">
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
                                                                                                <span style="font-style:italic"><%#:Eval("cfSignOutVerifiedByPhysician2Info") %></span>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="text-align:center">
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
                                                            <div><%=GetLabel("Belum ada formulir Safety Check List untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddSafetyChecklistForm">
                                                                <%= GetLabel("+ Formulir Safety Check List")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt2"></div>
                                    </div>
                                </div> 
                            </div>
                           <div class="containerOrderDt" id="radiationLog"  style="display:none">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <div id="preVitalSign">
                                                <dxcp:ASPxCallbackPanel ID="cbpViewDt1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1"
                                                    ShowLoadingPanel="false" OnCallback="cbpViewDt1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-Width="140px">
                                                                            <HeaderTemplate>
                                                                                <img class="imgAddRadiotheraphyLog imgLink" title='<%=GetLabel("+ Catatan Harian")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                    alt="" recordID="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgEditRadiotheraphyLog imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" recordID="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgDeleteRadiotheraphyLog imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" recordID="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgCopyRadiotheraphyLog imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                        alt="" recordID="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img class="imgViewLog imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                        alt="" recordid="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgVerification1 imgLink hvr-pulse-grow" title='<%=GetLabel("Verifikasi Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                                                        alt="" recordid="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "False" ? "Style='display:none'":"" %> class="imgCancelVerification1 imgLink hvr-pulse-grow" title='<%=GetLabel("Batal Verifikasi Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                                                        alt="" recordid="<%#:Eval("BrachytherapyLogID") %>" programID="<%#:Eval("ProgramID") %>" /> 
                                                                                    <div <%# Eval("IsVerified").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                        <%#Eval("cfVerifiedInformation")%>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="BrachytherapyLogID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="FractionNo" HeaderText="Fraksi Ke-" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="LogTime" HeaderText="Jam" HeaderStyle-Width="60px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Petugas" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="BrachytherapyType" HeaderText="Jenis Brakiterapi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="ApplicatorType" HeaderText="Aplikator" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="TotalChannel" HeaderText="Jumlah Channel" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="TotalDosage" HeaderText="Dosis"  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
<%--                                                                        <asp:BoundField DataField="Wkt" HeaderText="GS" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />--%>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <div>
                                                                            <div>
                                                                                <%=GetLabel("Belum ada informasi catatan harian untuk program ini") %></div>
                                                                            <br />
                                                                            <span class="lblLink" id="lblAddProgramLog">
                                                                                <%= GetLabel("+ Catatan Harian")%></span>
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt1">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingDt1">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="containerOrderDt" id="monitoringVitalSign" style="display:none">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
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
                                                                    <asp:BoundField DataField="BrachytherapyLogID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField HeaderText="Fraksi Ke-" DataField="FractionNo" HeaderStyle-HorizontalAlign="Right"
                                                                        ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="fractionNo" />
                                                                    <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="LogTime" HeaderText="Jam" HeaderStyle-Width="60px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div class="blink">
                                                                            <%=GetLabel("Belum ada informasi catatan radiasi") %></div>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                <div class="wrapperPaging">
                                                </div>
                                                <div class="containerPaging">
                                                    <div id="pagingDt3">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <div id="Div3">
                                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3_1" runat="server" Width="100%" ClientInstanceName="cbpViewDt3_1"
                                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3_1').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDt3_1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent3_1" runat="server">
                                                            <asp:Panel runat="server" ID="Panel3_1" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdViewDt3_1" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt3_1_RowDataBound">
                                                                    <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="80px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddIntra imgLink" title='<%=GetLabel("+ Pemantauan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PatientETTLogID") %>" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div id="div1" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                <img class="imgEditIntraVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("RadiotherapyProgramLogID") %>" paramedicID="<%#:Eval("ParamedicID") %>"  />
                                                                                <img class="imgDeleteIntraVitalSign imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("RadiotherapyProgramLogID") %>" paramedicID="<%#:Eval("ParamedicID") %>"  />
                                                                                <img class="imgCopyIntraVitalSign imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("RadiotherapyProgramLogID") %>" />                                        
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="hiddenColumn keyUser" ItemStyle-CssClass="hiddenColumn keyUser" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <div>
                                                                                    Indikator Pemantauan Pemasangan Observasi</div>
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
                                                                                                    <div style="width: 110px; float: left; color: blue" class="labelColumn">
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
                                                                                <div>
                                                                                    <span style="font-weight:bold; text-decoration: underline; color:Black"><%=GetLabel("Catatan Tambahan :")%></span>
                                                                                    <br />
                                                                                    <span style="font-style:italic">
                                                                                        <%#: Eval("Remarks")%>
                                                                                    </span>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>                                                
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <div>
                                                                            <div><%=GetLabel("Belum ada informasi pemantauan tanda vital untuk data pemasangan ini") %></div>
                                                                            <br />
                                                                            <span class="lblLink" id="lblAddIntra">
                                                                                <%= GetLabel("+ Pemantauan")%></span>
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>   
                                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3_1" >
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingDt3_1"></div>
                                                    </div>
                                                </div>                                             
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="containerOrderDt" id="procedureReport" style="display: none">
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
                                                                    alt="" programID="<%#:Eval("ProgramID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditReport imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("BrachytherapyProcedureReportID") %>"
                                                                                programID="<%#:Eval("ProgramID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteReport imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicid="<%#:Eval("ParamedicID") %>" recordid="<%#:Eval("BrachytherapyProcedureReportID") %>"
                                                                                programID="<%#:Eval("ProgramID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgViewReport imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordid="<%#:Eval("BrachytherapyProcedureReportID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="BrachytherapyProcedureReportID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("BrachytherapyProcedureReportID") %>" bindingfield="BrachytherapyProcedureReportID" />
                                                                <input type="hidden" value="<%#:Eval("cfReportDate") %>" bindingfield="cfReportDate" />
                                                                <input type="hidden" value="<%#:Eval("ReportTime") %>" bindingfield="ReportTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                        
                                                        <asp:BoundField HeaderText="Fraksi Ke-" DataField="FractionNo" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfReportDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="ReportTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Pembuat Laporan" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Jenis Aplikator" DataField="ApplicatorType"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Panjang Intra uterine" DataField="IntrauterineLength"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Sudut Intra uterine" DataField="IntrauterineCorner"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Diameter/Silinder" DataField="Cylinder"
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteLog" runat="server" Width="100%" ClientInstanceName="cbpDeleteLog"
            ShowLoadingPanel="false" OnCallback="cbpDeleteLog_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteLogEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteCheckListInfo" runat="server" Width="100%" ClientInstanceName="cbpDeleteCheckListInfo"
            ShowLoadingPanel="false" OnCallback="cbpDeleteCheckListInfo_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteCheckListInfoEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteSafetyCheckList" runat="server" Width="100%" ClientInstanceName="cbpDeleteSafetyCheckList"
            ShowLoadingPanel="false" OnCallback="cbpDeleteSafetyCheckList_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteSafetyCheckListEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteIntra" runat="server" Width="100%" ClientInstanceName="cbpDeleteIntra"
            ShowLoadingPanel="false" OnCallback="cbpDeleteIntra_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteIntraEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteReport" runat="server" Width="100%" ClientInstanceName="cbpDeleteReport"
            ShowLoadingPanel="false" OnCallback="cbpDeleteReport_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteReportEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpVerification" runat="server" Width="100%" ClientInstanceName="cbpVerification"
            ShowLoadingPanel="false" OnCallback="cbpVerification_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVerificationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpCancelVerification" runat="server" Width="100%" ClientInstanceName="cbpCancelVerification"
            ShowLoadingPanel="false" OnCallback="cbpCancelVerification_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpCancelVerificationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
