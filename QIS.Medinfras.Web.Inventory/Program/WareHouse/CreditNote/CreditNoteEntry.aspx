<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="CreditNoteEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CreditNoteEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=txtCreditNoteDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtCreditNoteDate.ClientID %>');
                $('#<%=txtCreditNoteDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            }

            //#region Credit Note No
            $('#lblCreditNoteNo.lblLink').click(function () {
                openSearchDialog('suppliercreditnote', '', function (value) {
                    $('#<%=txtCreditNoteNo.ClientID %>').val(value);
                    onTxtCreditNoteNoChanged(value);
                });
            });

            $('#<%=txtCreditNoteNo.ClientID %>').change(function () {
                onTxtCreditNoteNoChanged($(this).val());
            });

            function onTxtCreditNoteNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Supplier
            function getSupplierFilterExpression() {
                var filterExpression = "<%:GetSupplierFilterExpression() %>";
                return filterExpression;
            }

            $('#<%=lblSupplier.ClientID %>.lblLink').click(function () {
                openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                    $('#<%=txtSupplierCode.ClientID %>').val(value);
                    onTxtSupplierChanged(value);
                });
            });

            $('#<%=txtSupplierCode.ClientID %>').change(function () {
                onTxtSupplierChanged($(this).val());
            });

            function onTxtSupplierChanged(value) {
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val('');
                        $('#<%=txtPurchaseReturnNo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Product Line
            function getProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblProductLine.ClientID %>.lblLink').click(function () {
                openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnProductLineItemType.ClientID %>').val(result.GCItemType);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                        $('#<%=hdnProductLineItemType.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Purchase Return
            function getPurchaseReturnFilterExpression() {
                var filterExpression = "<%:GetPurchaseReturnFilterExpression() %>";
                var supplierID = $('#<%=hdnSupplierID.ClientID %>').val();
                if (supplierID != '')
                    filterExpression += " AND BusinessPartnerID = " + $('#<%=hdnSupplierID.ClientID %>').val();
                return filterExpression;
            }

            $('#<%=lblPurchaseReturn.ClientID %>.lblLink').click(function () {
                openSearchDialog('purchasereturnhd', getPurchaseReturnFilterExpression(), function (value) {
                    $('#<%=txtPurchaseReturnNo.ClientID %>').val(value);
                    onTxtPurchaseReturnChanged(value);
                });
            });

            $('#<%=txtPurchaseReturnNo.ClientID %>').change(function () {
                onTxtPurchaseReturnChanged($(this).val());
            });

            function onTxtPurchaseReturnChanged(value) {
                var filterExpression = getPurchaseReturnFilterExpression() + " AND PurchaseReturnNo = '" + value + "'";
                Methods.getObject('GetvPurchaseReturnHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val(result.PurchaseReturnID);
                        $('#<%=txtCNAmount.ClientID %>').val(result.TransactionAmount).trigger('changeValue');
                        if ($('#<%=hdnSupplierID.ClientID %>').val() == '') {
                            $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                            $('#<%=txtSupplierCode.ClientID %>').val(result.BusinessPartnerCode);
                            $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                        }
                        $('#<%=chkPPN.ClientID %>').prop('checked', result.IsIncludeVAT);
                        $('#<%=txtVATPercentage.ClientID %>').val(result.VATPercentage).trigger('changeValue');
                        $('#<%=chkIsReturnCostInPct.ClientID %>').prop('checked', result.IsIncludeVAT);
                        $('#<%=txtReturnCostPercentage.ClientID %>').val(result.ReturnCostPercentage).trigger('changeValue');
                        $('#<%=txtReturnCostAmount.ClientID %>').val(result.ReturnCostAmount).trigger('changeValue');
                    }
                    else {
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val('');
                        $('#<%=txtPurchaseReturnNo.ClientID %>').val('');
                        $('#<%=txtCNAmount.ClientID %>').val('0').trigger('changeValue');
                        $('#<%=chkPPN.ClientID %>').prop('checked', false);
                        $('#<%=txtVATPercentage.ClientID %>').val('0').trigger('changeValue');
                        $('#<%=chkIsReturnCostInPct.ClientID %>').prop('checked', false);
                        $('#<%=txtReturnCostPercentage.ClientID %>').val('0').trigger('changeValue');
                        $('#<%=txtReturnCostAmount.ClientID %>').val('0').trigger('changeValue');
                    }
                });
            }
            //#endregion

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var creditNoteID = $('#<%=hdnCreditNoteID.ClientID %>').val();
            if (creditNoteID == '' || creditNoteID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "CreditNoteID = " + creditNoteID;
                return true;
            }
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <input type="hidden" id="hdnCreditNoteID" value="0" runat="server" />
                <input type="hidden" id="hdnVATPercentage" value="0" runat="server" />
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 170px" />
                        <col style="width: 30px" />
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblCreditNoteNo">
                                <%=GetLabel("No Nota Kredit")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCreditNoteNo" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCreditNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                            <label class="lblMandatory lblLink" id="lblSupplier" runat="server">
                                <%=GetLabel("Supplier/Penyedia")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trProductLine" runat="server" style="display: none">
                        <td>
                            <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                            <input type="hidden" id="hdnProductLineItemType" value="" runat="server" />
                            <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                <%=GetLabel("Product Line")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" runat="server" id="lblPurchaseReturn">
                                <%=GetLabel("No. Pengembalian")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" runat="server" id="hdnPurchaseReturnID" value="" />
                            <asp:TextBox ID="txtPurchaseReturnNo" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("No Faktur Pajak")%>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTaxInvoiceNo" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal Faktur Pajak") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTaxInvoiceDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Nota Kredit")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboGCCreditNoteType" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Total (Sebelum PPN)")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtCNAmount" Width="100%" CssClass="txtCurrency" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("PPN")%></label>
                        </td>
                        <td align="right">
                            <asp:CheckBox ID="chkPPN" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVATPercentage" Width="80%" CssClass="txtCurrency" runat="server" /><label>%</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Return Cost")%></label>
                        </td>
                        <td align="right">
                            <asp:CheckBox ID="chkIsReturnCostInPct" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReturnCostPercentage" Width="80%" CssClass="txtCurrency" runat="server" /><label>%</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReturnCostAmount" Width="100%" CssClass="txtCurrency" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 600px;">
                                    <div class="pageTitle" style="text-align: center">
                                        <%=GetLabel("Informasi")%></div>
                                    <div style="background-color: #EAEAEA;">
                                        <table width="600px" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="30px" />
                                            </colgroup>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Pada") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Pada")%>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
