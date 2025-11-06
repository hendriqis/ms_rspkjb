<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewNursePatientAssessmentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewNursePatientAssessmentCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ViewNursePatientAssessmentCtl">
    $(function () {
        if ($('#<%=hdnFormValuesViewCtl.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValuesViewCtl.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1")
                        $(this).prop('checked', true);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1")
                        $(this).prop('checked', true);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.txtNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
    });
</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnIDViewCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormTypeViewCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormLayoutViewCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormValuesViewCtl" value="" />
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("No. RM")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtMedicalNo" Width="120px" runat="server" ReadOnly />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Nama Pasien")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtPatientName" Width="450px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Tanggal Lahir")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtDateOfBirth" Width="180px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtRegistrationNo" Width="180px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal ")%>
                    -
                    <%=GetLabel("Jam ")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtObservationDate" Width="120px" runat="server" ReadOnly="true" />
                <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" ReadOnly="true" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("PPA")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtParamedicInfo" Width="350px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkIsInitialAssessment" runat="server" Text=" Bagian Pengkajian Awal Pasien"
                    Enabled="false" />
            </td>
        </tr>
        <tr id="trIsNeedVerify" runat="server">
            <td class="tdLabel">
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkIsNeedVerify" runat="server" Text=" Perlu Verifikasi Ahli Gizi"
                    Enabled="false" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <div id="divFormContent" runat="server" style="height: 300px; overflow-y: auto;">
                </div>
            </td>
        </tr>
        <tr id="trToddlerNutritionProblem" runat="server">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Masalah Gizi Balita")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboToddlerNutritionProblemViewCtl" Width="450px" runat="server" Enabled="false" />
            </td>
        </tr>
    </table>
</div>
