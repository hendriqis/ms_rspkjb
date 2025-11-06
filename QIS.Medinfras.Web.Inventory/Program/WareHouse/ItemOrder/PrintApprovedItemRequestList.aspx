<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PrintApprovedItemRequestList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PrintApprovedItemRequestList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPrintItemRequestHdItem" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print")%></div>
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

            $('#<%=btnPrintItemRequestHdItem.ClientID %>').click(function () {
                showLoadingPanel();
                clickPrint();
            });

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Location From
        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }
        $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                $('#<%=txtLocationCode.ClientID %>').val(value);
                onTxtLocationCodeChanged(value);
            });
        });

        $('#<%=txtLocationCode.ClientID %>').live('change', function () {
            onTxtLocationCodeChanged($(this).val());
        });
        function onTxtLocationCodeChanged(value) {
            var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
            Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                    $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                    filterExpression = "LocationID = " + result.LocationID;
                    Methods.getObject('GetLocationList', filterExpression, function (result) {
                        $('#<%=hdnFromLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result.GCLocationGroup);
                    });
                }
                else {
                    $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                    $('#<%=txtLocationCode.ClientID %>').val('');
                    $('#<%=txtLocationName.ClientID %>').val('');
                    $('#<%=hdnFromLocationItemGroupID.ClientID %>').val('');
                    $('#<%=hdnGCLocationGroupFrom.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Location To
        function getLocationFilterExpressionTo() {
            var filterExpression = "<%:filterExpressionLocationTo %>";
            return filterExpression;
        }

        $('#<%=lblLocationTo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpressionTo(), function (value) {
                $('#<%=txtLocationCodeTo.ClientID %>').val(value);
                onTxtLocationToCodeChanged(value);
            });
        });

        $('#<%=txtLocationCodeTo.ClientID %>').live('change', function () {
            onTxtLocationToCodeChanged($(this).val());
        });

        function onTxtLocationToCodeChanged(value) {
            var filterExpression = getLocationFilterExpressionTo() + "LocationCode = '" + value + "'";
            Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                    $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);

                    filterExpression = "LocationID = " + result.LocationID;
                    Methods.getObject('GetLocationList', filterExpression, function (result) {
                        $('#<%=hdnToLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=hdnGCLocationGroupTo.ClientID %>').val(result.GCLocationGroup);
                    });
                }
                else {
                    $('#<%=hdnLocationIDTo.ClientID %>').val('');
                    $('#<%=txtLocationCodeTo.ClientID %>').val('');
                    $('#<%=txtLocationNameTo.ClientID %>').val('');
                    $('#<%=hdnToLocationItemGroupID.ClientID %>').val('');
                    $('#<%=hdnGCLocationGroupTo.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

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
                if (pageCount > 0) {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                }
                else {
                    $('#<%=hdnID.ClientID %>').val('');
                    cbpViewDt.PerformCallback('refresh');
                }

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Check Box
        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }
        //#endregion

        //#region Button Print
        function clickPrint() {
            getCheckedMember();
            var reportCode = "IM090250";
            var itemRequestID = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
            var filterExpression = "ItemRequestID IN (" + itemRequestID + ")";
            if (itemRequestID == "" || itemRequestID == 0) {
                showToast('Warning', 'Harap pilih No. Permintaan yang ingin dicetak!');
            } else {
                openReportViewer(reportCode, filterExpression);
            }
            hideLoadingPanel();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative">
        <tr>
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col style="width: 5%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal lblLink" runat="server" id="lblLocation">
                            <%=GetLabel("Dari Lokasi")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                        <input type="hidden" id="hdnFromLocationItemGroupID" value="" runat="server" />
                        <input type="hidden" id="hdnGCLocationGroupFrom" value="" runat="server" />
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 10%" />
                                <col style="width: 3px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocationName" Width="30%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal lblLink" runat="server" id="lblLocationTo">
                            <%=GetLabel("Kepada Lokasi")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                        <input type="hidden" id="hdnToLocationItemGroupID" value="" runat="server" />
                        <input type="hidden" id="hdnGCLocationGroupTo" value="" runat="server" />
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 10%" />
                                <col style="width: 3px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtLocationCodeTo" Width="100%" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocationNameTo" Width="30%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </tr>
        <tr>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 55%" />
                    <col style="width: 45%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="ItemRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="100px" />
                                                <asp:BoundField DataField="ItemRequestNo" HeaderStyle-HorizontalAlign="Left" HeaderText="No. Permintaan"
                                                    HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="FromLocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Dari Lokasi"
                                                    HeaderStyle-Width="250px" />
                                                <asp:BoundField DataField="ToLocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Kepada Lokasi"
                                                    HeaderStyle-Width="250px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada transaksi permintaan barang")%>
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
                    </td>
                    <td style="vertical-align: top">
                        <div style="position: relative;">
                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList">
                                            <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Item")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("ItemName1")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="150px"
                                                        ItemStyle-HorizontalAlign="Right">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <%=GetLabel("Jumlah Permintaan")%></div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div>
                                                                <%#: Eval("cfItemRequestQuantity")%></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Tidak ada informasi Detail Permintaan Barang")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingDt">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </tr>
    </div>
</asp:Content>
