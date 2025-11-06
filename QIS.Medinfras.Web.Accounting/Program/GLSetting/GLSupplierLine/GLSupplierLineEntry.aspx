<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="GLSupplierLineEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLSupplierLineEntry" %>

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

            //#region AP
            $('#lblAP.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPGLAccountNo.ClientID %>').val(value);
                    onTxtAPGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPGLAccountNo.ClientID %>').change(function () {
                onTxtAPGLAccountCodeChanged($(this).val());
            });

            function onTxtAPGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPID.ClientID %>').val('');
                        $('#<%=txtAPGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPMethodName.ClientID %>').val('');
                        $('#<%=hdnAPFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPSubLedger.ClientID %>').val('');
                    $('#<%=txtAPSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPSubLedgerIDChanged() {
                if ($('#<%=hdnAPSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region AP Sub Ledger
            function onGetAPSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPSearchDialogTypeName.ClientID %>').val(), onGetAPSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPSubLedgerCode.ClientID %>').val(value);
                        onTxtAPSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPSubLedgerCode.ClientID %>').change(function () {
                onTxtAPSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPSubLedger.ClientID %>').val(result[$('#<%=hdnAPIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPSubLedger.ClientID %>').val('');
                            $('#<%=txtAPSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region APInProcess
            $('#lblAPInProcess.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPInProcessGLAccountNo.ClientID %>').val(value);
                    onTxtAPInProcessGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPInProcessGLAccountNo.ClientID %>').change(function () {
                onTxtAPInProcessGLAccountCodeChanged($(this).val());
            });

            function onTxtAPInProcessGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPInProcessID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPInProcessGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPInProcessSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPInProcessSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPInProcessIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPInProcessCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPInProcessDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPInProcessMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPInProcessFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPInProcessSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPInProcessID.ClientID %>').val('');
                        $('#<%=txtAPInProcessGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPInProcessSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPInProcessSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPInProcessIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPInProcessCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPInProcessDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPInProcessMethodName.ClientID %>').val('');
                        $('#<%=hdnAPInProcessFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPInProcessSubLedger.ClientID %>').val('');
                    $('#<%=txtAPInProcessSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPInProcessSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPInProcessSubLedgerIDChanged() {
                if ($('#<%=hdnAPInProcessSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPInProcessSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPInProcessSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPInProcessSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPInProcessSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPInProcessSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region APInProcess Sub Ledger
            function onGetAPInProcessSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPInProcessFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPInProcessSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPInProcessSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPInProcessSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPInProcessSearchDialogTypeName.ClientID %>').val(), onGetAPInProcessSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPInProcessSubLedgerCode.ClientID %>').val(value);
                        onTxtAPInProcessSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPInProcessSubLedgerCode.ClientID %>').change(function () {
                onTxtAPInProcessSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPInProcessSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPInProcessSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPInProcessSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPInProcessCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPInProcessMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPInProcessSubLedger.ClientID %>').val(result[$('#<%=hdnAPInProcessIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPInProcessSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPInProcessDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPInProcessSubLedger.ClientID %>').val('');
                            $('#<%=txtAPInProcessSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPInProcessSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region AP Discount
            $('#lblAPDiscount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPDiscountGLAccountNo.ClientID %>').val(value);
                    onTxtAPDiscountGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPDiscountGLAccountNo.ClientID %>').change(function () {
                onTxtAPDiscountGLAccountCodeChanged($(this).val());
            });

            function onTxtAPDiscountGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPDiscountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPDiscountGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPDiscountSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPDiscountSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPDiscountIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPDiscountCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPDiscountDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPDiscountMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPDiscountFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPDiscountSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPDiscountID.ClientID %>').val('');
                        $('#<%=txtAPDiscountGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPDiscountSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPDiscountSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPDiscountIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDiscountCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDiscountDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDiscountMethodName.ClientID %>').val('');
                        $('#<%=hdnAPDiscountFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPDiscountSubLedger.ClientID %>').val('');
                    $('#<%=txtAPDiscountSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPDiscountSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPDiscountSubLedgerIDChanged() {
                if ($('#<%=hdnAPDiscountSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPDiscountSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPDiscountSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPDiscountSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPDiscountSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPDiscountSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region APDiscount Sub Ledger
            function onGetAPDiscountSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPDiscountFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPDiscountSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPDiscountSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPDiscountSearchDialogTypeName.ClientID %>').val(), onGetAPDiscountSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPDiscountSubLedgerCode.ClientID %>').val(value);
                        onTxtAPDiscountSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPDiscountSubLedgerCode.ClientID %>').change(function () {
                onTxtAPDiscountSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPDiscountSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPDiscountSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPDiscountCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPDiscountMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPDiscountSubLedger.ClientID %>').val(result[$('#<%=hdnAPDiscountIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPDiscountSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPDiscountDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPDiscountSubLedger.ClientID %>').val('');
                            $('#<%=txtAPDiscountSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPDiscountSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region APStamp
            $('#lblAPStamp.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPStampGLAccountNo.ClientID %>').val(value);
                    onTxtAPStampGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPStampGLAccountNo.ClientID %>').change(function () {
                onTxtAPStampGLAccountCodeChanged($(this).val());
            });

            function onTxtAPStampGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPStampID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPStampGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPStampSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPStampSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPStampIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPStampCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPStampDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPStampMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPStampFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPStampSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPStampID.ClientID %>').val('');
                        $('#<%=txtAPStampGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPStampSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPStampSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPStampIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPStampCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPStampDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPStampMethodName.ClientID %>').val('');
                        $('#<%=hdnAPStampFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPStampSubLedger.ClientID %>').val('');
                    $('#<%=txtAPStampSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPStampSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPStampSubLedgerIDChanged() {
                if ($('#<%=hdnAPStampSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPStampSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPStampSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPStampSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPStampSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPStampSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region APStamp Sub Ledger
            function onGetAPStampSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPStampFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPStampSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPStampSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPStampSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPStampSearchDialogTypeName.ClientID %>').val(), onGetAPStampSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPStampSubLedgerCode.ClientID %>').val(value);
                        onTxtAPStampSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPStampSubLedgerCode.ClientID %>').change(function () {
                onTxtAPStampSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPStampSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPStampSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPStampSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPStampCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPStampMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPStampSubLedger.ClientID %>').val(result[$('#<%=hdnAPStampIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPStampSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPStampDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPStampSubLedger.ClientID %>').val('');
                            $('#<%=txtAPStampSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPStampSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region APDownPayment
            $('#lblAPDownPayment.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPDownPaymentGLAccountNo.ClientID %>').val(value);
                    onTxtAPDownPaymentGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPDownPaymentGLAccountNo.ClientID %>').change(function () {
                onTxtAPDownPaymentGLAccountCodeChanged($(this).val());
            });

            function onTxtAPDownPaymentGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPDownPaymentID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPDownPaymentGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPDownPaymentSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPDownPaymentSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPDownPaymentIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPDownPaymentCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPDownPaymentDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPDownPaymentMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPDownPaymentFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPDownPaymentSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPDownPaymentID.ClientID %>').val('');
                        $('#<%=txtAPDownPaymentGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentMethodName.ClientID %>').val('');
                        $('#<%=hdnAPDownPaymentFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPDownPaymentSubLedger.ClientID %>').val('');
                    $('#<%=txtAPDownPaymentSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPDownPaymentSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPDownPaymentSubLedgerIDChanged() {
                if ($('#<%=hdnAPDownPaymentSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPDownPaymentSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPDownPaymentSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPDownPaymentSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPDownPaymentSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPDownPaymentSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region APDownPayment Sub Ledger
            function onGetAPDownPaymentSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPDownPaymentFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPDownPaymentSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPDownPaymentSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPDownPaymentSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPDownPaymentSearchDialogTypeName.ClientID %>').val(), onGetAPDownPaymentSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPDownPaymentSubLedgerCode.ClientID %>').val(value);
                        onTxtAPDownPaymentSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPDownPaymentSubLedgerCode.ClientID %>').change(function () {
                onTxtAPDownPaymentSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPDownPaymentSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPDownPaymentSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPDownPaymentSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPDownPaymentCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPDownPaymentMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPDownPaymentSubLedger.ClientID %>').val(result[$('#<%=hdnAPDownPaymentIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPDownPaymentSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPDownPaymentDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPDownPaymentSubLedger.ClientID %>').val('');
                            $('#<%=txtAPDownPaymentSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPDownPaymentSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region APCharge
            $('#lblAPCharge.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPChargeGLAccountNo.ClientID %>').val(value);
                    onTxtAPChargeGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPChargeGLAccountNo.ClientID %>').change(function () {
                onTxtAPChargeGLAccountCodeChanged($(this).val());
            });

            function onTxtAPChargeGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPChargeID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPChargeGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPChargeSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPChargeSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPChargeIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPChargeCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPChargeDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPChargeMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPChargeFilterExpression.ClientID %>').val(result.FilterExpression);
                        onAPChargeSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnAPChargeID.ClientID %>').val('');
                        $('#<%=txtAPChargeGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPChargeSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPChargeSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPChargeIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPChargeCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPChargeDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPChargeMethodName.ClientID %>').val('');
                        $('#<%=hdnAPChargeFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnAPChargeSubLedger.ClientID %>').val('');
                    $('#<%=txtAPChargeSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtAPChargeSubLedgerName.ClientID %>').val('');
                });
            }

            function onAPChargeSubLedgerIDChanged() {
                if ($('#<%=hdnAPChargeSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnAPChargeSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblAPChargeSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtAPChargeSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblAPChargeSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtAPChargeSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region APRounding
            $('#lblAPRounding.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPRoundingGLAccountNo.ClientID %>').val(value);
                    onTxtAPRoundingGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPRoundingGLAccountNo.ClientID %>').change(function () {
                onTxtAPRoundingGLAccountCodeChanged($(this).val());
            });

            function onTxtAPRoundingGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPRoundingID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPRoundingGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnAPRoundingSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnAPRoundingSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnAPRoundingIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnAPRoundingCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnAPRoundingDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnAPRoundingMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnAPRoundingFilterExpression.ClientID %>').val(result.FilterExpression);
                    }
                    else {
                        $('#<%=hdnAPRoundingID.ClientID %>').val('');
                        $('#<%=txtAPRoundingGLAccountName.ClientID %>').val('');
                        $('#<%=hdnAPRoundingSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnAPRoundingSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnAPRoundingIDFieldName.ClientID %>').val('');
                        $('#<%=hdnAPRoundingCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnAPRoundingDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnAPRoundingMethodName.ClientID %>').val('');
                        $('#<%=hdnAPRoundingFilterExpression.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region APCharge Sub Ledger
            function onGetAPChargeSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnAPChargeFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnAPChargeSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblAPChargeSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnAPChargeSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnAPChargeSearchDialogTypeName.ClientID %>').val(), onGetAPChargeSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtAPChargeSubLedgerCode.ClientID %>').val(value);
                        onTxtAPChargeSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtAPChargeSubLedgerCode.ClientID %>').change(function () {
                onTxtAPChargeSubLedgerCodeChanged($(this).val());
            });

            function onTxtAPChargeSubLedgerCodeChanged(value) {
                if ($('#<%=hdnAPChargeSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetAPChargeSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnAPChargeCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnAPChargeMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnAPChargeSubLedger.ClientID %>').val(result[$('#<%=hdnAPChargeIDFieldName.ClientID %>').val()]);
                            $('#<%=txtAPChargeSubLedgerName.ClientID %>').val(result[$('#<%=hdnAPChargeDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnAPChargeSubLedger.ClientID %>').val('');
                            $('#<%=txtAPChargeSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtAPChargeSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region ARPurchaseReturn
            $('#lblARPurchaseReturn.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARPurchaseReturnGLAccountNo.ClientID %>').val(value);
                    onTxtARPurchaseReturnGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARPurchaseReturnGLAccountNo.ClientID %>').change(function () {
                onTxtARPurchaseReturnGLAccountCodeChanged($(this).val());
            });

            function onTxtARPurchaseReturnGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARPurchaseReturnID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARPurchaseReturnGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARPurchaseReturnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARPurchaseReturnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARPurchaseReturnIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARPurchaseReturnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARPurchaseReturnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARPurchaseReturnMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARPurchaseReturnFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARPurchaseReturnSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARPurchaseReturnID.ClientID %>').val('');
                        $('#<%=txtARPurchaseReturnGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnMethodName.ClientID %>').val('');
                        $('#<%=hdnARPurchaseReturnFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARPurchaseReturnSubLedger.ClientID %>').val('');
                    $('#<%=txtARPurchaseReturnSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARPurchaseReturnSubLedgerName.ClientID %>').val('');
                });
            }

            function onARPurchaseReturnSubLedgerIDChanged() {
                if ($('#<%=hdnARPurchaseReturnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARPurchaseReturnSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARPurchaseReturnSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARPurchaseReturnSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARPurchaseReturnSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARPurchaseReturnSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region ARPurchaseReturn Sub Ledger
            function onGetARPurchaseReturnSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARPurchaseReturnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARPurchaseReturnSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARPurchaseReturnSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARPurchaseReturnSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARPurchaseReturnSearchDialogTypeName.ClientID %>').val(), onGetARPurchaseReturnSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARPurchaseReturnSubLedgerCode.ClientID %>').val(value);
                        onTxtARPurchaseReturnSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARPurchaseReturnSubLedgerCode.ClientID %>').change(function () {
                onTxtARPurchaseReturnSubLedgerCodeChanged($(this).val());
            });

            function onTxtARPurchaseReturnSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARPurchaseReturnSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARPurchaseReturnSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARPurchaseReturnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARPurchaseReturnMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARPurchaseReturnSubLedger.ClientID %>').val(result[$('#<%=hdnARPurchaseReturnIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARPurchaseReturnSubLedgerName.ClientID %>').val(result[$('#<%=hdnARPurchaseReturnDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARPurchaseReturnSubLedger.ClientID %>').val('');
                            $('#<%=txtARPurchaseReturnSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARPurchaseReturnSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region ARCreditNote
            $('#lblARCreditNote.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARCreditNoteGLAccountNo.ClientID %>').val(value);
                    onTxtARCreditNoteGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARCreditNoteGLAccountNo.ClientID %>').change(function () {
                onTxtARCreditNoteGLAccountCodeChanged($(this).val());
            });

            function onTxtARCreditNoteGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARCreditNoteID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARCreditNoteGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARCreditNoteSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARCreditNoteSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARCreditNoteIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARCreditNoteCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARCreditNoteDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARCreditNoteMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARCreditNoteFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARCreditNoteSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARCreditNoteID.ClientID %>').val('');
                        $('#<%=txtARCreditNoteGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteMethodName.ClientID %>').val('');
                        $('#<%=hdnARCreditNoteFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARCreditNoteSubLedger.ClientID %>').val('');
                    $('#<%=txtARCreditNoteSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARCreditNoteSubLedgerName.ClientID %>').val('');
                });
            }

            function onARCreditNoteSubLedgerIDChanged() {
                if ($('#<%=hdnARCreditNoteSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARCreditNoteSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARCreditNoteSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARCreditNoteSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARCreditNoteSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARCreditNoteSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region ARCreditNote Sub Ledger
            function onGetARCreditNoteSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARCreditNoteFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARCreditNoteSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARCreditNoteSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARCreditNoteSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARCreditNoteSearchDialogTypeName.ClientID %>').val(), onGetARCreditNoteSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARCreditNoteSubLedgerCode.ClientID %>').val(value);
                        onTxtARCreditNoteSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARCreditNoteSubLedgerCode.ClientID %>').change(function () {
                onTxtARCreditNoteSubLedgerCodeChanged($(this).val());
            });

            function onTxtARCreditNoteSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARCreditNoteSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARCreditNoteSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARCreditNoteCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARCreditNoteMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARCreditNoteSubLedger.ClientID %>').val(result[$('#<%=hdnARCreditNoteIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARCreditNoteSubLedgerName.ClientID %>').val(result[$('#<%=hdnARCreditNoteDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARCreditNoteSubLedger.ClientID %>').val('');
                            $('#<%=txtARCreditNoteSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARCreditNoteSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            onAPSubLedgerIDChanged();
            onAPInProcessSubLedgerIDChanged();
            onAPDiscountSubLedgerIDChanged();
            onAPStampSubLedgerIDChanged();
            onAPDownPaymentSubLedgerIDChanged();
            onAPChargeSubLedgerIDChanged();
            onARPurchaseReturnSubLedgerIDChanged();
            onARCreditNoteSubLedgerIDChanged();
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("supplierType", $('#<%=hdnGCSupplierType.ClientID %>').val()));
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCSupplierType" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:190px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Supplier Line")%></label></td>
                        <td><asp:TextBox ID="txtSupplierLineCode" Width="30%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Supplier Line")%></label></td>
                        <td><asp:TextBox ID="txtSupplierLineName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Catatan")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <colgroup>
                        <col style="width: 50%"/>
                    </colgroup>
                    <tr>
                        <td>
                            <table width="50%">
                                <colgroup>
                                    <col width="190px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAP"><%=GetLabel("COA Hutang Usaha")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPID" runat="server" />
                                        <input type="hidden" id="hdnAPSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAPInProcess"><%=GetLabel("COA Hutang Dalam Proses")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPInProcessID" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPInProcessFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPInProcessGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPInProcessGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAPDiscount"><%=GetLabel("COA Diskon Tukar Faktur")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPDiscountID" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPDiscountFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPDiscountGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPDiscountGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAPStamp"><%=GetLabel("COA Materai Tukar Faktur")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPStampID" runat="server" />
                                        <input type="hidden" id="hdnAPStampSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPStampSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPStampIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPStampCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPStampDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPStampMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPStampFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPStampGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPStampGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAPDownPayment"><%=GetLabel("COA Uang Muka Tukar Faktur")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPDownPaymentID" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPDownPaymentFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPDownPaymentGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPDownPaymentGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAPCharge"><%=GetLabel("COA Biaya Tukar Faktur")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPChargeID" runat="server" />
                                        <input type="hidden" id="hdnAPChargeSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPChargeSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPChargeIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPChargeCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPChargeDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPChargeMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPChargeFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPChargeGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPChargeGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAPRounding"><%=GetLabel("COA Pembulatan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPRoundingID" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingMethodName" runat="server" />
                                        <input type="hidden" id="hdnAPRoundingFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPRoundingGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPRoundingGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td class="tdLabel"><label class="lblLink" id="lblARPurchaseReturn"><%=GetLabel("COA Piutang Retur")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnARPurchaseReturnID" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnMethodName" runat="server" />
                                        <input type="hidden" id="hdnARPurchaseReturnFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtARPurchaseReturnGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtARPurchaseReturnGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td class="tdLabel"><label class="lblLink" id="lblARCreditNote"><%=GetLabel("COA Piutang Nota Kredit")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnARCreditNoteID" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteMethodName" runat="server" />
                                        <input type="hidden" id="hdnARCreditNoteFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtARCreditNoteGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtARCreditNoteGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="display: none;">
                            <table width="100%">
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPInProcessSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPInProcessSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPInProcessSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPInProcessSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPDiscountSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPDiscountSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPDiscountSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPDiscountSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPStampSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPStampSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPStampSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPStampSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPDownPaymentSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPDownPaymentSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPDownPaymentSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPDownPaymentSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblAPChargeSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPChargeSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPChargeSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPChargeSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblARPurchaseReturnSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnARPurchaseReturnSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtARPurchaseReturnSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtARPurchaseReturnSubLedgerName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblARCreditNoteSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnARCreditNoteSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtARCreditNoteSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtARCreditNoteSubLedgerName" Width="100%" /></td>
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