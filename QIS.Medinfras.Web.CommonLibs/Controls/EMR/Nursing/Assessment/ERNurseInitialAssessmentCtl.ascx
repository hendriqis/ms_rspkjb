<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ERNurseInitialAssessmentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ERNurseInitialAssessmentCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl1">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

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

        registerCollapseExpandHandler();

        $('#leftPanel ul li').first().click();
    });

    $(function () {
        setPaging($("#diagnosisPaging"), parseInt('<%=gridDiagnosisPageCount %>'), function (page) {
            cbpDiagnosisView.PerformCallback('changepage|' + page);
        });

        setPaging($("#allergyPaging"), parseInt('<%=gridAllergyPageCount %>'), function (page) {
            cbpDiagnosisView.PerformCallback('changepage|' + page);
        });

        setPaging($("#vitalSignPaging"), parseInt('<%=gridVitalSignPageCount %>'), function (page) {
            cbpVitalSignView.PerformCallback('changepage|' + page);
        });

    });

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


    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }

    //#region RAPUH SCORE CALCULATION
    function onCboRAPUH_R_Changed(s) {
        if (cboRAPUH_R.GetValue() != null) {
            if (cboRAPUH_R.GetValue().indexOf('^01') > -1) {
                $('#<%=txtRAPUH_R.ClientID %>').val('1');
            }
            else {
                $('#<%=txtRAPUH_R.ClientID %>').val('0');
            }
        }
        calculateRAPUHScore();
    }
    function onCboRAPUH_A_Changed(s) {
        if (cboRAPUH_A.GetValue() != null) {
            if (cboRAPUH_A.GetValue().indexOf('^01') > -1) {
                $('#<%=txtRAPUH_A.ClientID %>').val('1');
            }
            else if (cboRAPUH_A.GetValue().indexOf('^02') > -1) {
                $('#<%=txtRAPUH_A.ClientID %>').val('1');
            }
            else if (cboRAPUH_A.GetValue().indexOf('^03') > -1) {
                $('#<%=txtRAPUH_A.ClientID %>').val('0');
            }
            else {
                $('#<%=txtRAPUH_A.ClientID %>').val('0');
            }
        }
        calculateRAPUHScore();
    }
    function onCboRAPUH_P_Changed(s) {
        if (cboRAPUH_P.GetValue() != null) {
            if (cboRAPUH_P.GetValue().indexOf('^01') > -1) {
                $('#<%=txtRAPUH_P.ClientID %>').val('0');
            }
            else {
                $('#<%=txtRAPUH_P.ClientID %>').val('1');
            }
        }
        calculateRAPUHScore();
    }
    function onCboRAPUH_U_Changed(s) {
        if (cboRAPUH_U.GetValue() != null) {
            if (cboRAPUH_U.GetValue().indexOf('^01') > -1) {
                $('#<%=txtRAPUH_U.ClientID %>').val('1');
            }
            else {
                $('#<%=txtRAPUH_U.ClientID %>').val('0');
            }
        }
        calculateRAPUHScore();
    }
    function onCboRAPUH_H_Changed(s) {
        if (cboRAPUH_H.GetValue() != null) {
            if (cboRAPUH_H.GetValue().indexOf('^01') > -1) {
                $('#<%=txtRAPUH_H.ClientID %>').val('0');
            }
            else {
                $('#<%=txtRAPUH_H.ClientID %>').val('1');
            }
        }
        calculateRAPUHScore();
    }

    function calculateRAPUHScore() {
        var p1 = 0;
        var p2 = 0;
        var p3 = 0;
        var p4 = 0;
        var p5 = 0;

        if ($('#<%=txtRAPUH_R.ClientID %>').val())
            p1 = parseInt($('#<%=txtRAPUH_R.ClientID %>').val());

        if ($('#<%=txtRAPUH_A.ClientID %>').val())
            p2 = parseInt($('#<%=txtRAPUH_A.ClientID %>').val());

        if ($('#<%=txtRAPUH_P.ClientID %>').val())
            p3 = parseInt($('#<%=txtRAPUH_P.ClientID %>').val());

        if ($('#<%=txtRAPUH_U.ClientID %>').val())
            p4 = parseInt($('#<%=txtRAPUH_U.ClientID %>').val());

        if ($('#<%=txtRAPUH_H.ClientID %>').val())
            p5 = parseInt($('#<%=txtRAPUH_H.ClientID %>').val());

        var total = p1 + p2 + p3 + p4 + p5;
        $('#<%=txtRAPUHScore.ClientID %>').val(total);

        if (total <= 0) {
            cboRAPUHScore.SetValue("X096^01");
        }
        else if (total >= 1 && total <= 2) {
            cboRAPUHScore.SetValue("X096^02");
        }
        else {
            cboRAPUHScore.SetValue("X096^03");
        }
    }
    //#endregion 
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
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("ASESMEN AWAL PASIEN (PERAWAT)")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" title="Keluhan & Riwayat Kesehatan" class="w3-hover-red">Keluhan & Riwayat Kesehatan</li>
                        <li contentID="divPage3" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>
                        <li contentID="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>
                        <li contentID="divPage5" title="Body Diagram" class="w3-hover-red">Body Diagram</li>
                        <li contentID="divPage6" title="Kebutuhan Edukasi" class="w3-hover-red">Kebutuhan Edukasi</li>
