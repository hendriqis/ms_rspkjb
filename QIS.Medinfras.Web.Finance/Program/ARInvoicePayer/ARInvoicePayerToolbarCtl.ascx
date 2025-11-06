<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePayerToolbarCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerToolbarCtl" %>
<div class="divNavigationPane">
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
</div>
<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/ARInvoicePayer/ARInvoicePayerList.aspx");
    }
</script>