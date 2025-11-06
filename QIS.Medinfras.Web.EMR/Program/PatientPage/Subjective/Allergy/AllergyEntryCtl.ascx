﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllergyEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.AllergyEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $('#<%=txtAllergenName.ClientID %>').focus();
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
</script>
<div style="height:300px;overflow-y:scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <table style="width:100%" class="tblEntryContent">
                <colgroup>
                    <col style="width:130px"/>
                    <col style="width:130px"/>
                    <col style="width:100px"/>
                    <col style="width:80px"/>
                    <col style="width:80px"/>
                    <col style="width:80px"/>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Log Date")%></label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="txtObservationDate" CssClass="datepicker" Width="130px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Allergen Type")%></label>
                    </td>
                    <td colspan="5">
                        <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Allergen Name")%></label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtAllergenName" Width="295px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Finding Source")%></label>
                    </td>
                    <td colspan="5">
                        <dxe:ASPxComboBox runat="server" ID="cboFindingSource" ClientInstanceName="cboFindingSource" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Since")%></label></td>
                    <td><dxe:ASPxComboBox runat="server" ID="cboYear" ClientInstanceName="cboYear" Width="100%" /></td>
                    <td><dxe:ASPxComboBox runat="server" ID="cboMonth" ClientInstanceName="cboMonth" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Severity")%></label>
                    </td>
                    <td colspan="5">
                        <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Reaction")%></label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtReaction" Width="295px" runat="server" />
                    </td>
                </tr>
            </table>
            </td>
        </tr>
    </table>
</div>