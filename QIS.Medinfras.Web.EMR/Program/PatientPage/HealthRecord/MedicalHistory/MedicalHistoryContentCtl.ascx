<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalHistoryContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.MedicalHistoryContentCtl" %>

<script id="dxis_patientinstructionquickpicks1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>' type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>' type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>' type='text/javascript'></script>
<script type="text/javascript" id="dxss_patientinstructionquickpicksctl">
    $(function () {
        $('#listInstruction<%=_visitID %>').show();
        $('#listInstruction<%=_visitID %>').accordion({ fillSpace: true });
    });
</script>

<style type="text/css">
    div.containerUl ul  { margin:0; padding:0; margin-left:25px;}
    div.containerUl ul:not(:first-child) { margin-top: 10px; }
    div.containerUl ul li  { list-style-type:none; font-size: 12px; padding-bottom:5px; }
    div.containerUl ul li span  { color:#3E3EE3; }
    div.containerUl a        { font-size:12px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
    div.containerUl a:hover  { text-decoration: underline; }
    div.headerHistoryContent  { text-align: left; border-bottom:1px groove black; font-size: 13px; margin:0; padding:0; margin-bottom: 5px; }
    
    .divContentTitle { margin-left:25px; font-weight:bold; font-size:11px; text-decoration:underline}
    .divContent { margin-left:25px; font-weight:bold; font-size:11px}
    
    .divNotAvailableContent { margin-left:25px; font-size:11px; font-style:italic; color:red}
</style>

<input type="hidden" value="" id="hdnHSUImagingID" runat="server" />
<input type="hidden" value="" id="hdnHSULaboratoryID" runat="server" />
<div class="headerHistoryContent">
    <div id="divInformationLine2" runat="server" style="font-weight:bold"></div>
    <div id="divRegistrationDate" runat="server"></div>
    <div id="divInformationLine3" runat="server"></div>
</div>
<div style="height:450px;">
    <div class="accordion" id="listInstruction<%=_visitID %>" style="display:none;text-align:left;">
        <a style="font-size:12px;"><%= GetLabel("SUBJECTIVE")%></a> 
        <div class="containerUl">
            <div class="divContentTitle"><%= GetLabel("CHIEF COMPLAINT & HPI")%></div>
            <asp:Repeater ID="rptChiefComplaint" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("HistoryDate")%> <%#: Eval("HistoryTime")%> <%#: Eval("ParamedicName")%></span>
                        <textarea style="padding-left:10px;border:0; width:350px; height:250px" readonly><%#: Eval("ChiefComplaintText")%></textarea>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divHPI" class="divContent" runat="server"></div>
        </div>
        <a style="font-size:12px;"><%= GetLabel("OBJECTIVE")%></a> 
        <div class="containerUl">
            <div class="divContentTitle"><%= GetLabel("REVIEW OF SYSTEM")%></div>
            <asp:Repeater ID="rptReviewOfSystemHd" OnItemDataBound="rptReviewOfSystemHd_ItemDataBound" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("ObservationDateInString")%> <%#: Eval("ObservationTime")%> <br /> <%#: Eval("ParamedicName")%></span>
                        <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left:5px;">
                                    <strong> <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %> : </strong>&nbsp;
                                    <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <br style="clear:both"/>
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <hr />
            <div class="divContentTitle"><%= GetLabel("VITAL SIGN")%></div>
            <asp:Repeater ID="rptVitalSignHd" OnItemDataBound="rptVitalSignHd_ItemDataBound" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                       <span><%#: Eval("ObservationDateInString")%> <%#: Eval("ObservationTime")%> <br /> <%#: Eval("ParamedicName")%></span>
                       <asp:Repeater ID="rptVitalSignDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left:5px;">
                                    <strong><%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %> : </strong>&nbsp; 
                                    <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate> 
                                <br style="clear:both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size:12px;"><%= GetLabel("ASSESSMENT")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptDifferentDiagnosis" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("DiagnosisDate")%> <%#: Eval("DiagnosisTime")%> <%#: Eval("PhysicianName")%></span>
                        <div style="font-weight:bold"><%#:Eval("DiagnosisText")%></div>
                        <div><%#: Eval("DiagnosisType")%></div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size:12px;"><%= GetLabel("PEMERIKSAAN : LABORATORIUM")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptLabTestOrder" runat="server" OnItemDataBound="rptLabTestOrder_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("TestDate")%> <%#: Eval("TestTime")%>, <%#: Eval("ItemName") %></span>
                        <asp:Repeater ID="rptLaboratoryDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left:10px;">
                                    <strong><%#: DataBinder.Eval(Container.DataItem, "FractionName") %> : </strong>&nbsp;                                    
                                    <span <%# Eval("IsNormal").ToString() == "False" ?  (Eval("IsPanicRange").ToString() == "False" ? "Style='color:red;font-weight:bold'" : "Style='color:orange;font-weight:bold'") : "Style='color:black'" %>>
                                        <%#: DataBinder.Eval(Container.DataItem, "ResultValue") %>
                                    </span>                                    
                                    <%#: DataBinder.Eval(Container.DataItem, "ResultUnit") %>&nbsp;&nbsp;
                                    <span <%# Eval("RefRange").ToString() == "" ? "Style='display:none'":"Style='color:black;font-style:italic'" %>>(<%#: DataBinder.Eval(Container.DataItem, "RefRange") %>)</span>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate> 
                                <br style="clear:both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divLaboratoryNA" class="divNotAvailableContent" runat="server">This information is not available</div>
        </div>
        <a style="font-size:12px;"><%= GetLabel("PEMERIKSAAN : RADIOLOGI")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptImagingTestOrder" runat="server" OnItemDataBound="rptImagingTestOrder_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("TestDate")%> <%#: Eval("TestTime")%>, <%#: Eval("ItemName") %></span>
                        <asp:Repeater ID="rptImagingTestOrderDt" runat="server">
                            <ItemTemplate>
                                <textarea style="padding-left:10px;border:0;width:350px; height:150px" readonly><%#: DataBinder.Eval(Container.DataItem, "Resultvalue") %> </textarea>
                            </ItemTemplate>
                            <FooterTemplate> 
                                <br style="clear:both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divImagingNA" class="divNotAvailableContent" runat="server">This information is not available</div>
        </div>
        <a style="font-size:12px;"><%= GetLabel("CATATAN : HASIL PEMERIKSAAN PENUNJANG DAN TINDAKAN")%></a> 
        <div class="containerPreviousVisitUl">
            <div class="divContentTitle" style="margin-top:10px; margin-bottom: 5px"><%= GetLabel("Catatan : Hasil Pemeriksaan Penunjang")%></div>
            <div id="divDiagnosticResultSummaryContent" class="divContent" runat="server" style="white-space:nowrap; overflow:auto"></div>
            <div class="divContentTitle" style="margin-top:10px; margin-bottom: 5px"><%= GetLabel("Catatan Tindakan")%></div>
            <div id="divPlanningSummary" class="divContent" runat="server" style="white-space:nowrap; overflow:auto"></div>
        </div>
        <a style="font-size:12px;"><%= GetLabel("TERAPI/MEDICATION")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptMedication" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div>
                            <span <%# Eval("IsRFlag").ToString() == "0" ? "Style='display:none'":"Style='color:black;font-weight:bold'" %>>R/&nbsp;&nbsp</span>
                            <strong><%#: Eval("InformationLine1")%></strong>
                        </div>
                        <div <%# Eval("IsCompound").ToString() == "0" ? "Style='display:none'":"Style='white-space: pre-line;color:black;font-style:italic;margin-left:20px'" %>>
                            <%#: Eval("MedicationLine")%>
                        </div>
                        <div style="color: Blue; width: 35px; float: left;margin-left:20px">DOSE</div>
                        <%#: Eval("NumberOfDosage")%>
                        <%#: Eval("DosingUnit")%>
                        -
                        <%#: Eval("Route")%>
                        -
                        <%#: Eval("cfDoseFrequency")%>
                        -
                        <%#: Eval("MedicationAdministration")%>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divMedicationNA" class="divNotAvailableContent" runat="server">This information is not available</div>
        </div>
        <a style="font-size:12px;"><%= GetLabel("CATATAN TERINTEGRASI : DOKTER")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptNotes" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("cfNoteDate")%> <%#: Eval("NoteTime")%>, <%#: Eval("ParamedicName")%></span>
                        <textarea style="padding-left:10px;border:0; width:350px; height:150px" readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size:12px;"><%= GetLabel("CATATAN TERINTEGRASI : PERAWAT")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptNursingNotes" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <span><%#: Eval("cfNoteDate")%> <%#: Eval("NoteTime")%>, <%#: Eval("ParamedicName")%></span>
                        <textarea style="padding-left:10px;border:0; width:350px; height:150px" readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size:12px;"><%= GetLabel("DISCHARGE AND FOLLOW-UP VISIT")%></a> 
        <div class="containerUl">
            <asp:Repeater ID="rptDischargeInformation" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div><span><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></span></div>
                        <%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> 
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rptFollowUpVisit" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div><span><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></span></div>
                        <%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> 
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>