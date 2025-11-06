<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPainAssessmentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewPainAssessmentCtl" %>
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
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                        $(this).prop('disabled', true);
                    }
                    else {
                        $(this).prop('disabled', true);
                    }
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
        var paramInfo = param.split('|');
        $('#<%=txtTotalScore.ClientID %>').val(paramInfo[0]);
        SetRiskFactorType(paramInfo[1]);
    }

    function SetRiskFactorType(score) {
        var formType = $('#<%=hdnFormType.ClientID %>').val();
        $('#<%=txtTotalScoreType.ClientID %>').val(score);

        //cboPainScoreType.SetValue("X383^03");
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
                            <asp:TextBox ID="txtObservationDate" Width="120px" runat="server" ReadOnly="true" />
                           <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" ReadOnly="true" />
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
                            <asp:TextBox ID="txtParamedicInfo" Width="350px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Penyebab")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtProvocation" Width="350px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Quality / Kualitas")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtQuality" Width="350px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Regio / Lokasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRegio" Width="350px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Time / Waktu")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTime" Width="350px" runat="server" ReadOnly=true />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea" border="0">
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="height: auto;overflow-y: auto;"></div>
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
                                    <td><asp:TextBox ID="txtTotalScore" Width="60px" CssClass="number" runat="server" ReadOnly="true" /></td>
                                    <td>
                                        <asp:TextBox ID="txtTotalScoreType" Width="285px" runat="server" ReadOnly="true" />
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
                            <dxe:ASPxComboBox ID="cboPainScoreType" ClientInstanceName="cboPainScoreType" Width="350px" runat="server" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsPain" runat="server" Text=" Ada Keluhan Nyeri" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsInitialAssessment" runat="server" Text=" Bagian Pengkajian Awal Pasien" Enabled="false" />
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
