<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MSTFormEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MSTFormEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_MSTFormEntryCtl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function onGCWeightChangedStatus(s) {
        if (cboGCWeightChangedStatus.GetValue() != null) {
            if (cboGCWeightChangedStatus.GetValue().indexOf('^01') > -1) {
                $('#<%=txtWeightChangedStatusScore.ClientID %>').val('0');
            }
            else if (cboGCWeightChangedStatus.GetValue().indexOf('^02') > -1) {
                $('#<%=txtWeightChangedStatusScore.ClientID %>').val('2');
            }
            else {
                $('#<%=txtWeightChangedStatusScore.ClientID %>').val('');
            }
        }
        calculateMSTScore();
    }
    function onGCMSTDiagnosisChanged(s) {
        $txt = $('#<%=txtOtherMSTDiagnosis.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^99') > -1)
            $txt.show();
        else
            $txt.hide();
    }
    function onGCWeightChangedGroup(s) {
        if (cboGCWeightChangedGroup.GetValue() != null) {
            var param1 = cboGCWeightChangedGroup.GetValue().split('^');
            $('#<%=txtWeightChangedGroupScore.ClientID %>').val(parseInt(param1[1]));
        }
        else {
            $('#<%=txtWeightChangedGroupScore.ClientID %>').val('');
        }
        calculateMSTScore();
    }
    function onGCMSTDiagnosisChanged(s) {
        $txt = $('#<%=txtOtherMSTDiagnosis.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^99') > -1)
            $txt.show();
        else
            $txt.hide();
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

    $('#<%=rblIsReducedFoodIntake.ClientID %> input').change(function () {
        $('#<%=txtFoodIntakeScore.ClientID %>').val($(this).val());
        calculateMSTScore();
    });

    function SetTotalScore(param) {
        $('#<%=txtTotalMST.ClientID %>').val(param);
    }

</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnEM0113" value="" />
    <div class="containerTblEntryContent">
        <table class="tblContentArea" border="0" cellpadding="1" cellspacing="0" width="100%"
            style="margin-top: 5px">
            <colgroup>
                <col width="210px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <%=GetLabel("Tanggal - Jam ")%>
                </td>
                <td>
                    <asp:TextBox ID="txtAssessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtAssessmentTime" Width="120px" CssClass="time" runat="server"
                        Style="text-align: center" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("PPA")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboParamedicID" Width="360px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                    <%=GetLabel("Apakah Pasien mengalami penurunan berat badan dalam waktu 6 bulan terakhir ?")%>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboGCWeightChangedStatus" ClientInstanceName="cboGCWeightChangedStatus"
                        Width="210px">
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCWeightChangedStatus(s); }"
                            Init="function(s,e){ onGCWeightChangedStatus(s); }" />
                    </dxe:ASPxComboBox>
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
                    <%=GetLabel("Apakah Pasien mengalami penurunan berat badan?")%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblIsHasWeightLoss" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Ya" Value="1" />
                        <asp:ListItem Text="Tidak" Value="0" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                    <%=GetLabel("Jika Ya, berapa penurunan berat badan tersebut ?") %>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboGCWeightChangedGroup" ClientInstanceName="cboGCWeightChangedGroup"
                        Width="210px">
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCWeightChangedGroup(s); }"
                            Init="function(s,e){ onGCWeightChangedGroup(s); }" />
                    </dxe:ASPxComboBox>
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
                    <asp:RadioButtonList ID="rblIsReducedFoodIntake" runat="server" RepeatDirection="Horizontal">
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
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblIsHasSpecificDiagnosis" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Ya" Value="1" />
                        <asp:ListItem Text="Tidak" Value="0" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboGCMSTDiagnosis" ClientInstanceName="cboGCMSTDiagnosis"
                        Width="100%">
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onGCMSTDiagnosisChanged(s); }"
                            Init="function(s,e){ onGCMSTDiagnosisChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <asp:TextBox ID="txtOtherMSTDiagnosis" runat="server" Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" valign="top" style="padding-top: 5px;" colspan="2">
                    <%=GetLabel("Apakah Sudah dibaca dan diketahui oleh tenaga gizi?")%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblIsReadedByNutritionist" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Ya" Value="1" />
                        <asp:ListItem Text="Tidak" Value="0" />
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div>
</div>
