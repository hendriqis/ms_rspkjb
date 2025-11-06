<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentRequestChangePayerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Outpatient.Program.AppointmentRequestChangePayerCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        if ($('#<%=hdnGCCustomerType.ClientID %>').val() == 'X004^999') {
            $('#<%=trPayerCompany.ClientID %>').hide();
        }
        else {
            $('#<%=trPayerCompany.ClientID %>').show();
        }
    });
    //#region Payer Company
    function getPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    $('#lblPayerCompany').click(function () {
        openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
            $('#<%=txtPayerCompanyCode.ClientID %>').val(value);
            onTxtPayerCompanyCodeChanged(value);
        });
    });

    $('#<%=txtPayerCompanyCode.ClientID %>').change(function () {
        onTxtPayerCompanyCodeChanged($(this).val());
    });

    function onTxtPayerCompanyCodeChanged(value) {
        var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                $('#<%=hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
            }
            else {
                $('#<%=hdnPayerID.ClientID %>').val('');
                $('#<%=txtPayerCompanyCode.ClientID %>').val('');
                $('#<%=hdnGCTariffScheme.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboPayerValueChanged(s) {
        setTblPayerCompanyVisibility();
        $('#<%=hdnPayerID.ClientID %>').val('');
        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
        $('#<%=txtPayerCompanyName.ClientID %>').val('');
    }

    function setTblPayerCompanyVisibility() {
        if (cboPayer.GetValue() == 'X004^999') {
            $('#<%=trPayerCompany.ClientID %>').hide();
        }
        else {
            $('#<%=trPayerCompany.ClientID %>').show();
        }
    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnChargeClassID" value="" />
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" id="hdnGCCustomerType" runat="server" />
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
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Pendaftaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pembayar")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPayer" ClientInstanceName="cboPayer" Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trPayerCompany" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblPayerCompany">
                                <%=GetLabel("Instansi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                            <input type="hidden" id="hdnPayerID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 140px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
