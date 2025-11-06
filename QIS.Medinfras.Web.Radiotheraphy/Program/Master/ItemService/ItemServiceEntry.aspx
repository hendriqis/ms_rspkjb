<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ItemServiceEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.ItemServiceEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Item Group
            $('#lblItemGroup.lblLink').click(function () {
                var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                openSearchDialog('itemgroup', filterExpression, function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                });
            }

            //#region Product Line
            $('#lblProductLine.lblLink').click(function () {
                openSearchDialog('productline', 'IsDeleted = 0', function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = "ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
            //#region Revenue Sharing
            $('#lblRevenueSharing.lblLink').click(function () {
                openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
                    $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                    onTxtRevenueSharingCodeChanged(value);
                });
            });

            $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
                onTxtRevenueSharingCodeChanged($(this).val());
            });

            function onTxtRevenueSharingCodeChanged(value) {
                var filterExpression = "RevenueSharingCode = '" + value + "'";
                Methods.getObject('GetRevenueSharingList', filterExpression, function (result) {
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

            registerCollapseExpandHandler();
        }
    </script>
 <input type="hidden" id="hdnID" runat="server" value="" />
 <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Item Service")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <h4 class="h4expanded"><%=GetLabel("Item Information")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Item Code")%></label></td>
                            <td><asp:TextBox ID="txtItemCode" Width="200px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Item Name 1")%></label></td>
                            <td><asp:TextBox ID="txtItemName1" Width="300px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item Name 2")%></label></td>
                            <td><asp:TextBox ID="txtItemName2" Width="300px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Item Unit")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboItemUnit" runat="server" Width="200px" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblItemGroup"><%=GetLabel("Item Group")%></label></td>
                            <td>
                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblProductLine"><%=GetLabel("Product Line")%></label></td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtProductLineName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblRevenueSharing"><%=GetLabel("Revenue Sharing")%></label></td>
                            <td>
                                <input type="hidden" id="hdnRevenueSharingID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtRevenueSharingName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Remarks")%></label></td>
                            <td><asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded"><%=GetLabel("Custom Attribute")%></h4>
                    <asp:Repeater ID="rptCustomAttribute" runat="server">
                        <HeaderTemplate>
                            <div class="containerTblEntryContent">
                            <table class="tblEntryContent" style="width:100%">
                                <colgroup>
                                    <col style="width:30%"/>
                                </colgroup>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%#: Eval("Value") %></label></td>
                                <td>
                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnTagFieldCode" />
                                    <asp:TextBox ID="txtTagField" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
            <td style="padding:5px;vertical-align:top">
                <h4 class="h4expanded"><%=GetLabel("Item Status")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <tr><td><asp:CheckBox ID="chkIsAllowVariable" runat="server" /> <%=GetLabel("Allow Variable Price")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsAllowCITO" runat="server" /> <%=GetLabel("Allow Cito")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsAllowComplication" runat="server" /> <%=GetLabel("Allow Complication")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsAllowDiscount" runat="server" /> <%=GetLabel("Allow Discount")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsUnbilledItem" runat="server" /> <%=GetLabel("Allow Unbilled Item")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsSubContractItem" runat="server" /> <%=GetLabel("Sub Contract Item")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsPrintWithDoctorName" runat="server" /> <%=GetLabel("Print With Doctor Name")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsAssetUtilization" runat="server" /> <%=GetLabel("Asset Utilization")%></td></tr>
                        <tr><td><asp:CheckBox ID="chkIsIncludeInAdminCalculation" runat="server" /> <%=GetLabel("Include in Admin Calculation")%></td></tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
