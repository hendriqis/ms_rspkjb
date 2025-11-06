<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RadiotherapyProgramLogEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RadiotherapyProgramLogEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_obstetricHistoryEntryctl">
    setDatePicker('<%=txtLogDate.ClientID %>');
    $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshLogGrid == 'function')
            onRefreshLogGrid();
    }
    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshLogGrid == 'function')
            onRefreshLogGrid();
    }
</script>
<div style="height: 450px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnProgramLogID" value="" />
    <input type="hidden" runat="server" id="hdnProgramID" value="" />
    <input type="hidden" runat="server" id="hdnTotalFraction" value="" />
    <input type="hidden" value="" id="hdnLMPDate" runat="server" />
    <table>
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Fraksi Ke-")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionNo" Width="60px" CssClass="number" runat="server"/>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam Penyinaran")%></label>
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
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Energi")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCEnergy" ClientInstanceName="cboGCEnergy"
                                Width="50%" ToolTip="Energi">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Pesawat")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCPesawat" ClientInstanceName="cboGCPesawat"
                                Width="50%" ToolTip="Pesawat">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Rad")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCBeamUnit" ClientInstanceName="cboGCBeamUnit"
                                Width="50%" ToolTip="Unit Rad">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Accessories")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCAccess" ClientInstanceName="cboGCAccess"
                                Width="50%" ToolTip="Access">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Setup")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCSetup" ClientInstanceName="cboGCSetup"
                                Width="50%" ToolTip="Setup">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Posisi Meja")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 100px" />
                                    <col style="width: 80px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("VRT")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVRT" Width="60px" CssClass="number" runat="server"/> cm
                                    </td>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("LNG")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLNG" Width="60px" CssClass="number" runat="server"/> cm
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("LAT")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLAT" Width="60px" CssClass="number" runat="server"/> cm
                                    </td>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("ROT")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtROT" Width="60px" CssClass="number" runat="server"/> n°
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Durasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDuration" Width="60px" CssClass="number" runat="server"/> menit
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Jumlah Field")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNumberOfFields" Width="60px" CssClass="number" runat="server"/> 
                        </td>
                    </tr>  
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Total MU")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtMachineUnit" Width="60px" CssClass="number" runat="server"/> 
                        </td>
                    </tr>         
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Jumlah Isocenter")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtIsoCenter" Width="60px" CssClass="number" runat="server"/> 
                        </td>
                    </tr>                                                                                                                                                                          
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Tambahan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
