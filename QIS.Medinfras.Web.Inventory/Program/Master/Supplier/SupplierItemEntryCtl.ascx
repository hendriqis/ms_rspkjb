<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierItemEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inventory.Program.SupplierItemEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtSupplierItemCode.ClientID %>').val('');
        $('#<%=txtSupplierItemName.ClientID %>').val('');
        $('#<%=txtLeadTime.ClientID %>').val('0');
        $('#<%=txtPrice.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtDiscountInPercentage.ClientID %>').val('0');    
        
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
        $('#<%=txtSupplierItemCode.ClientID %>').val(entity.SupplierItemCode);
        $('#<%=txtSupplierItemName.ClientID %>').val(entity.SupplierItemName);
        $('#<%=txtLeadTime.ClientID %>').val(entity.LeadTime);
        $('#<%=txtPrice.ClientID %>').val(entity.Price).trigger('changeValue');
        $('#<%=txtDiscountInPercentage.ClientID %>').val(entity.DiscountPercentage);

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#lblEntryPopupAddData').click();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }

    //#region Item
    $('#lblItem.lblLink').live('click', function () {
        var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        var filterExpression = "<%:filterExpressionItemProduct %>";
        filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM SupplierItem WHERE BusinessPartnerID = " + businessPartnerID + " AND IsDeleted = 0)";
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtCustomerItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtCustomerItemCodeChanged($(this).val());
    });

    function onTxtCustomerItemCodeChanged(value) {
        var filterExpression = "ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=txtSupplierItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtSupplierItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtSupplierItemCode.ClientID %>').val('');
                $('#<%=txtSupplierItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion    
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Item")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Supplier")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblItem"><%=GetLabel("Item")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Supplier Item Code")%></label></td>
                                <td><asp:TextBox ID="txtSupplierItemCode" runat="server" Width="200px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Supplier Item Name")%></label></td>
                                <td><asp:TextBox ID="txtSupplierItemName" runat="server" Width="100%" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Lead Time")%></label></td>
                                <td><asp:TextBox ID="txtLeadTime" CssClass="number required" runat="server" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Price")%></label></td>
                                <td><asp:TextBox ID="txtPrice" CssClass="txtCurrency required" runat="server" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Discount Percentage")%></label></td>
                                <td><asp:TextBox ID="txtDiscountInPercentage" CssClass="number required" runat="server" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("SupplierItemCode") %>" bindingfield="SupplierItemCode" />
                                                <input type="hidden" value="<%#:Eval("SupplierItemName") %>" bindingfield="SupplierItemName" />
                                                <input type="hidden" value="<%#:Eval("LeadTime") %>" bindingfield="LeadTime" />
                                                <input type="hidden" value="<%#:Eval("Price") %>" bindingfield="Price" />
                                                <input type="hidden" value="<%#:Eval("DiscountPercentage") %>" bindingfield="DiscountPercentage" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" ItemStyle-CssClass="tdItemName" HeaderText="Item Name" />
                                        <asp:BoundField DataField="SupplierItemCode" ItemStyle-CssClass="tdSupplierItemCode" HeaderText="Supplier Item Code" />
                                        <asp:BoundField DataField="SupplierItemName" ItemStyle-CssClass="tdSupplierItemName" HeaderText="Supplier Item Name" />
                                        <asp:BoundField DataField="LeadTime" ItemStyle-CssClass="tdLeadTime" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderText="Lead Time" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

