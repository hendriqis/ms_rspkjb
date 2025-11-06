<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcessEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcessEditCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_APInvoiceSupplierProcessEditCtl">
    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    //#region Inside Grid
    $('.txtUnitPrice').change(function () {
        $(this).trigger('changeValue');
        $tr = $(this).closest('tr').parent().closest('tr');
        calculateSubTotal($tr);
    });

    $('.txtDiscountPercentage1').change(function () {
        $(this).trigger('changeValue');
        $tr = $(this).closest('tr');
        var qty = parseFloat($tr.find('.hdnQuantity').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));

        var subTotal = qty * unitPrice;

        var discountPercentage1 = parseFloat($tr.find('.txtDiscountPercentage1').attr('hiddenVal'));
        var discountAmount1 = subTotal * discountPercentage1 / 100;
        $tr.find('.txtDiscountAmount1').val(discountAmount1).trigger('changeValue');

        calculateSubTotal($tr);
    });

    $('.txtDiscountAmount1').change(function () {
        $(this).trigger('changeValue');
        $tr = $(this).closest('tr');
        var qty = parseFloat($tr.find('.hdnQuantity').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));

        var subTotal = qty * unitPrice;

        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').attr('hiddenVal'));
        var discountPercentage1 = discountAmount1 * 100 / subTotal;
        $tr.find('.txtDiscountPercentage1').val(discountPercentage1).trigger('changeValue');

        calculateSubTotal($tr);
    });

    $('.txtDiscountPercentage2').change(function () {
        $(this).trigger('changeValue');
        $tr = $(this).closest('tr');
        var qty = parseFloat($tr.find('.hdnQuantity').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));
        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').attr('hiddenVal'));

        var subTotal = (qty * unitPrice) - discountAmount1;

        var discountPercentage2 = parseFloat($tr.find('.txtDiscountPercentage2').attr('hiddenVal'));
        var discountAmount2 = subTotal * discountPercentage2 / 100;
        $tr.find('.txtDiscountAmount2').val(discountAmount2).trigger('changeValue');

        calculateSubTotal($tr);
    });

    $('.txtDiscountAmount2').change(function () {
        $(this).trigger('changeValue');
        $tr = $(this).closest('tr');
        var qty = parseFloat($tr.find('.hdnQuantity').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));
        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').attr('hiddenVal'));

        var subTotal = (qty * unitPrice) - discountAmount1;

        var discountAmount2 = parseFloat($tr.find('.txtDiscountAmount2').attr('hiddenVal'));
        var discountPercentage2 = discountAmount2 * 100 / subTotal;
        $tr.find('.txtDiscountPercentage2').val(discountPercentage2).trigger('changeValue');

        calculateSubTotal($tr);
    });

    function calculateSubTotal($tr) {
        var qty = parseFloat($tr.find('.hdnQuantity').val());
        var unitPrice = parseFloat($tr.find('.txtUnitPrice').attr('hiddenVal'));
        var discountAmount1 = parseFloat($tr.find('.txtDiscountAmount1').attr('hiddenVal'));
        var discountAmount2 = parseFloat($tr.find('.txtDiscountAmount2').attr('hiddenVal'));

        var subTotal = (qty * unitPrice) - discountAmount1 - discountAmount2;
        $tr.find('.txtLineAmount').val(subTotal).trigger('changeValue');

        var total = 0;
        $('.txtLineAmount').each(function () {
            total += parseFloat($(this).attr('hiddenVal'));
        });
        $('#<%=txtTotalOrder.ClientID %>').val(total).trigger('changeValue');
        calculateDiscount();
        calculateTotal();
    }
    //#endregion

    $('#<%=chkPPN.ClientID %>').change(function () {
        calculateTotal();
    });

    $('#<%=txtDP.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        calculateTotal();
    });

    $('#<%=txtCharges.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        calculateTotal();
    });

    $('#<%=txtStamp.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        calculateTotal();
    });

    $('#<%=txtFinalDiscountInPercentage.ClientID %>').change(function () {
        calculateDiscount();
        calculateTotal();
    });

    $('#<%=txtRoundingInvoiceDtAmount.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        calculateTotal();
    });

    function calculateDiscount() {
        $('#<%=txtFinalDiscountInPercentage.ClientID %>').trigger('changeValue');
        var finalDiscount = (parseFloat($('#<%=txtFinalDiscountInPercentage.ClientID %>').attr('hiddenVal')) / 100) * parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
        $('#<%=txtFinalDiscount.ClientID %>').val(finalDiscount).trigger('changeValue');
    }

    $('#<%=txtFinalDiscount.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        var finalDiscountInPercentage = (parseFloat($(this).attr('hiddenVal')) / parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'))) * 100;
        $('#<%=txtFinalDiscountInPercentage.ClientID %>').val(finalDiscountInPercentage).trigger('changeValue');
        calculateTotal();
    });

    $('#<%=txtFinalDiscount.ClientID %>').change();

    function calculateTotal() {
        var totalKotor = parseFloat($('#<%=txtTotalOrder.ClientID %>').attr('hiddenVal'));
        var Discount = parseFloat($('#<%=txtFinalDiscount.ClientID %>').attr('hiddenVal'));

        if ($('#<%=chkPPN.ClientID %>').is(':checked')) {
            var temp = totalKotor - Discount;
            var PPN = parseFloat('<%=GetVATPercentage() %>') / 100 * parseFloat(temp);
            $('#<%=txtPPN.ClientID %>').val(PPN).trigger('changeValue');
        }
        else {
            $('#<%=txtPPN.ClientID %>').val('0').trigger('changeValue');
        }
        var PPN = parseFloat($('#<%=txtPPN.ClientID %>').attr('hiddenVal'));
        var DP = parseFloat($('#<%=txtDP.ClientID %>').attr('hiddenVal'));
        var Charge = parseFloat($('#<%=txtCharges.ClientID %>').attr('hiddenVal'));
        var Stamp = parseFloat($('#<%=txtStamp.ClientID %>').attr('hiddenVal'));
        var Pph = parseFloat($('#<%=txtPPH.ClientID %>').attr('hiddenVal'));
        var totalHarga = totalKotor - (Discount + DP) + PPN + Charge + Stamp + Pph;
        $('#<%=txtTotalOrderSaldo.ClientID %>').val(totalHarga).trigger('changeValue');

        var roundingDt = parseFloat($('#<%=txtRoundingInvoiceDtAmount.ClientID %>').val());
        var grandTotal = parseFloat(totalHarga + roundingDt);
        $('#<%=txtGrandTotalOrderSaldo.ClientID %>').val(grandTotal).trigger('changeValue');
    }

    function onBeforeSaveRecord(errMessage) {
        var lstID = '';
        var lstUnitPrice = '';
        var lstDiscountPercentage1 = '';
        var lstDiscountPercentage2 = '';
        $('.grdPurchaseReceive > tbody > tr:gt(1)').each(function () {
            var id = parseFloat($(this).find('.hdnID').val());
            var unitPrice = parseFloat($(this).find('.txtUnitPrice').attr('hiddenVal'));
            var discountPercentage1 = parseFloat($(this).find('.txtDiscountPercentage1').attr('hiddenVal'));
            var discountPercentage2 = parseFloat($(this).find('.txtDiscountPercentage2').attr('hiddenVal'));
            if (lstID != '') {
                lstID += ",";
                lstUnitPrice += ",";
                lstDiscountPercentage1 += ",";
                lstDiscountPercentage2 += ",";
            }

            lstID += id;
            lstUnitPrice += unitPrice;
            lstDiscountPercentage1 += discountPercentage1;
            lstDiscountPercentage2 += discountPercentage2;
        });

        $('#<%=hdnLstID.ClientID %>').val(lstID);
        $('#<%=hdnLstUnitPrice.ClientID %>').val(lstUnitPrice);
        $('#<%=hdnLstDiscountPercentage1.ClientID %>').val(lstDiscountPercentage1);
        $('#<%=hdnLstDiscountPercentage2.ClientID %>').val(lstDiscountPercentage2);
        return true;
    }
