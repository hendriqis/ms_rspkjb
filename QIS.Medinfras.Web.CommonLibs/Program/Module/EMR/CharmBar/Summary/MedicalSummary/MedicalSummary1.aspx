<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="MedicalSummary1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalSummary1" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" id="dxss_SymptomInfoCtl">
        $(function () {
            $('#leftPanel ul li').click(function () {
                $('#leftPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                var contentUrl = $(this).attr('contentUrl');

                if (contentID != null) {
                    showContent(contentID, contentUrl);
                }
            });

            $('#leftPanel ul li').first().click();
        });

        function showContent(contentID, contentUrl) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            $('#divLeftPanelContentLoading').show();

            Methods.getHtmlControl(ResolveUrl(contentUrl), visitID, function (result) {
                $('#divLeftPanelContentLoading').hide();
                $('#containerLeftPanelContent').html(result.replace(/\VIEWSTATE/g, ''));
            }, function () {
                $('#divLeftPanelContentLoading').hide();
            });
        }
    </script>
    <style type="text/css">
        #leftPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
        #leftPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
        #leftPanel > ul > li    { list-style-type: none; font-size: 13px; display:list-item; border: 1px solid #fdf5e6!important; padding: 5px 8px; cursor: pointer; background-color:#87CEEB!important; }
        #leftPanel > ul > li.selected { background-color: #ff5722!important; color: White; }   
        .divContent { padding-left: 3px; min-height:490px;} 
    </style>

    <input type="hidden" value="2" runat="server" id="hdnVisitID" />
    <div style="width:100%">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
            <colgroup>
                <col style="width:180px" />
                <col />
            </colgroup>  
  
            <tr>
                <td style="vertical-align:top">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div id="leftPanel" class="w3-border">
                                    <ul>
                                        <li contentID="divPage1" title="Data Pasien dan Kunjungan" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent1Ctl.ascx" class="w3-hover-red">Data Pasien dan Kunjungan</li>
                                        <li contentID="divPage2" title="Kajian Awal Medis" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent2Ctl.ascx" class="w3-hover-red">Kajian Awal Medis</li>
                                        <li contentID="divPage3" title="Kajian Awal Perawat" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent3Ctl.ascx" class="w3-hover-red">Kajian Awal Perawat</li>
                                        <li contentID="divPage4" title="Catatan Terintegrasi" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent4Ctl.ascx" class="w3-hover-red">Catatan Terintegrasi</li>
                                        <li contentID="divPage5" title="Catatan Perawat" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent5Ctl.ascx" class="w3-hover-red">Catatan Perawat</li>
                                        <li contentID="divPage6" title="Asuhan Keperawatan" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent6Ctl.ascx" class="w3-hover-red">Asuhan Keperawatan</li>
                                        <li contentID="divPage7" title="Konsultasi/Rawat Bersama" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent7Ctl.ascx" class="w3-hover-red">Konsultasi/Rawat Bersama</li>
                                        <li contentID="divPage8" title="Laporan Operasi" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent8Ctl.ascx" class="w3-hover-red">Laporan Operasi</li>
                                        <li contentID="divPage9" title="Form Pengkajian" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent9Ctl.ascx" class="w3-hover-red">Form Pengkajian</li>
                                        <li contentID="divPage10" title="e-Document" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent10Ctl.ascx" class="w3-hover-red">e-Document</li>
                                        <li contentID="divPage11" title="Hasil Laboratorium" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent11Ctl.ascx" class="w3-hover-red">Hasil Laboratorium</li>
                                        <li contentID="divPage12" title="Hasil Laboratorium" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent12Ctl.ascx" class="w3-hover-red">Hasil Radiologi</li>
                                        <li contentID="divPage13" title="Hasil Penunjang Lain-lain" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent13Ctl.ascx" class="w3-hover-red">Hasil Penunjang Lain-lain</li>
                                        <li contentID="divPage14" title="Daftar Obat" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent14Ctl.ascx" class="w3-hover-red">Daftar Obat</li>
                                        <li contentID="divPage15" title="Resume Medis" contentUrl="~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryContent/MedicalSummaryContent15Ctl.ascx" class="w3-hover-red">Resume Medis</li>
                                    </ul>     
                                </div> 
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top; padding-left: 5px;" rowspan="2">
                    <div id="containerLeftPanelContent">
                    </div>
                    <div id="divLeftPanelContentLoading" style="position:absolute;display:none">
                        <div style="margin:0 auto">
                            <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                        </div>
                    </div>   
                </td>
            </tr>
        </table>
    </div>    
</asp:Content>

