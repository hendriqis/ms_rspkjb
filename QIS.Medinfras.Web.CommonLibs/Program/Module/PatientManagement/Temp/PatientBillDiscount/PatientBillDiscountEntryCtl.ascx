<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillDiscountEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillDiscountEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_patientbilldiscountentryctl">
    $('#<%=txtTotalPatientAmount.ClientID %>').trigger('changeValue');
    $('#<%=txtTotalPatient.ClientID %>').trigger('changeValue');
    $('#<%=txtTotalPayerAmount.ClientID %>').trigger('changeValue');
    $('#<%=txtTotalPayer.ClientID %>').trigger('changeValue');

    $('#<%=txtPatientDiscountPercentage.ClientID %>').change(function () {
        var billingTotal = parseInt($('#<%=txtTotalPatientAmount.ClientID %>').attr('hiddenVal'));
        var discountPercentage = parseInt($(this).val());
        if (discountPercentage > 100) {
            discountPercentage = 100;
            $(this).val(discountPercentage).trigger('changeValue');
        }
        else if (discountPercentage < 0) {
            discountPercentage = 0;
            $(this).val(discountPercentage).trigger('changeValue');
        }
        var discountAmount = billingTotal * discountPercentage / 100;
        $('#<%=txtPatientDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
        $('#<%=txtTotalPatient.ClientID %>').val(billingTotal - discountAmount).trigger('changeValue');
    });

    $('#<%=txtPatientDiscountAmount.ClientID %>').change(function () {
        $(this).blur();
        var billingTotal = parseInt($('#<%=txtTotalPatientAmount.ClientID %>').attr('hiddenVal'));
        var discountAmount = parseInt($(this).attr('hiddenVal'));
        if (discountAmount > billingTotal) {
            discountAmount = billingTotal;
            $(this).val(discountAmount).trigger('changeValue');
        }
        else if (discountAmount < 0) {
            discountAmount = 0;
            $(this).val(discountAmount).trigger('changeValue');
        }
        var discountPercentage = 0;
        if (billingTotal != 0)
            discountPercentage = discountAmount * 100 / billingTotal;
        $('#<%=txtPatientDiscountPercentage.ClientID %>').val(discountPercentage);
        $('#<%=txtTotalPatient.ClientID %>').val(billingTotal - discountAmount).trigger('changeValue');
    });

    $('#<%=txtPayerDiscountPercentage.ClientID %>').change(function () {
        var billingTotal = parseInt($('#<%=txtTotalPayerAmount.ClientID %>').attr('hiddenVal'));
        var discountPercentage = parseInt($(this).val());
        if (discountPercentage > 100) {
            discountPercentage = 100;
            $(this).val(discountPercentage).trigger('changeValue');
        }
        else if (discountPercentage < 0) {
            discountPercentage = 0;
            $(this).val(discountPercentage).trigger('changeValue');
        }
        var discountAmount = billingTotal * discountPercentage / 100;
        $('#<%=txtPayerDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
        $('#<%=txtTotalPayer.ClientID %>').val(billingTotal - discountAmount).trigger('changeValue');
    });

    $('#<%=txtPayerDiscountAmount.ClientID %>').change(function () {
        $(this).blur();
        var billingTotal = parseInt($('#<%=txtTotalPayerAmount.ClientID %>').attr('hiddenVal'));
        var discountAmount = parseInt($(this).attr('hiddenVal'));
        if (discountAmount > billingTotal) {
            discountAmount = billingTotal;
            $(this).val(discountAmount).trigger('changeValue');
        }
        else if (discountAmount < 0) {
            discountAmount = 0;
            $(this).val(discountAmount).trigger('changeValue');
        }
        var discountPercentage = 0;
        if (billingTotal != 0)
            discountPercentage = discountAmount * 100 / billingTotal;
        $('#<%=txtPayerDiscountPercentage.ClientID %>').val(discountPercentage);
        $('#<%=txtTotalPayer.ClientID %>').val(billingTotal - discountAmount).trigger('changeValue');
    });
</script>

<input type="hidden" id="hdnPatientBillingID" runat="server" />
<div class="pageTitle"><%=GetLabel("Ubah Bill Diskon")%></div>
<table class="tblContentArea" style="width:100%;">
    <colgroup>
        <col style="width:50%"/>
        <col style="width:50%"/>
    </colgroup>
    <tr>
        <td colspan="2">
            <table class="tblEntryContent" style="width:100%">
                <colgroup>
                    <col style="width:150px"/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Alasan Discount")%></label></td>
                    <td><dxe:ASPxComboBox ID="cboDiscountReason" Width="200px" runat="server" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="padding:5px;vertical-align:top;">
            <h4><%=GetLabel("Pasien")%></h4>
            <div class="containerTblEntryContent">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label runat="server"><%=GetLabel("Total Tagihan")%></label></td>
                        <td><asp:TextBox ID="txtTotalPatientAmount" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diskon Final")%> [%]</label></td>
                        <td><asp:TextBox ID="txtPatientDiscountPercentage" CssClass="number" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diskon Final")%> [Rp]</label></td>
                        <td><asp:TextBox ID="txtPatientDiscountAmount" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label id="Label2" runat="server"><%=GetLabel("Total Setelah Diskon")%></label></td>
                        <td><asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </div>
        </td>
        <td style="padding:5px;vertical-align:top;">
            <h4><%=GetLabel("Instansi")%></h4>
            <div class="containerTblEntryContent">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label id="Label1" runat="server"><%=GetLabel("Total Tagihan")%></label></td>
                        <td><asp:TextBox ID="txtTotalPayerAmount" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diskon Final")%> [%]</label></td>
                        <td><asp:TextBox ID="txtPayerDiscountPercentage" CssClass="number" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diskon Final")%> [Rp]</label></td>
                        <td><asp:TextBox ID="txtPayerDiscountAmount" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label id="Label3" runat="server"><%=GetLabel("Total Setelah Diskon")%></label></td>
                        <td><asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="txtCurrency" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
    