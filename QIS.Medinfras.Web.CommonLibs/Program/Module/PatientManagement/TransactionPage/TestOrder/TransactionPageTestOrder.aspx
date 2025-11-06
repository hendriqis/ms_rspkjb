<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TransactionPageTestOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageTestOrder" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnOperatingRoomID" runat="server" />
    <input type="hidden" value="" id="hdnRadiotheraphyServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnBloodBankHSUID" runat="server" />
    <input type="hidden" value="" id="hdnEM0079" runat="server" />
    <input type="hidden" value="" id="hdnIPAddress" runat="server" />
    <input type="hidden" value="6000" id="hdnPort" runat="server" />
    <input type="hidden" value="" id="hdnIsBPJSRegistration" runat="server" />
    <input type="hidden" value="" id="hdnIsOnlyBPJSItem" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = $('#<%=hdnTestOrderID.ClientID %>').val();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(testOrderID) {
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnTestOrderID.ClientID %>').val(testOrderID);
                var filterExpression = 'TestOrderID = ' + testOrderID;
                Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                    $('#<%=txtTestOderNo.ClientID %>').val(result.TestOrderNo).trigger('change');
                });

                onAfterCustomSaveSuccess();
            }
        }

        function onLoad() {
            setCustomToolbarVisibility();
            setRightPanelButtonEnabled();

            if ($('#<%=txtTestOrderDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtTestOrderDate.ClientID %>');
                $('#<%=txtTestOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

                $('#<%=txtScheduledDate.ClientID %>').attr('readonly', 'readonly');
                //////$('#<%=txtScheduledDate.ClientID %>').datepicker('disable');
                $('#<%=txtScheduledTime.ClientID %>').attr('readonly', 'readonly');

                if (cboToBePerformed.GetValue() == "X125^002") {
                    setDatePicker('<%=txtScheduledDate.ClientID %>');
                    $('#<%=txtScheduledDate.ClientID %>').datepicker('option', 'minDate', getDateNow());
                }
            }

            $('#lblTestOrderQuickPick').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                    var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                    var isLabUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
                    var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                    var radiotheraphyServiceUnitID = $('#<%=hdnRadiotheraphyServiceUnitID.ClientID %>').val();

                    var isUsingForm = $('#<%=hdnIsQuickPicksUsingForm.ClientID %>').val();
                    var isDisplayPrice = $('#<%=hdnIsDisplayPrice.ClientID %>').val();

                    var url = '';
                    var width = 0;

                    if (serviceUnitID == radiologyServiceUnitID || serviceUnitID == labServiceUnitID || isLabUnit == '1') {
                        if (isUsingForm == "1") {
                            url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TestOrderLabQuickPicksCtl.ascx');
                            width = 1150;
                        }
                        else {
                            if (isDisplayPrice == "1") {
                                url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TestOrderQuickPicksCtl.ascx');
                                width = 1100;
                            }
                            else {
                                url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TestOrderLabQuickPicksCtl.ascx');
                                width = 1150;
                            }
                        }
                    }
                    else {
                        url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TestOrderQuickPicksCtl.ascx');
                        width = 1100;
                    }
                    var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                    var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                    var testOrderDate = $('#<%=txtTestOrderDate.ClientID %>').val();
                    var testOrderTime = $('#<%=txtTestOrderTime.ClientID %>').val();
                    var gcToBePerformed = cboToBePerformed.GetValue();
                    var realizationDate = $('#<%=txtScheduledDate.ClientID %>').val();
                    var realizationTime = $('#<%=txtScheduledTime.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var svcUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                    var notes = $('#<%=txtRemarksHd.ClientID %>').val();
                    var chkChecked = $('#<%=chkIsCITO.ClientID %>').is(":checked");
                    var paramedicName = $('#<%=txtPhysicianName.ClientID %>').val();
                    var chkPATest = $('#<%=chkIsPATest.ClientID %>').is(":checked");
                    var chkIsMultiVisitScheduleOrder = $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').is(":checked");
                    $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician2.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');

                    var id = testOrderID + '|' + paramedicID + '|' + testOrderDate + '|' + testOrderTime + '|' + serviceUnitID + '|' + visitID + '|' + svcUnitID + '|' + realizationDate + '|' + realizationTime + '|' + gcToBePerformed + '|' + notes + '|' + chkChecked + '|' + registrationID + '|' + isLabUnit + '|' + paramedicName + '|' + chkPATest + '|' + chkIsMultiVisitScheduleOrder;
                    openUserControlPopup(url, id, 'Quick Picks', width, 550);
                }
            });

            //#region Test Order Date Change
            $('#<%=txtScheduledDate.ClientID %>').keyup(function () {
                var dateorder = $(this).val();
                $this.val(dateorder);
            });

            $('#<%=txtScheduledTime.ClientID %>').keyup(function () {
                var dateorder = $(this).val();
                $this.val(dateorder);
            });

            $('#<%=txtTestOrderDate.ClientID %>').change(function () {
                onDateorderTxtDateChanged($(this).val());
            });

            $('#<%=txtTestOrderTime.ClientID %>').change(function () {
                onDateorderTxtDateChanged($(this).val());
            });

            function onDateorderTxtDateChanged(value) {
                var date = $('#<%=txtTestOrderDate.ClientID %>').val();
                var time = $('#<%=txtTestOrderTime.ClientID %>').val();
                var dateorder = "";

                if (date != "" && date != null) {
                    if (time != "" && time != null) {
                        dateorder = date;

                    } else {
                        dateorder = "";
                    }
                } else {
                    dateorder = "";
                }

                $('#<%=txtScheduledDate.ClientID %>').val(dateorder);

                if (date != "" && date != null) {
                    if (time != "" && time != null) {
                        dateorder = time;

                    } else {
                        dateorder = "";
                    }
                } else {
                    dateorder = "";
                }
                $('#<%=txtScheduledTime.ClientID %>').val(dateorder);
            }
            //#endregion

            $('#lblTestTemplate').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                    var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                    var isLabUnit = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
                    var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();

                    if (serviceUnitID == radiologyServiceUnitID || isLabUnit == "1") {
                        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
                        var testOrderDate = $('#<%=txtTestOrderDate.ClientID %>').val();
                        var testOrderTime = $('#<%=txtTestOrderTime.ClientID %>').val();
                        var gcToBePerformed = cboToBePerformed.GetValue();
                        var realizationDate = $('#<%=txtScheduledDate.ClientID %>').val();
                        var realizationTime = $('#<%=txtScheduledTime.ClientID %>').val();
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var svcUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
                        var notes = $('#<%=txtRemarksHd.ClientID %>').val();
                        var chkChecked = $('#<%=chkIsCITO.ClientID %>').is(":checked");
                        var paramedicName = $('#<%=txtPhysicianName.ClientID %>').val();
                        $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%=lblPhysician2.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');

                        url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/TestOrderTemplateOrderCtl.ascx');

                        var id = testOrderID + '|' + paramedicID + '|' + testOrderDate + '|' + testOrderTime + '|' + serviceUnitID + '|' + visitID + '|' + svcUnitID + '|' + realizationDate + '|' + realizationTime + '|' + gcToBePerformed + '|' + notes + '|' + chkChecked + '|' + registrationID + '|' + isLabUnit + '|' + paramedicName;
                        openUserControlPopup(url, id, 'Panel Order', 1000, 500);
                    }
                    else {
                        showToast('Information', 'Panel Order hanya dapat digunakan untuk order penunjang Laboratorium dan Radiologi.');
                    }
                }
            });


            //#region Transaction No
            $('#lblTestOrderNo.lblLink').click(function () {
                var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val()
                                        + "  AND HealthcareServiceUnitID NOT IN (" + $('#<%=hdnOperatingRoomID.ClientID %>').val() + "," + $('#<%=hdnBloodBankHSUID.ClientID %>').val() + ")";
                openSearchDialog('testorderhd', filterExpression, function (value) {
                    $('#<%=txtTestOderNo.ClientID %>').val(value);
                    onTxtTestOrderNoChanged(value);
                });
            });

            $('#<%=txtTestOderNo.ClientID %>').change(function () {
                onTxtTestOrderNoChanged($(this).val());
            });

            function onTxtTestOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Physician
            function GetPhysicianFilterExpression() {
                var filterExpression = "IsDeleted = 0";

                if (cboToBePerformed.GetValue() == "X125^002") {
                    var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

                    if (hsuID != '') {
                        filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID IN (" + hsuID + "))";
                    }
                }

                return filterExpression;
            }

            $('#<%=lblPhysician.ClientID %>.lblLink').click(function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('paramedic', GetPhysicianFilterExpression(), function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPhysicianCodeChanged(value);
                });
            });

            $('#<%=lblPhysician2.ClientID %>.lblLink').click(function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('paramedic', GetPhysicianFilterExpression(), function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPhysicianCodeChanged(value);
                });
            });

            $('#<%=txtPhysicianCode.ClientID %>').change(function () {
                onTxtPhysicianCodeChanged($(this).val());
            });

            function onTxtPhysicianCodeChanged(value) {
                if (value != "") {
                    var filterExpression = GetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                            $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);

                            if (cboToBePerformed.GetValue() == "X125^002") {
                                var filterCheck = "ParamedicID = " + result.ParamedicID;
                                if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() != '') {
                                    filterCheck += " AND HealthcareServiceUnitID = " + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                                }
                                Methods.getObject('GetvServiceUnitParamedicList', filterCheck, function (resultCheck) {
                                    if (resultCheck == null) {
                                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                                    }
                                });
                            }

                            var isCopyDiagnose = $('#<%=hdnIsNotesCopyDiagnose.ClientID %>').val();
                            if (isCopyDiagnose == "1") {
                                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                                var filterDiag = "VisitID = '" + visitID + "' AND IsDeleted = 0 AND ID = (SELECT MAX(ID) FROM PatientDiagnosis WHERE ParamedicID = " + result.ParamedicID + " AND VisitID = " + visitID + " AND IsDeleted = 0)";
                                Methods.getObject('GetvPatientDiagnosis1List', filterDiag, function (resultdiag) {
                                    if (resultdiag != null) {
                                        var oRemarks = "";
                                        if (resultdiag.DiagnoseID != "") {
                                            oRemarks = resultdiag.DiagnoseName + " (" + resultdiag.DiagnoseID + ")";
                                        } else {
                                            oRemarks = resultdiag.DiagnoseText;
                                        }
                                        $('#<%=txtRemarksHd.ClientID %>').val(oRemarks);
                                    }
                                    else {
                                        var filterDiag2 = "VisitID = '" + visitID + "' AND IsDeleted = 0 AND ID = (SELECT MAX(ID) FROM PatientDiagnosis" + " WHERE VisitID = " + visitID + " AND IsDeleted = 0)";
                                        Methods.getObject('GetvPatientDiagnosis1List', filterDiag2, function (resultdiag) {
                                            if (resultdiag != null) {
                                                var oRemarks = "";
                                                if (resultdiag.DiagnoseID != "") {
                                                    oRemarks = resultdiag.DiagnoseName + " (" + resultdiag.DiagnoseID + ")";
                                                } else {
                                                    oRemarks = resultdiag.DiagnoseText;
                                                }
                                                $('#<%=txtRemarksHd.ClientID %>').val(oRemarks);
                                            }
                                        });
                                    }
                                });
                            }
                            else {
                                $('#<%=txtRemarksHd.ClientID %>').val('');
                            }
                        }
                        else {
                            $('#<%=hdnPhysicianID.ClientID %>').val('');
                            $('#<%=txtPhysicianCode.ClientID %>').val('');
                            $('#<%=txtPhysicianName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            }
            //#endregion

            $('#<%=txtRemarksHd.ClientID %>').die('change');
            $('#<%=txtRemarksHd.ClientID %>').live('change', function () {
                $('#<%=hdnRemarks.ClientID %>').val($(this).val());
            });

            //#region Service Unit
            function getServiceUnitFilterFilterExpression() {
                //HealthcareID = '001' AND DepartmentID = 'DIAGNOSTIC' AND HealthcareServiceUnitID != 100 AND IsDeleted = 0 AND IsUsingRegistration = 1
                var filterExpression = "<%:GetServiceUnitFilterFilterExpression() %>";

                if (cboToBePerformed.GetValue() == "X125^002") {
                    var physicianID = $('#<%=hdnPhysicianID.ClientID %>').val();

                    if (physicianID != '') {
                        filterExpression += " AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID IN (" + physicianID + "))";
                    }
                }

                return filterExpression;
            }
            $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
                if ($('#<%=txtTestOderNo.ClientID %>').val() != '')
                    return;
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
                Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

                        $('#<%=chkIsPATest.ClientID %>').prop("checked", false);
                        if (result.IsLaboratoryUnit) {
                            $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("1");
                            $('#<%:trLaboratoryOrder.ClientID %>').removeAttr('style');
                        } else {
                            $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("0");
                            $('#<%:trLaboratoryOrder.ClientID %>').attr('style', 'display:none');
                        }

                        if ($('#<%=hdnOperatingRoomID.ClientID %>').val() == result.HealthcareServiceUnitID) {
                            $('#<%:trProcedureGroup.ClientID %>').removeAttr('style');
                        } else {
                            $('#<%:trProcedureGroup.ClientID %>').attr('style', 'display:none');
                        }

                        if (cboToBePerformed.GetValue() == "X125^002") {
                            var filterCheck = "HealthcareServiceUnitID = " + result.HealthcareServiceUnitID;
                            if ($('#<%=hdnPhysicianID.ClientID %>').val() != '') {
                                filterCheck += " AND ParamedicID = " + $('#<%=hdnPhysicianID.ClientID %>').val();
                            }
                            Methods.getObject('GetvServiceUnitParamedicList', filterCheck, function (resultCheck) {
                                if (resultCheck == null) {
                                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                                    $('#<%=txtPhysicianName.ClientID %>').val('');
                                }
                            });
                        }

                        if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() == $('#<%=hdnImagingServiceUnitID.ClientID %>').val() && $('#<%=hdnEM0079.ClientID %>').val() == "0") {
                            $('#<%=txtRemarksHd.ClientID %>').val('');
                        }
                        else {
                            $('#<%=txtRemarksHd.ClientID %>').val($('#<%=hdnRemarks.ClientID %>').val());
                        }

                        if ($('#<%=hdnIsUsingMultiVisitScheduleOrder.ClientID %>').val() == "1") {
                            if (result.IsAllowMultiVisitSchedule) {
                                $('#<%:trMultiVisitScheduleOrder.ClientID %>').removeAttr('style');
                            }
                            else {
                                $('#<%:trMultiVisitScheduleOrder.ClientID %>').attr('style', 'display:none');
                                $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                            }
                        }
                        else {
                            $('#<%:trMultiVisitScheduleOrder.ClientID %>').attr('style', 'display:none');
                            $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                        }

                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                        $('#<%:trMultiVisitScheduleOrder.ClientID %>').attr('style', 'display:none');
                        $('#<%=chkIsMultiVisitScheduleOrder.ClientID %>').prop("checked", false);
                    }
                });
            }
            //#endregion

            //#region Procedure Group
            $('#<%=lblProcedureGroup.ClientID %>.lblLink').click(function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('proceduregroup', filterExpression, function (value) {
                    $('#<%=txtProcedureGroupCode.ClientID %>').val(value);
                    onTxtProcedureGroupCodeChanged(value);
                });
            });

            $('#<%=txtProcedureGroupCode.ClientID %>').change(function () {
                onTxtProcedureGroupCodeChanged($(this).val());
            });

            function onTxtProcedureGroupCodeChanged(value) {
                var filterExpression = "ProcedureGroupCode = '" + value + "'";
                Methods.getObject('GetProcedureGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProcedureGroupID.ClientID %>').val(result.ProcedureGroupID);
                        $('#<%=txtProcedureGroupName.ClientID %>').val(result.ProcedureGroupName);

                        $('#lblAddData').hide();
                        $('#lblTestOrderQuickPick').hide();
                        $('#lblTestTemplate').hide();
                    }
                    else {
                        $('#<%=hdnProcedureGroupID.ClientID %>').val('');
                        $('#<%=txtProcedureGroupCode.ClientID %>').val('');
                        $('#<%=txtProcedureGroupName.ClientID %>').val('');

                        $('#lblAddData').show();
                        $('#lblTestOrderQuickPick').show();
                        $('#lblTestTemplate').show();
                    }
                });
            }
            //#endregion

            //#region Item
            function getItemMasterFilterExpression() {
                var isOnlyTestItem = $('#<%=hdnOrderHanyaItemPemeriksaan.ClientID %>').val();
                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var filterExpression = '';

                if (isOnlyTestItem == "1") {
                    if (testOrderID != '' && testOrderID != '0') {
                        filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ' AND IsTestItem = 1) AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnTestOrderID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
                    }
                    else {
                        filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ' AND IsTestItem = 1) AND IsDeleted = 0';
                    }
                } else {
                    if (testOrderID != '' && testOrderID != '0') {
                        filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = ' + $('#<%=hdnTestOrderID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
                    }
                    else {
                        filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND IsDeleted = 0';
                    }
                }
                filterExpression += " AND GCItemStatus != 'X181^999'";

                return filterExpression;
            }

            $('#lblItem.lblLink').click(function () {
                openSearchDialog('item', getItemMasterFilterExpression(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = getItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);

                        if (result.DefaultParamedicID != null && result.DefaultParamedicID != 0) {
                            $('#<%=hdnParamedicDetailID.ClientID %>').val(result.DefaultParamedicID);
                            $('#<%=txtParamedicDetailCode.ClientID %>').val(result.DefaultParamedicCode);
                            $('#<%=txtParamedicDetailName.ClientID %>').val(result.DefaultParamedicName);
                        }
                        else {
                            $('#<%=hdnParamedicDetailID.ClientID %>').val('');
                            $('#<%=txtParamedicDetailCode.ClientID %>').val('');
                            $('#<%=txtParamedicDetailName.ClientID %>').val('');
                        }

                        var filterExpressionItemService = "ItemID = " + result.ItemID;
                        Methods.getObject('GetItemServiceList', filterExpressionItemService, function (resultItemService) {
                            if (resultItemService != null) {
                                if (resultItemService.IsAllowCito == false) {
                                    $('#<%=chkIsCITO_Detail.ClientID %>').attr('disabled', true);
                                }
                            }
                        });
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        $('#<%=hdnParamedicDetailID.ClientID %>').val('');
                        $('#<%=txtParamedicDetailCode.ClientID %>').val('');
                        $('#<%=txtParamedicDetailName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Diagnose
            $('#lblDiagnose.lblLink').live('click', function () {
                openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                    $('#<%=txtDiagnoseID.ClientID %>').val(value);
                    onTxtDiagnoseIDChanged(value);
                });
            });

            $('#<%=txtDiagnoseID.ClientID %>').live('change', function () {
                onTxtDiagnoseIDChanged($(this).val());
            });

            function onTxtDiagnoseIDChanged(value) {
                var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    }
                    else {
                        $('#<%=txtDiagnoseID.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region ParamedicDetail
            $('#lblParamedicDetail.lblLink').live('click', function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtParamedicDetailCode.ClientID %>').val(value);
                    onTxtParamedicDetailCodeChanged(value);
                });
            });

            $('#<%=txtParamedicDetailCode.ClientID %>').change(function () {
                onTxtParamedicDetailCodeChanged($(this).val());
            });

            function onTxtParamedicDetailCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicDetailID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtParamedicDetailName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnParamedicDetailID.ClientID %>').val('');
                        $('#<%=txtParamedicDetailCode.ClientID %>').val('');
                        $('#<%=txtParamedicDetailName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=lblServiceUnit.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician2.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtDiagnoseID.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                    $('#<%=txtRemarksDt.ClientID %>').val('');
                    $('#<%=txtItemQty.ClientID %>').val('1');
                    if ($('#<%=chkIsCITO.ClientID %>').is(":checked")) {
                        $('#<%=hdnIsCITOHd.ClientID %>').val("1");
                        $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", true);
                    }
                    else {
                        $('#<%=hdnIsCITOHd.ClientID %>').val("0");
                        $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", false);
                    }
                    $('#containerEntry').show();
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            setDatePicker('<%=txtPerformDate.ClientID %>');
        }

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtDiagnoseID.ClientID %>').val(entity.DiagnoseID);
            $('#<%=txtDiagnoseName.ClientID %>').val(entity.DiagnoseName);
            $('#<%=txtParamedicDetailCode.ClientID %>').val(entity.DetailOrderParamedicCode);
            $('#<%=txtParamedicDetailName.ClientID %>').val(entity.DetailOrderParamedicName);
            $('#<%=txtRemarksDt.ClientID %>').val(entity.Remarks);
            $('#<%=txtItemQty.ClientID %>').val(entity.ItemQty);

            if (entity.IsCITO == 'False') {
                $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", false);
            }
            else $('#<%=chkIsCITO_Detail.ClientID %>').prop("checked", true);

            $('#containerEntry').show();
        });

        function onCboToBePerformedChanged() {
            if (cboToBePerformed.GetValue() != null && (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED || cboToBePerformed.GetValue() == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)) {
                if (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED || cboToBePerformed.GetValue() == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT) {
                    $('#<%=txtScheduledDate.ClientID %>').removeAttr('readonly');
                    $('#<%=txtScheduledDate.ClientID %>').datepicker('enable');
                    setDatePicker('<%=txtScheduledDate.ClientID %>');
                    $('#<%=txtScheduledDate.ClientID %>').datepicker('option', 'minDate', '0');
                    $('#<%=txtScheduledTime.ClientID %>').removeAttr('readonly');

                    if (cboToBePerformed.GetValue() == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT) {
                        $('#<%:tdScheduledTime.ClientID %>').attr('style', 'display:none');
                        $('#<%=lblPhysician.ClientID %>').attr('style', 'display:none');
                        $('#<%:lblPhysician2.ClientID %>').removeAttr('style');
                    }
                    else {
                        $('#<%:tdScheduledTime.ClientID %>').removeAttr('style');
                        $('#<%:lblPhysician.ClientID %>').removeAttr('style');
                        $('#<%=lblPhysician2.ClientID %>').attr('style', 'display:none');
                    }
                }

                $('#<%=chkIsCITO.ClientID %>').attr('disabled', true);
                $('#<%=chkIsCITO.ClientID %>').prop("checked", false);
                $('#<%=hdnIsCITOHd.ClientID %>').val("0");
            }
            else {
                $('#<%=txtScheduledDate.ClientID %>').val($('#<%=hdnDatePickerToday.ClientID %>').val());
                $('#<%=txtScheduledTime.ClientID %>').val($('#<%=hdnTimeToday.ClientID %>').val());
                $('#<%:tdScheduledTime.ClientID %>').removeAttr('style');
                $('#<%=txtScheduledDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtScheduledDate.ClientID %>').datepicker('disable');
                $('#<%=txtScheduledTime.ClientID %>').attr('readonly', 'readonly');

                $('#<%=chkIsCITO.ClientID %>').removeAttr('disabled');
                $('#<%:lblPhysician.ClientID %>').removeAttr('style');
                $('#<%=lblPhysician2.ClientID %>').attr('style', 'display:none');
            }
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTestOrderDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var testOrderID = s.cpTestOrderID;
                    onAfterSaveRecordDtSuccess(testOrderID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=txtTestOderNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        $('#<%=chkIsCITO.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=hdnIsCITOHd.ClientID %>').val("1");
                $('#<%=chkIsCITO_Detail.ClientID %>').prop('checked', true);
            } else {
                $('#<%=hdnIsCITOHd.ClientID %>').val("0");
                $('#<%=chkIsCITO_Detail.ClientID %>').prop('checked', false);
            }
        });

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

        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnTestOrderID.ClientID %>').val() == '' || $('#<%:hdnTestOrderID.ClientID %>').val() == '0') {
                $('#btnDocumentChecklist').attr('enabled', 'false');
            }
            else {
                $('#btnDocumentChecklist').removeAttr('enabled');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'orderDocumentCheckList') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();
                var patientInformation = $('#<%:hdnPatientInformation.ClientID %>').val();
                var operatingRoomID = $('#<%:hdnOperatingRoomID.ClientID %>').val();
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + patientInformation + "|" + serviceunitID + "|" + operatingRoomID;
            }
            else if (code == 'specimenInfo' || code == 'specimenDeliveryInfo') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();
                var patientInformation = $('#<%:hdnPatientInformation.ClientID %>').val();
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + patientInformation + "|" + serviceunitID + "|";
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        //#region right panel
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();
            var TransactionStatus = $('#<%:hdnGCTransactionStatus.ClientID %>').val();

            if (testOrderID != null && testOrderID != "" && testOrderID != "0") {
                if (TransactionStatus == Constant.TransactionStatus.OPEN) {
                    errMessage.text = 'Order status OPEN tidak diperbolehkan cetak.';
                    return false;
                } else if (TransactionStatus == Constant.TransactionStatus.VOID) {
                    errMessage.text = 'Order status VOID tidak diperbolehkan cetak.';
                    return false;
                } else {
                    if (code == 'PM-00515' || code == 'PM-00578') {
                        filterExpression.text = testOrderID;
                        return true;
                    }
                    else if (code == 'PM-00528' || code == 'PM-00539' || code == 'PM-00548' || code == 'PM-00556' || code == 'PM-00597') {
                        filterExpression.text = 'TestOrderID = ' + testOrderID;
                        return true;
                    }
                }
            }
            else {
                errMessage.text = 'Pilih nomor order terlebih dahulu.';
                return false;
            }
        }
        //#endregion

        $('.imgPreconditionNotes.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            var id = entity.ItemID;
            var notes = entity.PreconditionNotes;
            var itemName = entity.ItemName1;
            var title = "Syarat dan Kondisi Pemeriksaan (" + itemName + ")";
            displayMessageBox(title, notes);
        });

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

        function onGetScheduleFilterExpression() {
            var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
            if (serviceUnitID == operatingRoomID) {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var scheduleDate = $('#<%=txtScheduledDate.ClientID %>').val();
                var scheduleDateInDatePicker = Methods.getDatePickerDate(scheduleDate);
                var scheduleDateFormatString = Methods.dateToString(scheduleDateInDatePicker);
                var filterExpression = "VisitID = " + visitID + " AND ScheduledDate = '" + scheduleDateFormatString + "' AND GCTransactionStatus NOT IN ('X121^999','X121^005') AND GCOrderStatus NOT IN ('X126^006','X121^005') AND TestOrderID != '" + orderID + "'";
                return filterExpression;
            }
        }

        function onBeforeProposeRecord(errMessage) {
            var resultFinal = true;
            var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == operatingRoomID) {
                var filterExpression = onGetScheduleFilterExpression();
                Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                    if (result != null) {
                        errMessage.text = "Masih ada outstanding order kamar operasi, lanjutkan proses pembuatan order lagi?";
                        resultFinal = false;
                    }
                });
            }
            return resultFinal;
        }
    </script>
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDiagnosa" runat="server" />
    <input type="hidden" value="" id="hdnIsNotesCopyDiagnose" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsLaboratoryUnit" runat="server" />
    <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
    <input type="hidden" value="" id="hdnTimeToday" runat="server" />
    <input type="hidden" value="" id="hdnPatientInformation" runat="server" />
    <input type="hidden" value="" id="hdnIsCITOHd" runat="server" />
    <input type="hidden" value="" id="hdnRemarks" runat="server" />
    <input type="hidden" value="" id="hdnOrderHanyaItemPemeriksaan" runat="server" />
    <input type="hidden" value="" id="hdnPenjaminDisamakanRegistrasi" runat="server" />
    <input type="hidden" value="" id="hdnIsDisplayPrice" runat="server" />
    <input type="hidden" value="" id="hdnIsQuickPicksUsingForm" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingMultiVisitScheduleOrder" runat="server" />
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
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblTestOrderNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestOderNo" Width="232px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                -
                                <%=GetLabel("Jam Order") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtTestOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Catatan Klinis / Diagnosa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarksHd" Width="400px" TextMode="MultiLine" Height="90px" runat="server" onkeyUp="return MaxCount(this,event,500);" />
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
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter Pengirim Order")%></label>
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician2">
                                    <%=GetLabel("Dokter Pelaksana")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                    <%=GetLabel("Unit Penunjang Medis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="120px" runat="server" />
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
                        <tr id="trProcedureGroup" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" runat="server" id="lblProcedureGroup">
                                    <%=GetLabel("Kelompok Tindakan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProcedureGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProcedureGroupCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProcedureGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Waktu Pengerjaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboToBePerformed" ClientInstanceName="cboToBePerformed"
                                    Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboToBePerformedChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Dilakukan Tanggal")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtScheduledDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td id='tdScheduledTime' runat="server">
                                            <asp:TextBox ID="txtScheduledTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td>
                                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
                            </td>
                        </tr>
                        <tr id="trMultiVisitScheduleOrder" runat="server" style="display: none">
                            <td />
                            <td>
                                <asp:CheckBox ID="chkIsMultiVisitScheduleOrder" Width="200px" runat="server" Text=" Penjadwalan Multi Kunjungan" />
                            </td>
                        </tr>
                        <tr id="trLaboratoryOrder" runat="server" style="display: none">
                            <td />
                            <td>
                                <input type="hidden" id="Hidden1" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsPATest" Width="150px" runat="server" Text=" Pemeriksaan PA" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsAdditionalTest" Width="100%" runat="server" Text=" Pemeriksaan Tambahan (Sampel yang ada)" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 70%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 200px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Pelayanan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 300px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblDiagnose">
                                                        <%=GetLabel("Diagnosa")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 300px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblParamedicDetail">
                                                        <%=GetLabel("Dokter / Tenaga Medis Pelaksana")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnParamedicDetailID" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtParamedicDetailCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtParamedicDetailName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td />
                                                <td>
                                                    <asp:CheckBox ID="chkIsCITO_Detail" Width="100px" runat="server" Text="CITO" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan Klinis/Order")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemarksDt" Width="375px" runat="server" TextMode="MultiLine"  onkeyUp="return MaxCount(this,event,500);"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jumlah")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemQty" Width="120px" runat="server" CssClass="number" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%; display: none">
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Perform Date")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPerformDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseName") %>" bindingfield="DiagnoseName" />
                                                    <input type="hidden" value="<%#:Eval("DetailOrderParamedicCode") %>" bindingfield="DetailOrderParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("DetailOrderParamedicName") %>" bindingfield="DetailOrderParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("GCToBePerformed") %>" bindingfield="GCToBePerformed" />
                                                    <input type="hidden" value="<%#:Eval("IsCITO") %>" bindingfield="IsCITO" />
                                                    <input type="hidden" value="<%#:Eval("PerformedDateInDatePickerFormat") %>" bindingfield="PerformedDate" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("ItemQty") %>" bindingfield="ItemQty" />
                                                    <input type="hidden" value='<%#:Eval("PreconditionNotes") %>' bindingfield="PreconditionNotes" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="250px" />
                                            <asp:BoundField DataField="ItemQty" HeaderText="Qty" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="DetailOrderParamedicName" HeaderText="Dokter / Tenaga Medis Pelaksana"
                                                HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DiagnoseName" HeaderText="Diagnosa" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan Klinis/Order" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="TestOrderStatus" HeaderText="Status Order" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgPreconditionNotes imgLink" <%# Eval("PreconditionNotes").ToString() != "" ?  "" : "style='display:none'" %>
                                                                    title='<%=GetLabel("Precondition Notes")%>' src='<%# ResolveUrl("~/Libs/Images/Button/info.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" <%=IsEditable.ToString() == "False" ? "style='display:none'" : "style='margin-right: 300px'" %>>
                            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblTestOrderQuickPick"
                                <%=IsEditable.ToString() == "False" ? "style='display:none'" : "style='margin-right: 300px'" %>>
                                <%= GetLabel("Quick Picks")%></span> <span class="lblLink" id="lblTestTemplate" <%=IsEditable.ToString() == "False" ? "style='display:none'" : "" %>>
                                    <%= GetLabel("Panel Order")%></span>
                    </div>
                </td>
            </tr>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
