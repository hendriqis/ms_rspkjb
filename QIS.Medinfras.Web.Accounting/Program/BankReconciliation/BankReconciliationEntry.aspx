<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="BankReconciliationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.BankReconciliationEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            if (getIsAdd()) {
                setDatePicker('<%=txtBankReconciliationDate.ClientID %>');
                $('#<%=txtBankReconciliationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            }

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblReconciliationDtEntry').show();
            }
            else {
                $('#lblReconciliationDtEntry').hide();
            }
        }

        //#region Bank Reconciliation No
        $('#lblBankReconciliationNo.lblLink').live('click', function () {
            var filterExpression = "1=1";
            openSearchDialog('bankreconciliationhd', filterExpression, function (value) {
                $('#<%=txtBankReconciliationNo.ClientID %>').val(value);
                onTxtBankReconciliationNoChanged(value);
            });
        });

        $('#<%=txtBankReconciliationNo.ClientID %>').live('change', function () {
            onTxtBankReconciliationNoChanged($(this).val());
        });

        function onTxtBankReconciliationNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        //#region GL Account
        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblGLAccount.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountNo.ClientID %>').val(value);
                ontxtGLAccountNoChanged(value);
            });
        });

        $('#<%=txtGLAccountNo.ClientID %>').live('change', function () {
            ontxtGLAccountNoChanged($(this).val());
        });

        function ontxtGLAccountNoChanged(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);
                }
                else {
                    $('#<%=hdnGLAccountID.ClientID %>').val('');
                    $('#<%=txtGLAccountNo.ClientID %>').val('');
                    $('#<%=txtGLAccountName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#<%=txtCurrentAccountBalance.ClientID %>').live('change', function () {
            var oCurrentAccountBalance = parseFloat($('#<%=txtCurrentAccountBalance.ClientID %>').val().replace('.00', '').split(',').join(''));
            var oCalculationBalance = parseFloat($('#<%=txtCalculationBalance.ClientID %>').val().replace('.00', '').split(',').join(''));

            var oDifferentBalance = oCurrentAccountBalance - oCalculationBalance;
            $('#<%=txtDifferentBalance.ClientID %>').val(oDifferentBalance).trigger('changeValue');
        });

        $('#lblReconciliationDtEntry').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Program/BankReconciliation/BankReconciliationEntryCtl.ascx');
                var bankReconciliationID = $('#<%=hdnBankReconciliationID.ClientID %>').val();
                var glAccountID = $('#<%=hdnGLAccountID.ClientID %>').val();
                var sendParamReconciliation = bankReconciliationID + "|" + glAccountID;
                openUserControlPopup(url, sendParamReconciliation, 'Entry Reconciliation Detail', 1000, 450);
            }
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnGLTransactionDtID.ClientID %>').val(entity.TransactionDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        function onLoadCurrentRecord() {
            onLoadObject($('#<%=txtBankReconciliationNo.ClientID %>').val());
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onLoadObject(param);
        }

        var isAfterAdd = false;
        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    isAfterAdd = false;
                    cbpView.PerformCallback('refresh');
                    var txtBankReconciliationNo = $('#<%=txtBankReconciliationNo.ClientID %>').val();
                    onTxtBankReconciliationNoChanged(txtBankReconciliationNo);
                }
            }
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
            }
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var oBankReconciliationID = $('#<%=hdnBankReconciliationID.ClientID %>').val();

        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" id="hdnBankReconciliationID" runat="server" value="" />
    <input type="hidden" id="hdnGLTransactionDtID" runat="server" value="" />
    <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal lblLink" id="lblBankReconciliationNo">
                                <%=GetLabel("Nomor Rekonsiliasi") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankReconciliationNo" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Rekonsiliasi") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtBankReconciliationDate" CssClass="datepicker"
                                Width="130px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Mata Uang")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboCurrency" ClientInstanceName="cboCurrency" Width="150px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr style="padding-top: 2px; padding-left: 0px">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblGLAccount">
                                <%=GetLabel("COA")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccountID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccountNo" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccountName" ReadOnly="true" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Saldo Rekening Koran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrentAccountBalance" Width="150px" runat="server" CssClass="txtCurrency" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kalkulasi Saldo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCalculationBalance" Width="150px" runat="server" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="middle">
                            <table width="350px">
                                <col style="width: 300px" />
                                <col style="width: 50px" />
                                <tr>
                                    <td valign="middle">
                                        <hr style="margin: 0 0 0 0;" />
                                    </td>
                                    <td valign="middle">
                                        <%=GetLabel("[-]") %>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Selisih Saldo") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDifferentBalance" Width="150px" runat="server" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative;">
                                <input type="hidden" value="0" id="hdnDisplayCount" runat="server" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                                src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("TransactionDtID") %>" bindingfield="TransactionDtID" />
                                                <input type="hidden" value="<%#:Eval("GLAccount") %>" bindingfield="GLAccount" />
                                                <input type="hidden" value="<%#:Eval("GLAccountNo") %>" bindingfield="GLAccountNo" />
                                                <input type="hidden" value="<%#:Eval("GLAccountName") %>" bindingfield="GLAccountName" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Tanggal Jurnal")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("cfJournalDateInString") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("No Jurnal")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("JournalNo") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Debit")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("cfDebitAmountInString") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Kredit")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("cfCreditAmountInString") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Rekonsil")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("cfIsReconciled") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Informasi Rekonsiliasi")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-size: 14px;">
                                                    <%#:Eval("LastReconciledByName") %></div>
                                                <div style="font-size: 10px;">
                                                    <%#:Eval("cfLastReconciledDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblReconciliationDtEntry">
                            <%= GetLabel("Quick Pick")%></span>
                    </div>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 450px;">
                                    <div class="lblComponent" style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Informasi Jurnal") %></div>
                                    <div style="background-color: #EAEAEA;">
                                        <table width="400px" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="10px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Pada") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Pada")%>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
