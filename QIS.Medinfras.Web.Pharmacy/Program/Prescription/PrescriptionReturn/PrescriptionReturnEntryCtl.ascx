<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionReturnEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionReturnEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_prescriptionreturnentryctl">
    function onCbpViewDrugMSReturnEndCallback(s) {
        hideLoadingPanel();
    }

    //#region Registration No
    function getRegistrationNoFilterExpression() {
        var filterExpression = "<%:OnGetRegistrationNoFilterExpression() %>";
        return filterExpression;
    }

    $('#lblNoReg.lblLink').live('click', function () {
        openSearchDialog('registration', getRegistrationNoFilterExpression(), function (value) {
            $('#<%:txtRegistrationNo.ClientID %>').val(value);
            onTxtRegistrationNoChanged(value);
        });
    });
    $('#<%:txtRegistrationNo.ClientID %>').live('change', function () {
        onTxtRegistrationNoChanged($(this).val());
    });
    function onTxtRegistrationNoChanged(value) {
        var filterExpression = getRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
        Methods.getObject('GetvRegistration3List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPreviousRegistrationID.ClientID %>').val(result.RegistrationID);
                $('#<%=hdnPreviousVisitID.ClientID %>').val(result.VisitID);
            }
            else {
                $('#<%=hdnPreviousRegistrationID.ClientID %>').val('');
                $('#<%=hdnPreviousVisitID.ClientID %>').val('');
                $('#<%:txtRegistrationNo.ClientID %>').val('');
            }
            cbpViewDrugMSReturn.PerformCallback('refresh');
        });
    }
    //#endregion

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedDrugsReturn input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    $('#<%=grdView.ClientID %> .chkIsSelectedDrugsReturn input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelectedDrugsReturn input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked'))
            $tr.find('.txtQtyDrugsReturn').removeAttr('readonly');
        else
            $tr.find('.txtQtyDrugsReturn').attr('readonly', 'readonly');
    });

    function onBeforeSaveRecord(errMessage) {
        errMessage.text = '';
        if (IsValid(null, 'fsDrugsReturn', 'mpDrugsReturn')) {
            getCheckedMember(errMessage);
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                if (errMessage.text != '')
                    return false;
                return true;
            }
            else {
                errMessage.text = 'Please Select Item First';
                return false;
            }
            return false;
        }
    }

    function getCheckedMember(errMessage) {
        var lstSelectedMember = [];
        var lstSelectedMemberItemID = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberChargeClassID = [];
        var lstSelectedMemberGCItemUnit = [];
        var result = '';
        $('#<%=grdView.ClientID %> .chkIsSelectedDrugsReturn input:checked').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var itemID = parseFloat($tr.find('.hdnItemID').val());
            var qty = parseFloat($tr.find('.txtQtyDrugsReturn').val());
            var usedQty = parseFloat($tr.find('.txtUsedQuantity').val());
            var chargeClassID = "0";
            var itemName1 = $tr.find('.txtItemName1').val();
            var GCItemUnit = $tr.find('.hdnGCItemUnit').val();
            if (qty > usedQty)
                errMessage.text += 'Jumlah <b>' + itemName1 + '</b> Yang Dikembalikan Tidak Boleh Lebih Dari Jumlah Yang Digunakan<br>';
            lstSelectedMember.push(key);
            lstSelectedMemberItemID.push(itemID);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberChargeClassID.push(chargeClassID);
            lstSelectedMemberGCItemUnit.push(GCItemUnit);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberItemID.ClientID %>').val(lstSelectedMemberItemID.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberChargeClassID.ClientID %>').val(lstSelectedMemberChargeClassID.join(','));
        $('#<%=hdnSelectedMemberGCItemUnit.ClientID %>').val(lstSelectedMemberGCItemUnit.join(','));
    }
</script>
<input type="hidden" runat="server" id="hdnVisitID" />
<input type="hidden" runat="server" id="hdnLocationID" />
<input type="hidden" runat="server" id="hdnTransactionID" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberItemID" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberChargeClassID" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberGCItemUnit" runat="server" value="" />
<input type="hidden" id="hdnDepartmentID" runat="server" value="" />
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnPreviousRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnPreviousVisitID" runat="server" value="" />
<input type="hidden" id="hdnMRN" runat="server" value="" />
<input type="hidden" id="hdnGuestID" runat="server" value="" />
<input type="hidden" id="hdnLinkedRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnChargeClassID" runat="server" value="" />
<input type="hidden" id="hdnPhysicianID" runat="server" value="" />
<input type="hidden" id="hdnTransactionDate" runat="server" value="" />
<input type="hidden" id="hdnTransactionTime" runat="server" value="" />
<input type="hidden" id="hdnReturnType" runat="server" value="" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<input type="hidden" runat="server" id="hdnParam" />
<div style="height: 450px; overflow-y: auto">
    <div style="height: 400px; overflow-y: auto">
        <fieldset id="fsDrugsReturn">
            <table border="0" cellpadding="0" cellspacing="0" style="padding-bottom: 10px" id="tblRegistrationNo"
                runat="server">
                <colgroup>
                    <col style="width: 150px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink" id="lblNoReg">
                            <%:GetLabel("No. Registrasi")%></label>
                    </td>
                    <td style="padding-left: 60px">
                        <asp:TextBox ID="txtRegistrationNo" Width="200px" runat="server" ReadOnly="true" />
                    </td>
                </tr>
            </table>
            <dxcp:ASPxCallbackPanel ID="cbpViewDrugMSReturn" runat="server" Width="100%" ClientInstanceName="cbpViewDrugMSReturn"
                ShowLoadingPanel="false" OnCallback="cbpViewDrugMSReturn_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewDrugMSReturnEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <input id="chkSelectAll" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="hidden" value='<%#:Eval("ItemID")%>' class="hdnItemID" />
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelectedDrugsReturn" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TransactionNo" HeaderText="Transaction No" />
                                    <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Transaction Date" />
                                    <asp:BoundField DataField="ItemName1" HeaderText="Obat/Alkes" />
                                    <%--<asp:TemplateField HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div style="display:none" class="divChargeClassID" runat="server" id="divChargeClassID"></div>
                                            <dxe:ASPxComboBox ID="cboChargeClass" runat="server" Width="150px">
                                                <ClientSideEvents ValueChanged="function(s,e){ onCboChargeClassValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Jumlah">
                                        <ItemTemplate>
                                            <input type="hidden" value='<%#:Eval("RemainingChargesQty")%>' class="txtUsedQuantity" />
                                            <input type="hidden" value='<%#:Eval("ItemName1")%>' class="txtItemName1" />
                                            <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                            <%#:Eval("RemainingChargesQty")%>
                                            <%#:Eval("ItemUnit") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        HeaderText="Dikembalikan">
                                        <ItemTemplate>
                                            <input type="text" validationgroup="mpDrugsReturn" class="txtQtyDrugsReturn number min"
                                                min="0.1" value="0" readonly="readonly" style="width: 60px" />
                                            <%#:Eval("ItemUnit") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Tidak ada transaksi resep untuk pasien ini.")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </fieldset>
    </div>
</div>
