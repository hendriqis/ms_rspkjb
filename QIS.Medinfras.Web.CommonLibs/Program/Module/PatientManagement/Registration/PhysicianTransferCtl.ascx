<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicianTransferCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PhysicianTransferCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_registrationeditctl">
    function onGetEntryPopupReturnValue() {
        var result = $('#<%=txtPhysicianCode.ClientID %>').val();
        return result;
    }

    //#region Physician
    function onGetPatientVisitParamedicFilterExpression() {
        var filterExpression = "<%:OnGetParamedicFilterExpression() %>";
        return filterExpression;
    }

    $('#lblPatientVisitPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPatientVisitPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPatientVisitPhysicianCodeChanged($(this).val());
    });

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                cboRegistrationEditSpecialty.SetValue('');
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null)
            $('#<%:hdnParamedicID.ClientID %>').val(s.GetValue());
        else
            $('#<%:hdnParamedicID.ClientID %>').val('');
    }

    function onCboTransferReasonChanged(s) {
        $txt = $('#<%=txtOtherReason.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1) {
            $txt.show();
            $txt.focus();
        }
        else
            $txt.hide();
    }
</script>
<div style="height: 350px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnMRNCtl" value="" />
    <input type="hidden" runat="server" id="hdnGender" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationID" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. RM")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtMRN" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Ruang Perawatan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table style="width:100%">
                    <colgroup width="70px" />
                    <colgroup />
                    <tr>
                        <td>
                            <img src='<%=ResolveUrl("~/Libs/Images/warning.png")%>' alt="" height="65px" width="65px" />
                        </td>
                        <td style="vertical-align:top;">
                            <h4 style="background-color:transparent;color:red;font-weight:bold"><%=GetLabel("PERINGATAN : Proses tidak bisa dibatalkan")%></h4>
                            <%=GetLabel("Jika terjadi kesalahan, pasien harus ditransfer ulang dari Dokter Tujuan ke Dokter Asal")%>
                            <br />
                            <%=GetLabel("Pastikan Dokter Tujuan sudah benar.")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory" id="lblPatientVisitPhysician">
                    <%=GetLabel("DPJP Utama Saat Ini")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter Pengganti")%></label></td>
            <td>
                <dxe:ASPxComboBox runat="server" ID="cboPhysician2" ClientInstanceName="cboPhysician2" Width="100%">
                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }" Init="function(s,e){ onCboPhysicianChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal lblMandatory">
                    <%=GetLabel("Alasan Transfer")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboTransferReason" ClientInstanceName="cboTransferReason" Width="150px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTransferReasonChanged(s); }" Init="function(s,e){ onCboTransferReasonChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOtherReason" CssClass="txtOtherReason" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
