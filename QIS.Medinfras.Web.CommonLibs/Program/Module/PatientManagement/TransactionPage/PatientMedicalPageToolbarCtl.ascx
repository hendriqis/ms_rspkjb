<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientMedicalPageToolbarCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientMedicalPageToolbarCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<div class="pageTitle" style="height: 43px; margin-top: 5px;">
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnModuleID" value="" runat="server" />
    <input type="hidden" id="hdnParentCode" value="" runat="server" />
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
                <td>
                </td>
                <td>
                </td>
                <td class="tdSystemInfo">
                </td>
            </tr>
        </table>
    </div>
     <div  class="divNavigationPane">
    <asp:Repeater ID="rptHeader" runat="server" OnItemDataBound="rptHeader_ItemDataBound">
        <HeaderTemplate>
            <ul id="ulPatientPageHeader" class="ulNavigationPane">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
     </div>
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
<script type="text/javascript" id="dxss_transactionpagetoolbarctl">
    function onGetUrlReferrer() {
        if ($('#<%=hdnModuleID.ClientID %>').val() == 'MD' && $('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.VISIT_NOTE) {
            return ResolveUrl("~/Program/PatientList/NoteList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.FOLLOWUP_PATIENT) {
            return ResolveUrl("~/Program/PatientList/FollowupPatientDischargeList.aspx");
        }
        else {
            return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=ptp");
        }
    }

    $(function () {
        $('#imgVisitNote.imgLink').click(function () {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
        });
    });

</script>
