<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HDVitalSignEntry1Ctl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.HDVitalSignEntry1Ctl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_PatientMedicalDeviceEntryCtl">
    $(function () {
        setDatePicker('<%=txtLogDate.ClientID %>');
        $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');
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
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnItemID" value="" />
    <input type="hidden" runat="server" id="hdnItemName" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnUserID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionDtID" value="" />
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
                        <col style="width: 180px" />
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
                            <asp:TextBox ID="txtLogDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Perawat/Tenaga Medis")%></label>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="235px" runat="server" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Quick Blood (QB)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQB" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> ml/menit
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Quick Infusion (Q Inf)")%></label>
                        </td>     
                        <td>
                            <asp:TextBox ID="txtQInf" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> ltr/jam
                        </td>                                     
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pressure Vena (PV)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPV" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> 
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Trans Membran Pressure")%></label>
                        </td>     
                        <td>
                            <asp:TextBox ID="txtTMP" Width="60px" CssClass="number" runat="server" Style="text-align: right" />
                        </td>                                     
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Ultra Filtrasi Goal")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUFG" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> 
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Ultra Filtrasi Rate")%></label>
                        </td>     
                        <td>
                            <asp:TextBox ID="txtUFR" Width="60px" CssClass="number" runat="server" Style="text-align: right" />
                        </td>                                     
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Ultra Filtrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUF" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> 
                        </td>
                        <td class="tdLabel">
                        </td>     
                        <td>
                        </td>                                     
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Temperature")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemp" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> °C
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Blood Pressure")%></label>
                        </td>     
                        <td>
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSysBP" Width="40px" CssClass="number" runat="server" Style="text-align: right" /> /
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDisBP" Width="40px" CssClass="number" runat="server" Style="text-align: right" />
                                    </td>
                                </tr>
                            </table>
                        </td>                                     
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Heart Rate (HR)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHR" Width="60px" CssClass="number" runat="server" Style="text-align: right" /> x/menit
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Respiration Rate (RR)")%></label>
                        </td>     
                        <td>
                            <asp:TextBox ID="txtRR" Width="40px" CssClass="number" runat="server" Style="text-align: right" /> x/menit
                        </td>                                     
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Conductivity") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtConductivity" runat="server" Width="99%" TextMode="Multiline"
                                 Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <%=GetLabel("Catatan Tambahan") %>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
