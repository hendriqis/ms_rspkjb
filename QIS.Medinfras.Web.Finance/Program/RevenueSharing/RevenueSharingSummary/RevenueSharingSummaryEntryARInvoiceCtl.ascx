<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingSummaryEntryARInvoiceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingSummaryEntryARInvoiceCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_RevenueSharingSummaryEntryARInvoiceCtl">

    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    $(function () {
        setDatePicker('<%=txtPeriodFrom.ClientID %>');
        $('#<%=txtPeriodFrom.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtPeriodTo.ClientID %>');
        $('#<%=txtPeriodTo.ClientID %>').datepicker('option', 'maxDate', '0');

    });

    $('#btnRefresh').live('click', function () {
        getCheckedARInvoice();
        cbpProcessDetail.PerformCallback('refresh');
    });

    function getCheckedARInvoice() {
        var lstSelectedARInvoice = $('#<%=hdnSelectedARParamedicInvoiceID.ClientID %>').val().split(',');
        var lstSelectedRevenueARClaimedAmount = $('#<%=hdnSelectedARClaimedAmount.ClientID %>').val().split(',');
        $('.chkARParamedicInvoice input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var idx = lstSelectedARInvoice.indexOf(key);
                var revenueAdjAmount = $tr.find('.txtCopyRevenueAdjustment').val().replace('.00', '').split(',').join('');
                if (idx < 0) {
                    lstSelectedARInvoice.push(key);
                    lstSelectedRevenueARClaimedAmount.push(revenueAdjAmount);
                } else {
                    lstSelectedRevenueARClaimedAmount[idx] = revenueAdjAmount;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedARInvoice.indexOf(key);
                if (idx > -1) {
                    lstSelectedARInvoice.splice(idx, 1);
                    lstSelectedRevenueARClaimedAmount.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedARParamedicInvoiceID.ClientID %>').val(lstSelectedARInvoice.join(','));
        $('#<%=hdnSelectedARClaimedAmount.ClientID %>').val(lstSelectedRevenueARClaimedAmount.join(','));
    }

    $('#chkAllARParamedicInvoice').die('change');
    $('#chkAllARParamedicInvoice').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkARParamedicInvoice input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
        calculateTotalAdjustmentSelected();
    });

    $('.chkARParamedicInvoice input').live('change', function () {
        var isChecked = $(this).is(":checked");
        $txt = $(this).closest('tr').find('.txtCopyRevenueAdjustment');
        if (isChecked) {
            $txt.removeAttr('readonly');
        }
        else {
            $txt.attr('readonly', 'readonly');
        }

        calculateTotalAdjustmentSelected();
        $('.chkAllARParamedicInvoice input').prop('checked', false);
    });

    $('.txtCopyRevenueAdjustment').live('change', function () {
        $(this).trigger('changeValue');
        calculateTotalAdjustmentSelected();
    });

    function calculateTotalAdjustmentSelected() {
        var lstSelectedCopy = 0;
        $('.chkARParamedicInvoice input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var copyAmount = parseFloat($tr.find('.txtCopyRevenueAdjustment').val().replace('.00', '').split(',').join(''));
                lstSelectedCopy += copyAmount;
            }
        });
        $('#<%=txtCopyRevenueAdjustmentTotal.ClientID %>').val(lstSelectedCopy).trigger('changeValue');
        $('#<%=hdnSelectedARClaimedAmountTotal.ClientID %>').val(lstSelectedCopy);
    }

    function onBeforeSaveRecord(errMessage) {
        getCheckedARInvoice();
        if ($('#<%=hdnSelectedARParamedicInvoiceID.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Transaction First';
            displayErrorMessageBox('', errMessage.text);
            return false;
        } else {
            var totalARAdj = parseFloat($('#<%=hdnSelectedARClaimedAmountTotal.ClientID %>').val());
            var totalMaxAdj = parseFloat($('#<%=hdnRSSummaryMaxAmountCtl.ClientID %>').val());

            if (totalARAdj > totalMaxAdj) {
                errMessage.text = 'Nilai salin piutang dokter tidak boleh melebihi nilai transaksi rekap jasa medis nya.';
                displayErrorMessageBox('', errMessage.text);
                return false;
            } else {
                return true;
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        getCheckedARInvoice();

        $('.txtCopyRevenueAdjustment').each(function () {
            $(this).trigger('changeValue');
        });
    }
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnRSSummaryIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnRSSummaryMaxAmountCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedARParamedicInvoiceID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedARClaimedAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedARClaimedAmountTotal" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td align="left" valign="top">
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Invoice") %></label>
                                    </td>
                                    <td>
                                        <table>
                                            <colgroup>
                                                <col style="width: 150px;" />
                                                <col style="width: 30px;" />
                                                <col style="width: 150px;" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" Width="120px" />
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel("s/d") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" Width="120px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" valign="top">
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Klaim Terpilih")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCopyRevenueAdjustmentTotal" CssClass="txtCurrency" ReadOnly="true"
                                            Width="200px" Style="text-align: right; color: Blue" runat="server" Text="0" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
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
                                                <th style="width: 40px" align="center">
                                                    <input id="chkAllARParamedicInvoice" type="checkbox" />
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Penjamin Bayar") %>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No Invoice") %>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Invoice") %>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Jatuh Tempo") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Total Klaim") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai Salin Piutang") %>
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
                                                <th style="width: 40px" align="center">
                                                    <input id="chkAllARParamedicInvoice" type="checkbox" />
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Penjamin Bayar") %>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No Invoice") %>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Invoice") %>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Jatuh Tempo") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Total Klaim") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai Salin Piutang") %>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkARParamedicInvoice" runat="server" CssClass="chkARParamedicInvoice" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ARInvoiceID")%>' />
                                            </td>
                                            <td align="left">
                                                <%#:Eval("BusinessPartnerName")%>
                                                (<%#:Eval("BusinessPartnerCode")%>)
                                            </td>
                                            <td align="left">
                                                <%#:Eval("ARInvoiceNo")%>
                                            </td>
                                            <td align="center">
                                                <%#:Eval("cfARInvoiceDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#:Eval("cfDueDateInString")%>
                                            </td>
                                            <td align="right">
                                                <%#:Eval("cfTotalClaimedAmountInString")%>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCopyRevenueAdjustment" Width="80%" runat="server" ReadOnly="true"
                                                    CssClass="txtCopyRevenueAdjustment txtCurrency" />
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
