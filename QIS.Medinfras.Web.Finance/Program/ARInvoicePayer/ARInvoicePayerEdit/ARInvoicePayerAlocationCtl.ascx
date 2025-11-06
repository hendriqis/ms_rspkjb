<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePayerAlocationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerAlocationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_alocationarinvoice2payerentryctl">
    $(function () {
        setTimeout(function () {
            cbpProcessDetail.PerformCallback('refresh');
            hideLoadingPanel();
        }, 0);

        $('#<%=txtPaymentAmount.ClientID %>').trigger('changeValue');
        $('#<%=txtInvoiceTotal.ClientID %>').trigger('changeValue');
    });
    
    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();
        var totalDetailClaim = $('#<%=hdnTotalSelectedDtClaimAmount.ClientID %>').val();
        var totalDetailPayment = $('#<%=hdnTotalSelectedDtPaymentAmount.ClientID %>').val();
        $('#<%=txtInvoiceDetailPaymentTotal.ClientID %>').val(totalDetailPayment).trigger('changeValue');

        var totalOutstanding = totalDetailClaim - totalDetailPayment;
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
        var lstSelectedDtID = $('#<%=hdnSelectedDtID.ClientID %>').val().split(',');
        var lstSelectedDtPaymentDetailID = $('#<%=hdnSelectedDtPaymentDetailID.ClientID %>').val().split(',');
        var lstSelectedDtARInvoiceID = $('#<%=hdnSelectedDtARInvoiceID.ClientID %>').val().split(',');
        var lstSelectedDtRegistrationID = $('#<%=hdnSelectedDtRegistrationID.ClientID %>').val().split(',');
        var lstSelectedDtPaymentID = $('#<%=hdnSelectedDtPaymentID.ClientID %>').val().split(',');
        var lstSelectedDtPaymentAmount = $('#<%=hdnSelectedDtPaymentAmount.ClientID %>').val().split(',');
        var result = '';
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var DtID = $tr.find('.keyField').val();
                var DtPaymentDetailID = $tr.find('.hdnDtPaymentDetailID').val();
                var DtARInvoiceID = $tr.find('.hdnDtARInvoiceID').val();
                var DtRegistrationID = $tr.find('.hdnDtRegistrationID').val();
                var DtPaymentID = $tr.find('.hdnDtPaymentID').val();
                var DtPaymentAmount = $tr.find('.txtPembayaran').attr('hiddenVal');
                var idx = lstSelectedDtID.indexOf(DtID);
                if (idx < 0) {
                    lstSelectedDtID.push(DtID);
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
                var idx = lstSelectedDtPaymentDetailID.indexOf(key);
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
        $('#<%=hdnSelectedDtID.ClientID %>').val(lstSelectedDtID.join(','));
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
    <input type="hidden" id="hdnIsFromAlocation" runat="server" value="1" />
    <input type="hidden" id="hdnARInvoiceID" runat="server" value="" />
    <input type="hidden" id="hdnTotalInvoiceAmount" runat="server" value="" />
    <input type="hidden" id="hdnListInvoiceIDCtl" runat="server" value="0" />
    <input type="hidden" id="hdnClaimedAmountDt" runat="server" value="" />
    <input type="hidden" id="hdnInvoicePaymentOutstandingAmountDt" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtPaymentDetailID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtARInvoiceID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtPaymentAmount" runat="server" value="" />
    <input type="hidden" id="hdnTotalInputSelectedDtPaymentAmount" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 55%" />
            <col style="width: 45%" />
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
                                            <%=GetLabel("Customer")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustomerInfo" Style="text-align: left" ReadOnly="true" Width="450px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Tagihan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtARInvoiceNo" Style="text-align: left" ReadOnly="true" Width="200px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Tagihan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceTotal" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Blue" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Bayar")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtARReceivingNo" Style="text-align: left" ReadOnly="true" Width="450px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Bayar")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentAmount" CssClass="txtCurrency" ReadOnly="true" Width="200px" Style="text-align: right; color: Green" runat="server" />
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
                                    <input type="hidden" id="hdnTotalSelectedDtPaymentAmount" runat="server" value="0" />
                                    <input type="hidden" id="hdnTotalSelectedDtClaimAmount" runat="server" value="0" />
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
                                                    <input type="hidden" class="hdnDtPaymentDetailID" id="hdnDtPaymentDetailID" value="<%#: Eval("PaymentDetailID")%>" />
                                                    <input type="hidden" class="hdnDtRegistrationID" id="hdnDtRegistrationID" value="<%#: Eval("RegistrationID")%>" />
                                                    <input type="hidden" class="hdnDtPaymentID" id="hdnDtPaymentID" value="<%#: Eval("PaymentID")%>" />
                                                    <input type="hidden" class="hdnDtPaymentAmount" id="hdnDtPaymentAmount" value='<%#: Eval("TotalOutstandingPayment")%>' />
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
