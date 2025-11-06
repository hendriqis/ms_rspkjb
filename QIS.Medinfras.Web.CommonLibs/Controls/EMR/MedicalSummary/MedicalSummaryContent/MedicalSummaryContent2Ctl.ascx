<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent2Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent2Ctl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_MedicalSummaryContent2Ctl">
    $(function () {
        registerCollapseExpandHandler();
    });

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
</script>
<input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
<div class="w3-border divContent w3-animate-left">
    <table style="margin-top: 10px; width: 100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 45%" />
            <col style="width: 55%" />
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
                                <%--                                <dxe:ASPxComboBox runat="server" ID="cboGCWeightChangedStatus" ClientInstanceName="cboGCWeightChangedStatus"
                                    Width="210px" Enabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCWeightChangedStatus(s); }"
                                        Init="function(s,e){ onGCWeightChangedStatus(s); }" />
                                </dxe:ASPxComboBox>--%>
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
                                <%--                                <dxe:ASPxComboBox runat="server" ID="cboGCWeightChangedGroup" ClientInstanceName="cboGCWeightChangedGroup"
                                    Width="210px" Enabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCWeightChangedGroup(s); }"
                                        Init="function(s,e){ onGCWeightChangedGroup(s); }" />
                                </dxe:ASPxComboBox>--%>
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
                                <asp:RadioButtonList ID="rblIsFoodIntakeChanged" runat="server" RepeatDirection="Horizontal">
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
                                <asp:RadioButtonList ID="rblIsHasSpecificDiagnosis" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ya" Value="1" />
                                    <asp:ListItem Text="Tidak" Value="0" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--                                <dxe:ASPxComboBox runat="server" ID="cboGCMSTDiagnosis" ClientInstanceName="cboGCMSTDiagnosis"
                                    Width="100%" Enabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCMSTDiagnosisChanged(s); }"
                                        Init="function(s,e){ onGCMSTDiagnosisChanged(s); }" />
                                </dxe:ASPxComboBox>--%>
                                <asp:TextBox ID="txtGCMSTDiagnosis" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherMSTDiagnosis" runat="server" Width="100%" ReadOnly="true" />
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
