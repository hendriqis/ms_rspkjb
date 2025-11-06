<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ConsignmentOrderApprovalList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentOrderApprovalList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseOrderHdItem" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Item")%></div>
    </li>
    <li id="btnPurchaseOrderHdApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnPurchaseOrderHdDecline" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Decline")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnPurchaseOrderHdItem.ClientID %>').click(function () {
                showLoadingPanel();
                var id = $('#<%=hdnID.ClientID %>').val();
                var url = ResolveUrl('~/Program/Consignment/ConsignmentOrder/ConsignmentOrderApprovalDetailList.aspx?id=' + id);
                document.location = url;
            });

            $('#<%=btnPurchaseOrderHdApprove.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Purchase Request First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var purchaseOrderHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += purchaseOrderHdID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('approve');
                }
            });

            $('#<%=btnPurchaseOrderHdDecline.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Purchase Request First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var purchaseOrderHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += purchaseOrderHdID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('decline');
                }
            });
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="PurchaseOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PurchaseOrderNo" HeaderText="No Pemesanan" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="BusinessPartnerName" HeaderText="Nama Supplier" HeaderStyle-Width="300px" />
                                <asp:BoundField DataField="TransactionAmount" HeaderText="Jumlah Transaksi" ItemStyle-HorizontalAlign="Right"
                                    DataFormatString="{0:N}" />
                                <asp:BoundField DataField="OrderDateInString" HeaderText="Tanggal Pemesanan" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="Remarks" HeaderText="Keterangan" />
                                <asp:TemplateField HeaderStyle-Width="140px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Number Of Items")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("NumberOfApprovedItems")%>
                                            /
                                            <%#: Eval("NumberOfItems")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
