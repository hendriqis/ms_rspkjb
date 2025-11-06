<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="IPDefaultConfigurationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.IPDefaultConfigurationEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
<script type="text/javascript">

    function onCboDepartmentValueChanged(evt) {
        var cboValue = cboDepartment.GetValue();

        $('#<%=hdnServiceUnitID.ClientID %>').val('');
        $('#<%=txtServiceUnitCode.ClientID %>').val('');
        $('#<%=txtServiceUnitName.ClientID %>').val('');
    }

    function onCboServiceUnitValueChanged(evt) {
        var cboValue = cboServiceUnit.GetValue();

        $('#<%=hdnGCCashier.ClientID %>').val('');
    }

    //#region Service Unit
    $('#lblServiceUnit.lblLink').live('click', function () {
        var DepartmentID = cboDepartment.GetValue();
        var filterExpression = '';
        if (DepartmentID != '')
            filterExpression = "DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0";
        openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            ontxtServiceUnitCodeChanged(value);
        });
    });
    $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
        ontxtServiceUnitCodeChanged($(this).val());
    });
    function ontxtServiceUnitCodeChanged(value) {
        var filterExpression = "ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%=hdnServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }
        });
    }

</script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcare" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 25%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("IP Address")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIPAddress" Width="130px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Departemen")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="300px"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblServiceUnit">
                                <%=GetLabel("Unit")%></label>
                         </td>
                         <td>
                            <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:20%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="350px" runat="server" /></td>
                                        </tr>
                                    </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                            <input type="hidden" id="hdnGCCashier" runat="server" value="" />
                                <%=GetLabel("Kelompok Kasir")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboCashierGroup" runat="server" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Lokasi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboLocation" runat="server" Width="300px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
