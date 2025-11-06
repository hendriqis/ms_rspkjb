<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="EREvaluationNotes.aspx.cs" Inherits="QIS.Medinfras.Web.EmergencyCare.Program.EREvaluationNotes" %>

<%@ Register Src="~/Libs/Program/Module/PhysicalExamination/PhysicalExaminationToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnEvaluationNotesSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhList" runat="server">
<script type="text/javascript">
    $('#<%=btnEvaluationNotesSave.ClientID %>').click(function (){
        if(IsValid(null, 'fsEvaluationNotes', 'mpEvaluationNotes'))
            onCustomButtonClick('save')
    });
</script>
<input type="hidden" id="hdnVisitID" runat="server" />
    <fieldset id="fsEvaluationNotes">
        <table style="width: 70%">
            <colgroup>
                <col style="width: 150px" />
            </colgroup>
            <tr>
                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                    <label class="lblNormal">
                        <%=GetLabel("Catatan Perkembangan") %></label>
                </td>
                <td>
                    <asp:TextBox ID="txtEvaluationNotes" runat="server" Width="100%" TextMode="Multiline" Rows="10" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
