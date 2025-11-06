<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    CodeBehind="AccountDisplaySettingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.AccountDisplaySettingEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onCboTableNameValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnTableName.ClientID %>').val(value);
            cboFieldName.PerformCallback();
        }

        function onCboFieldNameEndCallBack(s) {
            onCboFieldNameValueChanged(s);
        }

        function onCboFieldNameValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnFieldName.ClientID %>').val(value);
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnTableName" runat="server" value="" />
    <input type="hidden" id="hdnFieldName" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Table Name")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTableName" ClientInstanceName="cboTableName" Width="300px"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboTableNameValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Field Name")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFieldName" ClientInstanceName="cboFieldName" Width="300px"
                                runat="server" OnCallback="cboFieldName_Callback">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboFieldNameValueChanged(s); }"
                                    EndCallback="function(s,e){ onCboFieldNameEndCallBack(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Display Order")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboDisplayOrder" ClientInstanceName="cboDisplayOrder"
                                Width="100px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