<%--                    <li contentID="divPage7" title="Kebutuhan Informasi dan Edukasi" class="w3-hover-red">Kebutuhan Informasi dan Edukasi</li>
                        <li contentID="divPage8" title="Perencanaan Pemulangan Pasien" class="w3-hover-red">Perencanaan Pemulangan Pasien</li>
                        <li contentID="divPage9" title="Asesmen Tambahan (Populasi Khusus)" class="w3-hover-red">Asesmen Tambahan (Populasi Khusus)</li>
                        <li contentID="divPage10" title="Pemeriksaan Fisik" class="w3-hover-red">a</li>
                        <li contentID="divPage11" title="Psikososial Spiritual dan Kultural" class="w3-hover-red">b</li>--%>
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
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <colgroup>
                            <col width="50%" />
                            <col width="50%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("Keluhan & Riwayat Kesehatan")%></h4>
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
                                                    <asp:TextBox ID="txtPhysicianName" Width="340px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Anamnesa Perawat")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtChiefComplaint" Width="340px" runat="server" TextMode="MultiLine"
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
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("SURVEY PRIMER")%></h4>
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
                                                        <%=GetLabel("Airway")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboAirway" ClientInstanceName="cboAirway" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Breathing")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboBreathing" ClientInstanceName="cboBreathing" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Circulation")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboCirculation" ClientInstanceName="cboCirculation" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Disability")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDisability" ClientInstanceName="cboDisability" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Exposure")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboExposure" ClientInstanceName="cboExposure" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("Riwayat Penyakit Dahulu dan Terapi")%></h4>
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
                                                        <%=GetLabel("Riwayat Penyakit Dahulu")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicalHistory" Width="340px" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Pengobatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicationHistory" Width="340px" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td style="vertical-align:top">
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("TRIASE")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:300px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>        
                                            <tr>
                                                <td class="tdLabel">
                                                    <label>
                                                        <%:GetLabel("Jenis Kunjungan")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Alasan Kunjungan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%" ReadOnly="true" />
                                                    <asp:TextBox ID="txtVisitNotes" Width="340px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 100%;">
                                                        <%=GetLabel("Waktu Kejadian") %></div>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmergencyCaseDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                    <asp:TextBox ID="txtEmergencyCaseTime" Width="80px" runat="server" CssClass="time" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 100%;">
                                                        <%=GetLabel("Waktu Registrasi")%></div>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                    <asp:TextBox ID="txtRegistrationTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                                                </td>
                                             </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 100%;">
                                                        <%=GetLabel("Waktu Pelayanan")%></div>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                                                    <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        <%=GetLabel("Cara Datang")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <dxe:ASPxComboBox runat="server" ID="cboAdmissionRoute" ClientInstanceName="cboAdmissionRoute"
                                                        Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        <%=GetLabel("Triage")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label">
                                                        <%=GetLabel("Keadaan Datang")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                                        Width="100%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                    <%=GetLabel("Lokasi dan Mekanisme Trauma") %>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtEmergencyCase" runat="server" Width="340px" TextMode="Multiline"
                                                        Rows="3" ReadOnly="true" />
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
                                                    <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                                                        ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage3">
                                                                    <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                                                ItemStyle-CssClass="keyField" >
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
                                                                                HeaderStyle-HorizontalAlign="Left" >
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                                                HeaderStyle-HorizontalAlign="Left" >
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" >
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                                                HeaderStyle-HorizontalAlign="Left" >
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Reaction" HeaderText="Reaction" 
                                                                                HeaderStyle-HorizontalAlign="Left" >
                                                                            </asp:BoundField>
                                                                        </Columns>        
                                                                        <EmptyDataRowStyle CssClass="trEmpty">
                                                                        </EmptyDataRowStyle>
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
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Diagnosa Pasien")%></h4>
                                <div class="containerTblEntryContent containerEntryPanel1">
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                        <tr>
                                            <td>
                                                <div style="position: relative;">
                                                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                                                        ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                                    <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage" 
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                                                ItemStyle-CssClass="keyField" >
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
                                                                                        <span Style="color: Blue; font-size: 1.1em">
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
                                                                        <EmptyDataRowStyle CssClass="trEmpty">
                                                                        </EmptyDataRowStyle>
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
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Asesment Fungsional")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col style="width: 500px" />
                                                <col />
                                            </colgroup>        
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Ketergantungan Fungsional")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboFunctionalType" ClientInstanceName="cboFunctionalType" Width="100%" ReadOnly="true" />
                                                    <asp:TextBox ID="txtFunctionalTypeRemarks" CssClass="txtFunctionalTypeRemarks" Width="340px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Riwayat Psikososial - Spiritual")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:250px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">   
                                            <colgroup>
                                                <col width="155px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="labelColumn" colspan="2">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Hubungan dengan Anggota Keluarga :")%></label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblFamilyRelationship" CssClass="rblFamilyRelationship" runat="server"
                                                        RepeatDirection="Horizontal" CellPadding="10" ReadOnly="true" >
                                                        <asp:ListItem Text=" Baik" Value="1" />
                                                        <asp:ListItem Text=" Kurang Baik" Value="0" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelColumn">
                                                    <label class="lblNormal" style="padding-left:10px">
                                                        <%=GetLabel("Jelaskan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtFamilyRelationshipRemarks" CssClass="txtFamilyRelationshipRemarks" runat="server" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelColumn" colspan="2">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Memerlukan kebutuhan privasi tambahan :")%></label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblIsNeedAdditionalPrivacy" CssClass="rblIsNeedAdditionalPrivacy" runat="server"
                                                        RepeatDirection="Horizontal" CellPadding="14" ReadOnly="true" >
                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelColumn">
                                                    <label class="lblNormal" style="padding-left:10px">
                                                        <%=GetLabel("Jelaskan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtNeedAdditionalPrivacyRemarks" CssClass="txtNeedAdditionalPrivacyRemarks" runat="server" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Status Psikologis")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox runat="server" ID="cboPsychologyStatus" ClientInstanceName="cboPsychologyStatus"
                                                        Width="100%" ReadOnly="true" >
                <%--                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPsychologyStatusChanged(s); }"
                                                            Init="function(s,e){ onCboPsychologyStatusChanged(s); }" />--%>
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelColumn" colspan="2">
                                                    <label class="lblNormal" style="padding-left:10px">
                                                        <%=GetLabel("Kecenderungan bunuh diri dilaporkan ke")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCommitSuicideRemarks" CssClass="txtCommitSuicideRemarks" Width="100%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelColumn" colspan="2">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Terdapat masalah ekonomi :")%></label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblHasFinancialProblem" CssClass="rblHasFinancialProblem" runat="server"
                                                        RepeatDirection="Horizontal" CellPadding="14" ReadOnly="true" >
                                                        <asp:ListItem Text=" Ya" Value="1" />
                                                        <asp:ListItem Text=" Tidak" Value="0" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelColumn">
                                                    <label class="lblNormal" style="padding-left:10px">
                                                        <%=GetLabel("Jelaskan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtFinancialProblemRemarks" CssClass="txtFinancialProblemRemarks" runat="server" Width="100%" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Asesment RAPUH")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsHasRAPUHAssessment" runat="server" Text="  Asesment RAPUH" Checked="false" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" title="Dengan diri sendiri atau tanpa bantuan alat mengalami kesulitan untuk naik 10 anak tangga dan tanpa istirahat diantaranya?">
                                                        <%=GetLabel("Resistensi")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRAPUH_R" ClientInstanceName="cboRAPUH_R"
                                                        Width="100%" ReadOnly="true" >
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_R_Changed(s); }"
                                                            Init="function(s,e){ onCboRAPUH_R_Changed(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_R" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" title="Seberapa sering dalam 4 minggu merasa kelelahan?">
                                                        <%=GetLabel("Aktifitas")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRAPUH_A" ClientInstanceName="cboRAPUH_A"
                                                        Width="100%" ReadOnly="true" >
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_A_Changed(s); }"
                                                            Init="function(s,e){ onCboRAPUH_A_Changed(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_A" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" title="Apakah Dokter pernah mengatakan bahwa pasien mempunyai hipertensi, diabetes, kanker, penyakit paru kronis, serangan jantung, gagal jantung, kongestif, nyeri dada, asma, nyeri sendi, stroke dan penyakit ginjal?">
                                                        <%=GetLabel("Penyakit lebih dari 5")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRAPUH_P" ClientInstanceName="cboRAPUH_P"
                                                        Width="100%" ReadOnly="true" >
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_P_Changed(s); }"
                                                            Init="function(s,e){ onCboRAPUH_P_Changed(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_P" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" title="Dengan diri sendiri dan tanpa bantuan apakah pasien mengalami kesulitan berjalan sejauh 100-200 meter?">
                                                        <%=GetLabel("Usaha Berjalan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRAPUH_U" ClientInstanceName="cboRAPUH_U"
                                                        Width="100%" ReadOnly="true" >
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_U_Changed(s); }"
                                                            Init="function(s,e){ onCboRAPUH_U_Changed(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_U" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" title="Berapa berat badan pasien dengan mengenakan baju tanpa alas kaki saat ini dan 1 tahun yang lalu?">
                                                        <%=GetLabel("Hilangnya berat badan")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboRAPUH_H" ClientInstanceName="cboRAPUH_H"
                                                        Width="100%" ReadOnly="true" >
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRAPUH_H_Changed(s); }"
                                                            Init="function(s,e){ onCboRAPUH_H_Changed(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="padding-left:5px;"><%=GetLabel("Skor") %></td>
                                                            <td style="padding-left:5px;width:60px"><asp:TextBox ID="txtRAPUH_H" runat="server" Width="100%" ReadOnly="true" CssClass="number" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal" title="Berapa berat badan pasien dengan mengenakan baju tanpa alas kaki saat ini dan 1 tahun yang lalu?">
                                                        <%=GetLabel("Total Nilai RAPUH")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                       <tr>
                                                            <td><asp:TextBox ID="txtRAPUHScore" runat="server" Width="100px" CssClass="number" ReadOnly="true" /></td>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Kesimpulan")%></label>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboRAPUHScore" ClientInstanceName="cboRAPUHScore"
                                                                    Width="100%" ReadOnly="true" >
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                       </tr>
                                                    </table>     
                                                </td>                              
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Masalah Keperawatan")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                            <tr>
                                                <td>
                                                    <dxcp:ASPxCallbackPanel ID="cbpNursingProblemView" runat="server" Width="100%" ClientInstanceName="cbpNursingProblemView"
                                                        ShowLoadingPanel="false" OnCallback="cbpNursingProblemView_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ oncbpNursingProblemViewEndCallback(s); }" />
                                                        <PanelCollection>
                                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage4">
                                                                    <asp:GridView ID="grdNursingProblemView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                                                ItemStyle-CssClass="keyField" >
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                                <ItemTemplate>
                                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                                    <input type="hidden" value="<%#:Eval("ProblemName") %>" bindingfield="ProblemName" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="ProblemName" HeaderText="Masalah Keperawatan" HeaderStyle-Width="200px"
                                                                                HeaderStyle-HorizontalAlign="Left" >
                                                                            </asp:BoundField>
                                                                        </Columns>        
                                                                        <EmptyDataRowStyle CssClass="trEmpty">
                                                                        </EmptyDataRowStyle>
                                                                        <EmptyDataTemplate>
                                                                            <%=GetLabel("Tidak ada data masalah keperawatan dalam episode ini")%>
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dxcp:ASPxCallbackPanel>
                                                    <div class="containerPaging">
                                                        <div class="wrapperPaging">
                                                            <div id="Div1">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Catatan Instruksi")%></h4>
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
                                                        <%=GetLabel("Catatan Instruksi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInstructionText" Width="100%" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4collapsed">
                                    <%=GetLabel("Catatan Planning")%></h4>
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
                                                        <%=GetLabel("Catatan Planning")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPlanningNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table> 
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display:none">
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpReviewOfSystemView" runat="server" Width="100%" ClientInstanceName="cbpReviewOfSystemView"
                            ShowLoadingPanel="false" OnCallback="cbpReviewOfSystemView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ oncbpReviewOfSystemViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage4">
                                        <asp:GridView ID="grdReviewOfSystemView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdReviewOfSystemView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <h3><%=GetLabel("Pemeriksaan Fisik")%></h3> 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                            <br />
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 350px;">
                                                                        <strong>
                                                                            <div style="width: 120px; float: left;" class="labelColumn">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %></div>
                                                                            <div style="width: 20px; float: left;">
                                                                                :</div>
                                                                        </strong>
                                                                        <div style="width: 80px; float: left;">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "cfRemarks") %></div>
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
                                                <%=GetLabel("Tidak ada pemeriksaan fisik") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="Div3">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display:none">
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
                                                        <h3><%=GetLabel("Tanda Vital dan Indikator Lainnya")%></h3> 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                            <br />
                                                            <span style="font-style:italic">
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
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="vitalSignPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage5" class="w3-border divContent w3-animate-left" style="display:none">
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
                                                    <div class="templatePatientBodyDiagram" style="width:100% ; height:500px; overflow-y:auto">
                                                        <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />
                                                        <div class="containerImage boxShadow">
                                                            <img src='' alt="" id="imgBodyDiagram" runat="server" />
                                                        </div>
                                                        <span class="spLabel">
                                                            <%=GetLabel("Nama Diagram") %></span> : <span class="spValue" id="spnDiagramName" runat="server"></span><br />
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
                                                                    <td><img alt="" style="width: 16px; height: 16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>" /></td>
                                                                    <td>:</td>
                                                                    <td><%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%></td>
                                                                    <td>:</td>
                                                                    <td><%#: DataBinder.Eval(Container.DataItem, "SymbolName")%></td>
                                                                    <td>:</td>
                                                                    <td><%#: DataBinder.Eval(Container.DataItem, "Remarks")%></td>
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
                <div id="divPage6" class="w3-border divContent w3-animate-left" style="display:none">
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpPatientEducationView" runat="server" Width="100%" ClientInstanceName="cbpPatientEducationView"
                            ShowLoadingPanel="false" OnCallback="cbpPatientEducationView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ oncbpPatientEducationViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent7" runat="server">
                                    <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage4">
                                        <asp:GridView ID="grdPatientEducationView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdPatientEducationView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <h3><%=GetLabel("Kebutuhan Edukasi")%></h3> 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("cfEducationDate")%>,
                                                                <%#: Eval("EducationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptPatientEducationDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 350px;">
                                                                        <strong>
                                                                            <div style="width: 250px; float: left;" class="labelColumn">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "EducationType") %></div>
                                                                            <div style="width: 20px; float: left;">
                                                                                :</div>
                                                                        </strong>
                                                                        <div style="width: 80px; float: left;">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "Remarks") %></div>
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
                                                <%=GetLabel("Tidak ada pemeriksaan fisik") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <br />
                        <asp:CheckBox ID="chkIsNeedPatientEducation" runat="server" Text=" Kebutuhan Edukasi"
                                Checked="false" Enabled="false" />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="Div4">
                            </div>
                        </div>
                    </div>
                </div>


                <div id="divPage7" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
                    <input type="hidden" runat="server" id="hdnEducationValue" value="" />
                    <div id="divFormContent3" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>    
                <div id="divPage8" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
                    <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
                    <div id="divFormContent4" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="divPage9" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnAdditionalAssessmentLayout" value="" />
                    <input type="hidden" runat="server" id="hdnAdditionalAssessmentValue" value="" />
                    <div id="divFormContent5" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="divPage10" class="w3-border divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
                    <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
                    <div id="divFormContent1" runat="server" style="height: 490px;width:100%;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="divPage11" class="divContent w3-animate-left" style="display:none">
                    <input type="hidden" runat="server" id="hdnSocialHistoryLayout" value="" />
                    <input type="hidden" runat="server" id="hdnSocialHistoryValue" value="" />
                    <div id="divFormContent2" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div> 
            </td>
        </tr>
    </table>
</div>         