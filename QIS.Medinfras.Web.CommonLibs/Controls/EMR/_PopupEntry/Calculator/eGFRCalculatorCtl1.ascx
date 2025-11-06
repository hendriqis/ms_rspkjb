<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="eGFRCalculatorCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GFRCalculatorCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_eGFRCalculatorCtl1">
    $('#btnPropose').live('click', function () {
        var param1 = parseFloat($('#<%:txtParameter1.ClientID %>').val());
        var param2 = parseFloat($('#<%:txtParameter2.ClientID %>').val());
        var param3 = parseFloat($('#<%:txtParameter3.ClientID %>').val());
        var param4 = parseFloat($('#<%:txtParameter4.ClientID %>').val());
        var param5 = parseFloat($('#<%:txtParameter5.ClientID %>').val());
        var endFct = parseFloat($('#<%:hdnEndFct.ClientID %>').val())
        var par = parseFloat($('#<%:hdnPar.ClientID %>').val());

        var result = 142 * param4 * param5 * par * endFct;
        $('#<%:txtResult.ClientID %>').val(result.toFixed(3));
    });

    $('#<%:txtParameter4.ClientID %>').live('change', function () {
        var value = parseFloat($(this).val());
        if (value < 1) {
            $(this).val('1');
        }
    });

    $('#<%:txtParameter5.ClientID %>').live('change', function () {
        var value = parseFloat($(this).val());
        if (value > 1) {
            $(this).val('1');
        }
    });

    function onBeforeProcess(errMessage) {
        if(parseFloat($('#<%:txtResult.ClientID %>').val()) < 0 || $('#<%:txtResult.ClientID %>').val() == '') {
            errMessage.text = "GFR Tidak Boleh Kurang Dari Nol ";
            return false;
        }
        else {
            return true;
        }
    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnEndFct" value="" />
    <input type="hidden" runat="server" id="hdnItemIDKreatininSerum" value="" />
    <input type="hidden" runat="server" id="hdnFractionIDKreatininSerumDarah" value="" />
    <input type="hidden" runat="server" id="hdnFractionIDeGFR" value="" />
    <input type="hidden" runat="server" id="hdnItemID" value="" />
    <input type="hidden" runat="server" id="hdnReferenceDtID" value="" />
    <input type="hidden" runat="server" id="hdnPar" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div style="height: 80px;">
                    <input type="hidden" id="hdnbannerPayerID" value="" runat="server" />
                    <input type="hidden" id="hdnbannerContractID" value="" runat="server" />
                    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
                    <input type="hidden" id="hdnGuestID" value="" runat="server" />
                    <div style="float: right">
                        <img id="imgPhysician" runat="server" class="imgPhysician" alt="" width="55" style="float: right;"
                            src="" />
                        <input type="hidden" id="hdnPatientGender" runat="server" class="hdnPatientGender" />
                        <div style="text-align: right; margin-right: 75px;">
                            <div style="font-weight: bold;">
                                <label id="lblPhysicianName" runat="server">
                                </label>
                            </div>
                            <div class="tblPatientBannerInfo" style="font-size: 10pt">
                                <div style="font-style: italic">
                                    <label id="lblParamedicLicenseNo" runat="server">
                                    </label>
                                </div>
                                <div>
                                    <label id="lblParamedicType" runat="server">
                                    </label>
                                    <label style="font-weight: bold; color: #1BA1E2;" id="lblGradingInfo" runat="server">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <table style="float: left; background-color: White" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <div class="divPatientBannerImgInfo" id="divPatientBannerImgInfo" runat="server"
                                    style="background-color: Red; height: 70px; width: 12px">
                                </div>
                            </td>
                            <td>
                                <div id="id=divPatientImage">
                                    <img id="imgPatientProfilePicture" class="imgLink hvr-glow imgPatient" runat="server"
                                        src='' alt="" width="55" />
                                </div>
                            </td>
                            <td style="vertical-align: bottom">
                                <div id="divPatientStatusDNR" class="divPatientStatusDNR" style="background-color: Purple;
                                    padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center; color: white"
                                    runat="server">
                                    D
                                </div>
                                <div id="divPatientStatusFallRisk" class="divPatientStatusFallRisk" style="background-color: Yellow;
                                    padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center" runat="server">
                                    F
                                </div>
                                <div id="divPatientStatusAllergy" class="divPatientStatusAllergy" style="background-color: Red;
                                    padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center; color: white"
                                    runat="server">
                                    A
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0">
                        <col style="width: 120px" />
                        <col style="width: 200px" />
                        <col style="width: 120px" />
                        <col style="width: 220px" />
                        <col style="width: 100px" />
                        <col style="width: 480px" />
                        <tr style="font-size: 1.2em">
                            <td colspan="3" style="padding-left: 20px">
                                <span class="lblLink" id="lblPatientInfo" style="color: Black">
                                    <label id="lblPatientName" runat="server" style="font-weight: bold;">
                                    </label>
                                </span>
                            </td>
                            <td class="lblRegistrationNo">
                                <input type="hidden" id="hdnVisitID" value="0" runat="server" />
                                <input type="hidden" id="hdnRegistrationID" value="0" runat="server" />
                                <input type="hidden" id="hdnRegistrationNo" value="0" runat="server" />
                            </td>
                            <td colspan="2">
                                <label id="lblFromRegistrationNo" runat="server" style="float: left; color: Red;
                                    font-size: smaller; font-style: oblique" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("No. RM")%>
                            </td>
                            <td style="width: 120px;" class="tdPatientBannerValue">
                                <input type="hidden" id="hdnMRN" value="" runat="server" />
                                <input type="hidden" id="hdnMedicalNo" value="" runat="server" />
                                <input type="hidden" id="hdnPatientName" value="" runat="server" />
                                <label id="lblMRN" runat="server" style="float: left; font-weight: bold;" />
                                <span id="spnOldMedicalNo" runat="server">
                                    <label id="lblOldMRN" runat="server" style="margin-left: 3px; color: Red" />
                                </span>
                            </td>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Tanggal Lahir")%>
                            </td>
                            <td class="tdPatientBannerValue">
                                <label id="lblDOB" runat="server">
                                </label>
                            </td>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Pengirim")%>
                            </td>
                            <td class="tdPatientBannerValue">
                                <label id="lblReferralNo" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Alergi")%>
                            </td>
                            <td>
                                <span class="lblLink" id="lblAllergyInfo" style="color: Red">
                                    <label id="lblAllergy" runat="server" style="color: Red; font-weight: bold">
                                    </label>
                                </span>
                            </td>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Umur - Agama")%>
                            </td>
                            <td class="tdPatientBannerValue">
                                <label id="lblPatientAge" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Kategori") %>
                            </td>
                            <td class="tdPatientBannerValue">
                                <label id="lblPatientCategory" runat="server">
                                </label>
                            </td>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Jenis Kelamin")%>
                            </td>
                            <td class="tdPatientBannerValue">
                                <label id="lblGender" runat="server">
                                </label>
                            </td>
                            <td class="tdPatientBannerLabel">
                                <%=GetLabel("Pembayar")%>
                            </td>
                            <td class="tdPatientBannerValue">
                                <table>
                                    <tr>
                                        <td>
                                            <label id="lblPayer" runat="server">
                                            </label>
                                        </td>
                                        <td>
                                            <label id="lblPayerInfo" runat="server">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="max-height: 500px;
                    overflow-y: scroll">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <colgroup>
                            <col width="100%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top">
                                <div class="containerTblEntryContent">
                                    <div style="max-height: 400px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("No. Transaksi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransactionNo" Width="200px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Pemeriksaan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemName" Width="400px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Jenis Kelamin")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSex" Width="100px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Umur Pasien")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAge" Width="100px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kreatinin")%>
                                                        (S<sub>cr</sub>)</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter1" Width="100px" runat="server" ReadOnly="true" Style="text-align: right" />
                                                    mg/dL
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("k")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter2" Width="100px" runat="server" Style="text-align: right"
                                                        ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("α")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter3" Width="100px" runat="server" Style="text-align: right"
                                                        ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("A")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter4" Width="100px" runat="server" Style="text-align: right" />
                                                    min(S<sub>cr</sub>/<b>κ</b>, 1)<sup>α</sup>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("B")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter5" Width="100px" runat="server" Style="text-align: right" />
                                                    max(S<sub>cr</sub>/<b>κ</b>, 1)<sup>-1.200</sup>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-top: 50px; text-align: center" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal" style="font-weight: bold">
                                                        <%=GetLabel("GFR")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtResult" Width="100px" runat="server" Style="text-align: right" />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnPropose" class="btnPropose w3-btn w3-hover-blue" value="Calculate"
                                                                    style="background-color: Red; color: White; width: 100px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <b>GFR<sub>cr</sub></b> = 142 x A x B x 0.9938<sup>Age</sup> x 1.012 [if female]
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
