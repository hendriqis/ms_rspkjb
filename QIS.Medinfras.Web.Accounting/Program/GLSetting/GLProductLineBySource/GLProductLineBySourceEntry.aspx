<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLProductLineBySourceEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLProductLineBySourceEntry" %>

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

            //#region Product Line
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

            //#region HealthcareServiceUnit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    ontxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').die('change');
            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                ontxtServiceUnitCodeChanged($(this).val());
            });

            function ontxtServiceUnitCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region SourceHealthcareServiceUnit
            function getSourceHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblSource.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getSourceHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtSourceServiceUnitCode.ClientID %>').val(value);
                    ontxtSourceServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtSourceServiceUnitCode.ClientID %>').die('change');
            $('#<%=txtSourceServiceUnitCode.ClientID %>').live('change', function () {
                ontxtSourceServiceUnitCodeChanged($(this).val());
            });

            function ontxtSourceServiceUnitCodeChanged(value) {
                var filterExpression = getSourceHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSourceHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtSourceServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnSourceHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtSourceServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtSourceServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Class
            function getClassFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblClass.lblLink').live('click', function () {
                openSearchDialog('classcare', getClassFilterExpression(), function (value) {
                    $('#<%=txtClassCode.ClientID %>').val(value);
                    onTxtClassCodeChanged(value);
                });
            });

            $('#<%=txtClassCode.ClientID %>').die('change');
            $('#<%=txtClassCode.ClientID %>').live('change', function () {
                onTxtClassCodeChanged($(this).val());
            });

            function onTxtClassCodeChanged(value) {
                var filterExpression = getClassFilterExpression() + " AND ClassCode = '" + value + "'";
                Methods.getObject('GetClassCareList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                        $('#<%=txtClassName.ClientID %>').val(result.ClassName);
                    }
                    else {
                        $('#<%=hdnClassID.ClientID %>').val('');
                        $('#<%=txtClassCode.ClientID %>').val('');
                        $('#<%=txtClassName.ClientID %>').val('');
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
                        $('#<%=txtGLAccount1Code.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtGLAccount1Name.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccount1ID.ClientID %>').val('');
                        $('#<%=txtGLAccount1Code.ClientID %>').val('');
                        $('#<%=txtGLAccount1Name.ClientID %>').val('');
                    }
                });
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
                        $('#<%=txtGLAccount2Code.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtGLAccount2Name.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccount2ID.ClientID %>').val('');
                        $('#<%=txtGLAccount2Code.ClientID %>').val('');
                        $('#<%=txtGLAccount2Name.ClientID %>').val('');
                    }
                });
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
                        $('#<%=txtGLAccount3Code.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtGLAccount3Name.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccount3ID.ClientID %>').val('');
                        $('#<%=txtGLAccount3Code.ClientID %>').val('');
                        $('#<%=txtGLAccount3Name.ClientID %>').val('');
                    }
                });
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
                        $('#<%=txtGLAccount4Code.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtGLAccount4Name.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccount4ID.ClientID %>').val('');
                        $('#<%=txtGLAccount4Code.ClientID %>').val('');
                        $('#<%=txtGLAccount4Name.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblProductLine">
                                <%=GetLabel("Product Line")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnProductLineID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductLineName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblServiceUnit">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblSource">
                                <%=GetLabel("Asal Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnSourceHealthcareServiceUnitID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSourceServiceUnitCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSourceServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblClass">
                                <%=GetLabel("Class")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnClassID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClassName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
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
                <table width="50%">
                    <colgroup>
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td>
                            <table width="100%">
                                <colgroup>
                                    <col width="30%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblGLAccount1">
                                            <%=GetLabel("COA COGS")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount1ID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
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
                                                    <asp:TextBox runat="server" ID="txtGLAccount1Name" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblGLAccount2">
                                            <%=GetLabel("COA Pemakaian")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount2ID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
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
                                                    <asp:TextBox runat="server" ID="txtGLAccount2Name" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblGLAccount3">
                                            <%=GetLabel("COA Penyesuaian Masuk")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount3ID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
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
                                                    <asp:TextBox runat="server" ID="txtGLAccount3Name" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblGLAccount4">
                                            <%=GetLabel("COA Penyesuaian Keluar")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnGLAccount4ID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
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
                                                    <asp:TextBox runat="server" ID="txtGLAccount4Name" ReadOnly="true" Width="100%" />
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
