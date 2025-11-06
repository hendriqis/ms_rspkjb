<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganKematianCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganKematianCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    setDatePicker('<%=txtValueDate.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtValueDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnTanggal.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalString.ClientID %>').val(date);
    });

    $('#<%=rblCauseOfDeath.ClientID %>').live('change', function () {
        var displayOption = $('#<%=rblCauseOfDeath.ClientID %>').find(":checked").val();
        if (displayOption == "filterOther") {
            $('#<%=txtOther.ClientID %>').removeAttr('style');
            $('#<%=txtOther.ClientID %>').val('');
        } else {
            $('#<%=txtOther.ClientID %>').attr('style', 'display:none');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var reportCode = $('#<%=hdnReportCode.ClientID%>').val();

        var date = $('#<%=txtValueDate.ClientID %>').val();
        var day = date.substring(0, 2);
        var month = date.substring(3, 5);
        var year = date.substring(6, 10);
        var newDate = year + '-' + month + '-' + day;

        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var letterNo = $('#<%=txtLetterNo.ClientID %>').val();
        var time1 = $('#<%=txtDeathTime1.ClientID %>').val();
        var time2 = $('#<%=txtDeathTime2.ClientID %>').val();
        
        var causeOfDeath = $('#<%=rblCauseOfDeath.ClientID %>').find(":checked").val();
        var other = $('#<%=txtOther.ClientID %>').val();
        var funeralPlan = $('#<%=txtFuneralPlan.ClientID %>').val();

        var filterExpression = registrationID + "|" + newDate + "|" + time1 + "|" + time2 + "|" + letterNo + "|" + causeOfDeath + "|" + other + "|" + funeralPlan;
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
        <input type="hidden" id="hdnTanggal" runat="server" value="" />
        <input type="hidden" id="hdnTanggalString" runat="server" value="" />
        <input type="hidden" id="hdnReportCode" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Nomor Surat")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtLetterNo" runat="server"
                        Width="250px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Meninggal")%></label>
                </td>
                    <td>
                        <asp:TextBox ID="txtValueDate" CssClass="txtValueDate datepicker" runat="server"
                            Width="150px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory" />
                    <%=GetLabel("Jam Meninggal")%>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDeathTime1" Width="80px" CssClass="number" runat="server"
                                    Style="text-align: center" MaxLength="2" max="24" min="0" />
                            </td>
                            <td>
                                <label class="lblNormal" />
                                <%=GetLabel(":")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeathTime2" Width="80px" CssClass="number" runat="server"
                                    Style="text-align: center" MaxLength="2" max="59" min="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Penyebab Kematian")%></label>
                </td>
                <td>
                <asp:RadioButtonList ID="rblCauseOfDeath" runat="server" RepeatDirection="Vertical">
                    <asp:ListItem Text="Meninggal Saat Kedatangan" Value="filterDOA" />
                    <asp:ListItem Text="Lahir Mati" Value="filterIUFD" />
                    <asp:ListItem Text="Lain-Lain" Value="filterOther" Selected="true" />
                </asp:RadioButtonList>
                <asp:TextBox ID="txtOther" width="200px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Rencana Pemulasaran")%></label>
                </td>
                <td colspan="2" style="vertical-align:top">
                    <asp:TextBox ID="txtFuneralPlan" Width="300px" runat="server" />
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
