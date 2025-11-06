<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryCopyUserPatientPaymentBalanceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryCopyUserPatientPaymentBalanceCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_TreasuryCopyUserPatientPaymentBalanceCtl">

    $(function () {
        setDatePicker('<%=txtPeriodFrom.ClientID %>');
        setDatePicker('<%=txtPeriodTo.ClientID %>');
    });

    $('#btnRefresh').live('click', function () {
        cbpProcessDetail.PerformCallback('refresh');
    });

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
        }, 0);
    }

    function onRefreshGrid() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        getCheckedUserPatientPaymentBalance();
        cbpProcessDetail.PerformCallback('refresh');
    }

    $('#chkSelectAllUserPatientPaymentBalance').die('change');
    $('#chkSelectAllUserPatientPaymentBalance').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectUserPatientPaymentBalance').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
        calculateTotal();
    });

    $('.grdView .chkSelectUserPatientPaymentBalance input').live('click', function () {
        $('.chkSelectAllUserPatientPaymentBalance input').prop('checked', false);
        calculateTotal();
    });

    function getCheckedUserPatientPaymentBalance() {
        var lstSelectedUserPatientPaymentBalance = $('#<%=hdnSelectedUserPatientPaymentBalanceID.ClientID %>').val().split(',');
        var lstSelectedPaymentAmountOUT = $('#<%=hdnSelectedPaymentAmountOUT.ClientID %>').val().split(',');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split(',');
        $('.chkSelectUserPatientPaymentBalance input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtPaymentAmountOUT = parseFloat($tr.find('.txtPaymentAmountOUT').val());
                var txtRemarks = $tr.find('.txtRemarks').val();
                var idx = lstSelectedUserPatientPaymentBalance.indexOf(key);
                if (idx < 0) {
                    lstSelectedUserPatientPaymentBalance.push(key);
                    lstSelectedPaymentAmountOUT.push(txtPaymentAmountOUT);
                    lstSelectedRemarks.push(txtRemarks);
                }
                else {
                    lstSelectedPaymentAmountOUT[idx] = txtPaymentAmountOUT;
                    lstSelectedRemarks[idx] = txtRemarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedUserPatientPaymentBalance.indexOf(key);
                if (idx > -1) {
                    lstSelectedUserPatientPaymentBalance.splice(idx, 1);
                    lstSelectedPaymentAmountOUT.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedUserPatientPaymentBalanceID.ClientID %>').val(lstSelectedUserPatientPaymentBalance.join(','));
        $('#<%=hdnSelectedPaymentAmountOUT.ClientID %>').val(lstSelectedPaymentAmountOUT.join(','));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join(','));
    }

    function calculateTotal() {
        var totalSelectedAmount = 0;

        $('.grdView .chkSelectUserPatientPaymentBalance input:checked').each(function () {
            $tr = $(this).closest('tr');
            totalSelectedAmount += parseFloat($tr.find('.PaymentAmountEND').val());
        });

        $('#<%=txtTotalAmountSelected.ClientID %>').val(totalSelectedAmount).trigger('changeValue');
    }

    function onBeforeSaveRecord(errMessage) {
        var accountdt = $('#<%=hdnGLTransactionIDctl.ClientID %>').val();
        if (accountdt != "" && accountdt != "0") {
            getCheckedUserPatientPaymentBalance();
            if ($('#<%=hdnSelectedUserPatientPaymentBalanceID.ClientID %>').val() == '') {
                errMessage.text = 'Please Select Data First';
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
        getCheckedUserPatientPaymentBalance();
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnGLTransactionIDctl" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountTreasuryIDctl" runat="server" value="" />
    <input type="hidden" id="hdnTreasuryTypectl" runat="server" value="" />
    <input type="hidden" id="hdnDisplayOrderTemp" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnBusinessPartnerIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnCashFlowTypeIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnSelectedUserPatientPaymentBalanceID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentAmountOUT" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRemarks" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 60%" />
            <col style="width: 40%" />
        </colgroup>
        <tr>
            <td align="left" style="vertical-align: top">
                <table>
                    <colgroup>
                        <col style="width: 180px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Payment Date") %></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                    </td>
                                    <td style="width: 30px; text-align: center">
                                        s/d
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Quick Search")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="300px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="CashierName" FieldName="CashierName" />
                                    <qis:QISIntellisenseHint Text="PaymentMethod" FieldName="PaymentMethod" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div id="divRefresh" runat="server" style="float: left; margin-top: 0px;">
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right" style="vertical-align: top">
                <table>
                    <colgroup>
                        <col style="width: 180px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Payment Selected") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Width="300px" ID="txtTotalAmountSelected" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 30px" align="center">
                                                    <input id="chkSelectAllUserPatientPaymentBalance" type="checkbox" />
                                                </th>
                                                <th>
                                                    <%=GetLabel("Cashier Name")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Payment DateTime")%>
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("Payment Method")%>
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("EDC Machine")%>
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("Bank")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount IN")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount OUT")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount END")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount*")%>
                                                </th>
                                                <th align="center">
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
                                                    <input id="chkSelectAllUserPatientPaymentBalance" type="checkbox" />
                                                </th>
                                                <th>
                                                    <%=GetLabel("Cashier Name")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Payment DateTime")%>
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("Payment Method")%>
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("EDC Machine")%>
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("Bank")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount IN")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount OUT")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount END")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payment Amount*")%>
                                                </th>
                                                <th align="center">
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
                                                <asp:CheckBox ID="chkSelectUserPatientPaymentBalance" runat="server" CssClass="chkSelectUserPatientPaymentBalance" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                                <input type="hidden" class="PaymentAmountEND" value="<%#: Eval("PaymentAmountEND")%>" />
                                            </td>
                                            <td>
                                                <%#: Eval("CashierName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfPaymentDateInString")%>
                                                <br />
                                                <%#: Eval("LastPaymentTime")%>
                                            </td>
                                            <td>
                                                <%#: Eval("PaymentMethod") %>
                                            </td>
                                            <td>
                                                <%#: Eval("EDCMachineName") %>
                                            </td>
                                            <td>
                                                <%#: Eval("BankName") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfPaymentAmountINInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfPaymentAmountOUTInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfPaymentAmountENDInString")%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPaymentAmountOUT" CssClass="txtPaymentAmountOUT number"
                                                    Width="95%" Text="0.00" />
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
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
