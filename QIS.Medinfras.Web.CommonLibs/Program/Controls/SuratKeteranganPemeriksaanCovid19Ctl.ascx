<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganPemeriksaanCovid19Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganPemeriksaanCovid19Ctl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">
    $(function () {
        $('#<%=chkAntibody.ClientID %>').change(function () {
            var isPatient = $(this).is(":checked");
            if (isPatient) {
                $('#<%=txtAntibodyDate.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtAntibodyDate.ClientID %>').attr('readonly', 'readonly');
            }
        });
    });

    $(function () {
        $('#<%=chkAntigen.ClientID %>').change(function () {
            var isPatient = $(this).is(":checked");
            if (isPatient) {
                $('#<%=txtAntigenDate.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtAntigenDate.ClientID %>').attr('readonly', 'readonly');
            }
        });
    });

    $(function () {
        $('#<%=chkPCR.ClientID %>').change(function () {
            var isPatient = $(this).is(":checked");
            if (isPatient) {
                $('#<%=txtPCRDate.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtPCRDate.ClientID %>').attr('readonly', 'readonly');
            }
        });
    });

    setDatePicker('<%=txtAntibodyDate.ClientID %>');
    setDatePicker('<%=txtAntigenDate.ClientID %>');
    setDatePicker('<%=txtPCRDate.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpCovidTest.PerformCallback('Print');
        }
    });

    $('#<%=txtAntibodyDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = DD + '-' + MM + '-' + YYYY;
        var today = moment().format('DD + ' - ' + MM + ' - ' + YYYY');
        $('#<%=hdnTanggalAntibody.ClientID %>').val(today);
    });

    $('#<%=txtPCRDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = DD + '-' + MM + '-' + YYYY;
        var today = moment().format('DD + ' - ' + MM + ' - ' + YYYY');
        $('#<%=hdnTanggalPCR.ClientID %>').val(today);
    });

    $('#<%=txtAntibodyDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = DD + '-' + MM + '-' + YYYY;
        var today = moment().format('DD + ' - ' + MM + ' - ' + YYYY');
        $('#<%=hdnTanggalAntibody.ClientID %>').val(today);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var documentText = $('#<%=txtDocumentText.ClientID %>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var antibody = $('#<%=chkAntibody.ClientID %>').prop('checked');
        var abreact = $('#<%=rblAntibody.ClientID %>').find('input:checked').val();
        var abdate = $('#<%=txtAntibodyDate.ClientID %>').val();
        var antigen = $('#<%=chkAntigen.ClientID %>').prop('checked');
        var agreact = $('#<%=rblAntigen.ClientID %>').find('input:checked').val();
        var agdate = $('#<%=txtAntigenDate.ClientID %>').val();
        var pcr = $('#<%=chkPCR.ClientID %>').prop('checked');
        var pcrreact = $('#<%=rblPCR.ClientID %>').find('input:checked').val();
        var pcrdate = $('#<%=txtPCRDate.ClientID %>').val();
        var filterExpression = visitID + "|" + documentText + "|" + antibody + "|" + abreact + "|" + abdate + "|" + antigen + "|" + agreact + "|" + agdate + "|" + pcr + "|" + pcrreact + "|" + pcrdate;
        var reportCode = "PM-00643";
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
        <input type="hidden" id="hdnTanggalAntibody" runat="server" value="" />
        <input type="hidden" id="hdnTanggalAntigen" runat="server" value="" />
        <input type="hidden" id="hdnTanggalPCR" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Sebagai persyaratan :")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocumentText" Width="280px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <asp:CheckBox ID="chkAntibody" runat="server" />
                    <label class="lblMandatory">
                        <%=GetLabel("Rapid Antibody SARS-COV2:")%></label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblAntibody" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="REAKTIF" Value="0" Selected="True" />
                        <asp:ListItem Text="NON REAKTIF" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Diperiksa :")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtAntibodyDate" CssClass="txtDate datepicker" runat="server" Width="100px"
                        ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkAntigen" runat="server" />
                    <label class="lblMandatory">
                        <%=GetLabel("Rapid Antigen SARS-COV2:")%></label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblAntigen" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="REAKTIF" Value="0" Selected="True" />
                        <asp:ListItem Text="NON REAKTIF" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Diperiksa :")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtAntigenDate" CssClass="txtDate datepicker" runat="server" Width="100px"
                        ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkPCR" runat="server" />
                    <label class="lblMandatory">
                        <%=GetLabel("PCR Swab Naso-oropharyng:")%></label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblPCR" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="REAKTIF" Value="0" Selected="True" />
                        <asp:ListItem Text="NON REAKTIF" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Diperiksa :")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtPCRDate" CssClass="txtDate datepicker" runat="server" Width="100px"
                        ReadOnly="true" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpCovidTest" runat="server" Width="100%" ClientInstanceName="cbpCovidTest"
            ShowLoadingPanel="false" OnCallback="cbpCovidTest_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
