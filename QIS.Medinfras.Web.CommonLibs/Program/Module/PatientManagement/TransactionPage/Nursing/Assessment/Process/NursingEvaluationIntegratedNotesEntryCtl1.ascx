<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingEvaluationIntegratedNotesEntryCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingEvaluationIntegratedNotesEntryCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $(function () {
        setDatePicker('<%=txtNoteDate.ClientID %>');
        $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanel1Content");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();
    });
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Perawat")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                                <colgroup>
                                    <col style="width: 20%" />
                                    <col style="width: 80%" />
                                </colgroup>
                                <tr>
                                    <td style="vertical-align:top">
                                        <div id="leftPageNavPanel" class="w3-border">
                                            <ul>
                                                <li contentID="divPage1" title="Data Subjektif dari Diagnosa Keperawatan" class="w3-hover-red">Subjective</li>     
                                                <li contentID="divPage2" title="Data Objektif dari Diagnosa Keperawatan" class="w3-hover-red">Objective</li>    
                                                <li contentID="divPage3" title="Masalah Keperawatan" class="w3-hover-red">Assessment</li>                                                
                                                <li contentID="divPage4" title="Luaran + Ekspektasi dari tiap masalah serta scoringnya" class="w3-hover-red">Planning</li>     
                                                <li contentID="divPage5" title="Aktifitas dalam intervensi tiap masalah" class="w3-hover-red">Instruksi</li>                                                                                            
                                            </ul>     
                                        </div> 
                                    </td>
                                    <td style="vertical-align:top">
                                        <div id="divPage1" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                            <table class="tblEntryContent" style="width: 100%">
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtSubjectiveText" runat="server" Width="100%" TextMode="MultiLine" Rows="20"  />
                                                    </td>
                                                </tr>                                            
                                            </table>
                                        </div>
                                        <div id="divPage2" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                            <table class="tblEntryContent" style="width: 100%">
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtObjectiveText" runat="server" Width="100%" TextMode="MultiLine" Rows="20" />
                                                    </td>
                                                </tr>                                            
                                            </table>
                                        </div>
                                        <div id="divPage3" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                            <table class="tblEntryContent" style="width: 100%">
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtAssessmentText" runat="server" Width="100%" TextMode="MultiLine" Rows="20" />
                                                    </td>
                                                </tr>                                            
                                            </table>
                                        </div>
                                        <div id="divPage4" class=divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                            <table class="tblEntryContent" style="width: 100%">
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtPlanningText" runat="server" Width="100%" TextMode="MultiLine" Rows="20" />
                                                    </td>
                                                </tr>                                            
                                            </table>
                                        </div>
                                        <div id="divPage5" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                            <table class="tblEntryContent" style="width: 100%">
                                                <colgroup>
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtInstructionText" runat="server" Width="100%" TextMode="MultiLine" Rows="20" />
                                                    </td>
                                                </tr>                                            
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
