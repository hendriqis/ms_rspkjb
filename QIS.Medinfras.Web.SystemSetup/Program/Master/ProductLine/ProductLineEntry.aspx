<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ProductLineEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ProductLineEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Product Line")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Product Line Code")%></label></td>
                        <td><asp:TextBox ID="txtProductLineCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Product Line Name")%></label></td>
                        <td><asp:TextBox ID="txtProductLineName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Inventory")%></label></td>
                        <td><asp:TextBox ID="txtInventory" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Cost of Good Sold")%></label></td>
                        <td><asp:TextBox ID="txtCOGS" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Purchase")%></label></td>
                        <td><asp:TextBox ID="txtPurchase" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Purchase Return")%></label></td>
                        <td><asp:TextBox ID="txtPurchaseReturn" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Purchase Discount")%></label></td>
                        <td><asp:TextBox ID="txtPurchaseDiscount" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sales")%></label></td>
                        <td><asp:TextBox ID="txtSales" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sales Return")%></label></td>
                        <td><asp:TextBox ID="txtSalesReturn" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sales Discount")%></label></td>
                        <td><asp:TextBox ID="txtSalesDiscount" Width="200px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
