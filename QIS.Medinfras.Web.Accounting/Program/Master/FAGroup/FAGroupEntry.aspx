<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="FAGroupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAGroupEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Method
            $('#lblMethod.lblLink').click(function () {
                openSearchDialog('fadepreciationmethod', onGetFilterExpression(), function (value) {
                    $('#<%=txtMethodCode.ClientID %>').val(value);
                    ontxtMethodCodeChanged(value);
                });
            });

            $('#<%=txtMethodCode.ClientID %>').change(function () {
                ontxtMethodCodeChanged($(this).val());
            });

            function ontxtMethodCodeChanged(value) {
                var filterExpression = onGetFilterExpression() + " AND MethodCode = '" + value + "'";
                Methods.getObject('GetFADepreciationMethodList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMethodID.ClientID %>').val(result.MethodID);
                        $('#<%=txtMethodName.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnMethodID.ClientID %>').val('');
                        $('#<%=txtMethodCode.ClientID %>').val('');
                        $('#<%=txtMethodName.ClientID %>').val('');
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
                    $('#<%=txtSourceProductLineCode.ClientID %>').val(value);
                    ontxtSourceProductLineCodeChanged(value);
                });
            });

            $('#<%=txtSourceProductLineCode.ClientID %>').change(function () {
                ontxtSourceProductLineCodeChanged($(this).val());
            });

            function ontxtSourceProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSourceProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtSourceProductLineName.ClientID %>').val(result.ProductLineName);
                    }
                    else {
                        $('#<%=hdnSourceProductLineID.ClientID %>').val('');
                        $('#<%=txtSourceProductLineCode.ClientID %>').val('');
                        $('#<%=txtSourceProductLineName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region GL Account & Sub Ledger
            function onGetGLAccountFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

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

                        onSubLedgerID1Changed();
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

                    $('#<%=hdnSubLedgerDt1ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt1Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt1Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID1Changed() {
                if ($('#<%=hdnSubLedgerID1.ClientID %>').val() != '0' && $('#<%=hdnSubLedgerID1.ClientID %>').val() != '') {
                    $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt1Code.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt1Code.ClientID %>').attr('readonly', 'readonly');
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

                        onSubLedgerID2Changed();
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

                    $('#<%=hdnSubLedgerDt2ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt2Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt2Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID2Changed() {
                if ($('#<%=hdnSubLedgerID2.ClientID %>').val() != '0' && $('#<%=hdnSubLedgerID2.ClientID %>').val() != '') {
                    $('#<%=lblSubLedgerDt2.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt2Code.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=lblSubLedgerDt2.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt2Code.ClientID %>').attr('readonly', 'readonly');
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

                        onSubLedgerID3Changed();
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

                    $('#<%=hdnSubLedgerDt3ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt3Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt3Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID3Changed() {
                if ($('#<%=hdnSubLedgerID3.ClientID %>').val() != '0' && $('#<%=hdnSubLedgerID3.ClientID %>').val() != '') {
                    $('#<%=lblSubLedgerDt3.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt3Code.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=lblSubLedgerDt3.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt3Code.ClientID %>').attr('readonly', 'readonly');
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

                        onSubLedgerID4Changed();
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

                    $('#<%=hdnSubLedgerDt4ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt4Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt4Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID4Changed() {
                if ($('#<%=hdnSubLedgerID4.ClientID %>').val() != '0' && $('#<%=hdnSubLedgerID4.ClientID %>').val() != '') {
                    $('#<%=lblSubLedgerDt4.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt4Code.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=lblSubLedgerDt4.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt4Code.ClientID %>').attr('readonly', 'readonly');
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

                        onSubLedgerID5Changed();
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

                    $('#<%=hdnSubLedgerDt5ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt5Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt5Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID5Changed() {
                if ($('#<%=hdnSubLedgerID5.ClientID %>').val() != '0' && $('#<%=hdnSubLedgerID5.ClientID %>').val() != '') {
                    $('#<%=lblSubLedgerDt5.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt5Code.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=lblSubLedgerDt5.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt5Code.ClientID %>').attr('readonly', 'readonly');
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

            //#region GL Account 6
            $('#lblGLAccount6.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccount6Code.ClientID %>').val(value);
                    onTxtGLAccount6CodeChanged(value);
                });
            });

            $('#<%=txtGLAccount6Code.ClientID %>').change(function () {
                onTxtGLAccount6CodeChanged($(this).val());
            });

            function onTxtGLAccount6CodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccount6ID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccount6Name.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID6.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName6.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression6.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName6.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName6.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName6.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName6.ClientID %>').val(result.MethodName);

                        onSubLedgerID6Changed();
                    }
                    else {
                        $('#<%=hdnGLAccount6ID.ClientID %>').val('');
                        $('#<%=txtGLAccount6Code.ClientID %>').val('');
                        $('#<%=txtGLAccount6Name.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID6.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName6.ClientID %>').val('');
                        $('#<%=hdnFilterExpression6.ClientID %>').val('');
                        $('#<%=hdnIDFieldName6.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName6.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName6.ClientID %>').val('');
                        $('#<%=hdnMethodName6.ClientID %>').val('');
                    }

                    $('#<%=hdnSubLedgerDt6ID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt6Code.ClientID %>').val('');
                    $('#<%=txtSubLedgerDt6Name.ClientID %>').val('');
                });
            }

            function onSubLedgerID6Changed() {
                if ($('#<%=hdnSubLedgerID6.ClientID %>').val() != '0' && $('#<%=hdnSubLedgerID6.ClientID %>').val() != '') {
                    $('#<%=lblSubLedgerDt6.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDt6Code.ClientID %>').removeAttr('readonly');
                } else {
                    $('#<%=lblSubLedgerDt6.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDt6Code.ClientID %>').attr('readonly', 'readonly');
                }

            }
            //#endregion

            //#region Sub Ledger 6
            function onGetSubLedgerDt6FilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression6.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID6.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt6.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName6.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName6.ClientID %>').val(), onGetSubLedgerDt6FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDt6Code.ClientID %>').val(value);
                        onTxtSubLedgerDt6CodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDt6Code.ClientID %>').change(function () {
                onTxtSubLedgerDt6CodeChanged($(this).val());
            });

            function onTxtSubLedgerDt6CodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName6.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDt6FilterExpression() + " AND " + $('#<%=hdnCodeFieldName6.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName6.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDt6ID.ClientID %>').val(result[$('#<%=hdnIDFieldName6.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDt6Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName6.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDt6ID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt6Code.ClientID %>').val('');
                            $('#<%=txtSubLedgerDt6Name.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion
            //#endregion

            onSubLedgerID1Changed();
            onSubLedgerID2Changed();
            onSubLedgerID3Changed();
            onSubLedgerID4Changed();
            onSubLedgerID5Changed();
            onSubLedgerID6Changed();
        }

        function onGetFilterExpression() {
            return "IsDeleted = 0";
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Kelompok")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAGroupCode" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Kelompok")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAGroupName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" id="lblMethod">
                                <%=GetLabel("Metode Penyusutan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnMethodID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMethodCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMethodName" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kategori Anggaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBudgetCategory" ClientInstanceName="cboBudgetCategory" Width="150px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr id="trProductLine" runat="server">
                        <td>
                            <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                <%=GetLabel("Product Line Asal")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSourceProductLineID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSourceProductLineCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSourceProductLineName" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h4>
                    <%=GetLabel("Pengaturan COA Penyusutan Aset & Inventaris")%></h4>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblGLAccount1">
                                <%=GetLabel("COA Aset Tetap")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount1ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID1" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName1" runat="server" />
                            <input type="hidden" id="hdnIDFieldName1" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName1" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName1" runat="server" />
                            <input type="hidden" id="hdnMethodName1" runat="server" />
                            <input type="hidden" id="hdnFilterExpression1" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount1Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount1Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblGLAccount2">
                                <%=GetLabel("COA Akumulasi Penyusutan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount2ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID2" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName2" runat="server" />
                            <input type="hidden" id="hdnIDFieldName2" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName2" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName2" runat="server" />
                            <input type="hidden" id="hdnMethodName2" runat="server" />
                            <input type="hidden" id="hdnFilterExpression2" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount2Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount2Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblGLAccount3">
                                <%=GetLabel("COA Beban Penyusutan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount3ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID3" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName3" runat="server" />
                            <input type="hidden" id="hdnIDFieldName3" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName3" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName3" runat="server" />
                            <input type="hidden" id="hdnMethodName3" runat="server" />
                            <input type="hidden" id="hdnFilterExpression3" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount3Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount3Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                    
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top; display: none">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt1">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt1ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt1Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt1Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt2">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt2ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt2Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt2Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt3">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt3ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt3Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt3Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt4">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt4ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt4Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt4Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt5">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt5ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt5Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt5Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt6">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt6ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt6Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt6Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h4>
                    <%=GetLabel("Pengaturan COA Penjualan dan Pemusnahan Aset & Inventaris")%></h4>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblGLAccount4">
                                <%=GetLabel("COA Keuntungan Penjualan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount4ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID4" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName4" runat="server" />
                            <input type="hidden" id="hdnIDFieldName4" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName4" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName4" runat="server" />
                            <input type="hidden" id="hdnMethodName4" runat="server" />
                            <input type="hidden" id="hdnFilterExpression4" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount4Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount4Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblGLAccount5">
                                <%=GetLabel("COA Kerugian Penjualan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount5ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID5" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName5" runat="server" />
                            <input type="hidden" id="hdnIDFieldName5" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName5" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName5" runat="server" />
                            <input type="hidden" id="hdnMethodName5" runat="server" />
                            <input type="hidden" id="hdnFilterExpression5" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount5Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount5Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblGLAccount6">
                                <%=GetLabel("COA Pemusnahan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount6ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID6" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName6" runat="server" />
                            <input type="hidden" id="hdnIDFieldName6" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName6" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName6" runat="server" />
                            <input type="hidden" id="hdnMethodName6" runat="server" />
                            <input type="hidden" id="hdnFilterExpression6" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount6Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount6Name" Width="100%" />
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
