<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HealthcareParameter.ascx.cs"
 Inherits="QIS.Medinfras.Web.SystemSetup.Program.ControlPanel.SettingVariables.SettingParameter.HealthcareParameter" %>

 <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_healthcareparamterctl">
$(function () {
    //#region Parameter
    $('#lblSDParameter.lblLink').click(function () {
        var filterExpression = "<%:SearchDialogFilterExpression %>";
        openSearchDialog('<%=SearchDialogType %>', filterExpression, function (value) {
            $('#<%=txtSDParameterCode.ClientID %>').val(value);
            onTxtParameterCodeChanged(value);
        });
    });

    $('#<%=txtSDParameterCode.ClientID %>').change(function () {
        onTxtParameterCodeChanged($(this).val());
    });

    function onTxtParameterCodeChanged(value) {
        var sdfilterExpression = "<%:SearchDialogFilterExpression %>";
        var filterExpression = "<%:SearchDialogCodeField %> = '" + value + "'";
        if (sdfilterExpression != '')
            sdfilterExpression += " AND " + filterExpression;
        Methods.getObject('<%=SearchDialogMethodName %>', sdfilterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSDParameterID.ClientID %>').val(result['<%=SearchDialogIDField %>']);
                $('#<%=txtSDParameterName.ClientID %>').val(result['<%=SearchDialogNameField %>']);
            }
            else {
                $('#<%=hdnSDParameterID.ClientID %>').val('');
                $('#<%=txtSDParameterCode.ClientID %>').val('');
                $('#<%=txtSDParameterName.ClientID %>').val('');
            }
        });
    }
    //#endregion
});
</script>

<input type="hidden" id="hdnHealthcareID" value="" runat="server" />
<input type="hidden" id="hdnParameterCode" value="" runat="server" />
<input type="hidden" id="hdnTempType" value="" runat="server" />
<table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rumah Sakit")%></label></td>
                        <td><asp:TextBox ID="txtHealthcareName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Parameter")%></label></td>
                        <td><asp:TextBox ID="txtParameterCode" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Parameter")%></label></td>
                        <td><asp:TextBox ID="txtParameterName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr class="tdLabel" style="display:none" id="trCboParameterValue" runat="server">
                        <td class="tdLabel"><label class="lblMandatory" id="lblParameterValue"><%=GetLabel("Nilai")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboParameterValue" runat="server" Width="300px" /> </td>
                    </tr>
                    <tr id="trTxtParameterValue" runat="server">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nilai")%></label></td>
                        <td><asp:TextBox ID="txtParameterValue" Width="300px" runat="server" /></td>
                    </tr>
                    <tr id="trSDParameterValue" runat="server">
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblSDParameter"><%=GetLabel("Nilai")%></label></td>
                        <td>
                            <input type="hidden" id="hdnSDParameterID" runat="server" value="" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtSDParameterCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtSDParameterName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>