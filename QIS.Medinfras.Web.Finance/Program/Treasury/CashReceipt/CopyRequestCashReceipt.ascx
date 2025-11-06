<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyRequestCashReceipt.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.CopyRequestCashReceipt" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_CopyRequestCashReceipt">

    $('#btnRefresh').die('click');
    $('#btnRefresh').live('click', function () {
        cbpProcessDetail.PerformCallback('refresh');
    });

    $('#chkSelectAllCashReceipt').die('change');
    $('#chkSelectAllCashReceipt').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectCashReceipt').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function getCheckedCashReceipt() {
        var lstSelectedGLTransactionDtID = $('#<%=hdnSelectedGLTransactionDtID.ClientID %>').val().split(',');
        var lstSelectedDebitAmount = $('#<%=hdnSelectedDebitAmount.ClientID %>').val().split(',');
        var lstSelectedCreditAmount = $('#<%=hdnSelectedCreditAmount.ClientID %>').val().split(',');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split(',');
        $('.chkSelectCashReceipt input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtDebitAmount = parseFloat($tr.find('.txtDebitAmount').val().split(',').join(""));
                var txtCreditAmount = parseFloat($tr.find('.txtCreditAmount').val().split(',').join(""));
                var txtRemarks = $tr.find('.txtRemarks').val();
                var idx = lstSelectedGLTransactionDtID.indexOf(key);
                if (idx < 0) {
                    lstSelectedGLTransactionDtID.push(key);
                    lstSelectedDebitAmount.push(txtDebitAmount);
                    lstSelectedCreditAmount.push(txtCreditAmount);
                    lstSelectedRemarks.push(txtRemarks);
                }
                else {
                    lstSelectedDebitAmount[idx] = txtDebitAmount;
                    lstSelectedCreditAmount[idx] = txtCreditAmount;
                    lstSelectedRemarks[idx] = txtRemarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedGLTransactionDtID.indexOf(key);
                if (idx > -1) {
                    lstSelectedGLTransactionDtID.splice(idx, 1);
                    lstSelectedDebitAmount.splice(idx, 1);
                    lstSelectedCreditAmount.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedGLTransactionDtID.ClientID %>').val(lstSelectedGLTransactionDtID.join(','));
        $('#<%=hdnSelectedDebitAmount.ClientID %>').val(lstSelectedDebitAmount.join(','));
        $('#<%=hdnSelectedCreditAmount.ClientID %>').val(lstSelectedCreditAmount.join(','));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join(','));
    }

    function onBeforeSaveRecord(errMessage) {
        var accountdt = $('#<%=hdnCOACashReceiptCtl.ClientID %>').val();
        if (accountdt != "" && accountdt != "0") {
            getCheckedCashReceipt();
            if ($('#<%=hdnSelectedGLTransactionDtID.ClientID %>').val() == '') {
                errMessage.text = 'Please Select Detail First';
                return false;
            }
        } else {
            errMessage.text = 'Please Select Account Detail First';
            return false;
        }
        return true;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        getCheckedCashReceipt();
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnGLTransactionIDctl" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountTreasuryIDctl" runat="server" value="" />
    <input type="hidden" id="hdnTreasuryTypectl" runat="server" value="" />
    <input type="hidden" id="hdnDisplayOrderTemp" runat="server" value="0" />
    <input type="hidden" id="hdnDepartmentIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnBusinessPartnerIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnCOACashReceiptCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedGLTransactionDtID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDebitAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedCreditAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRemarks" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("Pilihan Salin Saldo") %></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboAmountCopy" ClientInstanceName="cboAmountCopy" Width="30%"
                    runat="server">
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="2">
                <div style="height: 400px; overflow-y: auto">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 30px" align="center">
                                                        <input id="chkSelectAllCashReceipt" type="checkbox" />
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Akun")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Nama Akun")%>
                                                    </th>
                                                    <th style="width: 120px" align="left">
                                                        <%=GetLabel("No. Referensi")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Awal")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Masuk")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Keluar")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Akhir")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Debit")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Kredit")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Catatan")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 30px" align="center">
                                                        <input id="chkSelectAllCashReceipt" type="checkbox" />
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Akun")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Nama Akun")%>
                                                    </th>
                                                    <th style="width: 120px" align="left">
                                                        <%=GetLabel("No. Referensi")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Awal")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Masuk")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Keluar")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Saldo Akhir")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Debit")%>
                                                    </th>
                                                    <th style="width: 100px" align="right">
                                                        <%=GetLabel("Kredit")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Catatan")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkSelectCashReceipt" runat="server" CssClass="chkSelectCashReceipt" />
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("TransactionDtID")%>' />
                                                </td>
                                                <td>
                                                    <%#: Eval("GLAccountNo")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("GLAccountName")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("ReferenceNo") %>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfBalanceBEGINInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfBalanceINInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfBalanceOUTInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfBalanceENDInString")%>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox runat="server" Width="100%" ID="txtDebitAmount" CssClass="txtCurrency txtDebitAmount"
                                                        Text="0" />
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox runat="server" Width="100%" ID="txtCreditAmount" CssClass="txtCurrency txtCreditAmount"
                                                        Text="0" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" Width="100%" ID="txtRemarks" CssClass="txtRemarks" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging" style="display: none">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
