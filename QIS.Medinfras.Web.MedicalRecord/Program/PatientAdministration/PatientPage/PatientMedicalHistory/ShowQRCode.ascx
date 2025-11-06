<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowQRCode.ascx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ShowQRCode" %>

<div align="center">
<input type="hidden" value="" id="hdnRegIDTransfer" runat="server" />
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 220px" />
            <col />
        </colgroup>
        <tr id="trPayer" runat="server">
            <td class="tdLabel">
                <label class="lblNormal" runat="server" id="lblNoReg">
                    <%:GetLabel("Penjamin Bayar")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 175px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPayer" Width="250px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr id="tr1" runat="server">
            <td class="tdLabel">
                <label class="lblNormal" runat="server" id="Label1">
                    <%:GetLabel("Expired Date")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 175px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtExpiredDate" Width="250px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
    </table>
    <asp:Button ID="btnGenerate" runat="server" Text="Generate" onclick="btnGenerate_Click" />
    <asp:PlaceHolder ID="plBarCode" runat="server" />
</div>