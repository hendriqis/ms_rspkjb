<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="ItemServiceAIOList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceAIOList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnpPackageItemID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnpPackageItemID.ClientID %>').val($(this).find('.keyField').html());

                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var filterExpression = "<%:OnGetItemGroupFilterExpression() %>";
                return filterExpression;
            }

            $('#lblItemGroup.lblLink').live('click', function () {
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
                    cbpViewDt.PerformCallback('refresh');
                });
            }
            //#endregion
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            cbpViewDt.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnpPackageItemID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        $('#lblAddData').live('click', function () {
            var id = "add|0|" + $('#<%=hdnpPackageItemID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/ItemServiceAllInOneEntryCtl.ascx");
            showLoadingPanel();
            openUserControlPopup(url, id, 'Item Detail Package AIO', 1000, 500);
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);

            var id = "edit|" + entity.ID + "|" + $('#<%=hdnpPackageItemID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemService/ItemServiceAllInOneEntryCtl.ascx");
            showLoadingPanel();
            openUserControlPopup(url, id, 'Item Detail Package AIO', 1000, 500);

        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function (evt) {
            evt.stopPropagation();
            evt.preventDefault();
            if (confirm("Are You Sure Want To Delete This Data?")) {
                $row = $(this).closest('tr');
                var entity = rowToObject($row);
                $('#<%=hdnItemServiceDtID.ClientID %>').val(entity.ID);

                cbpViewDt.PerformCallback('delete');
            }
        });

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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
            }
            else {
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnpPackageItemID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnItemServiceDtID" runat="server" />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 130px">
                <label class="lblLink" id="lblItemGroup">
                    <%=GetLabel("Kelompok Item") %></label>
            </td>
            <td>
                <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <table style="width: 100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top;">
                                <table style="width: 96%;">
                                    <tr>
                                        <td>
                                            <div style="height: 450px; overflow-y: auto; overflow-x: hidden">
                                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail')); showLoadingPanel(); }"
                                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Item Paket" HeaderStyle-CssClass="gridColumnText"
                                                                            ItemStyle-CssClass="gridColumnText" HeaderStyle-Width="100px" />
                                                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item Paket" HeaderStyle-CssClass="gridColumnText"
                                                                            ItemStyle-CssClass="gridColumnText" />
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
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top;">
                                <div style="width: 100%; text-align: center" id="divAddData" runat="server">
                                    <span class="lblLink" id="lblAddData">
                                        <%= GetLabel("Add Data")%></span>
                                </div>
                                <div style="height: 500px; overflow-y: auto; overflow-x: hidden">
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
                                                            <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" />
                                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" />
                                                                    <input type="hidden" bindingfield="ID" value="<%#: Eval("ID")%>" />
                                                                    <input type="hidden" bindingfield="ItemID" value="<%#: Eval("ItemID")%>" />
                                                                    <input type="hidden" bindingfield="DepartmentID" value="<%#: Eval("DepartmentID")%>" />
                                                                    <input type="hidden" bindingfield="DepartmentName" value="<%#: Eval("DepartmentName")%>" />
                                                                    <input type="hidden" bindingfield="HealthcareServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                                    <input type="hidden" bindingfield="ServiceUnitID" value="<%#: Eval("ServiceUnitID")%>" />
                                                                    <input type="hidden" bindingfield="ServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                                    <input type="hidden" bindingfield="ServiceUnitName" value="<%#: Eval("ServiceUnitName")%>" />
                                                                    <input type="hidden" bindingfield="GCItemType" value="<%#: Eval("GCItemType")%>" />
                                                                    <input type="hidden" bindingfield="ItemType" value="<%#: Eval("ItemType")%>" />
                                                                    <input type="hidden" bindingfield="DetailItemID" value="<%#: Eval("DetailItemID")%>" />
                                                                    <input type="hidden" bindingfield="DetailItemCode" value="<%#: Eval("DetailItemCode")%>" />
                                                                    <input type="hidden" bindingfield="DetailItemName1" value="<%#: Eval("DetailItemName1")%>" />
                                                                    <input type="hidden" bindingfield="Quantity" value="<%#: Eval("Quantity")%>" />
                                                                    <input type="hidden" bindingfield="IsControlAmount" value="<%#: Eval("IsControlAmount")%>" />
                                                                    <input type="hidden" bindingfield="ParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                                    <input type="hidden" bindingfield="ParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderText="Unit Pelayanan"
                                                                HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <label style="font-size: small; font-style: italic">
                                                                        <%#: Eval("DepartmentID") %></label><br />
                                                                    <label style="font-size: small">
                                                                        <b>
                                                                            <%#: Eval("ServiceUnitName")%></b></label><br />
                                                                    <label style="font-size: x-small">
                                                                        <%#: Eval("ServiceUnitCode")%></label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Detail Item" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <label style="font-size: xx-small; font-weight: bold">
                                                                        <%#: Eval("ItemType")%></label><br />
                                                                    <label style="font-size: small; font-weight: bold">
                                                                        <%#: Eval("DetailItemName1")%></label>
                                                                    <img class="lblIsControlAmount" title="<%=GetLabel("Kontrol Batasan Nilai Obat Alkes") %>"
                                                                        src='<%# ResolveUrl("~/Libs/Images/Status/coverage_ok.png")%>' alt="" style='<%# Eval("IsControlAmount").ToString() == "True" ? "": "display:none" %>'
                                                                        width="20px" />
                                                                    <br />
                                                                    <label style="font-size: x-small; font-style: italic">
                                                                        <%#: Eval("DetailItemCode") %></label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderText="Buku Tariff"
                                                                HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <label style="font-size: xx-small; font-style: italic">
                                                                        <%#: Eval("TariffScheme") %></label><br />
                                                                    <label style="font-size: small">
                                                                        <b>
                                                                            <%#: Eval("DocumentNo")%></b></label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                HeaderText="Quantity" HeaderStyle-Width="70px" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak Ada Detail Paket AIO")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
