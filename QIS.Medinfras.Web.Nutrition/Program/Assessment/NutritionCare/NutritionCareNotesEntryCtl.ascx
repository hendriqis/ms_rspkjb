<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionCareNotesEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionCareNotesEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sgaentryctl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

    //#region Diagnose
    $('#lblDiagnose.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsNutritionDiagnosis = 1 AND IsDeleted = 0", function (value) {
            $('#<%=txtDiagnoseID.ClientID %>').val(value);
            onTxtDiagnoseIDChanged(value);
        });
    });

    $('#<%=txtDiagnoseID.ClientID %>').live('change', function () {
        onTxtDiagnoseIDChanged($(this).val());
    });

    function onTxtDiagnoseIDChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnoseID.ClientID %>').val(result.DiagnoseID);
                $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
            }
            else {
                $('#<%=txtDiagnoseID.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<div style="height: 500px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 170px" />
                        <col style="width: 150px" />
                        <col style="width: 80px" />
                        <col style="width: 100px" />
                        <col style="width: 50px" />
                        <col style="width: 50px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal - Jam Pengkajian")%>
                        </td>
                        <td colspan="2">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 170px" />
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtAssessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAssessmentTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Ahli Gizi") %></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label5" style="font-weight: bold">
                                <%=GetLabel("ASESMEN GIZI")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label16">
                                <%=GetLabel("Antropometri (AD)")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtAntropometri" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label19">
                                <%=GetLabel("Pemeriksaan Fisik/Klinik (PD)")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtReviewOfSystem" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Riwayat Personal (CH)")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtPersonalHistory" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label8">
                                <%=GetLabel("Biokimia (BD)")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtBioChemical" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label7">
                                <%=GetLabel("Riwayat Terkait Gizi dan Makanan (FH)")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtNutritionHistory" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblLink" id="lblDiagnose">
                                <%=GetLabel("Diagnosa Gizi")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDiagnoseID" Width="120px" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="250px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label17">
                                <%=GetLabel("Etiologi")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtEtiologi" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label18">
                                <%=GetLabel("Signs/Symptoms")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtSymptoms" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label2" style="font-weight: bold">
                                <%=GetLabel("INTERVENSI GIZI")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label3">
                                <%=GetLabel("Tujuan")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtInterventionPurposes" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label10">
                                <%=GetLabel("Prinsip / Syarat Diet")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtDietPrincipal" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label11">
                                <%=GetLabel("Jenis Diet")%></label>
                        </td>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboGCDietType" ClientInstanceName="cboGCDietType"
                                Width="210px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label12">
                                <%=GetLabel("Bentuk Makanan")%></label>
                        </td>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboGCMealForm" ClientInstanceName="cboGCMealForm"
                                Width="210px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label13">
                                <%=GetLabel("Rute")%></label>
                        </td>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboGCRoute" ClientInstanceName="cboGCRoute"
                                Width="210px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label4" style="font-style: italic;">
                                <%=GetLabel("Edukasi / Konseling Gizi")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label6">
                                <%=GetLabel("Materi :")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtCouncelingMaterial" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label9">
                                <%=GetLabel("Media :")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtCouncelingMedia" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label14">
                                <%=GetLabel("Sasaran :")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtCouncelingGoal" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label15">
                                <%=GetLabel("Rencana Monitoring & Evaluasi :")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtEvaluationPlanning" runat="server" Width="100%" TextMode="Multiline"
                                Height="100px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
