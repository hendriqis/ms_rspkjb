<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsignmentOrderAddFromReceiveCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentOrderAddFromReceiveCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ConsignmentOrderAddFromReceiveCtl">
    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
    }

    $('#<%=grdPopupView.ClientID %> #chkSelectAll').live('change', function () {
        var isChecked = $(this).is(':checked');

        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
        });
    })

    function onBeforeSaveRecord(errMessage) {
        var count = 0;
        var data = "";
        $('.chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                $row = $(this).closest('tr');
                var purchaseReceiveID = $row.find('.hdnPurchaseReceiveID').val();

                var gcPurchaseUnit = $row.find('.hdnGCPurchaseUnit').val();
                var gcBaseUnit = $row.find('.hdnGCBaseUnit').val();
                var conversionFactor = $row.find('.hdnConversionFactor').val();
                var unitPrice = $row.find('.hdnUnitPrice').val();

                var isDiscountInPercentage1 = $row.find('.hdnIsDiscountInPercentage1').val();
                var isDiscountInPercentage2 = $row.find('.hdnIsDiscountInPercentage2').val();
                var discountPercentage1 = $row.find('.hdnDiscountPercentage1').val();
                var discountPercentage2 = $row.find('.hdnDiscountPercentage2').val();
                var discountAmount1 = $row.find('.hdnDiscountAmount1').val();
                var discountAmount2 = $row.find('.hdnDiscountAmount2').val();

                $row = $row.closest('tr');
                var itemID = $row.find('.keyField').html();
                var quantity = $row.find('.txtQuantity').val();
                var pOQty = $row.find('.txtPOQty').val();
                var receivedQty = $row.find('.txtReceivedQty').val();
                var receiveQtyConf = receivedQty * conversionFactor;

                data += itemID + ";" + quantity + ";" + gcPurchaseUnit + ";" + gcBaseUnit + ";" + conversionFactor + ";" + unitPrice + ";" + isDiscountInPercentage1 + ";" + isDiscountInPercentage2 + ";" + discountPercentage1 + ";" + discountPercentage2 + ";" + discountAmount1 + ";" + discountAmount2 + ";" + pOQty + ";" + receiveQtyConf + "|";
                count += 1;
            }
            $('#<%=hdnLstItem.ClientID %>').val(data);
        });

        if (count == 0) {
            errMessage.text = 'Please Select Item First';
            return false;
        }

        getCheckedMember(errMessage);

        if (errMessage.text != '') {
            showToast('Save Failed', 'Error Message : <br/>' + errMessage.text);
        }
        else {
            return true;
        }
    }

    function getCheckedMember(errMessage) {
        var result = '';
        $('#<%=grdPopupView.ClientID %> .chkIsSelected input:checked').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var qtyPO = parseFloat($tr.find('.txtPOQty').val());
            var qtyReturn = parseFloat($tr.find('.txtReturnQty').val());
            var qtyReceive = parseFloat($tr.find('.txtReceivedQty').val());
            var qtyAllow = qtyReceive - qtyReturn - qtyPO;
            var qtyInput = parseFloat($tr.find('.txtQuantity').val());
            var itemName1 = $tr.find('.hdnItemName1').val();

            if (qtyInput > qtyAllow) {
                errMessage.text += 'Jumlah <b>' + itemName1 + '</b> yang diproses melebihi jumlah yang diperbolehkan, yaitu maksimal sejumlah <b>' + qtyAllow + '</b> <br/>';
            }
        });
    }

    //#region PurchaseReceiveHd
    function getPurchaseReceiveExpression() {
        var filterExpression = "<%=GetPurchaseReceiveExpression() %>";
        return filterExpression;
    }

    $('#lblReceiveNo.lblLink').click(function () {
        openSearchDialog('purchasereceivehdconsignmentcopy', getPurchaseReceiveExpression(), function (value) {
            $('#<%=txtPurchaseReceiveNo.ClientID %>').val(value);
            onTxtPurchaseReceiveNoChanged(value);
        });
    });

    $('#<%=txtPurchaseReceiveNo.ClientID %>').change(function () {
        onTxtPurchaseReceiveNoChanged($(this).val());
    });

    function onTxtPurchaseReceiveNoChanged(value) {
        var filterExpression = "PurchaseReceiveNo = '" + value + "'";
        Methods.getObject('GetvPurchaseReceiveConsignmentSearchList', filterExpression, function (result) {
            if (result != null)
                $('#<%=hdnPurchaseReceiveID.ClientID %>').val(result.PurchaseReceiveID);
            else
                $('#<%=hdnPurchaseReceiveID.ClientID %>').val('');
            cbpViewPopup.PerformCallback("refresh");
        });
    }
    //#endregion

