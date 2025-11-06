<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.Master"
    AutoEventWireup="true" CodeBehind="TransactionPageCharges.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageCharges" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailServiceCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSReturnCtl.ascx"
    TagName="DrugMSReturnCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailLogisticCtl.ascx"
    TagName="LogisticCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailLogisticReturnCtl.ascx"
    TagName="LogisticReturnCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGoBillSummary" crudmode="R" runat="server" style="display: none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Transaction") %></div>
    </li>
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnVisitDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocationID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLogisticLocationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnReferrerParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnHSUImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHSULaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentID" runat="server" />
    <input type="hidden" value="" id="hdnDiagnoseText" runat="server" />
    <input type="hidden" value="" id="hdnIsLaboratoryUnit" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasAIOPackage" runat="server" />
    <input type="hidden" value="" id="hdnRadioteraphyUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function getTransactionHdID() {
            return $('#<%=hdnTransactionHdID.ClientID %>').val();
        }
        function getPhysicianID() {
            return $('#<%=hdnPhysicianID.ClientID %>').val();
        }
        function getIsHealthcareServiceUnitHasParamedic() {
            return ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.Value %>').val() == '1');
        }
        function onGetPhysicianFilterExpression() {
            var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0 AND IsAvailable = 1";
            return filterExpression;
        }
        function onGetTestPartnerFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND GCTestPartnerType != '" + Constant.TestPartnerType.LABORATORIUM + "'";

            if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnHSULaboratoryID.ClientID %>').val()) {
                filterExpression = "IsDeleted = 0 AND GCTestPartnerType = '" + Constant.TestPartnerType.LABORATORIUM + "'";
            }

            return filterExpression;
        }
        function getPhysicianCode() {
            return $('#<%=hdnPhysicianCode.ClientID %>').val();
        }
        function getPhysicianName() {
            return $('#<%=hdnPhysicianName.ClientID %>').val();
        }
        function getBusinessPartnerID() {
            return $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        }
        function getClassID() {
            return $('#<%=hdnClassID.ClientID %>').val();
        }
        function getRegistrationID() {
            return $('#<%=hdnRegistrationID.ClientID %>').val();
        }
        function getVisitID() {
            return $('#<%=hdnVisitID.ClientID %>').val();
        }
        function getHealthcareServiceUnitID() {
            return $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        }
        function getLocationID() {
            return $('#<%=hdnLocationID.ClientID %>').val();
        }
        function getLogisticLocationID() {
            return $('#<%=hdnLogisticLocationID.ClientID %>').val();
        }
        function getGCItemType() {
            return $('#<%=hdnGCItemType.ClientID %>').val();
        }
        function getDepartmentID() {
            return $('#<%=hdnDepartmentID.ClientID %>').val();
        }
        function getRemarksHd() {
            return $('#<%=txtRemarks.ClientID %>').val();
        }
        function getchkIsPATest() {
            if ($('#<%=chkIsPATest.ClientID %>').is(":checked")) {
                return "1";
            }
            else {
                return "0";            
            }
        }
        function getchkIsAIOTransaction() {
            return $('#<%=chkIsAIOTransaction.ClientID %>').is(":checked");
        }
        function getchkIsCopyMultiVisitOrder() {
            return $('#<%=chkIsCopyMultiVisitSchedule.ClientID %>').is(":checked");
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if (param != '') {                
                var transactionID = getTransactionHdID();
                if (transactionID == '' || transactionID == '0')
                    onAfterAddRecordAddRowCount();
                onLoadObject(param);
            }
        }

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

        function onBeforeProposeRecord(message) {
            var hdnTransactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var hdnIsShowItemNotificationWhenProposed = $('#<%=hdnIsShowItemNotificationWhenProposed.ClientID %>').val();
            var hdnVisitID = $('#<%:hdnVisitID.ClientID %>').val();
            var transactionDate = $('#<%:txtTransactionDate.ClientID %>').val();
            var transactionDateInDatePicker = Methods.getDatePickerDate(transactionDate);
            var transactionDateFormatString = Methods.dateToString(transactionDateInDatePicker);

            if (hdnIsShowItemNotificationWhenProposed == "1") {
                Methods.getPatientChargesValidationDoubleInputList(hdnVisitID, hdnTransactionID, transactionDateFormatString, function (result) {
                    if (result.check != '') {
                        message.text = "<b>Item berikut pada hari ini sudah diinput sebelumnya di nomor transaksi lain,</b>";
                        message.text += result.check + '<BR>';
                    }
                });
            }
        }

        function onAddRecordSetControlDisabled() {
            $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
            $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
        }

        function onAfterSaveRecordDtSuccess(transactionID) {
            var hdnTransactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            if (hdnTransactionID == '0' || hdnTransactionID == '') {
                $('#<%=hdnTransactionHdID.ClientID %>').val(transactionID);
                var filterExpression = 'TransactionID = ' + transactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
                });

                setServiceItemFilterExpression($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(), transactionID);
                setDrugMSItemFilterExpression(transactionID);
                setLogisticItemFilterExpression(transactionID);
                onAfterCustomSaveSuccess();
                setRightPanelButtonEnabled();
            }
        }

        function onAfterSaveRecordDtSuccessForSetRightPanelButtonEnabled() {
            var trnsNo = $('#<%=txtTransactionNo.ClientID %>').val();
            if (trnsNo != '') {
                var filterExpression = "TransactionNo = '" + trnsNo + "'";
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=hdnTransactionHdID.ClientID %>').val(result.TransactionID);
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
                });

                setServiceItemFilterExpression($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(), $('#<%=hdnTransactionHdID.ClientID %>').val());
                setDrugMSItemFilterExpression($('#<%=hdnTransactionHdID.ClientID %>').val());
                setLogisticItemFilterExpression($('#<%=hdnTransactionHdID.ClientID %>').val());
                onAfterCustomSaveSuccess();
                setRightPanelButtonEnabled();
            }
        }

        function fillServiceUnit() {
            var transno = $('#<%=txtTransactionNo.ClientID %>').val();
            if (transno == null || transno == "") {
                var filterExpression = 'RegistrationID = ' + $('#<%=hdnRegistrationID.ClientID %>').val();
                Methods.getObject('GetvConsultVisitList', filterExpression, function (result) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    //$('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                    $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                    $('#<%=hdnLogisticLocationID.ClientID %>').val(result.LogisticLocationID);
                    setServiceItemFilterExpression(result.HealthcareServiceUnitID);
                });
            }
        }

        $('#<%=btnGoBillSummary.ClientID %>').live('click', function () {
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryCharges.aspx");
            if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnHSULaboratoryID.ClientID %>').val())
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryLab.aspx");
            else if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnHSUImagingID.ClientID %>').val())
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryTestOrder.aspx?id=is");
            showLoadingPanel();
            window.location.href = url;
        });

        var lastContentID = '';
        function onLoad() {
            setCustomToolbarVisibility();
            setRightPanelButtonEnabled();

            $('#ulTabClinicTransaction li').click(function () {
                $('#ulTabClinicTransaction li.selected').removeAttr('class');
                $('.containerTransDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                if ($contentID == "containerDrugMS")
                    $('#<%=hdnGCItemType.ClientID %>').val(Constant.ItemGroupMaster.DRUGS);
                else if ($contentID == "containerLogistics")
                    $('#<%=hdnGCItemType.ClientID %>').val(Constant.ItemGroupMaster.LOGISTIC);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                lastContentID = $contentID;
            });

            if (lastContentID != '')
                $('#ulTabClinicTransaction li[contentid=' + lastContentID + ']').click();

            if ($('#<%=txtTransactionDate.ClientID %>').attr('readonly') == null)
                setDatePicker('<%=txtTransactionDate.ClientID %>');

            $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT")
                fillServiceUnit();

            //#region Transaction No
            function onGetTransactionNoFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblTransactionNo.lblLink').click(function () {
                openSearchDialog('patientchargeshd', onGetTransactionNoFilterExpression(), function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtTransactionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtTransactionNoChanged($(this).val());
            });

            function onTxtTransactionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Service Unit
            function getServiceUnitFilterFilterExpression() {
                var filterExpression = "<%:GetServiceUnitFilterFilterExpression() %>";
                //var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = 'DIAGNOSTIC' AND HealthcareServiceUnitID NOT IN (" + $('#<%=hdnHSULaboratoryID.ClientID %>').val() + "," + $('#<%=hdnHSUImagingID.ClientID %>').val() + ") AND IsDeleted = 0";
                return filterExpression;
            }
            $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=hdnLogisticLocationID.ClientID %>').val(result.LogisticLocationID);
                        setServiceItemFilterExpression(result.HealthcareServiceUnitID);
                        cboDrugMSLocation.PerformCallback();
                        cboLogisticLocation.PerformCallback();
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            onLoadService();
            onLoadDrugMS();
            onLoadDrugMSReturn();
            onLoadLogistic();
            onLoadLogisticReturn();
            calculateAllTotal();
        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                //                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/Charges/ChargesVoidCtl.ascx');
                //                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                //                var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
                //                var id = registrationID + '|' + transactionID;
                //                openUserControlPopup(url, id, 'Void Transaction', 400, 230);
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        function calculateAllTotal() {
            var serviceTotalPatient = getServiceTotalPatient();
            var serviceTotalPayer = getServiceTotalPayer();

            var drugMSTotalPatient = getDrugMSTotalPatient();
            var drugMSTotalPayer = getDrugMSTotalPayer();

            var drugMSReturnTotalPatient = getDrugMSReturnTotalPatient();
            var drugMSReturnTotalPayer = getDrugMSReturnTotalPayer();

            var logisticTotalPatient = getLogisticTotalPatient();
            var logisticTotalPayer = getLogisticTotalPayer();

            var logisticReturnTotalPatient = getLogisticReturnTotalPatient();
            var logisticReturnTotalPayer = getLogisticReturnTotalPayer();

            var totalPayer = (serviceTotalPayer + drugMSTotalPayer + drugMSReturnTotalPayer + logisticTotalPayer + logisticReturnTotalPayer);
            var totalPatient = (serviceTotalPatient + drugMSTotalPatient + drugMSReturnTotalPatient + logisticTotalPatient + logisticReturnTotalPatient);

            $('#<%=txtTotalPayer.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=txtTotalPatient.ClientID %>').val(totalPatient).trigger('changeValue');
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function setRightPanelButtonEnabled() {
            var transactionHdID = parseInt($('#<%=hdnTransactionHdID.ClientID %>').val());
            var isParturition = $('#<%:hdnIsParturition.ClientID %>').val();

            if (transactionHdID != null && transactionHdID != "0" && transactionHdID != "") {
                $('#btnModality').removeAttr('enabled');

                var filterExpression = "TransactionID = '" + transactionHdID + "'";
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    if (result != null) {
                        if (result.TestOrderID != null && result.TestOrderID != "0" && result.TestOrderID != "") {
                            var filterExpressionOrder = "TestOrderID = '" + result.TestOrderID + "'";
                            Methods.getObject('GetTestOrderHdList', filterExpressionOrder, function (resultOrder) {
                                if (resultOrder != null) {
                                    if (resultOrder.IsPathologicalAnatomyTest) {
                                        $('#btnPathologyAnatomyInfo').removeAttr('enabled');
                                    }
                                    else {
                                        $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
                                    }
                                }
                                else {
                                    $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
                                }
                            });
                        }
                        else {
                            var filterExpressionHdInfo = "TransactionID = '" + transactionHdID + "'";
                            Methods.getObject('GetPatientChargesHdInfoList', filterExpressionHdInfo, function (resultInfo) {
                                if (resultInfo != null) {
                                    if (resultInfo.IsPathologicalAnatomyTest == '1') {
                                        $('#btnPathologyAnatomyInfo').removeAttr('enabled');
                                    }
                                    else {
                                        $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
                                    }
                                }
                                else {
                                    $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
                                }
                            });
                        }
                    }
                    else {
                        $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
                    }
                });
            }
            else {
                $('#btnModality').attr('enabled', 'false');
                $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
            }

            if (isParturition != 'True') {
                $('#btnPatientBirthEntry').attr('enabled', 'false');
            } else {
                $('#btnPatientBirthEntry').removeAttr('enabled');
            }

            if (transactionHdID != null && transactionHdID != "0" && transactionHdID != "") {
                if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == 'X121^001') {
                    $('#btnChangeTransactionPhysician').removeAttr('enabled');
                }
                else {
                    $('#btnChangeTransactionPhysician').attr('enabled', 'false');
                }
                $('#btnInputSerialNo').removeAttr('enabled');
                $('#btnInputFixedAsset').removeAttr('enabled');
            } else {
                $('#btnInputSerialNo').attr('enabled', 'false');
                $('#btnInputFixedAsset').attr('enabled', 'false');
                $('#btnChangeTransactionPhysician').attr('enabled', 'false');
            }
        }

        $('#<%=chkIsAIOTransaction.ClientID %>').live('click', function (evt) {
            if ($('#<%=chkIsAIOTransaction.ClientID %>').is(':checked')) {
                $('#<%=hdnIsCheckedAIOTransaction.ClientID %>').val("1");
            } else {
                $('#<%=hdnIsCheckedAIOTransaction.ClientID %>').val("0");
            }

            if (typeof onItemServiceAIOClicked == 'function') {
                onItemServiceAIOClicked(getchkIsAIOTransaction());
            }

            if (typeof onDrugAIOClicked == 'function') {
                onDrugAIOClicked(getchkIsAIOTransaction());
            }

            if (typeof onDrugReturnAIOClicked == 'function') {
                onDrugReturnAIOClicked(getchkIsAIOTransaction());
            }

            if (typeof onLogisticAIOClicked == 'function') {
                onLogisticAIOClicked(getchkIsAIOTransaction());
            }

            if (typeof onLogisticReturnAIOClicked == 'function') {
                onLogisticReturnAIOClicked(getchkIsAIOTransaction());
            }
        });

        $('#<%=chkIsCopyMultiVisitSchedule.ClientID %>').live('click', function (evt) {
            if ($('#<%=chkIsCopyMultiVisitSchedule.ClientID %>').is(':checked')) {
                
            } else {

            }

            if (typeof onChkIsCopyMultiVisitSchedule == 'function') {
                onChkIsCopyMultiVisitSchedule($('#<%=chkIsCopyMultiVisitSchedule.ClientID %>').is(':checked'));
            }
        });

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'sendOrderToRIS' || code == 'sendOrderToLIS' || code == 'sendOrderToOIS' || code == 'changeParamedic' || code == 'changeDetailRemarks' || code == 'inputSerialNumber' || code == 'inputFixedAsset' || code == "printLabelCover" || code == "printLabelCoverEditable" || code == "testPartner") {
                return $('#<%:hdnTransactionHdID.ClientID %>').val();
            }
            else if (code == 'transactionNotes' || code == 'patientBirth') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'draftAppointmentTestOrder') {
                return $('#<%:hdnAppointmentID.ClientID %>').val() + "|DIAGNOSTIC";
            }
            else if (code == 'keteranganKematian') {
                var filterExpression = "VisitID = " + $('#<%:hdnVisitID.ClientID %>').val() + " AND GCDiagnoseType = '" + Constant.DiagnosisType.MAIN_DIAGNOSIS + "' AND IsDeleted = 0";
                Methods.getObject('GetPatientDiagnosisList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDiagnoseText.ClientID %>').val(result.DiagnosisText);
                    }
                    else {
                        $('#<%=hdnDiagnoseText.ClientID %>').val('');
                    }
                });
                return $('#<%:hdnVisitID.ClientID %>').val() + "|" + $('#<%:hdnDiagnoseText.ClientID %>').val();
            }
            else if (code == 'dataPemeriksaanPasien') {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VitalSign/VitalSign.aspx?id=DIAGNOSTIC');
                window.location.href = url;
            }
            else if (code == 'modality') {
                return $('#<%:hdnTransactionHdID.ClientID %>').val() + "|" + $('#<%:hdnRegistrationID.ClientID %>').val() + "|" + $('#<%=txtTransactionNo.ClientID %>').val() + "|" + $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            }
            else if (code == 'pathologyAnatomyInfoEntry') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = "0";
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                var transactionID = $('#<%:hdnTransactionHdID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + "" + "|" + serviceunitID + "|" + transactionID;
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();
            var isParturition = $('#<%:hdnIsParturition.ClientID %>').val();
            var MRN = $('#<%=hdnMRN.ClientID %>').val();
            if (registrationID == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            } else if (code == 'PM-00614') {
                if (isParturition == 'True') {
                    filterExpression.text = visitID;
                    return true;
                } else {
                    errMessage.text = 'Bukan Pasien Partus';
                    return false;
                }
            }
            else if (code == 'PM-00136') {
                if (registrationID == '' || registrationID == '0') {
                    errMessage.text = 'Mohon refresh dulu.';
                    return false;
                } else {
                    filterExpression.text = registrationID;
                    return true;
                }
            }
            else if (code == 'PM-00137' || code == 'PM-00357') {
                if (MRN == '' || MRN == '0') {
                    errMessage.text = 'Mohon refresh dulu.';
                    return false;
                } else {
                    filterExpression.text = MRN;
                    return true;
                }
            }
            else if (code == 'PM-00509') {
                filterExpression.text = transactionID;
                return true;
            }
            else if (code == 'PM-00427' || code == 'PM-00432') {
                filterExpression.text = transactionID;
                return true;
            }
            else {
                if (GCTransactionStatus != Constant.TransactionStatus.OPEN && GCTransactionStatus != Constant.TransactionStatus.VOID) {
                    if (code == 'PM-00119' || code == 'PM-00120' || code == 'PM-00121' || code == 'PM-00122' || code == 'PM-00123' || code == 'OP-00101' || code == 'OP-00102') {
                        filterExpression.text = 'RegistrationID = ' + registrationID;
                        return true;
                    }
                    else {
                        if (transactionID == '' || transactionID == '0') {
                            errMessage.text = 'Simpan Transaksi Terlebih Dahulu!';
                            return false;
                        }
                        else {
                            filterExpression.text = transactionID;
                            return true;
                        }
                    }
                }
                else if (code == 'PM-00138' || code == "PM-00189" || code == 'PM-00358' || code == 'PM-00363'
                        || code == 'PM-00364' || code == 'PM-00393' || code == 'PM-00589') {
                    filterExpression.text = transactionID;
                    return true;
                }
                else {
                    errMessage.text = 'Please Process / Proposed Transaction First!';
                    return false;
                }
            }
        }
    </script>
    <input type="hidden" value="" id="hdnIsParturition" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutomaticallySendToRIS" runat="server" />
    <input type="hidden" value="0" id="hdnIsShowItemNotificationWhenProposed" runat="server" />
    <input type="hidden" value="" id="hdnIsCheckedAIOTransaction" runat="server" />
    <input type="hidden" value="" id="hdnIsServiceUnitMultiVisitSchedule" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 65%" />
                            <col style="width: 35%" />
                        </colgroup>
                        <tr>
                            <td style="padding: 5px; vertical-align: top">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink" id="lblTransactionNo">
                                                <%=GetLabel("No Transaksi")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tanggal") %>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-right: 1px; width: 145px">
                                                        <asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
                                                    </td>
                                                    <td style="width: 5px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 50px">
                                                        <%=GetLabel("Jam") %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server"
                                                            Style="text-align: center" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="padding: 5px; vertical-align: top">
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr id="trIsAutoTransaction" runat="server" style="display: none">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsAutoTransaction" runat="server" /><%:GetLabel(" Transaksi Otomatis")%>
                                        </td>
                                    </tr>
                                    <tr id="trIsAIOTransaction" runat="server">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsAIOTransaction" runat="server" /><%:GetLabel(" Transaksi AIO")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none" id="trServiceUnit" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                    <%=GetServiceUnitLabel()%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                                <input type="hidden" value="" id="hdnLocationID" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticLocationID" runat="server" />
                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none" id="trPATest" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsPATest" Width="150px" runat="server" Text=" Pemeriksaan PA" />
                            </td>
                        </tr>
                        <tr style="display: none" id="trIsCopyMultiVisitSchedule" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCopyMultiVisitSchedule" Width="200px" runat="server" Text=" Copy dari Multi Kunjungan" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabClinicTransaction">
                            <li class="selected" contentid="containerService">
                                <%=GetLabel("PELAYANAN") %></li>
                            <li contentid="containerDrugMS">
                                <%=GetLabel("OBAT & ALKES") %></li>
                            <li contentid="containerDrugMSReturn">
                                <%=GetLabel("RETUR OBAT & ALKES") %></li>
                            <li contentid="containerLogistics">
                                <%=GetLabel("BARANG UMUM") %></li>
                            <li contentid="containerLogisticReturn">
                                <%=GetLabel("RETUR BARANG UMUM") %></li>
                        </ul>
                    </div>
                    <div id="containerService" class="containerTransDt">
                        <uc1:ServiceCtl ID="ctlService" runat="server" />
                    </div>
                    <div id="containerDrugMS" style="display: none" class="containerTransDt">
                        <uc1:DrugMSCtl ID="ctlDrugMS" runat="server" />
                    </div>
                    <div id="containerDrugMSReturn" style="display: none" class="containerTransDt">
                        <uc1:DrugMSReturnCtl ID="ctlDrugMSReturn" runat="server" />
                    </div>
                    <div id="containerLogistics" style="display: none" class="containerTransDt">
                        <uc1:LogisticCtl ID="ctlLogistic" runat="server" />
                    </div>
                    <div id="containerLogisticReturn" style="display: none" class="containerTransDt">
                        <uc1:LogisticReturnCtl ID="ctlLogisticReturn" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 100%" cellpadding="0" cellspacing="0">
            <colgroup>
                <col style="width: 15%" />
                <col style="width: 35%" />
                <col style="width: 15%" />
                <col style="width: 35%" />
            </colgroup>
            <tr>
                <td>
                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                        <%=GetLabel("TOTAL INSTANSI") %>
                        :
                    </div>
                </td>
                <td style="text-align: right; padding-right: 10px;">
                    Rp.
                    <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="txtCurrency" runat="server"
                        Width="200px" />
                </td>
                <td>
                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                        <%=GetLabel("TOTAL PASIEN") %>
                        :
                    </div>
                </td>
                <td style="text-align: right; padding-right: 10px;">
                    Rp.
                    <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="txtCurrency" runat="server"
                        Width="200px" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <tr>
                <td colspan="2">
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 600px;">
                                        <div class="pageTitle" style="text-align: center">
                                            <%=GetLabel("Informasi")%></div>
                                        <div style="background-color: #EAEAEA;">
                                            <table width="600px" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="30px" />
                                                </colgroup>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trProposedBy" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Dipropose Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divProposedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trProposedDate" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Dipropose Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divProposedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidBy" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Divoid Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidDate" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Divoid Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidReason" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Alasan Batal")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidReason">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
