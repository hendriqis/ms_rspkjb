<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryCtl" %>

 <!--
<script id="dxis_episodesummaryctl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script id="dxis_episodesummaryctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script id="dxis_episodesummaryctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>
    -->
<script type="text/javascript" id="dxss_episodesummaryctl">
    $(function () {
        setTimeout(function () {
            $('#mybook').booklet({
                keyboard: true,
                menu: '#custom-menu',
                chapterSelector: true,
                pageSelector: true,
                width: 1200,
                height: 550,
                closed: true,
                covers: true,
                autoCenter: true
            });
            $('#mybook').show();
        }, 0);

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

        var visitID = parseInt('<%=VisitID %>');

        $('#tdEpisodeSummaryBookCover').height($('#tdEpisodeSummaryBookCover').parent().height());

        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryEpisodeInformationCtl.ascx", visitID, 'page1');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryEpisodeInformationCtl2.ascx", visitID, 'page2');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryReviewOfSystemCtl.ascx", visitID, 'page3');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryVitalSignIndicatorCtl.ascx", visitID, 'page4');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryPatientInstructionCtl.ascx", visitID, 'page5');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryDiagnoseCtl.ascx", visitID, 'page6');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryIntegratedNotesCtl.ascx", visitID, 'page7');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryPatientReferralCtl.ascx", visitID, 'page8');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryLaboratoryCtl.ascx", visitID, 'page9');
        getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryImagingCtl.ascx", visitID, 'page10');
        //            getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryBodyDiagramCtl.ascx", visitID, 'page4');
        //            getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryPrescriptionOrderCtl.ascx", visitID, 'page9');
        //            getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryFollowUpVisitCtl.ascx", visitID, 'page10');

        if ($("#<%=hdnIDBodyDiagram.ClientID %>").val() != '') {
            var lstID = $("#<%=hdnIDBodyDiagram.ClientID %>").val().split('|');
            for (var i = 0; i < lstID.length; i++) {
                if (i == 0) {
                    $section = $('<div title="Body Diagram" rel="Body Diagram" id="episodeBodyDiagram"></div>');
                    $section.insertAfter($('#sectionDischargeInformation'));

                    $page = $('<div><div class="contentEpisodeSummary" id="bodydiagram' + i + '"></div></div>');
                    $section.append($page);
                }
                else {
                    $page = $('<div><div class="contentEpisodeSummary" id="bodydiagram' + i + '"></div></div>');
                    $page.insertAfter($('#episodeBodyDiagram'));
                }

                getDataEpisodeSummaryCtl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryContent/EpisodeSummaryBodyDiagramCtl.ascx", lstID[i], 'bodydiagram' + i);
            }
        }
    });

    function getDataEpisodeSummaryCtl(controlLocation, queryString, pageID) {
        Methods.getHtmlControl(controlLocation, queryString, function (result) {
            $('#' + pageID).html(result.replace(/\VIEWSTATE/g, ''));
        }, function (result) {
            $('#' + pageID).html(result.replace(/\VIEWSTATE/g, ''));
        });
    }

