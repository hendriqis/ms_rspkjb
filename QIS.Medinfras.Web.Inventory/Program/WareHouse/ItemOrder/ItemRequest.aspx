<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ItemRequest.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemRequest" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setRightPanelButtonEnabled();
        });

        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblAddData').show();
                $('#lblQuickPick').show();
            }
            else {
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();
            }

            setDatePicker('<%=txtItemOrderDate.ClientID %>');
            $('#<%=txtItemOrderDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=hdnIsFilterQtyOnHand.ClientID %>').val('0');

            //#region Order No
            $('#lblOrderNo.lblLink').click(function () {
                openSearchDialog('itemrequesthd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtOrderNo.ClientID %>').val(value);
                    onTxtOrderNoChanged(value);
                });
            });

            $('#<%=txtOrderNo.ClientID %>').change(function () {
                onTxtOrderNoChanged($(this).val());
            });

            function onTxtOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Registration
            $('#<%=lblRegistrationNo.ClientID %>.lblLink').live('click', function () {
                var filterReg = "GCRegistrationStatus IN ('X020^002','X020^003')"
                openSearchDialog('registration', filterReg, function (value) {
                    $('#<%=txtRegistrationNo.ClientID %>').val(value);
                    onTxtRegistrationNoChanged(value);
                });
            });

            $('#<%=txtRegistrationNo.ClientID %>').live('change', function () {
                onTxtRegistrationNoChanged($(this).val());

            });

            function onTxtRegistrationNoChanged(value) {
                var filterExpression = "RegistrationNo = '" + value + "'";
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                        $('#<%=txtRegistrationNo.ClientID %>').val(result.RegistrationNo);

                    }
                    else {
                        $('#<%:hdnRegistrationID.ClientID %>').val('');
                        $('#<%:txtRegistrationNo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

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
                var LocationToID = $('#<%=hdnLocationIDTo.ClientID %>').val();
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        if (LocationToID != "" && LocationToID != "0") {
                            if (LocationToID != result.LocationID) {
                                $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                                $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                                filterExpression = "LocationID = " + result.LocationID;
                                Methods.getObject('GetLocationList', filterExpression, function (result2) {
                                    $('#<%=hdnFromLocationItemGroupID.ClientID %>').val(result2.ItemGroupID);
                                    $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result2.GCLocationGroup);
                                    $('#<%=hdnRequestLocationID.ClientID %>').val(result2.RequestLocationID);
                                });
                            } else {
                                showToast('Information', 'Anda memilih lokasi yang sama');
                                $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                                $('#<%=txtLocationCode.ClientID %>').val('');
                                $('#<%=txtLocationName.ClientID %>').val('');
                                $('#<%=hdnFromLocationItemGroupID.ClientID %>').val('');
                                $('#<%=hdnGCLocationGroupFrom.ClientID %>').val('');
                                $('#<%=hdnRequestLocationID.ClientID %>').val('');
                            }
                        } else {
                            $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                            $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                            filterExpression = "LocationID = " + result.LocationID;
                            Methods.getObject('GetLocationList', filterExpression, function (result) {
                                $('#<%=hdnFromLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                                $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result.GCLocationGroup);
                                $('#<%=hdnRequestLocationID.ClientID %>').val(result.RequestLocationID);
                            });
                        }
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnFromLocationItemGroupID.ClientID %>').val('');
                        $('#<%=hdnGCLocationGroupFrom.ClientID %>').val('');
                        $('#<%=hdnRequestLocationID.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Location To
            function getLocationFilterExpressionTo() {
                var requestLocationID = $('#<%=hdnRequestLocationID.ClientID %>').val();
                var filterExpression = "<%:filterExpressionLocationTo %>";

                if (requestLocationID != '') {
                    filterExpression += " LocationID = " + requestLocationID;
                }
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
                var filterExpression = "LocationCode = '" + value + "' AND IsDeleted = 0";
                var LocationFromID = $('#<%=hdnLocationIDFrom.ClientID %>').val();

                Methods.getObject('GetLocationList', filterExpression, function (result) {
                    if (result != null) {
                        if (LocationFromID != result.LocationID) {
                            $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                            $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);
                            $('#<%=hdnToLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                            $('#<%=hdnGCLocationGroupTo.ClientID %>').val(result.GCLocationGroup);
                        } else {
                            showToast('Information', 'Anda memilih lokasi yang sama');
                            $('#<%=hdnLocationIDTo.ClientID %>').val('');
                            $('#<%=txtLocationCodeTo.ClientID %>').val('');
                            $('#<%=txtLocationNameTo.ClientID %>').val('');
                            $('#<%=hdnToLocationItemGroupID.ClientID %>').val('');
                            $('#<%=hdnGCLocationGroupTo.ClientID %>').val('');
                        }
                    }
                    else {
                        $('#<%=hdnLocationIDTo.ClientID %>').val('');
                        $('#<%=txtLocationCodeTo.ClientID %>').val('');
                        $('#<%=txtLocationNameTo.ClientID %>').val('');
                        $('#<%=hdnToLocationItemGroupID.ClientID %>').val('');
                        $('#<%=hdnGCLocationGroupTo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region IsFilterQtyOnHand
            $('#<%=chkIsFilterQtyOnHand.ClientID %>').change(function () {
                if ($('#<%:chkIsFilterQtyOnHand.ClientID %>').is(':checked')) {
                    $('#<%=hdnIsFilterQtyOnHand.ClientID %>').val('1');
                }
                else {
                    $('#<%=hdnIsFilterQtyOnHand.ClientID %>').val('0');
                }
            });
            //#endregion

            //#region Product Line
            function getProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblProductLine.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').live('change', function () {
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


            //#region Item Group

            function onGetItemGroupFilterExpression() {
                var filterExpression = "";
                var chk = $('#<%=hdnItemLocationIs.ClientID %>').val();

                if (chk == 'from') {
                    filterExpression = onGetItemTypeFromFilterExpression();
                } else {
                    filterExpression = onGetItemTypeToFilterExpression();
                }

                if (chk == 'from') {
                    if ($('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '0') {
                        filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WITH(NOLOCK) WHERE DisplayPath like '%/" + $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() + "/%')";
                    }
                } else {
                    if ($('#<%=hdnToLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnToLocationItemGroupID.ClientID %>').val() != '0') {
                        filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WITH(NOLOCK) WHERE DisplayPath like '%/" + $('#<%=hdnToLocationItemGroupID.ClientID %>').val() + "/%')";
                    }
                }

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
                });
            }
            //#endregion

            //#region Item

            function getItemFilterExp() {
                var filterExpression = "";
                var chk = $('#<%=hdnItemLocationIs.ClientID %>').val();
                var chkIsFilterQtyOnHand = $('#<%=hdnIsFilterQtyOnHand.ClientID %>').val();
                if (chk == 'from') {
                    filterExpression = onGetItemTypeFromFilterExpression();
                } else {
                    filterExpression = onGetItemTypeToFilterExpression();
                }

                var orderID = $('#<%=hdnOrderID.ClientID %>').val();
                var fromLocationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                var toLocationID = $('#<%=hdnLocationIDTo.ClientID %>').val();
                var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();

                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WITH(NOLOCK) WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
                }
                else {
                    if (chk == 'from') {
                        if ($('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '0')
                            filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WITH(NOLOCK) WHERE DisplayPath like '%/" + $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() + "/%')";
                    } else {
                        if ($('#<%=hdnToLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnToLocationItemGroupID.ClientID %>').val() != '0')
                            filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WITH(NOLOCK) WHERE DisplayPath like '%/" + $('#<%=hdnToLocationItemGroupID.ClientID %>').val() + "/%')";
                    }
                }

                if (orderID != '' && orderID != '0') {
                    filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM ItemRequestDt WITH(NOLOCK) WHERE ItemRequestID = " + orderID + " AND IsDeleted = 0)";
                }

                if (ProductLineID != "" && ProductLineID != "0") {
                    filterExpression += " AND ProductLineID = " + ProductLineID;
                }

                if (chk == 'from') {
                    filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WITH(NOLOCK) WHERE LocationID IN (" + fromLocationID + ") AND IsDeleted = 0)";
                } else {
                    filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WITH(NOLOCK) WHERE LocationID IN (" + toLocationID + ") AND IsDeleted = 0)";
                }

                filterExpression += " AND GCItemStatus != 'X181^999'";

                if (chkIsFilterQtyOnHand == '1') {
                    filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WITH(NOLOCK) WHERE LocationID = " + toLocationID + " AND IsDeleted = 0)";
                }
                return filterExpression;
            }

            $('#lblItem.lblLink').live('click', function () {
                openSearchDialog('item', getItemFilterExp(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').live('change', function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = getItemFilterExp() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        if ($('#<%=txtItemGroupCode.ClientID %>').val() == '') {
                            $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                            $('#<%=txtItemGroupCode.ClientID %>').val(result.ItemGroupCode);
                            $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                        }
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        Methods.getItemQtyOnOrder(result.ItemID, $('#<%=hdnLocationIDFrom.ClientID %>').val(), 1, function (result3) {
                            if (result3 != null)
                                $('#<%=txtQtyOnProcess.ClientID %>').val(result3.QtyOnOrder + " " + result.ItemUnit);
                            else
                                $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + result.ItemUnit);
                            GetItemQtyFromLocation();
                        });

                        var filterItemBalance = "ItemID = " + result.ItemID + " AND LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val();
                        Methods.getObject('GetItemBalanceList', filterItemBalance, function (result5) {
                            if (result5 != null) {
                                $('#<%=txtMinMax.ClientID %>').val(result5.QuantityMIN + " - " + result5.QuantityMAX);
                            } else {
                                $('#<%=txtMinMax.ClientID %>').val("0");
                            }
                        });

                        cboItemUnit.PerformCallback();
                        onTransactionTypeEntryPopup(result.ItemID);
                    }
                    else {
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=txtQtyOnProcess.ClientID %>').val('');
                        $('#<%=txtStockFromLocation.ClientID %>').val('');
                        $('#<%=txtBinLocationName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=hdnIsEdit.ClientID %>').val('0');
                    $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblLocationTo.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtLocationCodeTo.ClientID %>').attr('readonly', 'readonly');

                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                    $('#<%=txtStockFromLocation.ClientID %>').val('');
                    $('#<%=txtMinMax.ClientID %>').val('');
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#<%=txtNotes2.ClientID %>').val('');
                    $('#<%=txtBinLocationName.ClientID %>').val('');

                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');

                    $('#containerEntry').show();
                }
            });

            $('#lblQuickPick').click(function () {

                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Warehouse/ItemOrder/ItemRequestQuickPicksCtl.ascx');
                    var transactionID = $('#<%=hdnOrderID.ClientID %>').val();
                    var locationIDFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                    var GCLocationGroupFrom = $('#<%=hdnGCLocationGroupFrom.ClientID %>').val();
                    var FromLocationItemGroupID = $('#<%=hdnFromLocationItemGroupID.ClientID %>').val();
                    var locationIDTo = $('#<%=hdnLocationIDTo.ClientID %>').val();
                    var GCLocationGroupTo = $('#<%=hdnGCLocationGroupTo.ClientID %>').val();
                    var ToLocationItemGroupID = $('#<%=hdnToLocationItemGroupID.ClientID %>').val();
                    var chk = $('#<%=hdnItemLocationIs.ClientID %>').val();
                    var chkIsFilterQtyOnHand = $('#<%=hdnIsFilterQtyOnHand.ClientID %>').val();
                    var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                    var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
                    var id = transactionID + '|' + chk + '|' + chkIsFilterQtyOnHand + '|' + locationIDFrom + '|' + GCLocationGroupFrom + '|' + FromLocationItemGroupID + '|' + locationIDTo + '|' + GCLocationGroupTo + '|' + ToLocationItemGroupID + '|' + ProductLineID + '|' + GCItemType;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        $('#<%=rbFilterItemLocation.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == 'from') {
                $('#<%=hdnItemLocationIs.ClientID %>').val('from');
            } else {
                $('#<%=hdnItemLocationIs.ClientID %>').val('to');
            }
        });

        function onGetItemTypeToFilterExpression() {
            var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
            var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
            var GCLocationGroup = $('#<%=hdnGCLocationGroupTo.ClientID %>').val();
            var filterExpression = "IsDeleted = 0";

            if (ProductLineID != "" && ProductLineID != "0") {
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM ItemMaster WITH(NOLOCK) WHERE ProductLineID = " + ProductLineID + ")";
            } else {
                if (GCLocationGroup != "") {
                    if (GCLocationGroup == Constant.LocationGroup.DRUGS_AND_MEDICALSUPPLIES) {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003')";
                    } else if (GCLocationGroup == Constant.LocationGroup.LOGISTICS) {
                        filterExpression += " AND GCItemType IN ('X001^008')";
                    } else {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
                    }
                } else {
                    filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
                }
            }

            return filterExpression;
        }

        function onGetItemTypeFromFilterExpression() {
            var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
            var GCItemType = $('#<%=hdnProductLineItemType.ClientID %>').val();
            var GCLocationGroup = $('#<%=hdnGCLocationGroupFrom.ClientID %>').val();
            var filterExpression = "IsDeleted = 0";

            if (ProductLineID != "" && ProductLineID != "0") {
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM ItemMaster WITH(NOLOCK) WHERE ProductLineID = " + ProductLineID + ")";
            } else {
                if (GCLocationGroup != "") {
                    if (GCLocationGroup == Constant.LocationGroup.DRUGS_AND_MEDICALSUPPLIES) {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003')";
                    } else if (GCLocationGroup == Constant.LocationGroup.LOGISTICS) {
                        filterExpression += " AND GCItemType IN ('X001^008')";
                    } else {
                        filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008')";
                    }
                } else {
                    filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008')";
                }
            }

            return filterExpression;
        }

        function GetItemQtyFromLocation() {
            var filterExpression = "LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                if (result == null) {
                    $('#<%=txtStockFromLocation.ClientID %>').val('');
                    $('#<%=txtBinLocationName.ClientID %>').val('');
                }
                else {
                    $('#<%=txtStockFromLocation.ClientID %>').val(result.QuantityEND + ' ' + result.ItemUnit);
                    $('#<%=txtBinLocationName.ClientID %>').val(result.BinLocationName);
                }
            });
        }

        //#region edit dan delete
        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIsEdit.ClientID %>').val('1');
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnItemGroupID.ClientID %>').val(entity.ItemGroupID);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            var ID = $('#<%=hdnEntryID.ClientID %>').val();
            var filterExpression = "ID = " + ID;
            Methods.getObjectValue('GetvItemRequestDtList', filterExpression, 'Remarks', function (result) {
                $('#<%=txtNotes2.ClientID %>').val(result);
            });
            cboItemUnit.PerformCallback();
            cboTransactionType.SetValue(entity.GCItemRequestType);
            Methods.getItemQtyOnOrder(entity.ItemID, $('#<%=hdnLocationIDFrom.ClientID %>').val(), 1, function (result3) {
                if (result3 != null)
                    $('#<%=txtQtyOnProcess.ClientID %>').val((result3.QtyOnOrder - entity.CustomTotal) + " " + entity.BaseUnit);
                else
                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                GetItemQtyFromLocation();
            });

            var filterItemBalance = "ItemID = " + entity.ItemID + " AND LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val();
            Methods.getObject('GetItemBalanceList', filterItemBalance, function (result4) {
                if (result4 != null) {
                    $('#<%=txtMinMax.ClientID %>').val(result4.QuantityMIN + " - " + result4.QuantityMAX);
                } else {
                    $('#<%=txtMinMax.ClientID %>').val("0");
                }
            });
            $('#containerEntry').show();
        });
        //#endregion

        //#region cboItemUnit
        function onCboItemUnitEndCallBack() {
            if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '') {
                cboItemUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
            }
            else cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            onCboItemUnitChanged();
        }

        function onCboItemUnitChanged() {
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var baseText = getItemUnitName(baseValue);

            if (baseValue == toUnitItem) {
                $('#<%=hdnItemUnitValue.ClientID %>').val('1');
                var conversion = "1 " + baseText + " = 1 " + baseText;
                $('#<%=txtConversion.ClientID %>').val(conversion);
            }
            else {
                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                    var toConversion = getItemUnitName(toUnitItem);
                    $('#<%=hdnItemUnitValue.ClientID %>').val(result);
                    var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                    $('#<%=txtConversion.ClientID %>').val(conversion);
                });
            }
        }

        function getItemUnitName(baseValue) {
            var value = cboItemUnit.GetValue();
            cboItemUnit.SetValue(baseValue);
            var text = cboItemUnit.GetText();
            cboItemUnit.SetValue(value);
            return text;
        }
        //#endregion

        function onTransactionTypeEntryPopup(ItemID) {
            var fromLocationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var toLocationID = $('#<%=hdnLocationIDTo.ClientID %>').val();

            var filterExpressionBalanceFrom = 'ItemID = ' + ItemID + ' AND LocationID = ' + fromLocationID + ' AND IsDeleted = 0';
            Methods.getObject('GetItemBalanceList', filterExpressionBalanceFrom, function (result1) {
                if (result1 != null) {
                    cboTransactionType.SetValue(result1.GCItemRequestType);
                } else {
                    var filterExpressionLocationFrom = 'LocationID = ' + fromLocationID + ' AND IsDeleted = 0';
                    Methods.getObject('GetLocationList', filterExpressionLocationFrom, function (result2) {
                        if (result2.GCItemRequestType != '') {
                            cboTransactionType.SetValue(result2.GCItemRequestType);
                        } else {
                            var filterExpressionBalanceTo = 'ItemID = ' + ItemID + ' AND LocationID = ' + toLocationID + ' AND IsDeleted = 0';
                            Methods.getObject('GetItemBalanceList', filterExpressionBalanceTo, function (result3) {
                                if (result3 != null) {
                                    cboTransactionType.SetValue(result3.GCItemRequestType);
                                } else {
                                    var filterExpressionLocationTo = 'LocationID = ' + toLocationID + ' AND IsDeleted = 0';
                                    Methods.getObject('GetLocationList', filterExpressionLocationTo, function (result4) {
                                        if (result4.GCItemRequestType != '') {
                                            cboTransactionType.SetValue(result4.GCItemRequestType);
                                        } else {
                                            var filterExpressionItemProduct = 'ItemID = ' + ItemID + ' AND IsDeleted = 0';
                                            Methods.getObject('GetItemProductList', filterExpressionItemProduct, function (result5) {
                                                if (result5 != null) {
                                                    cboTransactionType.SetValue(result5.GCItemRequestType);
                                                } else {
                                                    cboTransactionType.SetValue(Constant.StandardCode.ItemRequestType.DISTRIBUTION);
                                                }
                                            });
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });
        }

        function onAfterSaveRecordDtSuccess(OrderID) {
            if ($('#<%=hdnOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnOrderID.ClientID %>').val(OrderID);
                var filterExpression = 'ItemRequestID = ' + OrderID;
                Methods.getObject('GetItemRequestHdList', filterExpression, function (result) {
                    $('#<%=txtOrderNo.ClientID %>').val(result.ItemRequestNo);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            }
            else {
                cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    var OrderID = s.cpOrderID;
                    onAfterSaveRecordDtSuccess(OrderID);
                    var isEdit = $('#<%=hdnIsEdit.ClientID %>').val();
                    if (isEdit == '1') {
                        $('#containerEntry').hide();
                    } else if (isEdit == '0') {
                        $('#lblAddData').click();
                    }
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            $('#containerEntry').hide();
        }

        //#region Paging
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var itemRequestID = $('#<%=hdnOrderID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            var transaction = $('#<%=hdnGCTransactionStatus.ClientID %>').val();

            if (itemRequestID == '' || itemRequestID == '0') {
                errMessage.text = 'Please Save Transaction First!';
                return false;
            }
            else {
                if (code == 'IM-00049' || code == 'IM-00104' || code == 'IM-00204') {
                    if (transaction == Constant.TransactionStatus.APPROVED || transaction == Constant.TransactionStatus.CLOSED) {
                        filterExpression.text = "ItemRequestID = " + itemRequestID;
                        return true;
                    }
                    else {
                        errMessage.text = "Data Doesn't Approved or Closed";
                        return false;
                    }
                }
                else {
                    filterExpression.text = "ItemRequestID = " + itemRequestID;
                    return true;
                }
            }

        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoDistribution' || code == 'infoConsumption' || code == 'infoPurchaseRequest' || code == 'infoItemRequestDecline' || code == 'infoItemRequestClosed') {
                var param = $('#<%:hdnOrderID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnOrderID.ClientID %>').val();
            }
        }

        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnOrderID.ClientID %>').val() == '' || $('#<%:hdnOrderID.ClientID %>').val() == '0') {
                $('#btnInfoDistribution').attr('enabled', 'false');
                $('#btnInfoConsumption').attr('enabled', 'false');
                $('#btnInfoPurchaseRequest').attr('enabled', 'false');
            }
            else {
                $('#btnInfoDistribution').removeAttr('enabled');
                $('#btnInfoConsumption').removeAttr('enabled');
                $('#btnInfoPurchaseRequest').removeAttr('enabled');
            }
        }

        function onBeforeProposedApprovedRecord(errMessage) {
            var locationIDFrom = $('#<%:hdnLocationIDFrom.ClientID %>').val();
            var requestID = $('#<%:hdnOrderID.ClientID %>').val();
            var oItemName = '';

            var filterExpression = 'IsDeleted = 0 AND ItemRequestID = ' + requestID;
            Methods.getListObject('GetvItemRequestDtList', filterExpression, function (result) {
                for (i = 0; i < result.length; i++) {
                    var filterExpressionBalance = 'ItemID = ' + result[i].ItemID + ' AND LocationID = ' + locationIDFrom + ' AND IsDeleted = 0';
                    Methods.getObject('GetItemBalanceList', filterExpressionBalance, function (result2) {
                        if (result2 != null) {
                            if (result[i].Quantity > result2.QuantityMAX) {
                                if (oItemName != "") {
                                    oItemName += ", ";
                                }
                                oItemName += result[i].ItemName1;
                            }
                        }
                    });
                }
            });

            if (oItemName.length > 0) {
                if (confirm("Permintaan untuk item " + oItemName + " melebihi Quantity Max, lanjutkan proses?")) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        } 
		 
    </script>
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowUsingAlternateUnit" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="from" id="hdnItemLocationIs" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnPurchaseRequest" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowGetFromDestinationLocation" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="0" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="0" id="hdnIsFilterQtyOnHand" runat="server" />
    <input type="hidden" value="0" id="hdnQtyMaxID" runat="server" />
    <div style="overflow-y: auto; overflow-x: hidden;">
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
                                <label class="lblLink" id="lblOrderNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <input type="hidden" id="hdnFromLocationItemGroupID" value="" runat="server" />
                                <input type="hidden" id="hdnRequestLocationID" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
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
                                        <col style="width: 150px" />
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
                        <%--<tr>
                            <td />
                            <td>
                                <asp:CheckBox ID="chkIsDisplayToItemOnly" Width="100%" runat="server" Text="Tampilkan Barang di Lokasi Tujuan saja"
                                    Checked="true" />
                            </td>
                        </tr>--%>
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
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderTime" Width="100px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <input type="hidden" id="hdnToLocationItemGroupID" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 1px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeTo" Width="120px" runat="server" />
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
                            <td class="tdLabel" style="vertical-align: top">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblRegistrationNo" runat="server">
                                    <%=GetLabel("No. Registrasi Pasien")%></label>
                            </td>
                            <td style="width: 300px">
                                <asp:TextBox ID="txtRegistrationNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Permintaan Barang")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 50%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblItemGroup">
                                                        <%=GetLabel("Kelompok Item")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width: 120px" />
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
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Item")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Rak")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 120px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBinLocationName" Width="120px" runat="server" ReadOnly="true" />
                                                            </td>
                                                            <td class="tdLabel" style="padding-left: 10px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("MIN - MAX")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMinMax" ReadOnly="true" Width="120px" runat="server" Style="text-align: center" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Stock saat ini")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 120px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStockFromLocation" ReadOnly="true" CssClass="number" Width="120px"
                                                                    runat="server" />
                                                            </td>
                                                            <td class="tdLabel" style="padding-left: 10px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("On Order Qty")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtQtyOnProcess" Width="120px" runat="server" Style="text-align: right;"
                                                                    ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah Permintaan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" CssClass="number" Width="120px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Satuan Permintaan")%></label>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 120px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                                    Width="125px" OnCallback="cboItemUnit_Callback">
                                                                    <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                            <td class="tdLabel" style="padding-left: 10px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Tipe Transaksi")%></label>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboTransactionType" ClientInstanceName="cboTransactionType"
                                                                    Width="120px">
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnItemUnitValue" runat="server" />
                                                    <asp:TextBox ID="txtConversion" Width="360px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top">
                                                    <%=GetLabel("Keterangan") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes2" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupID") %>" bindingfield="ItemGroupID" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupCode") %>" bindingfield="ItemGroupCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupName1") %>" bindingfield="ItemGroupName1" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("BaseUnit") %>" bindingfield="BaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCItemRequestType") %>" bindingfield="GCItemRequestType" />
                                                    <input type="hidden" value="<%#:Eval("CustomTotal") %>" bindingfield="CustomTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomItemUnit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Diminta" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="BaseUnit" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Satuan Dasar" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="CustomConversion" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Konversi" />
                                            <asp:BoundField DataField="CustomItemRequest" HeaderStyle-HorizontalAlign="Right"
                                                ItemStyle-HorizontalAlign="Right" HeaderText="Total Diminta" HeaderStyle-Width="150px" />
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
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                            <%= GetLabel("Tambah Data")%></span> <span class="lblLink" id="lblQuickPick">
                                <%= GetLabel("Quick Picks")%></span>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 600px;">
                                        <div class="pageTitle" style="text-align: center">
                                            <%=GetLabel("Informasi")%></div>
                                        <div style="background-color: #EAEAEA;">
                                            <table width="600px" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="30px" />
                                                </colgroup>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
