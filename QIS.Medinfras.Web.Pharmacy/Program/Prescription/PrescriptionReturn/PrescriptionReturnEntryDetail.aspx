<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="PrescriptionReturnEntryDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionReturnEntryDetail" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnPrescriptionBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
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
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClinicTransactionTestOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Order Detail")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnChargeClassID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocationID" runat="server" />
    <input type="hidden" value="0" id="hdnIsDischarges" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnClinicTransactionTestOrder.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/Prescription/PrescriptionReturnOrder/PrescriptionReturnOrderNotesCtl.ascx");
                var prescriptionReturnOrderID = $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val();
                if (prescriptionReturnOrderID == '') prescriptionReturnOrderID = $('#<%=hdnDefaultPrescriptionReturnOrderID.ClientID %>').val();
                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                var imagingServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var laboratoryServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var filterExpression = prescriptionReturnOrderID + '|' + departmentID + '|' + imagingServiceUnitID + '|' + laboratoryServiceUnitID + '|' + transactionID;
                openUserControlPopup(url, filterExpression, 'Prescription Return Order Notes', 1000, 500);
            });

            if ($('#<%=btnClinicTransactionTestOrder.ClientID %>').is(':visible')) {
                if ($('#<%=hdnTransactionID.ClientID %>').val() == '') {
                    $('#<%=btnClinicTransactionTestOrder.ClientID %>').click();
                }
            }
        });

        function onLoad() {
            setCustomToolbarVisibility();
            $('#<%=btnPrescriptionBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl("~/Program/Prescription/PrescriptionReturn/PrescriptionReturnEntryList.aspx");
            });

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1')
                $('#lblAddData').show();
            else
                $('#lblAddData').hide();

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
                $('#<%=hdnTransactionID.ClientID %>').val('');
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=txtItemPrice.ClientID %>').val('');
                $('#<%=hdnDiscountAmount.ClientID %>').val('');
                cboChargeClass.SetValue('');

                $('#containerEntry').hide();
            })

            //#region Transaction No
            $('#lblPrescriptionNo.lblLink').click(function () {
                var filterExpression = "<%:GetFilterExpression() %>";
                openSearchDialog('patientchargeshd', filterExpression, function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtPrescriptionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtPrescriptionNoChanged($(this).val());
            });

            function onTxtPrescriptionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion
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
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();
            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if (transactionStatus == Constant.TransactionStatus.OPEN) {
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
                //                var url = ResolveUrl('~/Program/Prescription/PrescriptionReturn/PrescriptionReturnVoidCtl.ascx');
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
            onRefreshControl();
        }

        function onAfterSaveRecordDtSuccess(TransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                cboLocation.SetEnabled(false);
                $('#<%=hdnTransactionID.ClientID %>').val(TransactionID);
                var filterExpression = 'TransactionID = ' + TransactionID;
                Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "ItemID IN (SELECT ItemID FROM ItemBalance WHERE " + "LocationID = " + LocationID + " AND IsDeleted = 0" + ")";
            return filterExpression;
        }

        //#region Operasi
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var url = ResolveUrl("~/Program/Prescription/PrescriptionReturn/PrescriptionReturnEntryCtl.ascx");
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                var healthcareServiceUnitID = $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val();
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var chargeClassID = $('#<%=hdnChargeClassID.ClientID %>').val();
                var physicianID = $('#<%=hdnPhysicianID.ClientID %>').val();
                var transactionDate = $('#<%=txtPrescriptionDate.ClientID %>').val();
                var transactionTime = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var guestID = $('#<%=hdnGuestID.ClientID %>').val();

                var queryString = visitID + '|' + locationID + '|' + transactionID + '|' + departmentID + '|' + healthcareServiceUnitID + '|' +
                        registrationID + '|' + chargeClassID + '|' + physicianID + '|' + transactionDate + '|' + transactionTime + '|' + cboReturnType.GetValue() + '|' + mrn + '|' + guestID;
                openUserControlPopup(url, queryString, 'Prescription Return', 1000, 600);
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);

            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtReturnQty.ClientID %>').val(entity.ItemQty);
            $('#<%=txtReturnItemUnit.ClientID %>').val(entity.ItemUnit);
            cboChargeClass.SetValue(entity.ChargeClassID);
            $('#<%=txtItemPrice.ClientID %>').val(entity.Tariff).trigger('changeValue');
            $('#<%=txtDiscountAmount.ClientID %>').val(entity.DiscountAmount).trigger('changeValue');
            $('#<%=txtPatientAmount.ClientID %>').val(entity.PatientAmount).trigger('changeValue');
            $('#<%=txtPayerAmount.ClientID %>').val(entity.PayerAmount).trigger('changeValue');
            $('#<%=txtLineAmount.ClientID %>').val(entity.LineAmount).trigger('changeValue');
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=hdnTransactionID.ClientID %>').val(entity.TransactionID);
            $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionReturnOrderDtID);

            $('#containerEntry').show();
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            showDeleteConfirmation(function (data) {
                var obj = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(obj.PrescriptionReturnOrderDtID);
                $('#<%=hdnTransactionID.ClientID %>').val(obj.TransactionID);
                $('#<%=hdnItemID.ClientID %>').val(obj.ItemID);
                $('#<%=hdnChargesDetailIDNew.ClientID %>').val(obj.ID);
                var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
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
                    var transactionID = s.cpTransactionID;
                    onAfterSaveRecordDtSuccess(transactionID);
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
            if ($('#<%=txtTransactionNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var prescriptionID = $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            if (prescriptionID == '' || prescriptionID == '0') {
                errMessage.text = 'Harap transaksi retur resep diselesaikan terlebih dahulu';
                return false;
            }
            else {
                if (code == 'PM-00201' || code == 'PM-00236' || code == 'PM-00239' || code == 'PM-00413' || code == 'PM-002361' || code == 'PM-002362' || code == 'PM-90027') {
                    filterExpression.text = transactionID;
                    return true;
                }
                else {
                    filterExpression.text = "PrescriptionReturnOrderID = " + prescriptionID;
                    return true;
                }
            }
        }
    </script>
    <input type="hidden" value="" id="hdnSignaID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnChargesDetailIDNew" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionDetailID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionReturnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLocationID" runat="server" />
    <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCDosingUnit" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
    <input type="hidden" value="" id="hdnDefaultPrescriptionReturnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnCoverageAmount" runat="server" />
    <input type="hidden" value="0" id="hdnDiscountAmount" runat="server" />
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
    <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnGuestID" runat="server" />
    <div style="height: 435px; overflow-y: auto; overflow-x: hidden;">
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
                                    <%=GetLabel("No. Transaksi")%></label>
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
                                <label class="lblNormal">
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                    Width="300px" />
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
                                    <%=GetLabel("Alasan Pengembalian")%></label>
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
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="Table1" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 70px">
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <%=GetLabel("Nama Obat") %>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("DIKEMBALIKAN") %>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("NILAI") %>
                                                        </th>
                                                    </tr>
                                                    <tr>
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
                                                        <td colspan="7">
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
                                                        <th rowspan="2" align="left">
                                                            <%=GetLabel("Nama Obat") %>
                                                        </th>
                                                        <th colspan="2">
                                                            <%=GetLabel("DIKEMBALIKAN") %>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("NILAI") %>
                                                        </th>
                                                    </tr>
                                                    <tr>
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
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <img class="imgEdit <%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        &nbsp;
                                                        <img class="imgDelete <%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                            src='<%# IsEditable() == "0" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                        <input type="hidden" value="<%#: Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#: Eval("PrescriptionReturnOrderDtID") %>" bindingfield="PrescriptionReturnOrderDtID" />
                                                        <input type="hidden" value="<%#: Eval("TransactionID") %>" bindingfield="TransactionID" />
                                                        <input type="hidden" value="<%#: Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#: Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                        <input type="hidden" value="<%#: Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#: Eval("ChargedQuantity") %>" bindingfield="ItemQty" />
                                                        <input type="hidden" value="<%#: Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                        <input type="hidden" value="<%#: Eval("ChargeClassID") %>" bindingfield="ChargeClassID" />
                                                        <input type="hidden" value="<%#: Eval("Tariff") %>" bindingfield="Tariff" />
                                                        <input type="hidden" value="<%#: Eval("DiscountAmount") %>" bindingfield="DiscountAmount" />
                                                        <input type="hidden" value="<%#: Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                                        <input type="hidden" value="<%#: Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                                        <input type="hidden" value="<%#: Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                    </td>
                                                    <td align="left">
                                                        <div style="<%# Eval("GCDrugClass").ToString() == "X123^O" ? "color: Red": Eval("GCDrugClass").ToString() == "X123^P" ? "color: Blue" : "color: Black"%>">
                                                            <%#: Eval("ItemName1")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("ChargedQuantity") %>
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
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div style="text-align: center">
                                            <span class="lblLink" id="lblAddData" style="text-align: center;">
                                                <%= GetLabel("Tambah Obat Retur")%></span>
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
