<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicalExaminationToolbarCtlzz.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EmergencyCare.Program.PhysicalExaminationToolbarCtl" %>

<div class="pageTitle" style="height:43px; margin-top: 5px;">
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" style="float:left; margin-top: 3px;" title="<%=GetLabel("Back")%>" /> 
    <ul id="ulPatientPageHeader" class="ulNavigationPane">
        <li id="liToolbarPatientStatus" runat="server" url="~/Program/PhysicalExamination/ERPatientStatus.aspx" ><%=GetLabel("Status Pasien") %></li>
        <li id="liToolbarPatientExamination" runat="server" url="~/Program/PhysicalExamination/ERInitialPhysicalExam/ERInitialPhysicalExam.aspx"><%=GetLabel("Tanda Vital")%></li>
        <li id="liToolbarPatientPhysicalExamination" runat="server" url="~/Program/PhysicalExamination/ERPhysicalExam/ERPhysicalExam.aspx"><%=GetLabel("Riwayat Sistemik")%></li>
        <li id="liToolbarPatientProgressNote" style="width: 150px" runat="server" url="~/Program/PhysicalExamination/EREvaluationNotes/EREvaluationNotes.aspx"><%=GetLabel("Catatan Perkembangan")%></li>
    </ul>
</div>

<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=pe");
    }
</script>