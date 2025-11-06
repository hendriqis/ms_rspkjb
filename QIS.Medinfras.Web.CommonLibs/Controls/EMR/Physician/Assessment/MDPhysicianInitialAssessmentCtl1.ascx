<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDPhysicianInitialAssessmentCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MDPhysicianInitialAssessmentCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
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

    function onCbpROSViewEndCallback(s) {
        $('#containerImgLoadingView').hide();

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
    <input type="hidden" id="hdnPopupAssessmentLayout" runat="server" value='0' />
    <input type="hidden" id="hdnPopupAssessmentValue" runat="server" value='0' />
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
                    <%=GetLabel("PENGKAJIAN AWAL MEDIS")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentid="divPage2" title="Keluhan & Riwayat Kesehatan" class="w3-hover-red">Keluhan
                            & Riwayat Kesehatan</li>
                        <li contentid="divPage3" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan
                            Fisik</li>
                        <li contentid="divPage4" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">
                            Tanda Vital dan Indikator Lainnya</li>
                        <li contentid="divPage5" title="Body Diagram" class="w3-hover-red">Body Diagram</li>
                        <li contentid="divPage6" title="Diagnosa" class="w3-hover-red">
                            Diagnosa</li>
                        <li contentid="divPage7" title="Prosedur/Tindakan" class="w3-hover-red">
                            Prosedur/Tindakan</li>
                        <li contentid="divPage8" title="Catatan Hasil Pemeriksaan Penunjang" class="w3-hover-red">
                            Catatan Hasil Pemeriksaan</li>
                        <li contentid="divPage9" title="Pemeriksaan Penunjang & Rencana Tindakan" class="w3-hover-red">Pemeriksaan Penunjang & Rencana Tindakan</li>
                        <li contentid="divPage10" title="Instruksi Dokter" class="w3-hover-red">Instruksi Dokter</li>
                        <li contentid="divPage11" title="Sasaran Asuhan" class="w3-hover-red">Sasaran Asuhan</li>
                    </ul>
                </div>
                <div>
                    <table class="w3-table-all" style="width: 100%">
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class=" w3-small">
                                    <%=GetLabel("Dikaji Oleh :")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div id="lblAssessmentParamedicName" runat="server" class="w3-medium">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 5px;">
                <div id="divPage1" class="w3-border divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 130px" />
                            <col style="width: 10px; text-align:center" />
                            <col />
                            <col style="width: 130px" />
                        </colgroup>
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
                                                    <asp:TextBox ID="txtPhysicianName" Width="99%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Keluhan Utama")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtChiefComplaint" Width="99%" runat="server" TextMode="MultiLine"
                                                        Rows="8" ReadOnly="true" />
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
                                                    <asp:TextBox ID="txtHPISummary" Width="99%" runat="server" TextMode="MultiLine"
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
                            <td style="vertical-align: top">
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
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
                                                    <asp:TextBox ID="txtMedicalHistory" Width="99%" runat="server" TextMode="MultiLine"
                                                        Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <h4 class="w3-blue h4expanded">
                                    <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
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
                                                        <%=GetLabel("Riwayat Penggunaan Obat")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMedicationHistory" Width="99%" runat="server" TextMode="MultiLine"
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
                                                <col style="width: 150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" valign="top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Riwayat Penyakit Keluarga")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFamilyHistory" Width="99%" runat="server" TextMode="MultiLine"
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
                <div id="divPage3" class="w3-border divContent w3-animate-left" style="display: none">
                    <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                        ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContentROS" runat="server">
                                <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
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
                                                    Pemeriksaan Fisik
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
                                            <%=GetLabel("Belum ada pemeriksaan fisik untuk kajian awal medis") %>
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
                            <div id="rosPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage4" class="w3-border divContent w3-animate-left" style="display: none">
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
                                                        Tanda Vital dan Indikator Lainnya
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
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="vitalSignPaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage5" class="w3-border divContent w3-animate-left" style="display: none">
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
                <div id="divPage6" class="divContent w3-animate-left" style="display: none">
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
                <div id="divPage7" class="divContent w3-animate-left" style="display: none">
                    <dxcp:ASPxCallbackPanel ID="cbpProcedureView" runat="server" Width="100%" ClientInstanceName="cbpProcedureView"
                        ShowLoadingPanel="false" OnCallback="cbpProcedureView_Callback">
                        <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContentProcedure" runat="server">
                                <asp:Panel runat="server" ID="Panel11" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdProcedureView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Procedure Information")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <%#: Eval("ProcedureDateInString")%>,
                                                        <%#: Eval("ProcedureTime")%>,
                                                        <%#: Eval("ParamedicName")%></div>
                                                    <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <span style="color: Blue; font-size: 1.1em">
                                                            <%#: Eval("ProcedureText")%></span> (<b><%#: Eval("ProcedureID")%></b>)
                                                    </div>
                                                    <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <%#: Eval("Remarks")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Belum ada informasi Prosedur/Tindakan untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="procedurePaging">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPage8" class="divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Hasil Pemeriksaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnosticResultSummary" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="10" ReadOnly="true" />
                            </td>                            
                        </tr>
                    </table>
                </div>
                <div id="divPage9" class="divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Order Penunjang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlanningText" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="20" ReadOnly="true" />
                            </td>                            
                        </tr>
                    </table>
                </div>
                 <div id="divPage10" class="divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Instruksi Dokter")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstructionText" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="20" ReadOnly="true" />
                            </td>                            
                        </tr>
                    </table>
                </div>
                 <div id="divPage11" class="divContent w3-animate-left" style="display: none">
                    <table style="margin-top: 5px; width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Sasaran Asuhan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNursingObjectives" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="20" ReadOnly="true" />
                            </td>                            
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>