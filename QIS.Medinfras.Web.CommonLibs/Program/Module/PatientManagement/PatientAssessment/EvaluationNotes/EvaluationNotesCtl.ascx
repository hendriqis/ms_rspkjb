<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluationNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EvaluationNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtJournalDate.ClientID %>');
    $('#<%=txtJournalDate.ClientID %>').datepicker('option', 'maxDate', '0');
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
<div style="height: 330px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
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
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal")%>
                                -
                                <%=GetLabel("Jam")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJournalDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtJournalTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Perawat")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="235px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Situation") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSituation" runat="server" Width="100%" TextMode="Multiline"
                                Rows="3" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Background") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtBackground" runat="server" Width="100%" TextMode="Multiline"
                                Rows="3" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Assessment") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAssessment" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Rekomendasi") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRecommendation" runat="server" Width="100%" TextMode="Multiline"
                                Rows="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Lainnya") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtEvaluationRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="10" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
