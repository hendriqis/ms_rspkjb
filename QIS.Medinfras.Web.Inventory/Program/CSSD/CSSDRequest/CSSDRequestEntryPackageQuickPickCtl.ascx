<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CSSDRequestEntryPackageQuickPickCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDRequestEntryPackageQuickPickCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_cssdpackagequickpicksctl">
    $(function () {
        hideLoadingPanel();
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
            getCheckedMember();
            return true;
        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        var lstSelectedMemberIsConsumption = $('#<%=hdnSelectedMemberIsConsumption.ClientID %>').val().split(',');
        var lstSelectedMemberBaseQty = $('#<%=hdnSelectedMemberBaseQty.ClientID %>').val().split(',');
        var lstSelectedMemberQty = $('#<%=hdnSelectedMemberQty.ClientID %>').val().split(',');
        var lstSelectedMemberItemUnit = $('#<%=hdnSelectedMemberItemUnit.ClientID %>').val().split(',');

        $('#<%=grdView.ClientID %> .keyField').each(function () {
            var key = $(this).closest('tr').find('.keyField').html();
            var isConsumption = $(this).closest('tr').find('.cfIsConsumption').html();
            var baseQty = $(this).closest('tr').find('.Quantity').html();
            var qty = $(this).closest('tr').find('.txtQuantity').val();
            var itemUnit = $(this).closest('tr').find('.GCItemUnit').val();

            var idx = lstSelectedMember.indexOf(key);
            if (idx < 0) {
                if (key != "&nbsp;" && key != "") {
                    lstSelectedMember.push(key);
                    lstSelectedMemberIsConsumption.push(isConsumption);
                    lstSelectedMemberBaseQty.push(baseQty);
                    lstSelectedMemberQty.push(qty);
                    lstSelectedMemberItemUnit.push(itemUnit);
                }
            }
        });

        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberIsConsumption.ClientID %>').val(lstSelectedMemberIsConsumption.join(','));
        $('#<%=hdnSelectedMemberBaseQty.ClientID %>').val(lstSelectedMemberBaseQty.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberItemUnit.ClientID %>').val(lstSelectedMemberItemUnit.join(','));
    }
</script>
<div style="padding: 5px;">
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsConsumption" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberBaseQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberItemUnit" runat="server" value="" />
    <input type="hidden" id="hdnRequestIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnLocationIDFromCtl" runat="server" value="" />
    <input type="hidden" id="hdnLocationHealthcareUnitFromCtl" runat="server" />
    <input type="hidden" id="hdnLocationIDToCtl" runat="server" value="" />
    <input type="hidden" id="hdnPackageIDCtl" runat="server" />
    <table style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" />
                                        <asp:BoundField DataField="cfIsConsumption" HeaderStyle-Width="100px" HeaderText="Is Consumption"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfIsConsumption" />
                                        <asp:BoundField DataField="Quantity" HeaderStyle-Width="60px" HeaderText="Quantity" ItemStyle-CssClass="Quantity"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Selected") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" id="GCItemUnit" class="GCItemUnit" value='<%#:Eval("GCItemUnit")%>' runat="server" />
                                                <asp:TextBox ID="txtQuantity" Text='<%#:Eval("Quantity")%>' CssClass="number txtQuantity" runat="server" Width="100px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemUnit" HeaderStyle-Width="80px" HeaderText="Item Unit" ItemStyle-CssClass="ItemUnit" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada data persediaan")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
