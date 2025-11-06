<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LembarKonsultasiCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.LembarKonsultasiCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="LembarKonsultasiCtldxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalconsultation">

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var referralphysicianText = $('#<%=txtReferralPhysicianText.ClientID %>').val();
        var therapyText = $('#<%=txtTheraphyText.ClientID %>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var filterExpression = "VisitID = " + visitID + "|" + referralphysicianText + "|" + therapyText;
        var reportCode = "PM-00527";
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
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 200px" />
                <col style="width: 100px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Ditujukan kepada")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReferralPhysicianText" Width="250px" CssClass="required" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Terapi yang telah diberikan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtTheraphyText" Width="250px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpMedicalSickLeave" runat="server" Width="100%" ClientInstanceName="cbpMedicalSickLeave"
            ShowLoadingPanel="false" OnCallback="cbpMedicalSickLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
