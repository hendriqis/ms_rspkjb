<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionStrongKidsEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NutritionStrongKidsEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sgaentryctl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#<%=rblIsSkinny.ClientID %> input').change(function () {
        $('#<%=txtIsSkinny.ClientID %>').val($(this).val());
        calculateStrongKidsTotalScore();
    });

    $('#<%=rblIsWeightChanged.ClientID %> input').change(function () {
        $('#<%=txtIsWeightChanged.ClientID %>').val($(this).val());
        calculateStrongKidsTotalScore();
    });

    $('#<%=rblIsSpecificCondition.ClientID %> input').change(function () {
        $('#<%=txtIsSpecificCondition.ClientID %>').val($(this).val());
        calculateStrongKidsTotalScore();
    });

    $('#<%=rblIsMalnutrition.ClientID %> input').change(function () {
        $('#<%=txtIsMalnutrition.ClientID %>').val($(this).val());
        calculateStrongKidsTotalScore();
    });

    function calculateStrongKidsTotalScore() {
        var p1 = 0;
        var p2 = 0;
        var p3 = 0;
        var p4 = 0;

        if ($('#<%=txtIsSkinny.ClientID %>').val())
            p1 = parseInt($('#<%=txtIsSkinny.ClientID %>').val());
        if ($('#<%=txtIsWeightChanged.ClientID %>').val())
            p2 = parseInt($('#<%=txtIsWeightChanged.ClientID %>').val());
        if ($('#<%=txtIsSpecificCondition.ClientID %>').val())
            p3 = parseInt($('#<%=txtIsSpecificCondition.ClientID %>').val());
        if ($('#<%=txtIsMalnutrition.ClientID %>').val())
            p4 = parseInt($('#<%=txtIsMalnutrition.ClientID %>').val());

        var total = p1 + p2 + p3 + p4;
        $('#<%=txtTotalScore.ClientID %>').val(total);
    }
</script>
<div style="height: 100%; overflow-y: scroll;">
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
                        <col style="width: 700px" />
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
                        <td>
                            <asp:TextBox ID="txtAssessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessmentTime" Width="100%" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Ahli Gizi") %></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label5" style="font-weight: bold">
                                <%=GetLabel("Asesmen Nutrisi Pasien Anak < 15 Tahun")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("1. Berdasarkan penilaian klinis, Apakah pasien berstatus gizi kurang / buruk?")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsSkinny" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtIsSkinny" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("2. Apakah terdapat penurunan BB selama satu bulan terakhir? (berdasarkan penilaian objecktif data BB bila ada / penilaian subjektif dari orang tua)")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsWeightChanged" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtIsWeightChanged" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label6">
                                <%=GetLabel("3. Apakah terdapat salah satu dari kondisi berikut?")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label7">
                                <%=GetLabel("- Diare >= 5x/hari dan / atau muntah >3x/hari dalam seminggu terakhir")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label3">
                                <%=GetLabel("- Asupan makan berkurang selama 1 minggu terakhir")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsSpecificCondition" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtIsSpecificCondition" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label4">
                                <%=GetLabel("4. Apakah terdapat penyakit atau keadaan yang mengakibatkan pasien berisiko mengalami malnutrisi?")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsMalnutrition" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="2" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtIsMalnutrition" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label8" style="font-weight: bold">
                                <%=GetLabel("TOTAL SKOR")%></label>
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtTotalScore" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
