<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ItemProductList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ItemProductList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#ulTabGrdDetail li').click(function () {
                $('#ulTabGrdDetail li.selected').removeAttr('class');
                $('.containerGrdDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
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
            $('#<%=hdnID.ClientID %>').val('');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            $('#containerDetail2').hide();
            if ($trDetail.attr('class') != 'trDetail') {
                $('#ulTabGrdDetail li:eq(0)').click();

                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='4'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('.grdDetail1 tr:gt(0)').remove();
                $('.grdDetail2 tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail2.PerformCallback('refresh');
                cbpViewDetail1.PerformCallback('refresh');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $('.grdDetail1 tr:gt(0)').remove();
                $('.grdDetail2 tr:gt(0)').remove();

                $trDetail.remove();
            }
        });

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            if($('#containerDetail2').is(":visible"))
                cbpViewDetail2.PerformCallback();
            else
                cbpViewDetail1.PerformCallback();
        }

        $('.lnkEditItemCost').live('click', function () {
            var itemCostID = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ItemService/ItemServiceItemCostEntryCtl.ascx");
            openUserControlPopup(url, itemCostID, 'Item Cost', 600, 400);
        });

        $('.lnkEditItemPlanning').live('click', function () {
            var itemPlanningID = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ItemProduct/ItemProductPlanningEntryCtl.ascx");
            openUserControlPopup(url, itemPlanningID, 'Item Planning', 600, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemCode" HeaderText="Item Code" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="ItemName1" HeaderText="Item Name 1" />
                                <asp:BoundField DataField="ItemName2" HeaderText="Item Name 2" />
                                <asp:BoundField DataField="ItemUnit" HeaderText="Item Unit" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
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

    <div id="tempContainerGrdDetail" style="display:none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%;padding: 10px 5px;">
            <div class="containerUlTabPage">
                <ul class="ulTabPage" id="ulTabGrdDetail">
                    <li class="selected" contentid="containerDetail1"><%=GetLabel("Item Planning") %></li>
                    <li contentid="containerDetail2"><%=GetLabel("Item Cost") %></li>
                </ul>
            </div>
            <div style="position: relative;">
                <div id="containerDetail1" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail1" runat="server" Width="100%" ClientInstanceName="cbpViewDetail1"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail1_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail1').show(); }"
                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail1').hide(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:ListView runat="server" ID="lvwDetail1">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail1" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th class="keyField" rowspan="2"></th>
                                                    <th style="width:50px" rowspan="2">&nbsp;</th>
                                                    <th style="width:150px" rowspan="2"><%=GetLabel("Healthcare")%></th>
                                                    <th rowspan="2"><%=GetLabel("Default Supplier")%></th>
                                                    <th style="width:120px" rowspan="2"><%=GetLabel("Base Price")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Lead Time")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Safety Time")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Safety Stock")%></th>   
                                                    <th colspan="2"><%=GetLabel("Order Qty")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Tolerance")%> (%)</th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Time Fence")%></th>                                            
                                                </tr>
                                                <tr>
                                                    <th style="width:70px"><%=GetLabel("Min")%> (%)</th>
                                                    <th style="width:70px"><%=GetLabel("Max")%></th>   
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th class="keyField" rowspan="2"></th>
                                                    <th style="width:50px" rowspan="2">&nbsp;</th>
                                                    <th style="width:150px" rowspan="2"><%=GetLabel("Healthcare")%></th>
                                                    <th rowspan="2"><%=GetLabel("Default Supplier")%></th>
                                                    <th style="width:120px" rowspan="2"><%=GetLabel("Base Price")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Lead Time")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Safety Time")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Safety Stock")%></th>   
                                                    <th colspan="2"><%=GetLabel("Order Qty")%></th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Tolerance")%> (%)</th>
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Time Fence")%></th>                                            
                                                </tr>
                                                <tr>
                                                    <th style="width:70px"><%=GetLabel("Min")%> (%)</th>
                                                    <th style="width:70px"><%=GetLabel("Max")%></th>   
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("ID")%></td>
                                                <td align="center"><img class="lnkEditItemPlanning imgLink" title="<%=GetLabel("Edit") %>" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt=""  /></td>
                                                <td><%#: Eval("HealthcareName")%></td>
                                                <td><%#: Eval("BusinessPartnerName")%></td>
                                                <td align="right"><%#: Eval("HNAAmount")%></td>
                                                <td align="right"><%#: Eval("LeadTime")%></td>
                                                <td align="right"><%#: Eval("SafetyTime")%></td>
                                                <td align="right"><%#: Eval("SafetyStock")%></td>
                                                <td align="right"><%#: Eval("MinOrderQty")%></td>
                                                <td align="right"><%#: Eval("MaxOrderQty")%></td>
                                                <td align="right"><%#: Eval("TolerancePercentage")%></td>
                                                <td align="right"><%#: Eval("TimeFence")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
                <div id="containerDetail2" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail2" runat="server" Width="100%" ClientInstanceName="cbpViewDetail2"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail2_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail2').show(); }"
                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail2').hide(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:GridView ID="grdDetail2" runat="server" CssClass="grdView notAllowSelect grdDetail2" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ItemCostID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <HeaderTemplate></HeaderTemplate>
                                                <ItemTemplate>
                                                    <img class="lnkEditItemCost imgLink" title="<%=GetLabel("Edit") %>" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt=""  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="HealthcareName" HeaderText="Healthcare" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfTotalMaterial" HeaderText="Material" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"/>
                                            <asp:BoundField DataField="cfTotalLabor" HeaderText="Labor" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"/>
                                            <asp:BoundField DataField="cfTotalOverhead" HeaderText="Overhead" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"/>
                                            <asp:BoundField DataField="cfTotalSubContract" HeaderText="Sub Contract" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"/>
                                            <asp:BoundField DataField="cfTotalBurden" HeaderText="Burden" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"/>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>                                  
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>  
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail2">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div> 
            </div>
        </div>
    </div>
</asp:Content>