<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionNonMasterEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionNonMasterEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
    $('.txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });
    
    $('#<%=txtNonMasterQtyUsed.ClientID %>').change(function () {
        $('#<%=txtNonMasterQtyCharged.ClientID %>').val($(this).val());
        $('#<%=txtNonMasterQtyCharged.ClientID %>').change();
    });

    $('#<%=txtNonMasterQtyCharged.ClientID %>').change(function () {
        calculateNonMasterTariffTotal();
    });

    $('#<%=txtNonMasterUnitTariff.ClientID %>').change(function () {
        $(this).blur();
        calculateNonMasterTariffTotal();
    });

    $('#<%=txtNonMasterPriceDiscount.ClientID %>').change(function () {
        $(this).blur();
        calculateNonMasterTotal();
    });

    $('#<%=txtNonMasterPatient.ClientID %>').change(function () {
        var patientTotal = parseFloat($(this).val());
        var total = parseFloat($('#<%=txtNonMasterTotal.ClientID %>').attr('hiddenVal'));
        var payerTotal = total - patientTotal;
        $('#<%=txtNonMasterPayer.ClientID %>').val(payerTotal).trigger('changeValue');
    });

    $('#<%=txtNonMasterPayer.ClientID %>').change(function () {
        var payerTotal = parseFloat($(this).val());
        var total = parseFloat($('#<%=txtNonMasterTotal.ClientID %>').attr('hiddenVal'));
        var patientTotal = total - payerTotal;
        $('#<%=txtNonMasterPatient.ClientID %>').val(patientTotal).trigger('changeValue');
    });

    function calculateNonMasterTariffTotal() {
        var tariff = parseFloat($('#<%=txtNonMasterUnitTariff.ClientID %>').attr('hiddenVal'));
        var qty = parseFloat($('#<%=txtNonMasterQtyCharged.ClientID %>').val());
        $('#<%=txtNonMasterPriceTariff.ClientID %>').val(tariff * qty).trigger('changeValue');
        calculateNonMasterTotal();
    }

    function calculateNonMasterTotal() {
        var tariff = parseFloat($('#<%=txtNonMasterPriceTariff.ClientID %>').attr('hiddenVal'));
        var discount = parseFloat($('#<%=txtNonMasterPriceDiscount.ClientID %>').attr('hiddenVal'));
        var total = tariff - discount;
        $('#<%=txtNonMasterPatient.ClientID %>').val(total).trigger('changeValue');
        $('#<%=txtNonMasterPayer.ClientID %>').val(0).trigger('changeValue');
        $('#<%=txtNonMasterTotal.ClientID %>').val(total).trigger('changeValue');
    }
</script>

<div style="padding:10px;">
    <input type="hidden" id="hdnPrescriptionDtID" runat="server" value="" />
    <input type="hidden" id="hdnItemID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    
    <table>
        <colgroup>
            <col style="width:150px"/>
            <col style="width:120px"/>
            <col style="width:120px"/>
            <col style="width:120px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label><%=GetLabel("Lokasi")%></label></td>
            <td colspan="3"><dxe:ASPxComboBox ID="cboNonMasterLocation" ClientInstanceName="cboNonMasterLocation" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Obat")%></label></td>
            <td colspan="3"><asp:TextBox ID="txtDrugName" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label><%=GetLabel("Kelas Tagihan")%></label></td>
            <td colspan="3"><dxe:ASPxComboBox ID="cboNonMasterChargeClassID" ClientInstanceName="cboNonMasterChargeClassID" Width="200px" runat="server" /></td> 
        </tr> 
        <tr>
            <td class="tdLabel"><label><%=GetLabel("Harga Satuan")%></label></td>
            <td colspan="3"><asp:TextBox ID="txtNonMasterUnitTariff" CssClass="txtCurrency" Width="150px" runat="server" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><div class="lblComponent"><%=GetLabel("Digunakan")%></div></td>
            <td><div class="lblComponent"><%=GetLabel("Dibebankan")%></div></td>
            <td><div class="lblComponent"><%=GetLabel("Satuan")%></div></td>
        </tr>
        <tr>
            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
            <td><asp:TextBox ID="txtNonMasterQtyUsed" Width="100%" CssClass="number min" min="0.1" runat="server" /></td>
            <td><asp:TextBox ID="txtNonMasterQtyCharged" Width="100%" CssClass="number min" min="0.1" runat="server" /></td>
            <td><dxe:ASPxComboBox ID="cboNonMasterUoM" runat="server" ClientInstanceName="cboNonMasterUoM" Width="200px" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><div class="lblComponent"><%=GetLabel("Harga")%></div></td>
            <td><div class="lblComponent"><%=GetLabel("Diskon")%></div></td>
        </tr>
        <tr>
            <td class="tdLabel"><label><%=GetLabel("Harga")%></label></td>
            <td><asp:TextBox ID="txtNonMasterPriceTariff" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
            <td><asp:TextBox ID="txtNonMasterPriceDiscount" CssClass="txtCurrency" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><div class="lblComponent"><%=GetLabel("Pasien")%></div></td>
            <td><div class="lblComponent"><%=GetLabel("Instansi")%></div></td>
            <td><div class="lblComponent"><%=GetLabel("Total")%></div></td>
        </tr>
        <tr>
            <td class="tdLabel"><label><%=GetLabel("Total")%></label></td>
            <td><asp:TextBox ID="txtNonMasterPatient" CssClass="txtCurrency" Width="100%" runat="server" /></td>
            <td><asp:TextBox ID="txtNonMasterPayer" CssClass="txtCurrency" Width="100%" runat="server" /></td>
            <td><asp:TextBox ID="txtNonMasterTotal" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
        </tr>
    </table>
</div>