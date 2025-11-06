<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="CreateTariffBookEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CreateTariffBookEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtEffectiveDate.ClientID %>');
            setDatePicker('<%=txtDocumentDate.ClientID %>');
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboHealthcare" Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Skema Tarif")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTariffScheme" Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Berlaku")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEffectiveDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("No. Buku Tarif")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDocumentNo" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Dokumen")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDocumentDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nomor Revisi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRevisionNo" Width="120px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 10px; font-weight: bold;">
                <%=GetLabel("Ringkasan Buku Tarif") %><br />
                <br />
                <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtDocumentSummary"
                    runat="server" CssClass="htmlEditor" />
                <div style="border: 1px solid #AAA; width: 100%; height: 300px" id="pnlDocumentSummary"
                    runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
