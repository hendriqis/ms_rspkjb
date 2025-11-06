<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObstetricScreeningEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ObstetricScreeningEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sgaentryctl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

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
                        <col style="width: 800px" />
                        <col style="width: 200px" />
                        <col style="width: 100px" />
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
                            <label class="lblNormal" id="Label5" style="font-weight: bold; padding-top: 10px;">
                                <%=GetLabel("Skrining Gizi Lanjut :")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Apakah asupan makan berkurang karena tidak nafsu makan ?")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsIntakeDecrease" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Ada gangguan merabolisme (DM, gangguan fungsi tiroid, infeksi kronis seperti HIV/AIDS, TB, Lupus, lain-lain")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsMetabolismProblem" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label3">
                                <%=GetLabel("Ada pertambahan BB yang kurang atau lebih selama kehamilan")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsWeightIncrease" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label4">
                                <%=GetLabel("Nilai Hb < 10 g/dl")%></label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsHbLessThan10" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
