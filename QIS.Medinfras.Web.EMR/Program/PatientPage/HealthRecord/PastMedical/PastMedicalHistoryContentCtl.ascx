<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PastMedicalHistoryContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PastMedicalHistoryContentCtl" %>

<script id="dxis_patientinstructionquickpicks1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>' type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>' type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>' type='text/javascript'></script>
<script type="text/javascript" id="dxss_patientinstructionquickpicksctl">
    $(function () {
        $('#listInstruction<%=VisitID %>').show();
        $('#listInstruction<%=VisitID %>').accordion({ fillSpace: true });
    });
</script>

<style type="text/css">
    div.containerUl div { overflow-x: hidden; overflow-y:auto; }
    div.containerUl ul  { margin:0; padding:0; margin-left:25px; }
    div.containerUl ul:not(:first-child) { margin-top: 10px; }
    div.containerUl ul li  { font-size: 12px; }
    div.containerUl ul li span  { color:#3E3EE3; }
    div.containerUl a        { font-size:11px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
    div.containerUl a:hover  { text-decoration: underline; }
    div.headerHistoryContent  { text-align: left; border-bottom:1px groove black; font-size: 13px; margin:0; padding:0; margin-bottom: 5px; }
 .ChiefComplaintTextContent {
   white-space: pre-wrap
  }
</style>

<div class="headerHistoryContent">
    <div id="divRegistrationDate" runat="server"></div>
    <div id="divInformationLine2" runat="server"></div>
    <div id="divInformationLine3" runat="server"></div>
</div>
<div style="height:320px;width:340px">
    <div class="accordion" id="listInstruction<%=VisitID %>" style="display:none;text-align:left;">
        <a><%= GetLabel("Chief Complaint")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptChiefComplaint" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="ChiefComplaintTextContent"><%#: Eval("ChiefComplaintText")%></div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div> 
        <a><%= GetLabel("Diagnosis")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptDiagnosis" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="ChiefComplaintTextContent"><%#: Eval("DiagnosisText")%></div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div> 
        <a><%= GetLabel("Medication")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptMedication" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="ChiefComplaintTextContent"><%#: Eval("MedicationSummaryText")%></div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a><%= GetLabel("Test/Examination")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptTreatment" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="ChiefComplaintTextContent"><%#: Eval("TreatmentSummaryText")%></div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>