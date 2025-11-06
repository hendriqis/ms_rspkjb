<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" CodeBehind="PatientVentilatorList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientVentilatorList" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content9" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_ventilatorList">
        $(function () {
            $('#<%=grdDeviceTypeList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDeviceTypeList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDeviceType.ClientID %>').val($(this).find('.keyField').html());

                if ($('#<%=hdnDeviceType.ClientID %>').val() != "") {
                    cbpView.PerformCallback('refresh');
                    $('#<%=hdnRecordID.ClientID %>').val("0");
                    cbpViewDt2.PerformCallback('refresh');
                }

            });

            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {

                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnRecordID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnIsReleased.ClientID %>').val($(this).find('.isReleased').html());
                $('#<%=hdnGCDeviceType.ClientID %>').val($(this).find('.gcDeviceType').html());
                ///////cbpViewDt3.PerformCallback('refresh');
                //cbpViewDt4.PerformCallback('refresh');
                tab2Click("TabmonitoringVitalSign");
            });

            $('#<%=grdViewDt3.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDt3.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnUserID2.ClientID %>').val($(this).find('.keyUser').html());
            });

            function tabClick() {
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = "monitoringVitalSign";
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                if ($contentID == "monitoringNosokomial") {
                    cbpViewDt2.PerformCallback('refresh');
                }

            }

            function tab2Click($contentID) {
                $('#ulTabOrderDetail li.selected').removeAttr('class');

                $('.containerOrderDt').filter(':visible').hide();
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
            
                $('#' + $contentID).addClass('selected');
                if ($contentID == "TabmonitoringNosokomial") {
                    cbpViewDt2.PerformCallback('refresh');
                } else {
                    $('#monitoringVitalSign').show();
                    cbpViewDt3.PerformCallback('refresh');
                }

            }
            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                if ($contentID == "monitoringNosokomial") {
                    cbpViewDt2.PerformCallback('refresh');
                } else {
                    cbpViewDt3.PerformCallback('refresh');
                }
            });
            //#endregion

            $('#<%=grdDeviceTypeList.ClientID %> tr:eq(1)').click();
        });

        //#region Detail Grid Button
        $('.imgAdd.imgLink').die('click');
        $('.imgAdd.imgLink').live('click', function (evt) {
            addRecord();
        });

        $('#lblAdd2').die('click');
        $('#lblAdd2').live('click', function (evt) {
            addRecord();
        });

        $('.imgEditRecord.imgLink').die('click');
        $('.imgEditRecord.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) {
                var recordID = $(this).attr('recordID');
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var typeID = $('#<%=hdnDeviceType.ClientID %>').val();
                var isReleased = $(this).attr('isReleased');
                var title = "Data Pemasangan Alat";
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Ventilator/PatientVentilatorEntryCtl1.ascx");
                if (isReleased) {
                    title = "Data Pelepasan Alat";
                    url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Ventilator/PatientVentilatorStopEntryCtl1.ascx");
                }
                var param = recordID + "|" + visitID + "|" + mrn + "|" + typeID; ;
                openUserControlPopup(url, param, title, 700, 500);
            }
        });

        $('.imgDeleteRecord.imgLink').die('click');
        $('.imgDeleteRecord.imgLink').live('click', function () {
            var isHasIntraRecord = $('#<%=hdnHasIntraVitalSignRecord.ClientID %>').val();
            if (isHasIntraRecord == "0") {
                var recordID = $(this).attr('recordID');
                var message = "Hapus Data Pemasangan Alat untuk pasien ini ?";
                displayConfirmationMessageBox("Pemasangan Alat", message, function (result) {
                    if (result) {
                        cbpDelete.PerformCallback(recordID);
                    }
                });
            }
            else {
                displayErrorMessageBox('Pemasangan Alat', "Data Pemasangan Alat hanya bisa diisi jika tidak ada record/data pemantauan yang tercatat.");
            }
        });

        $('.imgStop.imgLink').die('click');
        $('.imgStop.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) {
                var recordID = $(this).attr('recordID');
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var typeID = $('#<%=hdnDeviceType.ClientID %>').val();
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Ventilator/PatientVentilatorStopEntryCtl1.ascx");
                var param = recordID + "|" + visitID + "|" + mrn + "|" + typeID; ;
                openUserControlPopup(url, param, "Data Pelepasan Alat", 700, 500);
            }
        });

        $('.imgCancelStop.imgLink').die('click');
        $('.imgCancelStop.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Batal Data Pelepasan Alat untuk pasien ini ?";
            displayConfirmationMessageBox("Pelepasan Alat", message, function (result) {
                if (result) {
                    cbpCancelStop.PerformCallback(recordID);
                }
            });
        });

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var isAllowPrint = true;
            var gcDeviceType = $(this).attr('gcDeviceType');
            var reportCode = $('#<%=hdnReportCode.ClientID %>').val();
            var errMessage = { text: "" };
            var filterExpression = { text: "" };

            if (typeof onBeforeRightPanelPrint == 'function') {
                isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
            }

            if (isAllowPrint) {
                openReportViewer(reportCode, filterExpression.text);
            }
            else {
                displayErrorMessageBox('Cetak Data Pemantauan Alat', errMessage.text);
            }
        });

        function addRecord() {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var typeID = $('#<%=hdnDeviceType.ClientID %>').val();
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Ventilator/PatientVentilatorEntryCtl1.ascx");
                var param = "0|" + visitID + "|" + mrn + "|" + typeID;
                openUserControlPopup(url, param, "Data Pemasangan Alat", 700, 500);
            }
        }
        //#endregion

        //#region Pemantauan
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
            var title = "Pemantauan Pemasangan Alat";
            var monitoringType = "X487^003";
            if ($('#<%=hdnDeviceType.ClientID %>').val() == "X525^001") {
                title = "Pemantauan Pemasangan Ventilator";
                monitoringType = "X487^003";
            }
            if ($('#<%=hdnRecordID.ClientID %>').val() != "" && $('#<%=hdnRecordID.ClientID %>').val() != "0" && $('#<%=hdnIsReleased.ClientID %>').val() != "1") {
                allow = true;
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "07" + "|" + $('#<%=hdnRecordID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, title, 700, 500, monitoringType);
            }
            else {
                displayErrorMessageBox('Data Pemantauan', "Data Pemantauan hanya bisa diisi jika ada record/data alat yang terpasang atau belum dilepas/stop");
            }
        }

        $('.imgEditIntraVitalSign.imgLink').die('click');
        $('.imgEditIntraVitalSign.imgLink').live('click', function (evt) {
            var userID = $(this).attr('createdBy');
            if (onBeforeEditIntraRecord(userID)) {
                var recordID = $(this).attr('recordID');
                var title = "Pemantauan Pemasangan Alat";
                var monitoringType = "X487^003";
                if ($('#<%=hdnDeviceType.ClientID %>').val() == "X525^001") {
                    title = "Pemantauan Pemasangan Ventilator";
                    monitoringType = "X487^003";
                }
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "07" + "|" + $('#<%=hdnRecordID.ClientID %>').val() + "|" + recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, title, 700, 500, monitoringType);
            }
        });

        $('.imgDeleteIntraVitalSign.imgLink').die('click');
        $('.imgDeleteIntraVitalSign.imgLink').live('click', function () {
            var userID = $(this).attr('createdBy');
            if (onBeforeEditIntraRecord(userID)) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi data pemantauan pemasangan alat untuk pasien ini ?";
                displayConfirmationMessageBox("Data Pemantauan Alat", message, function (result) {
                    if (result) {
                        cbpDeleteIntra.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgCopyIntraVitalSign.imgLink').die('click');
        $('.imgCopyIntraVitalSign').live('click', function () {
            var allow = false;
            if ($('#<%=hdnRecordID.ClientID %>').val() != "" && $('#<%=hdnRecordID.ClientID %>').val() != "0" && $('#<%=hdnIsReleased.ClientID %>').val() != "1") {
                allow = true;
            }
            if (allow) {
                var recordID = $(this).attr('recordID');
                var message = "Lakukan copy indikator data pemantauan alat ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var monitoringType = "X487^003";
                        if ($('#<%=hdnDeviceType.ClientID %>').val() == "X525^001") {
                            title = "Pemantauan Pemasangan Ventilator";
                            monitoringType = "X487^003";
                        }
                        var param = "07" + "||" + recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "||" + $('#<%=hdnRecordID.ClientID %>').val();
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/CopyVitalSignCtl1.ascx");
                        openUserControlPopup(url, param, 'Copy Pemantauan Indikator', 700, 500, monitoringType);
                    }
                });
            }
            else {
                displayErrorMessageBox('Data Pemantauan', "Data Pemantauan hanya bisa diisi jika ada record/data alat yang terpasang atau belum dilepas/stop");
            }
        });

        $('.imgAddDt2.imgLink').die('click');
        $('.imgAddDt2.imgLink').live('click', function (evt) {
            addNosokomialRecord();
        });

        $('#lblAddNosokomial').die('click');
        $('#lblAddNosokomial').live('click', function (evt) {
            addNosokomialRecord();
        });

        function addNosokomialRecord() {
            var allow = false;
            var title = "Pemantauan Kasus Nosokomial";

            if ($('#<%=hdnRecordID.ClientID %>').val() != "" && $('#<%=hdnRecordID.ClientID %>').val() != "0" && $('#<%=hdnIsReleased.ClientID %>').val() != "1") {
                allow = true;
            }
            if (allow) {
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Ventilator/PatientNosokomialEntry.ascx");
                var param = "1" + "|" + $('#<%=hdnDeviceType.ClientID %>').val() + "|" + $('#<%=hdnRecordID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, title, 700, 500, $('#<%=hdnDeviceType.ClientID %>').val());
            }
            else {
                displayErrorMessageBox('Data Pemantauan', "Data Pemantauan hanya bisa diisi jika ada record/data alat yang terpasang atau belum dilepas/stop");
            }
        }

        $('.imgEditNosokomial.imgLink').die('click');
        $('.imgEditNosokomial.imgLink').live('click', function (evt) {
            var userID = $(this).attr('createdBy');
            if (onBeforeEditIntraRecord(userID)) {
                var id = $(this).attr('recordID');
                var assessmentDate = $(this).attr('assessmentDate');
                var assessmentTime = $(this).attr('assessmentTime');
                var paramedicID = $(this).attr('paramedicID');
                var formType = $(this).attr('formType');
                var formLayout = $(this).attr('formLayout');
                var formValue = $(this).attr('formValue');
                var remarks = $(this).attr('remarks');

                var title = "Pemantauan Kasus Nosokomial";

                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Ventilator/PatientNosokomialEntry.ascx");
                var param = "0" + "|" + $('#<%=hdnDeviceType.ClientID %>').val() + "|" + $('#<%=hdnRecordID.ClientID %>').val() + "|" + id + "|" + assessmentDate + "|" +  assessmentTime + "|" + paramedicID + "|" + formType + "|" + formLayout + "|" + formValue + "|" + remarks;
                openUserControlPopup(url, param, title, 700, 500,  $('#<%=hdnDeviceType.ClientID %>').val());
            }
        });

        $('.imgDeleteNosokomial.imgLink').die('click');
        $('.imgDeleteNosokomial.imgLink').live('click', function () {
            var userID = $(this).attr('createdBy');
            if (onBeforeEditIntraRecord(userID)) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi data pemantauan pemasangan alat untuk pasien ini ?";
                displayConfirmationMessageBox("Data Pemantauan Alat", message, function (result) {
                    if (result) {
                        cbpDeleteNosokomial.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgCopyNosokomial.imgLink').die('click');
        $('.imgCopyNosokomial').live('click', function () {
            var allow = false;
            if ($('#<%=hdnRecordID.ClientID %>').val() != "" && $('#<%=hdnRecordID.ClientID %>').val() != "0" && $('#<%=hdnIsReleased.ClientID %>').val() != "1") {
                allow = true;
            }
            if (allow) {
                var recordID = $(this).attr('recordID');
                var id = $(this).attr('recordID');
                var assessmentDate = $(this).attr('assessmentDate');
                var assessmentTime = $(this).attr('assessmentTime');
                var paramedicID = $(this).attr('paramedicID');
                var formType = $(this).attr('formType');
                var formLayout = $(this).attr('formLayout');
                var formValue = $(this).attr('formValue');
                var remarks = $(this).attr('remarks');
                var message = "Lakukan copy indikator data pemantauan alat ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var title = "Pemantauan Kasus Nosokomial";

                        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Ventilator/CopyNosokomialEntry.ascx");
                        var param = "0" + "|" + $('#<%=hdnDeviceType.ClientID %>').val() + "|" + $('#<%=hdnRecordID.ClientID %>').val() + "|" + id + "|" + assessmentDate + "|" + assessmentTime + "|" + paramedicID + "|" + formType + "|" + formLayout + "|" + formValue + "|" + remarks;
                        openUserControlPopup(url, param, title, 700, 500, $('#<%=hdnDeviceType.ClientID %>').val());
                    }
                });
            }
            else {
                displayErrorMessageBox('Data Pemantauan', "Data Pemantauan hanya bisa diisi jika ada record/data alat yang terpasang atau belum dilepas/stop");
            }
        });

        $('.imgViewNosokomial.imgLink').die('click');
        $('.imgViewNosokomial').live('click', function () {
            var recordID = $(this).attr('recordID');
            var id = $(this).attr('recordID');
            var assessmentDate = $(this).attr('assessmentDate');
            var assessmentTime = $(this).attr('assessmentTime');
            var ppa = $(this).attr('paramedicName');
            var formType = $(this).attr('formType');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');
            var remarks = $(this).attr('remarks');
            var title = "Pemantauan Kasus Nosokomial";

            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Ventilator/ViewNosokomialEntryCtl.ascx");
            var param = "0" + "|" + $('#<%=hdnDeviceType.ClientID %>').val() + "|" + $('#<%=hdnRecordID.ClientID %>').val() + "|" + id + "|" + assessmentDate + "|" + assessmentTime + "|" + ppa + "|" + formType + "|" + formLayout + "|" + formValue + "|" + remarks;
            openUserControlPopup(url, param, title, 700, 500, $('#<%=hdnDeviceType.ClientID %>').val());
        });
        //#endregion

        //#region Paging
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

            $('#<%=hdnIsHasLogRecord.ClientID %>').val(s.cpHasRecord);
            if ($('#<%=hdnIsHasLogRecord.ClientID %>').val() == "0") {
                $('#<%=hdnRecordID.ClientID %>').val("0");
                cbpViewDt3.PerformCallback('refresh');
            }
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDt2EndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');
            $('#<%=hdnHasNosokomialRecord.ClientID %>').val(s.cpHasRecord);
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
            $('#<%=hdnHasIntraVitalSignRecord.ClientID %>').val(s.cpHasRecord);
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
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshVitalSignGrid() {
            cbpViewDt3.PerformCallback('refresh');
        }

        function onRefreshNosokomialGrid() {
            cbpViewDt2.PerformCallback('refresh');
        }

        function onCbpDeleteEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Pemasangan Alat', param[1]);
            }
        }

        function oncbpCancelStopEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Pemasangan Alat', param[1]);
            }
        }

        function onBeforeEditIntraRecord(userID) {
            if ($('#<%=hdnCurrentSessionID.ClientID %>').val() == userID) {
                return true;
            }
            else {
                displayErrorMessageBox('MEDINFRAS', 'Maaf, tidak diijinkan mengedit/menghapus record user lain.');
                return false;

            }
        }

        function oncbpDeleteIntraEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Data Pemantauan Alat - Tanda Vital dan Indikator Lainnya', param[1]);
            }
        }

        function oncbpDeleteNosokomialEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Data Pemantauan Alat - Infeksi Nosokomial', param[1]);
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var recordID = $('#<%=hdnRecordID.ClientID%>').val();
            var gcDeviceType = $('#<%=hdnGCDeviceType.ClientID%>').val();

            if (recordID == '') {
                errMessage.text = 'Terjadi kesalahan ketika proses cetak';
                return false;
            }
            else {
                filterExpression.text = recordID + "|" + gcDeviceType;
                return true;
            }
        }
    </script>

    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnDeviceType" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnRecordID" value="" />
    <input type="hidden" runat="server" id="hdnUserID1" value="" />
    <input type="hidden" runat="server" id="hdnUserID2" value="" />
    <input type="hidden" runat="server" id="hdnVitalSignRecordID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnIsReleased" value="0" />
    <input type="hidden" runat="server" id="hdnIsHasLogRecord" value="0" />
    <input type="hidden" runat="server" id="hdnHasIntraVitalSignRecord" value="0" />
    <input type="hidden" runat="server" id="hdnHasNosokomialRecord" value="0" />
    <input type="hidden" runat="server" id="hdnReportCode" value="0" />
    <input type="hidden" runat="server" id="hdnGCDeviceType" value="0" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
        <colgroup>
            <col style="width:15%" />
            <col style="width:40%" />
            <col style="width:45%" />
        </colgroup>
        <tr>
            <td style="vertical-align:top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpDeviceTypeList" runat="server" Width="100%" ClientInstanceName="cbpDeviceTypeList"
                        ShowLoadingPanel="false" OnCallback="cbpDeviceTypeList_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                            EndCallback="function(s,e){ oncbpDeviceTypeListEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="panDeviceTypeList" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdDeviceTypeList" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Alat" HeaderStyle-CssClass="StandardCodeName" ItemStyle-CssClass="StandardCodeName" HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada jenis/tipe alat yang tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerHdImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingHd"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td style="vertical-align:top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                 <HeaderTemplate>
                                                    <img class="imgAdd imgLink" title='<%=GetLabel("+ Data Pemasangan Alat")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                        alt="" />
                                                </HeaderTemplate>  
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEditRecord imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" isReleased = "<%#:Eval("cfIsReleased") %>"/>
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td <%# Eval("cfIsReleased").ToString() == "1" ? "Style='display:none'":"Style='display:block'" %>>
                                                                <img class="imgDeleteRecord imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td <%# Eval("cfIsReleased").ToString() == "0" ? "Style='display:block'":"Style='display:none'" %>>
                                                                <img class="imgStop imgLink" title='<%=GetLabel("Stop Penggunaan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/stop.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td <%# Eval("cfIsReleased").ToString() == "1" && Eval("GCDeviceType").ToString() == "X525^001" || Eval("cfIsReleased").ToString() == "1" && Eval("GCDeviceType").ToString() == "X525^005" ? "Style='display:block'":"Style='display:none'" %>>
                                                                <img class="imgPrint imgLink" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                    alt="" gcDeviceType = "<%#:Eval("GCDeviceType") %>" recordID = "<%#:Eval("ID") %>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>                                         
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfIsReleased" HeaderStyle-CssClass="isReleased hiddenColumn" ItemStyle-CssClass="isReleased hiddenColumn" />
                                            <asp:BoundField DataField="GCDeviceType" HeaderStyle-CssClass="gcDeviceType hiddenColumn" ItemStyle-CssClass="gcDeviceType hiddenColumn" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    Tanggal dan Waktu Pemasangan
                                                    <br />
                                                    Dipasang Oleh
                                                    <br />
                                                    Alasan Pemasangan Alat 
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                     <%#Eval("cfStartDate")%>, <%#Eval("StartTime")%>
                                                     <br />
                                                     <%#Eval("ParamedicName1")%>
                                                     <br />
                                                     <br />
                                                     <%#Eval("ETTReason")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>           
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    Tanggal dan Waktu Pelepasan
                                                    <br />
                                                    Dilepas Oleh
                                                    <br />
                                                    Alasan Pelepasan Alat
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width:60px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                 <%#Eval("cfEndDate")%> <%#Eval("EndTime")%>
                                                                 <br />
                                                                 <%#Eval("ParamedicName2")%>
                                                                 <br />
                                                                 <br />
                                                                 <%#Eval("ETTStopReason")%>
                                                            </td>
                                                            <td <%# Eval("cfIsReleased").ToString() == "0" ? "Style='display:none'":"Style='display:block;float:right'" %>>
                                                                <img class="imgCancelStop imgLink" title='<%=GetLabel("Batal Pelepasan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                                            
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi pemasangan alat untuk pasien ini")%>
                                            <br />
                                            <span class="lblLink" id="lblAdd2">
                                                <%= GetLabel("+ Data Pemasangan")%></span>
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
                            <div id="paging"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td style="vertical-align:top">
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                     <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="monitoringVitalSign" id="TabmonitoringVitalSign">
                                        <%=GetLabel("Pemantauan Tanda Vital dan Indikator Lainnya")%></li>
                                    <li contentid="monitoringNosokomial"  id="TabmonitoringNosokomial">
                                        <%=GetLabel("Pemantauan Kasus Infeksi Nosokomial")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="monitoringVitalSign">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt3_RowDataBound">
                                                    <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="80px">
                                                        <HeaderTemplate>
                                                            <img class="imgAddIntra imgLink" title='<%=GetLabel("+ Pemantauan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PatientETTLogID") %>" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                <img class="imgEditIntraVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PatientETTLogID") %>" createdBy="<%#:Eval("CreatedBy") %>"  />
                                                                <img class="imgDeleteIntraVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PatientETTLogID") %>" createdBy="<%#:Eval("CreatedBy") %>"  />
                                                                <img class="imgCopyIntraVitalSign imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PatientETTLogID") %>" />                                        
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    Indikator Pemantauan Pemasangan Alat</div>
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
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt3"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="monitoringNosokomial">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="120px" ItemStyle-Width="80px">
                                                             <HeaderTemplate>
                                                                <img class="imgAddDt2 imgLink" title='<%=GetLabel("+ Data Pemantauan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" />
                                                            </HeaderTemplate>  
                                                            <ItemTemplate>
                                                                <div id="divView" runat="server" style='text-align: center'>
                                                                    <img class="imgEditNosokomial imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" recordID = "<%#:Eval("AssessmentID") %>" deviceLogID = "<%#:Eval("PatientETTLogID") %>" createdBy="<%#:Eval("CreatedBy") %>" assessmentType = "<%#:Eval("GCAssessmentType") %>" paramedicID = "<%#:Eval("ParamedicID") %>" formLayout = "<%#:Eval("AssessmentFormLayout") %>" formValue = "<%#:Eval("AssessmentFormValue") %>" assessmentDate = "<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime = "<%#:Eval("AssessmentTime") %>" remarks = "<%#:Eval("Remarks") %>"  />
                                                                    <img class="imgDeleteNosokomial imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" recordID = "<%#:Eval("AssessmentID") %>" deviceLogID = "<%#:Eval("PatientETTLogID") %>" createdBy="<%#:Eval("CreatedBy") %>"  />
                                                                    <img class="imgCopyNosokomial imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                        alt="" recordID = "<%#:Eval("AssessmentID") %>" deviceLogID = "<%#:Eval("PatientETTLogID") %>" createdBy="<%#:Eval("CreatedBy") %>" assessmentType = "<%#:Eval("GCAssessmentType") %>" paramedicID = "<%#:Eval("ParamedicID") %>" formLayout = "<%#:Eval("AssessmentFormLayout") %>" formValue = "<%#:Eval("AssessmentFormValue") %>" assessmentDate = "<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime = "<%#:Eval("AssessmentTime") %>" remarks = "<%#:Eval("Remarks") %>" />   
                                                                    <img class="imgViewNosokomial imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/view.png")%>'
                                                                        alt="" recordID = "<%#:Eval("AssessmentID") %>" deviceLogID = "<%#:Eval("PatientETTLogID") %>" createdBy="<%#:Eval("CreatedBy") %>" assessmentType = "<%#:Eval("GCAssessmentType") %>" paramedicName = "<%#:Eval("ParamedicName") %>" formLayout = "<%#:Eval("AssessmentFormLayout") %>" formValue = "<%#:Eval("AssessmentFormValue") %>" assessmentDate = "<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime = "<%#:Eval("AssessmentTime") %>" remarks = "<%#:Eval("Remarks") %>" />                                          
                                                                </div>
                                                            </ItemTemplate>                                         
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:BoundField DataField="cfAssessmentDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField DataField="AssessmentTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                                        <asp:BoundField DataField="ParamedicID" HeaderText = "ParamedicID" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                                        <asp:BoundField DataField="GCAssessmentType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                        <asp:BoundField DataField="AssessmentFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                        <asp:BoundField DataField="AssessmentFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                        <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />                                                           
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada informasi pemantauan nosokomial untuk pasien ini")%>
                                                        <br />
                                                        <span class="lblLink" id="lblAddNosokomial">
                                                            <%= GetLabel("+ Data Pemantauan")%></span>
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
                        </td>
                    </tr>              
                </table>
            </td>
        </tr>
    </table>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDelete" runat="server" Width="100%" ClientInstanceName="cbpDelete"
            ShowLoadingPanel="false" OnCallback="cbpDelete_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteIntra" runat="server" Width="100%" ClientInstanceName="cbpDeleteIntra"
            ShowLoadingPanel="false" OnCallback="cbpDeleteIntra_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteIntraEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteNosokomial" runat="server" Width="100%" ClientInstanceName="cbpDeleteNosokomial"
            ShowLoadingPanel="false" OnCallback="cbpDeleteNosokomial_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteNosokomialEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpCancelStop" runat="server" Width="100%" ClientInstanceName="cbpCancelStop"
            ShowLoadingPanel="false" OnCallback="cbpCancelStop_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpCancelStopEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
