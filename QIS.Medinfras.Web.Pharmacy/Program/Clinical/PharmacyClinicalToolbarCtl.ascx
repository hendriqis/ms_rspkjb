<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PharmacyClinicalToolbarCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PharmacyClinicalToolbarCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<div class="pageTitle" style="height: 43px; margin-top: 5px;">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>'
        alt="" style="float: left; margin-top: 3px;" title="<%=GetLabel("Back")%>" />
    <div id="divVisitNote" runat="server" style="float: right; margin-top: 3px;">
        <img class="imgStatus" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
            alt="" title="<%=GetLabel("Catatan Pasien")%>" />
    </div>
    <asp:Repeater ID="rptHeader" runat="server" OnItemDataBound="rptHeader_ItemDataBound">
        <HeaderTemplate>
            <ul id="ulPatientPageHeader" class="ulNavigationPane">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<div class="containerUlTabPage">
    <asp:Repeater ID="rptMenuChild" runat="server" OnItemDataBound="rptMenuChild_ItemDataBound">
        <HeaderTemplate>
            <ul class="ulTabPage" id="ulTabMenuChild">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript" id="dxss_inpatientprescriptionordertoolbarctl">
    $(function () {
        $('#imgVisitNote.imgStatus').click(function () {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
        });
    });

    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/PatientList/PatientList1.aspx");
    }
</script>
