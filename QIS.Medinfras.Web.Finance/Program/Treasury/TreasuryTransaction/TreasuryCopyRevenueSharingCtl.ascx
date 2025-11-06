<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryCopyRevenueSharingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryCopyRevenueSharingCtl" %>
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
<script type="text/javascript" id="dxss_copyrevenuesharingtreasuryctl">
    $('#btnRefresh').live('click', function () {
        $('.grdView .chkSelectAllRSPayment input').each(function () {
            $(this).prop('checked', false);
        });
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpProcessDetail.PerformCallback('refresh');
    });

    $('#chkSelectAllRSPayment').die('change');
    $('#chkSelectAllRSPayment').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectRSPayment').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function getCheckedRevenueSharing() {
        var lstSelectedRevenueSharing = $('#<%=hdnSelectedRSPaymentID.ClientID %>').val().split(',');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split(',');
        $('.chkSelectRSPayment input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtRemarks = $tr.find('.txtRemarks').val();
                var idx = lstSelectedRevenueSharing.indexOf(key);
                if (idx < 0) {
                    lstSelectedRevenueSharing.push(key);
                    lstSelectedRemarks.push(txtRemarks);
                }
                else {
                    lstSelectedRemarks[idx] = txtRemarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedRevenueSharing.indexOf(key);
                if (idx > -1) {
                    lstSelectedRevenueSharing.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedRSPaymentID.ClientID %>').val(lstSelectedRevenueSharing.join(','));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join(','));
    }

    function onBeforeSaveRecord(errMessage) {
        var accountdt = $('#<%=hdnGLTransactionIDctl.ClientID %>').val();
        if (accountdt != "" && accountdt != "0") {
            getCheckedRevenueSharing();
            if ($('#<%=hdnSelectedRSPaymentID.ClientID %>').val() == '') {
                errMessage.text = 'Please Select Revenue Sharing Payment First';
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
        getCheckedRevenueSharing();
    }

    function onCboPaymentMethodValueChanged(evt) {
        var value = cboPaymentMethod.GetValue();
        if (value == '<%=GetRevenuePaymentMethodTransfer() %>' || value == null) {
            $('#<%=trBank.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=trBank.ClientID %>').attr('style', 'display:none');
        }
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
    <input type="hidden" id="hdnCOARevenueSharingCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRSPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRemarks" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 300px" />
            <col style="width: 300px" />
            <col />
        </colgroup>
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
                        <qis:QISIntellisenseHint Text="No. Pembayaran" FieldName="RSPaymentNo" />
                        <qis:QISIntellisenseHint Text="Tanggal Verifikasi (YYYY-MM-DD)" FieldName="VerificationDate" />
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
                                                    <input id="chkSelectAllRSPayment" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No. Pembayaran")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Cara Pembayaran")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Bank")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tanggal Verifikasi")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Diverifikasi Oleh")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Catatan")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="9">
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
                                                    <input id="chkSelectAllRSPayment" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No. Pembayaran")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("Cara Pembayaran")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Bank")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tanggal Verifikasi")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Diverifikasi Oleh")%>
                                                </th>
                                                <th style="width: 150px" align="right">
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
                                                <asp:CheckBox ID="chkSelectRSPayment" runat="server" CssClass="chkSelectRSPayment" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("RSPaymentID")%>' />
                                                <input type="hidden" class="RSPaymentNo" id="RSPaymentNo" runat="server"
                                                    value='<%#: Eval("RSPaymentNo")%>' />
                                                <input type="hidden" class="TotalRevenueSharingAmount" id="TotalRevenueSharingAmount" runat="server"
                                                    value='<%#: Eval("TotalRevenueSharingAmount")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("RSPaymentNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("SupplierPaymentMethod") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("BankName") %>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfVerificationDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("VerifiedByName") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("TotalRevenueSharingAmount", "{0:N2}")%>
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
