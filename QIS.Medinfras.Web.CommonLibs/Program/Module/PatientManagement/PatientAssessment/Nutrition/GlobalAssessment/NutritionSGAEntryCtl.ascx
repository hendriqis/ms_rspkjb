<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionSGAEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NutritionSGAEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_sgaentryctl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $('#<%=txtWeightCurrent.ClientID %>').change(function () {
        var current = $('#<%=txtWeightCurrent.ClientID %>').val();
        var before = $('#<%=txtWeightBefore.ClientID %>').val();

        if (current != '' && parseInt(current) > 0 && before != '' && parseInt(before) > 0) {
            calculateWeightPercentage();
        }
        else {
            $('#<%=txtWeightCurrent.ClientID %>').val('');
        }
    });

    $('#<%=txtWeightBefore.ClientID %>').change(function () {
        var current = $('#<%=txtWeightCurrent.ClientID %>').val();
        var before = $('#<%=txtWeightBefore.ClientID %>').val();

        if (current != '' && parseInt(current) > 0 && before != '' && parseInt(before) > 0) {
            calculateWeightPercentage();
        }
        else {
            $('#<%=txtWeightCurrent.ClientID %>').val('');
        }
    });

    function calculateWeightPercentage() {
        var current = $('#<%=txtWeightCurrent.ClientID %>').val();
        var before = $('#<%=txtWeightBefore.ClientID %>').val();

        var percentage = 0;

        percentage = 100 - Math.ceil((current / before) * 100);
        $('#<%=txtWeightPercentage.ClientID %>').val(percentage);
    }

    //#region tab
    $(function () {
        $('#ulSGA li').click(function () {
            $('#ulSGA li.selected').removeAttr('class');
            $('.containerInfo').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });
    });
    //#endregion
</script>
<style type="text/css">
    .ulFooter li
    {
        display: inline-block;
        border-radius: 2px;
        list-style-type: none;
        min-width: 75px;
        height: 15px;
        margin: 0 10px;
        padding: 5px;
        font-size: 11px;
    }
    .ulTab
    {
        margin: 0;
        padding: 0;
    }
    .ulTab li
    {
        list-style-type: none;
        width: 100px;
        height: 40px;
        margin: 0 10px;
        padding: 5px;
    }
    .TabContent
    {
        background-color: #F8C299;
    }
