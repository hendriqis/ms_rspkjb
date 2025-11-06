<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ItemProductEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ItemProductEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region ATC Class
            $('#lblATCClass.lblLink').click(function () {
                openSearchDialog('atcclass', 'IsDeleted = 0', function (value) {
                    $('#<%=txtATCClassCode.ClientID %>').val(value);
                    onTxtATCClassCodeChanged(value);
                });
            });

            $('#<%=txtATCClassCode.ClientID %>').change(function () {
                onTxtATCClassCodeChanged($(this).val());
            });

            function onTxtATCClassCodeChanged(value) {
                var filterExpression = "ATCClassCode = '" + value + "'";
                Methods.getObject('GetATCClassificationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnATCClassID.ClientID %>').val(result.ATCClassID);
                        $('#<%=txtATCClassName.ClientID %>').val(result.ATCClassName);
                    }
                    else {
                        $('#<%=hdnATCClassID.ClientID %>').val('');
                        $('#<%=txtATCClassCode.ClientID %>').val('');
                        $('#<%=txtATCClassName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
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
            //#endregion        
            //#region MIMS Class
            $('#lblMIMSClass.lblLink').click(function () {
                openSearchDialog('mimsclass', 'IsDeleted = 0', function (value) {
                    $('#<%=txtMIMSClassCode.ClientID %>').val(value);
                    onTxtMIMSClassCodeChanged(value);
                });
            });

            $('#<%=txtMIMSClassCode.ClientID %>').change(function () {
                onTxtMIMSClassCodeChanged($(this).val());
            });

            function onTxtMIMSClassCodeChanged(value) {
                var filterExpression = "MIMSClassCode = '" + value + "'";
                Methods.getObject('GetMIMSClassList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMIMSClassID.ClientID %>').val(result.MIMSClassID);
                        $('#<%=txtMIMSClassName.ClientID %>').val(result.MIMSClassName);
                    }
                    else {
                        $('#<%=hdnMIMSClassID.ClientID %>').val('');
                        $('#<%=txtMIMSClassCode.ClientID %>').val('');
                        $('#<%=txtMIMSClassName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
            //#region Product Brand
            $('#lblProductBrand.lblLink').click(function () {
                openSearchDialog('productbrand', 'IsDeleted = 0', function (value) {
                    $('#<%=txtProductBrandCode.ClientID %>').val(value);
                    onTxtProductBrandCodeChanged(value);
                });
            });

            $('#<%=txtProductBrandCode.ClientID %>').change(function () {
                onTxtProductBrandCodeChanged($(this).val());
            });

            function onTxtProductBrandCodeChanged(value) {
                var filterExpression = "ProductBrandCode = '" + value + "'";
                Methods.getObject('GetProductBrandList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductBrandID.ClientID %>').val(result.ProductBrandID);
                        $('#<%=txtProductBrandName.ClientID %>').val(result.ProductBrandName);
                    }
                    else {
                        $('#<%=hdnProductBrandID.ClientID %>').val('');
                        $('#<%=txtProductBrandCode.ClientID %>').val('');
                        $('#<%=txtProductBrandName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
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

            registerCollapseExpandHandler();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />    
    <div class="pageTitle"><%=GetPageTitle()%></div>
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
                            <td><asp:TextBox ID="txtItemName1" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item Name 2")%></label></td>
                            <td><asp:TextBox ID="txtItemName2" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblProductBrand"><%=GetLabel("Brand Name")%></label></td>
                            <td>
                                <input type="hidden" id="hdnProductBrandID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtProductBrandCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtProductBrandName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
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
                    </table>
                </div>
                <asp:Panel ID="pnlDrugInformation" runat="server">
                    <h4 class="h4expanded"><%=GetLabel("Drug Information")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width:100%">
                            <colgroup>
                                <col style="width:30%"/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Generic Name")%></label></td>
                                <td><asp:TextBox ID="txtGenericName" Width="100%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dose")%></label></td>
                                <td>
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:50%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtDose" CssClass="number required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><dxe:ASPxComboBox ID="cboDose" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Drug Form")%></label></td>
                                <td><dxe:ASPxComboBox ID="cboDrugForm" runat="server" Width="300px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblMIMSClass"><%=GetLabel("MIMS Class")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnMIMSClassID" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtMIMSClassCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtMIMSClassName" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblATCClass"><%=GetLabel("ATC Class")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnATCClassID" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtATCClassCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtATCClassName" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Drug Classification")%></label></td>
                                <td><dxe:ASPxComboBox ID="cboDrugClassification" runat="server" Width="100%" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Pregnancy Category")%></label></td>
                                <td><dxe:ASPxComboBox ID="cboPregnancyCategory" runat="server" Width="100%" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Generic")%></label></td>
                                <td><asp:CheckBox ID="chkIsGeneric" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Formularium")%></label></td>
                                <td><asp:CheckBox ID="chkIsFormularium" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <h4 class="h4expanded"><%=GetLabel("Inventory Information")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("ABC Class")%></label></td>
                            <td>
                                <asp:RadioButtonList ID="rblABCClass" runat="server" RepeatDirection="Horizontal" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Unit Of Measure")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboItemUnit" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Cycle Count Interval")%></label></td>
                            <td><asp:TextBox ID="txtCountInterval" Width="200px" CssClass="number required" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Transaction Restriction")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboTransactionRestriction" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Batch Control")%></label></td>
                            <td><asp:CheckBox ID="chkIsBatchControl" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Inventory Item")%></label></td>
                            <td><asp:CheckBox ID="chkIsInventoryItem" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Production Item")%></label></td>
                            <td><asp:CheckBox ID="chkIsProductionItem" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top">
                <h4 class="h4expanded"><%=GetLabel("Finance Information")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
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
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Suggested Detail Price")%></label></td>
                            <td><asp:TextBox ID="txtSuggestedPrice" CssClass="txtCurrency" runat="server" Width="200px" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Markup")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboMarkup" runat="server" Width="100%" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Margin")%> (%)</label></td>
                            <td><asp:TextBox ID="txtMargin" CssClass="number" runat="server" Width="200px" /></td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded"><%=GetLabel("Other Information")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Notes")%></label></td>
                            <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
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
        </tr>
    </table>
</asp:Content>
