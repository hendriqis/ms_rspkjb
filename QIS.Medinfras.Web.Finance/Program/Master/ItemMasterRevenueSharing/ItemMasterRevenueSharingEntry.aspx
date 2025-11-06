<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="ItemMasterRevenueSharingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ItemMasterRevenueSharingEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        //#region Item
        function onGetItemMasterCodeFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND GCItemType = '" + cboItemType.GetValue() + "'";
            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('item', onGetItemMasterCodeFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemMasterCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemMasterCodeChanged($(this).val());
        });

        function onTxtItemMasterCodeChanged(value) {
            var filterExpression = onGetItemMasterCodeFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
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
        //#endregion

        //#region Revenue Sharing
        function onGetRevenueSharingMasterCodeFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblRevenueSharing.lblLink').live('click', function () {
            openSearchDialog('revenuesharing', onGetRevenueSharingMasterCodeFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                onTxtRevenueSharingMasterCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            onTxtRevenueSharingMasterCodeChanged($(this).val());
        });

        function onTxtRevenueSharingMasterCodeChanged(value) {
            var filterExpression = onGetRevenueSharingMasterCodeFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        }
        //#endregion

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100px" />
            <col style="width: 500px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <label class="lblMandatory" id="lblItemType">
                    <%=GetLabel("Tipe Item")%></label>
            </td>
            <td colspan="2">
                <table style="width: 50%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                    </colgroup>
                    <tr>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" runat="server"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblMandatory lblLink" id="lblItem">
                    <%=GetLabel("Item")%></label>
            </td>
            <td colspan="2">
                <input type="hidden" id="hdnItemID" runat="server" value="" />
                <table style="width: 50%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblMandatory" id="lblCustomerType">
                    <%=GetLabel("Tipe Pembayar")%></label>
            </td>
            <td colspan="2">
                <table style="width: 50%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                    </colgroup>
                    <tr>
                        <td>
                            <dxe:ASPxComboBox ID="cboCustomerType" ClientInstanceName="cboCustomerType" runat="server"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblMandatory lblLink" id="lblRevenueSharing">
                    <%=GetLabel("Jasa Medis")%></label>
            </td>
            <td colspan="2">
                <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
                <table style="width: 50%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtRevenueSharingCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
