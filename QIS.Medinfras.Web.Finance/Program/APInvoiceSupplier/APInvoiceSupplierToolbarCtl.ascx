<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierToolbarCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierToolbarCtl" %>

<asp:Repeater ID="rptHeader" runat="server" OnItemDataBound="rptHeader_ItemDataBound">
    <HeaderTemplate>
        <ul id="ulPatientPageHeader" class="ulNavigationPane">
    </HeaderTemplate>  
    <ItemTemplate>
        <li id="liCaption" runat="server" url='<%#:Eval("MenuUrl") %>'><%#:Eval("MenuCaption") %></li>
    </ItemTemplate>  
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierList.aspx");
    }
</script>