</script>
<input type="hidden" runat="server" id="hdnLstItem" value="" />
<input type="hidden" runat="server" id="hdnIsUsedProductLineReceiveCtl" value="" />
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 160px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink" id="lblReceiveNo"><%=GetLabel("No. Penerimaan")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnPurchaseReceiveID" value="" runat="server" />
                        <asp:TextBox ID="txtPurchaseReceiveNo" Width="150px" ReadOnly="true" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.95em; max-height: 300px; overflow-y: scroll;">
                            <asp:GridView ID="grdPopupView" runat="server" CssClass="grdNormal" AutoGenerateColumns="false" 
                                ShowHeaderWhenEmpty="true" OnRowDataBound="grdPopupView_RowDataBound" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <input id="chkSelectAll" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            <input type="hidden" id="hdnItemName1" class="hdnItemName1" value='<%#: Eval("ItemName1")%>' />
                                            <input type="hidden" id="hdnPurchaseReceiveID" class="hdnPurchaseReceiveID" value='<%#: Eval("PurchaseReceiveID")%>' />
                                            <input type="hidden" id="hdnGCPurchaseUnit" class="hdnGCPurchaseUnit" value='<%#: Eval("GCPurchaseUnit")%>' />
                                            <input type="hidden" id="hdnGCBaseUnit" class="hdnGCBaseUnit" value='<%#: Eval("GCBaseUnit")%>' />
                                            <input type="hidden" id="hdnConversionFactor" class="hdnConversionFactor" value='<%#: Eval("ConversionFactor")%>' />
                                            <input type="hidden" id="hdnUnitPrice" class="hdnUnitPrice" value='<%#: Eval("UnitPrice")%>' />
                                            <input type="hidden" id="hdnIsDiscountInPercentage1" class="hdnIsDiscountInPercentage1" value='<%#: Eval("IsDiscountInPercentage1")%>' />
                                            <input type="hidden" id="hdnIsDiscountInPercentage2" class="hdnIsDiscountInPercentage2" value='<%#: Eval("IsDiscountInPercentage2")%>' />
                                            <input type="hidden" id="hdnDiscountPercentage1" class="hdnDiscountPercentage1" value='<%#: Eval("DiscountPercentage1")%>' />
                                            <input type="hidden" id="hdnDiscountPercentage2" class="hdnDiscountPercentage2" value='<%#: Eval("DiscountPercentage2")%>' />
                                            <input type="hidden" id="hdnDiscountAmount1" class="hdnDiscountAmount1" value='<%#: Eval("DiscountAmount1")%>' />
                                            <input type="hidden" id="hdnDiscountAmount2" class="hdnDiscountAmount2" value='<%#: Eval("DiscountAmount2")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemName1" HeaderText="Item Name" HeaderStyle-Width="300px" />
                                    <asp:TemplateField HeaderText="Jumlah Terima" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtReceivedQty" CssClass="txtReceivedQty txtCurrency" Width="100%" ReadOnly="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jumlah Retur" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtReturnQty" CssClass="txtReturnQty txtCurrency" Width="100%" ReadOnly="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jumlah di PO" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtPOQty" CssClass="txtPOQty txtCurrency" Width="100%" ReadOnly="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jumlah" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" >
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtQuantity" CssClass="txtQuantity txtCurrency" Width="100%" />
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
        </td>
    </tr>    
</table>
