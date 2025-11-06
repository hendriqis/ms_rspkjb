<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemProductPlanningEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemProductPlanningEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ItemProductPlanningEntryCtl">
    $(function () {
        //#region Supplier
        function onGetSupplierFilterExpression() {
            return "<%=OnGetSupplierFilterExpression() %>";
        }

        $('#lblSupplier.lblLink').live('click', function () {
            openSearchDialog('businesspartners', onGetSupplierFilterExpression(), function (value) {
                $('#<%=txtSupplierCode.ClientID %>').val(value);
                onTxtSupplierCodeChanged(value);
            });
        });

        $('#<%=txtSupplierCode.ClientID %>').live('change', function () {
            onTxtSupplierCodeChanged($(this).val());
        });

        function onTxtSupplierCodeChanged(value) {
            var filterExpression = onGetSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnSupplierID.ClientID %>').val('');
                    $('#<%=txtSupplierCode.ClientID %>').val('');
                    $('#<%=txtSupplierName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#<%=txtBasePrice.ClientID %>').change(function () {
            $(this).trigger('changeValue');
            var basePrice = parseFloat($(this).attr('hiddenVal'));
            var conversionFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
            var purchasePrice = basePrice * conversionFactor;
            $('#<%=txtPurchasePrice.ClientID %>').val(purchasePrice).trigger('changeValue');
        });

        $('#<%=txtPurchasePrice.ClientID %>').change(function () {
            $(this).trigger('changeValue');
            var purchasePrice = $(this).attr('hiddenVal');
            var conversionFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
            var basePrice = purchasePrice / conversionFactor;
            $('#<%=txtBasePrice.ClientID %>').val(basePrice).trigger('changeValue');
        });

        $('#<%=txtLeadTime.ClientID %>').change(function () {
            $(this).trigger('changeValue');
            var leadTime = parseFloat($('#<%=txtLeadTime.ClientID %>').val());
            var factorXMax = parseFloat($('#<%=txtFactorXMax.ClientID %>').val());
            var forwardDays = Math.ceil(leadTime * factorXMax);
            $('#<%=txtForward.ClientID %>').val(forwardDays).trigger('changeValue');
        });

        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });
    });

    var conversionFactor = 1;
    function onCboPurchaseUnitValueChanged() {
        var GCPurchaseUnit = cboPurchaseUnit.GetValue();
        if (GCPurchaseUnit == null)
            $('#<%=hdnConversionFactor.ClientID %>').val('1');
        else {
            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + GCPurchaseUnit + "' AND IsDeleted = 0 AND IsActive = 1";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                if (result != null && result != "") {
                    $('#<%=hdnConversionFactor.ClientID %>').val(result);
                } else {
                    $('#<%=hdnConversionFactor.ClientID %>').val('1');
                }
            });
        }

        var basePrice = parseFloat($('#<%=txtBasePrice.ClientID %>').attr('hiddenVal'));
        var conversionFactor = parseFloat($('#<%=hdnConversionFactor.ClientID %>').val());
        var purchasePrice = basePrice * conversionFactor;
        $('#<%=txtPurchasePrice.ClientID %>').val(purchasePrice).trigger('changeValue');
    }
