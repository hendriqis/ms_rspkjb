<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="MappingTreasuryTransactionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.MappingTreasuryTransactionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        //#region Akun Treasury
        function onGetGLAccountTreasuryFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsUsedAsTreasury = 1";
            return filterExpression;
        }

        $('#<%=lblGLAccountTreasury.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountTreasuryFilterExpression(), function (value) {
                $('#<%=txtGLAccountCodeTreasury.ClientID %>').val(value);
                ontxtGLAccountTreasuryCodeChanged(value);
            });
        });

        $('#<%=txtGLAccountCodeTreasury.ClientID %>').live('change', function () {
            ontxtGLAccountTreasuryCodeChanged($(this).val());
        });

        function ontxtGLAccountTreasuryCodeChanged(value) {
            var filterExpression = onGetGLAccountTreasuryFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountTreasuryID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountNameTreasury.ClientID %>').val(result.GLAccountName);
                }
                else {
                    $('#<%=hdnGLAccountTreasuryID.ClientID %>').val('');
                    $('#<%=txtGLAccountCodeTreasury.ClientID %>').val('');
                    $('#<%=txtGLAccountNameTreasury.ClientID %>').val('');
                }
            });
        }
        //#endregion
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
                                <%=GetLabel("Sumber Data") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTransactionCode" ClientInstanceName="cboTransactionCode"
                                Width="100%" runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Transaksi") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTreasuryType" ClientInstanceName="cboTreasuryType" Width="100%"
                                runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Sumber Transaksi") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTreasuryGroup" ClientInstanceName="cboTreasuryGroup" Width="100%"
                                runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblGLAccountTreasury" runat="server">
                                <%=GetLabel("COA Kas & Bank")%></label>
                        </td>
                        <td id="td1" runat="server">
                            <input type="hidden" id="hdnGLAccountTreasuryID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccountCodeTreasury" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccountNameTreasury" Width="100%" ReadOnly="true" />
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
