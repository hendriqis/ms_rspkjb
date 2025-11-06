<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ReorderItemRequest.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ReorderItemRequest" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnReorderItemRequestProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtItemOrderDate.ClientID %>');
            $('#<%=txtItemOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnReorderItemRequestProcess.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    getCheckedMember();
                    if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                        showToast('Warning', 'Please Select Item First');
                    }
                    else {
                        onCustomButtonClick('approve');
                    }
                }
            });

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

//            function onTxtLocationCodeChanged(value) {
//                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
//                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
//                    if (result != null) {
//                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
//                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
//                    }
//                    else {
//                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
//                        $('#<%=txtLocationCode.ClientID %>').val('');
//                        $('#<%=txtLocationName.ClientID %>').val('');
//                    }
//                });
//            }
            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
                var LocationToID = $('#<%=hdnLocationIDTo.ClientID %>').val();
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        if (LocationToID != "" && LocationToID != "0") {
                            if (LocationToID != result.LocationID) {
                                $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                                $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                                filterExpression = "LocationID = " + result.LocationID;
                                Methods.getObject('GetLocationList', filterExpression, function (result2) {
                                });
                            } else {
                                showToast('Information', 'Anda memilih lokasi yang sama');
                                $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                                $('#<%=txtLocationCode.ClientID %>').val('');
                                $('#<%=txtLocationName.ClientID %>').val('');
                            }
                        } else {
                            $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                            $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                            filterExpression = "LocationID = " + result.LocationID;
                            Methods.getObject('GetLocationList', filterExpression, function (result) {
                            });
                        }
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
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

//            function onTxtLocationToCodeChanged(value) {
//                var filterExpression = getLocationFilterExpressionTo() + "LocationCode = '" + value + "'";
//                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
//                    if (result != null) {
//                        $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
//                        $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);

