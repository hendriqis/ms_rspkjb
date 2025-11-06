<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntryLinkedToRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EntryLinkedToRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_EntryLinkedToRegistrationCtl">

    //#region Linked To Registration No
    function getLinkedToRegistrationNoFilterExpression() {
        var filterExpression = "DepartmentID = '" + cboDepartmentID.GetValue() + "' AND RegistrationID != " + $('#<%:hdnRegistrationIDCtl.ClientID %>').val();

        filterExpression += " AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.OPEN + "','" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "')";
        filterExpression += " AND RegistrationID IN (SELECT rg.RegistrationID FROM Registration rg WITH(NOLOCK) WHERE rg.GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED + "') AND rg.LinkedToRegistrationID IS NULL)";
        
        return filterExpression;
    }

    $('#<%:lblLinkedToRegistrationNo.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('registrationlinkedto', getLinkedToRegistrationNoFilterExpression(), function (value) {
            $('#<%:txtLinkedToRegistrationNoCtl.ClientID %>').val(value);
            ontxtLinkedToRegistrationNoCtlChanged(value);
        });
    });

    $('#<%:txtLinkedToRegistrationNoCtl.ClientID %>').live('change', function () {
        ontxtLinkedToRegistrationNoCtlChanged($(this).val());
    });

    function ontxtLinkedToRegistrationNoCtlChanged(value) {
        var filterExpression = getLinkedToRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
        Methods.getObject('GetvRegistration10List', filterExpression, function (result) {
            if (result != null) {
                var oRegistrationID = result.RegistrationID;
                var oRegistrationNo = result.RegistrationNo;
                var oPatient = "(" + result.MedicalNo + ") " + result.PatientName;
                var oServiceUnit = result.DepartmentID + " || " + result.ServiceUnitName;
                var oParamedic = result.ParamedicName;
                var oBusinessPartner = result.BusinessPartnerName;
                var oNoSEP = result.NoSEP;

                $('#<%:hdnLinkedToRegistrationIDCtl.ClientID %>').val(oRegistrationID);
                $('#<%:txtLinkedToRegistrationNoCtl.ClientID %>').val(oRegistrationNo);
                $('#<%:txtLinkedToPatientCtl.ClientID %>').val(oPatient);
                $('#<%:txtLinkedToServiceUnitCtl.ClientID %>').val(oServiceUnit);
                $('#<%:txtLinkedToParamedicCtl.ClientID %>').val(oParamedic);
                $('#<%:txtLinkedToBusinessPartnerCtl.ClientID %>').val(oBusinessPartner);
                $('#<%:txtLinkedToSEPNoCtl.ClientID %>').val(oNoSEP);
            }
            else {
                $('#<%:hdnLinkedToRegistrationIDCtl.ClientID %>').val('');
                $('#<%:txtLinkedToRegistrationNoCtl.ClientID %>').val('');
                $('#<%:txtLinkedToPatientCtl.ClientID %>').val('');
                $('#<%:txtLinkedToServiceUnitCtl.ClientID %>').val('');
                $('#<%:txtLinkedToParamedicCtl.ClientID %>').val('');
                $('#<%:txtLinkedToBusinessPartnerCtl.ClientID %>').val('');
                $('#<%:txtLinkedToSEPNoCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div style="height: 450px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 200px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNoCtl" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatientCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Unit Pelayanan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtServiceUnitCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dokter / Tenaga Medis")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtParamedicCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Penjamin Bayar")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtBusinessPartnerCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. SEP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtSEPNoCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr style="padding: 0 0 0 0; margin: 0 0 1 0;" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Department Tujuan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboDepartmentID" ClientInstanceName="cboDepartmentID" runat="server"
                    Width="100%" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblLinkedToRegistrationNo" runat="server">
                    <%:GetLabel("No Registrasi Tujuan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnLinkedToRegistrationIDCtl" value="" runat="server" />
                <input type="hidden" id="hdnLinkedToRegistrationIDCtlORI" value="" runat="server" />
                <asp:TextBox ID="txtLinkedToRegistrationNoCtl" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Pasien Tujuan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtLinkedToPatientCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Unit Pelayanan Tujuan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtLinkedToServiceUnitCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Dokter / Tenaga Medis Tujuan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtLinkedToParamedicCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Penjamin Bayar Tujuan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtLinkedToBusinessPartnerCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. SEP Tujuan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtLinkedToSEPNoCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
</div>
