<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="SupplierEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.SupplierEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Province
            function onGetSCProvinceFilterExpression() {
                var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
                return filterExpression;
            }

            $('#lblProvince.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
                    $('#<%=txtProvinceCode.ClientID %>').val(value);
                    onTxtProvinceCodeChanged(value);
                });
            });

            $('#<%=txtProvinceCode.ClientID %>').change(function () {
                onTxtProvinceCodeChanged($(this).val());
            });

            function onTxtProvinceCodeChanged(value) {
                var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtProvinceName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Zip Code
            $('#lblZipCode.lblLink').click(function () {
                openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                    $('#<%=txtZipCode.ClientID %>').val(value);
                    onTxtZipCodeChanged(value);
                });
            });

            $('#<%=txtZipCode.ClientID %>').change(function () {
                onTxtZipCodeChanged($(this).val());
            });

            function onTxtZipCodeChanged(value) {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetZipCodesList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            registerCollapseExpandHandler();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Supplier")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <h4 class="h4expanded"><%=GetLabel("General Information")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Supplier Code")%></label></td>
                            <td><asp:TextBox ID="txtSupplierCode" Width="200px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Supplier Name")%></label></td>
                            <td><asp:TextBox ID="txtSupplierName" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Short Name")%></label></td>
                            <td><asp:TextBox ID="txtShortName" Width="200px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Supplier Type")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboSupplierType" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Contact Person")%></label></td>
                            <td><asp:TextBox ID="txtContactPerson" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded"><%=GetLabel("Supplier Information")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Healthcare")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboHealthcare" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("VAT Registration No")%></label></td>
                            <td><asp:TextBox ID="txtVATRegistrationNo" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Term")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboTerm" Width="100%" runat="server" /></td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Max PO Amount")%></label></td>
                            <td><asp:TextBox ID="txtMaxPOAmount" Width="100%" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Lead Time")%></label></td>
                            <td><asp:TextBox ID="txtLeadTime" CssClass="number" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Order Completion")%></label></td>
                            <td><asp:TextBox ID="txtOrderPerformance" CssClass="number" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Ontime Delivery")%></label></td>
                            <td><asp:TextBox ID="txtTimePerformance" CssClass="number" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded"><%=GetLabel("Supplier Status")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:10px"/>
                        </colgroup>
                        <tr>
                            <td><asp:CheckBox ID="chkIsTaxable" Width="100%" runat="server" /></td>
                            <td><%=GetLabel("Taxable")%></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkIsBlacklist" Width="100%" runat="server" /></td>
                            <td><%=GetLabel("Blacklist")%></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkIsPaymentHold" Width="100%" runat="server" /></td>
                            <td><%=GetLabel("Payment Hold")%></td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top">
                <h4 class="h4expanded"><%=GetLabel("Address")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Address")%></label></td>
                            <td><asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("County")%></label></td>
                            <td><asp:TextBox ID="txtCounty" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("District")%></label></td>
                            <td><asp:TextBox ID="txtDistrict" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("City")%></label></td>
                            <td><asp:TextBox ID="txtCity" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblProvince"><%=GetLabel("Province")%></label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtProvinceCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtProvinceName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblZipCode"><%=GetLabel("ZipCode")%></label></td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCode" value="" />
                                <asp:TextBox ID="txtZipCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Telephone")%></label></td>
                            <td><asp:TextBox ID="txtTelephoneNo" Width="100%" runat="server" /></td>
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
