<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.VitalSignEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_vitalsignentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $('#lblIntegratedNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^005','X011^012','X011^016')";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnVisitNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
            }
            else {
                $('#<%=hdnVisitNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        });
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
        margin-right: 10px;
    }
    .tdVitalSignHeader
    {
        padding: 1px;
        background-color: #09abd2;
        color: Black;
        border: 1px solid gray;
        text-align: center;
    }
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnVisitNoteID" value="" />
    <table class="tblEntryContent">
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
                            <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("PPA")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <%=GetLabel("Catatan Tambahan") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Status Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                <colgroup>
                                    <col style="width: 20px" />
                                    <col style="width: 100px" />
                                    <col style="width: 20px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsFallRisk" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <%=GetLabel("Resiko Jatuh")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsRAPUH" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <%=GetLabel("RAPUH")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsTerminalPatient" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <%=GetLabel("Pasien Terminal")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsDNR" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <%=GetLabel("DNR")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:CheckBox ID="chkIsInitialAssessment" runat="server" Text=" Bagian Pengkajian Awal Pasien" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblLink" id="lblIntegratedNoteID">
                                <%=GetLabel("Catatan Terintegrasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="50px" runat="server" TextMode="MultiLine" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea">
        <tr>
            <td style="padding-left: 15px">
                <div style="height: 400px;overflow-y: auto;" class="w3-container w3-border w3-animate-left peGroup">
                    <asp:Repeater ID="rptVitalSign" runat="server" OnItemDataBound="rptVitalSign_ItemDataBound">
                        <HeaderTemplate>
                            <ul id="ulVitalSign">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdVitalSignHeader">
                                            <input type="hidden" id="hdnVitalSignID" runat="server" value='<%#:Eval("VitalSignID") %>' />
                                            <input type="hidden" id="hdnVitalSignType" runat="server" value='<%#:Eval("GCValueType") %>' />
                                            <%#:Eval("VitalSignLabel") %>
                                            &nbsp;<span style="color: White"><%#:Eval("ValueUnit") %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divTxt" runat="server" visible="false">
                                                <asp:TextBox ID="txtVitalSignType" Width="97%" runat="server" Style="float: left" />
                                            </div>
                                            <div id="divDdl" runat="server" visible="false">
                                                <dxe:ASPxComboBox ID="cboVitalSignType" Width="100%" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <table id="tblVitalSignType" runat="server" cellpadding="0" cellspacing="1">
                </table>
            </td>
        </tr>
    </table>
    </table>
</div>
