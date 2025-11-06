<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogisticsReturnQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.LogisticsReturnQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_logisticsreturnquickpicksctl">
    function onCbpViewLogisticReturnEndCallback(s) {
        hideLoadingPanel();
    }

    $('#<%=rblFilterItemType.ClientID %>').live('change', function () {
        cbpViewLogisticReturn.PerformCallback('refresh');
    });

    $('#<%=grdView.ClientID %> .chkIsSelectedLogisticsReturn input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelectedLogisticsReturn input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked'))
            $tr.find('.txtQtyLogisticsReturn').removeAttr('readonly');
        else
            $tr.find('.txtQtyLogisticsReturn').attr('readonly', 'readonly');
    });

    function onBeforeSaveRecord(errMessage) {
        errMessage.text = '';
        if (IsValid(null, 'fsLogisticsReturn', 'mpLogisticsReturn')) {
            getCheckedMember(errMessage);
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                if (errMessage.text != '') {
                    return false;
                } else {
                    var isQtyChecked = "0";
                    var qtyBeginList = $('#<%=hdnSelectedMemberQty.ClientID %>').val().split(",");

                    var i = 0;
                    while (i < qtyBeginList.length) {
                        var qtyBegin = qtyBeginList[i];
                        if (qtyBegin.includes(".")) {
                            var qtyCheckDesimalList = qtyBegin.split(".");
                            var qtyCheckDesimal = qtyCheckDesimalList[1];
                            if (qtyCheckDesimal.length > 2) {
                                isQtyChecked = "1";
                                break;
                            }
                        }

                        i++;
                    }

                    if (isQtyChecked == "1") {
                        errMessage.text = 'Maksimal digit desimal belakang koma adalah 2 digit.';
                        return false;
                    } else {
                        return true;
                    }
                }
            }
            else {
                errMessage.text = 'Harap pilih item terlebih dahulu.';
                return false;
            }
            return false;
        }
    }

    function onCboChargeClassValueChanged(s) {
        $tr = $(s.GetInputElement()).closest('tr').parent().closest('tr');
        $tr.find('.divChargeClassID').html(s.GetValue());
    }

    function getCheckedMember(errMessage) {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberChargeClassID = [];
        var result = '';
        $('#<%=grdView.ClientID %> .chkIsSelectedLogisticsReturn input:checked').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var qty = parseFloat($tr.find('.txtQtyLogisticsReturn').val());
            var usedQty = parseFloat($tr.find('.txtUsedQuantity').val());
            var chargeClassID = $tr.find('.divChargeClassID').html();
            var itemName1 = $tr.find('.txtItemName1').val();
            if (qty > usedQty)
                errMessage.text += 'Jumlah <b>' + itemName1 + '</b> Yang Dikembalikan Tidak Boleh Lebih Dari Jumlah Yang Digunakan<br>';
            lstSelectedMember.push(key);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberChargeClassID.push(chargeClassID);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberChargeClassID.ClientID %>').val(lstSelectedMemberChargeClassID.join(','));
    }
</script>
<input type="hidden" runat="server" id="hdnVisitID" />
<input type="hidden" runat="server" id="hdnLocationID" />
<input type="hidden" runat="server" id="hdnTransactionID" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberChargeClassID" runat="server" value="" />
<input type="hidden" id="hdnDepartmentID" runat="server" value="" />
<input type="hidden" id="hdnIsAccompany" runat="server" value="" />
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnLinkedRegistrationID" runat="server" value="" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<input type="hidden" runat="server" id="hdnParam" />
<table>
    <colgroup>
        <col style="width: 100px" />
        <col style="width: 400px" />
    </colgroup>
    <tr>
        <td class="tdLabel">
            <label>
                <%=GetLabel("Lokasi")%></label>
        </td>
        <td>
            <dxe:ASPxComboBox ID="cboLogisticReturnLocation" ClientInstanceName="cboLogisticReturnLocation"
                Width="100%" runat="server" OnCallback="cboLogisticReturnLocation_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }"
                    ValueChanged="function(s,e) { cbpViewLogisticReturn.PerformCallback(); }" />
            </dxe:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <td class="tdLabel">
            <label>
                <%=GetLabel("Filter")%></label>
        </td>
        <td colspan="3">
            <asp:RadioButtonList runat="server" ID="rblFilterItemType" RepeatDirection="Horizontal">
                <asp:ListItem Text="Barang Umum" Value="logistic" Selected="True" />
                <asp:ListItem Text="Bahan Makanan" Value="nutrition" />
            </asp:RadioButtonList>
        </td>
    </tr>
</table>
<div style="height: 350px; overflow-y: auto">
    <fieldset id="fsLogisticsReturn">
        <dxcp:ASPxCallbackPanel ID="cbpViewLogisticReturn" runat="server" Width="100%" ClientInstanceName="cbpViewLogisticReturn"
            ShowLoadingPanel="false" OnCallback="cbpViewLogisticReturn_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewLogisticReturnEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelectedLogisticsReturn" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemName1" HeaderText="Barang Umum" />
                                <asp:TemplateField HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div style="display: none" class="divChargeClassID" runat="server" id="divChargeClassID">
                                        </div>
                                        <dxe:ASPxComboBox ID="cboChargeClass" runat="server" Width="150px">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboChargeClassValueChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                    HeaderText="Jumlah">
                                    <ItemTemplate>
                                        <input type="hidden" value='<%#:Eval("UsedQuantity")%>' class="txtUsedQuantity" />
                                        <input type="hidden" value='<%#:Eval("ItemName1")%>' class="txtItemName1" />
                                        <%#:Eval("UsedQuantity")%>
                                        <%#: Eval("ItemUnit") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right" HeaderText="Dikembalikan">
                                    <ItemTemplate>
                                        <input type="text" validationgroup="mpLogisticsReturn" class="txtQtyLogisticsReturn number min"
                                            min="0.1" value="0" readonly="readonly" style="width: 60px" />
                                        <%#: Eval("ItemUnit") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
