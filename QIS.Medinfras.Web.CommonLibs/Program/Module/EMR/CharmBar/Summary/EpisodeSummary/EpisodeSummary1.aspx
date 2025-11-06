<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="EpisodeSummary1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EpisodeSummary1"
    EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
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
            min-height: 550px;
            width: 100%;
        }
        
        #contentDetail3NavPane > a
        {
            margin: 0;
            font-size: 11px;
        }
        #contentDetail3NavPane > a.selected
        {
            color: #fff !important;
            background-color: #f44336 !important;
        }
        .keyUser
        {
            display: none;
        }
    </style>
    <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <input type="hidden" id="hdnContentRegistrationID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCollapseID" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnSurgeryHistoryID" value="0" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnLogDate" value="0" />
    <input type="hidden" runat="server" id="hdnOrderNo" value="" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTabSurgeryHistory" runat="server" />
    <input type="hidden" runat="server" id="hdnFluidName" value="0" />
    <input type="hidden" runat="server" id="hdnGCFluidType" value="0" />
    <input type="hidden" runat="server" id="hdnGCAssessmentType" value="" />
    <input type="hidden" runat="server" id="hdnGCAssessmentTypePainAssessment" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnOperatingRoomID" value="" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrl" value="0" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" value="" id="hdnLaboratoryID1" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryID2" runat="server" />
    <input type="hidden" value="" id="hdnLabHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDocumentPathImaging" runat="server" />
    <input type="hidden" value="" id="hdnReferenceNoImaging" runat="server" />
    <input type="hidden" id="hdnRISVendorImaging" runat="server" value="" />
    <input type="hidden" value="" id="hdnViewerUrlImaging" runat="server" />
    <input type="hidden" id="hdnMedicalNo" runat="server" value="" />
    <input type="hidden" value="" id="hdnImagingID" runat="server" />
    <input type="hidden" value="" id="hdnItemIDImaging" runat="server" />
    <input type="hidden" value="" id="hdnImagingHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnIsRISUsingPDFResult" runat="server" value="" />
    <input type="hidden" value="1" id="hdnDisplayMode" runat="server" />
    <input type="hidden" value="1" id="hdnMedicationStatus" runat="server" />
    <input type="hidden" id="hdnIsUsingPrintLBResult" runat="server" value="" />
    <input type="hidden" id="hdnRptCodeSurgery" runat="server" value="" />
    <input type="hidden" id="hdnRptCodePatientRefferal" runat="server" value="" />
    <input type="hidden" id="hdnRptCodePatientRefferalAnswer" runat="server" value="" />
    <input type="hidden" id="hdnReportCodeRadResult" runat="server" value="" />
    <input type="hidden" id="hdnReportCodeLabResult" runat="server" value="" />
    <div style="width:1200px;margin:0 auto; height:500px">
        <input type="hidden" value="2" runat="server" id="hdnVisitIDPopUpCtl" />
        <div style="width: 100%">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <colgroup>
                    <col style="width: 180px" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <div id="lblMedicalNoTitle" runat="server" class="w3-lime w3-xxlarge" style="text-align: center;
                            text-shadow: 1px 1px 0 #444; width: 100%">
                        </div>
                    </td>
                    <td style="vertical-align: top; padding-left: 5px;">
                        <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align: center;
                            text-shadow: 1px 1px 0 #444; width: 100%">
                            <%=GetLabel("RINGKASAN PERAWATAN")%></div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <div id="leftPanel" class="w3-border">
                            <ul>
                                <li contentid="divPage1" title="Data Pasien dan Kunjungan" class="w3-hover-red">Data
                                    Pasien dan Kunjungan</li>
                                <li contentid="divPage2" title="Kajian Awal Medis" class="w3-hover-red">Kajian Awal
                                    Medis</li>
                                <li contentid="divPage3" title="Kajian Awal Perawat" class="w3-hover-red">Kajian Awal
                                    Perawat</li>
                                <li contentid="divPage4" title="Catatan Terintegrasi" class="w3-hover-red">Catatan Terintegrasi</li>
                                <li contentid="divPage5" title="Catatan Perawat" class="w3-hover-red">Catatan Perawat</li>
                                <li contentid="divPage6" title="Patient Handover" class="w3-hover-red">Patient Handover</li>
                                <li contentid="divPage7" title="Konsultasi/Rawat Bersama" class="w3-hover-red">Konsultasi/Rawat
                                    Bersama</li>
                                <li contentid="divPage8" title="Laporan Operasi" class="w3-hover-red">Laporan Operasi</li>
                                <li contentid="divPage9" title="Riwayat Operasi" class="w3-hover-red">Riwayat Operasi</li>
                                <li contentid="divPage10" title="Intake Output" class="w3-hover-red">Intake Output</li>
                                <li contentid="divPage11" title="Hasil Laboratorium" class="w3-hover-red">Hasil Laboratorium</li>
                                <li contentid="divPage12" title="Hasil Radiologi" class="w3-hover-red">Hasil Radiologi</li>
                                <li contentid="divPage13" title="Hasil Penunjang Lain-lain" class="w3-hover-red">Hasil
                                    Penunjang Lain-lain</li>
                                <li contentid="divPage14" title="Daftar Obat" class="w3-hover-red">Daftar Obat</li>
                            </ul>
                        </div>
                    </td>
                    <td style="vertical-align: top; padding-left: 5px;">
                        <div id="divPage1" class="w3-border divContent w3-animate-left">
                            <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="vertical-align: top">
                                        <img style="width: 110px; height: 125px" runat="server" runat="server" id="imgPatientImage" />
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 160px" />
                                                <col style="width: 10px; text-align: center" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td colspan="3" style="width: 100%">
                                                    <span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold;
                                                        width: 100%"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <%=GetLabel("No. Rekam Medis")%>
                                                </td>
                                                <td class="tdLabel">
                                                    <%=GetLabel(":")%>
                                                </td>
                                                <td>
                                                    <span id="lblMedicalNo" runat="server" style="color: Black"></span>
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
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="tdLabel">
                                        <%=GetLabel("Diagnosa")%>
                                    </td>
                                    <td class="tdLabel">
                                        <%=GetLabel(":")%>
                                    </td>
                                    <td>
                                        <textarea id="lblDiagnosis" runat="server" style="border: 0; width: 100%; height: 250px;
                                            background-color: transparent" readonl></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divContent w3-animate-left">
                            <div style="width: 100%; height: 500px; overflow: scroll">
                                <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 45%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td style="vertical-align: top">
                                            <table border="0" cellpadding="1" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 130px" />
                                                    <col style="width: 10px; text-align: center" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel" valign="top">
                                                        <label>
                                                            <%=GetLabel("Tanggal dan Waktu")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceDate" Width="120px" runat="server" ReadOnly="true" />
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                            MaxLength="5" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            <%=GetLabel("Jenis Kunjungan") %></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" valign="top" style="width: 120px">
                                                        <label id="lblChiefComplaint">
                                                            <%=GetLabel("Keluhan Utama")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="3"
                                                            Width="100%" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" valign="top" style="width: 120px">
                                                        <label class="lblNormal" id="lblHPI">
                                                            <%=GetLabel("Keluhan Lain yang menyertai")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtHPISummary" runat="server" Width="100%" TextMode="Multiline"
                                                            Rows="10" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Autoanamnesis" Checked="false"
                                                                        Enabled="false" />
                                                                </td>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Alloanamnesis / Heteroanamnesis"
                                                                        Checked="false" Enabled="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top">
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <colgroup>
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMedicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                                                Rows="5" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <colgroup>
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMedicationHistory" runat="server" Width="100%" TextMode="Multiline"
                                                                Rows="5" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Riwayat Penyakit Keluarga")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <colgroup>
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFamilyHistory" runat="server" Width="100%" TextMode="Multiline"
                                                                Rows="5" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("MST")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <colgroup>
                                                        <col width="210px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                                            <%=GetLabel("Apakah Pasien mengalami penurunan berat badan dalam waktu 6 bulan terakhir ?") %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtGCWeightChangedStatus" runat="server" Width="100%" ReadOnly="true" />
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="padding-left: 5px;">
                                                                        <%=GetLabel("Skor") %>
                                                                    </td>
                                                                    <td style="padding-left: 5px; width: 60px">
                                                                        <asp:TextBox ID="txtWeightChangedStatusScore" runat="server" Width="100%" ReadOnly="true"
                                                                            CssClass="number" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                                            <%=GetLabel("Jika Ya, berapa penurunan berat badan tersebut ?") %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtGCWeightChangedGroup" runat="server" Width="100%" ReadOnly="true" />
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="padding-left: 5px;">
                                                                        <%=GetLabel("Skor") %>
                                                                    </td>
                                                                    <td style="padding-left: 5px; width: 60px">
                                                                        <asp:TextBox ID="txtWeightChangedGroupScore" runat="server" Width="100%" ReadOnly="true"
                                                                            CssClass="number" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                                            <%=GetLabel("Apakah Asupan makanan berkurang karena tidak nafsu makan ?") %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblIsFoodIntakeChanged" runat="server" RepeatDirection="Horizontal"
                                                                Enabled="false">
                                                                <asp:ListItem Text="Tidak" Value="0" />
                                                                <asp:ListItem Text="Ya" Value="1" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="padding-left: 5px;">
                                                                        <%=GetLabel("Skor") %>
                                                                    </td>
                                                                    <td style="padding-left: 5px; width: 60px">
                                                                        <asp:TextBox ID="txtFoodIntakeScore" runat="server" Width="100%" ReadOnly="true"
                                                                            CssClass="number" />
                                                                    </td>
                                                                    <td style="width: 60px">
                                                                    </td>
                                                                    <td style="padding-left: 5px; width: 60px">
                                                                        <asp:TextBox ID="txtTotalMST" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                            <%=GetLabel("Pasien dengan Diagnosa Khusus ?") %>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblIsHasSpecificDiagnosis" runat="server" RepeatDirection="Horizontal"
                                                                Enabled="false">
                                                                <asp:ListItem Text="Ya" Value="1" />
                                                                <asp:ListItem Text="Tidak" Value="0" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtGCMSTDiagnosis" runat="server" Width="100%" ReadOnly="true" />
                                                        </td>
                                                        <td style='display: none'>
                                                            <asp:TextBox ID="txtOtherMSTDiagnosis" runat="server" Width="100%" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                                                            <%=GetLabel("Apakah Sudah dibaca dan diketahui oleh tenaga gizi?")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblIsReadedByNutritionist" runat="server" RepeatDirection="Horizontal"
                                                                Enabled="false">
                                                                <asp:ListItem Text="Ya" Value="1" />
                                                                <asp:ListItem Text="Tidak" Value="0" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Pemeriksaan Fisik")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <tr>
                                                        <td>
                                                            <div style="position: relative;">
                                                                <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                                                                    ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#content2ImgLoadingView').show(); }"
                                                                        EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                                                <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="False" ShowHeader="false" EmptyDataRowStyle-CssClass="trEmpty"
                                                                                    OnRowDataBound="grdROSView_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                            <ItemTemplate>
                                                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                            <HeaderTemplate>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <div>
                                                                                                    <b>
                                                                                                        <%#: Eval("ObservationDateInString")%>,
                                                                                                        <%#: Eval("ObservationTime") %>,
                                                                                                        <%#: Eval("ParamedicName") %>
                                                                                                    </b>
                                                                                                </div>
                                                                                                <div>
                                                                                                    <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                                                                        <ItemTemplate>
                                                                                                            <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                                                <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                                    <strong>
                                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                                                                        : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                                                            <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <br style="clear: both" />
                                                                                                        </FooterTemplate>
                                                                                                    </asp:Repeater>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <%=GetLabel("Tidak ada data pemeriksaan fisik untuk pasien ini") %>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dxcp:ASPxCallbackPanel>
                                                                <div class="imgLoadingGrdView" id="content2ImgLoadingView">
                                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                                </div>
                                                                <div class="containerPaging">
                                                                    <div class="wrapperPaging">
                                                                        <div id="rosPaging">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Body Diagram")%></h4>
                                            <div class="containerTblEntryContent containerEntryPanel1">
                                                <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 1px">
                                                    <tr>
                                                        <td>
                                                            <table id="tblBodyDiagramNavigation2" runat="server" border="0" cellpadding="0" cellspacing="0"
                                                                style="display: none; float: right; margin-top: 0px;">
                                                                <tr>
                                                                    <td>
                                                                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px"
                                                                            alt="" class="imgLink" id="btnBodyDiagramContainerPrev2" style="margin-left: 5px;" />
                                                                    </td>
                                                                    <td>
                                                                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px"
                                                                            alt="" class="imgLink" id="btnBodyDiagramContainerNext2" style="margin-left: 5px;" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="position: relative;" id="divBodyDiagram2" runat="server">
                                                                <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpBodyDiagramView2"
                                                                    ShowLoadingPanel="false" OnCallback="cbpBodyDiagramView2_Callback">
                                                                    <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramView2EndCallback(s); }" />
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent40" runat="server">
                                                                            <asp:Panel runat="server" ID="Panel37" CssClass="pnlContainerGrid">
                                                                                <div class="templatePatientBodyDiagram">
                                                                                    <input type="hidden" id="hdnBodyDiagram2ID" runat="server" value='' />
                                                                                    <div class="containerImage boxShadow">
                                                                                        <img src='' alt="" id="imgBodyDiagram2" runat="server" />
                                                                                    </div>
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Diagram Name") %></span>:<br />
                                                                                    <span class="spValue" id="spnDiagramName2" runat="server"></span>
                                                                                    <br />
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Keterangan") %></span>:
                                                                                    <br />
                                                                                    <asp:Repeater ID="rptRemarks2" runat="server">
                                                                                        <HeaderTemplate>
                                                                                            <table>
                                                                                                <colgroup width="20px" />
                                                                                                <colgroup width="2px" />
                                                                                                <colgroup width="15px" />
                                                                                                <colgroup width="2px" />
                                                                                                <colgroup width="60px" />
                                                                                                <colgroup width="2px" />
                                                                                                <colgroup width="*" />
                                                                                                <colgroup width="16px" />
                                                                                                <colgroup width="16px" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <img alt="" style="width: 16px; height: 16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%>
                                                                                                </td>
                                                                                                <td>
                                                                                                    :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "SymbolName")%>
                                                                                                </td>
                                                                                                <td>
                                                                                                    :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "Remarks")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            </table>
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                    <br />
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Physician") %></span>:<br />
                                                                                    <span class="spValue" id="spnParamedicName2" runat="server"></span>
                                                                                    <br />
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Date/Time")%></span>:<br />
                                                                                    <span class="spValue" id="spnObservationDateTime2" runat="server"></span>
                                                                                    <br />
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dxcp:ASPxCallbackPanel>
                                                            </div>
                                                            <table id="tblEmpty2" style="display: none; width: 100%" runat="server">
                                                                <tr class="trEmpty">
                                                                    <td align="center" valign="middle">
                                                                        <%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Diagnosa Pasien")%></h4>
                                            <div class="containerTblEntryContent containerEntryPanel1">
                                                <div style="max-width: 500px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                                        <tr>
                                                            <td>
                                                                <div style="position: relative; max-width: 500px;">
                                                                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisParamedicView" runat="server" Width="100%"
                                                                        ClientInstanceName="cbpDiagnosisParamedicView" ShowLoadingPanel="false" OnCallback="cbpDiagnosisParamedicView_Callback">
                                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisParamedicViewEndCallback(s); }" />
                                                                        <PanelCollection>
                                                                            <dx:PanelContent ID="PanelContent17" runat="server">
                                                                                <asp:Panel runat="server" ID="Panel15" CssClass="pnlContainerGrid" Style="height: 300px">
                                                                                    <asp:GridView ID="grdDiagnosisParamedicView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                                                                                <HeaderStyle CssClass="keyField"></HeaderStyle>
                                                                                                <ItemStyle CssClass="keyField"></ItemStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                                <ItemTemplate>
                                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle CssClass="hiddenColumn"></HeaderStyle>
                                                                                                <ItemStyle CssClass="hiddenColumn"></ItemStyle>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                <ItemTemplate>
                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                        <%#: Eval("DifferentialDateInString")%>,
                                                                                                        <%#: Eval("DifferentialTime")%>,
                                                                                                        <%#: Eval("ParamedicName")%></div>
                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                        <span style="color: Blue; font-size: 1.1em">
                                                                                                            <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                                                                    </div>
                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                        <%#: Eval("ICDBlockName")%></div>
                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                        <b>
                                                                                                            <%#: Eval("DiagnoseType")%></b> -
                                                                                                        <%#: Eval("DifferentialStatus")%></div>
                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                        <%#: Eval("Remarks")%></div>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                                <ItemTemplate>
                                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                                                    <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                                                                    <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                                                                    <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                                                                    <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                                                                    <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                                                                        <EmptyDataTemplate>
                                                                                            <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </dx:PanelContent>
                                                                        </PanelCollection>
                                                                    </dxcp:ASPxCallbackPanel>
                                                                    <div class="containerPaging">
                                                                        <div class="wrapperPaging">
                                                                            <div id="diagnosisParamedicPaging">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Catatan : Hasil Pemeriksaan Penunjang")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                                    <colgroup>
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtDiagnosticResultSummary" runat="server" Width="100%" TextMode="Multiline"
                                                                Rows="10" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Instruksi Dokter")%></h4>
                                            <div class="containerTblEntryContent containerEntryPanel1">
                                                <div style="position: relative;">
                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                        <colgroup>
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtInstructionText" runat="server" Width="100%" TextMode="Multiline"
                                                                    Rows="8" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Sasaran Asuhan")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <colgroup>
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtNursingObjectives" runat="server" Width="100%" TextMode="Multiline"
                                                                Rows="5" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <h4 class="h4collapsed">
                                                <%=GetLabel("Discharge Planning")%></h4>
                                            <div class="containerTblEntryContent">
                                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                                    <colgroup>
                                                        <col width="450px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                            <%=GetLabel("Perkiraan lama hari perawatan") %>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEstimatedLOS" runat="server" Width="60px" CssClass="number" ReadOnly="true" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblEstimatedLOSUnit" runat="server" RepeatDirection="Horizontal"
                                                                Enabled="false">
                                                                <asp:ListItem Text=" hari" Value="1" />
                                                                <asp:ListItem Text=" minggu" Value="0" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                            <%=GetLabel("Rencana Pemulangan Kritis sehingga membutuhkan rencana pemulangan") %>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:RadioButtonList ID="rblIsNeedDischargePlan" runat="server" RepeatDirection="Horizontal"
                                                                Enabled="false">
                                                                <asp:ListItem Text="Ya" Value="1" />
                                                                <asp:ListItem Text="Tidak" Value="0" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="divPage3" class="w3-border divContent w3-animate-left">
                            <div style="width: 100%; height: 500px; overflow: scroll">
                                <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 130px" />
                                        <col style="width: 10px; text-align: center" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td colspan="3" style="width: 100%; text-align: left">
                                            <div id="contentDetail3NavPane" class="w3-bar w3-black">
                                                <a contentid="content3DetailPage1" class="w3-bar-item w3-button w3-text-yellow tablink selected">
                                                    Keluhan dan Riwayat Kesehatan</a> <a contentid="content3DetailPage2" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                        Pemeriksaan Fisik</a> <a contentid="content3DetailPage3" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                            Tanda Vital</a> <a contentid="content3DetailPage4" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                Body Diagram</a> <a contentid="content3DetailPage5" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                    Psikososial Spiritual dan Kultural</a> <a contentid="content3DetailPage6" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                        Kebutuhan Informasi dan Edukasi</a> <a contentid="content3DetailPage7" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                            Perencanaan Pemulangan Pasien</a> <a contentid="content3DetailPage8" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                                Asesmen Tambahan (Populasi Khusus)</a><a contentid="content3DetailPage9" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                                    Resiko Jatuh</a><a contentid="content3DetailPage10" class="w3-bar-item w3-button w3-text-yellow tablink">
                                                                                        Skala Nyeri</a>
                                            </div>
                                            <div id="content3DetailPage1" class="content3Detail w3-animate-top" style="display: none">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <colgroup>
                                                        <col width="50%" />
                                                        <col width="50%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td style="vertical-align: top">
                                                            <h4 class="w3-blue h4expanded">
                                                                <%=GetLabel("Keluhan Utama Pasien")%></h4>
                                                            <div class="containerTblEntryContent">
                                                                <div style="max-height: 220px; overflow-y: auto; padding: 5px 0px 5px 0px">
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
                                                                                            <asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                                                        </td>
                                                                                        <td style="padding-left: 5px">
                                                                                            <asp:TextBox ID="txtTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
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
                                                                                <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="tdLabel" valign="top">
                                                                                <label class="lblNormal">
                                                                                    <%=GetLabel("Keluhan Utama")%></label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtChiefComplaintNurse" Width="100%" runat="server" TextMode="MultiLine"
                                                                                    Rows="5" ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                            <h4 class="w3-blue h4expanded">
                                                                <%=GetLabel("Keluhan lain yang menyertai")%></h4>
                                                            <div class="containerTblEntryContent">
                                                                <div style="max-height: 220px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                                        <colgroup>
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHPISummaryNurse" Width="100%" runat="server" TextMode="MultiLine"
                                                                                    Rows="5" ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkAutoAnamnesisNurse" runat="server" Text="Autoanamnesis" Checked="false"
                                                                                            Enabled="false" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkAlloAnamnesisNurse" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                                                                            Checked="false" Enabled="false" />
                                                                                    </td>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="vertical-align: top">
                                                            <h4 class="w3-blue h4expanded">
                                                                <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
                                                            <div class="containerTblEntryContent">
                                                                <div style="max-height: 220px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                                        <colgroup>
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMedicalHistoryNurse" Width="100%" runat="server" TextMode="MultiLine"
                                                                                    Rows="5" ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                            <h4 class="w3-blue h4collapsed">
                                                                <%=GetLabel("Diagnosa Pasien")%></h4>
                                                            <div class="containerTblEntryContent containerEntryPanel1">
                                                                <div style="max-width: 500px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div style="position: relative; max-width: 500px;">
                                                                                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                                                                                        ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                                                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                                                                                        <PanelCollection>
                                                                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                                                                    <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                                        <Columns>
                                                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                                                                                                <HeaderStyle CssClass="keyField"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="keyField"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                                                <ItemTemplate>
                                                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle CssClass="hiddenColumn"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="hiddenColumn"></ItemStyle>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                                <ItemTemplate>
                                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                                        <%#: Eval("DifferentialDateInString")%>,
                                                                                                                        <%#: Eval("DifferentialTime")%>,
                                                                                                                        <%#: Eval("ParamedicName")%></div>
                                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                                        <span style="color: Blue; font-size: 1.1em">
                                                                                                                            <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                                                                                    </div>
                                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                                        <%#: Eval("ICDBlockName")%></div>
                                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                                        <b>
                                                                                                                            <%#: Eval("DiagnoseType")%></b> -
                                                                                                                        <%#: Eval("DifferentialStatus")%></div>
                                                                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                                                                        <%#: Eval("Remarks")%></div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                                                <ItemTemplate>
                                                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                                                                    <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                                                                                    <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                                                                                    <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                                                                                    <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                                                                                    <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                                                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                                                                                        <EmptyDataTemplate>
                                                                                                            <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
                                                                                                        </EmptyDataTemplate>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                            </dx:PanelContent>
                                                                                        </PanelCollection>
                                                                                    </dxcp:ASPxCallbackPanel>
                                                                                    <div class="containerPaging">
                                                                                        <div class="wrapperPaging">
                                                                                            <div id="diagnosisPaging">
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                            <h4 class="w3-blue h4collapsed">
                                                                <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                                                            <div class="containerTblEntryContent">
                                                                <div style="max-height: 220px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                                        <colgroup>
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMedicationHistoryNurse" Width="100%" runat="server" TextMode="MultiLine"
                                                                                    Rows="5" ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                            <h4 class="w3-blue h4collapsed">
                                                                <%=GetLabel("Riwayat Penyakit Keluarga")%></h4>
                                                            <div class="containerTblEntryContent">
                                                                <div style="max-height: 220px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                                        <colgroup>
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFamilyHistoryNurse" Width="100%" runat="server" TextMode="MultiLine"
                                                                                    Rows="5" ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                            <h4 class="w3-blue h4collapsed">
                                                                <%=GetLabel("Riwayat Alergi")%></h4>
                                                            <div class="containerTblEntryContent">
                                                                <div style="max-height: 450px; overflow-y: auto; padding: 5px 0px 5px 0px">
                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                                                                                    Checked="false" Enabled="false" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                                                                                    ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                                                                                    <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                                                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage3">
                                                                                                <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                                                                                        </asp:BoundField>
                                                                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                                            <ItemTemplate>
                                                                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                                                <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                                                                                                <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                                                                                                <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                                                                                                <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                                                                                                <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                                                                                                                <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px"
                                                                                                            HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                                                                            HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                                                                            HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                                                                            HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Left">
                                                                                                        </asp:BoundField>
                                                                                                    </Columns>
                                                                                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                                                                                    <EmptyDataTemplate>
                                                                                                        <%=GetLabel("Tidak ada data alergi pasien dalam episode ini")%>
                                                                                                    </EmptyDataTemplate>
                                                                                                </asp:GridView>
                                                                                            </asp:Panel>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dxcp:ASPxCallbackPanel>
                                                                                <div class="containerPaging">
                                                                                    <div class="wrapperPaging">
                                                                                        <div id="allergyPaging">
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="content3DetailPage2" class="content3Detail w3-animate-top" style="display: none">
                                                <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
                                                <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
                                                <div id="divFormContent1" runat="server" style="height: 490px; width: 100%; overflow-y: scroll;
                                                    overflow-x: auto;">
                                                </div>
                                            </div>
                                            <div id="content3DetailPage3" class="content3Detail w3-animate-top" style="display: none">
                                                <div>
                                                    <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                                                        ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage4">
                                                                    <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                        OnRowDataBound="grdVitalSignView_RowDataBound">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                <HeaderTemplate>
                                                                                    <h3>
                                                                                        <%=GetLabel("Tanda Vital dan Indikator Lainnya")%></h3>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <div>
                                                                                        <b>
                                                                                            <%#: Eval("ObservationDateInString")%>,
                                                                                            <%#: Eval("ObservationTime") %>,
                                                                                            <%#: Eval("ParamedicName") %>
                                                                                        </b>
                                                                                        <br />
                                                                                        <span style="font-style: italic">
                                                                                            <%#: Eval("Remarks") %>
                                                                                        </span>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div>
                                                                                        <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <div style="padding-left: 20px; float: left; width: 350px;">
                                                                                                    <strong>
                                                                                                        <div style="width: 110px; float: left;" class="labelColumn">
                                                                                                            <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                                                        <div style="width: 20px; float: left;">
                                                                                                            :</div>
                                                                                                    </strong>
                                                                                                    <div style="float: left;">
                                                                                                        <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <br style="clear: both" />
                                                                                            </FooterTemplate>
                                                                                        </asp:Repeater>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            <%=GetLabel("Tidak ada pemeriksaan tanda vital") %>
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dxcp:ASPxCallbackPanel>
                                                </div>
                                                <div style="max-width: 1000px; display: none">
                                                    <div class="containerPaging">
                                                        <div class="wrapperPaging">
                                                            <div id="vitalSignPaging">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="content3DetailPage4" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 1px">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <table id="tblBodyDiagramNavigation" runat="server" border="0" cellpadding="0" cellspacing="0"
                                                                style="float: right; margin-top: 0px;">
                                                                <tr>
                                                                    <td>
                                                                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px"
                                                                            alt="" class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" />
                                                                    </td>
                                                                    <td>
                                                                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px"
                                                                            alt="" class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div style="position: relative;" id="divBodyDiagram" runat="server">
                                                                <dxcp:ASPxCallbackPanel ID="cbpBodyDiagramView" runat="server" Width="100%" ClientInstanceName="cbpBodyDiagramView"
                                                                    ShowLoadingPanel="false" OnCallback="cbpBodyDiagramView_Callback">
                                                                    <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramViewEndCallback(s); }" />
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGrid">
                                                                                <div class="templatePatientBodyDiagram" style="width: 100%; height: 500px; overflow-y: auto">
                                                                                    <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />
                                                                                    <div class="containerImage boxShadow">
                                                                                        <img src='' alt="" id="imgBodyDiagram" runat="server" />
                                                                                    </div>
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Nama Diagram") %></span> : <span class="spValue" id="spnDiagramName"
                                                                                            runat="server"></span>
                                                                                    <br />
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Keterangan") %></span>:
                                                                                    <br />
                                                                                    <asp:Repeater ID="rptRemarks" runat="server">
                                                                                        <HeaderTemplate>
                                                                                            <table>
                                                                                                <colgroup width="20px" />
                                                                                                <colgroup width="2px" />
                                                                                                <colgroup width="15px" />
                                                                                                <colgroup width="2px" />
                                                                                                <colgroup width="60px" />
                                                                                                <colgroup width="2px" />
                                                                                                <colgroup width="*" />
                                                                                                <colgroup width="16px" />
                                                                                                <colgroup width="16px" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <img alt="" style="width: 16px; height: 16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%>
                                                                                                </td>
                                                                                                <td>
                                                                                                    :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "SymbolName")%>
                                                                                                </td>
                                                                                                <td>
                                                                                                    :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%#: DataBinder.Eval(Container.DataItem, "Remarks")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            </table>
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                    <br />
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
                                                                                    <span class="spLabel">
                                                                                        <%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime"
                                                                                            runat="server"></span><br />
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dxcp:ASPxCallbackPanel>
                                                            </div>
                                                            <table id="tblEmpty" style="display: none; width: 100%" runat="server">
                                                                <tr class="trEmpty">
                                                                    <td align="center" valign="middle">
                                                                        <%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="content3DetailPage5" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <input type="hidden" runat="server" id="hdnSocialHistoryLayout" value="" />
                                                <input type="hidden" runat="server" id="hdnSocialHistoryValue" value="" />
                                                <div id="divFormContent2" runat="server" style="min-height: 490px; height: 490px;
                                                    overflow-y: scroll; overflow-x: auto;">
                                                </div>
                                            </div>
                                            <div id="content3DetailPage6" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
                                                <input type="hidden" runat="server" id="hdnEducationValue" value="" />
                                                <div id="divFormContent3" runat="server" style="min-height: 490px; height: 490px;
                                                    overflow-y: scroll; overflow-x: auto;">
                                                </div>
                                            </div>
                                            <div id="content3DetailPage7" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
                                                <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
                                                <div id="divFormContent4" runat="server" style="min-height: 490px; height: 490px;
                                                    overflow-y: scroll; overflow-x: auto;">
                                                </div>
                                            </div>
                                            <div id="content3DetailPage8" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <input type="hidden" runat="server" id="hdnAdditionalAssessmentLayout" value="" />
                                                <input type="hidden" runat="server" id="hdnAdditionalAssessmentValue" value="" />
                                                <div id="divFormContent5" runat="server" style="min-height: 490px; height: 490px;
                                                    overflow-y: scroll; overflow-x: auto;">
                                                </div>
                                            </div>
                                            <div id="content3DetailPage9" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <div id="divFormContent9" runat="server" style="min-height: 490px; height: 490px;
                                                    overflow-y: scroll; overflow-x: auto;">
                                                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col width="30%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="vertical-align: top">
                                                                <div style="position: relative;">
                                                                    <dxcp:ASPxCallbackPanel ID="cbpFormListFallRiskAssessmentHeader" runat="server" Width="100%"
                                                                        ClientInstanceName="cbpFormListFallRiskAssessmentHeader" ShowLoadingPanel="false"
                                                                        OnCallback="cbpFormListFallRiskAssessmentHeader_Callback">
                                                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                                                                            EndCallback="function(s,e){onCbpFormListFallRiskAssessmentHeaderEndCallback(s); }" />
                                                                        <PanelCollection>
                                                                            <dx:PanelContent ID="PanelContent18" runat="server">
                                                                                <asp:Panel runat="server" ID="panFormList" CssClass="pnlContainerGridPatientPage">
                                                                                    <asp:GridView ID="grdFallRiskAssessmentHeader" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Nama Metode" HeaderStyle-CssClass="gridColumnText"
                                                                                                ItemStyle-CssClass="gridColumnText" />
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            <%=GetLabel("Tidak ada template form pengkajian resiko jatuh yang bisa digunakan")%>
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </dx:PanelContent>
                                                                        </PanelCollection>
                                                                    </dxcp:ASPxCallbackPanel>
                                                                    <div class="imgLoadingGrdView" id="containerHdImgLoadingView">
                                                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                                    </div>
                                                                    <div class="containerPaging">
                                                                        <div class="wrapperPaging">
                                                                            <div id="pagingFallRiskAssessment">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="vertical-align: top">
                                                                <dxcp:ASPxCallbackPanel ID="cbpFallRiskAssessmentDetail" runat="server" Width="100%"
                                                                    ClientInstanceName="cbpFallRiskAssessmentDetail" ShowLoadingPanel="false" OnCallback="cbpFallRiskAssessmentDetail_Callback">
                                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                        EndCallback="function(s,e){ oncbpFallRiskAssessmentDetailEndCallback(s); }" />
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent19" runat="server">
                                                                            <asp:Panel runat="server" ID="Panel16" CssClass="pnlContainerGridPatientPage">
                                                                                <asp:GridView ID="grdFallRiskAssessmentDetail" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                        <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                                                            HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                                                                        <asp:BoundField DataField="AssessmentTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                                                            HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime"
                                                                                            ItemStyle-CssClass="assessmentTime" />
                                                                                        <asp:BoundField DataField="ParamedicID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn"
                                                                                            ItemStyle-CssClass="paramedicID hiddenColumn" />
                                                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                                                                        <asp:BoundField DataField="FallRiskScore" HeaderText="Skor" HeaderStyle-HorizontalAlign="Right"
                                                                                            HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                                                                        <asp:BoundField DataField="FallRiskScoreType" HeaderText="Kesimpulan" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                                                                        <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                                                            HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                                                            ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCFallRiskAssessmentType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                                                        <asp:BoundField DataField="AssessmentFormLayout" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                                                        <asp:BoundField DataField="AssessmentFormValue" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                                                        <asp:BoundField DataField="IsInitialAssessment" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn"
                                                                                            ItemStyle-CssClass="isInitialAssessment hiddenColumn" />
                                                                                        <asp:BoundField DataField="cfIsFallRisk" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="cfIsFallRisk hiddenColumn" ItemStyle-CssClass="cfIsFallRisk hiddenColumn" />
                                                                                        <asp:BoundField DataField="FallRiskScore" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="fallRiskScore hiddenColumn" ItemStyle-CssClass="fallRiskScore hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCFallRiskScoreType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="gcFallRiskScoreType hiddenColumn" ItemStyle-CssClass="gcFallRiskScoreType hiddenColumn" />
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <%=GetLabel("Tidak ada data pengkajian untuk pasien ini") %>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dxcp:ASPxCallbackPanel>
                                                                <div class="containerPaging">
                                                                    <div class="wrapperPaging">
                                                                        <div id="pagingFallRiskAssessmentDetail">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div id="content3DetailPage10" class="w3-container w3-border w3-animate-top content3Detail"
                                                style="display: none">
                                                <div id="divFormContent10" runat="server" style="min-height: 490px; height: 490px;
                                                    overflow-y: scroll; overflow-x: auto;">
                                                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col width="25%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td style="vertical-align: top">
                                                                <div style="position: relative;">
                                                                    <dxcp:ASPxCallbackPanel ID="cbpPainAssessmentHeader" runat="server" Width="100%"
                                                                        ClientInstanceName="cbpPainAssessmentHeader" ShowLoadingPanel="false" OnCallback="cbpPainAssessmentHeader_Callback">
                                                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                                                                            EndCallback="function(s,e){ onCbpPainAssessmentHeaderEndCallback(s); }" />
                                                                        <PanelCollection>
                                                                            <dx:PanelContent ID="PanelContent20" runat="server">
                                                                                <asp:Panel runat="server" ID="Panel17" CssClass="pnlContainerGridPatientPage">
                                                                                    <asp:GridView ID="grdcbpPainAssessmentHeader" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Nama Metode" HeaderStyle-CssClass="gridColumnText"
                                                                                                ItemStyle-CssClass="gridColumnText" />
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            <%=GetLabel("Tidak ada template form pengkajian resiko jatuh yang bisa digunakan")%>
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </dx:PanelContent>
                                                                        </PanelCollection>
                                                                    </dxcp:ASPxCallbackPanel>
                                                                    <div class="containerPaging">
                                                                        <div class="wrapperPaging">
                                                                            <div id="pagingPainAssessmentHeader">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="vertical-align: top">
                                                                <dxcp:ASPxCallbackPanel ID="cbpPainAssessmentDetail" runat="server" Width="100%"
                                                                    ClientInstanceName="cbpPainAssessmentDetail" ShowLoadingPanel="false" OnCallback="cbpPainAssessmentDetail_Callback">
                                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                        EndCallback="function(s,e){ onCbpPainAssessmentDetailEndCallback(s); }" />
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent21" runat="server">
                                                                            <asp:Panel runat="server" ID="Panel18" CssClass="pnlContainerGridPatientPage">
                                                                                <asp:GridView ID="grdcbpPainAssessmentDetail" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                        <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                                                            HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                                                                        <asp:BoundField DataField="AssessmentTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                                                            HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime"
                                                                                            ItemStyle-CssClass="assessmentTime" />
                                                                                        <asp:BoundField DataField="ParamedicID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn"
                                                                                            ItemStyle-CssClass="paramedicID hiddenColumn" />
                                                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" HeaderStyle-Width="200px" />
                                                                                        <asp:BoundField DataField="cfProvoking" HeaderText="Penyebab" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-Width="120px" ItemStyle-CssClass="cfProvoking" />
                                                                                        <asp:BoundField DataField="cfQuality" HeaderText="Kualitas" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-Width="120px" ItemStyle-CssClass="cfQuality" />
                                                                                        <asp:BoundField DataField="cfRegio" HeaderText="Regio/Lokasi" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-Width="120px" ItemStyle-CssClass="cfRegio" />
                                                                                        <asp:BoundField DataField="PainScore" HeaderText="Skor" HeaderStyle-HorizontalAlign="Right"
                                                                                            HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                                                        <asp:BoundField DataField="cfTime" HeaderText="Waktu" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-Width="120px" ItemStyle-CssClass="cfTime" />
                                                                                        <asp:BoundField DataField="PainScoreType" HeaderText="Kesimpulan" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                                                                        <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                                                            HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                                                            ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCPainAssessmentType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCProvoking" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="gcProvoking hiddenColumn" ItemStyle-CssClass="gcProvoking hiddenColumn" />
                                                                                        <asp:BoundField DataField="Provoking" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="provoking hiddenColumn" ItemStyle-CssClass="provoking hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCQuality" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="gcQuality hiddenColumn" ItemStyle-CssClass="gcQuality hiddenColumn" />
                                                                                        <asp:BoundField DataField="Quality" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="quality hiddenColumn" ItemStyle-CssClass="quality hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCRegio" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="gcRegio hiddenColumn" ItemStyle-CssClass="gcRegio hiddenColumn" />
                                                                                        <asp:BoundField DataField="Regio" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="regio hiddenColumn" ItemStyle-CssClass="regio hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCTime" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="gcRegio hiddenColumn" ItemStyle-CssClass="gcTime hiddenColumn" />
                                                                                        <asp:BoundField DataField="Time" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="time hiddenColumn" ItemStyle-CssClass="time hiddenColumn" />
                                                                                        <asp:BoundField DataField="AssessmentFormLayout" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                                                        <asp:BoundField DataField="AssessmentFormValue" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                                                        <asp:BoundField DataField="IsInitialAssessment" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn"
                                                                                            ItemStyle-CssClass="isInitialAssessment hiddenColumn" />
                                                                                        <asp:BoundField DataField="cfIsInitialAssessment" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="cfIsInitialAssessment hiddenColumn"
                                                                                            ItemStyle-CssClass="cfIsInitialAssessment hiddenColumn" />
                                                                                        <asp:BoundField DataField="cfIsPain" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="cfIsPain hiddenColumn" ItemStyle-CssClass="cfIsPain hiddenColumn" />
                                                                                        <asp:BoundField DataField="PainScore" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="PainScore hiddenColumn" ItemStyle-CssClass="PainScore hiddenColumn" />
                                                                                        <asp:BoundField DataField="GCPainScoreType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                                            HeaderStyle-CssClass="gcPainScoreType hiddenColumn" ItemStyle-CssClass="gcPainScoreType hiddenColumn" />
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <%=GetLabel("Tidak ada data pengkajian untuk pasien ini") %>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dxcp:ASPxCallbackPanel>
                                                                <div class="containerPaging">
                                                                    <div class="wrapperPaging">
                                                                        <div id="pagingPainAssessmentDetail">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top">
                                        </td>
                                        <td />
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="divPage4" class="w3-border divContent w3-animate-left">
                            <div style="width: 100%; height: 500px; overflow: scroll">
                                <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 130px" />
                                        <col style="width: 10px; text-align: center" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td colspan="3">
                                            <div id="filterArea">
                                                <table style="margin-top: 10px; margin-bottom: 10px">
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal" style="font-weight: bold">
                                                                <%=GetLabel("Tanggal ")%></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <%=GetLabel("s/d") %>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                                                        </td>
                                                        <td style="width: 30px">
                                                        </td>
                                                        <td class="tdLabel" style="display: none; width: 150px">
                                                            <label>
                                                                <%=GetLabel("Display Option")%></label>
                                                        </td>
                                                        <td style='display: none'>
                                                            <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                                                                Width="300px">
                                                                <ClientSideEvents ValueChanged="function() { cbpMedicalSummaryContent4View.PerformCallback('refresh'); }" />
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td style='display: none'>
                                                            <input type="button" class="btnContentRefresh w3-btn w3-hover-blue" value="Refresh"
                                                                style="background-color: Red; color: White; width: 100px;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <dxcp:ASPxCallbackPanel ID="cbpMedicalSummaryContent4View" runat="server" Width="100%"
                                                ClientInstanceName="cbpMedicalSummaryContent4View" ShowLoadingPanel="false" OnCallback="cbpMedicalSummaryContent4View_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingContent4View').show(); }"
                                                    EndCallback="function(s,e){ oncbpMedicalSummaryContent4ViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdMedicalSummaryContent4" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty"
                                                                ShowHeader="true" OnRowDataBound="grdMedicalSummaryContent4_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                                    <asp:BoundField DataField="DepartmentID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn departmentID" />
                                                                    <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                                                    <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                                                    <asp:BoundField DataField="GCPatientNoteType" HeaderStyle-CssClass="controlColumn"
                                                                        ItemStyle-CssClass="controlColumn" />
                                                                    <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                                                    <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                                                    <asp:BoundField DataField="NoteTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                                                    <asp:TemplateField HeaderText="PPA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <%#:Eval("cfPPA") %>
                                                                            </div>
                                                                            <div>
                                                                                <img class="imgNeedConfirmation" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                                                    alt="" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                                    cursor: pointer; min-width: 30px;' title="Need confirmation" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SOAP" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <span style="color: blue; font-style: italic; vertical-align: top">
                                                                                    <%#:Eval("ParamedicName") %>
                                                                                    - <b>
                                                                                        <%#:Eval("DepartmentID") %>
                                                                                        (<%#:Eval("ServiceUnitName") %>)
                                                                                        <%#:Eval("cfParamedicMasterType") %>
                                                                                        <div style='display: none'>
                                                                                            <span style="float: right; <%# Eval("IsEdited").ToString() == "False" ? "display:none": "" %>">
                                                                                                <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                                                                                    alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                                                                                            </span>
                                                                                        </div>
                                                                            </div>
                                                                            <div style="height: 130px; overflow-y: auto; margin-top: 15px;">
                                                                                <%#Eval("NoteText").ToString().Replace("\n","<br />")%><br />
                                                                            </div>
                                                                            <div id="divView" runat="server" style='display: none; margin-top: 5px;'>
                                                                                <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue"
                                                                                    value="Detail Kajian Awal" style='width: 150px; background-color: Green; color: White;' />
                                                                            </div>
                                                                            <div style="margin-top: 10px; text-align: left">
                                                                                <table border="0" cellpadding="1" cellspacing="0">
                                                                                    <tr id="divNursingNotesInfo" runat="server">
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkIsWritten" runat="server" Enabled="false" Checked='<%# Eval("IsWrite")%>' />
                                                                                            TULIS
                                                                                            <asp:CheckBox ID="chkIsReadback" runat="server" Enabled="false" Checked='<%# Eval("IsReadback")%>' />
                                                                                            BACA
                                                                                            <asp:CheckBox ID="chkIsConfirmed" runat="server" Enabled="false" Checked='<%# Eval("IsConfirmed")%>' />
                                                                                            KONFIRMASI
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="divConfirmationInfo" runat="server">
                                                                                        <td>
                                                                                            <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                                                                <span style='color: red;'>Konfirmasi : </span><span style='color: Blue;'>
                                                                                                    <%#:Eval("cfConfirmationDateTime") %>,
                                                                                                    <%#:Eval("ConfirmationPhysicianName") %></span>
                                                                                                <div id="divConfirmationRemarks">
                                                                                                    <br />
                                                                                                    <textarea style="border: 0; width: 99%; height: auto; background-color: transparent;
                                                                                                        font-style: italic" readonly><%#: DataBinder.Eval(Container.DataItem, "ConfirmationRemarks") %> </textarea>
                                                                                                </div>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                                                    readonly><%#: DataBinder.Eval(Container.DataItem, "InstructionText") %> </textarea>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <div style="color: blue; font-style: italic; vertical-align: top; display: none">
                                                                                <%#:Eval("cfCreatedDate") %>,
                                                                            </div>
                                                                            <div>
                                                                                <b>
                                                                                    <label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'>
                                                                                        <%#:Eval("cfCreatedByName") %></label></b>
                                                                            </div>
                                                                            <div id="divParamedicSignature" runat="server" style='display: none; margin-top: 5px;
                                                                                text-align: left'>
                                                                                <input type="button" id="btnSignature" runat="server" class="btnSignature" value="Ttd"
                                                                                    title="Tanda Tangan" style='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>' />
                                                                            </div>
                                                                            <div>
                                                                                <img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                                                    alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                                    cursor: pointer; min-width: 30px; float: left;' title="Using Notification" /></div>
                                                                            <div id="divVerified" align="center" style='<%# Eval("IsVerified").ToString() == "True" ? "display:none;": "" %>'>
                                                                                <br />
                                                                                <div>
                                                                                    <input type="button" id="btnVerify" runat="server" class="btnVerify" value="VERIFY"
                                                                                        style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                                                </div>
                                                                            </div>
                                                                            <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                                                    <span style='color: red;'>Verifikasi :</span>
                                                                                    <br />
                                                                                    <span style='color: Blue;'>
                                                                                        <%#:Eval("cfVerifiedPrimaryNurseDateTime") %>
                                                                                        <br />
                                                                                        <%#:Eval("VerifiedPrimaryNurseName") %></span>
                                                                                </div>
                                                                            </div>
                                                                            <div id="divPhysicianVerifiedInformation" runat="server" style="margin-top: 10px;
                                                                                text-align: left">
                                                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                                                    <span style='color: red;'>Verified :</span>
                                                                                    <br />
                                                                                    <span style='color: Blue;'>
                                                                                        <%#:Eval("cfVerifiedDateTime") %>,
                                                                                        <%#:Eval("VerifiedPhysicianName") %></span>
                                                                                    <div id="divVerificationRemarks">
                                                                                        <br />
                                                                                        <textarea style="border: 0; width: 99%; height: auto; background-color: transparent;
                                                                                            font-style: italic" readonly><%#: DataBinder.Eval(Container.DataItem, "VerificationRemarks") %> </textarea>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Signature1" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature1" />
                                                                    <asp:BoundField DataField="Signature2" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature2" />
                                                                    <asp:BoundField DataField="Signature3" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature3" />
                                                                    <asp:BoundField DataField="Signature4" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature4" />
                                                                    <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada catatan terintegrasi untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="imgLoadingGrdView" id="containerImgLoadingContent4View">
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div style="max-width: 1000px;">
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingContent4">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="divPage5" class="w3-border divContent w3-animate-left">
                            <div style="width: 100%; height: 500px; overflow: scroll">
                                <div>
                                    <table style="margin-top: 10px; margin-bottom: 10px">
                                        <tr style='display: none'>
                                            <td class="tdLabel" style="width: 150px">
                                                <label>
                                                    <%=GetLabel("Display Option")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboDisplay1" ClientInstanceName="cboDisplay1" runat="server"
                                                    Width="300px">
                                                    <ClientSideEvents ValueChanged="function() { cbpMedicalSummaryContent5View.PerformCallback('refresh'); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <dxcp:ASPxCallbackPanel ID="cbpMedicalSummaryContent5View" runat="server" Width="100%"
                                    ClientInstanceName="cbpMedicalSummaryContent5View" ShowLoadingPanel="false" OnCallback="cbpMedicalSummaryContent5View_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                        EndCallback="function(s,e){ onCbpMedicalSummaryContent5ViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent7" runat="server">
                                            <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdMedicalSummaryContent5" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty"
                                                    ShowHeader="true">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                                        <asp:BoundField DataField="cfJournalDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="JournalTime" HeaderText="Jam" HeaderStyle-Width="60px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <span style="color: blue; font-style: italic; vertical-align: top">
                                                                        <%#:Eval("ParamedicName") %>
                                                                        - <b>(<%#:Eval("ServiceUnitName") %>) <span style="color: red">
                                                                            <%#:Eval("cfConfirmationInfo") %></span></b> </span>
                                                                </div>
                                                                <div style="height: 130px; overflow-y: auto; margin-top: 15px;">
                                                                    <%#Eval("Remarks").ToString().Replace("\n","<br />")%><br />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <div style="color: blue; font-style: italic; vertical-align: top">
                                                                    <%#:Eval("cfLastUpdatedDate") %>,
                                                                </div>
                                                                <div>
                                                                    <b>
                                                                        <%#:Eval("cfLastUpdatedByUserName") %></b>
                                                                </div>
                                                                <div>
                                                                    <img class="imgNeedConfirmation" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                                        alt="" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                        cursor: pointer; min-width: 30px; float: left;' title="Need confirmation" /></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada catatan perawat untuk pasien ini") %>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingContent5View">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingContent5">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divPage6" class="w3-border divContent w3-animate-left">
                            <dxcp:ASPxCallbackPanel ID="cbpViewPatientTransferList" runat="server" Width="100%"
                                ClientInstanceName="cbpViewPatientTransferList" ShowLoadingPanel="false" OnCallback="cbpViewPatientTransferList_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerImgLoadingView'));showLoadingPanel(); }"
                                    EndCallback="function(s,e){ onCbpViewPatientTransferListEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent38" runat="server">
                                        <asp:Panel runat="server" ID="Panel35" CssClass="pnlContainerGridPatientPage">
                                            <asp:GridView ID="grdViewPatientTransferList" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                                alt='' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="IsConfirmed" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="hiddenColumn isConfirmed" />
                                                    <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isEditable" />
                                                    <asp:BoundField DataField="FromNurseID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fromNurseID" />
                                                    <asp:BoundField DataField="cfTransferDate" HeaderText="Tanggal" HeaderStyle-Width="80px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="TransferTime" HeaderText="Jam" HeaderStyle-Width="50px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="PatientNurseTransferType" HeaderText="Jenis Transfer"
                                                        HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="FromNurseName" HeaderText="Dari Perawat" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="ToNurseName" HeaderText="Ke Perawat" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="cfConfirmedDateTime" HeaderText="Tanggal Konfirmasi" HeaderStyle-Width="130px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="cfIsConfirmed" HeaderText="Dikonfirmasi" HeaderStyle-Width="80px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada riwayat Pasien Pindah / Patient Handover")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingPatientTransferList">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tempContainerGrdDetail" style="display: none">
                            <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px;">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewPatientTransferDetail" runat="server" Width="100%"
                                        ClientInstanceName="cbpViewPatientTransferDetail" ShowLoadingPanel="false" OnCallback="cbpViewPatientTransferDetail_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent39" runat="server">
                                                <asp:Panel runat="server" ID="Panel36" Style="width: 100%; margin-left: auto; margin-right: auto">
                                                    <asp:GridView ID="grdPatientTransferDetail" runat="server" CssClass="grdSelected"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="Situation" HeaderText="Situation" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Background" HeaderText="Background" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Assessment" HeaderText="Assessment" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Recommendation" HeaderText="Recommendation" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Data Tidak Tersedia")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divPage7" class="w3-border divContent w3-animate-left">
                            <dxcp:ASPxCallbackPanel ID="cbpViewPatientReferralList" runat="server" Width="100%"
                                ClientInstanceName="cbpViewPatientReferralList" ShowLoadingPanel="false" OnCallback="cbpViewPatientReferralList_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                    EndCallback="function(s,e){ onCbpViewPatientReferralListEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent16" runat="server">
                                        <asp:Panel runat="server" ID="Panel14" CssClass="pnlContainerGridPatientPage">
                                            <asp:GridView ID="grdViewPatientReferralList" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                        <ItemTemplate>
                                                            <input type="hidden" value="<%#:Eval("ID") %>" class="hdnID" bindingfield="ID" />
                                                            <input type="hidden" value="<%#:Eval("cfReferralDate") %>" class="hdnReferralDate"
                                                                bindingfield="cfReferralDate" />
                                                            <input type="hidden" value="<%#:Eval("ReferralTime") %>" class="hdnReferralTime"
                                                                bindingfield="ReferralTime" />
                                                            <input type="hidden" value="<%#:Eval("FromPhysicianID") %>" class="hdnFromPhysicianID"
                                                                bindingfield="FromPhysicianID" />
                                                            <input type="hidden" value="<%#:Eval("FromPhysicianName") %>" class="hdnFromPhysicianName"
                                                                bindingfield="FromPhysicianName" />
                                                            <input type="hidden" value="<%#:Eval("GCRefferalType") %>" class="hdnGCRefferalType"
                                                                bindingfield="GCRefferalType" />
                                                            <input type="hidden" value="<%#:Eval("ToHealthcareServiceUnitID") %>" class="hdnToHealthcareServiceUnitID"
                                                                bindingfield="ToHealthcareServiceUnitID" />
                                                            <input type="hidden" value="<%#:Eval("ToServiceUnitCode") %>" class="hdnToServiceUnitCode"
                                                                bindingfield="ToServiceUnitCode" />
                                                            <input type="hidden" value="<%#:Eval("ToServiceUnitName") %>" class="hdnToServiceUnitName"
                                                                bindingfield="ToServiceUnitName" />
                                                            <input type="hidden" value="<%#:Eval("ToPhysicianID") %>" class="hdnToPhysicianID"
                                                                bindingfield="ToPhysicianID" />
                                                            <input type="hidden" value="<%#:Eval("ToPhysicianCode") %>" class="hdnToPhysicianCode"
                                                                bindingfield="ToPhysicianCode" />
                                                            <input type="hidden" value="<%#:Eval("ToPhysicianName") %>" class="hdnToPhysicianName"
                                                                bindingfield="ToPhysicianName" />
                                                            <input type="hidden" value="<%#:Eval("ToSpecialtyID") %>" class="ToSpecialtyID" bindingfield="ToSpecialtyID" />
                                                            <input type="hidden" value="<%#:Eval("cfResponseDateTime") %>" class="hdncfResponseDateTime"
                                                                bindingfield="cfResponseDateTime" />
                                                            <input type="hidden" value="<%#:Eval("IsReply") %>" class="hdnIsReply" bindingfield="IsReply" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="cfReferralDate" HeaderText="Tanggal" HeaderStyle-Width="80px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ReferralTime" HeaderText="Jam" HeaderStyle-Width="50px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ReferralType" HeaderText="Jenis Rujukan" HeaderStyle-Width="150px"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Dirujuk/Konsultasi Ke" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#Eval("ToPhysicianName")%><br />
                                                            </div>
                                                            <div>
                                                                <%#Eval("ToServiceUnitName")%><br />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Diagnosa" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="250px">
                                                        <ItemTemplate>
                                                            <div style="height: 130px; overflow-y: auto; vertical-align: top; white-space: normal;">
                                                                <%#Eval("DiagnosisText").ToString().Replace("\n","<br />")%><br />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pemeriksaan Penunjang" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <div style="height: 130px; overflow-y: auto; vertical-align: top; white-space: normal;">
                                                                <%#Eval("MedicalResumeText").ToString().Replace("\n","<br />")%><br />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Terapi yang sudah diberikan" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px">
                                                        <ItemTemplate>
                                                            <div style="height: 130px; overflow-y: auto; vertical-align: top; white-space: normal;">
                                                                <%#Eval("PlanningResumeText").ToString().Replace("\n","<br />")%><br />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="cfResponseDateTime" HeaderText="Tanggal Response" HeaderStyle-Width="130px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="cfIsResponse" HeaderText="Response" HeaderStyle-Width="80px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Print" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <img class="imgPatientReferralPrint imglink" title='<%=GetLabel("Formulir Konsultasi/Rawat Bersama/Alih Rawat")%>'
                                                                alt="" src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>' />
                                                            <br />
                                                            <br />
                                                            <img class="imgPatientReferralAnswerPrint imglink" title='<%=GetLabel("Formulir Jawaban Konsultasi/Rawat Bersama/Alih Rawat")%>'
                                                                alt="" src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>' />
                                                            <input type="hidden" value="<%#:Eval("VisitID") %>" bindingfield="VisitID" />
                                                            <input type="hidden" class="hdnReferralID" value="<%#: Eval("ID")%>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada riwayat konsultasi / rawat bersama")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingPatientReferralList">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divPage8" class="w3-border divContent w3-animate-left">
                            <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div style="padding: 5px; min-height: 300px;">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewSugery" runat="server" Width="100%" ClientInstanceName="cbpViewSugery"
                                                ShowLoadingPanel="false">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent8" runat="server">
                                                        <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                                            margin-right: auto; position: relative; font-size: 0.95em;">
                                                            <asp:ListView ID="lvwViewSugery" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblView" runat="server" class="grdSelected notAllowSelect grdSurgery"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 150px" align="left">
                                                                                <%= GetLabel("Nomor Order")%>
                                                                            </th>
                                                                            <th style="width: 120px" align="center">
                                                                                <%= GetLabel("Tanggal Order")%>
                                                                            </th>
                                                                            <th style="width: 140px" align="left">
                                                                                <%=GetLabel("Dokter")%>
                                                                            </th>
                                                                            <th style="width: 200px" align="left">
                                                                                <%=GetLabel("Pre Diagnosis")%>
                                                                            </th>
                                                                            <th style="width: 200px" align="left">
                                                                                <%=GetLabel("Post Diagnosis")%>
                                                                            </th>
                                                                            <th style="width: 50px" align="center">
                                                                                <%=GetLabel("Print")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="7">
                                                                                <%=GetLabel("Tidak ada informasi Laporan Operasi pada saat ini") %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblView" runat="server" class="grdSelected notAllowSelect grdSurgery"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 150px" align="left">
                                                                                <%= GetLabel("Nomor Order")%>
                                                                            </th>
                                                                            <th style="width: 120px" align="center">
                                                                                <%= GetLabel("Tanggal Order")%>
                                                                            </th>
                                                                            <th style="width: 140px" align="left">
                                                                                <%=GetLabel("Dokter")%>
                                                                            </th>
                                                                            <th style="width: 200px" align="left">
                                                                                <%=GetLabel("Pre Diagnosis")%>
                                                                            </th>
                                                                            <th style="width: 200px" align="left">
                                                                                <%=GetLabel("Post Diagnosis")%>
                                                                            </th>
                                                                            <th style="width: 50px" align="center">
                                                                                <%=GetLabel("Print")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr runat="server" id="trItem">
                                                                        <td align="left" style="width: 150px">
                                                                            <div>
                                                                                <%#: Eval("TestOrderNo") %></div>
                                                                        </td>
                                                                        <td align="center" style="width: 120px">
                                                                            <div>
                                                                                <%#: Eval("cfReportDate") %></div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div>
                                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientSurgeryID")%>" />
                                                                                <%#: Eval("ParamedicName") %></div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div>
                                                                                <%#: Eval("PreOperativeDiagnosisText") %></div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div>
                                                                                <%#: Eval("PostOperativeDiagnosisText") %></div>
                                                                        </td>
                                                                        <td align="center" style="width: 50px">
                                                                            <img class="btnPrintSurgeryReportList imglink" title='<%=GetLabel("Laporan Operasi Versi Lengkap")%>'
                                                                                alt="" src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>' />
                                                                            <input type="hidden" value="<%#:Eval("VisitID") %>" bindingfield="VisitID" />
                                                                            <input type="hidden" class="hdnSugeryID" value="<%#: Eval("PatientSurgeryID")%>" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage9" class="w3-border divContent w3-animate-left">
                            <table style="width: 100%">
                                <colgroup>
                                    <col style="width: 20%" />
                                    <col style="width: 80%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <div style="position: relative; width: 100%">
                                            <table border="0" cellpadding="0" cellspacing="1">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Filter")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:RadioButtonList ID="rblItemTypeSurgeryHistory" runat="server" RepeatDirection="Horizontal"
                                                            RepeatLayout="Table">
                                                            <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                                            <asp:ListItem Text=" Perawatan saat ini " Value="1" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" valign="top">
                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                        </table>
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewSurgeryHistory" runat="server" Width="100%" ClientInstanceName="cbpViewSurgeryHistory"
                                                            ShowLoadingPanel="false" OnCallback="cbpViewSurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewSurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent22" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel19" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewSurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:BoundField DataField="TestOrderNo" HeaderStyle-CssClass="hiddenColumn orderNo"
                                                                                    ItemStyle-CssClass="hiddenColumn orderNo" />
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        <div>
                                                                                            <%=GetLabel("Tanggal ") %>
                                                                                            -
                                                                                            <%=GetLabel("Jam Order") %>, <span style="color: blue">
                                                                                                <%=GetLabel("No. Order") %></span></div>
                                                                                        <div style="font-weight: bold">
                                                                                            <%=GetLabel("Diorder Oleh") %></div>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div>
                                                                                                        <%#: Eval("TestOrderDateTimeInString")%>, <span>
                                                                                                            <%#: Eval("TestOrderNo")%></span></div>
                                                                                                    <div style="font-weight: bold; color: blue">
                                                                                                        <%#: Eval("ParamedicName") %></div>
                                                                                                    <div style="font-style: italic; color: Red" class="blink">
                                                                                                        <%#: Eval("ScheduleStatus")%>,
                                                                                                        <%#: Eval("cfRoomScheduleDate")%>
                                                                                                        -
                                                                                                        <%#: Eval("RoomScheduleTime")%></div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                    <ItemTemplate>
                                                                                        <input type="hidden" value="<%#:Eval("EstimatedDuration") %>" bindingfield="EstimatedDuration" />
                                                                                        <input type="hidden" value="<%#:Eval("IsUsingSpecificItem") %>" bindingfield="IsUsingSpecificItem" />
                                                                                        <input type="hidden" value="<%#:Eval("IsUsedRequestTime") %>" bindingfield="IsUsedRequestTime" />
                                                                                        <input type="hidden" value="<%#:Eval("IsCITO") %>" bindingfield="IsCITO" />
                                                                                        <input type="hidden" value="<%#:Eval("cfScheduledDateInString") %>" bindingfield="cfScheduledDateInString" />
                                                                                        <input type="hidden" value="<%#:Eval("ScheduledTime") %>" bindingfield="ScheduledTime" />
                                                                                        <input type="hidden" value="<%#:Eval("RoomCode") %>" bindingfield="RoomCode" />
                                                                                        <input type="hidden" value="<%#:Eval("RoomName") %>" bindingfield="RoomName" />
                                                                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <%=GetLabel("Tidak ada informasi order Penunjang untuk pasien ini")%>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistory">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                                        <ul class="ulTabPage" id="ulTabOrderDetailSurgeryHistory">
                                                            <li class="selected" contentid="preSurgeryAssessment">
                                                                <%=GetLabel("Pra Bedah")%></li>
                                                            <li contentid="preAnesthesyAssessment">
                                                                <%=GetLabel("Pra Anestesi")%></li>
                                                            <li contentid="anesthesyStatus">
                                                                <%=GetLabel("Status Anestesi")%></li>
                                                            <li contentid="preSurgicalSafetyChecklist">
                                                                <%=GetLabel("Surgical Safety Checklist")%></li>
                                                            <li contentid="periOperative">
                                                                <%=GetLabel("Perioperatif")%></li>
                                                            <li contentid="surgeryReport">
                                                                <%=GetLabel("Laporan Operasi")%></li>
                                                            <li contentid="patientMedicalDevice">
                                                                <%=GetLabel("Pemasangan Implant")%></li>
                                                            <li contentid="surgeryDocument">
                                                                <%=GetLabel("e-Document")%></li>
                                                        </ul>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="containerOrderDt" id="preSurgeryAssessment">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDtSurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDtSurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDtSurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDtSurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent23" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel20" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDtSurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <%--                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img class="imgViewPreSurgeryAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>'
                                                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreSurgicalAssessmentID") %>"
                                                                                                    visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                                <asp:BoundField DataField="PreSurgicalAssessmentID" HeaderStyle-CssClass="keyField"
                                                                                    ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                    <ItemTemplate>
                                                                                        <input type="hidden" value="<%#:Eval("PreSurgicalAssessmentID") %>" bindingfield="PreSurgicalAssessmentID" />
                                                                                        <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                                        <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                                                <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                                                <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                                                <asp:BoundField HeaderText="Pre Diagnosis" DataField="PreDiagnoseText" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div class="blink">
                                                                                        <%=GetLabel("Belum ada Asesmen Pra Bedah untuk pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt1">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="preAnesthesyAssessment" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt7SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt7SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt7SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt7SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent24" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel21" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt7SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <%--                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img class="imgViewPreAnesthesyAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreAnesthesyAssessmentID") %>"
                                                                                                    visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" testorderno="<%#:Eval("TestOrderNo") %>" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                                <asp:BoundField DataField="PreAnesthesyAssessmentID" HeaderStyle-CssClass="keyField"
                                                                                    ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                    <ItemTemplate>
                                                                                        <input type="hidden" value="<%#:Eval("PreAnesthesyAssessmentID") %>" bindingfield="PreAnesthesyAssessmentID" />
                                                                                        <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                                        <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                                                <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                                                <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="ASA" DataField="cfASAStatus" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" />
                                                                                <asp:BoundField HeaderText="Teknik Anestesi" DataField="cfAnesthesiaType" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada Asesmen Pra Anestesi untuk pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt7">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="anesthesyStatus" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt8SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt8SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt8SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt8SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent25" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel22" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt8SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <%--                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img class="imgViewAnesthesyStatus imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("AnesthesyStatusID") %>"
                                                                                                    visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                                <asp:BoundField DataField="AnesthesyStatusID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                    <ItemTemplate>
                                                                                        <input type="hidden" value="<%#:Eval("AnesthesyStatusID") %>" bindingfield="AnesthesyStatusID" />
                                                                                        <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                                        <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="AssessmentTime" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                                                <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                                                <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada Status Anestesi untuk pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt8">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="preSurgicalSafetyChecklist" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt2SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt2SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt2SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt2SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent26" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel23" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt2SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                                                    HeaderText="SIGN IN">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <colgroup>
                                                                                                <col style="width: 60px" />
                                                                                                <col />
                                                                                            </colgroup>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("cfSignInDateTime") %></div>
                                                                                                    <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("SignInParamedicName") %></div>
                                                                                                    <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Belum ada informasi Sign In Surgical Safety Check List untuk pasien ini") %></div>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                                    HeaderStyle-Width="220px" HeaderText="TIME OUT">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <colgroup>
                                                                                                <col style="width: 60px" />
                                                                                                <col />
                                                                                            </colgroup>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("cfTimeOutDateTime") %></div>
                                                                                                    <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("TimeOutParamedicName") %></div>
                                                                                                    <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Belum ada informasi Time Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                                                    HeaderText="SIGN OUT">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <colgroup>
                                                                                                <col style="width: 60px" />
                                                                                                <col />
                                                                                            </colgroup>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("cfSignOutDateTime") %></div>
                                                                                                    <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("SignOutNurseName") %></div>
                                                                                                    <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Belum ada informasi Sign Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada formulir Surgical Safety Check List untuk pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt2">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="periOperative" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt6SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt6SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt6SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt6SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent27" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel24" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt6SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                                                    HeaderText="PRE OPERATIVE">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <colgroup>
                                                                                                <col style="width: 60px" />
                                                                                                <col />
                                                                                            </colgroup>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("cfPreOperativeDateTime") %></div>
                                                                                                    <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("PreOperativeSurgeryNurseName") %></div>
                                                                                                    <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Belum ada informasi Pre Operasi untuk pasien ini") %></div>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                                                    HeaderText="INTRA OPERASI">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <colgroup>
                                                                                                <col style="width: 60px" />
                                                                                                <col />
                                                                                            </colgroup>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("cfIntraOperativeDateTime") %></div>
                                                                                                    <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("IntraOperativeRecoveryRoomNurseName") %></div>
                                                                                                    <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Belum ada informasi Intra Operasi untuk pasien ini") %></div>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                                                    HeaderText="PASKA OPERASI">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <colgroup>
                                                                                                <col style="width: 60px" />
                                                                                                <col />
                                                                                            </colgroup>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("cfPostOperativeDateTime") %></div>
                                                                                                    <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                                        <%#:Eval("PostOperativeRecoveryRoomNurseName") %></div>
                                                                                                    <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Belum ada informasi Paska Operasi untuk pasien ini") %></div>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada Asuhan Keperawatan Perioperatif untuk pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt6">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="surgeryReport" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt4SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt4SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt4SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt4SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent28" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel25" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt4SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="PatientSurgeryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                    <ItemTemplate>
                                                                                        <input type="hidden" value="<%#:Eval("PatientSurgeryID") %>" bindingfield="PatientSurgeryID" />
                                                                                        <input type="hidden" value="<%#:Eval("cfReportDate") %>" bindingfield="cfReportDate" />
                                                                                        <input type="hidden" value="<%#:Eval("ReportTime") %>" bindingfield="ReportTime" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Tanggal" DataField="cfReportDate" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                                                <asp:BoundField HeaderText="Jam" DataField="ReportTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                                                <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                                                <asp:BoundField HeaderText="Pre Diagnosis" DataField="PreOperativeDiagnosisText"
                                                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Post Diagnosis" DataField="PostOperativeDiagnosisText"
                                                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada laporan operasi untuk pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt4">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="patientMedicalDevice" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt3SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt3SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt3SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt3SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent29" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel26" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt3SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:BoundField DataField="cfImplantDate" HeaderText="Tanggal Pemasangan" HeaderStyle-Width="100px"
                                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="ItemName" HeaderText="Nama Implant" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" HeaderStyle-Width="200px"
                                                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada informasi pemasangan implant untuk tindakan operasi di pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt3">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="surgeryDocument" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt5SurgeryHistory" runat="server" Width="100%"
                                                            ClientInstanceName="cbpViewDt5SurgeryHistory" ShowLoadingPanel="false" OnCallback="cbpViewDt5SurgeryHistory_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt5SurgeryHistoryEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent30" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel27" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt5SurgeryHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px"
                                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px"
                                                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn"
                                                                                    ItemStyle-CssClass="hiddenColumn fileName" />
                                                                                <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div>
                                                                                        <%=GetLabel("Belum ada informasi e=document untuk tindakan operasi di pasien ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingSurgeryHistoryDt5">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage10" class="w3-border divContent w3-animate-left">
                            <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 40%" />
                                    <col style="width: 60%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <div style="position: relative; width: 100%; padding-top: 26px">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewIntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpViewIntakeOutput"
                                                            ShowLoadingPanel="false">
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent9" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage5">
                                                                        <asp:GridView ID="grdViewIntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:BoundField DataField="cfLogDate1" HeaderStyle-CssClass="hiddenColumn cfLogDate1"
                                                                                    ItemStyle-CssClass="hiddenColumn cfLogDate1" />
                                                                                <asp:BoundField HeaderText="TANGGAL" DataField="cfLogDate" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" ItemStyle-CssClass="cfLogDate" />
                                                                                <asp:TemplateField HeaderText="INTAKE" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                                                    HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div style="text-align: right; color: blue">
                                                                                            <label class="lblIntakeSummary" visitid='<%#:Eval("VisitID")%>' logdate='<%#:Eval("cfLogDate")%>'>
                                                                                                <%#:Eval("TotalIntake", "{0:N}")%></label>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="INTAKE TIDAK DIUKUR" HeaderStyle-HorizontalAlign="Right"
                                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div style="text-align: right; color: blue">
                                                                                            <label class="lblIntake2Summary" visitid='<%#:Eval("VisitID")%>' logdate='<%#:Eval("cfLogDate")%>'>
                                                                                                <%#:Eval("TotalIntake2", "{0:N}")%>
                                                                                            </label>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="OUTPUT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                                                    HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div style="text-align: right; color: blue">
                                                                                            <label class="lblOutputSummary" visitid='<%#:Eval("VisitID")%>' logdate='<%#:Eval("cfLogDate")%>'>
                                                                                                <%#:Eval("TotalOutput1", "{0:N}")%></label>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="OUTPUT TIDAK DIUKUR" HeaderStyle-HorizontalAlign="Right"
                                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div style="text-align: right; color: blue">
                                                                                            <label class="lblOutput2Summary" visitid='<%#:Eval("VisitID")%>' logdate='<%#:Eval("cfLogDate")%>'>
                                                                                                <%#:Eval("TotalOutput2", "{0:N}")%></label>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="BALANCE" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                                                    HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <div <%# Eval("cfIsDehydration").ToString() == "False" ? "Style='text-align: right; color: blue'":"Style='text-align: right; color: red'" %>>
                                                                                            <label class="lblBalance" visitid='<%#:Eval("VisitID")%>' logdate='<%#:Eval("cfLogDate")%>'>
                                                                                                <%#:Eval("cfFluidBalance", "{0:N}")%></label>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <%=GetLabel("Belum ada informasi intake-output untuk pasien ini")%>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                        </div>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingIntakeOutput">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <dxcp:ASPxCallbackPanel ID="cbpView6IntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpView6IntakeOutput"
                                                        ShowLoadingPanel="false" OnCallback="cbpView6IntakeOutput_Callback">
                                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView6').show(); }"
                                                            EndCallback="function(s,e){ onCbpView6IntakeOutputEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent10" runat="server">
                                                                <asp:Panel runat="server" ID="Panel8" CssClass="pnlContainerGridPatientPage6">
                                                                    <asp:GridView ID="grdView6IntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                            <asp:BoundField HeaderText="Tanggal" DataField="cfIVTherapyNoteDate" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="LogDate" />
                                                                            <asp:BoundField HeaderText="Jam" DataField="IVTherapyNoteTime" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                            <asp:TemplateField HeaderText="Catatan Terapi Infus" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <div style="height: 50px; overflow-y: auto;">
                                                                                        <%#Eval("IVTherapyNotes").ToString().Replace("\n","<br />")%><br />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField HeaderText="Tenaga Medis" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            <div>
                                                                                <div class="blink">
                                                                                    <%=GetLabel("Belum ada informasi program infus untuk pasien ini") %></div>
                                                                            </div>
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dxcp:ASPxCallbackPanel>
                                                    <div class="imgLoadingGrdView" id="containerImgLoadingView6">
                                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                    </div>
                                                    <div class="containerPaging">
                                                        <div class="wrapperPaging">
                                                            <div id="paging6IntakeOutput">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                                        <ul class="ulTabPage" id="ulTabOrderDetailIntakeOutput">
                                                            <li class="selected" contentid="panIntake">
                                                                <%=GetLabel("INTAKE")%></li>
                                                            <li contentid="panIntake2">
                                                                <%=GetLabel("INTAKE YANG TIDAK DIUKUR")%></li>
                                                            <li contentid="panOutput1">
                                                                <%=GetLabel("OUTPUT")%></li>
                                                            <li contentid="panOutput2">
                                                                <%=GetLabel("OUTPUT YANG TIDAK DIUKUR")%></li>
                                                        </ul>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="containerOrderDt" id="panIntake">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 45%" />
                                                                <col style="width: 55%" />
                                                            </colgroup>
                                                            <tr>
                                                                <td style="vertical-align: top">
                                                                    <dxcp:ASPxCallbackPanel ID="cbpViewDtIntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpViewDtIntakeOutput"
                                                                        ShowLoadingPanel="false" OnCallback="cbpViewDtIntakeOutput_Callback">
                                                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                                            EndCallback="function(s,e){ onCbpViewDtIntakeOutputEndCallback(s); }" />
                                                                        <PanelCollection>
                                                                            <dx:PanelContent ID="PanelContent11" runat="server">
                                                                                <asp:Panel runat="server" ID="Panel9" CssClass="pnlContainerGridPatientPage">
                                                                                    <asp:GridView ID="grdViewDtIntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                            <asp:BoundField DataField="GCFluidType" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn gcFluidType" />
                                                                                            <asp:BoundField DataField="FluidName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fluidName" />
                                                                                            <asp:BoundField HeaderText="Jam" DataField="LogTime" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                <HeaderTemplate>
                                                                                                    <div style="font-weight: bold">
                                                                                                        Nama Cairan</div>
                                                                                                    <div>
                                                                                                        Jenis Cairan</div>
                                                                                                    <div style="font-style: italic">
                                                                                                        Tenaga Medis</div>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <div style="font-weight: bold">
                                                                                                        <%#:Eval("FluidName") %></div>
                                                                                                    <div>
                                                                                                        <%#:Eval("FluidType") %></div>
                                                                                                    <div style="font-style: italic">
                                                                                                        <%#:Eval("ParamedicName") %></div>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField HeaderText="Jumlah Inisiasi" DataField="FluidBalance" HeaderStyle-HorizontalAlign="Right"
                                                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            <div>
                                                                                                <div class="blink">
                                                                                                    <%=GetLabel("Belum ada informasi intake pada tanggal ini") %></div>
                                                                                            </div>
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </dx:PanelContent>
                                                                        </PanelCollection>
                                                                    </dxcp:ASPxCallbackPanel>
                                                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                                    </div>
                                                                    <div class="containerPaging">
                                                                        <div class="wrapperPaging">
                                                                            <div id="pagingDt1IntakeOutput">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <dxcp:ASPxCallbackPanel ID="cbpViewDt4IntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpViewDt4IntakeOutput"
                                                                        ShowLoadingPanel="false" OnCallback="cbpViewDt4IntakeOutput_Callback">
                                                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                                                            EndCallback="function(s,e){ onCbpViewDt4IntakeOutputEndCallback(s); }" />
                                                                        <PanelCollection>
                                                                            <dx:PanelContent ID="PanelContent12" runat="server">
                                                                                <asp:Panel runat="server" ID="Panel10" CssClass="pnlContainerGridPatientPage">
                                                                                    <asp:GridView ID="grdViewDt4IntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                            <asp:BoundField HeaderText="Jam" DataField="LogTime" HeaderStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                                            <asp:BoundField HeaderText="Cairan Ada" DataField="FluidBalance" HeaderStyle-HorizontalAlign="Right"
                                                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                                                                            <asp:BoundField HeaderText="Jumlah" DataField="FluidAmount" HeaderStyle-HorizontalAlign="Right"
                                                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                                                                            <asp:BoundField HeaderText="Tenaga Medis" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            <div>
                                                                                                <div class="blink">
                                                                                                    <%=GetLabel("Belum ada informasi intake pada tanggal ini") %></div>
                                                                                            </div>
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </dx:PanelContent>
                                                                        </PanelCollection>
                                                                    </dxcp:ASPxCallbackPanel>
                                                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt4">
                                                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                                    </div>
                                                                    <div class="containerPaging">
                                                                        <div class="wrapperPaging">
                                                                            <div id="pagingDt4IntakeOutput">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="containerOrderDt" id="panIntake2" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt5IntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpViewDt5IntakeOutput"
                                                            ShowLoadingPanel="false" OnCallback="cbpViewDt5IntakeOutput_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt5IntakeOutputEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent13" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel11" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt5IntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Jam" DataField="LogTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                                                <asp:BoundField HeaderText="Tenaga Medis" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                                                <asp:BoundField HeaderText="Jenis" DataField="FluidType" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                                                <asp:BoundField HeaderText="Cairan" DataField="FluidName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Frekuensi" DataField="Frequency" HeaderStyle-HorizontalAlign="Right"
                                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div class="blink">
                                                                                        <%=GetLabel("Belum ada informasi intake yang tidak diukur pada tanggal ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewDt5">
                                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                        </div>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingDt5IntakeOutput">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="panOutput1" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt2IntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpViewDt2IntakeOutput"
                                                            ShowLoadingPanel="false" OnCallback="cbpViewDt2IntakeOutput_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt2IntakeOutputEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent14" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel12" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt2IntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                <asp:BoundField HeaderText="Jam" DataField="LogTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                                                <asp:BoundField HeaderText="Tenaga Medis" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                                                <asp:BoundField HeaderText="Jenis Cairan" DataField="FluidType" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                                                <asp:BoundField HeaderText="Cairan" DataField="FluidName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Jumlah" DataField="FluidAmount" HeaderStyle-HorizontalAlign="Right"
                                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div class="blink">
                                                                                        <%=GetLabel("Belum ada informasi output pada tanggal ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2">
                                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                        </div>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingDt2IntakeOutput">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="containerOrderDt" id="panOutput2" style="display: none">
                                                        <dxcp:ASPxCallbackPanel ID="cbpViewDt3IntakeOutput" runat="server" Width="100%" ClientInstanceName="cbpViewDt3IntakeOutput"
                                                            ShowLoadingPanel="false" OnCallback="cbpViewDt3IntakeOutput_Callback">
                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                                                EndCallback="function(s,e){ onCbpViewDt3IntakeOutputEndCallback(s); }" />
                                                            <PanelCollection>
                                                                <dx:PanelContent ID="PanelContent15" runat="server">
                                                                    <asp:Panel runat="server" ID="Panel13" CssClass="pnlContainerGridPatientPage">
                                                                        <asp:GridView ID="grdViewDt3IntakeOutput" runat="server" CssClass="grdSelected grdPatientPage"
                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                            <Columns>
                                                                                <asp:BoundField HeaderText="Jam" DataField="LogTime" HeaderStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                                                <asp:BoundField HeaderText="Tenaga Medis" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                                                <asp:BoundField HeaderText="Jenis" DataField="FluidType" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                                                <asp:BoundField HeaderText="Cairan" DataField="FluidName" HeaderStyle-HorizontalAlign="Left"
                                                                                    ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField HeaderText="Frekuensi" DataField="Frequency" HeaderStyle-HorizontalAlign="Right"
                                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <div>
                                                                                    <div class="blink">
                                                                                        <%=GetLabel("Belum ada informasi output yang tidak diukur pada tanggal ini") %></div>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </dx:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3">
                                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                        </div>
                                                        <div class="containerPaging">
                                                            <div class="wrapperPaging">
                                                                <div id="pagingDt3IntakeOutput">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage11" class="w3-border divContent w3-animate-left">
                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td>
                                        <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                            <ul class="ulTabPage" id="ulResultDetail">
                                                <li class="selected" contentid="panByTransactionNo">
                                                    <%=GetLabel("Berdasarkan No. Transaksi")%></li>
                                                <li contentid="panFraction">
                                                    <%=GetLabel("Per Artikel")%></li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="containerOrderDt" id="panByTransactionNo">
                                            <table style="width: 100%">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 70%" />
                                                </colgroup>
                                                <tr>
                                                    <td valign="top">
                                                        <div style="position: relative;">
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <colgroup>
                                                                    <col style="width: 100px" />
                                                                    <col />
                                                                </colgroup>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblNormal">
                                                                            <%=GetLabel("Filter Transaksi")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButtonList ID="rblItemTypeLaboratoryTab1" runat="server" RepeatDirection="Horizontal"
                                                                            RepeatLayout="Table">
                                                                            <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                                                            <asp:ListItem Text=" Kunjungan/Perawatan saat ini " Value="1" />
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" valign="top">
                                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                                        </table>
                                                                        <dxcp:ASPxCallbackPanel ID="cbpViewLaboratory" runat="server" Width="100%" ClientInstanceName="cbpViewLaboratory"
                                                                            ShowLoadingPanel="false" OnCallback="cbpViewLaboratory_Callback">
                                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                                EndCallback="function(s,e){ onCbpViewLaboratoryEndCallback(s); }" />
                                                                            <PanelCollection>
                                                                                <dx:PanelContent ID="PanelContent31" runat="server">
                                                                                    <asp:Panel runat="server" ID="Panel28" CssClass="pnlContainerGridPatientPage">
                                                                                        <asp:GridView ID="grdViewLaboratory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                            OnRowDataBound="grdViewLaboratory_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                                            EmptyDataRowStyle-CssClass="trEmpty">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                    <HeaderTemplate>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Tanggal Transaksi") %>
                                                                                                            -
                                                                                                            <%=GetLabel("Time") %></div>
                                                                                                        <div>
                                                                                                            <%=GetLabel("No. Transaksi") %>
                                                                                                            |
                                                                                                            <%=GetLabel("Status") %></div>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <div>
                                                                                                            <%#: Eval("TransactionDateInString")%>
                                                                                                            |
                                                                                                            <%#: Eval("TransactionTime") %></div>
                                                                                                        <div>
                                                                                                            <%#: Eval("TransactionNo")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <div id="divBtnLaboratoryResult" runat="server" style="text-align: center">
                                                                                                            <input type="button" id="btnViewLaboratoryReport" runat="server" class="btnViewLaboratoryReport"
                                                                                                                value="PRINT" style="height: 20px; width: 75px; background-color: Red; color: White;
                                                                                                                text-align: center" />
                                                                                                        </div>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <EmptyDataTemplate>
                                                                                                <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini")%>
                                                                                            </EmptyDataTemplate>
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </dx:PanelContent>
                                                                            </PanelCollection>
                                                                        </dxcp:ASPxCallbackPanel>
                                                                        <div class="containerPaging">
                                                                            <div class="wrapperPaging">
                                                                                <div id="pagingLaboratory">
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td valign="top">
                                                        <div style="position: relative; padding-top: 25px">
                                                            <dxcp:ASPxCallbackPanel ID="cbpViewDtLaboratory" runat="server" Width="100%" ClientInstanceName="cbpViewDtLaboratory"
                                                                ShowLoadingPanel="false" OnCallback="cbpViewDtLaboratory_Callback">
                                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                    EndCallback="function(s,e){ onCbpViewDtLaboratoryEndCallback(s); }" />
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent32" runat="server">
                                                                        <asp:Panel runat="server" ID="Panel29" CssClass="pnlContainerGridPatientPage">
                                                                            <asp:GridView ID="grdViewDtLaboratory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="TextValue" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="textResultValue hiddenColumn" />
                                                                                    <asp:TemplateField HeaderText="Pemeriksaan" HeaderStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" && Eval("cfIsResultInPDF").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor blink" : "isPanicRangeColor blink") : "" %>'>
                                                                                                <%#: Eval("FractionName1") %></div>
                                                                                            <div style="font-style: italic">
                                                                                                <%#: Eval("ItemName1") %></div>
                                                                                            <input type="hidden" value="<%#:Eval("ChargeTransactionID") %>" bindingfield="ChargeTransactionID" />
                                                                                            <input type="hidden" value="<%#:Eval("cfReferenceRange") %>" bindingfield="cfReferenceRange" />
                                                                                            <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Hasil" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                                                        ItemStyle-Width="150px">
                                                                                        <ItemTemplate>
                                                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" && Eval("cfIsResultInPDF").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'
                                                                                                style="text-align: right">
                                                                                                <div class='<%#: Eval("cfIsResultInPDF").ToString() != "False" ? "lblLink lblViewPDF" : "" %>'>
                                                                                                    <asp:Literal ID="literal" Text='<%# Eval("cfTestResultValue") %>' Mode="PassThrough"
                                                                                                        runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Satuan" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'>
                                                                                                <%#: Eval("MetricUnit") %></div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Nilai Referensi" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <div class='<%#: Eval("cfIsReferenceAsLink").ToString() == "True" ? "lblLink lblReffRange" : "" %>'>
                                                                                                <asp:Literal ID="literal" Text='<%# Eval("cfReferenceRangeCustom") %>' Mode="PassThrough"
                                                                                                    runat="server" /></div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Flag" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'
                                                                                                style="text-align: right">
                                                                                                <%#: Eval("ResultFlag") %></div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Verifikasi" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <div>
                                                                                                <%#: Eval("VerifiedUserName") %></div>
                                                                                            <div>
                                                                                                <%#: Eval("VerifiedDateInString") %></div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left" Visible="true">
                                                                                        <ItemTemplate>
                                                                                            <div class='lblLink lblCommentDetail'>
                                                                                                <%=GetLabel("Comment")%>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <EmptyDataTemplate>
                                                                                    <span class="blink">
                                                                                        <%=GetLabel("Belum ada informasi hasil pemeriksaan untuk transaksi ini") %></span>
                                                                                </EmptyDataTemplate>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dxcp:ASPxCallbackPanel>
                                                            <div class="containerPaging">
                                                                <div class="wrapperPaging">
                                                                    <div id="pagingDtLaboratory">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="containerOrderDt" id="panFraction" style='display: none'>
                                            <table style="width: 100%">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 70%" />
                                                </colgroup>
                                                <tr>
                                                    <td valign="top">
                                                        <div style="position: relative;">
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <colgroup>
                                                                    <col style="width: 100px" />
                                                                    <col />
                                                                </colgroup>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblNormal">
                                                                            <%=GetLabel("Filter Transaksi")%></label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButtonList ID="rblItemTypeLaboratoryTab2" runat="server" RepeatDirection="Horizontal"
                                                                            RepeatLayout="Table">
                                                                            <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                                                            <asp:ListItem Text=" Kunjungan/Perawatan saat ini " Value="1" />
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdLabel">
                                                                        <label class="lblNormal">
                                                                            <%=GetLabel("Periode")%></label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <table width="100%" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox runat="server" Width="120px" ID="txtPeriodFromLaboratory" CssClass="datepicker" />
                                                                                            </td>
                                                                                            <td style="width: 30px; text-align: center">
                                                                                                s/d
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox runat="server" Width="120px" ID="txtPeriodToLaboratory" CssClass="datepicker" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" valign="top">
                                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                                        </table>
                                                                        <dxcp:ASPxCallbackPanel ID="cbpViewTab2Laboratory" runat="server" Width="100%" ClientInstanceName="cbpViewTab2Laboratory"
                                                                            ShowLoadingPanel="false" OnCallback="cbpViewTab2Laboratory_Callback">
                                                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                                EndCallback="function(s,e){ onCbpViewTab2LaboratoryEndCallback(s); }" />
                                                                            <PanelCollection>
                                                                                <dx:PanelContent ID="PanelContent33" runat="server">
                                                                                    <asp:Panel runat="server" ID="Panel30" CssClass="pnlContainerGridPatientPage">
                                                                                        <asp:GridView ID="grdViewTab2Laboratory" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="FractionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                    <HeaderTemplate>
                                                                                                        <div>
                                                                                                            <%=GetLabel("Artikel Pemeriksaan") %>
                                                                                                            <div>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <div>
                                                                                                            <%#: Eval("FractionName1")%></div>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <EmptyDataTemplate>
                                                                                                <%=GetLabel("Tidak ada data artikel pemeriksaan untuk pasien ini")%>
                                                                                            </EmptyDataTemplate>
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </dx:PanelContent>
                                                                            </PanelCollection>
                                                                        </dxcp:ASPxCallbackPanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td valign="top">
                                                        <div style="position: relative; padding-top: 25px">
                                                            <dxcp:ASPxCallbackPanel ID="cbpViewDtTab2Laboratory" runat="server" Width="100%"
                                                                ClientInstanceName="cbpViewDtTab2Laboratory" ShowLoadingPanel="false" OnCallback="cbpViewDtTab2Laboratory_Callback">
                                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                                    EndCallback="function(s,e){ onCbpViewDtTab2LaboratoryEndCallback(s); }" />
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent34" runat="server">
                                                                        <asp:Panel runat="server" ID="Panel31" CssClass="pnlContainerGridPatientPage">
                                                                            <table id="Table1" class="grdSelected grdPatientPage" cellspacing="0" rules="all"
                                                                                style="overflow: auto;" width="100%">
                                                                                <tr>
                                                                                    <th style="width: 20px">
                                                                                        <<
                                                                                    </th>
                                                                                    <asp:Repeater ID="rptFractionDateHeader" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <th style="width: 90px; font-weight: bold; font-size: 14pt" align="center">
                                                                                                <%#: Eval("cfCreatedDate")%>
                                                                                            </th>
                                                                                        </ItemTemplate>
                                                                                    </asp:Repeater>
                                                                                    <th>
                                                                                        &nbsp;
                                                                                    </th>
                                                                                    <th style="width: 20px">
                                                                                        >>
                                                                                    </th>
                                                                                </tr>
                                                                                <asp:ListView ID="lvwViewDtLaboratory" runat="server" OnItemDataBound="lvwViewDtLaboratory_ItemDataBound"
                                                                                    Style="width: 100%">
                                                                                    <LayoutTemplate>
                                                                                        <tr runat="server" id="itemPlaceholder">
                                                                                        </tr>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td style="background-color: Lightgray; text-align: center; font-size: 14pt; font-weight: bold;
                                                                                                vertical-align: middle">
                                                                                            </td>
                                                                                            <asp:Repeater ID="rptFractionDetailValue" runat="server">
                                                                                                <ItemTemplate>
                                                                                                    <td align="left">
                                                                                                        <table class="rptFractionDetailValue1" border="0" cellpadding="0" cellspacing="0"
                                                                                                            width="100%">
                                                                                                            <tr>
                                                                                                                <td align="center">
                                                                                                                    <div class='<%# Eval("IsNormal").ToString() == "False" ? "blink": "" %>' style='<%# Eval("IsNormal").ToString() == "False" ? "color: red;": "" %> font-size: 25px;
                                                                                                                        height: 50px; width: 150px;' />
                                                                                                                    <b>
                                                                                                                        <%#: Eval("Fractionvalue") %>
                                                                                                                        <%#: Eval("MetricUnitName") %>
                                                                                                                    </b></div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <EmptyDataTemplate>
                                                                                        <tr class="trEmpty">
                                                                                            <td colspan="10">
                                                                                                <%=GetLabel("Tidak ada data") %>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:ListView>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dxcp:ASPxCallbackPanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage12" class="w3-border divContent w3-animate-left">
                            <table style="width: 100%">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 100%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewImaging" runat="server" Width="100%" ClientInstanceName="cbpViewImaging"
                                                ShowLoadingPanel="false" OnCallback="cbpViewImaging_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewImagingEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent35" runat="server">
                                                        <asp:Panel runat="server" ID="Panel32" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewImaging" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div>
                                                                                <%=GetLabel("Tanggal Transaksi") %>
                                                                                -
                                                                                <%=GetLabel("Time") %></div>
                                                                            <div>
                                                                                <%=GetLabel("No. Transaksi") %>
                                                                                |
                                                                                <%=GetLabel("Status") %></div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <%#: Eval("TransactionDateInString")%>
                                                                                |
                                                                                <%#: Eval("TransactionTime") %></div>
                                                                            <div>
                                                                                <%#: Eval("TransactionNo")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini")%>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingImaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td valign="top">
                                        <div style="position: relative;">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDtImaging" runat="server" Width="100%" ClientInstanceName="cbpViewDtImaging"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDtImaging_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewDtImagingEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent36" runat="server">
                                                        <asp:Panel runat="server" ID="Panel33" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewDtImaging" runat="server" CssClass="grdSelected grdPatientPage"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                OnRowDataBound="grdViewDtImaging_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div>
                                                                                <%=GetLabel("Nama Pemeriksaan") %></div>
                                                                            <div>
                                                                                <%=GetLabel("Dokter Pelaksana") %></div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <span style="font-weight: bold">
                                                                                    <%#: Eval("ItemName1") %></span></div>
                                                                            <div>
                                                                                <span style="color: blue">
                                                                                    <%#: Eval("ParamedicName")%></span></div>
                                                                            <input type="hidden" class="hdnImagingID" value="<%#: Eval("ImagingID")%>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="DetailReferenceNo" HeaderText="Accession No." HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-CssClass="accessionNo" />
                                                                    <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn"
                                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px"
                                                                        ItemStyle-CssClass="hiddenColumn fileName" />
                                                                    <asp:BoundField DataField="ImageViewerLinkUrl" HeaderText="Viewer Link" HeaderStyle-CssClass="hiddenColumn"
                                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px"
                                                                        ItemStyle-CssClass="hiddenColumn imageViewerLinkUrl" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-Width="80px">
                                                                        <HeaderTemplate>
                                                                            <%=GetLabel("STATUS")%>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <%#: Eval("cfRISBridgingStatus")%></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Report" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <input type="button" id="btnViewReport" runat="server" class="btnViewReport" value="View Report"
                                                                                    style="height: 20px; width: 75px; background-color: Red; color: White;" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="File PDF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <input type="button" id="btnViewPDF" runat="server" class="btnViewPDF" value="View PDF"
                                                                                    style="height: 20px; width: 75px; background-color: Red; color: White;" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PACS Viewer" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <input type="button" id="btnViewer" runat="server" class="btnViewer" value="View Image"
                                                                                    style="height: 20px; width: 75px; background-color: Red; color: White;" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingDtImaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage13" class="w3-border divContent w3-animate-left">
                            <div style="width: 100%; height: 500px; overflow: scroll">
                                <asp:GridView ID="grdViewOtherDiagnostic" runat="server" CssClass="grdSelected grdPatientPage"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                    OnRowDataBound="grdViewOtherDiagnostic_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <h3>
                                                    <%=GetLabel("Other Diagnostic Result")%></h3>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("TransactionDateInString")%>,<%#: Eval("TransactionTime") %>
                                                    <span style="color: blue">
                                                        <%#: Eval("ItemName1")%></span>
                                                </div>
                                                <div>
                                                    <asp:Literal ID="literalOtherDiagnostic" Mode="PassThrough" runat="server" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                        <div id="divPage14" class="w3-border divContent w3-animate-left">
                            <div style="width: 100%; height: 500px; overflow: scroll">
                                <%--                            <h3>
                                <a style="font-size: 12px;">
                                    <%= GetLabel("Daftar Obat")%></a>
                            </h3>
                            <asp:Repeater ID="rptMedication" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div>
                                            <span <%# Eval("IsRFlag").ToString() == "0" ? "Style='display:none'":"Style='color:black;font-weight:bold'" %>>
                                                R/&nbsp;&nbsp</span><strong>
                                                    <%#: Eval("InformationLine1")%></strong>
                                            <img class="imgUDD blink-alert" <%# Eval("IsUsingUDD").ToString() == "True" ?  "" : "style='display:none'" %>
                                                title='<%=GetLabel("UDD Medication")%>' src='<%# ResolveUrl("~/Libs/Images/Icon/uddMedication.png")%>'
                                                height="20px" />
                                        </div>
                                        <div <%# Eval("IsCompound").ToString() == "0" ? "Style='display:none'":"Style='white-space: pre-line;color:black;font-style:italic;margin-left:20px'" %>>
                                            <%#: Eval("MedicationLine")%>
                                        </div>
                                        <div style="color: Blue; width: 35px; float: left; margin-left: 20px">
                                            DOSE</div>
                                        <%#: Eval("NumberOfDosage")%>
                                        <%#: Eval("DosingUnit")%>
                                        -
                                        <%#: Eval("Route")%>
                                        -
                                        <%#: Eval("cfDoseFrequency")%>
                                        -
                                        <%#: Eval("MedicationAdministration")%>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>--%>
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <table border="0" style="width: 100%">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="200px" />
                                                    <col width="150px" />
                                                    <col width="150px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label>
                                                            <%=GetLabel("Jenis Obat")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboDisplayDrug" ClientInstanceName="cboDisplayDrug" runat="server"
                                                            Width="100%">
                                                            <ClientSideEvents ValueChanged="function() { onRefreshList();}" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td class="tdLabel">
                                                        <label>
                                                            <%=GetLabel("Status Obat")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboMedicationStatus" ClientInstanceName="cboMedicationStatus"
                                                            runat="server" Width="100%">
                                                            <ClientSideEvents ValueChanged="function() { onRefreshList();}" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDrug" runat="server" Width="100%" ClientInstanceName="cbpViewDrug"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDrug_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewDrugEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent37" runat="server">
                                                        <asp:Panel runat="server" ID="Panel34" CssClass="pnlContainerGridPatientPage2">
                                                            <asp:ListView ID="lvwViewDrug" runat="server" OnItemDataBound="lvwViewDrug_ItemDataBound">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th rowspan="2" align="center" style="width: 30px">
                                                                                <div>
                                                                                    <%=GetLabel("UDD")%></div>
                                                                            </th>
                                                                            <th rowspan="2" align="center" style="width: 30px">
                                                                            </th>
                                                                            <th rowspan="2" align="left">
                                                                                <div>
                                                                                    <%=GetLabel("Drug Name")%>
                                                                                    -
                                                                                    <%=GetLabel("Form")%></div>
                                                                                <div>
                                                                                    <div style="color: Blue; float: left;">
                                                                                        <%=GetLabel("Generic Name")%></div>
                                                                                </div>
                                                                            </th>
                                                                            <th colspan="6" align="center">
                                                                                <div>
                                                                                    <%=GetLabel("Signa")%></div>
                                                                            </th>
                                                                            <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                                                <div>
                                                                                    <%=GetLabel("Start Date")%></div>
                                                                            </th>
                                                                            <th colspan="4">
                                                                                <div>
                                                                                    <%=GetLabel("Medication Time") %></div>
                                                                            </th>
                                                                            <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                                                <div>
                                                                                    <%=GetLabel("End Date")%></div>
                                                                            </th>
                                                                            <th colspan="3">
                                                                                <div style="text-align: left; font-weight: bold">
                                                                                    <%=GetLabel("QUANTITY") %></div>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 40px;">
                                                                                <div style="text-align: right">
                                                                                    <%=GetLabel("Frequency") %></div>
                                                                            </th>
                                                                            <th style="width: 40px; text-align: left">
                                                                                <div>
                                                                                    <%=GetLabel("Timeline") %></div>
                                                                            </th>
                                                                            <th style="width: 40px;">
                                                                                <div style="text-align: right">
                                                                                    <%=GetLabel("Dose") %></div>
                                                                            </th>
                                                                            <th style="width: 50px;">
                                                                                <div style="text-align: left">
                                                                                    <%=GetLabel("Unit") %></div>
                                                                            </th>
                                                                            <th align="center" style="width: 30px;">
                                                                                <div>
                                                                                    <%=GetLabel("PRN")%></div>
                                                                            </th>
                                                                            <th style="width: 100px;">
                                                                                <div style="text-align: left">
                                                                                    <%=GetLabel("Route") %></div>
                                                                            </th>
                                                                            <th style="width: 40px;">
                                                                                <div>
                                                                                    <%=GetLabel("Morning") %></div>
                                                                            </th>
                                                                            <th style="width: 40px;">
                                                                                <div>
                                                                                    <%=GetLabel("Noon") %></div>
                                                                            </th>
                                                                            <th style="width: 40px;">
                                                                                <div>
                                                                                    <%=GetLabel("Evening") %></div>
                                                                            </th>
                                                                            <th style="width: 40px;">
                                                                                <div>
                                                                                    <%=GetLabel("Night") %></div>
                                                                            </th>
                                                                            <th style="width: 60px;">
                                                                                <div>
                                                                                    <%=GetLabel("Dispensed") %></div>
                                                                            </th>
                                                                            <th style="width: 60px;">
                                                                                <div>
                                                                                    <%=GetLabel("Taken") %></div>
                                                                            </th>
                                                                            <th style="width: 60px;">
                                                                                <div>
                                                                                    <%=GetLabel("Balance") %></div>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                                        rules="all">
                                                                        <tr>
                                                                            <th rowspan="2" align="center" style="width: 30px">
                                                                                <div>
                                                                                    <%=GetLabel("UDD")%></div>
                                                                            </th>
                                                                            <th rowspan="2" align="center" style="width: 30px">
                                                                            </th>
                                                                            <th rowspan="2" align="left">
                                                                                <div>
                                                                                    <%=GetLabel("Drug Name")%>
                                                                                    -
                                                                                    <%=GetLabel("Form")%></div>
                                                                                <div>
                                                                                    <div style="color: Blue; float: left;">
                                                                                        <%=GetLabel("Generic Name")%></div>
                                                                                </div>
                                                                            </th>
                                                                            <th colspan="6" align="center">
                                                                                <div>
                                                                                    <%=GetLabel("Signa")%></div>
                                                                            </th>
                                                                            <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                                                <div>
                                                                                    <%=GetLabel("Start Date")%></div>
                                                                            </th>
                                                                            <th rowspan="2" align="center" style="padding: 3px; width: 110px;">
                                                                                <div>
                                                                                    <%=GetLabel("End Date")%></div>
                                                                            </th>
                                                                            <th colspan="3" align="center">
                                                                                <div style="font-weight: bold">
                                                                                    <%=GetLabel("QUANTITY") %></div>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 40px;">
                                                                                <div style="text-align: right">
                                                                                    <%=GetLabel("Frequency") %></div>
                                                                            </th>
                                                                            <th style="width: 40px; text-align: left">
                                                                                <div>
                                                                                    <%=GetLabel("Timeline") %></div>
                                                                            </th>
                                                                            <th style="width: 40px;">
                                                                                <div style="text-align: right">
                                                                                    <%=GetLabel("Dose") %></div>
                                                                            </th>
                                                                            <th style="width: 50px;">
                                                                                <div style="text-align: left">
                                                                                    <%=GetLabel("Unit") %></div>
                                                                            </th>
                                                                            <th align="center" style="width: 30px;">
                                                                                <div>
                                                                                    <%=GetLabel("PRN")%></div>
                                                                            </th>
                                                                            <th style="width: 100px;">
                                                                                <div style="text-align: left">
                                                                                    <%=GetLabel("Route") %></div>
                                                                            </th>
                                                                            <th style="width: 60px;">
                                                                                <div style="font-weight: bold">
                                                                                    <%=GetLabel("Dispensed") %></div>
                                                                            </th>
                                                                            <th style="width: 60px;">
                                                                                <div style="font-weight: bold">
                                                                                    <%=GetLabel("Taken") %></div>
                                                                            </th>
                                                                            <th style="width: 60px;">
                                                                                <div style="font-weight: bold">
                                                                                    <%=GetLabel("Balance") %></div>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td align="center" style="width: 30px; background: #ecf0f1; vertical-align: middle">
                                                                            <asp:CheckBox ID="chkIsUsingUDD" runat="server" Enabled="false" CssClass="chkIsUsingUDD"
                                                                                Checked='<%# Eval("IsUsingUDD")%>' />
                                                                        </td>
                                                                        <td align="center" style="background: #ecf0f1; vertical-align: middle">
                                                                            <div <%# Eval("IsUsingUDD").ToString() != "True" ? "Style='display:none'":"" %>>
                                                                                <img id="imgStatusImageUri" runat="server" width="24" height="24" alt="" visible="true"
                                                                                    src="" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                                            <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID"
                                                                                class="hdnOrderDetailID" />
                                                                            <input type="hidden" value="<%#:Eval("PastMedicationID") %>" bindingfield="PastMedicationID"
                                                                                class="hdnPastMedicationID" />
                                                                            <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                                            <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                                            <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" class="hdnItemID" />
                                                                            <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" class="hdnDrugName" />
                                                                            <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName"
                                                                                class="hdnParamedicName" />
                                                                            <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                                                            <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                                                            <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                                                            <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                                                            <input type="hidden" value="<%#:Eval("StartDate") %>" bindingfield="StartDate" />
                                                                            <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                                                            <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                                                            <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                                                            <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                                                            <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                                                            <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                                                            <input type="hidden" value="<%#:Eval("cfDispenseQuantity") %>" bindingfield="cfDispenseQuantity"
                                                                                class="hdnCfDispenseQuantity" />
                                                                            <input type="hidden" value="<%#:Eval("cfTakenQuantity") %>" bindingfield="cfTakenQuantity"
                                                                                class="hdnCfTakenQuantity" />
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <b>
                                                                                            <div style="<%# Eval("GCDrugClass").ToString() == "X123^O" ? "color: Red": Eval("GCDrugClass").ToString() == "X123^P" ? "color: Blue" : "color: Black"%>">
                                                                                                <%#: Eval("cfItemNameWithNumero")%></div>
                                                                                        </b>
                                                                                        <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                                            <%#: Eval("cfCompoundDetail")%></div>
                                                                                    </td>
                                                                                    <td rowspan="2">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td rowspan="2">
                                                                                        <div>
                                                                                            <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                                                min-width: 30px; float: left;' /></div>
                                                                                        <div>
                                                                                            <img class="imgIsHAM blink-alert" src='<%# ResolveUrl("~/Libs/Images/Status/ham.png")%>'
                                                                                                alt="" style='<%# Eval("IsHAM").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                                                cursor: pointer; min-width: 30px; float: left;' /></div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div>
                                                                                            <div style="color: Blue; width: 35px; float: left;">
                                                                                                <%=GetLabel("DOSE ")%></div>
                                                                                            <%#: Eval("NumberOfDosage")%>
                                                                                            <%#: Eval("DosingUnit")%>
                                                                                            -
                                                                                            <%#: Eval("Route")%>
                                                                                            -
                                                                                            <%#: Eval("cfDoseFrequency")%>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--                                                                            <tr>
                                                                                <td>
                                                                                    <div style="color: Blue; float: left;">
                                                                                        <%#: Eval("GenericName")%></div>
                                                                                </td>
                                                                            </tr>--%>
                                                                                <%--                                                                            <tr>
                                                                                <td>
                                                                                    <div style="float: left; font-style: italic">
                                                                                        <%#: Eval("ParamedicName")%></div>
                                                                                </td>
                                                                            </tr>--%>
                                                                            </table>
                                                                        </td>
                                                                        <td align="right">
                                                                            <div>
                                                                                <%#: Eval("Frequency")%></div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div>
                                                                                <%#: Eval("DosingFrequency")%></div>
                                                                        </td>
                                                                        <td align="right">
                                                                            <div>
                                                                                <%#: Eval("NumberOfDosage")%></div>
                                                                        </td>
                                                                        <td align="left">
                                                                            <div>
                                                                                <%#: Eval("DosingUnit")%></div>
                                                                        </td>
                                                                        <td align="right">
                                                                            <div style="text-align: center;">
                                                                                <asp:CheckBox ID="chkIsRequired" runat="server" Enabled="false" Checked='<%# Eval("IsAsRequired")%>' />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div>
                                                                                <%#: Eval("Route")%></div>
                                                                        </td>
                                                                        <td align="center">
                                                                            <div>
                                                                                <%#: Eval("cfStartDate")%></div>
                                                                        </td>
                                                                        <td align="center">
                                                                            <div>
                                                                                <%#: Eval("cfEndDate")%></div>
                                                                        </td>
                                                                        <td valign="middle" style="background: #ecf0f1">
                                                                            <div style="text-align: right; color: Black">
                                                                                <label class="lblDispenseQuantity">
                                                                                    <%#:Eval("cfDispenseQuantity", "{0:N}")%></label>
                                                                            </div>
                                                                        </td>
                                                                        <td valign="middle" style="background: #ecf0f1">
                                                                            <div style="text-align: right; color: Black">
                                                                                <label class="lblTakenQuantity">
                                                                                    <%#:Eval("cfTakenQuantity", "{0:N}")%></label>
                                                                            </div>
                                                                        </td>
                                                                        <td valign="middle" style="background: #ecf0f1">
                                                                            <div style="text-align: right; color: Black">
                                                                                <%#: Eval("cfRemainingQuantity")%>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingDrug">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="divLeftPanelContentLoading" style="position: absolute; display: none">
                            <div style="margin: 0 auto">
                                <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <dx:ASPxPopupControl ID="pcNotes" runat="server" ClientInstanceName="pcNotes" Height="150px"
        HeaderText="Signature" CloseAction="None" Width="650px" Modal="True" PopupAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseButtonImage-Width="0">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" ID="pccc1">
                <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent99" runat="server">
                            <div style="text-align: center; width: 100%;">
                                <canvas id="sketchpad" width="400" height="300" style="border: 1px solid #d3d3d3;
                                    background-color: #FFFFFF"> 
                                </canvas>
                            </div>
                            <div style="text-align: center; width: 100%;">
                                <table style="margin-left: auto; margin-right: auto; margin-top: 5px;">
                                    <tr>
                                        <td>
                                            <input type="button" id="btnClose" style="width: 100px" value='<%= GetLabel("Close")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <script type="text/javascript" id="dxss_medicalsummaryctl">
        $(function () {
            registerCollapseExpandHandler();

            //region Imaging Result
            $('#<%=grdViewImaging.ClientID %> tr:eq(1)').click();

            $('.btnViewReport').live('click', function () {
                $row = $(this).closest('tr');
                var entity = rowToObject($row);
                var imagingID = $row.find('.hdnImagingID').val();
                var testOrderID = $('#<%=hdnID.ClientID %>').val();
                var itemID = $(this).closest('tr').find('.keyField').html();
                if (itemID != '' && testOrderID != '') {
                    var reportCode = $('#<%=hdnReportCodeRadResult.ClientID %>').val();
                    var filterExpression = 'ImagingID = ' + imagingID;
                    openReportViewer(reportCode, filterExpression);
                }
            });

            $('.btnViewPDF').live('click', function () {
                var fileName = $(this).closest('tr').find('.fileName').html();
                if (fileName != '') {
                    var url = $('#<%=hdnDocumentPathImaging.ClientID %>').val() + fileName;
                    window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    showToast("ERROR", 'Error Message : ' + "Tidak ada file hasil untuk pemeriksaan ini !");
                }
            });

            $('.btnViewer').live('click', function () {
                $('#<%=hdnReferenceNoImaging.ClientID %>').val($(this).closest('tr').find('.accessionNo').html());
                var postData = $('#<%=hdnReferenceNoImaging.ClientID %>').val();
                if (postData != '' && postData != '&nbsp;') {
                    if ($('#<%=hdnRISVendorImaging.ClientID %>').val() == "X081^04") {
                        var viewerUrl = $('#<%=hdnViewerUrlImaging.ClientID %>').val() + postData + "&id=1&redirect=y";
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                    else if ($('#<%=hdnRISVendorImaging.ClientID %>').val() == "X081^05") {
                        var viewerUrl = $(this).closest('tr').find('.imageViewerLinkUrl').html();
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                    else if ($('#<%=hdnRISVendorImaging.ClientID %>').val() == "X081^06") {
                        var viewerUrl = $('#<%=hdnViewerUrlImaging.ClientID %>').val(); // + postData + "&Username=hisuser&Password=hisuser";

                        var mapForm = document.createElement("form");
                        mapForm.target = "_blank";
                        mapForm.method = "POST"; // or "post" if appropriate
                        mapForm.id = "form2";
                        mapForm.style.display = "none";
                        mapForm.action = viewerUrl;

                        var mapInput2 = document.createElement("input");
                        mapInput2.type = "text";
                        mapInput2.name = "Username";
                        mapInput2.value = "hisuser";
                        mapForm.appendChild(mapInput2);

                        var mapInput3 = document.createElement("input");
                        mapInput3.type = "text";
                        mapInput3.name = "Password";
                        mapInput3.value = "hisuser";
                        mapForm.appendChild(mapInput3);

                        var mapInput1 = document.createElement("input");
                        mapInput1.type = "text";
                        mapInput1.name = "AccessionNumber";
                        mapInput1.value = postData;
                        mapForm.appendChild(mapInput1);

                        document.body.appendChild(mapForm);

                        map = window.open(viewerUrl, '', 'menubar=no,toolbar=no,height=' + (window.screen.availHeight - 30) + ',scrollbars=no,status=no,width=' + (window.screen.availWidth - 10) + ',left=0,top=0,dependent=yes');

                        //                        map = window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                        if (map) {
                            document.getElementById("form2").submit();
                        } else {
                            alert('You must allow popups for this map to work.');
                        }
                    }
                    else {
                        var viewerUrl = $('#<%=hdnViewerUrlImaging.ClientID %>').val() + postData;
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                }
                else if ($('#<%=hdnRISVendorImaging.ClientID %>').val() == "X081^07") {
                    var medicalNo = $('#<%=hdnMedicalNo.ClientID %>').val().replaceAll('-', '');
                    var viewerUrl = $('#<%=hdnViewerUrlImaging.ClientID %>').val() + medicalNo;
                    window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    showToast("ERROR", 'Error Message : ' + "Accession Number untuk membuka file image tidak tersedia !");
                }
            });
            //#endregion

            //region Laboratory Result
            setDatePicker('<%=txtPeriodFromLaboratory.ClientID %>');
            setDatePicker('<%=txtPeriodToLaboratory.ClientID %>');

            $('#<%=grdViewLaboratory.ClientID %> tr:eq(1)').click();

            $('.btnViewLaboratoryReport').live('click', function () {
                $row = $(this).closest('tr');
                var entity = rowToObject($row);
                var chargeTransactionID = $(this).closest('tr').find('.keyField').html();
                var laboratoryID = "";
                Methods.getObject("GetvLaboratoryResultDtList", "ChargeTransactionID = " + chargeTransactionID + "AND IsDeleted = 0", function (resultLab) {
                    if (resultLab != null) {
                        laboratoryID = resultLab.ID;
                    }
                });
                var param = laboratoryID;
                var reportCode = "LB-00032";
                var filterExpression = 'ID = ' + param;

                openReportViewer(reportCode, filterExpression);
            });

            $('.lblViewPDF.lblLink').live('click', function (evt) {
                $tr = $(this).parent().closest('tr');
                var textResultValue = $tr.find('.textResultValue').html();
                window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
            });

            $('#<%=rblItemTypeLaboratoryTab1.ClientID %> input').change(function () {
                cbpViewLaboratory.PerformCallback('refresh');
                cbpViewDtLaboratory.PerformCallback('refresh');
            });

            $('#<%=rblItemTypeLaboratoryTab2.ClientID %> input').change(function () {
                cbpViewTab2Laboratory.PerformCallback('refresh');
            });

            $('#<%:txtPeriodFromLaboratory.ClientID %>').live('change', function () {
                cbpViewTab2Laboratory.PerformCallback('refresh');
            });

            $('#<%:txtPeriodToLaboratory.ClientID %>').live('change', function () {
                cbpViewTab2Laboratory.PerformCallback('refresh');
            });

            //#region Detail Tab
            $('#ulResultDetail li').click(function () {
                $('#ulResultDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion

            $('.lblCommentDetail.lblLink').die('click');
            $('.lblCommentDetail.lblLink').live('click', function () {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                var itemID = entity.ItemID;
                var chargeTransactionID = entity.ChargeTransactionID;
                var chargesTransactionDetailID = 0;

                var filterExpressionCharges = "TransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetPatientChargesDtList', filterExpressionCharges, function (result) {
                    if (result != null) {
                        chargesTransactionDetailID = result.ID;
                    }
                });

                var filterExpressionOrder = "ChargeTransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetvLaboratoryResultDtList', filterExpressionOrder, function (result1) {
                    if (result1 != null) {
                        showToast('Comment', result1.Remarks);
                    }
                    else {
                        var filterExpressionChargesInfo = "ID = '" + chargesTransactionDetailID + "'";
                        Methods.getObject('GetPatientChargesDtInfoList', filterExpressionChargesInfo, function (result2) {
                            if (result2 != null) {
                                showToast('Comment', result2.Remarks);
                            }
                        });
                    }
                });
            });

            $('.lblCommentDetailTab2.lblLink').die('click');
            $('.lblCommentDetailTab2.lblLink').live('click', function () {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                var itemID = entity.ItemID;
                var testOrderID = entity.TestOrderID;
                var chargeTransactionID = entity.ChargeTransactionID;
                var chargesTransactionDetailID = 0;

                var filterExpressionCharges = "TransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetPatientChargesDtList', filterExpressionCharges, function (result) {
                    if (result != null) {
                        chargesTransactionDetailID = result.ID;
                    }
                });

                var filterExpressionOrder = "ChargeTransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetvLaboratoryResultDtList', filterExpressionOrder, function (result1) {
                    if (result1 != null) {
                        showToast('Comment', result1.Remarks);
                    }
                    else {
                        var filterExpressionChargesInfo = "ID = '" + chargesTransactionDetailID + "'";
                        Methods.getObject('GetPatientChargesDtInfoList', filterExpressionChargesInfo, function (result2) {
                            if (result2 != null) {
                                showToast('Comment', result2.Remarks);
                            }
                        });
                    }
                });
            });
            //#endregion

            //region SurgeryHistory
            $('#<%=rblItemTypeSurgeryHistory.ClientID %> input').live('change', function () {
                cbpViewSurgeryHistory.PerformCallback('refresh');
                cbpViewDtSurgeryHistory.PerformCallback('refresh');
                cbpViewDt2SurgeryHistory.PerformCallback('refresh');
                cbpViewDt3SurgeryHistory.PerformCallback('refresh');
                cbpViewDt4SurgeryHistory.PerformCallback('refresh');
                cbpViewDt5SurgeryHistory.PerformCallback('refresh');
                cbpViewDt6SurgeryHistory.PerformCallback('refresh');
                cbpViewDt7SurgeryHistory.PerformCallback('refresh');
                cbpViewDt8SurgeryHistory.PerformCallback('refresh');
            });

            $('#<%=grdViewSurgeryHistory.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewSurgeryHistory.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnSurgeryHistoryID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
                if ($('#<%=hdnSurgeryHistoryID.ClientID %>').val() != "") {
                    cbpViewDtSurgeryHistory.PerformCallback('refresh');
                    cbpViewDt2SurgeryHistory.PerformCallback('refresh');
                    cbpViewDt3SurgeryHistory.PerformCallback('refresh');
                    cbpViewDt4SurgeryHistory.PerformCallback('refresh');
                    cbpViewDt5SurgeryHistory.PerformCallback('refresh');
                    cbpViewDt6SurgeryHistory.PerformCallback('refresh');
                    cbpViewDt7SurgeryHistory.PerformCallback('refresh');
                    cbpViewDt8SurgeryHistory.PerformCallback('refresh');
                }
            });

            $('#<%=grdViewSurgeryHistory.ClientID %> tr:eq(1)').click();
            //#endregion

            //region PainAssessment
            $('#<%=grdcbpPainAssessmentHeader.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdcbpPainAssessmentHeader.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCAssessmentTypePainAssessment.ClientID %>').val($(this).find('.keyField').html());
                cbpPainAssessmentDetail.PerformCallback('refresh');
            });
            $('#<%=grdcbpPainAssessmentHeader.ClientID %> tr:eq(1)').click();
            //#endregion

            //region FallRiskAssessment
            $('#<%=grdFallRiskAssessmentHeader.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFallRiskAssessmentHeader.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCAssessmentType.ClientID %>').val($(this).find('.keyField').html());
                cbpFallRiskAssessmentDetail.PerformCallback('refresh');
            });
            $('#<%=grdFallRiskAssessmentHeader.ClientID %> tr:eq(1)').click();
            //#endregion

            //region intake output
            $('#<%=grdViewIntakeOutput.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewIntakeOutput.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnLogDate.ClientID %>').val($(this).find('.cfLogDate1').html());
                $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
                if ($('#<%=hdnLogDate.ClientID %>').val() != "") {
                    cbpViewDtIntakeOutput.PerformCallback('refresh');
                    cbpViewDt2IntakeOutput.PerformCallback('refresh');
                    cbpViewDt3IntakeOutput.PerformCallback('refresh');
                    cbpViewDt5IntakeOutput.PerformCallback('refresh');
                }
            });

            $('#<%=grdViewDtIntakeOutput.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDtIntakeOutput.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnGCFluidType.ClientID %>').val($(this).find('.gcFluidType').html());
                $('#<%=hdnFluidName.ClientID %>').val($(this).find('.fluidName').html());
                $(this).addClass('selected');

                cbpViewDt4IntakeOutput.PerformCallback('refresh');
            });

            $('#<%=grdViewIntakeOutput.ClientID %> tr:eq(1)').click();

            //#region Detail Tab
            $('#ulTabOrderDetailIntakeOutput li').click(function () {
                $('#ulTabOrderDetailIntakeOutput li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#ulTabOrderDetailSurgeryHistory li').click(function () {
                $('#ulTabOrderDetailSurgeryHistory li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTabSurgeryHistory.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                switch ($('#<%=hdnSelectedTabSurgeryHistory.ClientID %>').val()) {
                    case "preSurgeryAssessment":
                        cbpViewDtSurgeryHistory.PerformCallback('refresh');
                        break;
                    case "preAnesthesyAssessment":
                        cbpViewDt7SurgeryHistory.PerformCallback('refresh');
                        break;
                    case "anesthesyStatus":
                        cbpViewDt8SurgeryHistory.PerformCallback('refresh');
                        break;
                    case "preSurgicalSafetyChecklist":
                        cbpViewDt2SurgeryHistory.PerformCallback('refresh');
                        break;
                    case "periOperative":
                        cbpViewDt6SurgeryHistory.PerformCallback('refresh');
                        break;
                    case "surgeryReport":
                        cbpViewDt4SurgeryHistory.PerformCallback('refresh');
                        break;
                    case "patientMedicalDevice":
                        cbpViewDt3SurgeryHistory.PerformCallback('refresh');
                        break;
                    case "surgeryDocument":
                        cbpViewDt5SurgeryHistory.PerformCallback('refresh');
                        break;
                    default:
                        break;
                }
            });
            //#endregion
            //#endregion

            $('#<%=grdMedicalSummaryContent5.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdMedicalSummaryContent5.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.hiddenColumn').html());
            });
            $('#<%=grdMedicalSummaryContent5.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                cbpMedicalSummaryContent4View.PerformCallback('refresh');
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                cbpMedicalSummaryContent4View.PerformCallback('refresh');
            });

            $('#contentDetail3NavPane a').click(function () {
                $('#contentDetail3NavPane a.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                if (contentID != null) {
                    showDetailContent(contentID);
                }
            });

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            $('#<%=grdDiagnosisParamedicView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisParamedicView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            });
            $('#<%=grdDiagnosisParamedicView.ClientID %> tr:eq(1)').click();

            $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            });
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

            //#region Pemeriksaan Fisik
            $('#<%=divFormContent1.ClientID %>').html($('#<%=hdnPhysicalExamLayout.ClientID %>').val());
            if ($('#<%=hdnPhysicalExamValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnPhysicalExamValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent1.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                }
            }
            //#endregion

            //#region Psikososial Spiritual dan Kultural
            $('#<%=divFormContent2.ClientID %>').html($('#<%=hdnSocialHistoryLayout.ClientID %>').val());
            if ($('#<%=hdnSocialHistoryValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnSocialHistoryValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent2.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                }
            }
            //#endregion

            //#region Psikososial Spiritual dan Kultural
            $('#<%=divFormContent3.ClientID %>').html($('#<%=hdnEducationLayout.ClientID %>').val());
            if ($('#<%=hdnEducationValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnEducationValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent3.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent3.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                }
            }
            //#endregion

            //#region Perencanaan Pasien Pulang
            $('#<%=divFormContent4.ClientID %>').html($('#<%=hdnDischargePlanningLayout.ClientID %>').val());
            if ($('#<%=hdnDischargePlanningValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnDischargePlanningValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent4.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent4.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                }
            }
            //#endregion

            //#region Asessment Tambahan
            $('#<%=divFormContent5.ClientID %>').html($('#<%=hdnAdditionalAssessmentLayout.ClientID %>').val());
            if ($('#<%=hdnAdditionalAssessmentValue.ClientID %>').val() != '') {
                var oldData = $('#<%=hdnAdditionalAssessmentValue.ClientID %>').val().split(';');
                for (var i = 0; i < oldData.length; ++i) {
                    var temp = oldData[i].split('=');
                    $('#<%=divFormContent5.ClientID %>').find('.ddlForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.optForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.chkForm').each(function () {
                        if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.chkNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                            $(this).prop('checked', true);
                        }
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.txtForm').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                    $('#<%=divFormContent5.ClientID %>').find('.txtNursingProblem').each(function () {
                        if ($(this).attr('controlID') == temp[0])
                            $(this).val(temp[1]);
                        $(this).prop('disabled', true);
                    });
                }
            }
            //#endregion

            setPaging($("#diagnosisPaging"), parseInt('<%=gridDiagnosisPageCount %>'), function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });

            setPaging($("#diagnosisParamedicPaging"), parseInt('<%=gridDiagnosisParamedicPageCount %>'), function (page) {
                cbpDiagnosisParamedicView.PerformCallback('changepage|' + page);
            });

            setPaging($("#allergyPaging"), parseInt('<%=gridAllergyPageCount %>'), function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });

            setPaging($("#vitalSignPaging"), parseInt('<%=gridVitalSignPageCount %>'), function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });

            setPaging($("#pagingContent5"), parseInt('<%=gridPatientNursingJournalPageCount %>'), function (page) {
                cbpMedicalSummaryContent5View.PerformCallback('changepage|' + page);
            });

            registerCollapseExpandHandler();

            $('#contentDetail3NavPane a').first().click();

            $('#leftPanel ul li').click(function () {
                $('#leftPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                if (contentID == 'divPage4') {
                    cbpMedicalSummaryContent4View.PerformCallback('refresh');
                }
                else if (contentID == 'divPage9') {
                    cbpViewSurgeryHistory.PerformCallback('refresh');
                }
                else if (contentID == 'divPage11') {
                    cbpViewLaboratory.PerformCallback('refresh');
                }
                else if (contentID == 'divPage12') {
                    cbpViewImaging.PerformCallback('refresh');
                }
                else if (contentID == 'divPage14') {
                    cbpViewDrug.PerformCallback('refresh');
                }
                showContent(contentID);
            });

            function bodyKeyPress(e) {
                var charCode = (e.which) ? e.which : e.keyCode;
                if (charCode == 39) {
                    e.preventDefault();
                }
            }

            if ($.browser.mozilla) {
                $(document).keypress(bodyKeyPress);
            } else {
                $(document).keydown(bodyKeyPress);
            }

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

        function onCbpROSViewEndCallback(s) {
            $('#content2ImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdROSView.ClientID %> tr:eq(1)').click();

                setPaging($("#rosPaging"), pageCount, function (page) {
                    cbpROSView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdROSView.ClientID %> tr:eq(1)').click();
        }

        function calculateMSTScore() {
            var p1 = 0;
            var p2 = 0;
            var p3 = 0;

            if ($('#<%=txtWeightChangedGroupScore.ClientID %>').val())
                p1 = parseInt($('#<%=txtWeightChangedGroupScore.ClientID %>').val());

            if ($('#<%=txtWeightChangedStatusScore.ClientID %>').val())
                p2 = parseInt($('#<%=txtWeightChangedStatusScore.ClientID %>').val());

            if ($('#<%=txtFoodIntakeScore.ClientID %>').val())
                p3 = parseInt($('#<%=txtFoodIntakeScore.ClientID %>').val());

            var total = p1 + p2 + p3;
            $('#<%=txtTotalMST.ClientID %>').val(total);
        }

        function onCbpDiagnosisViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var isMainDiagnosisExists = s.cpRetval;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

                setPaging($("#diagnosisPaging"), pageCount, function (page) {
                    cbpDiagnosisView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
        }

        function onCbpDiagnosisParamedicViewEndCallback(s) {
            var param = s.cpResult.split('|');
            var isMainDiagnosisExists = s.cpRetval;

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdDiagnosisParamedicView.ClientID %> tr:eq(1)').click();

                setPaging($("#diagnosisParamedicPaging"), pageCount, function (page) {
                    cbpDiagnosisParamedicView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdDiagnosisParamedicView.ClientID %> tr:eq(1)').click();
        }

        function onCbpAllergyViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

                setPaging($("#allergyPaging"), pageCount, function (page) {
                    cbpAllergyView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
        }

        function onCbpVitalSignViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

                setPaging($("#vitalSignPaging"), pageCount, function (page) {
                    cbpVitalSignView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
        }

        var pageCountSurgeryHistory = parseInt('<%=PageCountSurgeryHistory %>');
        $(function () {
            setPaging($("#pagingSurgeryHistory"), pageCountSurgeryHistory, function (page) {
                cbpViewSurgeryHistory.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewSurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewSurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistory"), pageCount, function (page) {
                    cbpViewSurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewSurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDtSurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDtSurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt1"), pageCount1, function (page) {
                    cbpViewDtSurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDtSurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt2SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt2SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt2"), pageCount1, function (page) {
                    cbpViewDt2SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt2SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt3SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt3SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt3"), pageCount1, function (page) {
                    cbpViewDt3SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt4SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt4SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt4"), pageCount1, function (page) {
                    cbpViewDt4SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt4SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt5SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt5SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt5"), pageCount1, function (page) {
                    cbpViewDt5SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt6SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt6SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt6"), pageCount1, function (page) {
                    cbpViewDt6SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt6SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt7SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt7SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt7"), pageCount1, function (page) {
                    cbpViewDt7SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt7SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt8SurgeryHistoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt8SurgeryHistory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingSurgeryHistoryDt8"), pageCount1, function (page) {
                    cbpViewDt8SurgeryHistory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt8SurgeryHistory.ClientID %> tr:eq(1)').click();
        }

        $('.lblParamedicName').live('click', function () {
            $tr = $(this).closest('tr');
            $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
            var ppa = $tr.find('.paramedicName').html();
            var noteDate = $tr.find('.cfNoteDate').html();
            var noteTime = $tr.find('.noteTime').html();
            var signature1 = $tr.find('.signature1').html();
            var signature2 = $tr.find('.signature2').html();
            var signature3 = $tr.find('.signature3').html();
            var signature4 = $tr.find('.signature4').html();
            var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;

            var canvas = document.getElementById('sketchpad');
            var ctx = canvas.getContext('2d');

            var img = new Image();
            img.src = signature1;
            img.onload = function () {
                ctx.drawImage(img, 0, 0, 400, 300);
            }

            pcNotes.Show();
        });

        $("#btnClose").click(cancelPcNotes);
        function cancelPcNotes() {
            pcNotes.Hide();
        }

        $('#btnBodyDiagramContainerPrev').live('click', function () {
            if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                cbpBodyDiagramView.PerformCallback('prev');
        });
        $('#btnBodyDiagramContainerNext').live('click', function () {
            if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                cbpBodyDiagramView.PerformCallback('next');
        });

        //#region Body Diagram
        function onCbpBodyDiagramViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'count') {
                if (param[1] != '0') {
                    $('#<%=divBodyDiagram.ClientID %>').show();
                    $('#<%=tblEmpty.ClientID %>').hide();
                }
                else {
                    $('#<%=divBodyDiagram.ClientID %>').hide();
                    $('#<%=tblEmpty.ClientID %>').show();
                }

                $('#<%=hdnPageCount.ClientID %>').val(param[1]);
                $('#<%=hdnPageIndex.ClientID %>').val('0');
            }
            else if (param[0] == 'index')
                $('#<%=hdnPageIndex.ClientID %>').val(param[1]);
            hideLoadingPanel();
        }

        function onRefreshBodyDiagram(filterExpression) {
            if (filterExpression == 'edit')
                cbpBodyDiagramView.PerformCallback('edit');
            else
                cbpBodyDiagramView.PerformCallback('refresh');
        }
        //endregion

        $('#btnBodyDiagramContainerPrev2').live('click', function () {
            if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                cbpBodyDiagramView2.PerformCallback('prev');
        });
        $('#btnBodyDiagramContainerNext2').live('click', function () {
            if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                cbpBodyDiagramView2.PerformCallback('next');
        });

        //#region Body Diagram 2
        function onCbpBodyDiagramView2EndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'count') {
                if (param[1] != '0') {
                    $('#<%=divBodyDiagram2.ClientID %>').show();
                    $('#<%=tblEmpty2.ClientID %>').hide();
                }
                else {
                    $('#<%=divBodyDiagram2.ClientID %>').hide();
                    $('#<%=tblEmpty2.ClientID %>').show();
                }

                $('#<%=hdnPageCount.ClientID %>').val(param[1]);
                $('#<%=hdnPageIndex.ClientID %>').val('0');
            }
            else if (param[0] == 'index')
                $('#<%=hdnPageIndex.ClientID %>').val(param[1]);
            hideLoadingPanel();
        }

        function onRefreshBodyDiagram(filterExpression) {
            if (filterExpression == 'edit')
                cbpBodyDiagramView2.PerformCallback('edit');
            else
                cbpBodyDiagramView2.PerformCallback('refresh');
        }
        //endregion

        function showDetailContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("content3Detail");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }

        $('.btnContentRefresh').die('click');
        $('.btnContentRefresh').live('click', function () {
            cbpMedicalSummaryContent4View.PerformCallback('refresh');
        });


        //#region Paging
        var pageCount = parseInt('<%=Content4PageCount %>');
        $(function () {
            setPaging($("#pagingContent4"), pageCount, function (page) {
                cbpMedicalSummaryContent4View.PerformCallback('changepage|' + page);
            });
        });

        function oncbpMedicalSummaryContent4ViewEndCallback(s) {
            $('#containerImgLoadingContent4View').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdMedicalSummaryContent4.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingContent4"), pageCount, function (page) {
                    cbpMedicalSummaryContent4View.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdMedicalSummaryContent4.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onCbpMedicalSummaryContent5ViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdMedicalSummaryContent5.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingContent5"), pageCount, function (page) {
                    cbpMedicalSummaryContent5View.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdMedicalSummaryContent5.ClientID %> tr:eq(1)').click();
        }

        $('.btnPrintSurgeryReportList.imglink').die('click');
        $('.btnPrintSurgeryReportList.imglink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var patientSurgeryID = $row.find('.hdnSugeryID').val();
            var visitID = $('#<%=hdnVisitIDPopUpCtl.ClientID %>').val();
            var reportCode = $('#<%=hdnRptCodeSurgery.ClientID %>').val();
            var filterExpression = visitID + '|' + patientSurgeryID;
            openReportViewer(reportCode, filterExpression);
        });

        $('.imgPatientReferralPrint.imglink').die('click');
        $('.imgPatientReferralPrint.imglink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var reportCode = $('#<%=hdnRptCodePatientRefferal.ClientID %>').val();
            var visitID = $('#<%=hdnVisitIDPopUpCtl.ClientID %>').val();
            var id = $row.find('.hdnReferralID').val();
            var filterExpression = 'VisitID = ' + visitID + 'AND ID = ' + id;
            openReportViewer(reportCode, filterExpression);
        });

        $('.imgPatientReferralAnswerPrint.imglink').die('click');
        $('.imgPatientReferralAnswerPrint.imglink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var reportCode = $('#<%=hdnRptCodePatientRefferalAnswer.ClientID %>').val();
            var visitID = $('#<%=hdnVisitIDPopUpCtl.ClientID %>').val();
            var id = $row.find('.hdnReferralID').val();
            var filterExpression = 'VisitID = ' + visitID + 'AND ID = ' + id;
            openReportViewer(reportCode, filterExpression);
        });

        //#region Summary : Action Button
        $('.lblIntakeSummary.lblLink').die('click');
        $('.lblIntakeSummary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^01';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "INTAKE : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblIntake2Summary.lblLink').die('click');
        $('.lblIntake2Summary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^04';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "INTAKE YANG TIDAK DIUKUR : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblOutputSummary.lblLink').die('click');
        $('.lblOutputSummary.lblLink').live('click', function () {
            var group = 'X459^02';
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "OUTPUT : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblOutput2Summary.lblLink').die('click');
        $('.lblOutput2Summary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^03';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "OUTPUT YANG TIDAK DIUKUR : " + $(this).attr('logDate'), 800, 500);
        });
        //#endregion

        function onRefreshControl() {
            cbpViewIntakeOutput.PerformCallback('refresh');
            cbpView6IntakeOutput.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCountIntakeOutput %>');
        $(function () {
            setPaging($("#pagingIntakeOutput"), pageCount, function (page) {
                cbpViewIntakeOutput.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewIntakeOutputEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewIntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingIntakeOutput"), pageCount, function (page) {
                    cbpViewIntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewIntakeOutput.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtIntakeOutputEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDtIntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1IntakeOutput"), pageCount1, function (page) {
                    cbpViewDtIntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDtIntakeOutput.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt2IntakeOutputEndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount2 = parseInt(param[1]);

                if (pageCount2 > 0)
                    $('#<%=grdViewDt2IntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2IntakeOutput"), pageCount2, function (page) {
                    cbpViewDt2IntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt2IntakeOutput.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt3IntakeOutputEndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount3 = parseInt(param[1]);

                if (pageCount3 > 0)
                    $('#<%=grdViewDt3IntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3IntakeOutput"), pageCount3, function (page) {
                    cbpViewDt3IntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3IntakeOutput.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt4IntakeOutputEndCallback(s) {
            $('#containerImgLoadingViewDt4').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount4 = parseInt(param[1]);

                if (pageCount4 > 0)
                    $('#<%=grdViewDt4IntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt4IntakeOutput"), pageCount4, function (page) {
                    cbpViewDt4IntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt4IntakeOutput.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt5IntakeOutputEndCallback(s) {
            $('#containerImgLoadingViewDt5').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount5 = parseInt(param[1]);

                if (pageCount5 > 0)
                    $('#<%=grdViewDt5IntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt5IntakeOutput"), pageCount5, function (page) {
                    cbpViewDt5IntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5IntakeOutput.ClientID %> tr:eq(1)').click();
        }

        function onCbpView6IntakeOutputEndCallback(s) {
            $('#containerImgLoadingView6').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount6 = parseInt(param[1]);

                if (pageCount6 > 0)
                    $('#<%=grdView6IntakeOutput.ClientID %> tr:eq(1)').click();

                setPaging($("#paging6IntakeOutput"), pageCount6, function (page) {
                    cbpView6IntakeOutput.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView6IntakeOutput.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('#<%=grdViewPatientTransferList.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='7'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=grdPatientTransferDetail.ClientID %> tr:gt(0)').remove();
                $('#<%=hdnCollapseID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewPatientTransferDetail.PerformCallback();
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $trDetail.remove();
            }
        });

        //#region Paging
        var pageCountPatientTransferList = parseInt('<%=PageCountPatientTransferList %>');
        $(function () {
            setPaging($("#pagingPatientTransferList"), pageCountPatientTransferList, function (page) {
                cbpViewPatientTransferList.PerformCallback('changepage|' + page);
            });
        });

        function oncbpViewPatientTransferListEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewPatientTransferList.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingPatientTransferList"), pageCount, function (page) {
                    cbpViewPatientTransferList.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewPatientTransferList.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCountPatientReferralList %>');
        $(function () {
            setPaging($("#pagingPatientReferralList"), pageCount, function (page) {
                cbpViewPatientReferralList.PerformCallback('changepage|' + page);
            });
        });

        function oncbpViewPatientReferralListEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewPatientReferralList.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingPatientReferralList"), pageCount, function (page) {
                    cbpViewPatientReferralList.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewPatientReferralList.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCountFallRiskAssessment %>');
        $(function () {
            setPaging($("#pagingFallRiskAssessment"), pageCount, function (page) {
                cbpFormListFallRiskAssessmentHeader.PerformCallback('changepage|' + page);
            });
        });

        function onCbpFormListFallRiskAssessmentHeaderEndCallback(s) {
            $('#containerHdImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdFallRiskAssessmentHeader.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingFallRiskAssessment"), pageCount, function (page) {
                    cbpFormListFallRiskAssessmentHeader.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdFallRiskAssessmentHeader.ClientID %> tr:eq(1)').click();
        }

        function oncbpFallRiskAssessmentDetailEndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdFallRiskAssessmentDetail.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingFallRiskAssessmentDetail"), pageCount, function (page) {
                    cbpFallRiskAssessmentDetail.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdFallRiskAssessmentDetail.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Header
        var pageCount = parseInt('<%=PageCountPainAssessment %>');
        $(function () {
            setPaging($("#pagingPainAssessmentHeader"), pageCount, function (page) {
                cbpPainAssessmentHeader.PerformCallback('changepage|' + page);
            });
        });

        function onCbpPainAssessmentHeaderEndCallback(s) {
            $('#containerHdImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdcbpPainAssessmentHeader.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingPainAssessmentHeader"), pageCount, function (page) {
                    cbpPainAssessmentHeader.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdcbpPainAssessmentHeader.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Detail
        function onCbpPainAssessmentDetailEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdcbpPainAssessmentDetail.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingPainAssessmentDetail"), pageCount, function (page) {
                    cbpPainAssessmentDetail.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdcbpPainAssessmentDetail.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Laboratory
        $('#<%=grdViewLaboratory.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewLaboratory.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnLaboratoryID1.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDtLaboratory.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewTab2Laboratory.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewTab2Laboratory.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnLaboratoryID2.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDtTab2Laboratory.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDtLaboratory.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDtLaboratory.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        var pageCount = parseInt('<%=PageCountLaboratory %>');
        $(function () {
            setPaging($("#pagingLaboratory"), pageCount, function (page) {
                cbpViewLaboratory.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewLaboratoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewLaboratory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingLaboratory"), pageCount, function (page) {
                    cbpViewLaboratory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewLaboratory.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging 2
        function onCbpViewTab2LaboratoryEndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            if (param[1] != '0') {
                $('#<%=grdViewTab2Laboratory.ClientID %> tr:eq(1)').click();
            }
            else {
                $('#<%=hdnLaboratoryID2.ClientID %>').val('0');
                cbpViewTab2Laboratory.PerformCallback('refresh');
            }
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtLaboratoryEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDtLaboratory.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDtLaboratory"), pageCount, function (page) {
                    grdViewDtLaboratory.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDtLaboratory.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt 2
        function onCbpViewDtTab2LaboratoryEndCallback(s) {
            $('#containerImgLoadingView').hide();
        }
        //#endregion        

        $('.lblReffRange.lblLink').die('click');
        $('.lblReffRange.lblLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            showToast('Nilai Referensi', entity.cfReferenceRange);
        });

        $('.lblReffRangeTab2.lblLink').die('click');
        $('.lblReffRangeTab2.lblLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            showToast('Nilai Referensi', entity.cfReferenceRange);
        });

        $('#lblResultInPDF').die('click');
        $('#lblResultInPDF').live('click', function (evt) {
            var result = $(this).closest($('div[class=result]'));
            var id = $(result).find($('[id*=hdnResultInfPDF]')).val();
            if (id != '')
                window.open("data:application/pdf;base64," + id, "popupWindow", "width=600, height=600,scrollbars=yes");
        });

        //region Imaging
        $('#<%=grdViewImaging.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewImaging.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnImagingID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDtImaging.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDtImaging.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDtImaging.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemIDImaging.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnReferenceNoImaging.ClientID %>').val($(this).find('.accessionNo').html());
            }
        });

        //#region Paging
        var pageCountImaging = parseInt('<%=PageCountImaging %>');
        $(function () {
            setPaging($("#pagingImaging"), pageCountImaging, function (page) {
                cbpViewImaging.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewImagingEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewImaging.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingImaging"), pageCount, function (page) {
                    cbpViewImaging.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewImaging.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtImagingEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDtImaging.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDtImaging"), pageCount, function (page) {
                    cbpViewDtImaging.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDtImaging.ClientID %> tr:eq(1)').click();
        }
        //#endregion
        //#endregion

        //#region Paging
        var pageCountDrug = parseInt('<%=PageCountDrug %>');
        $(function () {
            setPaging($("#pagingDrug"), pageCountDrug, function (page) {
                cbpViewDrug.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewDrugEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#pagingDrug"), pageCount, function (page) {
                    cbpViewDrug.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        //#region e-Document
        $('.lnkViewDocument a').die('click');
        $('.lnkViewDocument a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
            window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
        });
        //#endregion

        function onRefreshList() {
            cbpViewDrug.PerformCallback('refresh');
        }
    </script>
</asp:Content>
