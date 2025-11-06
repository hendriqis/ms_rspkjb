<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcessPOTermCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcessPOTermCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_APInvoiceSupplierProcessPOTermCtl">

     $(function () {
         $('.txtTaxInvoiceDate').each(function () {
            setDatePickerElement($(this));
        });
    });

    function getCheckedPOTerm() {
        var param = '';
        var tempSelectedDate = "";
        $('.chkPOTerm input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');

                var key = $tr.find('.keyField').val();
                var termAmount = $tr.find('.TermAmount').val();
                var txtTaxInvoiceNoPref = $tr.find('.txtTaxInvoiceNoPref').val();
                var taxInvoiceNo = txtTaxInvoiceNoPref + "^" + $tr.find('.txtTaxInvoiceNo').val();
                var taxInvoiceDate = $(this).closest('tr').find('.txtTaxInvoiceDate').val();
                if (param == '') {
                    param = '$setData|' + key + '|' + termAmount + '|' + taxInvoiceNo + '|' + taxInvoiceDate;
                }
                else {
                    param += '$setData|' + key + '|' + termAmount + '|' + taxInvoiceNo + '|' + taxInvoiceDate;
                }
            }
        });
        $('#<%=hdnDataSave.ClientID %>').val(param);
    }

    $('#chkSelectAllPOTerm').die('change');
    $('#chkSelectAllPOTerm').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkPOTerm').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function onBeforeSaveRecord(errMessage) {
        var result = false;
        getCheckedPOTerm();
        if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
            errMessage.text = 'Please Select PO Term First';
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
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnSelectedPurchaseOrderTermID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedTermAmount" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseInvoiceIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnIsUsedProductLineCtl" value="" runat="server" />
    <input type="hidden" id="hdnProductLineIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnDataSave" value="" runat="server" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
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
                                                    <input id="chkSelectAllPOTerm" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No. Pemesanan")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tgl. Pemesanan")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tgl. Input Termin")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Catatan Termin")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No. Faktur Pajak")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Tgl. Pajak")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("POR Required")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai Termin")%>
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
                                                    <input id="chkSelectAllPOTerm" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No. Pemesanan")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tgl. Pemesanan")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tgl. Input Termin")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Catatan Termin")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No. Faktur Pajak")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Tgl. Pajak")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("POR Required")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai Termin")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkPOTerm" runat="server" CssClass="chkPOTerm" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PurchaseOrderTermID")%>' />
                                                <input type="hidden" class="TermAmount" id="TermAmount" runat="server" value='<%#: Eval("TermAmount")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("PurchaseOrderNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfOrderDateInStringDateFormat")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfCreatedDateInStringDateFormat")%>
                                            </td>
                                            <td>
                                                <%#: Eval("TermRemarks")%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxInvoiceNoPref" CssClass="txtTaxInvoiceNoPref" Width="20%" />
                                                <asp:TextBox runat="server" ID="txtTaxInvoiceNo" CssClass="txtTaxInvoiceNo" Width="68%" />
                                            </td>
                                            <td class="tdCustomDate"> 
                                                <asp:TextBox runat="server" ID="txtTaxInvoiceDate" CssClass="txtTaxInvoiceDate datepicker" Width="80px" />
                                            </td>
                                            <td align="center">
                                                <%#: Eval("IsPurchaseReceiveRequired")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfTermAmountInString")%>
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
