<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="NutritionOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NutritionOrder" %>

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
    <input type="hidden" value="" id="hdnIPAddress" runat="server" />
    <input type="hidden" value="6000" id="hdnPort" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = $('#<%=hdnOrderID.ClientID %>').val();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(orderID) {
            if ($('#<%=hdnOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnOrderID.ClientID %>').val(orderID);
                var filterExpression = 'NutritionOrderHdID = ' + orderID;
                Methods.getObject('GetNutritionOrderHdList', filterExpression, function (result) {
                    $('#<%=txtOrderNo.ClientID %>').val(result.NutritionOrderNo).trigger('change');
                });

                onAfterCustomSaveSuccess();
            }
        }

        function onLoad() {
            setCustomToolbarVisibility();

            onBeforeRightPanelEnabled();

            if ($('#<%=txtOrderDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtOrderDate.ClientID %>');
                $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            }

            if ($('#<%=txtScheduleDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtScheduleDate.ClientID %>');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
            }

            if ($('#<%=hdnItemIndex.ClientID %>').val() != '' || $('#<%=hdnItemIndex.ClientID %>').val() != null) {
                if ($('#<%=txtOrderNo.ClientID %>').val() == '') {
                    var itemIndex = $('#<%=hdnItemIndex.ClientID %>').val();
                    cboDietType.SetValue(itemIndex);
                }
            }

            if ($('#<%=txtOrderNo.ClientID %>').val() != "") {
                $('.btnMultiDiet').removeAttr('enabled');
            }
            else {
                $('.btnMultiDiet').attr('enabled', 'false');
            }

            $btnSave = null;
            $('.btnMultiDiet').live('click', function () {
                if ($(this).attr('enabled') != 'false') {
                    var param = $('#<%:hdnOrderID.ClientID %>').val();
                    $btnSave = $(this);
                    openMatrixControl('NutritionOrderHdDietType', param, 'Jenis Diet');
                }
            });

            //#region Transaction No
            $('#lblOrderNo.lblLink').click(function () {
                var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('nutritionorderhd', filterExpression, function (value) {
                    $('#<%=txtOrderNo.ClientID %>').val(value);
                    onTxtOrderNoChanged(value);
                });
            });

            $('#<%=txtOrderNo.ClientID %>').change(function () {
                onTxtOrderNoChanged($(this).val());
            });

            function onTxtOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Paramedic
            $('#<%=lblParamedic.ClientID %>.lblLink').click(function () {
                var filterExpression = "GCParamedicMasterType IN ('" + Constant.ParamedicType.Nutritionist + "','" + Constant.ParamedicType.Nurse + "') AND IsDeleted = 0";
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    onTxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').change(function () {
                onTxtParamedicCodeChanged($(this).val());
            });

            function onTxtParamedicCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID %>').val('');
                        $('#<%=txtParamedicCode.ClientID %>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Diagnose
            $('#lblDiagnose.lblLink').live('click', function () {
                openSearchDialog('diagnose', "IsNutritionDiagnosis = 1 AND IsDeleted = 0", function (value) {
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
                        $('#<%=txtDiagnoseID.ClientID %>').val(result.DiagnoseID);
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    }
                    else {
                        $('#<%=txtDiagnoseID.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Schedule Date

            //#region first date load
            var scheDate = $('#<%=txtScheduleDate.ClientID %>').val().substring(0, 2);
            if (scheDate == '01' || scheDate == '11' || scheDate == '21') {
                $('#<%=hdnMealDay.ClientID %>').val('1');
            } else if (scheDate == '02' || scheDate == '12' || scheDate == '22') {
                $('#<%=hdnMealDay.ClientID %>').val('2');
            } else if (scheDate == '03' || scheDate == '13' || scheDate == '23') {
                $('#<%=hdnMealDay.ClientID %>').val('3');
            } else if (scheDate == '04' || scheDate == '14' || scheDate == '24') {
                $('#<%=hdnMealDay.ClientID %>').val('4');
            } else if (scheDate == '05' || scheDate == '15' || scheDate == '25') {
                $('#<%=hdnMealDay.ClientID %>').val('5');
            } else if (scheDate == '06' || scheDate == '16' || scheDate == '26') {
                $('#<%=hdnMealDay.ClientID %>').val('6');
            } else if (scheDate == '07' || scheDate == '17' || scheDate == '27') {
                $('#<%=hdnMealDay.ClientID %>').val('7');
            } else if (scheDate == '08' || scheDate == '18' || scheDate == '28') {
                $('#<%=hdnMealDay.ClientID %>').val('8');
            } else if (scheDate == '09' || scheDate == '19' || scheDate == '29') {
                $('#<%=hdnMealDay.ClientID %>').val('9');
            } else if (scheDate == '10' || scheDate == '20' || scheDate == '30') {
                $('#<%=hdnMealDay.ClientID %>').val('10');
            } else {
                $('#<%=hdnMealDay.ClientID %>').val('11');
            }
            //#endregion

            //#region any date changes
            $('#<%=txtScheduleDate.ClientID %>').change(function () {
                $('#containerEntry').hide();
                onTxtScheduleDateChanged($(this).val());
            });

            function onTxtScheduleDateChanged(value) {
                var scheduleDate = value.substring(0, 2);
                if (scheduleDate == '01' || scheduleDate == '11' || scheduleDate == '21') {
                    $('#<%=hdnMealDay.ClientID %>').val('1');
                } else if (scheduleDate == '02' || scheduleDate == '12' || scheduleDate == '22') {
                    $('#<%=hdnMealDay.ClientID %>').val('2');
                } else if (scheduleDate == '03' || scheduleDate == '13' || scheduleDate == '23') {
                    $('#<%=hdnMealDay.ClientID %>').val('3');
                } else if (scheduleDate == '04' || scheduleDate == '14' || scheduleDate == '24') {
                    $('#<%=hdnMealDay.ClientID %>').val('4');
                } else if (scheduleDate == '05' || scheduleDate == '15' || scheduleDate == '25') {
                    $('#<%=hdnMealDay.ClientID %>').val('5');
                } else if (scheduleDate == '06' || scheduleDate == '16' || scheduleDate == '26') {
                    $('#<%=hdnMealDay.ClientID %>').val('6');
                } else if (scheduleDate == '07' || scheduleDate == '17' || scheduleDate == '27') {
                    $('#<%=hdnMealDay.ClientID %>').val('7');
                } else if (scheduleDate == '08' || scheduleDate == '18' || scheduleDate == '28') {
                    $('#<%=hdnMealDay.ClientID %>').val('8');
                } else if (scheduleDate == '09' || scheduleDate == '19' || scheduleDate == '29') {
                    $('#<%=hdnMealDay.ClientID %>').val('9');
                } else if (scheduleDate == '10' || scheduleDate == '20' || scheduleDate == '30') {
                    $('#<%=hdnMealDay.ClientID %>').val('10');
                } else {
                    $('#<%=hdnMealDay.ClientID %>').val('11');
                }
            }
            //#endregion

            //#endregion

            //#region Item
            function getItemMasterFilterExpression() {
                var oMealTime = cboMealTime.GetValue();
                var filterExpression = "GCMealTime = '" + oMealTime + "' AND IsDeletedDetail = 0 AND IsDeletedHd = 0";

                var isUsingDay = $('#<%=hdnIsMealPlanByDay.ClientID %>').val();

                if (isUsingDay == "1") {
                    if ($('#<%=hdnMealDay.ClientID %>').val() == "1") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^001')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "2") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^002')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "3") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^003')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "4") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^004')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "5") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^005')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "6") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^006')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "7") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^007')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "8") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^008')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "9") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^009')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "10") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^010')";
                    } else if ($('#<%=hdnMealDay.ClientID %>').val() == "11") {
                        filterExpression += " AND MealPlanDtID IN (SELECT a.MealPlanDtID FROM MealPlanDtItem a WITH(NOLOCK) WHERE a.IsDeleted = 0 AND a.GCMealDay = 'X196^011')";
                    }
                }

                return filterExpression;
            }

            $('#lblItem.lblLink').click(function () {
                var oMealTime = cboMealTime.GetValue();
                if (oMealTime != null && oMealTime != "") {
                    openSearchDialog('mealplan1', getItemMasterFilterExpression(), function (value) {
                        $('#<%=txtItemCode.ClientID %>').val(value);
                        onTxtItemCodeChanged(value);
                    });
                } else {
                    displayMessageBox('INFORMATION', "Harap pilih Jadwal Makan terlebih dahulu.");
                }
            });

            $('#<%=txtItemCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = getItemMasterFilterExpression() + " AND MealPlanCode = '" + value + "'";
                Methods.getObject('GetvMealPlan1List', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.MealPlanID);
                        $('#<%=txtItemCode.ClientID %>').val(result.MealPlanCode);
                        $('#<%=txtItemName.ClientID %>').val(result.MealPlanName);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                    $('#<%=hdnEntryID.ClientID%>').val('');

                    cboMealTime.SetValue('');
                    $('#<%=txtMealDay.ClientID %>').val($('#<%=hdnMealDay.ClientID %>').val());
                    $('#<%=lblParamedic.ClientID %>').attr('class', 'lblDisabled');

                    $('#<%=txtParamedicCode.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtDiagnoseID.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');

                    $('#<%=chkIsNotForPatient.ClientID %>').prop('checked', false);
                    $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);

                    $('#<%=txtRemarks.ClientID %>').val('');
                    $('#containerEntry').show();
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
                $('.btnMultiDiet').removeAttr('enabled');
            });

            $('#lblQuickPickHistoryOrder').die('click');
            $('#lblQuickPickHistoryOrder').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Nutrition/NutritionOrderHistoryCtl.ascx");
                    var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var nutritionOrderHdID = $('#<%=hdnOrderID.ClientID %>').val();
                    var paramedic = $('#<%=hdnParamedicID.ClientID %>').val();
                    var classID = $('#<%=hdnClassID.ClientID %>').val();
                    var orderDate = $('#<%=txtOrderDate.ClientID %>').val();
                    var orderTime = $('#<%=txtOrderTime.ClientID %>').val();
                    var heatlhcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                    var scheduleDate = $('#<%=txtScheduleDate.ClientID %>').val();
                    var scheduleTime = $('#<%=txtScheduleTime.ClientID %>').val();
                    var dietType = cboDietType.GetValue();
                    var diet = "";
                    if (dietType != null) {
                        diet = dietType;
                    }
                    else {
                        diet = "";
                    }

                    var YYYY = scheduleDate.substring(6, 10);
                    var MM = scheduleDate.substring(3, 5);
                    var DD = scheduleDate.substring(0, 2);
                    var dateALL = YYYY + '-' + MM + '-' + DD;

                    var queryString = registrationID + '|' + visitID + '|' + nutritionOrderHdID + '|' + paramedic + '|' + classID + '|' + orderDate + '|' + orderTime + '|' + heatlhcareServiceUnitID + '|' + dateALL + '|' + scheduleTime + '|' + diet;
                    openUserControlPopup(url, queryString, 'History Order Menu Makan', 1300, 600);
                }
            });

            $('#lblQuickPicks').die('click');
            $('#lblQuickPicks').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var url = '~/Libs/Program/Module/PatientManagement/TransactionPage/Nutrition/NutritionOrderQuickPicksCtl.ascx';
                    var nutritionOrderID = $('#<%=hdnOrderID.ClientID %>').val();
                    var mealDay = $('#<%=hdnMealDay.ClientID %>').val();
                    var orderDate = $('#<%=txtOrderDate.ClientID %>').val();
                    var orderTime = $('#<%=txtOrderTime.ClientID %>').val();
                    var notes = $('#<%=txtNotes.ClientID %>').val();
                    var scheduleDate = $('#<%=txtScheduleDate.ClientID %>').val();
                    var scheduleTime = $('#<%=txtScheduleTime.ClientID %>').val();

                    var YYYY = scheduleDate.substring(6, 10);
                    var MM = scheduleDate.substring(3, 5);
                    var DD = scheduleDate.substring(0, 2);
                    var dateALL = YYYY + '-' + MM + '-' + DD;

                    var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                    var paramedic = $('#<%=hdnParamedicID.ClientID %>').val();
                    var classID = $('#<%=hdnClassID.ClientID %>').val();
                    var diagnoseID = $('#<%=txtDiagnoseID.ClientID %>').val();
                    var numOfCal = $('#<%=txtNumberOfCalories.ClientID %>').val();
                    var numOfProtein = $('#<%=txtNumberOfProtein.ClientID %>').val();
                    var dietType = cboDietType.GetValue();
                    var diet = "";
                    if (dietType != null) {
                        diet = dietType;
                    }
                    else {
                        diet = "";
                    }

                    var queryString = nutritionOrderID + "|" + mealDay + "|" + orderDate + "|" + orderTime + "|" + dateALL + "|" + scheduleTime + "|" + visitID + "|" + paramedic + "|" + classID + "|" + diagnoseID + "|" + numOfCal + "|" + diet + "|" + numOfProtein + "|" + notes;
                    openUserControlPopup(url, queryString, 'Quick Picks : Order Menu Makan', 1300, 600);
                }
            });
        }

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.NutritionOrderDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {

            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);

            cboMealTime.SetValue(entity.GCMealTime);
            //            cboMealDay.SetValue(entity.GCMealDay);
            $('#<%=txtMealDay.ClientID %>').val(entity.MealDay);
            $('#<%=hdnEntryID.ClientID %>').val(entity.NutritionOrderDtID);
            $('#<%=hdnItemID.ClientID %>').val(entity.MealPlanID);
            $('#<%=txtItemCode.ClientID %>').val(entity.MealPlanCode);
            $('#<%=txtItemName.ClientID %>').val(entity.MealPlanName);
            if (entity.IsNotForPatient == "True") {
                $('#<%=chkIsNotForPatient.ClientID %>').prop('checked', true);
            } else {
                $('#<%=chkIsNotForPatient.ClientID %>').prop('checked', false);
            }
            if (entity.IsNewPatient == "True") {
                $('#<%=chkIsNewPatient.ClientID %>').prop('checked', true);
            } else {
                $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);
            }
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#containerEntry').show();
        });

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtOrderDate.ClientID %>').val());
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
                    var orderID = s.cpOrderID;
                    onAfterSaveRecordDtSuccess(orderID);
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
            if ($('#<%=txtOrderNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
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
                //                var transactionID = $('#<%=hdnOrderID.ClientID %>').val();
                //                var id = registrationID + '|' + transactionID;
                //                openUserControlPopup(url, id, 'Void Transaction', 400, 230);
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        function onBeforeRightPanelEnabled() {
            if ($('#<%=txtOrderNo.ClientID %>').val() != '') {
                $('#btnMultipleDietType').removeAttr('enabled');
            }
            else {
                $('#btnMultipleDietType').attr('enabled', 'false');
            }
        }

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }
    </script>
    <input type="hidden" value="" id="hdnIsMealPlanByDay" runat="server" />
    <input type="hidden" value="" id="hdnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnMealDay" runat="server" />
    <input type="hidden" value="" id="hdnItemIndex" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultInterventionNotes" runat="server" />
    <input type="hidden" value="" id="hdnDiagnoseID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnIsDefaultOrderDate" runat="server" />
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
                                <label class="lblLink" id="lblOrderNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="232px" ReadOnly="true" runat="server" />
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
                                            <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label>
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="400px" TextMode="MultiLine" Height="90px" runat="server" />
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
                                <label class="lblLink lblMandatory" runat="server" id="lblParamedic">
                                    <%=GetLabel("Perawat / Nutritionist")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnParamedicID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtParamedicCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParamedicName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDiagnose">
                                    <%=GetLabel("Diagnosa Gizi")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDiagnoseID" Width="120px" runat="server" />
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
                                <%=GetLabel("Jadwal Tanggal") %>
                                -
                                <%=GetLabel(" Jam") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblDietType">
                                    <%=GetLabel("Jenis Diet") %>
                                </label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboDietType" ClientInstanceName="cboDietType" runat="server"
                                                Width="175px">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <input type="button" id="btnMultiDiet" class="btnMultiDiet" title='<%:GetLabel("Multi Diet") %>'
                                                value="Multi Diet" enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblNumberOfCalories">
                                    <%=GetLabel("Jumlah Kalori") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNumberOfCalories" Width="80px" CssClass="number" runat="server"
                                    Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="LblNumberOfProtein">
                                    <%=GetLabel("Jumlah Protein") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNumberOfProtein" Width="80px" CssClass="number" runat="server"
                                    Style="text-align: right" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="tdLabel"><label id="lblMultiDietType" class="lblLink"><%=GetLabel("Jenis Diet") %></td>
                            <td><asp:TextBox ID="txtMultiDietType" Width="400px" TextMode="MultiLine" Height="90px" runat="server" ReadOnly="true"/></td>
                       </tr>--%>
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
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory" id="Label1">
                                                        <%=GetLabel("Jadwal Makan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboMealTime" ClientInstanceName="cboMealTime" runat="server"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <table>
                                                        <colgroup>
                                                            <col style="width: 150px" />
                                                            <col style="width: 150px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsNotForPatient" runat="server" />
                                                                <%=GetLabel("Bukan untuk Pasien")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsNewPatient" runat="server" />
                                                                <%=GetLabel("Pasien Baru")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory" id="Label2">
                                                        <%=GetLabel("Hari Ke-")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMealDay" ReadOnly="true" Width="10%" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Panel Menu")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
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
                                                    <label class="lblMandatory" id="Label3">
                                                        <%=GetLabel("Status Makan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboMealStatus" ClientInstanceName="cboMealStatus" runat="server"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan Order")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemarks" Width="375px" runat="server" TextMode="MultiLine" />
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
                                                    <center>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" class="detailEntrySaveCancelButton" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                                </td>
                                                                <td>
                                                                    <input type="button" class="detailEntrySaveCancelButton" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </center>
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
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="NutritionOrderDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
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
                                                    <input type="hidden" value="<%#:Eval("NutritionOrderDtID") %>" bindingfield="NutritionOrderDtID" />
                                                    <input type="hidden" value="<%#:Eval("GCMealTime") %>" bindingfield="GCMealTime" />
                                                    <input type="hidden" value="<%#:Eval("GCMealDay") %>" bindingfield="GCMealDay" />
                                                    <input type="hidden" value="<%#:Eval("MealDay") %>" bindingfield="MealDay" />
                                                    <input type="hidden" value="<%#:Eval("cfMealDay") %>" bindingfield="cfMealDay" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("MealPlanID") %>" bindingfield="MealPlanID" />
                                                    <input type="hidden" value="<%#:Eval("MealPlanCode") %>" bindingfield="MealPlanCode" />
                                                    <input type="hidden" value="<%#:Eval("MealPlanName") %>" bindingfield="MealPlanName" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseName") %>" bindingfield="DiagnoseName" />
                                                    <input type="hidden" value="<%#:Eval("IsNotForPatient") %>" bindingfield="IsNotForPatient" />
                                                    <input type="hidden" value="<%#:Eval("IsNewPatient") %>" bindingfield="IsNewPatient" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MealTime" HeaderText="Jadwal Makan" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MealDay" HeaderText="Hari Ke - " HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MealPlanName" HeaderText="Panel Menu" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="300px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" />
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
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px; <%=IsEditable.ToString() == "False" ? "display:none": "" %>">
                            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblQuickPicks" style="margin-right: 300px;
                                <%=IsEditable.ToString() == "False" ? "display:none": "" %>">
                                <%= GetLabel("Quick Picks")%></span><span class="lblLink" id="lblQuickPickHistoryOrder"
                                    style="<%=IsEditable.ToString() == "False" ? "display:none": "" %>">
                                    <%= GetLabel("Quick Picks From History")%></span>
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
