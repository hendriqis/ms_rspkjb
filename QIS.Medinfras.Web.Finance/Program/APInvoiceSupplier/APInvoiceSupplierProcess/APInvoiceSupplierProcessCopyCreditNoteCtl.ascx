<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcessCopyCreditNoteCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcessCopyCreditNoteCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_apcreditnoteprocessctl">

    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    $('.txtCNAmountBeforePPN').live('change', function () {
        var $tr = $(this).closest('tr');
        var txtCNAmountBeforePPN = $tr.find('.txtCNAmountBeforePPN').val();
        var IsIncludeVAT = $tr.find('.IsIncludeVAT').val();
        var VATPercentage = parseFloat($tr.find('.VATPercentage').val());
        var total = 0;

        if (IsIncludeVAT == "True") {
            total = txtCNAmountBeforePPN * (100 + VATPercentage) / 100;
        } else {
            total = txtCNAmountBeforePPN;
        }

        $tr.find('.txtCNAmountAfterPPN').val(total);
        $('.txtCNAmountAfterPPN').trigger('changeValue');
    });

    function getCheckedCreditNote() {
        var param = '';
        $('.chkCreditNote input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtCNAmountBeforePPN = $tr.find('.txtCNAmountBeforePPN').attr('hiddenVal');
                var txtRemarks = $tr.find('.txtRemarks').val();

                if (param == '') {
                    param = '$setData|' + key + '|' + txtCNAmountBeforePPN + '|' + txtRemarks;
                }
                else {
                    param += '$setData|' + key + '|' + txtCNAmountBeforePPN + '|' + txtRemarks;
                }
            }
        });
        $('#<%=hdnDataSave.ClientID %>').val(param);
    }

    $('#chkSelectAllCN').die('change');
    $('#chkSelectAllCN').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkCreditNote').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function onBeforeSaveRecord(errMessage) {
        var result = false;
        getCheckedCreditNote();
        if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Credit Note First';
        }
        else {
            result = true;
        }
        return result;
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnSelectedCreditNote" runat="server" value="" />
    <input type="hidden" id="hdnSelectedCNAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRemarks" runat="server" value="" />
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
                                                    <input id="chkSelectAllCN" type="checkbox" />
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Nota Kredit")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Retur")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Penerimaan")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Faktur")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Faktur Kirim")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tipe Nota Kredit")%>
                                                </th>
                                                <th style="width: 40px" align="center">
                                                    <%=GetLabel("PPN")%>
                                                </th>
                                                <th style="width: 130px" align="right">
                                                    <%=GetLabel("Nilai Nota Kredit (Sebelum PPN)")%>
                                                </th>
                                                <th style="width: 130px" align="right">
                                                    <%=GetLabel("Nilai Nota Kredit (Setelah PPN)")%>
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Catatan")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="11">
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
                                                    <input id="chkSelectAllCN" type="checkbox" />
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Nota Kredit")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Retur")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Penerimaan")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Faktur")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("No/Tgl Faktur Kirim")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tipe Nota Kredit")%>
                                                </th>
                                                <th style="width: 40px" align="center">
                                                    <%=GetLabel("PPN")%>
                                                </th>
                                                <th style="width: 130px" align="right">
                                                    <%=GetLabel("Nilai Nota Kredit (Sebelum PPN)")%>
                                                </th>
                                                <th style="width: 130px" align="right">
                                                    <%=GetLabel("Nilai Nota Kredit (Setelah PPN)")%>
                                                </th>
                                                <th style="width: 120px">
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
                                                <asp:CheckBox ID="chkCreditNote" runat="server" CssClass="chkCreditNote" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("CreditNoteID")%>' />
                                                <input type="hidden" class="IsIncludeVAT" id="IsIncludeVAT" runat="server" value='<%#: Eval("IsIncludeVAT")%>' />
                                                <input type="hidden" class="VATPercentage" id="VATPercentage" runat="server" value='<%#: Eval("VATPercentage")%>' />
                                            </td>
                                            <td>
                                                <b>
                                                    <%#: Eval("CreditNoteNo")%></b>
                                                <br />
                                                <%#: Eval("CreditNoteDateInString")%>
                                            </td>
                                            <td>
                                                <b>
                                                    <%#: Eval("PurchaseReturnNo")%></b>
                                                <br />
                                                <%#: Eval("ReturnDateInString")%>
                                            </td>
                                            <td>
                                                <b>
                                                    <%#: Eval("PurchaseReceiveNo")%></b>
                                                <br />
                                                <%#: Eval("ReceiveDateInString")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ReferenceNo")%>
                                                <br />
                                                <%#: Eval("ReferenceDateInString")%>
                                            </td>
                                            <td>
                                                <%#: Eval("TaxInvoiceNo")%>
                                                <br />
                                                <%#: Eval("TaxInvoiceDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("CreditNoteType")%>
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkPPN" runat="server" Enabled="false" CssClass="chkPPN" />
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="txtCNAmountBeforePPN" CssClass="txtCNAmountBeforePPN txtCurrency"
                                                    Width="90%" />
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="txtCNAmountAfterPPN" CssClass="txtCNAmountAfterPPN txtCurrency"
                                                    Width="90%" ReadOnly="true" />
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
