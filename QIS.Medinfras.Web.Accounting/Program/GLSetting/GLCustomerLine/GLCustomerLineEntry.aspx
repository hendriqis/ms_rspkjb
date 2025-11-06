<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLCustomerLineEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLCustomerLineEntry" %>

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

            //#region AR
            $('#lblAR.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARGLAccountNo.ClientID %>').val(value);
                    onTxtARGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARGLAccountNo.ClientID %>').change(function () {
                onTxtARGLAccountCodeChanged($(this).val());
            });

            function onTxtARGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARID.ClientID %>').val('');
                        $('#<%=txtARGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARMethodName.ClientID %>').val('');
                        $('#<%=hdnARFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARSubLedger.ClientID %>').val('');
                    $('#<%=txtARSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARSubLedgerName.ClientID %>').val('');
                });
            }

            function onARSubLedgerIDChanged() {
                if ($('#<%=hdnARSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region AR Sub Ledger
            function onGetARSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARSearchDialogTypeName.ClientID %>').val(), onGetARSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARSubLedgerCode.ClientID %>').val(value);
                        onTxtARSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARSubLedgerCode.ClientID %>').change(function () {
                onTxtARSubLedgerCodeChanged($(this).val());
            });

            function onTxtARSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARSubLedger.ClientID %>').val(result[$('#<%=hdnARIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARSubLedgerName.ClientID %>').val(result[$('#<%=hdnARDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARSubLedger.ClientID %>').val('');
                            $('#<%=txtARSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region ARInProcess
            $('#lblARInProcess.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARInProcessGLAccountNo.ClientID %>').val(value);
                    onTxtARInProcessGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARInProcessGLAccountNo.ClientID %>').change(function () {
                onTxtARInProcessGLAccountCodeChanged($(this).val());
            });

            function onTxtARInProcessGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARInProcessID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARInProcessGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARInProcessSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARInProcessSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARInProcessIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARInProcessCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARInProcessDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARInProcessMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARInProcessFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARInProcessSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARInProcessID.ClientID %>').val('');
                        $('#<%=txtARInProcessGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARInProcessSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARInProcessSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARInProcessIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARInProcessCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARInProcessDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARInProcessMethodName.ClientID %>').val('');
                        $('#<%=hdnARInProcessFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARInProcessSubLedger.ClientID %>').val('');
                    $('#<%=txtARInProcessSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARInProcessSubLedgerName.ClientID %>').val('');
                });
            }

            function onARInProcessSubLedgerIDChanged() {
                if ($('#<%=hdnARInProcessSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARInProcessSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARInProcessSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARInProcessSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARInProcessSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARInProcessSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region ARInProcess Sub Ledger
            function onGetARInProcessSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARInProcessFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARInProcessSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARInProcessSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARInProcessSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARInProcessSearchDialogTypeName.ClientID %>').val(), onGetARInProcessSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARInProcessSubLedgerCode.ClientID %>').val(value);
                        onTxtARInProcessSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARInProcessSubLedgerCode.ClientID %>').change(function () {
                onTxtARInProcessSubLedgerCodeChanged($(this).val());
            });

            function onTxtARInProcessSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARInProcessSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARInProcessSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARInProcessCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARInProcessMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARInProcessSubLedger.ClientID %>').val(result[$('#<%=hdnARInProcessIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARInProcessSubLedgerName.ClientID %>').val(result[$('#<%=hdnARInProcessDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARInProcessSubLedger.ClientID %>').val('');
                            $('#<%=txtARInProcessSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARInProcessSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region ARInCare
            $('#lblARInCare.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARInCareGLAccountNo.ClientID %>').val(value);
                    onTxtARInCareGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARInCareGLAccountNo.ClientID %>').change(function () {
                onTxtARInCareGLAccountCodeChanged($(this).val());
            });

            function onTxtARInCareGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARInCareID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARInCareGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARInCareSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARInCareSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARInCareIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARInCareCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARInCareDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARInCareMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARInCareFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARInCareSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARInCareID.ClientID %>').val('');
                        $('#<%=txtARInCareGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARInCareSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARInCareSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARInCareIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARInCareCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARInCareDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARInCareMethodName.ClientID %>').val('');
                        $('#<%=hdnARInCareFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARInCareSubLedger.ClientID %>').val('');
                    $('#<%=txtARInCareSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARInCareSubLedgerName.ClientID %>').val('');
                });
            }

            function onARInCareSubLedgerIDChanged() {
                if ($('#<%=hdnARInCareSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARInCareSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARInCareSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARInCareSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARInCareSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARInCareSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region ARInCare Sub Ledger
            function onGetARInCareSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARInCareFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARInCareSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARInCareSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARInCareSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARInCareSearchDialogTypeName.ClientID %>').val(), onGetARInCareSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARInCareSubLedgerCode.ClientID %>').val(value);
                        onTxtARInCareSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARInCareSubLedgerCode.ClientID %>').change(function () {
                onTxtARInCareSubLedgerCodeChanged($(this).val());
            });

            function onTxtARInCareSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARInCareSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARInCareSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARInCareCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARInCareMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARInCareSubLedger.ClientID %>').val(result[$('#<%=hdnARInCareIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARInCareSubLedgerName.ClientID %>').val(result[$('#<%=hdnARInCareDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARInCareSubLedger.ClientID %>').val('');
                            $('#<%=txtARInCareSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARInCareSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region ARAdjustment
            $('#lblARAdjustment.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').val(value);
                    onTxtARAdjustmentGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').change(function () {
                onTxtARAdjustmentGLAccountCodeChanged($(this).val());
            });

            function onTxtARAdjustmentGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARAdjustmentID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARAdjustmentGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARAdjustmentSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARAdjustmentSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARAdjustmentIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARAdjustmentCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARAdjustmentDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARAdjustmentMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARAdjustmentFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARAdjustmentSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARAdjustmentID.ClientID %>').val('');
                        $('#<%=txtARAdjustmentGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentMethodName.ClientID %>').val('');
                        $('#<%=hdnARAdjustmentFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARAdjustmentSubLedger.ClientID %>').val('');
                    $('#<%=txtARAdjustmentSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARAdjustmentSubLedgerName.ClientID %>').val('');
                });
            }

            function onARAdjustmentSubLedgerIDChanged() {
                if ($('#<%=hdnARAdjustmentSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARAdjustmentSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARAdjustmentSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARAdjustmentSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARAdjustmentSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARAdjustmentSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region ARAdjustment Sub Ledger
            function onGetARAdjustmentSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARAdjustmentFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARAdjustmentSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARAdjustmentSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARAdjustmentSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARAdjustmentSearchDialogTypeName.ClientID %>').val(), onGetARAdjustmentSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARAdjustmentSubLedgerCode.ClientID %>').val(value);
                        onTxtARAdjustmentSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARAdjustmentSubLedgerCode.ClientID %>').change(function () {
                onTxtARAdjustmentSubLedgerCodeChanged($(this).val());
            });

            function onTxtARAdjustmentSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARAdjustmentSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARAdjustmentSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARAdjustmentCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARAdjustmentMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARAdjustmentSubLedger.ClientID %>').val(result[$('#<%=hdnARAdjustmentIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARAdjustmentSubLedgerName.ClientID %>').val(result[$('#<%=hdnARAdjustmentDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARAdjustmentSubLedger.ClientID %>').val('');
                            $('#<%=txtARAdjustmentSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARAdjustmentSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region ARDiscount
            $('#lblARDiscount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtARDiscountGLAccountNo.ClientID %>').val(value);
                    onTxtARDiscountGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtARDiscountGLAccountNo.ClientID %>').change(function () {
                onTxtARDiscountGLAccountCodeChanged($(this).val());
            });

            function onTxtARDiscountGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnARDiscountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtARDiscountGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnARDiscountSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnARDiscountSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnARDiscountIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnARDiscountCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnARDiscountDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnARDiscountMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnARDiscountFilterExpression.ClientID %>').val(result.FilterExpression);
                        onARDiscountSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnARDiscountID.ClientID %>').val('');
                        $('#<%=txtARDiscountGLAccountName.ClientID %>').val('');
                        $('#<%=hdnARDiscountSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnARDiscountSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnARDiscountIDFieldName.ClientID %>').val('');
                        $('#<%=hdnARDiscountCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnARDiscountDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnARDiscountMethodName.ClientID %>').val('');
                        $('#<%=hdnARDiscountFilterExpression.ClientID %>').val('');
                    }

                    $('#<%=hdnARDiscountSubLedger.ClientID %>').val('');
                    $('#<%=txtARDiscountSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtARDiscountSubLedgerName.ClientID %>').val('');
                });
            }

            function onARDiscountSubLedgerIDChanged() {
                if ($('#<%=hdnARDiscountSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnARDiscountSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblARDiscountSubLedger.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtARDiscountSubLedgerCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblARDiscountSubLedger.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtARDiscountSubLedgerCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region ARDiscount Sub Ledger
            function onGetARDiscountSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnARDiscountFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnARDiscountSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblARDiscountSubLedger.ClientID %>').click(function () {
                if ($('#<%=hdnARDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnARDiscountSearchDialogTypeName.ClientID %>').val(), onGetARDiscountSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtARDiscountSubLedgerCode.ClientID %>').val(value);
                        onTxtARDiscountSubLedgerCodeChanged(value);
                    });
                }
            });

            $('#<%=txtARDiscountSubLedgerCode.ClientID %>').change(function () {
                onTxtARDiscountSubLedgerCodeChanged($(this).val());
            });

            function onTxtARDiscountSubLedgerCodeChanged(value) {
                if ($('#<%=hdnARDiscountSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetARDiscountSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnARDiscountCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnARDiscountMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnARDiscountSubLedger.ClientID %>').val(result[$('#<%=hdnARDiscountIDFieldName.ClientID %>').val()]);
                            $('#<%=txtARDiscountSubLedgerName.ClientID %>').val(result[$('#<%=hdnARDiscountDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnARDiscountSubLedger.ClientID %>').val('');
                            $('#<%=txtARDiscountSubLedgerCode.ClientID %>').val('');
                            $('#<%=txtARDiscountSubLedgerName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            onARSubLedgerIDChanged();
            onARInProcessSubLedgerIDChanged();
            onARInCareSubLedgerIDChanged();
            onARAdjustmentSubLedgerIDChanged();
            onARDiscountSubLedgerIDChanged();
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("customerType", $('#<%=hdnGCCustomerType.ClientID %>').val()));
        }

    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCCustomerType" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 190px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Customer Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerLineCode" Width="30%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Customer Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerLineName" Width="100%" runat="server" />
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
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td>
                            <table width="50%">
                                <colgroup>
                                    <col width="190px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblAR">
                                            <%=GetLabel("COA Piutang Usaha")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARID" runat="server" />
                                        <input type="hidden" id="hdnARSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARMethodName" runat="server" />
                                        <input type="hidden" id="hdnARFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblARInProcess">
                                            <%=GetLabel("COA Piutang dalam Proses")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARInProcessID" runat="server" />
                                        <input type="hidden" id="hdnARInProcessSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARInProcessSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARInProcessIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARInProcessCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARInProcessDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARInProcessMethodName" runat="server" />
                                        <input type="hidden" id="hdnARInProcessFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInProcessGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInProcessGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblARInCare">
                                            <%=GetLabel("COA Piutang dalam Perawatan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARInCareID" runat="server" />
                                        <input type="hidden" id="hdnARInCareSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARInCareSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARInCareIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARInCareCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARInCareDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARInCareMethodName" runat="server" />
                                        <input type="hidden" id="hdnARInCareFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInCareGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInCareGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblARAdjustment">
                                            <%=GetLabel("COA Penyesuaian Piutang")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARAdjustmentID" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentMethodName" runat="server" />
                                        <input type="hidden" id="hdnARAdjustmentFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARAdjustmentGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARAdjustmentGLAccountName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblARDiscount">
                                            <%=GetLabel("COA Diskon Piutang")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARDiscountID" runat="server" />
                                        <input type="hidden" id="hdnARDiscountSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnARDiscountSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnARDiscountIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnARDiscountCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnARDiscountDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnARDiscountMethodName" runat="server" />
                                        <input type="hidden" id="hdnARDiscountFilterExpression" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARDiscountGLAccountNo" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARDiscountGLAccountName" Width="100%" />
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
                                        <label class="lblDisabled" runat="server" id="lblARSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblARInProcessSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARInProcessSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInProcessSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInProcessSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblARInCareSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARInCareSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInCareSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARInCareSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblARAdjustmentSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARAdjustmentSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARAdjustmentSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARAdjustmentSubLedgerName" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblDisabled" runat="server" id="lblARDiscountSubLedger">
                                            <%=GetLabel("Sub Perkiraan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnARDiscountSubLedger" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARDiscountSubLedgerCode" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtARDiscountSubLedgerName" Width="100%" />
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
