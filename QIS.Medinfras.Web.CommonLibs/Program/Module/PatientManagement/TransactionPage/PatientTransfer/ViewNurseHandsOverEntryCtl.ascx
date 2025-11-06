<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewNurseHandsOverEntryCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.ViewNurseHandsOverEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_nursePatientTransferEntryctl">
    function onAfterSaveRecordPatientPageEntry(value) {
        cbpView.PerformCallback('refresh');
    }

    function onSearchPatientVisitNote(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                var situationText = result.SubjectiveText + "\n" + result.ObjectiveText;
                var assessmentText = result.AssessmentText + "\n" + result.PlanningText;
                var recommendationText = result.InstructionText;
                $('#<%=txtSituationText.ClientID %>').val(situationText);
                $('#<%=txtBackgroundText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val(assessmentText);
                $('#<%=txtRecommendationText.ClientID %>').val(recommendationText);
            }
            else {
                $('#<%=txtSituationText.ClientID %>').val('');
                $('#<%=txtBackgroundText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val('');
                $('#<%=txtRecommendationText.ClientID %>').val('');
            }
        });
    }

</script>

<div style="height: 500px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:240px"/>
            <col style="width:150px"/>
            <col style="width:80px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td><asp:TextBox ID="txtTransferDate" Width="120px" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtTransferTime" Width="80px" CssClass="time" runat="server" ReadOnly="true"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dari Perawat")%></label></td>
            <td colspan="3"><asp:TextBox ID="txtFromParamedicInfo" Width="350px" runat="server" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Ke Perawat")%></label></td>
            <td colspan="3"><asp:TextBox ID="txtToParamedicInfo" Width="350px" runat="server" ReadOnly="true" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tipe Transfer")%></label></td>
            <td colspan="3">
                <asp:TextBox ID="txtTransferType" Width="350px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top" style="font-weight: bold;padding-top: 5px;vertical-align: top;">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Situation") %></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtSituationText" runat="server" Width="620px" TextMode="MultiLine"
                    Rows="4" ReadOnly="true"/>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Background")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtBackgroundText" Width="620px" runat="server" TextMode="MultiLine"
                    Rows="4" ReadOnly="true"/>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Assessment")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtAssessmentText" Width="620px" runat="server" TextMode="MultiLine"
                    Rows="4" ReadOnly="true"/>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Recommendation")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtRecommendationText" Width="620px" runat="server" TextMode="MultiLine"
                    Rows="4" ReadOnly="true" />
            </td>
        </tr>   
    </table>
</div>
