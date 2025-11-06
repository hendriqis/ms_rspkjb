<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OPMedicalResumeCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.OPMedicalResumeCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_IPMedicalResumeCtl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#leftPanel ul li').first().click();
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
</script>
<style type="text/css">
    #leftPanel
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #leftPanel > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #leftPanel > ul > li
    {
        list-style-type: none;
        font-size: 15px;
        display: list-item;
        border: 1px solid #fdf5e6 !important;
        padding: 5px 8px;
        cursor: pointer;
        background-color: #87CEEB !important;
    }
    #leftPanel > ul > li.selected
    {
        background-color: #ff5722 !important;
        color: White;
    }
    .divContent
    {
        padding-left: 3px;
        min-height: 490px;
    }
</style>
<div style="width: 100%;">
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <colgroup>
            <col style="width: 300px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <div id="lblMedicalNo" runat="server" class="w3-lime w3-xxlarge" style="text-align: center;
                    text-shadow: 1px 1px 0 #444; width: 100%">
                </div>
            </td>
            <td>
                <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align: center;
                    text-shadow: 1px 1px 0 #444">
                    <%=GetLabel("RESUME MEDIS")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentid="divPage2" title="Anamnesa/Keluhan Utama Pasien" class="w3-hover-red">
                            Anamnesa/Keluhan Utama Pasien</li>
                        <li contentid="divPage3" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan
                            Fisik</li>
                        <li contentid="divPage4" title="Pemeriksaan Penunjang" class="w3-hover-red">Pemeriksaan
                            Penunjang</li>
                        <li contentid="divPage5" title="Diagnosa" class="w3-hover-red">Diagnosa</li>
                        <li contentid="divPage6" title="Pengobatan/Terapi" class="w3-hover-red">Pengobatan/Terapi</li>
                        <li contentid="divPage7" title="Tindakan" class="w3-hover-red">Tindakan yang dilakukan</li>
                        <li contentid="divPage8" title="Prosedur Terapi dan Tindakan" class="w3-hover-red">Anjuran
                            Pemeriksaan</li>
                    </ul>
                </div>
                <div>
                    <table class="w3-table-all" style="width: 100%">
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class="w3-small">
                                    <%=GetLabel("Tanggal Resume Medis:")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div id="lblMedicalResumeDateTime" runat="server" class="w3-medium">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class=" w3-small">
                                    <%=GetLabel("Dibuat Oleh :")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div id="lblResumeParamedicName" runat="server" class="w3-medium">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class="w3-small">
                                    <%=GetLabel("Tanggal Revisi:")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div id="lblRevisionDateTime" runat="server" class="w3-medium">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup style="width: 130px" />
                        <colgroup style="width: 10px; text-align: center" />
                        <colgroup />
                        <colgroup style="width: 130px" />
                        <tr>
                            <td style="vertical-align: top">
                                <img style="width: 110px; height: 125px" runat="server" runat="server" id="imgPatientImage" />
                            </td>
                            <td />
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup style="width: 160px" />
                                    <colgroup style="width: 10px; text-align: center" />
                                    <colgroup />
                                    <tr>
                                        <td colspan="3" style="width: 100%">
                                            <span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold;
                                                width: 100%"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Jenis Kelamin")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblGender" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tanggal Lahir (Umur)")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblDateOfBirth" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Tanggal & Jam Registrasi")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <div id="lblRegistrationDateTime" runat="server" style="color: Black">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("No. Registrasi")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <div id="lblRegistrationNo" runat="server" style="color: Black">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("DPJP Utama")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPhysician" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Pembayar")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPayerInformation" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <%=GetLabel("Lokasi Pasien")%>
                                        </td>
                                        <td class="tdLabel">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <span id="lblPatientLocation" runat="server" style="color: Black"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="vertical-align: top">
                                            <%=GetLabel("Diagnosa")%>
                                        </td>
                                        <td class="tdLabel" style="vertical-align: top">
                                            <%=GetLabel(":")%>
                                        </td>
                                        <td>
                                            <textarea id="lblDiagnosis" runat="server" style="border: 0; width: 100%; height: 120px;
                                                background-color: transparent" readonly></textarea>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 220px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Waktu")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtResumeDate" Width="120px" runat="server" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:TextBox ID="txtResumeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                MaxLength="5" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 150px; vertical-align: top">
                                <label class="lblNormal" id="lblHPI">
                                    <%=GetLabel("Anamnesa/Riwayat Penyakit")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubjectiveResumeText" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="8" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <tr>
                                    <td style="vertical-align: top">
                                        <label class="lblNormal" id="Label5">
                                            <%=GetLabel("Pemeriksaan Fisik") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="15" ReadOnly="true" />
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top">
                                <label class="lblNormal" id="Label4">
                                    <%=GetLabel("Pemeriksaan Penunjang") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlanningResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage5" class="divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Diagnosa") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAssessmentResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblLink" id="lblEpisodeMedication">
                                    <%=GetLabel("Pengobatan/Terapi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicationResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage7" class="divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label id="lblProgressNote">
                                    <%=GetLabel("Prosedur Terapi dan Tindakan") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="15" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage8" class="divContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Prosedur Terapi dan Tindakan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstructionResumeText" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="18" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
