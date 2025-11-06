<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="PatientManagementTransactionDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientManagementTransactionDetail" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnOperatingTheaterOrder" runat="server" style="display: none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Order Kamar Operasi")%></div>
    </li>
    <li id="btnClinicTransactionTestOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Informasi Order")%></div>
    </li>
    <li id="btnTransactionServiceOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Informasi Order")%></div>
    </li>
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
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
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
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
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnHSUImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHSULaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnHSUOperatingTheaterID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnDefaultTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="0" id="hdnIsDischarges" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasAIOPackage" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
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
        function getIsAIOTransaction() {
            return $('#<%=hdnIsCheckedAIOTransaction.ClientID %>').val();
        }
        function getchkIsAIOTransaction() {
            return $('#<%=chkIsAIOTransaction.ClientID %>').is(":checked");
        }
        function onAfterSaveAddRecordEntryPopup(param) {
            if (param != '') {
                var transactionID = getTransactionHdID();
                if (transactionID == '' || transactionID == '0') {
                    onAfterAddRecordAddRowCount();
                }
                onLoadObject(param);
            }
        }

        function onAfterSaveRecordDtSuccess(transactionID) {
            var hdnTransactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            if (hdnTransactionID == '0' || hdnTransactionID == '') {
                $('#<%=hdnTransactionHdID.ClientID %>').val(transactionID);
                var filterExpression = 'TransactionID = ' + transactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    $('#<%=hdnGCTransactionStatus.ClientID %>').val(result.GCTransactionStatus);
                    $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);
                });

                setServiceItemFilterExpression($('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(), transactionID);
                setDrugMSItemFilterExpression(transactionID);
                setLogisticItemFilterExpression(transactionID);

                onAfterCustomSaveSuccess();
            }
            setRightPanelButtonEnabled();
        }

        function setRightPanelButtonEnabled() {
            var transactionHdID = parseInt($('#<%=hdnTransactionHdID.ClientID %>').val());
            var isBloodBankOrder = $('#<%=hdnIsBloodBankOrder.ClientID %>').val();
            if (transactionHdID != null && transactionHdID != "0" && transactionHdID != "") {
                $('#btnModality').removeAttr('enabled');
                $('#btnInputSerialNo').removeAttr('enabled');
                $('#btnInputFixedAsset').removeAttr('enabled');
            } else {
                $('#btnInputSerialNo').attr('enabled', 'false');
                $('#btnInputFixedAsset').attr('enabled', 'false');
                $('#btnModality').attr('enabled', 'false');
            }

            if (isBloodBankOrder == "1") {
                $('#btnBloodBankOrder').removeAttr('enabled');
                $('#btnEditBloodBankDetailOrder').removeAttr('enabled');
            }
            else {
                $('#btnBloodBankOrder').attr('enabled', 'false');
                $('#btnEditBloodBankDetailOrder').attr('enabled', 'false');
            }

        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            var testOrderID = parseInt($('#<%=hdnTestOrderID.ClientID %>').val());
            var serviceOrderID = parseInt($('#<%=hdnServiceOrderID.ClientID %>').val());
            var transactionHdID = parseInt($('#<%=hdnTransactionHdID.ClientID %>').val());

            if (testOrderID > 0) {
                $('#<%=btnClinicTransactionTestOrder.ClientID %>').show();
            }
            else {
                $('#<%=btnClinicTransactionTestOrder.ClientID %>').hide();
            }

            if (serviceOrderID > 0) {
                $('#<%=btnTransactionServiceOrder.ClientID %>').show();
            }
            else {
                $('#<%=btnTransactionServiceOrder.ClientID %>').hide();
            }

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
                //                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/PatientChargesDiagnosticVoidCtl.ascx');
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

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'diagnosticSupportNotes') {
                return $('#<%:hdnRegistrationID.ClientID %>').val() + '|' + $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            }
            else if (code == 'keteranganButaWarna' || code == 'keteranganSehat' || code == 'keteranganSehat1') {
                return $('#<%:hdnVisitID.ClientID %>').val();
            }
            else if (code == 'bloodBankOrder' || code == 'editBloodBankDetailOrder') {
                return $('#<%:hdnVisitID.ClientID %>').val() + '|' + $('#<%:hdnTestOrderID.ClientID %>').val();
            }
            else if (code == 'sendOrderToRIS' || code == 'sendOrderToOIS' || code == 'changeDetailRemarks' || code == 'inputSerialNumber' || code == 'inputFixedAsset' || code == "printLabelCover" || code == "printLabelCoverEditable") {
                return $('#<%:hdnTransactionHdID.ClientID %>').val();
            }
            else if (code == 'transactionNotes' || code == 'patientBirth' || code == 'paramedicTeam') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'dataPemeriksaanPasien') {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VitalSign/VitalSign.aspx?id=DIAGNOSTIC');
                window.location.href = url;
            }
            else if (code == 'dataPemeriksaanPasienRawatJalan') {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/VitalSign/VitalSign.aspx?id=OUTPATIENT|dp');
                window.location.href = url;
            }
            else if (code == 'orderFarmasi') {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/TransactionPageMedicationOrder.aspx?id=md');
                window.location.href = url;
            }
            else if (code == 'modality') {
                return $('#<%:hdnTransactionHdID.ClientID %>').val() + "|" + $('#<%:hdnRegistrationID.ClientID %>').val() + "|" + $('#<%=txtTransactionNo.ClientID %>').val() + "|" + $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            }
            else if (code == 'surgeryRoomOrderInfo') {
                return $('#<%:hdnVisitID.ClientID %>').val() + "|" + $('#<%:hdnTestOrderID.ClientID %>').val();
            }
            else if (code == 'keteranganSehat1') {
                return $('#<%:hdnVisitID.ClientID %>').val();
            }
            else if (code == 'patientVisaNo') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else if (code == 'changeParamedic') {
                return $('#<%:hdnTransactionHdID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            }
        }

        $(function () {
            $('#<%=btnOperatingTheaterOrder.ClientID %>').click(function () {
                var TestOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TestOrder/OperatingTheaterTestOrderDtCtl.ascx");
                openUserControlPopup(url, TestOrderID, 'Order Pelayanan Kamar Operasi', 1200, 600);
            });

            $('#imgVisitNote.imgLink').click(function () {
                var id = $('#<%=hdnRegistrationID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
                openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
            });

            $('#<%=btnClinicTransactionTestOrder.ClientID %>').click(function () {
                var isOT = $('#<%=hdnIsOperatingRoomOrderProcedure.ClientID %>').val();
                var isBloodBankOrder = $('#<%=hdnIsBloodBankOrder.ClientID %>').val();

                if (isOT == "1") {
                    $('#<%=btnOperatingTheaterOrder.ClientID %>').click();
                }
                else {
                    var TestOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TestOrder/TestOrderDtCtl.ascx");
                    openUserControlPopup(url, TestOrderID, 'Order Pelayanan Detail', 800, 600);
                }
            });

            $('#<%=btnTransactionServiceOrder.ClientID %>').click(function () {
                var ServiceOrderID = $('#<%=hdnServiceOrderID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/ServiceOrderTrans/ServiceOrderDetail/ServiceOrderDtCtl.ascx");
                openUserControlPopup(url, ServiceOrderID, 'Order Detail', 800, 600);
            });


            var param = $('#<%=hdnParam.ClientID %>').val();
            var orderStatus = $('#<%=hdnOrderGCTransactionStatus.ClientID %>').val();
            if (param == 'to') {
                if (orderStatus != Constant.TransactionStatus.PROCESSED) {
                    var isOT = $('#<%=hdnIsOperatingRoomOrderProcedure.ClientID %>').val();

                    if (isOT == "1") {
                        $('#<%=btnOperatingTheaterOrder.ClientID %>').click();
                    } else {
                        $('#<%=btnClinicTransactionTestOrder.ClientID %>').click();
                    }
                }
            }
            else if (param == 'soop' || param == 'soer') {
                $('#<%=btnTransactionServiceOrder.ClientID %>').click();
            }
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

            $('#<%=btnClinicTransactionBack.ClientID %>').click(function () {
                showLoadingPanel();
                if (getDepartmentID() == 'DIAGNOSTIC') {
                    document.location = ResolveUrl('~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDOrder/MDOrderList.aspx');
                }
                else {
                    document.location = document.referrer;
                }
            });

            //#region Transaction No
            $('#lblTransactionNo.lblLink').click(function () {
                var filterExpression = "<%:GetFilterExpression() %>";
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
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDate.ClientID %>').val());
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
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var isAllowPrint = $('#<%=hdnIsAllowPrint.ClientID %>').val();
            var testOrderID = $('#<%=hdnDefaultTestOrderID.ClientID %>').val();
            var isPregnant = $('#<%=hdnIsPregnant.ClientID %>').val();
            if (code == 'PM-00136') {
                if (registrationID == '' || registrationID == '0') {
                    errMessage.text = 'Mohon refresh dulu.';
                    return false;
                } else {
                    filterExpression.text = registrationID;
                    return true;
                }
            }
            else if (code == 'PM-00137') {
                if (MRN == '' || MRN == '0') {
                    errMessage.text = 'Mohon refresh dulu.';
                    return false;
                } else {
                    filterExpression.text = MRN;
                    return true;
                }
            }
            else if (code == 'PM-00138' || code == "PM-00189" || code == 'PM-00509' || code == 'PM-00589' || code == 'PM-00725' || code == 'PM-00726') {
                filterExpression.text = transactionID;
                return true;
            }
            else if (code == 'LB-00002' || code == 'LB-00040') {
                filterExpression.text = transactionID + '|' + testOrderID;
                return true;
            }
            else if (code == 'PM-00358' || code == 'PM-00361' || code == 'PM-00363' || code == 'PM-00364'
                    || code == 'PM-00427' || code == 'PM-00432' || code == 'PM-00579') {
                filterExpression.text = transactionID;
                return true;
            } else if (code == 'PM-00614') {
                if (isPregnant != "0") {
                    filterExpression.text = visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien Tidak Melahirkan!';
                    return false;
                }
            }
            if (isAllowPrint == "1") {
                if (transactionID == '' || transactionID == '0') {
                    errMessage.text = 'Simpan Transaksi Terlebih Dahulu!';
                    return false;
                }
                else {
                    filterExpression.text = transactionID;
                    return true;
                }
            }
            else {
                errMessage.text = 'Transaksi belum di-proposed!';
                return false;
            }
        }

    </script>
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnOrderGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnServiceOrderID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutomaticallySendToRIS" runat="server" />
    <input type="hidden" value="0" id="hdnIsOperatingRoomOrderProcedure" runat="server" />
    <input type="hidden" value="0" id="hdnIsBloodBankOrder" runat="server" />
    <input type="hidden" value="1" id="hdnIsAllowPrint" runat="server" />
    <input type="hidden" value="" id="hdnIsPregnant" runat="server" />
    <input type="hidden" value="" id="hdnProcedureGroupID" runat="server" />
    <input type="hidden" value="" id="hdnIsCheckedAIOTransaction" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
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
                                            <div style="position: relative;">
                                                <label class="lblLink lblKey" id="lblTransactionNo">
                                                    <%=GetLabel("No. Transaksi")%></label></div>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
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
                                <label class="lblNormal" runat="server">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="150px" runat="server" />
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
                        <%=GetLabel("Total Instansi") %>
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
                        <%=GetLabel("Total Pasien") %>
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
