<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="TestOrderEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.TestOrderEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackTestOrderList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back to Order List")%></div>
    </li>
    <li id="btnSendOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
            <%=GetLabel("Send Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightButtonToolbar" runat="server">
    <li id="btnOpenTestOrderEntryQuickPicks" runat="server" crudmode="C">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("Quick Picks")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtPerformDate.ClientID %>');
            $('#<%=txtPerformDate.ClientID %>').datepicker('option', 'minDate', '0');

            if (cboToBePerformed.GetValue() != null && (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED)) {
                $('#<%=txtPerformDate.ClientID %>').removeAttr('readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('enable');
                $('#<%=txtPerformTime.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtPerformDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('disable');
                $('#<%=txtPerformTime.ClientID %>').attr('readonly', 'readonly');
            }
        });

        function onAfterSaveRecordPatientPageEntry(value) {
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '') {
                $('#<%=hdnTestOrderID.ClientID %>').val(value);
                cboParamedicID.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
            }
            cbpView.PerformCallback('refresh');
        }

        function onBeforeBasePatientPageListAdd() {
            var filterExpression = '';
            if ($('#<%=hdnTestOrderID.ClientID %>').val() != '')
                filterExpression = $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue()).replace('{ItemID}', $('#<%=hdnItemID.ClientID %>').val());
            else
                filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());

            filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'";

            ledItem.SetFilterExpression(filterExpression);
            return true;
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
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
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

        //#region Entity To Control
        function entityToControl(entity) {
            onCboToBePerformedChanged();
            cboServiceUnit.SetEnabled(false);
            ledItem.SetFocus();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var Remarks = $('#<%=hdnRemarks.ClientID %>').val();
            var isLaboratoryUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
            var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
                $('#<%=txtItemQty.ClientID %>').val(entity.ItemQty);
                cboDiagnose.SetValue(entity.DiagnoseID);
                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
                $('#<%=txtTestOrderDate.ClientID %>').val(entity.TestOrderDate);
                $('#<%=txtTestOrderTime.ClientID %>').val(entity.TestOrderTime);
                cboToBePerformed.SetValue(entity.GCToBePerformed);
                if (entity.GCToBePerformed == Constant.ToBePerformed.SCHEDULLED) {
                    $('#<%=txtPerformDate.ClientID %>').val(entity.PerformedDate);
                    $('#<%=txtPerformTime.ClientID %>').val(entity.PerformedTime);
                }
                else {
                    $('#<%=txtPerformDate.ClientID %>').val(entity.TestOrderDate);
                    $('#<%=txtPerformTime.ClientID %>').val(entity.TestOrderTime);
                }

                var filterExpression = '';
                if ($('#<%=hdnTestOrderID.ClientID %>').val() != '')
                    filterExpression = $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue()).replace('{ItemID}', entity.ItemID);
                else
                    filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());

                if (isLaboratoryUnit != '0') {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLimitedCPOEItemForBPJSLab.ClientID %>').val() == '1') {
                            filterExpression += " AND IsBPJS = 1 ";
                        }
                    }
                }
                else if (serviceUnitID == radiologyServiceUnitID) {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLimitedCPOEItemForBPJSRad.ClientID %>').val() == '1') {
                            filterExpression += " AND IsBPJS = 1 ";
                        }
                    }
                }
                else {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLimitedCPOEItemForBPJSOth.ClientID %>').val() == '1') {
                            filterExpression += " AND IsBPJS = 1 ";
                        }
                    }
                }

                filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'";

                ledItem.SetFilterExpression(filterExpression);
                ledItem.SetValue(entity.ItemID);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemQty.ClientID %>').val('1');
                cboDiagnose.SetValue('');
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var isLaboratoryUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();

                if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnImagingServiceUnitID.ClientID %>').val() && $('#<%=hdnEM0079.ClientID %>').val() == "0") {
                    Remarks = "";
                }
                $('#<%=txtRemarks.ClientID %>').val(Remarks);

                var filterExpression = '';
                if ($('#<%=hdnTestOrderID.ClientID %>').val() != '')
                    filterExpression = $('#<%=hdnFilterExpressionItemAdd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());
                else
                    filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());

                if (isLaboratoryUnit != '0') {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLimitedCPOEItemForBPJSLab.ClientID %>').val() == '1') {
                            filterExpression += " AND IsBPJS = 1 ";
                        }
                    }
                }
                else if (serviceUnitID == radiologyServiceUnitID) {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLimitedCPOEItemForBPJSRad.ClientID %>').val() == '1') {
                            filterExpression += " AND IsBPJS = 1 ";
                        }
                    }
                }
                else {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLimitedCPOEItemForBPJSOth.ClientID %>').val() == '1') {
                            filterExpression += " AND IsBPJS = 1 ";
                        }
                    }
                }

                filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'";

                ledItem.SetFilterExpression(filterExpression);
                ledItem.SetValue('');
                cbpView.PerformCallback('refresh');
            }
        }

        function onCboToBePerformedChanged() {
            if (cboToBePerformed.GetValue() != null && (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED)) {
                $('#<%=txtPerformDate.ClientID %>').removeAttr('readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('enable');
                $('#<%=txtPerformTime.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtPerformDate.ClientID %>').val($('#<%=hdnDatePickerToday.ClientID %>').val());
                $('#<%=txtPerformTime.ClientID %>').val($('#<%=hdnTimeToday.ClientID %>').val());
                $('#<%=txtPerformDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('disable');
                $('#<%=txtPerformTime.ClientID %>').attr('readonly', 'readonly');
            }
        }
        //#endregion

        function onBeforeChangePage() {
            if (($('#<%:hdnTestOrderID.ClientID %>').val() != "") && $('#<%:hdnIsProposed.ClientID %>').val() == "0") {
                var resultFinal = true;
                var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                if (serviceUnitID == operatingRoomID) {
                    var filterExpression = onGetScheduleFilterExpression();
                    Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                        if (result != null) {
                            var message = "Masih ada outstanding order kamar operasi, <b>lanjutkan proses pembuatan order lagi?<b>";
                            displayConfirmationMessageBox("CONFIRMATION", message, function (result) {
                                if (result) {
                                    cbpSendOrder.PerformCallback('sendOrder');
                                }
                                else {
                                    showLoadingPanel();
                                    document.location = document.referrer;
                                }
                            });
                            resultFinal = false;
                        }
                        else {
                            var message1 = "Send your order to Service Unit ?";
                            displayConfirmationMessageBox("SEND ORDER", message1, function (result1) {
                                if (result1) {
                                    cbpSendOrder.PerformCallback('sendOrder');
                                }
                                else {
                                    showLoadingPanel();
                                    document.location = document.referrer;
                                }
                            });
                        }
                    });
                }
                else {
                    if (orderID != '' && orderID != '0') {
                        var message2 = "Send your order to Service Unit ?";
                        displayConfirmationMessageBox("SEND ORDER", message2, function (result2) {
                            if (result2) {
                                cbpSendOrder.PerformCallback('sendOrder');
                            }
                            else {
                                showLoadingPanel();
                                document.location = document.referrer;
                            }
                        });
                    }
                    else {
                        showLoadingPanel();
                        document.location = document.referrer;
                    }
                }
            }
            else {
                gotoNextPage();
            }
        }

        function onBeforeBackToListPage() {
            if (($('#<%:hdnTestOrderID.ClientID %>').val() != "") && $('#<%:hdnIsProposed.ClientID %>').val() == "0") {
                var resultFinal = true;
                var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                if (serviceUnitID == operatingRoomID) {
                    var filterExpression = onGetScheduleFilterExpression();
                    Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                        if (result != null) {
                            var message = "Masih ada outstanding order kamar operasi, <b>lanjutkan proses pembuatan order lagi?<b>";
                            displayConfirmationMessageBox("CONFIRMATION", message, function (result) {
                                if (result) {
                                    cbpSendOrder.PerformCallback('sendOrder');
                                }
                                else {
                                    showLoadingPanel();
                                    document.location = document.referrer;
                                }
                            });
                            resultFinal = false;
                        }
                        else {
                            var message1 = "Send your order to Service Unit ?";
                            displayConfirmationMessageBox("SEND ORDER", message1, function (result1) {
                                if (result1) {
                                    cbpSendOrder.PerformCallback('sendOrder');
                                }
                                else {
                                    showLoadingPanel();
                                    document.location = document.referrer;
                                }
                            });
                        }
                    });
                }
                else {
                    if (orderID != '' && orderID != '0') {
                        var message2 = "Send your order to Service Unit ?";
                        displayConfirmationMessageBox("SEND ORDER", message2, function (result2) {
                            if (result2) {
                                cbpSendOrder.PerformCallback('sendOrder');
                            }
                            else {
                                showLoadingPanel();
                                document.location = document.referrer;
                            }
                        });
                    }
                    else {
                        showLoadingPanel();
                        document.location = document.referrer;
                    }
                }
            }
            else {
                backToPatientList();
            }
        }

        $(function () {
            setDatePicker('<%=txtPerformDate.ClientID %>');

            $('#<%=btnBackTestOrderList.ClientID %>').click(function () {
                var resultFinal = true;
                var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                if (serviceUnitID == operatingRoomID) {
                    var filterExpression = onGetScheduleFilterExpression();
                    Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                        if (result != null) {
                            var message = "Masih ada outstanding order kamar operasi, <b>lanjutkan proses pembuatan order lagi?<b>";
                            displayConfirmationMessageBox("CONFIRMATION", message, function (result) {
                                if (result) {
                                    cbpSendOrder.PerformCallback('sendOrder');
                                }
                                else {
                                    showLoadingPanel();
                                    document.location = document.referrer;
                                }
                            });
                            resultFinal = false;
                        }
                        else {
                            var message1 = "Send your order to Service Unit ?";
                            displayConfirmationMessageBox("SEND ORDER", message1, function (result1) {
                                if (result1) {
                                    cbpSendOrder.PerformCallback('sendOrder');
                                }
                                else {
                                    showLoadingPanel();
                                    document.location = document.referrer;
                                }
                            });
                        }
                    });
                }
                else {
                    if (orderID != '' && orderID != '0') {
                        var message2 = "Send your order to Service Unit ?";
                        displayConfirmationMessageBox("SEND ORDER", message2, function (result2) {
                            if (result2) {
                                cbpSendOrder.PerformCallback('sendOrder');
                            }
                            else {
                                showLoadingPanel();
                                document.location = document.referrer;
                            }
                        });
                    }
                    else {
                        showLoadingPanel();
                        document.location = document.referrer;
                    }
                }
            });

            $('#<%=btnOpenTestOrderEntryQuickPicks.ClientID %>').click(function () {
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                //                var serviceUnitID = cboServiceUnit.GetValue();
                var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var isLaboratoryUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
                var gcToBePerformed = cboToBePerformed.GetValue();
                var textToBePerformed = cboToBePerformed.GetText();
                var performDate = $('#<%=txtPerformDate.ClientID %>').val();
                var performTime = $('#<%=txtPerformTime.ClientID %>').val();
                var isMultiVisit = $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').is(":checked");
                var url = '';
                var width = 0;
                var testOrderID = "0";
                var clinicalNotes = $('#<%=txtRemarks.ClientID %>').val();
                var postSurgeryInstruction = '';
                var chiefComplaint = '';
                var param = "";
                var title = 'Quick Picks';

                if ($('#<%=hdnTestOrderID.ClientID %>').val() != '') {
                    testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                }
                if (isLaboratoryUnit == "1") {
                    title = "Order Pemeriksaan Laboratorium - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                    param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + gcToBePerformed + "|" + textToBePerformed + "|" + performDate + "|" + performTime + "|" + postSurgeryInstruction + "|" + serviceUnitID;
                    url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx');
                    width = 1200;
                }
                else if (serviceUnitID == radiologyServiceUnitID) {
                    title = "Order Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                    param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + gcToBePerformed + "|" + textToBePerformed + "|" + performDate + "|" + performTime + "|" + postSurgeryInstruction;
                    url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx');
                    width = 1200;
                }
                else {
                    title = "Order Pemeriksaan Penunjang - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                    param = "X001^006" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint + "|" + gcToBePerformed + "|" + textToBePerformed + "|" + performDate + "|" + performTime + "|1|" + isMultiVisit;
                    url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx');
                    width = 1200;
                }

                openUserControlPopup(url, param, title, width, 600);
            });

            $('.imgEdit.imgLink').die('click');
            $('.imgEdit.imgLink').live('click', function () {
                onPatientListEntryEditRecord();
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                onPatientListEntryDeleteRecord();
            });
        });

        function onGetScheduleFilterExpression() {
            var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
            if (serviceUnitID == operatingRoomID) {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var performDate = $('#<%=txtPerformDate.ClientID %>').val();
                var performDateInDatePicker = Methods.getDatePickerDate(performDate);
                var performDateFormatString = Methods.dateToString(performDateInDatePicker);
                var filterExpression = "VisitID = " + visitID + " AND ScheduledDate = '" + performDateFormatString + "' AND GCTransactionStatus NOT IN ('X121^999','X121^005') AND GCOrderStatus NOT IN ('X126^006','X121^005') AND TestOrderID != '" + orderID + "' AND HealthcareServiceUnitID = '" + serviceUnitID + "'";
                return filterExpression;
            }
        }

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

        $('#<%=btnSendOrder.ClientID %>').click(function () {
            if (validateTime($('#<%=txtPerformTime.ClientID %>').val())) {
                if ($('#<%:hdnTestOrderID.ClientID %>').val() == "") {
                    displayErrorMessageBox("SEND ORDER", "There is no order to be sent !");
                }
                else {
                    var resultFinal = true;
                    var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
                    var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                    var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                    if (serviceUnitID == operatingRoomID) {
                        var filterExpression = onGetScheduleFilterExpression();
                        Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                            if (result != null) {
                                var message = "Masih ada outstanding order kamar operasi, <b>lanjutkan proses pembuatan order lagi?<b>";
                                displayConfirmationMessageBox("CONFIRMATION", message, function (result) {
                                    if (result) cbpSendOrder.PerformCallback('sendOrder');
                                });
                                resultFinal = false;
                            }
                            else {
                                var message1 = "Send your order to Service Unit ?";
                                displayConfirmationMessageBox("SEND ORDER", message1, function (result1) {
                                    if (result1) cbpSendOrder.PerformCallback('sendOrder');
                                });
                            }
                        });
                    }
                    else {
                        var message2 = "Send your order to Service Unit ?";
                        displayConfirmationMessageBox("SEND ORDER", message2, function (result2) {
                            if (result2) cbpSendOrder.PerformCallback('sendOrder');
                        });
                    }
                }
            }
            else {
                showToast('Warning', 'Format Waktu yang diinput salah');
                return false;
            }
        });

        function onLedItemLostFocus(value) {
            $('#<%=hdnItemID.ClientID %>').val(value);
        }

        function onAfterSaveRecord(param) {
            var paramInfo = param.split('|');
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '') {
                $('#<%=hdnFilterExpressionItemAdd.ClientID %>').val($('#<%=hdnFilterExpressionItemAdd.ClientID %>').val().replace('{TestOrderID}', paramInfo[0]));
                $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val($('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{TestOrderID}', paramInfo[0]));

                $('#<%=hdnTestOrderID.ClientID %>').val(paramInfo[0]);
                cboParamedicID.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);

                if ($('#<%=hdnIsUsingMultiVisitScheduleOrder.ClientID %>').val() == "1") {
                    Methods.getObject("GetTestOrderHdList", "TestOrderID = " + paramInfo[0], function (result) {
                        if (result != null) {
                            if (result.IsMultiVisitScheduleOrder) {
                                $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", true);
                            }
                            else {
                                $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                            }
                        }
                    });
                }

                Methods.getObject("GetTestOrderHdList", "TestOrderID = " + paramInfo[0], function (result) {
                    if (result != null) {
                        if (result.IsCITO) {
                            $('#<%=chkIsCITO.ClientID %>').prop("checked", true);
                        } else {
                            $('#<%=chkIsCITO.ClientID %>').prop("checked", false);
                        }
                    }
                });


                cbpView.PerformCallback('refresh');
            }
            else {
                $('#<%=hdnTestOrderID.ClientID %>').val(paramInfo[0]);

                if ($('#<%=hdnIsUsingMultiVisitScheduleOrder.ClientID %>').val() == "1") {
                    Methods.getObject("GetTestOrderHdList", "TestOrderID = " + paramInfo[0], function (result) {
                        if (result != null) {
                            if (result.IsMultiVisitScheduleOrder) {
                                $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", true);
                            }
                            else {
                                $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                            }
                        }
                    });
                }

                cboParamedicID.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);

                cbpView.PerformCallback('refresh');
            }
        }

        function onCboServiceUnitValueChanged(param) {
            var filterExpression = "HealthcareServiceUnitID = " + param;
            Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                if (result != null) {
                    if (result.IsLaboratoryUnit) {
                        $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("1");
                        $('#<%:tdPATest.ClientID %>').removeAttr('style');
                    } else {
                        $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("0");
                        $('#<%:tdPATest.ClientID %>').attr('style', 'display:none');
                    }
                    if ($('#<%=hdnIsUsingMultiVisitScheduleOrder.ClientID %>').val() == "1") {
                        if (result.IsAllowMultiVisitSchedule) {
                            $('#<%:tdMultiVisitScheduleOrder.ClientID %>').removeAttr('style');
                        }
                        else {
                            $('#<%:tdMultiVisitScheduleOrder.ClientID %>').attr('style', 'display:none');
                            $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                        }
                    }
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                }
                else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("0");
                    $('#<%:tdPATest.ClientID %>').attr('style', 'display:none');
                    $('#<%:tdMultiVisitScheduleOrder.ClientID %>').attr('style', 'display:none');
                    $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                }
            });
        }

        function onBeforeSaveRecord() {
            return ledItem.Validate();
        }

        function onCbpSendOrderEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'sendOrder') {
                if (param[1] == 'success') {
                    showToast('Send Success', 'The test order was successfully sent to Service Unit.');
                    document.location = document.referrer;
                }
                else {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
            }
        }
    </script>
    <input type="hidden" id="hdnFilterExpressionItemNewTransHd" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemAdd" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemEdit" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnPopupResultID" runat="server" value="0" />
    <input type="hidden" id="hdnIsProposed" runat="server" value="0" />
    <input type="hidden" id="hdnEM0079" runat="server" value="0" />
    <input type="hidden" id="hdnPatientInformation" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDiagnosa" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChiefComplaint" runat="server" />
    <input type="hidden" value="" id="hdnIsNotesCopyDiagnose" runat="server" />
    <input type="hidden" value="" id="hdnRemarks" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
    <input type="hidden" value="" id="hdnTimeToday" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingMultiVisitScheduleOrder" runat="server" />
    <input type="hidden" value="" id="hdnIsLimitedCPOEItemForBPJSLab" runat="server" />
    <input type="hidden" value="" id="hdnIsLimitedCPOEItemForBPJSRad" runat="server" />
    <input type="hidden" value="" id="hdnIsLimitedCPOEItemForBPJSOth" runat="server" />
    <input type="hidden" value="" id="hdnIsBPJS" runat="server" />
    <table cellpadding="2" cellspacing="0">
        <colgroup>
            <col width="150px" />
            <col width="120px" />
            <col width="80px" />
            <col width="170px" />
            <col width="200px" />
            <col width="170px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <%=GetLabel("Tanggal ") %>
                -
                <%=GetLabel("Jam Order") %>
            </td>
            <td style="padding-right: 1px; width: 120px">
                <asp:TextBox ID="txtTestOrderDate" ReadOnly="true" Width="120px" CssClass="datepicker"
                    runat="server" />
            </td>
            <td style="width: 120px">
                <asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" ReadOnly="true" runat="server"
                    Style="text-align: center" />
            </td>
            <td class="tdLabel">
                <%=GetLabel("Physician") %>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="100%"
                    runat="server" />
            </td>
            <td>
                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
            </td>
            <td id="tdPATest" runat="server" colspan="2">
                <asp:CheckBox ID="chkIsPathologicalAnatomyTest" Width="150px" runat="server" Text=" Pemeriksaan PA" />
            </td>
            <td id="tdMultiVisitScheduleOrder" runat="server" colspan="2" style="display: none">
                <asp:CheckBox ID="chkIsMultiVisitScheduleOrder" Width="200px" runat="server" Text=" Penjadwalan Multi Kunjungan" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <%=GetLabel("Unit Pelayanan") %>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="210px"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){onCboServiceUnitValueChanged(s.GetValue()); }"
                        Init="function(s,e){ onCboServiceUnitValueChanged(s.GetValue()); }" />
                </dxe:ASPxComboBox>
            </td>
            <td class="tdLabel">
                <%=GetLabel("Waktu Pengerjaan Order") %>
            </td>
            <td>
                <dxe:ASPxComboBox runat="server" ID="cboToBePerformed" ClientInstanceName="cboToBePerformed"
                    Width="300px">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboToBePerformedChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
            <td class="tdLabel">
                <%=GetLabel("Tanggal - Jam Pengerjaan") %>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-right: 1px; width: 145px">
                            <asp:TextBox ID="txtPerformDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td style="width: 5px">
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPerformTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsLaboratoryUnit" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table style="width: 100%" class="tblEntryDetail">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Pemeriksaan/Pelayanan")%></label>
                        </td>
                        <td colspan="2">
                            <qis:QISSearchTextBox ID="ledItem" ClientInstanceName="ledItem" runat="server" Width="500px"
                                ValueText="ItemID" FilterExpression="IsDeleted = 0" DisplayText="ItemName1" MethodName="GetvServiceUnitItemList">
                                <ClientSideEvents ValueChanged="function(s){ onLedItemLostFocus(s.GetValueText()); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Item Name" FieldName="ItemName1" Width="300px" />
                                    <qis:QISSearchTextBoxColumn Caption="Item Code" FieldName="ItemCode" Width="100px" />
                                    <qis:QISSearchTextBoxColumn Caption="Item Name 2" FieldName="ItemName2" Width="300px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Jumlah")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemQty" Width="100px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Diagnosa")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboDiagnose" ClientInstanceName="cboDiagnose"
                                Width="500px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Catatan Klinis")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="500px" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
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
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img class="imgEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                </td>
                                                <td style="width: 1px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                        <input type="hidden" value="<%#:Eval("ItemQty") %>" bindingfield="ItemQty" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                        <input type="hidden" value="<%#:Eval("GCToBePerformed") %>" bindingfield="GCToBePerformed" />
                                        <input type="hidden" value="<%#:Eval("PerformedDateInDatePickerFormat") %>" bindingfield="PerformedDate" />
                                        <input type="hidden" value="<%#:Eval("PerformedTime") %>" bindingfield="PerformedTime" />
                                        <input type="hidden" value="<%#:Eval("TestOrderDateInString") %>" bindingfield="TestOrderDate" />
                                        <input type="hidden" value="<%#:Eval("TestOrderTime") %>" bindingfield="TestOrderTime" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemName1" HeaderText="Item" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ItemQty" HeaderText="Qty" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="DiagnoseName" HeaderText="Diagnosis" HeaderStyle-Width="300px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
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
</asp:Content>
