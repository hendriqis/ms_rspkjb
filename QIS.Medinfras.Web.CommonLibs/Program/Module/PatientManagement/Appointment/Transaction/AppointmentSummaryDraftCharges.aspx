<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx4.Master"
    AutoEventWireup="true" CodeBehind="AppointmentSummaryDraftCharges.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentSummaryDraftCharges" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/Appointment/Transaction/AppointmentDraftTransactionDetailServiceCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Appointment/Transaction/AppointmentDraftTransactionDetailDrugMSCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Appointment/Transaction/AppointmentDraftTransactionDetailLogisticCtl.ascx"
    TagName="LogisticCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/Appointment/Transaction/AppointmentBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
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
    <input type="hidden" value="" id="hdnGCAppointmentStatus" runat="server" />
    <input type="hidden" value="" id="hdnVisitDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocationID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLogisticLocationID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitAppointmentID" runat="server" />
    <input type="hidden" value="" id="hdnHSUImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHSULaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="0" id="hdnIsDischarges" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function getTransactionHdID() {
            return $('#<%=hdnTransactionHdID.ClientID %>').val();
        }
        function getIsHealthcareServiceUnitHasParamedic() {
            return ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.Value %>').val() == '1');
        }
        function onGetPhysicianFilterExpression() {
            var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
            return filterExpression;
        }
        function getBusinessPartnerID() {
            return $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        }
        function getClassID() {
            return $('#<%=hdnClassID.ClientID %>').val();
        }
        function getAppointmentID() {
            return $('#<%=hdnAppointmentID.ClientID %>').val();
        }
        function getAppointmentID() {
            return $('#<%=hdnAppointmentID.ClientID %>').val();
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

        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = getTransactionHdID();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
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
            }
        }

        function fillServiceUnit() {
            var filterExpression = 'AppointmentID = ' + $('#<%=hdnAppointmentID.ClientID %>').val();
            Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                setServiceItemFilterExpression(result.HealthcareServiceUnitID);
            });
        }

        var lastContentID = '';
        function onLoad() {
            setCustomToolbarVisibility();

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

            if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT")
                fillServiceUnit();

            if (lastContentID != '')
                $('#ulTabClinicTransaction li[contentid=' + lastContentID + ']').click();

            if ($('#<%=txtTransactionDate.ClientID %>').attr('readonly') == null)
                setDatePicker('<%=txtTransactionDate.ClientID %>');


            //#region Transaction No
            function onGetTransactionNoFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblDraftTransactionNo.lblLink').click(function () {
                openSearchDialog('draftpatientchargeshd', onGetTransactionNoFilterExpression(), function (value) {
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
                    if ($('#<%:hdnGCAppointmentStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
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

            var logisticTotalPatient = getLogisticTotalPatient();
            var logisticTotalPayer = getLogisticTotalPayer();

            var totalPayer = (serviceTotalPayer + drugMSTotalPayer + logisticTotalPayer);
            var totalPatient = (serviceTotalPatient + drugMSTotalPatient + logisticTotalPatient);

            $('#<%=txtTotalPayer.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=txtTotalPatient.ClientID %>').val(totalPatient).trigger('changeValue');
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCAppointmentStatus.ClientID%>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Simpan Transaksi Terlebih Dahulu!';
                return false;
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
    <input type="hidden" value="" id="hdnHealthcareID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
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
                                <div style="position: relative">
                                    <label class="lblLink lblKey" id="lblDraftTransactionNo">
                                        <%=GetLabel("No. Draft Transaksi")%></label></div>
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
</asp:Content>
