<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APSupplierPaymentVerificationDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.APSupplierPaymentVerificationDtCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_APSupplierPaymentVerificationDtCtl">
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
<input type="hidden" id="hdnSupplierPaymentIDDtCtl" runat="server" />

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
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Pembayaran")%></label></td>
                    <td><asp:TextBox ID="txtSupplierPaymentNoDtCtl" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Materai")%></label></td>
                    <td><asp:TextBox ID="txtStampAmountDtCtl" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total Bayar")%></label></td>
                    <td><asp:TextBox ID="txtPaymentAmountDtCtl" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:400px; overflow-y: scroll;">
                                <table class="grdSupplierPayment grdSelected" cellspacing="0" width="100%" rules="all">
                                    <colgroup>
                                        <col style="width:150px" />
                                        <col style="width:150px" />
                                        <col  />
                                    </colgroup>
                                    <tr>
                                        <th align="left"><%=GetLabel("Nama Supplier") %></th>
                                        <th align="left"><%=GetLabel("No Tukar Faktur") %></th>
                                        <th align="center" style="width:150px"><%=GetLabel("Tanggal Tukar Faktur") %></th>
                                        <th align="center" style="width:150px"><%=GetLabel("Tanggal Jatuh Tempo") %></th>
                                        <th align="right" style="width:170px"><%=GetLabel("Total Hutang") %></th>
                                        <th align="right" style="width:170px"><%=GetLabel("Jumlah Pembayaran") %></th>
                                    </tr>
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <tr class="trEmpty">
                                                <td colspan="14"><%=GetLabel("Data Tidak Tersedia") %></td>
                                            </tr>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="left"><%#:Eval("BusinessPartnerName") %></td>
                                                <td align="left"><%#:Eval("PurchaseInvoiceNo") %></td>
                                                <td align="center"><%#:Eval("PurchaseInvoiceDateInString")%></td>
                                                <td align="center"><%#:Eval("DueDateInString")%></td>
                                                <td align="right"><%#:Eval("TotalNetTransactionAmount","{0:N}")%></td>
                                                <td align="right"><%#:Eval("PaymentAmount","{0:N}")%></td>
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