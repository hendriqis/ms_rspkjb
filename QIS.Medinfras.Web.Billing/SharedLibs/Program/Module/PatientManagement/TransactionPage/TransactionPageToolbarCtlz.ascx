<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionPageToolbarCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageToolbarCtl" %>

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
        return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=ptp");
    }
</script>