</style>
<div style="height: 100%; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <tr>
        <td style="padding: 5px; vertical-align: top;">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 150px" />
                    <col style="width: 120px" />
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
            </table>
        </td>
    </tr>
    <div class="containerSGA" style="margin-bottom: 3px; background-color:#F8C299">
        <ul class="ulTabPage" id="ulSGA">
            <li contentid="containerChanges" class="selected">
                <%=GetLabel("PERUBAHAN")%></li>
            <li contentid="containerDiagnose">
                <%=GetLabel("PENYAKIT DAN HUBUNGAN DENGAN KEBUTUHAN GIZI")%></li>
            <li contentid="containerAssessment">
                <%=GetLabel("PENILAIAN DAN TINDAK LANJUT")%></li>
        </ul>
    </div>
    <div id="containerChanges" class="containerInfo">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 120px" />
                            <col style="width: 80px" />
                            <col style="width: 100px" />
                            <col style="width: 50px" />
                            <col style="width: 50px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label5" style="font-weight: bold">
                                    <%=GetLabel("Perubahan :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Berat Badan")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblIsWeightChanged" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ya" Value="1" />
                                    <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <label class="lblNormal" id="Label10">
                                    <%=GetLabel("Dalam Waktu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeightChangedPeriod" Width="50px" runat="server" CssClass="number" />
                                <%=GetLabel("hari ")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 90px" />
                                        <col style="width: 50px" />
                                        <col style="width: 90px" />
                                        <col style="width: 50px" />
                                        <col style="width: 25px" />
                                        <col style="width: 80px" />
                                    </colgroup>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <label class="lblNormal" id="Label7">
                                                <%=GetLabel("BB Sebelum")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWeightBefore" Width="100%" runat="server" CssClass="number" />
                                        </td>
                                        <td style="padding-left: 10px">
                                            <label class="lblNormal" id="Label8">
                                                <%=GetLabel("BB Sekarang")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWeightCurrent" Width="100%" runat="server" CssClass="number" />
                                        </td>
                                        <td style="padding-left: 10px">
                                            =
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWeightPercentage" Width="50px" runat="server" CssClass="number" />
                                            %
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Intake Makanan")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblIsIntakeChanged" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ya" Value="1" />
                                    <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <label class="lblNormal" id="Label13">
                                    <%=GetLabel("Dalam Waktu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFoodIntakeChangedPeriod" Width="50px" runat="server" CssClass="number" />
                                <%=GetLabel("hari ")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                            </td>
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                    </colgroup>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <asp:CheckBox ID="chkIsSubOptimumFood" runat="server" CssClass="chkIsSubOptimumFood"
                                                Text="Sub Optimum" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsHypoCaloric" runat="server" CssClass="chkIsHypoCaloric" Text="Hipo Kalori" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:CheckBox ID="chkIsOnlyLiquids" runat="server" CssClass="chkIsOnlyLiquids" Text="Total Cair" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsStarvation" runat="server" CssClass="chkIsStarvation" Text="Starvasi" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Gastrointestinal")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblIsGastroChanged" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ya" Value="1" />
                                    <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <label class="lblNormal" id="Label15">
                                    <%=GetLabel("Dalam Waktu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGastroChangedPeriod" Width="50px" runat="server" CssClass="number" />
                                <%=GetLabel("hari ")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                            </td>
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                    </colgroup>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <asp:CheckBox ID="chkIsNausea" runat="server" CssClass="chkIsNausea" Text="Mual" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsVomitting" runat="server" CssClass="chkIsVomitting" Text="Muntah" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:CheckBox ID="chkIsDiarrhea" runat="server" CssClass="chkIsDiarrhea" Text="Diare" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsAnorexia" runat="server" CssClass="chkIsAnorexia" Text="Anoreksia" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                                <label class="lblNormal" id="Label4">
                                    <%=GetLabel("Kapasitas Fungsional")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblIsActivityChanged" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ya" Value="1" />
                                    <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <label class="lblNormal" id="Label16">
                                    <%=GetLabel("Dalam Waktu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtActivityChangedPeriod" Width="50px" runat="server" CssClass="number" />
                                <%=GetLabel("hari ")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                            </td>
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                    </colgroup>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            <asp:CheckBox ID="chkIsSubOptimumActivity" runat="server" CssClass="chkIsSubOptimumActivity"
                                                Text="Sub Optimum" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsAmbulatory" runat="server" CssClass="chkIsAmbulatory" Text="Ambulatory" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:CheckBox ID="chkIsBedRest" runat="server" CssClass="chkIsBedRest" Text="Bed Rest" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerDiagnose" class="containerInfo" style="display: none; background-color:#F8C299">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 120px" />
                            <col style="width: 80px" />
                            <col style="width: 100px" />
                            <col style="width: 50px" />
                            <col style="width: 50px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" colspan="2">
                                <label class="lblNormal" id="Label9" style="font-weight: bold">
                                    <%=GetLabel("Penyakit dan Hubungan dengan Kebutuhan Gizi :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                                <label class="lblNormal" id="lblDiagnosisText">
                                    <%=GetLabel("Diagnosa Medis")%></label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtDiagnosisText" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                                <label class="lblNormal" id="lblDiagnosisRelated">
                                    <%=GetLabel("Tingkat Hubungan")%></label>
                            </td>
                            <td colspan="5">
                                <dxe:ASPxComboBox ID="cboGCDiagnosisSeverity" Width="150px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerAssessment" class="containerInfo" style="display: none; background-color:#F8C299">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 100px" />
                            <col style="width: 120px" />
                            <col style="width: 80px" />
                            <col style="width: 100px" />
                            <col style="width: 50px" />
                            <col style="width: 50px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" colspan="2">
                                <label class="lblNormal" id="lblPhysicalExam" style="font-weight: bold">
                                    <%=GetLabel("Penilaian Fisik :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="padding-left: 15px">
                            </td>
                            <td colspan="5">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="150px" />
                                        <col width="80px" />
                                        <col width="10px" />
                                        <col width="150px" />
                                        <col width="80px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <label class="lblNormal" id="lblSubcutaneousFat">
                                                <%=GetLabel("Hilang Lemak Subkutan")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboGCSubcutaneousFat" Width="80px" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label class="lblNormal" id="lblArmMuscle">
                                                <%=GetLabel("Hilang Otot Lengan")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboGCArmMuscle" Width="80px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal" id="lblAnkleEdema">
                                                <%=GetLabel("Edema Pergelangan Kaki")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboGCAnkleEdema" Width="80px" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label class="lblNormal" id="lblSacralEdema">
                                                <%=GetLabel("Edema Sacral")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboGCSacralEdema" Width="80px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal" id="lblAscites">
                                                <%=GetLabel("Ascites")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboGCAscites" Width="80px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblFoodAllergy">
                                    <%=GetLabel("Alergi Makanan")%></label>
                            </td>
                            <td colspan="5">
                                <table cellpadding="1" cellspacing="1" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblIsFoodAllergy" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Ya" Value="1" />
                                                <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="tdLabel" style="padding-left: 5px;">
                                            <label class="lblNormal">
                                                <%=GetLabel("Nama makanan") %></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFoodAllergenName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px; font-weight: bold">
                                <label class="lblMandatory">
                                    <%=GetLabel("Penilaian") %></label>
                            </td>
                            <td colspan="5">
                                <asp:RadioButtonList ID="rblSGAScore" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Gizi Baik" Value="A" />
                                    <asp:ListItem Text="Malnutrisi Sedang (B1)" Value="B1" />
                                    <asp:ListItem Text="Malnutrisi Sedang (B2)" Value="B2" />
                                    <asp:ListItem Text="Gizi Buruk" Value="C" />
                                    <asp:ListItem Text="Gizi Lebih" Value="D" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label6" style="font-weight: bold; text-decoration: underline">
                                    <%=GetLabel("Tindak Lanjut")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsUsingDietMenu" runat="server" CssClass="chkIsUsingDietMenu"
                                    Text="Diberikan Makanan Standar Rumah Sakit dengan jenis pilihan diet" />
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDietType" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsNeedAssessment" runat="server" CssClass="chkIsNeedAssessment"
                                    Text="Dilanjutkan Asuhan Gizi" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
