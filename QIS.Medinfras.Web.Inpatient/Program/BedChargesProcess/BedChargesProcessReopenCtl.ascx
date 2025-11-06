<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BedChargesProcessReopenCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.BedChargesProcessReopenCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_BedChargesProcessReopenCtl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnReopenJobBedCtl.ClientID %>').click(function (evt) {
        if ($('#<%=txtReason.ClientID %>').val() != "") {
            showToastConfirmation("Are You Sure Want To Reopen Job Bed?", function (result) {
                if (result) {
                    cbpReopenJobBedCtl.PerformCallback('reopenjob');
                }
            });
        } else {
            displayErrorMessageBox('Proses gagal', 'Error Message : Harap isi alasan reopen terlebih dahulu.');
        }
    });

    function onReopenJobBedSuccess() {
        pcRightPanelContent.Hide();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnReopenJobBedCtl" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Reopen")%></div>
        </li>
    </ul>
</div>
<input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr id="trReason" runat="server">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Reopen Reason")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReason" Width="300px" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpReopenJobBedCtl" runat="server" Width="100%" ClientInstanceName="cbpReopenJobBedCtl"
            ShowLoadingPanel="false" OnCallback="cbpReopenJobBedCtl_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onReopenJobBedSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
