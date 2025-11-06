<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEWSAssessmentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewEWSAssessmentCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_vitalsignentryctl">
    $(function () {
        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1")
                        $(this).prop('checked', true);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                        $(this).prop('disabled', true);
                    }
                    else {
                        $(this).prop('disabled', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
    });

    function onBeforeSaveRecordEntryPopup() {
        var values = getFormValues();
        return true;
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });

        $('#<%=hdnFormValues.ClientID %>').val(controlValues);

        return controlValues;
    }

    function SetTotalScore(param) {
        $('#<%=txtTotalScore.ClientID %>').val(param);
        //var score = parseInt(param);
        //SetRiskFactorType(param);
    }

    function SetRiskFactorType(score) {
        var formType = $('#<%=hdnFormType.ClientID %>').val();
        var scoreType = "";
        switch (formType) {
            case "X202^01":
                // HUMPTY-DUMPTY
                if (score >= 7 && score <= 11) {
                    scoreType = "X383^01";
                }
                else if (score >= 12) {
                    scoreType = "X383^03";
                }
                break;
            case "X202^02":
                // MORSE FALL SCALE
                if (score >= 0 && score <= 24) {
                    scoreType = "X383^01";
                }
                else if (score >= 25 && score < 45) {
                    scoreType = "X383^02";
                }
                else {
                    scoreType = "X383^03";
                }
                break;
            case "X202^03":
                // ONTARIO
                if (score >= 0 && score <= 5) {
                    scoreType = "X383^01";
                }
                else if (score >= 6 && score <= 16) {
                    scoreType = "X383^02";
                }
                else {
                    scoreType = "X383^03";
                }
                break;
            case "X202^04":
                // EDMONSION
                if (score >= 90) {
                    scoreType = "X383^05";
                }
                else {
                    scoreType = "X383^04";
                }
                break;
            case "X202^05":
                // Get Up and Go Test
                if (score >= 2) {
                    scoreType = "X383^03";
                }
                else if (score == 1) {
                    scoreType = "X383^02";
                }
                else {
                    scoreType = "X383^01";
                }
                break;
            case "X202^06":
                // Neonatus
                if (score > 3) {
                    scoreType = "X383^03";
                }
                else if (score >= 1 && score <= 3) {
                    scoreType = "X383^01";
                }
                else {
                    scoreType = "X383^01";
                }
                break;
            case "X202^91":
                // Custom : RSSEBK
                if (score > 1) {
                    scoreType = "X383^03";
                }
                else if (score = 1) {
                    scoreType = "X383^01";
                }
                else {
                    scoreType = "X383^04";
                }
                break;
            default:
                // code block
                break;
        }

        if (formType != "X202^05") {
            switch (scoreType) {
                case "X383^01":
                    $('#<%=txtTotalScoreType.ClientID %>').val("01 - Resiko Rendah");
                    break;
                case "X383^02":
                    $('#<%=txtTotalScoreType.ClientID %>').val("02 - Resiko Sedang");
                    break;
                case "X383^03":
                    $('#<%=txtTotalScoreType.ClientID %>').val("03 - Resiko Tinggi");
                    break;
                case "X383^04":
                    $('#<%=txtTotalScoreType.ClientID %>').val("TB - Tidak Beresiko Jatuh");
                    break;
                case "X383^05":
                    $('#<%=txtTotalScoreType.ClientID %>').val("RJ - Resiko Jatuh");
                    break;
                default:
                    break;
            }
        }
        else {
            switch (scoreType) {
                case "X383^01":
                    $('#<%=txtTotalScoreType.ClientID %>').val("01 - Tidak Beresiko");
                    break;
                case "X383^02":
                    $('#<%=txtTotalScoreType.ClientID %>').val("02 - Resiko Rendah");
                    break;
                case "X383^03":
                    $('#<%=txtTotalScoreType.ClientID %>').val("03 - Resiko Tinggi");
                    break;
                case "X383^04":
                    $('#<%=txtTotalScoreType.ClientID %>').val("TB - Tidak Beresiko Jatuh");
                    break;
                case "X383^05":
                    $('#<%=txtTotalScoreType.ClientID %>').val("RJ - Resiko Jatuh");
                    break;
                default:
                    break;
            }
        }

        //cboFallRiskScoreType.SetValue("X383^03");
    }
</script>
<style type="text/css">
</style>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnFormType" value="" />
    <input type="hidden" runat="server" id="hdnFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />

    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. RM / Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtMedicalNo" Width="120px" runat="server" ReadOnly />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPatientName" Width="310px" runat="server" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Tanggal Lahir / No. Registrasi")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDateOfBirth" Width="120px" runat="server" ReadOnly />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationNo" Width="310px" runat="server" ReadOnly />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                         <asp:TextBox ID="txtObservationDate" Width="120px" runat="server" ReadOnly=true />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                            Style="text-align: center" ReadOnly=true />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("PPA")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParamedicInfo" Width="350px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsInitialAssessment" runat="server" Text=" Bagian Pengkajian Awal Pasien" Enabled="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea" border="0">
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="max-height: 300px;overflow-y: auto;"></div>
            </td>
        </tr>
    </table>

    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 290px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" title="User dapat melakukan intervensi terhadap hasil perhitungan dari formula form">
                                <%=GetLabel("Total Skor (versi manual / pengkaji)")%>
                        </td>
                        <td><asp:TextBox ID="txtTotalScore" Width="60px" CssClass="number" runat="server" ReadOnly="true" /></td>
                        <td style="padding-left: 10px">
                            <asp:CheckBox ID="chkIsEWSAlert" runat="server" Text=" Indikator Pemantauan EWS" Enabled="false" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kesimpulan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTotalScoreType" Width="350px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <%=GetLabel("Catatan Tambahan") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
