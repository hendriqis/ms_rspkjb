<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FallRiskAssessmentFormEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.FallRiskAssessmentFormEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_fallRiskEntryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    $(function () {
        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
    });

    function onBeforeSaveRecordEntryPopup() {
        var values = getFormValues();
        var totalScore = parseInt($('#<%=txtTotalScore.ClientID %>').val());

        if (cboFallRiskScoreType.GetValue() == null) {
            var message = 'Form pengkajian masih kosong/belum terisi atau jenis resiko belum terisi';
            if (totalScore == 0) {
                errMessage = message;
                displayErrorMessageBox('SAVE', message);
                return false;
            }
            else if (totalScore > 0) {
                message = 'Kesimpulan resiko jatuh belum ditentukan';
                errMessage = message;
                displayErrorMessageBox('SAVE', message);
                return false;
            }
        }
        else if (cboFallRiskScoreType.GetValue() != '') {
            return true;
        }
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
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
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

    //#region Propose
    $('.btnTest').die('click');
    $('.btnTest').live('click', function () {
        alert(getFormValues());
    });
    //#endregion

    function SetTotalScore(param) {
        $('#<%=txtTotalScore.ClientID %>').val(param);
        var score = parseInt(param);
        SetRiskFactorType(param);
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
                else if (score == 1){
                    scoreType = "X383^02";
                }
                else {
                    scoreType = "X383^01";
                }
                break;
            case "X202^06":
                // Neonatus
                if (score >3) {
                    scoreType = "X383^03";
                }
                else if (score >= 1 && score <=3) {
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
    <input type="hidden" runat="server" id="hdnPageVisitID" value="" />
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
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Medical No")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtMedicalNo" Width="120px" runat="server" ReadOnly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtPatientName" Width="450px" runat="server" ReadOnly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Tanggal Lahir")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtDateOfBirth" Width="180px" runat="server" ReadOnly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Registrasi")%></label>
                        </td>
                        <td class="tdLabel">
                            <asp:TextBox ID="txtRegistrationNo" Width="180px" runat="server" ReadOnly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                            <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("PPA")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsInitialAssessment" runat="server" Text=" Bagian Pengkajian Awal Pasien" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea" border="0">
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="max-height: 250px;overflow-y: auto;"></div>
            </td>
        </tr>
    </table>
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Skor ")%>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:TextBox ID="txtTotalScore" Width="60px" CssClass="number" runat="server" /></td>
                                    <td>
                                        <asp:TextBox ID="txtTotalScoreType" Width="285px" runat="server" ReadOnly=true />
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kesimpulan")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboFallRiskScoreType" ClientInstanceName="cboFallRiskScoreType" Width="350px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsFallRisk" runat="server" Text=" Penanda Resiko Jatuh" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td>
                            <input type="button" class="btnTest w3-btn w3-hover-blue" value="Display HTML" style="background-color:Red;color:White; width:120px;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
