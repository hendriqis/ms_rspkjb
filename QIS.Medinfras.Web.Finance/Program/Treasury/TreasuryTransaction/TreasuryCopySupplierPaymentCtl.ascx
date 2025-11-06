<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryCopySupplierPaymentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryCopySupplierPaymentCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_copysupplierpaymenttreasuryctl">

    $('#btnRefresh').live('click', function () {
        $('.grdView .chkSelectSupplierPayment input').each(function () {
            $(this).prop('checked', false);
        });
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpProcessDetail.PerformCallback('refresh');
    });

    $('#chkSelectAllSPH').die('change');
    $('#chkSelectAllSPH').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectSupplierPayment').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function getCheckedSupplierPayment() {
        var lstSelectedSupplierPayment = $('#<%=hdnSelectedSupplierPaymentID.ClientID %>').val().split(',');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split(',');
        $('.chkSelectSupplierPayment input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtRemarks = $tr.find('.txtRemarks').val();
                var idx = lstSelectedSupplierPayment.indexOf(key);
                if (idx < 0) {
                    lstSelectedSupplierPayment.push(key);
                    lstSelectedRemarks.push(txtRemarks);
                }
                else {
                    lstSelectedRemarks[idx] = txtRemarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedSupplierPayment.indexOf(key);
                if (idx > -1) {
                    lstSelectedSupplierPayment.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedSupplierPaymentID.ClientID %>').val(lstSelectedSupplierPayment.join(','));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join(','));
    }

    function onBeforeSaveRecord(errMessage) {
        var accountdt = $('#<%=hdnGLTransactionIDctl.ClientID %>').val();
        if (accountdt != "" && accountdt != "0") {
            getCheckedSupplierPayment();
            if ($('#<%=hdnSelectedSupplierPaymentID.ClientID %>').val() == '') {
                errMessage.text = 'Please Select Supplier Payment First';
                return false;
            }
        } else {
            errMessage.text = 'Please Select Account Detail First';
            return false;
        }
        return true;
    }

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();
        getCheckedSupplierPayment();
    }

    function onCboPaymentMethodValueChanged(evt) {
        var value = cboPaymentMethod.GetValue();
        if (value == '<%=GetSupplierPaymentMethodTransfer() %>' || value == '<%=GetSupplierPaymentMethodGiro() %>' || value == '<%=GetSupplierPaymentMethodCheque() %>' || value == null) {
            $('#<%=trBank.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=trBank.ClientID %>').attr('style', 'display:none');
        }
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnFN0171" runat="server" value="" />
    <input type="hidden" id="hdnGLTransactionIDctl" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountTreasuryIDctl" runat="server" value="" />
    <input type="hidden" id="hdnTreasuryTypectl" runat="server" value="" />
    <input type="hidden" id="hdnDisplayOrderTemp" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnBusinessPartnerIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnCashFlowTypeIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnJournalDateCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedSupplierPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRemarks" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 300px" />
            <col style="width: 300px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <%=GetLabel("Filter Data") %>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboFilterData" ClientInstanceName="cboFilterData" Width="300px"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Cara Pembayaran")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" Width="200px"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e) { onCboPaymentMethodValueChanged(e); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr id="trBank" runat="server">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Bank")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" Width="200px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td colspan="2">
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="300px" runat="server" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="No. Pembayaran" FieldName="SupplierPaymentNo" />
                        <qis:QISIntellisenseHint Text="Tgl Verifikasi (YYYY-MM-DD)" FieldName="VerificationDate" />
                        <qis:QISIntellisenseHint Text="Tgl Rencana Bayar (YYYY-MM-DD)" FieldName="PlanningPaymentDate" />
                        <qis:QISIntellisenseHint Text="Diverifikasi Oleh" FieldName="VerifiedByName" />
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
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="3">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 20px" align="center">
                                                    <input id="chkSelectAllSPH" type="checkbox" />
                                                </th>
                                                <th style="width: 130px">
                                                    <%=GetLabel("No. Pembayaran")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Tgl Verifikasi")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Tgl Rencana Bayar")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Cara Pembayaran")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Bank")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Tgl Proposed")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Diverifikasi (Proposed) Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Nilai")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Catatan")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 20px" align="center">
                                                    <input id="chkSelectAllSPH" type="checkbox" />
                                                </th>
                                                <th style="width: 130px">
                                                    <%=GetLabel("No. Pembayaran")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Tgl Verifikasi")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Tgl Rencana Bayar")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Cara Pembayaran")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Bank")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Tgl Proposed")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Diverifikasi (Proposed) Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Nilai")%>
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
                                                <asp:CheckBox ID="chkSelectSupplierPayment" runat="server" CssClass="chkSelectSupplierPayment" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("SupplierPaymentID")%>' />
                                                <input type="hidden" class="SupplierPaymentNo" id="SupplierPaymentNo" runat="server"
                                                    value='<%#: Eval("SupplierPaymentNo")%>' />
                                                <input type="hidden" class="TotalPaymentAmount" id="TotalPaymentAmount" runat="server"
                                                    value='<%#: Eval("TotalPaymentAmount")%>' />
                                                <input type="hidden" class="BusinessPartnerID" id="BusinessPartnerID" runat="server"
                                                    value='<%#: Eval("BusinessPartnerID")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("SupplierPaymentNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("VerificationDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("PlanningPaymentDateInString") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("PaymentMethod") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("BankName") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("VerifiedDateInString") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("VerifiedByName") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("TotalPaymentAmount", "{0:N}")%>
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
