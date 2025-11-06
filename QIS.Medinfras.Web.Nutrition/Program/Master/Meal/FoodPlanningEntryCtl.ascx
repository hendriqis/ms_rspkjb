<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FoodPlanningEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Nutrition.Program.FoodPlanningEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
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

        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });
    });

    var conversionFactor = 1;
    function onCboPurchaseUnitValueChanged() {
        var GCPurchaseUnit = cboPurchaseUnit.GetValue();
        if (GCPurchaseUnit == '')
            $('#<%=hdnConversionFactor.ClientID %>').val('1');
        else {
            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + GCPurchaseUnit + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                $('#<%=hdnConversionFactor.ClientID %>').val(result);
            });
        }
    }
</script>

<input type="hidden" id="hdnItemPlanningID" value="" runat="server" />
<input type="hidden" id="hdnItemID" value="" runat="server" />
<input type="hidden" id="hdnConversionFactor" value="" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width:100%"/>
    </colgroup>
    <tr>            
        <td style="padding:5px;vertical-align:top">
            <fieldset id="fsEntryPopup" style="margin:0"> 
                <table class="tblEntryContent" cellpadding="0" cellspacing="1" >
                    <colgroup>
                        <col style="width:160px"/>
                        <col style="width:120px"/>
                        <col style="width:20px"/>
                        <col style="width:120px"/>
                        <col style="width:120px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Rumah Sakit")%></label></td>
                        <td colspan="4"><asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td colspan="4"><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblSupplier"><%=GetLabel("Supplier")%></label></td>
                        <td colspan="4">
                            <input type="hidden" value="" runat="server" id="hdnSupplierID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtSupplierCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Satuan Pembelian") %></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPurchaseUnit" ClientInstanceName="cboPurchaseUnit" runat="server" Width="117px">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboPurchaseUnitValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr><td><div style="height:10px"></div></td></tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Harga Rata-Rata")%> </label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtAveragePrice" Width="115px" CssClass="txtCurrency" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Harga Satuan Kecil")%> </label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtBasePrice" Width="115px" CssClass="txtCurrency" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Harga Satuan Besar")%> </label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtPurchasePrice" Width="115px" CssClass="txtCurrency" runat="server" /></td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Lead Time")%></label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtLeadTime" Width="50px" CssClass="number" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tolerance")%> </label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtTolerance" Width="100%" CssClass="number" runat="server" /></td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Safety Time")%></label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtSafetyTime" Width="50px" CssClass="number" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Time Fence")%> </label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtTimeFence" Width="50px" CssClass="number" runat="server" /></td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Safety Stock")%></label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtSafetyStock" Width="100%" CssClass="number" runat="server" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Min Order Qty")%></label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtMinOrderQty" Width="100%" CssClass="number" runat="server" /></td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Max Order Qty")%></label></td>
                        <td class="tdLabel"><asp:TextBox ID="txtMaxOrderQty" Width="100%" CssClass="number" runat="server" /></td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>

