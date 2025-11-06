<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdultScreeningEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AdultScreeningEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sgaentryctl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function onGCNutritionDisruptionStatus(s) {
        if (cboGCNutritionDisruptionStatus.GetValue() != null) {
            if (cboGCNutritionDisruptionStatus.GetValue().indexOf('^001') > -1) {
                $('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val('0');
            }
            else if (cboGCNutritionDisruptionStatus.GetValue().indexOf('^002') > -1) {
                $('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val('1');
            }
            else if (cboGCNutritionDisruptionStatus.GetValue().indexOf('^003') > -1) {
                $('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val('2');
            }
            else if (cboGCNutritionDisruptionStatus.GetValue().indexOf('^004') > -1) {
                $('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val('3');
            }
            else {
                $('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val('');
            }
        }
        calculateNutritionalRiskScreeningTotalScore();
    }

    function onGCDiseaseSeverityChanged(s) {
        if (cboGCDiseaseSeverity.GetValue() != null) {
            if (cboGCDiseaseSeverity.GetValue().indexOf('^001') > -1) {
                $('#<%=txtDiseaseSeverity.ClientID %>').val('0');
            }
            else if (cboGCDiseaseSeverity.GetValue().indexOf('^002') > -1) {
                $('#<%=txtDiseaseSeverity.ClientID %>').val('1');
            }
            else if (cboGCDiseaseSeverity.GetValue().indexOf('^003') > -1) {
                $('#<%=txtDiseaseSeverity.ClientID %>').val('2');
            }
            else if (cboGCDiseaseSeverity.GetValue().indexOf('^004') > -1) {
                $('#<%=txtDiseaseSeverity.ClientID %>').val('3');
            }
            else {
                $('#<%=txtDiseaseSeverity.ClientID %>').val('');
            }
        }
        calculateNutritionalRiskScreeningTotalScore();
    }

    $('#<%=rblIsAgeMoreThan70.ClientID %> input').change(function () {
        $('#<%=txtIsAgeMoreThan70.ClientID %>').val($(this).val());
        calculateNutritionalRiskScreeningTotalScore();
    });

    function calculateNutritionalRiskScreeningTotalScore() {
        var p1 = 0;
        var p2 = 0;
        var p3 = 0;

        if ($('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val())
            p1 = parseInt($('#<%=txtNutritionDisruptionStatusScore.ClientID %>').val());

        if ($('#<%=txtDiseaseSeverity.ClientID %>').val())
            p2 = parseInt($('#<%=txtDiseaseSeverity.ClientID %>').val());

        if ($('#<%=txtIsAgeMoreThan70.ClientID %>').val())
            p3 = parseInt($('#<%=txtIsAgeMoreThan70.ClientID %>').val());

        var total = p1 + p2 + p3;
        $('#<%=txtTotalScore.ClientID %>').val(total);
    }

    $('#<%=rblIsBMIChanged.ClientID %> input').change(function () {
        $('#<%=txtIsBMIChanged.ClientID %>').val($(this).val());
    });

    $('#<%=rblIsWeightChanged.ClientID %> input').change(function () {
        $('#<%=txtIsWeightChanged.ClientID %>').val($(this).val());
    });

    $('#<%=rblIsIntakeDecreasing.ClientID %> input').change(function () {
        $('#<%=txtIsIntakeDecreasing.ClientID %>').val($(this).val());
    });

    $('#<%=rblIsSeverelySick.ClientID %> input').change(function () {
        $('#<%=txtIsSeverelySick.ClientID %>').val($(this).val());
    });

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
                        <col style="width: 300px" />
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
                                <%=GetLabel("Skrining Awal :")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Body Mass Index < 20,5 atau LLA < 23,5 cm")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsBMIChanged" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 100px">
                            <asp:TextBox ID="txtIsBMIChanged" runat="server" Width="50%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Berat badan hilang dalam 3 bulan")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsWeightChanged" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 100px">
                            <asp:TextBox ID="txtIsWeightChanged" runat="server" Width="50%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label3">
                                <%=GetLabel("Asupan makan turun dalam seminggu terakhir")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsIntakeDecreasing" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 100px">
                            <asp:TextBox ID="txtIsIntakeDecreasing" runat="server" Width="50%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label4">
                                <%=GetLabel("Menderita sakit berat, misalnya : terapi intensif")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsSeverelySick" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td style="padding-left: 5px; width: 100px">
                            <asp:TextBox ID="txtIsSeverelySick" runat="server" Width="50%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" colspan="2">
                            <label class="lblNormal" id="Label9" style="font-weight: bold">
                                <%=GetLabel("Skrining Lanjut :")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Gangguan Status Gizi") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboGCNutritionDisruptionStatus" ClientInstanceName="cboGCNutritionDisruptionStatus"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCNutritionDisruptionStatus(s); }"
                                    Init="function(s,e){ onGCNutritionDisruptionStatus(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtNutritionDisruptionStatusScore" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Kegawatan Penyakit") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboGCDiseaseSeverity" ClientInstanceName="cboGCDiseaseSeverity"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCDiseaseSeverityChanged(s); }"
                                    Init="function(s,e){ onGCDiseaseSeverityChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtDiseaseSeverity" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Usia > 70 Tahun") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblIsAgeMoreThan70" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtIsAgeMoreThan70" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                        <td style="width: 60px">
                                        </td>
                                        <td style="padding-left: 5px; width: 60px">
                                            <asp:TextBox ID="txtTotalScore" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                        </td>
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
