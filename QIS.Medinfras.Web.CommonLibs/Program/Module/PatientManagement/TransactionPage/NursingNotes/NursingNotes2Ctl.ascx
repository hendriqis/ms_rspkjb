<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingNotes2Ctl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingNotes2Ctl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $(function () {
        setDatePicker('<%=txtNoteDate.ClientID %>');
        $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

        $('#lblPhysicianNoteID').removeClass('lblLink');

        $("#<%=txtNoteText.ClientID %>").focus();
    });

    $('#lblPhysicianNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^002','X011^010','X011^011')";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
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
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnInstructionID" value="" />
    <input type="hidden" runat="server" id="hdnInstructionText" value="" />
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
                        <col style="width: 100px" />
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
                        <td class="tdLabel" style="vertical-align:top">
                            <input type="button" id="btnCopyNote" runat="server" class="btnCopyNote" value="Salin Catatan Terintegrasi"
                                style="height: 25px; width: 160px; background-color: Red; color: White;" />
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
                        <td class="tdLabel" valign="top" style="font-weight: bold;padding-top: 5px;">
                            <label class="lblNormal lblMandatory">
                                <%=GetLabel("Catatan") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNoteText" runat="server" Width="100%" TextMode="MultiLine"
                                Rows="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                            <label class="lblNormal lblMandatory">
                                <%=GetLabel("Instruksi Dokter")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInstruction" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="3" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="3">
                           <asp:CheckBox ID="chkIsCompleted" runat="server" Checked = "false" /> <%:GetLabel("Update Status Instruksi menjadi selesai / complete")%>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Cara Pemberian Instruksi")%></label>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                            runat="server" Width="300px" >
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblLink" id="lblPhysicianNoteID">
                                <%=GetLabel("Instruksi Dokter")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="60px" runat="server" TextMode="MultiLine" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Perlu Konfirmasi Dokter")%>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboPhysician" ClientInstanceName="cboPhysician" runat="server" Width="300px" >
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }" Init="function(s,e){ onCboPhysicianChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:CheckBox ID="chkIsNeedNotification" runat="server" Checked = "false" /> <%:GetLabel("Kirim Notifikasi Ke Unit")%>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server" Width="300px" >
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