//                        filterExpression = "LocationID = " + result.LocationID;
//                        Methods.getObject('GetLocationList', filterExpression, function (result) {
//                            $('#<%=hdnGCLocationGroupTo.ClientID %>').val(result.GCLocationGroup);
//                        });
//                    }
//                    else {
//                        $('#<%=hdnLocationIDTo.ClientID %>').val('');
//                        $('#<%=txtLocationCodeTo.ClientID %>').val('');
//                        $('#<%=txtLocationNameTo.ClientID %>').val('');
//                    }
//                });
            //            }
            function onTxtLocationToCodeChanged(value) {
                var filterExpression = "LocationCode = '" + value + "' AND IsDeleted = 0";
                var LocationFromID = $('#<%=hdnLocationIDFrom.ClientID %>').val();

                Methods.getObject('GetLocationList', filterExpression, function (result) {
                    if (result != null) {
                        if (LocationFromID != result.LocationID) {
                            $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                            $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);
                        } else {
                            showToast('Information', 'Anda memilih lokasi yang sama');
                            $('#<%=hdnLocationIDTo.ClientID %>').val('');
                            $('#<%=txtLocationCodeTo.ClientID %>').val('');
                            $('#<%=txtLocationNameTo.ClientID %>').val('');
                        }
                    }
                    else {
                        $('#<%=hdnLocationIDTo.ClientID %>').val('');
                        $('#<%=txtLocationCodeTo.ClientID %>').val('');
                        $('#<%=txtLocationNameTo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=btnRefresh.ClientID %>').click(function () {
                var value = $('#<%=rbFilterItemLocation.ClientID %>').find('input:checked').val();
                var fromID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                var toID = $('#<%=hdnLocationIDTo.ClientID %>').val();

                if (value == 'from') {
                    if (fromID != '' && fromID != 0) {
                        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                            getCheckedMember();
                            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                                var message = "Ada item-item yang sudah dipilih, apakah proses refresh tetap dilanjutkan ?";
                                showToastConfirmation(message, function (result) {
                                    if (result) cbpView.PerformCallback('refresh'); ;
                                });
                            }
                            cbpView.PerformCallback('refresh');
                        }
                    }
                    else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Lokasi Asal terlebih dahulu.");
                        $('#<%=rbFilterItemLocation.ClientID %>').find("input[value='from']").attr("checked", "checked");
                    }
                }
                else {
                    if (toID != '' && toID != 0) {
                        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                            getCheckedMember();
                            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                                var message = "Ada item-item yang sudah dipilih, apakah proses refresh tetap dilanjutkan ?";
                                showToastConfirmation(message, function (result) {
                                    if (result) cbpView.PerformCallback('refresh'); ;
                                });
                            }
                            cbpView.PerformCallback('refresh');
                        }
                    }
                    else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Lokasi Tujuan terlebih dahulu.");
                        $('#<%=rbFilterItemLocation.ClientID %>').find("input[value='from']").attr("checked", "checked");
                    }
                }
            });

            $('#<%=rbFilterItemLocation.ClientID %>').live('change', function () {
                var value = $(this).find('input:checked').val();
                var fromID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                var toID = $('#<%=hdnLocationIDTo.ClientID %>').val();

                if (value == 'from') {
                    if (fromID != '' && fromID != 0) {
                    }
                    else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Lokasi Asal terlebih dahulu.");
                        $('#<%=rbFilterItemLocation.ClientID %>').find("input[value='from']").attr("checked", "checked");
                    }
                } else {
                    if (toID != '' && toID != 0) {
                    }
                    else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Lokasi Tujuan terlebih dahulu.");
                        $('#<%=rbFilterItemLocation.ClientID %>').find("input[value='from']").attr("checked", "checked");
                    }
                }
            });

            //#region Product Line
            function getProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblProductLine.ClientID %>.lblLink').click(function () {
                openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnProductLineItemType.ClientID %>').val(result.GCItemType);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                        $('#<%=hdnProductLineItemType.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked'))
                $tr.find('.txtItemRequest').removeAttr('readonly');
            else
                $tr.find('.txtItemRequest').attr('readonly', 'readonly');
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Save Success', 'Permintaan Barang Berhasil Dibuat Dengan No Permintaan <b>' + retval + '</b>', function () {
                $('#<%=hdnItemRequest.ClientID %>').val('');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
            var lstItemRequest = $('#<%=hdnItemRequest.ClientID %>').val().split('|');
            var result = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var itemRequest = $(this).closest('tr').find('.txtItemRequest').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstItemRequest.push(itemRequest);
                    }
                    else {
                        lstItemRequest[idx] = itemRequest;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var itemRequest = $(this).closest('tr').find('.txtItemRequest').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstItemRequest.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnItemRequest.ClientID %>').val(lstItemRequest.join('|'));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
        }

        $('#<%=rblROPDynamic.ClientID %>').live('change', function () {
        });

        $('#<%=rblDisplayMinimum.ClientID %>').live('change', function () {
        });

        $('#<%=rblDisplayOption.ClientID %>').live('change', function () {
            var displayOption = $('#<%=rblDisplayOption.ClientID %>').find(":checked").val();
            $('#<%=hdnDisplayOption.ClientID %>').val(displayOption);
        });


        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnItemRequest" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="1" id="hdnDisplayOption" runat="server" />
    <div style="height: 495px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
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
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trProductLine" runat="server" style="display: none">
                            <td>
                                <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <input type="hidden" id="hdnProductLineItemType" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblFilterItem" runat="server">
                                    <%=GetLabel("Filter Item") %></label>
                            </td>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rbFilterItemLocation" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Lokasi Asal" Value="from" Selected="True" />
                                    <asp:ListItem Text="Lokasi Tujuan" Value="to" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsFilterQtyOnHand" runat="server" Checked="false" /><%:GetLabel("Tersedia di Lokasi Tujuan Minta")%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
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
                                            <asp:TextBox ID="txtLocationNameTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 160px">
                                            <asp:TextBox ID="txtItemOrderDate" Width="110px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderTime" Width="100px" CssClass="time" runat="server" Style="text-align: center"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("R.O.P")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblROPDynamic" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Statis" Value="false" Selected="True" />
                                    <asp:ListItem Text="Dinamis" Value="true" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tampilkan")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblDisplayMinimum" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="<= Minimum" Value="true" Selected="True" />
                                    <asp:ListItem Text="< Minimum" Value="false" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Display Option")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblDisplayOption" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Semua" Value="0" />
                                    <asp:ListItem Text="Rekomendasi Sistem" Value="1" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" />
                                            <asp:BoundField DataField="BinLocationName" HeaderText="Lokasi Rak" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomMinimum" HeaderText="Minimum" HeaderStyle-Width="80px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomMaximum" HeaderText="Maximum" HeaderStyle-Width="80px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomEndingBalance" HeaderText="Stok Saat Ini" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomEndingBalanceOthersLocation" HeaderText="Stok Lokasi Lain"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomQtyChargesLastMonth" HeaderText="Penjualan Bulan Lalu"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomQtyChargesThisMonth" HeaderText="Penjualan Bulan Ini"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomAverageQty" HeaderText="Pemakaian Rata-Rata" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfItemMovementPerDateThisMonthInString" HeaderText="Pemakaian Rata-Rata (Bln Ini)"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" HeaderText="Diminta">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtItemRequest" Width="50%" runat="server" CssClass="number txtItemRequest"
                                                        ReadOnly="true" />
                                                    &nbsp;
                                                    <%#: Eval("ItemUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomQtyOnOrderItemRequest" HeaderText="Quantity On Unit Request"
                                                HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
