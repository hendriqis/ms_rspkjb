<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoiceOutstandingReceivingInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoiceOutstandingReceivingInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_arinvoiceoutstandingreceivinginformationctl">
    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
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
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

//    $(function () {
//        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
//            if ($(this).attr('class') != 'selected') {
//                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
//                $(this).addClass('selected');
//            }
//        });
//    });

</script>
<input type="hidden" id="hdnBusinessPartnerID" runat="server" value=""/>
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <table style="width: 50%">
                    <colgroup>
                        <col style="width: 15%" />
                    </colgroup>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl" 
                    ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:panel runat="server" id="Panel1" cssclass="pnlContainerGridPatientPage">
                                <asp:gridview id="grdView" runat="server" cssclass="grdSelected grdView" autogeneratecolumns="false"
                                    showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                    <columns>
                                        <asp:BoundField DataField="ARInvoiceID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ARInvoiceNo" HeaderText="No. Invoice" HeaderStyle-Width="120px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ARInvoiceDateInString" HeaderText="Tanggal Invoice" HeaderStyle-Width="90px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalClaimedAmountInString" HeaderText="Total Tagih" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalPaymentAmountInString" HeaderText="Total Transaksi" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-Width="100px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    </columns>
                                    <emptydatatemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </emptydatatemplate>
                                </asp:gridview>
                            </asp:panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
<%--                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>--%>
            </div>
        </td>
    </tr>
</table>