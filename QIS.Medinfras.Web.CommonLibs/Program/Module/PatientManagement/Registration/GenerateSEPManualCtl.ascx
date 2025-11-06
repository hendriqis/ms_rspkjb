<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateSEPManualCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GenerateSEPManualCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_GenerateSEPManualCtl">
    $(function () {
        setDatePicker('<%:txtTglSEP.ClientID %>');
        $('#<%:txtTglSEP.ClientID %>').datepicker('option', 'maxDate', 0);
    });

    $('#<%=txtJamSEP.ClientID %>').live('change', function () {
        var jamSEP = $('#<%=txtJamSEP.ClientID %>').val();
        checkTimeFormat(jamSEP);
    });

    function checkTimeFormat(value) {
        if (value.substr(2, 1) == ':') {
            if (!value.match(/^\d\d:\d\d/)) {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
            else if (parseInt(value.substr(0, 2)) >= 24 || parseInt(value.substr(3, 2)) >= 60) {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
        }
        else {
            displayErrorMessageBox('ERROR', "Format jam salah !");
        }
    }
     

</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnNoPeserta" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingVclaim" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingEklaim" value="" />

     <input type="hidden" runat="server" id="hdnMedicalNo" value="" />
     <input type="hidden" runat="server" id="hdnEKlaimMedicalNo" value="" />
     <input type="hidden" runat="server" id="hdnPatientName" value="" />
     <input type="hidden" runat="server" id="hdnDOB" value="" />
     <input type="hidden" runat="server" id="hdnGender" value="" />
     <input type="hidden" runat="server" id="hdnoldNoSep" value = "" />
     
    <input type="hidden" id="hdnIsSendEKlaimMedicalNo" runat="server" value="" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatient" ReadOnly="true" Width="90%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("No. SEP")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtNoSEP" runat="server" Width="150px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal SEP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTglSEP" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Jam SEP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtJamSEP" Width="100px" runat="server" CssClass="time" />
            </td>
        </tr>
    </table>
</div>
