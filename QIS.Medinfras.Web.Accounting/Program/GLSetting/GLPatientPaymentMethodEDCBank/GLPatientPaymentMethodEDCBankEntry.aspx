<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLPatientPaymentMethodEDCBankEDCBankEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.GLPatientPaymentMethodEDCBankEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
        }

        //#region GL Account
        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblGLAccount.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode.ClientID %>').val(value);
                onTxtGLAccountCodeChanged(value);
            });
        });

        $('#<%=txtGLAccountCode.ClientID %>').live('change', function () {
            onTxtGLAccountCodeChanged($(this).val());
        });

        function onTxtGLAccountCodeChanged(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);
                }
                else {
                    $('#<%=hdnGLAccountID.ClientID %>').val('');
                    $('#<%=txtGLAccountCode.ClientID %>').val('');
                    $('#<%=txtGLAccountName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region EDC Machine
        function onGetEDCMachineFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblEDCMachine.lblLink').live('click', function () {
            openSearchDialog('edcmachine', onGetEDCMachineFilterExpression(), function (value) {
                $('#<%=txtEDCMachineCode.ClientID %>').val(value);
                onTxtEDCMachineCodeChanged(value);
            });
        });

        $('#<%=txtEDCMachineCode.ClientID %>').live('change', function () {
            onTxtEDCMachineCodeChanged($(this).val());
        });

        function onTxtEDCMachineCodeChanged(value) {
            var filterExpression = onGetEDCMachineFilterExpression() + " AND EDCMachineCode = '" + value + "'";
            Methods.getObject('GetEDCMachineList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnEDCMachineID.ClientID %>').val(result.EDCMachineID);
                    $('#<%=txtEDCMachineName.ClientID %>').val(result.EDCMachineName);
                }
                else {
                    $('#<%=hdnEDCMachineID.ClientID %>').val('');
                    $('#<%=txtEDCMachineCode.ClientID %>').val('');
                    $('#<%=txtEDCMachineName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Bank
        function onGetBankFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblBank.lblLink').live('click', function () {
            openSearchDialog('bank2', onGetBankFilterExpression(), function (value) {
                $('#<%=txtBankCode.ClientID %>').val(value);
                onTxtBankCodeChanged(value);
            });
        });

        $('#<%=txtBankCode.ClientID %>').live('change', function () {
            onTxtBankCodeChanged($(this).val());
        });

        function onTxtBankCodeChanged(value) {
            var filterExpression = onGetBankFilterExpression() + " AND BankCode = '" + value + "'";
            Methods.getObject('GetBankList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBankID.ClientID %>').val(result.BankID);
                    $('#<%=txtBankName.ClientID %>').val(result.BankName);
                }
                else {
                    $('#<%=hdnBankID.ClientID %>').val('');
                    $('#<%=txtBankCode.ClientID %>').val('');
                    $('#<%=txtBankName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onCboGCPaymentMethodValueChanged(s) {
            if (cboGCPaymentMethod.GetValue() == 'X035^004') {
                $('#<%:trEDCMachine.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%:trEDCMachine.ClientID %>').removeAttr('style');
            }
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcare" runat="server" value="" />
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
                            <label class="lblMandatory lblLink" id="lblGLAccount">
                                <%=GetLabel("COA")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccountID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccountCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccountName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Payment Method")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCPaymentMethod" ClientInstanceName="cboGCPaymentMethod"
                                Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboGCPaymentMethodValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trEDCMachine" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblEDCMachine">
                                <%=GetLabel("EDC Machine")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnEDCMachineID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEDCMachineCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEDCMachineName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblBank">
                                <%=GetLabel("Bank")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnBankID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBankCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBankName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Cashier Group")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboCashierGroup" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px">
                            <label>
                                <%=GetLabel("Notes")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
