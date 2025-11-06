<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="EpisodeSummary2.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EpisodeSummary2" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" id="dxss_SymptomInfoCtl">
        $(function () {
            $('#leftPanel ul li').click(function () {
                $('#leftPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                showContent(contentID);
            });
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
        #leftPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
        #leftPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
        #leftPanel > ul > li    { list-style-type: none; font-size: 15px; display:list-item; border: 1px solid #fdf5e6!important; padding: 5px 8px; cursor: pointer; background-color:#87CEEB!important; }
        #leftPanel > ul > li.selected { background-color: #ff5722!important; color: White; }   
        .divContent { padding-left: 3px; min-height:490px;} 
    </style>
<div style="width:100%;">
    <input type="hidden" runat="server" id="hdnMRN" value="0" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' />
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
    <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
        <colgroup>
            <col style="width:300px" />
            <col />
        </colgroup>
        <tr>
            <td>
               <div id="lblMedicalNo" runat="server" class="w3-lime w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444; width:100%"></div>
            </td>
            <td>
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("RINGKASAN PERAWATAN PASIEN")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" title="Resume Medis" class="w3-hover-red">Resume Medis</li>
                        <li contentID="divPage3" title="Keluhan & Riwayat Kesehatan" class="w3-hover-red">Keluhan & Riwayat Kesehatan</li>
                        <li contentID="divPage4" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>
                        <li contentID="divPage5" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>
                        <li contentID="divPage6" title="Body Diagram" class="w3-hover-red">Body Diagram</li>
                        <li contentID="divPage7" title="Psikososial Spiritual dan Kultural" class="w3-hover-red">Psikososial Spiritual dan Kultural</li>
                        <li contentID="divPage8" title="Kebutuhan Informasi dan Edukasi" class="w3-hover-red">Kebutuhan Informasi dan Edukasi</li>
                        <li contentID="divPage9" title="Perencanaan Pemulangan Pasien" class="w3-hover-red">Perencanaan Pemulangan Pasien</li>
                        <li contentID="divPage10" title="Asesmen Tambahan (Populasi Khusus)" class="w3-hover-red">Asesmen Tambahan (Populasi Khusus)</li>
                    </ul>     
                </div> 
                <div>
                    <table class="w3-table-all" style="width:100%">
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class=" w3-small"><%=GetLabel("Dikaji Oleh :")%></div></td>
                        </tr>        
                        <tr>
                            <td style="text-align:left"><div id="lblAssessmentParamedicName" runat="server" class="w3-medium"></div></td>
                        </tr>
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class="w3-small"><%=GetLabel("Diverifikasi Oleh : PPJA")%></div></td>
                        </tr>
                        <tr>
                            <td style="text-align:left"><div id="lblPrimaryNurseName" runat="server" class="w3-medium"></div></td>
                        </tr>   
                        <tr>
                            <td style="text-align:left" class="w3-blue-grey"><div class="w3-small"><%=GetLabel("Diverifikasi Oleh : DPJP")%></div></td>
                        </tr>
                        <tr>
                            <td style="text-align:left"><div id="lblPhysicianName2" runat="server" class="w3-medium"></div></td>
                        </tr>                                                         
                    </table>
                </div>
            </td>
            <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display:none"> 
                    <table style="margin-top:5px; width:100%" cellpadding="0" cellspacing="0">
                        <colgroup style="width:130px"/>
                            <colgroup style="width:10px; text-align: center"/>
                            <colgroup />
                        <colgroup style="width:130px"/>
                        <tr>
                            <td style="vertical-align:top">
                                <img style="width:110px;height:125px" runat="server" runat="server" id="imgPatientImage" />
                            </td>
                            <td />
                            <td>
                                <table border = "0" cellpadding="0" cellspacing="0">
                                    <colgroup style="width:160px"/>
                                    <colgroup style="width:10px; text-align: center"/>
                                    <colgroup />   
                                    <tr>
                                        <td colspan="3" style="width:100%"><span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold; width:100%"></span></td>
                                    </tr> 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Jenis Kelamin")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblGender" runat="server" style="color:Black"></span></td>
                                    </tr>                                 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Tanggal Lahir (Umur)")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblDateOfBirth" runat="server" style="color:Black"></span></td>
                                    </tr>       
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Tanggal & Jam Registrasi")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><div id="lblRegistrationDateTime" runat="server" style="color:Black"></div></td>
                                    </tr>      
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("No. Registrasi")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><div id="lblRegistrationNo" runat="server" style="color:Black"></div></td>
                                    </tr> 
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("DPJP Utama")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPhysician" runat="server" style="color:Black"></span></td>
                                    </tr>                  
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Pembayar")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPayerInformation" runat="server" style="color:Black"></span></td>
                                    </tr>     
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Lokasi Pasien")%></td>
                                        <td class="tdLabel"><%=GetLabel(":")%></td>
                                        <td><span id="lblPatientLocation" runat="server" style="color:Black"></span></td>
                                    </tr>          
                                    <tr>
                                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel("Diagnosa")%></td>
                                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel(":")%></td>
                                        <td>
                                            <textarea id="lblDiagnosis" runat="server" style="border:0; width:100%; height:120px; background-color: transparent" readonly></textarea>
                                        </td>
                                    </tr>                                                                                                                                                                                                                                                          
                                </table>
                            </td>
                            <td style="vertical-align:top">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="divContent w3-animate-left" style="display:none">
                </div>
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <colgroup>
                            <col width="50%" />
                            <col width="50%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
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
                                                    <asp:TextBox ID="txtChiefComplaint" Width="100%" runat="server" TextMode="MultiLine"
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
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
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
                            </td>
                            <td style="vertical-align:top">
                                <h4 class="w3-blue h4expanded">
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
                                    <%=GetLabel("Diagnosa Pasien")%></h4>
                                <div class="containerTblEntryContent containerEntryPanel1">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                        <tr>
                                            <td>
                                                <div style="position: relative;">
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
                                    <%=GetLabel("Riwayat Alergi")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:450px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                                                        Checked="false" Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
                    <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
                    <div id="divFormContent1" runat="server" style="height: 490px;width:100%;overflow-y: scroll;overflow-x: auto;"></div>
                </div>     
                <div id="divPage5" class="w3-border divContent w3-animate-left" style="display:none">
                    <div>
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="vitalSignPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display:none">
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
                <div id="divPage7" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnSocialHistoryLayout" value="" />
                    <input type="hidden" runat="server" id="hdnSocialHistoryValue" value="" />
                    <div id="divFormContent2" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>     
                <div id="divPage8" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
                    <input type="hidden" runat="server" id="hdnEducationValue" value="" />
                    <div id="divFormContent3" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>    
                <div id="divPage9" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
                    <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
                    <div id="divFormContent4" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="divPage10" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnAdditionalAssessmentLayout" value="" />
                    <input type="hidden" runat="server" id="hdnAdditionalAssessmentValue" value="" />
                    <div id="divFormContent5" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
            </td>
        </tr>
    </table>
</div>     
</asp:Content>