</script>
<input type="hidden" id="hdnItemPlanningID" value="" runat="server" />
<input type="hidden" id="hdnItemID" value="" runat="server" />
<input type="hidden" id="hdnConversionFactor" value="" runat="server" />
<input type="hidden" id="hdnIsUsingPurchaseDiscountShared" runat="server" value="0" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <fieldset id="fsEntryPopup" style="margin: 0">
                <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 140px" />
                        <col style="width: 20px" />
                        <col style="width: 230px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Item")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblSupplier">
                                <%=GetLabel("Supplier")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnSupplierID" />
                            <asp:TextBox ID="txtSupplierCode" Width="100%" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Satuan Pembelian") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPurchaseUnit" ClientInstanceName="cboPurchaseUnit" runat="server"
                                Width="117px">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboPurchaseUnitValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkIsUsingSupplierCatalog" Width="100%" runat="server" Text="Pemesanan berdasarkan Katalog Supplier" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="height: 10px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Harga Rata-Rata")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtAveragePrice" Width="115px" CssClass="txtCurrency" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("HNA (Harga Satuan Kecil)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtBasePrice" Width="115px" CssClass="txtCurrency" runat="server" />
                        </td>
                        <td class="tdLabel" colspan="2" style="padding-left: 5px">
                            <label class="lblNormal">
                                <%=GetLabel("Pembelian Terakhir (Satuan Kecil)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtLastPurchaseKecilInfo" ReadOnly="true" Width="150px" runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Harga Satuan Pembelian")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtPurchasePrice" Width="115px" CssClass="txtCurrency" runat="server" />
                        </td>
                        <td class="tdLabel" colspan="2" style="padding-left: 5px">
                            <label class="lblNormal">
                                <%=GetLabel("Pembelian Terakhir (Satuan Pembelian)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtLastPurchaseBesarInfo" ReadOnly="true" Width="150px" runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Presentase PO Rawat Inap")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtInpatientPOPrecentage" Width="115px" CssClass="txtCurrency" runat="server" />
                        </td>
                        <td class="tdLabel" colspan="2" style="padding-left: 5px">
                            <label class="lblNormal">
                                <%=GetLabel("Diskon 1 Terakhir (%)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtLastPurchaseDiscount" ReadOnly="true" Width="150px" runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Presentase PO Rawat Jalan")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtOutpatientPOPrecentage" Width="115px" CssClass="txtCurrency"
                                runat="server" />
                        </td>
                        <td class="tdLabel" colspan="2" style="padding-left: 5px">
                            <label class="lblNormal">
                                <%=GetLabel("Diskon 2 Terakhir (%)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtLastPurchaseDiscount2" ReadOnly="true" Width="150px" runat="server"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr id="trPurchaseDiscountShared" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Diskon Pembelian (%)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtPurchaseDiscountSharedInPercentage" Width="115px" CssClass="txtCurrency"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Diskon Sharing Pasien (%)")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtPatientDiscountSharedInPercentage" Width="115px" CssClass="txtCurrency"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="height: 10px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("R.O.P")%>
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblROP" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Statis" Value="false" Selected="True" />
                                <asp:ListItem Text="Dinamis" Value="true" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Lead Time")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtLeadTime" Width="115px" CssClass="number" runat="server" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("hari")%>
                            </label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="1">
                                <colgroup>
                                    <col />
                                    <col width="60px" />
                                    <col width="60px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" style="padding-left: 10px; padding-right: 10px">
                                        <label class="lblNormal">
                                            <%=GetLabel("Faktor X : Min - Max")%>
                                        </label>
                                    </td>
                                    <td class="tdLabel">
                                        <asp:TextBox ID="txtFactorXMin" Width="100%" CssClass="number" runat="server" ReadOnly="true" />
                                    </td>
                                    <td class="tdLabel">
                                        <asp:TextBox ID="txtFactorXMax" Width="100%" CssClass="number" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Backward")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtBackward" Width="115px" CssClass="number" runat="server" />
                        </td>
                        <td class="tdLabel" colspan="3">
                            <label class="lblNormal">
                                <%=GetLabel("hari (Jumlah Hari Pemakaian Rata-rata)")%>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Forward")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtForward" Width="115px" CssClass="number" runat="server" />
                        </td>
                        <td class="tdLabel" colspan="3">
                            <label class="lblNormal">
                                <%=GetLabel("hari (Jumlah Hari Kebutuhan Persediaan)")%>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="height: 10px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table>
                                <colgroup>
                                    <col style="width: 60px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" colspan="2">
                                        <label class="lblNormal">
                                            <%=GetLabel("Keterangan R.O.P. (Re-Order Point): ")%>
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <b>
                                                <%=GetLabel("Statis")%></b>
                                        </label>
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Dihitung berdasarkan Min dan Max saldo item per Gudang dan Lokasi  ")%>
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <b>
                                                <%=GetLabel("Dinamis")%></b>
                                        </label>
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Dihitung berdasarkan Rumus berikut  ")%>
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            ( Qty Pemakaian Rata-rata <b>X</b> Jumlah Hari Forward ) - Qty Saldo Tersedia
                                        </label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            &nbsp;
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tolerance")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtTolerance" Width="100%" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Safety Time")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtSafetyTime" Width="50px" CssClass="number" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Time Fence")%>
                            </label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtTimeFence" Width="50px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Safety Stock")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtSafetyStock" Width="100%" CssClass="number" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Min Order Qty")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtMinOrderQty" Width="100%" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Max Order Qty")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtMaxOrderQty" Width="100%" CssClass="number" runat="server" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
