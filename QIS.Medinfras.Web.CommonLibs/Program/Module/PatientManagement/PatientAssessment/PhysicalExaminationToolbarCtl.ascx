<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicalExaminationToolbarCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PhysicalExaminationToolbarCtl" %>
<div class="pageTitle" style="height: 43px; margin-top: 5px;">
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>'
        alt="" style="float: left; margin-top: 3px;" title="<%=GetLabel("Back")%>" />
    <div style="float: right; margin-top: 3px;">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div id="divVisitNote" runat="server">
                        <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                            alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:Repeater ID="rptMenuHeader" runat="server" OnItemDataBound="rptMenuHeader_ItemDataBound">
        <HeaderTemplate>
            <ul id="ulPatientPageHeader" class="ulNavigationPane">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server">
                <%#:Eval("MenuCaption") %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=pe");
    }

    $(function () {
        $('#imgVisitNote.imgLink').click(function () {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
        });
    });
</script>
