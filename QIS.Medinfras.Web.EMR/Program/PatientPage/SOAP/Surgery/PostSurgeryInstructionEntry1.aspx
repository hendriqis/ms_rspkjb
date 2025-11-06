<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="PostSurgeryInstructionEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PostSurgeryInstructionEntry1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Kembali ke Daftar")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Batal Perubahan")%></div>
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
                    Instruksi Paska Bedah Terintegrasi</div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .txtForm
        {
            border-color: #f5f6fa;
            background-color: #f5f6fa;
        }
        .txtForm:disabled
        {
            border-color: #dfe6e9;
            background-color: #dfe6e9;
        }
        .ddlForm
        {
            border-color: #f5f6fa;
            background-color: #f5f6fa;
        }
        .ddlForm:disabled
        {
            border-color: #dfe6e9;
            background-color: #dfe6e9;
        }
    </style>
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                showContent(contentID);
            });

            function showContent(contentID) {
                var i, x, tablinks;
                x = document.getElementsByClassName("divPageNavPanelContent");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(contentID).style.display = "block";
            }
            //#endregion

            setDatePicker('<%=txtInstructionDate.ClientID %>');
            $('#<%=txtInstructionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if ($('#<%=hdnRecordID.ClientID %>').val() == '') {
                    var mainRecordID = $('#<%=hdnMainRecordID.ClientID %>').val();
                    $('#<%=hdnRecordID.ClientID %>').val(mainRecordID);
                }

                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtInstructionTime.ClientID %>').val())) {
                        getFormContent1Values();
                        getFormContent2Values();
                        getFormContent3Values();
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                    else {
                        displayErrorMessageBox('Asesmen', 'Format Waktu pengisian asesmen yang diinput salah');
                    }
                }
            });

            function validateTime(timeValue) {
                var result = true;
                if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
                    result = false;
                }
                else {
                    var sHours = timeValue.split(':')[0];
                    var sMinutes = timeValue.split(':')[1];

                    if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                        result = false;
                    }
                    else if (parseInt(sHours) == 0)
                        sHours = "00";
                    else if (sHours < 10)
                        sHours = "0" + sHours;

                    if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                        result = false;
                    }
                    else if (parseInt(sMinutes) == 0)
                        sMinutes = "00";
                    else if (sMinutes < 10)
                        sMinutes = "0" + sMinutes;
                }
                return result;
            }

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Are you sure to discard your changes ?";
                    displayConfirmationMessageBox("Asesmen Pra Anestesi", message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload();
                        }
                    });
                }
            });

            $('#<%=grdMedicationView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdMedicationView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnPrescriptionOrderID.ClientID %>').val($(this).find('.keyField').html());
                    $('#<%=hdnPrescriptionOrderNo.ClientID %>').val($(this).find('.prescriptionOrderNo').html());
                    $('#<%=hdnIsMedicationEditable.ClientID %>').val($(this).find('.isEditable').html());
                    cbpMedicationViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdMedicationView.ClientID %> tr:eq(1)').click();

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


            //#region Form Values
            if ($('#<%=hdnMonitoringValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnMonitoringValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            if ($('#<%=hdnNutritionFormValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnNutritionFormValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            if ($('#<%=hdnOtherInstructionValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnOtherInstructionValue.ClientID %>').val().split(';');
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
                    $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                    });
                }
            }
            //#endregion

            registerCollapseExpandHandler();

            $('#leftPageNavPanel ul li').first().click();
        });

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
            addLabOrder();
        });

        $('#lblAddLabOrder2').die('click');
        $('#lblAddLabOrder2').live('click', function (evt) {
            if ($('#<%=hdnRecordID.ClientID %>').val() == '0' || $('#<%=hdnRecordID.ClientID %>').val() == '') {
                displayMessageBox("Instruksi Paska Bedah Terintegrasi", "Harap lakukan proses penyimpanan data terlebih dahulu sebelum proses ini.");
            } else {

                var width = 1150;
                var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
                var chiefComplaint = "";
                var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var recordID = $('#<%=hdnRecordID.ClientID %>').val();
                var param = "X001^004|0|" + clinicalNotes + "|" + chiefComplaint + "|" + laboratoryServiceUnitID + "|" + recordID;
                var title = "Laboratory Test Order - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                openUserControlPopup('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderLabQuickPicksCtl1.ascx', param, title, width, 600);
            }

        });

        function addLabOrder() {
            if ($('#<%=hdnRecordID.ClientID %>').val() == '0' || $('#<%=hdnRecordID.ClientID %>').val() == '') {
                displayMessageBox("Instruksi Paska Bedah Terintegrasi", "Harap lakukan proses penyimpanan data terlebih dahulu sebelum proses ini.");
            }
            else {
                var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
                var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
                var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
                var chiefComplaint = "";
                var testOrderID = "0";
                var recordID = $('#<%=hdnRecordID.ClientID %>').val();
                var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime + "|" + recordID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Order Pemeriksaan Laboratorium", 1200, 600);
            }
        }

        $('.imgAddLabOrderDt.imgLink').die('click');
        $('.imgAddLabOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = "";
            var recordID = $('#<%=hdnRecordID.ClientID %>').val();
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val($(this).attr('recordID'));
            var testOrderID = $(this).attr('recordID');

            var param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime + "|" + recordID;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Order Pemeriksaan Laboratorium", 1200, 600);
        });

        $('.imgEditLabOrder.imgLink').die('click');
        $('.imgEditLabOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val($(this).attr('recordID'));
            var testOrderID = $(this).attr('recordID');

            var param = "LB|" + $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Order Laboratorium", 700, 500);
        });

        $('.imgDeleteLabOrder.imgLink').die('click');
        $('.imgDeleteLabOrder.imgLink').live('click', function () {
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val($(this).attr('recordID'));
            var message = "Hapus order pemeriksaan laboratorium ?";
            displayConfirmationMessageBox("Instruksi Paska Bedah Terintegrasi", message, function (result) {
                if (result) {
                    cbpDeleteTestOrder.PerformCallback('LB');
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
            });
        });

        $('.imgSendLabOrder.imgLink').die('click');
        $('.imgSendLabOrder.imgLink').live('click', function () {
            $('#<%=hdnLaboratoryTestOrderID.ClientID %>').val($(this).attr('recordID'));

            if ($('#<%:hdnLaboratoryTestOrderID.ClientID %>').val() == "") {
                displayErrorMessageBox("Send Order : Laboratorium", "Tidak ada order pemeriksaan laboratorium yang dikirim");
            }
            else {
                var message = "Kirim order pemeriksaan laboratorium ?";
                displayConfirmationMessageBox("Instruksi Paska Bedah Terintegrasi", message, function (result) {
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
            var summaryText = s.cpSummary;
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

            $('#<%=hdnLaboratorySummary.ClientID %>').val(summaryText);
        }

        function onRefreshLaboratoryGrid() {
            cbpLaboratoryView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
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
            if ($('#<%=hdnRecordID.ClientID %>').val() == '0' || $('#<%=hdnRecordID.ClientID %>').val() == '') {
                displayMessageBox("Instruksi Paska Bedah Terintegrasi", "Harap lakukan proses penyimpanan data terlebih dahulu sebelum proses ini.");
            }
            else {
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
                var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
                var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
                var chiefComplaint = "";
                var recordID = $('#<%=hdnRecordID.ClientID %>').val();
                var testOrderID = "0";
                var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime + "|" + recordID;
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Order Pemeriksaan Radiologi", 1200, 600);
            }
        });

        $('.imgAddImagingOrderDt.imgLink').die('click');
        $('.imgAddImagingOrderDt.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            var performDate = $('#<%=hdnDatePickerToday.ClientID %>').val()
            var performTime = $('#<%=hdnTimeToday.ClientID %>').val();
            var clinicalNotes = $('#<%=hdnDiagnosisSummary.ClientID %>').val();
            var chiefComplaint = "";
            var recordID = $('#<%=hdnRecordID.ClientID %>').val();
            $('#<%=hdnImagingTestOrderID.ClientID %>').val($(this).attr('recordID'));
            var testOrderID = $(this).attr('recordID');

            var param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + "X125^001" + "|" + 'Langsung Dikerjakan' + "|" + performDate + "|" + performTime + "|" + recordID;
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx", param, "Imaging Test Order", 1200, 600);
        });

        $('.imgEditImagingOrder.imgLink').die('click');
        $('.imgEditImagingOrder.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            $('#<%=hdnImagingTestOrderID.ClientID %>').val($(this).attr('recordID'));
            var param = "IS|" + $('#<%=hdnImagingTestOrderID.ClientID %>').val() + "|" + $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemEditCtl1.ascx", param, "Edit Imaging Order", 700, 500);
        });

        $('.imgDeleteImagingOrder.imgLink').die('click');
        $('.imgDeleteImagingOrder.imgLink').live('click', function () {
            $('#<%=hdnImagingTestOrderID.ClientID %>').val($(this).attr('recordID'));
            var message = "Hapus order pemeriksaan radiologi ?";
            displayConfirmationMessageBox("Instruksi Paska Bedah Terintegrasi", message, function (result) {
                if (result) {
                    cbpDeleteTestOrder.PerformCallback('IS');
                    $('#<%=hdnIsChanged.ClientID %>').val('1');
                }
            });
        });

        $('.imgSendImagingOrder.imgLink').die('click');
        $('.imgSendImagingOrder.imgLink').live('click', function () {
            $('#<%=hdnImagingTestOrderID.ClientID %>').val($(this).attr('recordID'));

            if ($('#<%:hdnImagingTestOrderID.ClientID %>').val() == "") {
                displayErrorMessageBox("Send Order : Radiologi", "Tidak ada order pemeriksaan radiologi yang dikirim");
            }
            else {
                var message = "Kirim order pemeriksaan radiologi ?";
                displayConfirmationMessageBox("Instruksi Paska Bedah Terintegrasi", message, function (result) {
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
            var summaryText = s.cpSummary;
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

            $('#<%=hdnImagingSummary.ClientID %>').val(summaryText);
        }

        function onRefreshImagingGrid() {
            cbpImagingView.PerformCallback('refresh');
            $('#<%=hdnIsChanged.ClientID %>').val('1');
        }
        //#endregion

        //#region Medication
        //#region Paging
        var pageCount = parseInt('<%=gridMedicationPageCount %>');
        $(function () {
            setPaging($("#pagingMedication"), pageCount, function (page) {
                cbpMedicationView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpMedicationViewEndCallback(s) {
            $('#containerImgLoadingMedicationView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdMedicationView.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingMedication"), pageCount, function (page) {
                    cbpMedicationView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdMedicationView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpMedicationViewDtEndCallback(s) {
            $('#containerImgLoadingMedicationViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdMedicationViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingMedicationDt"), pageCount, function (page) {
                    cbpMedicationViewDt.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'delete') {
                if (param[1] != '') {
                    displayErrorMessageBox("WARNING", param[1]);
                }
                else {
                    cbpMedicationViewDt.PerformCallback('refresh');
                }
            }
            else
                $('#<%=grdMedicationViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.imgAddMedicationOrder.imgLink').die('click');
        $('.imgAddMedicationOrder.imgLink').live('click', function (evt) {

            if (IsValid(null, 'fsTrx', 'mpTrx')) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/PrescriptionQuickPicksEntryCtl1.ascx');
                    var orderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
                    var registration = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var postSurgerInstructionID = $('#<%=hdnRecordID.ClientID %>').val();
                    var id = orderID + '|' + registration + '|' + visitID + '|' + postSurgerInstructionID;

                    if (postSurgerInstructionID == "0") {
                        showToast("WARNING", 'Mohon dilakukan proses simpan dahulu.');
                        hideLoadingPanel();
                    } else {
                        openUserControlPopup(url, id, 'Medication Order', 1200, 600);
                    }

                }
            }
        });

        $('.imgEditMedicationItem.imgLink').die('click');
        $('.imgEditMedicationItem.imgLink').live('click', function (evt) {
            var prescriptionOrderDetailID = $(this).attr('recordID');
            var dispensaryServiceUnitID = $(this).attr('dispensaryServiceUnitID')
            var locationID = $(this).attr('locationID')
            var url = ResolveUrl("~/Program/PatientPage/_PopupEntry/CPOE/EditPrescriptionDtCtl.ascx");
            var param = prescriptionOrderDetailID + '|' + dispensaryServiceUnitID + '|' + locationID;
            openUserControlPopup(url, param, 'Edit - Medication Order', 700, 450);
            cbpMedicationViewDt.PerformCallback(param);
        });

        $('.imgDeleteMedicationItem.imgLink').die('click');
        $('.imgDeleteMedicationItem.imgLink').live('click', function (evt) {
            var prescriptionOrderDetailID = $(this).attr('recordID');
            var param = 'delete|' + prescriptionOrderDetailID;
            cbpMedicationViewDt.PerformCallback(param);
        });

        //#region Propose
        $('.btnPropose').die('click');
        $('.btnPropose').live('click', function () {
            $btnPropose = $(this);
            displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Kirim order resep ke farmasi ?', function (result) {
                if (result) {
                    onCustomButtonClick('Propose');
                }
            });
        });
        //#endregion

        //#region Reopen
        $('.btnReopen').die('click');
        $('.btnReopen').live('click', function () {
            $btnReopen = $(this);
            displayConfirmationMessageBox('REOPEN ORDER : FARMASI', 'Reopen order resep ke farmasi ?', function (result) {
                if (result) {
                    onCustomButtonClick('ReOpen');
                }
            });
        });
        //#endregion

        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            if (type == 'save') {
                $('#<%:hdnMainRecordID.ClientID %>').val(retval);                
            }
            cbpMedicationView.PerformCallback('refresh');
        }

        function onRefreshDetailList() {
            cbpMedicationViewDt.PerformCallback('refresh');
        }

        function onRefreshList() {
            cbpMedicationView.PerformCallback('refresh');
        }

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

        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                getFormContent1Values();
                getFormContent2Values();
                getFormContent3Values();
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                displayConfirmationMessageBox("Asesment", message, function (result) {
                    if (result) {
                        getFormContent1Values();
                        getFormContent2Values();
                        getFormContent3Values();
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
            displayConfirmationMessageBox("Assessment", message, function (result) {
                if (result) {
                    getPhysicalFormValues();
                    getFormContent2Values();
                    getFormContent3Values();
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion     

        //#region Get Form Values
        function getFormContent1Values() {
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

            $('#<%=hdnMonitoringValue.ClientID %>').val(controlValues);

            return controlValues;
        }

        function getFormContent2Values() {
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

            $('#<%=hdnNutritionFormValue.ClientID %>').val(controlValues);

            return controlValues;
        }

        function getFormContent3Values() {
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

            $('#<%=hdnOtherInstructionValue.ClientID %>').val(controlValues);

            return controlValues;
        }
        //#endregion    

        function onGetLocalHiddenFieldValue(param) {
            $('#<%=hdnRecordID.ClientID %>').val(param);
        }

        function onBeforeOpenTrxPopup() {
            var assessmentID = $('#<%=hdnRecordID.ClientID %>').val();
            if (assessmentID == '0' || assessmentID == '') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onCbpSendOrderEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'sendOrder') {
                if (param[2] == 'success') {
                    displayMessageBox('ORDER PEMERIKSAAN', 'Permintaan Order Pemeriksaan Penunjang telah berhasil dikirim kepada unit penunjang.');

                    if (param[1] == 'LB') {
                        onRefreshLaboratoryGrid();
                    }
                    else if (param[1] == 'IS') {
                        onRefreshImagingGrid();
                    }
                    else if (param[1] == 'MD') {
                        onRefreshDiagnosticGrid();
                    }
                }
                else {
                    displayErrorMessageBox('SEND ORDER : FAILED', 'Error Message : ' + param[3]);
                }
            }
        }
        function onAfterSaveDetail(value) {
            cbpMedicationView.PerformCallback('refresh');
        }
    </script>
    <div>
        <input type="hidden" id="hdnRecordID" value="0" runat="server" />
        <input type="hidden" id="hdnMainRecordID" value="0" runat="server" />
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" id="hdnTestOrderID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="" />
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
        <input type="hidden" value="" id="hdnPrescriptionOrderNo" runat="server" />
        <input type="hidden" runat="server" id="hdnPastMedicalID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" runat="server" id="hdnIsMedicationEditable" value="0" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" id="hdnIsMainDiagnosisExists" runat="server" value='0' />
        <input type="hidden" value="" id="hdnLaboratorySummary" runat="server" />
        <input type="hidden" value="" id="hdnImagingSummary" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosticSummary" runat="server" />
        <input type="hidden" runat="server" id="hdnMonitoringLayout" value="" />
        <input type="hidden" runat="server" id="hdnMonitoringValue" value="" />
        <input type="hidden" runat="server" id="hdnNutritionLayout" value="" />
        <input type="hidden" runat="server" id="hdnNutritionFormValue" value="" />
        <input type="hidden" runat="server" id="hdnOtherInstructionLayout" value="" />
        <input type="hidden" runat="server" id="hdnOtherInstructionValue" value="" />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
        <input type="hidden" value="" id="hdnTimeToday" runat="server" />
        <input type="hidden" id="hdnIsOutstandingOrder" runat="server" value="0" />
        <input type="hidden" value="" id="hdnDefaultGCMedicationRoute" runat="server" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <div id="leftPageNavPanel" class="w3-border">
                            <ul>
                                <li contentid="divPage1" title="Observasi" class="w3-hover-red">Observasi</li>
                                <li contentid="divPage2" title="Nutrisi dan Transfusi" class="w3-hover-red">Nutrisi
                                    dan Transfusi</li>
                                <li contentid="divPage3" title="Manajemen Nyeri" class="w3-hover-red">Manajemen Nyeri</li>
                                <li contentid="divPage5" title="Pemeriksaan Laboratorium" class="w3-hover-red">Pemeriksaan
                                    Laboratorium</li>
                                <li contentid="divPage6" title="Pemeriksaan Radiologi" class="w3-hover-red">Pemeriksaan
                                    Radiologi</li>
                                <li contentid="divPage7" title="Instruksi Lain-lain" class="w3-hover-red">Instruksi
                                    Lain-lain</li>
                            </ul>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Waktu Pemberian Instruksi")%></label>
                                    </td>
                                    <td colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtInstructionDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtInstructionTime" Width="80px" CssClass="time" runat="server"
                                                        Style="text-align: center" MaxLength="5" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="padding-top: 5px;">
                                    <td style="vertical-align: top" colspan="4">
                                        <div id="divFormContent1" runat="server" style="height: 520px; overflow-y: auto;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr style="padding-top: 5px;">
                                    <td style="vertical-align: top" colspan="4">
                                        <div id="divFormContent2" runat="server" style="height: 520px; overflow-y: auto;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 70%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpMedicationView" runat="server" Width="100%" ClientInstanceName="cbpMedicationView"
                                                ShowLoadingPanel="false" OnCallback="cbpMedicationView_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMedicationView').show(); }"
                                                    EndCallback="function(s,e){ onCbpMedicationViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <asp:Panel runat="server" ID="pnlMedicationView" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdMedicationView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdMedicationView_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField DataField="PrescriptionOrderNo" HeaderStyle-CssClass="hiddenColumn prescriptionOrderNo"
                                                                        ItemStyle-CssClass="hiddenColumn prescriptionOrderNo" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddMedicationOrder imgLink" title='<%=GetLabel("+ Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" recordid="<%#:Eval("PostSurgeryInstructionID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgProceed" <%# Eval("IsProceed").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>' alt="" style="float: left"
                                                                                            title="Sudah Diproses" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgViewLog imgLink" <%# Eval("IsPrescriptionOrderHasChangesLog").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                                                                            style="float: left" title="Catatan Perubahan Order" width="32px" height="32px" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div>
                                                                                <%=GetLabel("Prescription Date - Time")%>,
                                                                                <%=GetLabel("Prescription No.")%></div>
                                                                            <div style="width: 250px; float: left">
                                                                                <%=GetLabel("Physician")%></div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <%#: Eval("PrescriptionDateInString")%>,
                                                                                            <%#: Eval("PrescriptionTime") %>,
                                                                                            <%#: Eval("PrescriptionOrderNo")%>
                                                                                        </div>
                                                                                        <div style="width: 250px; float: left">
                                                                                            <%#: Eval("ParamedicName") %>
                                                                                            <b>
                                                                                                <%# Eval("IsCreatedBySystem").ToString() == "False" ? "":"(Diorder Farmasi)" %></b>
                                                                                        </div>
                                                                                        <div style="width: 250px; float: left; font-size: x-small; font-style: italic">
                                                                                            <%#: Eval("cfSendOrderDateInformationInString") %>
                                                                                            <%#: Eval("cfChargesProposedInformationInString") %>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %>>
                                                                                        <div>
                                                                                            <input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color: Red;
                                                                                                color: White; width: 100px;" /></div>
                                                                                    </td>
                                                                                    <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %>>
                                                                                        <div>
                                                                                            <input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color: Green;
                                                                                                color: White; width: 100px" /></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada informasi order resep untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingMedicationView">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingMedication">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td valign="top">
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpMedicationViewDt" runat="server" Width="100%" ClientInstanceName="cbpMedicationViewDt"
                                                ShowLoadingPanel="false" OnCallback="cbpMedicationViewDt_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMedicationViewDt').show(); }"
                                                    EndCallback="function(s,e){ onCbpMedicationViewDtEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdMedicationViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdMedicationViewDt_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                                                        ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderTemplate>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditMedicationItem imgLink <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCTransactionStatus").ToString() != "X121^001" ? "imgDisabled" : "imgLink" %>"
                                                                                            title='<%=GetLabel("Edit")%>' src='<%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCTransactionStatus").ToString() != "X121^001" ? "display:none" : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" style="float: left" recordid='<%#: Eval("PrescriptionOrderDetailID")%>'
                                                                                            dispensaryserviceunitid='<%#: Eval("DispensaryServiceUnitID")%>' locationid='<%#: Eval("LocationID")%>' />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteMedicationItem imgLink <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCTransactionStatus").ToString() != "X121^001" ? "imgDisabled" : "imgLink" %>"
                                                                                            title='<%=GetLabel("Delete")%>' src='<%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCTransactionStatus").ToString() != "X121^001" ? "display:none" : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" style="float: left" recordid='<%#: Eval("PrescriptionOrderDetailID")%>'
                                                                                            dispensaryserviceunitid='<%#: Eval("DispensaryServiceUnitID")%>' locationid='<%#: Eval("LocationID")%>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemName"
                                                                        HeaderStyle-Width="220px">
                                                                        <HeaderTemplate>
                                                                            <div>
                                                                                <%=GetLabel("Drug Name")%>
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div <%# Eval("OrderIsDeleted").ToString() == "True"  || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "Style='color:red;font-style:italic; text-decoration: line-through'":"" %>>
                                                                                <span style="font-weight: bold">
                                                                                    <%#: Eval("cfMedicationName")%></span></div>
                                                                            <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                                <%#: Eval("cfCompoundDetail")%></div>
                                                                            <div>
                                                                                <%#: Eval("cfConsumeMethod3")%></div>
                                                                            <div>
                                                                                <%#: Eval("Route")%></div>
                                                                            <div>
                                                                                <%#: Eval("MedicationAdministration")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="StartDateInDatePickerFormat" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="DispenseQtyInString" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="cfTakenQty" HeaderText="Taken" HeaderStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="ItemUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <img id="imgHAM" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png") %>'
                                                                                    title='High Alert Medication' alt="" style="height: 24px; width: 24px;" /></div>
                                                                            <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAllergyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                                title='<%=GetLabel("Allergy Alert") %>' alt="" />
                                                                            <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAdverseReactionAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                                title='<%=GetLabel("Adverse Reaction") %>' alt="" />
                                                                            <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsDuplicateTheraphyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                                title='<%=GetLabel("Duplicate Theraphy") %>' alt="" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="120px"
                                                                        ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <img src='<%# ResolveUrl("~/Libs/Images/Button/plus.png") %>' <%# Eval("IsCreatedFromOrder").ToString() == "False" ? "" : "Style ='display:none'" %>
                                                                                    title='<%=GetLabel("Drug Add by Pharmacist") %>' alt="" width="10" height="10"
                                                                                    style="padding: 2px" />
                                                                                <img src='<%# ResolveUrl("~/Libs/Images/Status/stop_service.png") %>' <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "Style='cursor:pointer;padding-right:10px'" : "Style ='display:none'" %>
                                                                                    title='<%#: Eval("cfVoidReson")%>' alt="" width="15" height="15" />
                                                                            </div>
                                                                            <div <%# Eval("IsCreatedFromOrder").ToString() == "False" ? "" : "Style ='display:none'" %>>
                                                                                <i>
                                                                                    <%=GetLabel("C : ")%></i>
                                                                                <%#: Eval("CreatedByUserFullName")%>
                                                                            </div>
                                                                            <div <%# Eval("OrderIsDeleted").ToString() == "True" || Eval("GCPrescriptionOrderStatus").ToString() == "X126^004" ? "" : "Style ='display:none'" %>>
                                                                                <i>
                                                                                    <%=GetLabel("U : ")%></i>
                                                                                <%#: Eval("LastUpdatedByUserFullName")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada informasi order resep untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingMedicationViewDt">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingMedicationDt">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="divPageNavPanelContent w3-animate-left" style="display: none">
                            <div>
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
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: center">
                                                                                    <span class="lblLink" id="lblAddLabOrder2">
                                                                                        <%= GetLabel("+ Order (Form)")%></span>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img class="imgAddLabOrderDt imgLink" title='<%=GetLabel("Add Detail")%>' src='<%# ResolveUrl("~/Libs/Images/Button/add.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                                recordid="<%#:Eval("TestOrderID") %>" />
                                                                                        </td>
                                                                                        <td style="width: 1px">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <img class="imgEditLabOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                                recordid="<%#:Eval("TestOrderID") %>" />
                                                                                        </td>
                                                                                        <td style="width: 1px">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <img class="imgDeleteLabOrder imgLink" title='<%=GetLabel("Delete Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                                recordid="<%#:Eval("TestOrderID") %>" />
                                                                                        </td>
                                                                                        <td style="width: 1px">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <img class="imgSendLabOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                                alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                                recordid="<%#:Eval("TestOrderID") %>" />
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
                        </div>
                        <div id="divPage6" class="divPageNavPanelContent w3-animate-left" style="display: none">
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
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                            recordid="<%#:Eval("TestOrderID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgEditImagingOrder imgLink" title='<%=GetLabel("Edit Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                            recordid="<%#:Eval("TestOrderID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteImagingOrder imgLink" title='<%=GetLabel("Delete Order")%>'
                                                                                            src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                            recordid="<%#:Eval("TestOrderID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgSendImagingOrder imgLink" title='<%=GetLabel("Send Order")%>' src='<%# ResolveUrl("~/Libs/Images/Button/done.png")%>'
                                                                                            alt="" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                                            recordid="<%#:Eval("TestOrderID") %>" />
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
                        <div id="divPage7" class="divPageNavPanelContent w3-animate-left" style="display: none">
                            <table class="tblContentArea" style="width: 100%">
                                <colgroup>
                                    <col width="120px" />
                                </colgroup>
                                <tr>
                                    <td colspan="4" style="vertical-align: top">
                                        <div id="divFormContent3" runat="server" style="height: 110px; overflow-y: auto;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width: 120px">
                                                    <label class="lblNormal" id="lblAnesthesyRemarks">
                                                        <%=GetLabel("Instruksi Lainnya")%></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
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
