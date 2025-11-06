<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentBillSummaryToolbarCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentBillSummaryToolbarCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<div class="pageTitle" style="height: 43px; margin-top: 5px;">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnIsAllowClosePatientWithoutBill" value="" runat="server" />
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>'
        alt="" style="float: left; margin-top: 3px;" title="<%=GetLabel("Back")%>" />
    <div style="margin-right:20px">
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
<script type="text/javascript" id="dxss_patientbillsummarytoolbarctl">
    $(function () {
    });

    function onGetUrlReferrer() {
        return ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentEntry1.aspx?id=OP");
    }

    function onCbpProcessRegistrationEndCallback(s) {
        var param = s.cpResult.split('|');
        hideLoadingPanel();
    }
</script>
<div style="display: none">
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessRegistration" ClientInstanceName="cbpProcessRegistration"
        ShowLoadingPanel="false" OnCallback="cbpProcessRegistration_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessRegistrationEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
