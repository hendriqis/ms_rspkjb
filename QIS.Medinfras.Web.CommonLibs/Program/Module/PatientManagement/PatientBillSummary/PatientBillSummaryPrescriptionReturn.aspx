<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx3.Master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryPrescriptionReturn.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPrescriptionReturn" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
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
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnChargeClassID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocationID" runat="server" />
    <input type="hidden" value="0" id="hdnIsDischarges" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblAddData').show();
            }
            else {
                $('#lblAddData').hide();
            }

            setCustomToolbarVisibility();

            //#region Transaction No
            function onGetFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblPrescriptionNo.lblLink').click(function () {
                openSearchDialog('patientchargeshd', onGetFilterExpression(), function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtPrescriptionOrderNoChanged(value);
                });
            });

            function onTxtPrescriptionOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            $('#<%=txtPayerAmount.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                var payerAmount = parseFloat($('#<%=txtPayerAmount.ClientID %>').attr('hiddenVal'));
                var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));

                if (lineAmount * -1 > payerAmount * -1)
                    $('#<%=txtPatientAmount.ClientID %>').val(lineAmount - payerAmount).trigger('changeValue');
                else
                    $('#<%=txtPatientAmount.ClientID %>').val(lineAmount).trigger('changeValue');
            });

            $('#<%=txtPatientAmount.ClientID %>').change(function () {
                $(this).trigger('changeValue');
                var patientAmount = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
                var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));

                if (lineAmount * -1 > patientAmount * -1)
                    $('#<%=txtPayerAmount.ClientID %>').val(lineAmount - patientAmount).trigger('changeValue');
                else
                    $('#<%=txtPayerAmount.ClientID %>').val(lineAmount).trigger('changeValue');

            });

            $('#btnSave').click(function () {
                if (IsValid(null, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            })

            $('#btnCancel').click(function () {
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtReturnQty.ClientID %>').val('');
                $('#<%=txtReturnItemUnit.ClientID %>').val('');
                $('#<%=txtDiscountAmount.ClientID %>').val('')
                $('#<%=txtPatientAmount.ClientID %>').val('');
                $('#<%=txtPayerAmount.ClientID %>').val('');
                $('#<%=txtLineAmount.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=txtItemPrice.ClientID %>').val('');
                $('#<%=txtDiscountAmount.ClientID %>').val('');
                cboChargeClass.SetValue('');

                $('#containerEntry').hide();
            })
        }

        $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
            var discount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val());
            $('#<%=hdnDiscountAmount.ClientID %>').val(discount);

            calculateAllTotal();
        });

        function calculateAllTotal() {
            var qty = $('#<%=txtReturnQty.ClientID %>').val();
            var itemPrice = parseFloat($('#<%=txtItemPrice.ClientID %>').attr('hiddenVal'));
            var subTotal = qty * itemPrice;
            var patientAmount = parseFloat($('#<%=txtPatientAmount.ClientID %>').attr('hiddenVal'));
            var payerAmount = parseFloat($('#<%=txtPayerAmount.ClientID %>').attr('hiddenVal'));
            var lineAmount = parseFloat($('#<%=txtLineAmount.ClientID %>').attr('hiddenVal'));
            var discount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').val());

            if (payerAmount == subTotal) {
                payerAmount = subTotal + discount
                patientAmount = 0;
            } else {
                payerAmount = payerAmount;
                patientAmount = subTotal - payerAmount + discount;
            }

            lineAmount = payerAmount + patientAmount;

            $('#<%=txtPatientAmount.ClientID %>').val(patientAmount).trigger('changeValue');
            $('#<%=txtPayerAmount.ClientID %>').val(payerAmount).trigger('changeValue');
            $('#<%=txtLineAmount.ClientID %>').val(lineAmount).trigger('changeValue');
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
                //                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientChargesPrescriptionReturnVoidCtl.ascx');
                //                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                //                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
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

        function onAfterSaveRecordDtSuccess(TransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                $('#<%=hdnTransactionID.ClientID %>').val(TransactionID);
                var filterExpression = 'TransactionID = ' + TransactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val(result.PrescriptionReturnOrderID);
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    cboDispensaryServiceUnitID.SetEnabled(false);
                    cboReturnType.SetEnabled(false);
                    cbpView.PerformCallback('refresh');
                });
                getStatusPerRegOutstanding(); //general ctl
                onAfterCustomSaveSuccess();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            var tempParam = param.split('|');
            onAfterSaveRecordDtSuccess(tempParam[0]);
            setCustomToolbarVisibility();
            onLoadObject(tempParam[1]);
        }

        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "ItemID IN (SELECT ItemID FROM ItemBalance WHERE " + "LocationID = " + LocationID + " AND IsDeleted = 0" + ")";
            return filterExpression;
        }

        //#region Operasi
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryPrescriptionReturnEntryCtl.ascx");
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                var healthcareServiceUnitID = cboDispensaryServiceUnitID.GetValue();
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                var physicianID = $('#<%=hdnPhysicianID.ClientID %>').val();
                var transactionDate = $('#<%=txtPrescriptionDate.ClientID %>').val();
                var transactionTime = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var isCorrectionTransaction = $("#<%=chkIsCorrectionTransaction.ClientID%>").is(':checked') ? '1' : '0';

                var queryString = visitID + '|' + locationID + '|' + transactionID + '|' + departmentID + '|' + healthcareServiceUnitID + '|' +
                        registrationID + '|' + chargeClassID + '|' + physicianID + '|' + transactionDate + '|' + transactionTime + '|' + cboReturnType.GetValue() + '|' + isCorrectionTransaction;
                openUserControlPopup(url, queryString, 'Prescription Return', 1000, 600);
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);

            $('#<%=txtLocation.ClientID %>').val(entity.LocationName);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtReturnQty.ClientID %>').val(entity.ItemQty);
            $('#<%=txtReturnItemUnit.ClientID %>').val(entity.ItemUnit);
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
            $('#<%=txtPatientAmount.ClientID %>').val(entity.PatientAmount).trigger('changeValue');
            $('#<%=txtPayerAmount.ClientID %>').val(entity.PayerAmount).trigger('changeValue');
            $('#<%=txtLineAmount.ClientID %>').val(entity.LineAmount).trigger('changeValue');
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionReturnOrderDtID);

            var filterExpression = "IsDeleted = 0 AND TransactionID = " + entity.TransactionID;
            Methods.getObject('GetvPatientChargesDtList', filterExpression, function (result) {
                $('#<%=txtItemPrice.ClientID %>').val(result.Tariff).trigger('changeValue');
                $('#<%=txtDiscountAmount.ClientID %>').val(result.DiscountAmount).trigger('changeValue');
                cboChargeClass.SetValue(result.ChargeClassID);
            });

            $('#containerEntry').show();
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionReturnOrderDtID);
            showDeleteConfirmation(function (data) {
                var param = 'delete|' + entity.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
                cbpProcess.PerformCallback(param);
            });
        });
        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'switch') {
                if (param[1] == 'fail')
                    showToast('Switch Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'sendBARNotificationMessage') {
                if ($('#<%:hdnTransactionID.ClientID %>').val() != '' || $('#<%:hdnTransactionID.ClientID %>').val() != '0') {
                    var messageType = "01";
                    var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
                    var referenceID = $('#<%:hdnTransactionID.ClientID %>').val();
                    var param = messageType + "|" + registrationID + "|" + referenceID;
                    return param;
                }
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
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

        function onCboDispensaryServiceUnitIDValueChanged() {
            var filterExpression = "HealthcareServiceUnitID = " + cboDispensaryServiceUnitID.GetValue();
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
            });
        }

        $('.imgSwitch.imgLink').die('click');
        $('.imgSwitch.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            cbpProcess.PerformCallback('switch|' + obj.ID);
        });
    </script>
    <input type="hidden" value="" id="hdnDefaultReturnType" runat="server" />
    <input type="hidden" value="" id="hdnSignaID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnVisitDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionDetailID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionReturnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCDosingUnit" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
    <input type="hidden" value="" id="hdnCoverageAmount" runat="server" />
    <input type="hidden" value="" id="hdnDiscountAmount" runat="server" />
    <input type="hidden" value="" id="hdnDiscountInPercentage" runat="server" />
    <input type="hidden" value="" id="hdnCoverageInPercentage" runat="server" />
    <input type="hidden" value="" id="hdnBaseTariff" runat="server" />
    <input type="hidden" value="" id="hdnBaseComp1" runat="server" />
    <input type="hidden" value="" id="hdnBaseComp2" runat="server" />
    <input type="hidden" value="" id="hdnBaseComp3" runat="server" />
    <input type="hidden" value="" id="hdnTariff" runat="server" />
    <input type="hidden" value="" id="hdnTariffComp1" runat="server" />
    <input type="hidden" value="" id="hdnTariffComp2" runat="server" />
    <input type="hidden" value="" id="hdnTariffComp3" runat="server" />
    <input type="hidden" value="" id="hdnTakenQty" runat="server" />
    <input type="hidden" value="" id="hdnTakenUnit" runat="server" />
    <input type="hidden" value="" id="hdnEmbalaceID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAdminCanCancelAllTransaction" runat="server" />
    <input type="hidden" value="" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
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
                            <label class="lblLink" id="lblPrescriptionNo">
                                <%=GetLabel("Transaction No")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal lblMandatory" />
                            <%=GetLabel("Tanggal") %>
                            -
                            <%=GetLabel("Jam") %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrescriptionTime" Width="80px" CssClass="time" runat="server"
                                            Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Farmasi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboDispensaryServiceUnitID" ClientInstanceName="cboDispensaryServiceUnitID"
                                Width="300px">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboDispensaryServiceUnitIDValueChanged() }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCorrectionTransaction" runat="server" /><%:GetLabel("Transaksi Koreksi")%>
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
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
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
                            <label class="lblMandatory">
                                <%=GetLabel("Return Type")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboReturnType" ClientInstanceName="cboReturnType" Width="100%"
                                runat="server">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            </dxe:ASPxComboBox>
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
                    <fieldset id="fsTrx" style="margin: 0">
                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                        <table class="tblEntryDetail">
                            <colgroup>
                                <col width="10px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Lokasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocation" Width="300px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <%=GetLabel("Item Name") %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemCode" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemName" Width="200px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Charge Class") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboChargeClass" ClientInstanceName="cboChargeClass" runat="server"
                                        ClientEnabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Dikembalikan") %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Quantity") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Satuan") %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" runat="server" class="number" id="txtReturnQty" readonly="readonly" />
                                            </td>
                                            <td>
                                                <input type="text" runat="server" id="txtReturnItemUnit" readonly="readonly" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Harga") %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Harga") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Diskon") %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" runat="server" class="txtCurrency" id="txtItemPrice" readonly="readonly" />
                                            </td>
                                            <td>
                                                <input type="text" runat="server" class="txtCurrency" id="txtDiscountAmount" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Total") %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Pasien") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Instansi") %></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("Total") %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="txtCurrency" ID="txtPatientAmount" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="txtCurrency" ID="txtPayerAmount" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="txtCurrency" ID="txtLineAmount" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="3">
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
                <input type="hidden" value="" id="hdnID" runat="server" />
                <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                <input type="hidden" id="hdnPrescriptionFlag" runat="server" value="" />
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="Table1" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th rowspan="2" style="width: 70px">
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("Item Name") %>
                                                    </th>
                                                    <th colspan="2">
                                                        <%=GetLabel("Diambil") %>
                                                    </th>
                                                    <th colspan="2">
                                                        <%=GetLabel("Dikembalikan") %>
                                                    </th>
                                                    <th colspan="3">
                                                        <%=GetLabel("Jumlah") %>
                                                    </th>
                                                    <th rowspan="2" style="padding: 3px; width: 40px;">
                                                        <div>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Instansi") %>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Pasien") %>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Jumlah") %>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Satuan") %>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Instansi") %>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Pasien") %>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Total") %>
                                                    </th>
                                                </tr>
                                                <tr align="center" style="height: 50px; vertical-align: middle;">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th rowspan="2" style="width: 70px">
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("Item Name") %>
                                                    </th>
                                                    <th colspan="2">
                                                        <%=GetLabel("Diambil") %>
                                                    </th>
                                                    <th colspan="2">
                                                        <%=GetLabel("Dikembalikan") %>
                                                    </th>
                                                    <th colspan="3">
                                                        <%=GetLabel("Jumlah") %>
                                                    </th>
                                                    <th rowspan="2" style="padding: 3px; width: 40px;">
                                                        <div>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Instansi") %>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Pasien") %>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Jumlah") %>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <%=GetLabel("Satuan") %>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Instansi") %>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Pasien") %>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Total") %>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr id="Tr1" class="trFooter" runat="server">
                                                    <td colspan="6" align="right" style="padding-right: 3px">
                                                        <%=GetLabel("Total") %>
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdTotalPayer" class="tdTotalPayer"
                                                        runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdTotalPatient" class="tdTotalPatient"
                                                        runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdTotal" class="tdTotal" runat="server">
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    &nbsp;
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#: Eval("PrescriptionReturnOrderDtID") %>" bindingfield="PrescriptionReturnOrderDtID" />
                                                    <input type="hidden" value="<%#: Eval("TransactionID") %>" bindingfield="TransactionID" />
                                                    <input type="hidden" value="<%#: Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#: Eval("LocationName") %>" bindingfield="LocationName" />
                                                    <input type="hidden" value="<%#: Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#: Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#: Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#: Eval("ItemQty") %>" bindingfield="ItemQty" />
                                                    <input type="hidden" value="<%#: Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    <input type="hidden" value="<%#: Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                                    <input type="hidden" value="<%#: Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                                    <input type="hidden" value="<%#: Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                </td>
                                                <td align="left">
                                                    <%#:Eval("ItemName1") %>
                                                </td>
                                                <td align="right" style="padding-right: 3px" id="tdPayerAmount" class="tdPayerAmount"
                                                    runat="server">
                                                </td>
                                                <td align="right" style="padding-right: 3px" id="tdPatientAmount" class="tdPatientAmount"
                                                    runat="server">
                                                </td>
                                                <td align="right">
                                                    <%#:Eval("ItemQty") %>
                                                </td>
                                                <td align="center">
                                                    <%#:Eval("ItemUnit") %>
                                                </td>
                                                <td align="right">
                                                    <%#:Eval("PayerAmount", "{0:N}") %>
                                                </td>
                                                <td align="right">
                                                    <%#:Eval("PatientAmount", "{0:N}") %>
                                                </td>
                                                <td align="right">
                                                    <%#:Eval("LineAmount", "{0:N}") %>
                                                </td>
                                                <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %> valign="middle">
                                                    <img style="margin-left: 2px" class="imgSwitch imgLink" title='<%=GetLabel("Switch")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <div style="width: 100%; text-align: center">
                                        <span class="lblLink" id="lblAddData" style="text-align: center">
                                            <%= GetLabel("Tambah Data")%></span>
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
