<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="StockDetailInfo.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.StockDetailInfo" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Refresh")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView2();
            grd.init('grdStockDetail', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

//            $('#<%=txtDateFrom.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });
//            $('#<%=txtDateTo.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });

            $('#<%=txtItemName.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#trTransactionTime').attr('style', 'display:none');

            $('#<%=chkIsFilterByTime.ClientID %>').change(function () {
                if ($('#<%:chkIsFilterByTime.ClientID %>').is(':checked')) {
                    $('#trTransactionTime').removeAttr('style');
                }
                else {
                    $('#trTransactionTime').attr('style', 'display:none');
                    $('#<%=txtStartTime.ClientID %>').val('00:00');
                    $('#<%=txtEndTime.ClientID %>').val('23:59');
                }
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });
        });

        //#region Location
        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }

        $('#lblLocation.lblLink').live('click', function () {
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
                    $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                    $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                }
                else {
                    $('#<%=hdnLocationID.ClientID %>').val('');
                    $('#<%=txtLocationCode.ClientID %>').val('');
                    $('#<%=txtLocationName.ClientID %>').val('');
                }
            });

//            cbpView.PerformCallback('refresh');
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
                if (pageCount > 0)
                    $('.grdStockDetail tr:eq(2)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdStockDetail tr:eq(2)').click();
        }
        //#endregion

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

//        function onCboTypeValueChanged() {
//            onRefreshGrid();
//        }

