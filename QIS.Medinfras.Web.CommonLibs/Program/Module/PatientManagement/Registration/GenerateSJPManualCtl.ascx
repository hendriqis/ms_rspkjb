<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateSJPManualCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GenerateSJPManualCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_GenerateSJPManualCtl">
    $(function () {
        setDatePicker('<%:txtTglSJP.ClientID %>');
        $('#<%:txtTglSJP.ClientID %>').datepicker('option', 'maxDate', 0);
    });

    $('#<%=txtJamSJP.ClientID %>').live('change', function () {
        var jamSJP = $('#<%=txtJamSJP.ClientID %>').val();
        checkTimeFormat(jamSJP);
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
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="200px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatient" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("No. SJP")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtNoSJP" runat="server" Width="200px">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal SJP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTglSJP" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Jam SJP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtJamSJP" Width="120px" runat="server" CssClass="time" />
            </td>
        </tr>
    </table>
</div>
