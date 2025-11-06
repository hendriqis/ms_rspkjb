<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="TemplatePanelEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplatePanelEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        //#region Healthcare Service Unit
        function getServiceUnitFilterFilterExpression() {
            var filterExpression = "DepartmentID = 'DIAGNOSTIC' AND IsUsingRegistration = 1 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblServiceUnit.ClientID %>.lblLink').live('click', function () {
            var parameter = getServiceUnitFilterFilterExpression();
            openSearchDialog('serviceunitperhealthcare', parameter, function (value) {
                $('#<%:txtServiceUnitCode.ClientID %>').val(value);
                onTxtClinicCodeChanged(value);
            });
        });

        $('#<%:txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtClinicCodeChanged($(this).val());
        });

        function onTxtClinicCodeChanged(value) {
            var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

                }
                else {
                    $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%:txtServiceUnitCode.ClientID %>').val('');
                    $('#<%:txtServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    </script>

    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnTemplateID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Item")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboItemType" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                            <label class="lblLink" runat="server" id="lblServiceUnit">
                                <%=GetLabel("Unit Penunjang")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
