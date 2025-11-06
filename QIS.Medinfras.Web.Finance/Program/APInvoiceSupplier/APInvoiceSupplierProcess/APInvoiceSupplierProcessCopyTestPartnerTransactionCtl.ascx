<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierProcessCopyTestPartnerTransactionCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierProcessCopyTestPartnerTransactionCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_APInvoiceSupplierProcessCopyTestPartnerTransactionCtl">

    $('#containerPopup .txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    function getCheckedTestPartnerTransaction() {
        var lstSelectedTransactionID = $('#<%=hdnSelectedTestPartnerTransactionID.ClientID %>').val().split(',');
        $('.chkTestPartnerTransaction input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var idx = lstSelectedTransactionID.indexOf(key);
                if (idx < 0) {
                    lstSelectedTransactionID.push(key);
                }
                else {
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedTransactionID.indexOf(key);
                if (idx > -1) {
                    lstSelectedTransactionID.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedTestPartnerTransactionID.ClientID %>').val(lstSelectedTransactionID.join(','));
    }

    $('#chkSelectAllTPT').die('change');
    $('#chkSelectAllTPT').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkTestPartnerTransaction').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function onBeforeSaveRecord(errMessage) {
        var result = false;
        getCheckedTestPartnerTransaction();
        if ($('#<%=hdnSelectedTestPartnerTransactionID.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Transaction First';
        }
        else {
            result = true;
        }
        return result;
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnSelectedTestPartnerTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseInvoiceIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnIsUsedProductLineCtl" value="" runat="server" />
    <input type="hidden" id="hdnProductLineIDCtl" value="" runat="server" />
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
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAllTPT" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No Transaksi")%>
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Tgl Transaksi")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Catatan")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nett Partner Amount")%>
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
                                                    <input id="chkSelectAllTPT" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No Transaksi")%>
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Tgl Transaksi")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Catatan")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nett Partner Amount")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkTestPartnerTransaction" runat="server" CssClass="chkTestPartnerTransaction" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("TransactionID")%>' />
                                            </td>
                                            <td>
                                                <b>
                                                    <%#: Eval("TransactionNo")%></b>
                                            </td>
                                            <td>
                                                <%#: Eval("cfTransactionDateInString")%>
                                            </td>
                                            <td>
                                                <%#: Eval("Remarks")%>
                                            </td>
                                            <td>
                                                <%#: Eval("cfNettPartnerTransactionAmountInString")%>
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
