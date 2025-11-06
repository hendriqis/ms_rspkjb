<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLPatientPaymentMethodEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLPatientPaymentMethodEntry" %>

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

            //#region GL Account
            $('#lblGLAccount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountCode.ClientID %>').val(value);
                    onTxtGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtGLAccountCode.ClientID %>').change(function () {
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
                            <label class="lblMandatory">
                                <%=GetLabel("Metode Pembayaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCPaymentMethod" runat="server" Width="100%" />
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" id="lblGLAccount">
                                <%=GetLabel("COA Cara Bayar")%></label>
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
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
