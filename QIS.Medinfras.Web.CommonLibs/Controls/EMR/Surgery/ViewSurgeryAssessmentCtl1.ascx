<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSurgeryAssessmentCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ViewSurgeryAssessmentCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_patientEducationEntryctl">
    $(function () {
    });

    //#region Left Navigation Panel
    $('#leftPageNavPanel ul li').click(function () {
        $('#leftPageNavPanel ul li.selected').removeClass('selected');
        $(this).addClass('selected');
        var contentID = $(this).attr('contentID');

        showContent(contentID);
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divPageNavPanelContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
    //#endregion

    registerCollapseExpandHandler();

    $('#leftPageNavPanel ul li').first().click();
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDivFormContent" value="" />
    <input type="hidden" runat="server" id="hdnDivFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnEducationFormGroup" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 250px" />
            <col />
        </colgroup>
        <tr>
            <td colspan="2">
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
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border" style="width:250px">
                    <ul>
                        <li contentID="divPage1" contentIndex="1" title="Asesmen Pra Bedah" class="w3-hover-red">Asesmen Pra Bedah</li>
                        <li contentID="divPage2" contentIndex="2" title="Page-2" class="w3-hover-red">Page-2</li>
                        <li contentID="divPage3" contentIndex="3" title="Page-3" class="w3-hover-red">Page-3</li>
                        <li contentID="divPage4" contentIndex="4" title="Page-4" class="w3-hover-red">Page-4</li>
                        <li contentID="divPage5" contentIndex="5" title="Page-5" class="w3-hover-red">Page-5</li>
                    </ul>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none; max-height: 500px; overflow-y: scroll">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <colgroup>
                            <col width="100%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Anamnesis dan Riwayat Kesehatan")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height: 300px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPreAssesmentDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td style="padding-left: 5px">
                                                                <asp:TextBox ID="txtPreAssessmentTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                                    ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Dikaji Oleh")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPreAssessmentPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Anamnesis")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPreAssessmentText" Width="100%" runat="server" TextMode="MultiLine"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Keluhan lain yang menyertai")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHPISummary" Width="100%" runat="server" TextMode="MultiLine"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <td>
                                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Autoanamnesis" Checked="false"
                                                                Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                                                Checked="false" Enabled="false" />
                                                        </td>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>        
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Penyakit Dahulu")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicalHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Riwayat Penyakit Keluarga")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>    
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Penyakit Keluarga")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFamilyHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table> 
                                    </div>
                                </div>

                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>        
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Penggunaan Obat")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicationHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Riwayat Operasi Sebelumnya")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>        
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Penggunaan Obat")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPastSurgicalHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Tanda Vital dan Indikator Lainnya") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Pemeriksaan Fisik") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Hasil Pemeriksaan Penunjang") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Kelengkapan Berkas/Dokumen Terkait") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Penandaan Lokasi Operasi") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Pemeriksaan Fisik") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                </div>
                                <h4  class="w3-blue h4collapsed">
                                    <%=GetLabel("Analisis") %>
                                </h4>
                                <div class="containerTblEntryContent">
                                    <div  style="max-height: 300px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                         <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Pre Diagnosis (ICD X)")%></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtPreDiagnosisID" runat="server" Width="99%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Profilaxis")%></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtProfilaxis" runat="server" Width="99%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 5px">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Posisi Pasien Saat Operasi")%></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtPatientPositionSummary" runat="server" Width="99%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="padding-left: 5px"><%=GetLabel("Estimasi Lama Operasi") %></td>
                                                <td><asp:TextBox ID="txtEstimatedDuration" Width="60px" CssClass="number" runat="server" ReadOnly="true" /> menit</td>                                              
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                                    <%=GetLabel("Alat Khusus/Darah") %>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtSurgeryItemSummary" runat="server" Width="100%" TextMode="Multiline"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                                    <label class="lblNormal" id="Label3">
                                                        <%=GetLabel("Catatan Konsultasi Ahli lainnya") %></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtReferralSummary" runat="server" Width="100%" TextMode="Multiline"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align:top; padding-left: 5px">
                                                    <label class="lblNormal" id="Label2">
                                                        <%=GetLabel("Catatan Lainnya") %></label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtOtherSummary" runat="server" Width="100%" TextMode="Multiline"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>                                   
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width:180px"/>
                            <col style="width:150px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam Edukasi")%></label></td>
                            <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" /></td>
                            <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" ReadOnly="true" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pemberi Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtParamedicName" Width="350px" runat="server" Style="text-align:left" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Penerima Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                    <asp:ListItem Text="Pasien" Value="0" Selected="True" />
                                    <asp:ListItem Text="Keluarga / Lain-lain" Value="1"  />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trFamilyInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Penerima")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:140px"/>
                                        <col style="width:85px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSignature2Name" CssClass="txtSignature2Name" runat="server" Width="100%" ReadOnly="true"  />
                                        </td>
                                        <td class="tdLabel" style="padding-left:5px">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Hubungan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFamilyRelation" CssClass="txtFamilyRelation" runat="server" Width="100%" ReadOnly="true"  />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>    
                  </table>             
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                </div>
                <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                </div>
            </td>
        </tr>
    </table>
</div>


