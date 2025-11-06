<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MealItemEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Nutrition.Program.MealItemEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=hdnGCItemUnit.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtItemAmount.ClientID %>').val('1');
        $('#<%=txtNotes.ClientID %>').val('');
        cboItemUnit.SetValue('');

        $('#<%=hdnIsAdd.ClientID %>').val('1');
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
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
        $('#<%=txtItemAmount.ClientID %>').val(entity.FoodQuantity);
        $('#<%=txtNotes.ClientID %>').val(entity.AmountPerServingLabel);
        $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
        cboItemUnit.SetValue(entity.GCItemUnit);
        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    //#region Item
    function onGetItemFilterExpression() {
        var filterExpression = "<%:OnGetItemFilterExpression() %>";
        return filterExpression;
    }

    $('#lblItem.lblLink').click(function () {
        openSearchDialog('item', onGetItemFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });


    $('#<%=txtItemCode.ClientID %>').change(function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = onGetItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                cboItemUnit.SetValue(result.GCItemUnit);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                cboItemUnit.SetValue('');
            }
        });
    }
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnMealID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:120px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kode Menu")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtMealCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Menu")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtMealName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah Porsi")%></label></td>
                        <td colspan="5"><asp:TextBox ID="txtStandardPortion" ReadOnly="true" CssClass="number required" runat="server" Width="60px" /> <%=GetLabel("porsi")%></td>
                    </tr>
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <input type="hidden" runat="server" id="hdnIsAdd" />
                        <input type="hidden" runat="server" id="hdnID" />
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:160px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblItem"><%=GetLabel("Bahan Makanan")%></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnItemID" />
                                    <table style="width:60%" cellpadding="0" cellspacing="0">
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
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah")%></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnGCItemUnit" />
                                    <table style="width:60%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox runat="server" ID="txtItemAmount" CssClass="number" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboItemUnit" Width="130px" runat="server" ClientInstanceName = "cboItemUnit" OnCallback="cboItemUnit_Callback">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblLabel"><%=GetLabel("Keterangan")%></label></td>
                                <td ><asp:TextBox ID="txtNotes" Width="60%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" class="detailEntrySaveCancelButton" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" class="detailEntrySaveCancelButton" id="btnEntryPopupCancel"
                                                        value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                               <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" /></td>
                                                        <td style="width:3px">&nbsp;</td>
                                                        <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                <input type="hidden" value="<%#:Eval("FoodQuantity") %>" bindingfield="FoodQuantity" />
                                                <input type="hidden" value="<%#:Eval("AmountPerServingLabel") %>" bindingfield="AmountPerServingLabel" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Bahan" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />   
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Bahan" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                        <asp:BoundField DataField="FoodQuantity" HeaderText="Jumlah" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Unit" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="AmountPerServingLabel" HeaderText="Catatan" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
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
</div>

