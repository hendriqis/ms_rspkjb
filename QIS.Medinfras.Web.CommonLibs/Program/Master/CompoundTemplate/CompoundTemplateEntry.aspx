<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" Debug="true" CodeBehind="CompoundTemplateEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CompoundTemplateEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxis_prescriptioncompoundentryctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
    <script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
        $(function () {
            $('#<%=txtTemplateCode.ClientID %>').focus();
        });

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

        $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            calculateDispenseQty();
        });

        $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
            var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
            $('#<%=txtEmbalaceQty.ClientID %>').val(dispQty);
        });

        function calculateDispenseQty() {
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
            var dose = $('#<%=txtDosingDose.ClientID %>').val();
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();

            var dispenseQty = dosingDuration * frequency * dose;
            $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
            $('#<%=txtDispenseQty.ClientID %>').change();
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnItemID" runat="server" value="" />
    <input type="hidden" id="hdnModuleID" runat="server" value="" />
    <input type="hidden" value="" id="hdnInlineEditingData" runat="server" />
    <input type="hidden" value="" id="hdnQueryString" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col width="80px" />
                        <col width="40px" />
                        <col width="60px" />
                        <col width="40px" />
                        <col width="50px" />
                        <col width="20px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Template")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTemplateCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Template")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtTemplateName" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Signa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFrequencyTimeline" runat="server" Width="100%" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDosingUnit" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Duration (day)")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                        </td>
                        <td class="tdLabel" style="text-align: center">
                            <label class="lblMandatory">
                                <%=GetLabel("Quantity")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel("Sweetener")%>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsAsRequired" /><%=GetLabel("PRN")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Embalace")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboEmbalace" Width="100%" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Quantity")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmbalaceQty" Width="100%" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Medication Route")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" Width="100%" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("AC/DC/PC")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" Width="100%" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 170px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 2px;">
                            <label class="lblNormal">
                                <%=GetLabel("Administration Instruction")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationAdministration" Width="350px" runat="server" TextMode="MultiLine"
                                Height="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 2px;">
                            <label class="lblNormal">
                                <%=GetLabel("Medication Purpose")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationPurpose" Width="350px" runat="server" TextMode="MultiLine"
                                Height="80px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
