<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryEditRevenueSharingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryEditRevenueSharingCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript">
</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnGLTransactionIDedit" value="" />
    <input type="hidden" runat="server" id="hdnTransactionDtIDedit" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Akun")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGLAccountNo" Width="400px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Segment")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSegment" Width="150px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Reference No")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierPaymentNo" Width="150px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Debit Amount")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDebitAmount" Width="250px" CssClass="number" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Credit Amount")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreditAmount" Width="250px" CssClass="number" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Remarks")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="400px" TextMode="MultiLine" Rows="3" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
