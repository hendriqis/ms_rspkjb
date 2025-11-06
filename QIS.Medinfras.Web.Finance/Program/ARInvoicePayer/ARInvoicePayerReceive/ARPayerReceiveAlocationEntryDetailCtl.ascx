<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARPayerReceiveAlocationEntryDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARPayerReceiveAlocationEntryDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_ARPayerReceiveAlocationEntryDetailCtl">

    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    function getCheckedARInvoiceReceiving() {
        var lstSelectedARInvoice = $('#<%=hdnSelectedARInvoice.ClientID %>').val().split(',');
        var lstSelectedReceivingAmount = $('#<%=hdnSelectedARReceivingAmount.ClientID %>').val().split(',');
        $('.chkARInvoice input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtAlocationAmount = $tr.find('.txtAlocationAmount').attr('hiddenVal').replace('.00', '').split(',').join('');

                var idx = lstSelectedARInvoice.indexOf(key);
                if (idx < 0) {
                    lstSelectedARInvoice.push(key);
                    lstSelectedReceivingAmount.push(txtAlocationAmount);
                }
                else {
                    lstSelectedReceivingAmount[idx] = txtAlocationAmount;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedARInvoice.indexOf(key);
                if (idx > -1) {
                    lstSelectedARInvoice.splice(idx, 1);
                    lstSelectedReceivingAmount.splice(idx, 1);
                }
            }
        });

        $('#<%=hdnSelectedARInvoice.ClientID %>').val(lstSelectedARInvoice.join(','));
        $('#<%=hdnSelectedARReceivingAmount.ClientID %>').val(lstSelectedReceivingAmount.join(','));
    }

    $('.txtAlocationAmount').live('change', function () {
        $('.txtAlocationAmount').trigger('changeValue');
        calculateTotal();
    });

    $('#chkSelectAllAR').die('change');
    $('#chkSelectAllAR').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkARInvoice').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
            calculateTotal();
        });
    });

    $('.chkARInvoice input').live('click', function () {
        $('.chkSelectAllAR input').prop('checked', false);
        calculateTotal();
    });

    function calculateTotal() {
        var totalClaimed = 0;
        var totalReceiving = 0;
        var totalOutstanding = 0;
        var totalAlocation = 0;

        $('.chkARInvoice input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtAlocationAmount = parseFloat($tr.find('.txtAlocationAmount').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var TotalClaimedAmount = parseFloat($tr.find('.TotalClaimedAmount').val());
                var TotalPaymentAmount = parseFloat($tr.find('.TotalPaymentAmount').val());
                var RemainingAmount = parseFloat($tr.find('.RemainingAmount').val());

                totalClaimed = totalClaimed + TotalClaimedAmount;
                totalReceiving = totalReceiving + TotalPaymentAmount;
                totalOutstanding = totalOutstanding + RemainingAmount;
                totalAlocation = totalAlocation + txtAlocationAmount;
            }
        });

        $('#<%=txtTotalClaimedAmount.ClientID %>').val(totalClaimed).trigger('changeValue');
        $('#<%=txtTotalReceivingAmount.ClientID %>').val(totalReceiving).trigger('changeValue');
        $('#<%=txtTotalOutstandingAmount.ClientID %>').val(totalOutstanding).trigger('changeValue');
        $('#<%=txtTotalAlocationAmount.ClientID %>').val(totalAlocation).trigger('changeValue');
    }

    function onCboFilterByValueChanged() {
        cbpProcessDetail.PerformCallback('refresh');
    }

    function onBeforeSaveRecord(errMessage) {
        getCheckedARInvoiceReceiving();
        if ($('#<%=hdnSelectedARInvoice.ClientID %>').val() == '') {
            errMessage.text = 'Please Select AR Invoice First';
            return false;
        }
        return true;
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpProcessDetail.PerformCallback('refresh');
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onCbpProcessDetailEndCallback(s) {
        $('.txtAlocationAmount').each(function () {
            $('.txtAlocationAmount').trigger('changeValue');
        });
        hideLoadingPanel();
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        getCheckedARInvoiceReceiving();
    }
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnSelectedARInvoice" runat="server" value="" />
    <input type="hidden" id="hdnSelectedARReceivingAmount" runat="server" value="" />
    <input type="hidden" id="hdnARReceivingIDCtl" runat="server" value="" />
    <table>
        <colgroup>
            <col style="width: 120px" />
            <col style="width: 500px" />
            <col style="width: 170px" />
            <col style="width: 300px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="lblFilterCaption">
                    <%=GetLabel("Filter")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboFilterBy" ClientInstanceName="cboFilterBy" Width="200px"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e) { onCboFilterByValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
            <td class="tdLabel" style="font-style: oblique">
                <label>
                    <%=GetLabel("Total Claimed Amount") %></label>
            </td>
            <td align="right">
                <asp:TextBox ID="txtTotalClaimedAmount" ReadOnly="true" Width="90%" CssClass="txtCurrency"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td>
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="350px" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="No Invoice" FieldName="ARInvoiceNo" />
                        <qis:QISIntellisenseHint Text="Tgl Invoice (yyyy-mm-dd)" FieldName="ARInvoiceDate" />
                        <qis:QISIntellisenseHint Text="Keterangan" FieldName="Remarks" />

                    </IntellisenseHints>
                </qis:QISIntellisenseTextBox>
            </td>
            <td class="tdLabel" style="font-style: oblique">
                <label>
                    <%=GetLabel("Total Receiving Amount") %></label>
            </td>
            <td align="right">
                <asp:TextBox ID="txtTotalReceivingAmount" ReadOnly="true" Width="90%" CssClass="txtCurrency"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="tdLabel" style="font-style: oblique">
                <label>
                    <%=GetLabel("Total Outstanding Amount") %></label>
            </td>
            <td align="right">
                <asp:TextBox ID="txtTotalOutstandingAmount" ReadOnly="true" Width="90%" CssClass="txtCurrency" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="tdLabel" style="font-style: oblique">
                <label>
                    <%=GetLabel("Total Alocation Amount") %></label>
            </td>
            <td align="right">
                <asp:TextBox ID="txtTotalAlocationAmount" ReadOnly="true" Width="90%" CssClass="txtCurrency" runat="server" />
            </td>
        </tr>
    </table>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
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
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAllAR" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Invoice No")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Reference No")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Invoice Date")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Claimed Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Receiving Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Outstanding Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Alocation Amount")%>
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
                                                    <input id="chkSelectAllAR" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Invoice No")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Reference No")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Invoice Date")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Claimed Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Receiving Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Outstanding Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Alocation Amount")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkARInvoice" runat="server" CssClass="chkARInvoice" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ARInvoiceID")%>' />
                                                <input type="hidden" class="TotalClaimedAmount" id="TotalClaimedAmount" runat="server" value='<%#: Eval("TotalClaimedAmount")%>' />
                                                <input type="hidden" class="TotalPaymentAmount" id="TotalPaymentAmount" runat="server" value='<%#: Eval("TotalPaymentAmount")%>' />
                                                <input type="hidden" class="RemainingAmount" id="RemainingAmount" runat="server" value='<%#: Eval("RemainingAmount")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("ARInvoiceNo")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ARReferenceNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("ARInvoiceDateInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfTotalClaimedAmountInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfTotalPaymentAmountInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfRemainingAmountInString")%>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="txtAlocationAmount" CssClass="txtAlocationAmount txtCurrency"
                                                    Width="120px" />
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
