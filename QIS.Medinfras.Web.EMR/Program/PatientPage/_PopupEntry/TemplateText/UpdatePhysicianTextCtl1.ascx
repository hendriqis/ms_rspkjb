<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdatePhysicianTextCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.UpdatePhysicianTextCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_testorderdetail1">
    $(function () {
        $('#<%:txtTemplateCode.ClientID %>').focus();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpView.PerformCallback('save');
        return false;
    });

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
                cbpView.PerformCallback('refresh');
            }
        }

        $('#containerPopupEntryData').hide();
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnTestOrderType" runat="server" value="" />
<input type="hidden" id="hdnTestOrderID" runat="server" value="" />
<input type="hidden" id="hdnItemID" runat="server" value="" />
<div id="containerPopupEntryData">
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col />
        </colgroup>
        <tr>
            <td class="tdlabel">
                <label class="lblnormal">
                    <%=GetLabel("Template Group")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboGCTemplateGroup" ClientInstanceName="cboGCTemplateGroup" Width="100%"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Template Code")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTemplateCode" Width="100px" CssClass="required" runat="server" MaxLength="5" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel"  style="vertical-align:top">
                <label class="lblMandatory">
                    <%=GetLabel("Template Text")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTemplateText" Width="100%" CssClass="required" runat="server" TextMode="Multiline"
                    Rows="15" />
            </td>
        </tr>
    </table>
</div>
