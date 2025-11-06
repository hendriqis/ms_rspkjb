<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.Master" AutoEventWireup="true" 
    CodeBehind="DataMigration.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Tools.DataMigration" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">
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

                $('#' + _self.gridID + ' tr:gt(0):not(.trEmpty)').live('click', function () {
                    $('#' + _self.gridID + ' tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#' + _self.hdnID).val($(this).find('.keyField').html());
                });

                $('#' + _self.gridID + ' tr:gt(0):not(.trEmpty)').live('dblclick', function () {
                    cbpMPListProcess.PerformCallback('edit');
                });
                $('#' + _self.gridID + ' tr:eq(1)').click();

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
                //if (e.target.tagName.toUpperCase() == 'INPUT') return;
                var charCode = (e.which) ? e.which : e.keyCode;
                if (e.ctrlKey) {
                    switch (charCode) {
                        case 38: if (!isLoadingPanelVisible()) _self.changeCursorUp(); break; //up
                        case 40: if (!isLoadingPanelVisible()) _self.changeCursorDown(); break; //down
                        case 33: if (!isLoadingPanelVisible()) _self.changePage(-1); break; //page down
                        case 34: if (!isLoadingPanelVisible()) _self.changePage(1); break; //page up
                        case 13: if (!isLoadingPanelVisible()) {
                                $('#<%=btnMPListSave.ClientID %>').click();
                            } break;
                        case 48: if (!isLoadingPanelVisible()) {
                                if (confirm('Data Will Be Lost. Are You Sure?')) {
                                    $('#<%=hdnEntryID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
                                    cbpEntry.PerformCallback('');
                                }
                            } break;
                        case 46: if (!isLoadingPanelVisible()) {
                                if (confirm('Are You Sure Want To Delete This Data?')) {
                                    $('#<%=hdnEntryID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
                                    cbpProcess.PerformCallback('delete');
                                }
                            } break;
                    }
                }
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                var filterExpression = s.GenerateFilterExpression();
                onRefreshControl(filterExpression);
            }, 0);
        }

        $(function () {
            $('#<%=btnMPListEdit.ClientID %>').click(function () {
                if (confirm('Data Will Be Lost. Are You Sure?')) {
                    if ($('#<%=hdnID.ClientID %>').val() != '') {
                        $('#<%=hdnEntryID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
                        cbpEntry.PerformCallback('');
                    }
                }
            });
            $('#<%=btnMPListDelete.ClientID %>').click(function () {
                if (confirm('Are You Sure Want To Delete This Data?')) {
                    $('#<%=hdnEntryID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
                    cbpProcess.PerformCallback('delete');
                }
            });

            $('#<%=btnMPListSave.ClientID %>').click(function (evt) {
                if ($('#<%=grdView.ClientID %> tr.selected').length > 0) {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        $('#<%=hdnIsSave.ClientID %>').val('1');
                        cbpProcess.PerformCallback('save');
                    }
                    else if ($('#<%=hdnIsSaveAll.ClientID %>').val() == '1') {
                        cbpProcess.PerformCallback('savefailed');
                    }
                }
                else
                    $('#<%=hdnIsSaveAll.ClientID %>').val('0');
            });

            $('#<%=btnMPListProcessedData.ClientID %>').click(function () {
                var url = ResolveUrl("~/Libs/Tools/DataMigration/Migration/DataMigrationProcessedDataCtl.ascx");
                openUserControlPopup(url, '', 'Processed Data', 900, 500);
            });

            $('#<%=btnMPListSaveAll.ClientID %>').click(function () {
                $('#<%=hdnIsSaveAll.ClientID %>').val('1');
                $('#<%=btnMPListSave.ClientID %>').click();
            });

        });

        function onCbpEntryEndCallback() {
            $('#fsEntryPopup :input:visible:first').focus();

            $('.datepicker:visible').each(function () {
                setDatePickerElement($(this));
            });
            hideLoadingPanel();
            if ($('#<%=hdnIsSaveAll.ClientID %>').val() == '1')
                $('#<%=btnMPListSave.ClientID %>').click();
        }

        function onCbpProcessEndCallback(s) {
            $('#<%=hdnID.ClientID %>').val('');
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
                $('#<%=hdnIsSave.ClientID %>').val('0');
            }
            else {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            
            hideLoadingPanel();
        }

        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnID.ClientID %>').val('');
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }


        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
            $('#<%=hdnEntryID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
            cbpEntry.PerformCallback('');
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

            $('#<%=hdnEntryID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
            cbpEntry.PerformCallback('');
        }
        //#endregion
    </script>    
    <div class="toolbarArea">
        <table style="float:right;margin-top:20px;" id="tblFilter" runat="server">
            <tr>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView" Width="300px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
        </table>
        <ul>
            <li id="btnMPListEdit" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Edit")%></div></li>
            <li id="btnMPListDelete" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Delete")%></div></li>
            <li id="btnMPListSave" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Save")%></div></li>
            <li id="btnMPListProcessedData" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Processed Data")%></div></li>
            <li id="btnMPListSaveAll" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Save All")%></div></li>
        </ul>
    </div>
    
    <input type="hidden" value="0" id="hdnIsSaveAll" runat="server" />
    <input type="hidden" value="" id="hdnHeaderID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div id="containerPopupEntryData" style="margin-top:10px;">
        <div class="pageTitle"><%=GetLabel("Entry")%></div>
        <input type="hidden" id="hdnEntryID" runat="server" value="" />
        <fieldset id="fsMPEntry" style="margin:0"> 
            <dxcp:ASPxCallbackPanel ID="cbpEntry" runat="server" Width="100%" ClientInstanceName="cbpEntry"
                ShowLoadingPanel="false" OnCallback="cbpEntry_Callback">  
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel();}"
                    EndCallback="function(s,e){ onCbpEntryEndCallback(); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:Panel runat="server" ID="Panel1" Style="max-height:300px; width:1366px; overflow: scroll;" >
                            <table class="tblEntryDetail">
                            <asp:Repeater ID="rptEntry" runat="server" OnItemDataBound="rptEntry_ItemDataBound">
                                <ItemTemplate>
                                    <td valign="top">
                                        <h4><%#:Eval("LinkTableName")%> - <%#:Eval("ColumnName")%> </h4>
                                        <input type="hidden" id="hdnCode" runat="server" value='<%#:Eval("LinkTableName") %>' />
                                        <input type="hidden" id="hdnLinkTableColumn" runat="server" value='<%#:Eval("ColumnName") %>' />
                                        <asp:Repeater ID="rptEntryDt" runat="server" OnItemDataBound="rptEntryDt_ItemDataBound">
                                            <HeaderTemplate>
                                                 <table>
                                                    <colgroup>
                                                        <col style="width:150px"/>
                                                        <col style="width:150px"/>
                                                        <col />
                                                    </colgroup>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="tdLabel"><label id="lblColumn" runat="server" dbid="<%#: Container.ItemIndex %>" ><%#:Eval("ColumnCaption")%></label></td>
                                                    <td><asp:TextBox ID="txtOldValue" ReadOnly="true" Width="150px" runat="server" /></td>
                                                    <td>
                                                        <input type="hidden" id="hdnIsRequired" runat="server" value='<%#:Eval("IsRequired") %>' />
                                                        <input type="hidden" id="hdnType" runat="server" value='<%#:Eval("Type") %>' />
                                                        <input type="hidden" id="hdnColumn" runat="server" value='<%#:Eval("ColumnName") %>' />
                                                        <div id="divCode" runat="server" visible="false">
                                                            <input type="hidden" runat="server" id="hdnFormatCode" />
                                                            <input type="hidden" runat="server" id="hdnIDColumn" />
                                                            <asp:TextBox ID="txtCode" Width="200px" ReadOnly="true" runat="server" />
                                                        </div>
                                                        <div id="divTxt" runat="server" visible="false">
                                                            <asp:TextBox ID="txtValue" Width="200px" runat="server" Style="float:left" />
                                                        </div>
                                                        <div id="divDdl" runat="server" visible="false">
                                                            <dxe:ASPxComboBox ID="cboNewValue" Width="200px" runat="server" />
                                                        </div>
                                                        <div id="divDte" runat="server" visible="false">                                                            
                                                            <asp:TextBox ID="txtDteValue" Width="120px" runat="server" CssClass="datepicker" dteid="<%#: Container.ItemIndex %>"  />
                                                        </div>
                                                        <div id="divChk" runat="server" visible="false">
                                                            <asp:CheckBox ID="chkValue" runat="server" />
                                                        </div>
                                                        <div id="divSd" runat="server" visible="false">
                                                            <input type="hidden" runat="server" id="hdnSdNewID" />
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtSdNewCode" dbid="<%#: Container.ItemIndex %>" Width="200px" runat="server" /></td>
                                                                    <td style="width:5px">&nbsp;</td>
                                                                    <td><asp:TextBox ID="txtSdNewText" disabled="disabled" ReadOnly="true" Width="200px" runat="server" /></td>
                                                                </tr>
                                                            </table>
                                                            <script type="text/javascript">
                                                                var dbId = '<%#: Container.ItemIndex %>';
                                                                $('label[dbid=' + dbId + ']').live('click', function () {
                                                                    $tr = $(this).closest('tr');
                                                                    $txtCode = $tr.find('input:visible:eq(1)');
                                                                    $txtName = $txtCode.closest('div').find('input:eq(2)');
                                                                    $hdnID = $txtCode.closest('div').find('input').first();
                                                                    openSearchDialog('<%#:Eval("SearchDialogType") %>', '', function (value) {
                                                                        $txtCode.val(value);
                                                                        $txtCode.focus();
                                                                        var codeField = '<%#:Eval("SearchDialogCodeField") %>';
                                                                        var filterExpression = codeField + " = '" + value + "'";
                                                                        var baseFilterExpression = '<%#:Eval("SearchDialogFilterExpression") %>';
                                                                        if (baseFilterExpression != "")
                                                                            filterExpression += " AND " + baseFilterExpression;
                                                                        Methods.getObject('<%#:Eval("SearchDialogMethodName") %>', filterExpression, function (result) {
                                                                            if (result != null) {
                                                                                $hdnID.val(result.<%#:Eval("SearchDialogIDField") %>);
                                                                                $txtName.val(result.<%#:Eval("SearchDialogNameField") %>);
                                                                            }
                                                                            else {
                                                                                $txtCode.val('');
                                                                                $hdnID.val('');
                                                                                $txtName.val('');
                                                                            }
                                                                        });
                                                                    });
                                                                });
                                                                $('input[dbid=' + dbId + ']').live('change', function () {
                                                                    var value = $(this).val();
                                                                    $txtCode = $(this);
                                                                    $txtName = $txtCode.closest('div').find('input:eq(2)');
                                                                    $hdnID = $txtCode.closest('div').find('input').first();

                                                                    var codeField = '<%#:Eval("SearchDialogCodeField") %>';
                                                                    var filterExpression = codeField + " = '" + value + "'";
                                                                    var baseFilterExpression = '<%#:Eval("SearchDialogFilterExpression") %>';
                                                                    if (baseFilterExpression != "")
                                                                        filterExpression += " AND " + baseFilterExpression;

                                                                    Methods.getObject('<%#:Eval("SearchDialogMethodName") %>', filterExpression, function (result) {
                                                                        if (result != null) {
                                                                            $hdnID.val(result.<%#:Eval("SearchDialogIDField") %>);
                                                                            $txtName.val(result.<%#:Eval("SearchDialogNameField") %>);
                                                                        }
                                                                        else {
                                                                            $txtCode.val('');
                                                                            $hdnID.val('');
                                                                            $txtName.val('');
                                                                        }
                                                                    });
                                                                });
                                                            </script>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                                <tr style="display:none">
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </fieldset>
    </div>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="max-height:230px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">                            
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
    
    <input type="hidden" value="" id="hdnIsSave" runat="server" />
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
            EndCallback="function(s,e){ onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
