<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="HealthcareParameterEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.HealthcareParameterEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
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
                var sdFilterExpression = "<%:SearchDialogFilterExpression %>";
                var filterExpression = "<%:SearchDialogCodeField %> = '" + value + "'";
                if (sdFilterExpression != '')
                    filterExpression += " AND " + filterExpression;
                Methods.getObject('<%=SearchDialogMethodName %>', filterExpression, function (result) {
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
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcare" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent">
                 <colgroup>
                    <col style="width:150px"/>
                    <col />
                </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Parameter")%></label></td>
                        <td><asp:TextBox ID="txtParameterCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Parameter")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtParameterName" Width="600px" runat="server" /></td>
                    </tr>
                    <tr class="tdLabel" style="display:none" id="trCboParameterValue" runat="server">
                        <td class="tdLabel"><label class="lblMandatory" id="lblParameterValue"><%=GetLabel("Nilai")%></label></td>
                        <td colspan="2"><dxe:ASPxComboBox ID="cboParameterValue" runat="server" Width="600px" /></td>
                    </tr>
                    <tr id="trTxtParameterValue" runat="server">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nilai")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtParameterValue" Width="300px" runat="server" /></td>
                    </tr>
                    <tr id="trSDParameterValue" runat="server">
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblSDParameter"><%=GetLabel("Nilai")%></label></td>
                        <td colspan="2">
                            <input type="hidden" id="hdnSDParameterID" runat="server" value="" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:0px"/>
                                    <col style="width:0px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtSDParameterCode" Width="100px" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtSDParameterName" Width="492px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Keterangan")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtNotes" Width="600px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                    <tr>
                    <td>&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>