</script>
<input type="hidden" id="hdnIsAllowChangePriceInAP" runat="server" value="1" />
<input type="hidden" id="hdnIsChangedAPAmountFromRoundingInvoice" runat="server"
    value="1" />
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />
<input type="hidden" id="hdnPurchaseReceiveID" runat="server" />
<input type="hidden" id="hdnPurchaseInvoiceID" runat="server" />
<input type="hidden" id="hdnVATPercentage" runat="server" />
<input type="hidden" id="hdnLstUnitPrice" runat="server" />
<input type="hidden" id="hdnLstDiscountPercentage1" runat="server" />
<input type="hidden" id="hdnLstDiscountPercentage2" runat="server" />
<input type="hidden" id="hdnLstID" runat="server" />
<div style="max-height: 440px; overflow-y: auto" id="containerPopup">
    <table style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No BPB")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseReceiveNo" ReadOnly="true" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal") %>
                            -
                            <%=GetLabel("Waktu Penerimaan") %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtPurchaseReceiveDate" ReadOnly="true" Width="120px" CssClass="datepicker"
                                            runat="server" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPurchaseReceiveTime" Width="60px" ReadOnly="true" CssClass="time"
                                            runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("No.Faktur/Kirim")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNo" CssClass="required" ValidationGroup="mpEntry" Width="100px"
                                ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal di Faktur") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateReferrence" Width="120px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Waktu Pembayaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="300px" runat="server"
                                Enabled="false" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Mata Uang")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox Visible="false" ID="cboCurrency" ClientInstanceName="cboCurrency"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <%=GetLabel("Nilai Kurs (Rp)") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKurs" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPPN" Width="100%" runat="server" Enabled="false" />&nbsp;<%=GetLabel("PPN")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                        <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th rowspan="2" style="width: 30px" align="center">
                                                        <%=GetLabel("Bonus")%>
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("Item")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 140px">
                                                        <%=GetLabel("Harga Satuan")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 80px">
                                                        <%=GetLabel("Diterima")%>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("DISKON 1")%>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("DISKON 2")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 100px">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 60px" align="center">
                                                        <%=GetLabel("[%]")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                    <th style="width: 60px" align="center">
                                                        <%=GetLabel("[%]")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="16">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPurchaseReceive grdView notAllowSelect"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th rowspan="2" style="width: 30px" align="center">
                                                        <%=GetLabel("Bonus")%>
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("Item")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 130px">
                                                        <%=GetLabel("Harga Satuan")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 80px">
                                                        <%=GetLabel("Diterima")%>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("DISKON 1")%>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("DISKON 2")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 95px">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" align="center" style="width: 70px">
                                                        <%=GetLabel("Penerima")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 60px" align="center">
                                                        <%=GetLabel("[%]")%>
                                                    </th>
                                                    <th style="width: 95px" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                    <th style="width: 60px" align="center">
                                                        <%=GetLabel("[%]")%>
                                                    </th>
                                                    <th style="width: 95px" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsBonus" Enabled="false" runat="server" Checked='false' />
                                                </td>
                                                <td>
                                                    <div style="padding: 1px">
                                                        <div>
                                                            <span style="font-style: normal; font-weight: bold">
                                                                <%#: Eval("ItemName1")%></div>
                                                        <div>
                                                            <span style="font-style: italic;">
                                                                <%#: Eval("ItemCode")%></span></div>
                                                        <div>
                                                            <span style="font-style: normal;">
                                                                <%#: Eval("PurchaseOrderNo")%></span></div>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 50px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtUnitPrice" runat="server" Width="100%" CssClass="txtCurrency txtUnitPrice"
                                                                    ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp/&nbsp<%#: Eval("ItemUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <input type="hidden" class="hdnID" value='<%#: Eval("ID")%>' />
                                                    <input type="hidden" class="hdnQuantity" value='<%#: Eval("Quantity")%>' />
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 40px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("Quantity")%>
                                                            </td>
                                                            <td>
                                                                &nbsp<%#: Eval("ItemUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtDiscountPercentage1" runat="server" Width="100%" CssClass="txtCurrency txtDiscountPercentage1"
                                                        ReadOnly="true" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtDiscountAmount1" runat="server" Width="100%" CssClass="txtCurrency txtDiscountAmount1"
                                                        ReadOnly="true" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtDiscountPercentage2" runat="server" Width="100%" CssClass="txtCurrency txtDiscountPercentage2"
                                                        ReadOnly="true" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtDiscountAmount2" runat="server" Width="100%" CssClass="txtCurrency txtDiscountAmount2"
                                                        ReadOnly="true" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtLineAmount" ReadOnly="true" runat="server" Width="100%" CssClass="txtCurrency txtLineAmount" />
                                                </td>
                                                <td>
                                                    <%#: Eval("CreatedByName")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
                <table style="width: 100%;">
                    <colgroup>
                        <col style="width: 60%" />
                        <col style="width: 40px" />
                    </colgroup>
                    <tr>
                        <td valign="top">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col style="width: 100px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Catatan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNotes" Width="300px" runat="server" TextMode="MultiLine" Rows="5" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td valign="top">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col style="width: 180px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jumlah Nilai Pembelian")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalOrder" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr id='trDiskonFinal' runat='server'>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Diskon Final %")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="txtCurrency" Width="180px"
                                            ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                                <tr id='trDiskonFinal1' runat='server'>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Diskon Final")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFinalDiscount" CssClass="txtCurrency" Width="180px" runat="server"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("PPN (" + GetVATPercentage() + "%)")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPPN" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server" />
                                    </td>
                                </tr>
                                <tr id='trPph' runat='server'>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("PPh")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPPH" CssClass="txtCurrency" ReadOnly="true" Width="180px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Reff Uang Muka")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDPReferrenceNo" Width="180px" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Uang Muka")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDP" CssClass="txtCurrency" Width="180px" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr id='trJenisPembiayaan' runat='server'>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jenis Pembiayaan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboChargesType" ClientEnabled="false" ClientInstanceName="cboChargesType"
                                            Width="180px" runat="server" />
                                    </td>
                                </tr>
                                <tr id='trOngkosKirim' runat='server'>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Biaya")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCharges" CssClass="txtCurrency" Width="180px" runat="server"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr id='trBiayaMaterai' runat='server'>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Biaya Materai")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStamp" CssClass="txtCurrency" Width="180px" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Total Nilai Penerimaan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalOrderSaldo" CssClass="txtCurrency" ReadOnly="true" Width="180px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Pembulatan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRoundingInvoiceDtAmount" CssClass="txtCurrency" Width="180px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Total Tukar Faktur")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrandTotalOrderSaldo" CssClass="txtCurrency" ReadOnly="true"
                                            Width="180px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
