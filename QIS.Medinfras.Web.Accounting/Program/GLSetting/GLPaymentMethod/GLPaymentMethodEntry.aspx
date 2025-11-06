<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLPaymentMethodEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLPaymentMethodEntry" %>

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
                    $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblLink lblMandatory');
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

            //#region Bank
            $('#lblIBank.lblLink').click(function () {
                var filterExpression = "IsDeleted = 0";
                openSearchDialog('bank', filterExpression, function (value) {
                    $('#<%=txtBankCode.ClientID %>').val(value);
                    onTxtBankCodeChanged(value);
                });
            });

            $('#<%=txtBankCode.ClientID %>').change(function () {
                onTxtBankCodeChanged($(this).val());
            });

            function onTxtBankCodeChanged(value) {
                var filterExpression = "BankID = '" + value + "'";
                Methods.getObject('GetBankList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBankID.ClientID %>').val(result.BankID);
                        $('#<%=txtBankCode.ClientID %>').val(result.BankCode);
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

            //#region EDC Machine
            function onGetEDCMachineFilterExpression() {
                var filterExpression = "IsDeleted = 0";

                return filterExpression;
            }

            $('#lblEDCMachine.lblLink').click(function () {
                openSearchDialog('edcmachine', onGetEDCMachineFilterExpression(), function (value) {
                    $('#<%=txtEDCMachineCode.ClientID %>').val(value);
                    onTxtEDCMachineCodeChanged(value);
                });
            });

            $('#<%=txtEDCMachineCode.ClientID %>').change(function () {
                onTxtEDCMachineCodeChanged($(this).val());
            });

            function onTxtEDCMachineCodeChanged(value) {
                var filterExpression = onGetEDCMachineFilterExpression() + " AND EDCMachineCode = '" + value + "'";
                Methods.getObject('GetvEDCMachineList', filterExpression, function (result) {
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

            onSubLedgerID1Changed();
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
                        <col style="width: 190px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kelompok Transaksi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGLTransactionGroup" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Metode Pembayaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCPaymentMethod" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblIBank">
                                <%=GetLabel("Bank")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnBankID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBankCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankName" Width="42%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblEDCMachine">
                                <%=GetLabel("Mesin EDC")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnEDCMachineID" />
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
                                        <asp:TextBox runat="server" ID="txtEDCMachineName" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px">
                            <label>
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" Width="100%" runat="server" />
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
                                    <col width="190px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory lblLink" id="lblGLAccount1">
                                            <%=GetLabel("COA Cara Bayar")%></label>
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
                                                    <asp:TextBox runat="server" ID="txtGLAccount1Name" Width="100%" />
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
                                        <label class="lblDisabled" runat="server" id="lblSubLedgerDt1">
                                            <%=GetLabel("Sub Perkiraan")%></label>
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
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
