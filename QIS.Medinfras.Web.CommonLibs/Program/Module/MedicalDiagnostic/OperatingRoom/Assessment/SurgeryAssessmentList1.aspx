<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="SurgeryAssessmentList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryAssessmentList1" %>

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
    <script type="text/javascript" id="dxss_MDSurgeryAssessmentList">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
                if ($('#<%=hdnID.ClientID %>').val() != "") {
                    cbpViewDt.PerformCallback('refresh');
                    cbpViewDt2.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                    cbpViewDt4.PerformCallback('refresh');
                    cbpViewDt5.PerformCallback('refresh');
                    cbpViewDt6.PerformCallback('refresh');
                    cbpViewDt7.PerformCallback('refresh');
                    cbpViewDt8.PerformCallback('refresh');
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
            });
            //#endregion
            registerCollapseExpandHandler();
        });

        //#region Asesmen Pra Bedah
        $('.imgViewPreSurgeryAssessment.imgLink').die('click');
        $('.imgViewPreSurgeryAssessment.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewAssesmentPreOpCtl1.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var testorderID = $(this).attr('testorderID');
            $('#<%=hdnAssessmentID.ClientID %>').val(recordID);
            var param = visitID + '|' + testorderID + '|' + recordID;
            openUserControlPopup(url, param, "Asesmen Pra Bedah", 1024, 500);
        });
        //#endregion

        //#region Asesmen Pra Anestesi dan Pra Sedasi
        $('.imgViewPreAnesthesyAssessment.imgLink').die('click');
        $('.imgViewPreAnesthesyAssessment.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPreAnesthesyAssesmentCtl1.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var testorderID = $(this).attr('testorderID');
            var param = visitID + '|' + testorderID + '|' + recordID;
            openUserControlPopup(url, param, "Asesmen Pra Anestesi", 1024, 500);
        });
        //#endregion

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
                var param = "0" + "|" + $('#<%=hdnID.ClientID %>').val() +"||||||||";
                openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
            }
        });

        $('#lblAddSignInSafetyChecklistForm').die('click');
        $('#lblAddSignInSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignInFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "||||||||";
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
        });

        $('#lblAddTimeOutSafetyChecklistForm').die('click');
        $('#lblAddTimeOutSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgeryTimeOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "||||||||";
            openUserControlPopup(url, param, "Surgical Safety Check List - TIME OUT", 900, 500);
        });

        $('#lblAddSignOutSafetyChecklistForm').die('click');
        $('#lblAddSignOutSafetyChecklistForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/SurgerySignOutFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "||||||||";
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
                    var param = recordID + "|" + testOrderID + "|" + signInDate + "|" + signInTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + nurseID + "|" + layout + "|" + values;
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
                    var param = recordID + "|" + testOrderID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + nurseID + "|" + layout + "|" + values;
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
                    var param = recordID + "|" + testOrderID + "|" + signOutDate + "|" + signOutTime + "|" + paramedicID + "|" + physicianID + "|" + anesthetistID + "|" + nurseID + "|" + layout + "|" + values + "|" + surgeryEndDate + "|" + surgeryEndTime;
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

        //#region Asuhan Keperawatan Perioperatif
        $('#lblAddPerioperativeNursingForm').die('click');
        $('#lblAddPerioperativeNursingForm').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingPreOperativeFormEntry.ascx");
                var param = "0" + "|" + $('#<%=hdnID.ClientID %>').val() + "||||||||";
                openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - PRE OPERATIVE", 700, 400);
            }
        });

        $('#lblAddPreOperativeForm').die('click');
        $('#lblAddPreOperativeForm').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingPreOperativeFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|||||||||";
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - PRE OPERATIVE", 700, 400);
        });

        $('.imgEditPreOperative.imgLink').die('click');
        $('.imgEditPreOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingPreOperativeFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('preOperativeDate');
            var time = $(this).attr('preOperativeTime');
            var paramedicID1 = $(this).attr('preoperativeSurgeryNurseID');
            var paramedicID2 = $(this).attr('preoperativeWardNurseID');
            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID1) {
                var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID1 + "|" + paramedicID2 + "|" + "|" + "|" + layout + "|" + values + "|" + remarks;
                openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - PRE OPERATIVE", 700, 400);
            }
            else {
                displayErrorMessageBox('Asuhan Keperawatan Perioperatif - PRE OPERATIVE', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }

        });

        $('.imgPreVitalSign.imgLink').die('click');
        $('.imgPreVitalSign.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/ServiceUnitVitalSignEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('preOperativeDate');
            var time = $(this).attr('preOperativeTime');
            var paramedicID1 = $(this).attr('preoperativeSurgeryNurseID');
            var paramedicID2 = $(this).attr('preoperativeWardNurseID');
            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var healthcareServiceUnitID = $(this).attr('healthcareServiceUnitID');
            var vitalSignID = $(this).attr('vitalSignID');
            var param = "01"+ "|" + recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID1 + "|" + healthcareServiceUnitID + "|" + vitalSignID;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - Tanda Vital (Pre Operasi)", 700, 500);
        });

        $('.imgDeletePreOperative.imgLink').die('click');
        $('.imgDeletePreOperative.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Asuhan Keperawatan Perioperatif - PRE OPERATIVE untuk pasien ini ?";
            displayConfirmationMessageBox("Asuhan Keperawatan Perioperatif - PRE OPERATIVE", message, function (result) {
                if (result) {
                    cbpDeletePerioperativeInfo.PerformCallback("1" + "|" + recordID);
                }
            });
        });

        $('.imgViewPreOperative.imgLink').die('click');
        $('.imgViewPreOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewNursingPreOperativeCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('preOperativeDate');
            var time = $(this).attr('preOperativeTime');
            var paramedicID1 = $(this).attr('preoperativeSurgeryNurseID');
            var nurseName1 = $(this).attr('nurseName1');
            var nurseName2 = $(this).attr('nurseName2');
            var paramedicID2 = $(this).attr('preoperativeWardNurseID');


            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + nurseName1 + "|" + nurseName2 + "|" + "|" + "|" + layout + "|" + values + "|" + remarks;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - PRE OPERATIVE", 700, 500);
        });

        $('#lblAddIntraOperativeForm').die('click');
        $('#lblAddIntraOperativeForm').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingIntraOperativeFormEntry.ascx");
                var recordID = $(this).attr('recordID');
                var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|||||||||";
                openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - INTRA OPERASI", 700, 500);
            }
        });

        $('.imgEditIntraOperative.imgLink').die('click');
        $('.imgEditIntraOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingIntraOperativeFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('intraOperativeDate');
            var time = $(this).attr('intraOperativeTime');
            var paramedicID1 = $(this).attr('intraOperativeCircularNurseID');
            var paramedicID2 = $(this).attr('intraOperativeRecoveryRoomNurseID');
            var paramedicID3 = $(this).attr('intraOperativeInstrumentNurseID');
            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID1) {
                var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID1 + "|" + paramedicID2 + "|" + paramedicID3 + "|" + "|" + layout + "|" + values + "|" + remarks;
                openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - INTRA OPERATIVE", 700, 500);
            }
            else {
                displayErrorMessageBox('Asuhan Keperawatan Perioperatif - INTRA OPERATIVE', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgDeleteIntraOperative.imgLink').die('click');
        $('.imgDeleteIntraOperative.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Asuhan Keperawatan Perioperatif - INTRA OPERATIVE untuk pasien ini ?";
            displayConfirmationMessageBox("Asuhan Keperawatan Perioperatif - INTRA OPERATIVE", message, function (result) {
                if (result) {
                    cbpDeletePerioperativeInfo.PerformCallback("2" + "|" + recordID);
                }
            });
        });

        $('.imgViewIntraOperative.imgLink').die('click');
        $('.imgViewIntraOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewNursingIntraOperativeCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('intraOperativeDate');
            var time = $(this).attr('intraOperativeTime');
            var paramedicID1 = $(this).attr('intraOperativeCircularNurseID');
            var nurseName1 = $(this).attr('nurseName1');
            var nurseName2 = $(this).attr('nurseName2');
            var nurseName3 = $(this).attr('nurseName3');
            var paramedicID2 = $(this).attr('intraOperativeRecoveryRoomNurseID');
            var paramedicID3 = $(this).attr('intraOperativeInstrumentNurseID');


            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + nurseName1 + "|" + nurseName2 + "|" + nurseName3 + "|" + "|" + layout + "|" + values + "|" + remarks;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - INTRA OPERATIVE", 700, 500);
        });

        $('#lblAddPostOperativeForm').die('click');
        $('#lblAddPostOperativeForm').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingPostOperativeFormEntry.ascx");
                var recordID = $(this).attr('recordID');
                var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|||||||||";
                openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - POST OPERASI", 700, 500);
            }
        });

        $('.imgEditPostOperative.imgLink').die('click');
        $('.imgEditPostOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/NursingPostOperativeFormEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('postOperativeDate');
            var time = $(this).attr('postOperativeTime');
            var paramedicID1 = $(this).attr('postOperativeRecoveryRoomNurseID');
            var paramedicID2 = $(this).attr('postOperativeWardNurseID');
            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID1) {
                var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID1 + "|" + paramedicID2 + "|" + "|" + "|" + layout + "|" + values + "|" + remarks;
                openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - POST OPERATIVE", 700, 500);
            }
            else {
                displayErrorMessageBox('Asuhan Keperawatan Perioperatif - POST OPERATIVE', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgPostVitalSign.imgLink').die('click');
        $('.imgPostVitalSign.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/ServiceUnitVitalSignEntry.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('postOperativeDate');
            var time = $(this).attr('postOperativeTime');
            var paramedicID1 = $(this).attr('postOperativeRecoveryRoomNurseID');
            var paramedicID2 = $(this).attr('postOperativeWardNurseID');
            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var healthcareServiceUnitID = $(this).attr('healthcareServiceUnitID');
            var vitalSignID = $(this).attr('vitalSignID');
            var param = "02" + "|" + recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID1 + "|" + healthcareServiceUnitID + "|" + vitalSignID;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - Tanda Vital (Paska Operasi)", 700, 500);
        });

        $('.imgDeletePostOperative.imgLink').die('click');
        $('.imgDeletePostOperative.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Asuhan Keperawatan Perioperatif - POST OPERATIVE untuk pasien ini ?";
            displayConfirmationMessageBox("Asuhan Keperawatan Perioperatif - POST OPERATIVE", message, function (result) {
                if (result) {
                    cbpDeletePerioperativeInfo.PerformCallback("3" + "|" + recordID);
                }
            });
        });

        $('.imgViewPostOperative.imgLink').die('click');
        $('.imgViewPostOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewNursingPostOperativeCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('postOperativeDate');
            var time = $(this).attr('postOperativeTime');
            var paramedicID1 = $(this).attr('postOperativeRecoveryRoomNurseID');
            var nurseName1 = $(this).attr('nurseName1');
            var nurseName2 = $(this).attr('nurseName2');
            var paramedicID2 = $(this).attr('postOperativeWardNurseID');


            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + nurseName1 + "|" + nurseName2 + "|" + "|" + "|" + layout + "|" + values + "|" + remarks;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - POST OPERATIVE", 700, 500);
        });

        $('.imgDeletePerioperativeNursing.imgLink').die('click');
        $('.imgDeletePerioperativeNursing.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Asuhan Keperawatan Perioperatif untuk pasien ini ?";
            displayConfirmationMessageBox("Asuhan Keperawatan Perioperatif", message, function (result) {
                if (result) {
                    cbpDeletePerioperative.PerformCallback(recordID);
                }
            });
        });
        //#endregion

        //#region Laporan Operasi
        $('.imgViewSurgeryReport.imgLink').die('click');
        $('.imgViewSurgeryReport.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewSurgeryReportCtl1.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var testorderID = $(this).attr('testorderID');
            var param = visitID + '|' + testorderID + '|' + recordID + '|' + '';
            openUserControlPopup(url, param, "Laporan Operasi", 1024, 500);
        });
        //#endregion

        //#region Pemasangan Implant

        $('#lblAddImplantDevice').die('click');
        $('#lblAddImplantDevice').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientMedicalDevice/PatientMedicalDeviceEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val();
                openUserControlPopup(url, param, "Pemasangan Implant", 700, 500);
            }
        });

        $('.imgAddDevice.imgLink').die('click');
        $('.imgAddDevice.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientMedicalDevice/PatientMedicalDeviceEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val();
                openUserControlPopup(url, param, "Pemasangan Implant", 700, 500);
            }
        });

        $('.imgEditDevice.imgLink').die('click');
        $('.imgEditDevice.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientMedicalDevice/PatientMedicalDeviceEntryCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val();
            openUserControlPopup(url, param, "Pemasangan Implant", 700, 500);
        });

        $('.imgDeleteDevice.imgLink').die('click');
        $('.imgDeleteDevice.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var itemName = $(this).attr('itemName');
            var message = "Hapus informasi implant " + itemName + " dari Pengkajian Kamar Operasi untuk pasien ini ?";
            displayConfirmationMessageBox("Pemasangan Implant", message, function (result) {
                if (result) {
                    cbpDeleteDevice.PerformCallback(recordID);
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
                var param = "|" + $('#<%=hdnID.ClientID %>').val();
                openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
            }
        });

        $('.imgAddDocument.imgLink').die('click');
        $('.imgAddDocument.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
            var param = "|" + $('#<%=hdnID.ClientID %>').val();
            openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
        });

        $('.imgEditDocument.imgLink').die('click');
        $('.imgEditDocument.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val();
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
                var param = "0" + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "||||||" + $('#<%=hdnRegistrationID.ClientID %>').val() + "|";
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
                var param = recordID + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegistrationID.ClientID %>').val() + "|";
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
            var param = recordID + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicName + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegistrationID.ClientID %>').val();
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
                    var param = "0" + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegistrationID.ClientID %>').val();
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
                displayErrorMessageBox('EDIT', 'Maaf, Assessment hanya bisa diubah oleh user yang mengkaji.');
                return false;
            }
        }

        function onBeforeBasePatientPageListDelete() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('DELETE', 'Maaf, Assessment hanya bisa dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshDocumentGrid() {
            //cbpViewDt5.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup() {
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
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
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
                    cbpViewDt4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt6EndCallback(s) {
            $('#containerImgLoadingViewDt6').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt6.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt6"), pageCount1, function (page) {
                    cbpViewDt6.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt6.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt7EndCallback(s) {
            $('#containerImgLoadingViewDt7').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt7.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt7"), pageCount1, function (page) {
                    cbpViewDt7.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt7.ClientID %> tr:eq(1)').click();
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
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();

            if (hdnID == '' || hdnID == '0') {
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

        function onCbpDeleteDeviceEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Pemasangan Implant', param[1]);
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

        function onCbpDeletePerioperativeInfoEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt6.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Asuhan Keperawatan Perioperatif', param[1]);
            }
        }

        function onCbpDeletePerioperativeEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt6.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Asuhan Keperawatan Perioperatif', param[1]);
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
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnMRN" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnModuleID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="0" />
    <input type="hidden" runat="server" id="hdnMenuType" value="" />
    <input type="hidden" runat="server" id="hdnDeptType" value="" />
    <input type="hidden" runat="server" id="hdnSurgeryReportID" value="0" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnOrderNo" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnRevisedParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevised" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevision" value="0" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrl" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:25%"/>
            <col style="width:75%"/>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="TestOrderNo" HeaderStyle-CssClass="hiddenColumn orderNo" ItemStyle-CssClass="hiddenColumn orderNo" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal ") %> - <%=GetLabel("Jam Order") %>,  <span style="color:blue"><%=GetLabel("Diorder Oleh") %></span></div>
                                                    <div style="font-weight: bold"><%=GetLabel("No. Order") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div><%#: Eval("TestOrderDateTimeInString")%>, <%#: Eval("TestOrderNo")%></div>
                                                                <div style="font-weight: bold"><span style="color:blue"> <%#: Eval("ParamedicName") %></span></div>
                                                                <div style="font-style:italic;color: Red" class="blink"><%#: Eval("ScheduleStatus")%>, <%#: Eval("cfRoomScheduleDate")%> - <%#: Eval("RoomScheduleTime")%></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("EstimatedDuration") %>" bindingfield="EstimatedDuration" />
                                                    <input type="hidden" value="<%#:Eval("IsUsingSpecificItem") %>" bindingfield="IsUsingSpecificItem" />
                                                    <input type="hidden" value="<%#:Eval("IsUsedRequestTime") %>" bindingfield="IsUsedRequestTime" />
                                                    <input type="hidden" value="<%#:Eval("IsCITO") %>" bindingfield="IsCITO" />
                                                    <input type="hidden" value="<%#:Eval("cfScheduledDateInString") %>" bindingfield="cfScheduledDateInString" />
                                                    <input type="hidden" value="<%#:Eval("ScheduledTime") %>" bindingfield="ScheduledTime" />
                                                    <input type="hidden" value="<%#:Eval("RoomCode") %>" bindingfield="RoomCode" />
                                                    <input type="hidden" value="<%#:Eval("RoomName") %>" bindingfield="RoomName" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
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
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">   
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="preSurgeryAssessment">
                                        <%=GetLabel("Asesmen Pra Bedah")%></li>
                                    <li contentid="preSurgeryAnesthesy">
                                        <%=GetLabel("Asesmen Pra Anestesi")%></li>
                                    <li contentid="preSurgicalSafetyChecklist">
                                        <%=GetLabel("Surgical Safety Checklist")%></li>
                                    <li contentid="periOperative">
                                        <%=GetLabel("Asuhan Perioperatif")%></li>
<%--                                    <li contentid="anesthesyStatus">
                                        <%=GetLabel("Status Anestesi")%></li>--%>
                                    <li contentid="surgeryReport">
                                        <%=GetLabel("Laporan Operasi")%></li>
                                    <li contentid="patientMedicalDevice">
                                        <%=GetLabel("Pemasangan Implant")%></li>
                                    <li contentid="surgeryDocument">
                                        <%=GetLabel("e-Document")%></li>
                                    <li contentid="preProcedureChecklist">
                                        <%=GetLabel("Checklist Persiapan Operasi")%></li>
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
                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreSurgeryAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordID = "<%#:Eval("PreSurgicalAssessmentID") %>" visitID = "<%#:Eval("VisitID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PreSurgicalAssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PreSurgicalAssessmentID") %>" bindingfield="PreSurgicalAssessmentID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis"  DataField="PreDiagnoseText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada asesmen pra bedah untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt1"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="preSurgeryAnesthesy" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt7" runat="server" Width="100%" ClientInstanceName="cbpViewDt7"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt7_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt7').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt7EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent8" runat="server">
                                            <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt7" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreAnesthesyAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreAnesthesyAssessmentID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PreAnesthesyAssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PreAnesthesyAssessmentID") %>" bindingfield="PreAnesthesyAssessmentID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="ASA"  DataField="cfASAStatus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Teknik Anestesi"  DataField="cfAnesthesiaType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada asesmen pra anestesi dan sedasi untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt7" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt7"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="preSurgicalSafetyChecklist" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-top:4px">
                                                                    <tr>
                                                                        <td style="text-align:center">
                                                                            <img class="imgDeleteSurgicalSafetyCheck imgLink" title='<%=GetLabel("Delete Check List")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
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
                                                                                <div><%=GetLabel("Belum ada informasi Sign In Surgical Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddSignInSafetyChecklistForm" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Sign In Check List")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditSignIn imgLink" title='<%=GetLabel("Edit Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("SignInLayout") %>" values = "<%#:Eval("SignInValues") %>" 
                                                                                            paramedicID = "<%#:Eval("SignInParamedicID") %>" physicianID = "<%#:Eval("SignOutPhysicianID") %>" anesthetistID = "<%#:Eval("SignOutAnesthetistID") %>" nurseID = "<%#:Eval("SignOutNurseID") %>"
                                                                                            signInDate = "<%#:Eval("cfSignInDate") %>" signInTime = "<%#:Eval("SignInTime") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" />
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
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("SignInLayout") %>" values = "<%#:Eval("SignInValues") %>" 
                                                                                            paramedicName = "<%#:Eval("SignInParamedicName") %>" physicianName = "<%#:Eval("SignOutPhysicianName") %>" anesthetistName = "<%#:Eval("SignOutAnesthetistName") %>" nurseName = "<%#:Eval("SignOutNurseName") %>"
                                                                                            signInDate = "<%#:Eval("cfSignInDate") %>" signInTime = "<%#:Eval("SignInTime") %>" />
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
                                                                                <div><%=GetLabel("Belum ada informasi Time Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddTimeOutSafetyChecklistForm" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Time Out Check List")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditTimeOut imgLink" title='<%=GetLabel("Edit Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("TimeOutLayout") %>" values = "<%#:Eval("TimeOutValues") %>" 
                                                                                            paramedicID = "<%#:Eval("TimeOutParamedicID") %>" physicianID = "<%#:Eval("SignOutPhysicianID") %>" anesthetistID = "<%#:Eval("SignOutAnesthetistID") %>" nurseID = "<%#:Eval("SignOutNurseID") %>"
                                                                                            timeOutDate = "<%#:Eval("cfTimeOutDate") %>" timeOutTime = "<%#:Eval("TimeOutTime") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>" />
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
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("TimeOutLayout") %>" values = "<%#:Eval("TimeOutValues") %>" 
                                                                                            paramedicName = "<%#:Eval("TimeOutParamedicName") %>" physicianName = "<%#:Eval("SignOutPhysicianName") %>" anesthetistName = "<%#:Eval("SignOutAnesthetistName") %>" nurseName = "<%#:Eval("SignOutNurseName") %>"
                                                                                            timeOutDate = "<%#:Eval("cfTimeOutDate") %>" timeOutTime = "<%#:Eval("TimeOutTime") %>" />
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
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("SignOutNurseName") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Sign Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddSignOutSafetyChecklistForm" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Sign Out Check List")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditSignOut imgLink" title='<%=GetLabel("Edit Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("SignOutLayout") %>" values = "<%#:Eval("SignOutValues") %>" 
                                                                                            paramedicID = "<%#:Eval("SignOutNurseID") %>" physicianID = "<%#:Eval("SignOutPhysicianID") %>" anesthetistID = "<%#:Eval("SignOutAnesthetistID") %>" nurseID = "<%#:Eval("SignOutNurseID") %>"
                                                                                            signOutDate = "<%#:Eval("cfSignOutDate") %>" signOutTime = "<%#:Eval("SignOutTime") %>" surgeryEndDate = "<%#:Eval("cfSurgeryEndDate") %>" surgeryEndTime = "<%#:Eval("SurgeryEndTime") %>" isSignInVerified = "<%#:Eval("cfIsSignInVerified") %>" isTimeOutVerified = "<%#:Eval("cfIsTimeOutVerified") %>" isSignOutVerified = "<%#:Eval("cfIsSignOutVerified") %>"  />
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
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("SignOutLayout") %>" values = "<%#:Eval("SignOutValues") %>" 
                                                                                            paramedicName = "<%#:Eval("SignInParamedicName") %>" physicianName = "<%#:Eval("SignOutPhysicianName") %>" anesthetistName = "<%#:Eval("SignOutAnesthetistName") %>" nurseName = "<%#:Eval("SignOutNurseName") %>"
                                                                                            signOutDate = "<%#:Eval("cfSignOutDate") %>" signOutTime = "<%#:Eval("SignOutTime") %>" surgeryEndDate = "<%#:Eval("cfSurgeryEndDate") %>" surgeryEndTime = "<%#:Eval("SurgeryEndTime") %>" />
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
                                                            <div><%=GetLabel("Belum ada formulir Surgical Safety Check List untuk pasien ini") %></div>
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
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt2"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="periOperative" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt6" runat="server" Width="100%" ClientInstanceName="cbpViewDt6"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt6_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt6').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt6EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent7" runat="server">
                                            <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt6" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%" style="padding-top:4px">
                                                                    <tr>
                                                                        <td style="text-align:center">
                                                                            <img class="imgDeletePerioperativeNursing imgLink" title='<%=GetLabel("Delete Asuhan Perioperatif")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="PRE OPERATIVE" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfPreOperativeDateTime") %></div>
                                                                            <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("PreOperativeSurgeryNurseName") %></div>
                                                                            <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Pre Operasi untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddPreOperativeForm" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ PRE OPERASI")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditPreOperative imgLink" title='<%=GetLabel("Edit Pre Operative")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("PreOperativeLayout") %>" values = "<%#:Eval("PreOperativeValues") %>" 
                                                                                            preoperativeWardNurseID = "<%#:Eval("PreoperativeWardNurseID") %>" preoperativeSurgeryNurseID = "<%#:Eval("PreOperativeSurgeryNurseID") %>" 
                                                                                            preOperativeDate = "<%#:Eval("cfPreOperativeDate") %>" preOperativeTime = "<%#:Eval("PreOperativeTime") %>" remarks = "<%#:Eval("PreOperativeRemarks") %>"/>
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgPreVitalSign imgLink" title='<%=GetLabel("Tanda Vital")%>' src='<%# ResolveUrl("~/Libs/Images/Button/vitalsign.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("PreOperativeLayout") %>" values = "<%#:Eval("PreOperativeValues") %>" 
                                                                                            preoperativeWardNurseID = "<%#:Eval("PreoperativeWardNurseID") %>" preoperativeSurgeryNurseID = "<%#:Eval("PreOperativeSurgeryNurseID") %>" 
                                                                                            preOperativeDate = "<%#:Eval("cfPreOperativeDate") %>" preOperativeTime = "<%#:Eval("PreOperativeTime") %>" remarks = "<%#:Eval("PreOperativeRemarks") %>" vitalSignID = "<%#:Eval("PreOperativeVitalSignID") %>" healthcareServiceUnitID = "<%#:Eval("HealthcareServiceUnitID") %>"/>
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeletePreOperative imgLink" title='<%=GetLabel("Delete Pre Operative")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewPreOperative imgLink" title='<%=GetLabel("Lihat Pre Operative")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("PreOperativeLayout") %>" values = "<%#:Eval("PreOperativeValues") %>" 
                                                                                            preoperativeWardNurseID = "<%#:Eval("PreoperativeWardNurseID") %>" preoperativeSurgeryNurseID = "<%#:Eval("PreOperativeSurgeryNurseID") %>" nurseName1 = "<%#:Eval("PreOperativeSurgeryNurseName") %>" nurseName2 = "<%#:Eval("PreOperativeWardNurseName") %>"
                                                                                            preOperativeDate = "<%#:Eval("cfPreOperativeDate") %>" preOperativeTime = "<%#:Eval("PreOperativeTime") %>" remarks = "<%#:Eval("PreOperativeRemarks") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="INTRA OPERASI" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfIntraOperativeDateTime") %></div>
                                                                            <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("IntraOperativeCircularNurseName") %></div>
                                                                            <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Intra Operasi untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddIntraOperativeForm" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Intra Operasi")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditIntraOperative imgLink" title='<%=GetLabel("Edit Intra Operative")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("IntraOperativeLayout") %>" values = "<%#:Eval("IntraOperativeValues") %>" 
                                                                                            intraOperativeCircularNurseID = "<%#:Eval("IntraOperativeCircularNurseID") %>" intraOperativeRecoveryRoomNurseID = "<%#:Eval("IntraOperativeRecoveryRoomNurseID") %>" intraOperativeInstrumentNurseID = "<%#:Eval("IntraOperativeInstrumentNurseID") %>"
                                                                                            intraOperativeDate = "<%#:Eval("cfIntraOperativeDate") %>" intraOperativeTime = "<%#:Eval("IntraOperativeTime") %>" remarks = "<%#:Eval("IntraOperativeRemarks") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteIntraOperative imgLink" title='<%=GetLabel("Delete Intra Operative")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewIntraOperative imgLink" title='<%=GetLabel("Lihat Intra Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("IntraOperativeLayout") %>" values = "<%#:Eval("IntraOperativeValues") %>" 
                                                                                            intraOperativeCircularNurseID = "<%#:Eval("IntraOperativeCircularNurseID") %>" intraOperativeRecoveryRoomNurseID = "<%#:Eval("IntraOperativeRecoveryRoomNurseID") %>" intraOperativeInstrumentNurseID = "<%#:Eval("IntraOperativeInstrumentNurseID") %>" nurseName1 = "<%#:Eval("IntraOperativeCircularNurseName") %>" nurseName2 = "<%#:Eval("IntraOperativeRecoveryRoomNurseName") %>" nurseName3 = "<%#:Eval("IntraOperativeInstrumentNurseName") %>"
                                                                                            intraOperativeDate = "<%#:Eval("cfIntraOperativeDate") %>" intraOperativeTime = "<%#:Eval("IntraOperativeTime") %>" remarks = "<%#:Eval("IntraOperativeRemarks") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="PASKA OPERASI" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfPostOperativeDateTime") %></div>
                                                                            <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("PostOperativeRecoveryRoomNurseName") %></div>
                                                                            <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Paska Operasi untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddPostOperativeForm" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>">
                                                                                    <%= GetLabel("+ Paska Operasi")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditPostOperative imgLink" title='<%=GetLabel("Edit Paska Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("PostOperativeLayout") %>" values = "<%#:Eval("PostOperativeValues") %>" 
                                                                                            postOperativeRecoveryRoomNurseID = "<%#:Eval("postOperativeRecoveryRoomNurseID") %>" postOperativeWardNurseID = "<%#:Eval("PostOperativeWardNurseID") %>" 
                                                                                            postOperativeDate = "<%#:Eval("cfPostOperativeDate") %>" postOperativeTime = "<%#:Eval("PostOperativeTime") %>" remarks = "<%#:Eval("PostOperativeRemarks") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgPostVitalSign imgLink" title='<%=GetLabel("Tanda Vital")%>' src='<%# ResolveUrl("~/Libs/Images/Button/vitalsign.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("PostOperativeLayout") %>" values = "<%#:Eval("PostOperativeValues") %>" 
                                                                                            postOperativeRecoveryRoomNurseID = "<%#:Eval("postOperativeRecoveryRoomNurseID") %>" postOperativeWardNurseID = "<%#:Eval("PostOperativeWardNurseID") %>" 
                                                                                            postOperativeDate = "<%#:Eval("cfPostOperativeDate") %>" postOperativeTime = "<%#:Eval("PostOperativeTime") %>" remarks = "<%#:Eval("PostOperativeRemarks") %>" vitalSignID = "<%#:Eval("PostOperativeVitalSignID") %>" healthcareServiceUnitID = "<%#:Eval("HealthcareServiceUnitID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeletePostOperative imgLink" title='<%=GetLabel("Delete Paska Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewPostOperative imgLink" title='<%=GetLabel("Lihat Paska Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" layout = "<%#:Eval("PostOperativeLayout") %>" values = "<%#:Eval("PostOperativeValues") %>" 
                                                                                            postOperativeRecoveryRoomNurseID = "<%#:Eval("PostOperativeRecoveryRoomNurseID") %>" postOperativeRecoveryRoomNurseID = "<%#:Eval("PostOperativeRecoveryRoomNurseID") %>" 
                                                                                            nurseName1 = "<%#:Eval("PostOperativeWardNurseName") %>" nurseName2 = "<%#:Eval("PostOperativeRecoveryRoomNurseName") %>"
                                                                                            postOperativeDate = "<%#:Eval("cfPostOperativeDate") %>" postOperativeTime = "<%#:Eval("PostOperativeTime") %>" remarks = "<%#:Eval("PostOperativeRemarks") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada Asuhan Keperawatan Perioperatif untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddPerioperativeNursingForm">
                                                                <%= GetLabel("+ Asuhan Keperawatan Perioperatif")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt6" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt6"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="surgeryReport" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewSurgeryReport imgLink" title='<%=GetLabel("Lihat Hasil Operasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordID = "<%#:Eval("PatientSurgeryID") %>" visitID = "<%#:Eval("VisitID") %>"  testOrderID = "<%#:Eval("TestOrderID") %>" />
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
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfReportDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="ReportTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis"  DataField="PreOperativeDiagnosisText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Post Diagnosis"  DataField="PostOperativeDiagnosisText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada laporan operasi untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>   
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt4" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt4"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="patientMedicalDevice" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddDevice imgLink" title='<%=GetLabel("+ Implant")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditDevice imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" itemName = "<%#:Eval("ItemName") %>"  />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteDevice imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>"  itemName = "<%#:Eval("ItemName") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfImplantDate" HeaderText="Tanggal Pemasangan" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="Nama Implant" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />                                                        
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada informasi pemasangan implant untuk tindakan operasi di pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddImplantDevice">
                                                                <%= GetLabel("+ Implant")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>   
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt3"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="surgeryDocument" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt5" runat="server" Width="100%" ClientInstanceName="cbpViewDt5"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt5_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt5EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt5" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddDocument imgLink" title='<%=GetLabel("+ Document")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" fileName = "<%#:Eval("FileName") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditDocument imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" fileName = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteDocument imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" fileName = "<%#:Eval("FileName") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fileName" />
                                                        <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada informasi e=document untuk tindakan operasi di pasien ini") %></div>
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
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt5" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt5"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="preProcedureChecklist" style="position: relative;display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt8" runat="server" Width="100%" ClientInstanceName="cbpViewDt8"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt8_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt8').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt8EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent9" runat="server">
                                           <input type="hidden" value="" id="hdnFileString" runat="server" />
                                            <asp:Panel runat="server" ID="Panel8" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt8" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddChecklist imgLink" title='<%=GetLabel("+ Checklist")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditChecklist imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" checklistDate = "<%#:Eval("cfChecklistDatePickerFormat") %>" checklistTime = "<%#:Eval("ChecklistTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>"
                                                                                formLayout = "<%#:Eval("ChecklistFormLayout") %>" formValue = "<%#:Eval("ChecklistFormValue") %>"/>
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteChecklist imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"  />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgViewChecklist imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" checklistDate = "<%#:Eval("cfChecklistDatePickerFormat") %>" checklistTime = "<%#:Eval("ChecklistTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>" paramedicName = "<%#:Eval("ParamedicName") %>"
                                                                                formLayout = "<%#:Eval("ChecklistFormLayout") %>" formValue = "<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgCopyChecklist imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" checklistDate = "<%#:Eval("cfChecklistDatePickerFormat") %>" checklistTime = "<%#:Eval("ChecklistTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>" paramedicName = "<%#:Eval("ParamedicName") %>"
                                                                                formLayout = "<%#:Eval("ChecklistFormLayout") %>" formValue = "<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:BoundField DataField="cfChecklistDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField DataField="ChecklistTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                                        <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                                        <asp:BoundField DataField="GCChecklistType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                        <asp:BoundField DataField="ChecklistFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                        <asp:BoundField DataField="ChecklistFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                        <asp:BoundField DataField="cfChecklistDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
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

        <dxcp:ASPxCallbackPanel ID="cbpDeleteDevice" runat="server" Width="100%" ClientInstanceName="cbpDeleteDevice"
            ShowLoadingPanel="false" OnCallback="cbpDeleteDevice_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteDeviceEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteDocument" runat="server" Width="100%" ClientInstanceName="cbpDeleteDocument"
            ShowLoadingPanel="false" OnCallback="cbpDeleteDocument_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteDocumentEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeletePerioperativeInfo" runat="server" Width="100%" ClientInstanceName="cbpDeletePerioperativeInfo"
            ShowLoadingPanel="false" OnCallback="cbpDeletePerioperativeInfo_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeletePerioperativeInfoEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteCheckListInfo" runat="server" Width="100%" ClientInstanceName="cbpDeleteCheckListInfo"
            ShowLoadingPanel="false" OnCallback="cbpDeleteCheckListInfo_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteCheckListInfoEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeletePerioperative" runat="server" Width="100%" ClientInstanceName="cbpDeletePerioperative"
            ShowLoadingPanel="false" OnCallback="cbpDeletePerioperative_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeletePerioperativeEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteSurgicalCheckList" runat="server" Width="100%" ClientInstanceName="cbpDeleteSurgicalCheckList"
            ShowLoadingPanel="false" OnCallback="cbpDeleteSurgicalCheckList_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteSurgicalCheckListEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteChecklist" runat="server" Width="100%" ClientInstanceName="cbpDeleteChecklist"
            ShowLoadingPanel="false" OnCallback="cbpDeleteChecklist_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteChecklistEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
