<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcessConsignmentFromOrderCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcessConsignmentFromOrderCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_apinvoicesupplierprocessconsignmentctl">
    $(function () {
        setDatePicker('<%=txtFilterDate.ClientID %>');

        $('#<%=lvwView.ClientID %> tr:gt(0)').each(function () {
            $txtReferenceDate = $(this).find('.txtReferenceDate');
            if ($txtReferenceDate != null) {
                setDatePickerElement($txtReferenceDate);
                $txtReferenceDate.val('<%=DateTimeNowDatePicker() %>');
            }

            $txtTaxInvoiceDate = $(this).find('.txtTaxInvoiceDate');
            if ($txtTaxInvoiceDate != null) {
                setDatePickerElement($txtTaxInvoiceDate);
                $txtTaxInvoiceDate.val('<%=DateTimeNowDatePicker() %>');
            }
        });

    });

    function getCheckedPurchaseReceive() {
        var param = '';
        $('.chkPurchaseReceive input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');

                var poID = $tr.find('.hdnPurchaseOrderID').val();
                var key = $tr.find('.keyField').val();
                var referenceNo = $tr.find('.txtReferenceNo').val();
                var referenceDate = $tr.find('.txtReferenceDate').val();
                var taxInvoiceNoPref = $tr.find('.txtTaxInvoiceNoPref').val();
                var taxInvoiceNo = taxInvoiceNoPref + "^" + $tr.find('.txtTaxInvoiceNo').val();
                var taxInvoiceDate = $tr.find('.txtTaxInvoiceDate').val();

                var amount = $tr.find('.txtNetAmount').val();
                var token = ",";
                var newToken = "";
                amount = amount.split(token).join(newToken);
                var netAmount = parseFloat(amount);

                if (param == '') {
                    param = '$setData|' + poID + '|' + key + '|' + referenceNo + '|' + referenceDate + '|' + taxInvoiceNo + '|' + taxInvoiceDate + '|' + netAmount;
                }
                else {
                    param += '$setData|' + poID + '|' + key + '|' + referenceNo + '|' + referenceDate + '|' + taxInvoiceNo + '|' + taxInvoiceDate + '|' + netAmount;
                }
            }
        });
        $('#<%=hdnDataSave.ClientID %>').val(param);
    }

    $('#chkSelectAllPR').die('change');
    $('#chkSelectAllPR').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkPurchaseReceive').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    $('.txtNetAmount').die('change');
    $('.txtNetAmount').live('change', function () {
        var $tr = $(this).closest('tr');
        var value = $tr.find('.txtNetAmount').val();
        var result = value.indexOf(".");
        if (result == -1) {
            value += '.00';
        }
        $tr.find('.txtNetAmount').val(value);
    });

    $('#<%=chkFilter.ClientID%>').die('change');
    $('#<%=chkFilter.ClientID%>').live('change', function () {
        if ($(this).is(":checked")) {
            $('#<%=hdnIsChecked.ClientID%>').val('1');
        } else {
            $('#<%=hdnIsChecked.ClientID%>').val('0');
        }

        var totalChecked = 0;
        $('.chkPurchaseReceive').each(function () {
            $chk = $(this).find('input');
            if ($chk.is(":checked")) {
                totalChecked += 1;
            }
        });

        if (totalChecked > 0) {
            showToastConfirmation('Perubahan akan menghapus semua data yang sudah terpilih di bawah. Lanjutkan?', function (result) {
                if (result) {
                    var isChecked = $(this).is(":checked");
                    $('.chkPurchaseReceive').each(function () {
                        $chk = $(this).find('input');
                        if (!$chk.is(":disabled")) {
                            $chk.prop('checked', isChecked);
                        }
                    });
                }
            });
        }
    });

    $('#<%=txtFilterDate.ClientID%>').die('change');
    $('#<%=txtFilterDate.ClientID %>').live('change', function () {
        var filterDate = $('#<%=txtFilterDate.ClientID %>').val();
        $('#<%=hdnFilterDate.ClientID %>').val(filterDate);
    });

    $('#<%=txtFilterDay.ClientID%>').die('change');
    $('#<%=txtFilterDay.ClientID %>').live('change', function () {
        var filterDay = $('#<%=txtFilterDay.ClientID %>').val();
        $('#<%=hdnFilterDay.ClientID %>').val(filterDay);
    });

    $('#btnRefresh').live('click', function () {
        cbpProcessDetail.PerformCallback('refresh');
    });

    function onBeforeSaveRecord(errMessage) {
        var result = false;
        getCheckedPurchaseReceive();
        if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Purchase Receive First';
        }
        else {
            result = true;
        }
        return result;
    }

    $('.txtTaxInvoiceNo').die('change');
    $('.txtTaxInvoiceNo').live('change', function () {
        var value = $(this).val();
        var valueSplit = value.split("/");
        if (valueSplit[0] == "http:" || valueSplit[0] == "https:") {
            $(this).val(valueSplit[6]);
        }
    });
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnSelectedPurchaseReceive" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPurchaseOrder" runat="server" value="" />
    <input type="hidden" id="hdnSelectedReferenceNo" runat="server" value="" />
    <input type="hidden" id="hdnSelectedReferenceDate" runat="server" value="" />
    <input type="hidden" id="hdnSelectedTaxInvoiceNo" runat="server" value="" />
    <input type="hidden" id="hdnSelectedTaxInvoiceDate" runat="server" value="" />
    <input type="hidden" id="hdnSelectedNetAmount" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseInvoiceIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <input type="hidden" id="hdnLabResultID" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnDueDate" value="" runat="server" />
    <input type="hidden" id="hdnIsChecked" value="" runat="server" />
    <input type="hidden" id="hdnFilterDate" value="" runat="server" />
    <input type="hidden" id="hdnFilterDay" value="" runat="server" />
    <input type="hidden" id="hdnIsUsedProductLineCtl" value="" runat="server" />
    <input type="hidden" id="hdnProductLineIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnDataSave" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 360px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td colspan="2" style="text-align: left">
                                        <asp:CheckBox ID="chkFilter" Checked="true" runat="server" />&nbsp;<b><%=GetLabel("Filter Tanggal PO :")%></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 30px">
                                        <%=GetLabel("Tanggal PO") %>
                                    </td>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 5px">
                                        <asp:TextBox ID="txtFilterDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 30px">
                                        <%=GetLabel("Selisih Hari Jatuh Tempo") %>
                                    </td>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 5px">
                                        <asp:TextBox ID="txtFilterDay" class="txtFilterDay txtCurrency" Width="30px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <div id="divRefresh" runat="server" style="float: left; margin-top: 0px;">
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </div>
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
                                                    <input id="chkSelectAllPR" type="checkbox" />
                                                </th>
                                                <th>
                                                    <%=GetLabel("No. Pemesanan")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No. Penerimaan")%>
                                                </th>
                                                <th style="width: 100px">
                                                    <%=GetLabel("No. Faktur")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tgl. Faktur")%>
                                                </th>
                                                <th style="width: 250px" align="center">
                                                    <%=GetLabel("No. Faktur Pajak")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tgl. Faktur Pajak")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tgl. Jatuh Tempo")%>
                                                </th>
                                                <th style="width: 100px" align="Right">
                                                    <%=GetLabel("Jumlah Penerimaan")%>
                                                </th>
                                                <th style="width: 100px" align="Right">
                                                    <%=GetLabel("PPN")%>
                                                </th>
                                                <th style="width: 100px" align="Right">
                                                    <%=GetLabel("Sub Total")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Total")%>
                                                </th>
                                                <th style="width: 30px" align="center">
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
                                                    <input id="chkSelectAllPR" type="checkbox" />
                                                </th>
                                                <th>
                                                    <%=GetLabel("No. Pemesanan")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No. Penerimaan")%>
                                                </th>
                                                <th style="width: 100px">
                                                    <%=GetLabel("No. Faktur")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tgl. Faktur")%>
                                                </th>
                                                <th style="width: 250px" align="center">
                                                    <%=GetLabel("No. Faktur Pajak")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tgl. Faktur Pajak")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Jatuh Tempo")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Jumlah Penerimaan")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("PPN")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Sub Total")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Total")%>
                                                </th>
                                                <th style="width: 30px" align="center">
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkPurchaseReceive" runat="server" CssClass="chkPurchaseReceive" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PurchaseReceiveID")%>' />
                                                <input type="hidden" class="hdnPurchaseOrderID" id="hdnPurchaseOrderID" runat="server"
                                                    value='<%#: Eval("PurchaseOrderID")%>' />
                                                <input type="hidden" id="hdnTransactionAmount" runat="server" value='<%#: Eval("TransactionAmount")%>' />
                                                <input type="hidden" id="hdnDiscountAmount" runat="server" value='<%#: Eval("DiscountAmount")%>' />
                                                <input type="hidden" id="hdnFinalDiscountAmount" runat="server" value='<%#: Eval("FinalDiscount")%>' />
                                                <input type="hidden" id="hdnVATAmount" runat="server" value='<%#: Eval("VATAmount")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("PurchaseOrderNo")%>
                                            </td>
                                            <td>
                                                <%#: Eval("PurchaseReceiveNo")%>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox runat="server" ID="txtReferenceNo" CssClass="txtReferenceNo" Width="95%" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox runat="server" ID="txtReferenceDate" CssClass="txtReferenceDate datepicker"
                                                    Width="95%" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox runat="server" ID="txtTaxInvoiceNoPref" CssClass="txtTaxInvoiceNoPref"
                                                    Width="20%" />
                                                <asp:TextBox runat="server" ID="txtTaxInvoiceNo" CssClass="txtTaxInvoiceNo" Width="60%" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox runat="server" ID="txtTaxInvoiceDate" CssClass="txtTaxInvoiceDate datepicker"
                                                    Width="95%" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox runat="server" ID="txtPaymentDueDate" CssClass="txtPaymentDueDate datepicker"
                                                    Width="95%" ReadOnly="true" />
                                            </td>
                                            <td align="right">
                                                <%#: Eval("TransactionAmount", "{0:N}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("VATAmount", "{0:N}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("NetTransactionAmount", "{0:N}")%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNetAmount" CssClass="txtNetAmount number" Width="95%" />
                                            </td>
                                            <td align="center">
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
