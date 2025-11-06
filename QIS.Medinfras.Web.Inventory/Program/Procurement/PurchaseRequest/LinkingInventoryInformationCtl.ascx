<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkingInventoryInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.LinkingInventoryInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    setDatePicker('<%=txtValueDateFrom.ClientID %>');

    setDatePicker('<%=txtValueDateTo.ClientID %>');

    $('#<%=txtValueDateFrom.ClientID %>').change(function () {
        cbpPopupView.PerformCallback('refresh');
    });

    $('#<%=txtValueDateTo.ClientID %>').change(function () {
        cbpPopupView.PerformCallback('refresh');
    });

    function oncboItemTypeCallBackChanged() {
        cbpPopupView.PerformCallback('refresh');
    }

    //#region Item
    function getItemFilterExpression() {
        var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";
        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        openSearchDialog('item', getItemFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').die('change');
    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = getItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
            } else {
                $('#<%=hdnItemID.ClientID %>').val(0);
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
            }
        });
        cbpPopupView.PerformCallback('refresh');
    }
    //#endregion
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnBusinessPartnerID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />
<div class="pageTitle">
    <%=GetLabel("Detail Information")%></div>
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width: 70%">
                <colgroup>
                    <col style="width: 160px" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <label>
                            <%=GetLabel("Tanggal Transaksi") %></label>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtValueDateFrom" CssClass="txtValueDateFrom datepicker" runat="server"
                                        Width="120px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValueDateTo" CssClass="txtValueDateTo datepicker" runat="server"
                                        Width="120px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="lblNormal" id="lblItemType">
                            <%=GetLabel("Tipe Item") %></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox runat="server" ID="cboItemType" ClientInstanceName="cboItemType"
                            Width="200px">
                            <ClientSideEvents ValueChanged="function(s,e){ oncboItemTypeCallBackChanged(); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink lblMandatory" id="lblItem">
                            <%=GetLabel("Item")%></label>
                    </td>
                    <td colspan="2">
                        <input type="hidden" value="" id="hdnItemID" runat="server" />
                        <table cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 120px" />
                                <col style="width: 3px" />
                                <col style="width: 250px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 330px;
                                overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="PurchaseRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderText="Informasi Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                (<%#:Eval("ItemCode")%>) -
                                                <%#:Eval("ItemName")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurchaseRequestNo" ItemStyle-HorizontalAlign="Left" HeaderText="No. Permintaan"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                        <asp:TemplateField HeaderText="Qty Permintaan" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#:Eval("QtyPR", "{0:N}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurchaseOrderNo" ItemStyle-HorizontalAlign="Left" HeaderText="No. Pemesanan"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                        <asp:TemplateField HeaderText="Qty Pemesanan" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#:Eval("QtyPO", "{0:N}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurchaseReceiveNo" ItemStyle-HorizontalAlign="Left" HeaderText="No. Penerimaan"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                        <asp:TemplateField HeaderText="Qty Penerimaan" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#:Eval("QtyPOR", "{0:N}")%>
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
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
