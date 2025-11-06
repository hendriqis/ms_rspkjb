<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APInvoiceSupplierVerificationDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierVerificationDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });
    
    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />
<input type="hidden" id="hdnPurchaseInvoiceID" runat="server" />

<div class="pageTitle"><%=GetLabel("Detail Information")%></div>
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:70%">
                <colgroup>
                    <col style="width:160px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Purchase Invoice No")%></label></td>
                    <td><asp:TextBox ID="txtPurchaseInvoiceNo" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
            </table>

            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:400px; overflow-y: scroll;">
                                <table class="grdPurchaseInvoice grdSelected" cellspacing="0" width="100%" rules="all">
                                    <colgroup>
                                        <col style="width:150px" />
                                        <col style="width:150px" />
                                        <col  />
                                    </colgroup>
                                    <tr>
                                        <th align="left"><%=GetLabel("No Penerimaan") %></th>
                                        <th align="left"><%=GetLabel("No Faktur") %></th>
                                        <th align="right"><%=GetLabel("Jumlah") %></th>
                                        <th align="right"><%=GetLabel("Diskon Transaksi") %></th>
                                        <th align="right"><%=GetLabel("Diskon Final") %></th>
                                        <th align="right"><%=GetLabel("PPN") %></th>
                                        <th align="right"><%=GetLabel("Materai") %></th>
                                        <th align="right"><%=GetLabel("Ongkos Kirim") %></th>
                                        <th align="right"><%=GetLabel("Uang Muka") %></th>
                                        <th align="right"><%=GetLabel("Nota Kredit") %></th>
                                        <th align="right" style="width:100px"><%=GetLabel("Total") %></th>
                                    </tr>
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <tr class="trEmpty">
                                                <td colspan="14"><%=GetLabel("Data Tidak Tersedia") %></td>
                                            </tr>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="left"><%#:Eval("PurchaseReceiveNo") %></td>
                                                <td align="left"><%#:Eval("ReferenceNo")%></td>
                                                <td align="right"><%#:Eval("TransactionAmount","{0:N}")%></td>
                                                <td align="right"><%#:Eval("DiscountAmount","{0:N}")%></td>
                                                <td align="right"><%#:Eval("FinalDiscountAmount","{0:N}")%></td>
                                                <td align="right"><%#:Eval("VATAmount","{0:N}") %></td>
                                                <td align="right"><%#:Eval("StampAmount", "{0:N}")%></td>
                                                <td align="right"><%#:Eval("ChargesAmount", "{0:N}")%></td>
                                                <td align="right"><%#:Eval("DownPaymentAmount", "{0:N}")%></td>
                                                <td align="right"><%#:Eval("CreditNoteAmount", "{0:N}")%></td>
                                                <td align="right"><%#:Eval("LineAmount", "{0:N}")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>