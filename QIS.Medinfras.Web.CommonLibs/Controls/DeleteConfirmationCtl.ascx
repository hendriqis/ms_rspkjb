<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteConfirmationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.DeleteConfirmationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_chargesvoidctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnDeleteCharges.ClientID %>').click(function (evt) {
        var data = { GCDeleteReason: "", Reason: "" };
        data.GCDeleteReason = cboDeleteReason.GetValue();
        data.Reason = $('#<%=txtReason.ClientID %>').val();
        if (typeof onDeleteDetailUsingReason == 'function')
            onDeleteDetailUsingReason(data);
    });

    function onCboDeleteReasonChanged() {
        if (cboDeleteReason.GetValue() != 'X129^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnDeleteCharges" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Delete Reason")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboDeleteReason" ClientInstanceName="cboDeleteReason"
                        Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboDeleteReasonChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Reason")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReason" Width="200px" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </fieldset>
</div>
