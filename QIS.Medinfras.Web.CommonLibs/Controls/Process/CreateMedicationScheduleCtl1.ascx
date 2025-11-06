<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateMedicationScheduleCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CreateMedicationScheduleCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_medicationScheduleCtl">
</script>
<input type="hidden" id="hdnID" value="" runat="server" />
<input type="hidden" id="hdnItemID" value="" runat="server" />
<div style="height: 150px; overflow-y: auto"> 
    <table border="0" style="width:99%">
        <colgroup>
            <col style="width: 99%" />
        </colgroup>
        <tr>
            <td>
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table cellpadding="0" cellspacing="1" border="0">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 150px" />
                            <col style="width: 5px" />
                            <col style="width: 150px" />
                            <col style="width: 120px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblPatientInfo">
                                    <%=GetLabel("No. RM - Nama Pasien")%></label>
                            </td>   
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>                           
                             <td colspan="3">
                                <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                            </td>                                                
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblDrugInfo">
                                    <%=GetLabel("Obat")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtDrugName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Aturan Pemberian")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtSignaInfo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionDate">
                                    <%=GetLabel("Tanggal ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" Width="120px" ReadOnly="true" runat="server"
                                    CssClass="datepicker" />
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Durasi")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDuration" Width="100px" CssClass="number" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
