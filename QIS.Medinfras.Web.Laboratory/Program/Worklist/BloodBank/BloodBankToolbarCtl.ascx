<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloodBankToolbarCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Laboratory.Program.BloodBankToolbarCtl" %>

<div class="pageTitle" style="height:43px; margin-top: 5px;">
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" style="float:left; margin-top: 3px;" title="<%=GetLabel("Back")%>" /> 
    <div class="divNavigationPane">
    <ul id="ulPatientPageHeader" class="ulNavigationPane">
        <li id="liToolbarPatientDiagnose" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientDiagnose/MRPatientDiagnose.aspx"><%=GetLabel("Diagnosa Pasien")%></li>
        <li id="liToolbarAllergy" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/Allergy/PatientAllergyList.aspx" visible="false" ><%=GetLabel("Alergi")%></li>
        <li id="liToolbarPatientProcedure" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientProcedure/MRPatientProcedure.aspx"><%=GetLabel("Tindakan Pasien") %></li>
        <li id="liToolbarPatientDischarge" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientDischarge/MRPatientDischarge.aspx"><%=GetLabel("Pasien Pulang") %></li>
        <li id="liToolbarCBGGrouper" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRProcessCBG/MRProcessCBG.aspx" visible="false"><%=GetLabel("INACBG Grouper") %></li>
        <li id="liToolbarPatientNotes" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientNotes/MRPatientNotes.aspx" visible="false"><%=GetLabel("Catatan Pasien") %></li>
        <li id="liIntegratedNotes" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRIntegratedNotes/MRIntegratedNotes.aspx"><%=GetLabel("Integrated Notes") %></li>
        <li id="liToolbarSOAP" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAP/MRPatientSOAP.aspx" visible="false"><%=GetLabel("SOAP") %></li>
        <li id="liEpisodeSummary" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MREpisodeSummary/MREpisodeSummary.aspx"><%=GetLabel("Episode Summary") %></li>
        <li id="liToolbarMedicalFolderStatus" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MRMedicalFolderStatus/MRMedicalFolderStatusEntry.aspx"><%=GetLabel("Analisa Berkas") %></li>
        <li id="liNursingNotes" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/NursingNotes3/NursingNotes3.aspx"><%=GetLabel("Konsultasi/Rawat Bersama") %></li>
        <li id="liMedicalResume" runat="server" url="~/Program/PatientMedicalRecord/MRPatientSOAP/MedicalResume/MedicalResumeList.aspx"><%=GetLabel("Medical Resume") %></li>
    </ul>
    </div>
</div>

<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=ps");
    }
</script>