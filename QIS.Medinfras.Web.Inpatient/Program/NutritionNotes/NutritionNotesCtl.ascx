<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.NutritionNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript">
    function onLoad() {
        setDatePicker('<%=txtAssessmentDate.ClientID %>');
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
                        <col style="width: 100px" />
                        <col style="width: 150px" />
                        <col style="width: 100px" />
                        <col style="width: 50px" />
                        <col style="width: 50px" />
                    </colgroup>
                    <tr style="display:none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal - Jam Pengkajian")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessmentTime" Width="100%" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nutritionists") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                        </td>
                        <td class="tdLabel" valign="top" style="padding-left: 50px; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Allow Milk") %></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAllowMilk" runat="server" CssClass="chkIsSelected" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Diet Type") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDietType" Width="100%" runat="server" />
                        </td>
                        <td class="tdLabel" valign="top" style="padding-left: 50px; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Allow Tea") %></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAllowTea" runat="server" CssClass="chkIsSelected" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Meal Form") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMealForm" Width="100%" runat="server" />
                        </td>
                        <td class="tdLabel" valign="top" style="padding-left: 50px; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Allow Coffee") %></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAllowCoffee" runat="server" CssClass="chkIsSelected" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Number of Calories") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoOfCalories" Width="100%" runat="server" />
                        </td>
                        <td class="tdLabel" valign="top" style="padding-left: 50px; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Vegetarian") %></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkVegetarian" runat="server" CssClass="chkIsSelected" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Prohibition Remarks") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProhibitionRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Other Remarks") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtOtherRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
