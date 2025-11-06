<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARPayerReceiveAlocationPerDetailEntryDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARPayerReceiveAlocationPerDetailEntryDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_ARPayerReceiveAlocationPerDetailEntryDetailCtl">

    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    function getCheckedARInvoiceReceiving() {
        var lstSelectedARInvoice = $('#<%=hdnSelectedARInvoiceDtID.ClientID %>').val().split(',');
        var lstSelectedReceivingAmount = $('#<%=hdnSelectedARReceivingAmount.ClientID %>').val().split(',');
        $('.chkARInvoice input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtAlocationAmountCtl = $tr.find('.txtAlocationAmountCtl').attr('hiddenVal').replace('.00', '').split(',').join('');
                var idx = lstSelectedARInvoice.indexOf(key);
                if (idx < 0) {
                    lstSelectedARInvoice.push(key);
                    lstSelectedReceivingAmount.push(txtAlocationAmountCtl);
                }
                else {
                    lstSelectedReceivingAmount[idx] = txtAlocationAmountCtl;
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
        $('#<%=hdnSelectedARInvoiceDtID.ClientID %>').val(lstSelectedARInvoice.join(','));
        $('#<%=hdnSelectedARReceivingAmount.ClientID %>').val(lstSelectedReceivingAmount.join(','));
    }

    $('.txtAlocationAmountCtl').live('change', function () {
        $('.txtAlocationAmountCtl').trigger('changeValue');
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
                var txtAlocationAmountCtl = parseFloat($tr.find('.txtAlocationAmountCtl').attr('hiddenVal').replace('.00', '').split(',').join(''));
                var ClaimedAmount = parseFloat($tr.find('.ClaimedAmount').val());
                var PaymentAmount = parseFloat($tr.find('.PaymentAmount').val());
                var OutstandingPayment = parseFloat($tr.find('.OutstandingPayment').val());

                totalClaimed = totalClaimed + ClaimedAmount;
                totalReceiving = totalReceiving + PaymentAmount;
                totalOutstanding = totalOutstanding + OutstandingPayment;
                totalAlocation = totalAlocation + txtAlocationAmountCtl;
            }
        });

        $('#<%=txtTotalClaimedAmount.ClientID %>').val(totalClaimed).trigger('changeValue');
        $('#<%=txtTotalReceivingAmount.ClientID %>').val(totalReceiving).trigger('changeValue');
        $('#<%=txtTotalOutstandingAmount.ClientID %>').val(totalOutstanding).trigger('changeValue');
        $('#<%=txtTotalAlocationAmount.ClientID %>').val(totalAlocation).trigger('changeValue');
    }

    function onCboFilterByValueChanged() {
        cbpProcessDetail.PerformCallback('Refresh');
    }

    function onBeforeSaveRecord(errMessage) {
        getCheckedARInvoiceReceiving();
        if ($('#<%=hdnSelectedARInvoiceDtID.ClientID %>').val() == '') {
            errMessage.text = 'Please Select AR Invoice First';
            return false;
        }
        return true;
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpProcessDetail.PerformCallback('Refresh');
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onCbpProcessDetailEndCallback(s) {
        $('.txtAlocationAmountCtl').each(function () {
            $('.txtAlocationAmountCtl').trigger('changeValue');
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
    <input type="hidden" id="hdnSelectedARInvoiceDtID" runat="server" value="" />
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
                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                        <qis:QISIntellisenseHint Text="NoRM" FieldName="MedicalNo" />
                        <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
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
                                                    <%=GetLabel("Invoice Info")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Detail Payment Info")%>
                                                </th>
                                                <th style="width: 180px">
                                                    <%=GetLabel("Registration Info")%>
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
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAllAR" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Invoice Info")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Detail Payment Info")%>
                                                </th>
                                                <th style="width: 180px">
                                                    <%=GetLabel("Registration Info")%>
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
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                                <input type="hidden" class="ClaimedAmount" id="ClaimedAmount" runat="server" value='<%#: Eval("ClaimedAmount")%>' />
                                                <input type="hidden" class="PaymentAmount" id="PaymentAmount" runat="server" value='<%#: Eval("PaymentAmount")%>' />
                                                <input type="hidden" class="OutstandingPayment" id="OutstandingPayment" runat="server" value='<%#: Eval("OutstandingPayment")%>' />
                                            </td>
                                            <td>
                                                <b><%#: Eval("ARInvoiceNo")%></b>
                                                <br />
                                                <%#: Eval("cfARInvoiceDateInString")%>
                                            </td>
                                            <td>
                                                <b><%#: Eval("PaymentNo")%></b>
                                                <br />
                                                <%#: Eval("cfPaymentDateInString")%>
                                            </td>
                                            <td>
                                                <b><%#: Eval("RegistrationNo")%></b>
                                                <br />
                                                <i>
                                                    <%#: Eval("MedicalNo")%></i>
                                                <br />
                                                <%#: Eval("PatientName")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfClaimedAmountInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfPaymentAmountInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfOutstandingPaymentInString")%>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="txtAlocationAmountCtl" CssClass="txtAlocationAmountCtl txtCurrency"
                                                    Width="120px" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
