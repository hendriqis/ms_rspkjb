<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="ChartOfAccountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.ChartOfAccountEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Parent
            function onGetParentFilterExpression() {
                var filterExpression = "GCGLAccountType = '" + cboGCGLAccountType.GetValue() + "' AND IsHeader = 1 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('vchartofaccount', onGetParentFilterExpression(), function (value) {
                    $('#<%=txtParentAccountNo.ClientID %>').val(value);
                    onTxtParentAccountNoChanged(value);
                });
            });

            $('#<%=txtParentAccountNo.ClientID %>').change(function () {
                onTxtParentAccountNoChanged($(this).val());
            });

            function onTxtParentAccountNoChanged(value) {
                var filterExpression = onGetParentFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentAccountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtParentAccountName.ClientID %>').val(result.GLAccountName);
                        $('#<%=txtAccountLevel.ClientID %>').val(result.AccountLevel + 1);
                    }
                    else {
                        $('#<%=hdnParentAccountID.ClientID %>').val('');
                        $('#<%=txtParentAccountNo.ClientID %>').val('');
                        $('#<%=txtParentAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Sub Ledger
            function onGetSubLedgerFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblSubLedger.lblLink').click(function () {
                openSearchDialog('subledgerhd', onGetSubLedgerFilterExpression(), function (value) {
                    $('#<%=txtSubLedgerCode.ClientID %>').val(value);
                    onTxtSubLedgerCodeChanged(value);
                });
            });

            $('#<%=txtSubLedgerCode.ClientID %>').change(function () {
                onTxtSubLedgerCodeChanged($(this).val());
            });

            function onTxtSubLedgerCodeChanged(value) {
                var filterExpression = onGetSubLedgerFilterExpression() + " AND SubLedgerCode = '" + value + "'";
                Methods.getObject('GetSubLedgerHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=txtSubLedgerName.ClientID %>').val(result.SubLedgerName);
                    }
                    else {
                        $('#<%=hdnSubLedgerID.ClientID %>').val('');
                        $('#<%=txtSubLedgerCode.ClientID %>').val('');
                        $('#<%=txtSubLedgerName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#btnSubLedgerDt').click(function () {
                var subLedgerID = $('#<%=hdnSubLedgerID.ClientID %>').val();
                if (subLedgerID != '' && subLedgerID != '0') {
                    var url = ResolveUrl("~/Program/Master/SubLedger/SubLedgerDtViewCtl.ascx");
                    openUserControlPopup(url, subLedgerID, 'Detail', 1000, 520);
                }
            });

            $('#<%:chkIsUsingBusinessPartner.ClientID %>').click(function () {
                if ($(this).is(':checked')) {
                    $('#<%=tdGCBusinessPartnerType.ClientID%>').removeAttr('style');
                } else {
                    $('#<%=tdGCBusinessPartnerType.ClientID%>').attr('style', 'display:none');
                }
            });
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col style="width: 250px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode COA")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGLAccountNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama COA")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtGLAccountName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelompok COA")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboGCGLAccountType" ClientInstanceName="cboGCGLAccountType"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <input type="hidden" id="hdnParentAccountID" runat="server" />
                            <label class="lblLink" id="lblParent">
                                <%=GetLabel("Kode COA Induk")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtParentAccountNo" Width="100%" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtParentAccountName" Width="100%" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <input type="hidden" id="hdnSubLedgerID" runat="server" />
                            <label class="lblLink" id="lblSubLedger">
                                <%=GetLabel("Sub COA")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSubLedgerCode" Width="100%" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSubLedgerName" Width="100%" />
                        </td>
                        <td>
                            <input type="button" value="..." id="btnSubLedgerDt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Level COA")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox runat="server" ID="txtAccountLevel" Width="100%" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Saldo Normal")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblPosition" runat="server" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsActive" Text=" Aktif" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                        </td>
                        <td colspan="5">
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsHeader" Text=" COA Induk" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsUsedAsTreasury" Text=" COA Kas dan Bank" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsUsingDocumentControl" Text=" Using Document Control" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsUsingRevenueCostCenter" Text=" Using Revenue Cost Center" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsUsingCustomerGroup" Text=" Using Customer Group" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsUsingParamedicMaster" Text=" Using Paramedic Master" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIsUsingBusinessPartner" Text=" Using Business Partner" />
                                    </td>
                                    <td id="tdGCBusinessPartnerType" runat="server">
                                        <dxe:ASPxComboBox runat="server" ID="cboGCBusinessPartnerType" ClientInstanceName="cboGCBusinessPartnerType"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
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
