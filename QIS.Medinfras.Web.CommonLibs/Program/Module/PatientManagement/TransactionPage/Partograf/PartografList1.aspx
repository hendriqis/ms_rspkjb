<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PartografList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PartografList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
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
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnPregnancyNo.ClientID %>').val($(this).find('.pregnancyNo').html());
                $('#<%=hdnLMPDate.ClientID %>').val($(this).find('.lmpDate').html());

                if ($('#<%=hdnID.ClientID %>').val() != "") {
                    cbpViewDt1.PerformCallback('refresh');
                    cbpViewDt2.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                    cbpViewDt4.PerformCallback('refresh');
                }
            });

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
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $('#<%=grdViewDt1.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt1.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnFetusID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnFetusNo.ClientID %>').val($(this).find('.fetusNo').html());
            if ($('#<%=hdnFetusID.ClientID %>').val() != "") {
                cbpViewDt1_1.PerformCallback('refresh');
            }
        });



        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
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

        function onCbpViewDt1_1EndCallback(s) {
            $('#containerImgLoadingViewDt1_1').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt1_1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1_1"), pageCount1, function (page) {
                    cbpViewDt1_1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt1_1.ClientID %> tr:eq(1)').click();
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

        $('.imgEditAntenatal.imgLink').die('click');
        $('.imgEditAntenatal.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/AntenatalRecordFormEntry.ascx");
                var param = "antenatalForm01" + "|" + recordID + "|" + "1";
                openUserControlPopup(url, param, "Antenatal Record", 700, 350, "");
            }
        });

        $('.imgAddFetus.imgLink').die('click');
        $('.imgAddFetus.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalEntryCtl1.ascx");
                var param = "0" + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Data Janin", 700, 350, "");
            }
        });

        $('.imgEditFetus.imgLink').die('click');
        $('.imgEditFetus.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FetalMeasurement/FetalEntryCtl1.ascx");
                var param = recordID + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Data Janin", 700, 350, "");
            }
        });

        $('.imgDeleteFetus.imgLink').die('click');
        $('.imgDeleteFetus.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi pengukuran fetal/janin untuk pasien ini ?";
            displayConfirmationMessageBox("Fetal Measurement", message, function (result) {
                if (result) {
                    var param = recordID;
                    cbpDeleteFetus.PerformCallback(param);
                }
            });
        });

        $('.imgAddMeasurement.imgLink').die('click');
        $('.imgAddMeasurement.imgLink').live('click', function (evt) {
            addMeasurement();
        });

        $('#lblAddMeasurement').die('click');
        $('#lblAddMeasurement').live('click', function (evt) {
            addMeasurement();
        });

        function addMeasurement() {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Partograf/PartografMonitoringEntryCtl1.ascx");
                var param = "0" + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Partograf", 700, 500, "X487^002");
            }
        }

        $('.imgEditMeasurement.imgLink').die('click');
        $('.imgEditMeasurement.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Partograf/PartografMonitoringEntryCtl1.ascx");
                var param = recordID + "|" + +$('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val(); // hdnID = TestOrderID
                openUserControlPopup(url, param, "Partograf", 700, 500, "X487^002");
            }
        });

        $('.imgDeleteMeasurement.imgLink').die('click');
        $('.imgDeleteMeasurement.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDelete(paramedicID)) {
                var message = "Hapus informasi pengukuran kondisi jalan partograf untuk pasien ini ?";
                displayConfirmationMessageBox("Partograf", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpDeleteMeasurement.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgCopyMeasurement.imgLink').die('click');
        $('.imgCopyMeasurement').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var message = "Lakukan copy pengukuran fetal/janin ?";
            displayConfirmationMessageBox('COPY :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Partograf/PartografMonitoringEntryCtl1.ascx");
                    var param = "0" + "|" + $('#<%=hdnPregnancyNo.ClientID %>').val() + "|" + $('#<%=hdnFetusID.ClientID %>').val() + "|" + $('#<%=hdnFetusNo.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnLMPDate.ClientID %>').val() + "|" + recordID;
                    openUserControlPopup(url, param, "Partograf", 700, 500, "X487^002");
                }
            });
        });

        $('.imgAddVitalSign.imgLink').die('click');
        $('.imgAddVitalSign.imgLink').live('click', function (evt) {
            addVitalSign();
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            addVitalSign();
        });

        function addVitalSign() {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "08" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + "0" + "|" + "0" + "|" + "0" + "|";
                openUserControlPopup(url, param, "Partograf : Kondisi Ibu", 700, 500, "X487^004");
            }
        }

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "08" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + recordID + "|" + "0" + "|" + "0" + "|";
                openUserControlPopup(url, param, "Partograf : Kondisi Ibu", 700, 500, "X487^004");
            }
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi pemantauan kondisi ibu untuk partograf ini ?";
                displayConfirmationMessageBox("Partograf : Kondisi Ibu", message, function (result) {
                    if (result) {
                        cbpDeleteIntra.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgCopyVitalSign.imgLink').die('click');
        $('.imgCopyVitalSign').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Lakukan copy pemantauan kondisi ibu ?";
            displayConfirmationMessageBox('COPY :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/CopyVitalSignCtl1.ascx");
                    var param = "08" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + recordID + "|" + "0" + "|" + "0" + "|";
                    openUserControlPopup(url, param, 'Copy', 700, 500, "X487^004");
                }
            });
        });

        $('.imgAddMedication.imgLink').die('click');
        $('.imgAddMedication.imgLink').live('click', function (evt) {
            addMedication();
        });

        $('#lblAddMedication').die('click');
        $('#lblAddMedication').live('click', function (evt) {
            addMedication();
        });

        function addMedication() {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Partograf/PartografMedicationEntryCtl1.ascx");
                var param = "0" + "|" + +$('#<%=hdnPageVisitID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnID.ClientID %>').val();
                openUserControlPopup(url, param, "Partograf - Oksitosin", 700, 500, "X487^002");
            }
        }

        $('.imgEditMedication.imgLink').die('click');
        $('.imgEditMedication.imgLink').live('click', function (evt) {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Partograf/PartografMedicationEntryCtl1.ascx");
                var param = recordID + "|" + +$('#<%=hdnPageVisitID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnID.ClientID %>').val();
                openUserControlPopup(url, param, "Partograf - Oksitosin", 700, 500, "X487^002");
            }
        });

        $('.imgDeleteMedication.imgLink').die('click');
        $('.imgDeleteMedication.imgLink').live('click', function () {
            var allow = true;
            if (allow) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi pemberian Oksitosin ?";
                displayConfirmationMessageBox("Partograf : Pemberian Oksitosin", message, function (result) {
                    if (result) {
                        cbpDeleteMedication.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgCopyMedication.imgLink').die('click');
        $('.imgCopyMedication').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var message = "Lakukan copy informasi pemberian oksitosin ?";
            displayConfirmationMessageBox('COPY :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Partograf/PartografMedicationEntryCtl1.ascx");
                    var param = "0" + "|" + +$('#<%=hdnPageVisitID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + recordID;
                    openUserControlPopup(url, param, "Partograf : Pemberian Oksitosin", 700, 500, "X487^002");
                }
            });
        });

        $('#lblAddLaborStageForm').die('click');
        $('#lblAddLaborStageForm').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
                var param = "0" + "|" + $('#<%=hdnID.ClientID %>').val() + "|1|||||||";
                openUserControlPopup(url, param, "Catatan Persalinan - Kala I", 700, 500);
            }
        });

        $('#lblAddStage1Form').die('click');
        $('#lblAddStage1Form').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|1|||||||";
            openUserControlPopup(url, param, "Catatan Persalinan - Kala I", 700, 500);
        });

        $('#lblAddStage2Form').die('click');
        $('#lblAddStage2Form').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|2|||||||";
            openUserControlPopup(url, param, "Catatan Persalinan - Kala II", 700, 500);
        });

        $('#lblAddStage3Form').die('click');
        $('#lblAddStage3Form').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|3|||||||";
            openUserControlPopup(url, param, "Catatan Persalinan - Kala III", 700, 500);
        });

        $('#lblAddStage4Form').die('click');
        $('#lblAddStage4Form').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val() + "|4|||||||";
            openUserControlPopup(url, param, "Catatan Persalinan - Kala IV", 700, 500);
        });

        $('.imgEditStage1.imgLink').die('click');
        $('.imgEditStage1.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var param = recordID + "|" + antenatalRecordID + "|1|" + stageDate + "|" + stageTime + "|" + paramedicID + "|" + layout + "|" + values;
                openUserControlPopup(url, param, "Catatan Persalinan - KALA I", 700, 500);
            }
            else {
                displayErrorMessageBox('Catatan Persalinan', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgEditStage2.imgLink').die('click');
        $('.imgEditStage2.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var param = recordID + "|" + antenatalRecordID + "|2|" + stageDate + "|" + stageTime + "|" + paramedicID + "|" + layout + "|" + values;
                openUserControlPopup(url, param, "Catatan Persalinan - KALA II", 700, 500);
            }
            else {
                displayErrorMessageBox('Catatan Persalinan', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgEditStage3.imgLink').die('click');
        $('.imgEditStage3.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var param = recordID + "|" + antenatalRecordID + "|3|" + stageDate + "|" + stageTime + "|" + paramedicID + "|" + layout + "|" + values;
                openUserControlPopup(url, param, "Catatan Persalinan - KALA III", 700, 500);
            }
            else {
                displayErrorMessageBox('Catatan Persalinan', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgEditStage4.imgLink').die('click');
        $('.imgEditStage4.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/LaborStage1Entry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicID = $(this).attr('paramedicID');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var param = recordID + "|" + antenatalRecordID + "|4|" + stageDate + "|" + stageTime + "|" + paramedicID + "|" + layout + "|" + values;
                openUserControlPopup(url, param, "Catatan Persalinan - KALA IV", 700, 500);
            }
            else {
                displayErrorMessageBox('Catatan Persalinan', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgDeleteStage1.imgLink').die('click');
        $('.imgDeleteStage1.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Catatan Persalinan KALA I untuk pasien ini ?";
            displayConfirmationMessageBox("Catatan Persalinan", message, function (result) {
                if (result) {
                    cbpDeleteLaborStageInfo.PerformCallback("1" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteStage2.imgLink').die('click');
        $('.imgDeleteStage2.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Catatan Persalinan KALA II untuk pasien ini ?";
            displayConfirmationMessageBox("Catatan Persalinan", message, function (result) {
                if (result) {
                    cbpDeleteLaborStageInfo.PerformCallback("2" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteStage3.imgLink').die('click');
        $('.imgDeleteStage3.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Catatan Persalinan KALA III untuk pasien ini ?";
            displayConfirmationMessageBox("Catatan Persalinan", message, function (result) {
                if (result) {
                    cbpDeleteLaborStageInfo.PerformCallback("3" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteStage4.imgLink').die('click');
        $('.imgDeleteStage4.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Catatan Persalinan KALA IV untuk pasien ini ?";
            displayConfirmationMessageBox("Catatan Persalinan", message, function (result) {
                if (result) {
                    cbpDeleteLaborStageInfo.PerformCallback("4" + "|" + recordID);
                }
            });
        });

        $('.imgViewStage1.imgLink').die('click');
        $('.imgViewStage1.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/ViewLaborStageEntry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicName = $(this).attr('paramedicName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            var param = recordID + "|" + antenatalRecordID + "|1|" + stageDate + "|" + stageTime + "|" + paramedicName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Catatan Persalinan - KALA I", 700, 500);
        });

        $('.imgViewStage2.imgLink').die('click');
        $('.imgViewStage2.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/ViewLaborStageEntry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicName = $(this).attr('paramedicName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            var param = recordID + "|" + antenatalRecordID + "|2|" + stageDate + "|" + stageTime + "|" + paramedicName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Catatan Persalinan - KALA II", 700, 500);
        });

        $('.imgViewStage3.imgLink').die('click');
        $('.imgViewStage3.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/ViewLaborStageEntry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicName = $(this).attr('paramedicName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            var param = recordID + "|" + antenatalRecordID + "|3|" + stageDate + "|" + stageTime + "|" + paramedicName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Catatan Persalinan - KALA III", 700, 500);
        });

        $('.imgViewStage4.imgLink').die('click');
        $('.imgViewStage4.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Partograf/ViewLaborStageEntry.ascx");
            var recordID = $(this).attr('recordID');
            var antenatalRecordID = $(this).attr('antenatalRecordID');
            var stageDate = $(this).attr('stageDate');
            var stageTime = $(this).attr('stageTime');
            var paramedicName = $(this).attr('paramedicName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');

            var param = recordID + "|" + antenatalRecordID + "|4|" + stageDate + "|" + stageTime + "|" + paramedicName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Catatan Persalinan - KALA IV", 700, 500);
        });

        $('.imgDeleteLaborStage.imgLink').die('click');
        $('.imgDeleteLaborStage.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus Catatan Persalinan untuk pasien ini ?";
            displayConfirmationMessageBox("Catatan Persalinan", message, function (result) {
                if (result) {
                    cbpDeleteLaborStage.PerformCallback(recordID);
                }
            });
        });

        function onCbpDeleteFetusEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Informasi Janin', param[1]);
            }
        }

        function onCbpDeleteMeasurementEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1_1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Fetal Measurement', param[1]);
            }
        }

        function onCbpDeleteVitalSignEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Partograf : Kondisi Ibu', param[1]);
            }
        }

        function onCbpDeleteMedicationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt4.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Partograf : Pemberian Oksitosin', param[1]);
            }
        }

        function onCbpDeleteLaborStageEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Catatan Persalinan', param[1]);
            }
        }

        function onCbpDeleteLaborStageInfoEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Catatan Persalinan', param[1]);
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

        function onRefreshGridPartografDt1() {
            cbpViewDt1_1.PerformCallback('refresh');
        }

        function onRefreshVitalSignGrid() {
            cbpViewDt3.PerformCallback('refresh');
        }

        function onRefreshLaborStageGrid() {
            cbpViewDt2.PerformCallback('refresh');
        }

        function onRefreshMedicationAdministrationLog() {
            cbpViewDt4.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPregnancyNo" runat="server" />
    <input type="hidden" value="" id="hdnLMPDate" runat="server" />
    <input type="hidden" value="" id="hdnFetusID" runat="server" />
    <input type="hidden" value="" id="hdnFetusNo" runat="server" />
    <input type="hidden" value="" id="hdnPageVisitID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnSubMenuType" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
<input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col />
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
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEditAntenatal imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordid="<%#:Eval("ID") %>" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                               <asp:BoundField DataField="PregnancyNo" HeaderText="Kehamilan Ke-" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="pregnancyNo" />
                                            <asp:BoundField DataField="cfGPA" HeaderText="G-P-A" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="cfMembraneDateTime" HeaderText="Pecah Ketuban " HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="cfColicDateTime" HeaderText="Mulai Mules " HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>                                            
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada catatan antenatal record untuk pasien ini")%>
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
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="partografPage1">
                                        <%=GetLabel("Kondisi Jalan dan Kemajuan Persalinan")%></li>
                                    <li contentid="partografPage2">
                                        <%=GetLabel("Kondisi Ibu")%></li>
                                    <li contentid="partografPage3">
                                        <%=GetLabel("Oksitosin")%></li>
                                    <li contentid="partografPage4">
                                        <%=GetLabel("Catatan Persalinan")%></li>
                                    <li contentid="partografPage5">
                                        <%=GetLabel("Grafik Partograf")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="partografPage1">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:250px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                       <td valign="top">
                                            <div style="position:relative">
                                                <dxcp:ASPxCallbackPanel ID="cbpViewDt1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1"
                                                    ShowLoadingPanel="false" OnCallback="cbpViewDt1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px" Visible="False">
                                                                            <HeaderTemplate>                                                    
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table cellpadding="1" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Janin Ke-"  DataField="FetusNo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="fetusNo" />
                                                                        <asp:BoundField HeaderText="Jenis Kelamin"  DataField="Sex" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="sex" />
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <div>
                                                                            <div class="blink"><%=GetLabel("Belum ada informasi janin") %></div>
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>    
                                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt1" >
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                    <div class="wrapperPaging">
                                                </div>
                                                <div class="containerPaging">
                                                        <div id="pagingDt1"></div>
                                                    </div>
                                                </div> 
                                            </div>
                                        </td>
                                       <td valign="top">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt1_1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1_1"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDt1_1_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1_1').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewDt1_1EndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent6" runat="server">
                                                        <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewDt1_1" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="80px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddMeasurement imgLink" title='<%=GetLabel("+ Pengukuran")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" fetusID = "<%#:Eval("FetusID") %>"  />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                <img class="imgEditMeasurement imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" fetusID = "<%#:Eval("FetusID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"/>
                                                                                <img class="imgDeleteMeasurement imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" fetusID = "<%#:Eval("FetusID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"/>
                                                                                <img class="imgCopyMeasurement imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" fetusID = "<%#:Eval("FetusID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"/>                                        
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>    
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign = "Center" /> 
                                                                    <asp:BoundField DataField="LogTime" HeaderText="Jam" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign = "Center" />                                                     
                                                                    <asp:BoundField DataField="DJJ" HeaderText="DJJ (/menit)" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"  />
                                                                    <asp:BoundField DataField="AirKetuban" HeaderText="Air Ketuban" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />
                                                                    <asp:BoundField DataField="Penyusupan" HeaderText="Penyusupan"  HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />
                                                                    <asp:BoundField DataField="Pembukaan" HeaderText="Pembukaan" HeaderStyle-Width="60px"  HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                    <asp:BoundField DataField="cfTurunKepala" HeaderText="Turunnya Kepala" HeaderStyle-Width="60px"  HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                    <asp:BoundField DataField="TipeKonstraksi" HeaderText="Kontraksi"  HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                    <asp:BoundField DataField="FrekuensiKontraksi" HeaderText="Frekuensi Kontraksi" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div><%=GetLabel("Belum ada informasi pengukuran janin ini.") %></div>
                                                                        <br />
                                                                        <span class="lblLink" id="lblAddMeasurement">
                                                                            <%= GetLabel("+ Pengukuran")%></span>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>   
                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt1_1" >
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingDt1_1"></div>
                                                </div>
                                            </div> 
                                        </td>
                                    </tr>
                                </table>
                             </div>
                            <div class="containerOrderDt" id="partografPage2" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="panContentGridViewDt3" runat="server">
                                            <asp:Panel runat="server" ID="panGridViewDt3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt3_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="80px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddVitalSign imgLink" title='<%=GetLabel("+ Pemantauan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("AntenatalRecordID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div id="divVitalSignButton" runat="server" style='margin-top: 5px; text-align: center'>
                                                                    <img class="imgEditVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("AntenatalRecordID") %>" />
                                                                    <img class="imgDeleteVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("AntenatalRecordID") %>" />
                                                                    <img class="imgCopyVitalSign imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("AntenatalRecordID") %>" />                                        
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser hiddenColumn" ItemStyle-CssClass="keyUser hiddenColumn" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    Indikator Pemantauan Kondisi Ibu</div>
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
                                                            <div><%=GetLabel("Belum ada informasi pemantauan kondisi Ibu") %></div>
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
                            <div class="containerOrderDt" id="partografPage3" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="panContentGridViewDt4" runat="server">
                                            <asp:Panel runat="server" ID="panGridViewDt4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="80px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddMedication imgLink" title='<%=GetLabel("+ Pengukuran")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>"  />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div id="divMedicationButton" runat="server" style='margin-top: 5px; text-align: center'>
                                                                    <img class="imgEditMedication imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"/>
                                                                    <img class="imgDeleteMedication imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"/>
                                                                    <img class="imgCopyMedication imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"/>                                        
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>    
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign = "Center" /> 
                                                        <asp:BoundField DataField="LogTime" HeaderText="Jam" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Center" ItemStyle-HorizontalAlign = "Center" />    
                                                        <asp:BoundField DataField="ItemName" HeaderText="Oksitosin" HeaderStyle-HorizontalAlign = "Left" ItemStyle-HorizontalAlign = "Left" />      
                                                        <asp:BoundField DataField="cfDosingWithUnit" HeaderText="Dosis Pemberian" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />                                                
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada informasi pemberian oksitosin") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddIntraMedication">
                                                                <%= GetLabel("+ Oksitosin")%></span>
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
                            <div class="containerOrderDt" id="partografPage4" style="display:none">
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
                                                                            <img class="imgDeleteLaborStage imgLink" title='<%=GetLabel("Delete Catatan Persalinan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="KALA I" ItemStyle-Width="22.50%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsStage1InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfStage1DateTime") %></div>
                                                                            <div <%# Eval("cfIsStage1InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("Stage1ParamedicName") %></div>
                                                                            <div <%# Eval("cfIsStage1InfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Catatan Kala Persalinan I untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddStage1Form" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>">
                                                                                    <%= GetLabel("+ Catatan Kala I")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsStage1InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditStage1 imgLink" title='<%=GetLabel("Edit Kala I")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage1Layout") %>" values = "<%#:Eval("Stage1Values") %>" 
                                                                                            paramedicID = "<%#:Eval("Stage1ParamedicID") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage1Date") %>" stageTime = "<%#:Eval("Stage1Time") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteStage1 imgLink" title='<%=GetLabel("Delete Kala I")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewStage1 imgLink" title='<%=GetLabel("Lihat Catatan Persalinan : Kala I")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage1Layout") %>" values = "<%#:Eval("Stage1Values") %>" 
                                                                                            paramedicName = "<%#:Eval("Stage1ParamedicName") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage1Date") %>" stageTime = "<%#:Eval("Stage1Time") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="KALA II" ItemStyle-Width="22.50%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsStage2InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfStage2DateTime") %></div>
                                                                            <div <%# Eval("cfIsStage2InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("Stage2ParamedicName") %></div>
                                                                            <div <%# Eval("cfIsStage2InfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Catatan Persalinan Kala II untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddStage2Form" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>">
                                                                                    <%= GetLabel("+ Catatan Kala II")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsStage2InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditStage2 imgLink" title='<%=GetLabel("Edit Kala II")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage2Layout") %>" values = "<%#:Eval("Stage2Values") %>" 
                                                                                            paramedicID = "<%#:Eval("Stage2ParamedicID") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage2Date") %>" stageTime = "<%#:Eval("Stage2Time") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteStage2 imgLink" title='<%=GetLabel("Delete Kala II")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewStage2 imgLink" title='<%=GetLabel("Lihat Catatan Persalinan : Kala II")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage2Layout") %>" values = "<%#:Eval("Stage2Values") %>" 
                                                                                            paramedicName = "<%#:Eval("Stage2ParamedicName") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage2Date") %>" stageTime = "<%#:Eval("Stage2Time") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="KALA III" ItemStyle-Width="22.50%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsStage3InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfStage3DateTime") %></div>
                                                                            <div <%# Eval("cfIsStage3InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("Stage3ParamedicName") %></div>
                                                                            <div <%# Eval("cfIsStage3InfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Catatan Persalinan Kala III untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddStage3Form" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>">
                                                                                    <%= GetLabel("+ Catatan Kala III")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsStage3InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditStage3 imgLink" title='<%=GetLabel("Edit Kala III")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage3Layout") %>" values = "<%#:Eval("Stage3Values") %>" 
                                                                                            paramedicID = "<%#:Eval("Stage3ParamedicID") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage3Date") %>" stageTime = "<%#:Eval("Stage3Time") %>"  />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteStage3 imgLink" title='<%=GetLabel("Delete Kala III")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewStage3 imgLink" title='<%=GetLabel("Lihat Catatan Persalinan : Kala III")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage3Layout") %>" values = "<%#:Eval("Stage3Values") %>" 
                                                                                            paramedicName = "<%#:Eval("Stage3ParamedicName") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage3Date") %>" stageTime = "<%#:Eval("Stage3Time") %>"  />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px" HeaderText="KALA IV" ItemStyle-Width="22.50%">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width:60px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <div <%# Eval("cfIsStage4InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("cfStage4DateTime") %></div>
                                                                            <div <%# Eval("cfIsStage4InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>><%#:Eval("Stage4ParamedicName") %></div>
                                                                            <div <%# Eval("cfIsStage4InfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div><%=GetLabel("Belum ada informasi Catatan Persalinan Kala IV untuk pasien ini") %></div>
                                                                                <br />
                                                                                <span class="lblLink" id="lblAddStage4Form" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>">
                                                                                    <%= GetLabel("+ Catatan Kala IV")%></span>                                                                            
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" <%# Eval("cfIsStage4InfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditStage4 imgLink" title='<%=GetLabel("Edit Kala IV")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage4Layout") %>" values = "<%#:Eval("Stage4Values") %>" 
                                                                                            paramedicID = "<%#:Eval("Stage4ParamedicID") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage4Date") %>" stageTime = "<%#:Eval("Stage4Time") %>"  />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteStage4 imgLink" title='<%=GetLabel("Delete Kala IV")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewStage4 imgLink" title='<%=GetLabel("Lihat Catatan Persalinan : Kala IV")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" antenatalRecordID = "<%#:Eval("AntenatalRecordID") %>" layout = "<%#:Eval("Stage4Layout") %>" values = "<%#:Eval("Stage4Values") %>" 
                                                                                            paramedicName = "<%#:Eval("Stage4ParamedicName") %>" 
                                                                                            stageDate = "<%#:Eval("cfStage4Date") %>" stageTime = "<%#:Eval("Stage4Time") %>"  />
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
                                                            <div><%=GetLabel("Belum ada informasi Catatan Persalinan untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddLaborStageForm">
                                                                <%= GetLabel("+ Catatan Persalinan")%></span>
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteFetus" runat="server" Width="100%" ClientInstanceName="cbpDeleteFetus"
            ShowLoadingPanel="false" OnCallback="cbpDeleteFetus_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteFetusEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>  
        <dxcp:ASPxCallbackPanel ID="cbpDeleteMeasurement" runat="server" Width="100%" ClientInstanceName="cbpDeleteMeasurement"
            ShowLoadingPanel="false" OnCallback="cbpDeleteMeasurement_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteMeasurementEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteVitalSignEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>  
        <dxcp:ASPxCallbackPanel ID="cbpDeleteMedication" runat="server" Width="100%" ClientInstanceName="cbpDeleteMedication"
            ShowLoadingPanel="false" OnCallback="cbpDeleteMedication_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteMedicationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>   
        <dxcp:ASPxCallbackPanel ID="cbpDeleteLaborStage" runat="server" Width="100%" ClientInstanceName="cbpDeleteLaborStage"
            ShowLoadingPanel="false" OnCallback="cbpDeleteLaborStage_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteLaborStageEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>             
        <dxcp:ASPxCallbackPanel ID="cbpDeleteLaborStageInfo" runat="server" Width="100%" ClientInstanceName="cbpDeleteLaborStageInfo"
            ShowLoadingPanel="false" OnCallback="cbpDeleteLaborStageInfo_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteLaborStageInfoEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>                      
    </div>
</asp:Content>
