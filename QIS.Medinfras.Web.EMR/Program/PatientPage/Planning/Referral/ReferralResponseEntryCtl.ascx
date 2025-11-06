<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferralResponseEntryCtl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ReferralResponseEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtReplyDate.ClientID %>');
    $('#<%=txtReplyDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    $('#<%=txtDiagnosisText.ClientID %>').focus();

    function onAfterSaveRecordPatientPageEntry(value) {
        cbpView.PerformCallback('refresh');
    }

    $('#<%=btnCopyNote.ClientID %>').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnPopupVisitID.ClientID %>').val() + " AND ParamedicID = " + $('#<%=hdnPopupParamedicID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011','X011^002','X011^004') AND SubjectiveText IS NOT NULL";
        openSearchDialog('planningNote', filterExpression, function (value) {
            $('#<%=hdnPopupVisitNoteID.ClientID %>').val(value);
            onSearchPatientVisitNote(value);
        });
    });

    function onSearchPatientVisitNote(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPopupVisitNoteID.ClientID %>').val(result.ID);
                $('#<%=txtReplySubjectiveText.ClientID %>').val(result.SubjectiveText);
                $('#<%=txtReplyObjectiveText.ClientID %>').val(result.ObjectiveText);
                $('#<%=txtDiagnosisText.ClientID %>').val(result.AssessmentText);
                $('#<%=txtPlanningResumeText.ClientID %>').val(result.PlanningText);
                $('#<%=txtInstructionResumeText.ClientID %>').val(result.InstructionText);
            }
            else {
                $('#<%=hdnPopupVisitNoteID.ClientID %>').val('0');
                $('#<%=txtReplySubjectiveText.ClientID %>').val('');
                $('#<%=txtReplyObjectiveText.ClientID %>').val('');
                $('#<%=txtDiagnosisText.ClientID %>').val('');
                $('#<%=txtPlanningResumeText.ClientID %>').val('');
                $('#<%=txtInstructionResumeText.ClientID %>').val('');
            }
        });
    }

    registerCollapseExpandHandler();
</script>

<div style="height: 500px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitNoteID" value="0" />
    <input type="hidden" id="hdnIsCopyFromSource" runat="server" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td><asp:TextBox ID="txtReplyDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtReplyTime" Width="80px" CssClass="time" runat="server"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter ")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter Rujukan")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician2" ClientInstanceName="cboPhysician2" Width="300px" /></td>
        </tr>
    </table>
    <div>
        <h4 class="h4collapsed">
            <%=GetLabel("Catatan Konsultasi/Rawat Bersama")%></h4>
        <div class="containerTblEntryContent containerEntryPanel1">
            <table style="width:100%">
                <colgroup>
                    <col style="width:150px"/>
                    <col style="width:150px"/>
                    <col />
                </colgroup>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory"><%=GetLabel("Diagnosis Pasien:")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtSourceDiagnosisText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly=true/></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory"><%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtSourceMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly=true /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory"><%=GetLabel("Terapi yang telah diberikan :")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtSourcePlanningResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly=true /></td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <h4 class="h4expanded">
            <%=GetLabel("Respon Konsultasi/Rawat Bersama")%></h4>
        <div class="containerTblEntryContent containerEntryPanel1">
            <table style="width:100%">
                <colgroup>
                    <col style="width:150px"/>
                    <col style="width:150px"/>
                    <col />
                </colgroup>
                <tr>
                    <td colspan="3">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <colgroup>
                                <col style="width:450px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Sesuai permintaan konsultasi, pada kasus ini dijumpai : (S)")%></label>
                                </td>
                                <td id="tdCopyButton" runat="server" style="vertical-align:top;text-align:right">
                                    <input type="button" id="btnCopyNote" runat="server" value="Salin SOAP" class="btnCopyNote w3-btn w3-hover-blue"
                                        style="width: 100px;background-color: Red; color: White;" />
                                </td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtReplySubjectiveText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Catatan Pemeriksaan Fisik : (O)")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtReplyObjectiveText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian : (O)")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Diagnosis Pasien: (A)")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtDiagnosisText" Width="95%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblMandatory" style="font-weight:bold"><%=GetLabel("Tindakan atau Terapi yang disarankan : (P)")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtPlanningResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="3" /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <label class="lblNormal" style="font-weight:bold"><%=GetLabel("Catatan Instruksi : (I)")%></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><asp:TextBox ID="txtInstructionResumeText" Width="95%" runat="server" TextMode="MultiLine" Rows="3" /></td>
                </tr>
            </table>
        </div>
    </div>
</div>
