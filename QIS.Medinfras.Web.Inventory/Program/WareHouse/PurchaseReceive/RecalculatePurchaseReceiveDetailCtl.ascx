<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecalculatePurchaseReceiveDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.RecalculatePurchaseReceiveDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();

        $('#<%=hdnEntryID.ClientID %>').val("");
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    //#region edit
    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var ID = $row.find('.hdnID').val();
        var IsBonusItem = $row.find('.IsBonusItem').val();
        var PurchaseOrderNo = $row.find('.PurchaseOrderNo').val();
        var ItemGroupCode = $row.find('.ItemGroupCode').val();
        var ItemGroupName1 = $row.find('.ItemGroupName1').val();
        var ItemID = $row.find('.ItemID').val();
        var ItemCode = $row.find('.ItemCode').val();
        var ItemName1 = $row.find('.ItemName1').val();
        var OrderQuantity = $row.find('.OrderQuantity').val();
        var OrderPurchaseUnit = $row.find('.OrderPurchaseUnit').val();
        var Quantity = $row.find('.Quantity').val();
        var ItemUnit = $row.find('.ItemUnit').val();
        var SupplierItemCode = $row.find('.SupplierItemCode').val();
        var SupplierItemName = $row.find('.SupplierItemName').val();
        var UnitPrice = $row.find('.UnitPrice').val();
        var DiscountPercentage1 = $row.find('.DiscountPercentage1').val();
        var DiscountPercentage2 = $row.find('.DiscountPercentage2').val();

        $('#<%=hdnEntryID.ClientID %>').val(ID);
        $('#<%=chkIsBonus.ClientID %>').prop('checked', (IsBonusItem == 'True'));
        $('#<%=txtOrderNo.ClientID %>').val(PurchaseOrderNo);
        $('#<%=txtItemGroupCode.ClientID %>').val(ItemGroupCode);
        $('#<%=txtItemGroupName.ClientID %>').val(ItemGroupName1);
        $('#<%=hdnItemID.ClientID %>').val(ItemID);
        $('#<%=txtItemCode.ClientID %>').val(ItemCode);
        $('#<%=txtItemName.ClientID %>').val(ItemName1);
        $('#<%=txtOrderQty.ClientID %>').val(OrderQuantity);
        $('#<%=txtOrderUnit.ClientID %>').val(OrderPurchaseUnit);
        $('#<%=txtQuantity.ClientID %>').val(Quantity);
        $('#<%=txtQuantityUnit.ClientID %>').val(ItemUnit);
        $('#<%=txtSupplierItemCode.ClientID %>').val(SupplierItemCode);
        $('#<%=txtSupplierItemName.ClientID %>').val(SupplierItemName);
        $('#<%=txtPrice.ClientID %>').val(UnitPrice).trigger('changeValue');
        $('#<%=txtDiscount.ClientID %>').val(DiscountPercentage1).trigger('changeValue');
        $('#<%=txtDiscount2.ClientID %>').val(DiscountPercentage2).trigger('changeValue');

        $('#<%=txtDiscount.ClientID %>').change();
        calculateSubTotal();

        $('#containerPopupEntryData').show();
    });
    //#endregion

    $('#<%=txtDiscount.ClientID %>').change(function () {
        var discountPercentage = parseFloat($(this).val());
        var receivedItem = parseFloat($('#<%=txtQuantity.ClientID %>').val());
        var unitPrice = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));

        var discountAmount = (unitPrice) * discountPercentage / 100;
        $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');

        $('#<%=txtDiscount2.ClientID %>').change();
    });

    $('#<%=txtDiscountAmount.ClientID %>').change(function () {
        $(this).blur();
        var discountAmount = parseFloat($(this).attr('hiddenVal'));
        var receivedItem = parseFloat($('#<%=txtQuantity.ClientID %>').val());
        var unitPrice = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));

        var discountPercentage = (discountAmount * 100) / (unitPrice);
        $('#<%=txtDiscount.ClientID %>').val(discountPercentage);

        $('#<%=txtDiscount2.ClientID %>').change();
    });

    $('#<%=txtDiscount2.ClientID %>').change(function () {
        var discountPercentage = parseFloat($(this).val());
        var discountAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal'));
        var receivedItem = parseFloat($('#<%=txtQuantity.ClientID %>').val());
        var unitPrice = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));

        var discountAmount = (unitPrice - discountAmount1) * discountPercentage / 100;
        $('#<%=txtDiscountAmount2.ClientID %>').val(discountAmount).trigger('changeValue');

        calculateSubTotal();
    });

    $('#<%=txtDiscountAmount2.ClientID %>').change(function () {
        $(this).blur();
        var discountAmount2 = parseFloat($(this).attr('hiddenVal'));
        var discountAmount1 = parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal'));
        var receivedItem = parseFloat($('#<%=txtQuantity.ClientID %>').val());
        var unitPrice = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));

        var discountPercentage = (discountAmount2 * 100) / (unitPrice - discountAmount1);
        $('#<%=txtDiscount2.ClientID %>').val(discountPercentage);

        calculateSubTotal();
    });

    $('#<%=txtPrice.ClientID %>').change(function () {
        $(this).blur();
        $('#<%=txtDiscount.ClientID %>').change();
    });

    function calculateSubTotal() {
        var price = parseFloat($('#<%=txtPrice.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
        var subTotal = price * qty;
        var discount1 = parseInt($('#<%=txtDiscount.ClientID %>').val());
        var discount2 = parseInt($('#<%=txtDiscount2.ClientID %>').val())
        subTotal = subTotal * (100 - discount1) / 100;
        subTotal = subTotal * (100 - discount2) / 100;
        $('#<%=txtSubTotalPrice.ClientID %>').val(subTotal).trigger('changeValue');
    }

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
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnPurchaseReceiveID" value="" runat="server" />
    <input type="hidden" id="hdnEntryID" value="" runat="server" />
    <input type="hidden" id="hdnIsEdit" value="" runat="server" />
    <input type="hidden" id="hdnTempFactor" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. BPB")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPurchaseReceiveNo" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 130px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsBonus" Width="100%" runat="server" Checked="true" Enabled="false"
                                                    Text="Bonus" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label>
                                                    <%=GetLabel("No. Pemesanan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label id="lblItemGroup" runat="server">
                                                    <%=GetLabel("Kelompok Item")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtItemGroupCode" ReadOnly="true" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="lblItem" runat="server">
                                                    <%=GetLabel("Item")%></label>
                                            </td>
                                            <td colspan="2">
                                                <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
                                                <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                <input type="hidden" value="" id="hdnConversionFactor" runat="server" />
                                                <input type="hidden" value="" id="hdnUnitPrice" runat="server" />
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
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
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Jumlah Dipesan")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtOrderQty" ReadOnly="true" Width="120px" CssClass="number" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOrderUnit" ReadOnly="true" Width="150px" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Jumlah Diterima")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtQuantity" ReadOnly="true" CssClass="number" Width="120px" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtQuantityUnit" ReadOnly="true" Width="150px" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" id="lblSupplierItem">
                                                    <%=GetLabel("Supplier Item")%></label>
                                            </td>
                                            <td colspan="2">
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 250px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtSupplierItemCode" ReadOnly="true" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSupplierItemName" ReadOnly="true" Width="100%" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trPrice" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Harga")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPrice" Width="120px" runat="server" CssClass="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr id="trDiscount" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Diskon 1 %")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 120px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscount" value="0" CssClass="number" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" Width="100%" runat="server" ReadOnly="true"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trDiscount2" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Diskon 2 %")%></label>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col style="width: 120px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscount2" value="0" CssClass="number" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscountAmount2" CssClass="txtCurrency" Width="100%" runat="server" ReadOnly="true"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trSubTotalPrice" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Total Harga")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubTotalPrice" Width="120px" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="IsBonusItem" value="<%#: Eval("IsBonusItem")%>" />
                                                <input type="hidden" class="Quantity" value="<%#: Eval("Quantity")%>" />
                                                <input type="hidden" class="PurchaseOrderNo" value="<%#: Eval("PurchaseOrderNo")%>" />
                                                <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="ItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                <input type="hidden" class="ItemGroupCode" value="<%#: Eval("ItemGroupCode")%>" />
                                                <input type="hidden" class="ItemGroupName1" value="<%#: Eval("ItemGroupName1")%>" />
                                                <input type="hidden" class="GCBaseUnit" value="<%#: Eval("GCBaseUnit")%>" />
                                                <input type="hidden" class="GCItemUnit" value="<%#: Eval("GCItemUnit")%>" />
                                                <input type="hidden" class="OrderQuantity" value="<%#: Eval("OrderQuantity")%>" />
                                                <input type="hidden" class="OrderPurchaseUnit" value="<%#: Eval("OrderPurchaseUnit")%>" />
                                                <input type="hidden" class="ItemUnit" value="<%#: Eval("ItemUnit")%>" />
                                                <input type="hidden" class="UnitPrice" value="<%#: Eval("UnitPrice")%>" />
                                                <input type="hidden" class="ConversionFactor" value="<%#: Eval("ConversionFactor")%>" />
                                                <input type="hidden" class="SupplierItemCode" value="<%#: Eval("SupplierItemCode")%>" />
                                                <input type="hidden" class="SupplierItemName" value="<%#: Eval("SupplierItemName")%>" />
                                                <input type="hidden" class="DiscountPercentage1" value="<%#: Eval("DiscountPercentage1")%>" />
                                                <input type="hidden" class="DiscountPercentage2" value="<%#: Eval("DiscountPercentage2")%>" />
                                                <input type="hidden" class="CustomSubTotal" value="<%#: Eval("CustomSubTotal")%>" />
                                                <input type="hidden" class="RemarksDetail" value="<%#: Eval("RemarksDetail")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="250px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:N}"
                                            HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:N2}"
                                            HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
