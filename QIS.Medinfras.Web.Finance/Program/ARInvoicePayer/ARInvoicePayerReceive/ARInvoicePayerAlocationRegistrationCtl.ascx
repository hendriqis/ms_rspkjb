<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePayerAlocationRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerAlocationRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_alocationarinvoicepayerentryctl">
    $(function () {
        setTimeout(function () {
            setddeInvoiceNoDetailText();
            hideLoadingPanel();
        }, 0);

        $('#<%=txtPaymentAmount.ClientID %>').trigger('changeValue');
    });

    $('.chkIsSelectedCtl input').live('change', function () {
        $('.chkSelectAllCtl input').prop('checked', false);
        setddeInvoiceNoDetailText();
    });

    $('.chkSelectAllCtl input').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedCtl').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
        setddeInvoiceNoDetailText();
    });

    function setddeInvoiceNoDetailText() {
        var transactionAmount = 0;
        var lineAmount = 0;
        var invoiceNo = '';
        var lstInvoiceID = '';
        $('.chkIsSelectedCtl input:checked').each(function () {
            $tr = $(this).closest('tr');
            if (invoiceNo != '') {
                invoiceNo += ', ';
                lstInvoiceID += ',';
            }
            lstInvoiceID += $tr.find('.hdnKeyFieldCtl').val();
            invoiceNo += $.trim($tr.find('.lnkARInvoiceNoCtl').html());
            lineAmount += parseFloat($tr.find('.hdnClaimedAmount').val());
        });
        ddeInvoiceNoDetail.SetText(invoiceNo);
        $('#<%=hdnListInvoiceIDCtl.ClientID %>').val(lstInvoiceID);
        $('#<%=txtInvoiceTotal.ClientID %>').val(lineAmount).trigger('changeValue');
        $('#<%=hdnTotalInvoiceAmount.ClientID %>').val(lineAmount);
        cbpProcessDetail.PerformCallback('refresh');
    }

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();
        var totalPayment = $('#<%=txtPaymentAmount.ClientID %>').attr('hiddenVal');
        var totalDetailPayment = $('#<%=hdnTotalSelectedDtPaymentAmount.ClientID %>').val();
        $('#<%=txtInvoiceDetailPaymentTotal.ClientID %>').val(totalDetailPayment).trigger('changeValue');

        var totalOutstanding = totalPayment - totalDetailPayment;
        $('#<%=hdnInvoicePaymentOutstandingAmountDt.ClientID %>').val(totalOutstanding);
        $('#<%=txtInvoiceDetailOutstandingPaymentTotal.ClientID %>').val(totalOutstanding).trigger('changeValue');

        $('.txtPembayaran').each(function () {
            $(this).trigger('changeValue');
        });
    }

    function onBeforeSaveRecord() {
        $('#<%=hdnSelectedDtPaymentDetailID.ClientID %>').val('');
        $('#<%=hdnSelectedDtARInvoiceID.ClientID %>').val('');
        $('#<%=hdnSelectedDtRegistrationID.ClientID %>').val('');
        $('#<%=hdnSelectedDtPaymentID.ClientID %>').val('');
        $('#<%=hdnSelectedDtPaymentAmount.ClientID %>').val('');
        getCheckedMember();
        if ($('#<%=hdnSelectedDtPaymentDetailID.ClientID %>').val() != '') {
            return true;
        }
        else {
            errMessage.text = 'Please Select Invoice First';
            return false;
        }
    }

    function getCheckedMember() {
        var lstSelectedDtID = $('#<%=hdnSelectedDtARInvoiceDtID.ClientID %>').val().split(',');
        var lstSelectedDtPaymentDetailID = $('#<%=hdnSelectedDtPaymentDetailID.ClientID %>').val().split(',');
        var lstSelectedDtARInvoiceID = $('#<%=hdnSelectedDtARInvoiceID.ClientID %>').val().split(',');
        var lstSelectedDtRegistrationID = $('#<%=hdnSelectedDtRegistrationID.ClientID %>').val().split(',');
        var lstSelectedDtPaymentID = $('#<%=hdnSelectedDtPaymentID.ClientID %>').val().split(',');
        var lstSelectedDtPaymentAmount = $('#<%=hdnSelectedDtPaymentAmount.ClientID %>').val().split(',');
        var result = '';
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var DtARInvoiceDtID = $tr.find('.keyField').val();
                var DtPaymentDetailID = $tr.find('.hdnDtPaymentDetailID').val();
                var DtARInvoiceID = $tr.find('.hdnDtARInvoiceID').val();
                var DtRegistrationID = $tr.find('.hdnDtRegistrationID').val();
                var DtPaymentID = $tr.find('.hdnDtPaymentID').val();
                var DtPaymentAmount = $tr.find('.txtPembayaran').attr('hiddenVal');
                var idx = lstSelectedDtID.indexOf(DtARInvoiceDtID);
                if (idx < 0) {
                    lstSelectedDtID.push(DtARInvoiceDtID);
                    lstSelectedDtPaymentDetailID.push(DtPaymentDetailID);
                    lstSelectedDtARInvoiceID.push(DtARInvoiceID);
                    lstSelectedDtRegistrationID.push(DtRegistrationID);
                    lstSelectedDtPaymentID.push(DtPaymentID);
                    lstSelectedDtPaymentAmount.push(DtPaymentAmount);
                }
                else {
                    lstSelectedDtPaymentDetailID[idx] = DtPaymentDetailID;
                    lstSelectedDtARInvoiceID[idx] = DtARInvoiceID;
                    lstSelectedDtRegistrationID[idx] = DtRegistrationID;
                    lstSelectedDtPaymentID[idx] = DtPaymentID;
                    lstSelectedDtPaymentAmount[idx] = DtPaymentAmount;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html().trim();
                var idx = lstSelectedDtID.indexOf(key);
                if (idx > -1) {
                    lstSelectedDtID.splice(idx, 1);
                    lstSelectedDtPaymentDetailID.splice(idx, 1);
                    lstSelectedDtARInvoiceID.splice(idx, 1);
                    lstSelectedDtRegistrationID.splice(idx, 1);
                    lstSelectedDtPaymentID.splice(idx, 1);
                    lstSelectedDtPaymentAmount.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedDtARInvoiceDtID.ClientID %>').val(lstSelectedDtID.join(','));
        $('#<%=hdnSelectedDtPaymentDetailID.ClientID %>').val(lstSelectedDtPaymentDetailID.join(','));
        $('#<%=hdnSelectedDtARInvoiceID.ClientID %>').val(lstSelectedDtARInvoiceID.join(','));
        $('#<%=hdnSelectedDtRegistrationID.ClientID %>').val(lstSelectedDtRegistrationID.join(','));
        $('#<%=hdnSelectedDtPaymentID.ClientID %>').val(lstSelectedDtPaymentID.join(','));
        $('#<%=hdnSelectedDtPaymentAmount.ClientID %>').val(lstSelectedDtPaymentAmount.join(','));
    }

    $('.chkIsSelectedDetail input').live('change', function () {
        var isChecked = $(this).is(":checked");
        $txt = $(this).closest('tr').find('.txtPembayaran');
        if (isChecked) {
            $txt.removeAttr('readonly');
        }
        else {
            $txt.attr('readonly', 'readonly');
        }

        $('.chkSelectAllDetail input').prop('checked', false);
        calculateTotalVerification();
    });

    $('#chkSelectAllDetail').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedDetail input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
        calculateTotalVerification();
    });

    $('.txtPembayaran').live('change', function () {
        $(this).trigger('changeValue');
        calculateTotalVerification();
    });

    function calculateTotalVerification() {
        var lstSelectedPayment = 0;
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var paymentAmount = parseFloat($tr.find('.txtPembayaran').attr('hiddenVal'));
                lstSelectedPayment += paymentAmount;
            }
        });
        $('#<%=hdnTotalInputSelectedDtPaymentAmount.ClientID %>').val(lstSelectedPayment);
        $('#<%=txtInvoiceDetailTotal.ClientID %>').val(lstSelectedPayment).trigger('changeValue');
    }
