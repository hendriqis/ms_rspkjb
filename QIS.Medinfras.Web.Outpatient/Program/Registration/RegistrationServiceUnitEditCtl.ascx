<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationServiceUnitEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Outpatient.Program.RegistrationServiceUnitEditCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_registrationeditctl">
    $(function () {
    });

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnRegistrationID.ClientID %>').val();
        return result;
    }

    //#region ServiceUnit
    function onGetServiceUnitFilterExpression() {
        var filterExpression = "<%:OnGetParamedicFilterExpression() %>";
        return filterExpression;
    }

    $('#lblServiceUnit.lblLink').live('click', function () {
        openSearchDialog('serviceunitbyparamedic', onGetServiceUnitFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            onoTxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
        onoTxtServiceUnitCodeChanged($(this).val());
    });

    function onoTxtServiceUnitCodeChanged(value) {
        var filterExpression = onGetServiceUnitFilterExpression() + " AND HealthcareServiceUnitID = '" + value + "'";
        Methods.getObject('GetvServiceUnitParamedicList', filterExpression, function (result) {
            if (result != null) {
                Methods.getObject('GetvHealthcareServiceUnitList', 'HealthcareServiceUnitID = ' + value, function (resultHSU) {
                    if (resultHSU != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(resultHSU.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(resultHSU.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(resultHSU.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
        });
    }
    //#endregion

</script>
<div style="height: 350px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnMRNCtl" value="" />
    <input type="hidden" runat="server" id="hdnGender" value="" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <input type="hidden" id="hdnParamedicID" value="0" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationID" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trMotherRegNoCtl" style="display: none" runat="server">
            <td class="tdLabel">
                <div style="position: relative;">
                    <label class="lblLink lblKey" id="lblMotherRegNoCtl">
                        <%:GetLabel("No. Registrasi Ibu")%></label></div>
            </td>
            <td>
                <input type="hidden" id="hdnMotherVisitIDCtl" value="" runat="server" />
                <input type="hidden" id="hdnMotherMRNCtl" value="" runat="server" />
                <input type="hidden" id="hdnMotherNameCtl" value="" runat="server" />
                <asp:TextBox ID="txtMotherRegNoCtl" Width="175px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No RM")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtMRN" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPhysician" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblServiceUnit">
                    <%=GetLabel("Klinik")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
