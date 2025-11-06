<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClinicPauseCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ClinicPauseCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnProcess.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPrintPatientLabel.PerformCallback('pause|');
        }
    });

    $('#<%=txtPauseReason.ClientID %>').change(function () {
        var voidReason = $('#<%=txtPauseReason.ClientID %>').val();
        $('#<%=hdnVoidReason.ClientID %>').val(voidReason);
    });

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnProcess" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
        <input type="hidden" id="hdnIsImagingUnit" runat="server" value="" />
        <input type="hidden" id="hdnVoidReason" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationStatus" runat="server" value="" />
        <input type="hidden" id="hdnParamedicID" runat="server" value="" />
        <input type="hidden" id="hdnTempDate" runat="server" value="" />
        <input type="hidden" id="hdnOperationalTimeID" runat="server" value="" />
        <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
        <input type="hidden" value="" id="hdnIsBridgingToGateway" runat="server" />
        <input type="hidden" value="" id="hdnProviderGatewayService" runat="server" />
        <table>
            <colgroup>
                <col style="width: 40%" />
                <col style="width: 60%" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Alasan Tutup Sementara")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboPauseReason" Width="200px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Keterangan")%></label>
                </td>
                <td id="tdPauseReason">
                    <asp:TextBox ID="txtPauseReason" runat="server" Width="250px" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPrintPatientLabel" runat="server" Width="100%" ClientInstanceName="cbpPrintPatientLabel"
            ShowLoadingPanel="false" OnCallback="cbpPrintPatientLabel_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    hideLoadingPanel();
                    pcRightPanelContent.Hide();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
