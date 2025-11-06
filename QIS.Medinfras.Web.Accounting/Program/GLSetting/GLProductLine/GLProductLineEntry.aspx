<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLProductLineEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLProductLineEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            function onGetGLAccountFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

            //#region Inventory
            $('#lblInventory.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtInventoryGLAccountNo.ClientID %>').val(value);
                    onTxtInventoryGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtInventoryGLAccountNo.ClientID %>').change(function () {
                onTxtInventoryGLAccountCodeChanged($(this).val());
            });

            function onTxtInventoryGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnInventoryID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtInventoryGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnInventorySubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnInventorySearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnInventoryIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnInventoryCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnInventoryDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnInventoryMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnInventoryFilterExpression.ClientID %>').val(result.FilterExpression);
                        onInventorySubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnInventoryID.ClientID %>').val('');
                        $('#<%=txtInventoryGLAccountName.ClientID %>').val('');
                        $('#<%=hdnInventorySubLedgerID.ClientID %>').val('');
                        $('#<%=hdnInventorySearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnInventoryIDFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryMethodName.ClientID %>').val('');
                        $('#<%=hdnInventoryFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnInventorySubLedger.ClientID %>').val('');
                    $('#<%=txtInventorySubLedgerCode.ClientID %>').val('');
                    $('#<%=txtInventorySubLedgerName.ClientID %>').val('');
                });
            }

            function onInventorySubLedgerIDChanged() {
                if ($('#<%=hdnInventorySubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnInventorySubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblInventorySubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtInventorySubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblInventorySubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtInventorySubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Inventory Sub Ledger
            function onGetInventorySubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnInventoryFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnInventorySubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblInventorySubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnInventorySearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnInventorySearchDialogTypeName.ClientID %>').val(), onGetInventorySubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtInventorySubLedgerCode.ClientID %>').val(value);
                        onTxtInventorySubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtInventorySubLedgerCode.ClientID %>').change(function () {
                onTxtInventorySubLedgerCodeChanged($(this).val());
            });

            function onTxtInventorySubLedgerCodeChanged(value) {
                if ($('#<%=hdnInventorySearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetInventorySubLedgerDtFilterExpression() + " AND " + $('#<%=hdnInventoryCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnInventoryMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnInventorySubLedger.ClientID %>').val(result[$('#<%=hdnInventoryIDFieldName.ClientID %>').val()]);
                            $('#<%=txtInventorySubLedgerName.ClientID %>').val(result[$('#<%=hdnInventoryDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnInventorySubLedger.ClientID %>').val('');
                            $('#<%=txtInventorySubLedgerCode.ClientID %>').val('');
                            $('#<%=txtInventorySubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Inventory Discount
            $('#lblInventoryDiscount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtInventoryDiscountGLAccountNo.ClientID %>').val(value);
                    onTxtInventoryDiscountGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtInventoryDiscountGLAccountNo.ClientID %>').change(function () {
                onTxtInventoryDiscountGLAccountCodeChanged($(this).val());
            });

            function onTxtInventoryDiscountGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnInventoryDiscountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtInventoryDiscountGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnInventoryDiscountSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnInventoryDiscountSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnInventoryDiscountIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnInventoryDiscountCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnInventoryDiscountDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnInventoryDiscountMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnInventoryDiscountFilterExpression.ClientID %>').val(result.FilterExpression);
                        onInventoryDiscountSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnInventoryDiscountID.ClientID %>').val('');
                        $('#<%=txtInventoryDiscountGLAccountName.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountIDFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountMethodName.ClientID %>').val('');
                        $('#<%=hdnInventoryDiscountFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnInventoryDiscountSubLedger.ClientID %>').val('');
                    $('#<%=txtInventoryDiscountSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtInventoryDiscountSubLedgerName.ClientID %>').val('');
                });
            }

            function onInventoryDiscountSubLedgerIDChanged() {
                if ($('#<%=hdnInventoryDiscountSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnInventoryDiscountSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblInventoryDiscountSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtInventoryDiscountSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblInventoryDiscountSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtInventoryDiscountSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Inventory Discount Sub Ledger
            function onGetInventoryDiscountSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnInventoryDiscountFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnInventoryDiscountSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblInventoryDiscountSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnInventoryDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnInventoryDiscountSearchDialogTypeName.ClientID %>').val(), onGetInventoryDiscountSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtInventoryDiscountSubLedgerCode.ClientID %>').val(value);
                        onTxtInventoryDiscountSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtInventoryDiscountSubLedgerCode.ClientID %>').change(function () {
                onTxtInventoryDiscountSubLedgerCodeChanged($(this).val());
            });

            function onTxtInventoryDiscountSubLedgerCodeChanged(value) {
                if ($('#<%=hdnInventoryDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetInventoryDiscountSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnInventoryDiscountCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnInventoryDiscountMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnInventoryDiscountSubLedger.ClientID %>').val(result[$('#<%=hdnInventoryDiscountIDFieldName.ClientID %>').val()]);
                            $('#<%=txtInventoryDiscountSubLedgerName.ClientID %>').val(result[$('#<%=hdnInventoryDiscountDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnInventoryDiscountSubLedger.ClientID %>').val('');
                            $('#<%=txtInventoryDiscountSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtInventoryDiscountSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region InventoryVAT
            $('#lblInventoryVAT.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtInventoryVATGLAccountNo.ClientID %>').val(value);
                    onTxtInventoryVATGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtInventoryVATGLAccountNo.ClientID %>').change(function () {
                onTxtInventoryVATGLAccountCodeChanged($(this).val());
            });

            function onTxtInventoryVATGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnInventoryVATID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtInventoryVATGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnInventoryVATSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnInventoryVATSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnInventoryVATIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnInventoryVATCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnInventoryVATDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnInventoryVATMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnInventoryVATFilterExpression.ClientID %>').val(result.FilterExpression);
                        onInventoryVATSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnInventoryVATID.ClientID %>').val('');
                        $('#<%=txtInventoryVATGLAccountName.ClientID %>').val('');
                        $('#<%=hdnInventoryVATSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnInventoryVATSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnInventoryVATIDFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryVATCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryVATDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnInventoryVATMethodName.ClientID %>').val('');
                        $('#<%=hdnInventoryVATFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnInventoryVATSubLedger.ClientID %>').val('');
                    $('#<%=txtInventoryVATSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtInventoryVATSubLedgerName.ClientID %>').val('');
                });
            }

            function onInventoryVATSubLedgerIDChanged() {
                if ($('#<%=hdnInventoryVATSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnInventoryVATSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblInventoryVATSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtInventoryVATSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblInventoryVATSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtInventoryVATSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region InventoryVAT Sub Ledger
            function onGetInventoryVATSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnInventoryVATFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnInventoryVATSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblInventoryVATSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnInventoryVATSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnInventoryVATSearchDialogTypeName.ClientID %>').val(), onGetInventoryVATSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtInventoryVATSubLedgerCode.ClientID %>').val(value);
                        onTxtInventoryVATSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtInventoryVATSubLedgerCode.ClientID %>').change(function () {
                onTxtInventoryVATSubLedgerCodeChanged($(this).val());
            });

            function onTxtInventoryVATSubLedgerCodeChanged(value) {
                if ($('#<%=hdnInventoryVATSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetInventoryVATSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnInventoryVATCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnInventoryVATMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnInventoryVATSubLedger.ClientID %>').val(result[$('#<%=hdnInventoryVATIDFieldName.ClientID %>').val()]);
                            $('#<%=txtInventoryVATSubLedgerName.ClientID %>').val(result[$('#<%=hdnInventoryVATDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnInventoryVATSubLedger.ClientID %>').val('');
                            $('#<%=txtInventoryVATSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtInventoryVATSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region COGS
            $('#lblCOGS.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtCOGSGLAccountNo.ClientID %>').val(value);
                    onTxtCOGSGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtCOGSGLAccountNo.ClientID %>').change(function () {
                onTxtCOGSGLAccountCodeChanged($(this).val());
            });

            function onTxtCOGSGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCOGSID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtCOGSGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnCOGSSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnCOGSSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnCOGSIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCOGSCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnCOGSDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnCOGSMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnCOGSFilterExpression.ClientID %>').val(result.FilterExpression);
                        onCOGSSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnCOGSID.ClientID %>').val('');
                        $('#<%=txtCOGSGLAccountName.ClientID %>').val('');
                        $('#<%=hdnCOGSSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnCOGSSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnCOGSIDFieldName.ClientID %>').val('');
                        $('#<%=hdnCOGSCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnCOGSDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnCOGSMethodName.ClientID %>').val('');
                        $('#<%=hdnCOGSFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnCOGSSubLedger.ClientID %>').val('');
                    $('#<%=txtCOGSSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtCOGSSubLedgerName.ClientID %>').val('');
                });
            }

            function onCOGSSubLedgerIDChanged() {
                if ($('#<%=hdnCOGSSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnCOGSSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblCOGSSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtCOGSSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblCOGSSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtCOGSSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region COGS Sub Ledger
            function onGetCOGSSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnCOGSFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnCOGSSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblCOGSSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnCOGSSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnCOGSSearchDialogTypeName.ClientID %>').val(), onGetCOGSSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtCOGSSubLedgerCode.ClientID %>').val(value);
                        onTxtCOGSSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtCOGSSubLedgerCode.ClientID %>').change(function () {
                onTxtCOGSSubLedgerCodeChanged($(this).val());
            });

            function onTxtCOGSSubLedgerCodeChanged(value) {
                if ($('#<%=hdnCOGSSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetCOGSSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnCOGSCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnCOGSMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnCOGSSubLedger.ClientID %>').val(result[$('#<%=hdnCOGSIDFieldName.ClientID %>').val()]);
                            $('#<%=txtCOGSSubLedgerName.ClientID %>').val(result[$('#<%=hdnCOGSDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnCOGSSubLedger.ClientID %>').val('');
                            $('#<%=txtCOGSSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtCOGSSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Purchase
            $('#lblPurchase.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtPurchaseGLAccountNo.ClientID %>').val(value);
                    onTxtPurchaseGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtPurchaseGLAccountNo.ClientID %>').change(function () {
                onTxtPurchaseGLAccountCodeChanged($(this).val());
            });

            function onTxtPurchaseGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtPurchaseGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnPurchaseSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnPurchaseSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnPurchaseIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnPurchaseCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnPurchaseDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnPurchaseMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnPurchaseFilterExpression.ClientID %>').val(result.FilterExpression);
                        onPurchaseSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnPurchaseID.ClientID %>').val('');
                        $('#<%=txtPurchaseGLAccountName.ClientID %>').val('');
                        $('#<%=hdnPurchaseSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnPurchaseSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnPurchaseIDFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseMethodName.ClientID %>').val('');
                        $('#<%=hdnPurchaseFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnPurchaseSubLedger.ClientID %>').val('');
                    $('#<%=txtPurchaseSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtPurchaseSubLedgerName.ClientID %>').val('');
                });
            }

            function onPurchaseSubLedgerIDChanged() {
                if ($('#<%=hdnPurchaseSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnPurchaseSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblPurchaseSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPurchaseSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblPurchaseSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtPurchaseSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Purchase Sub Ledger
            function onGetPurchaseSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnPurchaseFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnPurchaseSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblPurchaseSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnPurchaseSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnPurchaseSearchDialogTypeName.ClientID %>').val(), onGetPurchaseSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtPurchaseSubLedgerCode.ClientID %>').val(value);
                        onTxtPurchaseSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtPurchaseSubLedgerCode.ClientID %>').change(function () {
                onTxtPurchaseSubLedgerCodeChanged($(this).val());
            });

            function onTxtPurchaseSubLedgerCodeChanged(value) {
                if ($('#<%=hdnPurchaseSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetPurchaseSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnPurchaseCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnPurchaseMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnPurchaseSubLedger.ClientID %>').val(result[$('#<%=hdnPurchaseIDFieldName.ClientID %>').val()]);
                            $('#<%=txtPurchaseSubLedgerName.ClientID %>').val(result[$('#<%=hdnPurchaseDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnPurchaseSubLedger.ClientID %>').val('');
                            $('#<%=txtPurchaseSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtPurchaseSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region PurchaseReturn
            $('#lblPurchaseReturn.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtPurchaseReturnGLAccountNo.ClientID %>').val(value);
                    onTxtPurchaseReturnGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtPurchaseReturnGLAccountNo.ClientID %>').change(function () {
                onTxtPurchaseReturnGLAccountCodeChanged($(this).val());
            });

            function onTxtPurchaseReturnGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtPurchaseReturnGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnPurchaseReturnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnPurchaseReturnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnPurchaseReturnIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnPurchaseReturnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnPurchaseReturnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnPurchaseReturnMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnPurchaseReturnFilterExpression.ClientID %>').val(result.FilterExpression);
                        onPurchaseReturnSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val('');
                        $('#<%=txtPurchaseReturnGLAccountName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnIDFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnMethodName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnPurchaseReturnSubLedger.ClientID %>').val('');
                    $('#<%=txtPurchaseReturnSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtPurchaseReturnSubLedgerName.ClientID %>').val('');
                });
            }

            function onPurchaseReturnSubLedgerIDChanged() {
                if ($('#<%=hdnPurchaseReturnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnPurchaseReturnSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblPurchaseReturnSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPurchaseReturnSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblPurchaseReturnSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtPurchaseReturnSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region PurchaseReturn Sub Ledger
            function onGetPurchaseReturnSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnPurchaseReturnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnPurchaseReturnSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblPurchaseReturnSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnPurchaseReturnSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnPurchaseReturnSearchDialogTypeName.ClientID %>').val(), onGetPurchaseReturnSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtPurchaseReturnSubLedgerCode.ClientID %>').val(value);
                        onTxtPurchaseReturnSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtPurchaseReturnSubLedgerCode.ClientID %>').change(function () {
                onTxtPurchaseReturnSubLedgerCodeChanged($(this).val());
            });

            function onTxtPurchaseReturnSubLedgerCodeChanged(value) {
                if ($('#<%=hdnPurchaseReturnSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetPurchaseReturnSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnPurchaseReturnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnPurchaseReturnMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnPurchaseReturnSubLedger.ClientID %>').val(result[$('#<%=hdnPurchaseReturnIDFieldName.ClientID %>').val()]);
                            $('#<%=txtPurchaseReturnSubLedgerName.ClientID %>').val(result[$('#<%=hdnPurchaseReturnDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnPurchaseReturnSubLedger.ClientID %>').val('');
                            $('#<%=txtPurchaseReturnSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtPurchaseReturnSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region PurchaseDiscount
            $('#lblPurchaseDiscount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtPurchaseDiscountGLAccountNo.ClientID %>').val(value);
                    onTxtPurchaseDiscountGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtPurchaseDiscountGLAccountNo.ClientID %>').change(function () {
                onTxtPurchaseDiscountGLAccountCodeChanged($(this).val());
            });

            function onTxtPurchaseDiscountGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseDiscount.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtPurchaseDiscountGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnPurchaseDiscountSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnPurchaseDiscountSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnPurchaseDiscountIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnPurchaseDiscountCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnPurchaseDiscountDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnPurchaseDiscountMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnPurchaseDiscountFilterExpression.ClientID %>').val(result.FilterExpression);
                        onPurchaseDiscountSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnPurchaseDiscount.ClientID %>').val('');
                        $('#<%=txtPurchaseDiscountGLAccountName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountIDFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountMethodName.ClientID %>').val('');
                        $('#<%=hdnPurchaseDiscountFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnPurchaseDiscountSubLedger.ClientID %>').val('');
                    $('#<%=txtPurchaseDiscountSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtPurchaseDiscountSubLedgerName.ClientID %>').val('');
                });
            }

            function onPurchaseDiscountSubLedgerIDChanged() {
                if ($('#<%=hdnPurchaseDiscountSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnPurchaseDiscountSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblPurchaseDiscountSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPurchaseDiscountSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblPurchaseDiscountSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtPurchaseDiscountSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region PurchaseDiscount Sub Ledger
            function onGetPurchaseDiscountSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnPurchaseDiscountFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnPurchaseDiscountSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblPurchaseDiscountSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnPurchaseDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnPurchaseDiscountSearchDialogTypeName.ClientID %>').val(), onGetPurchaseDiscountSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtPurchaseDiscountSubLedgerCode.ClientID %>').val(value);
                        onTxtPurchaseDiscountSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtPurchaseDiscountSubLedgerCode.ClientID %>').change(function () {
                onTxtPurchaseDiscountSubLedgerCodeChanged($(this).val());
            });

            function onTxtPurchaseDiscountSubLedgerCodeChanged(value) {
                if ($('#<%=hdnPurchaseDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetPurchaseDiscountSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnPurchaseDiscountCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnPurchaseDiscountMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnPurchaseDiscountSubLedger.ClientID %>').val(result[$('#<%=hdnPurchaseDiscountIDFieldName.ClientID %>').val()]);
                            $('#<%=txtPurchaseDiscountSubLedgerName.ClientID %>').val(result[$('#<%=hdnPurchaseDiscountDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnPurchaseDiscountSubLedger.ClientID %>').val('');
                            $('#<%=txtPurchaseDiscountSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtPurchaseDiscountSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Account Payable In Process
            $('#lblAccountPayableInProcess.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAccountPayableInProcessGLAccountNo.ClientID %>').val(value);
                    onTxtAccountPayableInProcessGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAccountPayableInProcessGLAccountNo.ClientID %>').change(function () {
                onTxtAccountPayableInProcessGLAccountCodeChanged($(this).val());
            });

            function onTxtAccountPayableInProcessGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAccountPayableInProcessID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAccountPayableInProcessGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAccountPayableInProcessSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAccountPayableInProcessIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAccountPayableInProcessCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAccountPayableInProcessDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAccountPayableInProcessMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAccountPayableInProcessFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAccountPayableInProcessSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAccountPayableInProcessID.ClientID %>').val('');
                        $('#<%=txtAccountPayableInProcessGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessMethodName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableInProcessFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val('');
                    $('#<%=txtAccountPayableInProcessSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAccountPayableInProcessSubLedgerName.ClientID %>').val('');
                });
            }

            function onAccountPayableInProcessSubLedgerIDChanged() {
                if ($('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAccountPayableInProcessSubLedgerID.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAccountPayableInProcessSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAccountPayableInProcessSubLedgerID.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAccountPayableInProcessSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Account Payable In Process Sub Ledger
            function onGetAccountPayableInProcessSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAccountPayableInProcessFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAccountPayableInProcessSubLedgerID.ClientID %>').click(function () {
                if ($('#<%=hdnAccountPayableInProcessSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAccountPayableInProcessSearchDialogTypeName.ClientID %>').val(), onGetAccountPayableInProcessSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAccountPayableInProcessSubLedgerCode.ClientID %>').val(value);
                        onTxtAccountPayableInProcessSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAccountPayableInProcessSubLedgerCode.ClientID %>').change(function () {
                onTxtAccountPayableInProcessSubLedgerCodeChanged($(this).val());
            });

            function onTxtAccountPayableInProcessSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAccountPayableInProcessSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAccountPayableInProcessSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAccountPayableInProcessCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAccountPayableInProcessMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val(result[$('#<%=hdnAccountPayableInProcessIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAccountPayableInProcessSubLedgerName.ClientID %>').val(result[$('#<%=hdnAccountPayableInProcessDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAccountPayableInProcessSubLedgerID.ClientID %>').val('');
                            $('#<%=txtAccountPayableInProcessSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAccountPayableInProcessSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Account Payable
            $('#lblAccountPayable.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAccountPayableGLAccountNo.ClientID %>').val(value);
                    onTxtAccountPayableGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAccountPayableGLAccountNo.ClientID %>').change(function () {
                onTxtAccountPayableGLAccountCodeChanged($(this).val());
            });

            function onTxtAccountPayableGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAccountPayableID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAccountPayableGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAccountPayableSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAccountPayableIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAccountPayableCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAccountPayableDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAccountPayableMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAccountPayableFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAccountPayableSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAccountPayableID.ClientID %>').val('');
                        $('#<%=txtAccountPayableGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAccountPayableSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableMethodName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val('');
                    $('#<%=txtAccountPayableSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAccountPayableSubLedgerName.ClientID %>').val('');
                });
            }

            function onAccountPayableSubLedgerIDChanged() {
                if ($('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAccountPayableSubLedgerID.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAccountPayableSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAccountPayableSubLedgerID.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAccountPayableSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Account Payable Consignment
            $('#lblAccountPayableConsignment.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAccountPayableConsignmentGLAccountNo.ClientID %>').val(value);
                    onTxtAccountPayableConsignmentGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAccountPayableConsignmentGLAccountNo.ClientID %>').change(function () {
                onTxtAccountPayableConsignmentGLAccountCodeChanged($(this).val());
            });

            function onTxtAccountPayableConsignmentGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAccountPayableConsignmentID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAccountPayableConsignmentGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAccountPayableConsignmentSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAccountPayableConsignmentSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAccountPayableConsignmentIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAccountPayableConsignmentCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAccountPayableConsignmentDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAccountPayableConsignmentMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAccountPayableConsignmentFilterExpression.ClientID %>').val(result.FilterExpression);
                    }
                    else {
                        $('#<%=hdnAccountPayableConsignmentID.ClientID %>').val('');
                        $('#<%=txtAccountPayableConsignmentGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentMethodName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableConsignmentFilterExpression.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Account Payable Down Payment
            $('#lblAccountPayableDownPayment.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAccountPayableDownPaymentGLAccountNo.ClientID %>').val(value);
                    onTxtAccountPayableDownPaymentGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAccountPayableDownPaymentGLAccountNo.ClientID %>').change(function () {
                onTxtAccountPayableDownPaymentGLAccountCodeChanged($(this).val());
            });

            function onTxtAccountPayableDownPaymentGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAccountPayableDownPaymentID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAccountPayableDownPaymentGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAccountPayableDownPaymentSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAccountPayableDownPaymentSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAccountPayableDownPaymentIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAccountPayableDownPaymentCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAccountPayableDownPaymentDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAccountPayableDownPaymentMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAccountPayableDownPaymentFilterExpression.ClientID %>').val(result.FilterExpression);
                    }
                    else {
                        $('#<%=hdnAccountPayableDownPaymentID.ClientID %>').val('');
                        $('#<%=txtAccountPayableDownPaymentGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentMethodName.ClientID %>').val('');
                        $('#<%=hdnAccountPayableDownPaymentFilterExpression.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Account Payable Sub Ledger
            function onGetAccountPayableSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAccountPayableFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAccountPayableSubLedgerID.ClientID %>').click(function () {
                if ($('#<%=hdnAccountPayableSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAccountPayableSearchDialogTypeName.ClientID %>').val(), onGetAccountPayableSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAccountPayableSubLedgerCode.ClientID %>').val(value);
                        onTxtAccountPayableSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAccountPayableSubLedgerCode.ClientID %>').change(function () {
                onTxtAccountPayableSubLedgerCodeChanged($(this).val());
            });

            function onTxtAccountPayableSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAccountPayableSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAccountPayableSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAccountPayableCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAccountPayableMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val(result[$('#<%=hdnAccountPayableIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAccountPayableSubLedgerName.ClientID %>').val(result[$('#<%=hdnAccountPayableDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAccountPayableSubLedgerID.ClientID %>').val('');
                            $('#<%=txtAccountPayableSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAccountPayableSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region PurchasePriceVariant
            $('#lblPurchasePriceVariant.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtPurchasePriceVariantGLAccountNo.ClientID %>').val(value);
                    onTxtPurchasePriceVariantGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtPurchasePriceVariantGLAccountNo.ClientID %>').change(function () {
                onTxtPurchasePriceVariantGLAccountCodeChanged($(this).val());
            });

            function onTxtPurchasePriceVariantGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchasePriceVariant.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtPurchasePriceVariantGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnPurchasePriceVariantIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnPurchasePriceVariantCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnPurchasePriceVariantDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnPurchasePriceVariantMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnPurchasePriceVariantFilterExpression.ClientID %>').val(result.FilterExpression);
                        onPurchasePriceVariantSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnPurchasePriceVariant.ClientID %>').val('');
                        $('#<%=txtPurchasePriceVariantGLAccountName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantIDFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantMethodName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnPurchasePriceVariantSubLedger.ClientID %>').val('');
                    $('#<%=txtPurchasePriceVariantSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtPurchasePriceVariantSubLedgerName.ClientID %>').val('');
                });
            }

            function onPurchasePriceVariantSubLedgerIDChanged() {
                if ($('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblPurchasePriceVariantSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPurchasePriceVariantSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblPurchasePriceVariantSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtPurchasePriceVariantSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region PurchasePriceVariant Sub Ledger
            function onGetPurchasePriceVariantSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnPurchasePriceVariantFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblPurchasePriceVariantSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val(), onGetPurchasePriceVariantSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtPurchasePriceVariantSubLedgerCode.ClientID %>').val(value);
                        onTxtPurchasePriceVariantSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtPurchasePriceVariantSubLedgerCode.ClientID %>').change(function () {
                onTxtPurchasePriceVariantSubLedgerCodeChanged($(this).val());
            });

            function onTxtPurchasePriceVariantSubLedgerCodeChanged(value) {
                if ($('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetPurchasePriceVariantSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnPurchasePriceVariantCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnPurchasePriceVariantMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnPurchasePriceVariantSubLedger.ClientID %>').val(result[$('#<%=hdnPurchasePriceVariantIDFieldName.ClientID %>').val()]);
                            $('#<%=txtPurchasePriceVariantSubLedgerName.ClientID %>').val(result[$('#<%=hdnPurchasePriceVariantDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnPurchasePriceVariantSubLedger.ClientID %>').val('');
                            $('#<%=txtPurchasePriceVariantSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtPurchasePriceVariantSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Sales
            $('#lblSales.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtSalesGLAccountNo.ClientID %>').val(value);
                    onTxtSalesGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtSalesGLAccountNo.ClientID %>').change(function () {
                onTxtSalesGLAccountCodeChanged($(this).val());
            });

            function onTxtSalesGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSales.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtSalesGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnSalesSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSalesSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnSalesIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnSalesCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnSalesDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnSalesMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnSalesFilterExpression.ClientID %>').val(result.FilterExpression);
                        onSalesSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnSales.ClientID %>').val('');
                        $('#<%=txtSalesGLAccountName.ClientID %>').val('');
                        $('#<%=hdnSalesSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnSalesSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnSalesIDFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesMethodName.ClientID %>').val('');
                        $('#<%=hdnSalesFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnSalesSubLedger.ClientID %>').val('');
                    $('#<%=txtSalesSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtSalesSubLedgerName.ClientID %>').val('');
                });
            }

            function onSalesSubLedgerIDChanged() {
                if ($('#<%=hdnSalesSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSalesSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblSalesSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSalesSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSalesSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSalesSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sales Sub Ledger
            function onGetSalesSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnSalesFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSalesSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSalesSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnSalesSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSalesSearchDialogTypeName.ClientID %>').val(), onGetSalesSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtSalesSubLedgerCode.ClientID %>').val(value);
                        onTxtSalesSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtSalesSubLedgerCode.ClientID %>').change(function () {
                onTxtSalesSubLedgerCodeChanged($(this).val());
            });

            function onTxtSalesSubLedgerCodeChanged(value) {
                if ($('#<%=hdnSalesSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetSalesSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnSalesCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnSalesMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSalesSubLedger.ClientID %>').val(result[$('#<%=hdnSalesIDFieldName.ClientID %>').val()]);
                            $('#<%=txtSalesSubLedgerName.ClientID %>').val(result[$('#<%=hdnSalesDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSalesSubLedger.ClientID %>').val('');
                            $('#<%=txtSalesSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtSalesSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region SalesReturn
            $('#lblSalesReturn.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtSalesReturnGLAccountNo.ClientID %>').val(value);
                    onTxtSalesReturnGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtSalesReturnGLAccountNo.ClientID %>').change(function () {
                onTxtSalesReturnGLAccountCodeChanged($(this).val());
            });

            function onTxtSalesReturnGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSalesReturn.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtSalesReturnGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnSalesReturnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSalesReturnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnSalesReturnIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnSalesReturnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnSalesReturnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnSalesReturnMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnSalesReturnFilterExpression.ClientID %>').val(result.FilterExpression);
                        onSalesReturnSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnSalesReturn.ClientID %>').val('');
                        $('#<%=txtSalesReturnGLAccountName.ClientID %>').val('');
                        $('#<%=hdnSalesReturnSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnSalesReturnSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnSalesReturnIDFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesReturnCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesReturnDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesReturnMethodName.ClientID %>').val('');
                        $('#<%=hdnSalesReturnFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnSalesReturnSubLedger.ClientID %>').val('');
                    $('#<%=txtSalesReturnSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtSalesReturnSubLedgerName.ClientID %>').val('');
                });
            }

            function onSalesReturnSubLedgerIDChanged() {
                if ($('#<%=hdnSalesReturnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSalesReturnSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblSalesReturnSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSalesReturnSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSalesReturnSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSalesReturnSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region SalesReturn Sub Ledger
            function onGetSalesReturnSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnSalesReturnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSalesReturnSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSalesReturnSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnSalesReturnSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSalesReturnSearchDialogTypeName.ClientID %>').val(), onGetSalesReturnSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtSalesReturnSubLedgerCode.ClientID %>').val(value);
                        onTxtSalesReturnSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtSalesReturnSubLedgerCode.ClientID %>').change(function () {
                onTxtSalesReturnSubLedgerCodeChanged($(this).val());
            });

            function onTxtSalesReturnSubLedgerCodeChanged(value) {
                if ($('#<%=hdnSalesReturnSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetSalesReturnSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnSalesReturnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnSalesReturnMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSalesReturnSubLedger.ClientID %>').val(result[$('#<%=hdnSalesReturnIDFieldName.ClientID %>').val()]);
                            $('#<%=txtSalesReturnSubLedgerName.ClientID %>').val(result[$('#<%=hdnSalesReturnDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSalesReturnSubLedger.ClientID %>').val('');
                            $('#<%=txtSalesReturnSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtSalesReturnSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region SalesDiscount
            $('#lblSalesDiscount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtSalesDiscountGLAccountNo.ClientID %>').val(value);
                    onTxtSalesDiscountGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtSalesDiscountGLAccountNo.ClientID %>').change(function () {
                onTxtSalesDiscountGLAccountCodeChanged($(this).val());
            });

            function onTxtSalesDiscountGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSalesDiscount.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtSalesDiscountGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnSalesDiscountSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSalesDiscountSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnSalesDiscountIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnSalesDiscountCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnSalesDiscountDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnSalesDiscountMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnSalesDiscountFilterExpression.ClientID %>').val(result.FilterExpression);
                        onSalesDiscountSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnSalesDiscount.ClientID %>').val('');
                        $('#<%=txtSalesDiscountGLAccountName.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountIDFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountMethodName.ClientID %>').val('');
                        $('#<%=hdnSalesDiscountFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnSalesDiscountSubLedger.ClientID %>').val('');
                    $('#<%=txtSalesDiscountSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtSalesDiscountSubLedgerName.ClientID %>').val('');
                });
            }

            function onSalesDiscountSubLedgerIDChanged() {
                if ($('#<%=hdnSalesDiscountSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSalesDiscountSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblSalesDiscountSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSalesDiscountSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSalesDiscountSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSalesDiscountSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region SalesDiscount Sub Ledger
            function onGetSalesDiscountSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnSalesDiscountFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSalesDiscountSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSalesDiscountSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnSalesDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSalesDiscountSearchDialogTypeName.ClientID %>').val(), onGetSalesDiscountSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtSalesDiscountSubLedgerCode.ClientID %>').val(value);
                        onTxtSalesDiscountSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtSalesDiscountSubLedgerCode.ClientID %>').change(function () {
                onTxtSalesDiscountSubLedgerCodeChanged($(this).val());
            });

            function onTxtSalesDiscountSubLedgerCodeChanged(value) {
                if ($('#<%=hdnSalesDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetSalesDiscountSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnSalesDiscountCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnSalesDiscountMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSalesDiscountSubLedger.ClientID %>').val(result[$('#<%=hdnSalesDiscountIDFieldName.ClientID %>').val()]);
                            $('#<%=txtSalesDiscountSubLedgerName.ClientID %>').val(result[$('#<%=hdnSalesDiscountDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSalesDiscountSubLedger.ClientID %>').val('');
                            $('#<%=txtSalesDiscountSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtSalesDiscountSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region MaterialRevenue
            $('#lblMaterialRevenue.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtMaterialRevenueGLAccountNo.ClientID %>').val(value);
                    onTxtMaterialRevenueGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtMaterialRevenueGLAccountNo.ClientID %>').change(function () {
                onTxtMaterialRevenueGLAccountCodeChanged($(this).val());
            });

            function onTxtMaterialRevenueGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMaterialRevenue.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtMaterialRevenueGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnMaterialRevenueSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnMaterialRevenueSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnMaterialRevenueIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnMaterialRevenueCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnMaterialRevenueDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMaterialRevenueMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnMaterialRevenueFilterExpression.ClientID %>').val(result.FilterExpression);
                        onMaterialRevenueSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnMaterialRevenue.ClientID %>').val('');
                        $('#<%=txtMaterialRevenueGLAccountName.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueIDFieldName.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueMethodName.ClientID %>').val('');
                        $('#<%=hdnMaterialRevenueFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnMaterialRevenueSubLedger.ClientID %>').val('');
                    $('#<%=txtMaterialRevenueSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtMaterialRevenueSubLedgerName.ClientID %>').val('');
                });
            }

            function onMaterialRevenueSubLedgerIDChanged() {
                if ($('#<%=hdnMaterialRevenueSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnMaterialRevenueSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblMaterialRevenueSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtMaterialRevenueSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblMaterialRevenueSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtMaterialRevenueSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region MaterialRevenue Sub Ledger
            function onGetMaterialRevenueSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnMaterialRevenueFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnMaterialRevenueSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblMaterialRevenueSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnMaterialRevenueSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnMaterialRevenueSearchDialogTypeName.ClientID %>').val(), onGetMaterialRevenueSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtMaterialRevenueSubLedgerCode.ClientID %>').val(value);
                        onTxtMaterialRevenueSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtMaterialRevenueSubLedgerCode.ClientID %>').change(function () {
                onTxtMaterialRevenueSubLedgerCodeChanged($(this).val());
            });

            function onTxtMaterialRevenueSubLedgerCodeChanged(value) {
                if ($('#<%=hdnMaterialRevenueSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetMaterialRevenueSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnMaterialRevenueCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMaterialRevenueMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnMaterialRevenueSubLedger.ClientID %>').val(result[$('#<%=hdnMaterialRevenueIDFieldName.ClientID %>').val()]);
                            $('#<%=txtMaterialRevenueSubLedgerName.ClientID %>').val(result[$('#<%=hdnMaterialRevenueDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnMaterialRevenueSubLedger.ClientID %>').val('');
                            $('#<%=txtMaterialRevenueSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtMaterialRevenueSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Consumption
            $('#lblConsumption.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtConsumptionGLAccountNo.ClientID %>').val(value);
                    onTxtConsumptionGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtConsumptionGLAccountNo.ClientID %>').change(function () {
                onTxtConsumptionGLAccountCodeChanged($(this).val());
            });

            function onTxtConsumptionGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnConsumption.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtConsumptionGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnConsumptionSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnConsumptionSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnConsumptionIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnConsumptionCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnConsumptionDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnConsumptionMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnConsumptionFilterExpression.ClientID %>').val(result.FilterExpression);
                        onConsumptionSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnConsumption.ClientID %>').val('');
                        $('#<%=txtConsumptionGLAccountName.ClientID %>').val('');
                        $('#<%=hdnConsumptionSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnConsumptionSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnConsumptionIDFieldName.ClientID %>').val('');
                        $('#<%=hdnConsumptionCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnConsumptionDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnConsumptionMethodName.ClientID %>').val('');
                        $('#<%=hdnConsumptionFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnConsumptionSubLedger.ClientID %>').val('');
                    $('#<%=txtConsumptionSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtConsumptionSubLedgerName.ClientID %>').val('');
                });
            }

            function onConsumptionSubLedgerIDChanged() {
                if ($('#<%=hdnConsumptionSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnConsumptionSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblConsumptionSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtConsumptionSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblConsumptionSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtConsumptionSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Consumption Sub Ledger
            function onGetConsumptionSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnConsumptionFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnConsumptionSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblConsumptionSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnConsumptionSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnConsumptionSearchDialogTypeName.ClientID %>').val(), onGetConsumptionSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtConsumptionSubLedgerCode.ClientID %>').val(value);
                        onTxtConsumptionSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtConsumptionSubLedgerCode.ClientID %>').change(function () {
                onTxtConsumptionSubLedgerCodeChanged($(this).val());
            });

            function onTxtConsumptionSubLedgerCodeChanged(value) {
                if ($('#<%=hdnConsumptionSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetConsumptionSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnConsumptionCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnConsumptionMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnConsumptionSubLedger.ClientID %>').val(result[$('#<%=hdnConsumptionIDFieldName.ClientID %>').val()]);
                            $('#<%=txtConsumptionSubLedgerName.ClientID %>').val(result[$('#<%=hdnConsumptionDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnConsumptionSubLedger.ClientID %>').val('');
                            $('#<%=txtConsumptionSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtConsumptionSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region AdjustmentIN
            $('#lblAdjustmentIN.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAdjustmentINGLAccountNo.ClientID %>').val(value);
                    onTxtAdjustmentINGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAdjustmentINGLAccountNo.ClientID %>').change(function () {
                onTxtAdjustmentINGLAccountCodeChanged($(this).val());
            });

            function onTxtAdjustmentINGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAdjustmentIN.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAdjustmentINGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAdjustmentINSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAdjustmentINSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAdjustmentINIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAdjustmentINCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAdjustmentINDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAdjustmentINMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAdjustmentINFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAdjustmentINSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAdjustmentIN.ClientID %>').val('');
                        $('#<%=txtAdjustmentINGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINMethodName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentINFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAdjustmentINSubLedger.ClientID %>').val('');
                    $('#<%=txtAdjustmentINSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAdjustmentINSubLedgerName.ClientID %>').val('');
                });
            }

            function onAdjustmentINSubLedgerIDChanged() {
                if ($('#<%=hdnAdjustmentINSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAdjustmentINSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAdjustmentINSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAdjustmentINSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAdjustmentINSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAdjustmentINSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region AdjustmentIN Sub Ledger
            function onGetAdjustmentINSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAdjustmentINFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAdjustmentINSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAdjustmentINSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAdjustmentINSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAdjustmentINSearchDialogTypeName.ClientID %>').val(), onGetAdjustmentINSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAdjustmentINSubLedgerCode.ClientID %>').val(value);
                        onTxtAdjustmentINSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAdjustmentINSubLedgerCode.ClientID %>').change(function () {
                onTxtAdjustmentINSubLedgerCodeChanged($(this).val());
            });

            function onTxtAdjustmentINSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAdjustmentINSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAdjustmentINSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAdjustmentINCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAdjustmentINMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAdjustmentINSubLedger.ClientID %>').val(result[$('#<%=hdnAdjustmentINIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAdjustmentINSubLedgerName.ClientID %>').val(result[$('#<%=hdnAdjustmentINDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAdjustmentINSubLedger.ClientID %>').val('');
                            $('#<%=txtAdjustmentINSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAdjustmentINSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region AdjustmentOUT
            $('#lblAdjustmentOUT.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAdjustmentOUTGLAccountNo.ClientID %>').val(value);
                    onTxtAdjustmentOUTGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAdjustmentOUTGLAccountNo.ClientID %>').change(function () {
                onTxtAdjustmentOUTGLAccountCodeChanged($(this).val());
            });

            function onTxtAdjustmentOUTGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAdjustmentOUT.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAdjustmentOUTGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAdjustmentOUTSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAdjustmentOUTSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAdjustmentOUTIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAdjustmentOUTCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAdjustmentOUTDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAdjustmentOUTMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAdjustmentOUTFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAdjustmentOUTSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAdjustmentOUT.ClientID %>').val('');
                        $('#<%=txtAdjustmentOUTGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTMethodName.ClientID %>').val('');
                        $('#<%=hdnAdjustmentOUTFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAdjustmentOUTSubLedger.ClientID %>').val('');
                    $('#<%=txtAdjustmentOUTSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAdjustmentOUTSubLedgerName.ClientID %>').val('');
                });
            }

            function onAdjustmentOUTSubLedgerIDChanged() {
                if ($('#<%=hdnAdjustmentOUTSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAdjustmentOUTSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAdjustmentOUTSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAdjustmentOUTSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAdjustmentOUTSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAdjustmentOUTSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region AdjustmentOUT Sub Ledger
            function onGetAdjustmentOUTSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAdjustmentOUTFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAdjustmentOUTSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAdjustmentOUTSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAdjustmentOUTSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAdjustmentOUTSearchDialogTypeName.ClientID %>').val(), onGetAdjustmentOUTSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAdjustmentOUTSubLedgerCode.ClientID %>').val(value);
                        onTxtAdjustmentOUTSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAdjustmentOUTSubLedgerCode.ClientID %>').change(function () {
                onTxtAdjustmentOUTSubLedgerCodeChanged($(this).val());
            });

            function onTxtAdjustmentOUTSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAdjustmentOUTSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAdjustmentOUTSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAdjustmentOUTCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAdjustmentOUTMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAdjustmentOUTSubLedger.ClientID %>').val(result[$('#<%=hdnAdjustmentOUTIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAdjustmentOUTSubLedgerName.ClientID %>').val(result[$('#<%=hdnAdjustmentOUTDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAdjustmentOUTSubLedger.ClientID %>').val('');
                            $('#<%=txtAdjustmentOUTSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAdjustmentOUTSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            onInventorySubLedgerIDChanged();
            onInventoryVATSubLedgerIDChanged();
            onCOGSSubLedgerIDChanged();
            onPurchaseSubLedgerIDChanged();
            onPurchaseReturnSubLedgerIDChanged();
            onPurchaseDiscountSubLedgerIDChanged();
            onPurchasePriceVariantSubLedgerIDChanged();
            onSalesSubLedgerIDChanged();
            onSalesReturnSubLedgerIDChanged();
            onSalesDiscountSubLedgerIDChanged();
            onMaterialRevenueSubLedgerIDChanged();
            onConsumptionSubLedgerIDChanged();
            onAdjustmentINSubLedgerIDChanged();
            onAdjustmentOUTSubLedgerIDChanged();
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("itemType", $('#<%=hdnGCItemType.ClientID %>').val()));
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Product Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineCode" Width="30%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Product Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kategori Anggaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBudgetCategory" ClientInstanceName="cboBudgetCategory" Width="100%"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px">
                            <label>
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsInventoryItem" runat="server" Text="Persediaan/Stock" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsFixedAsset" runat="server" Text="Aset/Inventaris" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsConsigmentItem" runat="server" Text="Barang Konsinyasi" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td>
                            <table width="100%" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col width="30%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblInventory">
                                            <%=GetLabel("COA Persediaan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnInventoryID" runat="server" />
                                        <input type="hidden" id="hdnInventorySubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnInventorySearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnInventoryIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryMethodName" runat="server" />
                                        <input type="hidden" id="hdnInventoryFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblInventoryDiscount">
                                            <%=GetLabel("COA Diskon Persediaan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnInventoryDiscountID" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountMethodName" runat="server" />
                                        <input type="hidden" id="hdnInventoryDiscountFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryDiscountGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryDiscountGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblInventoryVAT">
                                            <%=GetLabel("COA PPN Masukkan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnInventoryVATID" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATMethodName" runat="server" />
                                        <input type="hidden" id="hdnInventoryVATFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryVATGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryVATGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblCOGS">
                                            <%=GetLabel("COA COGS")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnCOGSID" runat="server" />
                                        <input type="hidden" id="hdnCOGSSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnCOGSSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnCOGSIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnCOGSCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnCOGSDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnCOGSMethodName" runat="server" />
                                        <input type="hidden" id="hdnCOGSFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCOGSGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCOGSGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblPurchaseReturn">
                                            <%=GetLabel("COA Retur Pembelian")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchaseReturnID" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnMethodName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseReturnFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseReturnGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseReturnGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblPurchaseDiscount">
                                            <%=GetLabel("COA Diskon Pembelian")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchaseDiscount" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountMethodName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDiscountFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseDiscountGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseDiscountGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblPurchase">
                                            <%=GetLabel("COA Pembelian Tunai")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchaseID" runat="server" />
                                        <input type="hidden" id="hdnPurchaseSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnPurchaseSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseMethodName" runat="server" />
                                        <input type="hidden" id="hdnPurchaseFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccountPayableInProcess">
                                            <%=GetLabel("COA Hutang Dalam Proses")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAccountPayableInProcessID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessMethodName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableInProcessFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableInProcessGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableInProcessGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccountPayable">
                                            <%=GetLabel("COA Hutang Usaha")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAccountPayableID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableMethodName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccountPayableConsignment">
                                            <%=GetLabel("COA Hutang Usaha Konsinyasi")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAccountPayableConsignmentID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentMethodName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableConsignmentFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableConsignmentGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableConsignmentGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAccountPayableDownPayment">
                                            <%=GetLabel("COA Uang Muka Pembelian")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAccountPayableDownPaymentID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentMethodName" runat="server" />
                                        <input type="hidden" id="hdnAccountPayableDownPaymentFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableDownPaymentGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableDownPaymentGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><hr /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblSales">
                                            <%=GetLabel("COA Penjualan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnSales" runat="server" />
                                        <input type="hidden" id="hdnSalesSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnSalesSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnSalesIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesMethodName" runat="server" />
                                        <input type="hidden" id="hdnSalesFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblSalesReturn">
                                            <%=GetLabel("COA Retur Penjualan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnSalesReturn" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnMethodName" runat="server" />
                                        <input type="hidden" id="hdnSalesReturnFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesReturnGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesReturnGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblSalesDiscount">
                                            <%=GetLabel("COA Diskon Penjualan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnSalesDiscount" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountMethodName" runat="server" />
                                        <input type="hidden" id="hdnSalesDiscountFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesDiscountGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesDiscountGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblMaterialRevenue">
                                            <%=GetLabel("COA Pendapatan Material")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnMaterialRevenue" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueMethodName" runat="server" />
                                        <input type="hidden" id="hdnMaterialRevenueFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMaterialRevenueGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMaterialRevenueGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblConsumption">
                                            <%=GetLabel("COA Pemakaian Barang")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnConsumption" runat="server" />
                                        <input type="hidden" id="hdnConsumptionSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnConsumptionSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnConsumptionIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnConsumptionCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnConsumptionDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnConsumptionMethodName" runat="server" />
                                        <input type="hidden" id="hdnConsumptionFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtConsumptionGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtConsumptionGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAdjustmentIN">
                                            <%=GetLabel("COA Adjustment Bertambah")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAdjustmentIN" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINMethodName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentINFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentINGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentINGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAdjustmentOUT">
                                            <%=GetLabel("COA Adjustment Berkurang")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAdjustmentOUT" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTMethodName" runat="server" />
                                        <input type="hidden" id="hdnAdjustmentOUTFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentOUTGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentOUTGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblPurchasePriceVariant">
                                            <%=GetLabel("COA Perubahan Harga")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchasePriceVariant" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantMethodName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchasePriceVariantGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchasePriceVariantGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="display: none;">
                            <table width="100%">
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblInventorySubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnInventorySubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventorySubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventorySubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblInventoryDiscountSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnInventoryDiscountSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryDiscountSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryDiscountSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblInventoryVATSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnInventoryVATSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryVATSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInventoryVATSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblCOGSSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnCOGSSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCOGSSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCOGSSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblPurchaseSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchaseSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblPurchaseReturnSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchaseReturnSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseReturnSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseReturnSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblPurchaseDiscountSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchaseDiscountSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseDiscountSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchaseDiscountSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblAccountPayableInProcessSubLedgerID">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAccountPayableInProcessSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableInProcessSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableInProcessSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblAccountPayableSubLedgerID">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAccountPayableSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAccountPayableSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblPurchasePriceVariantSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnPurchasePriceVariantSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchasePriceVariantSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPurchasePriceVariantSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblSalesSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnSalesSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblSalesReturnSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnSalesReturnSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesReturnSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesReturnSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblSalesDiscountSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnSalesDiscountSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesDiscountSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSalesDiscountSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblMaterialRevenueSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnMaterialRevenueSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMaterialRevenueSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMaterialRevenueSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblConsumptionSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnConsumptionSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtConsumptionSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtConsumptionSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblAdjustmentINSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAdjustmentINSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentINSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentINSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblAdjustmentOUTSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAdjustmentOUTSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentOUTSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentOUTSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
