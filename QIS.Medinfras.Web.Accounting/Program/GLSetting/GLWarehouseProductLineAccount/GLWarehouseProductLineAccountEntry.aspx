<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="GLWarehouseProductLineAccountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLWarehouseProductLineAccountEntry" %>

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

            //#region Healthcare Service Unit
            function onGetProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblProductLine.lblLink').click(function () {
                openSearchDialog('productline', onGetProductLineFilterExpression(), function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = onGetProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
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

            //#region GL Account 1
            $('#lblGLAccount1.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccount1Code.ClientID %>').val(value);
                    onTxtGLAccount1CodeChanged(value);
                });
            });

            $('#<%=txtGLAccount1Code.ClientID %>').change(function () {
                onTxtGLAccount1CodeChanged($(this).val());
            });

            function onTxtGLAccount1CodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccount1ID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccount1Name.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID1.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName1.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression1.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName1.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName1.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName1.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName1.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnGLAccount1ID.ClientID %>').val('');
                        $('#<%=txtGLAccount1Code.ClientID %>').val('');
                        $('#<%=txtGLAccount1Name.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID1.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName1.ClientID %>').val('');
                        $('#<%=hdnFilterExpression1.ClientID %>').val('');
                        $('#<%=hdnIDFieldName1.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName1.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName1.ClientID %>').val('');
                        $('#<%=hdnMethodName1.ClientID %>').val('');
                    }
                    onSubLedgerID1Changed();
                    $('#<%=hdnSubLedgerDt1ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt1Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt1Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID1Changed() {
                if ($('#<%=hdnSubLedgerID1.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID1.ClientID %>').val() == '') {
                    $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt1Code.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt1Code.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sub Ledger 1
            function onGetSubLedgerDt1FilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression1.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID1.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt1.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName1.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName1.ClientID %>').val(), onGetSubLedgerDt1FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDt1Code.ClientID %>').val(value);
                        onTxtSubLedgerDt1CodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDt1Code.ClientID %>').change(function () {
                onTxtSubLedgerDt1CodeChanged($(this).val());
            });

            function onTxtSubLedgerDt1CodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName1.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDt1FilterExpression() + " AND " + $('#<%=hdnCodeFieldName1.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName1.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDt1ID.ClientID %>').val(result[$('#<%=hdnIDFieldName1.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDt1Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName1.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDt1ID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt1Code.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt1Name.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region GL Account 2
            $('#lblGLAccount2.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccount2Code.ClientID %>').val(value);
                    onTxtGLAccount2CodeChanged(value);
                });
            });

            $('#<%=txtGLAccount2Code.ClientID %>').change(function () {
                onTxtGLAccount2CodeChanged($(this).val());
            });

            function onTxtGLAccount2CodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccount2ID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccount2Name.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID2.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName2.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression2.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName2.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName2.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName2.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName2.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnGLAccount2ID.ClientID %>').val('');
                        $('#<%=txtGLAccount2Code.ClientID %>').val('');
                        $('#<%=txtGLAccount2Name.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID2.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName2.ClientID %>').val('');
                        $('#<%=hdnFilterExpression2.ClientID %>').val('');
                        $('#<%=hdnIDFieldName2.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName2.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName2.ClientID %>').val('');
                        $('#<%=hdnMethodName2.ClientID %>').val('');
                    }
                    onSubLedgerID2Changed();
                    $('#<%=hdnSubLedgerDt2ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt2Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt2Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID2Changed() {
                if ($('#<%=hdnSubLedgerID2.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID2.ClientID %>').val() == '') {
                    $('#<%=lblSubLedgerDt2.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt2Code.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSubLedgerDt2.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt2Code.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sub Ledger 2
            function onGetSubLedgerDt2FilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression2.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID2.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt2.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName2.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName2.ClientID %>').val(), onGetSubLedgerDt2FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDt2Code.ClientID %>').val(value);
                        onTxtSubLedgerDt2CodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDt2Code.ClientID %>').change(function () {
                onTxtSubLedgerDt2CodeChanged($(this).val());
            });

            function onTxtSubLedgerDt2CodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName2.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDt2FilterExpression() + " AND " + $('#<%=hdnCodeFieldName2.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName2.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDt2ID.ClientID %>').val(result[$('#<%=hdnIDFieldName2.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDt2Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName2.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDt2ID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt2Code.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt2Name.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region GL Account 3
            $('#lblGLAccount3.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccount3Code.ClientID %>').val(value);
                    onTxtGLAccount3CodeChanged(value);
                });
            });

            $('#<%=txtGLAccount3Code.ClientID %>').change(function () {
                onTxtGLAccount3CodeChanged($(this).val());
            });

            function onTxtGLAccount3CodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccount3ID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccount3Name.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID3.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName3.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression3.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName3.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName3.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName3.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName3.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnGLAccount3ID.ClientID %>').val('');
                        $('#<%=txtGLAccount3Code.ClientID %>').val('');
                        $('#<%=txtGLAccount3Name.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID3.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName3.ClientID %>').val('');
                        $('#<%=hdnFilterExpression3.ClientID %>').val('');
                        $('#<%=hdnIDFieldName3.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName3.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName3.ClientID %>').val('');
                        $('#<%=hdnMethodName3.ClientID %>').val('');
                    }
                    onSubLedgerID3Changed();
                    $('#<%=hdnSubLedgerDt3ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt3Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt3Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID3Changed() {
                if ($('#<%=hdnSubLedgerID3.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID3.ClientID %>').val() == '') {
                    $('#<%=lblSubLedgerDt3.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt3Code.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSubLedgerDt3.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt3Code.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sub Ledger 3
            function onGetSubLedgerDt3FilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression3.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID3.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt3.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName3.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName3.ClientID %>').val(), onGetSubLedgerDt3FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDt3Code.ClientID %>').val(value);
                        onTxtSubLedgerDt3CodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDt3Code.ClientID %>').change(function () {
                onTxtSubLedgerDt3CodeChanged($(this).val());
            });

            function onTxtSubLedgerDt3CodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName3.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDt3FilterExpression() + " AND " + $('#<%=hdnCodeFieldName3.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName3.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDt3ID.ClientID %>').val(result[$('#<%=hdnIDFieldName3.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDt3Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName3.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDt3ID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt3Code.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt3Name.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region GL Account 4
            $('#lblGLAccount4.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccount4Code.ClientID %>').val(value);
                    onTxtGLAccount4CodeChanged(value);
                });
            });

            $('#<%=txtGLAccount4Code.ClientID %>').change(function () {
                onTxtGLAccount4CodeChanged($(this).val());
            });

            function onTxtGLAccount4CodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccount4ID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccount4Name.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID4.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName4.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression4.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName4.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName4.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName4.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName4.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnGLAccount4ID.ClientID %>').val('');
                        $('#<%=txtGLAccount4Code.ClientID %>').val('');
                        $('#<%=txtGLAccount4Name.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID4.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName4.ClientID %>').val('');
                        $('#<%=hdnFilterExpression4.ClientID %>').val('');
                        $('#<%=hdnIDFieldName4.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName4.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName4.ClientID %>').val('');
                        $('#<%=hdnMethodName4.ClientID %>').val('');
                    }
                    onSubLedgerID4Changed();
                    $('#<%=hdnSubLedgerDt4ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt4Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt4Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID4Changed() {
                if ($('#<%=hdnSubLedgerID4.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID4.ClientID %>').val() == '') {
                    $('#<%=lblSubLedgerDt4.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt4Code.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSubLedgerDt4.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt4Code.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sub Ledger 4
            function onGetSubLedgerDt4FilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression4.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID4.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt4.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName4.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName4.ClientID %>').val(), onGetSubLedgerDt4FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDt4Code.ClientID %>').val(value);
                        onTxtSubLedgerDt4CodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDt4Code.ClientID %>').change(function () {
                onTxtSubLedgerDt4CodeChanged($(this).val());
            });

            function onTxtSubLedgerDt4CodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName4.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDt4FilterExpression() + " AND " + $('#<%=hdnCodeFieldName4.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName4.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDt4ID.ClientID %>').val(result[$('#<%=hdnIDFieldName4.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDt4Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName4.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDt4ID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt4Code.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt4Name.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region GL Account 5
            $('#lblGLAccount5.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccount5Code.ClientID %>').val(value);
                    onTxtGLAccount5CodeChanged(value);
                });
            });

            $('#<%=txtGLAccount5Code.ClientID %>').change(function () {
                onTxtGLAccount5CodeChanged($(this).val());
            });

            function onTxtGLAccount5CodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccount5ID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccount5Name.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID5.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName5.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression5.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName5.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName5.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName5.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName5.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnGLAccount5ID.ClientID %>').val('');
                        $('#<%=txtGLAccount5Code.ClientID %>').val('');
                        $('#<%=txtGLAccount5Name.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID5.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName5.ClientID %>').val('');
                        $('#<%=hdnFilterExpression5.ClientID %>').val('');
                        $('#<%=hdnIDFieldName5.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName5.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName5.ClientID %>').val('');
                        $('#<%=hdnMethodName5.ClientID %>').val('');
                    }
                    onSubLedgerID5Changed();
                    $('#<%=hdnSubLedgerDt5ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt5Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt5Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID5Changed() {
                if ($('#<%=hdnSubLedgerID5.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID5.ClientID %>').val() == '') {
                    $('#<%=lblSubLedgerDt5.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt5Code.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSubLedgerDt5.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt5Code.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sub Ledger 5
            function onGetSubLedgerDt5FilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression5.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID5.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt5.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName5.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName5.ClientID %>').val(), onGetSubLedgerDt5FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDt5Code.ClientID %>').val(value);
                        onTxtSubLedgerDt5CodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDt5Code.ClientID %>').change(function () {
                onTxtSubLedgerDt5CodeChanged($(this).val());
            });

            function onTxtSubLedgerDt5CodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName5.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDt5FilterExpression() + " AND " + $('#<%=hdnCodeFieldName5.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName5.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDt5ID.ClientID %>').val(result[$('#<%=hdnIDFieldName5.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDt5Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName5.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDt5ID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt5Code.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt5Name.ClientID %>').val('');
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
                        $('#<%=hdnPurchasePriceVariantID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtPurchasePriceVariantGLAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnPurchasePriceVariantIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnPurchasePriceVariantCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnPurchasePriceVariantDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnPurchasePriceVariantMethodName.ClientID %>').val(result.MethodName);
                        $('#<%=hdnPurchasePriceVariantFilterExpression.ClientID %>').val(result.FilterExpression);
                    }
                    else {
                        $('#<%=hdnPurchasePriceVariantID.ClientID %>').val('');
                        $('#<%=txtPurchasePriceVariantGLAccountName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantIDFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantMethodName.ClientID %>').val('');
                        $('#<%=hdnPurchasePriceVariantFilterExpression.ClientID %>').val('');
                    }
                    onPurchasePriceVariantSubLedgerIDChanged();
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
                        $('#<%=hdnAP.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPGLAccountName.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnAP.ClientID %>').val('');
                        $('#<%=txtAPGLAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region APInProgress
            $('#lblAPInProgress.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAPInProgressGLAccountNo.ClientID %>').val(value);
                    onTxtAPInProgressGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtAPInProgressGLAccountNo.ClientID %>').change(function () {
                onTxtAPInProgressGLAccountCodeChanged($(this).val());
            });

            function onTxtAPInProgressGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAPInProgress.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAPInProgressGLAccountName.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnAPInProgress.ClientID %>').val('');
                        $('#<%=txtAPInProgressGLAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            onSubLedgerID1Changed();
            onSubLedgerID2Changed();
            onSubLedgerID3Changed();
            onSubLedgerID4Changed();
            onSubLedgerID5Changed();
            onPurchasePriceVariantSubLedgerIDChanged();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcare" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr runat="server" id="trHealthcareServiceUnit">
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label class="lblMandatory lblLink" id="lblProductLine"><%=GetLabel("Product Line")%></label></td>
                        <td>
                            <input type="hidden" id="hdnProductLineID" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtProductLineCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtProductLineName" Width="100%" ReadOnly="true" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tipe Item")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboGCItemType" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Catatan")%></label></td>
                        <td><asp:TextBox ID="txtNotes" Width="100%" runat="server" /></td>
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
                <table width="50%">
                    <colgroup>
                        <col style="width: 50%"/>
                    </colgroup>
                    <tr>
                        <td>
                            <table width="100%">
                                <colgroup>
                                    <col width="30%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount1"><%=GetLabel("COA Persediaan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount1ID" runat="server" />
                                        <input type="hidden" id="hdnSubLedgerID1" runat="server" />
                                        <input type="hidden" id="hdnSearchDialogTypeName1" runat="server" />
                                        <input type="hidden" id="hdnIDFieldName1" runat="server" />
                                        <input type="hidden" id="hdnCodeFieldName1" runat="server" />
                                        <input type="hidden" id="hdnDisplayFieldName1" runat="server" />
                                        <input type="hidden" id="hdnMethodName1" runat="server" />
                                        <input type="hidden" id="hdnFilterExpression1" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount1Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount1Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount4"><%=GetLabel("COA PPN Masukkan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount4ID" runat="server" />
                                        <input type="hidden" id="hdnSubLedgerID4" runat="server" />
                                        <input type="hidden" id="hdnSearchDialogTypeName4" runat="server" />
                                        <input type="hidden" id="hdnIDFieldName4" runat="server" />
                                        <input type="hidden" id="hdnCodeFieldName4" runat="server" />
                                        <input type="hidden" id="hdnDisplayFieldName4" runat="server" />
                                        <input type="hidden" id="hdnMethodName4" runat="server" />
                                        <input type="hidden" id="hdnFilterExpression4" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount4Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount4Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount5"><%=GetLabel("COA Diskon Pembelian")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount5ID" runat="server" />
                                        <input type="hidden" id="hdnSubLedgerID5" runat="server" />
                                        <input type="hidden" id="hdnSearchDialogTypeName5" runat="server" />
                                        <input type="hidden" id="hdnIDFieldName5" runat="server" />
                                        <input type="hidden" id="hdnCodeFieldName5" runat="server" />
                                        <input type="hidden" id="hdnDisplayFieldName5" runat="server" />
                                        <input type="hidden" id="hdnMethodName5" runat="server" />
                                        <input type="hidden" id="hdnFilterExpression5" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount5Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount5Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount2"><%=GetLabel("COA COGS")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount2ID" runat="server" />
                                        <input type="hidden" id="hdnSubLedgerID2" runat="server" />
                                        <input type="hidden" id="hdnSearchDialogTypeName2" runat="server" />
                                        <input type="hidden" id="hdnIDFieldName2" runat="server" />
                                        <input type="hidden" id="hdnCodeFieldName2" runat="server" />
                                        <input type="hidden" id="hdnDisplayFieldName2" runat="server" />
                                        <input type="hidden" id="hdnMethodName2" runat="server" />
                                        <input type="hidden" id="hdnFilterExpression2" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount2Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount2Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount3"><%=GetLabel("COA Pemakaian Barang")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount3ID" runat="server" />
                                        <input type="hidden" id="hdnSubLedgerID3" runat="server" />
                                        <input type="hidden" id="hdnSearchDialogTypeName3" runat="server" />
                                        <input type="hidden" id="hdnIDFieldName3" runat="server" />
                                        <input type="hidden" id="hdnCodeFieldName3" runat="server" />
                                        <input type="hidden" id="hdnDisplayFieldName3" runat="server" />
                                        <input type="hidden" id="hdnMethodName3" runat="server" />
                                        <input type="hidden" id="hdnFilterExpression3" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount3Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtGLAccount3Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblPurchasePriceVariant"><%=GetLabel("COA Perubahan Harga")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnPurchasePriceVariantID" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantSubLedgerID" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantSearchDialogTypeName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantIDFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantCodeFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantDisplayFieldName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantMethodName" runat="server" />
                                        <input type="hidden" id="hdnPurchasePriceVariantFilterExpression" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtPurchasePriceVariantGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtPurchasePriceVariantGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblAP"><%=GetLabel("COA Hutang")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAP" runat="server" />
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
                                    <td class="tdLabel"><label class="lblLink" id="lblAPInProgress"><%=GetLabel("COA Hutang dlm Proses")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnAPInProgress" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtAPInProgressGLAccountNo" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtAPInProgressGLAccountName" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="display: none;">
                            <table width="100%">
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblSubLedgerDt1"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnSubLedgerDt1ID" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt1Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt1Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblSubLedgerDt4"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnSubLedgerDt4ID" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt4Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt4Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblSubLedgerDt5"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnSubLedgerDt5ID" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt5Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt5Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblSubLedgerDt2"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnSubLedgerDt2ID" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt2Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt2Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblSubLedgerDt3"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnSubLedgerDt3ID" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt3Code" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtSubLedgerDt3Name" Width="100%" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblDisabled" runat="server" id="lblPurchasePriceVariantSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnPurchasePriceVariantSubLedger" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox runat="server" ID="txtPurchasePriceVariantSubLedgerCode" Width="100%" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox runat="server" ID="txtPurchasePriceVariantSubLedgerName" Width="100%" /></td>
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