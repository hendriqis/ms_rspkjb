<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PurchaseRequestApprovalList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestApprovalList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseRequestHdItem" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Item")%></div>
    </li>
    <li id="btnPurchaseRequestHdApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnPurchaseRequestHdDecline" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Decline")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnPurchaseRequestHdItem.ClientID %>').click(function () {
                showLoadingPanel();
                var id = $('#<%=hdnID.ClientID %>').val();
                var url = ResolveUrl('~/Program/Procurement/PurchaseRequest/PurchaseRequestApprovalDetailList.aspx?id=' + id);
                document.location = url;
            });

            $('#<%=btnPurchaseRequestHdApprove.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Purchase Request First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var purchaseRequestHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += purchaseRequestHdID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('approve');
                }
            });

            $('#<%=btnPurchaseRequestHdDecline.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Purchase Request First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var purchaseRequestHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += purchaseRequestHdID;
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
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
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
                                <asp:BoundField DataField="PurchaseRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                        <div <%# Eval("IsUrgent").ToString() != "True" ? "Style='display:none'":"" %>>
                                            <img src='<%# ResolveUrl("~/Libs/Images/Button/warning.png") %>' width="17" height="17"
                                                alt="" visible="true" title='<%=GetLabel("Urgent") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal Permintaan"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="PurchaseRequestNo" HeaderText="No. Permintaan" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="LocationCode" HeaderText="Kode Lokasi" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="LocationName" HeaderText="Nama Lokasi" HeaderStyle-Width="250px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PurchaseOrderType" HeaderText="Jenis Permintaan" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfTotalDtAmountInString" HeaderText="Harga Total" HeaderStyle-Width="120px"
                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Approved")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("NumberOfApprovedItems")%>
                                            /
                                            <%#: Eval("NumberOfItems")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Declined")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("NumberOfDeclinedItems")%>
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
