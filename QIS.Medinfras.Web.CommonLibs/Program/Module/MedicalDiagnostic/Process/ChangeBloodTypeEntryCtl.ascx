<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeBloodTypeEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeBloodTypeEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChangeBloodTypeEntryCtl">
    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnRegistrationIDCtl" value="" runat="server" />
<input type="hidden" id="hdnMRNCtl" value="" runat="server" />
<div style="height: 200px; overflow-y: auto">
    <table border="0">
        <tr>
            <td>
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table cellpadding="0" cellspacing="1" border="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col style="width: 100px" />
                            <col style="width: 250px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtMedicalNoCtl" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Pasien")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPatientNameCtl" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblBloodTypeRhesus">
                                    <%=GetLabel("Golongan Darah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBloodType" ClientInstanceName="cboBloodType" Width="100%"
                                    runat="server" />
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBloodRhesus" ClientInstanceName="cboBloodRhesus" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
