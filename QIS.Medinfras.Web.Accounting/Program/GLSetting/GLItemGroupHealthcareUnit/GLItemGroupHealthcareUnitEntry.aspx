<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLItemGroupHealthcareUnitEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLItemGroupHealthcareUnitEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            if ($('#<%=hdnID.ClientID %>').val() != null && $('#<%=hdnID.ClientID %>').val() != "") {
                $('#<%=lblItemGroup.ClientID %>').attr("class", "lblNormal");
            } else {
                $('#<%=lblItemGroup.ClientID %>').attr("class", "lblLink lblMandatory");
            }

            function onGetGLAccountFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

            //#region Item Group Master
            function onGetItemGroupFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblItemGroup.ClientID %>').click(function () {
                if ($('#<%=hdnID.ClientID %>').val() == null || $('#<%=hdnID.ClientID %>').val() == "") {
                    openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                        $('#<%=txtItemGroupCode.ClientID %>').val(value);
                        onTxtItemGroupCodeChanged(value);
                    });
                }
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
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

            //#region ConsumptionGLAccount
            $('#lblConsumptionGLAccount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtConsumptionGLAccountNo.ClientID %>').val(value);
                    ontxtConsumptionGLAccountNoChanged(value);
                });
            });

            $('#<%=txtConsumptionGLAccountNo.ClientID %>').change(function () {
                ontxtConsumptionGLAccountNoChanged($(this).val());
            });

            function ontxtConsumptionGLAccountNoChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnConsumptionGLAccountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtConsumptionGLAccountNo.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtConsumptionGLAccountName.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnConsumptionGLAccountID.ClientID %>').val('');
                        $('#<%=txtConsumptionGLAccountNo.ClientID %>').val('');
                        $('#<%=txtConsumptionGLAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region AdjustmentINGLAccount
            $('#lblAdjustmentINGLAccount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAdjustmentINGLAccountNo.ClientID %>').val(value);
                    ontxtAdjustmentINGLAccountNoChanged(value);
                });
            });

            $('#<%=txtAdjustmentINGLAccountNo.ClientID %>').change(function () {
                ontxtAdjustmentINGLAccountNoChanged($(this).val());
            });

            function ontxtAdjustmentINGLAccountNoChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAdjustmentINGLAccountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAdjustmentINGLAccountNo.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtAdjustmentINGLAccountName.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnAdjustmentINGLAccountID.ClientID %>').val('');
                        $('#<%=txtAdjustmentINGLAccountNo.ClientID %>').val('');
                        $('#<%=txtAdjustmentINGLAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region AdjustmentOUTGLAccount
            $('#lblAdjustmentOUTGLAccount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtAdjustmentOUTGLAccountNo.ClientID %>').val(value);
                    ontxtAdjustmentOUTGLAccountNoChanged(value);
                });
            });

            $('#<%=txtAdjustmentOUTGLAccountNo.ClientID %>').change(function () {
                ontxtAdjustmentOUTGLAccountNoChanged($(this).val());
            });

            function ontxtAdjustmentOUTGLAccountNoChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAdjustmentOUTGLAccountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtAdjustmentOUTGLAccountNo.ClientID %>').val(result.GLAccountNo);
                        $('#<%=txtAdjustmentOUTGLAccountName.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnAdjustmentOUTGLAccountID.ClientID %>').val('');
                        $('#<%=txtAdjustmentOUTGLAccountNo.ClientID %>').val('');
                        $('#<%=txtAdjustmentOUTGLAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        function onAfterSaveNewSuccess() {
            $('#<%=lblItemGroup.ClientID %>').attr("class", "lblLink lblMandatory");
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
                            <label class="lblLink lblMandatory" id="lblItemGroup" runat="server">
                                <%=GetLabel("Kelompok Item")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory" id="lblHealthcareUnit">
                                <%=GetLabel("Healthcare Unit")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboHealthcareUnit" ClientInstanceName="cboHealthcareUnit"
                                Width="350px">
                            </dxe:ASPxComboBox>
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
                                        <label class="lblLink lblMandatory" id="lblConsumptionGLAccount">
                                            <%=GetLabel("COA Consumption")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnConsumptionGLAccountID" runat="server" />
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
                                                    <asp:TextBox runat="server" ID="txtConsumptionGLAccountName" ReadOnly="true" Width="100%" />
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
                            <table width="100%">
                                <colgroup>
                                    <col width="30%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblAdjustmentINGLAccount">
                                            <%=GetLabel("COA Adjustment IN")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAdjustmentINGLAccountID" runat="server" />
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
                                                    <asp:TextBox runat="server" ID="txtAdjustmentINGLAccountName" ReadOnly="true" Width="100%" />
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
                            <table width="100%">
                                <colgroup>
                                    <col width="30%" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblAdjustmentOUTGLAccount">
                                            <%=GetLabel("COA Adjustment OUT")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnAdjustmentOUTGLAccountID" runat="server" />
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
                                                    <asp:TextBox runat="server" ID="txtAdjustmentOUTGLAccountName" ReadOnly="true" Width="100%" />
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
