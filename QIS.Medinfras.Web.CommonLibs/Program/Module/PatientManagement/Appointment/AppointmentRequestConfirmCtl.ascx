<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentRequestConfirmCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentRequestConfirmCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_patiententryctl2">
    function oncboSessionValueChanged(s) { }
    function onAfterSaveAddRecordEntryPopup(param) {
        pcRightPanelContent.Hide();
        createApmSuccess(param); 
    }
</script>
<div>
    <input type="hidden" id="hdnParamedicID" runat="server" />
    <input type="hidden" id="hdnServiceUnitID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitLaboratoryID" runat="server" />
    <input type="hidden" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitImagingID" runat="server" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" />
    
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 49%" />
        </colgroup>
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Rekam Medis")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalNo" runat="server" Width="100%" ReadOnly />
                        </td>
                    </tr>
                     <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" runat="server" Width="100%" ReadOnly />
                        </td>
                    </tr>
                     <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Klinik")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName" runat="server" Width="100%" ReadOnly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtParmedicName" runat="server" Width="100%" ReadOnly />
                        </td>
                    </tr>
                     <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateApm" runat="server" Width="100%" ReadOnly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Sesi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboSession" runat="server" ClientInstanceName="cboSession" Width="100%"  >
                                                <ClientSideEvents ValueChanged="function(s,e) { oncboSessionValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    

                     <tr id="trRemarks" runat="server" style="display:none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td><asp:TextBox ID="txtRemarks" runat="server" Width="100%" ReadOnly /> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</div>
