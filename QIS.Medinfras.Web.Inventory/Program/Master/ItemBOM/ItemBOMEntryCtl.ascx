<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemBOMEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemBOMEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnEntryID.ClientID %>').val('');
        $('#<%=hdnItemBOMID.ClientID %>').val('');
        $('#<%=txtItemBOMCode.ClientID %>').val('');
        $('#<%=txtItemBOMName.ClientID %>').val('');
        $('#<%=txtItemQuantity.ClientID %>').val('1');
        $('#<%=txtBOMQuantity.ClientID %>').val('0');
        $('#<%=txtSequenceNo.ClientID %>').val('0');
        $('#<%=txtItemBOMName.ClientID %>').val('');
        $('#<%=txtItemUnit.ClientID %>').val('');
        $('#<%=txtItemBOMCode.ClientID %>').removeAttr('disabled');
        $('#lblItemBOM').addClass('lblLink');

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
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemBOMID.ClientID %>').val(entity.BillOfMaterialID);
        $('#<%=txtItemBOMCode.ClientID %>').val(entity.BillOfMaterialCode);
        $('#<%=txtItemBOMName.ClientID %>').val(entity.BillOfMaterialName1);
        $('#<%=txtItemQuantity.ClientID %>').val(entity.ItemQuantity);
        $('#<%=txtBOMQuantity.ClientID %>').val(entity.BOMQuantity);
        $('#<%=txtSequenceNo.ClientID %>').val(entity.SequenceNo);
        $('#<%=txtFormulaItemBOMName.ClientID %>').val(entity.BillOfMaterialName1);
        $('#<%=txtItemUnit.ClientID %>').val(entity.BillOfMaterialItemUnit);
        $('#<%=txtItemBOMCode.ClientID %>').attr('disabled', 'disabled');
        $('#lblItemBOM').removeClass('lblLink');
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
    function onGetLocationItemProductFilterExpression() {
        var filterExpression = "<%:OnGetItemProductFilterExpression() %>";
        return filterExpression;
    }

    $('#lblItemBOM.lblLink').click(function () {
        var isAddMode = ($('#<%=hdnIsAdd.ClientID %>').val() == '1');
        if (isAddMode) {
            openSearchDialog('item', onGetLocationItemProductFilterExpression(), function (value) {
                $('#<%=txtItemBOMCode.ClientID %>').val(value);
                onTxtItemBOMCodeChanged(value);
            });
        }
    });

    $('#<%=txtItemBOMCode.ClientID %>').change(function () {
        onTxtItemBOMCodeChanged($(this).val());
    });

    function onTxtItemBOMCodeChanged(value) {
        var filterExpression = onGetLocationItemProductFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemBOMID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemBOMName.ClientID %>').val(result.ItemName1);
                $('#<%=txtFormulaItemBOMName.ClientID %>').val(result.ItemName1);
                $('#<%=txtItemUnit.ClientID %>').val(result.ItemUnit); 
            }
            else {
                $('#<%=hdnItemBOMID.ClientID %>').val('');
                $('#<%=txtItemBOMCode.ClientID %>').val('');
                $('#<%=txtItemBOMName.ClientID %>').val('');
                $('#<%=txtFormulaItemBOMName.ClientID %>').val('');
                $('#<%=txtItemUnit.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <input type="hidden" id="hdnEntryID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Barang BOM")%></div>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kode Barang")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Brang")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <input type="hidden" runat="server" id="hdnIsAdd" />
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col style="width:40px"/>
                                <col style="width:160px"/>
                                <col style="width:20px"/>
                                <col style="width:40px"/>
                                <col style="width:160px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblItemBOM"><%=GetLabel("Barang BOM")%></label></td>
                                <td colspan="5">
                                    <input type="hidden" runat="server" id="hdnItemBOMID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemBOMCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemBOMName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nomor Urutan")%></label></td>
                                <td colspan="5"><asp:TextBox ID="txtSequenceNo" CssClass="number required" runat="server" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Formula")%></label></td>
                                <td><asp:TextBox ID="txtItemQuantity" CssClass="number required min" min="0" runat="server" Width="100%" /></td>
                                <td><asp:TextBox ID="txtFormulaItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                <td align="center">=</td>
                                <td><asp:TextBox ID="txtBOMQuantity" CssClass="number required min" min="0" runat="server" Width="100%" /></td>
                                <td><asp:TextBox ID="txtFormulaItemBOMName" ReadOnly="true" Width="100%" runat="server" /></td>
                                <td><asp:TextBox ID="txtItemUnit" ReadOnly="true" Width="30%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="6">
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
                                                <input type="hidden" value="<%#:Eval("BillOfMaterialID") %>" bindingfield="BillOfMaterialID" />
                                                <input type="hidden" value="<%#:Eval("BillOfMaterialCode") %>" bindingfield="BillOfMaterialCode" />
                                                <input type="hidden" value="<%#:Eval("BillOfMaterialName1") %>" bindingfield="BillOfMaterialName1" />
                                                <input type="hidden" value="<%#:Eval("SequenceNo") %>" bindingfield="SequenceNo" />
                                                <input type="hidden" value="<%#:Eval("ItemQuantity") %>" bindingfield="ItemQuantity" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("BOMQuantity") %>" bindingfield="BOMQuantity" />
                                                <input type="hidden" value="<%#:Eval("BillOfMaterialItemUnit") %>" bindingfield="BillOfMaterialItemUnit" />
                                            </ItemTemplate>

                                            <HeaderStyle Width="70px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BillOfMaterialCode" HeaderText="Kode Barang" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" >
                                        <HeaderStyle Width="120px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BillOfMaterialName1" HeaderText="Nama Barang" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" >
                                        <HeaderStyle Width="140px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SequenceNo" HeaderText="Nomor Urutan" 
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" >
                                        <HeaderStyle Width="80px"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Formula">
                                            <ItemTemplate>
                                                <b><%#:Eval("ItemQuantity") %></b> <%#:Eval("ItemName1") %> = <b><%#:Eval("BOMQuantity") %></b> <%#:Eval("BillOfMaterialName1") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
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
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

