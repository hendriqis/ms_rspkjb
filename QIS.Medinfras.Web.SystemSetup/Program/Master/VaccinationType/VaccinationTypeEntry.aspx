<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="VaccinationTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.VaccinationTypeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=txtDisplayColorPicker.ClientID %>').colorPicker();
            $('#<%=txtDisplayColorPicker.ClientID %>').change(function () {
                $('#<%=txtDisplayColor.ClientID %>').val($(this).val());
            });

            $('#<%=txtDisplayColor.ClientID %>').change(function () {
                $('#<%=txtDisplayColorPicker.ClientID %>').val($(this).val());
                $('#<%=txtDisplayColorPicker.ClientID %>').change();
            });
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Vaccination Type")%></div>
    <table class="tblContentArea" style="width:100%">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Vaccination Code")%></label></td>
                        <td><asp:TextBox ID="txtVaccinationTypeCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Vaccination Name")%></label></td>
                        <td><asp:TextBox ID="txtVaccinationTypeName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ShortName")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Vaccination Group")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboVaccinationGroup" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Display Order")%></label></td>
                        <td><asp:TextBox ID="txtDisplayOrder" CssClass="number" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Display Color")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0"> 
                                <tr>
                                    <td><asp:TextBox ID="txtDisplayColor" CssClass="colorpicker" Width="100px" runat="server" /></td>
                                    <td style="padding-left:5px"><asp:TextBox ID="txtDisplayColorPicker" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
