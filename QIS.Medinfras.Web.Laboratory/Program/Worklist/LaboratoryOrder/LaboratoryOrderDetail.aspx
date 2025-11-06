<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="LaboratoryOrderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryOrderDetail" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailServiceCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailLogisticCtl.ascx"
    TagName="LogisticCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSReturnCtl.ascx"
    TagName="DrugMSReturnCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnClinicTransactionBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClinicTransactionTestOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Informasi Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table style="float: right" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="100px" />
        </colgroup>
        <tr>
            <td>
                <div id="divVisitNote" runat="server" style="text-align: left">
                    <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                        alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                </div>
            </td>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnLogisticLocationID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnReferrerParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianName" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnValidateTariffOnPropose" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            var TestOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
            if (TestOrderID != "" && TestOrderID != "0") {
                var testOrderCount = parseInt('<%=testOrderOpenCount %>');
                if (testOrderCount > 0) {
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TestOrder/TestOrderDtCtl.ascx");
                    openUserControlPopup(url, TestOrderID, 'Detail Order Pelayanan', 800, 600);
                }
                $('#<%=btnClinicTransactionTestOrder.ClientID %>').click();
            }

            $('#imgVisitNote.imgLink').click(function () {
                var id = $('#<%=hdnRegistrationID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
                openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
            });
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            if (getTransactionHdID() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }
        function getTransactionHdID() {
            return $('#<%=hdnTransactionHdID.ClientID %>').val();
        }
        function getPhysicianID() {
            return $('#<%=hdnPhysicianID.ClientID %>').val();
        }
        function getIsHealthcareServiceUnitHasParamedic() {
            return ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.Value %>').val() == '1');
        }
        function getDepartmentID() {
            return $('#<%=hdnDepartmentID.ClientID %>').val();
        }
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            if ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.ClientID %>').val() == '1') {
                if ($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() != null && $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() != "") {
                    filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0 AND IsAvailable = 1";
                } else {
                    filterExpression = "IsDeleted = 0 AND IsAvailable = 1";
                }
            } else {
                filterExpression = "IsDeleted = 0 AND IsAvailable = 1";
            }
            return filterExpression;
        }
        function onGetTestPartnerFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND GCTestPartnerType = '" + Constant.TestPartnerType.LABORATORIUM + "'";
            return filterExpression;
        }

        function getLocationID() {
            return $('#<%=hdnLocationID.ClientID %>').val();
        }
        function getLogisticLocationID() {
            return $('#<%=hdnLogisticLocationID.ClientID %>').val();
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
        function getGCItemType() {
            return $('#<%=hdnGCItemType.ClientID %>').val();
        }
        function getIsAIOTransaction() {
            return $('#<%=hdnIsCheckedAIOTransaction.ClientID %>').val();
        }
        function getchkIsAIOTransaction() {
            return $('#<%=chkIsAIOTransaction.ClientID %>').is(":checked");
        }
        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = getTransactionHdID();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(transactionID) {
            if ($('#<%=hdnTransactionHdID.ClientID %>').val() == '0') {
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
            }
        }

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
            });

            if ($('#<%=txtTransactionDateLab.ClientID %>').attr('readonly') == null)
                setDatePicker('<%=txtTransactionDateLab.ClientID %>');

            $('#<%=btnClinicTransactionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl("~/Program/Worklist/LaboratoryOrder/LaboratoryOrderList.aspx"); ;
            });

            $('#<%=btnClinicTransactionTestOrder.ClientID %>').click(function () {
                var TestOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TestOrder/TestOrderDtCtl.ascx");
                if (TestOrderID != "" && TestOrderID != "0") {
                    openUserControlPopup(url, TestOrderID, 'Order Pelayanan Detail', 800, 600);
                }
                else showToast(' Failed', 'Error Message : ' + param[2]);
            });

            //#region Transaction No
            $('#lblTransactionNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val() + ' AND HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(); ;
                openSearchDialog('patientchargeshd', filterExpression, function (value) {
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

            onLoadService();
            onLoadDrugMS();
            onLoadDrugMSReturn();
            onLoadLogistic();
            calculateAllTotal();
        }

        function setCustomToolbarVisibility() {
            var testOrderID = parseInt($('#<%=hdnTestOrderID.ClientID %>').val());
            if (testOrderID > 0)
                $('#<%=btnClinicTransactionTestOrder.ClientID %>').show();
            else
                $('#<%=btnClinicTransactionTestOrder.ClientID %>').hide();

            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
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
                //                var url = ResolveUrl('~/Program/Worklist/LaboratoryOrder/LaboratoryChargesVoidCtl.ascx');
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

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

        function calculateAllTotal() {
            var serviceTotalPatient = getServiceTotalPatient();
            var serviceTotalPayer = getServiceTotalPayer();

            var drugMSTotalPatient = getDrugMSTotalPatient();
            var drugMSTotalPayer = getDrugMSTotalPayer();

            var drugMSReturnTotalPatient = getDrugMSReturnTotalPatient();
            var drugMSReturnTotalPayer = getDrugMSReturnTotalPayer();

            var logisticTotalPatient = getLogisticTotalPatient();
            var logisticTotalPayer = getLogisticTotalPayer();

            var totalPayer = (serviceTotalPayer + drugMSTotalPayer + drugMSReturnTotalPayer + logisticTotalPayer);
            var totalPatient = (serviceTotalPatient + drugMSTotalPatient + drugMSReturnTotalPatient + logisticTotalPatient);

            $('#<%=hdnTotalPayer.ClientID %>').val(totalPayer);
            $('#<%=hdnTotalPatient.ClientID %>').val(totalPatient);

            $('#<%=txtTotalPayer.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=txtTotalPatient.ClientID %>').val(totalPatient).trigger('changeValue');
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDateLab.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Simpan Transaksi Terlebih Dahulu!';
                return false;
            }
            else if (code == "PM-00426" || code == "PM-00105") {
                filterExpression.text = registrationID;
                return true;
            }
            else if (code == "LB-00011" || code == "PM-00125" || code == "PM-00655") {
                filterExpression.text = transactionID;
                return true;
            }
            else if (code == 'PM-00578' || code == "PM-00686") {
                filterExpression.text = testOrderID;
                return true;
            }
            else {
                filterExpression.text = transactionID + '|' + testOrderID;
                return true;
            }
        }

        function setRightPanelButtonEnabled() {
            var deptID = $('#<%=hdnDepartmentFromID.ClientID %>').val();
            var transactionHdID = $('#<%:hdnTransactionHdID.ClientID %>').val();
            var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();

            var isOutpatientMustCloseTransactionForSentToLIS = $('#<%=hdnIsOutpatientMustCloseTransactionForSentToLIS.ClientID %>').val();

            if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == 'X121^001') {
                $('#btnChangeTransactionPhysician').removeAttr('enabled');
            }
            else {
                $('#btnChangeTransactionPhysician').attr('enabled', 'false');
            }

            if (testOrderID != null && testOrderID != "0" && testOrderID != "") {
                var filterExpressionOrder = "TestOrderID = '" + testOrderID + "'";
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
                });
            }
            else {
                $('#btnPathologyAnatomyInfo').attr('enabled', 'false');
            }

            if (transactionHdID != "") {
                if (isOutpatientMustCloseTransactionForSentToLIS == "1") {
                    if (deptID != Constant.Facility.INPATIENT) {
                        if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == 'X121^005') {
                            $('#btnSendOrderToLIS').removeAttr('enabled');
                        }
                        else {
                            $('#btnSendOrderToLIS').attr('enabled', 'false');
                        }
                    }
                    else {
                        if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == 'X121^001') {
                            $('#btnSendOrderToLIS').attr('enabled', 'false');
                        }
                        else {
                            $('#btnSendOrderToLIS').removeAttr('enabled');
                        }
                    }
                }
                else {
                    if ($('#<%=hdnGCTransactionStatus.ClientID %>').val() == 'X121^001') {
                        $('#btnSendOrderToLIS').attr('enabled', 'false');
                    }
                    else {
                        $('#btnSendOrderToLIS').removeAttr('enabled');
                    }
                }
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'transactionNotes' || code == 'changeBloodType') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            } else if (code == 'sendPGxTestOrder') {
                return $('#<%:hdnTestOrderID.ClientID %>').val() + "|" + $('#<%:hdnTransactionHdID.ClientID %>').val() + "|" + $('#<%:txtTransactionNo.ClientID %>').val();
            }
            else if (code == 'specimenInfo') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + "" + "|" + serviceunitID + "|";
            }
            else if (code == 'specimenDeliveryInfo') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + "" + "|" + serviceunitID + "|";
            }
            else if (code == 'pathologyAnatomyInfoEntry') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = $('#<%:hdnTestOrderID.ClientID %>').val();
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                var transactionID = $('#<%:hdnTransactionHdID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + "" + "|" + serviceunitID + "|" + transactionID;
            }
            else if (code == 'printSuratRujukan') {
                return $('#<%:hdnVisitID.ClientID %>').val() + "|" + $('#<%:hdnTestOrderID.ClientID %>').val() + "|" + $('#<%:hdnTransactionHdID.ClientID %>').val() + "|" + $('#<%:txtTransactionNo.ClientID %>').val();
            }
            else {
                return $('#<%:hdnTransactionHdID.ClientID %>').val();
            }
        }

    </script>
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnProcedureGroupID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentFromID" runat="server" />
    <input type="hidden" value="" id="hdnIsOutpatientMustCloseTransactionForSentToLIS" runat="server" />
    <input type="hidden" value="" id="hdnIsCheckedAIOTransaction" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasAIOPackage" runat="server" />
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
                                <label class="lblLink" id="lblTransactionNo">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                                <img id="imgIsAIOTransaction" runat="server" width="40" src='' alt='' />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Jam") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtTransactionDateLab" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
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
                                    <tr id="trIsChargesGenerateMCU" runat="server" style="display: none">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsChargesGenerateMCU" runat="server" /><%:GetLabel(" Transaksi Generate MCU")%>
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
                                <label class="lblNormal" id="lblTestOrderInfo">
                                    <%=GetLabel("Informasi Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestOrderInfo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Kelompok Tindakan Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcedureOrderInfo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="Label1" class="lblNormal" runat="server">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="150px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr style='display: none'>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsPATest" Width="150px" runat="server" Text=" Pemeriksaan PA" />
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
                                <%=GetLabel("Pelayanan") %></li>
                            <li contentid="containerDrugMS">
                                <%=GetLabel("Obat & Alkes") %></li>
                            <li contentid="containerDrugMSReturn">
                                <%=GetLabel("Retur Obat & Alkes") %></li>
                            <li contentid="containerLogistics">
                                <%=GetLabel("Barang Umum") %></li>
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
                    <input type="hidden" value="" id="hdnTotalPayer" runat="server" />
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
                    <input type="hidden" value="" id="hdnTotalPatient" runat="server" />
                    Rp.
                    <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="txtCurrency" runat="server"
                        Width="200px" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
