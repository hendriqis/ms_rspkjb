<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagingResultCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ImagingResultCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $('.lnkDetail a').live('click', function () {
    });
</script>
<style type="text/css">    
    .tblTestItem
    {
        border: 1px solid #AAA;
        margin-top: 10px;
    }
    .tblTestItem > tbody > tr > th
    {
        background-color: #EAEAEA;
        padding: 5px;
        border-bottom: 1px solid #AAA;
    }
    .tblTestItem > tbody > tr > td
    {
        padding: 5px;
    }
</style>
<div style="height: 370px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 50%" />
            <col />
        </colgroup>
        <tr>
            <td valign="top">
                <table id="Table2" runat="server" cellpadding="0">
                    <colgroup>
                        <col style="width: 250px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Nomor Radiologi")%>
                        </td>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:TextBox ID="txtTransactionNo" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal / Jam Hasil")%>
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtResultDate" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                        <td style="padding-left: 6px">
                            <asp:TextBox ID="txtResultTime" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table id="Table1" runat="server" cellpadding="0">
                    <colgroup>
                        <col style="width: 250px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Nomor Order")%>
                        </td>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:TextBox ID="txtOrderNumber" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal / Jam Order")%>
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtTestOrderDate" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                        <td style="padding-left: 6px">
                            <asp:TextBox ID="txtTestOrderTime" Width="100%" ReadOnly="true" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Diorder Oleh") %>
                        </td>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:TextBox ID="txtOrderedBy" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
        <Columns>
            <asp:TemplateField HeaderText="Pemeriksaan" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="350px">
                <ItemTemplate>
                    <div style="font-style: italic">
                        <%#: Eval("ItemName") %></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Hasil" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="350px">
                <ItemTemplate>
                    <div>
                        <asp:Literal ID="literal" Text='<%# Eval("TestResult1") %>' Mode="PassThrough" runat="server" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Verifikasi" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <div>
                        <%#: Eval("VerifiedByName") %></div>
                    <div>
                        <%#: Eval("VerifiedDateInString") %></div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("No Data To Display")%>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
