<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierItemEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SupplierItemEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_SupplierItemEntryCtl">
    $(function () {
        setDatePicker('<%=txtStartDate.ClientID %>');

        $('#<%=txtPrice.ClientID %>').change(function () {
            $(this).trigger('changeValue');
            var basePrice = parseFloat($(this).attr('hiddenVal'));
            var conversionFactor = parseFloat($('#<%=txtConversionFactor.ClientID %>').val().replace(",", ""));
            var purchaseUnitPrice = basePrice * conversionFactor;
            $('#<%=txtPurchaseUnitPrice.ClientID %>').val(purchaseUnitPrice).trigger('changeValue');
        });

        $('#<%=txtPurchaseUnitPrice.ClientID %>').change(function () {
            $(this).trigger('changeValue');
            var purchaseUnitPrice = parseFloat($(this).attr('hiddenVal'));
            var conversionFactor = parseFloat($('#<%=txtConversionFactor.ClientID %>').val().replace(",", ""));
            var basePrice = purchaseUnitPrice / conversionFactor;
            $('#<%=txtPrice.ClientID %>').val(basePrice).trigger('changeValue');
        });

        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });
    });

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtSupplierItemCode.ClientID %>').val('');
        $('#<%=txtSupplierItemName.ClientID %>').val('');
        $('#<%=txtLeadTime.ClientID %>').val('0');
        $('#<%=txtStartDate.ClientID %>').val($('#<%=hdnDateTodayInDatePicker.ClientID %>').val());
        $('#<%=txtConversionFactor.ClientID %>').val('0');
        $('#<%=txtPurchaseUnitPrice.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtPrice.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtDiscountPercentage1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtDiscountPercentage2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtRemarks.ClientID %>').val('');
        cboPurchaseUnit.SetValue('');
        $('#containerPopupEntryData').show();
        cboPurchaseUnit.PerformCallback();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
        else {
            return false;
        }

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
        var ID = $row.find('.hdnID').val();
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var itemName = $row.find('.hdnItemName').val();
        var customerItemCode = $row.find('.hdnSupplierItemCode').val();
        var customerItemName = $row.find('.hdnSupplierItemName').val();
        var leadTime = $row.find('.hdnLeadTime').val();
        var startDate = $row.find('.hdncfStartDateInDatePicker').val();
        var purchaseUnit = $row.find('.hdnPurchaseUnit').val();
        var conversionFactor = $row.find('.hdnConversionFactor').val();
        var price = $row.find('.hdnPrice').val();
        var purchaseUnitprice = $row.find('.hdnPurchaseUnitPrice').val();
        var discountPercentage1 = $row.find('.hdnDiscountPercentage1').val();
        var discountPercentage2 = $row.find('.hdnDiscountPercentage2').val();
        var remarks = $row.find('.hdnRemarks').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtItemName.ClientID %>').val(itemName);
        $('#<%=txtSupplierItemCode.ClientID %>').val(customerItemCode);
        $('#<%=txtSupplierItemName.ClientID %>').val(customerItemName);
        $('#<%=txtLeadTime.ClientID %>').val(leadTime);
        $('#<%=txtStartDate.ClientID %>').val(startDate);
        $('#<%=txtConversionFactor.ClientID %>').val(conversionFactor);
        $('#<%=txtPurchaseUnitPrice.ClientID %>').val(purchaseUnitprice).trigger('changeValue');
        $('#<%=txtPrice.ClientID %>').val(price).trigger('changeValue');
        $('#<%=txtDiscountPercentage1.ClientID %>').val(discountPercentage1).trigger('changeValue');
        $('#<%=txtDiscountPercentage2.ClientID %>').val(discountPercentage2).trigger('changeValue');
        $('#<%=txtRemarks.ClientID %>').val(remarks);
        $('#<%=hdnGCPurchaseUnit.ClientID %>').val(purchaseUnit);
        $('#containerPopupEntryData').show();
        cboPurchaseUnit.PerformCallback();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblEntryPopupAddData').click();
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('.grdPopup tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshControl();
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onRefreshControl(filterExpression) {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpEntryPopupView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

    //#region Item
    function onGetSupplierItemCodeFilterExpression() {
        var filterExpression = "GCItemType IN ('X001^002','X001^003','X001^008','X001^009') AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        openSearchDialog('item', onGetSupplierItemCodeFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtSupplierItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtSupplierItemCodeChanged($(this).val());
    });

    function onTxtSupplierItemCodeChanged(value) {
        var filterExpression = onGetSupplierItemCodeFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=txtSupplierItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtSupplierItemName.ClientID %>').val(result.ItemName1);
                var filterExpression2 = "ItemID = " + result.ItemID;
                Methods.getObject('GetvItemAlternateUnitList', filterExpression2, function (result2) {
                    if (result2 != '' && result2 != null) {
                        $('#<%=hdnGCPurchaseUnit.ClientID %>').val(result2.GCAlternateUnit);
                        $('#<%=txtConversionFactor.ClientID %>').val(result2.ConversionFactorInString);
                    }
                    else {
                        cboPurchaseUnit.SetValue(0);
                        $('#<%=txtConversionFactor.ClientID %>').val('1');
                    }
                });
                cboPurchaseUnit.PerformCallback();
                Methods.getObject('GetItemPlanningList', filterExpression2, function (resultIP) {
                    if (resultIP != null) {
                        $('#<%=txtPrice.ClientID %>').val(resultIP.UnitPrice).trigger('changeValue');
                        var token = ",";
                        var newToken = "";
                        var value = $('#<%=txtConversionFactor.ClientID %>').val().split(token).join(newToken);
                        var conversion = parseFloat(value);
                        $('#<%=txtPurchaseUnitPrice.ClientID %>').val(resultIP.UnitPrice * conversion).trigger('changeValue');
                        $('#<%=txtDiscountPercentage1.ClientID %>').val(resultIP.DiscountPercentage).trigger('changeValue');
                    }
                    else {
                        $('#<%=txtPrice.ClientID %>').val(0).trigger('changeValue');
                        $('#<%=txtPurchaseUnitPrice.ClientID %>').val(0).trigger('changeValue');
                        $('#<%=txtDiscountPercentage1.ClientID %>').val(0).trigger('changeValue');
                    }
                });
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

    function onCboPurchaseUnitEndCallBack(s) {
        if (s.GetValue() != null) {
            if ($('#<%=hdnGCPurchaseUnit.ClientID %>').val() != '') {
                cboPurchaseUnit.SetValue($('#<%=hdnGCPurchaseUnit.ClientID %>').val());
            }
        }
    }

    function onCboPurchaseUnitValueChanged() {
        var GCPurchaseUnit = cboPurchaseUnit.GetValue();
        if (GCPurchaseUnit == '')
            $('#<%=txtConversionFactor.ClientID %>').val('1');
        else {
            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + GCPurchaseUnit + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactorInString', function (result) {
                if (result != '')
                    $('#<%=txtConversionFactor.ClientID %>').val(result);
                else
                    $('#<%=txtConversionFactor.ClientID %>').val('1');
            });
        }
        $('#<%=txtPrice.ClientID %>').val($('#<%=txtPrice.ClientID %>').val()).trigger('change');
    }
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnDateTodayInDatePicker" value="" runat="server" />
    <input type="hidden" id="hdnGCPurchaseUnit" value="" runat="server" />
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnQuickText" value="" runat="server" />
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
                                <%=GetLabel("Pemasok")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Alamat Pemasok")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAddress" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Quick Filter")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="100%" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="Kode Item" FieldName="ItemCode" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
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
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
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
                                <td class="tdLabel" style="padding-left: 5px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Harga Satuan Besar")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPurchaseUnitPrice" CssClass="txtCurrency required" runat="server"
                                        Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kode Item (Pemasok)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupplierItemCode" runat="server" Width="100px" />
                                </td>
                                <td class="tdLabel" style="padding-left: 5px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Harga Satuan Kecil")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrice" CssClass="txtCurrency required" runat="server" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nama Item (Pemasok)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupplierItemName" runat="server" Width="100%" />
                                </td>
                                <td class="tdLabel" style="padding-left: 5px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Diskon 1 [%]")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiscountPercentage1" CssClass="txtCurrency required" runat="server"
                                        Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Satuan")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboPurchaseUnit" ClientInstanceName="cboPurchaseUnit" runat="server"
                                                    Width="100px" OnCallback="cboPurchaseUnit_Callback">
                                                    <ClientSideEvents EndCallback="function(s,e){ onCboPurchaseUnitEndCallBack(s); }"
                                                        ValueChanged="function(s,e){ onCboPurchaseUnitValueChanged(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td class="tdLabel" style="padding-left: 5px; padding-right: 5px">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Faktor Konversi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConversionFactor" ReadOnly="true" Style="text-align: right" runat="server"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="tdLabel" style="padding-left: 5px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Diskon 2 [%]")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiscountPercentage2" CssClass="txtCurrency required" runat="server"
                                        Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Waktu Tunggu")%></label>
                                </td>
                                <td style="vertical-align: top; padding-top: 5px;">
                                    <asp:TextBox ID="txtLeadTime" CssClass="number required" runat="server" Width="100px" />
                                </td>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px; padding-left: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Remarks")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Berlaku")%></label>
                                </td>
                                <td style="vertical-align: top; padding-top: 5px;">
                                    <asp:TextBox ID="txtStartDate" runat="server" Width="120px" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnItemName" value="<%#: Eval("ItemName1")%>" />
                                                <input type="hidden" class="hdnSupplierItemCode" value="<%#: Eval("SupplierItemCode")%>" />
                                                <input type="hidden" class="hdnSupplierItemName" value="<%#: Eval("SupplierItemName")%>" />
                                                <input type="hidden" class="hdnLeadTime" value="<%#: Eval("LeadTime")%>" />
                                                <input type="hidden" class="hdncfStartDateInDatePicker" value="<%#: Eval("cfStartDateInDatePicker")%>" />
                                                <input type="hidden" class="hdnPurchaseUnit" value="<%#: Eval("GCPurchaseUnit")%>" />
                                                <input type="hidden" class="hdnConversionFactor" value="<%#: Eval("ConversionFactorInString")%>" />
                                                <input type="hidden" class="hdnPurchaseUnitPrice" value="<%#: Eval("PurchaseUnitPrice")%>" />
                                                <input type="hidden" class="hdnPrice" value="<%#: Eval("Price")%>" />
                                                <input type="hidden" class="hdnDiscountPercentage1" value="<%#: Eval("DiscountPercentage")%>" />
                                                <input type="hidden" class="hdnDiscountPercentage2" value="<%#: Eval("DiscountPercentage2")%>" />
                                                <input type="hidden" class="hdnRemarks" value="<%#: Eval("Remarks")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="220px" HeaderText="Item<BR>Item Pemasok" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                            <%#: Eval("Item")%>
                                            <BR>
                                            <%#: Eval("SupplierItem")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:BoundField DataField="cfStartDateInDatePicker" HeaderStyle-Width="30px" HeaderText="Tanggal Berlaku"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="LeadTime" HeaderStyle-Width="30px" HeaderText="Waktu Tunggu"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="PurchaseUnit" HeaderStyle-Width="30px" HeaderText="Satuan"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="PurchaseUnitPrice" HeaderStyle-Width="80px" HeaderText="Harga Satuan Besar"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="Price" HeaderStyle-Width="80px" HeaderText="Harga Satuan Kecil"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="DiscountPercentage" HeaderStyle-Width="40px" HeaderText="Diskon 1 [%]"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DiscountPercentage2" HeaderStyle-Width="40px" HeaderText="Diskon 2 [%]"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Dibuat Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("CreatedByName") %></div>
                                                <div>
                                                    <%#:Eval("cfCreatedInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Diubah Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("LastUpdateByName") %></div>
                                                <div>
                                                    <%#:Eval("cfLastUpdateDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
        });
    </script>
</div>
