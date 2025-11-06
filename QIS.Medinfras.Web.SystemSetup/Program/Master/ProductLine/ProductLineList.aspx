<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ProductLineList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ProductLineList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView2();
            grd.init('grdProductLine', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdProductLine tr:eq(2)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdProductLine tr:eq(2)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:ListView runat="server" ID="lvwView">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdProductLine grdSelected" cellspacing="0" rules="all" >
                                    <tr>  
                                        <th class="keyField" rowspan="2">&nbsp;</th>
                                        <th style="width:120px" rowspan="2"><%=GetLabel("Product Line Code")%></th>
                                        <th rowspan="2"><%=GetLabel("Product Line Name")%></th>                       
                                        <th colspan="8"><%=GetLabel("Account No")%></th>                       
                                    </tr>
                                    <tr>
                                        <th style="width:100px"><%=GetLabel("Inventory")%></th>
                                        <th style="width:100px"><%=GetLabel("COGS")%></th>
                                        <th style="width:100px"><%=GetLabel("Purchase")%></th> 
                                        <th style="width:120px"><%=GetLabel("Purchase Return")%></th>  
                                        <th style="width:120px"><%=GetLabel("Purchase Disount")%></th>  
                                        <th style="width:100px"><%=GetLabel("Sales")%></th>  
                                        <th style="width:100px"><%=GetLabel("Sales Return")%></th>  
                                        <th style="width:100px"><%=GetLabel("Sales Discount")%></th>  
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="10">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdProductLine grdSelected" cellspacing="0" rules="all" >
                                    <tr>  
                                        <th class="keyField" rowspan="2">&nbsp;</th>
                                        <th style="width:120px" rowspan="2"><%=GetLabel("Product Line Code")%></th>
                                        <th rowspan="2"><%=GetLabel("Product Line Name")%></th>                       
                                        <th colspan="8"><%=GetLabel("Account No")%></th>                       
                                    </tr>
                                    <tr>
                                        <th style="width:100px"><%=GetLabel("Inventory")%></th>
                                        <th style="width:100px"><%=GetLabel("COGS")%></th>
                                        <th style="width:100px"><%=GetLabel("Purchase")%></th> 
                                        <th style="width:120px"><%=GetLabel("Purchase Return")%></th>  
                                        <th style="width:120px"><%=GetLabel("Purchase Disount")%></th>  
                                        <th style="width:100px"><%=GetLabel("Sales")%></th>  
                                        <th style="width:100px"><%=GetLabel("Sales Return")%></th>  
                                        <th style="width:100px"><%=GetLabel("Sales Discount")%></th>  
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField"><%#: Eval("ProductLineID")%></td>
                                    <td><%#: Eval("ProductLineCode")%></td>
                                    <td><%#: Eval("ProductLineName")%></td>
                                    <td><%#: Eval("Inventory")%></td>
                                    <td><%#: Eval("COGS")%></td>
                                    <td><%#: Eval("Purchase")%></td>
                                    <td><%#: Eval("PurchaseReturn")%></td>
                                    <td><%#: Eval("PurchaseDiscount")%></td>
                                    <td><%#: Eval("Sales")%></td>
                                    <td><%#: Eval("SalesReturn")%></td>
                                    <td><%#: Eval("SalesDiscount")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>