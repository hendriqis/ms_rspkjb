<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="ItemPlanningList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemPlanningList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView2();
            grd.init('grdView', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#ulTabGrdDetail li').click(function () {
                $('#ulTabGrdDetail li.selected').removeAttr('class');
                $('.containerGrdDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');

            });

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                return "<%=OnGetItemGroupFilterExpression() %>";
            }

            $('#lblItemGroupMaster.lblLink').live('click', function () {
                openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').live('change', function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion
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

        function onCbpViewBeginCallback() {
            showLoadingPanel();
            $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

            $('.grdDetail1 tr:gt(0)').remove();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            $('#<%=hdnID.ClientID %>').val('');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('grdView tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            $('grdView tr:eq(2)').click();
        }
        //#endregion

        $('.grdView td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $('#ulTabGrdDetail li:eq(0)').click();

                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='10'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('.grdDetail1 tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail1.PerformCallback('refresh');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $('.grdDetail1 tr:gt(0)').remove();

                $trDetail.remove();
            }
        });

        function oncboItemTypeValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            cbpViewDetail1.PerformCallback();
        }

        $('.lnkEditItemPlanning').live('click', function () {
            var itemPlanningID = $(this).closest('tr').find('.keyField').html();
            var factorXMin = $('#<%=hdnFactorXMin.ClientID %>').val();
            var factorXMax = $('#<%=hdnFactorXMax.ClientID %>').val();
            var param = itemPlanningID + '|' + factorXMin + '|' + factorXMax;
            var url = ResolveUrl("~/Program/Master/ItemProduct/ItemProductPlanningEntryEditCtl.ascx");
            openUserControlPopup(url, param, 'Perencanaan Persediaan', 750, 500);
        });

        $('.lnkItemPriceHistory').live('click', function () {
            var param = $(this).closest('tr').find('.keyItemID').val();
            var url = ResolveUrl("~/Program/Master/ItemProduct/ItemPriceHistoryDetailCtl.ascx");
            openUserControlPopup(url, param, 'Item Price History', 1200, 500);
        });

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" value="0" id="hdnFactorXMin" runat="server" />
    <input type="hidden" value="0" id="hdnFactorXMax" runat="server" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col style="width: 300px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <label class="lblNormal" id="lblItemType">
                    <%=GetLabel("Tipe Item")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" runat="server"
                    Width="150px">
                    <ClientSideEvents ValueChanged="function(s,e){ oncboItemTypeValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <input type="hidden" value="" runat="server" id="hdnItemGroupID" />
                <label class="lblLink" id="lblItemGroupMaster">
                    <%=GetLabel("Kelompok Item")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ onCbpViewBeginCallback(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:ListView runat="server" ID="lvwView">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdView notAllowSelect grdItemBalance"
                                    cellspacing="0" rules="all">
                                    <tr>
                                        <th class="keyField" rowspan="2">
                                            &nbsp;
                                        </th>
                                        <th style="width: 20px">
                                            &nbsp;
                                        </th>
                                        <th style="width: 150px; text-align: left">
                                            <%=GetLabel("Tipe Item")%>
                                        </th>
                                        <th style="width: 70px;">
                                            <%=GetLabel("Kode")%>
                                        </th>
                                        <th style="width: 70px;">
                                            <%=GetLabel("Kode Lama")%>
                                        </th>
                                        <th style="text-align: left">
                                            <%=GetLabel("Nama Item")%>
                                        </th>
                                        <th style="width: 230px; text-align: left">
                                            <%=GetLabel("Kelompok Item & Product Line")%>
                                        </th>
                                        <th style="width: 120px;">
                                            <%=GetLabel("Satuan Kecil")%>
                                        </th>
                                        <th style="width: 150px; text-align: center">
                                            <%=GetLabel("Dibuat Oleh")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="20">
                                            <%=GetLabel("Tidak ada data persediaan")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdView notAllowSelect grdItemBalance"
                                    cellspacing="0" rules="all">
                                    <tr>
                                        <th class="keyField" rowspan="2">
                                            &nbsp;
                                        </th>
                                        <th style="width: 20px">
                                            &nbsp;
                                        </th>
                                        <th style="width: 150px;">
                                            <%=GetLabel("Tipe Item")%>
                                        </th>
                                        <th style="width: 70px;">
                                            <%=GetLabel("Kode")%>
                                        </th>
                                        <th style="width: 70px;">
                                            <%=GetLabel("Kode Lama")%>
                                        </th>
                                        <th style="text-align: left">
                                            <%=GetLabel("Nama Item")%>
                                        </th>
                                        <th style="width: 230px; text-align: left">
                                            <%=GetLabel("Kelompok Item & Product Line")%>
                                        </th>
                                        <th style="width: 120px;">
                                            <%=GetLabel("Satuan Kecil")%>
                                        </th>
                                        <th style="width: 150px; text-align: center">
                                            <%=GetLabel("Dibuat Oleh")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("ItemID")%>
                                    </td>
                                    <td align="center" class="tdExpand">
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                            alt='' />
                                    </td>
                                    <td>
                                        <%#: Eval("ItemType")%>
                                    </td>
                                    <td>
                                        <%#: Eval("ItemCode")%>
                                    </td>
                                    <td>
                                        <%#: Eval("OldItemCode")%>
                                    </td>
                                    <td>
                                        <%#: Eval("ItemName1")%>
                                    </td>
                                    <td>
                                        <i><b>
                                            <%=GetLabel("Kelompok Item : ")%></b></i><%#: Eval("ItemGroupName1")%>
                                        (<%#: Eval("ItemGroupCode")%>)<br />
                                        <i><b>
                                            <%=GetLabel("Product Line : ")%></b></i><%#: Eval("ProductLineName")%>
                                        (<%#: Eval("ProductLineCode")%>)
                                    </td>
                                    <td align="center">
                                        <%#: Eval("ItemUnit")%>
                                    </td>
                                    <td align="center">
                                        <%#: Eval("CreatedByName")%><div>
                                            <%#: Eval("cfCreatedDateInString")%></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
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
    <div id="tempContainerGrdDetail" style="display: none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px;">
            <div class="containerUlTabPage">
                <ul class="ulTabPage" id="ulTabGrdDetail">
                    <li class="selected" contentid="containerDetail1">
                        <%=GetLabel("Perencanaan") %></li>
                </ul>
            </div>
            <div style="position: relative;">
                <div id="containerDetail1" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail1" runat="server" Width="100%" ClientInstanceName="cbpViewDetail1"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail1_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:ListView runat="server" ID="lvwDetail1">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail1" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">
                                                    </th>
                                                    <th style="width: 50px" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 300px; text-align: left" rowspan="2">
                                                        <%=GetLabel("Healthcare")%>
                                                    </th>
                                                    <th style="text-align: left" rowspan="2">
                                                        <%=GetLabel("Default Supplier")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 120px; text-align: center">
                                                        <%=GetLabel("Satuan Pembelian")%>
                                                    </th>
                                                    <th colspan="3">
                                                        <%=GetLabel("Harga")%>
                                                    </th>
                                                    <th style="width: 50px" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Rata-Rata")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Satuan Kecil")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Satuan Besar")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Tidak ada data perencanaan persediaan")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">
                                                    </th>
                                                    <th style="width: 50px" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 300px; text-align: left" rowspan="2">
                                                        <%=GetLabel("Healthcare")%>
                                                    </th>
                                                    <th rowspan="2" style="text-align: left">
                                                        <%=GetLabel("Default Supplier")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 120px; text-align: center">
                                                        <%=GetLabel("Satuan Pembelian")%>
                                                    </th>
                                                    <th colspan="3">
                                                        <%=GetLabel("Harga")%>
                                                    </th>
                                                    <th style="width: 50px" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Rata-Rata")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Satuan Kecil")%>
                                                    </th>
                                                    <th style="width: 120px">
                                                        <%=GetLabel("Satuan Besar")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("ID")%>
                                                </td>
                                                <td align="center">
                                                    <input type="hidden" class="keyItemID" value="<%#: Eval("ItemID")%>"><img class="lnkEditItemPlanning imgLink"
                                                        title="<%=GetLabel("Edit") %>" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                </td>
                                                <td>
                                                    <%#: Eval("HealthcareName")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("BusinessPartnerName")%>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("PurchaseUnit")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("AveragePrice", "{0:N}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("UnitPrice", "{0:N}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("PurchaseUnitPrice", "{0:N}")%>
                                                </td>
                                                <td align="center">
                                                    <img class="lnkItemPriceHistory imgLink" title="<%=GetLabel("Price History") %>"
                                                        src='<%# ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                                        width="30px" />
                                                </td>
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
            </div>
        </div>
    </div>
</asp:Content>
