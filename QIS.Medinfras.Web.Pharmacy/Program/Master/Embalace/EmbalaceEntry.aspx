<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="EmbalaceEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.EmbalaceEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtBUD.ClientID %>');
        };

        $('#lblItem.lblLink').live('click', function () {
            var filterExpression = "";
            openSearchDialog('item', filterExpression, function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                ontxtItemCodeChanged(value);
            });
        });

        function ontxtItemCodeChanged(value) {
            var filterExpression = "ItemCode = '" + value + "'";
            Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                }
            });
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnEmbalaceID" runat="server" value="" />
    <input type="hidden" id="hdnItemID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Embalace")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:120px" />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Embalace Code")%></label></td>
            <td><asp:TextBox ID="txtEmbalaceCode" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td><label class="lblMandatory"><%=GetLabel("Embalace Name")%></label></td>
            <td><asp:TextBox ID="txtEmbalaceName" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td><label class="lblNormal"><%=GetLabel("Signa Label")%></label></td>
            <td><dxe:ASPxComboBox ID="cboSignaLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("BUD")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtBUD" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td><label class="lblLink" id="lblItem"><%=GetLabel("Item")%></label></td>
            <td>
                <table cellpadding="0px" cellspacing="0px" width="300px">
                    <colgroup>
                        <col style="width:100px" />
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtItemCode" Width="100%" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td><asp:TextBox ID="txtItemName" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td><label class="lblNormal"><%=GetLabel("Tariff")%></label></td>
            <td><asp:TextBox ID="txtTariff" CssClass="txtCurrency" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:CheckBox runat="server" ID="chkIsUsingRangePrice" Text="Using Range Price" Checked="true" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:CheckBox runat="server" ID="chkIsUnitPrice" Text="Unit Price" /></td>
        </tr>
    </table>
</asp:Content>
