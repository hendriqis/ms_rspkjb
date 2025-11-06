<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BPJSClaimToolbarCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.BPJSClaimToolbarCtl" %>

<div class="pageTitle" style="height:43px; margin-top: 5px;">
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" style="float:left; margin-top: 3px;" title="<%=GetLabel("Back")%>" /> 
    <ul id="ulPatientPageHeader" class="ulNavigationPane">
        <li id="liToolbarPatientDiagnoseClaim" runat="server" url="~/Program/BPJS/ClaimDiagnoseProcedure/ClaimDiagnoseEntry.aspx"><%=GetLabel("Diagnosa Klaim")%></li>
        <li id="liToolbarPatientProcedureClaim" runat="server" url="~/Program/BPJS/ClaimDiagnoseProcedure/ClaimProcedureEntry.aspx"><%=GetLabel("Prosedur / Tindakan Klaim")%></li>
        <li id="liToolbarPatientIntegratedNotesClaim" runat="server" url="~/Program/BPJS/ClaimDiagnoseProcedure/ClaimIntegratedNotes.aspx"><%=GetLabel("Integrated Notes")%></li>
        <li id="liToolbarPatientEpisodeSummaryClaim" runat="server" url="~/Program/BPJS/ClaimDiagnoseProcedure/ClaimEpisodeSummary.aspx"><%=GetLabel("Episode Summary")%></li>
    </ul>
</div>

<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/BPJS/ClaimDiagnoseProcedure/ClaimDiagnoseProcedureList.aspx");
    }
</script>