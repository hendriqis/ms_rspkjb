<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsignmentReceiveDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentReceiveDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    //#region Order No
    function getPurchaseOrderExpression() {
        var filterExpression = "<%:filterExpressionPurchaseOrder %>";
        return filterExpression;
    }

    $('#lblOrderNo.lblLink').click(function () {
        openSearchDialog('purchaseorderhd', getPurchaseOrderExpression(), function (value) {
            $('#<%=txtOrderNo.ClientID %>').val(value);
            onTxtOrderNoChanged(value);
        });
    });

    $('#<%=txtOrderNo.ClientID %>').change(function () {
        onTxtOrderNoChanged($(this).val());
    });

    function onTxtOrderNoChanged(value) {
        var filterExpression = "PurchaseOrderNo = '" + value + "'";
        Methods.getObject('GetPurchaseOrderHdList', filterExpression, function (result) {
            if (result != null) 
                $('#<%=hdnOrderID.ClientID %>').val(result.PurchaseOrderID);
            else 
                $('#<%=hdnOrderID.ClientID %>').val('');
            cbpViewPopup.PerformCallback("refresh");
        });
    }
    //#endregion

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
        $('#<%=grdView.ClientID %> .txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });

        $('.txtReceivedItem').each(function () {
            $(this).change();
        });

        $('#<%=grdView.ClientID %> tr:gt(0)').each(function () {
            $txtExpired = $(this).find('.txtExpired');
            if ($txtExpired != null) {
                setDatePickerElement($txtExpired);
                $txtExpired.val('<%=DateTimeNowDatePicker() %>');
            }
        });
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    $('.chkIsSelected input').die('change');
    $('.chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtReceivedItem').removeAttr('readonly');
            $tr.find('.txtUnitPrice').removeAttr('readonly');
            $tr.find('.txtDiscountPercentage1').removeAttr('readonly');
            $tr.find('.txtDiscount1').removeAttr('readonly');
            $tr.find('.txtDiscountPercentage2').removeAttr('readonly');
            $tr.find('.txtDiscount2').removeAttr('readonly');
            $tr.find('.txtBatchNo').removeAttr('readonly');
            $tr.find('.txtExpired').removeAttr('readonly');
            $tr.find('.lblPurchaseUnit').addClass('lblLink');
        }
        else {
            $tr.find('.txtReceivedItem').attr('readonly', 'readonly');
            $tr.find('.txtUnitPrice').attr('readonly', 'readonly');
            $tr.find('.txtDiscountPercentage1').attr('readonly', 'readonly');
            $tr.find('.txtDiscount1').attr('readonly', 'readonly');
            $tr.find('.txtDiscountPercentage2').attr('readonly', 'readonly');
            $tr.find('.txtDiscount2').attr('readonly', 'readonly');
            $tr.find('.txtBatchNo').attr('readonly', 'readonly');
            $tr.find('.txtExpired').attr('readonly', 'readonly');
            $tr.find('.lblPurchaseUnit').removeClass('lblLink');
        }
    });

    $('.txtReceivedItem').die('change');
    $('.txtReceivedItem').live('change', function () {
        var $tr = $(this).closest('tr');
        $tr.find('.txtDiscountPercentage1').change();
    });

    $('.txtUnitPrice').die('change');
    $('.txtUnitPrice').live('change', function () {
        $(this).blur();
        var $tr = $(this).closest('tr');
        $tr.find('.txtDiscountPercentage1').change();
    });

    $('.txtDiscountPercentage1').die('change');
    $('.txtDiscountPercentage1').live('change', function () {
        var $tr = $(this).closest('tr');
        var discountPercentage = parseFloat($(this).val());
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));

        var discountAmount = (receivedItem * unitPrice) * discountPercentage / 100;
        $tr.find('.txtDiscount1').val(discountAmount).trigger('changeValue');

        $tr.find('.txtDiscountPercentage2').change();
    });

    $('.txtDiscount1').die('change');
    $('.txtDiscount1').live('change', function () {
        $(this).blur();
        var $tr = $(this).closest('tr');
        var discountAmount = parseFloat($(this).attr('hiddenVal'));
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));

        var discountPercentage = (discountAmount * 100) / (receivedItem * unitPrice);
        $tr.find('.txtDiscountPercentage1').val(discountPercentage);

        $tr.find('.txtDiscountPercentage2').change();
    });

    $('.txtDiscount2').die('change');
    $('.txtDiscount2').live('change', function () {
        $(this).blur();
        var $tr = $(this).closest('tr');
        var discountAmount = parseFloat($(this).attr('hiddenVal'));
        var discountAmount1 = parseFloat($tr.find('.txtDiscount1').attr('hiddenVal'));
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));

        var discountPercentage = (discountAmount * 100) / ((receivedItem * unitPrice) - discountAmount1);
        $tr.find('.txtDiscountPercentage2').val(discountPercentage);
    });

    $('.txtDiscountPercentage2').die('change');
    $('.txtDiscountPercentage2').live('change', function () {
        var $tr = $(this).closest('tr');
        var discountPercentage = parseFloat($(this).val());
        var discountAmount1 = parseFloat($tr.find('.txtDiscount1').attr('hiddenVal'));
        var receivedItem = parseFloat($tr.find('.txtReceivedItem').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));

        var discountAmount = ((receivedItem * unitPrice) - discountAmount1) * discountPercentage / 100;
        $tr.find('.txtDiscount2').val(discountAmount).trigger('changeValue');
    });

    function onBeforeSaveRecord(errMessage) {
        var count = 0;
        $('.chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                count += 1;
            }
        });
        if (count == 0) {
            errMessage.text = 'Please Select Item First';
            return false;
        }
        return true;
    }

    //#region Purchase Unit
    function getPurchaseUnitFilterExpression() {
        var filterExpression = "ItemID = " + itemID;
        return filterExpression;
    }

    var itemID = 0;
    $tr = null;
    $('.lblPurchaseUnit.lblLink').live('click', function () {
        $tr = $(this).closest('tr');
        itemID = $tr.find('.hdnItemID').val();
        var orderDate = $tr.find('.hdnOrderDate').val();
        var supplierID = $('#<%=hdnSupplierID.ClientID %>').val();

        var filter = getPurchaseUnitFilterExpression();
        var filterExpressionIP = "ItemID = " + itemID + " AND IsDeleted = 0 AND IsUsingSupplierCatalog = 1";
        var isUsingSupplierCatalog = 0;
        Methods.getObject('GetItemPlanningList', filterExpressionIP, function (result) {
            if (result != null) {
                isUsingSupplierCatalog = 1;
            }
        });

        var filterItem = "ItemID = " + itemID;
        var itemUnit = "";
        Methods.getObject('GetItemMasterList', filterItem, function (resultItem) {
            if (resultItem != null) {
                itemUnit = resultItem.GCItemUnit;
            }
        });

        if (isUsingSupplierCatalog == 1) {
            var lstUnit = "";

            var hdnIM0131 = $('#<%=hdnIM0131Ctl.ClientID %>').val();
            var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
            var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

            if (hdnIM0131 == "0") {
                Methods.getItemMasterPurchaseList(itemID, supplierID, function (result1) {
                    if (result1.length > 0) {
                        for (i = 0; i < result1.length; i++) {
                            if (lstUnit == "") {
                                lstUnit = "'" + result1[i].PurchaseUnit + "'";
                            }
                            else {
                                lstUnit += ",'" + result1[i].PurchaseUnit + "'";
                            }
                        }
                    }
                });
            }
            else {
                Methods.getItemMasterPurchaseWithDateList(itemID, supplierID, orderDateFormatString, function (result1) {
                    if (result1.length > 0) {
                        for (i = 0; i < result1.length; i++) {
                            if (lstUnit == "") {
                                lstUnit = "'" + result1[i].PurchaseUnit + "'";
                            }
                            else {
                                lstUnit += ",'" + result1[i].PurchaseUnit + "'";
                            }
                        }
                    }
                });
            }

            if (lstUnit == "") {
                lstUnit = "'" + itemUnit + "'";
            }
            else {
                lstUnit += ",'" + itemUnit + "'";
            }

            filter += " AND GCAlternateUnit IN (" + lstUnit + ")";
        }

        openSearchDialog('itemalternateunit2', filter, function (value) {
            onTxtPurchaseUnitChanged(value);
        });
    });

    function onTxtPurchaseUnitChanged(value) {
        var conversionOld = parseFloat($tr.find('.hdnConversionFactor').val());

        var filterExpression = getPurchaseUnitFilterExpression() + " AND GCAlternateUnit = '" + value + "'";
        Methods.getObject('GetvItemAlternateUnit2List', filterExpression, function (result) {
            if (result != null) {
                var baseUnit = $tr.find('.hdnBaseUnit').val();

                $tr.find('.lblPurchaseUnit').html(result.AlternateUnit);
                $tr.find('.lblConversion').html("1 " + result.AlternateUnit + " = " + result.ConversionFactor + " " + baseUnit);
                $tr.find('.hdnGCPurchaseUnit').val(result.GCAlternateUnit);
                $tr.find('.hdnConversionFactor').val(result.ConversionFactor);

                var hdnGCPurchaseUnit = $tr.find('.hdnGCPurchaseUnit').val();
                var hdnConversionFactor = $tr.find('.hdnConversionFactor').val();

                var qtyBegin = parseFloat($tr.find('.txtReceivedItem').val().replace('.00', '').split(',').join(''));
                var conversion = parseFloat(result.ConversionFactor);

                var price = parseFloat($tr.find('.txtUnitPrice').val().replace('.00', '').split(',').join(''));

                var qtyEnd = parseFloat((qtyBegin * conversionOld / conversion).toFixed(2));

                var totalPrice = parseFloat((price / conversionOld * conversion).toFixed(2));

                $tr.find('.txtReceivedItem').val(qtyEnd).trigger('changeValue');
                $tr.find('.txtUnitPrice').val(totalPrice).trigger('changeValue');
            }
            else {
                $tr.find('.hdnGCPurchaseUnit').val('');
                $tr.find('.lblPurchaseUnit').html('');
                $tr.find('.hdnConversionFactor').val('');
                $tr.find('.lblConversion').val('');
            }
        });
    }
    //#endregion
