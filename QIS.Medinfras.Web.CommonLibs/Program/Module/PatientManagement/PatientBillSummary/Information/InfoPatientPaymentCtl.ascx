<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPatientPaymentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientPaymentCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshControl();
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onRefreshControl(filterExpression) {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion
</script>
<div style="overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnParam" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td class="tdLabel" style="width:150px">
                <label class="lblNormal">
                    <%=GetLabel("No. Pembayaran")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPaymentNo" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="PaymentDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PaymentMethod"  HeaderText="Metode Pembayaran" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BusinessPartnerName"  HeaderText="Instansi" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="EDCMachineName" HeaderText="EDC" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="CardNumber" HeaderText="Informasi Kartu" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BankName" HeaderText="Bank" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfPaymentAmount" HeaderText="Pembayaran" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
    </table>
</div>
