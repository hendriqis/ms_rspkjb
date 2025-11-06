<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TransactionPageMedicationReturnOrder.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageMedicationReturnOrder" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
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
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onAfterSaveAddRecordEntryPopup(param) {
            var PrescriptionReturnOrderID = $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val();
            if (PrescriptionReturnOrderID == '' || PrescriptionReturnOrderID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onGetDrugFilterExpression() {
            var LocationID = cboLocation.GetValue();
            var filterExpression = "ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + LocationID + " AND IsDeleted = 0)";
            return filterExpression;
        }

        function onLoad() {
            setCustomToolbarVisibility();

            setDatePicker('<%=txtPrescriptionDate.ClientID %>');

            //#region Transaction No
            $('#lblPrescriptionReturnOrderNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('prescriptionreturnorderhd', filterExpression, function (value) {
                    $('#<%=txtPrescriptionReturnOrderNo.ClientID %>').val(value);
                    onTxtPrescriptionReturnOrderNoChanged(value);
                });
            });

            $('#<%=txtPrescriptionReturnOrderNo.ClientID %>').change(function () {
                onTxtPrescriptionReturnOrderNoChanged($(this).val());
            });

            function onTxtPrescriptionReturnOrderNoChanged(value) {
                $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val(value);
                onLoadObject(value);
            }
            //#endregion

            //#region Physician
            $('#<%=lblPhysician.ClientID %>.lblLink').live('click', function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPhysicianCodeChanged(value);
                });
            });

            $('#<%=txtPhysicianCode.ClientID %>').change(function () {
                onTxtPhysicianCodeChanged($(this).val());
            });

            function onTxtPhysicianCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Operasi
            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            });
            //#endregion
        }

        $('#lblAddData').die('click');
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/MedicationOrder/PrescriptionReturnOrderEntryCtl.ascx");
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var locationID = cboLocation.GetValue();

                var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                var healthcareServiceUnitID = cboDispensaryUnit.GetValue();
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();

                var physicianID = $('#<%=hdnPhysicianID.ClientID %>').val();
                var transactionDate = $('#<%=txtPrescriptionDate.ClientID %>').val();
                var transactionTime = $('#<%=txtPrescriptionTime.ClientID %>').val();
                var transactionID = $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val();
                var returnType = cboReturnType.GetValue();
                //disini
                var queryString = visitID + '|' + locationID + '|' + departmentID + '|' + healthcareServiceUnitID + '|' +
                        registrationID + '|' + physicianID + '|' + transactionDate + '|' + transactionTime + '|' + returnType + '|' + transactionID;
                openUserControlPopup(url, queryString, 'Pilih obat yang diretur', 1000, 600);
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtReturnQty.ClientID %>').val(entity.ItemQty);
            $('#<%=txtReturnItemUnit.ClientID %>').val(entity.ItemUnit);
            $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionReturnOrderDtID);

            $('#containerEntry').show();
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionReturnOrderDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=txtPrescriptionReturnOrderNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(PrescriptionReturnOrderID) {
            if ($('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val() == '0') {
                cboDispensaryUnit.SetEnabled(false);
                cboLocation.SetEnabled(false);
                $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPrescriptionTime.ClientID %>').attr('readonly', 'readonly');
                $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val(PrescriptionReturnOrderID);
                var filterExpression = 'PrescriptionReturnOrderID = ' + PrescriptionReturnOrderID;
                Methods.getObject('GetPrescriptionReturnOrderHdList', filterExpression, function (result) {
                    $('#<%=txtPrescriptionReturnOrderNo.ClientID %>').val(result.PrescriptionReturnOrderNo).trigger('change');
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    onAfterSaveRecordDtSuccess('0');
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
                //                var transactionID = $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val();
                //                var id = registrationID + '|' + transactionID;
                //                openUserControlPopup(url, id, 'Void Transaction', 400, 230);
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var PrescriptionReturnOrderID = $('#<%=hdnPrescriptionReturnOrderID.ClientID %>').val();
            var TransactionStatus = $('#<%:hdnGCTransactionStatus.ClientID %>').val();
            if (PrescriptionReturnOrderID != null && PrescriptionReturnOrderID != "" && PrescriptionReturnOrderID != "0") {
                if (code == 'PM-00530' || code == 'PM-00541' || code == 'PM-00558') {
                    if (TransactionStatus == Constant.TransactionStatus.OPEN) {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    } else {
                        filterExpression.text = 'PrescriptionReturnOrderID = ' + PrescriptionReturnOrderID;
                        return true;
                    }
                }
                else {
                    errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                    return false;
                }
            }
            else {
                errMessage.text = 'Pilih nomor order terlebih dahulu.';
                return false;
            }
        }

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

        $('#<%=txtReturnQty.ClientID %>').live('change', function () {
            var value = $(this).val();

            value = value.replace(',', '');
            var newchar = '';
            value = value.split(',').join(newchar);
            if (isNaN(value)) {
                value = 0;
            }
            else {
                if (value > 0) {
                    value = 0;
                }
            }

            $(this).val(value).trigger('changeValue');
        });
    </script>
    <input type="hidden" value="" id="hdnPrescriptionReturnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
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
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPrescriptionReturnOrderNo">
                                    <%=GetLabel("No. Order Retur")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrescriptionReturnOrderNo" Width="150px" ReadOnly="true" runat="server" />
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
                                    <%=GetLabel("Farmasi") %></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDispensaryUnit" ClientInstanceName="cboDispensaryUnit" runat="server"
                                    Width="234px">
                                    <ClientSideEvents ValueChanged="function() { cboLocation.PerformCallback(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Location")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                    Width="300px" OnCallback="cboLocation_Callback">
                                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter")%></label>
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
                                                    <input type="text" runat="server" class="number" id="txtReturnQty" />
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
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PrescriptionReturnOrderDtID" HeaderStyle-CssClass="keyField"
                                                    ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
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
                                                        <input type="hidden" value="<%#:Eval("PrescriptionReturnOrderDtID") %>" bindingfield="PrescriptionReturnOrderDtID" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                        <input type="hidden" value="<%#:Eval("ItemQty") %>" bindingfield="ItemQty" />
                                                        <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div>
                                                            <%=GetLabel("Nama Obat / Alkes")%></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#: Eval("ItemName1") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                    <HeaderTemplate>
                                                        <div>
                                                            <%=GetLabel("Jumlah")%></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#:Eval("ItemQty") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div>
                                                            <%=GetLabel("Satuan")%></div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#: Eval("ItemUnit") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada transaksi retur/pengembalian obat ke farmasi")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <div style="width: 100%; text-align: center">
                                            <span class="lblLink" id="lblAddData" style="text-align: center; <%=IsEditable.ToString() == "False" ? "display:none": "" %>">
                                                <%= GetLabel("Tambah Obat Retur")%></span>
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
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
