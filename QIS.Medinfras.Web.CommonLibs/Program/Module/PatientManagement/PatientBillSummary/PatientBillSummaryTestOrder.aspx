<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx3.Master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryTestOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryTestOrder" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailServiceCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailDrugMSReturnCtl.ascx"
    TagName="DrugMSReturnCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetailLogisticCtl.ascx"
    TagName="LogisticCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnVisitDepartmentID" runat="server" />
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
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianName" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="0" id="hdnIsDischarges" runat="server" />
    <input type="hidden" value="0" id="hdnChargesHdRowCount" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasAIOPackage" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
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

            if ($('#<%=hdnRequestID.Value %>').val() == 'lb') {
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
        function getchkIsPATest() {
            return $('#<%=chkIsPATest.ClientID %>').is(":checked");
        }
        function getchkIsAIOTransaction() {
            return $('#<%=chkIsAIOTransaction.ClientID %>').is(":checked");
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = getTransactionHdID();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();

            getStatusPerRegOutstanding(); //general ctl
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(transactionID) {
            var hdnTransactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            if (hdnTransactionID == '0' || hdnTransactionID == '') {
                $('#<%=hdnTransactionHdID.ClientID %>').val(transactionID);
                var filterExpression = 'TransactionID = ' + transactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo).trigger('change');
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
                if ($contentID == "containerLogistics") $('#<%=hdnGCItemType.ClientID %>').val(Constant.ItemGroupMaster.LOGISTIC); else $('#<%=hdnGCItemType.ClientID %>').val(Constant.ItemGroupMaster.DRUGS);
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            if ($('#<%=txtTransactionDate.ClientID %>').attr('readonly') == null)
                setDatePicker('<%=txtTransactionDate.ClientID %>');

            //#region Transaction No
            function onGetTransactionNoFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblTransactionNo.lblLink').click(function () {
                openSearchDialog('patientchargeshd6', onGetTransactionNoFilterExpression(), function (value) {
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
                var filterExpression = "ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=hdnLogisticLocationID.ClientID %>').val(result.LogisticLocationID);
                        $('#<%=hdnIsHealthcareServiceUnitHasParamedic.ClientID %>').val(result.IsHasParamedic ? '1' : '0');
                        setServiceItemFilterExpression(result.HealthcareServiceUnitID);
                        cboDrugMSLocation.PerformCallback();
                        cboLogisticLocation.PerformCallback();
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                        $('#<%=hdnIsHealthcareServiceUnitHasParamedic.ClientID %>').val('0');
                    }
                });
            }
            //#endregion

            onLoadService();
            onLoadDrugMS();
            onLoadDrugMSReturn();
            onLoadLogistic();
            calculateAllTotal();
        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnIsAdminCanCancelAllTransaction.ClientID %>').val() == "0") {
                        if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
                            $('#<%=btnVoid.ClientID %>').show();
                        } else {
                            $('#<%=btnVoid.ClientID %>').hide();
                        }
                    } else {
                        if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN || $('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.WAIT_FOR_APPROVAL) {
                            $('#<%=btnVoid.ClientID %>').show();
                        } else {
                            $('#<%=btnVoid.ClientID %>').hide();
                        }
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        $('#<%=chkIsCorrectionTransaction.ClientID %>').live('click', function (evt) {
            if ($('#<%=chkIsCorrectionTransaction.ClientID %>').is(':checked')) {
                $('#<%=hdnIsCheckedTransactionCorrection.ClientID %>').val("1");
            } else {
                $('#<%=hdnIsCheckedTransactionCorrection.ClientID %>').val("0");
            }
        });

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

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                //                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientChargesTestVoidCtl.ascx');
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
            getStatusPerRegOutstanding(); //general ctl
            onRefreshControl();
        }

        function setRightPanelButtonEnabled() {
            var transactionHdID = parseInt($('#<%=hdnTransactionHdID.ClientID %>').val());
            if (transactionHdID != null && transactionHdID != "0" && transactionHdID != "") {
                $('#btnModality').removeAttr('enabled');
            }
            else {
                $('#btnModality').attr('enabled', 'false');
            }
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

            $('#<%=txtTotalPayer.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=txtTotalPatient.ClientID %>').val(totalPatient).trigger('changeValue');
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'sendBARNotificationMessage') {
                if ($('#<%:hdnTransactionHdID.ClientID %>').val() != '' || $('#<%:hdnTransactionHdID.ClientID %>').val() != '0') {
                    var messageType = "01";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = $('#<%:hdnTransactionHdID.ClientID %>').val();
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else if (code == 'modality') {
                return $('#<%:hdnTransactionHdID.ClientID %>').val() + "|" + $('#<%:hdnRegistrationID.ClientID %>').val() + "|" + $('#<%=txtTransactionNo.ClientID %>').val() + "|" + $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Simpan Transaksi Terlebih Dahulu!';
                return false;
            }
            else if (code == "PM-00655") {
                filterExpression.text = transactionID;
                return true;
            }
            else {
                if (GCTransactionStatus != Constant.TransactionStatus.OPEN && GCTransactionStatus != Constant.TransactionStatus.VOID) {
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
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAdminCanCancelAllTransaction" runat="server" />
    <input type="hidden" value="" id="hdnIsCheckedTransactionCorrection" runat="server" />
    <input type="hidden" value="" id="hdnIsCheckedAIOTransaction" runat="server" />
    <input type="hidden" value="" id="hdnIsLaboratoryUnit" runat="server" />
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
                                            <div style="position: relative">
                                                <label class="lblLink lblKey" id="lblTransactionNo">
                                                    <%=GetLabel("No. Transaksi")%></label></div>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionNo" Width="200px" ReadOnly="true" runat="server" />
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
                                                    <td style="padding-right: 1px; width: 150px">
                                                        <asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                    </tr>
                                </table>
                            </td>
                            <td style="padding: 5px; vertical-align: top">
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsCorrectionTransaction" runat="server" /><%:GetLabel(" Transaksi Koreksi")%>
                                        </td>
                                    </tr>
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
                                <label id="Label1" class="lblNormal" runat="server">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trServiceUnit" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                    Penunjang Medis</label>
                            </td>
                            <td>
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
                                <%=GetLabel("OBAT & ALKES") %></li>
                            <li contentid="containerDrugMSReturn">
                                <%=GetLabel("RETUR OBAT & ALKES") %></li>
                            <li contentid="containerLogistics">
                                <%=GetLabel("BARANG UMUM") %></li>
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
                        <%=GetLabel("Total Instansi") %>
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
                        <%=GetLabel("Total Pasien") %>
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
                                        <%=GetLabel("Proposed Oleh")%>
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
                                        <%=GetLabel("Proposed Pada")%>
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
                                        <%=GetLabel("Void Oleh") %>
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
                                        <%=GetLabel("Void Pada")%>
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
</asp:Content>
