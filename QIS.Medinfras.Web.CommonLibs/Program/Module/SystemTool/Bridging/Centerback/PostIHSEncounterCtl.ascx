<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostIHSEncounterCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PostIHSEncounterCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sendADTNotificationCtl">

</script>
    
    <input type="hidden" id="hdnMessageType" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnEncounterID" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td>
            <fieldset id="fsEntryPopup" style="margin: 0">
                <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col style="width: 5px" />
                        <col style="width: 100px" />
                        <col style="width: 150px" />
                    </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Jenis Pengiriman")%></label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlNotificationType" runat="server" Width="100%" Enabled="false">
                                    <asp:ListItem Text="Kunjungan Awal (Post Encounter)" Value="01" />
                                    <asp:ListItem Text="Selesai Kunjungan (Put Encounter with Condition)" Value="02" />
                                    <asp:ListItem Text="" Value="03" />

<%--                                    <asp:ListItem Text="Tanda Vital" Value="05" />
                                    <asp:ListItem Text="Update Encounter with Condition" Value="05" />--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label5">
                                    <%=GetLabel("No. Registrasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionNo">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label6">
                                    <%=GetLabel("No. IHS Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientIHSNumber" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                             <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionDate">
                                    <%=GetLabel("Tanggal ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionDate" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pelepasan Informasi")%></label>
                            </td>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblSATUSEHAT" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                    <asp:ListItem Text=" Setuju" Value="1" />
                                    <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Consent")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtSatuSEHATConsentDate" Width="120px" runat="server" Enabled="false" />                                        
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal" id="Label4">
                                    <%=GetLabel("Parameter Integrasi")%></label>
                            </td>
                            <td colspan="4">

                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtNotificationDetail" Width="100%" ReadOnly="true" runat="server" TextMode="MultiLine" Rows="12" />
                            </td>
                        </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
