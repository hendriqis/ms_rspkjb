<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MCUPatientToolbarCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUPatientToolbarCtl" %>

<div class="pageTitle" style="height:43px; margin-top: 5px;">
    <img class="imgLink" id="btnBackToListToolbar" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" style="float:left; margin-top: 3px;" title="<%=GetLabel("Back")%>" /> 
    <ul id="ulPatientPageHeader" class="ulNavigationPane">
        <li id="liToolbarFormMCU" runat="server" url="~/Program/MCUResultForm/MCUResultFormEntry.aspx?id=18745"><%=GetLabel("Pengisian Hasil Form")%></li>
        <li id="liToolbarPatientDocument" runat="server" url="~/Program/PatientDocument/PatientDocumentList1.aspx?id=MEDICALCHECKUP"><%=GetLabel("e-Document")%></li>
 
    </ul>
</div>

<script type="text/javascript">
    function onGetUrlReferrer() {
        return ResolveUrl("~/Program/MCUResultForm/MCUResultFormEntry.aspx");
    }

    $('#btnBackToListToolbar').live('click', function () {
        showLoadingPanel();
        document.location = ResolveUrl("~/Program/PatientList/VisitList.aspx?id=resultform");
    });
</script>