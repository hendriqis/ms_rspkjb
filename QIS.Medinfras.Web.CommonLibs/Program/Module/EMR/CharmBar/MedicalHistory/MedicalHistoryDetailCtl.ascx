<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalHistoryDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalHistoryDetailCtl" %>
<script id="dxis_patientinstructionquickpicks1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'
    type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'
    type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>'
    type='text/javascript'></script>
<script type="text/javascript" id="dxss_patientinstructionquickpicksctl">
    $(function () {
        $('#listInstruction<%=_visitID %>').show();
        $('#listInstruction<%=_visitID %>').accordion({ fillSpace: true });

        $('.btnViewPDF').live('click', function () {
            var textResultValue = $(this).attr('pdfValue');
            window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
        });

        //#region Discharge User
        var DischargePhysician = $('#<%=hdnDischargePhysician.ClientID %>').val();
        var DischargeRM = $('#<%=hdnDischargeRM.ClientID %>').val();
        if (DischargePhysician == 0 || DischargePhysician == "" || DischargePhysician == null) {
            if (DischargeRM == 0 || DischargeRM == "" || DischargeRM == null) {
                $("#trDischargePhysician").attr('style', 'display:none');
                $("#trDischargeRM").attr('style', 'display:none');
            }
            else {
                $("#trDischargePhysician").attr('style', 'display:none');
                $("#trDischargeRM").removeAttr('style');
            }
        }
        else {
            if (DischargeRM == 0 || DischargeRM == "" || DischargeRM == null) {
                $("#trDischargePhysician").removeAttr('style');
                $("#trDischargeRM").attr('style', 'display:none');
            }
            else {
                $("#trDischargePhysician").removeAttr('style');
                $("#trDischargeRM").removeAttr('style');
            }
        }
        //#endregion

        //#region Discharge
        var DischargeCondition = $('#<%=hdnDischargeCondition.ClientID %>').val();
        var DischargeMethod = $('#<%=hdnDischargeMethod.ClientID %>').val();
        var isPreventiveCare = $('#<%=hdnPreventiveCare.ClientID %>').val();
        var isCurativeCare = $('#<%=hdnCurativeCare.ClientID %>').val();
        var isRehabilitationCare = $('#<%=hdnRehabilitationCare.ClientID %>').val();
        var isPalliativeCare = $('#<%=hdnPalliativeCare.ClientID %>').val();
        if (DischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || DischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48) {
            $("#trDeathInfo").show();
            $("#trAppointment").hide();
            $('#trInpatientPhysician').hide();
            $('#trInpatientPhysician1').hide();
            $('#trInpatientPhysician2').hide();
            $('#trInpatientPhysician3').hide();
            $('#trReferrerGroup').attr('style', 'display:none');
            $('#trReferrer').attr('style', 'display:none');
            $('#trDischargeReason').attr('style', 'display:none');
            $('#trDischargeRemarks').attr('style', 'display:none');
        }
        else {
            if (DischargeMethod == Constant.DischargeRoutine.OTHER_HOSPITAL || DischargeMethod == Constant.DischargeRoutine.REFER_TO_OUTPATIENT || DischargeMethod == Constant.DischargeRoutine.REFER_TO_INPATIENT || DischargeMethod == Constant.DischargeRoutine.FORCE_DISCHARGE) {
                if (DischargeMethod == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    $('#trInpatientPhysician').hide();
                    $('#trInpatientPhysician1').hide();
                    $('#trInpatientPhysician2').hide();
                    $('#trInpatientPhysician3').hide();
                    $("#trAppointment").hide();
                    $('#trReferrerGroup').removeAttr('style');
                    $('#trReferrer').removeAttr('style');
                    $('#trDischargeReason').removeAttr('style');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
                else if (DischargeMethod == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                    $('#trInpatientPhysician').hide();
                    $('#trInpatientPhysician1').hide();
                    $('#trInpatientPhysician2').hide();
                    $('#trInpatientPhysician3').hide();
                    $("#trAppointment").show();
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
                else if (DischargeMethod == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                    $("#trAppointment").hide();
                    $('#trInpatientPhysician').show();
                    $('#trInpatientPhysician1').show();
                    $('#trInpatientPhysician2').show();
                    $('#trInpatientPhysician3').show();
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
                else if (DischargeMethod == Constant.DischargeRoutine.FORCE_DISCHARGE) {
                    $("#trAppointment").hide();
                    $('#trInpatientPhysician').hide();
                    $('#trInpatientPhysician1').hide();
                    $('#trInpatientPhysician2').hide();
                    $('#trInpatientPhysician3').hide();
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trDischargeRemarks').removeAttr('style');
                }
                else {
                    $("#trAppointment").hide();
                    $('#trInpatientPhysician').show();
                    $('#trInpatientPhysician1').show();
                    $('#trInpatientPhysician2').show();
                    $('#trInpatientPhysician3').show();
                    $('#trReferrerGroup').attr('style', 'display:none');
                    $('#trReferrer').attr('style', 'display:none');
                    $('#trDischargeReason').attr('style', 'display:none');
                    $('#trDischargeOtherReason').attr('style', 'display:none');
                    $('#trDischargeRemarks').attr('style', 'display:none');
                }
            }
            else {
                $("#trAppointment").hide();
                $('#trInpatientPhysician').hide();
                $('#trInpatientPhysician1').hide();
                $('#trInpatientPhysician2').hide();
                $('#trInpatientPhysician3').hide();
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trDischargeRemarks').attr('style', 'display:none');
            }
        }

        if (isPreventiveCare) {
            $("#trIsPreventiveCare").show();
        }
        else {
            $("#trIsPreventiveCare").hide();
        }

        if (isCurativeCare) {
            $("#trIsCurativeCare").show();
        }
        else {
            $("#trIsCurativeCare").hide();
        }

        if (isRehabilitationCare) {
            $("#trIsRehabilitationCare").show();
        }
        else {
            $("#trIsRehabilitationCare").hide();
        }

        if (isPalliativeCare) {
            $("#trIsPalliativeCare").show();
        }
        else {
            $("#trIsPalliativeCare").hide();
        }
        //endRegion
        var param = '<%= RegistrationID %>' + "|" + '<%= LinkedRegistrationID %>';
        $(".imgFooterToolbarInformation").each(function () {
            $(this).attr('isselected', "0");
            if ($(this).attr('keyField') == null) {
                $(this).attr('keyField', param);
            }
            setImageFooterToolbarInformation($(this), "normalsrc");
        });

        $(".imgFooterToolbarInformation").hover(
                function () {
                    setImageFooterToolbarInformation($(this), "hoversrc");
                }, function () {
                    if ($(this).attr('isselected') == '0')
                        setImageFooterToolbarInformation($(this), "normalsrc");
                    else
                        setImageFooterToolbarInformation($(this), "selectedsrc");
                }
           );

        $('.imgViewPrescriptionOrderLog').die('click');
        $('.imgViewPrescriptionOrderLog').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/MedicalSummary/PrescriptionOriginalCtl1.ascx");
            //var param = '<%= RegistrationID %>' + "|" + '<%= LinkedRegistrationID %>';
            var param = $(this).attr('keyField');
            if (param != null) {
                openUserControlPopup(url, param, "Log Pengiriman Order Resep", 700, 500, "X487^001");
            }
        });
    });

    function setImageFooterToolbarInformation(elm, type) {
        var src = $(elm).attr(type);
        $(elm).attr("src", src);
    }
</script>
<style type="text/css">
    div.containerUl ul
    {
        margin: 0;
        padding: 0;
        margin-left: 25px;
    }
    div.containerUl ul:not(:first-child)
    {
        margin-top: 10px;
    }
    div.containerUl ul li
    {
        list-style-type: none;
        font-size: 12px;
        padding-bottom: 5px;
    }
    div.containerUl ul li span
    {
        color: #3E3EE3;
    }
    div.containerUl a
    {
        font-size: 12px;
        color: #3E93E3;
        cursor: pointer;
        float: right;
        margin-right: 20px;
    }
    div.containerUl a:hover
    {
        text-decoration: underline;
    }
    div.headerHistoryContent
    {
        text-align: left;
        border-bottom: 1px groove black;
        font-size: 13px;
        margin: 0;
        padding: 5px;
        margin-bottom: 5px;
    }
    
    .divContentTitle
    {
        margin-left: 25px;
        font-weight: bold;
        font-size: 11px;
        text-decoration: underline;
    }
    .divContent
    {
        margin-left: 25px;
        font-weight: bold;
        font-size: 11px;
    }
    
    .divNotAvailableContent
    {
        margin-left: 25px;
        font-size: 11px;
        font-style: italic;
        color: red;
    }
    div.footerHistoryContent
    {
        text-align: left;
        border-bottom: 1px groove black;
        font-size: 13px;
        margin: 0;
        padding-top: 10px;
        margin-bottom: 5px;
        height: 45px;
    }
    .imgFooterToolbarInformation
    {
        width: 30px;
        height: 30px;
    }
</style>
<input type="hidden" value="" id="hdnHSUImagingIDCBCtl" runat="server" />
<input type="hidden" value="" id="hdnHSULaboratoryIDCBCtl" runat="server" />
<input type="hidden" value="" id="hdnRegistrationIDCBCtl" runat="server" />
<input type="hidden" value="" id="hdnLinkedRegistrationIDCBCtl" runat="server" />
<input type="hidden" value="" id="hdnDischargeMethod" runat="server" />
<input type="hidden" value="" id="hdnDischargeCondition" runat="server" />
<input type="hidden" value="" id="hdnDischargePhysician" runat="server" />
<input type="hidden" value="" id="hdnDischargeRM" runat="server" />
<input type="hidden" value="" id="hdnPreventiveCare" runat="server" />
<input type="hidden" value="" id="hdnCurativeCare" runat="server" />
<input type="hidden" value="" id="hdnRehabilitationCare" runat="server" />
<input type="hidden" value="" id="hdnPalliativeCare" runat="server" />
<div class="headerHistoryContent">
    <div id="divInformationLine2" runat="server" style="font-weight: bold">
    </div>
    <div id="divRegistrationDate" runat="server">
    </div>
    <div id="divInformationLine4" runat="server">
    </div>
    <div id="divInformationLine3" runat="server">
    </div>
</div>
<div style="height: 450px;">
    <div class="accordion" id="listInstruction<%=_visitID %>" style="display: none; text-align: left;">
        <a style="font-size: 12px;">
            <%= GetLabel("SUBJECTIVE")%></a>
        <div class="containerUl">
            <div class="divContentTitle">
                <%= GetLabel("CHIEF COMPLAINT & HPI")%></div>
            <asp:Repeater ID="rptChiefComplaint" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("HistoryDate")%>
                        <%#: Eval("HistoryTime")%>
                        <%#: Eval("ParamedicName")%></span>
                        <textarea style="padding-left: 10px; border: 0; width: 350px; height: 250px" readonly><%#: Eval("ChiefComplaintText")%></textarea>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divHPI" class="divContent" runat="server">
            </div>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("OBJECTIVE")%></a>
        <div class="containerUl">
            <div class="divContentTitle">
                <%= GetLabel("REVIEW OF SYSTEM")%></div>
            <asp:Repeater ID="rptReviewOfSystemHd" OnItemDataBound="rptReviewOfSystemHd_ItemDataBound"
                runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("ObservationDateInString")%>
                        <%#: Eval("ObservationTime")%>
                        <br />
                        <%#: Eval("ParamedicName")%></span>
                        <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left: 5px;">
                                    <strong>
                                        <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                        : </strong>&nbsp;
                                    <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <br style="clear: both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <hr />
            <div class="divContentTitle">
                <%= GetLabel("VITAL SIGN")%></div>
            <asp:Repeater ID="rptVitalSignHd" OnItemDataBound="rptVitalSignHd_ItemDataBound"
                runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("ObservationDateInString")%>
                        <%#: Eval("ObservationTime")%>
                        <br />
                        <%#: Eval("ParamedicName")%></span>
                        <asp:Repeater ID="rptVitalSignDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left: 5px;">
                                    <strong>
                                        <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %>
                                        : </strong>&nbsp;
                                    <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <br style="clear: both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("ASSESSMENT")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptDifferentDiagnosis" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("DiagnosisDate")%>
                        <%#: Eval("DiagnosisTime")%>
                        <%#: Eval("PhysicianName")%></span>
                        <div style="font-weight: bold">
                            <%#:Eval("DiagnosisText")%></div>
                        <div>
                            <%#: Eval("DiagnosisType")%></div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("PEMERIKSAAN : LABORATORIUM")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptLabTestOrder" runat="server" OnItemDataBound="rptLabTestOrder_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("TestDate")%>
                        <%#: Eval("TestTime")%>,
                        <%#: Eval("ItemName") %></span>
                        <asp:Repeater ID="rptLaboratoryDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <strong>
                                        <%#: DataBinder.Eval(Container.DataItem, "FractionName") %>
                                        : </strong>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" ?  (Eval("IsPanicRange").ToString() == "False" ? "Style='color:red;font-weight:bold'" : "Style='color:orange;font-weight:bold'") : "Style='color:black'" %>>
                                            <%#: DataBinder.Eval(Container.DataItem, "ResultValue") %>
                                        </span>
                                    <%#: DataBinder.Eval(Container.DataItem, "ResultUnit") %>&nbsp;&nbsp; <span <%# (Eval("RefRange").ToString() == "" || Eval("IsResultInPDF").ToString() == "True") ? "Style='display:none'":"Style='color:black;font-style:italic'" %>>
                                        (<%#: DataBinder.Eval(Container.DataItem, "RefRange") %>)</span> <span <%# Eval("IsResultInPDF").ToString() == "False" ? "Style='display:none'":"" %>>
                                            <input type="button" id="btnViewPDF" runat="server" class="btnViewPDF" value="View PDF"
                                                style="height: 25px; width: 100px; background-color: Red; color: White;" pdfvalue='<%#: DataBinder.Eval(Container.DataItem, "PDFValue") %>' /></span>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <br style="clear: both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divLaboratoryNA" class="divNotAvailableContent" runat="server">
                This information is not available</div>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("PEMERIKSAAN : RADIOLOGI")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptImagingTestOrder" runat="server" OnItemDataBound="rptImagingTestOrder_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("TestDate")%>
                        <%#: Eval("TestTime")%>,
                        <%#: Eval("ItemName") %></span>
                        <asp:Repeater ID="rptImagingTestOrderDt" runat="server">
                            <ItemTemplate>
                                <textarea style="padding-left: 10px; border: 0; width: 350px; height: 150px" readonly><%#: DataBinder.Eval(Container.DataItem, "Resultvalue") %> </textarea>
                            </ItemTemplate>
                            <FooterTemplate>
                                <br style="clear: both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div id="divImagingNA" class="divNotAvailableContent" runat="server">
                This information is not available</div>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("CATATAN : HASIL PEMERIKSAAN PENUNJANG DAN TINDAKAN")%></a>
        <div class="containerPreviousVisitUl">
            <div class="divContentTitle" style="margin-top: 10px; margin-bottom: 5px">
                <%= GetLabel("Catatan : Hasil Pemeriksaan Penunjang")%></div>
            <%--<div id="divDiagnosticResultSummaryContent" class="divContent" runat="server" style="white-space:nowrap; overflow:auto"></div>--%>
            <textarea style="padding-left: 10px; border: 0; width: 80%; height: 150px" readonly="readonly"
                id="taDiagnosticResultSummaryContent" runat="server"></textarea>
            <div class="divContentTitle" style="margin-top: 10px; margin-bottom: 5px">
                <%= GetLabel("Catatan Tindakan")%></div>
            <%--<div id="divPlanningSummary" class="divContent" runat="server" style="white-space:nowrap; overflow:auto"></div>--%>
            <textarea style="padding-left: 10px; border: 0; width: 80%; height: 150px" readonly="readonly"
                id="taPlanningSummary" runat="server"></textarea>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("TERAPI/MEDICATION")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptMedication" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div>
                            <span <%# Eval("IsRFlag").ToString() == "0" ? "Style='display:none'":"Style='color:black;font-weight:bold'" %>>
                                R/&nbsp;&nbsp</span> <strong>
                                    <%#: Eval("InformationLine1")%></strong>
                            <img class="imgUDD blink-alert" <%# Eval("IsUsingUDD").ToString() == "True" ?  "" : "style='display:none'" %>
                                title='<%=GetLabel("UDD Medication")%>' src='<%# ResolveUrl("~/Libs/Images/Icon/uddMedication.png")%>'
                                height="20px" />
                        </div>
                        <div <%# Eval("IsCompound").ToString() == "0" ? "Style='display:none'":"Style='white-space: pre-line;color:black;font-style:italic;margin-left:20px'" %>>
                            <%#: Eval("MedicationLine")%>
                        </div>
                        <div style="color: Blue; width: 35px; float: left; margin-left: 20px">
                            DOSE</div>
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
            <div id="divMedicationNA" class="divNotAvailableContent" runat="server">
                This information is not available</div>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("PROCEDURE ATAU TINDAKAN")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptPatientProcedure" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("ProcedureDate")%>
                        <%#: Eval("ProcedureTime")%>
                        <%#: Eval("ParamedicName")%></span>
                        <div style="white-space: normal; overflow: auto">
                            <%#: Eval("ProcedureText")%>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("CATATAN TERINTEGRASI : DOKTER")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptNotes" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("cfNoteDate")%>
                        <%#: Eval("NoteTime")%>,
                        <%#: Eval("ParamedicName")%></span>
                        <textarea style="padding-left: 10px; border: 0; width: 350px; height: 150px" readonly="readonly"><%#: DataBinder.Eval(Container.DataItem, "cfNoteSOAPI") %> </textarea>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("CATATAN TERINTEGRASI : PERAWAT DAN TENAGA MEDIS LAINNYA")%></a>
        <div class="containerUl">
            <asp:Repeater ID="rptNursingNotes" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><span>
                        <%#: Eval("cfNoteDate")%>
                        <%#: Eval("NoteTime")%>,
                        <%#: Eval("ParamedicName")%></span>
                        <textarea style="padding-left: 10px; border: 0; width: 350px; height: 150px" readonly="readonly"><%#: DataBinder.Eval(Container.DataItem, "cfNoteSOAPI") %> </textarea>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <a style="font-size: 12px;">
            <%= GetLabel("DISCHARGE AND FOLLOW-UP VISIT")%></a>
        <div class="containerUl">
            <div class="divContentTitle">
                <%= GetLabel("DISCHARGE")%></div>
            <asp:Repeater ID="rptDischargeInformation" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div id="trDischargePhysician" style="overflow: auto; display: none">
                            <label>
                                <%=GetLabel("Oleh Dokter : ")%></label><br />
                            <span>
                                <%#: Eval("cfPhysicianDischargedDateTimeOrderInString")%>
                                ,
                                <%#: Eval("PhysicianDischargedByName")%>
                            </span>
                        </div>
                        <div id="trDischargeRM" style="overflow: auto; display: none">
                            <label>
                                <%=GetLabel("Oleh Rekam Medis : ")%></label><br />
                            <span>
                                <%#: Eval("cfDischargeDateInString")%>
                                ,
                                <%#: Eval("AdminDischargedByName")%></span>
                        </div>
                        <br />
                        <div style="white-space: normal; overflow: auto">
                            <label style="font-weight: bold">
                                <%=GetLabel("Kondisi Keluar : ")%></label>
                            <%#: Eval("DischargeCondition")%><br />
                            <label style="font-weight: bold">
                                <%=GetLabel("Cara Keluar : ")%></label>
                            <%#: Eval("DischargeMethod")%>
                        </div>
                        <div id="trDischargeRemarks" style="display: none">
                            <td>
                                <label style="font-weight: bold">
                                    <%=GetLabel("Keterangan Cara Keluar :")%></label>
                            </td>
                            <td colspan="4">
                                <%#: Eval("DischargeRemarks")%>
                            </td>
                        </div>
                        <div id="trDeathInfo" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Death Date - Time :")%></label>
                            <%#: Eval("DateTimeOfDeathInString")%>
                        </div>
                        <div id="trInpatientPhysician" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("DPJP Utama :")%></label>
                            <%#: Eval("ReferralPhysicianName")%>
                        </div>
                        <div id="trInpatientPhysician1" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Jenis Kamar :")%></label>
                            <%#: Eval("RoomType")%>
                        </div>
                        <div id="trInpatientPhysician2" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Indikasi :")%></label>
                            <%#: Eval("HospitalizationIndication")%>
                        </div>
                        <div id="trInpatientPhysician3" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Jenis Pelayanan :")%></label>
                            <div id="trIsPreventiveCare" style="display: none">
                                <%#: Eval("PreventiveCare")%>
                            </div>
                            <div id="trIsCurativeCare" style="display: none">
                                <%#: Eval("CurativeCare")%>
                            </div>
                            <div id="trIsRehabilitationCare" style="display: none">
                                <%#: Eval("RehabilitationCare")%>
                            </div>
                            <div id="trIsPalliativeCare" style="display: none">
                                <%#: Eval("PalliativeCare")%>
                            </div>
                        </div>
                        <div id="trAppointment" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Klinik Rujukan :")%></label>
                            <%#: Eval("ReferralUnitName")%>
                            <br />
                            <label style="font-weight: bold">
                                <%=GetLabel("Dokter :")%></label>
                            <%#: Eval("ReferralPhysicianName")%>
                        </div>
                        <div id="trReferrerGroup" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Rujuk Ke :")%></label>
                            <%#: Eval("ReferrerGroup")%>
                        </div>
                        <div id="trReferrer" style="display: none">
                            <label style="font-weight: bold">
                                <%:GetLabel("Rumah Sakit / Faskes :")%></label>
                            <%#: Eval("ReferrerName")%>
                        </div>
                        <div id="trDischargeReason" style="display: none">
                            <label style="font-weight: bold">
                                <%=GetLabel("Alasan Ke Rumah Sakit lain :")%></label>
                            <%#: Eval("ReferralDischargeReason")%>
                        </div>
                        <div id="trDischargeOtherReason" style="display: none">
                            <%#: Eval("ReferralDischargeReasonOther")%>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div class="divContentTitle" style="margin-top: 10px; margin-bottom: 5px">
                <%= GetLabel("FOLLOW-UP VISIT")%></div>
            <asp:Repeater ID="rptFollowUpVisit" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div>
                            <span>
                                <%#: Eval("StartDateTimeInString")%>,
                                <%#: Eval("ParamedicName")%></span></div>
                        <%#: Eval("VisitTypeName")%>;
                        <%#: Eval("Notes")%>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="footerHistoryContent">
    <div id="divFooterPanel" runat="server" style="padding-left: 10px; font-weight: bold">
        <img id="imgViewPrescriptionOrderLog" class="imgLink imgViewPrescriptionOrderLog imgFooterToolbarInformation"
            normalsrc='<%= ResolveUrl("~/libs/Images/Toolbar/medication.png")%>' hoversrc='<%= ResolveUrl("~/libs/Images/Toolbar/medication_hover.png")%>'
            selectedsrc='<%= ResolveUrl("~/libs/Images/Toolbar/medication_selected.png")%>'
            pressedsrc='<%= ResolveUrl("~/libs/Images/Toolbar/medication_hover.png")%>' disabledsrc='<%= ResolveUrl("~/libs/Images/Toolbar/medication_disabled.png")%>'
            src='<%= ResolveUrl("~/libs/Images/Toolbar/medication.png")%>' title='Log Pengiriman Order Resep'
            alt="" height="30px" width="30px" />
    </div>
</div>
