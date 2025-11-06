<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent3Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent3Ctl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<style type="text/css">   
    #contentDetail3NavPane > a       { margin:0; font-size:11px}
    #contentDetail3NavPane > a.selected { color:#fff!important;background-color:#f44336!important }       
</style>

<script type="text/javascript" id="dxss_MedicalSummaryContent3Ctl">
    $(function () {
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

        $('#contentDetail3NavPane a').first().click();
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
    function showDetailContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("content3Detail");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
</script>

<input type="hidden" runat="server" id="hdnMRN" value="0" />
<input type="hidden" id="hdnPageCount" runat="server" value='0' />
<input type="hidden" id="hdnPageIndex" runat="server" value='0' />
<div class="w3-border divContent w3-animate-left">
    <table style="margin-top:10px; width:100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:130px" />
            <col style="width:10px; text-align: center"/>
            <col />
        </colgroup>
        <tr>
            <td colspan="3" style="width:100%; text-align:left">
                <div id="contentDetail3NavPane" class="w3-bar w3-black">
                    <a contentID="content3DetailPage1" class="w3-bar-item w3-button tablink selected">Keluhan dan Riwayat Kesehatan</a>
                    <a contentID="content3DetailPage2" class="w3-bar-item w3-button tablink">Pemeriksaan Fisik</a>
                    <a contentID="content3DetailPage3" class="w3-bar-item w3-button tablink">Tanda Vital</a>
                    <a contentID="content3DetailPage4" class="w3-bar-item w3-button tablink">Body Diagram</a>
                    <a contentID="content3DetailPage5" class="w3-bar-item w3-button tablink">Psikososial Spiritual dan Kultural</a>
                    <a contentID="content3DetailPage6" class="w3-bar-item w3-button tablink">Kebutuhan Informasi dan Edukasi</a>
                    <a contentID="content3DetailPage7" class="w3-bar-item w3-button tablink">Perencanaan Pemulangan Pasien</a>
                    <a contentID="content3DetailPage8" class="w3-bar-item w3-button tablink">Asesmen Tambahan (Populasi Khusus)</a>
                </div>
  
                <div id="content3DetailPage1" class="content3Detail w3-animate-top" style="display:none"> 
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
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtHPISummary" Width="100%" runat="server" TextMode="MultiLine"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
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
                                                <col />
                                            </colgroup>        
                                            <tr>
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
                                    <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
                                <div class="containerTblEntryContent">
                                    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                                            <colgroup>
                                                <col />
                                            </colgroup>        
                                            <tr>
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
                                                <col />
                                            </colgroup>    
                                            <tr>
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
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="content3DetailPage2" class="content3Detail w3-animate-top" style="display:none">
                    <input type="hidden" runat="server" id="hdnPhysicalExamLayout" value="" />
                    <input type="hidden" runat="server" id="hdnPhysicalExamValue" value="" />
                    <div id="divFormContent1" runat="server" style="height: 490px;width:100%;overflow-y: scroll;overflow-x: auto;"></div>
                </div>

                <div id="content3DetailPage3" class="content3Detail w3-animate-top" style="display:none">
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
                <div id="content3DetailPage4" class="w3-container w3-border w3-animate-top content3Detail" style="display:none">
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
                <div id="content3DetailPage5" class="w3-container w3-border w3-animate-top content3Detail" style="display:none">
                    <input type="hidden" runat="server" id="hdnSocialHistoryLayout" value="" />
                    <input type="hidden" runat="server" id="hdnSocialHistoryValue" value="" />
                    <div id="divFormContent2" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="content3DetailPage6" class="w3-container w3-border w3-animate-top content3Detail" style="display:none">
                    <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
                    <input type="hidden" runat="server" id="hdnEducationValue" value="" />
                    <div id="divFormContent3" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="content3DetailPage7" class="w3-container w3-border w3-animate-top content3Detail" style="display:none">
                    <input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
                    <input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
                    <div id="divFormContent4" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
                <div id="content3DetailPage8" class="w3-container w3-border w3-animate-top content3Detail" style="display:none">
                    <input type="hidden" runat="server" id="hdnAdditionalAssessmentLayout" value="" />
                    <input type="hidden" runat="server" id="hdnAdditionalAssessmentValue" value="" />
                    <div id="divFormContent5" runat="server" style="min-height:490px; height: 490px;overflow-y: scroll;overflow-x: auto;"></div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
            </td>
            <td />
            <td>
            </td>
        </tr>
    </table>
</div>

