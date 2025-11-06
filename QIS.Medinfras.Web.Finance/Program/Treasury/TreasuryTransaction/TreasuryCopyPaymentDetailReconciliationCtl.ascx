<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryCopyPaymentDetailReconciliationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryCopyPaymentDetailReconciliationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_TreasuryCopyPaymentDetailReconciliationCtl">

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
        getCheckedPaymentDtReconciled();
        cbpProcessDetail.PerformCallback('refresh');
    }

    $('#chkSelectAllPaymentDtReconciled').die('change');
    $('#chkSelectAllPaymentDtReconciled').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectPaymentDtReconciled').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
        calculateTotal();
    });

    $('.grdView .chkSelectPaymentDtReconciled input').live('click', function () {
        $('.chkSelectAllPaymentDtReconciled input').prop('checked', false);
        calculateTotal();
    });

    function getCheckedPaymentDtReconciled() {
        var lstSelectedPaymentDetailID = $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val().split(',');
        var lstSelectedPaymentAmount = $('#<%=hdnSelectedPaymentAmount.ClientID %>').val().split(',');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split(',');
        $('.chkSelectPaymentDtReconciled input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var paymentAmount = parseFloat($tr.find('.PaymentAmount').val());
                var txtRemarks = $tr.find('.txtRemarks').val();
                var idx = lstSelectedPaymentDetailID.indexOf(key);
                if (idx < 0) {
                    lstSelectedPaymentDetailID.push(key);
                    lstSelectedPaymentAmount.push(paymentAmount);
                    lstSelectedRemarks.push(txtRemarks);
                }
                else {
                    lstSelectedPaymentAmount[idx] = paymentAmount;
                    lstSelectedRemarks[idx] = txtRemarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedPaymentDetailID.indexOf(key);
                if (idx > -1) {
                    lstSelectedPaymentDetailID.splice(idx, 1);
                    lstSelectedPaymentAmount.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedPaymentDetailID.ClientID %>').val(lstSelectedPaymentDetailID.join(','));
        $('#<%=hdnSelectedPaymentAmount.ClientID %>').val(lstSelectedPaymentAmount.join(','));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join(','));
    }

    function calculateTotal() {
        var totalSelectedAmount = 0;

        $('.grdView .chkSelectPaymentDtReconciled input:checked').each(function () {
            $tr = $(this).closest('tr');
            totalSelectedAmount += parseFloat($tr.find('.PaymentAmount').val());
        });

        $('#<%=txtTotalAmountSelected.ClientID %>').val(totalSelectedAmount).trigger('changeValue');
    }

    function onBeforeSaveRecord(errMessage) {
        var accountdt = $('#<%=hdnGLTransactionIDctl.ClientID %>').val();
        if (accountdt != "" && accountdt != "0") {
            getCheckedPaymentDtReconciled();
            if ($('#<%=hdnSelectedPaymentDetailID.ClientID %>').val() == '') {
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
        getCheckedPaymentDtReconciled();
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
    <input type="hidden" id="hdnSelectedPaymentDetailID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPaymentAmount" runat="server" value="" />
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
                                    <qis:QISIntellisenseHint Text="PaymentMethod" FieldName="PaymentMethod" />
                                    <qis:QISIntellisenseHint Text="CreatedByName" FieldName="CreatedByName" />
                                    <qis:QISIntellisenseHint Text="EDCMachineName" FieldName="EDCMachineName" />
                                    <qis:QISIntellisenseHint Text="BankName" FieldName="BankName" />
                                    <qis:QISIntellisenseHint Text="PaymentType" FieldName="PaymentType" />
                                    <qis:QISIntellisenseHint Text="CardType" FieldName="CardType" />
                                    <qis:QISIntellisenseHint Text="VisitDepartmentID" FieldName="VisitDepartmentID" />
                                    <qis:QISIntellisenseHint Text="VisitServiceUnitName" FieldName="VisitServiceUnitName" />
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
                <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
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
                                                        <input id="chkSelectAllPaymentDtReconciled" type="checkbox" />
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Cashier Name")%>
                                                    </th>
                                                    <th style="width: 150px" align="left">
                                                        <%=GetLabel("Payment No")%>
                                                    </th>
                                                    <th style="width: 150px" align="center">
                                                        <%=GetLabel("Payment Info")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("EDC")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Bank")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Cashier Group")%>
                                                    </th>
                                                    <th style="width: 150px" align="right">
                                                        <%=GetLabel("Payment Amount")%>
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
                                                        <input id="chkSelectAllPaymentDtReconciled" type="checkbox" />
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Cashier Name")%>
                                                    </th>
                                                    <th style="width: 150px" align="left">
                                                        <%=GetLabel("Payment No")%>
                                                    </th>
                                                    <th style="width: 150px" align="center">
                                                        <%=GetLabel("Payment Info")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("EDC")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Bank")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("Cashier Group")%>
                                                    </th>
                                                    <th style="width: 150px" align="right">
                                                        <%=GetLabel("Payment Amount")%>
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
                                                    <asp:CheckBox ID="chkSelectPaymentDtReconciled" runat="server" CssClass="chkSelectPaymentDtReconciled" />
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PaymentDetailID")%>' />
                                                    <input type="hidden" class="PaymentAmount" id="PaymentAmount" runat="server" value='<%#: Eval("PaymentAmount")%>' />
                                                </td>
                                                <td>
                                                    <%#: Eval("CreatedByName")%>
                                                </td>
                                                <td>
                                                    <b>
                                                        <%#: Eval("PaymentNo")%></b>
                                                    <br />
                                                    <i>
                                                        <%#: Eval("cfPaymentDateInString")%><%=GetLabel(" ")%><%#: Eval("PaymentTime")%></i>
                                                    <br />
                                                    <%#: Eval("VisitDepartmentID")%><%=GetLabel(" | ")%><%#: Eval("VisitServiceUnitName")%>
                                                    <br />
                                                    <%=GetLabel("RegistrationNo : ")%><%#: Eval("RegistrationNo")%>
                                                    <br />
                                                    <%#: Eval("MedicalNo")%><%=GetLabel(" | ")%><%#: Eval("PatientName")%>
                                                </td>
                                                <td align="left">
                                                    <i>
                                                        <%=GetLabel("Method : ")%></i><b><%#: Eval("PaymentMethod") %></b><br />
                                                    <i>
                                                        <%=GetLabel("Type : ")%></i><b><%#: Eval("PaymentType") %></b><br />
                                                    <i>
                                                        <%=GetLabel("Tipe Kartu : ")%></i><b><%#: Eval("CardType") %></b>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("EDCMachineName") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("BankName") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("CashierGroup") %>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfPaymentAmountInString")%>
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
                </div>
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