</script>
<input type="hidden" id="hdnSupplierID" value="" runat="server" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnIsUsedProductLine" runat="server" value="0" />
<input type="hidden" id="hdnProductLineIDCtl" runat="server" value="0" />
<input type="hidden" id="hdnIM0131Ctl" runat="server" value="0" />

<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 50%">
                <colgroup>
                    <col style="width: 30%" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink" id="lblOrderNo"><%=GetLabel("No. Pemesanan")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnOrderID" value="" runat="server" />
                        <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
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
                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <input id="chkSelectAll" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                            <input type="hidden" id="hdnPOHdID" runat="server" value='<%#: Eval("PurchaseOrderID")%>' />
                                            <input type="hidden" id="hdnItemID" class="hdnItemID" runat="server" value='<%#: Eval("ItemID")%>' />
                                            <input type="hidden" id="hdnItemName1" class="hdnItemName1" runat="server" value='<%#: Eval("ItemName1")%>' />
                                            <input type="hidden" id="hdnGCPurchaseUnit" class="hdnGCPurchaseUnit" runat="server" value='<%#: Eval("GCPurchaseUnit")%>' />
                                            <input type="hidden" id="hdnDefaultConversionFactor" class="hdnDefaultConversionFactor"
                                                runat="server" value='<%#: Eval("ConversionFactor")%>' />
                                            <input type="hidden" id="hdnConversionFactor" class="hdnConversionFactor" runat="server" value='<%#: Eval("ConversionFactor")%>' />
                                            <input type="hidden" id="hdnBaseUnit" class="hdnBaseUnit" runat="server" value='<%#: Eval("BaseUnit")%>' />
                                            <input type="hidden" id="hdnReceivedQuantity" class="hdnReceivedQuantity" runat="server"
                                                value='<%#: Eval("ReceivedQuantity")%>' />
                                            <input type="hidden" id="hdnQuantity" class="hdnQuantity" runat="server" value='<%#: Eval("DraftQuantity")%>' />
                                            <input type="hidden" id="hdnOrderDate" class="hdnOrderDate" runat="server" value='<%#: Eval("OrderDate")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" />
                                    <asp:BoundField DataField="CustomQtyRemaining" HeaderText="Sisa" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Diterima" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReceivedItem" ReadOnly="true" Width="99%" value ="0" runat="server" CssClass="number txtReceivedItem"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Harga" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnitPrice" ReadOnly="true" Width="99%" runat="server" CssClass="txtCurrency txtUnitPrice"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="20px" HeaderText="Satuan" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <label runat="server" id="lblPurchaseUnit" class="lblPurchaseUnit"></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Disc1 (%)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscountPercentage1" ReadOnly="true" Width="99%" runat="server" Text="0" CssClass="number txtDiscountPercentage1"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Disc1 (Rp)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscount1" ReadOnly="true" Width="99%" runat="server" Text="0" CssClass="txtCurrency txtDiscount1"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Disc2 (%)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscountPercentage2" ReadOnly="true" Width="99%" runat="server" Text="0" CssClass="number txtDiscountPercentage2"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Disc2 (Rp)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscount2" ReadOnly="true" Width="99%" runat="server" Text="0" CssClass="txtCurrency txtDiscount2"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="120px" HeaderText="Conversion" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <label runat="server" id="lblConversion" class="lblConversion"><%#:Eval("CustomConversion")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="20px" HeaderText="Asset" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsAsset" runat="server" CssClass="chkIsAsset" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" Visible="false" HeaderText="Serial No" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSerialNo" ReadOnly="true" Width="50%" value ="0" runat="server" CssClass="number txtSerialNo"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="No Batch" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBatchNo" ReadOnly="true" Width="100%" value ="0" runat="server" CssClass="txtBatchNo"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Expired Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div style='<%#: Eval("IsControlExpired").ToString() == "False" ? "display:none" : ""%>'> <asp:TextBox ID="txtExpired" ReadOnly="true" Width="70%" value ="0" runat="server" CssClass="txtExpired datepicker" /> </div>
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
