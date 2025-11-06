<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationBPJSUpdateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationBPJSUpdateCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_payerdetailentryctl">
    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });
</script>
<div style="height: 100px">
    <input type="hidden" runat="server" id="hdnIsHasRegBPJS" value="" />    
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <table class="tblContentArea">
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi") %></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr style='display:none'>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("No SEP") %></label>
            </td>
            <td>
                <asp:TextBox ID="txtSEPNo" ReadOnly="true" Width="100%" runat="server" />
            </td>        
        </tr>
    </table>
</div>