</script>
<style type="text/css">
    .booklet         {width:1200px; height:750px; position:relative; margin:0 auto 10px; overflow:visible !important;}
	.booklet .b-page {left:0; top:0; position:absolute; overflow:hidden; padding:0;}
	   
   	/* Page Wrappers */
	.booklet .b-wrap       {top:0; position:absolute;}
	.booklet .b-wrap-left  {background:#eeeeee;}
	.booklet .b-wrap-right {background:#eaeaea;}
	
	.booklet .b-pN .b-wrap,
	.booklet .b-p1 .b-wrap,
	.booklet .b-p2 .b-wrap,
	.booklet .b-p3 .b-wrap,
	.booklet .b-p4 .b-wrap  {left:0;}
	.booklet .b-p0 .b-wrap  {right:0;}
   
    /* Custom Page Types */
	.booklet .b-page-blank  {padding:0; width:100%; height:100%;}
	.booklet .b-page-cover  {padding:0; width:100%; height:100%; background:#dff9fb;}
	
   	/* Page Numbers */
	.booklet .b-counter {bottom:10px; position:absolute; display:block; width:25px; height:20px; background:#ccc; color:#444; text-align:center; font-size:10px; padding:5px 0 0;}
	.booklet .b-wrap-left  .b-counter  {left:10px;}
	.booklet .b-wrap-right .b-counter {right:10px;}
	
	/* Page Shadows */
	.booklet .b-shadow-f  {right:0; top:0; position:absolute; opacity:0; background-image:url("../../../Images/Shadow/shadow-top-forward.png"); background-repeat:repeat-y; background-position:100% 0;}
	.booklet .b-shadow-b  {left:0;  top:0; position:absolute; opacity:0; background-image:url("../../../Images/Shadow/shadow-top-back.png");    background-repeat:repeat-y; background-position:0 0;}
	
/* @z-index fix (needed for older IE browsers)
----------------------------------------*/
    .b-menu           {z-index:100;}
    .b-selector       {z-index:100;}
    .booklet          {z-index:10;}
    .b-pN             {z-index:10;}
    .b-p0             {z-index:30;}
    .b-p1             {z-index:20;}
    .b-p2             {z-index:20;}
    .b-p3             {z-index:30;}
    .b-p4             {z-index:10;}
    .b-prev           {z-index:40;}
    .b-next           {z-index:40;}
    .b-counter        {z-index:40;}
	
/* @Menu Items
----------------------------------------*/
	.b-menu {height:40px; padding:0 0 10px;}
	
	.b-selector             {height:40px; position:relative; float:right; border:none; color:#cecece; cursor:pointer; font-size:12px;}
	.b-selector .b-current  {padding:8px 15px 12px; line-height:20px; min-width:18px; height:20px; display:block; background:#000; text-align:center;}
	.b-selector-page        {width:auto; margin-left:15px;}
	.b-selector-chapter     {width:auto; z-index:100000}
	
	.b-selector:hover            {color:#fff; background-position:left 0px;}
	.b-selector:hover .b-current {background-position:right 0px;}
	.b-selector ul               {overflow:hidden; margin:0; list-style:none !important; position:absolute; top:40px; right:0; padding:0 0 10px; background:#000; width:320px;}
	.b-selector li               {border:none;}
	.b-selector a                {color:#cecece; height:14px; text-decoration:none; display:block; padding:5px 10px;}
	.b-selector a .b-text        {float:left; clear:none;}
	.b-selector a .b-num         {float:right; clear:none;}
	.b-selector a:hover          {color:#fff;}
	
	.contentEpisodeSummary
	{
	    font-size:14px;
	}
	.contentEpisodeSummary h3
	{
	    margin:0;
	}
	.contentEpisodeSummary h3 span
	{
	    font-size:12px;
	}
	.contentEpisodeSummary h3.headerContent
	{
	    width:100%;
	    background-color:#AAA;
	    padding:3px;
	    margin-bottom:5px;
	}
	.contentEpisodeSummary table tr td
	{
	    vertical-align:top;
	}
	.contentEpisodeSummary table tr td ul
	{
	    margin:0;
	    list-style-type:square;
	    padding:0;
	}
	.contentEpisodeSummary table tr td ul li
	{
	    padding:0;
	    margin:0;
	    color:#000;
	    height:15px;
	}
	.contentEpisodeSummary table.tblSummary tr td
	{
	    border: 1px solid;
	    padding:2px;
	    vertical-align:top;
	}
	.contentEpisodeSummary table.tblSummary thead
	{
	    background-color:#AAA;
	}
</style>
<div style="width:1200px;margin:0 auto;">
    <input type="hidden" value="2" runat="server" id="hdnIDBodyDiagram" />
    <div id="custom-menu"></div>
    <div id="mybook" style="border:0px solid #AAA; display:none;">
        <div class="contentEpisodeSummary"> 
            <center>
	            <h2 class="w3-blue" style="text-shadow:1px 1px 0 #444"><%=GetLabel("RESUME MEDIS")%></h2>     
            </center>
            <table style="margin-top:10px; width:100%" cellpadding="0" cellspacing="0">
                <colgroup style="width:130px"/>
                <colgroup style="width:10px; text-align: center"/>
                <colgroup />
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
                        </table>
                    </td>
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
                    <td class="tdLabel"><%=GetLabel("Diagnosa")%></td>
                    <td class="tdLabel"><%=GetLabel(":")%></td>
                    <td>
                        <textarea id="lblDiagnosis" runat="server" style="border:0; width:100%; height:250px; background-color: transparent" readonly></textarea>
                    </td>
                </tr>
            </table>
            <center>
	            <div id="lblMedicalNo" runat="server" class="w3-deep-orange w3-xxxlarge" style="text-shadow:1px 1px 0 #444"></div>
            </center>
        </div>
        <div title="Keluhan Utama & Riwayat Penyakit Pasien" rel="Kajian Awal Pasien"> 
	        <div class="contentEpisodeSummary" id="page1"> 
            </div>
        </div>
        <div> 
	        <div class="contentEpisodeSummary" id="page2"> 
            </div>
        </div>
        <div title="Pemeriksaan Fisik dan Tanda Vital" rel="Pemeriksaan Fisik dan Tanda Vital">
            <div class="contentEpisodeSummary" id="page3"> 
            </div>
        </div>
        <div> 
	        <div class="contentEpisodeSummary" id="page4"> 
            </div>
        </div>
        <div title="Instruksi Dokter" rel="Instruksi Dokter"> 
	        <div class="contentEpisodeSummary" id="page5"> 
            </div>
        </div>
        <div title="Diagnosa Pasien" rel="Diagnosa Pasien"> 
	        <div class="contentEpisodeSummary" id="page6"> 
            </div>
        </div>
        <div title="Catatan Terintegrasi" rel="Catatan Terintegrasi dan Konsul / Rawat Bersama"> 
	        <div class="contentEpisodeSummary" id="page7"> 
            </div>
        </div>
        <div>
            <div class="contentEpisodeSummary" id="page8" rel="Pemeriksaan Laboratorium"> 
            </div>
        </div>
        <div>
            <div class="contentEpisodeSummary" id="page9" rel="Pemeriksaan Radiologi"> 
            </div>
        </div>
        <div title="Pemeriksaan Laboratorium" id="sectionDischargeInformation"> 
	        <div class="contentEpisodeSummary" id="page10"> 
            </div>
        </div>
        <div></div>
    </div>
</div>         