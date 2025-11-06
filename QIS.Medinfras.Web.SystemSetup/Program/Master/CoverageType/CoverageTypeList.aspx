<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="CoverageTypeList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.CoverageTypeList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView);


            $('#ulTabGrdDetail li').click(function () {
                $('#ulTabGrdDetail li.selected').removeAttr('class');
                $('.containerGrdDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');

                if ($('#ulTabGrdDetail li').index($(this)) == 1) {
                    if (pageCountGrdDetail2 > -1) {
                        setPaging($("#pagingDetail2"), pageCountGrdDetail2, function (page) {
                            cbpViewDetail2.PerformCallback('changepage|' + page);
                        }, 8);
                        pageCountGrdDetail2 = -1;
                    }
                }
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

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            $('#containerDetail2').hide();
            if ($trDetail.attr('class') != 'trDetail') {
                $('#ulTabGrdDetail li:eq(0)').click();

                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='6'></td></tr>").attr('class', 'trDetail');
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
        
        $('.lnkDepartment a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeDepartmentEntryCtl.ascx");
            openUserControlPopup(url, id, 'Facility', 1000, 500);
        });

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeItemEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item', 1000, 520);
        });

        $('.lnkDepartmentClass a').live('click', function () {
            var coverageTypeID = $('#<%=hdnExpandID.ClientID %>').val();
            var departmentID = $(this).closest('tr').find('.keyField').html();
            var param = coverageTypeID + '|' + departmentID;
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeDepartmentClassEntryCtl.ascx");
            openUserControlPopup(url, param, 'Class', 1000, 500);
        });

        $('.lnkItemClass a').live('click', function () {
            var coverageTypeID = $('#<%=hdnExpandID.ClientID %>').val();
            var itemID = $(this).closest('tr').find('.keyField').html();
            var param = coverageTypeID + '|' + itemID;
            var url = ResolveUrl("~/Program/Master/CoverageType/CoverageTypeItemClassEntryCtl.ascx");
            openUserControlPopup(url, param, 'Class', 1000, 500);
        });


        //#region Paging Detail 2
        function onCbpViewDetail2EndCallback(s) {
            $('#containerImgLoadingViewDetail2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdDetail2 tr:eq(1)').click();

                pageCountGrdDetail2 = pageCount;
            }
            else {
                $('.grdDetail2 tr:eq(1)').click();
            }
        }
        var pageCountGrdDetail2 = -1;
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail'));showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="CoverageTypeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CoverageTypeCode" HeaderText="Coverage Type Code" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="CoverageTypeName" HeaderText="Coverage Type Name" />
                                <asp:HyperLinkField HeaderText="Facility" Text="Facility" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-CssClass="lnkDepartment" />
                                <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-CssClass="lnkItem" />
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
                    <li class="selected" contentid="containerDetail1"><%=GetLabel("Faclity Per Class") %></li>
                    <li contentid="containerDetail2"><%=GetLabel("Item Per Class") %></li>
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
                                                    <th style="width:250px" rowspan="2"><%=GetLabel("Facility")%></th>
                                                    <th colspan="3"><%=GetLabel("Services")%></th>
                                                    <th colspan="3"><%=GetLabel("Drug & Medical Supply")%></th>
                                                    <th colspan="3"><%=GetLabel("Logistic")%></th>  
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Class")%></th>  
                                                </tr>
                                                <tr>  
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>  
                                                 
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>  
                                                    
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>                                                 
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="12">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:250px" rowspan="2"><%=GetLabel("Facility")%></th>
                                                    <th colspan="3"><%=GetLabel("Services")%></th>
                                                    <th colspan="3"><%=GetLabel("Drug & Medical Supply")%></th>
                                                    <th colspan="3"><%=GetLabel("Logistic")%></th>    
                                                    <th style="width:70px" rowspan="2"><%=GetLabel("Class")%></th>
                                                </tr>
                                                <tr>  
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>  
                                                 
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>  
                                                    
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>                                                 
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("DepartmentID")%></td>
                                                <td><%#: Eval("DepartmentName")%></td>                                                

                                                <td align="right"><%#: Eval("DisplayMarkupAmount1")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount1")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount1")%></td>

                                                <td align="right"><%#: Eval("DisplayMarkupAmount2")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount2")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount2")%></td>

                                                <td align="right"><%#: Eval("DisplayMarkupAmount3")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount3")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount3")%></td>  
                                                <td align="center" class="lnkDepartmentClass"><a><%=GetLabel("Class")%></a></td>                                            
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
                            EndCallback="function(s,e){ onCbpViewDetail2EndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:ListView runat="server" ID="lvwDetail2">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail2" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:350px"><%=GetLabel("Item")%></th>
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th>   
                                                    <th style="width:70px"><%=GetLabel("Class")%></th>                                           
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="6">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:350px"><%=GetLabel("Item")%></th>
                                                    <th style="width:120px"><%=GetLabel("Markup")%></th>
                                                    <th style="width:120px"><%=GetLabel("Discount")%></th>
                                                    <th style="width:120px"><%=GetLabel("Coverage")%></th> 
                                                    <th style="width:70px"><%=GetLabel("Class")%></th>                                                 
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("ItemID")%></td>
                                                <td><%#: Eval("ItemName1")%></td>

                                                <td align="right"><%#: Eval("DisplayMarkupAmount")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount")%></td>
                                                <td align="right"><%#: Eval("DisplayCoverageAmount")%></td>
                                                <td align="center" class="lnkItemClass"><a><%=GetLabel("Class")%></a></td>   
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>  
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail2">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div> 
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDetail2"></div>
                        </div>
                    </div> 
                </div>
            </div>
        </div>
    </div>
</asp:Content>