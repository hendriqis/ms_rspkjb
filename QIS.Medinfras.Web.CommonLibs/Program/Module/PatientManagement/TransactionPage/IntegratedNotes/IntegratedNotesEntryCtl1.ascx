<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntegratedNotesEntryCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.IntegratedNotesEntryCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $(function () {
        setDatePicker('<%=txtNoteDate.ClientID %>');
        $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

        $('#lblPhysicianNoteID').removeClass('lblLink');

        $("#<%=txtNoteText.ClientID %>").focus();
    });


    $('#<%=btnCopyNote.ClientID %>').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011','X011^012','X011^016') AND SubjectiveText IS NOT NULL";
        openSearchDialog('planningNote', filterExpression, function (value) {
            $('#<%=hdnPatientVisitNoteID.ClientID %>').val(value);
            onSearchPatientVisitNote(value);
        });
    });

    function onSearchPatientVisitNote(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtSubjectiveText.ClientID %>').val(result.SubjectiveText);
                $('#<%=txtObjectiveText.ClientID %>').val(result.ObjectiveText);
                $('#<%=txtAssessmentText.ClientID %>').val(result.AssessmentText);
                $('#<%=txtPlanningText.ClientID %>').val(result.PlanningText);
                $('#<%=txtInstructionText.ClientID %>').val(result.InstructionText);
            }
            else {
                $('#<%=txtSubjectiveText.ClientID %>').val('');
                $('#<%=txtObjectiveText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val('');
                $('#<%=txtPlanningText.ClientID %>').val('');
                $('#<%=txtInstructionText.ClientID %>').val('');
            }
        });
    }

    $('#lblLookupTTV.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val();
        openSearchDialog('vvitalsigndt2', filterExpression, function (value) {
            $('#<%=hdnVitalSignID.ClientID %>').val(value);
            onSearchVitalSignValue(value);
        });
    });

    function onSearchVitalSignValue(value) {
        var objText = $('#<%=txtObjectiveText.ClientID %>').val();
        var filterExpression = "ID = " + value;
        Methods.getObject('GetvVitalSignDt2List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtObjectiveText.ClientID %>').val(objText + result.cfVitalSignValue);
            }
            else {
                $('#<%=txtObjectiveText.ClientID %>').val('');
            }
        });
    }

    $('#lblPhysicianNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^002','X011^010','X011^011')";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetvPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.cfNoteSOAPI);
            }
            else {
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        });
    }

    function onCboPhysicianInstructionSourceChanged(s) {
        if (s.GetValue() != null && s.GetValue().indexOf('^01') > -1) {
            $('#lblPhysicianNoteID').addClass('lblLink');
        }
        else {
            $('#lblPhysicianNoteID').removeClass('lblLink');
            $('#<%=hdnPlanningNoteID.ClientID %>').val('');
            $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
        }
    }

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null) {
            $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
        }
        else
            $('#<%=hdnParamedicID.ClientID %>').val('');
    }

    $('#<%=rblNoteMode.ClientID %> input').change(function () {
        if ($(this).val() == '1') {
            $('#trSOAPINote').attr('style', 'display:none');
            $('#tdCopyButton').attr('style', 'display:none');
            $('#trFreeTextNote').removeAttr('style');
        }
        else {
            $('#trSOAPINote').removeAttr('style');
            $('#tdCopyButton').removeAttr('style');
            $('#trFreeTextNote').attr('style', 'display:none');
        }
    });
</script>
<style type="text/css">
    #ulVitalSign
    {
        margin: 0;
        padding: 0;
    }
    #ulVitalSign li
    {
        list-style-type: none;
        display: inline-block;
        padding-left: 5px;
        width: 48%;
    }
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" value="" id="hdnVitalSignID" runat="server" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
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
                                <%=GetLabel("PPA")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="Label1"><%=GetLabel("Mode")%></label></td>
                        <td colspan="2">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblNoteMode" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Free Text" Value="1" Selected="True" />
                                            <asp:ListItem Text=" SOAPI" Value="2" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td id="tdCopyButton" style="vertical-align:top; display:none">
                                        <input type="button" id="btnCopyNote" runat="server" value="Salin SOAP" class="btnCopyNote w3-btn w3-hover-blue"
                                            style="width: 100px;background-color: Red; color: White;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trFreeTextNote">
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Perawat (SOAP)") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoteText" runat="server" Width="100%" TextMode="Multiline"
                                Height="150px" />
                        </td>
                    </tr>
                    <tr id="trSOAPINote" style = "display: none">
                        <td colspan="3">
                            <table border="0" cellpadding = "0" cellspacing="1">
                                <colgroup>
                                    <col style="width: 195px" />
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="font-weight: bold;padding-top: 5px;vertical-align: top;">
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Subjective") %></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSubjectiveText" runat="server" Width="648px" TextMode="MultiLine"
                                            Rows="2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Objective")%></label>
                                            <br />
                                        <label class="lblLink" id="lblLookupTTV">
                                            <%=GetLabel("Lookup Tanda Vital")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtObjectiveText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Assessment")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAssessmentText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Planning")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPlanningText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="2" />
                                    </td>
                                </tr>    
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Instruksi")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtInstructionText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="2" />
                                    </td>
                                </tr>                                                               
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Cara Pemberian Instruksi")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 100px" />
                                    <col />
                                </colgroup>            
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                                        runat="server" Width="300px" >
                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                                        </dxe:ASPxComboBox>                                    
                                    </td>
                                    <td>
                                       <asp:CheckBox ID="chkIsWritten" runat="server" Checked = "false" /> <%:GetLabel("Tulis")%>
                                    </td>
                                    <td>
                                       <asp:CheckBox ID="chkIsReadback" runat="server" Checked = "false" /> <%:GetLabel("Baca")%>
                                    </td>
                                </tr>                    
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblLink" id="lblPhysicianNoteID">
                                <%=GetLabel("Instruksi Dokter")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="50px" runat="server" TextMode="MultiLine" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Perlu Konfirmasi Dokter")%>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPhysician" ClientInstanceName="cboPhysician" runat="server" Width="300px" >
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }" Init="function(s,e){ onCboPhysicianChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:CheckBox ID="chkIsNeedNotification" runat="server" Checked = "false" /> <%:GetLabel("Kirim Notifikasi Ke Unit")%>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server" Width="300px" >
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