</script>
<div style="height: 400px; overflow-y: auto">
    <input type="hidden" id="hdnARReceivingID" runat="server" value="" />
    <input type="hidden" id="hdnTotalInvoiceAmount" runat="server" value="" />
    <input type="hidden" id="hdnListInvoiceIDCtl" runat="server" value="0" />
    <input type="hidden" id="hdnClaimedAmountDt" runat="server" value="" />
    <input type="hidden" id="hdnInvoicePaymentOutstandingAmountDt" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtPaymentDetailID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtARInvoiceDtID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtARInvoiceID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtPaymentAmount" runat="server" value="" />
    <input type="hidden" id="hdnTotalInputSelectedDtPaymentAmount" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 55%" />
            <col style="width: 55%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top" style="width: 100%">
                <table class="tblEntryContent">
                    <tr>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 450px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Pembayaran")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtARReceivingNo" Style="text-align: center" ReadOnly="true" Width="150px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Tagihan")%></label>
                                    </td>
                                    <td>
                                        <div id="divddeInvoiceNoDetail">
                                            <dxe:ASPxDropDownEdit ClientInstanceName="ddeInvoiceNoDetail" ID="ddeInvoiceNoDetail"
                                                Width="400px" runat="server" EnableAnimation="False">
                                                <DropDownWindowStyle BackColor="#EDEDED" Font-Size="Medium" />
                                                <DropDownWindowTemplate>
                                                    <asp:ListView ID="lvwInvoice" runat="server">
                                                        <EmptyDataTemplate>
                                                            <table id="tblView" style="font-size: 0.8em" runat="server" class="grdNormal notAllowSelect"
                                                                cellspacing="0" rules="all">
                                                                <tr>
                                                                    <th style="width: 40px" rowspan="2">
                                                                        <div style="padding: 3px">
                                                                            <asp:CheckBox ID="chkSelectAllCtl" runat="server" CssClass="chkSelectAllCtl" />
                                                                        </div>
                                                                    </th>
                                                                    <th rowspan="2" align="left" style="width: 100px">
                                                                        <div style="padding: 3px; float: left;">
                                                                            <div>
                                                                                <%= GetLabel("No Tagihan")%></div>
                                                                            <div>
                                                                                <%= GetLabel("Tanggal Tagihan")%></div>
                                                                        </div>
                                                                    </th>
                                                                    <th colspan="3">
                                                                        <%=GetLabel("Jumlah")%>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <th style="width: 120px">
                                                                        <div style="text-align: right; padding-right: 3px">
                                                                            <%=GetLabel("Total Klaim")%>
                                                                        </div>
                                                                    </th>
                                                                    <th style="width: 120px">
                                                                        <div style="text-align: right; padding-right: 3px">
                                                                            <%=GetLabel("Total Bayar")%>
                                                                        </div>
                                                                    </th>
                                                                    <th style="width: 120px">
                                                                        <div style="text-align: right; padding-right: 3px">
                                                                            <%=GetLabel("Total Oustanding")%>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                                <tr class="trEmpty">
                                                                    <td colspan="6">
                                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                        <LayoutTemplate>
                                                            <table id="tblView" style="font-size: 0.8em" runat="server" class="grdNormal notAllowSelect"
                                                                cellspacing="0" rules="all">
                                                                <tr>
                                                                    <th style="width: 40px" rowspan="2">
                                                                        <div style="padding: 3px">
                                                                            <asp:CheckBox ID="chkSelectAllCtl" runat="server" CssClass="chkSelectAllCtl" />
                                                                        </div>
                                                                    </th>
                                                                    <th rowspan="2" align="left">
                                                                        <div style="padding: 3px; float: left;">
                                                                            <div>
                                                                                <%= GetLabel("No Tagihan")%></div>
                                                                            <div>
                                                                                <%= GetLabel("Tanggal Tagihan")%></div>
                                                                        </div>
                                                                    </th>
                                                                    <th colspan="3">
                                                                        <%=GetLabel("Jumlah")%>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <th style="width: 120px">
                                                                        <div style="text-align: right; padding-right: 3px">
                                                                            <%=GetLabel("Total Klaim")%>
                                                                        </div>
                                                                    </th>
                                                                    <th style="width: 120px">
                                                                        <div style="text-align: right; padding-right: 3px">
                                                                            <%=GetLabel("Total Bayar")%>
                                                                        </div>
                                                                    </th>
                                                                    <th style="width: 120px">
                                                                        <div style="text-align: right; padding-right: 3px">
                                                                            <%=GetLabel("Total Oustanding")%>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td align="center">
                                                                    <div style="padding: 3px">
                                                                        <asp:CheckBox ID="chkIsSelectedCtl" Checked="true" CssClass="chkIsSelectedCtl" runat="server" />
                                                                        <input type="hidden" class="hdnKeyFieldCtl" value="<%#: Eval("ARInvoiceID")%>" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div style="padding: 3px; float: left;">
                                                                        <a class="lnkARInvoiceNoCtl">
                                                                            <%#: Eval("ARInvoiceNo")%></a>
                                                                        <div>
                                                                            <%#: Eval("ARInvoiceDate")%></div>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div style="padding: 3px; text-align: right;">
                                                                        <input type="hidden" class="hdnClaimedAmount" value='<%#: Eval("TotalClaimedAmount")%>' />
                                                                        <div>
                                                                            <%#: Eval("TotalClaimedAmount", "{0:N}")%></div>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div style="padding: 3px; text-align: right;">
                                                                        <input type="hidden" class="hdnTotalPaymentAmountCtl" value='<%#: Eval("TotalPaymentAmount")%>' />
                                                                        <div>
                                                                            <%#: Eval("TotalPaymentAmount", "{0:N}")%></div>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div style="padding: 3px; text-align: right;">
                                                                        <input type="hidden" class="hdnLineAmount" value='<%#: Eval("RemainingAmount")%>' />
                                                                        <div>
                                                                            <%#: Eval("RemainingAmount", "{0:N}")%></div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </DropDownWindowTemplate>
                                            </dxe:ASPxDropDownEdit>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Tagihan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceTotal" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Green" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top" style="width: 100%">
                <table class="tblEntryContent">
                    <tr>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Pembayaran")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentAmount" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Blue" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Terbayar (Detail)")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceDetailPaymentTotal" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Purple" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Kurang Bayar (Detail)")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceDetailOutstandingPaymentTotal" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Red" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Terpilih (Detail)")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceDetailTotal" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Black" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative; height: 270px; overflow-y: auto" id="divView">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <input type="hidden" id="hdnTotalSelectedDtPaymentAmount" runat="server" value="" />
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center" id="thSelectAll">
                                                        <input id="chkSelectAllDetail" type="checkbox" />
                                                    </th>
                                                    <th align="left" style="width: 110px">
                                                        <%=GetLabel("No. Invoice")%>
                                                    </th>
                                                    <th align="left" style="width: 110px">
                                                        <%=GetLabel("No. Registrasi")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th align="left" style="width: 110px">
                                                        <%=GetLabel("No. Piutang")%>
                                                    </th>
                                                    <th align="right" style="width: 110px">
                                                        <%=GetLabel("Jumlah Klaim")%>
                                                    </th>
                                                    <th align="right" style="width: 110px">
                                                        <%=GetLabel("Jumlah Terbayar")%>
                                                    </th>
                                                    <th align="right" style="width: 110px">
                                                        <%=GetLabel("Jumlah Outstanding")%>
                                                    </th>
                                                    <th align="right">
                                                        <%=GetLabel("Jumlah Bayar")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="8">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center" id="thSelectAll">
                                                        <input id="chkSelectAllDetail" type="checkbox" />
                                                    </th>
                                                    <th align="left" style="width: 110px">
                                                        <%=GetLabel("No. Invoice")%>
                                                    </th>
                                                    <th align="left" style="width: 110px">
                                                        <%=GetLabel("No. Registrasi")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th align="left" style="width: 110px">
                                                        <%=GetLabel("No. Piutang")%>
                                                    </th>
                                                    <th align="right" style="width: 110px">
                                                        <%=GetLabel("Jumlah Klaim")%>
                                                    </th>
                                                    <th align="right" style="width: 110px">
                                                        <%=GetLabel("Jumlah Terbayar")%>
                                                    </th>
                                                    <th align="right" style="width: 110px">
                                                        <%=GetLabel("Jumlah Outstanding")%>
                                                    </th>
                                                    <th align="right">
                                                        <%=GetLabel("Jumlah Bayar")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelectedDetail" runat="server" CssClass="chkIsSelectedDetail" />
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                                    <input type="hidden" class="hdnDtARInvoiceID" id="hdnDtARInvoiceID" value="<%#: Eval("ARInvoiceID")%>" />
                                                    <input type="hidden" class="hdnDtRegistrationID" id="hdnDtRegistrationID" value="<%#: Eval("RegistrationID")%>" />
                                                    <input type="hidden" class="hdnDtPaymentID" id="hdnDtPaymentID" value="<%#: Eval("PaymentID")%>" />
                                                    <input type="hidden" class="hdnDtPaymentDetailID" id="hdnDtPaymentDetailID" value="<%#: Eval("PaymentDetailID")%>" />
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("ARInvoiceNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("RegistrationNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("PatientName")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("PaymentNo")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("ClaimedAmount", "{0:N2}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("PaymentAmount", "{0:N2}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("TotalOutstandingPayment", "{0:N2}")%>
                                                </td>
                                                <td align="center">
                                                    <input type="hidden" class="hdnDtPaymentAmount" id="hdnDtPaymentAmount" value='<%#: Eval("TotalOutstandingPayment")%>' />
                                                    <asp:TextBox ID="txtPembayaran" Width="80%" runat="server" ReadOnly="true" CssClass="txtPembayaran txtCurrency" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="divLoading">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
