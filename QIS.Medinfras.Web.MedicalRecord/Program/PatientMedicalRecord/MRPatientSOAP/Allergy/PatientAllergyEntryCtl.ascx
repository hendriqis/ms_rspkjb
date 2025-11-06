<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientAllergyEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientAllergyEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 170px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Alergi Sejak")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Alergi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType"
                                Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Alergi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAllergenName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Sumber Informasi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboFindingSource" ClientInstanceName="cboFindingSource"
                                Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tingkat Alergi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity"
                                Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Reaksi Alergi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReaction" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
