<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HealthyInformationReasonCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.HealthyInformationReasonCtl" %>
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

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPrintPatientLabel.PerformCallback('Print');
        }
    });

    $('#<%=txtReason.ClientID %>').change(function () {
        var reasonRemarks = $('#<%=txtReason.ClientID %>').val();
        $('#<%=hdnReasonRemarks.ClientID %>').val(reasonRemarks);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var reasonRemarks = $('#<%=hdnReasonRemarks.ClientID %>').val();
        var type = $('#<%=rblItemType.ClientID %>').find('input:checked').val();

        var errMessage = { text: "" };
        var filterExpression = "RegistrationID = " + registrationID + "|" + reasonRemarks + "|" + type;
        var reportCode = "PM-00142";

        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
        <input type="hidden" id="hdnIsImagingUnit" runat="server" value="" />
        <input type="hidden" id="hdnReasonRemarks" runat="server" value="" />
        <input type="hidden" id="hdntype" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 50px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Untuk Mengikuti / Menjadi")%></label>
                </td>
                <td>
                    <td class="tdPrintTotal">
                        <asp:TextBox ID="txtReason" runat="server" Width="300px" />
                    </td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Buta Warna")%></label>
                </td>
                <td>
                    <td class="tdPrintTotal">
                        <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Tidak Buta Warna" Value="0" Selected="True" />
                            <asp:ListItem Text="Buta Warna" Value="1" />
                        </asp:RadioButtonList>
                    </td>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPrintPatientLabel" runat="server" Width="100%" ClientInstanceName="cbpPrintPatientLabel"
            ShowLoadingPanel="false" OnCallback="cbpPrintPatientLabel_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
