<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemProductPlanningEntryEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemProductPlanningEntryEditCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ItemProductPlanningEntryEditCtl">
    $(function () {
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
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <fieldset id="fsEntryPopup" style="margin: 0">
                <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                    <colgroup>
                        <col style="width: 170px" />
                        <col style="width: 120px" />
                        <col style="width: 20px" />
                        <col style="width: 120px" />
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
                            <label class="lblNormal">
                                <%=GetLabel("Satuan Pembelian") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPurchaseUnit" ClientInstanceName="cboPurchaseUnit" runat="server"
                                Width="117px">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboPurchaseUnitValueChanged(); }" />
                            </dxe:ASPxComboBox>
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
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
