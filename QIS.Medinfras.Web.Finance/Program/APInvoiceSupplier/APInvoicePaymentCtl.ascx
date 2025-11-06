<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoicePaymentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoicePaymentCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_apinvoicepaymentctl">
    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=lvwDetail.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=lvwDetail.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }


</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnPurchaseInvoiceID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("No. Faktur")%></b></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseInvoiceNo" ReadOnly="true" Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("Total Faktur")%></b></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceNetAmount" CssClass="txtCurrency" ReadOnly="true" Width="250px" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:ListView runat="server" ID="lvwDetail">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                </th>
                                                <th style="width: 150px; text-align: left">
                                                    <%=GetLabel("No. Pembayaran")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Tgl. Verifikasi")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Tgl. Pembayaran")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Cara Bayar")%>
                                                </th>
                                                <th style="width: 150px; text-align: center">
                                                    <%=GetLabel("Dibuat Oleh")%>
                                                </th>
                                                <th style="width: 150px; text-align: center">
                                                    <%=GetLabel("Verifikasi Oleh")%>
                                                </th>
                                                <th style="width: 150px; text-align: right">
                                                    <%=GetLabel("Nilai Bayar")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="8">
                                                    <%=GetLabel("Belum ada verifikasi / pembayaran untuk Faktur ini")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                </th>
                                                <th style="width: 150px; text-align: left">
                                                    <%=GetLabel("No. Pembayaran")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Tgl. Verifikasi")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Tgl. Pembayaran")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Cara Bayar")%>
                                                </th>
                                                <th style="width: 150px; text-align: center">
                                                    <%=GetLabel("Dibuat Oleh")%>
                                                </th>
                                                <th style="width: 150px; text-align: center">
                                                    <%=GetLabel("Verifikasi Oleh")%>
                                                </th>
                                                <th style="width: 150px; text-align: right">
                                                    <%=GetLabel("Nilai Bayar")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left">
                                                <%#: Eval("SupplierPaymentNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfVerifiedDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfPaymentDateInString")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("SupplierPaymentMethod")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("CreatedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("VerifiedByName")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("PaymentAmount", "{0:N2}")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
