<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationItemSupplierCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InformationItemSupplierCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_InformationItemSupplierCtl">
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

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewItemSupplierCtl.PerformCallback('changepage|' + page);
        }, 8);
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
        cbpViewItemSupplierCtl.PerformCallback('refresh');
    }

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

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpViewItemSupplierCtl.PerformCallback('save');
        }
        else {
            return false;
        }

    });

    function onCbpPurchaseUnitEndCallBack(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
                cbpViewItemSupplierCtl.PerformCallback('refresh');
            }
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnParam" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnGCPurchaseUnit" value="" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <table class="tblEntryContent" style="width: 700px;">
                <td style="padding: 5px; vertical-align: top">
                    <table>
                        <colgroup>
                            <col style="width: 150px">
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Item")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItem" ReadOnly="true" Width="150%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:qisintellisensetextbox runat="server" clientinstancename="txtSearchView" id="txtSearchView"
                                    width="400px" watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Supplier" FieldName="SupplierName" />
                                        <qis:QISIntellisenseHint Text="Kode Supplier" FieldName="suppliercode" />
                                    </IntellisenseHints>
                                </qis:qisintellisensetextbox>
                            </td>
                        </tr>
                    </table>
                </td>
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
                                            <dxe:aspxcombobox id="cboPurchaseUnit" clientinstancename="cboPurchaseUnit" runat="server"
                                                width="100px" oncallback="cboPurchaseUnit_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCboPurchaseUnitEndCallBack(s); }"
                                                    ValueChanged="function(s,e){ onCboPurchaseUnitValueChanged(); }" />
                                            </dxe:aspxcombobox>
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
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:aspxcallbackpanel id="cbpViewItemSupplierCtl" runat="server" width="100%" clientinstancename="cbpViewItemSupplierCtl"
                    showloadingpanel="false" oncallback="cbpViewItemSupplierCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPurchaseUnitEndCallBack(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="BusinessPartnerID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
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
                                        <asp:TemplateField HeaderText="Supplier" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <label style="font-size: medium; font-weight: bold">
                                                    <%#:Eval("SupplierName") %></label><br />
                                                <label style="font-size: smaller; font-style: italic">
                                                    <%#:Eval("BusinessPartnerCode") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LeadTime" HeaderStyle-Width="30px" HeaderText="Waktu Tunggu"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="PurchaseUnit" HeaderText="Satuan" HeaderStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PurchaseUnitPrice" HeaderStyle-Width="120px" HeaderText="Harga Satuan Besar"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="Price" HeaderStyle-Width="120px" HeaderText="Harga Satuan Kecil"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="DiscountPercentage" HeaderStyle-Width="50px" HeaderText="Diskon 1 [%]"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DiscountPercentage2" HeaderStyle-Width="50px" HeaderText="Diskon 2 [%]"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Tanggal Berlaku" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <label>
                                                    <%#:Eval("cfStartDateInDatePicker") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Dibuat Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <label style="font-size: smaller; font-style: italic">
                                                    <%#:Eval("CreatedByName") %></label><br />
                                                <label style="font-size: smaller; font-weight: bold">
                                                    <%#:Eval("cfCreatedDateInStringFullFormat") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Terakhir Diubah Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <label style="font-size: smaller; font-style: italic">
                                                    <%#:Eval("LastUpdateByName") %></label><br />
                                                <label style="font-size: smaller; font-weight: bold">
                                                    <%#:Eval("cfLastUpdatedDateInStringFullFormat") %></label>
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
                </dxcp:aspxcallbackpanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
    </table>
</div>
