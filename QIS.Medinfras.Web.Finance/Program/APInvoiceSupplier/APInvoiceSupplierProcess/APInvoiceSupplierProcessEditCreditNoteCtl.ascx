<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcessEditCreditNoteCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcessEditCreditNoteCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    if ($('#<%=txtCreditNoteDate.ClientID %>').attr('readonly') == null) {
        setDatePicker('<%=txtCreditNoteDate.ClientID %>');
    }

    if ($('#<%=txtPurchaseReturnDate.ClientID %>').attr('readonly') == null) {
        setDatePicker('<%=txtPurchaseReturnDate.ClientID %>');
    }

    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    $('.txtCNAmountBeforePPN').live('change', function () {
        var txtCNAmountBeforePPN = $('#<%=txtCNAmountBeforePPN.ClientID %>').val();
        var IsIncludeVAT = $('#<%=hdnIsIncludeVAT.ClientID %>').val();
        var VATPercentage = parseFloat($('#<%=hdnVATPercentage.ClientID %>').val());
        var total = 0;

        if (IsIncludeVAT == "True") {
            total = txtCNAmountBeforePPN * (100 + VATPercentage) / 100;
        } else {
            total = txtCNAmountBeforePPN;
        }

        $('#<%=txtCNAmountAfterPPN.ClientID %>').val(total).trigger('changeValue');
    });
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnPurchaseReceiveID" runat="server" />
<input type="hidden" id="hdnPurchaseInvoiceDtID" runat="server" />
<input type="hidden" id="hdnVATPercentage" runat="server" />
<input type="hidden" id="hdnIsIncludeVAT" runat="server" />
<%--<div class="pageTitle">
    <%=GetLabel("Detail Nota Kredit")%></div>--%>
<div style="max-height: 440px; overflow-y: auto" id="containerPopup">
    <table style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <input type="hidden" id="hdnCreditNoteID" value="0" runat="server" />
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Nota Kredit")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditNoteNo" Width="150px" ReadOnly="true" runat="server" TabIndex="1" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Nota Kredit") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditNoteDate" Width="120px" CssClass="datepicker" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Supplier/Penyedia")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 20%" />
                                    <col style="width: 80%" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSupplierCode" ReadOnly="true" CssClass="required" ValidationGroup="mpEntry"
                                            Width="100%" runat="server" />
                                    </td>
                                    <td style="padding-left: 10px">
                                        <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Retur")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnPurchaseReturnID" value="" />
                            <asp:TextBox ID="txtPurchaseReturnNo" ReadOnly="true" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Retur") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseReturnDate" Width="120px" CssClass="datepicker" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Faktur") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNo" Width="120px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Faktur") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Faktur Pajak") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTaxInvoiceNo" Width="120px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Faktur Pajak") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTaxInvoiceDate" Width="120px" CssClass="datepicker" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe Nota Kredit")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGCCreditNoteType" ReadOnly="true" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPPN" Enabled="false" Width="100%" runat="server" />&nbsp;<%=GetLabel("PPN")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai (Sebelum PPN)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCNAmountBeforePPN" Width="150px" CssClass="txtCNAmountBeforePPN txtCurrency" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai (Setelah PPN)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCNAmountAfterPPN" Width="150px" CssClass="txtCNAmountAfterPPN txtCurrency" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
