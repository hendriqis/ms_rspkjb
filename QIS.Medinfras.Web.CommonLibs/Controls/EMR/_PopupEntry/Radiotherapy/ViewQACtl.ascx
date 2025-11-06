<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewQACtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewQACtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ViewQACtl">
</script>
<div style="height: 450px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnProgramQAID" value="" />
    <input type="hidden" runat="server" id="hdnProgramID" value="" />
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
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam QA")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQADate" Width="120px" runat="server" ReadOnly="True"/>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQATime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" ReadOnly="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Energi")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCEnergy" ClientInstanceName="cboGCEnergy"
                                Width="50%" ToolTip="Energi" Enabled="False">
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
                                Width="50%" ToolTip="Pesawat" Enabled="False">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Total Dosis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTotalDosage" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Jumlah Fraksi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTotalFraction" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> 
                        </td>
                    </tr>  
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Verifikasi")%></label>
                        </td>
                        <td colspan="2"> 
                            <dxe:ASPxComboBox runat="server" ID="cboGCVerificationType" ClientInstanceName="cboGCVerificationType"
                                Width="50%" ToolTip="Verifikasi" Enabled="False">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Total MU")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtMachineUnit" Width="60px" CssClass="number" runat="server" Enabled="False"/> 
                        </td>
                    </tr>                                                                                                                                                                               
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Tambahan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="True" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
