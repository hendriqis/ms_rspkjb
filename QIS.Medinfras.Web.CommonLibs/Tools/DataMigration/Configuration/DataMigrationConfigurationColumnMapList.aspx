<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="DataMigrationConfigurationColumnMapList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Tools.DataMigrationConfigurationColumnMapList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnDataMigrationBack" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("View")%></div></li> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        function customGridView() {
            this.gridID = '';
            this.hdnID = '';
            this.divID = '';
            this.cbp = null;
            this.pageID = '';
            var _self = this;
            this.init = function (gridID, hdnID, divID, cbp, pageID) {
                _self.gridID = gridID;
                _self.hdnID = hdnID;
                _self.divID = divID;
                _self.pageID = pageID;
                _self.cbp = cbp;

                $('.' + _self.gridID + ' tr:gt(0):not(.trEmpty)').live('click', function () {
                    $('.' + _self.gridID + ' tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#' + _self.hdnID).val($(this).find('.keyField').html());
                });

                $('.' + _self.gridID + ' tr:gt(0):not(.trEmpty)').live('dblclick', function () {
                    cbpMPListProcess.PerformCallback('edit');
                });
                $('.' + _self.gridID + ' tr:eq(2)').click();

                if ($.browser.mozilla) {
                    $(document).keypress(_self.bodyKeyPress);
                } else {
                    $(document).keydown(_self.bodyKeyPress);
                }
            }
            this.changeCursorUp = function () {
                $tr = $('#' + _self.gridID + ' tr.selected');
                if ($('#' + _self.gridID + ' tr').index($tr) > 1) {
                    $tr.removeClass('selected');
                    $prevTr = $tr.prev();
                    $prevTr.addClass('selected');
                    $('#' + _self.hdnID).val($prevTr.find('.keyField').html());

                    var position = $prevTr.position();
                    var objDiv = $("#" + _self.divID)[0];
                    var newScrollTop = objDiv.scrollTop;
                    if (position.top < 1)
                        newScrollTop -= $prevTr.height() - position.top;
                    objDiv.scrollTop = newScrollTop;
                }
            }
            this.changeCursorDown = function () {
                $tr = $('#' + _self.gridID + ' tr.selected');
                if ($('#' + _self.gridID + ' > tbody > tr').index($tr) < $('#' + _self.gridID + ' > tbody > tr').length - 1) {
                    $tr.removeClass('selected');
                    $nextTr = $tr.next();
                    $nextTr.addClass('selected');
                    $('#' + _self.hdnID).val($nextTr.find('.keyField').html());

                    var position = $nextTr.position();
                    var objDiv = $("#" + _self.divID)[0];
                    var newScrollTop = objDiv.scrollTop;

                    if ($("#" + _self.divID).height() <= position.top) {
                        var diff = position.top - $("#" + _self.divID).height();
                        newScrollTop += $nextTr.height() + diff;
                    }
                    objDiv.scrollTop = newScrollTop;
                }
            }
            this.changePage = function (idx) {
                var page = parseInt($('#' + _self.pageID).find('.jPag-current').html());
                if (idx < 0) {
                    if (page > 1) {
                        $('#' + _self.pageID).find('li:eq(' + (page - 2) + ')').click();
                    }
                }
                else {
                    if (page < $('#' + _self.pageID).find('li').length)
                        $('#' + _self.pageID).find('li:eq(' + page + ')').click();
                }
            }
            this.bodyKeyPress = function (e) {
                if (e.target.tagName.toUpperCase() == 'INPUT') return;
                var charCode = (e.which) ? e.which : e.keyCode;
                switch (charCode) {
                    case 38: if (!isLoadingPanelVisible()) _self.changeCursorUp(); break; //up
                    case 40: if (!isLoadingPanelVisible()) _self.changeCursorDown(); break; //down
                    case 33: if (!isLoadingPanelVisible()) _self.changePage(-1); break; //page down
                    case 34: if (!isLoadingPanelVisible()) _self.changePage(1); break; //page up
                    case 13: if (!isLoadingPanelVisible()) cbpMPListProcess.PerformCallback('edit'); break;
                }
            }
        }

        $(function () {
            var grd = new customGridView();
            grd.init('grdSelected', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnDataMigrationBack.ClientID %>').click(function () {
                var url = ResolveUrl("~/Libs/Tools/DataMigration/DataMigrationConfigurationList.aspx");
                document.location = url;
            });
        });

        $('.lnkTableLink a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Tools/DataMigration/Configuration/DataMigrationTableLinkCtl.ascx");
            openUserControlPopup(url, id, 'Table Link', 600, 500);
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
                    $('.grdSelected tr:eq(2)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdSelected tr:eq(2)').click();
        }
        //#endregion
    </script>
    <style type="text/css">
        .grdSelected td, .grdSelected th     { font-size: 10px; }
    </style>
    <input type="hidden" value="" id="hdnHeaderID" runat="server" />
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
                                <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all" >
                                    <tr>
                                        <th style="width:70px" rowspan="2" class="keyField">&nbsp;</th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("Table Name")%></th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("Column Name")%></th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("From Column")%></th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("Input Type")%></th>
                                        <th colspan="4"><%=GetLabel("Combo Box")%></th>
                                        <th colspan="3"><%=GetLabel("Check Box")%></th>
                                        <th colspan="1"><%=GetLabel("Date Edit")%></th>    
                                        <th colspan="6"><%=GetLabel("Search Dialog")%></th>   
                                        <th colspan="2"><%=GetLabel("Code")%></th>     
                                    </tr>
                                    <tr>  
                                        <th style="width:120px"><%=GetLabel("Method Name")%></th>
                                        <th style="width:120px"><%=GetLabel("Value Field")%></th>
                                        <th style="width:120px"><%=GetLabel("Text Field")%></th>  
                                        <th style="width:120px"><%=GetLabel("Filter Expression")%></th>  
                                                 
                                        <th style="width:120px"><%=GetLabel("Value Checked")%></th>
                                        <th style="width:120px"><%=GetLabel("Value Unchecked")%></th>
                                        <th style="width:120px"><%=GetLabel("Other Value")%></th>  
                                        
                                        <th style="width:120px"><%=GetLabel("Format Date")%></th>  
                                                    
                                        <th style="width:120px"><%=GetLabel("Type")%></th>
                                        <th style="width:120px"><%=GetLabel("Method Name")%></th>
                                        <th style="width:120px"><%=GetLabel("Filter Expression")%></th>                                                 
                                        <th style="width:120px"><%=GetLabel("ID Field")%></th>
                                        <th style="width:120px"><%=GetLabel("Code Field")%></th>
                                        <th style="width:120px"><%=GetLabel("Name Field")%></th>
                                        
                                        <th style="width:80px"><%=GetLabel("Column")%></th>  
                                        <th style="width:80px"><%=GetLabel("Format Code")%></th>  
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="20">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all" >
                                    <tr>
                                        <th style="width:70px" rowspan="2" class="keyField">&nbsp;</th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("Table Name")%></th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("Column Name")%></th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("From Column")%></th>
                                        <th style="width:100px" rowspan="2"><%=GetLabel("Input Type")%></th>
                                        <th colspan="4"><%=GetLabel("Combo Box")%></th>
                                        <th colspan="3"><%=GetLabel("Check Box")%></th>
                                        <th colspan="1"><%=GetLabel("Date Edit")%></th>    
                                        <th colspan="6"><%=GetLabel("Search Dialog")%></th>    
                                        <th colspan="2"><%=GetLabel("Code")%></th>    
                                    </tr>
                                    <tr>  
                                        <th style="width:80px"><%=GetLabel("Method Name")%></th>
                                        <th style="width:80px"><%=GetLabel("Value Field")%></th>
                                        <th style="width:80px"><%=GetLabel("Text Field")%></th>  
                                        <th style="width:80px"><%=GetLabel("Filter Expression")%></th>  
                                                 
                                        <th style="width:80px"><%=GetLabel("Value Checked")%></th>
                                        <th style="width:80px"><%=GetLabel("Value Unchecked")%></th>
                                        <th style="width:80px"><%=GetLabel("Other Value")%></th>  
                                        
                                        <th style="width:80px"><%=GetLabel("Format Date")%></th>  
                                                    
                                        <th style="width:80px"><%=GetLabel("Type")%></th>
                                        <th style="width:80px"><%=GetLabel("Method Name")%></th>
                                        <th style="width:80px"><%=GetLabel("Filter Expression")%></th>                                                 
                                        <th style="width:80px"><%=GetLabel("ID Field")%></th>
                                        <th style="width:80px"><%=GetLabel("Code Field")%></th>
                                        <th style="width:80px"><%=GetLabel("Name Field")%></th>
                                        
                                        <th style="width:80px"><%=GetLabel("Column")%></th>  
                                        <th style="width:80px"><%=GetLabel("Format Code")%></th>  
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField"><%#: Eval("ID")%></td>
                                    <td><%#: Eval("TableName")%></td>
                                    <td><%#: Eval("ColumnName")%></td>  
                                    <td><%#: Eval("FromColumn")%></td>
                                    <td><%#: Eval("InputType")%></td>

                                    <td><%#: Eval("MethodName")%></td>
                                    <td><%#: Eval("ValueField")%></td>
                                    <td><%#: Eval("TextField")%></td>
                                    <td><%#: Eval("FilterExpression")%></td>

                                    <td><%#: Eval("ValueChecked")%></td>
                                    <td><%#: Eval("ValueUnchecked")%></td>
                                    <td><%#: Eval("OtherValue")%></td>

                                    <td><%#: Eval("FormatDate")%></td>

                                    <td><%#: Eval("SearchDialogType")%></td>
                                    <td><%#: Eval("SearchDialogMethodName")%></td>
                                    <td><%#: Eval("SearchDialogFilterExpression")%></td>
                                    <td><%#: Eval("SearchDialogIDField")%></td>
                                    <td><%#: Eval("SearchDialogCodeField")%></td>
                                    <td><%#: Eval("SearchDialogNameField")%></td>

                                    <td><%#: Eval("IDColumn")%></td>
                                    <td><%#: Eval("FormatCode")%></td>
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
