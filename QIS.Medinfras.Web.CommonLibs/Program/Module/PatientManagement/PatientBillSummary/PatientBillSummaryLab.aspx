<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryLab.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryLab" %>

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

        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = getTransactionHdID();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
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

        function onLoad() {
            setCustomToolbarVisibility();

            $('#ulTabClinicTransaction li').click(function () {
                $('#ulTabClinicTransaction li.selected').removeAttr('class');
                $('.containerTransDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
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

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientChargesTestVoidCtl.ascx');
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
                var id = registrationID + '|' + transactionID;
                openUserControlPopup(url, id, 'Void Transaction', 400, 230);
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Simpan Transaksi Terlebih Dahulu!';
                return false;
            }
            else {
                filterExpression.text = transactionID;
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAdminCanCancelAllTransaction" runat="server" />
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
                                    <label class="lblLink" id="lblTransactionNo">
                                        <%=GetLabel("No. Transaksi")%></label></div>
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
</asp:Content>
