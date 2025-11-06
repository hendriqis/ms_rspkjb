<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryCopyDirectPurchaseCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.TreasuryCopyDirectPurchaseCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_copydirectpurchasetreasuryctl">

    $('#chkSelectAllDirectPurchase').die('change');
    $('#chkSelectAllDirectPurchase').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectDirectPurchase').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function getCheckedDirectPurchase() {
        var lstSelectedDirectPurchase = $('#<%=hdnSelectedDirectPurchaseID.ClientID %>').val().split(',');
        var lstSelectedRemarks = $('#<%=hdnSelectedRemarks.ClientID %>').val().split(',');
        $('.chkSelectDirectPurchase input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var txtRemarks = $tr.find('.txtRemarks').val();
                var idx = lstSelectedDirectPurchase.indexOf(key);
                if (idx < 0) {
                    lstSelectedDirectPurchase.push(key);
                    lstSelectedRemarks.push(txtRemarks);
                }
                else {
                    lstSelectedRemarks[idx] = txtRemarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedDirectPurchase.indexOf(key);
                if (idx > -1) {
                    lstSelectedDirectPurchase.splice(idx, 1);
                    lstSelectedRemarks.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedDirectPurchaseID.ClientID %>').val(lstSelectedDirectPurchase.join(','));
        $('#<%=hdnSelectedRemarks.ClientID %>').val(lstSelectedRemarks.join(','));
    }

    function onBeforeSaveRecord(errMessage) {
        var accountdt = $('#<%=hdnGLTransactionIDctl.ClientID %>').val();
        if (accountdt != "" && accountdt != "0") {
            getCheckedDirectPurchase();
            if ($('#<%=hdnSelectedDirectPurchaseID.ClientID %>').val() == '') {
                errMessage.text = 'Please Select Direct Purchase First';
                return false;
            }
        } else {
            errMessage.text = 'Please Select Account Detail First';
            return false;
        }
        return true;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        getCheckedDirectPurchase();
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnGLTransactionIDctl" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountTreasuryIDctl" runat="server" value="" />
    <input type="hidden" id="hdnTreasuryTypectl" runat="server" value="" />
    <input type="hidden" id="hdnDisplayOrderTemp" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnBusinessPartnerIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnCashFlowTypeIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnCOADirectPurchaseCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDirectPurchaseID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedRemarks" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 120px" />
            <col />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="2">
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
                                                <th style="width: 30px" align="center">
                                                    <input id="chkSelectAllDirectPurchase" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No. Pembelian")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tanggal Pembelian")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Disetujui Oleh")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Catatan")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="7">
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
                                                    <input id="chkSelectAllDirectPurchase" type="checkbox" />
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No. Pembelian")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Tanggal Pembelian")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Disetujui Oleh")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai")%>
                                                </th>
                                                <th align="center">
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
                                                <asp:CheckBox ID="chkSelectDirectPurchase" runat="server" CssClass="chkSelectDirectPurchase" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("DirectPurchaseID")%>' />
                                                <input type="hidden" class="DirectPurchaseNo" id="DirectPurchaseNo" runat="server"
                                                    value='<%#: Eval("DirectPurchaseNo")%>' />
                                                <input type="hidden" class="cfTotalDirectPurchase" id="cfTotalDirectPurchase" runat="server"
                                                    value='<%#: Eval("cfTotalDirectPurchase")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("DirectPurchaseNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("PurchaseDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("ApprovedByName") %>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfTotalDirectPurchaseInString")%>
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