//        function onCboDepartmentValueChanged() {
//            onRefreshGrid();
//        }

        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();

            var url = ResolveUrl("~/Program/Information/StockDetailInfoDtCtl.ascx");
            openUserControlPopup(url, itemID, 'Mutasi Stock - Detail', 1200, 550);
        });

        $('.lblPurchaseReceiveIN').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '1' ;
            var url = ResolveUrl("~/Program/Information/MovementDt/PurchaseReceiveInfoCtl.ascx");
            openUserControlPopup(url, param, 'Purchase Receive - Detail', 1200, 550);
        });

        $('.lblDistributionIN').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '1';
            var url = ResolveUrl("~/Program/Information/MovementDt/DistributionInfoCtl.ascx");
            openUserControlPopup(url, param, 'Distribution In - Detail', 1200, 550);
        });

        $('.lblAdjustmentIN').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '1';
            var url = ResolveUrl("~/Program/Information/MovementDt/AdjustmentInfoCtl.ascx");
            openUserControlPopup(url, param, 'Adjustment In - Detail', 1200, 550);
        });

        $('.lblReturnIN').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '1';
            var url = ResolveUrl("~/Program/Information/MovementDt/ReturnInfoCtl.ascx");
            openUserControlPopup(url, param, 'Return In - Detail', 1200, 550);
        });

        $('.lblChargesIN').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '1';
            var url = ResolveUrl("~/Program/Information/MovementDt/ChargesInfoCtl.ascx");
            openUserControlPopup(url, param, 'Charges In - Detail', 1200, 550);
        });

        $('.lblPriceChangedIN').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '1';
            var url = ResolveUrl("~/Program/Information/MovementDt/PriceChangedInfoCtl.ascx");
            openUserControlPopup(url, param, 'Price Changed In - Detail', 1200, 550);
        });

        $('.lblChargesOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/ChargesInfoCtl.ascx");
            openUserControlPopup(url, param, 'Charges Out - Detail', 1200, 550);
        });

        $('.lblDistributionOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/DistributionInfoCtl.ascx");
            openUserControlPopup(url, param, 'Distribution Out - Detail', 1200, 550);
        });

        $('.lblAdjustmentOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/AdjustmentInfoCtl.ascx");
            openUserControlPopup(url, param, 'Adjustment Out - Detail', 1200, 550);
        });

        $('.lblConsumptionOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/ConsumptionInfoCtl.ascx");
            openUserControlPopup(url, param, 'Consumption Out - Detail', 1200, 550);
        });

        $('.lblPurchaseReceiveVoidOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/PurchaseReceiveVoidInfoCtl.ascx");
            openUserControlPopup(url, param, 'Purchase Receive Out - Detail', 1200, 550);
        });

        $('.lblReturnOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/ReturnInfoCtl.ascx");
            openUserControlPopup(url, param, 'Return Out - Detail', 1200, 550);
        });

        $('.lblPriceChangedOUT').live('click', function () {
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var itemName = $tr.find('.lblDetail').html();
            var id = $tr.find('.keyField').html();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var locationName = $('#<%=txtLocationName.ClientID %>').val();
            var movementDate = $('#<%=txtDateFrom.ClientID %>').val() + ";" + $('#<%=txtDateTo.ClientID %>').val();
            var param = locationID + '|' + locationName + '|' + movementDate + '|' + itemID + '|' + itemName + '|' + '2';
            var url = ResolveUrl("~/Program/Information/MovementDt/PriceChangedInfoCtl.ascx");
            openUserControlPopup(url, param, 'Price Changed Out - Detail', 1200, 550);
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width:650px;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 600px" />
                    </colgroup>
                    <tr>
                        <td><label><%=GetLabel("Tanggal") %></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" /></td>
                                    <td><asp:CheckBox ID="chkIsFilterByTime" runat="server" Checked="false" /><%:GetLabel("Filter By Time")%></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trTransactionTime">
                        <td><label><%=GetLabel("Jam") %></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:TextBox ID="txtStartTime" Width="54px" CssClass="time" runat="server" Style="text-align: center" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtEndTime" Width="54px" CssClass="time" runat="server" Style="text-align: center" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td><label class="lblLink" id="lblLocation"><%=GetLabel("Lokasi") %></label></td>
                        <td>
                            <input type="hidden" id="hdnLocationID" runat="server" />
                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                <colgroup>
                                    <col width="120px" />
                                    <col width="3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="txtLocationCode" Width="100%" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" ID="txtLocationName" Width="335px" Enabled="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Nama Barang")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtItemName" Width="460px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                             <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <input type="hidden" id="hdnFilterExpression" value="" runat="server" />
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdStockDetail grdSelected" cellspacing="0" rules="all" >
                                            <tr>  
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th rowspan="2" style="width:70px" align="left"><%=GetLabel("Kode Item")%></th>
                                                <th rowspan="2" align="left"><%=GetLabel("Nama Item")%></th>
                                                <th rowspan="2" style="width:60px" align="center"><%=GetLabel("Satuan")%></th>
                                                <th rowspan="2" style="width:60px" align="center"><%=GetLabel("STOK AWAL")%></th>
                                                <th colspan="6" align="center"><%=GetLabel("MASUK")%></th>
                                                <th colspan="7" align="center"><%=GetLabel("KELUAR")%></th>
                                                <th rowspan="2" style="width:60px" align="center"><%=GetLabel("STOK AKHIR")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:60px" align="right"><%=GetLabel("Pembelian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Distribusi")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Penyesuaian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Retur")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Batal Pelayanan")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Perubahan Harga")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Pelayanan")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Distribusi")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Penyesuaian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Pemakaian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Batal Penerimaan")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Batal Retur")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Perubahan Harga")%></th> 
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="25">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdStockDetail grdSelected" cellspacing="0" rules="all" >
                                            <tr>  
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th rowspan="2" style="width:70px" align="left"><%=GetLabel("Kode Item")%></th>
                                                <th rowspan="2" align="left"><%=GetLabel("Nama Item")%></th>
                                                <th rowspan="2" style="width:60px" align="center"><%=GetLabel("Satuan")%></th>
                                                <th rowspan="2" style="width:60px" align="center"><%=GetLabel("STOK AWAL")%></th>
                                                <th colspan="6" align="center"><%=GetLabel("MASUK")%></th>
                                                <th colspan="7" align="center"><%=GetLabel("KELUAR")%></th>
                                                <th rowspan="2" style="width:60px" align="center"><%=GetLabel("STOK AKHIR")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:60px" align="right"><%=GetLabel("Pembelian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Distribusi")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Penyesuaian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Retur")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Batal Pelayanan")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Perubahan Harga")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Pelayanan")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Distribusi")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Penyesuaian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Pemakaian")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Batal Penerimaan")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Batal Retur")%></th> 
                                                <th style="width:60px" align="right"><%=GetLabel("Perubahan Harga")%></th> 
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="keyField"><%#: Eval("ItemID")%></td>
                                            <td><%#: Eval("ItemCode")%></td>
                                            <td><label class="lblLink lblDetail"><%#: Eval("ItemName1")%></label></td>
                                            <td align="center"><%#: Eval("ItemUnit")%></td>
                                            <td align="center"><%#: Eval("cfIN_QuantityBEGINInString")%></td>
                                            <td align="right"><label class="lblLink lblPurchaseReceiveIN"><%#: Eval("cfIN_PurchaseReceiveInString")%></label></td>
                                            <td align="right"><label class="lblLink lblDistributionIN"><%#: Eval("cfIN_DistributionInString")%></label></td>
                                            <td align="right"><label class="lblLink lblAdjustmentIN"><%#: Eval("cfIN_AdjustmentInString")%></td>
                                            <td align="right"><label class="lblLink lblReturnIN"><%#: Eval("cfIN_ReturnInString")%></td>
                                            <td align="right"><label class="lblLink lblChargesIN"><%#: Eval("cfIN_VoidInString")%></td>
                                            <td align="right"><label class="lblLink lblPriceChangedIN"><%#: Eval("cfIN_PriceChangedInString")%></td>                                            
                                            <td align="right"><label class="lblLink lblChargesOUT"><%#: Eval("cfOUT_ChargesInString")%></td>
                                            <td align="right"><label class="lblLink lblDistributionOUT"><%#: Eval("cfOUT_DistributionInString")%></td>
                                            <td align="right"><label class="lblLink lblAdjustmentOUT"><%#: Eval("cfOUT_AdjustmentInString")%></td>
                                            <td align="right"><label class="lblLink lblConsumptionOUT"><%#: Eval("cfOUT_ConsumptionInString")%></td>
                                            <td align="right"><label class="lblLink lblPurchaseReceiveVoidOUT"><%#: Eval("cfOUT_PurchaseReceiveInString")%></td>
                                            <td align="right"><label class="lblLink lblReturnOUT"><%#: Eval("cfOUT_VoidInString")%></td>
                                            <td align="right"><label class="lblLink lblPriceChangedOUT"><%#: Eval("cfOUT_PriceChangedInString")%></td>
                                            <td align="center"><%#: Eval("cfQuantityENDInString")%></td>
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
            </td>
        </tr>
    </table>
</asp:Content>
