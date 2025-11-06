<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="GLMDRevenueAccountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLMDRevenueAccountEntry" %>

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
                        if ($('#<%=hdnGLAccount2ID.ClientID %>').val() == "" || $('#<%=hdnGLAccount2ID.ClientID %>').val() == "0") {
                            $('#<%=txtGLAccount2Code.ClientID %>').val(value);
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
                        if ($('#<%=hdnGLAccount3ID.ClientID %>').val() == "" || $('#<%=hdnGLAccount3ID.ClientID %>').val() == "0") {
                            $('#<%=txtGLAccount3Code.ClientID %>').val(value);
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

                            if ($('#<%=hdnSubLedgerDt2ID.ClientID %>').val() == "" || $('#<%=hdnSubLedgerDt2ID.ClientID %>').val() == "0") {
                                $('#<%=txtSubLedgerDt2Code.ClientID %>').val(value);
                                $('#<%=hdnSubLedgerDt2ID.ClientID %>').val(result[$('#<%=hdnIDFieldName2.ClientID %>').val()]);
                                $('#<%=txtSubLedgerDt2Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName2.ClientID %>').val()]);
                            }
                            if ($('#<%=hdnSubLedgerDt3ID.ClientID %>').val() == "" || $('#<%=hdnSubLedgerDt3ID.ClientID %>').val() == "0") {
                                $('#<%=txtSubLedgerDt3Code.ClientID %>').val(value);
                                $('#<%=hdnSubLedgerDt3ID.ClientID %>').val(result[$('#<%=hdnIDFieldName3.ClientID %>').val()]);
                                $('#<%=txtSubLedgerDt3Name.ClientID %>').val(result[$('#<%=hdnDisplayFieldName3.ClientID %>').val()]);
                            }
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

            //#region Healthcare Service Unit
            function onGetHealthcareSeriveUnitFilterExpression() {
                var filterExpression = "<%:OnGetHealthcareSeriveUnitFilterExpression() %>";

                return filterExpression;
            }

            $('#lblHealthcareServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', onGetHealthcareSeriveUnitFilterExpression(), function (value) {
                    $('#<%=txtHealthcareServiceUnitCode.ClientID %>').val(value);
                    onTxtHealthcareServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtHealthcareServiceUnitCode.ClientID %>').change(function () {
                onTxtHealthcareServiceUnitCodeChanged($(this).val());
            });

            function onTxtHealthcareServiceUnitCodeChanged(value) {
                var filterExpression = onGetHealthcareSeriveUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtHealthcareServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtHealthcareServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtHealthcareServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            onSubLedgerID1Changed();
            onSubLedgerID2Changed();
            onSubLedgerID3Changed();
            onSubLedgerID4Changed();
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
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Asal Pasien")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboDepartment" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label class="lblMandatory lblLink" id="lblHealthcareServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
                        <td>
                            <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtHealthcareServiceUnitCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtHealthcareServiceUnitName" Width="100%" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Transaksi")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboGLRevenueTransaction" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top:5px"><label><%=GetLabel("Notes")%></label></td>
                        <td><asp:TextBox ID="txtNotes" Width="300px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
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
                            <table width="100%">
                                <colgroup>
                                    <col width="190px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount1"><%=GetLabel("Perkiraan Tariff Komponen 1")%></label></td>
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
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount2"><%=GetLabel("Perkiraan Tariff Komponen 2")%></label></td>
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
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount3"><%=GetLabel("Perkiraan Tariff Komponen 3")%></label></td>
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
                                <tr style="display:none">
                                    <td class="tdLabel"><label class="lblLink" id="lblGLAccount4"><%=GetLabel("Perkiraan Tariff Komponen 4")%></label></td>
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
                            </table>
                        </td>
                        <td>
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
                                <tr  style="display:none">
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
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>